(function () {
    "use strict";
    angular
        .module("tripGallery")
        .controller("tripCreateController",
                     ["tripForCreationResource",
                         TripCreateController]);

    function TripCreateController(tripForCreationResource) {
        var vm = this;
        vm.tripPictureFile = null;
        vm.tripForCreation = new tripForCreationResource();

        vm.submit = function () {

            // read file bytes (assumes file has been selected!)
            // readAsDataURL is async, so we need to catch the onload event.
            var fileReader = new FileReader();
            fileReader.onload = function (event) {

                // split so we only get the bytes (descriptive info before ',')
                var imagestr = event.target.result.split(',')[1];

                vm.tripForCreation.mainPictureBytes = imagestr;
                
                // save (POST) the resource
                vm.tripForCreation.$save(
                    // params
                    null,
                    // success
                    function () {
                        // back to trip overview
                        window.location.href = '#/trips';
                    },
                    // failure
                    null
                    );
            };
            fileReader.readAsDataURL(vm.tripPictureFile);
 
        };
    }
}());
