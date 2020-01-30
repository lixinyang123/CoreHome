'use strict';
var downMaxTime = 0;
var downloadStopped = false;
var download = function () {
    //thread safe
    if ($('#downloadbutton').attr('disabled') === 'disabled') {
        return;
    }
    $('#downloadbutton').attr('disabled', 'disabled');
    downloadStopped = false;
    startdownload();
};

var startdownload = function () {
    //prepare
    var st = new Date();
    $.get('/admin/overview/download?t=' + st.getMilliseconds(), function (data) {
        if (downloadStopped) {
            return;
        }
        //get time
        var et = new Date();
        var downloadTime = et - st;
        //update max value
        if (downloadTime > downMaxTime) {
            downMaxTime = downloadTime;
        }
        //get speed
        var speed = 1.0 / downloadTime * 1000;
        var minspeed = 1.0 / downMaxTime * 1000;
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

var stopDownload = function () {
    downloadStopped = true;
    if ($('#downloadbutton').removeAttr('disabled')) {
        return;
    }
};