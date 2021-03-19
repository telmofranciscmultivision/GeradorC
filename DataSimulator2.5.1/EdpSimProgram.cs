using System;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using EdpSimulator.Controllers;
using EdpSimulator.Producers;
using EdpSimulator.QueryTools;

namespace EdpSimulator
{
    public class EdpSimProgram
    {
        static async Task Main(string[] args)
        {
            try
            {
                IConfigurationRoot config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();


                if (config["Controller"] == "JavaControllerStream")
                {
                    var controller = new JavaControllerStream(config);
                    await controller.Run();
                }
                else{


                    if (config["Controller"] == "GenericQueryControllerBatch")   // TODO properly
                    {
                        CosmosQueryExecutor queryExecutor = new CosmosQueryExecutor(config["EndpointUrl"], config["PasswordOrAuthorizationKey"]);
                        var controller = new GenericQueryControllerBatch<CosmosQueryExecutor>(config, queryExecutor);
                        await controller.Run();
                    }
                    else
                    {              
                        var controller = SelectProducerAndController(config);
                        await controller.Run();
                    }

                }

            }
            catch (Exception e)
            {
                Console.WriteLine("StackTrace:\n{0}", e.StackTrace);    // uncomment for stack trace
                Exception baseException = e.GetBaseException();
                Console.WriteLine("Error: {0}", e.Message);
            }  
        }

        public static IEdpController SelectProducerAndController(IConfigurationRoot config)
        {
            try
            {
                // Reflection to infer entity and generator types
                Type[] entityTypeArr = { Assembly.GetExecutingAssembly().GetType("EdpSimulator.Entities." + config["Entity"]) };
                Type[] generatorTypeArr = { Assembly.GetExecutingAssembly().GetType("EdpSimulator.Generators." + config["Generator"]) };
                Type generatorType = Assembly.GetExecutingAssembly().GetType("EdpSimulator.Generators." + config["Generator"]);

                // Create generator
                var generator = Activator.CreateInstance(generatorType);

                switch (config["Producer"])
                {
                    case "CosmosProducer":

                        Type cosmosProducerType = typeof(CosmosProducer<,>);

                        // Create producer
                        var cosmosProducerTypeWithGeneric = cosmosProducerType.MakeGenericType(entityTypeArr);
                        var producer = Activator.CreateInstance(
                            cosmosProducerTypeWithGeneric, config["EndpointUrl"], config["PasswordOrAuthorizationKey"],Boolean.Parse(config["Upsert"]));

                        // Reflection to infer generic types for controller
                        Type[] cosmosTypesArr = { 
                            Assembly.GetExecutingAssembly().GetType("EdpSimulator.Entities." + config["Entity"]), 
                            Assembly.GetExecutingAssembly().GetType("EdpSimulator.Generators." + config["Generator"]), 
                            producer.GetType()
                        };
                        return SelectController(config, generator, producer, cosmosTypesArr);

                    case "KafkaProducer":

                        Type kafkaProducerType = typeof(KafkaProducer<,>);
                        
                        // Create producer
                        var kafkaProducerTypeWithGeneric = kafkaProducerType.MakeGenericType(entityTypeArr);
                        var kakfaProducer = Activator.CreateInstance(
                            kafkaProducerTypeWithGeneric, config["EndpointUrl"], config["SchemaRegistryUrl"], Boolean.Parse(config["Avro"]) );

                        // Reflection to infer generic types for controller
                        Type[] kafkaTypesArr = { 
                            Assembly.GetExecutingAssembly().GetType("EdpSimulator.Entities." + config["Entity"]), 
                            Assembly.GetExecutingAssembly().GetType("EdpSimulator.Generators." + config["Generator"]), 
                            kakfaProducer.GetType()
                        };
                        return SelectController(config, generator, kakfaProducer, kafkaTypesArr);

                    default:
                        throw new ArgumentException("Producer was not filled properly in appsettings. Check class names.");    
                }
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                throw new ArgumentException(
                    String.Format("Entity, Generator or Producer were not filled properly in appsettings. Check available class names.\n{0}", e.GetBaseException() ));
            }
        }

        public static IEdpController SelectController<TGen,TProd>(IConfigurationRoot config, TGen generator, TProd producer, Type[] typesArr)
        {
            try
            {
                switch (config["Controller"])
                {
                    case "GenericControllerStream":

                        // Create controller
                        var genericControllerStreamType = typeof(GenericControllerStream<,,,>);
                        var genericControllerStreamTypeWithGeneric = genericControllerStreamType.MakeGenericType(typesArr);          
                        return (IEdpController)Activator.CreateInstance(genericControllerStreamTypeWithGeneric, config, generator, producer);

                    case "GenericQueryLoadTesting":

                        // TODO

                        return null;
                    
                    default:
                        // Reflection to infer controller type with 3 generic parameters
                        Type controllerType = Assembly.GetExecutingAssembly().GetType("EdpSimulator.Controllers." + config["Controller"] + "`3");

                        // Create generic controller
                        var controllerTypeWithGeneric = controllerType.MakeGenericType(typesArr);
                        return (IEdpController)Activator.CreateInstance(
                            controllerTypeWithGeneric, config, generator, producer);
                }
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                throw new ArgumentException(
                    String.Format("Controller was not filled properly in appsettings. Check available class names.\n{0}", e.GetBaseException() ));
            }
        }
    }
}