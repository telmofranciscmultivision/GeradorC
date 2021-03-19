using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using EdpSimulator.Producers;
using EdpSimulator.Entities;
using EdpSimulator.Generators;
using Confluent.Kafka;
using EdpSimulator;
using key.SOURCEDB.DEP000PA.DEPPA;
using key.SOURCEDB.HTH000PA.HTHPA;
using key.SOURCEDB.CAM00020.CAM20;

namespace EdpSimulator.Controllers
{
    public class JavaControllerStream   //: IEdpControllerStream
    {
        private IConfigurationRoot config;


        private JavaDdaGenerator generator;
        private JavaCardsGenerator generatorCards;
        private JavaH2hGenerator generatorH2h;
        private JavaCategoryGenerator generatorCategory;

        private Random randomGen;


        private KafkaProducer<HMV_DDA,JavaDdaRecord> producer;
        private KafkaProducer<categoryKey, JavaCategoryRecord> producerCateg;
        private KafkaProducer<LOGTXN_T, JavaCardsRecord> producerCards;
        private KafkaProducer<HTHPSE_T, JavaH2hRecord> producerH2h;
        private long duration;
        private int batchSize;  // Rate in appsettings
        private int batchDuration;
        private int totalRecordsToProduce;
        private string containerOrTopicName;
        private string databaseName;
        
        public JavaControllerStream(IConfigurationRoot config)
        {
            try
            {
                this.config = config;

                this.generator = new JavaDdaGenerator();
                this.generatorCards = new JavaCardsGenerator();
                this.generatorH2h = new JavaH2hGenerator();
                this.generatorCategory = new JavaCategoryGenerator();
                this.randomGen = new Random();

                this.producer = new KafkaProducer<HMV_DDA,JavaDdaRecord>(config["EndpointUrl"], schemaRegistryUrl: config["schemaRegistryUrl"], avro: bool.Parse(config["avro"]));
                this.producerCateg = new KafkaProducer<categoryKey, JavaCategoryRecord>(config["EndpointUrl"], schemaRegistryUrl: config["schemaRegistryUrl"], avro: bool.Parse(config["avro"]));
                this.producerCards = new KafkaProducer<LOGTXN_T, JavaCardsRecord>(config["EndpointUrl"], schemaRegistryUrl: config["schemaRegistryUrl"], avro: bool.Parse(config["avro"]));
                this.producerH2h = new KafkaProducer<HTHPSE_T, JavaH2hRecord>(config["EndpointUrl"], schemaRegistryUrl: config["schemaRegistryUrl"], avro: bool.Parse(config["avro"]));

                

                if (generator is null || producer is null || config is null)
                    throw new NullReferenceException("Config, generator or producer are null. Check appsettings or Program class.");

                duration = long.Parse(config["Duration"]) * 1000;
                batchSize = int.Parse(config["Rate"]);
                batchDuration = 1 * 1000;   // always 1 second to generate records/s
                totalRecordsToProduce = int.Parse(config["TotalRecordsToProduce"]);

                containerOrTopicName = config["DdaTopic"];    // config["ContainerOrTopicName"];
                databaseName = config["DatabaseName"];
                
                if ( String.IsNullOrEmpty(containerOrTopicName) )
                    throw new ArgumentException("ContainerOrTopicName was not filled properly in appsettings.");

                // if ( producer is CosmosProducer<TRecord> && String.IsNullOrEmpty(databaseName) ) 
                //     throw new ArgumentException("DatabaseName was not filled properly in appsettings.");
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                throw new ArgumentException(
                    String.Format("Appsettings was not filled properly.\n{0}", e.GetBaseException() ));
            }
        }

        private static bool CheckStopCondition(long duration, long elapsedTime, int totalRecordsToProduce, int currentRecordsProduced)
            => (elapsedTime >= duration || currentRecordsProduced >= totalRecordsToProduce) ? true : false;

        private static async Task WaitBatchDuration(int batchDuration, int elapsedBatchTime)  
        {
            int waitTime = batchDuration - elapsedBatchTime;
            await Task.Delay(waitTime > 0 ? waitTime : 0);
        }

        private static async Task StopActions(List<Task> producerTasks, int initialRecordCount, int currentRecordsProduced, long duration,
            KafkaProducer<HMV_DDA,JavaDdaRecord> producer, string containerOrTopicName, string databaseName=null, Task forcedTask=null)
        {
            Stopwatch finishAllStopwatch = new Stopwatch();
            
            // await for all records to be published
            finishAllStopwatch.Start();
            try
            {
                await Task.WhenAll(producerTasks);
            }
            catch (ProduceException<HMV_DDA, JavaDdaRecord> ex)
            {
                // In some cases (notably Schema Registry connectivity issues), the InnerException
                // of the ProduceException contains additional informatiom pertaining to the root
                // cause of the problem. This information is automatically included in the output
                // of the ToString() method of the ProduceException, called implicitly in the below.
                Console.WriteLine($"error producing message: {ex.Message}");
            }// comment if using ProduceForced method
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

            Task<List<JavaDdaRecord>> generatorTask;
            List<Task> producerTasks = new List<Task>();

            Task<List<JavaCategoryRecord>> generatorCategTask;
            List<Task> producerCategTasks = new List<Task>();            

            Task<List<JavaCardsRecord>> generatorCardsTask;
            List<Task> producerCardsTasks = new List<Task>();

            Task<List<JavaH2hRecord>> generatorH2hTask;
            List<Task> producerH2hTasks = new List<Task>();

            List<HMV_DDA> ddaKeyRecords;
            List<categoryKey> categoryKeyRecords;
            List<LOGTXN_T> cardsKeyRecords;
            List<HTHPSE_T> h2hKeyRecords;

            // Create first async batch and await result
            string[] transactionids = new string[batchSize];
            string[] accountids = new string[batchSize];

            int[] IDMSGBAN_2 = new int[batchSize];
            long[] CONTA_CLI = new long[batchSize];
            long[] IDMSGBAN_1 = new long[batchSize];

            string[] TXN_DT = new string[batchSize];
            int[] TXN_TM = new int[batchSize];
            string[] TXN_AMT = new string[batchSize];
            string[] TXN_SINAL = new string[batchSize];
            string[] ORIG_PLAS = new string[batchSize];

            string[] HDDA_HST_DESCR_CONT = new string[batchSize];
            string[] HDDA_HST_TAMT = new string[batchSize];
            int[] HDDA_HST_TRAN_SIGN = new int[batchSize];
            string[] HDDA_HST_DATA_SISTEMA = new string[batchSize];
            int[] HDDA_HST_HORA = new int[batchSize];

            string[] HDDA_HST_TRACE_ID = new string[batchSize];

            int[] HDDA_KEY_COMP = new int[batchSize];
            int[] HDDA_KEY_CONTA = new int[batchSize];
            string[] HDDA_KEY_DATA_LANCAMENTO = new string[batchSize];
            string[] HDDA_KEY_ORIGEM = new string[batchSize];
            string[] HDDA_KEY_DATA_HORA = new string[batchSize];

            int[] ACCT_COID = new int[batchSize];
            string[] ACCT_NBR = new string[batchSize];

            for (int i=0; i < batchSize; i++)
            {
                IDMSGBAN_2[i] = 20;// randomGen.Next(1000000); //
                CONTA_CLI[i] = randomGen.Next(100000);
                IDMSGBAN_1[i] = randomGen.Next(1000000);

                TXN_DT[i] = randomGen.Next(1000000).ToString();
                TXN_TM[i] = randomGen.Next(1000000);
                TXN_AMT[i] = randomGen.Next(1000000).ToString();
                TXN_SINAL[i] = 1.ToString();
                ORIG_PLAS[i] = "1234567891234567";

                HDDA_HST_DESCR_CONT[i] = ORIG_PLAS[i];
                HDDA_HST_TAMT[i] = TXN_AMT[i];
                HDDA_HST_TRAN_SIGN[i] = Int32.Parse(TXN_SINAL[i]);
                HDDA_HST_DATA_SISTEMA[i] = TXN_DT[i];
                HDDA_HST_HORA[i] = TXN_TM[i];

                HDDA_HST_TRACE_ID[i] = "H003" + IDMSGBAN_1[i] + IDMSGBAN_2[i];

                HDDA_KEY_COMP[i] = IDMSGBAN_2[i]; //randomGen.Next(1000000);
                HDDA_KEY_CONTA[i] = Convert.ToInt32(CONTA_CLI[i]);
                HDDA_KEY_DATA_LANCAMENTO[i] = randomGen.Next(1000000).ToString();
                HDDA_KEY_ORIGEM[i] = randomGen.Next(1000000).ToString();
                HDDA_KEY_DATA_HORA[i] = randomGen.Next(1000000).ToString();

                ACCT_COID[i] = IDMSGBAN_2[i];
                ACCT_NBR[i] = HDDA_KEY_CONTA[i].ToString();

                transactionids[i] = String.Concat(HDDA_KEY_COMP[i] , HDDA_KEY_CONTA[i] , HDDA_KEY_DATA_LANCAMENTO[i] ,
                                    HDDA_KEY_ORIGEM[i] , HDDA_KEY_DATA_HORA[i]);
                accountids[i] = String.Concat(ACCT_COID[i], "DDA", ACCT_NBR[i]);

              //  Console.WriteLine("transactionid:{0} , keycomp:{1}, hddkeyconta:{2}, datalancamento:{3}, keyorig:{4}, keydata:{5}", transactionids[i], HDDA_KEY_COMP[i], HDDA_KEY_CONTA[i], HDDA_KEY_DATA_LANCAMENTO[i], HDDA_KEY_ORIGEM[i], HDDA_KEY_DATA_HORA[i]);

            }
            Task<List<JavaDdaRecord>> firstTask = generator.GenerateRecordsAsync(batchSize, HDDA_KEY_COMP,HDDA_KEY_CONTA,HDDA_KEY_DATA_LANCAMENTO,HDDA_KEY_ORIGEM,HDDA_KEY_DATA_HORA,
                    HDDA_HST_DESCR_CONT, HDDA_HST_TAMT, HDDA_HST_TRAN_SIGN, HDDA_HST_DATA_SISTEMA, HDDA_HST_HORA, HDDA_HST_TRACE_ID, false);
            Task<List<JavaCategoryRecord>> firstCategTask = generatorCategory.GenerateRecordsAsync(batchSize, transactionids, accountids, false);
            Task<List<JavaCardsRecord>> firstCardsTask = generatorCards.GenerateRecordsAsync(batchSize, TXN_DT,TXN_TM,TXN_AMT,TXN_SINAL,ORIG_PLAS,ACCT_COID,ACCT_NBR,  false); 
            Task<List<JavaH2hRecord>> firstH2hTask = generatorH2h.GenerateRecordsAsync(batchSize, IDMSGBAN_2,CONTA_CLI,IDMSGBAN_1, false);


           

            List<JavaDdaRecord> records = await firstTask;
            List<JavaCategoryRecord> recordsCateg = await firstCategTask;
            List<JavaCardsRecord> recordsCards = await firstCardsTask; 
            List<JavaH2hRecord> recordsH2h = await firstH2hTask;

            // Query count of records in container or topic
            Console.WriteLine("Arrived - First");
            int initialRecordCount = await producer.CountRecords(containerOrTopicName, databaseName);

            // Start running simulation
            runningStopwatch.Start();

            while (true)
            {             
                batchStopwatch.Start();    // Start measurement of time elapsed during batch
                Console.WriteLine("Arrived - A");

                for (int i=0; i < batchSize; i++)
                {
                    IDMSGBAN_2[i] = 20;// randomGen.Next(1000000); //
                    CONTA_CLI[i] = randomGen.Next(100000);
                    IDMSGBAN_1[i] = randomGen.Next(1000000);

                    TXN_DT[i] = randomGen.Next(1000000).ToString();
                    TXN_TM[i] = randomGen.Next(1000000);
                    TXN_AMT[i] = randomGen.Next(1000000).ToString();
                    TXN_SINAL[i] = 1.ToString();
                    ORIG_PLAS[i] = "1234567891234567";

                    HDDA_HST_DESCR_CONT[i] = ORIG_PLAS[i];
                    HDDA_HST_TAMT[i] = TXN_AMT[i];
                    HDDA_HST_TRAN_SIGN[i] = Int32.Parse(TXN_SINAL[i]);
                    HDDA_HST_DATA_SISTEMA[i] = TXN_DT[i];
                    HDDA_HST_HORA[i] = TXN_TM[i];

                    HDDA_HST_TRACE_ID[i] = "H003" + IDMSGBAN_1[i] + IDMSGBAN_2[i];

                    HDDA_KEY_COMP[i] = IDMSGBAN_2[i]; //randomGen.Next(1000000);
                    HDDA_KEY_CONTA[i] = Convert.ToInt32(CONTA_CLI[i]);
                    HDDA_KEY_DATA_LANCAMENTO[i] = randomGen.Next(1000000).ToString();
                    HDDA_KEY_ORIGEM[i] = randomGen.Next(1000000).ToString();
                    HDDA_KEY_DATA_HORA[i] = randomGen.Next(1000000).ToString();

                    ACCT_COID[i] = IDMSGBAN_2[i];
                    ACCT_NBR[i] = HDDA_KEY_CONTA[i].ToString();

                    transactionids[i] = String.Concat(HDDA_KEY_COMP[i], HDDA_KEY_CONTA[i], HDDA_KEY_DATA_LANCAMENTO[i],
                                    HDDA_KEY_ORIGEM[i], HDDA_KEY_DATA_HORA[i]);

                    accountids[i] = String.Concat(ACCT_COID[i], "DDA", ACCT_NBR[i]);

                }
                Console.WriteLine("Arrived - B");

                generatorTask = generator.GenerateRecordsAsync(batchSize, HDDA_KEY_COMP,HDDA_KEY_CONTA,HDDA_KEY_DATA_LANCAMENTO,HDDA_KEY_ORIGEM,HDDA_KEY_DATA_HORA,
                    HDDA_HST_DESCR_CONT, HDDA_HST_TAMT, HDDA_HST_TRAN_SIGN, HDDA_HST_DATA_SISTEMA, HDDA_HST_HORA, HDDA_HST_TRACE_ID, false);    // Create next async batch
                generatorCategTask = generatorCategory.GenerateRecordsAsync(batchSize, transactionids,accountids, false);
                generatorCardsTask = generatorCards.GenerateRecordsAsync(batchSize, TXN_DT,TXN_TM,TXN_AMT,TXN_SINAL,ORIG_PLAS,ACCT_COID,ACCT_NBR, false);    // Create next async batch
                generatorH2hTask = generatorH2h.GenerateRecordsAsync(batchSize, IDMSGBAN_2,CONTA_CLI,IDMSGBAN_1, false);

                ddaKeyRecords = generator.GenerateDda(generator.BuildKey(batchSize));
                categoryKeyRecords = generatorCategory.GenerateCategories(generatorCategory.BuildKey(batchSize));
                cardsKeyRecords = generatorCards.GenerateCards(generatorCards.BuildKey(batchSize));
                h2hKeyRecords = generatorH2h.GenerateH2h(generatorH2h.BuildKey(batchSize));
                Console.WriteLine("Arrived - C");
                // Produce records
                producerTasks = producer.Produce(records, config["DdaTopic"], databaseName, producerTasks, ddaKeyRecords);
                Console.WriteLine("Arrived - C-2");
                producerCategTasks = producerCateg.Produce(recordsCateg, config["CategoryTopic"], databaseName, producerCategTasks, categoryKeyRecords);
                Console.WriteLine("Arrived - C-3");
                producerCardsTasks = producerCards.Produce(recordsCards, config["CardsTopic"], databaseName, producerCardsTasks, cardsKeyRecords);
                Console.WriteLine("Arrived - C-4");
                producerH2hTasks = producerH2h.Produce(recordsH2h, config["H2hTopic"], databaseName, producerH2hTasks, h2hKeyRecords);
                Console.WriteLine("Arrived - C-5");

                // var forcedTask = producer.ProduceForced(records, containerOrTopicName, databaseName, producerTasks);    // uncomment if using ProducerForced method
                Console.WriteLine("Arrived - C-2");
                currentRecordsProduced += batchSize;
                

                // For debug purposes
                // Console.WriteLine(Convert.ToInt32(batchStopwatch.ElapsedMilliseconds));
                // Console.WriteLine(runningStopwatch.ElapsedMilliseconds);                

                // Await result from async data generation for next batch
                records = await generatorTask;
                recordsCateg = await generatorCategTask;
                recordsCards = await generatorCardsTask;
                recordsH2h = await generatorH2hTask;

                // Await batch duration (1 sec) if not exceeded
                Console.WriteLine("Arrived - Second");

                elapsedBatchTime = Convert.ToInt32(batchStopwatch.ElapsedMilliseconds);
                await WaitBatchDuration(batchDuration, elapsedBatchTime);
                await Task.WhenAll(producerTasks);

                // Check if simulation should stop
                if (CheckStopCondition(duration, runningStopwatch.ElapsedMilliseconds, totalRecordsToProduce, currentRecordsProduced) == true)
                {
                    Console.WriteLine("Arrived - Third-1");

                    await Task.WhenAll(producerTasks);
                    await Task.WhenAll(producerCardsTasks);
                    await Task.WhenAll(producerH2hTasks);
                    producerTasks.Clear();
                    producerCardsTasks.Clear();
                    producerH2hTasks.Clear();
                    Console.WriteLine("Arrived - Third-1-1");

                    await StopActions(producerTasks, initialRecordCount, currentRecordsProduced, duration,
                        producer, containerOrTopicName, databaseName);
                        // producer, containerOrTopicName, databaseName, forcedTask);   // uncomment if using ProducerForced method

                    break;    // break while running loop
                }
                else
                {
                    Console.WriteLine("Arrived - Third-2");

                    await Task.WhenAll(producerTasks);
                    await Task.WhenAll(producerCardsTasks);
                    await Task.WhenAll(producerH2hTasks);
                    producerTasks.Clear();
                    producerCardsTasks.Clear();
                    producerH2hTasks.Clear();

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