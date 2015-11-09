var researcherAnalyticFormPageModal = function ($scope, $location, $uibModalInstance, $window, $http, formFields) {

    $scope.fields = formFields;

    $scope.ok = function () {
        $uibModalInstance.close();
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.getDateInString = function (fieldFormDate) {
        var dateString = moment(fieldFormDate).format("dddd, MMMM Do YYYY, h:mm:ss a");
        return dateString;
    };

    $scope.getOptionsFromField = function (field) {
        if (field.fieldType == 'checkbox-group') {
            var options = [];
            for (var i = 0; i < field.options.length; i += 1) {
                var option = {
                    optionId: field.options[i].id,
                    value: field.options[i].isSelected
                };
                options.push(option)
            }
            return options;
        }
        else {
            return null;
        }
    }
};