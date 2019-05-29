// Set background from model
const trelloContainer = document.getElementById('trello-container');
trelloContainer.className = '';
// eslint-disable-next-line no-undef
trelloContainer.classList.add(backgroundClass); // backgroundClass => defined in cshtml from model

// Set name input (resize & update name)
const nameInput = document.getElementById('board-name-update');
const nameHidden = document.getElementById('board-name-hidden');

function resizeInput() {
  nameInput.style.width = `${nameInput.value.length * 12}px`;
}

async function updateBoardNameAsync(boardId, name) {
  const data = {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ boardId, name }),
  };
  const response = await fetch('/Boards/UpdateBoardNameAsync', data);
  const success = await response.json();

  return success;
}

async function updateBoardName(e) {
  if (e.keyCode === 13) {
    if (nameInput.value.length) {
      const success = await updateBoardNameAsync(nameInput.dataset.id, nameInput.value);
      if (success) {
        nameHidden.value = nameInput.value;
        document.title = `${nameInput.value} | Trello`;
        nameInput.blur();
      }
    } else {
      nameInput.blur();
    }
  } else {
    resizeInput();
  }
}

nameInput.addEventListener('blur', () => { nameInput.value = nameHidden.value; resizeInput(); });
nameInput.addEventListener('focus', () => nameInput.select());
nameInput.addEventListener('keyup', updateBoardName);

resizeInput();

// Star click handling: set/unset favorite
const star = document.querySelector('.board-link-star > div');

async function updateFavoriteAsync(boardId, isFavorite) {
  const data = {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ boardId, isFavorite }),
  };
  const response = await fetch('/Boards/UpdateFavoriteAsync', data);
  const success = await response.json();

  return success;
}

async function handleStarClick(e) {
  e.preventDefault();
  const success = await updateFavoriteAsync(this.dataset.id, !this.classList.contains('board-link-star-selected'));
  if (success) {
    this.classList.toggle('board-link-star-selected');
  }
}

star.addEventListener('click', handleStarClick);

// Edit list title
const labelTitleElements = document.querySelectorAll('.label-title');
const editTitleElements = document.querySelectorAll('.edit-title');

async function updateListTitleAsync(listId, title) {
  const data = {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ listId, title }),
  };
  const response = await fetch('/Boards/UpdateListTitleAsync', data);
  const success = await response.json();

  return success;
}

async function updateListTitle(e) {
  if (e.keyCode === 13) {
    const title = e.target.value.trim();
    if (title) {
      const success = await updateListTitleAsync(e.target.dataset.id, title);
      if (success) {
        e.target.previousElementSibling.innerHTML = title;
      }
      e.target.blur();
    }
  }
}

function blurListTitle(e) {
  e.target.value = e.target.previousElementSibling.innerHTML;
  e.target.classList.add('hide');
  e.target.previousElementSibling.classList.remove('hide');
  e.target.nextElementSibling.classList.remove('hide');
}

function showEditTitle(e) {
  e.target.classList.add('hide');
  e.target.parentNode.querySelector('.delete-list').classList.add('hide');
  e.target.nextElementSibling.classList.remove('hide');
  e.target.nextElementSibling.focus();
}

labelTitleElements.forEach(l => l.addEventListener('click', showEditTitle));
editTitleElements.forEach(t => t.addEventListener('mouseup', () => t.classList.add('edit-title.focus')));
editTitleElements.forEach(t => t.addEventListener('keyup', updateListTitle));
editTitleElements.forEach(t => t.addEventListener('blur', blurListTitle));

// Delete list
const deleteListButtons = document.querySelectorAll('.delete-list');


async function deleteListAsync(listId) {
  const data = {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ listId }),
  };
  const response = await fetch('/Boards/DeleteListAsync', data);
  const success = await response.json();

  return success;
}

function removeListElement(element) {
  const listElement = element.closest('.drop-list');
  listElement.parentNode.removeChild(listElement);
}

async function deleteList(e) {
  const success = await deleteListAsync(e.target.dataset.id);
  if (success) {
    removeListElement(e.target);
  }
}

deleteListButtons.forEach(b => b.addEventListener('click', deleteList));

// Add list
const listsContainer = document.querySelector('.board-lists');
const addListContainer = document.getElementById('add-list');
const addListLink = document.getElementById('add-list-link');
const addListForm = document.getElementById('add-list-form');
const addListTitle = addListForm.querySelector('#add-list-title');
const addListSubmit = addListForm.querySelector('#add-list-submit');
const addListClose = addListForm.querySelector('#add-list-submit-container > i');

function handleClickAddListLink() {
  addListLink.classList.add('hide');
  addListForm.classList.remove('hide');
  addListTitle.focus();
}

function closeListForm() {
  addListTitle.value = '';
  addListLink.classList.remove('hide');
  addListForm.classList.add('hide');
}

async function addListAsync(boardId, title, position) {
  const data = {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ boardId, title, position }),
  };
  const response = await fetch('/Boards/AddListAsync', data);
  const success = await response.json();

  return success;
}

function addListElement(list) {
  const element = document.createElement('div');
  element.className = 'drop-list';
  element.dataset.id = list.listId;
  element.dataset.position = list.position;
  element.innerHTML = `<div class="board-list edit-list grab-list" data-id="${list.listId}">
    <div class="list-actions">
      <label class="label-title">${list.title}</label>
      <input type="text" class="edit-title hide" value="${list.title}" data-id="${list.listId}"> 
      <i class="fas fa-trash-alt delete-list" data-id="${list.listId}"></i>
    </div>
  </div>`;

  listsContainer.insertBefore(element, addListContainer);
  // eslint-disable-next-line no-use-before-define
  addEventListeners(element);
  closeListForm();
}

async function addList() {
  if (addListTitle.value) {
    const drops = document.querySelectorAll('.drop-list');
    const position = parseInt(drops[drops.length - 1].dataset.position, 10) + 1;
    const list = await addListAsync(addListTitle.dataset.id, addListTitle.value, position);
    if (list) {
      addListElement(list);
    }
  }
}

addListLink.addEventListener('click', handleClickAddListLink);
addListForm.addEventListener('blur', closeListForm);
addListClose.addEventListener('click', closeListForm);
addListSubmit.addEventListener('click', addList);
addListTitle.addEventListener('keyup', (e) => {
  if (e.keyCode === 13) addList();
});

// Drag & drop list
const listContainer = document.querySelector('.board-lists');
const grabListElements = document.querySelectorAll('.grab-list');
let grabbed;
let grabbedDroppable;
let droppables;
let offsetXList;
let offsetYList;

const grabList = (e) => {
  e.preventDefault();
  grabbed = (e.target.classList.contains('grab-list') && e.target)
    || (e.target.classList.contains('list-actions') && e.target.parentNode) || null;
  if (grabbed) {
    document.body.style.cursor = 'grabbing';
    const rect = grabbed.getBoundingClientRect();
    grabbedDroppable = grabbed.parentNode;
    grabbedDroppable.style.width = `${rect.width}px`;
    grabbedDroppable.style.height = `${rect.height}px`;
    offsetXList = e.clientX - rect.x;
    offsetYList = e.clientY - rect.y;
    grabbed.classList.add('grabbing');
    grabbed.style.left = `${e.clientX - offsetXList}px`;
    grabbed.style.top = `${e.clientY - offsetYList}px`;

    listContainer.append(grabbed);
    droppables = document.querySelectorAll('.drop-list');
  }
};

const getIntersectionArea = (rect1, rect2) => Math.max(0, Math.min(rect1.right, rect2.right)
  - Math.max(rect1.left, rect2.left))
  * Math.max(0, Math.min(rect1.bottom, rect2.bottom) - Math.max(rect1.top, rect2.top));

const getIntersections = (rect, targets) => {
  const intersections = [];
  targets.forEach((d) => {
    const targetRect = d.getBoundingClientRect();
    intersections.push({
      element: d,
      rect: targetRect,
      area: getIntersectionArea(rect, targetRect),
    });
  });

  return intersections;
};

const moveList = (e) => {
  e.preventDefault();
  if (grabbed) {
    grabbed.style.left = `${e.clientX - offsetXList}px`;
    grabbed.style.top = `${e.clientY - offsetYList}px`;

    const intersections = getIntersections(grabbed.getBoundingClientRect(), droppables);
    const max = intersections.reduce((prev, curr) => ((prev.area > curr.area) ? prev : curr));
    if (max.area && max.element !== grabbedDroppable) {
      if (grabbedDroppable.dataset.position > max.element.dataset.position) {
        listContainer.insertBefore(grabbedDroppable, max.element);
      } else {
        listContainer.insertBefore(grabbedDroppable, max.element.nextSibling);
      }
      document.querySelectorAll('.drop-list').forEach((element, i) => {
        element.dataset.position = i;
      });
    }
  }
};

async function updateListPositionAsync(listId, position) {
  const data = {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ listId, position }),
  };
  const response = await fetch('/Boards/UpdateListPositionAsync', data);
  const success = await response.json();

  return success;
}

const dropList = (e) => {
  e.preventDefault();
  if (grabbed) {
    grabbed.classList.remove('grabbing');
    grabbedDroppable.append(grabbed);
    droppables.forEach(d => updateListPositionAsync(d.dataset.id, d.dataset.position));

    grabbed = null;
  }
};

grabListElements.forEach(l => l.addEventListener('mousedown', grabList));
document.addEventListener('mousemove', moveList);
document.addEventListener('mouseup', dropList);

// Add all necessary event listeners to new list
function addEventListeners(newList) {
  newList.querySelector('.grab-list').addEventListener('mousedown', grabList);
  newList.querySelector('.label-title').addEventListener('click', showEditTitle);
  const editTitleElement = newList.querySelector('.edit-title');
  editTitleElement.addEventListener('keyup', updateListTitle);
  editTitleElement.addEventListener('blur', blurListTitle);
  newList.querySelector('.delete-list').addEventListener('click', deleteList);
}
