var adminPageController = function ($scope, $http, $window, $uibModal) {
    $scope.courses = [];
    $scope.createCourseModalTemplate = '';
    $scope.removeCourseModalTemplate = '';
    $scope.showCreateCourseModal = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            template: $scope.createCourseModalTemplate,
            controller: 'adminPageModalController',
            //size: size,
            resolve: {
                courseId: function () {
                    return null;
                }
            }
        });
    };
};