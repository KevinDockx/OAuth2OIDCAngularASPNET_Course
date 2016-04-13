using Microsoft.Owin.FileSystems;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripGallery.Repository.Entities
{
    public class TripContext : IDisposable
    {
        private string _fileDBLocation;

        public TripContext(string fileDBLocation)
        {
            _fileDBLocation = fileDBLocation;
 
            var fileSystem = new Microsoft.Owin.FileSystems.PhysicalFileSystem("");

            IFileInfo fi;
            if (fileSystem.TryGetFileInfo(_fileDBLocation, out fi))
            {

                var json = File.ReadAllText(fi.PhysicalPath);
                var result = JsonConvert.DeserializeObject<List<Trip>>(json);
                
                Trips = result.ToList();
            }

        }

        public IList<Trip> Trips { get; set; }

        public bool SaveChanges()
        {
            // write trips to json file, overwriting the old one

            var json = JsonConvert.SerializeObject(Trips);

            var fileSystem = new Microsoft.Owin.FileSystems.PhysicalFileSystem("");

            IFileInfo fi;
            if (fileSystem.TryGetFileInfo(_fileDBLocation, out fi))
            {
                File.WriteAllText(fi.PhysicalPath, json);
                return true;
            }

            return false;
        }

        public void Dispose()
        {
            // cleanup, todo
        }
    }
}
