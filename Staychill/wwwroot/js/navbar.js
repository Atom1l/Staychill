// ======================================================= Make Dropdown Clickable ======================================================= //
// Select the productDropdown and dropdownMenu elements
const productDropdown = document.getElementById('productDropdown');
const dropdownMenu = productDropdown.nextElementSibling;
const arrowIcon = document.getElementById('arrowIcon'); // Select the arrow icon

// Function to toggle dropdown and arrow
const toggleDropdown = () => {
    // Check if the window width is below 576px (small screen size)
    if (window.innerWidth < 576) {
        // Toggle the "show" class
        dropdownMenu.classList.toggle('show');

        // Toggle the arrow direction based on whether the dropdown is open
        if (dropdownMenu.classList.contains('show')) {
            arrowIcon.classList.remove('arrow-right');
            arrowIcon.classList.add('arrow-down');
        } else {
            arrowIcon.classList.remove('arrow-down');
            arrowIcon.classList.add('arrow-right');
        }
    }
};

// Add click event listener to toggle the dropdown
productDropdown.addEventListener('click', function (event) {
    event.stopPropagation(); // Prevent event from bubbling up
    toggleDropdown();
});

// Close the dropdown if clicked outside
document.addEventListener('click', function (event) {
    if (!productDropdown.contains(event.target) && !dropdownMenu.contains(event.target)) {
        dropdownMenu.classList.remove('show');

        // Reset arrow direction when closing
        arrowIcon.classList.remove('arrow-down');
        arrowIcon.classList.add('arrow-right');
    }
});
// ======================================================= Make Dropdown Clickable ======================================================= //

