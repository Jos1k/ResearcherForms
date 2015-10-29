var adminChangeCourseNamePageModalController = function ($scope, $location, $uibModalInstance, $window, $http, courseName) {
    $scope.alerts = [];
    $scope.courseName = courseName;
    $scope.ok = function () {
        $http({
            method: 'POST',
            url: '/Admin/UpdateCourseName',
            headers: { 'Content-Type': 'application/json;' },
            data: {
                'courseName': $scope.courseName,
                'courseId': $scope.courseId
            }
        }).
        then(function (response) {
            $uibModalInstance.close($scope.courseName);
        }, function (response) {
            $scope.alerts[0] = { type: 'danger', msg: response.statusText };
        });
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.courseNameCheck = function () {
        if ($scope.courseName
            && $scope.courseName.length > 3
            && $scope.courseName.length < 25
        ) {
            return false;
        }
        else {
            return true;
        }
    }
};