let ctx = document.getElementById('httpChart').getContext('2d');
let downloadChartCtx = document.getElementById('downloadChart').getContext('2d');
let wsChartCtx = document.getElementById('wsChart').getContext('2d');

let chartData = {
    labels: [],
    datasets: [{
        label: "HTTP Lag",
        backgroundColor: 'rgb(255, 99, 132)',
        borderColor: 'rgb(255, 99, 132)',
        fill: false,
        data: []
    }]
};

let downloadchartData = {
    labels: [],
    datasets: [{
        label: "Download Speed",
        backgroundColor: 'rgb(70, 170, 252)',
        borderColor: 'rgb(70, 170, 252)',
        fill: true,
        data: []
    }]
};

let wschartData = {
    labels: [],
    datasets: [{
        label: "WebSocket Connection",
        backgroundColor: 'rgb(2, 232, 99)',
        borderColor: 'rgb(2, 232, 99)',
        fill: false,
        data: []
    }]
};

let chartOption = {
    responsive: true
};

window.myLine = new Chart(ctx, {
    type: 'line',
    data: chartData,
    options: chartOption
});

window.myDownloadLine = new Chart(downloadChartCtx, {
    type: 'line',
    data: downloadchartData,
    options: chartOption
});

window.myWSLine = new Chart(wsChartCtx, {
    type: 'line',
    data: wschartData,
    options: chartOption
});

let startAll = function () {
    $('#startAllButton').attr('disabled', 'disabled');
    WsTest();
    ping();
    download();
};

startAll();

function handleTickInit(tick) {
    let startTime = new Date(Date.parse(document.querySelector('#startupTime').value));

    Tick.helper.interval(function () {
        let time = new Date() - startTime;
        let seconds = Math.floor((time / 1000) % 60);
        let minutes = Math.floor((time / (1000 * 60)) % 60);
        let hours = Math.floor((time / (1000 * 60 * 60)) % 24);
        let days = Math.floor(time / (1000 * 60 * 60 * 24));
        tick.value = { sep: ':', days, hours, minutes, seconds };
    });
}