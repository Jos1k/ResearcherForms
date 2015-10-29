var adminManageCoursePageController = function ($scope, $http, $window, $uibModal) {

    $scope.showAddExisitingUserModal = function () {
        $http({
            method: 'POST',
            url: '/Admin/AddExisitingUsersToCourseGetUsers',
            headers: { 'Content-Type': 'application/json;' },
            params: {'courseId': $scope.course.id},
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
                addedUsers.each(function () {
                    $scope.course.researchers.push(this);
                });
                modalInstance.close();
            }, function (response) {
                $window.alert(response.statusText);
            });
        }, function (response) {
            $window.alert(response.statusText);
        });

        //$uibModalInstance.close();
    };
};