// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
    function validateForm() {
        let fname = document.getElementById('Fname').value;
    let lname = document.getElementById('Lname').value;
    let phonenumber = document.getElementById('Phonenumber').value;
        let Birthdate = document.getElementById('Birthdate').value;
    let gender = document.getElementById('Gender').value;
    let email = document.getElementById('Email').value;
    let password = document.getElementById('Password').value;

    // Simple checks for required fields
        if (!fname || !lname || !phonenumber || !Birthdate || !address2 || !gender || !email || !password) {
        alert("All fields must be filled out!");
    return false;
        }

    // Example: Basic email validation
    let emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2, 4}$/;
    if (!email.match(emailRegex)) {
        alert("Please enter a valid email address.");
    return false;
        }

    // Example: Phone number validation (simple)
    let phoneRegex = /^[0-9]{10}$/;
    if (!phonenumber.match(phoneRegex)) {
        alert("Please enter a valid phone number with 10 digits.");
    return false;
        }

    return true; // Allow form submission
}


//document.getElementById("ImageFile").addEventListener("change", function (event) {
  //  var reader = new FileReader();
    //reader.onload = function (e) {
      //  document.getElementById("profileImagePreview").src = e.target.result;
    //};
    //reader.readAsDataURL(this.files[0]);
//});


const urlParams = new URLSearchParams(window.location.search);
const tab = urlParams.get('tab');

// If the "tab" parameter is "signup", switch to the Sign Up tab
if (tab === 'signup') {
    document.getElementById('tab-2').checked = true; // Activate Sign Up tab
}