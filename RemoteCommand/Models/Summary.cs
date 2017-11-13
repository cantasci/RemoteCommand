using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteCommand.Models
{
    public class Summary
    {
        public Summary() {  SummaryItems = new List<SummaryItem>();}
        public List<SummaryItem> SummaryItems { get; set; }
    }

    public class SummaryItem
    {
        public string Machine { get; set; }

        public bool IsReached { get; set; }

        public bool IsSuccessCommand { get; set; }
        public string ErrorMessage { get; set; }
    }
}
