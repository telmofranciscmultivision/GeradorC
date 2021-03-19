using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
// using IBM.Data.DB2.Core;    // TODO fix processor architecture mismatch
using EdpSimulator.QueryTools;
using Avro.Specific;

namespace EdpSimulator.Producers
{
    public class Db2Producer<KRecord, TRecord> : IEdpProducer<KRecord, TRecord> 
        where TRecord : ISpecificRecord
        where KRecord : ISpecificRecord
    {

        public Db2Producer(string endpointUrl, string passwordOrAuthorizationKey, bool upsert)
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