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

namespace Crud_dapper.Controllers
{
    public class EmployeeController : Controller
    {
        string Skills=null;
        // string sort = "ASC";

        // GET: Employee
        public ActionResult Index(string sortOrder, string searchString)
        {
            EmployeeModel emp = new EmployeeModel();
            if (searchString == null)
            {
                ViewBag.SortingName = String.IsNullOrEmpty(sortOrder) ? "Name_Description" : "";
                ViewBag.SortingDepartment = sortOrder == "dept_desc" ? "dept_asc" : "dept_desc";
                ViewBag.SortingEmail = sortOrder == "email_desc" ? "email_asc" : "email_desc";
                //DynamicParameters param = new DynamicParameters();
                //param.Add("@Name", sortOrder);
                var a = DapperORM.ReturnList<EmployeeModel>("EmployeeViewAll", null);
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
                return View(a.ToList());
            }
            else
            {
                //EmployeeModel emp = new EmployeeModel();
                //emp.Sort = sort;
                emp.Sorted = true;
                if (sortOrder == null)
                {
                    ViewData["CurrentFilter"] = searchString;
                    if (searchString == null)
                    {
                        return View(DapperORM.ReturnList<EmployeeModel>("EmployeeViewAll", null));
                    }
                    else
                    {
                        DynamicParameters param1 = new DynamicParameters();
                        param1.Add("@Search", searchString);
                        return View(DapperORM.ReturnList<EmployeeModel>("SearchItem", param1));
                        //return RedirectToAction("Index");
                    }
                }
                return RedirectToAction("Index");
                ////ViewData["NameSortParm"] = sortOrder;
                ////ViewData["Department"] = sortOrder;
                //else
                //{
                //else
                //{

                //DynamicParameters param = new DynamicParameters();
                //param.Add("@Name", sortOrder);
                //var a = DapperORM.ReturnList<EmployeeModel>("SortAsc", param);
                //if (sortOrder == "Name")
                //{
                //    var b = a.OrderBy(x => x.Name);
                //}
                //else if ()
                //{

                //};
                //{

                //}

                //if (emp.Sorted == true)
                //{
                //    emp.Sorted = false;
                //    DynamicParameters param2 = new DynamicParameters();
                //    param2.Add("@Name", sortOrder);
                //    //var a = DapperORM.ReturnList<EmployeeModel>("SortAsc", param);
                //    // var b = a.OrderBy(x => x.Name);

                //    return View(DapperORM.ReturnList<EmployeeModel>("SortAsc", param2));
                //}
                //else if (emp.Sorted == false)
                //{
                //    emp.Sorted = true;
                //    DynamicParameters param3 = new DynamicParameters();
                //    param3.Add("@Name", sortOrder);
                //    return View(DapperORM.ReturnList<EmployeeModel>("SortDesc", param3));
                //}
                //else
                //{
                //    return View(DapperORM.ReturnList<EmployeeModel>("EmployeeViewAll", null));
                //}

                // }
            }

        }  

        //  ..../Employee/AddOrEdit/id

        public ActionResult DeletedEmployee()
        {
            return View(DapperORM.ReturnList<EmployeeModel>("DeletedEmployee", null) );
        }


        [HttpGet]
        public ActionResult AddOrEdit(int id=0)
        {
            if (id == 0)
            {
                return View();
            }
            else
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@EmployeeID" , id);
                return View(DapperORM.ReturnList<EmployeeModel>("EmployeeViewByID", param).FirstOrDefault<EmployeeModel>());
            }

            
        }

        [HttpPost]
        public ActionResult AddOrEdit(EmployeeModel emp)
        {
            DynamicParameters pram = new DynamicParameters();
            pram.Add("@EmployeeID", emp.EmployeeID);
            pram.Add("@Name", emp.Name);
            pram.Add("@Department", emp.Department);
            pram.Add("@Gender", emp.Gender);
            pram.Add("@Email", emp.Email);
            pram.Add("@Status",1);
            if(emp.IsCSharp==true)
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
                    Skills = Skills + " ," + "VBA";
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
                    Skills = Skills + " ," + "Xamarin";
                }
            }


            emp.Skills = Skills;
            pram.Add("@Skills", emp.Skills);
            DapperORM.Execute("EmployeeAddOrEdit", pram);

            return RedirectToAction("Index");
        
        }
        public ActionResult Delete(int id)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmployeeID", id);
            DapperORM.Execute("EmployeeDeleteByID",param);
            return RedirectToAction("Index");
        }

        public ActionResult Restore(int id)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmployeeID", id);
            DapperORM.Execute("RestoreByID", param);
            return RedirectToAction("Index");
        }

        public ActionResult Search(string search= "Utt")
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@Search", search);
           // return View(DapperORM.ReturnList<EmployeeModel>("SearchItem",param));
            return RedirectToAction("Index");

        }



    }
}