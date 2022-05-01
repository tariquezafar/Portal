var purchaseapp = angular.module('purchase', ['']);
angular.module('purchase', ['ui-notification']).config(function (NotificationProvider) {
    NotificationProvider.setOptions({
        delay: 10000,

        positionX: 'left',
        positionY: 'bottom'
    });
});