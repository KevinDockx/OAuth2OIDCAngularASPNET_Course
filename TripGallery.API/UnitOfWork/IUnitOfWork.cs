
namespace TripGallery.API.UnitOfWork
{

    public interface IUnitOfWork<T, S>
        where T : class
        where S : class
    {

        UnitOfWorkResult<T> Execute(S input);

    }

    public interface IUnitOfWork<T>
        where T : class
    {

        UnitOfWorkResult<T> Execute();

    }


    public interface IUnitOfWork
    {

        UnitOfWorkResult Execute();

    }

}
