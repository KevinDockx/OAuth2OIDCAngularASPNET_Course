using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripGallery.Repository;
using TripGallery.Repository.Entities;

namespace TripGallery.API.UnitOfWork.Trip
{
    public class CreateTrip : IUnitOfWork<DTO.Trip, DTO.TripForCreation>, IDisposable
    {

        ITripRepository _tripRepository;
        TripContext _ctx;
        string _ownerId = null;
     
        private CreateTrip()
        {
           _ctx = new TripContext(@"app_data/tripstore.json");
           _tripRepository = new TripRepository(_ctx);
        }
    
        public CreateTrip(string ownerId)
            : this()
        {
            _ownerId = ownerId;
        }


        public UnitOfWorkResult<DTO.Trip> Execute(DTO.TripForCreation input)
        {          
            if (input == null)
            {
                return new UnitOfWorkResult<DTO.Trip>(null, UnitOfWorkStatus.Invalid);
            }
 
            if (_ownerId == null)
            {
                // cannot create a trip when there's no owner id
                return new UnitOfWorkResult<DTO.Trip>(null, UnitOfWorkStatus.Forbidden);
            }

            // map to entity
            var tripEntity = Mapper.Map<DTO.TripForCreation, Repository.Entities.Trip>(input);

            // create guid
            var id = Guid.NewGuid();
            tripEntity.Id = id;
            tripEntity.OwnerId = _ownerId;

            // save the array of bytes on disk
            // file name (limited/assumes JPG)
            var fileSystem = new Microsoft.Owin.FileSystems.PhysicalFileSystem("images");
            string fileName = Guid.NewGuid().ToString() + ".jpg";

            // write and auto-close
            File.WriteAllBytes(fileSystem.Root + "/" + fileName, input.MainPictureBytes);

            // fill out URI
            tripEntity.MainPictureUri = "images/" + fileName;
     
            _tripRepository.InsertTrip(tripEntity);

            // commit
            _ctx.SaveChanges();
            
            // return a dto
            var dto = Mapper.Map<Repository.Entities.Trip, DTO.Trip>(tripEntity);
            return new UnitOfWorkResult<DTO.Trip>(dto, UnitOfWorkStatus.Ok);

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_tripRepository != null)
                {
                    _tripRepository.Dispose();
                    _tripRepository = null;
                } 
            }
        }
    }
}
