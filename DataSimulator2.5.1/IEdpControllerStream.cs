using System.Threading.Tasks;

namespace EdpSimulator
{
    public interface IEdpControllerStream : IEdpController
    {
        // using C# newest features. implement explicitly
        
        protected static bool CheckStopCondition(long duration, long elapsedTime, int totalRecordsToProduce, int currentRecordsProduced)
            => (elapsedTime >= duration || currentRecordsProduced >= totalRecordsToProduce) ? true : false;

        protected static async Task WaitBatchDuration(int batchDuration, int elapsedBatchTime)  
        {
            int waitTime = batchDuration - elapsedBatchTime;
            await Task.Delay(waitTime > 0 ? waitTime : 0);
        }
    }
}