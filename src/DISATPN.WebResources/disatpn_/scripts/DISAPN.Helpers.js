if (typeof (DISAPN) == "undefined") { DISAPN = { __namespace: true }; }
if (typeof (DISAPN.Helpers) == "undefined") { DISAPN.Helpers = { __namespace: true }; }

DISAPN.Helpers = new function () {
    var _self = this;

    _self.ArrowFactory = function (context, fromx, fromy, tox, toy) {
        var headlen = 10;
        var angle = Math.atan2(toy - fromy, tox - fromx);

        context.strokeStyle = '#458B74';
        context.lineWidth = 5;

        context.moveTo(fromx, fromy);
        context.lineTo(tox, toy);
        context.lineTo(tox - headlen * Math.cos(angle - Math.PI / 6), toy - headlen * Math.sin(angle - Math.PI / 6));
        context.moveTo(tox, toy);
        context.lineTo(tox - headlen * Math.cos(angle + Math.PI / 6), toy - headlen * Math.sin(angle + Math.PI / 6));
    }
}