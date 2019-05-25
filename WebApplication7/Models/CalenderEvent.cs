using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication7.Models
{
    public class CalenderEvent
    {
        public string eventDescription { get; set; }
        public string eventSummary { get; set; }
        public DateTime eventStart { get; set; }
        public DateTime eventEnd { get; set; }
    }
}