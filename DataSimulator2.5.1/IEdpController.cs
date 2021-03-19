using System.Threading.Tasks;

namespace EdpSimulator
{
    public interface IEdpController
    {
        /*
            A constructor with 3 generic types should be implemented,
            otherwise a specific controller needs to be added to
            switch statement in Program class. Example:

                public class MyController<TRecord,TGenerator,TProducer> : IEdpControllerStream
                    where TRecord : IEdpRecord 
                    where TGenerator : IEdpGenerator<TRecord>
                    where TProducer : IEdpProducer<TRecord>
        */

        public Task Run();
    }
}