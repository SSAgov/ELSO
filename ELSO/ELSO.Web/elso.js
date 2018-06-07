//created a module
var ElsoApp = angular.module('ElsoApp', [])

//Creating a Controller
ElsoApp.controller('ElsoController', function ($scope, ElsoService) {

   $scope.SendData = function () {
        // use $.param jQuery function to serialize data from JSON 
        var data = JSON.stringify( {
            EventName: $scope.EventName,
            Location: $scope.Location,
            EventStartDate: $scope.EventStartDate,
            EventEndDate: $scope.EventEndDate,
        });

        var config = {
            headers: {
                'Content-Type': 'application/json'
            }
        }

        ElsoService.createEvent(data,config)
       .success(function (data, status, headers, config) {
           $scope.Post = data;
           GetAll();
       })
         .error(function (data, status, header, config) {
             $scope.ResponseDetails = "Data: " + data +
                 "<hr />status: " + status +
                 "<hr />headers: " + header +
                 "<hr />config: " + config;
         });

    }


   GetAll();
    function GetAll() {
        ElsoService.getAll()
            .success(function (studs) {
                $scope.meetings = studs;
               console.log($scope.meetings);
            })
            .error(function (error) {
                $scope.status = 'Unable to load customer data: ' + error.message;
                console.log($scope.status);
           });
    }

    });


//Creating a Service
ElsoApp.factory('ElsoService', ['$http', function ($http) {
    var ElsoService = {};
   ElsoService.getAll = function () {
        return $http.get('/API/Events');
    };

 ElsoService.createEvent= function(data,config){
     return $http.post('/API/Events/Post', data, config)
  
    };
        
     return ElsoService;

 
}]);


