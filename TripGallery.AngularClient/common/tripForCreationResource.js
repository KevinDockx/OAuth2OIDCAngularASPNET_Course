(function () {
    "use strict";

    angular
        .module("common.services")
        .factory("tripForCreationResource",
                ["$resource",
                 "appSettings",               
                    tripForCreationResource])

    function tripForCreationResource($resource, appSettings) {
        return $resource(appSettings.tripGalleryAPI + "/api/trips", null,{});
    }
}());

