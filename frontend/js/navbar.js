import { API_HOST } from "./config.js";
import { logout } from "./lib.js";

const d = document;
const navLoader = d.getElementById("nav-loader");
const authButton = d.getElementById("auth-button");

// logout guest
const logoutGuest = () => {
  logout("guest_token");
  authButton.innerText = "login";
};

// authorize user
const authorizeGuest = async () => {
  try {
    const response = await fetch(`${API_HOST}/guest/authorize`, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("guest_token")}`,
      },
      credentials: "include",
    });

    navLoader.classList.add("d-none");
    authButton.classList.remove("d-none");

    if (response.ok) {
      authButton.innerText = "Logout";
      authButton.onclick = logoutGuest;
      return;
    }

    authButton.innerText = "Login";
    authButton.href = "/guest-login.html";
  } catch (error) {
    navLoader.classList.add("d-none");
  }
};

d.addEventListener("DOMContentLoaded", authorizeGuest);
