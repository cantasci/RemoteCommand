using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace RemoteCommand.Models
{
    public class Options
    {
        [Option('r', "remote-machine", Required = true,
            HelpText = "Remote machines(computer name or IP) which are joined with comma.")]
        public string RemoteMachine { get; set; }


        [Option('c', "command", Required = true,
            HelpText = "Command. ex: msg * /server:servername /time:seconds /v /w /? messagetext.")]
        public string CommandText { get; set; }

        [Option('d', "display", DefaultValue = true,
            HelpText = "Show Summary")]
        public bool ShowSummary { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }


        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
                (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
