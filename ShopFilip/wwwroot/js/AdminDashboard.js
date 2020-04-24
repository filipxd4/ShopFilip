var lab = [];
var money = [];

var ctx = document.getElementById("myChart");
$.ajax({
    url: '/Admin/GetOrdersInfo',
    traditional: true,
    success: function (response) {
        for (var i = 0; i < response.length; i++) {
            var b = response[i].date.substring(0, 10);
            lab.push(b)
            money.push(Number(response[i].sumOfMoney))
            console.log(lab)
        }
        var myChart = new Chart(ctx, {
            type: 'line',
            data: {
                labels: lab,
                datasets: [{
                    data: money,
                    lineTension: 0,
                    backgroundColor: 'transparent',
                    borderColor: '#007bff',
                    borderWidth: 4,
                    pointBackgroundColor: '#007bff'
                }]
            },
            options: {
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: false
                        }
                    }]
                },
                legend: {
                    display: false,
                }
            }
        });
    }
});