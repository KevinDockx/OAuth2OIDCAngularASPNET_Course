(function () {
    "use strict";
    angular
        .module("tripGallery")
        .controller("pictureCreateController",
                     ["pictureForCreationResource",
                         "$routeParams",
                         PictureCreateController]);

    function PictureCreateController(pictureForCreationResource, $routeParams) {
        var vm = this;

        vm.pictureFile = null;
        vm.tripId = $routeParams.tripId;

        // set to new instance of the resource.  We can now add
        // data to it.
        vm.pictureForCreation = new pictureForCreationResource();
  

        vm.submit = function () {
          
            // read file bytes (assumes file has been selected!)
            // readAsDataURL is async, so we need to catch the onload event.
             var fileReader = new FileReader();
             fileReader.onload = function (event) {

                 // split so we only get the bytes (descriptive info before ',')
                 var imagestr = event.target.result.split(',')[1];
 
                 debugger;

                 vm.pictureForCreation.pictureBytes = imagestr;

                 // save (POST) the resource
                 vm.pictureForCreation.$save(
                     // params
                     {
                         tripId: vm.tripId
                     },
                     // success
                     function () {
                         // back to image overview for this trip
                        window.location.href = '#/trips/' + vm.tripId + '/pictures';              
                     },
                     // failure
                     null
                     );
            };
             fileReader.readAsDataURL(vm.pictureFile);
             
        };
    }



}());
