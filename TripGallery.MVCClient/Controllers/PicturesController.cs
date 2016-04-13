using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TripGallery.DTO;
using TripGallery.MVCClient.Helpers;
using TripGallery.MVCClient.Models;

namespace TripGallery.MVCClient.Controllers
{

    public class PicturesController : Controller
    {

        public async Task<ActionResult> Index(Guid tripId)
        {
            var httpClient = TripGalleryHttpClient.GetClient();

            var rspPictures = await httpClient.GetAsync("api/trips/" + tripId.ToString() + "/pictures").ConfigureAwait(false);

            if (rspPictures.IsSuccessStatusCode)
            {
                var lstPicturesAsString = await rspPictures.Content.ReadAsStringAsync().ConfigureAwait(false);

                var vm = new PicturesIndexViewModel(
                    JsonConvert.DeserializeObject<IList<Picture>>(lstPicturesAsString).ToList()
                    , tripId);

                return View(vm);
            }
            else
            {
                return View("Error",
                           new HandleErrorInfo(ExceptionHelper.GetExceptionFromResponse(rspPictures),
                           "Pictures", "Index"));
            }
        }



        public ActionResult Create(Guid tripId)
        {
            return View(new PictureCreateViewModel(new PictureForCreation(), tripId));
        }


        [HttpPost]
        public async Task<ActionResult> Create(PictureCreateViewModel vm)
        {
            try
            {

                byte[] uploadedImage = new byte[vm.PictureFile.InputStream.Length];
                vm.PictureFile.InputStream.Read(uploadedImage, 0, uploadedImage.Length);

                vm.Picture.PictureBytes = uploadedImage;

                var httpClient = TripGalleryHttpClient.GetClient();

                var serializedTrip = JsonConvert.SerializeObject(vm.Picture);

                var response = await httpClient.PostAsync("api/trips/" + vm.TripId.ToString() + "/pictures",
                    new StringContent(serializedTrip, System.Text.Encoding.Unicode, "application/json")).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Pictures", new { tripId = vm.TripId });
                }
                else
                {
                    return View("Error",
                           new HandleErrorInfo(ExceptionHelper.GetExceptionFromResponse(response),
                           "Pictures", "Create"));
                }
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Pictures", "Create"));
            }
        }

        public async Task<ActionResult> Delete(Guid tripId, Guid pictureId)
        {
            var httpClient = TripGalleryHttpClient.GetClient();

            var response = await httpClient.DeleteAsync("api/trips/" + tripId.ToString() + "/pictures/" + pictureId.ToString())
                .ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", new { tripId = tripId });
            }
            else
            {
                return View("Error",
                     new HandleErrorInfo(ExceptionHelper.GetExceptionFromResponse(response),
                     "Pictures", "Delete"));
            }
        }
    }
}
