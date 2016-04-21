using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COC.Resource.Data
{
    public class MonitoringWSData
    {
        public DateTime? OperationDate { get; set; }
        public string WSName { get; set; }
        public string Channel { get; set; }
        public string TicketId { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseDesc { get; set; }
        public DateTime? ResponseDate { get; set; }
        public string ResponseTime { get; set; }
        public string CauseError { get; set; }
    }
}
