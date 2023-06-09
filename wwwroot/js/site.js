﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$('#Type').on('change', function () {
    if ($(this).val() === 'Custom') {
        $('#custom-type').removeClass('d-none');
    } else {
        $('#custom-type').addClass('d-none');
    }
});
function displayResults(workouts) {
    const dropdownResults = document.querySelector('.autocomplete-result-list');
    dropdownResults.innerHTML = '';
    if (workouts.length === 0) {
        dropdownResults.style.display = 'none';
        return;
    }
    dropdownResults.style.display = 'block';
    workouts.forEach(workout => {
        const workoutName = workout.name;
        const workoutId = workout.id;
        dropdownResults.innerHTML += `<li><a href="/Workouts/Details/${workoutId}">${workoutName}</a></li>`;
    });
}
const form = document.querySelector('#search-form');
const input = document.querySelector('#search-input');

form.addEventListener('submit', async (event) => {
    event.preventDefault();

    const searchTerm = input.value;

    try {
        const response = await fetch(`/api/WorkoutsSearchAPI/Search/${searchTerm}`);
        const workouts = await response.json();

        // Display the search results
        // ...
    } catch (error) {
        console.error(error);
    }
});
$('form').on('submit', function (e) {
    e.preventDefault(); // prevent default form submission behavior
    // submit form data using AJAX instead
});

const renderResults = (workouts) => {
    const resultsContainer = document.querySelector('#search-results');
    resultsContainer.innerHTML = '';

    if (workouts.length === 0) {
        resultsContainer.innerHTML = '<p>No results found.</p>';
        return;
    }

    workouts.forEach((workout) => {
        const workoutElement = document.createElement('div');
        workoutElement.classList.add('workout');

        const nameElement = document.createElement('h2');
        nameElement.textContent = workout.name;
        workoutElement.appendChild(nameElement);

        const typeElement = document.createElement('p');
        typeElement.textContent = workout.type;
        workoutElement.appendChild(typeElement);

        const descriptionElement = document.createElement('p');
        descriptionElement.textContent = workout.description;
        workoutElement.appendChild(descriptionElement);

        resultsContainer.appendChild(workoutElement);
    });
};

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

