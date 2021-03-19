using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;


namespace EdpSimulator.Producers
{
    public class KafkaProducer<KRecord,TRecord> : IEdpProducer<KRecord,TRecord> 
        where KRecord : IEdpRecord
        where TRecord : IEdpRecord
    {
        private string endpointUrl;
        private ProducerConfig producerConfig;
        private ConsumerConfig consumerConfig;
        private IProducer<KRecord, TRecord> kafkaProducer;
        private IConsumer<KRecord, TRecord> kafkaConsumer;
        private IAdminClient kafkaAdmin;
        private ISchemaRegistryClient schemaRegistryClient;
        bool equallySpacedInTime = true;  // only makes sense if batchTime = 1 second at the moment

        public KafkaProducer(string endpointUrl, string schemaRegistryUrl=null, bool avro=false)
        {
            try 
            {
                this.endpointUrl = endpointUrl;

                if ( String.IsNullOrEmpty(endpointUrl) )
                    throw new ArgumentException("EndpointUrl was not filled properly in appsettings.");

                this.producerConfig = new ProducerConfig
                {
                    BootstrapServers = endpointUrl,  // host:9092
                    LingerMs = 10,  //100,
                    // BatchSize = 128
                };
                this.consumerConfig = new ConsumerConfig
                {
                    BootstrapServers = endpointUrl,  // host:9092
                    GroupId = "EdpSim",
                    AutoOffsetReset = AutoOffsetReset.Earliest
                };

                if (avro != true)
                {
                    kafkaProducer = new ProducerBuilder<KRecord, TRecord>(producerConfig).Build();
                    kafkaConsumer = new ConsumerBuilder<KRecord, TRecord>(consumerConfig).Build();
                }
                else
                {
                    this.schemaRegistryClient = new CachedSchemaRegistryClient(
                        new SchemaRegistryConfig{ Url = schemaRegistryUrl });

                    kafkaProducer = new ProducerBuilder<KRecord, TRecord>(producerConfig)
                        .SetKeySerializer(new AvroSerializer<KRecord>(schemaRegistryClient, new AvroSerializerConfig { AutoRegisterSchemas = true }))
                        .SetValueSerializer(new AvroSerializer<TRecord>(schemaRegistryClient, new AvroSerializerConfig { AutoRegisterSchemas = true }))
                        .Build();

                    kafkaConsumer = new ConsumerBuilder<KRecord, TRecord>(consumerConfig)
                        .SetKeyDeserializer(new AvroDeserializer<KRecord>(schemaRegistryClient).AsSyncOverAsync())
                        .SetValueDeserializer(new AvroDeserializer<TRecord>(schemaRegistryClient).AsSyncOverAsync())
                        .Build();
                }
                kafkaAdmin = new AdminClientBuilder(producerConfig).Build();
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                throw new InvalidOperationException(
                    String.Format("Not able to create Kafka client. EndpointUrl must be host:9092. Avro implementation not tested.\n{0}", e.GetBaseException() ));
            } 
        }

        public List<Task> Produce(List<TRecord> records, string containerOrTopicName, string databaseName=null, List<Task> producerTasks=null, List<KRecord> keySchema = null)         
        {
            try
            {
                if (producerTasks == null) producerTasks = new List<Task>();

                int recordDelay = (equallySpacedInTime == true) ? Convert.ToInt32((1000 / records.Count) * 0.9) : 0;
                // times 0.9 as to give some margin within 1 second
                for (int i = 0; i < records.Count; i++)
                {
                    if (keySchema != null)
                    
                        {
                             producerTasks.Add(kafkaProducer.ProduceAsync(
                                containerOrTopicName, new Message<KRecord, TRecord>
                                {
                                    Key = keySchema[i],
                                    Value = records[i]
                                }));
                        }
                                
             //   Task.Delay(recordDelay);
                    
                };
                    
                return producerTasks;
            }
            catch (Exception e)
            {
                Console.WriteLine("Not able to produce to Kafka.Check topic in appsettings and Kafka instance.\n{ 0}", e.GetBaseException());

                throw new InvalidOperationException(
                    String.Format("Not able to produce to Kafka. Check topic in appsettings and Kafka instance.\n{0}", e.GetBaseException() ));
            }
        }

        public Task ProduceForced(List<TRecord> records, string containerOrTopicName, string databaseName=null, List<Task> producerTasks=null, List<KRecord> keySchema = null)
        {
            try 
            {
                if (producerTasks == null) producerTasks = new List<Task>();

                int recordDelay = (equallySpacedInTime == true) ? Convert.ToInt32( (1000 / records.Count) * 0.9 ) : 0;
                // times 0.9 as to give some margin within 1 second

                return Task.Run( () =>
                {
                    for (int i = 0; i < records.Count; i++)
                    {

                        producerTasks.Add(kafkaProducer.ProduceAsync(
                            containerOrTopicName, new Message<KRecord, TRecord>
                            {
                                Key = keySchema[i],
                                Value = records[i]
                            }));

                //        Task.Delay(recordDelay);

                    };
                } );
            }
            catch (Exception e)
            {
                Console.WriteLine("Not able to produce to Kafka.Check topic in appsettings and Kafka instance.\n{ 0}", e.GetBaseException());

                throw new InvalidOperationException(
                    String.Format("Not able to produce to Kafka. Check topic in appsettings and Kafka instance.\n{0}", e.GetBaseException() ));
            }
        }

        public Task<int> CountRecords(string containerOrTopicName, string databaseName=null)
        {
            try
            {
                // kafkaConsumer.Subscribe(containerOrTopicName);

                return Task<int>.Factory.StartNew(() =>    // to convert to task
                {
                    Console.WriteLine("Arrived");
                    int recordCount = 0;
                    int numPartitions = kafkaAdmin.GetMetadata(containerOrTopicName, TimeSpan.FromSeconds(5)).Topics[0].Partitions.Count;
                    List<Task<int>> tasks = new List<Task<int>>();

                    for(int i = 0; i < numPartitions; i++)  
                    {
                        Console.WriteLine("Arrived");
                        WatermarkOffsets wo = kafkaConsumer.GetWatermarkOffsets(new TopicPartition(containerOrTopicName, i));
                       // WatermarkOffsets wo = kafkaConsumer.QueryWatermarkOffsets(new TopicPartition(containerOrTopicName, i), TimeSpan.FromSeconds(5));
                        recordCount += Convert.ToInt32(wo.High.Value - wo.Low.Value);
                    }
                    Console.WriteLine("Offset count is {0} on all partitions.", recordCount);
                    return recordCount;
                });
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    String.Format("Not possible to count Kafka offsets.\n{0}", e.GetBaseException() ));
            }
        }
    }
}
        

        




