var researcherManageFormsInCoursePageController = function ($scope, $http, $window, $uibModal) {
    $scope.alertsNewForm = [];

    $scope.showResearcherEmptyFormModal = function (formId) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/Researcher/GetEmptyFormForResearcher/?formId=' + formId,
            controller: 'researcherEmptyFormPageModal',
            size: "newFormModal",
            //resolve: {
            //    researchers: function () {
            //        return $scope.selectedUsers(false);
            //    }
            //}
        });

        //modalInstance.result.then(function () {
        //    $scope.selectedUsers(true).forEach(function (removedUser) {
        //        var userIndex = $scope.getIndexOfArrayByProperty($scope.course.researchers, 'id', removedUser.id);
        //        $scope.course.researchers.splice(userIndex, 1);
        //    });
        //    modalInstance.close();
        //}, function (response) {
        //    //$window.alert(response.statusText);
        //});
        //$uibModalInstance.close();
    };
    

    
    $scope.getIndexOfArrayByProperty = function findWithAttr(array, attr, value) {
        for (var i = 0; i < array.length; i += 1) {
            if (array[i][attr] === value) {
                return i;
            }
        }
    }
};