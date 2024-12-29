function getWSAddress() {
    let ishttps = 'https:' === document.location.protocol ? true : false;
    let host = window.location.host;
    let head = ishttps ? "wss://" : "ws://";
    return head + host;
}

let webSocket;
let wsMaxLag = 0;

let startWsTest = function () {
    //prepare
    let wsStartTime = new Date();
    webSocket = new WebSocket(getWSAddress() + "/Admin/Overview/Pushing");
    webSocket.onopen = function () {
        $("#spanStatus").text("connected");
    };
    webSocket.onmessage = function (evt) {
        //show message
        $("#spanStatus").html('Server Time: ' + evt.data.split('|')[0]);
        //get time
        let wslag = new Date() - wsStartTime;
        wsStartTime = new Date();
        //update max
        if (wslag > wsMaxLag) {
            wsMaxLag = wslag;
        }
        //log
        if (wslag > $('#wslagfilter').val()) {
            trig('WebSocket', wslag + 'ms');
        }
        //update view
        $('#wsStatus').html('Current: ' + wslag + 'ms');
        $("#wsmax").html('Max: ' + wsMaxLag + 'ms');

        if (wsChartData.labels.length > 50) {
            wsChartData.labels.shift();
            wsChartData.datasets[0].data.shift();
        }
        wsChartData.labels.push('');
        wsChartData.datasets[0].data.push(wslag);
        window.wsLine.update();
    };
    webSocket.onclose = function () {
        $("#spanStatus").text("WebSocket Disconnected");
    };
};

let stopWsTest = function () {
    if (webSocket) {
        webSocket.close();
    }
};
