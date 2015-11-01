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
    $scope.showRemoveCourseModal = function (courseId) {
        var modalInstance = $uibModal.open({
            animation: true,
            template: $scope.removeCourseModalTemplate,
            controller: 'adminPageModalController',
           // size: 'createCourse',
            resolve: {
                courseId: function () {
                    return courseId;
                }
            }
        });

        modalInstance.result.then(function () {
            var formIndex = $scope.getIndexOfArrayByProperty($scope.courses, 'Id', $scope.courseId);
            $scope.courses.splice(formIndex, 1);
            modalInstance.close();
        }, function (response) {
            //$window.alert(response.statusText);
        });
    };

    $scope.gotoManageCoursePage = function (courseId) {
        $window.location.href = $scope.basicUrl + '/Admin/GetCourseInfo/?courseId=' + courseId;
    }

    $scope.getIndexOfArrayByProperty = function findWithAttr(array, attr, value) {
        for (var i = 0; i < array.length; i += 1) {
            if (array[i][attr] === value) {
                return i;
            }
        }
    }
};