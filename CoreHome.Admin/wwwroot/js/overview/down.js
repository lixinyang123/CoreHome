let downMaxTime = 0;
let downloadStopped = false;
let download = function () {
    //thread safe
    if ($('#downloadbutton').attr('disabled') === 'disabled') {
        return;
    }
    $('#downloadbutton').attr('disabled', 'disabled');
    downloadStopped = false;
    startdownload();
};

let startdownload = function () {
    //prepare
    let st = new Date();
    $.get('/admin/overview/download?t=' + st.getMilliseconds(), function (data) {
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

        if (downloadchartData.labels.length > 25) {
            downloadchartData.labels.shift();
            downloadchartData.datasets[0].data.shift();
        }
        downloadchartData.labels.push('');
        downloadchartData.datasets[0].data.push(speed.toFixed(2));
        window.myDownloadLine.update();

        setTimeout(startdownload, 1000);
    });
};

let stopDownload = function () {
    downloadStopped = true;
    if ($('#downloadbutton').removeAttr('disabled')) {
        return;
    }
};