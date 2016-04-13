using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripGallery.DTO;

namespace TripGallery.MVCClient.Models
{
    public class TripsIndexViewModel
    {
        public List<Trip> Trips { get; set; }
 


        public TripsIndexViewModel()
        {
            Trips = new List<Trip>();
        }
    }
}
