import { API_HOST } from "../config.js";
import { redirect, showMessage, createAddress, getDateString, hideElement, showElement } from "../lib.js";

const d = document;
const errorMessage = d.getElementById("error-message");
const bookingsList = d.getElementById("bookingsList");
const loader = d.getElementById("loader");
const content = d.getElementById("content");

let bookings = [];

const fetchBookings = async () => {
  try {
    const response = await fetch(`${API_HOST}/guest/bookings`, {
      credentials: "include",
      headers: {
        Authorization: `Bearer ${localStorage.getItem("guest_token")}`,
      },
    });

    if (response.status == 401) return redirect("/guest-login.html");

    const data = await response.json();

    if (!response.ok) throw new Error(data.message);

    return data.data;
  } catch (error) {
    console.log(error);
    throw new Error("Something went wrong");
  }
};

const createGuestsHTML = (guests) => {
    let guestList = "";

    guests.forEach(guest => {
        guestList += `
            <tr>
                <td>${guest.name}</td>
                <td>${guest.gender}</td>
                <td>${guest.age}</td>
            </tr>
        `;
    })

    return guestList;
}

const createBookingsHTML = (bookings) => {
    bookings.forEach(booking => {
        bookingsList.innerHTML += `
        <li class="list-group-item mb-3 p-3">
                  <div class="">
                      <h4 class="text-primary">${booking.room.hotel.name}</h4>
                      <span> ${getDateString(booking.checkinDateTime)} to ${getDateString(booking.checkoutDateTime)} </span>
                  </div>

                  <table class="table table-striped mt-2">
                      <tbody>
                          <tr>
                              <th scope="row">Amount</th>
                              <td>Rs. ${parseFloat(booking.amount).toFixed(2)}</td>
                          </tr>
                          <tr>
                              <th scope="row">Date of booking</th>
                              <td>${getDateString(booking.dateOfBooking)}</td>
                          </tr>
                      </tbody>
                  </table>

                  <div class="accordion" id="accordionExample">
                      <div class="accordion-item">
                          <h2 class="accordion-header">
                              <button class="accordion-button collapsed"
                                      type="button"
                                      data-bs-toggle="collapse"
                                      data-bs-target="#booking-${booking.id}"
                                      aria-expanded="false"
                                      aria-controls="booking-${booking.id}">
                                  Show more details
                              </button>
                          </h2>
                          <div id="booking-${booking.id}"
                               class="accordion-collapse collapse"
                               data-bs-parent="#accordionExample">
                              <div class="accordion-body">
                                  <h4 class="text-primary">Room and Hotel details</h4>
                                  <table class="table table-striped">
                                      <tbody>
                                          <tr>
                                              <th scope="row">Room No</th>
                                              <td>${booking.room.roomNumber}</td>
                                          </tr>
                                          <tr>
                                              <th scope="row">Floor No</th>
                                              <td>${booking.room.floorNumber}</td>
                                          </tr>
                                          <tr>
                                              <th scope="row">Hotel Address</th>
                                              <td>${createAddress(booking.room.hotel.address)}</td>
                                          </tr>
                                      </tbody>
                                  </table>

                                  <h4 class="text-primary">Guest Information</h4>
                                  <table class="table table-striped">
                                      <thead>
                                          <tr>
                                              <th scope="col">Name</th>
                                              <th scope="col">Gender</th>
                                              <th scope="col">Age</th>
                                          </tr>
                                      </thead>
                                      <tbody>${createGuestsHTML(booking.bookingGuests)}</tbody>
                                  </table>
                              </div>
                          </div>
                      </div>
                  </div>
              </li>
    `;
    })
}

const displayBookings = async () => {
    try {
        bookings = await fetchBookings();
        hideElement(loader);
        console.log(bookings);
        createBookingsHTML(bookings);
        showElement(content);
    } catch (error) {
        console.error(error);
        showMessage(errorMessage, error.message);
    }
};

d.addEventListener("DOMContentLoaded", displayBookings);
