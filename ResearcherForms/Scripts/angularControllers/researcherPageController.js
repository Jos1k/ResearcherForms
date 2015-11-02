var researcherPageController = function ($scope, $http, $window, $uibModal) {
    $scope.basicUrl = '';
    $scope.courses = [];

    $scope.gotoManageCoursePage = function (courseId) {
        $window.location.href = $scope.basicUrl + '/Researcher/GetCourseForms/?courseId=' + courseId;
    };

    $scope.getIndexOfArrayByProperty = function findWithAttr(array, attr, value) {
        for (var i = 0; i < array.length; i += 1) {
            if (array[i][attr] === value) {
                return i;
            }
        }
    };
};