// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$('#Type').on('change', function () {
    if ($(this).val() === 'Custom') {
        $('#custom-type').removeClass('d-none');
    } else {
        $('#custom-type').addClass('d-none');
    }
});
// [Todo] - Refactor this code to toggle the dark theme
// const toggleBtn = document.getElementById('theme-toggle');
// const body = document.querySelector('body');

// toggleBtn.addEventListener('click', function () {
//     body.classList.toggle('dark');
//     const navbar = document.querySelector('.navbar');
//     navbar.classList.toggle('dark');
//     const table = document.querySelector('table');
//     table.classList.toggle('dark');
//     const body = document.querySelector('body');
//     body.classList.toggle('dark');
//     const btnPrimary = document.querySelectorAll('.btn-primary');
//     btnPrimary.forEach(function (btn) {
//         btn.classList.toggle('dark');
//     });

//     const card = document.querySelectorAll('.card');
//     card.forEach(function (card) {
//         card.classList.toggle('dark');
//     });

//     const cardTitle = document.querySelectorAll('.card-title');
//     cardTitle.forEach(function (title) {
//         title.classList.toggle('dark');
//     });

//     const cardText = document.querySelectorAll('.card-text');
//     cardText.forEach(function (text) {
//         text.classList.toggle('dark');
//     });
// });


$(document).ready(function () {
    $("#workoutSearchInput").on("keyup", function () {
        var value = $(this).val().toLowerCase().trim();
        if (value) {
            $.ajax({
                url: "/api/WorkoutSearchAPI?searchString=" + value,
                type: "GET",
                success: function (response) {
                    $("#workoutDropdownMenu").html(response);
                    $("#workoutDropdown").addClass("show");
                },
                error: function (xhr) {
                    console.log(xhr.responseText);
                }
            });
        }
        else {
            $("#workoutDropdownMenu").empty();
            $("#workoutDropdown").removeClass("show");
        }
    });
});

