using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TripGallery.DTO;

namespace TripGallery.MVCClient.Models
{
    public class PictureCreateViewModel
    {
        public HttpPostedFileBase PictureFile { get; set; }
        public PictureForCreation Picture { get; set; }

        public Guid TripId { get; set; }

        public PictureCreateViewModel()
        {

        }

        public PictureCreateViewModel(PictureForCreation picture, Guid tripId)
        {
            Picture = picture;
            TripId = tripId;
        }
    }
}
