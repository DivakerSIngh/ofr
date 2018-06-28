/// <reference path="../jquery-1.10.2.js" />
/// <reference path="../toastr.js" />


(function ($) {

    //re-set all client validation given a jQuery selected form or child
    $.fn.resetValidation = function () {

        var $form = this.closest('form');

        //reset jQuery Validate's internals
        $form.validate().resetForm();

        //reset unobtrusive validation summary, if it exists
        $form.find("[data-valmsg-summary=true]")
            .removeClass("validation-summary-errors")
            .addClass("validation-summary-valid")
            .find("ul").empty();

        //reset unobtrusive field level, if it exists
        $form.find("[data-valmsg-replace]")
            .removeClass("field-validation-error")
            .addClass("field-validation-valid")
            .empty();

        return $form;
    };
})(jQuery);


function getNorDateTime(mydate) {

    if (mydate != undefined) {

        var month = new Array();
        month[0] = "Jan";
        month[1] = "Feb";
        month[2] = "Mar";
        month[3] = "Apr";
        month[4] = "May";
        month[5] = "Jun";
        month[6] = "Jul";
        month[7] = "Aug";
        month[8] = "Sep";
        month[9] = "Oct";
        month[10] = "Nov";
        month[11] = "Dec";

        var dateString = mydate.substr(6);
        var currentTime = new Date(parseInt(dateString));
        var mnth = currentTime.getMonth();
        var day = currentTime.getDate();
        day = day < 10 ? "0" + day : day;
        var year = currentTime.getFullYear();
        var hours = currentTime.getHours();
        var minuts = (currentTime.getMinutes() < 10) ? "0" + currentTime.getMinutes() : currentTime.getMinutes();
        var date = day + " " + month[mnth] + " " + year + " " + hours + ":" + minuts + " ";
        return date;
    }
}

function OnlyAlphabetWithSpace(element)
{
    $(element).keypress(function (event) {
        var inputValue = event.which;
        // allow letters and whitespaces only.
        if (!(inputValue >= 65 && inputValue <= 120) && (inputValue != 32 && inputValue != 0)) {
            event.preventDefault();
        }
    });
}

function onlyAlphabets(e, t) {
    try {
        if (window.event) {
            var charCode = window.event.keyCode;
        }
        else if (e) {
            var charCode = e.which;
        }
        else { return true; }

        if (!(charCode >= 65 && charCode <= 120) && (charCode != 32 && charCode != 0)) {
            event.preventDefault();
        }
        else
        {
            return true;
        }

        //if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123))
        //    return true;
        //else
        //    return false;

       
    }
    catch (err) {
        alert(err.Description);
    }
}


var toasterOptions = {
    closeButton: true,
    preventDuplicates: false,
    showDuration: 300,
    hideDuration: 1000,
    timeOut: 5000,
    extendedTimeOut: 1000,
    showEasing: 'swing',
    hideEasing: 'linear',
    showMethod: 'fadeIn',
    hideMethod: 'fadeOut'
}



function readURL(input, previewElementId) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#' + previewElementId).attr('src', e.target.result);
        }
        reader.readAsDataURL(input.files[0]);
    }
}

function SubmitData(form, callback) {

    if (form.valid()) {

        startSpinning(form);

        try {

            $.ajax({
                type: "POST",
                url: form.attr('action'),
                data: new FormData(form[0]),
                dataType: 'json',
                contentType: false,
                processData: false,
                async: true,
                success: function (response) {
                    callback(response);
                },
                error: function (error) {
                    callback(error);
                }
            }).always(function () {
                stopSpinning(form);
            });
        }
        catch (e) {
            stopSpinning(form);
            toastr.error(e.message, '', toasterOptions)
        }
    }
    else {
        return false;
    }
}

function GetPartialsView(url, data, spinnerDivId, callback) {

    if (data && url) {
        startSpinning(spinnerDivId);
        try {
            $.ajax({
                type: "GET",
                url: url,
                data: data,
                async: true,
                success: function (response) {
                    callback(response);
                },
                error: function (error) {
                    callback(error);
                }
            }).always(function () {
                stopSpinning(spinnerDivId);
            });
        }
        catch (e) {
            stopSpinning(spinnerDivId);
            toastr.error(e.message, '', toasterOptions)
        }
    }
}



//function GetPartialsView(url, data, targetId) {

//    if (data) {
//        startSpinning($(targetId));
//        try {
//            $.ajax({
//                type: "GET",
//                url: url,
//                data: data,
//                async: true,
//                success: function (response) {

//                    if (response) {
//                        $(targetId).html(response);
//                    }
//                },
//                error: function (error) {

//                    toastr.error(error.responseText, '', toasterOptions)
//                }
//            }).always(function () {
//                stopSpinning($(targetId));
//            });
//        }
//        catch (e) {
//            stopSpinning($(targetId));
//            toastr.error(e.message, '', toasterOptions)
//        }
//    }
//}


function startSpinning(form) {
    $(form).waitMe({
        effect: 'ios',
        text: 'Please wait...',
        bg: 'rgba(255,255,255,0.6)',
        color: '#000',
        opacity: 0.8
    });
}

function stopSpinning(form) {
    $(form).waitMe('hide')
}
