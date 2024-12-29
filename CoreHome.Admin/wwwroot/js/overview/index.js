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

function showArchive() {
    $.get('/Admin/Overview/Archive', data => {
        archiveChartData.labels = data.map(i => i.key);
        archiveChartData.datasets[0].data = data.map(i => i.value);
        window.archiveLine.update();
    });
}

let httpChartCtx = document.getElementById('httpChart').getContext('2d');
let downloadChartCtx = document.getElementById('downloadChart').getContext('2d');
let wsChartCtx = document.getElementById('wsChart').getContext('2d');
let archiveChartCtx = document.getElementById('archiveChart').getContext('2d');

let httpChartData = {
    labels: [],
    datasets: [{
        label: "HTTP Lag",
        backgroundColor: 'rgb(255, 99, 132)',
        borderColor: 'rgb(255, 99, 132)',
        fill: false,
        data: []
    }]
};

let downloadChartData = {
    labels: [],
    datasets: [{
        label: "Download Speed",
        backgroundColor: 'rgb(70, 170, 252)',
        borderColor: 'rgb(70, 170, 252)',
        fill: true,
        data: []
    }]
};

let wsChartData = {
    labels: [],
    datasets: [{
        label: "WebSocket Connection",
        backgroundColor: 'rgb(2, 232, 99)',
        borderColor: 'rgb(2, 232, 99)',
        fill: false,
        data: []
    }]
};

let archiveChartData = {
    labels: [],
    datasets: [
        {
            label: "Blog Count",
            backgroundColor: 'rgb(255, 205, 86)',
            borderColor: 'rgb(255, 205, 86)',
            fill: true,
            data: []
        }
    ]
}

let chartOption = {
    responsive: true
};

window.httpLine = new Chart(httpChartCtx, {
    type: 'line',
    data: httpChartData,
    options: chartOption
});

window.downloadLine = new Chart(downloadChartCtx, {
    type: 'line',
    data: downloadChartData,
    options: chartOption
});

window.wsLine = new Chart(wsChartCtx, {
    type: 'line',
    data: wsChartData,
    options: chartOption
});

window.archiveLine = new Chart(archiveChartCtx, {
    type: 'line',
    data: archiveChartData,
    options: chartOption
});

let showChart = () => {
    startWsTest();
    startPing();
    startDownload();
    showArchive();
};

showChart();
