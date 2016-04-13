using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripGallery.Repository.Entities
{
    public class Trip
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public string MainPictureUri { get; set; }
        public string OwnerId { get; set; }
        public IList<Picture> Pictures { get; set; }

        public Trip()
        {
            Pictures = new List<Picture>();
        }
    }
}
