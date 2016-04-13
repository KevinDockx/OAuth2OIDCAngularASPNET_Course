(function () {
    "use strict";

    angular
        .module("common.services")
        .factory("pictureForCreationResource",
                ["$resource",
                 "appSettings",
                    pictureForCreationResource])

    //cfr: https://docs.angularjs.org/api/ngResource/service/$resource


    function pictureForCreationResource($resource, appSettings)
    {
        //resource(url, paramDefaults, actions
        return $resource(appSettings.tripGalleryAPI + "/api/trips/:tripId/pictures", null,
            {
               
            });
    }
}());

