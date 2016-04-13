using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using TripGallery.API.Helpers;
using TripGallery.API.UnitOfWork.Trip;

namespace TripGallery.API.Controllers
{

    [EnableCors("https://localhost:44316", "*", "GET, POST, PATCH")]
    public class TripsController : ApiController
    {

        // anyone can get trips
        [Route("api/trips")]
        [HttpGet]
        public IHttpActionResult Get()
        {
            try
            {            
                using (var uow = new GetTrips(null))
                {
                    var uowResult = uow.Execute();

                    switch (uowResult.Status)
                    {
                        case UnitOfWork.UnitOfWorkStatus.Ok:
                            return Ok(uowResult.Result);

                        default:
                            return InternalServerError();
                    }
                }

            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }


        [Route("api/trips/{tripId}")]
        [HttpGet]
        public IHttpActionResult Get(Guid tripId)
        {
            try
            { 
                using (var uow = new GetTrip(null, tripId))
                    {
                        var uowResult = uow.Execute();

                        switch (uowResult.Status)
                        {
                            case UnitOfWork.UnitOfWorkStatus.Ok:
                                return Ok(uowResult.Result);

                            case UnitOfWork.UnitOfWorkStatus.NotFound:
                                return NotFound();

                            case UnitOfWork.UnitOfWorkStatus.Forbidden:
                                return  StatusCode(HttpStatusCode.Forbidden);
                            
                            default:
                                return InternalServerError();
                        }
                    }
                 
            }
            catch (Exception)
            {          
                return InternalServerError();
            }
        }
 

        [Route("api/trips")]
        [HttpPost]
        public IHttpActionResult Post([FromBody]DTO.TripForCreation tripForCreation)
        {
            try
            {                 
          
                using (var uow = new CreateTrip(null))
                {
                    var uowResult = uow.Execute(tripForCreation);

                    switch (uowResult.Status)
                    {
                        case UnitOfWork.UnitOfWorkStatus.Ok:
                            return Created<DTO.Trip>
                            (Request.RequestUri + "/" + uowResult.Result.Id.ToString(), uowResult.Result);
                                               
                        case UnitOfWork.UnitOfWorkStatus.Invalid:
                            return BadRequest();

                        case UnitOfWork.UnitOfWorkStatus.Forbidden:
                            return StatusCode(HttpStatusCode.Forbidden);

                        default:
                            return InternalServerError();
                    }
                }
            }
            catch (Exception)
            {             
                return InternalServerError();
            }
        }
         
        [Route("api/trips/{tripId}")]
        [HttpPatch]
        public IHttpActionResult Patch(Guid tripId,
            [FromBody]Marvin.JsonPatch.JsonPatchDocument<DTO.Trip> tripPatchDocument)
        {

            try
            {

             
                using (var uow = new PartiallyUpdateTrip(null, tripId))
                {
                    var uowResult = uow.Execute(tripPatchDocument);

                    switch (uowResult.Status)
                    {
                        case UnitOfWork.UnitOfWorkStatus.Ok:
                            return Ok(uowResult.Result);

                        case UnitOfWork.UnitOfWorkStatus.Invalid:
                            return BadRequest();

                        case UnitOfWork.UnitOfWorkStatus.Forbidden:
                            return StatusCode(HttpStatusCode.Forbidden);

                        case UnitOfWork.UnitOfWorkStatus.NotFound:
                            return NotFound();

                        default:
                            return InternalServerError();
                    }
                }
            }
            catch (Exception)
            {
                return InternalServerError();
            } 
        }
    }
}

