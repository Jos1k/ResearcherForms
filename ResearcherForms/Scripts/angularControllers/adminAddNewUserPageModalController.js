var adminAddNewUserPageModalController = function ($scope, $location, $uibModalInstance, $window, $http) {
    $scope.alerts = [];
    $scope.ok = function () {
        if ($scope.userPassword != $scope.userPasswordConfirm) {
            $scope.alerts[0] = { type: 'danger', msg: 'Passwords are not equal' };
            return;
        }

        $http({
            method: 'POST',
            url: '/Account/CreateNewUserOnCourse',
            headers: { 'Content-Type': 'application/json;' },
            data: {
                'userName': $scope.userName,
                'userEmail': $scope.userEmail,
                'userPassword': $scope.userPassword,
                'courseId': $scope.courseId
            }
        }).
        then(function (response) {
            $uibModalInstance.close({ 'id': response.data, 'name': $scope.userName });
        }, function (response) {
            $scope.alerts[0] = { type: 'danger', msg: response.statusText };
        });
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.isAllFieldsNotEmpty = function () {
        if ($scope.userName
            && $scope.userEmail
            && $scope.userPassword
            && $scope.userPasswordConfirm
        ) {
            return false;
        }
        else {
            return true;
        }
    }
};