using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using TripGallery.API.Helpers;
using TripGallery.API.UnitOfWork.Picture;

namespace TripGallery.API.Controllers
{
  
    [EnableCors("https://localhost:44316", "*", "GET, POST, DELETE")]
    public class PicturesController : ApiController
    {

        [Route("api/trips/{tripId}/pictures")]
        [HttpGet]
        public IHttpActionResult Get(Guid tripId)
        {
            try
            {
          
                using (var uow = new GetPictures(null, tripId))
                {
                    var uowResult = uow.Execute();

                    switch (uowResult.Status)
                    {
                        case UnitOfWork.UnitOfWorkStatus.Ok:
                            return Ok(uowResult.Result);

                        case UnitOfWork.UnitOfWorkStatus.NotFound:
                            return NotFound();

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

   
        [Route("api/trips/{tripId}/pictures")]
        [HttpPost]
        public IHttpActionResult Post(Guid tripId, [FromBody]DTO.PictureForCreation pictureForCreation)
        {
            try
            { 
                using (var uow = new CreatePicture(null, tripId))
                {
                    var uowResult = uow.Execute(pictureForCreation);

                    switch (uowResult.Status)
                    {
                        case UnitOfWork.UnitOfWorkStatus.Ok:
                            return Created<DTO.Picture>
                            (Request.RequestUri + "/" + uowResult.Result.Id.ToString(), uowResult.Result);
                            
                        case UnitOfWork.UnitOfWorkStatus.Invalid:
                            return BadRequest();

                        case UnitOfWork.UnitOfWorkStatus.NotFound:
                            return NotFound();

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

         
        [Route("api/trips/{tripId}/pictures/{pictureId}")]
        [HttpDelete]
        public IHttpActionResult Delete(Guid tripId, Guid pictureId)
        {
            try
            {
          
                using (var uow = new DeletePicture(null, tripId, pictureId))
                {
                    var uowResult = uow.Execute();

                    switch (uowResult.Status)
                    {
                        case UnitOfWork.UnitOfWorkStatus.Ok:
                            return StatusCode(HttpStatusCode.NoContent);

                        case UnitOfWork.UnitOfWorkStatus.Invalid:
                            return BadRequest();

                        case UnitOfWork.UnitOfWorkStatus.NotFound:
                            return NotFound();

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
    }
}
