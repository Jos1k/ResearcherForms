var adminRemoveExisitingFormsPageModalController = function ($scope, $location, $uibModalInstance, $window, $http, forms) {

    $scope.forms = forms;
    $scope.ok = function () {
        $http({
            method: 'POST',
            url: '/Admin/RemoveResearchForm',
            headers: { 'Content-Type': 'application/json;' },
            data: {
                'formIds': $scope.forms,
                'courseId': $scope.courseId
            }
        }).
        then(function (response) {
            $uibModalInstance.close();
        }, function (response) {
            $uibModalInstance.dismiss('cancel');
        });
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
};