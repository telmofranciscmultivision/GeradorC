using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using EdpSimulator.Entities;

namespace EdpSimulator.Generators
{
    public class JavaCategoryGenerator // : IEdpGenerator<JoinTransactionRecord>
    {
        protected Random randomGen;
        protected Stopwatch generatorStopwatch;
        protected static DateTime dateState;

        public JavaCategoryGenerator() 
        {
            randomGen = new Random();
            generatorStopwatch = new Stopwatch();
        }

        public virtual JavaCategoryRecord[] BuildRecords(int batchSize)
        {
            JavaCategoryRecord[] documents = new JavaCategoryRecord[batchSize];
            for(int i=0; i < documents.Length; i++)
            {
                documents[i] = new JavaCategoryRecord();
            }
            return documents;
        }

        public virtual JavaCategoryRecord[] GenerateRecords(JavaCategoryRecord[] documents, string[] transactionids, string[] accountids)
        {
            try {
                for(int i=0; i < documents.Length; i++)
                {
                    documents[i].transactionid = transactionids[i];
                    documents[i].accountid = accountids[i];
                    documents[i].category = randomGen.Next(11);
                    documents[i].subcategory = randomGen.Next(68);
                    documents[i].sourceengine = Guid.NewGuid().ToString();
                    documents[i].engineversion = Guid.NewGuid().ToString();
                    documents[i].modelversion = Guid.NewGuid().ToString();
                    documents[i].predictionconfidence = "CATEGORY_TOPIC";
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    String.Format("Check Generator class for invalid data generation or NullReference.\n{0}", e.GetBaseException() ));
            }
            return documents;
        }

        public virtual Task<List<JavaCategoryRecord>> GenerateRecordsAsync(int batchSize, string[] transactionids, string[] accountids, bool backgroundThread=false)
        {
            if (backgroundThread != true)
            {
                var task = new Task<List<JavaCategoryRecord>>( () => 
                {
                    generatorStopwatch.Start();
                    JavaCategoryRecord[] records = GenerateRecords( BuildRecords(batchSize), transactionids,accountids);
                    generatorStopwatch.Stop();

                    Console.WriteLine("Batch data async generation of {0} records took {1} ms.", batchSize, generatorStopwatch.ElapsedMilliseconds);
                    generatorStopwatch.Reset();

                    return new List<JavaCategoryRecord>(records);
                } );
                task.Start();
                return task;
            }
            else
            {
                return Task.Run( () =>
                {
                    generatorStopwatch.Start();
                    JavaCategoryRecord[] records = GenerateRecords( BuildRecords(batchSize), transactionids,accountids);
                    generatorStopwatch.Stop();

                    Console.WriteLine("Batch data async generation of {0} records took {1} ms.", batchSize, generatorStopwatch.ElapsedMilliseconds);
                    generatorStopwatch.Reset();

                    return new List<JavaCategoryRecord>(records);
                } );
            }
        }

        public virtual categoryKey[] BuildKey(int batchSize)
        {
            categoryKey[] documents = new categoryKey[batchSize];
            for (int i = 0; i < documents.Length; i++)
            {
                documents[i] = new categoryKey();
            }
            return documents;
        }

        public virtual List<categoryKey> GenerateCategories(categoryKey[] records)
        {

            try
            {
                for (int i = 0; i < records.Length; i++)
                {
                    records[i].transactionid = randomGen.Next(1000000).ToString();
                    records[i].accountid = randomGen.Next(1000000).ToString();
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    String.Format("Check Generator class for invalid data generation or NullReference.\n{0}", e.GetBaseException()));
            }
            return new List<categoryKey>(records);
        }
    }
}