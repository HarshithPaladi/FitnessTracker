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

$(document).ready(function () {
    $("#workoutSearchInput").on("keyup", function () {
        var value = $(this).val().toLowerCase().trim();
        if (value) {
            $.ajax({
                url: "/Workouts/SearchWorkouts?searchString=" + value,
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

