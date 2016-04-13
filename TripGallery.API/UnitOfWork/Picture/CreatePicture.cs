using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripGallery.Repository;
using TripGallery.Repository.Entities;

namespace TripGallery.API.UnitOfWork.Picture
{
    public class CreatePicture : IUnitOfWork<DTO.Picture, DTO.PictureForCreation>, IDisposable
    {

        ITripRepository _tripRepository;
        TripContext _ctx;
        string _ownerId = null;
        Guid _tripId;

        private CreatePicture()
        {
            _ctx = new TripContext(@"app_data/tripstore.json");
            _tripRepository = new TripRepository(_ctx);
        }

        public CreatePicture(string ownerId, Guid tripId)
            : this()
        {
            _ownerId = ownerId;
            _tripId = tripId;
        }


        public UnitOfWorkResult<DTO.Picture> Execute(DTO.PictureForCreation input)
        {
            if (input == null)
            {
                return new UnitOfWorkResult<DTO.Picture>(null, UnitOfWorkStatus.Invalid);
            }

            if (_ownerId == null)
            {
                // can only create a picture if we know who the user is
                return new UnitOfWorkResult<DTO.Picture>(null, UnitOfWorkStatus.Forbidden);
            }

            // does the trip exist?
            if (!(_tripRepository.TripExists(_tripId)))
                return new UnitOfWorkResult<DTO.Picture>(null, UnitOfWorkStatus.NotFound);

            // can this user add a picture to this trip?  Either the trip must be public,
            // or the user must own the trip
            if (!(_tripRepository.CanAddPictureToTrip(_tripId, _ownerId)))
                return new UnitOfWorkResult<DTO.Picture>(null, UnitOfWorkStatus.Forbidden);

            // map to entity
            var pictureEntity = Mapper.Map<DTO.PictureForCreation, Repository.Entities.Picture>(input);

            // create guid
            var id = Guid.NewGuid();
            pictureEntity.Id = id;
            pictureEntity.TripId = _tripId;
            pictureEntity.OwnerId = _ownerId;

            // save the array of bytes on disk
            // file name (limited/assumes JPG)
            var fileSystem = new Microsoft.Owin.FileSystems.PhysicalFileSystem("Images");
            string fileName = Guid.NewGuid().ToString() + ".jpg";

             // write and auto-close
            File.WriteAllBytes( fileSystem.Root + "/" + fileName,input.PictureBytes);
   
            // fill out URI
            pictureEntity.Uri = "images/" + fileName;

            _tripRepository.InsertPicture(pictureEntity);

            // commit
            _ctx.SaveChanges();


            // return a dto
            var dto = Mapper.Map<Repository.Entities.Picture, DTO.Picture>(pictureEntity);
            return new UnitOfWorkResult<DTO.Picture>(dto, UnitOfWorkStatus.Ok);

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
