using System;
using System.Linq;
using TripGallery.Repository.Entities;

namespace TripGallery.Repository
{
    public interface ITripRepository
    {

        bool TripExists(Guid tripId);
        bool CanGetTrip(Guid tripId, string ownerId);
        IQueryable<Trip> GetTrips();
        IQueryable<Trip> GetTrips(string ownerId, bool includePictures = true);
        Trip GetTrip(Guid id);
        void InsertTrip(Trip trip);
        void UpdateTrip(Trip trip);
        bool DeleteTrip(Guid tripId);


        bool PictureExistsForTrip(Guid tripId, Guid pictureId);

        bool CanGetPicturesForTrip(Guid tripId, string ownerId);

        bool CanAddPictureToTrip(Guid tripId, string ownerId);
        IQueryable<Picture> GetPicturesForTrip(Guid tripId);
        Picture GetPictureForTrip(Guid tripId, Guid id);
        void InsertPicture(Picture picture);
        void UpdatePicture(Picture picture);
        bool DeletePicture(Guid tripId, Guid pictureId);
        void Dispose();
    }
}
