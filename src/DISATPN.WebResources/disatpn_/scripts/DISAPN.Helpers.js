if (typeof (DISAPN) == "undefined") { DISAPN = { __namespace: true }; }
if (typeof (DISAPN.Helpers) == "undefined") { DISAPN.Helpers = { __namespace: true }; }

DISAPN.Helpers = new function () {
    var _self = this;

    _self.ArrowFactory = function (context, from, to, radius) {
        var x_center = to.X;
        var y_center = to.Y;

        var angle;
        var x;
        var y;

        // Line
        context.beginPath();
        context.lineWidth = 5;
        context.strokeStyle = "#5f9ea0";
        context.moveTo(from.X, from.Y + 7);
        context.lineTo(to.X, to.Y);
        context.stroke();

        // Arrow head
        context.beginPath();
        context.lineWidth = 10;
        context.fillStyle = "#7fff00";
        context.lineJoin = "round";

        angle = Math.atan2(to.Y - from.Y, to.X - from.X)
        x = radius * Math.cos(angle) + x_center;
        y = radius * Math.sin(angle) + y_center;

        context.moveTo(x, y);

        angle += (1.0 / 3.0) * (2 * Math.PI)
        x = radius * Math.cos(angle) + x_center;
        y = radius * Math.sin(angle) + y_center;

        context.lineTo(x, y);

        angle += (1.0 / 3.0) * (2 * Math.PI)
        x = radius * Math.cos(angle) + x_center;
        y = radius * Math.sin(angle) + y_center;

        context.lineTo(x, y);

        context.closePath();
        context.fill();
    }
}