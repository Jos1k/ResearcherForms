var researcherManageFormsInCoursePageController = function ($scope, $http, $window, $uibModal) {
    $scope.alertsNewForm = [];
    $scope.isFormActivitySelected = false;
    $scope.showResearcherEmptyFormModal = function (formId) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/Researcher/GetEmptyFormForResearcher/?formId=' + formId,
            controller: 'researcherEmptyFormPageModal',
            size: "newFormModal",
        });
    };

    $scope.gotoManageFormActivity = function (formId) {
        $window.location.href = $scope.basicUrl + '/Researcher/GetFormHistory/?formId=' + formId;
    };
    
    $scope.getFormWithData = function (formFieldId) {
        $scope.isFormActivitySelected = true;
       // $http({
       //     method: 'POST',
       //     url: '/Researcher/GetFormHistory',
       //     headers: { 'Content-Type': 'application/json;' },
       //     data: {
       //         'formModel': JSON.stringify(formModel)
       //     }
       // })
       //.then(function (response) {
       //    $window.location.href = $scope.basicUrl + '/Researcher/GetFormHistory/?formId=' + $scope.formModel.formId;
       //}, function (response) {
       //    $scope.alertsFormModal[0] = { type: 'danger', msg: response.statusText };
       //});
    };

    $scope.getIndexOfArrayByProperty = function findWithAttr(array, attr, value) {
        for (var i = 0; i < array.length; i += 1) {
            if (array[i][attr] === value) {
                return i;
            }
        }
    }
};