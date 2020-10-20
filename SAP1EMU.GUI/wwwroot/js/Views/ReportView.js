$(function () {
    $('#ReportBugForm').submit(function (e) {
        e.preventDefault();

        var data = $('#ReportBugForm').serializeArray();

        $.ajax({
            type: "POST",
            url: "../api/Report/SubmitBugReport",
            data: data,
            headers: headers = {
                RequestVerificationToken: $('input:hidden[name="__RequestVerificationToken"]').val()
            },
            success: function (data) {
                message = 'Issue with id: ' + data.number + ' has been created! Thank you.';
                $('#toast-message').text(message);
                clearBugReport();
                showToast('toast-success');
            },
            error: function (request, status, error) {
                if (request.status === 404) { //github was not found
                    message = "Something has gone wrong. Please try again later or contact us!";
                    $('#toast-message').text(message);
                    showToast('toast-error');
                } else if (request.status === 401) { //credentials are bad
                    message = "Authentication error. Please contact us!";
                    $('#toast-message').text(message);
                    showToast('toast-error');
                }
            }
        });
    });
});

$(function () {
    $('#RequestFeatureForm').submit(function (e) {
        e.preventDefault();

        var data = $('#RequestFeatureForm').serializeArray();

        $.ajax({
            type: "POST",
            url: "../api/Report/SubmitFeatureRequest",
            data: data,
            headers: headers = {
                RequestVerificationToken: $('input:hidden[name="__RequestVerificationToken"]').val()
            },
            success: function (data) {
                message = 'Feature request issue with id: ' + data.number + ' has been created! Thank you.';
                $('#toast-message').text(message);
                clearFeatureRequest();
                showToast('toast-success');
            },
            error: function (request, status, error) {
                if (request.status === 404) { //github was not found
                    //Todo: fix this to be more descriptive
                    message = "Something has gone wrong. Please try again later or contact us!";
                    $('#toast-message').text(message);
                    showToast('toast-error');
                } else if (request.status === 401) { //credentials are bad
                    message = "Authentication error. Please contact us!";
                    $('#toast-message').text(message);
                    showToast('toast-error');
                }
            }
        });
    });
});

function clearBugReport() {
    $('#Title').val('');
    $('#Description').val('');
    $('#ReproductionSteps').val('');
}

function clearFeatureRequest() {
    $('#Title').val('');
    $('#Description').val('');
}

$(document).ready(function () {
    $('.toast').toast('hide');
});

function showToast(mode) {
    toast = $('.toast');

    if (toast.hasClass('toast-success')) {
        toast.removeClass('toast-success');
    } else {
        toast.removeClass('toast-error');
    }

    toast.addClass(mode);

    toast.toast('show');
}