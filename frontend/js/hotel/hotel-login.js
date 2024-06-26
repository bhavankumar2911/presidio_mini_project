import { API_HOST } from "../config.js";
import { login } from "../lib.js";

const d = document;
const errorMessage = d.getElementById("error-message");
const form = d.getElementById("form");
const email = d.getElementById("email");
const password = d.getElementById("password");

const handleHotelLogin = async (event) => {
  const loginConfig = {
    event,
    email: email.value,
    password: password.value,
    errorMessage,
    url: `${API_HOST}/hotel/login`,
    tokenName: "hotel_token",
    redirectUrl: "/add-room.html",
  };

  login(loginConfig);
};

form.addEventListener("submit", handleHotelLogin);
