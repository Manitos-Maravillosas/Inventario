document.addEventListener('DOMContentLoaded', function () {
    var navItems = document.querySelectorAll('.inline-nav-item');

    navItems.forEach(function (navItem) {
        navItem.addEventListener('click', function (event) {
            // Check if the clicked element is not the submenu to prevent closing when submenu items are clicked
            if (!event.target.closest('.subMenuCollapse')) {
                event.preventDefault();
                navItem.classList.toggle('collapsed');

                // Find the arrow icon within the clicked nav item and toggle its classes

            }
        });
    });
});

