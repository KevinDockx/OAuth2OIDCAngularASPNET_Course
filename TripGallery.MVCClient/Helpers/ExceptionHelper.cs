using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TripGallery.MVCClient.Helpers
{
    public static class ExceptionHelper
    {
        public static Exception GetExceptionFromResponse(HttpResponseMessage response)
        {
            // unauthorized => missing/bad auth
            // forbidden => you're authenticated, but you can't do this
            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden
                || response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return  new Exception("You're not allowed to do that.");
            }
            else
            {
                return  new Exception("Something went wrong - please contact your administrator.");
            }          
        }
    }
}
