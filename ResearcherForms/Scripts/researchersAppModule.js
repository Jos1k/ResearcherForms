var researchersAppModule = angular.module('researchersAppModule', ['ui.bootstrap']);
researchersAppModule.controller('adminPageController', adminPageController);
researchersAppModule.controller('adminPageModalController', adminPageModalController);
researchersAppModule.controller('adminManageCoursePageController', adminManageCoursePageController);
researchersAppModule.controller('adminAddExisitingUsersPageModalController', adminAddExisitingUsersPageModalController);
researchersAppModule.controller('adminRemoveExisitingUsersPageModalController', adminRemoveExisitingUsersPageModalController);
researchersAppModule.controller('adminChangeCourseNamePageModalController', adminChangeCourseNamePageModalController);
researchersAppModule.controller('adminAddNewUserPageModalController', adminAddNewUserPageModalController);
researchersAppModule.controller('adminRemoveExisitingFormsPageModalController', adminRemoveExisitingFormsPageModalController);
researchersAppModule.controller('researcherPageController', researcherPageController);
researchersAppModule.controller('researcherManageFormsInCoursePageController', researcherManageFormsInCoursePageController);
researchersAppModule.controller('researcherEmptyFormPageModal', researcherEmptyFormPageModal);
researchersAppModule.controller('researcherManageFormActivityPageController', researcherManageFormActivityPageController);
researchersAppModule.controller('researcherAnalyticFormPageModal', researcherAnalyticFormPageModal);


var dateTimePicker = function () {
    return {
        restrict: "A",
        require: "ngModel",
        link: function (scope, element, attrs, ngModelCtrl) {
            var parent = $(element).parent();
            var dtp = parent.datetimepicker({
                format: "LL",
                showTodayButton: true
            });
            dtp.on("dp.change", function (e) {
                ngModelCtrl.$setViewValue(moment(e.date).format("dddd, MMMM Do YYYY, h:mm:ss a"));
                scope.$apply();
            });
        }
    };
};

researchersAppModule.directive('dateTimePicker', dateTimePicker);
