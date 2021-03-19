using System;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Avro;
namespace EdpSimulator.Entities
{
    public class DdaTransactionRawRecord : IEdpRecord
    {
        public AfterImage afterImage {get;set;}
        public String beforeImage {get;set;}  // = "";
        public String A_ENTTYP {get;set;} = "PT";
        public String A_CCID {get;set;} = "396692294088851456";
        public String A_TIMSTAMP {get;set;} = "2020-03-05 12:17:07.201736000000";

        public static string Md5Hash(string input)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] hashData = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            return BitConverter.ToString(hashData);
        }

        public string GetTransactionId() =>
            afterImage.HDDA_KEY_COMP + afterImage.HDDA_KEY_CONTA + Md5Hash(afterImage.HDDA_KEY_DATA_LANCAMENTO) 
            + afterImage.HDDA_KEY_ORIGEM + Md5Hash(afterImage.HDDA_KEY_DATA_HORA);
        public string GetPartitionValue() => this.GetTransactionId();
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