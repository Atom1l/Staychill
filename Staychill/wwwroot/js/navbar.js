// Select the productDropdown and dropdownMenu elements
const productDropdown = document.getElementById('productDropdown');
const dropdownMenu = productDropdown.nextElementSibling;

// Function to toggle dropdown
const toggleDropdown = () => {
    // Check if the window width is below 576px (small screen size)
    if (window.innerWidth < 576) {
        dropdownMenu.classList.toggle('show'); // Toggle the "show" class
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
    }
});

// === Change Icon === //
const toggleButton = document.getElementById('navbarToggleButton');
const icon = toggleButton.querySelector('i'); // Select the <i> element

// Add event listener to the collapse element
const navbarCollapse = document.querySelector('.navbar-collapse');

toggleButton.addEventListener('click', function () {
    // Check if the collapse element is already shown
    if (navbarCollapse.classList.contains('show')) {
        icon.classList.remove('fa-xmark');
        icon.classList.add('fa-bars');
    } else {
        icon.classList.remove('fa-bars');
        icon.classList.add('fa-xmark');
    }
});

// Listen for the collapse hidden event to reset the icon
navbarCollapse.addEventListener('hidden.bs.collapse', function () {
    icon.classList.remove('fa-xmark');
    icon.classList.add('fa-bars');
});




