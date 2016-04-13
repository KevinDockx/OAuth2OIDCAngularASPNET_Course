using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TripGallery.API.Helpers
{
    public class InjectImageBaseForPictureResolver : ValueResolver<Repository.Entities.Picture,string>
    {

        protected override string ResolveCore(Repository.Entities.Picture source)
        { 
            string fullUri = "https://localhost:44315/" + source.Uri;
            return fullUri;
        }
    }

    public class InjectImageBaseForTripResolver : ValueResolver<Repository.Entities.Trip, string>
    {

        protected override string ResolveCore(Repository.Entities.Trip source)
        {
            string fullUri = Constants.TripGalleryAPI + source.MainPictureUri;
            return fullUri;
        }
    }


    public class RemoveImageBaseForTripResolver : ValueResolver<DTO.Trip, string>
    {

        protected override string ResolveCore(DTO.Trip source)
        {
            string partialUri = source.MainPictureUri;
            // find
            var indexOfUri = partialUri.IndexOf(Constants.TripGalleryAPI);
            if (indexOfUri > -1)
            {
                partialUri = partialUri.Substring(Constants.TripGalleryAPI.Length);
            }
           
            return partialUri;
        }
    }






}