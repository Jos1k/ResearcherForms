var researcherManageFormActivityPageController = function ($scope, $http, $window, $uibModal) {
    $scope.alertsNewForm = [];

    //$scope.getDateAsString = function (dateField) {
    //    return dateField.toString();
    //};

    $scope.getDateInString = function (fieldFormDate) {
        var dateString = moment(fieldFormDate).format("dddd, MMMM Do YYYY, h:mm:ss a");
        return dateString;
    };

    $scope.showResearcherEmptyFormModal = function (formId) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/Researcher/GetEmptyFormForResearcher/?formId=' + formId,
            controller: 'researcherEmptyFormPageModal',
            size: "newFormModal",
            resolve: {
                isGotoAfter: false
            }
        });

        modalInstance.result.then(function (newFormActivity) {
            $scope.form.formActivity.splice(0, 0, JSON.parse(newFormActivity));

            modalInstance.close();
        }, function (response) {
            //$window.alert(response.statusText);
        });
    };

    $scope.showAnalyticFormModal = function (formId) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/Researcher/GetResearcherAnalytic/?formId=' + formId,
            controller: 'researcherEmptyFormPageModal'
            //size: "newFormModal"
        });

        modalInstance.result.then(function () {
            modalInstance.close();
        }, function (response) {
            //$window.alert(response.statusText);
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
           var fieldDataValues = JSON.parse(response.data);
           for (var i = 0; i < $scope.form.formModel.fields.length; i++) {
               var fielValueId = $scope.getIndexOfArrayByProperty(fieldDataValues, 'fieldId', $scope.form.formModel.fields[i].id);

               if ($scope.form.formModel.fields[i].fieldType == 'date') {
                   if (typeof fielValueId != 'undefined') {
                       $scope.form.formModel.fields[i].dataValue = new Date(fieldDataValues[fielValueId].value);
                   }
                   else {
                       $scope.form.formModel.fields[i].dataValue = null;
                   }
               }
               if ($scope.form.formModel.fields[i].fieldType == 'checkbox') {
                   if (typeof fielValueId != 'undefined') {
                       $scope.form.formModel.fields[i].dataValue = fieldDataValues[fielValueId].value.toLowerCase() == "true" ? true : false;
                   }
                   else {
                       $scope.form.formModel.fields[i].dataValue = false;
                   }
               }
               else if ($scope.form.formModel.fields[i].fieldType == 'checkbox-group') {
                   for (var j = 0; j < $scope.form.formModel.fields[i].options.length; j++) {
                       var fielValueId = $scope.getIndexOfArrayByProperty(fieldDataValues, 'fieldId', $scope.form.formModel.fields[i].options[j].id, true);
                       if (fielValueId) {
                           $scope.form.formModel.fields[i].options[j].isSelected = fieldDataValues[fielValueId].value.toLowerCase() == "true" ? true : false
                       }
                       else {
                           $scope.form.formModel.fields[i].options[j].isSelected = false
                       }
                   }
               }
               else {
                   if (typeof fielValueId != 'undefined') {
                       $scope.form.formModel.fields[i].dataValue = fieldDataValues[fielValueId].value;
                   }
                   else {
                       $scope.form.formModel.fields[i].dataValue = null;
                   }
               }
           }
       }, function (response) {
           $scope.isFormActivitySelected = false;
       });
    };



    $scope.getIndexOfArrayByProperty = function findWithAttr(array, attr, value, isOption) {
        for (var i = 0; i < array.length; i += 1) {
            if (isOption) {
                if (array[i][attr] === value && array[i]['isOption'] == true) {
                    return i;
                }
            }
            else if (array[i][attr] === value) {
                return i;
            }
        }
    }
};