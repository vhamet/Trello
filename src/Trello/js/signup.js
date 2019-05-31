const usernameInput = document.getElementById('username');
const emailInput = document.getElementById('email');
const passwordInput = document.getElementById('password');
const passwordConfirmationInput = document.getElementById('passwordConfirmation');
const submit = document.getElementById('submit-signup');

const passwordValid = () => passwordInput.value && passwordConfirmationInput.value
    && passwordInput.value === passwordConfirmationInput.value;
const usernameValid = () => usernameInput.value && document.getElementById('usernameUsed').style.display === 'none';
const emailValid = () => emailInput.value && document.getElementById('emailUsed').style.display === 'none';

function setSubmit() {
  if (passwordValid() && usernameValid() && emailValid()) {
    submit.disabled = false;
  } else {
    submit.disabled = true;
  }
}

async function checkFieldAsync(field, value) {
  const data = {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ [field]: value }),
  };

  const response = await fetch('/Authentication/CheckFieldAsync', data);
  const used = await response.json();

  return used;
}

async function checkField() {
  if (this.value) {
    const used = await checkFieldAsync(this.id, this.value);
    if (used) {
      this.classList.add('input-validation-error');
      document.getElementById(`${this.id}Used`).style.display = 'block';
    } else {
      this.classList.remove('input-validation-error');
      document.getElementById(`${this.id}Used`).style.display = 'none';
    }
    setSubmit();
  } else {
    this.classList.remove('input-validation-error');
    document.getElementById(`${this.id}Used`).style.display = 'none';
    setSubmit();
  }
}

function checkPassword() {
  if ((passwordInput.value || passwordConfirmationInput.value)
    && passwordInput.value !== passwordConfirmationInput.value) {
    passwordConfirmationInput.classList.add('input-validation-error');
    document.getElementById('passwordInvalid').style.display = 'block';
  } else {
    passwordConfirmationInput.classList.remove('input-validation-error');
    document.getElementById('passwordInvalid').style.display = 'none';
  }
  setSubmit();
}

usernameInput.addEventListener('input', checkField);
emailInput.addEventListener('input', checkField);
passwordInput.addEventListener('input', checkPassword);
passwordConfirmationInput.addEventListener('input', checkPassword);
