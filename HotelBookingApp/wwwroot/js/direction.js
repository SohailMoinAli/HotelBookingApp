document.addEventListener('DOMContentLoaded', function () {
    const hotelLat = 40.7128; // Change these coordinates to your hotel's actual coordinates
    const hotelLng = -74.0060;

    const map = L.map('map').setView([hotelLat, hotelLng], 13);
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '© OpenStreetMap contributors'
    }).addTo(map);

    // Marker for the central hotel
    const hotelMarker = L.marker([hotelLat, hotelLng]).addTo(map);
    hotelMarker.bindPopup("<b>Your Hotel Name Here</b><br>Address here").openPopup();

    // Fetch and display other hotels from the server
    fetch('/Maps/GetHotels')
        .then(response => {
            if (!response.ok) throw new Error('Network response was not ok ' + response.statusText);
            return response.json();
        })
        .then(data => {
            console.log('Hotel data:', data); // Debugging line to check what data is received
            data.forEach(hotel => {
                if (hotel.latitude !== hotelLat && hotel.longitude !== hotelLng) {
                    L.marker([hotel.latitude, hotel.longitude]).addTo(map)
                        .bindPopup(`<b>${hotel.name}</b><br/>${hotel.description}`);
                }
            });
        })
        .catch(error => console.error('Failed to fetch hotel data:', error));
});
