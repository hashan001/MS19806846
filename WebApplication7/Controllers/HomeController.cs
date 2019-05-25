using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication6.Logic;
using WebApplication6.Services;
using WebApplication7.Models;

namespace WebApplication7.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SyncToGoogleCalendar(CalenderEvent calEvent)
        {
            TempData["calEvent"] = calEvent;
            
            if (string.IsNullOrWhiteSpace(GoogleOauthTokenService.OauthToken))
            {
                var redirectUri = GoogleCalendarSyncer.GetOauthTokenUri(this);
                return Redirect(redirectUri);
            }
            else
            {
                var success = GoogleCalendarSyncer.SyncToGoogleCalendar(this, calEvent);
                if (!success)
                {
                    return Json("Token was revoked. Try again.", JsonRequestBehavior.AllowGet);
                }
            }
            return Redirect("~/");
        }
    }
}