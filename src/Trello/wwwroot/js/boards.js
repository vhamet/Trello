// Star click handling: set/unset favorite
const stars = document.querySelectorAll('.board-link-star > div');

async function updateFavoriteAsync(id, isFavorite) {
  const data = {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ id, isFavorite }),
  };
  const response = await fetch('/Boards/UpdateFavoriteAsync', data);
  const success = await response.json();

  return success;
}

function handleStarClick(e) {
  e.preventDefault();

  updateFavoriteAsync(this.dataset.id, !this.classList.contains('board-link-star-selected'))
    .then((success) => {
      if (success) {
        this.classList.toggle('board-link-star-selected');
      }
    });
}

stars.forEach(star => star.addEventListener('click', handleStarClick));

// Modal create new board: click handling
const createBoard = document.getElementById('board-create');
const overlay = document.querySelector('.board-create-overlay');
const cancelButton = document.getElementById('board-create-button-cancel');

function showModal() {
  overlay.classList.add('board-create-overlay-show');
}

function hideModal() {
  overlay.classList.remove('board-create-overlay-show');
}

function handleCancelButtonClick() {
  if (this === cancelButton) hideModal();
}

function handleOverlayClick(e) {
  if (e.target.classList.contains('board-create-overlay')) hideModal();
}

createBoard.addEventListener('click', showModal);
cancelButton.addEventListener('click', handleCancelButtonClick);
overlay.addEventListener('click', handleOverlayClick);

// Change board-create background
const backgroundButtons = document.querySelectorAll('.background-button');
const createBoardTitle = document.getElementById('board-create-title');
const selectedBackground = document.getElementById('board-create-background-selected');

function setBoardCreateBackground(e) {
  if (e.target.classList.contains('background-button')) {
    createBoardTitle.className = '';
    createBoardTitle.classList.add(`background-${e.target.dataset.background}`);
    this.appendChild(selectedBackground);
  }
}

backgroundButtons.forEach(b => b.addEventListener('click', setBoardCreateBackground));

// Enable create button
const titleInput = document.getElementById('board-create-title-input');
const createButton = document.getElementById('board-create-button-create');

function enableCreateButton() {
  if (this.value) {
    createButton.removeAttribute('disabled');
  } else {
    createButton.setAttribute('disabled', '');
  }
}

titleInput.addEventListener('input', enableCreateButton);

// Create new board
const boardsGrid = document.getElementById('boards-grid');

async function createBoardAsyncAsync(board) {
  const data = {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(board),
  };
  const response = await fetch('/Boards/CreateBoardAsync', data);
  const createdBoard = await response.json();

  return createdBoard;
}

async function handleCreateButtonClick() {
  const newBoard = {
    Name: titleInput.value,
    Background: selectedBackground.parentElement.dataset.background,
  };

  const createdBoard = await createBoardAsyncAsync(newBoard);
  if (createdBoard) {
    titleInput.value = '';
    hideModal();
    createButton.setAttribute('disabled', '');

    const boardNode = document.createElement('a');
    boardNode.classList.add('board-link');
    boardNode.href = `/Board/${createdBoard.id}`;
    boardNode.innerHTML = `<div class="board-link-content ${createdBoard.backgroundClass}">
    <div>
        ${createdBoard.name}
    </div>
    <div class="board-link-star">
        <div class="far fa-star" data-id="${createdBoard.id}"></div> 
    </div>
</div>`;
    boardNode.querySelector('.board-link-star > div').addEventListener('click', handleStarClick);
    boardsGrid.insertBefore(boardNode, createBoard);
  }
}

createButton.addEventListener('click', handleCreateButtonClick);
