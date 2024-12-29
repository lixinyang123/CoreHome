let pingMaxlag = 0;
let pingStop = false;

let startPing = function () {
    if(pingStop) {
        return;
    }
    //prepare
    let startTime = new Date();
    $.get('/Admin/Overview/Ping', function (data) {
        //get time
        let endtime = new Date();
        let lag = endtime - startTime - 7;
        //update max value
        if (lag > pingMaxlag) {
            pingMaxlag = lag;
        }
        //log
        if (lag > $('#pinglagfilter').val()) {
            trig('HTTP Get', lag + 'ms');
        }
        //update view
        $('#httpStatus').html('Current: ' + lag + 'ms');
        $('#httpMax').html('Max lag: ' + pingMaxlag + 'ms');
        if (httpChartData.labels.length > 25) {
            httpChartData.labels.shift();
            httpChartData.datasets[0].data.shift();
        }
        httpChartData.labels.push('');
        httpChartData.datasets[0].data.push(lag);
        window.httpLine.update();

        setTimeout(startPing, 1000);
    });
};

let stopPing = function () {
    pingStop = true;
};