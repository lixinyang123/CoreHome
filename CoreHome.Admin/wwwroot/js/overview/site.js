let trig = function (trigger, value) {
    let newTr = logTable.insertRow(-1);
    let newTd0 = newTr.insertCell();
    let newTd1 = newTr.insertCell();
    let newTd2 = newTr.insertCell();
    newTd0.innerText = new Date();
    newTd1.innerText = trigger;
    newTd2.innerText = value;
};


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