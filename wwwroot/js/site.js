// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('#vitalsCollapse').on('shown.bs.collapse', function () {
        $("#HeartRate").prop('required', true);
        $("#SystolicBP").prop('required', true);
        $("#DiastolicBP").prop('required', true);
        $("#OxygenSaturation").prop('required', true);
    });
    $('#vitalsCollapse').on('hidden.bs.collapse', function () {
        $("#HeartRate").prop('required', false);
        $("#SystolicBP").prop('required', false);
        $("#DiastolicBP").prop('required', false);
        $("#OxygenSaturation").prop('required', false);
    });
});
