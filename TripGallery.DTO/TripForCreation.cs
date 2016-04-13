using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripGallery.DTO
{
    public class TripForCreation
    {  
        public string Name { get; set; }
        public string Description { get; set; }
   
        public bool IsPublic { get; set; }
        public byte[] MainPictureBytes { get; set; }

        public TripForCreation()
        {
          
        }
        
    }
}
