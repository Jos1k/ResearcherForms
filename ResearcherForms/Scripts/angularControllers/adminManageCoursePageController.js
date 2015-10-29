var adminManageCoursePageController = function ($scope, $http, $window, $uibModal) {

    $scope.showAddExisitingUserModal = function () {
        $http({
            method: 'POST',
            url: '/Admin/AddExisitingUsersToCourseGetUsers',
            headers: { 'Content-Type': 'application/json;' },
            params: { 'courseId': $scope.course.id },
            data: ''
        })
        .then(function (response) {
            var modalInstance = $uibModal.open({
                animation: true,
                template: $scope.addExistingUserModalTemplate,
                controller: 'adminAddExisitingUsersPageModalController',
                //size: "editCompany",
                resolve: {
                    researchers: function () {
                        return JSON.parse(response.data);
                    }
                }
            });

            modalInstance.result.then(function (addedUsers) {
                addedUsers.forEach(function (addedUser) {
                    $scope.course.researchers.push(addedUser);
                });
                modalInstance.close();
            }, function (response) {
                //$window.alert(response.statusText);
            });
        }, function (response) {
            $window.alert(response.statusText);
        });

        //$uibModalInstance.close();
    };

    $scope.showRemoveExisitingUserModal = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            template: $scope.removeExistingUserModalTemplate,
            controller: 'adminRemoveExisitingUsersPageModalController',
            //size: "editCompany",
            resolve: {
                researchers: function () {
                    return $scope.selectedUsers(false);
                }
            }
        });

        modalInstance.result.then(function () {
            $scope.selectedUsers(true).forEach(function (removedUser) {
                var userIndex = $scope.getIndexOfArrayByProperty($scope.course.researchers, 'id', removedUser.id);
                $scope.course.researchers.splice(userIndex, 1);
            });
            modalInstance.close();
        }, function (response) {
            //$window.alert(response.statusText);
        });
        //$uibModalInstance.close();
    };

    $scope.selectedUsers = function (fullObjects) {
        var selectedUsers = $.grep($scope.course.researchers, function (e) { return e.isSelected == true; });
        var selectedUserIds = [];
        if (fullObjects == true) {
            selectedUsers.forEach(function (user) { selectedUserIds.push({ 'id': user.id, 'name': user.name }) });
        }
        else {
            selectedUsers.forEach(function (user) { selectedUserIds.push(user.id) });
        }
        return selectedUserIds;
    };

    $scope.getIndexOfArrayByProperty = function findWithAttr(array, attr, value) {
        for (var i = 0; i < array.length; i += 1) {
            if (array[i][attr] === value) {
                return i;
            }
        }
    }
};