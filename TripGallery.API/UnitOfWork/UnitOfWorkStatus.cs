
namespace TripGallery.API.UnitOfWork
{
    public enum UnitOfWorkStatus
    {
        Ok,
        NotFound,
        Conflict,
        Exception,
        Invalid,
        Forbidden
    }
}
