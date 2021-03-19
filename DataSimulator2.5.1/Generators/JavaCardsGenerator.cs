using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using EdpSimulator.Entities;
using AutoMapper;
using key.SOURCEDB.CAM00020.CAM20;

namespace EdpSimulator.Generators
{
    public class JavaCardsGenerator //: IEdpGenerator<JavaCardsRecord>
    {
        protected Random randomGen;
        protected Stopwatch generatorStopwatch;
        // protected Func<int,Task> generateRecordsFunc;
        protected static DateTime dateState;    // example state variable for data generation. not used at the moment
        // private Tuple<string,int>[] auxData;

        // TODO auxData need to go to external file
        public JavaCardsGenerator() 
        {
            randomGen = new Random();
            generatorStopwatch = new Stopwatch();
        }

        public virtual JavaCardsRecord[] BuildRecords(int batchSize)
        {
            JavaCardsRecord[] records = new JavaCardsRecord[batchSize];
            for(int i=0; i < records.Length; i++)
            {
                records[i] = new JavaCardsRecord();
            }
            return records;
        }

        public virtual JavaCardsRecord[] GenerateRecords(JavaCardsRecord[] records, string[] TXN_DT, int[] TXN_TM, string[] TXN_AMT, string[] TXN_SINAL, string[] ORIG_PLAS, int[] ACCT_COID, string[] ACCT_NBR)
        {
            try {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<JavaCardsImage, AfterImageCards>());
                var mapper = new Mapper(config);
                for (int i=0; i < records.Length; i++)
                {
                    // TODO implement date state mechanism as in previous simulator
                    
                    records[i].afterImage = mapper.Map<AfterImageCards>(new JavaCardsImage());
                    records[i].A_ENTTYP = randomGen.Next(1000000).ToString();
                    records[i].A_CCID = randomGen.Next(1000000).ToString();
                    records[i].A_TIMSTAMP = randomGen.Next(1000000).ToString();

                    records[i].afterImage.ORIG_PLAS = ORIG_PLAS[i];
                    records[i].afterImage.TXN_AMT = TXN_AMT[i];
                    records[i].afterImage.TXN_SINAL = TXN_SINAL[i];
                    records[i].afterImage.TXN_DT = TXN_DT[i];
                    records[i].afterImage.TXN_TM = TXN_TM[i];
                    records[i].afterImage.ACCT_COID = ACCT_COID[i];
                    records[i].afterImage.ACCT_NBR = ACCT_NBR[i];

                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    String.Format("Check Generator class for invalid data generation or NullReference.\n{0}", e.GetBaseException() ));
            }
            return records;
        }

        public virtual Task<List<JavaCardsRecord>> GenerateRecordsAsync(int batchSize, string[] TXN_DT, int[] TXN_TM, string[] TXN_AMT, string[] TXN_SINAL, string[] ORIG_PLAS, int[] ACCT_COID, string[] ACCT_NBR, bool backgroundThread=false)
        {
            if (backgroundThread != true)
            {
                var task = new Task<List<JavaCardsRecord>>( () => 
                {
                    generatorStopwatch.Start();
                    JavaCardsRecord[] records = GenerateRecords( BuildRecords(batchSize),TXN_DT,TXN_TM,TXN_AMT,TXN_SINAL,ORIG_PLAS,ACCT_COID,ACCT_NBR);  // 1st Build, 2nd Generate, 3rd Creates Task
                    generatorStopwatch.Stop();

                    Console.WriteLine("Batch data async generation of {0} records took {1} ms.", batchSize, generatorStopwatch.ElapsedMilliseconds);
                    generatorStopwatch.Reset();

                    return new List<JavaCardsRecord>(records);
                } );
                task.Start();
                return task;
            }
            else
            {
                return Task.Run( () =>
                {
                    generatorStopwatch.Start();
                    JavaCardsRecord[] records = GenerateRecords( BuildRecords(batchSize),TXN_DT,TXN_TM,TXN_AMT,TXN_SINAL,ORIG_PLAS,ACCT_COID,ACCT_NBR);  // 1st Build, 2nd Generate, 3rd Creates Task
                    generatorStopwatch.Stop();

                    Console.WriteLine("Batch data async generation of {0} records took {1} ms.", batchSize, generatorStopwatch.ElapsedMilliseconds);
                    generatorStopwatch.Reset();

                    return new List<JavaCardsRecord>(records);
                } );
            }
        }

        public virtual LOGTXN_T[] BuildKey(int batchSize)
        {
            LOGTXN_T[] documents = new LOGTXN_T[batchSize];
            for (int i = 0; i < documents.Length; i++)
            {
                documents[i] = new LOGTXN_T();
            }
            return documents;
        }

        public virtual List<LOGTXN_T> GenerateCards(LOGTXN_T[] records)
        {
            try
            {
                for (int i = 0; i < records.Length; i++)
                {
                    records[i].ACCT_COID = randomGen.Next(1000000).ToString();
                    records[i].ACCT_NBR = randomGen.Next(1000000).ToString();
                    records[i].ACCT_PROD = randomGen.Next(1000000).ToString();
                    records[i].FILE_ORIG = randomGen.Next(1000000).ToString();
                    records[i].PSTG_DT = randomGen.Next(1000000).ToString();
                    records[i].PSTG_SEQ = randomGen.Next(1000000).ToString();

                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    String.Format("Check Generator class for invalid data generation or NullReference.\n{0}", e.GetBaseException()));
            }
            return new List<LOGTXN_T>(records);
        }
    }
}