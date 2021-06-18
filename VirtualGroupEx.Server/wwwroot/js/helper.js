function setButtonBuzy(buttonid) {
    resetButtons();
    let spinner = document.createElement("div");
    spinner.className = "spinner-border spinner-border-sm";
    spinner.setAttribute("role", "status");
    let button = document.getElementById(buttonid);
    button.appendChild(spinner);
    setTimeout(function () {
        try {
            button.removeChild(spinner);
        } catch (e) { }
    }, 1000)
}

function resetButtons() {
    let spinners = document.getElementsByClassName("spinner-border");
    for (var i = 0; i < spinners.length; i++) {
        spinners[i].parentElement.removeChild(spinners[i]);
    }
}

function hideModals() {
    $('.modal.fade.show').modal('hide');
    $('.modal-backdrop.fade.show').hide();
}

$('.modal-backdrop.fade.show').click(hideModals);