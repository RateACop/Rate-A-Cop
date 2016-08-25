var app = angular.module('myapp', ['angucomplete-alt']); //add angucomplete-alt dependency in app
app.controller('AutoCompleteController', ['$scope', '$http', function ($scope, $http) {
    $scope.Officers = [];
    $scope.SelectedOfficers = null;
    //event fires when click on textbox
    $scope.SelectedOfficer = function (selected) {
        if (selected) {
            $scope.SelectedOfficer = selected.originalObject;
        }
    }
    //Gets data from the Database
    $http({
        method: 'GET',
        url: '/Home/getAllOfficers'
    }).then(function (data) {
        $scope.Officers = data.data;
    }, function () {
        alert('Error');
    })
}]);