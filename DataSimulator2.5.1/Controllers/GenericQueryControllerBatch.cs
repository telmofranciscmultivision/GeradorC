using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using EdpSimulator.QueryTools;

namespace EdpSimulator.Controllers
{
    public class GenericQueryControllerBatch<TExecutor> : IEdpControllerBatch
        where TExecutor : IEdpQueryExecutor
    {
        /* 
            Using https://nbomber.com/ might be a good idea.
            ...
        */

        private IConfigurationRoot config;
        private TExecutor queryExecutor;
        private string containerOrTopicName;
        private string databaseName;
        private int queryBatchSize;
        private string sqlQueryText;

        public GenericQueryControllerBatch(IConfigurationRoot config, TExecutor queryExecutor)
        {
            try
            {
                this.config = config;
                this.queryExecutor = queryExecutor;

                if (config is null || queryExecutor is null)
                    throw new NullReferenceException("Config or queryExecutor are null. Check appsettings or Program class.");

                containerOrTopicName = config["ContainerOrTopicName"];
                databaseName = config["DatabaseName"];
                queryBatchSize = int.Parse(config["QueryBatchSize"]);
                sqlQueryText = config["SqlQueryText"];

                if ( String.IsNullOrEmpty(containerOrTopicName) || String.IsNullOrEmpty(databaseName) || String.IsNullOrEmpty(sqlQueryText) )
                    throw new ArgumentException("Database, container or SQL query were not filled properly in appsettings.");                    
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                throw new ArgumentException(
                    String.Format("Appsettings was not filled properly.\n{0}", e.GetBaseException() ));
            }
        }

        public async virtual Task Run() {   // TODO refactor to eliminate duplicated code

            Console.WriteLine("Produced a batch of {0} queries: {1}.", queryBatchSize, sqlQueryText);

            // first time query is executed for Cosmos DB to compile. This way query times are not negatively affected
            List< Task<QueryResponse> > firstQueryForCompilationTask = queryExecutor.ExecuteQueryBulk(
                sqlQueryText, queryBatchSize: 1, returnResults: false, containerOrTopicName, databaseName);
            await Task.WhenAll(firstQueryForCompilationTask);


            // Measure batch query time using one client
            Stopwatch oneClientQueryStopwatch = new Stopwatch();
            oneClientQueryStopwatch.Start();               

            List< Task<QueryResponse> > oneClientQueryTasks = queryExecutor.ExecuteQueryBulk(
                sqlQueryText, queryBatchSize, returnResults: false, containerOrTopicName, databaseName);
            
            QueryResponse[] oneClientQueryResults = await Task.WhenAll(oneClientQueryTasks);    // await for result
            oneClientQueryStopwatch.Stop();

            // Calculate avg query time using one client
            int oneClientAvgQueryTime = IEdpQueryExecutor.CalculateAvgQueryTime(oneClientQueryResults);

            Console.WriteLine("One Client - Avg query time was {0} ms. \nActual batch query time was {1} ms", 
                oneClientAvgQueryTime, oneClientQueryStopwatch.ElapsedMilliseconds);


             // Measure batch query time using multiple clients
            Stopwatch multiClientQueryStopwatch = new Stopwatch();
            multiClientQueryStopwatch.Start();               
 
            List< Task<QueryResponse> > multiClientQueryTasks = queryExecutor.ExecuteQueryMultiClients(
                sqlQueryText, queryBatchSize, returnResults: false, containerOrTopicName, databaseName);
            
            QueryResponse[] multiClientQueryResults = await Task.WhenAll(multiClientQueryTasks);    // await for result
            multiClientQueryStopwatch.Stop();

            // Calculate avg query time using multiple clients
            int multiClientAvgQueryTime = IEdpQueryExecutor.CalculateAvgQueryTime(multiClientQueryResults);

            Console.WriteLine("Multiple Clients - Avg query time was {0} ms. (not accurate as query compilation time is included)",
                multiClientAvgQueryTime);
            Console.WriteLine("Actual batch query time was {0} ms (not accurate as query compilation time is included)",
                multiClientQueryStopwatch.ElapsedMilliseconds);


        }
    }
}