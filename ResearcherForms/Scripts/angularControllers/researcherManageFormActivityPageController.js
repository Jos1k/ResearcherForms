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
         $http({
             method: 'POST',
             url: '/Researcher/GetFormFieldHistory',
             headers: { 'Content-Type': 'application/json;' },
             data: {
                 'formFieldId': formFieldId
             }
         })
        .then(function (response) {
            
        }, function (response) {
            $scope.isFormActivitySelected = false;
        });
    };

    $scope.getIndexOfArrayByProperty = function findWithAttr(array, attr, value) {
        for (var i = 0; i < array.length; i += 1) {
            if (array[i][attr] === value) {
                return i;
            }
        }
    }
};