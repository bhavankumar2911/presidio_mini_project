import { checkAuthorization, redirect } from "../lib.js";
import { API_HOST } from "../config.js";

checkAuthorization("hotel", "/hotel-login.html");

const d = document;
const loader = d.getElementById("loader");
const content = d.getElementById("content");
const pricePerDay = d.getElementById("pricePerDay");
const size = d.getElementById("size");
const roomNumber = d.getElementById("roomNumber");
const floorNumber = d.getElementById("floorNumber");
const maxGuests = d.getElementById("maxGuests");
const isAvailable = d.getElementById("isAvailable");
const form = d.getElementById("form");
const errorMessage = d.getElementById("error-message");
const successMessage = d.getElementById("success-message");

loader.classList.add("d-none");
content.classList.remove("d-none");

const handleAddRoom = async (event) => {
  event.preventDefault();

  try {
    const response = await fetch(`${API_HOST}/room`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("hotel_token")}`,
      },
      body: JSON.stringify({
        pricePerDay: parseInt(pricePerDay.value),
        size: parseInt(size.value),
        roomNumber: parseInt(roomNumber.value),
        floorNumber: parseInt(floorNumber.value),
        maxGuests: parseInt(maxGuests.value),
        isAvailable: isAvailable.checked,
      }),
    });
    const data = await response.json();

    console.log(data);

    if (!response.ok) {
      if (response.status == 401) return redirect("/hotel-login.html");

      errorMessage.innerText = data.message;
      errorMessage.classList.remove("d-none");
      return;
    }

    successMessage.innerText = "Room added.";
    successMessage.classList.remove("d-none");
    errorMessage.classList.add("d-none");
  } catch (error) {
    errorMessage.classList.remove("d-none");
    errorMessage.innerText = "Something went wrong";
    console.error(error);
  }
};

form.addEventListener("submit", handleAddRoom);
