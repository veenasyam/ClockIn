using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClockInApp.Models
{
    public class ClockInLogModel
    {
        public Int32 UserID { get; set; }

        public DateTime ClockTime { get; set; }

        public String ShiftType { get; set; }

        public String ClockInType { get; set; }

        public String DisplayClockInType { get; set; }
    }
}
