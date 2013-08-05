define(function () {
    'use strict';

    return ['$scope', '$i18next', 'groupData', 'existingGroups',
    function ($scope, $i18next, groupData, existingGroups) {

            var blankGroup = {
                id: "",
                name: "",
                email: "",
                color: "#FFFFFF",
                parentId: "",
                children: []
            };

            var init = function () {
                $scope.newGroup = angular.copy(blankGroup);
                $scope.rootGroups = [];
                
                existingGroups.$then(function (result) {
                    var groups = result.data;
                    buildGroupTree(groups);
                });

                $scope.$apply();
            };

            var buildGroupTree = function (groups) {
                for (var i = 0; i < groups.length; i++) {
                    if (groups[i].parentId == undefined || groups[i].parentId == "") {
                        var currentParentGroup = groups[i];
                        for (var j = 0; j < groups.length; j++) {
                            if (groups[j].parentId == currentParentGroup.id) {
                                currentParentGroup.children.push(groups[j]);
                            }
                        }
                        $scope.rootGroups.push(currentParentGroup);
                    }
                }
            };

            $scope.edit = function (group) {
                $scope.memeGroup = angular.copy(group);
                group.editing = true;
            };

            $scope.cancelEdit = function (form, group) {
                group.editing = false;
                group = angular.copy($scope.memeGroup);
            };

            $scope.canDelete = function (group) {
                group.canDelete = true;
            };

            $scope.delete = function ($index, group) {
                groupData.delete(group.id).$then(
                    function () {
                        $scope.rootGroups.splice($index, 1);
                    }, callbackError);
            };

            var resetFormNewGroup = function () {
                $scope.newGroup = angular.copy(blankGroup);
            }

            $scope.insert = function (formNewGroup) {
                if (formNewGroup.$valid) {
                    var newGroup = angular.copy($scope.newGroup);
                    groupData.insert(newGroup)
                        .then(function (response) {
                            resetFormNewGroup();
                            newGroup.id = response.id;
                            $scope.rootGroups.push(newGroup);
                            saveSuccess();
                        },
                        callbackError);
                }
            }

            $scope.update = function (formEdit, group) {
                if (formEdit.$valid) {
                    groupData.update(group).then(function (response) {
                        group.editing = false;
                        saveSuccess();
                    }, callbackError);
                }
            }

            $scope.insertChild = function (formNewChildGroup, newChildGroup, parentGroup) {
                if (formNewChildGroup.$valid) {
                    var newGroup = angular.copy(newChildGroup);
                    newGroup.parentId = parentGroup.id;
                    groupData.insert(newGroup)
                        .then(function (response) {
                            newGroup.id = response.id;
                            parentGroup.children.push(newGroup);
                            saveSuccess();
                        },
                        callbackError);
                }
            }

            $scope.updateChild = function (formEdit, group, parentGroup) {
                if (formEdit.$valid) {
                    group.parentId = parentGroup.id;
                    groupData.update(group).then(function (response) {
                        group.editing = false;
                        saveSuccess();
                    }, callbackError);
                }
            }

            $scope.deleteChild = function ($index, group, parent) {
                groupData.delete(group.id).$then(
                    function () {
                        parent.children.splice($index, 1);
                    }, callbackError);
            };

            var saveSuccess = function () {
                toastr.success($i18next("groupForm.saveSuccess"));
            }

            var callbackError = function (result) {
                toastr.error(result.data.error);
            }

            init();
        }
    ];
});