using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ClockInApp.Models;
using System.Data.SqlClient;
using ClockInApp.Utils;
using ClockInApp.Attributes;
using Microsoft.Extensions.Configuration;
using Dapper;
using Microsoft.AspNetCore.Http;


namespace ClockInApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        public  DateTime ShiftConfigStartTime = DateTime.Today;
        private  TimeSpan starttime = new TimeSpan(0, 6, 0, 0);
        private TimeSpan endtime = new TimeSpan(0, 12, 0, 0);
        public  DateTime ShiftConfigEndTime = DateTime.Today;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
            ShiftConfigStartTime = DateTime.Today.Add(starttime);
            ShiftConfigEndTime = ShiftConfigStartTime.Add(endtime);
        }

        private List<ClockInLogModel> GetClockInData() {

            List<ClockInLogModel> c = new List<ClockInLogModel>();
            using (SqlConnection _cn = dbUtil.GetNewOpenConnection(_configuration))
            {

                var parameters = new DynamicParameters();
                c = _cn.Query<ClockInLogModel>(@"SELECT [UserID],[ClockTime],[ShiftType],[ClockInType],[DisplayClockInType] from dbo.[ClockInLog] 
                                    where UserID = @UserID and ClockTime >= @ShiftConfigStartTime and
                                        ClockTime <= @ShiftConfigEndTime", new
                {
                    ShiftConfigStartTime = ShiftConfigStartTime,
                    ShiftConfigEndTime = ShiftConfigEndTime,
                    UserID = HttpContext.Session.GetString("UserID")
                }).ToList();


            }
            return c;
        }

        private List<ClockInLogModel> GetAllClockInDataByUser()
        {

            List<ClockInLogModel> c = new List<ClockInLogModel>();
            using (SqlConnection _cn = dbUtil.GetNewOpenConnection(_configuration))
            {

                var parameters = new DynamicParameters();
                c = _cn.Query<ClockInLogModel>(@"SELECT [UserID],[ClockTime],[ShiftType],[ClockInType],[DisplayClockInType] from dbo.[ClockInLog] 
                                    where UserID = @UserID ", new
                {
                    ShiftConfigStartTime = ShiftConfigStartTime,
                    ShiftConfigEndTime = ShiftConfigEndTime,
                    UserID = HttpContext.Session.GetString("UserID")
                }).ToList();


            }
            return c;
        }


        [CustomAuthorizeAttribute]
        public IActionResult Index()
        {
            List<ClockInLogModel> c = new List<ClockInLogModel>();
            c = GetClockInData();
            ViewData["UserName"] = HttpContext.Session.GetString("UserName");
            ViewData["ShiftConfigStartTime"] = ShiftConfigStartTime;
            ViewData["ShiftConfigEndTime"] = ShiftConfigEndTime;
            return View(c);
        }
        
        [HttpPost]
        [CustomAuthorizeAttribute]
        public IActionResult Index(ClockInLogModel y) {
            using (SqlConnection _cn = dbUtil.GetNewOpenConnection(_configuration))
            {
                List<ClockInLogModel> objAllData = new List<ClockInLogModel>();

                objAllData = GetClockInData();
                if (DateTime.Now >= ShiftConfigStartTime && DateTime.Now <=ShiftConfigEndTime ) {
                    if (objAllData.FindAll(x => x.ShiftType == "Shift").Count < 2) {
                        if (((objAllData.FindAll(x => x.ShiftType == "Break").Count % 2 == 0) && (objAllData.FindAll(x => x.ShiftType == "Lunch").Count % 2 == 0)) ||
                                ((objAllData.FindAll(x => x.ShiftType == "Break").Count % 2 == 1) && (objAllData.FindAll(x => x.ShiftType == "Lunch").Count % 2 == 0)) ||
                                ((objAllData.FindAll(x => x.ShiftType == "Lunch").Count % 2 == 1) && (objAllData.FindAll(x => x.ShiftType == "Break").Count % 2 == 0))
                            )

                        {
                            ClockInLogModel c = new ClockInLogModel
                            {
                                UserID = Convert.ToInt32(HttpContext.Session.GetString("UserID")),
                                ClockTime = System.DateTime.Now,
                                ShiftType = y.ShiftType,
                                ClockInType = y.ClockInType,
                                DisplayClockInType = y.DisplayClockInType
                            };
                            _cn.Execute(@"INSERT INTO[dbo].[ClockInLog]
                                   ([UserID],[ClockTime],[ShiftType],[ClockInType],DisplayClockInType) 
                                    VALUES (@UserID,@ClockTime,@ShiftType,@ClockInType,@DisplayClockInType)", c
                                              );
                        }
                    }
                }
            }
            return RedirectToAction("Index","Home");
        }

        [CustomAuthorizeAttribute]
        public IActionResult Report()
        {
            List<ClockInLogModel> c = new List<ClockInLogModel>();
            c = GetAllClockInDataByUser();            
            return View(c);
        }


    }
}
