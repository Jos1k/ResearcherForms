var adminPageModalController = function ($scope, $location, $uibModalInstance, $window, $http, courseId) {
    $scope.alerts = [];
    $scope.ok = function () {

        $http({
            method: 'POST',
            url: '/Admin/CreateCourse',
            headers: { 'Content-Type': 'application/json;' },
            data: {
                'name': $scope.courseName
            }
        }).
        then(function (response) {
            $window.location.href = response.data;
        }, function (response) {
            $scope.alerts[0] = { type: 'danger', msg: response.statusText };
        });

        //$uibModalInstance.close();
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
};