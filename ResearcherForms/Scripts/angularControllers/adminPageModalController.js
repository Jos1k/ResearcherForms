var adminPageModalController = function ($scope, $uibModalInstance, $window, $http, courseId) {

    $scope.ok = function () {
        //some http post request for creating course here
        $uibModalInstance.close();
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
};