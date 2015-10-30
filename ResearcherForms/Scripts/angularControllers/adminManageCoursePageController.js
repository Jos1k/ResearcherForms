var adminManageCoursePageController = function ($scope, $http, $window, $uibModal) {

    $scope.alertsNewForm = [];
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
        $scope.alertsNewForm = [];
    };

    $scope.validationOnNewFormModalIsDisabled = function () {
        if ($scope.formName && $scope.formName.length > 5) {
            return false;
        }
        return true;
    };
    //&& $("#formBuilder").val().length > 0

    $scope.addNewForm = function () {
        if ($("#formBuilder").val().length == 0) {
            $scope.alertsNewForm[0] = { type: 'danger', msg: 'Form should not be empty!' };
            return;
        }

        $http({
            method: 'POST',
            url: '/Admin/AddNewForm',
            headers: { 'Content-Type': 'application/json;' },
            data: {
                'courseId': $scope.course.id,
                'formName': $scope.formName,
                'formBody': $("#formBuilder").val()
            }
        })
        .then(function (response) {
            $scope.course.forms.push(JSON.parse(response.data));
            $scope.formModalCancel();
        }, function (response) {
            $scope.alertsNewForm[0] = { type: 'danger', msg: response.statusText };
        });
    };

    $scope.showRemoveFormsModal = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            template: $scope.removeExistingFormsModalTemplate,
            controller: 'adminRemoveExisitingFormsPageModalController',
            //size: "editCompany",
            resolve: {
                forms: function () {
                    return $scope.selectedForms(false);
                }
            }
        });

        modalInstance.result.then(function () {
            $scope.selectedForms(true).forEach(function (removedForm) {
                var formIndex = $scope.getIndexOfArrayByProperty($scope.course.forms, 'id', removedForm.id);
                $scope.course.forms.splice(formIndex, 1);
            });
            modalInstance.close();
        }, function (response) {
            //$window.alert(response.statusText);
        });
        //$uibModalInstance.close();
    };


    $scope.showAddNewFormModal = function () {
        $('#myModal').modal('show');
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

    $scope.selectedForms = function (fullObjects) {
        var selectedForms = $.grep($scope.course.forms, function (e) { return e.isSelected == true; });
        var selectedFormIds = [];
        if (fullObjects == true) {
            selectedForms.forEach(function (form) { selectedFormIds.push({ 'id': form.id, 'name': form.name }) });
        }
        else {
            selectedForms.forEach(function (form) { selectedFormIds.push(form.id) });
        }
        return selectedFormIds;
    };

    $scope.getIndexOfArrayByProperty = function findWithAttr(array, attr, value) {
        for (var i = 0; i < array.length; i += 1) {
            if (array[i][attr] === value) {
                return i;
            }
        }
    }
};