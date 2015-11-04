var researcherManageFormActivityPageController = function ($scope, $http, $window, $uibModal) {
    $scope.alertsNewForm = [];

    //$scope.getDateAsString = function (dateField) {
    //    return dateField.toString();
    //};
    
    $scope.getDateInString = function (fieldFormDate) {
        var dateString = dateFormat(fieldFormDate, "dddd, mmmm dS, yyyy");
        return dateString;
    };

    $scope.showResearcherEmptyFormModal = function (formId) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/Researcher/GetEmptyFormForResearcher/?formId=' + formId,
            controller: 'researcherEmptyFormPageModal',
            size: "newFormModal",
        });
    };
    
    $scope.goBackToFormsInCourse = function (courseId) {
        $window.location.href = $scope.basicUrl + '/Researcher/GetCourseForms/?courseId=' + courseId;
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