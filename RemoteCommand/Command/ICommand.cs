using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteCommand.Command
{
    interface ICommand
    {
        /// <summary>
        /// Can reach remote machine
        /// </summary>
        /// <param name="remoteMachine"></param>
        /// <returns></returns>
        bool CanReach(string remoteMachine);

        /// <summary>
        /// Send command to remote machine(s) which is/are reached
        /// </summary>
        /// <param name="remoteMachines"></param>
        /// <param name="commandText"></param> 
        void SendCommand(string remoteMachines, string commandText);
    }
}
