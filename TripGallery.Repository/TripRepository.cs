using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripGallery.Repository.Entities;

namespace TripGallery.Repository
{
    public class TripRepository : ITripRepository, IDisposable
    {
        TripContext _ctx;

        public TripRepository(TripContext tripContext)
        {
            _ctx = tripContext;
        }


        public bool TripExists(Guid tripId)
        {
            return _ctx.Trips.Any(t => t.Id == tripId);
        }

        public bool CanGetTrip(Guid tripId, string ownerId)
        {
            return _ctx.Trips.Any(t => t.Id == tripId && (t.IsPublic || t.OwnerId == ownerId));
        }

        public bool CanAddPictureToTrip(Guid tripId, string ownerId)
        {
            return _ctx.Trips.Any(t => t.Id == tripId && (t.IsPublic || t.OwnerId == ownerId));
        }

        public IQueryable<Trip> GetTrips()
        {
            return _ctx.Trips.AsQueryable();
        }


        public IQueryable<Trip> GetTrips(string ownerId, bool includePictures = true)
        {

            var trips = _ctx.Trips.Where(t => t.OwnerId == ownerId || t.IsPublic);

            // note: demo works with in-mem db from json file.  With an EF repo, you'd use "include".
            if (includePictures)
            {
                return trips.AsQueryable();
            }
            else
            {
                return trips.Select(t => new Trip()
                {
                    Id = t.Id,
                    Description = t.Description,
                    Name = t.Name,
                    OwnerId = t.OwnerId,
                    MainPictureUri = t.MainPictureUri,
                    IsPublic = t.IsPublic
                }).AsQueryable();
            }
        }


        public Trip GetTrip(Guid id)
        {
            var trip = _ctx.Trips.FirstOrDefault(t => t.Id == id);
            return trip;

        }


        public void InsertTrip(Trip trip)
        {
            _ctx.Trips.Add(trip);
        }

        public void UpdateTrip(Trip trip)
        {
            // no code required
        }

        public bool DeleteTrip(Guid tripId)
        {

            var trip = _ctx.Trips.FirstOrDefault(t => t.Id == tripId);

            if (trip != null)
            {
                _ctx.Trips.Remove(trip);
                return true;
            }
            return false;
        }

       public bool PictureExistsForTrip(Guid tripId, Guid pictureId)
        {
            var trip = _ctx.Trips.FirstOrDefault(t => t.Id == tripId);

            if (trip != null)
            {
                var pic = trip.Pictures.FirstOrDefault(p => p.Id == pictureId);
                if (pic != null)
                {
                    return true;
                }
            }
            return false;
        }

       public bool CanGetPicturesForTrip(Guid tripId, string ownerId)
       {
           return _ctx.Trips.Any(t => t.Id == tripId && (t.IsPublic || t.OwnerId == ownerId));
       }

        public IQueryable<Picture> GetPicturesForTrip(Guid tripId)
        {

            if (_ctx.Trips.Any(t => t.Id == tripId))
            {
                return _ctx.Trips.FirstOrDefault(t => t.Id == tripId).Pictures.AsQueryable();
            }

            return null;
        }

        public Picture GetPictureForTrip(Guid tripId, Guid id)
        {
            var trip = _ctx.Trips.FirstOrDefault(t => t.Id == tripId);

            if (trip != null)
            {
                var pic = trip.Pictures.FirstOrDefault(p => p.Id == id);
                if (pic != null)
                {
                    return pic;
                }
            }
            return null;

        }


        public void InsertPicture(Picture picture)
        {
            var trip = _ctx.Trips.FirstOrDefault(t => t.Id == picture.TripId);

            if (trip != null)
            {
                trip.Pictures.Add(picture);
            }
        }



        public void UpdatePicture(Picture picture)
        {
            // no code required
        }


        public bool DeletePicture(Guid tripId, Guid pictureId)
        {

            var trip = _ctx.Trips.FirstOrDefault(t => t.Id == tripId);

            if (trip != null)
            {
                var pic = trip.Pictures.FirstOrDefault(p => p.Id == pictureId);
                if (pic != null)
                {
                    trip.Pictures.Remove(pic);
                    return true;
                }

            }

            return false;
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
                if (_ctx != null)
                {
                    _ctx.Dispose();
                    _ctx = null;
                }

            }
        }

    }
}
