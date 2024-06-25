export const redirect = (url) => (window.location.href = url);

// export async function login(e, url, tokenName, redirectUrl) {
//   e.preventDefault();

//   const email = $("#email").val();
//   const password = $("#password").val();

//   try {
//     const response = await fetch(url, {
//       method: "POST",
//       body: JSON.stringify({ email, plainTextPassword: password }),
//       headers: {
//         "Content-Type": "application/json",
//       },
//     });
//     const data = await response.json();
//     const statusCode = response.status;

//     if (statusCode != 200) {
//       $("#error-message").text(data.message);
//       $("#error-message").fadeIn();
//       return;
//     }

//     console.log(data);
//     localStorage.setItem(tokenName, data.token);
//     window.location.href = redirectUrl;
//   } catch (error) {
//     $("#error-message").text("Something went wrong");
//     $("#error-message").fadeIn();
//   }
// }

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

  console.log("token", token);

  if (!token) return (window.location.href = redirectUrl);
}
