using System;
using Newtonsoft.Json;
using Avro;

namespace EdpSimulator.Entities
{
    public class DdaTransactionRecord : IEdpRecord
    {
        // TODO alternative: implement Abstract Class in order to derive specific fields for Cosmos

        // public DdaTransactionRecord(string transactionid) => (this.transactionid = transactionid);    // constructor
        public string transactionid {get;set;}  // unique key
        public string accountid {get;set;}      // partition key
        public int amount {get;set;} = 100;     // example template value

        /*
            define other fields and methods here
            ...
        */

        public string GetPartitionValue() => this.accountid;
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