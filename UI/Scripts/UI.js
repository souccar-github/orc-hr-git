function Toggle(firedBy) {

    if (firedBy == "add") {

        if (($("#toggleFiredBy").val() == "") & $("#toggleStatus").val() == "") {

            $("#addValueObjectArea").fadeIn(); //slideToggle("fast");

            $("#toggleStatus").val(true);
            $("#toggleFiredBy").val(firedBy);
        } else if (($("#toggleFiredBy").val() == "add") & $("#toggleStatus").val() == "true") {

            $("#addValueObjectArea").fadeOut();

            $("#toggleStatus").val(false);
            $("#toggleFiredBy").val(firedBy);
        } else if (($("#toggleFiredBy").val() == "add") && $("#toggleStatus").val() == "false") {

            $("#addValueObjectArea").fadeIn();

            $("#toggleStatus").val(true);
            $("#toggleFiredBy").val(firedBy);
        } else if (($("#toggleFiredBy").val() == "edit") && $("#toggleStatus").val() == "true") {

            $("#addValueObjectArea").fadeIn();

            $("#toggleStatus").val(true);
            $("#toggleFiredBy").val(firedBy);
        } else if (($("#toggleFiredBy").val() == "edit") && $("#toggleStatus").val() == "false") {

            $("#addValueObjectArea").fadeIn();

            $("#toggleStatus").val(true);
            $("#toggleFiredBy").val(firedBy);
        }
    } else {
        if (($("#toggleFiredBy").val() == "") && $("#toggleStatus").val() == "") {

            $("#addValueObjectArea").fadeIn();

            $("#toggleStatus").val(true);
            $("#toggleFiredBy").val(firedBy);
        } else if (($("#toggleFiredBy").val() == "add") && $("#toggleStatus").val() == "true") {

            $("#addValueObjectArea").fadeIn();

            $("#toggleStatus").val(true);
            $("#toggleFiredBy").val(firedBy);
        } else if (($("#toggleFiredBy").val() == "add") && $("#toggleStatus").val() == "false") {

            $("#addValueObjectArea").fadeIn();

            $("#toggleStatus").val(true);
            $("#toggleFiredBy").val(firedBy);
        } else if (($("#toggleFiredBy").val() == "edit") && $("#toggleStatus").val() == "true") {

            $("#addValueObjectArea").fadeOut();

            $("#toggleStatus").val(false);
            $("#toggleFiredBy").val(firedBy);
        } else if (($("#toggleFiredBy").val() == "edit") && $("#toggleStatus").val() == "false") {

            $("#addValueObjectArea").fadeIn();

            $("#toggleStatus").val(true);
            $("#toggleFiredBy").val(firedBy);
        }
    }

}


function DisableSaveButton() {
    $('form').submit(function () {
        $('input[type=submit]').attr('disabled', 'disabled');
    });
}

function CancelButton() {
    $('#result').fadeOut('slow', function () {
        $('#result').empty();
    });
}