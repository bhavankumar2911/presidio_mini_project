import { API_HOST } from "../config.js";
import { redirect } from "../lib.js";

const d = document;
const email = d.getElementById("email");
const name = d.getElementById("name");
const phone = d.getElementById("phone");
const description = d.getElementById("description");
const buildingNoAndName = d.getElementById("buildingNoAndName");
const streetNoAndName = d.getElementById("streetNoAndName");
const city = d.getElementById("city");
const state = d.getElementById("state");
const pincode = d.getElementById("pincode");
const password = d.getElementById("password");
const password2 = d.getElementById("password2");
const form = d.getElementById("form");
const errorMessage = d.getElementById("error-message");

const handleHotelRegister = async (event) => {
  event.preventDefault();

  if (password.value != password2.value) {
    errorMessage.innerText = "Passwords did not match.";
    errorMessage.classList.remove("d-none");
    return;
  }

  try {
    const response = await fetch(`${API_HOST}/hotel/register`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        email: email.value,
        name: name.value,
        phone: phone.value,
        description: description.value,
        address: {
          buildingNoAndName: buildingNoAndName.value,
          streetNoAndName: streetNoAndName.value,
          city: city.value,
          state: state.value,
          pincode: pincode.value,
        },
        plainTextPassword: password.value,
      }),
    });
    const data = await response.json();
    console.log(data);

    if (!response.ok) {
      errorMessage.innerText = data.message;
      errorMessage.classList.remove("d-none");
      return;
    }

    redirect("/hotel-login.html");
  } catch (error) {
    errorMessage.innerText = "Something went wrong";
    errorMessage.classList.remove("d-none");
  }
};

form.addEventListener("submit", handleHotelRegister);
