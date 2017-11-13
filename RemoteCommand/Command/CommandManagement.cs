using System;
using System.Management;
using RemoteCommand.Models;
using RemoteCommand.Print;

namespace RemoteCommand.Command
{
    /// <summary>
    /// Send command to remote machine(s).
    /// </summary>
    public class CommandManagement : ICommand, IDisplay
    {
        /// <summary>
        /// Can reach remote machine
        /// </summary>
        /// <param name="remoteMachine"></param>
        /// <returns></returns>
        public bool CanReach(string remoteMachine)
        {
            System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
            System.Net.NetworkInformation.PingReply reply = ping.Send(remoteMachine);

            return reply != null && reply.Status.ToString() == "Success";
        }

        /// <summary>
        /// Send command to remote machine(s) which is/are reached
        /// </summary>
        /// <param name="remoteMachines"></param>
        /// <param name="commandText"></param> 
        public void SendCommand(string remoteMachines, string commandText)
        {
            ClearSummary();
            var remoteMachineList = remoteMachines.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var remoteMachine in remoteMachineList)
            {
                var summaryItem = new SummaryItem(){ Machine = remoteMachine};
                if (CanReach(remoteMachine))
                {
                    summaryItem.IsReached = true;
                    InternalSendCommand(remoteMachine, commandText, summaryItem);
                }
                else
                {
                    summaryItem.IsReached = false;
                }

                SummaryResult.SummaryItems.Add(summaryItem);
            }
        }


        /// <summary>
        /// Send command to per remote machine
        /// </summary>
        /// <param name="remoteMachine"></param> 
        /// <param name="commandText"></param> 
        /// <param name="summaryItem"></param> 
        private void InternalSendCommand(string remoteMachine, string commandText, SummaryItem summaryItem)
        {
            try
            {
                ConnectionOptions connOptions = new ConnectionOptions();
                connOptions.Impersonation = ImpersonationLevel.Impersonate;
                connOptions.EnablePrivileges = true;

                ManagementScope manScope = new ManagementScope($"\\\\{remoteMachine}\\ROOT\\CIMV2", connOptions);
                manScope.Connect();

                ObjectGetOptions objectGetOptions = new ObjectGetOptions();
                ManagementPath managementPath = new ManagementPath("Win32_Process");
                ManagementClass processClass = new ManagementClass(manScope, managementPath, objectGetOptions);

                ManagementBaseObject inParams = processClass.GetMethodParameters("Create");

                inParams["CommandLine"] = $"cmd.exe \\c {commandText}";
                ManagementBaseObject outParams = processClass.InvokeMethod("Create", inParams, null);
                summaryItem.IsSuccessCommand = true;

            }
            catch (Exception ex)
            {
                summaryItem.IsSuccessCommand = false;
                summaryItem.ErrorMessage = $"{ex.Message}\n{ex.StackTrace}";
            }
        }

        private void ClearSummary()
        {
            SummaryResult = new Summary();
        }

        public Summary SummaryResult { get; set; }
        public void Display()
        {
            Console.WriteLine("--------------------------------------------------------------------------");
            foreach (var summaryResultSummaryItem in SummaryResult.SummaryItems)
            {
                Console.WriteLine($"Remote Machine: {summaryResultSummaryItem.Machine}");
                Console.WriteLine($"Reached: {summaryResultSummaryItem.IsReached}");
                Console.WriteLine($"Success Command: {summaryResultSummaryItem.IsSuccessCommand}");
                Console.WriteLine($"ErrorMessage: {(summaryResultSummaryItem.IsSuccessCommand ? "-" : summaryResultSummaryItem.ErrorMessage)}");
                Console.WriteLine("*************************************************************************");
            }
            Console.WriteLine("--------------------------------------------------------------------------");
        }
    }
}
