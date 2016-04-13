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
    public class GetTrips : IUnitOfWork<IEnumerable<DTO.Trip>>, IDisposable
    {

        ITripRepository _tripRepository;
        TripContext _ctx;
        string _ownerId;
     
        private GetTrips()
        {
           _ctx = new TripContext(@"app_data/tripstore.json");
           _tripRepository = new TripRepository(_ctx);
        }

        public GetTrips(string ownerId)
            : this()
        {          
            _ownerId = ownerId;
        }

        public UnitOfWorkResult<IEnumerable<DTO.Trip>> Execute()
        {
            var tripEntities = _tripRepository.GetTrips(_ownerId, false);
                       
            // return dto's
            var dtos = Mapper.Map<IEnumerable<Repository.Entities.Trip>, IEnumerable<DTO.Trip>>(tripEntities);
            return new UnitOfWorkResult<IEnumerable<DTO.Trip>>(dtos, UnitOfWorkStatus.Ok);
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
