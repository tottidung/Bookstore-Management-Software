<script>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css">
        <link rel="stylesheet" href="~/css/styles.css"> <!-- Liên kết đến file CSS -->
            <script src="https://code.jquery.com/jquery-3.2.1.min.js"></script>
            <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js"></script>
            <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js"></script>
            <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
 {
            var ctx = document.getElementById('productChart').getContext('2d');

            var productNames = @Html.Raw(Json.Encode(productStats.Select(p => p.ProductName).ToList()));
            var quantitiesInStock = @Html.Raw(Json.Encode(productStats.Select(p => p.QuantityInStock).ToList()));
            var quantitiesSold = @Html.Raw(Json.Encode(productStats.Select(p => p.QuantitySold).ToList()));
            var revenues = @Html.Raw(Json.Encode(productStats.Select(p => p.TotalRevenue).ToList()));

    var chart = new Chart(ctx, {
        type: 'line',
    data: {
        labels: productNames,
    datasets: [
    {
        label: 'Số Lượng Tồn Kho',
    data: quantitiesInStock,
    borderColor: 'rgba(75, 192, 192, 1)',
    backgroundColor: 'rgba(75, 192, 192, 0.2)',
    borderWidth: 2,
    pointRadius: 5,
    pointHoverRadius: 8,
    lineTension: 0.3
                        },
    {
        label: 'Số Lượng Đã Bán',
    data: quantitiesSold,
    borderColor: 'rgba(255, 159, 64, 1)',
    backgroundColor: 'rgba(255, 159, 64, 0.2)',
    borderWidth: 2,
    pointRadius: 5,
    pointHoverRadius: 8,
    lineTension: 0.3
                        },
    {
        label: 'Tổng Doanh Thu',
    data: revenues,
    borderColor: 'rgba(153, 102, 255, 1)',
    backgroundColor: 'rgba(153, 102, 255, 0.2)',
    borderWidth: 2,
    pointRadius: 5,
    pointHoverRadius: 8,
    lineTension: 0.3
                        }
    ]
                },
    options: {
        responsive: true,
    maintainAspectRatio: false,
    scales: {
        x: {
        beginAtZero: true,
    ticks: {
        autoSkip: false,
    maxRotation: 45,
    minRotation: 0,
    font: {
        size: 14,
    family: "'Segoe UI', Tahoma, Geneva, Verdana, sans-serif",
    weight: 'bold',
    color: '#333'
                                }
                            }
                        },
    y: {
        beginAtZero: true,
    ticks: {
        font: {
        size: 14,
    family: "'Segoe UI', Tahoma, Geneva, Verdana, sans-serif",
    color: '#333'
                                }
                            }
                        }
                    },
    plugins: {
        legend: {
        position: 'top',
    labels: {
        font: {
        size: 14,
    family: "'Segoe UI', Tahoma, Geneva, Verdana, sans-serif",
    weight: 'bold',
    color: '#333'
                                }
                            }
                        },
    tooltip: {
        callbacks: {
        label: function(context) {
        let label = context.dataset.label || '';
    if (label) {
        label += ': ';
                                    }
    if (context.parsed.y !== null) {
        label += context.parsed.y;
                                    }
    return label;
                                }
                            },
    titleFont: {
        size: 14,
    family: "'Segoe UI', Tahoma, Geneva, Verdana, sans-serif",
    weight: 'bold',
    color: '#ffffff'
                            },
    bodyFont: {
        size: 12,
    family: "'Segoe UI', Tahoma, Geneva, Verdana, sans-serif",
    color: '#ffffff'
                            },
    footerFont: {
        size: 12,
    family: "'Segoe UI', Tahoma, Geneva, Verdana, sans-serif",
    color: '#ffffff'
                            },
    backgroundColor: 'rgba(0, 0, 0, 0.9)',
    borderColor: 'rgba(255, 255, 255, 0.1)',
    borderWidth: 1
                        }
                    }
                }
            });
        });
</script>