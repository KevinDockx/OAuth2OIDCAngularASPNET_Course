using Marvin.JsonPatch;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TripGallery.DTO;
using TripGallery.MVCClient.Helpers;
using TripGallery.MVCClient.Models;

namespace TripGallery.MVCClient.Controllers
{
    public class TripsController : Controller
    {
        // GET: Trips
        public async Task<ActionResult> Index()
        {

            var httpClient = TripGalleryHttpClient.GetClient();

            var rspTrips = await httpClient.GetAsync("api/trips").ConfigureAwait(false);

            if (rspTrips.IsSuccessStatusCode)
            {
                var lstTripsAsString = await rspTrips.Content.ReadAsStringAsync().ConfigureAwait(false);

                var vm = new TripsIndexViewModel();
                vm.Trips = JsonConvert.DeserializeObject<IList<Trip>>(lstTripsAsString).ToList();

                return View(vm);
            }
            else
            {
                return View("Error",
                         new HandleErrorInfo(ExceptionHelper.GetExceptionFromResponse(rspTrips),
                        "Trips", "Index"));
            }
        }

        // GET: Trips/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }



        // GET: Trips/Create
        public ActionResult Create()
        {
            return View(new TripCreateViewModel(new TripForCreation()));
        }


        public async Task<ActionResult> Album(Guid tripId)
        {
            return View();
        }

        // POST: Trips/Create
        [HttpPost]
        public async Task<ActionResult> Create(TripCreateViewModel vm)
        {
            try
            {

                byte[] uploadedImage = new byte[vm.MainImage.InputStream.Length];
                vm.MainImage.InputStream.Read(uploadedImage, 0, uploadedImage.Length);

                vm.Trip.MainPictureBytes = uploadedImage;

                var httpClient = TripGalleryHttpClient.GetClient();

                var serializedTrip = JsonConvert.SerializeObject(vm.Trip);

                var response = await httpClient.PostAsync("api/trips",
                    new StringContent(serializedTrip, System.Text.Encoding.Unicode, "application/json")).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Trips");
                }
                else
                {
                    return View("Error",
                            new HandleErrorInfo(ExceptionHelper.GetExceptionFromResponse(response),
                            "Trips", "Create"));
                }
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Trips", "Create"));
            }
        }


        public async Task<ActionResult> SwitchPrivacyLevel(Guid id, bool isPublic)
        {

            // create a patchdocument to change the privacy level of this trip

            JsonPatchDocument<Trip> tripPatchDoc = new JsonPatchDocument<Trip>();
            tripPatchDoc.Replace(t => t.IsPublic, !isPublic);

            var httpClient = TripGalleryHttpClient.GetClient();

            var rspPatchTrip = await httpClient.PatchAsync("api/trips/" + id.ToString(),
                 new StringContent(JsonConvert.SerializeObject(tripPatchDoc), System.Text.Encoding.Unicode, "application/json"))
                 .ConfigureAwait(false);

            if (rspPatchTrip.IsSuccessStatusCode)
            {
                // the patch was succesful.  Reload.
                return RedirectToAction("Index", "Trips");
            }
            else
            {
                return View("Error",
                         new HandleErrorInfo(ExceptionHelper.GetExceptionFromResponse(rspPatchTrip),
                        "Trips", "SwitchPrivacyLevel"));
            }
        }
    }
}
