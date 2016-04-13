using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TripGallery.API.Helpers
{
    public static class IListExtensions
    {
        public static void RemoveRange<T>(this IList<T> source, IEnumerable<T> rangeToRemove)
        {
            if (rangeToRemove == null | !rangeToRemove.Any())
                return;

            foreach (T item in rangeToRemove)
            {
                source.Remove(item);
            }


        }
    }
}