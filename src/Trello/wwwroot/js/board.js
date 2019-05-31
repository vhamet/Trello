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

async function handleStarClick() {
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
  e.target.nextElementSibling.select();
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
  addListTitle.select();
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
  element.innerHTML = `<div class="board-list edit-list grab-list empty-list">
    <div class="list-actions">
      <span class="label-title">${list.title}</span>
      <input type="text" class="edit-title hide" value="${list.title}" data-id="${list.listId}"> 
      <span class="fas fa-trash-alt delete-list" data-id="${list.listId}"></span>
    </div>
    <div class="add-card">
        <div class="add-card-link">
            <i class="fas fa-plus"></i>
            <span>'Add a card</span>
        </div>
        <div class="add-card-form add-form hide">
            <textarea class="add-card-title card-style" data-id="${list.listId}" rows="3" 
                placeholder="Enter a title for this card..."></textarea>
            <div>
                <button class="add-card-submit">Add card</button>
                <i class="fas fa-times add-card-close"></i>
            </div>
        </div>
    </div>
  </div>`;

  listsContainer.insertBefore(element, addListContainer);
  // eslint-disable-next-line no-use-before-define
  addListEventListeners(element);
  closeListForm();
}

async function addList() {
  if (addListTitle.value) {
    const drops = document.querySelectorAll('.drop-list');
    const position = drops.length ? parseInt(drops[drops.length - 1].dataset.position, 10) + 1 : 0;
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
let grabbedList;
let grabbedListDroppable;
let listDroppables;
let offsetXList;
let offsetYList;

const grabList = (e) => {
  grabbedList = (e.target.classList.contains('grab-list') && e.target)
    || (e.target.classList.contains('list-actions') && e.target.parentNode) || null;
  if (grabbedList) {
    grabbedList.style.cursor = 'grabbing';
    const rect = grabbedList.getBoundingClientRect();
    grabbedListDroppable = grabbedList.parentNode;
    grabbedListDroppable.style.maxWidth = `${rect.width}px`;
    grabbedListDroppable.style.height = `${rect.height}px`;
    offsetXList = e.clientX - rect.x;
    offsetYList = e.clientY - rect.y;
    grabbedList.classList.add('grabbing');
    grabbedList.style.width = `${rect.width}px`;
    grabbedList.style.height = `${rect.height}px`;
    grabbedList.style.left = `${e.clientX - offsetXList}px`;
    grabbedList.style.top = `${e.clientY - offsetYList}px`;

    listContainer.append(grabbedList);
    listDroppables = document.querySelectorAll('.drop-list');
  }
};

const getIntersectionArea = (rect1, rect2) => Math.max(0, Math.min(rect1.right, rect2.right)
  - Math.max(rect1.left, rect2.left))
  * Math.max(0, Math.min(rect1.bottom, rect2.bottom) - Math.max(rect1.top, rect2.top));

const getIntersections = (rect, targets) => {
  const intersections = [];
  targets.forEach((t) => {
    const targetRect = t.getBoundingClientRect();
    intersections.push({
      element: t,
      rect: targetRect,
      area: getIntersectionArea(rect, targetRect),
    });
  });

  return intersections;
};

const moveList = (e) => {
  if (grabbedList) {
    grabbedList.style.left = `${e.clientX - offsetXList}px`;
    grabbedList.style.top = `${e.clientY - offsetYList}px`;

    const intersections = getIntersections(grabbedList.getBoundingClientRect(), listDroppables);
    const max = intersections.reduce((prev, curr) => ((prev.area > curr.area) ? prev : curr));
    if (max.area && max.element !== grabbedListDroppable) {
      if (grabbedListDroppable.dataset.position > max.element.dataset.position) {
        listContainer.insertBefore(grabbedListDroppable, max.element);
      } else {
        listContainer.insertBefore(grabbedListDroppable, max.element.nextSibling);
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
  if (grabbedList) {
    grabbedList.classList.remove('grabbing');
    grabbedListDroppable.append(grabbedList);
    listDroppables.forEach(d => updateListPositionAsync(d.dataset.id, d.dataset.position));

    grabbedList.removeAttribute('style');
    grabbedListDroppable.removeAttribute('style');
    grabbedList = null;
  }
};

grabListElements.forEach(l => l.addEventListener('mousedown', grabList));
document.addEventListener('mousemove', moveList);
document.addEventListener('mouseup', dropList);

// Add card
const addCardLinkElements = document.querySelectorAll('.add-card-link');
const closeAddCardElements = document.querySelectorAll('.add-card-close');
const addCardSubmitElements = document.querySelectorAll('.add-card-submit');

function handleClickAddCard(e) {
  e.currentTarget.classList.add('hide');
  e.currentTarget.nextElementSibling.classList.remove('hide');
  e.currentTarget.nextElementSibling.querySelector('textarea').select();
}

function closeCardForm(form) {
  form.querySelector('textarea').value = '';
  form.classList.add('hide');
  form.previousElementSibling.classList.remove('hide');
}

function handleClickCloseCardForm(e) {
  const form = e.currentTarget.closest('.add-card-form');
  closeCardForm(form);
}

async function addCardAsync(listId, title, position) {
  const data = {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ listId, title, position }),
  };
  const response = await fetch('/Boards/AddCardAsync', data);
  const success = await response.json();

  return success;
}

const addCardElement = (submit, card) => {
  const element = document.createElement('div');
  element.className = 'drop-card';
  element.dataset.id = card.cardId;
  element.dataset.position = card.position;
  element.innerHTML = `<div class="card card-style grab-card">
      <span class="label-title-card">${card.title}</span>
      <span class="fas fa-pencil-alt edit-card-icon action-card" data-id="${card.cardId}"></span>
      <span class="fas fa-trash-alt delete-card-icon action-card" data-id="${card.cardId}"></span>
    </div>
    <div class="edit-card hide">
      <input placeholder="Edit card title..." value="${card.title}" data-id="${card.cardId}" />
    </div>`;

  const form = submit.closest('.add-card-form');
  const addCardContainer = form.parentNode;
  const cardContainer = addCardContainer.parentNode;
  cardContainer.insertBefore(element, addCardContainer);
  // eslint-disable-next-line no-use-before-define
  addCardEventListeners(element);
  closeCardForm(form);
};

async function handleClickSubmitCard(e) {
  const cardTitleElement = e.target.parentNode.previousElementSibling;
  if (cardTitleElement.value) {
    const drops = e.target.closest('.board-list').querySelectorAll('.drop-card');
    const position = drops.length ? (parseInt(drops[drops.length - 1].dataset.position, 10) || 0) + 1 : 0;
    const card = await addCardAsync(cardTitleElement.dataset.id, cardTitleElement.value, position);
    if (card) {
      addCardElement(e.target, card);
    }
  }
}

addCardLinkElements.forEach(e => e.addEventListener('click', handleClickAddCard));
closeAddCardElements.forEach(e => e.addEventListener('click', handleClickCloseCardForm));
addCardSubmitElements.forEach(e => e.addEventListener('click', handleClickSubmitCard));

// Drag & drop card
const grabCardElements = document.querySelectorAll('.grab-card');
let grabbedCard;
let grabbedCardDroppable;
let cardDroppables;
let offsetXCard;
let offsetYCard;

const grabCard = (e) => {
  grabbedCard = (e.target.classList.contains('grab-card') && e.target)
    || e.target.closest('.grab-card') || null;

  if (grabbedCard) {
    grabbedCard.style.cursor = 'grabbing';
    const rect = grabbedCard.getBoundingClientRect();
    grabbedCardDroppable = grabbedCard.parentNode;
    grabbedCardDroppable.style.width = `${rect.width}px`;
    grabbedCardDroppable.style.height = `${rect.height}px`;
    offsetXCard = e.clientX - rect.x;
    offsetYCard = e.clientY - rect.y;
    grabbedCard.classList.add('grabbing');
    grabbedCard.style.width = `${rect.width}px`;
    grabbedCard.style.height = `${rect.height}px`;
    grabbedCard.style.left = `${e.clientX - offsetXCard}px`;
    grabbedCard.style.top = `${e.clientY - offsetYCard}px`;
  }
  listContainer.append(grabbedCard);
  cardDroppables = document.querySelectorAll('.drop-card, .empty-list');
};

const moveCard = (e) => {
  if (grabbedCard) {
    grabbedCard.style.left = `${e.clientX - offsetXCard}px`;
    grabbedCard.style.top = `${e.clientY - offsetYCard}px`;

    const intersections = getIntersections(grabbedCard.getBoundingClientRect(), cardDroppables);
    const max = intersections.reduce((prev, curr) => ((prev.area > curr.area) ? prev : curr));
    if (max.area && max.element !== grabbedCardDroppable) {
      const droppableContainer = grabbedCardDroppable.parentNode;
      if (max.element.classList.contains('empty-list')) {
        const addContainer = max.element.querySelector('.add-card');
        max.element.insertBefore(grabbedCardDroppable, addContainer);
        max.element.classList.remove('empty-list');
        cardDroppables = document.querySelectorAll('.drop-card, .empty-list');
      } else {
        const cardContainer = max.element.parentNode;
        if (grabbedCardDroppable.dataset.position > max.element.dataset.position) {
          cardContainer.insertBefore(grabbedCardDroppable, max.element);
        } else {
          cardContainer.insertBefore(grabbedCardDroppable, max.element.nextSibling);
        }
      }

      if (!droppableContainer.querySelector('.drop-card')) {
        droppableContainer.classList.add('empty-list');
        cardDroppables = document.querySelectorAll('.drop-card, .empty-list');
      }
      document.querySelectorAll('.drop-card').forEach((element, i) => {
        element.dataset.position = i;
      });
    }
  }
};

async function UpdateCardPositionAsync(listId, cardId, position) {
  const data = {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ listId, cardId, position }),
  };
  const response = await fetch('/Boards/UpdateCardPositionAsync', data);
  const success = await response.json();

  return success;
}

const dropCard = () => {
  if (grabbedCard) {
    grabbedCard.classList.remove('grabbing');
    grabbedCardDroppable.append(grabbedCard);
    cardDroppables = document.querySelectorAll('.drop-card');
    cardDroppables.forEach(d => UpdateCardPositionAsync(d.closest('.drop-list').dataset.id, d.dataset.id, d.dataset.position));

    grabbedCard.removeAttribute('style');
    grabbedCardDroppable.removeAttribute('style');
    grabbedCard = null;
  }
};


grabCardElements.forEach(l => l.addEventListener('mousedown', grabCard));
document.addEventListener('mousemove', moveCard);
document.addEventListener('mouseup', dropCard);

// Delete card
const deleteCardElements = document.querySelectorAll('.delete-card-icon');

async function deleteCardAsync(cardId) {
  const data = {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ cardId }),
  };
  const response = await fetch('/Boards/DeleteCardAsync', data);
  const success = await response.json();

  return success;
}

function removeCardElement(element) {
  const cardElement = element.closest('.drop-card');
  cardElement.parentNode.removeChild(cardElement);
}

async function deleteCard(e) {
  const success = await deleteCardAsync(e.target.dataset.id);
  if (success) {
    removeCardElement(e.target);
  }
}

function handleMousedownDeleteCard(e) {
  e.stopPropagation();
}

deleteCardElements.forEach((e) => {
  e.addEventListener('mousedown', handleMousedownDeleteCard);
  e.addEventListener('click', deleteCard);
});

// Edit card
const editCardElements = document.querySelectorAll('.edit-card-icon');
const editCardInputs = document.querySelectorAll('.edit-card > input');

function showEditCard(e) {
  e.target.closest('.grab-card').classList.add('hide');
  const container = e.target.closest('.drop-card');
  const editCard = container.querySelector('.edit-card');
  editCard.classList.remove('hide');
  editCard.querySelector('input').select();
}

function hideEditCard(e) {
  const container = e.target.closest('.drop-card');
  container.querySelector('.edit-card').classList.add('hide');
  container.querySelector('input').value = container.querySelector('.label-title-card').textContent;
  container.querySelector('.grab-card').classList.remove('hide');
}

function handleMousedownEditCard(e) {
  e.stopPropagation();
}
async function updateCardTitleAsync(cardId, title) {
  const data = {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ cardId, title }),
  };
  const response = await fetch('/Boards/UpdateCardTitleAsync', data);
  const success = await response.json();

  return success;
}

async function updateCardTitle(e) {
  if (e.keyCode === 13) {
    const title = e.target.value.trim();
    if (title) {
      const success = await updateCardTitleAsync(e.target.dataset.id, title);
      if (success) {
        const container = e.target.closest('.drop-card');
        container.querySelector('.label-title-card').textContent = title;
      }
      e.target.blur();
    }
  }
}

editCardElements.forEach((e) => {
  e.addEventListener('mousedown', handleMousedownEditCard);
  e.addEventListener('click', showEditCard);
});
editCardInputs.forEach((e) => {
  e.addEventListener('blur', hideEditCard);
  e.addEventListener('mousedown', handleMousedownEditCard);
  e.addEventListener('keyup', updateCardTitle);
});

// Add all necessary event listeners to new list
function addListEventListeners(newList) {
  newList.querySelector('.grab-list').addEventListener('mousedown', grabList);
  newList.querySelector('.label-title').addEventListener('click', showEditTitle);

  const editTitleElement = newList.querySelector('.edit-title');
  editTitleElement.addEventListener('keyup', updateListTitle);
  editTitleElement.addEventListener('blur', blurListTitle);

  newList.querySelector('.delete-list').addEventListener('click', deleteList);

  newList.querySelector('.add-card-link').addEventListener('click', handleClickAddCard);
  newList.querySelector('.add-card-close').addEventListener('click', handleClickCloseCardForm);
  newList.querySelector('.add-card-submit').addEventListener('click', handleClickSubmitCard);
}

// Add all necessary event listeners to new card
function addCardEventListeners(newCard) {
  newCard.addEventListener('mousedown', grabCard);

  const deleteIconElement = newCard.querySelector('.delete-card-icon');
  deleteIconElement.addEventListener('mousedown', handleMousedownDeleteCard);
  deleteIconElement.addEventListener('click', deleteCard);

  const editIconElement = newCard.querySelector('.edit-card-icon');
  editIconElement.addEventListener('mousedown', handleMousedownEditCard);
  editIconElement.addEventListener('click', showEditCard);

  const editInput = newCard.querySelector('input');
  editInput.addEventListener('blur', hideEditCard);
  editInput.addEventListener('mousedown', handleMousedownEditCard);
  editInput.addEventListener('keyup', updateCardTitle);
}
