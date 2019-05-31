const linkMenus = document.querySelectorAll('.link-menu');
const dropdown = document.querySelector('.header-dropdown');
const dropdownContents = document.querySelectorAll('.header-dropdown-content');
const closeDropdowns = document.querySelectorAll('.close-dropdown');

function showMenu() {
  dropdownContents.forEach((d) => {
    d.style.display = 'none';
  });
  document.querySelector(`.content-${this.dataset.content}`).style.display = 'block';
  if (!dropdown.classList.contains('dropdown-show')) {
    dropdown.classList.add('dropdown-show');
  }
}

function closeMenu(e) {
  if (document.querySelector('.flex-nav')) {
    if (!e.target.classList.contains('link-menu') && ![...e.target.classList].some(c => c.startsWith('header-dropdown'))) {
      dropdown.classList.remove('dropdown-show');
    }
  }
}

linkMenus.forEach(l => l.addEventListener('click', showMenu));
closeDropdowns.forEach(l => l.addEventListener('click', closeMenu));
window.addEventListener('click', closeMenu);
