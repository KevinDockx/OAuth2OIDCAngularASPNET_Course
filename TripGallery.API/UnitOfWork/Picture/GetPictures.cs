using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripGallery.Repository;
using TripGallery.Repository.Entities;

namespace TripGallery.API.UnitOfWork.Picture
{
    public class GetPictures: IUnitOfWork<IEnumerable<DTO.Picture>>, IDisposable
    {

        ITripRepository _tripRepository;
        TripContext _ctx;
        string _ownerId;
        Guid _tripId;
     
        private GetPictures()
        {
           _ctx = new TripContext(@"app_data/tripstore.json");
           _tripRepository = new TripRepository(_ctx);
        }

        public GetPictures(string ownerId, Guid tripId)
            : this()
        {          
            _ownerId = ownerId;
            _tripId = tripId;
        }

        public UnitOfWorkResult<IEnumerable<DTO.Picture>> Execute()
        {


            if (!_tripRepository.TripExists(_tripId))
            {
                return new UnitOfWorkResult<IEnumerable<DTO.Picture>>(null, UnitOfWorkStatus.NotFound);
            }

            // the trip exists.  But: is the logged-in user allowed to view its pictures?  
            // => only return pictures for either public trips, or the users' own trips

            if (!_tripRepository.CanGetPicturesForTrip(_tripId, _ownerId))
            {
                return new UnitOfWorkResult<IEnumerable<DTO.Picture>>(null, UnitOfWorkStatus.Forbidden);
            }


            var pictureEntities = _tripRepository.GetPicturesForTrip(_tripId);
                       
            // return dto's
            var dtos = Mapper.Map<IEnumerable<Repository.Entities.Picture>, IEnumerable<DTO.Picture>>(pictureEntities);
            return new UnitOfWorkResult<IEnumerable<DTO.Picture>>(dtos, UnitOfWorkStatus.Ok);
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
