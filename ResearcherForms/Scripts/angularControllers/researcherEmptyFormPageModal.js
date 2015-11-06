var researcherEmptyFormPageModal = function ($scope, $location, $uibModalInstance, $window, $http, isGotoAfter) {

    $scope.isGotoAfter = isGotoAfter;
    $scope.alertsFormModal = [];

    $scope.initDateTimePicker = function () {
            $('#datetimepicker1').datetimepicker();
    }


    $scope.ok = function () {

        for (var i = 0; i < $scope.formModel.fields.length; i += 1) {
            if ($scope.formModel.fields[i]['required'] === true
                && (!$scope.formModel.fields[i]['dataValue']
                    || $scope.formModel.fields[i]['dataValue'] == '')
                && $scope.formModel.fields[i]['fieldType'] != 'checkbox-group'
                && $scope.formModel.fields[i]['fieldType'] != 'checkbox'
                ) {
                $scope.alertsFormModal[0] = { type: 'danger', msg: 'Please fill all mandatory fields!' };
                return;
            }
        }

        formFieldsData = [];
        for (var i = 0; i < $scope.formModel.fields.length; i += 1) {
            var fieldData = {
                fieldId: $scope.formModel.fields[i].id,
                fieldData: $scope.formModel.fields[i].dataValue,
                fieldType: $scope.formModel.fields[i].fieldType,
                options: $scope.getOptionsFromField($scope.formModel.fields[i])
            }
            formFieldsData.push(fieldData);
        }

        var formModel = {
            formId: $scope.formModel.formId,
            fieldsData: formFieldsData
        };

        $http({
            method: 'POST',
            url: '/Researcher/FillNewForm',
            headers: { 'Content-Type': 'application/json;' },
            data: {
                'formModel': JSON.stringify(formModel)
            }
        })
        .then(function (response) {
            if ($scope.isGotoAfter == true) {
                $window.location.href = $scope.basicUrl + '/Researcher/GetFormHistory/?formId=' + $scope.formModel.formId;
            }
            else {
                $uibModalInstance.close(response.data);
            }
        }, function (response) {
            $scope.alertsFormModal[0] = { type: 'danger', msg: response.statusText };
        });

    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
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