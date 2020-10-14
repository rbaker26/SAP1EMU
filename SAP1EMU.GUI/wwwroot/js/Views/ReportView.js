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
                alert('Issue with id: ' + data.number + ' has been created! Thank you.');
                clearBugReport();
            },
            error: function (request, status, error) {
                //Todo: fix this to be more descriptive
                alert("Something has gone wrong. Please try again or contact us!");
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
                alert('Feature request issue with id: ' + data.number + ' has been created! Thank you.');
                clearFeatureRequest();
            },
            error: function (request, status, error) {
                //Todo: fix this to be more descriptive
                alert("Something has gone wrong. Please try again or contact us!");
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