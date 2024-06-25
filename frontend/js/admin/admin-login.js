import { login } from "../lib.js";

const d = document;
const errorMessage = d.getElementById("error-message");
const form = d.getElementById("form");
const email = d.getElementById("email");
const password = d.getElementById("password");

const handleAdminLogin = async (event) => {
  const loginConfig = {
    event,
    email: email.value,
    password: password.value,
    errorMessage,
    url: "http://localhost:5229/admin/login",
    tokenName: "admin_token",
    redirectUrl: "/admin-dashboard.html",
  };

  login(loginConfig);
};

form.addEventListener("submit", handleAdminLogin);
