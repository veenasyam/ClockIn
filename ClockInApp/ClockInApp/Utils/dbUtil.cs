using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ClockInApp.Utils
{
    public static class dbUtil
    {
        public enum ShiftType {
            shiftstart = 1,
            shiftend = 1,
            breakstart = 1,
            breakend = 1,
            lunchstart = 1,
            lunchend = 1,


        }

        public static SqlConnection GetNewOpenConnection(IConfiguration _configuration)
        {
            string _cnStr = _configuration.GetConnectionString("ClockInAppConnection");
            var cn = new SqlConnection(_cnStr);
            cn.Open();
            return cn;
        }

        

    }
}
