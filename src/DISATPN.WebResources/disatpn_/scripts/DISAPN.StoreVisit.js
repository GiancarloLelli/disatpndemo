if (typeof (DISAPN) == "undefined") { DISAPN = { __namespace: true }; }
if (typeof (DISAPN.StoreVisit) == "undefined") { DISAPN.StoreVisit = { __namespace: true }; }

DISAPN.StoreVisit.Main = new function () {
    var _self = this;

    _self.Xrm = parent.Xrm != null ? parent.Xrm : null;
    _self.Canvas = null;
    _self.FixedPoints = [];

    _self.OnLoad = function () {
        _self.Canvas = document.getElementById("storeCanvas");
        _self.Canvas.addEventListener('click', _self.DebugLocation, false);
        _self.LoadFixedPoints();
        _self.DrawFixedPoints();
        _self.LoadLastRoute();
    }

    _self.DebugLocation = function (event) {
        var rect = _self.Canvas.getBoundingClientRect();

        var coords = {
            x: event.clientX - rect.left,
            y: event.clientY - rect.top
        };

        console.log("[DEBUG-MOUSECLICK] => X:" + coords.x + " - Y:" + coords.y);
    }

    _self.LoadFixedPoints = function () {
        _self.FixedPoints.push({ X: 106, Y: 566, Id: "L0", DeviceId: "BED8218D-8AE9-4EDE-BBD9-32942F1C6348" });
        _self.FixedPoints.push({ X: 106, Y: 232, Id: "L1", DeviceId: "77D09A9E-48E7-4B53-9285-A214651903DB" });
        _self.FixedPoints.push({ X: 690, Y: 216, Id: "R0", DeviceId: "2DE6F734-D134-45DD-827B-F5E06A4F09E1" });
        _self.FixedPoints.push({ X: 716, Y: 578, Id: "R1", DeviceId: "EA6CA285-9B85-4BDD-A608-A09087D04BA7" });
    }

    _self.DrawFixedPoints = function () {
        var ctx = _self.Canvas.getContext("2d");
        ctx.fillStyle = "#000";
        ctx.font = "20px Georgia"
        for (var i = 0; i < _self.FixedPoints.length; i++) {
            var pointItem = _self.FixedPoints[i];
            ctx.fillRect(pointItem.X, pointItem.Y, 20, 20);
            ctx.fillText(pointItem.Id, pointItem.X - 1, pointItem.Y - 3);
        }
    }

    _self.DrawRoute = function (data) {
        if (data.paths.length === 0)
            return;

        var helpers = DISAPN.Helpers;
        var ctx = _self.Canvas.getContext("2d");
        var startingPoint = data.paths[0];

        // Draw small rect to mark starting point
        ctx.fillStyle = "#FF0000";
        ctx.fillRect(startingPoint.X, startingPoint.Y, 10, 10);

        for (var i = 0; i < data.paths.length; i++) {
            var fromDevice = data.paths[i];
            var toDevice = (i + 1) == data.paths.length ? -1 : i + 1;

            var from = _self.FixedPoints.filter(p => p.DeviceId === fromDevice.deviceId.toUpperCase());
            var to = _self.FixedPoints.filter(p => p.DeviceId === toDevice.deviceId.toUpperCase());

            helpers.Arrowfactory(ctx, from.X, from.Y, to.X, to.Y);
        }
    }

    _self.LoadLastRoute = function () {
        if (_self.Xrm != null) {
            var userId = _self.Xrm.Page.data.entity.getId().replace('{', '').replace('}', '');
            var url = "https://disatpndcx.azurewebsites.net/api/Visit/" + userId;
            $.get(url, _self.DrawRoute);
        }
    }
}