using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using EdpSimulator.Entities;

namespace EdpSimulator.Generators
{
    public class JoinCategoryGenerator // : IEdpGenerator<JoinTransactionRecord>
    {
        protected Random randomGen;
        protected Stopwatch generatorStopwatch;
        protected static DateTime dateState;

        public JoinCategoryGenerator() 
        {
            randomGen = new Random();
            generatorStopwatch = new Stopwatch();
        }

        public virtual JavaDdaRecord[] BuildRecords(int batchSize)
        {
            JavaDdaRecord[] documents = new JavaDdaRecord[batchSize];
            for(int i=0; i < documents.Length; i++)
            {
                documents[i] = new JavaDdaRecord();
            }
            return documents;
        }

        public virtual JavaDdaRecord[] GenerateRecords(JavaDdaRecord[] documents, string[] transactionids)
        {
            try {
                for(int i=0; i < documents.Length; i++)
                {
                  //  documents[i].transactionid = transactionids[i];
                   // documents[i].accountid = Guid.NewGuid().ToString();
                    //documents[i].category = randomGen.Next(68);
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    String.Format("Check Generator class for invalid data generation or NullReference.\n{0}", e.GetBaseException() ));
            }
            return documents;
        }

        public virtual Task<List<JavaDdaRecord>> GenerateRecordsAsync(int batchSize, string[] transactionids, bool backgroundThread=false)
        {
            if (backgroundThread != true)
            {
                var task = new Task<List<JavaDdaRecord>>( () => 
                {
                    generatorStopwatch.Start();
                    JavaDdaRecord[] records = GenerateRecords( BuildRecords(batchSize), transactionids );
                    generatorStopwatch.Stop();

                    Console.WriteLine("Batch data async generation of {0} records took {1} ms.", batchSize, generatorStopwatch.ElapsedMilliseconds);
                    generatorStopwatch.Reset();

                    return new List<JavaDdaRecord>(records);
                } );
                task.Start();
                return task;
            }
            else
            {
                return Task.Run( () =>
                {
                    generatorStopwatch.Start();
                    JavaDdaRecord[] records = GenerateRecords( BuildRecords(batchSize), transactionids );
                    generatorStopwatch.Stop();

                    Console.WriteLine("Batch data async generation of {0} records took {1} ms.", batchSize, generatorStopwatch.ElapsedMilliseconds);
                    generatorStopwatch.Reset();

                    return new List<JavaDdaRecord>(records);
                } );
            }
        }
    }
}