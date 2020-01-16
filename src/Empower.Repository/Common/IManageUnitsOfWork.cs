using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Empower.Repository {
    //
    // Summary:
    //     Interface used by NServiceBus to manage units of work as a part of the message
    //     processing pipeline.
    public interface IManageUnitsOfWork
    {
        //
        // Summary:
        //     Called before all message handlers and modules
        void Begin();
        //
        // Summary:
        //     Called after all message handlers and modules, if an error has occurred the exception
        //     will be passed
        void End(Exception ex = null);
    }
}
