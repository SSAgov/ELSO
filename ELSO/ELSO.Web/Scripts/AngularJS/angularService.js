ElsoApp.service('ApiCall',['$http', function($http){
    var result;
    // get from web api
    this.getAll = function () {
        result = $http.get('/API/Events');
          
            
        return result;
    };
    // post method web api
    this.postEvent = function (data, config) {
        return $http.post('/API/Events/Post', data, config);
        //return $http.post('/API/Events/Post', data, config);
    };
    }]);