let downMaxTime = 0;
let downloadStopped = false;

let startDownload = function () {
    //prepare
    let st = new Date();
    $.get('/Admin/Overview/Download?t=' + st.getMilliseconds(), function (data) {
        if (downloadStopped) {
            return;
        }
        //get time
        let et = new Date();
        let downloadTime = et - st;
        //update max value
        if (downloadTime > downMaxTime) {
            downMaxTime = downloadTime;
        }
        //get speed
        let speed = 1.0 / downloadTime * 1000;
        let minspeed = 1.0 / downMaxTime * 1000;
        //log
        if (speed < $('#speedlagfilter').val()) {
            trig('Downloader', speed + 'MB/s');
        }
        //update view
        $('#downStatus').html('Speed: ' + speed.toFixed(2) + 'MB/s');
        $('#downMax').html('Min: ' + minspeed.toFixed(2) + 'MB/s');

        if (downloadChartData.labels.length > 25) {
            downloadChartData.labels.shift();
            downloadChartData.datasets[0].data.shift();
        }
        downloadChartData.labels.push('');
        downloadChartData.datasets[0].data.push(speed.toFixed(2));
        window.downloadLine.update();

        setTimeout(startDownload, 1000);
    });
};

let stopDownload = function () {
    downloadStopped = true;
};