using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using EdpSimulator.Entities;
using AutoMapper;
using key.SOURCEDB.HTH000PA.HTHPA;

namespace EdpSimulator.Generators
{
    public class JavaH2hGenerator //: IEdpGenerator<JavaH2hRecord>
    {
        protected Random randomGen;
        protected Stopwatch generatorStopwatch;
        // protected Func<int,Task> generateRecordsFunc;
        protected static DateTime dateState;    // example state variable for data generation. not used at the moment
        // private Tuple<string,int>[] auxData;

        // TODO auxData need to go to external file
        public JavaH2hGenerator() 
        {
            randomGen = new Random();
            generatorStopwatch = new Stopwatch();
        }

        public virtual JavaH2hRecord[] BuildRecords(int batchSize)
        {
            JavaH2hRecord[] records = new JavaH2hRecord[batchSize];
            for(int i=0; i < records.Length; i++)
            {
                records[i] = new JavaH2hRecord();
            }
            return records;
        }

        public virtual JavaH2hRecord[] GenerateRecords(JavaH2hRecord[] records, int[] IDMSGBAN_2, long[] CONTA_CLI, long[] IDMSGBAN_1)
        {
            try {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<JavaH2hImage, AfterImageH2h>());
                var mapper = new Mapper(config);

                for (int i=0; i < records.Length; i++)
                {
                    // TODO implement date state mechanism as in previous simulator

                    records[i].afterImage = mapper.Map<AfterImageH2h>(new JavaH2hImage());
                    records[i].A_ENTTYP = randomGen.Next(1000000).ToString();
                    records[i].A_CCID = randomGen.Next(1000000).ToString();
                    records[i].A_TIMSTAMP = randomGen.Next(1000000).ToString();
                    records[i].afterImage.IDMSGBAN_2 = IDMSGBAN_2[i];
                    records[i].afterImage.CONTA_CLI = (int)CONTA_CLI[i];
                    records[i].afterImage.IDMSGBAN_1 = (int)IDMSGBAN_1[i];
                    records[i].afterImage.APLIC_N = "H2H_TOPIC";
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    String.Format("Check Generator class for invalid data generation or NullReference.\n{0}", e.GetBaseException() ));
            }
            return records;
        }

        public virtual Task<List<JavaH2hRecord>> GenerateRecordsAsync(int batchSize, int[] IDMSGBAN_2, long[] CONTA_CLI, long[] IDMSGBAN_1, bool backgroundThread=false)
        {
            if (backgroundThread != true)
            {
                var task = new Task<List<JavaH2hRecord>>( () => 
                {
                    generatorStopwatch.Start();
                    JavaH2hRecord[] records = GenerateRecords( BuildRecords(batchSize),IDMSGBAN_2,CONTA_CLI,IDMSGBAN_1 );  // 1st Build, 2nd Generate, 3rd Creates Task
                    generatorStopwatch.Stop();

                    Console.WriteLine("Batch data async generation of {0} records took {1} ms.", batchSize, generatorStopwatch.ElapsedMilliseconds);
                    generatorStopwatch.Reset();

                    return new List<JavaH2hRecord>(records);
                } );
                task.Start();
                return task;
            }
            else
            {
                return Task.Run( () =>
                {
                    generatorStopwatch.Start();
                    JavaH2hRecord[] records = GenerateRecords( BuildRecords(batchSize),IDMSGBAN_2,CONTA_CLI,IDMSGBAN_1  );  // 1st Build, 2nd Generate, 3rd Creates Task
                    generatorStopwatch.Stop();

                    Console.WriteLine("Batch data async generation of {0} records took {1} ms.", batchSize, generatorStopwatch.ElapsedMilliseconds);
                    generatorStopwatch.Reset();

                    return new List<JavaH2hRecord>(records);
                } );
            }
        }

        public virtual HTHPSE_T[] BuildKey(int batchSize)
        {
            HTHPSE_T[] documents = new HTHPSE_T[batchSize];
            for (int i = 0; i < documents.Length; i++)
            {
                documents[i] = new HTHPSE_T();
            }
            return documents;
        }

        public virtual List<HTHPSE_T> GenerateH2h(HTHPSE_T[] records)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<JavaDdaImage, @value.SOURCEDB.DEP000PA.DEPPA.HMV_DDA>());
            var mapper = new Mapper(config);
            try
            {
                for (int i = 0; i < records.Length; i++)
                {

                    records[i].IDMSGBAN_1 = randomGen.Next(1000000).ToString();
                    records[i].IDMSGBAN_2 = randomGen.Next(1000000).ToString();

                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    String.Format("Check Generator class for invalid data generation or NullReference.\n{0}", e.GetBaseException()));
            }
            return new List<HTHPSE_T>(records);
        }
    }
}