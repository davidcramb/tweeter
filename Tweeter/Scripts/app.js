var app = angular.module("Tweeter", []);

app.controller("FormCtrl", ["$scope", "$http", function ($scope, $http) {

    $scope.welcome = 'hi';

    $scope.formSubmit = (event, user) => {
        event.preventDefault();
        var data = $("form").serialize();
        $http.post('/Account/Register', data)
        .then(console.log(data));
    }

}]);
