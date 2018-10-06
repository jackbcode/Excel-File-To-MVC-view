using DailyCalls.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DailyCalls.Controllers
{
    public class CallScreenController : Controller
    {
        // GET: CallScreen
        public ActionResult Index(string reference)
        {

            Phone_SystemEntities10 db = new Phone_SystemEntities10();
            DailyCall model = new DailyCall();

            model = db.DailyCalls.Where(x => x.ClientReference == reference).FirstOrDefault();
            model.Outcomes = new SelectList(db.CallOutcomeLists.ToList(), "OutcomeID", "Name");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(DailyCall model)
        {

            Phone_SystemEntities10 db = new Phone_SystemEntities10();

            int id = model.ID;

            //make sure reference is unique
            //if(db.CallOutcomes.Any(x=>x.Reference == model.Reference))
            //{

            //}


            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();



            return Redirect("~/Home/Index");
        }


    }
}