import { API_HOST } from "../config.js";

const d = document;
const loader = d.getElementById("loader");
const content = d.getElementById("content");
const alert = d.getElementById("alert");

let rooms = [];

const fetchRooms = async () => {
  try {
    const response = await fetch(`${API_HOST}/room`);
    const data = await response.json();

    if (!response.ok) return null;

    console.log(data.data);
    return data.data;
  } catch (error) {
    console.log(error);
    return null;
  }
};

const getRoomSize = (size) => {
  switch (size) {
    case 0:
      return "Small";
    case 1:
      return "Medium";
    case 2:
      return "Large";
  }
};

const getRoomHTML = (room) => {
  const template = `
        <div class="col-12 col-sm-6">
          <div class="card p-3 h-100">
              <div class="d-flex justify-content-between mb-2">
              <h3>${room.hotel.name}</h3>
              <a href="" class="btn btn-primary">View</a>
              </div>
              <div class="row">
              <p class="col-md-6 col-lg-3"><b>Price: </b>Rs. ${
                room.pricePerDay
              }/day</p>
              <p class="col-md-6 col-lg-3"><b>Size: </b>${getRoomSize(
                room.size
              )}</p>
              <p class="col-md-6 col-lg-3"><b>Max Guests: </b>${
                room.maxGuests
              }</p>
              <p class="col-md-6 col-lg-3"><b>Rating: </b>${
                room.hotel.starRating
              }</p>
              </div>
          </div>
        </div>
    `;

  return template;
};

const displayRooms = (rooms) => {
  for (let i = 0; i < rooms.length; i++) {
    const room = rooms[i];
    content.innerHTML += getRoomHTML(room);
  }
};

const buildPage = async () => {
  rooms = await fetchRooms();

  if (rooms == null) {
    alert.classList.remove("d-none");
  }

  loader.classList.add("d-none");
  content.classList.remove("d-none");

  displayRooms(rooms);
};

d.addEventListener("DOMContentLoaded", buildPage);
