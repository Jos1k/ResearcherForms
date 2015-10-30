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

    $scope.showChangeCourseNameModal = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            template: $scope.changeCourseNameModalTemplate,
            controller: 'adminChangeCourseNamePageModalController',
            //size: "editCompany",
            resolve: {
                courseName: function () {
                    return $scope.course.name;
                }
            }
        });

        modalInstance.result.then(function (courseName) {
            $scope.course.name = courseName;
            modalInstance.close();
        }, function (response) {
            //$window.alert(response.statusText);
        });
        //$uibModalInstance.close();
    };

    $scope.showAddNewUserModal = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            template: $scope.addNewUserToCourserModalTemplate,
            controller: 'adminAddNewUserPageModalController',
            //size: "editCompany",
        });

        modalInstance.result.then(function (newUser) {
            $scope.course.researchers.push(newUser);
            modalInstance.close();
        }, function (response) {
            //$window.alert(response.statusText);
        });
        //$uibModalInstance.close();
    };


    $scope.formModalCancel = function () {
        $('#myModal').modal('hide');
        $("#formBuilder").val('');
        $("#frmb-0").empty();
        $scope.formName = '';
    };

    $scope.showAddNewFormModal = function () {

        $('#myModal').modal('show');

        //var modalInstance = $uibModal.open({
        //    animation: true,
        //    template: $scope.addNewFormToCourserModalTemplate,
        //    controller: 'adminAddNewFormPageModalController',
        //    size: "addForm",
        //});

        //modalInstance.result.then(function (newForm) {
        //    $scope.course.forms.push(newForm);
        //    modalInstance.close();
        //}, function (response) {
        //    //$window.alert(response.statusText);
        //});
        ////$uibModalInstance.close();
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