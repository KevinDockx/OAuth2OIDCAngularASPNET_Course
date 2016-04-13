using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripGallery.Repository;
using TripGallery.Repository.Entities;

namespace TripGallery.API.UnitOfWork.Picture
{
    public class DeletePicture: IUnitOfWork, IDisposable
    {

        ITripRepository _tripRepository;
        TripContext _ctx;
        string _ownerId = null;
        Guid _pictureId;
        Guid _tripId;

        private DeletePicture()
        {
            _ctx = new TripContext(@"app_data/tripstore.json");
            _tripRepository = new TripRepository(_ctx);
        }

        public DeletePicture(string ownerId, Guid tripId, Guid pictureId)
            : this()
        {
            _ownerId = ownerId;
            _tripId = tripId;
            _pictureId = pictureId;
        }


        public UnitOfWorkResult Execute()
        {
            // a user can only delete pictures he himself created, so if there's no ownerId,
            // a picture cannot be deleted.
            if (_ownerId == null)
            {
                return new UnitOfWorkResult(UnitOfWorkStatus.Forbidden);
            }

            if (!_tripRepository.PictureExistsForTrip(_tripId, _pictureId))
            {
                return new UnitOfWorkResult(UnitOfWorkStatus.NotFound);
            }

            var picture = _tripRepository.GetPictureForTrip(_tripId, _pictureId);
            if (picture.OwnerId != _ownerId)
            {
                // not the picture's owner
                return new UnitOfWorkResult(UnitOfWorkStatus.Forbidden);
            }

        
            _tripRepository.DeletePicture(_tripId, _pictureId);

            _ctx.SaveChanges();

            return new UnitOfWorkResult(UnitOfWorkStatus.Ok);
           

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