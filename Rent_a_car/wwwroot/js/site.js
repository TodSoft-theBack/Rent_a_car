function ToggleInvisible(prompt) {
    var element = document.querySelector("" + prompt);
    if (element == null) {
        return;
    }
    element.classList.toggle("invisible");
}

function ToggleDeleteConfirmation()
{
    var element = document.querySelector(".toggler");
    if (element == null) {
        return;
    }
    ToggleInvisible(".deleteConfirmation");
    element.classList.remove("toggler");
}

function ToggleGearBoxesFilter()
{
    ToggleInvisible(".gearBoxesFilter");
}
function ToggleCarTypeFilter() {
    ToggleInvisible(".carTypeFilter");
}
function ToggleEngineTypeFilter() {
    ToggleInvisible(".engineTypeFilter");
}

function ToggleLoggedUserMenu()
{
    ToggleInvisible(".user-greeting + ul");
}
