using DailyCalls.Models;
using LinqToExcel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DailyCalls.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<DailyCall> CallList;

            Phone_SystemEntities10 db = new Phone_SystemEntities10();

            CallList = db.DailyCalls.ToArray().
                       Where(x => x.OutcomeId == null 
                      || x.OutcomeId == 2
                      || x.OutcomeId == 8
                      || x.OutcomeId == 9
                      || x.OutcomeId == 14 )
                       .ToList();

            //Outcomes = new SelectList(db.CallOutcomeLists.ToList(), "OutcomeID", "Name")

            return View(CallList);
        }

        public ActionResult viewCalls()
        {

            return View();
        }






        public ActionResult Upload()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Upload(DailyCall objEmpDetail, HttpPostedFileBase FileUpload)
        {

            Phone_SystemEntities10 objEntity = new Phone_SystemEntities10();
            string data = "";
            if (FileUpload != null)
            {
                if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    string filename = FileUpload.FileName;

                    if (filename.EndsWith(".xlsx"))
                    {
                        string targetpath = Server.MapPath("~/");
                        FileUpload.SaveAs(targetpath + filename);
                        string pathToExcelFile = targetpath + filename;

                        string sheetName = "Sheet1";

                        var excelFile = new ExcelQueryFactory(pathToExcelFile);
                        var empDetails = from a in excelFile.Worksheet<DailyCall>(sheetName) select a;
                        foreach (var a in empDetails)
                        {
                            if (a.Name != null)
                            {
                                
                                int result = PostExcelData(a.Name, a.ClientReference, a.TelephoneNumber,a.Date,a.OutcomeId,a.LastCalled);
                                if (result <= 0)
                                {
                                    data = "Hello User, Found some duplicate values!";
                                    ViewBag.Message = data;
                                    continue;

                                }
                                else
                                {
                                    data = "Successfully uploaded spreadsheet";
                                    ViewBag.Message = data;
                                }
                            }

                            else
                            {
                                data = a.Name + "Some fields are null, Please check your excel sheet";
                                ViewBag.Message = data;
                                return View("Upload");
                            }

                        }
                    }

                    else
                    {
                        data = "This file is not in a valid format";
                        ViewBag.Message = data;
                    }
                    return View("Upload");
                }
                else
                {

                    data = "Only Excel file format is allowed";

                    ViewBag.Message = data;
                    return View("ExcelUpload");

                }

            }
            else
            {

                if (FileUpload == null)
                {
                    data = "Please choose Excel file";
                }

                ViewBag.Message = data;
                return View("Upload");
            }
        }

   
       

        public int PostExcelData(string name, string clientReference, string telephoneNumber, DateTime? date, int? OutcomeId, DateTime? LastCalled  )
        {
            Phone_SystemEntities10 DbEntity = new Phone_SystemEntities10();
            var InsertExcelData = DbEntity.pr_InsertCallDetails(name, clientReference, telephoneNumber, date, OutcomeId, LastCalled);

            return InsertExcelData;
        }

       

    }





}