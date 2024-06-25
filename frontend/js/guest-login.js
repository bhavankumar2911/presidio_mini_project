async function loginGuest(e) {
  e.preventDefault();

  const email = $("#email").val();
  const password = $("#password").val();

  try {
    const response = await fetch("http://localhost:5229/guest/login", {
      method: "POST",
      body: JSON.stringify({ email, plainTextPassword: password }),
      headers: {
        "Content-Type": "application/json",
      },
    });
    const data = await response.json();

    console.log(data);
  } catch (error) {
    console.log(error);
  }
}

$("#form").submit(loginGuest);
