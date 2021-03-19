namespace EdpSimulator
{
    using Avro.Specific;
    public interface IEdpRecord : ISpecificRecord
    {
        public string GetPartitionValue();        // Returns partition value as string
        public string GetCosmosId();
        
         //public string GetPartitionValueJSON();    // Returns partition value as JSON string

        // public string ToString();
    }
}