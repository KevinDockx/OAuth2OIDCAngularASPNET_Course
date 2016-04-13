using System;

namespace TripGallery.DTO
{
    public class Picture
    {
        public Guid Id { get; set; }
        public Guid TripId { get; set; }
        public string Title { get; set; }
        public string Uri { get; set; }
        public string OwnerId { get; set; }
    }
}
