(function () {
    const page = document.getElementById('appPage');
    const toggle = document.getElementById('menuToggle');
    if (!page || !toggle) return;

    if (localStorage.getItem('menuColapsado') === 'true') {
        page.classList.add('sidebar-hidden');
    }

    toggle.addEventListener('click', function () {
        page.classList.toggle('sidebar-hidden');
        localStorage.setItem('menuColapsado', page.classList.contains('sidebar-hidden'));
    });
})();