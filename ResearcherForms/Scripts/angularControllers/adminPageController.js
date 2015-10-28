var adminPageController = function ($scope, $http, $window, $uibModal) {
    $scope.basicUrl = '';
    $scope.courses = [];
    $scope.createCourseModalTemplate = '';
    $scope.removeCourseModalTemplate = '';
    $scope.showCreateCourseModal = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            template: $scope.createCourseModalTemplate,
            controller: 'adminPageModalController',
            size: 'createCourse',
            resolve: {
                courseId: function () {
                    return null;
                }
            }
        });
    };
    $scope.gotoManageCoursePage = function (courseId) {
        $window.location.href = $scope.basicUrl + '/Admin/GetCourseInfo/?courseId=' + courseId;
    }
};