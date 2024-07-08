import { API_HOST } from "../config.js";
import { hideElement, redirect, showElement, showMessage } from "../lib.js";

const d = document;
const errorMessage = d.getElementById("error-message");
const infoMessage = d.getElementById("info-message");
const bookingErrorMessage = d.getElementById("booking-error-message");
const loader = d.getElementById("loader");
const content = d.getElementById("content");
const hotelName = d.getElementById("hotel-name");
const price = d.getElementById("price");
const maxGuests = d.getElementById("max-guests");
const size = d.getElementById("size");
const address = d.getElementById("address");
const reviews = d.getElementById("reviews");
const duration = d.getElementById("duration");
const roomPrice = d.getElementById("roomPrice");
const tax = d.getElementById("tax");
const totalPrice = d.getElementById("totalPrice");
const summary = d.getElementById("summary");
const addGuestButton = d.getElementById("add-guest-btn");
const guestList = d.getElementById("guest-list");
const checkinDateTime = d.getElementById("checkinDateTime");
const checkoutDateTime = d.getElementById("checkoutDateTime");
const bookBtn = d.getElementById("book-btn");
const summaryIntro = d.getElementById("summary-intro");
const bookingSummaryLoader = d.getElementById("booking-summary-loader");

let guestData = [];
let room;

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

const createAddress = (address) => {
  return `
          ${address.buildingNoAndName},
          ${address.streetNoAndName},
          ${address.city},
          ${address.state},
          ${address.pincode}
      `;
};

const getRoomIdFromURL = () => {
  const urlParams = new URLSearchParams(window.location.search);
  const roomId = urlParams.get("id");
  return roomId;
};

const displayReviews = (reviewsList) => {
  reviewsList.forEach((review) => {
    reviews.innerHTML += `
      <li class="list-group-item">
        <p>${review.content}</p>
        <p class="badge text-bg-primary"><em>${review.guest.name}</em></p>
      </li>
    `;
  });
};

const fetchRoom = async () => {
  try {
    const roomID = getRoomIdFromURL();

    if (roomID == null) {
      showMessage(errorMessage, "Blog not found");
      return;
    }

    const response = await fetch(`${API_HOST}/room/${roomID}`);
    const data = await response.json();

    hideElement(loader);

    if (!response.ok) {
      if (response.status == 404) {
        showMessage(errorMessage, data.message);
        return;
      }

      showMessage(errorMessage, "Something went wrong");
      return;
    }

    room = data.data;
    const hotel = room.hotel;

    hotelName.innerText = hotel.name;
    price.innerText = room.pricePerDay;
    maxGuests.innerText = room.maxGuests;
    size.innerText = getRoomSize(room.size);
    address.innerText = createAddress(hotel.address);
    displayReviews(hotel.reviews);
    showGuestAddForms();

    showElement(content);
    console.log(data);
  } catch (error) {
    showMessage(errorMessage, "Something went wrong");
    hideElement(loader);
    console.error(error);
  }
};

const updateGuestsData = (guestId, fieldName, fieldValue) => {
  guestData = guestData.map((guest) => {
    if (guest.id == guestId) return { ...guest, [fieldName]: fieldValue };

    return guest;
  });
  console.table(guestData);

  showGuestAddForms();
};

const getNewGuestId = () => {
  let max = 0;

  guestData.forEach((guest) => (max = Math.max(max, parseInt(guest.id))));
  max++;

  return max + "";
};

const addGuestForm = (event) => {
  event.preventDefault();

  guestData.push({
    id: getNewGuestId(),
    name: "",
    age: 0,
    gender: "",
  });

  if (checkinDateTime.value != "" && checkoutDateTime.value != "") {
    hideElement(summaryIntro);

    // todo
    const checkin = new Date(checkinDateTime.value);
    const checkout = new Date(checkoutDateTime.value);

    if (checkin >= checkout) {
      showMessage(
        bookingErrorMessage,
        "Choose valid checkin and checkout date and time."
      );
      hideElement(summary);
      hideElement(bookBtn);
      return;
    }

    showElement(bookBtn);
  }

  console.table(guestData);

  showGuestAddForms();
};

const createInputElement = (type, placeholder, value, guestId, fieldName) => {
  const input = d.createElement("input");
  input.type = type;
  input.value = value;
  input.placeholder = placeholder;
  input.className = "form-control";
  input.addEventListener("change", (e) =>
    updateGuestsData(guestId, fieldName, e.target.value)
  );

  return input;
};

const deleteGuest = (e, guestId) => {
  e.preventDefault();
  guestData = guestData.filter((guest) => guest.id != guestId);

  console.table(guestData);

  if (guestData.length == 0) {
    if (checkinDateTime.value == "" || checkoutDateTime.value == "") {
      summaryIntro.innerText = "Fill in the details first!";
    } else summaryIntro.innerText = "Add atleast one guest to book this room.";
    showElement(summaryIntro);
    hideElement(bookBtn);
  }

  showGuestAddForms();
};

const showGuestAddForms = () => {
  // guestList.innerHTML = "";

  // guestData.forEach((guest) => {
  //   guestList.innerHTML += `
  //   <li class="list-group-item">
  //     <div class="mb-2">
  //       <input
  //         type="text"
  //         class="form-control guest-name"
  //         placeholder="Guest name"
  //         data-guest-id=${guest.id}
  //         id=${`guest-${guest.id}-name`}
  //         name=${`guest-${guest.id}-name`}
  //         value="${guest.name}"
  //       />
  //     </div>

  //     <div class="row g-2">
  //       <div class="mb-3 col-12 col-sm-4 col-md-5">
  //         <select
  //           class="form-select guest-gender"
  //           aria-label="Default select example"
  //           name=${`guest-${guest.id}-gender`}
  //           id=${`guest-${guest.id}-gender`}
  //         >
  //           <option value="">Gender</option>
  //           <option value="male" ${
  //             guest.gender == "male" ? "selected" : ""
  //           }>Male</option>
  //           <option value="female" ${
  //             guest.gender == "female" ? "selected" : ""
  //           }>Female</option>
  //         </select>
  //       </div>

  //       <div class="mb-3 col-8 col-sm-4 col-md-5">
  //         <input
  //           type="number"
  //           class="form-control guest-age"
  //           placeholder="Age"
  //           id=${`guest-${guest.id}-age`}
  //           name=${`guest-${guest.id}-age`}
  //           value=${guest.age}
  //         />
  //       </div>

  //       <div class="mb-3 col-4 col-sm-4 col-md-2">
  //         <div class="d-grid">
  //           <button class="btn btn-outline-danger guest-delete-btn" data-guest-id=${
  //             guest.id
  //           }>Delete</button>
  //         </div>
  //       </div>
  //     </div>
  //   </li>
  // `;
  // });

  // addEventListeners();

  guestList.innerHTML = "";

  guestData.forEach((guest) => {
    const li = d.createElement("li");
    li.className = "list-group-item";

    const nameWrapper = d.createElement("div");
    nameWrapper.className = "mb-2";
    const name = createInputElement(
      "text",
      "Name",
      guest.name,
      guest.id,
      "name"
    );
    nameWrapper.append(name);

    const ageGenderRow = d.createElement("div");
    ageGenderRow.className = "row g-2";

    const ageCol = d.createElement("div");
    ageCol.className = "mb-3 col-12 col-sm-4 col-md-5";
    const age = createInputElement("number", "Age", guest.age, guest.id, "age");
    ageCol.append(age);

    const genderCol = d.createElement("div");
    genderCol.className = "mb-3 col-12 col-sm-4 col-md-5";
    const gender = d.createElement("select");
    gender.className = "form-select";

    const defaultOption = d.createElement("option");
    defaultOption.innerText = "Gender";
    defaultOption.selected = guest.gender == "" ? true : false;

    const male = d.createElement("option");
    male.innerText = "Male";
    male.value = "male";
    male.selected = guest.gender == "male" ? true : false;

    const female = d.createElement("option");
    female.innerText = "Female";
    female.value = "female";
    female.selected = guest.gender == "female" ? true : false;

    gender.append(defaultOption);
    gender.append(male);
    gender.append(female);
    gender.addEventListener("change", (e) =>
      updateGuestsData(guest.id, "gender", e.target.value)
    );
    genderCol.append(gender);

    const deleteBtnCol = d.createElement("div");
    deleteBtnCol.className = "mb-3 col-4 col-sm-4 col-md-2";

    const deleteBtnWrapper = d.createElement("div");
    deleteBtnWrapper.className = "d-grid";
    deleteBtnCol.append(deleteBtnWrapper);

    const deleteBtn = d.createElement("button");
    deleteBtn.className = "btn btn-outline-danger";
    deleteBtn.textContent = "Delete";
    deleteBtnWrapper.append(deleteBtn);
    deleteBtn.addEventListener("click", (e) => deleteGuest(e, guest.id));

    ageGenderRow.append(ageCol);
    ageGenderRow.append(genderCol);
    ageGenderRow.append(deleteBtnCol);

    li.append(nameWrapper);
    li.append(ageGenderRow);
    guestList.append(li);
  });
};

const getDuration = (checkin, checkout) => {
  let diffMs = Math.abs(checkout - checkin);

  let days = Math.floor(diffMs / (1000 * 60 * 60 * 24));
  let hours = Math.floor((diffMs % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));

  let dateString = "";

  switch (days) {
    case 0:
      break;

    case 1:
      dateString += "1 day";
      break;

    default:
      dateString += `${days} days`;
      break;
  }

  switch (hours) {
    case 0:
      break;

    case 1:
      dateString += `${days == 0 ? "" : ", "}1 hour`;
      break;

    default:
      dateString += `${days == 0 ? "" : ", "}${hours} hours`;
      break;
  }

  return dateString;
};

const showBookingDuration = async (e) => {
  hideElement(bookingErrorMessage);

  if (checkinDateTime.value == "" || checkoutDateTime.value == "")
    return showMessage(
      bookingErrorMessage,
      "Both checkin and checkout date and time is required."
    );

  const checkin = new Date(checkinDateTime.value);
  const checkout = new Date(checkoutDateTime.value);

  if (checkin >= checkout) {
    showMessage(
      bookingErrorMessage,
      "Choose valid checkin and checkout date and time."
    );
    hideElement(summary);
    hideElement(bookBtn);
    return;
  }

  try {
    // show loader
    showElement(bookingSummaryLoader);
    hideElement(summary);
    const amount = await getAmount();

    hideElement(bookingSummaryLoader);
    duration.innerText = getDuration(checkin, checkout);
    roomPrice.innerText = amount.withoutTax;
    tax.innerText = amount.withTax - amount.withoutTax;
    totalPrice.innerText = amount.withTax;
    showElement(summary);

    if (guestData.length == 0)
      return (summaryIntro.innerText = "Add atleast one guest to proceed.");

    hideElement(summaryIntro);
    showElement(bookBtn);
  } catch (error) {
    hideElement(bookingSummaryLoader);
    hideElement(summaryIntro);
    showMessage(bookingErrorMessage, error.message);
  }
};

const getAmount = async () => {
  try {
    const response = await fetch(`${API_HOST}/book/amount`, {
      method: "POST",
      headers: {
        Authorization: `Bearer ${localStorage.getItem("guest_token")}`,
        "Content-Type": "application/json",
      },
      credentials: "include",
      body: JSON.stringify({
        checkinDateTime: checkinDateTime.value + ":00Z",
        checkoutDateTime: checkoutDateTime.value + ":00Z",
        roomID: getRoomIdFromURL(),
        guests: [],
      }),
    });

    if (response.status == 401) return redirect("/guest-login.html");

    const data = await response.json();

    if (!response.ok) {
      throw new Error(data.message);
    }

    return data.data;
  } catch (error) {
    console.log(error);
    throw new Error(error.message);
  }
};

const bookRoom = async (e) => {
  e.preventDefault();

  try {
    bookBtn.innerText = "Please wait...";
    bookBtn.disabled = true;

    console.log({
      checkinDateTime: checkinDateTime.value + ":00Z",
      checkoutDateTime: checkoutDateTime.value + ":00Z",
      roomID: getRoomIdFromURL(),
      guests: [
        ...guestData.map((guest) => ({
          name: guest.name,
          age: guest.age,
          gender: guest.gender,
        })),
      ],
    });

    const response = await fetch(`${API_HOST}/payment/order`, {
      method: "POST",
      headers: {
        Authorization: `Bearer ${localStorage.getItem("guest_token")}`,
        "Content-Type": "application/json",
      },
      credentials: "include",
      body: JSON.stringify({
        checkinDateTime: checkinDateTime.value + ":00Z",
        checkoutDateTime: checkoutDateTime.value + ":00Z",
        roomID: getRoomIdFromURL(),
        guests: [
          ...guestData.map((guest) => ({
            name: guest.name,
            age: parseInt(guest.age),
            gender: guest.gender,
          })),
        ],
      }),
    });

    bookBtn.innerText = "Book Now";
    bookBtn.disabled = false;

    const data = await response.json();

    if (!response.ok) return showMessage(bookingErrorMessage, data.message);

    console.log(data);
    openRazorpay(data.data.orderId);
    // redirect(`./payment.html?order_id=${data.orderId}`);
  } catch (error) {
    console.error(error);
  }
};

const openRazorpay = (orderId) => {
  // var options = {
  //   name: "Hotel Booking",
  //   order_id: orderId,
  //   // image: "../../media/images/logo.png",
  //   // prefill: {
  //   //   name: "Gaurav Kumar",
  //   //   email: "gaurav.kumar@example.com",
  //   //   contact: "+919000090000",
  //   // },
  //   theme: {
  //     color: "#3399cc",
  //   },
  // };
  // options.theme.image_padding = false;
  // options.handler = function (razorpayResponse) {
  //   console.log(
  //     razorpayResponse.razorpay_payment_id,
  //     orderId,
  //     razorpayResponse.razorpay_signature
  //   );
  // };
  // options.modal = {
  //   ondismiss: function () {
  //     console.log("This code runs when the popup is closed");
  //   },
  //   escape: true,
  //   backdropclose: false,
  // };
  // var orderId = "order_OUcZ6LWdiaSPOS";
  var options = {
    name: "Hotel Booking",
    order_id: orderId,
    image: "../../media/images/logo.png",
    theme: {
      color: "#3399cc",
    },
  };
  options.theme.image_padding = false;
  options.handler = async function (razorpayResponse) {
    console.log(
      razorpayResponse.razorpay_payment_id,
      orderId,
      razorpayResponse.razorpay_signature
    );

    // verify payment
    try {
      let response = await fetch(`${API_HOST}/payment/verify`, {
        method: "POST",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("guest_token")}`,
          "Content-Type": "application/json",
        },
        credentials: "include",
        body: JSON.stringify({
          paymentId: razorpayResponse.razorpay_payment_id,
          orderId: orderId,
          signature: razorpayResponse.razorpay_signature,
        }),
      });

      if (response.status == 401) return redirect("/guest-login.html");

      if (!response.ok)
        return showMessage(bookingErrorMessage, response.data.message);

      // make booking
      response = await fetch(`${API_HOST}/book`, {
        method: "POST",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("guest_token")}`,
          "Content-Type": "application/json",
        },
        credentials: "include",
        body: JSON.stringify({
          checkinDateTime: checkinDateTime.value + ":00Z",
          checkoutDateTime: checkoutDateTime.value + ":00Z",
          roomID: getRoomIdFromURL(),
          guests: [
            ...guestData.map((guest) => ({
              name: guest.name,
              age: parseInt(guest.age),
              gender: guest.gender,
            })),
          ],
        }),
      });

      if (response.status == 401) return redirect("/guest-login.html");

      if (!response.ok)
        return showMessage(bookingErrorMessage, response.data.message);

      return redirect("/guest-bookings.html");
    } catch (error) {
      console.error(error);
      showMessage("Something went wrong.");
    }
  };
  options.modal = {
    ondismiss: function () {
      console.log("This code runs when the popup is closed");
    },
    escape: true,
    backdropclose: false,
  };
  var rzp = new Razorpay(options);
  rzp.open();
};

// bookBtn.addEventListener("click", showBookingAmount);
checkoutDateTime.addEventListener("change", showBookingDuration);
checkinDateTime.addEventListener("change", showBookingDuration);
addGuestButton.addEventListener("click", addGuestForm);
bookBtn.addEventListener("click", bookRoom);
d.addEventListener("DOMContentLoaded", fetchRoom);
