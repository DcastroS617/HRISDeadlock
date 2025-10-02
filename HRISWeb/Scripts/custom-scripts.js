
function showTab(tabLink) {    

    // Obtener el contenedor padre de los tabs
    var tabContainer = tabLink.closest('ul');
    var tabContentContainer = document.querySelector(tabContainer.getAttribute('data-target'));

    // Remover la clase 'active' de todos los links de tabs en el contenedor
    var tabLinks = tabContainer.querySelectorAll('a');
    tabLinks.forEach(function (link) {
        link.classList.remove('active');
        link.setAttribute('aria-selected', 'false');
    });

    // Añadir la clase 'active' solo al tab activado
    tabLink.classList.add('active');
    tabLink.setAttribute('aria-selected', 'true');

    // Remover la clase 'show active' de todos los contenidos de tabs en el contenedor correspondiente
    var tabContents = tabContentContainer.querySelectorAll('.tab-pane');
    tabContents.forEach(function (content) {
        content.classList.remove('show', 'active');
    });

    // Añadir la clase 'show active' solo al contenido del tab activado
    var activeTabContent = document.querySelector(tabLink.getAttribute('href'));
   
    activeTabContent.classList.add('show', 'active');
}

