using RemoteCommand.Command;
using RemoteCommand.Models;

namespace RemoteCommand
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {

                ICommand command = new CommandManagement();
                command.SendCommand(options.RemoteMachine, options.CommandText);
            } 
        }
    }
}
