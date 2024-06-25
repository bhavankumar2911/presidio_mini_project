import { redirect } from "../lib.js";

const d = document;

const email = d.getElementById("email");
const name = d.getElementById("name");
const phone = d.getElementById("phone");
const age = d.getElementById("age");
const gender = d.getElementById("gender");
const password = d.getElementById("password");
const password2 = d.getElementById("password2");
const form = d.getElementById("form");
const errorMessage = d.getElementById("error-message");

const showErrorMessage = (message) => {
  errorMessage.innerText = message;
  errorMessage.classList.remove("d-none");
};

const guestRegisterRequest = async () => {
  try {
    const response = await fetch("http://localhost:5229/guest/register", {
      method: "POST",
      body: JSON.stringify({
        email: email.value,
        phone: phone.value,
        name: name.value,
        age: age.value,
        gender: gender.value,
        plainTextPassword: password.value,
      }),
      headers: {
        "Content-Type": "application/json",
      },
    });

    if (response.ok) return redirect("/guest-login.html");

    const data = await response.json();

    if (response.status == 409) return showErrorMessage(data.message);

    console.log(data);
  } catch (error) {
    console.log(error);
    showErrorMessage("Something went wrong");
  }
};

const handleUserRegisteration = async (e) => {
  e.preventDefault();

  if (password.value != password2.value) {
    showErrorMessage("Password did not match.");
    return;
  }

  await guestRegisterRequest();
};

form.addEventListener("submit", handleUserRegisteration);
