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
        const response = await fetch(`${API_HOST}/hotel/bookings`, {
            credentials: "include",
            headers: {
                Authorization: `Bearer ${localStorage.getItem("hotel_token")}`,
            },
        });

        if (response.status == 401) return redirect("/hotel-login.html");

        const data = await response.json();

        if (!response.ok) throw new Error(data.message);

        return data.data;
    } catch (error) {
        console.log(error);
        throw new Error(error.message);
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
                <table class="table table-striped">
                    <tbody>
                        <tr>
                            <th scope="row">Booking Id</th>
                            <td>#${booking.id}</td>
                        </tr>
                        <tr>
                            <th scope="row">Checkin and Checkout</th>
                            <td>${getDateString(booking.checkinDateTime)} to ${getDateString(booking.checkoutDateTime)}</td>
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
                                Booking details
                            </button>
                        </h2>
                        <div id="booking-${booking.id}"
                             class="accordion-collapse collapse"
                             data-bs-parent="#accordionExample">
                            <div class="accordion-body">
                                <table class="table table-striped">
                                    <tbody>
                                        <tr>
                                            <th scope="row">Amount</th>
                                            <td>Rs. ${parseFloat(booking.amount).toFixed(2)}</td>
                                        </tr>
                                        <tr>
                                            <th scope="row">Booked on</th>
                                            <td>${booking.dateOfBooking}</td>
                                        </tr>
                                    </tbody>
                                </table>

                                <h4 class="text-primary">Booked Guest</h4>
                                <table class="table table-striped">
                                    <tbody>
                                        <tr>
                                            <th scope="row">Name</th>
                                            <td>${booking.guest.name}</td>
                                        </tr>
                                        <tr>
                                            <th scope="row">Email</th>
                                            <td>${booking.guest.user.email}</td>
                                        </tr>
                                        <tr>
                                            <th scope="row">Phone</th>
                                            <td>${booking.guest.phone}</td>
                                        </tr>
                                    </tbody>
                                </table>

                                <h4 class="text-primary">Room details</h4>
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
                                    </tbody>
                                </table>

                                <h4 class="text-primary">Visiting Guests</h4>
                                <table class="table table-striped">
                                    <thead>
                                        <tr>
                                            <th scope="col">Name</th>
                                            <th scope="col">Phone</th>
                                            <th scope="col">Gender</th>
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
        hideElement(loader);
        showElement(content);
        showMessage(errorMessage, error.message);
    }
};

d.addEventListener("DOMContentLoaded", displayBookings);
