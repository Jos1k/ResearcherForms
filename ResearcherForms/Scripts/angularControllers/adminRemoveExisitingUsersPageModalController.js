var adminRemoveExisitingUsersPageModalController = function ($scope, $location, $uibModalInstance, $window, $http, researchers) {

    $scope.researchers = researchers;
    $scope.ok = function () {
        $http({
            method: 'POST',
            url: '/Admin/RemoveExisitingUsersFromCourse',
            headers: { 'Content-Type': 'application/json;' },
            data: {
                'userIds': $scope.researchers,
                'courseId': $scope.courseId
            }
        }).
        then(function (response) {
            $uibModalInstance.close();
        }, function (response) {
            $uibModalInstance.dismiss('cancel');
        });
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
};