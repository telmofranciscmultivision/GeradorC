using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using EdpSimulator.Entities;
using EdpSimulator.Producers;
using Avro.Specific;

namespace EdpSimulator.Controllers
{
    public class GenericControllerStream<KRecord,TRecord,TGenerator,TProducer> : IEdpControllerStream
        where KRecord : ISpecificRecord
        where TRecord : ISpecificRecord 
        where TGenerator : IEdpGenerator<KRecord,TRecord>
        where TProducer : IEdpProducer<KRecord,TRecord>
    {
        /*
            Generic controller to produce record/s to a database or message broker.
            ...
        */

        private IConfigurationRoot config;
        private TGenerator generator;
        private TProducer producer;
        private long duration;
        private int batchSize;  // Rate in appsettings
        private int batchDuration;
        private int totalRecordsToProduce;
        private string containerOrTopicName;
        private string databaseName;
        
        public GenericControllerStream(IConfigurationRoot config, TGenerator generator, TProducer producer)
        {
            try
            {
                this.config = config;
                this.generator = generator;
                this.producer = producer;

                if (generator is null || producer is null || config is null)
                    throw new NullReferenceException("Config, generator or producer are null. Check appsettings or Program class.");

                duration = long.Parse(config["Duration"]) * 1000;
                batchSize = int.Parse(config["Rate"]);
                batchDuration = 1 * 1000;   // always 1 second to generate records/s
                totalRecordsToProduce = int.Parse(config["TotalRecordsToProduce"]);

                containerOrTopicName = config["ContainerOrTopicName"];
                databaseName = config["DatabaseName"];
                
                if ( String.IsNullOrEmpty(containerOrTopicName) )
                    throw new ArgumentException("ContainerOrTopicName was not filled properly in appsettings.");

                if ( producer is CosmosProducer<KRecord,TRecord> && String.IsNullOrEmpty(databaseName) ) 
                    throw new ArgumentException("DatabaseName was not filled properly in appsettings.");
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                throw new ArgumentException(
                    String.Format("Appsettings was not filled properly.\n{0}", e.GetBaseException() ));
            }
        }

        private async Task WaitBatchDuration(int batchDuration, int elapsedBatchTime)
            => await IEdpControllerStream.WaitBatchDuration(batchDuration, elapsedBatchTime);
        private bool CheckStopCondition(long duration, long elapsedTime, int totalRecordsToProduce, int currentRecordsProduced)
            => IEdpControllerStream.CheckStopCondition(duration, elapsedTime, totalRecordsToProduce, currentRecordsProduced);

        private static async Task StopActions(List<Task> producerTasks, int initialRecordCount, int currentRecordsProduced, long duration,
            TProducer producer, string containerOrTopicName, string databaseName=null, Task forcedTask=null)
        {
            Stopwatch finishAllStopwatch = new Stopwatch();
            
            // await for all records to be published
            finishAllStopwatch.Start();           
            await Task.WhenAll(producerTasks);      // comment if using ProduceForced method
            // await forcedTask;                    // uncomment if using ProduceForced method    
            finishAllStopwatch.Stop();

            // Detect excess time due to buffering or bottleneck on the Producer or the Sink
            long excessTime = finishAllStopwatch.ElapsedMilliseconds;

            // Display results
            Console.WriteLine("{1}s - Produced {0} records in total.", currentRecordsProduced, (duration + excessTime)/1000 );
            Console.WriteLine("Simulation exceeded expected time by {0} ms. Check RUs or other bottleneck.", excessTime);

            // Query record count after finishing
            int laterRecordCount = await producer.CountRecords(containerOrTopicName, databaseName);
            Console.WriteLine("Query validates {0} records were produced during simulation.", laterRecordCount - initialRecordCount);
            // This value will be greater than produced records if some other app is also producing
        }

        public async Task Run() {    // can be overriden

            // Declare needed variables and create objects

            Stopwatch runningStopwatch = new Stopwatch();    // Measurement of simulation elapsed time
            Stopwatch batchStopwatch = new Stopwatch();

            int currentRecordsProduced = 0;    // Keeps track of records produced during simulation
            int elapsedBatchTime = 0;          // For print purposes
            int totalBatchtime = 0;            // For print purposes
            // int counter = 0;

            Task<List<TRecord>> generatorTask;
            List<Task> producerTasks = new List<Task>();

            // Create first async batch and await result
            Task<List<TRecord>> firstTask = generator.GenerateRecordsAsync(batchSize, false);

            List<TRecord> records = await firstTask;

            // Query count of records in container or topic
            int initialRecordCount = await producer.CountRecords(containerOrTopicName, databaseName);

            // Start running simulation
            runningStopwatch.Start();

            while (true)
            {             
                batchStopwatch.Start();    // Start measurement of time elapsed during batch

                generatorTask = generator.GenerateRecordsAsync(batchSize, false);    // Create next async batch

                // Produce records
                //producerTasks = producer.Produce(records, containerOrTopicName, databaseName, producerTasks);
                // var forcedTask = producer.ProduceForced(records, containerOrTopicName, databaseName, producerTasks);    // uncomment if using ProducerForced method
                currentRecordsProduced += batchSize;

                // For debug purposes
                // Console.WriteLine(Convert.ToInt32(batchStopwatch.ElapsedMilliseconds));
                // Console.WriteLine(runningStopwatch.ElapsedMilliseconds);                

                // Await result from async data generation for next batch
                records = await generatorTask;

                // Await batch duration (1 sec) if not exceeded
                elapsedBatchTime = Convert.ToInt32(batchStopwatch.ElapsedMilliseconds);
                await WaitBatchDuration(batchDuration, elapsedBatchTime);

                // Check if simulation should stop
                if (CheckStopCondition(duration, runningStopwatch.ElapsedMilliseconds, totalRecordsToProduce, currentRecordsProduced) == true)
                {
                    await StopActions(producerTasks, initialRecordCount, currentRecordsProduced, duration,
                        producer, containerOrTopicName, databaseName);
                        // producer, containerOrTopicName, databaseName, forcedTask);   // uncomment if using ProducerForced method

                    break;    // break while running loop
                }
                else
                {
                    totalBatchtime = (elapsedBatchTime > batchDuration) ? elapsedBatchTime : batchDuration;
                    Console.WriteLine("{1}s - Submitted {0} records for async production. Exceeded expected batch time by {2} ms.", 
                        currentRecordsProduced, runningStopwatch.ElapsedMilliseconds/1000, Math.Abs(batchDuration - totalBatchtime));

                    batchStopwatch.Stop();
                    batchStopwatch.Reset();    // So that stopwatch does not store all batch interval elapsed times. Might be useful
                }
            }
        }
    }
}