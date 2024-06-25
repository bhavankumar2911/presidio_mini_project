import { login } from "../lib.js";

$("#error-message").hide();

$("#form").submit((e) =>
  login(
    e,
    "http://localhost:5229/admin/login",
    "admin_token",
    "/admin-dashboard.html"
  )
);
