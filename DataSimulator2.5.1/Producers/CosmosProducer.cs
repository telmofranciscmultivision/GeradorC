using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
// using System.Dynamic;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json.Linq;
using EdpSimulator.QueryTools;
using Avro.Specific;
using Newtonsoft.Json;
namespace EdpSimulator.Producers
{
    public class CosmosProducer<KRecord, TRecord> : IEdpProducer<KRecord, TRecord> 
        where TRecord : ISpecificRecord
        where KRecord : ISpecificRecord
    {
        protected string endpointUrl;
        protected string passwordOrAuthorizationKey;
        protected CosmosClient cosmosClient;
        protected CosmosQueryExecutor cosmosQueryExecutor;
        protected Func< Container,JObject,PartitionKey,ItemRequestOptions, Task<ItemResponse<JObject>> > produceToCosmos;    // Func delegate
        protected ItemRequestOptions itemReqOpt = new ItemRequestOptions(){};   // No options at the moment

        protected static JObject AddCosmosId(TRecord itemWithoutId)
        {
            JObject tmpJObject = JObject.FromObject(itemWithoutId);
            tmpJObject["id"] = itemWithoutId.GetHashCode();
            return tmpJObject;
        }

        public CosmosProducer(string endpointUrl, string passwordOrAuthorizationKey, bool upsert)
        {
            try 
            {
                this.endpointUrl = endpointUrl;
                this.passwordOrAuthorizationKey = passwordOrAuthorizationKey;

                if ( String.IsNullOrEmpty(endpointUrl) || String.IsNullOrEmpty(passwordOrAuthorizationKey) )
                    throw new ArgumentException("EndpointUrl or PasswordOrAuthorizationKey were not filled properly in appsettings.");

                cosmosClient = new CosmosClient(endpointUrl, passwordOrAuthorizationKey, new CosmosClientOptions() { AllowBulkExecution = true });
                cosmosQueryExecutor = new CosmosQueryExecutor(endpointUrl, passwordOrAuthorizationKey);

                if (upsert != true) 
                {
                    produceToCosmos = (Container container, JObject tmpJObject, PartitionKey partitionKey, ItemRequestOptions itemReqOpt) =>
                        container.CreateItemAsync<JObject>(tmpJObject, partitionKey, itemReqOpt);
                }
                else
                {
                    // TODO fix id when upsert == true to replace existing document

                    produceToCosmos = (Container container, JObject tmpJObject, PartitionKey partitionKey, ItemRequestOptions itemReqOpt) =>
                        container.UpsertItemAsync<JObject>(tmpJObject, partitionKey, itemReqOpt);
                }
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                throw new InvalidOperationException(
                    String.Format("Not able to create CosmosClient. Check appsettings and Cosmos DB instance.\n{0}", e.GetBaseException() ));
            }
        }

        public List<Task> Produce(List<TRecord> records, string containerOrTopicName, string databaseName, List<Task> producerTasks=null, List<KRecord> keySchema = null)
        {
            try {
                if (producerTasks == null) producerTasks = new List<Task>();
                Container container = cosmosClient.GetContainer(databaseName, containerOrTopicName);

                foreach(TRecord record in records)
                {
                    // Console.WriteLine(records[i].ToString());
                    producerTasks.Add( produceToCosmos(container, AddCosmosId(record), new PartitionKey(record.GetHashCode()), itemReqOpt) );               
                }
                return producerTasks;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    String.Format("Not able to produce to Cosmos. Check if record has an \"id\" field.\n{0}", e.GetBaseException() ));
            }
        }

        public async Task<int> CountRecords(string containerOrTopicName, string databaseName)
        {
            try 
            {
                // first time query is executed for Cosmos DB to compile. This way query times are not negatively affected
                List< Task<QueryResponse> > firstQueryForCompilationTask = cosmosQueryExecutor.ExecuteQueryBulk(
                    "SELECT VALUE COUNT(1) FROM c", queryBatchSize: 1, returnResults: false, containerOrTopicName, databaseName);
                await Task.WhenAll(firstQueryForCompilationTask);
                
                // Measure query time
                Stopwatch queryStopwatch = new Stopwatch();
                queryStopwatch.Start();               

                List< Task<QueryResponse> > queryTasks = cosmosQueryExecutor.ExecuteQueryBulk(
                    "SELECT VALUE COUNT(1) FROM c", queryBatchSize: 1, returnResults: true, containerOrTopicName, databaseName);
                
                QueryResponse[] queryResults = await Task.WhenAll(queryTasks);    // await for result
                queryStopwatch.Stop();

                int recordCount = int.Parse(CosmosQueryExecutor.GetQueryFirstResult(queryResults));
                Console.WriteLine("Record count is {0}, query time was {1} ms.", recordCount, queryStopwatch.ElapsedMilliseconds);

                return recordCount;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    String.Format("Not possible to count records with query. Check producer class.\n{0}", e.GetBaseException() ));
            }
        }
    }
}