// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function ToggleDateFilter()
{
    var element = document.querySelector(".dateFilter");
    if (element == null) {
        return;
    }
    element.classList.toggle("invisible");
}
function ToggleGearBoxesFilter()
{
    var element = document.querySelector(".gearBoxesFilter");
    if (element == null) {
        return;
    }
    element.classList.toggle("invisible");
}