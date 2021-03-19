using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;

namespace EdpSimulator.QueryTools
{
    public class CosmosQueryExecutor : IEdpQueryExecutor
    {
        protected CosmosClient cosmosClient;
        protected QueryRequestOptions options;
        protected string endpointUrl;
        protected string passwordOrAuthorizationKey;

        public CosmosQueryExecutor(string endpointUrl, string passwordOrAuthorizationKey)
        {
            try
            {
                this.endpointUrl = endpointUrl;
                this.passwordOrAuthorizationKey = passwordOrAuthorizationKey;

                if ( String.IsNullOrEmpty(endpointUrl)|| String.IsNullOrEmpty(passwordOrAuthorizationKey) )
                throw new ArgumentException("EndpointUrl or PasswordOrAuthorizationKey were not filled properly in appsettings.");

                cosmosClient = new CosmosClient(endpointUrl, passwordOrAuthorizationKey, new CosmosClientOptions() { AllowBulkExecution = true });
                options = new QueryRequestOptions() { MaxConcurrency = -1 };
                
                // Other QueryRequestOptions might be worth exploring:
                // https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.cosmos.queryrequestoptions?view=azure-dotnet
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                throw new InvalidOperationException(
                    String.Format("Not able to create CosmosClient. Check appsettings and Cosmos DB instance.\n{0}", e.GetBaseException() ));
            }
        }

        private async Task<QueryResponse> ExecuteQuery(QueryDefinition sqlQuery, Container container, bool returnResults)
        {
            List<string> resultingDocuments = new List<string>();
            Stopwatch queryTimeStopwatch = new Stopwatch();

            try
            {
                using (FeedIterator<object> queryIterator = container.GetItemQueryIterator<object>(sqlQuery, requestOptions: options))
                {
                    queryTimeStopwatch.Start();
                    while (queryIterator.HasMoreResults)
                    {
                        foreach (var document in await queryIterator.ReadNextAsync())
                        {
                            if (returnResults != true) resultingDocuments.Add(null);
                            else resultingDocuments.Add(JsonConvert.SerializeObject(document));
                        }
                    }
                    queryTimeStopwatch.Stop();  // includes time from JSON deserialization and retrieval of all docs
                }
                return new QueryResponse(resultingDocuments, Convert.ToInt32(queryTimeStopwatch.ElapsedMilliseconds));
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                throw new InvalidOperationException(
                    String.Format("Not able to query Cosmos container. Check appsettings and Cosmos DB instance.\n{0}", e.GetBaseException() ));
            }
        }

        public List< Task<QueryResponse> > ExecuteQueryBulk(
            string sqlQueryText, int queryBatchSize, bool returnResults, string containerName, string databaseName)
        {
            Container container = cosmosClient.GetContainer(databaseName, containerName);    
            QueryDefinition sqlQuery = new QueryDefinition(sqlQueryText);

            List< Task<QueryResponse> > queryTasks = new List< Task<QueryResponse> >();

            for(int i = 0; i < queryBatchSize; i++) 
            {
                queryTasks.Add(ExecuteQuery(sqlQuery, container, returnResults));
            }
            return queryTasks;
        }

        public List< Task<QueryResponse> > ExecuteQueryMultiClients(
            string sqlQueryText, int queryNumClients, bool returnResults, string containerName, string databaseName)
        {
            QueryDefinition sqlQuery = new QueryDefinition(sqlQueryText);

            List< Task<QueryResponse> > queryTasks = new List< Task<QueryResponse> >();

            CosmosClient[] multiCosmosClients = new CosmosClient[queryNumClients];
            Container[] multiContainers = new Container[queryNumClients];

            // TODO properly with client creation beforehand

            Func<int,Task<QueryResponse>> clientQuery = (int i) => {
                multiCosmosClients[i] = new CosmosClient(endpointUrl, passwordOrAuthorizationKey, new CosmosClientOptions() { AllowBulkExecution = true });
                multiContainers[i] = multiCosmosClients[i].GetContainer(databaseName, containerName);
                return ExecuteQuery(sqlQuery, multiContainers[i], returnResults);
            };

            for(int i = 0; i < queryNumClients; i++)
            {
                queryTasks.Add( clientQuery(i) );
            }
            return queryTasks;
        }

        // Use of Task.Run not feasible

        public static int CalculateAvgQueryTime(QueryResponse[] queryResultsAndTimes)
            => IEdpQueryExecutor.CalculateAvgQueryTime(queryResultsAndTimes);

        public static string GetQueryFirstResult(QueryResponse[] queryResultsAndTimes) 
            => IEdpQueryExecutor.GetQueryFirstResult(queryResultsAndTimes);
    }
}