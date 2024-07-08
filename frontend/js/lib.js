export const redirect = (url) => (window.location.href = url);

export const login = async (loginConfig) => {
  const { event, email, password, errorMessage, url, tokenName, redirectUrl } =
    loginConfig;

  event.preventDefault();

  try {
    const response = await fetch(url, {
      method: "POST",
      body: JSON.stringify({ email, plainTextPassword: password }),
      headers: {
        "Content-Type": "application/json",
      },
    });
    const data = await response.json();
    const statusCode = response.status;

    console.log(data);

    if (statusCode != 200) {
      errorMessage.innerText = data.message;
      errorMessage.classList.remove("d-none");
      return;
    }

    localStorage.setItem(tokenName, data.token);
    redirect(redirectUrl);
  } catch (error) {
    errorMessage.innerText = "Something went wrong.";
    errorMessage.classList.remove("d-none");
  }
};

export function checkAuthorization(userRole, redirectUrl) {
  const token = localStorage.getItem(`${userRole}_token`);

  if (!token) return redirect(redirectUrl);

  return;
}

export function logout(tokenName) {
  localStorage.removeItem(tokenName);
}

export const showMessage = (messageElement, errorText) => {
  messageElement.classList.remove("d-none");
  messageElement.innerText = errorText;
};

export const hideElement = (element) => {
  element.classList.add("d-none");
};

export const showElement = (element) => {
  element.classList.remove("d-none");
};

export const createAddress = (address) => {
    return `
        ${address.buildingNoAndName},
        ${address.streetNoAndName},
        ${address.city},
        ${address.state},
        ${address.pincode}
    `;
};

export const getDateString = (date) => {
    return new Date(date).toLocaleString();
}
