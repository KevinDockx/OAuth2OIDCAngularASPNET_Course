
namespace TripGallery
{
    public class Constants
    {

        public const string TripGalleryAPI = "https://localhost:44315/";
        public const string TripGalleryMVC = "https://localhost:44318/";
        public const string TripGalleryMVCSTSCallback = "https://localhost:44318/stscallback";
        public const string TripGalleryAngular = "https://localhost:44316/";

        public const string TripGalleryClientSecret = "myrandomclientsecret";

        public const string TripGalleryIssuerUri = "https://tripcompanysts/identity";
        public const string TripGallerySTSOrigin = "https://localhost:44317";
        public const string TripGallerySTS = TripGallerySTSOrigin + "/identity";
        public const string TripGallerySTSTokenEndpoint = TripGallerySTS + "/connect/token";
        public const string TripGallerySTSAuthorizationEndpoint = TripGallerySTS + "/connect/authorize";
        public const string TripGallerySTSUserInfoEndpoint = TripGallerySTS + "/connect/userinfo";
        public const string TripGallerySTSEndSessionEndpoint = TripGallerySTS + "/connect/endsession";
        public const string TripGallerySTSRevokeTokenEndpoint = TripGallerySTS + "/connect/revocation";


    }

}
