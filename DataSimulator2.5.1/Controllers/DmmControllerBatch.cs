using System;
using System.Threading.Tasks;

namespace EdpSimulator.Controllers
{
    public class DmmControllerBatch : IEdpControllerBatch
    {
        /* 
            This controller can be used to orchestrate the production of DMM 
            related data in a batch manner (e.g. DmmContext, Cycle, Budget)

            For DMM data produced in a streaming fashion, implement
            IEdpControllerStream interface instead.

            ...
        */

        public async Task Run() { 

            // TODO



            await Task.Delay(1);
            throw new NotImplementedException("DmmControllerBatch not implemented.");

        }
    }
}