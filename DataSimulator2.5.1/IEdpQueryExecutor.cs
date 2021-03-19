using System.Threading.Tasks;
using System.Collections.Generic;
using EdpSimulator.QueryTools;

namespace EdpSimulator
{
    public interface IEdpQueryExecutor
    {
        public List< Task<QueryResponse> > ExecuteQueryBulk(
            string sqlQueryText, int queryBatchSize, bool returnResults, string containerName, string databaseName);

        public List< Task<QueryResponse> > ExecuteQueryMultiClients(
            string sqlQueryText, int queryNumClients, bool returnResults, string containerName, string databaseName);

        public static int CalculateAvgQueryTime(QueryResponse[] queryResultsAndTimes)   // default implementation. implement explicitly
        {
            int numTimes = queryResultsAndTimes.Length;
            int sumTimes = 0;
            for(int i = 0; i < numTimes; i++)
                sumTimes += queryResultsAndTimes[i].QueryTime;
            return sumTimes / numTimes;
        }    

        public static string GetQueryFirstResult(QueryResponse[] queryResultsAndTimes)   // default implementation. implement explicitly 
            => queryResultsAndTimes[0].FirstResult;
    }
}