//created a module
var ElsoApp = angular.module('ElsoApp', []);

//Creating a Controller
ElsoApp.controller('ElsoController', function ($scope, ApiCall) {

    ApiCall.getAll().success(function (studs) {
        $scope.meetings = studs;
        console.log($scope.meetings);
    })
          .error(function (error) {
              $scope.status = 'Unable to load customer data: ' + error.message;
              console.log($scope.status);
          });

    $scope.SendData = function () {
        // use $.param jQuery function to serialize data from JSON 
        var data = JSON.stringify({
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
   
        ApiCall.postEvent(data, config).success(function (data, status, headers, config) {
          // $scope.Post = data;
           })
         .error(function (data, status, header, config) {
             $scope.ResponseDetails = "Data: " + data +
                 "<hr />status: " + status +
                 "<hr />headers: " + header +
                 "<hr />config: " + config;
         });

    }

});