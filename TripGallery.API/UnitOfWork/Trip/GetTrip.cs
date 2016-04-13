using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripGallery.Repository;
using TripGallery.Repository.Entities;

namespace TripGallery.API.UnitOfWork.Trip
{
    public class GetTrip : IUnitOfWork<DTO.Trip>, IDisposable
    {

        ITripRepository _tripRepository;
        TripContext _ctx;     
        Guid _tripId;
        string _ownerId;

        private GetTrip()
        {
           _ctx = new TripContext(@"app_data/tripstore.json");
           _tripRepository = new TripRepository(_ctx);
        }

        public GetTrip(string ownerId, Guid tripId)
            : this()
        {
            _ownerId = ownerId;
            _tripId = tripId;
        }

        public UnitOfWorkResult<DTO.Trip> Execute()
        {
            // is the user allowed to get this trip?  The trip must be public for this,
            // or the user must the owner

            if (!_tripRepository.CanGetTrip(_tripId, _ownerId))
            {
                return new UnitOfWorkResult<DTO.Trip>(null, UnitOfWorkStatus.Forbidden);
            }


            var tripEntity = _tripRepository.GetTrip(_tripId);

            if (tripEntity == null)
            {
                return new UnitOfWorkResult<DTO.Trip>(null, UnitOfWorkStatus.NotFound);
            }
                       
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
