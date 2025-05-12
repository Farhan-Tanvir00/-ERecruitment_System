
    //Fillup address...
    document.getElementById("gridCheck1").addEventListener("change", function() {
        if (this.checked) {
        document.getElementById("UsersDetails_PermanentAreaOrVillageOrHouseOrRoad").value = document.getElementById("UsersDetails_PresentAreaOrVillageOrHouseOrRoad").value;
    document.getElementById("UsersDetails_PermanentPostOffice").value = document.getElementById("UsersDetails_PresentPostOffice").value;
    document.getElementById("UsersDetails_PermanentPoliceStation").value = document.getElementById("UsersDetails_PresentPoliceStation").value;
    document.getElementById("UsersDetails_PermanentPostalCode").value = document.getElementById("UsersDetails_PresentPostalCode").value;
    document.getElementById("UsersDetails_PermanentDistrict").value = document.getElementById("UsersDetails_PresentDistrict").value;
        } else {
        document.getElementById("UsersDetails_PermanentAreaOrVillageOrHouseOrRoad").value = "";
    document.getElementById("UsersDetails_PermanentPostOffice").value = "";
    document.getElementById("UsersDetails_PermanentPoliceStation").value = "";
    document.getElementById("UsersDetails_PermanentPostalCode").value = "";
    document.getElementById("UsersDetails_PermanentDistrict").value = "";
        }
    });



    //Check To continue...
    const checkbox = document.getElementById('to-continue-check');
    const dateInput = document.getElementById('to-continue-inp');

    checkbox.addEventListener('change', function () {

        dateInput.value = "";
    dateInput.disabled = this.checked;
    });

    //Images display
document.getElementById('fileInput-image').addEventListener('change', function (event) {
    const imageFile = event.target.files[0];
    const previewContainer = document.getElementById('previewContainer-image');
    const allowedTypes = ['image/jpeg', 'image/png', 'image/gif'];
    const maxSize = 100 * 1024; // 100KB

    if (!imageFile) {
        previewContainer.innerHTML = '<p class="text-muted">No file chosen</p>';
        return;
    }

    // Validate file type
    if (!allowedTypes.includes(imageFile.type)) {
        previewContainer.innerHTML = '<p class="text-danger">Invalid file type. Allowed: .jpg, .png, .gif</p>';
        return;
    }

    // Validate file size
    if (imageFile.size > maxSize) {
        previewContainer.innerHTML = '<p class="text-danger">File size must not exceed 100KB.</p>';
        return;
    }

    // Validate dimensions using Image object
    const reader = new FileReader();
    reader.onload = function (e) {
        const img = new Image();
        img.onload = function () {
            if (img.width !== 600 || img.height !== 600) {
                previewContainer.innerHTML = '<p class="text-danger">Image must be exactly 600 x 600 pixels.</p>';
                return;
            }

            // Show preview
            previewContainer.innerHTML = `<img src="${e.target.result}" alt="Image Preview" style="max-width: 100%; border: 1px solid #ccc;">`;
        };
        img.onerror = function () {
            previewContainer.innerHTML = '<p class="text-danger">Could not load image for dimension check.</p>';
        };
        img.src = e.target.result;
    };

    reader.readAsDataURL(imageFile);
});


    document.getElementById('fileInput-signature').addEventListener('change', function (event) {
        const imageFile = event.target.files[0];
    const previewContainer = document.getElementById('previewContainer-signature');
    const allowedTypes = ['image/jpeg', 'image/png', 'image/gif'];
    const maxSize = 60 * 1024; // 60KB

    if (!imageFile) {
        previewContainer.innerHTML = '<p class="text-muted">No file chosen</p>';
    return;
        }

    // Check file type
    if (!allowedTypes.includes(imageFile.type)) {
        previewContainer.innerHTML = '<p class="text-danger">Invalid file type. Allowed: .jpg, .png, .gif</p>';
    return;
        }

        // Check file size
        if (imageFile.size > maxSize) {
        previewContainer.innerHTML = '<p class="text-danger">File size must not exceed 60KB.</p>';
    return;
        }

    // Dimension check
    const reader = new FileReader();
    reader.onload = function (e) {
            const img = new Image();
    img.onload = function () {
                if (img.width !== 300 || img.height !== 80) {
        previewContainer.innerHTML = '<p class="text-danger">Image dimensions must be exactly 300 x 80 pixels.</p>';
    return;
                }

    // Show preview if valid
    previewContainer.innerHTML = `<img src="${e.target.result}" alt="Signature Preview" style="max-width:100%; border:1px solid #ccc;">`;
            };
            img.onerror = function () {
                previewContainer.innerHTML = '<p class="text-danger">Could not load image for dimension check.</p>';
            };
            img.src = e.target.result;
        };

        reader.readAsDataURL(imageFile);
    });



    //Password Validations

    document.addEventListener("DOMContentLoaded", function () {
        const form = document.querySelector("form");
    const password = document.getElementById("PassVal");
    const confirmPassword = document.getElementById("confirmPassVal");
    const confirmPassMsg = document.getElementById("confirmPassMsg");

    form.addEventListener("submit", function (event) {
            if (password.value !== confirmPassword.value) {
        event.preventDefault();
    confirmPassMsg.textContent = "Passwords do not match!";
            } else {
                confirmPassMsg.innerHTML = '&nbsp;';
            }
        });

    confirmPassword.addEventListener("input", function () {
            if (password.value === confirmPassword.value) {
                confirmPassMsg.innerHTML = '&nbsp;';
            } else {
        confirmPassMsg.textContent = "Passwords do not match!";
            }
        });
    });

    //Terms And condition check

    document.querySelector("form").addEventListener("submit", function(event) {
        let checkbox = document.getElementById("termsCheck");
    if (!checkbox.checked) {
        alert("You must agree to the terms before submitting.");
    event.preventDefault();
        }
    });

    //Date input field

    let dateInputs = document.getElementsByClassName("datej");

        Array.from(dateInputs).forEach((dateInput) => {
        dateInput.addEventListener('keydown', function (event) {
            event.preventDefault();
        });

    dateInput.addEventListener('keyup', function(event) {
        event.preventDefault();
            });

    dateInput.addEventListener('input', function(event) {
        event.preventDefault();
            });
        });

    //Grade check section

    document.getElementById("SscGradeSelect").addEventListener('change', function(){
        if(document.getElementById("SscGradeSelect").value === 'Grade'){
        document.getElementById("sscGpa").readOnly = false;
    document.getElementById("sscOutOf").readOnly = false;
        }
    else{
        document.getElementById("sscGpa").readOnly = true;
    document.getElementById("sscOutOf").readOnly = true;
        }
    });

    document.getElementById("HscGradeSelect").addEventListener('change', function(){
        if(document.getElementById("HscGradeSelect").value === 'Grade'){
        document.getElementById("hscGpa").readOnly = false;
    document.getElementById("hscOutOf").readOnly = false;
        }
    else{
        document.getElementById("hscGpa").readOnly = true;
    document.getElementById("hscOutOf").readOnly = true;
        }
    });

    document.getElementById("GrGradeSelect").addEventListener('change', function(){
        if(document.getElementById("GrGradeSelect").value === 'Grade'){
        document.getElementById("grCgpa").readOnly = false;
    document.getElementById("grOutOf").readOnly = false;
        }
    else{
        document.getElementById("grCgpa").readOnly = true;
    document.getElementById("grOutOf").readOnly = true;
        }
    });

    document.getElementById("Pg1GradeSelect").addEventListener('change', function(){
        if(document.getElementById("Pg1GradeSelect").value === 'Grade'){
        document.getElementById("pg1Cgpa").readOnly = false;
    document.getElementById("pg1OutOf").readOnly = false;
        }
    else{
        document.getElementById("pg1Cgpa").readOnly = true;
    document.getElementById("pg1OutOf").readOnly = true;
        }
    });

    document.getElementById("Pg2GradeSelect").addEventListener('change', function(){
        if(document.getElementById("Pg2GradeSelect").value === 'Grade'){
        document.getElementById("pg2Cgpa").readOnly = false;
    document.getElementById("pg2OutOf").readOnly = false;
        }
    else{
        document.getElementById("pg2Cgpa").readOnly = true;
    document.getElementById("pg2OutOf").readOnly = true;
        }
    });

    document.getElementById("Pg3GradeSelect").addEventListener('change', function(){
        if(document.getElementById("Pg3GradeSelect").value === 'Grade'){
        document.getElementById("pg3Cgpa").readOnly = false;
    document.getElementById("pg3OutOf").readOnly = false;
        }
    else{
        document.getElementById("pg3Cgpa").readOnly = true;
    document.getElementById("pg3OutOf").readOnly = true;
        }
    });

