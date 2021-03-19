using System.Threading.Tasks;
using System.Collections.Generic;
using Avro.Specific;

namespace EdpSimulator
{
    public interface IEdpGenerator<KRecord,TRecord> 
        where KRecord : ISpecificRecord
        where TRecord : ISpecificRecord
        // where TRecordSpecific : IEdpRecord
    {
        public TRecord[] BuildRecords(int batchSize);
        public TRecord[] GenerateRecords(TRecord[] documents);
        public Task<List<TRecord>> GenerateRecordsAsync(int batchSize, bool backgroundThread=false);

        // GenerateRecordsTask should implement Task.Run() to execute immediatly on a separate thread (CPU intensive method)
    }
}