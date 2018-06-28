
$(document).ready(function () {
    if ($('select').select2 != undefined) {
        $('select:not(.notSelect2)').select2({ placeholder: "Select Value" });//Intialize select2 plugin
    }

    toastr.options = {    //Intialize toastr plugin
        "closeButton": true,
        "debug": false,
        "positionClass": "toast-top-center",
        "onclick": null,
        "showDuration": "3000",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut",
        "preventDuplicates": true

    }

    // Remove focus from any element when page is scrolled. This solves datepicker issue.
    $(window).scroll(function () {
        document.activeElement.blur();
    });

    $('.panel').scroll(function () {
        document.activeElement.blur();
    });

    // Start Loader when ajax services are called.
    $(document).ajaxStart(function () {
        $('.overlay').show();
    });

    // Stop loader when ajax services are completed
    $(document).ajaxStop(function () {
        $('.overlay').hide();
    });

    // Start loader for ajax form submit
    $('[data-ajax="true"]').each(function (index, element) {
        $(element).attr('data-ajax-begin', 'test');
    });

    // Start loader for html form submit
    $('[role="form"]').submit(function (e) {
        if (this.checkValidity())
            $('.overlay').show();
    });

});


(function ($) {
    $.fn.getCursorPosition = function () {
        var input = this.get(0);
        if (!input) return; // No (input) element found
        if ('selectionStart' in input) {
            // Standard-compliant browsers
            return input.selectionStart;
        } else if (document.selection) {
            // IE
            input.focus();
            var sel = document.selection.createRange();
            var selLen = document.selection.createRange().text.length;
            sel.moveStart('character', -input.value.length);
            return sel.text.length - selLen;
        }
    }
})(jQuery);

function test() {
    $('.overlay').show();
}

function resetform(form) {
    $(':input', '#' + form + '')
              .not(':button, :submit, :reset')
              .val('')
              .removeAttr('checked')
              .removeAttr('selected');
}
function AjaxCall(Pagepath) {
    // $('.overlay').show();
    var returnData = "";
    try {
        $.ajax({
            type: "POST",
            async: false,
            url: Pagepath,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data.d != null) {
                    returnData = data.d;
                }
            },
            error: function (x, e) {
                alert("The call to the server side failed. " + x.responseText);
            }
        });
    } catch (e) {
        alert(e.ToString());
    }
    // $('.overlay').hide();
    return returnData;
}


function AjaxCallWithData(Pagepath, Data) {
    // $('.overlay').show();

    var returnData = "";
    try {
        $.ajax({
            type: "POST",
            async: false,
            url: Pagepath,
            data: "{json : " + Data + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data.d != null) {
                    returnData = data.d;
                }
            },
            error: function (x, e) {
                alert("The call to the server side failed. " + x.responseText);
            }
        });
    } catch (e) {
        alert(e.ToString());
    }
    // $('.overlay').hide();
    return returnData;
}

function AjaxCallWithDataGET(Pagepath) {
    // $('.overlay').show();
    var returnData = "";
    try {
        $.ajax({
            type: "GET",
            async: false,
            url: Pagepath,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data != null) {
                    returnData = data;
                }
            },
            error: function (x, e) {
                console.log("The call to the server side failed. " + x.responseText);
            }
        });
    } catch (e) {
        console.log(e.ToString());
    }
    //  $('.overlay').hide();
    return returnData;
}

function AjaxCallWithDataMVC_Async(Pagepath, Data,callback) {
    //  $('.overlay').show();
    var returnData = "";
    try {
        $.ajax({
            type: "GET",
            async: true,
            url: Pagepath,
            data: Data,
            contentType: "application/json; charset=utf-8",
            traditional: true,
            success: function (data) {
                callback(data);
               
            },
            error: function (x, e) {
                //alert("The call to the server side failed. " + x.responseText);
            }
        });
    } catch (e) {
        //alert(e.ToString());
    }
    // $('.overlay').hide();
    return returnData;
}
function AjaxCallWithDataMVC(Pagepath, Data) {
    //  $('.overlay').show();
    var returnData = "";
    try {
        $.ajax({
            type: "GET",
            async: false,
            url: Pagepath,
            data: Data,
            contentType: "application/json; charset=utf-8",
            traditional: true,
            success: function (data) {
                if (data.d == undefined) {
                    returnData = data;
                }
                else if (data.d != null) {
                    returnData = data.d;
                }
            },
            error: function (x, e) {
                //alert("The call to the server side failed. " + x.responseText);
            }
        });
    } catch (e) {
        //alert(e.ToString());
    }
    // $('.overlay').hide();
    return returnData;
}

function AjaxCallWithDataMVCPOST(Pagepath, Data) {
    //$('.overlay').show();
    var done = false;

    var returnData = "";

    try {
        $.ajax({
            type: "POST",
            async: false,
            url: Pagepath,
            data: JSON.stringify(Data),
            contentType: "application/json; charset=utf-8",
            traditional: true,
            success: function (data) {
                done = true;
                if (data.d == undefined) {
                    returnData = data;
                }
                else if (data.d != null) {
                    done = true;
                    returnData = data.d;
                }
            },
            error: function (x, e) {
                //alert("The call to the server side failed. " + x.responseText);
            }
        });
    } catch (e) {
    }

    //setTimeout(function () {
    //    $('.overlay').hide();
    //}, 1000);

    return returnData;
}

function isNumeric(evt, element, l) {

    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode == 8)
    {
        return true;
    }
    else if (
        //(charCode != 45) &&      // “-” CHECK MINUS, AND ONLY ONE.
        (charCode != 46 || $(element).val().indexOf('.') != -1) &&      // “.” CHECK DOT, AND ONLY ONE.
        (charCode < 48 || charCode > 57))
    { return false; }
    else if ($(element).val().indexOf('.') > 0 && ($(element).val().length - $(element).val().indexOf('.')) > 2) {
        return false;
    }
    else {
        if ($(element).val().length >= l) {
            return false;
        }
    }

    return true;
}
function isInteger(evt, element, l) {

    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode == 8)
    {
        return true;
    }
    else if (
        //(charCode != 45) &&      // “-” CHECK MINUS, AND ONLY ONE.            
        (charCode < 48 || charCode > 57))
    { return false; } else {
        if ($(element).val().length >= l) {
            return false;
        }
    }

    return true;
}
function IsTime(evt, element, l) {

    var charCode = (evt.which) ? evt.which : event.keyCode

    //(charCode != 45) &&      // “-” CHECK MINUS, AND ONLY ONE.    

    if (charCode == 186 && $(element).val().length != 2)
    { return false; }
    else if (charCode < 48 || charCode > 57)
    { return false; } else {
        if ($(element).val().length >= l) {
            return false;
        }
    }

    return true;
}
function validate(obj, max, min) {
    if (!obj.checkValidity()) {
        toastr.warning('Value can only be between ' + min + ' and ' + max);
        obj.focus();
        return false;
    }
    return true;
}

function validateRate(obj, min) {
    if (!obj.checkValidity()) {
        toastr.warning('Rate should be higher than ' + min);
        obj.focus();
        return false;
    }
    return true;
}

function validateTypeText(obj, max, min) {
    var val = parseInt(obj.value);
    if (obj.value == "") {
        return true;
    }
    if (val < min || val > max) {
        toastr.warning('Value can only be between ' + min + ' and ' + max);
        obj.focus();
        return false;
    }
    if (isNaN(val)) {
        toastr.warning('Please provide a valid number');
        obj.focus();
        return false;
    }
    return true;
}

function validateUpgradeRate(obj, min) {
    var val = parseInt(obj.value);
    if (obj.value == "") {
        return true;
    }
    if (val < min) {
        toastr.warning('Rate should be higher than ' + min);
        obj.focus();
        return false;
    }
    if (isNaN(val)) {
        toastr.warning('Please provide a valid number');
        obj.focus();
        return false;
    }
    return true;
}

function validateRateTypeText(obj, min) {
    var val = parseFloat(obj.value);
    if (obj.value == "") {
        return true;
    }
    if (val < min) {
        toastr.warning('Rate should be higher than ' + min);
        obj.focus();
        return false;
    }
    if (isNaN(val)) {
        toastr.warning('Please provide a valid number');
        obj.focus();
        return false;
    }
    return true;
}

