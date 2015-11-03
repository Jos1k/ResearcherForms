var researcherManageFormsInCoursePageController = function ($scope, $http, $window, $uibModal) {
    $scope.alertsNewForm = [];

    $scope.showResearcherEmptyFormModal = function (formId) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/Researcher/GetEmptyFormForResearcher/?formId=' + formId,
            controller: 'researcherEmptyFormPageModal',
            size: "newFormModal",
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