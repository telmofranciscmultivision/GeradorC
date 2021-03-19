using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using MySqlConnector;
using EdpSimulator.QueryTools;
using Avro.Specific;

// Alternatively use MySql.Data package connector from Oracle

namespace EdpSimulator.Producers
{
    public class MySqlMemSqlProducer<KRecord,TRecord> : IEdpProducer<KRecord,TRecord> 
        where KRecord : ISpecificRecord
        where TRecord : ISpecificRecord
    {

        public MySqlMemSqlProducer(string endpointUrl, string passwordOrAuthorizationKey, bool upsert)
        {
           
            throw new NotImplementedException();
        }

        public List<Task> Produce(List<TRecord> records, string containerOrTopicName, string databaseName, List<Task> producerTasks=null, List<KRecord> keySchema = null)
        {

            throw new NotImplementedException();
        }

        public async Task<int> CountRecords(string containerOrTopicName, string databaseName)
        {

            await Task.Delay(1);
            throw new NotImplementedException();
        }
    }
}