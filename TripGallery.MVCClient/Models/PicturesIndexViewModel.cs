using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripGallery.DTO;

namespace TripGallery.MVCClient.Models
{
    public class PicturesIndexViewModel
    {
        public List<Picture> Pictures { get; set; }

        public Guid TripId { get; set; }

        public PicturesIndexViewModel(List<Picture> pictures, Guid tripId)
        {
            Pictures = pictures;
            TripId = tripId;
        }
    }
}
