using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COC.Resource.Data
{
    public class FlowLogData
    {
        public Int64? FlowLogId { get; set; }
        public string TicketId { get; set; }
        public string TeamFrom { get; set; }
        public string TeamTo { get; set; }
        public string FlowType { get; set; }
        public DateTime? ActionDate { get; set; }
        public string TeamFromEmpCode { get; set; }
    }
}
