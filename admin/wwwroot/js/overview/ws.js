'use strict';
function getWSAddress() {
    var ishttps = 'https:' === document.location.protocol ? true : false;
    var host = window.location.host;
    var head = ishttps ? "wss://" : "ws://";
    return head + host;
}
var webSocket;
var wsMaxLag = 0;
var wsOrder = 0;
var WsTest = function () {
    //thread safe
    if ($('#wsbutton').attr('disabled') === 'disabled') {
        return;
    }
    $('#wsbutton').attr('disabled', 'disabled');
    startWsTest();
};
var startWsTest = function () {
    //prepare
    var wsStartTime = new Date();
    wsOrder = 0;
    webSocket = new WebSocket(getWSAddress() + "/admin/Overview/Pushing");
    webSocket.onopen = function () {
        $("#spanStatus").text("connected");
    };
    webSocket.onmessage = function (evt) {
        //show message
        var order = Number(evt.data.split('|')[1]);
        $("#spanStatus").html('Server Time: ' + evt.data.split('|')[0] + '  Message Order: ' + order);
        //get time
        var wslag = new Date() - wsStartTime;
        wsStartTime = new Date();
        //update max
        if (wslag > wsMaxLag) {
            wsMaxLag = wslag;
        }
        //log
        if (wslag > $('#wslagfilter').val()) {
            trig('WebSocket', wslag + 'ms');
        }
        // check order
        if (order !== wsOrder + 1) {
            trig('WebSocket', 'Event Not constant! prev:' + wsOrder + ' current:' + order);
        }
        wsOrder = order;
        //update view
        $('#wsStatus').html('Current: ' + wslag + 'ms');
        $("#wsmax").html('Max: ' + wsMaxLag + 'ms');

        if (wschartData.labels.length > 200) {
            wschartData.labels.shift();
            wschartData.datasets[0].data.shift();
        }
        wschartData.labels.push('');
        wschartData.datasets[0].data.push(wslag);
        window.myWSLine.update();
    };
    webSocket.onerror = function (evt) {
        alert(evt.message);
    };
    webSocket.onclose = function () {
        $("#spanStatus").text("disconnected");
    };
};

var stopWsTest = function () {
    if (webSocket) {
        webSocket.close();
    }
    $('#wsbutton').removeAttr('disabled', 'disabled');
};