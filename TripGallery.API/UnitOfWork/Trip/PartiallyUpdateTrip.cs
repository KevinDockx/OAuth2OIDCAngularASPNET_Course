using AutoMapper;
using Marvin.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripGallery.API.Helpers;
using TripGallery.Repository;
using TripGallery.Repository.Entities;

namespace TripGallery.API.UnitOfWork.Trip
{
    public class PartiallyUpdateTrip : IUnitOfWork<DTO.Trip, 
        JsonPatchDocument<DTO.Trip>>, IDisposable
    {
        ITripRepository _tripRepository;
        TripContext _ctx;
        Guid _tripId;
        string _ownerId;
        
        private PartiallyUpdateTrip()
        {
            _ctx = new TripContext(@"app_data/tripstore.json");
            _tripRepository = new TripRepository(_ctx);
        }

        public PartiallyUpdateTrip(string ownerId, Guid tripId)
            : this()
        {
            _ownerId = ownerId;
            _tripId = tripId;
        }

        public UnitOfWorkResult<DTO.Trip> Execute(JsonPatchDocument<DTO.Trip> input)
        {
          
            if (input == null)
                return new UnitOfWorkResult<DTO.Trip>(null, UnitOfWorkStatus.Invalid); 

            // get the trip
            var tripEntity = _tripRepository.GetTrip(_tripId);
            if (tripEntity == null)
            {
                return new UnitOfWorkResult<DTO.Trip>(null, UnitOfWorkStatus.NotFound);
            }

            // we're updating.  Only the user that created the trip can update it.
            if (string.IsNullOrWhiteSpace(tripEntity.OwnerId) || tripEntity.OwnerId != _ownerId)
                return new UnitOfWorkResult<DTO.Trip>(null, UnitOfWorkStatus.Forbidden);

            // convert entity to DTO
            var dtoForPatch = Mapper.Map<TripGallery.Repository.Entities.Trip,
             DTO.Trip>(tripEntity);

            // apply the patchdoc to the DTO
            input.ApplyTo(dtoForPatch);

            // update entity
            tripEntity = Mapper.Map<DTO.Trip, TripGallery.Repository.Entities.Trip>
                (dtoForPatch);

            _tripRepository.UpdateTrip(tripEntity);

            // save
            _ctx.SaveChanges();
         
            // return DTO
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
