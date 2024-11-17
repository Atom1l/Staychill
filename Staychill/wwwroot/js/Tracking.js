function updateTrackingStatus() {
    const statusInput = document.getElementById('tracking-status-input').value; // Get input status
    const trackingItems = document.querySelectorAll('.tracking-pending'); // All tracking items

    trackingItems.forEach(item => {
        const status = item.getAttribute('data-status'); // Get the status from data-status attribute
        const icon = item.querySelector('.tracking-icon-background'); // Get the icon with the tracking-icon-background class

        // Reset background color for all icons
        icon.style.backgroundColor = ''; // Default color

        // If status matches, change background color of the icon
        if (status === statusInput) {
            icon.style.backgroundColor = '#C9F0F7'; // Set color for the matched status
        }
    });
}

// Run on load
updateTrackingStatus();