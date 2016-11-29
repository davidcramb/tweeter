//$.ajax({
//    url: "/api/Tweet/",
//    method: 'GET'
//}).success(function (response) {
//    console.log(response);
//});
var app = angular.module("Tweeter", []);
app.controller("TweetCtrl", function ($scope, $http) {
    $scope.tweets = [
        {
            username: "Bob",
            message: "Hello",
            image: "",
            date: "Nov, 21 2016"
        },
        {
            username: "FloFromProgressive",
            message: "I'm annoying!",
            image: "http://placehold.it/350x150",
            date: "Nov, 21 2016"
        },
        {
            username: "JakeFromStateFarm",
            message: "Buy insurance",
            image: "http://placehold.it/350x150",
            date: "TIME IS AN ILLUSION"
        },
    ];
    $scope.running = false;

    $scope.getTweets = () => {
        if (!$scope.running) {
            $scope.running = true;
            $http.get("api/Tweet").success(function (response) {
                console.log(response);
                $scope.tweets = response;
                console.log($scope.tweets);
            }).error(function (error) {
                console.log(error);
            })
            return $scope.tweets;
        }
    }
})
console.log('hi');  
$("#register-username").keyup(function () {
    $("form").submit(true);
    $("#username-ans").removeClass("glyphicon-ok"); 
    $("#username-ans").removeClass("glyphicon-remove");
    $.ajax({
        url: "/api/TwitUsername?candidate=" + $(this).val(),
        method: 'GET'
    }).success(function (response) {
        console.log(response.exists);
        if (!response.exists) {
            $("#submit").removeAttr("disabled");
            $("#username-ans").addClass("glyphicon-ok");
        } else {
            $("#submit").attr("disabled", "disabled");
            $("#username-ans").addClass("glyphicon-remove");
        }
    }).fail(function (error) {
        console.log(error);
    });
});





/*
$("#register-username").focusout(function () {
    //alert("defocused!!!");
    //console.log($(this).val());
    //$("#username-ans").addClass("hidden");
    $.ajax({
        url: "/api/TwitUsername?candidate=" + $(this).val(),
        method: 'GET'
    }).success(function (response) {
        console.log(response);
        if (response.exists) {
            $("#username-ans").addClass("glyphicon-ok");
        } else {
            $("#username-ans").addClass("glyphicon-remove");
        }
    }).fail(function (error) {
        console.log(error);
    });
});

$("#register-username").focusin(function () {
    $("#username-ans").removeClass("glyphicon-ok");
    $("#username-ans").removeClass("glyphicon-remove");
});
*/