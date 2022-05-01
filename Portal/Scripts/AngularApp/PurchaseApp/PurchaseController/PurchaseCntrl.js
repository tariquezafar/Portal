angular.module('purchase').controller('purchaseController', function ($scope, $http, Notification) {


    $scope.MdPendingPoCount = function () {
        var checkValue = $scope.TotalCount;
        var req = {
            method: 'POST',
            url: '../Dashboard/GetPendingPOCountList',
            headers: {
                'Content-Type': undefined
            },

        }
        $http(req).success(function (data, status) {
            if (data) {
                if (checkValue != data) {
                    $scope.TotalCount = data;
                    Notification.success({ message: '<a href="/PO/ListApprovedPO" target="_blank">Pending MD PO Approval Count : </a>' + data, positionY: 'bottom', positionX: 'right' });

                }
            }
        }).error(function (data, status) {
            //alert(status);
        });
    };

    setInterval(function () { $scope.MdPendingPoCount(); }, 3000);
});
