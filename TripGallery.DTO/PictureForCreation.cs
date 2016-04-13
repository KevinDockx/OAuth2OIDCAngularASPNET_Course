using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripGallery.DTO
{
    public class PictureForCreation
    {
        public string Title { get; set; }
        public byte[] PictureBytes { get; set; }
    }
}
