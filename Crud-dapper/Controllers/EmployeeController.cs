using Crud_dapper.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using PagedList;
using PagedList.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Crud_dapper.Controllers

{
    public class EmployeeController : Controller
    {
        string Skills=null;
        // string sort = "ASC";

        // GET: Employee
        public ActionResult Index(string sortOrder,string currentFilter, string searchString, int? page, int? pageSize)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            int defaSize = (pageSize ??5);

            ViewBag.psize = defaSize;

            //Dropdownlist code for PageSize selection  
            //In View Attach this  
            ViewBag.PageSize = new List<SelectListItem>()
            {
                new SelectListItem() { Value="5", Text= "5" },
                new SelectListItem() { Value="10", Text= "10" },
                new SelectListItem() { Value="15", Text= "15" },
                new SelectListItem() { Value="25", Text= "25" },
                new SelectListItem() { Value="50", Text= "50" },
                new SelectListItem() { Value="100", Text= "100" },
            };
            int pageNo = page ?? 1;
            ViewBag.pageNo = pageNo;
            ViewBag.SerialNumber = defaSize * (pageNo - 1);
            ViewBag.CurrentFilter = searchString;
            //var a;
            EmployeeModel emp = new EmployeeModel();
            var a = DapperORM.ReturnList<EmployeeModel>("EmployeeViewAll", null);
            if (!String.IsNullOrEmpty(searchString))
            {
                //ViewData["CurrentFilter"] = searchString;
                DynamicParameters param1 = new DynamicParameters();
                param1.Add("@Search", searchString);
               // a= a.Where(x=>x.Name.Contains(searchString));
                a = DapperORM.ReturnList<EmployeeModel>("SearchItem", param1);
            }
               
                ViewBag.SortingName = String.IsNullOrEmpty(sortOrder) ? "Name_Description" : "";
                ViewBag.SortingDepartment = sortOrder == "dept_desc" ? "dept_asc" : "dept_desc";
                ViewBag.SortingEmail = sortOrder == "email_desc" ? "email_asc" : "email_desc";
                ViewBag.CurrentSort = sortOrder;
            //DynamicParameters param = new DynamicParameters();
            //param.Add("@Name", sortOrder);
            // var a = DapperORM.ReturnList<EmployeeModel>("EmployeeViewAll", null);
            switch (sortOrder)
                {
                    case "Name_Description":
                        a = a.OrderByDescending(stu => stu.Name);
                        break;
                    case "dept_asc":
                        a = a.OrderBy(stu => stu.Department);
                        break;
                    case "dept_desc":
                        a = a.OrderByDescending(stu => stu.Department);
                        break;
                    case "email_asc":
                        a = a.OrderBy(stu => stu.Email);
                        break;
                    case "email_desc":
                        a = a.OrderByDescending(stu => stu.Email);
                        break;
                    default:
                        a = a.OrderBy(stu => stu.Name);
                        break;
                }
                return View(a.ToList().ToPagedList(page ?? 1, defaSize));
        }
           
        public ActionResult DeletedEmployee()
        {
            return View(DapperORM.ReturnList<EmployeeModel>("DeletedEmployee", null) );
        }


        [HttpGet]
        public ActionResult Add(int id=0)
        {
            return View();

        }
        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            if (id == 0)
            {
                return View();
            }
            else
            {

                DynamicParameters param = new DynamicParameters();
                param.Add("@EmployeeID", id);
                return View(DapperORM.ReturnList<EmployeeModel>("EmployeeViewByID", param).FirstOrDefault<EmployeeModel>());
            }
        }

        [HttpPost]
        public ActionResult Edit(EmployeeModel emp)
        {
            DynamicParameters pram = new DynamicParameters();
            pram.Add("@EmployeeID", emp.EmployeeID);
            pram.Add("@Name", emp.Name);
            pram.Add("@Department", emp.Department);
            pram.Add("@Gender", emp.Gender);
            pram.Add("@Email", emp.Email);
            pram.Add("@Status", 1);
            if (emp.IsCSharp == true)
            {
                Skills = "C#";
            }
            if (emp.IsVBA == true)
            {
                if (Skills == null)
                {
                    Skills = "VBA";
                }
                else
                {
                    Skills = Skills + ", " + "VBA";
                }

            }
            if (emp.IsXamarin == true)
            {
                if (Skills == null)
                {
                    Skills = "Xamarin";
                }
                else
                {
                    Skills = Skills + ", " + "Xamarin";
                }
            }
            emp.Skills = Skills;
            pram.Add("@Skills", emp.Skills);
            DapperORM.Execute("EmployeeAddOrEdit", pram);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult Add(EmployeeModel emp)
        {
            var a = DapperORM.ReturnList<EmployeeModel>("EmployeeViewAll", null);

            var isMailCheck = (from u in a
                               where u.Email.ToLower() == emp.Email.ToLower()
                               select new { }).FirstOrDefault();
          if (isMailCheck != null)
            {
                TempData["Data"] = "Email Address already exist";
                return View();
            }
            else
            {
                DynamicParameters pram = new DynamicParameters();
                pram.Add("@EmployeeID", emp.EmployeeID);
                pram.Add("@Name", emp.Name);
                pram.Add("@Department", emp.Department);
                pram.Add("@Gender", emp.Gender);
                pram.Add("@Email", emp.Email);
                pram.Add("@Status", 1);
                if (emp.IsCSharp == true)
                {
                    Skills = "C#";
                }
                if (emp.IsVBA == true)
                {
                    if (Skills == null)
                    {
                        Skills = "VBA";
                    }
                    else
                    {
                        Skills = Skills + ", " + "VBA";
                    }

                }
                if (emp.IsXamarin == true)
                {
                    if (Skills == null)
                    {
                        Skills = "Xamarin";
                    }
                    else
                    {
                        Skills = Skills + ", " + "Xamarin";
                    }
                }
                emp.Skills = Skills;
                pram.Add("@Skills", emp.Skills);
                DapperORM.Execute("EmployeeAddOrEdit", pram);

                return RedirectToAction("Index");
            }
        }
        public ActionResult ConfirmDelete(int? id)
        {
            if (id == 0)
            {
                return View(DapperORM.ReturnList<EmployeeModel>("EmployeeViewById", null).FirstOrDefault<EmployeeModel>());

            }
            else
            {
                var a = DapperORM.ReturnList<EmployeeModel>("EmployeeViewAll",null);

                return View(a.Where(x => x.EmployeeID == id).FirstOrDefault());
            }
        }
        public ActionResult Delete(int? id)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmployeeID", id);
            DapperORM.Execute("EmployeeDeleteByID",param);
            //return View();
            return RedirectToAction("Index");
        }

        public ActionResult Restore(int id)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmployeeID", id);
            DapperORM.Execute("RestoreByID", param);
            return RedirectToAction("Index");
        }

        //public ActionResult Search(string search= "Utt")
        //{
        //    DynamicParameters param = new DynamicParameters();
        //    param.Add("@Search", search);
        //   // return View(DapperORM.ReturnList<EmployeeModel>("SearchItem",param));
        //    return RedirectToAction("Index");

        //}

        public ActionResult ConfirmAdd()
        {
            return View();
        }

    }
}