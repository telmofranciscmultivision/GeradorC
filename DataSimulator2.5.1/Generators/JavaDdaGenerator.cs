using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using EdpSimulator.Entities;
using AutoMapper;
using key.SOURCEDB.DEP000PA.DEPPA;

namespace EdpSimulator.Generators
{
    public partial class JavaDdaGenerator // : IEdpGenerator<JavaDdaRecord>
    {
        protected Random randomGen;
        protected Stopwatch generatorStopwatch;
        protected static DateTime dateState;

        public JavaDdaGenerator() 
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

        public virtual HMV_DDA[] BuildKey(int batchSize)
        {
            HMV_DDA[] documents = new HMV_DDA[batchSize];
            for (int i = 0; i < documents.Length; i++)
            {
                documents[i] = new HMV_DDA();
            }
            return documents;
        }

        public virtual List<HMV_DDA> GenerateDda(HMV_DDA[] records)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<JavaDdaImage, @value.SOURCEDB.DEP000PA.DEPPA.HMV_DDA>());
            var mapper = new Mapper(config);
            try
            {
                for (int i = 0; i < records.Length; i++)
                {

                    records[i].HDDA_KEY_CONTA = randomGen.Next(1000000).ToString();
                    records[i].HDDA_KEY_DATA_HORA = randomGen.Next(1000000).ToString();
                    records[i].HDDA_KEY_DATA_LANCAMENTO = randomGen.Next(1000000).ToString();

                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    String.Format("Check Generator class for invalid data generation or NullReference.\n{0}", e.GetBaseException()));
            }
            return new List<HMV_DDA>(records);
        }

        public virtual JavaDdaRecord[] GenerateRecords(JavaDdaRecord[] documents, int[] HDDA_KEY_COMP, int[] HDDA_KEY_CONTA, string[] HDDA_KEY_DATA_LANCAMENTO,
            string[] HDDA_KEY_ORIGEM, string[] HDDA_KEY_DATA_HORA,
            string[] HDDA_HST_DESCR_CONT, string[] HDDA_HST_TAMT, int[] HDDA_HST_TRAN_SIGN, string[] HDDA_HST_DATA_SISTEMA, int[] HDDA_HST_HORA,
            string[] HDDA_HST_TRACE_ID
             )
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<JavaDdaImage,@value.SOURCEDB.DEP000PA.DEPPA.HMV_DDA>());
            var mapper = new Mapper(config);
            try {
                for(int i=0; i < documents.Length; i++)
                {
                    documents[i].afterImage = mapper.Map<@value.SOURCEDB.DEP000PA.DEPPA.HMV_DDA>(new JavaDdaImage());
                    documents[i].A_ENTTYP = randomGen.Next(1000000).ToString();
                    documents[i].afterImage.HDDA_KEY_COMP = HDDA_KEY_COMP[i];
                    documents[i].afterImage.HDDA_KEY_CONTA = HDDA_KEY_CONTA[i];
                    documents[i].afterImage.HDDA_KEY_DATA_LANCAMENTO = HDDA_KEY_DATA_LANCAMENTO[i];
                    documents[i].afterImage.HDDA_KEY_ORIGEM = HDDA_KEY_ORIGEM[i];
                    documents[i].afterImage.HDDA_KEY_DATA_HORA = HDDA_KEY_DATA_HORA[i];

                    documents[i].afterImage.HDDA_HST_DESCR_CONT = HDDA_HST_DESCR_CONT[i];
                    documents[i].afterImage.HDDA_HST_TAMT = HDDA_HST_TAMT[i];
                    documents[i].afterImage.HDDA_HST_TRAN_SIGN = HDDA_HST_TRAN_SIGN[i];
                    documents[i].afterImage.HDDA_HST_DATA_SISTEMA = HDDA_HST_DATA_SISTEMA[i];
                    documents[i].afterImage.HDDA_HST_HORA = HDDA_HST_HORA[i];
                    documents[i].afterImage.HDDA_HST_STMT_REVERS = "DDA_TOPIC";

                    documents[i].afterImage.HDDA_HST_TRACE_ID = HDDA_HST_TRACE_ID[i];

                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    String.Format("Check Generator class for invalid data generation or NullReference.\n{0}", e.GetBaseException() ));
            }
            return documents;
        }

        public virtual Task<List<JavaDdaRecord>> GenerateRecordsAsync(int batchSize,  int[] HDDA_KEY_COMP, int[] HDDA_KEY_CONTA, string[] HDDA_KEY_DATA_LANCAMENTO,
            string[] HDDA_KEY_ORIGEM, string[] HDDA_KEY_DATA_HORA,
            string[] HDDA_HST_DESCR_CONT, string[] HDDA_HST_TAMT, int[] HDDA_HST_TRAN_SIGN, string[] HDDA_HST_DATA_SISTEMA, int[] HDDA_HST_HORA, string[] HDDA_HST_TRACE_ID,
            bool backgroundThread=true)
        {
            if (backgroundThread != true)
            {
                var task = new Task<List<JavaDdaRecord>>( () => 
                {
                    generatorStopwatch.Start();
                    JavaDdaRecord[] records = GenerateRecords( BuildRecords(batchSize), HDDA_KEY_COMP,HDDA_KEY_CONTA,HDDA_KEY_DATA_LANCAMENTO,HDDA_KEY_ORIGEM,HDDA_KEY_DATA_HORA,
                    HDDA_HST_DESCR_CONT, HDDA_HST_TAMT, HDDA_HST_TRAN_SIGN, HDDA_HST_DATA_SISTEMA, HDDA_HST_HORA,HDDA_HST_TRACE_ID);

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
                    JavaDdaRecord[] records = GenerateRecords( BuildRecords(batchSize), HDDA_KEY_COMP,HDDA_KEY_CONTA,HDDA_KEY_DATA_LANCAMENTO,HDDA_KEY_ORIGEM,HDDA_KEY_DATA_HORA,
                    HDDA_HST_DESCR_CONT, HDDA_HST_TAMT, HDDA_HST_TRAN_SIGN, HDDA_HST_DATA_SISTEMA, HDDA_HST_HORA,HDDA_HST_TRACE_ID);
                    generatorStopwatch.Stop();

                    Console.WriteLine("Batch data async generation of {0} records took {1} ms.", batchSize, generatorStopwatch.ElapsedMilliseconds);
                    generatorStopwatch.Reset();

                    return new List<JavaDdaRecord>(records);
                } );
            }
        }
    }
}