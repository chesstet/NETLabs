function WarningMessage(text, duration) {
    RemovePreviousAlert();
    SetAlertIntoCache("warning", text, duration);
    $('body').prepend('<div class="alert alert-warning alert-dismissible fade show" style="z-index: 999; position: absolute; left:50%; transform: translate(-50%);" role="alert"><strong>' + text + '</strong><button type ="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close" ></button></div>');
    $(".alert-warning").delay(duration).fadeOut(2000);
}

function ErrorMessage(text, duration) {
    RemovePreviousAlert();
    SetAlertIntoCache("error", text, duration);
    $('body').prepend('<div class="alert alert-danger alert-dismissible fade show" style="z-index: 999; position: absolute; left:50%; transform: translate(-50%);" role="alert"><strong>' + text + '</strong><button type ="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close" ></button></div>');
    $(".alert-danger").delay(duration).fadeOut(2000);
}

function SuccessMessage(text, duration) {
    RemovePreviousAlert();
    SetAlertIntoCache("success", text, duration);
    $('body').prepend('<div class="alert alert-success alert-dismissible fade show" style="z-index: 999; position: absolute; left:50%; transform: translate(-50%);" role="alert"><strong>' + text + '</strong><button type ="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close" ></button></div>');
    $(".alert-success").delay(duration).fadeOut(2000);
}

function RemovePreviousAlert() {
    $('body').find('.alert').alert('close');
}

function SetAlertIntoCache(alertType, text, duration) {
    localStorage.setItem('cacheAlert', JSON.stringify([alertType, text, duration]));
}

function CallCacheAlert() {
    if (localStorage.hasOwnProperty('cacheAlert')) {
        let callAlert = JSON.parse(localStorage.getItem("cacheAlert"));
        switch (callAlert[0]) {
            case "success":
                SuccessMessage(callAlert[1], callAlert[2]);
                break;
            case "warning":
                WarningMessage(callAlert[1], callAlert[2]);
                break;
            case "error":
                ErrorMessage(callAlert[1], callAlert[2]);
                break;
        }
        localStorage.removeItem("cacheAlert");
    }
}

$(document).ready(function () {
    CallCacheAlert();
});