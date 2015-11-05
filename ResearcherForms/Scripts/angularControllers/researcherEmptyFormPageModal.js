﻿var researcherEmptyFormPageModal = function ($scope, $location, $uibModalInstance, $window, $http) {
    $scope.alertsFormModal = [];

    $scope.initDateTimePicker = function () {
            $('#datetimepicker1').datetimepicker();
    }

    //$uibModalInstance.opened.then(function () {
    //    $('#datetimepicker1').datetimepicker();
    //});

    $scope.ok = function () {

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
            $window.location.href = $scope.basicUrl + '/Researcher/GetFormHistory/?formId=' + $scope.formModel.formId;
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
                    value: field.options[i].isSelected || field.options[i].isSelected == false
                        ? false
                        : true
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