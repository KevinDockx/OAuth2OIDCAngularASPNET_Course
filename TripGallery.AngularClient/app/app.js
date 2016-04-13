(function () {
    //"use strict";

    var app = angular.module("tripGallery",
                            ["ngRoute", "common.services"]);
     
     

    app.config(function ($routeProvider) {
 
        $routeProvider            
            .when("/trips", {
                templateUrl: "/app/trips/tripIndex.html",
                controller: "tripIndexController as vm"
            })
            .when("/trips/create", {
                templateUrl: "/app/trips/tripCreate.html",
                controller: "tripCreateController as vm"
            })
            .when("/trips/:tripId/pictures", {
                templateUrl: "/app/pictures/pictureIndex.html",
                controller: "pictureIndexController as vm"
            })
            .when("/trips/:tripId/pictures/create", {
                templateUrl: "/app/pictures/pictureCreate.html",
                controller: "pictureCreateController as vm"
            })
            .when("/trips/:tripId/createalbum", {
                 templateUrl: "/app/trips/tripAlbum.html",
                 controller: "tripAlbumController as vm"
             })
           .otherwise({ redirectTo: "/trips" });
       
    });




    // file upload directive cfr http://uncorkedstudios.com/blog/multipartformdata-file-upload-with-angularjs
    app.directive('fileModel', ['$parse', function ($parse) {
        return {
            restrict: 'A',
            link: function(scope, element, attrs) {
                var model = $parse(attrs.fileModel);
                var modelSetter = model.assign;
            
                element.bind('change', function(){
                    scope.$apply(function(){
                        modelSetter(scope, element[0].files[0]);
                    });
                });
            }
        };
    }]);

}());