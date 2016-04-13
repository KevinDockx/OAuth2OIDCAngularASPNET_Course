using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripGallery.Repository.Entities
{
    public class Picture
    {
        public Guid Id { get; set; }
        public Guid TripId { get; set; }
        public string Title { get; set; }
        public string Uri { get; set; }

        public string OwnerId { get; set; }
    }
}
