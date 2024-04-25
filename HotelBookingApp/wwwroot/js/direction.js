<script>
    document.addEventListener('DOMContentLoaded', function() {
        var map = L.map('map').setView([51.505, -0.09], 13); // Set the initial position and zoom level

    // Load and display the base layer of the map
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
    attribution: '© OpenStreetMap'
        }).addTo(map);

    // Function to add markers to the map
    function addMarkers(hotels) {
        hotels.forEach(function (hotel) {
            var marker = L.marker([hotel.Latitude, hotel.Longitude]).addTo(map);
            marker.bindPopup('<strong>' + hotel.Name + '</strong><br>' + hotel.Description);
        });
        }

    // Fetch the hotel data from your controller
    fetch('/Maps/GetHotels')
    .then(function(response) {
                return response.json();
            })
    .then(function(hotels) {
        addMarkers(hotels); // Add markers after data is loaded
            })
    .catch(function(error) {
        console.error('Error loading the hotel data:', error);
            });
    });
</script>
