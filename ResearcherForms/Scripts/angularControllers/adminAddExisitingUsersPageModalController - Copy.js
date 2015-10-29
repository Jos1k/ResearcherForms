var adminAddExisitingUsersPageModalController = function ($scope, $location, $uibModalInstance, $window, $http, researchers) {

    $scope.researchers = researchers;
    $scope.alerts = [];
    $scope.ok = function () {

        $http({
            method: 'POST',
            url: '/Admin/AddExisitingUsersToCourse',
            headers: { 'Content-Type': 'application/json;' },
            data: {
                'userIds': $scope.selectedUsers(false),
                'courseId': $scope.courseId
            }
        }).
        then(function (response) {
            $uibModalInstance.close($scope.selectedUsers(true));
        }, function (response) {
            $scope.alerts[0] = { type: 'danger', msg: response.statusText };
        });
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.isNotSelectedResearchers = function () {
        if ($.grep($scope.researchers, function (e) { return e.isSelected == true; }).length > 0) {
            return false;
        }
        else {
            return true;
        }
    };

    $scope.selectedUsers = function (fullObjects) {
        var selectedUsers = $.grep($scope.researchers, function (e) { return e.isSelected == true; });
        var selectedUserIds = [];
        if (fullObjects == true) {
            selectedUsers.forEach(function (user) { selectedUserIds.push({ 'id': user.id, 'name': user.name }) });
        }
        else {
            selectedUsers.forEach(function (user) { selectedUserIds.push(user.id) });
        }
        return selectedUserIds;
    };

};