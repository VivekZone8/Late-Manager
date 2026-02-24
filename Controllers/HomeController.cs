using ClassLateManage.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.ComponentModel;

namespace ClassLateManage.Controllers
{
    public class HomeController : Controller
    {



        //LOGIN

        public ActionResult Index()
        {
            DBLayer db = new DBLayer();
            DataTable dt = db.LoginUser("sp_UserList");
            return View(dt);
        }
        [HttpPost]
        public ActionResult Index(string UserId, string Password)
        {

            if (UserId != null && Password != null)
            {
                DBLayer db = new DBLayer();
                DataTable dt = db.LoginUser("sp_LoginUser", new SqlParameter("@UserId", UserId),
                                                            new SqlParameter("@Password", Password));
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Role"]?.ToString() == "NormalUser")
                    {
                        // string user= dt.Rows[0]["UserId"].ToString();
                        Session["UserId"] = dt.Rows[0]["UserId"].ToString();
                        return RedirectToAction("Dashboard", "Home");
                    }
                    else if (dt.Rows[0]["Role"]?.ToString() == "Prinipal")
                    {
                        Session["UserId"] = dt.Rows[0]["UserId"].ToString();
                        return RedirectToAction("PrintLateReport", "home");
                    }
                    else
                    {
                        //Server Error
                        ViewBag.ErrorMsg = "0";
                        return RedirectToAction("Index", "Home");

                    }
                }
                else
                {
                    //Wrong Password
                    ViewBag.ErrorMsg = "1";
                    return RedirectToAction("Index", "Home");
                }

            }
            else
            {
                //User Name and password required
                ViewBag.ErrorMsg = "-1";
                return View();
            }
        }

        public ActionResult DashBoard()
        {
            if (Session["UserId"] != null)
            {
                DBLayer db = new DBLayer();
                DataTable dt = db.ExecuteSelect("sp_TeacherList");
                return View(dt);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //Show Late Data
        public ActionResult ShowData()
        {
            if (Session["UserId"] != null)
            {
                DBLayer db = new DBLayer();
                DataTable dt = db.ExecuteSelect("sp_ShowLateData");
                return View(dt);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult SaveLateData(LateTeacher model)
        {
            if (Session["UserId"] != null)
            {

                if (model != null)
                {
                    DBLayer db = new DBLayer();
                    //string name = model.TeacherName;
                    string[] Temp = model.TeacherName.Split(',');
                    string TeacherName = Temp[0];
                    int TeacherId = Convert.ToInt32(Temp[1]);
                    int result = db.ExecuteInsert("sp_InsertLateData", new SqlParameter("@TeacherId", TeacherId),
                        new SqlParameter("@TeacherName", TeacherName),
                        new SqlParameter("@Period", model.Period),
                        new SqlParameter("@Class", model.Class),
                        new SqlParameter("@LateTime", Convert.ToInt32(model.Time))
                        );
                    if (result > 0)
                    {
                        return Json("Data Add");
                    }
                    else if (result==0)
                    {
                        return Json("Data Already Added");
                    }
                    else
                    {

                        return Json("Data Not Add");
                    }
                }
                return Json("Error");
            }

            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        public ActionResult PrintLateReport()
        {
            if (Session["UserId"].ToString() == "1003")
            { 

                DBLayer db = new DBLayer();
                DataTable dt = db.ExecuteSelect("sp_ShowLateData");
                return View(dt);
               
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}