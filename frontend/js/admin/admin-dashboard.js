import { checkAuthorization } from "../lib.js";

checkAuthorization("admin", "/admin-login.html");

const d = document;
const loader = d.getElementById("loader");
const hotelList = d.getElementById("hotel-list");
const content = d.getElementById("content");

// data
let hotels = [];

const createAddress = (address) => {
  return `
        ${address.buildingNoAndName},
        ${address.streetNoAndName},
        ${address.city},
        ${address.state},
        ${address.pincode}
    `;
};

const fetchHotels = async () => {
  try {
    const response = await fetch("http://localhost:5229/hotels", {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("admin_token")}`,
      },
    });

    if (response.status == 401) {
      return (window.location.href = "/admin-login.html");
    }

    hotels = await response.json();

    console.log(hotels);
  } catch (error) {
    console.error(error);
  }
};

const createHotelDOMList = () => {
  let list = "";

  for (let i = 0; i < hotels.length; i++) {
    const hotel = hotels[i];

    const template = `
        <tr>
          <th scope="row">${hotel.id}</th>
          <td>${hotel.name}</td>
          <td>${hotel.user.email}</td>
          <td>${hotel.phone}</td>
          <td>
          <button
              class="btn btn-outline-secondary"
              type="button"
              data-bs-toggle="collapse"
              data-bs-target="#${hotel.id}"
              aria-expanded="false"
              aria-controls="${hotel.id}"
          >
              Read
          </button>
          <div class="collapse mt-2" id="${hotel.id}">
              <div class="card card-body">${hotel.description}</div>
          </div>
          </td>
          <td>
          <button
              class="btn btn-outline-secondary"
              type="button"
              data-bs-toggle="collapse"
              data-bs-target="#${hotel.id}-address"
              aria-expanded="false"
              aria-controls="${hotel.id}-address"
          >
              View
          </button>
          <div class="collapse mt-2" id="${hotel.id}-address">
              <div class="card card-body">${createAddress(hotel.address)}</div>
          </div>
          </td>
          <td>
              <span data-hotel-id="${hotel.id}" data-is-approved=${
      hotel.isApproved
    } role="button" class="status-btn badge ${
      hotel.isApproved ? "text-bg-success" : "text-bg-danger"
    }"
              >${hotel.isApproved ? "Approved" : "Not Approved"}</span
          >
          </td>
      </tr>
      `;

    list += template;
  }

  return list;
};

const updateHotelStatusRequest = async (hotelId, newApprovalStatus) => {
  try {
    const response = await fetch(
      `http://localhost:5229/hotel/status_update?hotelId=${hotelId}&newApprovalStatus=${newApprovalStatus}`,
      {
        method: "PUT",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("admin_token")}`,
        },
      }
    );

    if (response.ok) return true;

    return false;
  } catch (error) {
    console.error(error);
    return false;
  }
};

const updateHotelsState = (hotelId, newApprovalStatus) => {
  hotels = [
    ...hotels.map((hotel) => {
      if (hotel.id == hotelId)
        return { ...hotel, isApproved: newApprovalStatus };

      return { ...hotel };
    }),
  ];

  // create hotel dom list
  const hotelDOMList = createHotelDOMList();

  // add hotels to dom
  hotelList.innerHTML = hotelDOMList;

  listenToHotelStatusUpdate();
};

const handleHotelStatusUpdate = async (hotelId, newApprovalStatus) => {
  if (
    confirm(
      `Do you really want to ${
        newApprovalStatus ? "approve" : "block"
      } this hotel?`
    )
  ) {
    const hasUpdated = await updateHotelStatusRequest(
      hotelId,
      newApprovalStatus
    );

    if (hasUpdated) updateHotelsState(hotelId, newApprovalStatus);
  }
};

const listenToHotelStatusUpdate = () => {
  const hotelStatusUpdateButtons = d.getElementsByClassName("status-btn");

  for (let i = 0; i < hotelStatusUpdateButtons.length; i++) {
    const statusUpdateButton = hotelStatusUpdateButtons[i];
    const hotelId = statusUpdateButton.dataset.hotelId;
    const isApproved =
      statusUpdateButton.dataset.isApproved == "true" ? true : false;
    const newApprovalStatus = !isApproved;

    statusUpdateButton.addEventListener("click", () =>
      handleHotelStatusUpdate(hotelId, newApprovalStatus)
    );
  }
};

document.addEventListener("DOMContentLoaded", async () => {
  // fetch hotels from api
  await fetchHotels();

  // create hotel dom list
  const hotelDOMList = createHotelDOMList();

  // add hotels to dom
  hotelList.innerHTML = hotelDOMList;

  // show content
  loader.classList.add("d-none");
  content.classList.remove("d-none");

  listenToHotelStatusUpdate();
});
