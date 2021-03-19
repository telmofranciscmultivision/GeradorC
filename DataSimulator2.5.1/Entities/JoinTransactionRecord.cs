using System;
using Newtonsoft.Json;
using Avro;

namespace EdpSimulator.Entities
{
    public class JoinTransactionRecord : IEdpRecord
    {
        public string transactionid {get;set;}
        public string accountid {get;set;}
        public int amount {get;set;} = 100;
        public int updateDelete {get;set;} = 0;


        // public string GetPartitionValue() => this.accountid;
        public string GetPartitionValue() => this.transactionid;


        public string GetCosmosId() => Guid.NewGuid().ToString();

        public override string ToString() => JsonConvert.SerializeObject(this);

        public virtual object Get(int fieldPos)
        {
            return "empty";
        }
        public virtual void Put(int fieldPos, object fieldValue) { }

        public virtual Schema Schema
        {
            get
            {
                return Schema.Parse("EmptySchema");
            }
        }
    }
}