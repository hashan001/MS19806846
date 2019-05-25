using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using WebApplication6.Logic.Google_Implementations;
using WebApplication6.Services;
using WebApplication7.Models;

namespace WebApplication6.Logic
{
    public class GoogleCalendarSyncer
    {
        public static string GetOauthTokenUri(Controller controller)
        {
            var authResult = GetAuthResult(controller);
            return authResult.RedirectUri;
        }

        public static bool SyncToGoogleCalendar(Controller controller, CalenderEvent calEvent)
        {
            try
            {
                var authResult = GetAuthResult(controller);

                var service = InitializeService(authResult);

                var calendarId = GetMainCalendarId(service);

                var calendarEvent = GetCalendarEvent(calEvent);

                SyncCalendarEventToCalendar(service, calendarEvent, calendarId);
                return true;
            }
            catch (Exception ex)
            {
                GoogleOauthTokenService.OauthToken = null;
                return false;
            }
        }

        private static Google.Apis.Auth.OAuth2.Web.AuthorizationCodeWebApp.AuthResult GetAuthResult(Controller controller)
        {
            var dataStore = new DataStore();
            var clientID = WebConfigurationManager.AppSettings["GoogleClientID"];
            var clientSecret = WebConfigurationManager.AppSettings["GoogleClientSecret"];
            var appFlowMetaData = new GoogleAppFlowMetaData(dataStore, clientID, clientSecret);
            var factory = new AuthorizationCodeMvcAppFactory(appFlowMetaData, controller);
            var cancellationToken = new CancellationToken();
            var authCodeMvcApp = factory.Create();
            var authResultTask = authCodeMvcApp.AuthorizeAsync(cancellationToken);
            authResultTask.Wait();
            var result = authResultTask.Result;
            return result;
        }

        private static CalendarService InitializeService(Google.Apis.Auth.OAuth2.Web.AuthorizationCodeWebApp.AuthResult authResult)
        {
            var result = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = authResult.Credential,
                ApplicationName = "Google Calendar Integration Test"
            });
            return result;
        }

        private static string GetMainCalendarId(CalendarService service)
        {
            var calendarListRequest = new CalendarListResource.ListRequest(service);
            var calendars = calendarListRequest.Execute();
            var result = calendars.Items.First().Id;
            return result;
        }

        private static Event GetCalendarEvent(CalenderEvent calEvent)
        {
            var result = new Event();
            result.Summary = calEvent.eventSummary;
            result.Description = calEvent.eventDescription;
            result.Sequence = 1;
            var eventStartDate = new EventDateTime();
            eventStartDate.DateTime = calEvent.eventStart;
            result.Start = eventStartDate;
            var eventEndDate = new EventDateTime();
            eventEndDate.DateTime = calEvent.eventEnd;
            result.End = eventEndDate;

            return result;
        }

        private static void SyncCalendarEventToCalendar(CalendarService service, Event calendarEvent, string calendarId)
        {
            var eventRequest = new EventsResource.InsertRequest(service, calendarEvent, calendarId);
            eventRequest.Execute();
        }
    }
}