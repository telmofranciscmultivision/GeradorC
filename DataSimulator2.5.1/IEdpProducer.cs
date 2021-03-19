using System.Threading.Tasks;
using System.Collections.Generic;
using Avro.Specific;
namespace EdpSimulator
{
    public interface IEdpProducer<KRecord, TRecord> 
        where KRecord : ISpecificRecord
        where TRecord : ISpecificRecord
    {
        public List<Task> Produce(List<TRecord> records, string containerOrTopicName, string databaseName=null, List<Task> producerTasks=null, List<KRecord> keySchema=null);
        public Task<int> CountRecords(string containerOrTopicName, string databaseName=null);
    }
}