(function () {
    "use strict";

    angular
        .module("common.services")
        .factory("tokenContainer",
                  [tokenContainer])

    function tokenContainer() {
   
        var container = {
            token: ""
        };
         
        var setToken = function (token) {
            container.token = token;
        };

        var getToken = function () {
            if (container.token === "") {
                if (localStorage.getItem("access_token") === null) {
 
                }
                else {
                    // set the token in localstorage
                    setToken(localStorage["access_token"]);
                }
            }
            return container;
        };

        // return value to call when calling the API 
        return {
            getToken: getToken
        };
        

    };

})();