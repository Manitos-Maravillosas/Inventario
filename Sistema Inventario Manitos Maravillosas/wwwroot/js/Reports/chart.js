document.addEventListener('DOMContentLoaded', function () {
    // Fetch data from the backend
    fetch('/Reports/Home/GetDataFromService')  // Assuming your backend endpoint is at "~/Reports/Home"
        .then(response => response.json())
        .then(data => {
            // Capitalize the first letter of each word in the labels
            const formattedLabels = Object.keys(data).map(label => label.replace(/_/g, ' ').replace(/([a-z])([A-Z])/g, '$1 $2').replace(/\b\w/g, c => c.toUpperCase()));

            // Use the fetched data to create the chart
            const ctx = document.getElementById('myChart');

            new Chart(ctx, {
                type: 'pie',
                data: {
                    labels: formattedLabels,
                    datasets: [{
                        label: 'Ganancia mensual($)',
                        data: Object.values(data),
                        backgroundColor: [
                            'rgba(158, 197, 254, 1)',
                            'rgba(158, 168, 254, 1)',
                        ],
                        borderWidth: 1
                    }]
                },
                options: {
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                }
            });
        })
        .catch(error => console.error('Error fetching data:', error));
});