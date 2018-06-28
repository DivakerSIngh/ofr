
$(document).ready(function () {
    if ($('select').select2 != undefined) {
        $('select').select2({ placeholder: "Select Value" });//Intialize select2 plugin
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
});

function resetform(form) {
    $(':input', '#'+form+'')
              .not(':button, :submit, :reset')
              .val('')
              .removeAttr('checked')
              .removeAttr('selected');
}
function AjaxCall(Pagepath) {
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
    return returnData;
}


function AjaxCallWithData(Pagepath, Data) {
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
    return returnData;
}

function AjaxCallWithDataGET(Pagepath) {
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
    return returnData;
}


function AjaxCallWithDataMVC(Pagepath, Data) {
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
    return returnData;
}