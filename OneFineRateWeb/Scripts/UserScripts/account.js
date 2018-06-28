


function CheckPopupAvailibility(callback) {
    
    resetError();

    if ($('#forgotPasswordForm') && $('#loginForm').length && $('#registerForm').length && $('#frmSearchBookingPopup').length) {

        callback();
    }
    else {
        $("#accountPopupContainer").load('/Account/GetLoginRegisterForm', function () {

            $.validator.unobtrusive.parse($('#registerForm'));
            $.validator.unobtrusive.parse($('#loginForm'));
            $.validator.unobtrusive.parse($('#frmSearchBookingPopup'));
            $.validator.unobtrusive.parse($('#forgotPasswordForm'));
            callback();
        });
    }
}

$(function () {

    $('a[data-toggle="modal"]').on('click', function (e) {

        var target = $(e.target).data("buttontype");

        CheckPopupAvailibility(function () {

            $('#forgotPasswordTab').hide();
            $('#signInTab').show();

            if (target === 'managebooking') {

                $('#signUpTab').hide();
                $('#manageBookingTab').show();
                $('.nav-tabs a[href="#manageMyBooking"]').tab('show');
                $('#hdnExternalLoginReturnUrl').val("/Manage/Booking");
                $('#hdnLoginReturnUrl').val("/Manage/Booking");

            }
            else if (target === 'loginregister') {
                $('#manageBookingTab').hide();
                $('#signUpTab').show();
                $('.nav-tabs a[href="#signin"]').tab('show')
            }
            else {

                $('#manageBookingTab').hide();
                $('#signUpTab').show();
                $('.nav-tabs a[href="#signin"]').tab('show');
            }
        });
    });

    $("#accountModel").on("hidden.bs.modal", function () {
        $("label.error").hide();
        $(".error").removeClass("error");
        $('#txtCorporateEmail').rules('remove');
        $('#txtCorporateEmail').attr("placeholder", "Corporate Email (Optional)")
        $('#hdnExternalLoginReturnUrl').val('');
        $('#hdnLoginReturnUrl').val('');
        $('form').each(function (index, element) {
            clearValidation(element);
        });
        $('.hdnCorporateRequested').val(false);

    });

    $('#accountPopupContainer').delegate('input:radio[name="searchByQ"]', 'change', function () {

        if ($(this).is(':checked') && $(this).val() == 'email') {
            $('#txtEmailPopup').show();
            $('#txtMobilePopup').hide().val(null);
            $('#txtMobilePopup-error').hide();
        }
        else if ($(this).is(':checked') && $(this).val() == 'mobile') {
            $('#txtMobilePopup').show();
            $('#txtEmailPopup').hide().val(null);
            $('#txtEmailPopup-error').hide();
        }

    });

    $('#accountPopupContainer').delegate('.backToAccount', 'click', function () {

        CheckPopupAvailibility(function () {

            $('#forgotPasswordTab').hide();
            $('#manageBookingTab').hide();
            $('#signInTab').show();
            $('#signUpTab').show();
            $('.nav-tabs a[href="#signin"]').tab('show');

        });

    });

    $('#accountPopupContainer').delegate('#lnkForgotPassword', 'click', function () {

        CheckPopupAvailibility(function () {

            $('#signUpTab').hide();
            $('#signInTab').hide();
            $('#manageBookingTab').hide();
            $('#forgotPasswordTab').show();
            $('.nav-tabs a[href="#forgot"]').tab('show');

        });


    });

    $('#accountPopupContainer, #corporateAccountPopupContainer').delegate('#loginForm', 'submit', function (event) {
        debugger;
        event.preventDefault();
        
        var currentForm = $(this);
        if (currentForm.valid()) {
            resetError(currentForm);
            startSpinning();
            var returnUrl = window.location.href;
            var frmData = currentForm.serializeArray();

            //var returnUrlVal = getParameterByName('ReturnUrl', returnUrl);
            delete frmData[1];
            //debugger;
            if ($('#hdnLoginReturnUrl').val() != undefined && $('#hdnLoginReturnUrl').val() != '') {

                if (returnUrl.toString().includes("ResetPassword")) {
                    frmData.push({ name: 'returnUrl', value: window.location.origin });
                }
                else {
                    frmData.push({ name: 'returnUrl', value: $('#hdnLoginReturnUrl').val() });
                }
            }
            else {
                frmData.push({ name: 'returnUrl', value: returnUrl });
            }


            $.post($(this).attr('action'), frmData).done(function (data) {

                if (data.status == false && data.errors) {
                    resetError(currentForm);
                    $('#loginPassword').val('').focus();
                    $('#loginErrors').html(data.errors);
                    stopSpinning();
                }
                else if (data.status == true && data.isCorporateRequested) {


                    $.removeCookie('ofr.corporate.required.refresh.click');

                    $.cookie("ofr.corporate.required.refresh.click", true, { expires: 1, path: '/' });

                    location.reload();

                    return false;


                    //CheckCorporateAndOpenModal();

                    //// Get cookie details and go to corporate search
                    //var searchDetails = JSON.parse($.cookie('ofr.corporate.searchdata'));
                    ////window.location.href = '/Corporate/Search?cid=' + searchDetails.cid + '&ctype=' + searchDetails.ctype + '&cname=' + searchDetails.cname + '&sCheckIn=' + searchDetails.sCheckIn + '&sCheckOut=' + searchDetails.sCheckOut + '&sRoomData=' + searchDetails.sRoomData;
                    //window.location.href = '/Search/Index?cid=' + searchDetails.cid + '&ctype=' + searchDetails.ctype + '&cname=' + searchDetails.cname + '&sCheckIn=' + searchDetails.sCheckIn + '&sCheckOut=' + searchDetails.sCheckOut + '&sRoomData=' + searchDetails.sRoomData;

                }
                else if (data.status == true && data.returnUrl) {

                    //debugger;

                    //window.location = data.returnUrl;
                    //location.reload();
                    var completeInfo = 'IP Address :' + ipAddress + ', Place Details : ' + placeDetail + ', Url : ' + window.location.href;

                    ga('send', {
                        hitType: 'event',
                        eventCategory: 'Login Form Submit',
                        eventAction: completeInfo,
                        eventLabel: 'Page View'
                    });

                    if (data.returnUrl.length > 200) {
                        window.location = data.returnUrl;
                        location.reload();
                    }
                    else {
                        //debugger;
                        window.location.href = data.returnUrl;
                    }

                    ////location.reload();
                    //window.location.href = data.returnUrl;
                    ////window.location.replace(data.returnUrl)

                }
                else if (data.status == true) {

                    location.href = '/';
                    //return false;
                }
                else {
                    location.reload();
                }

            }).fail(function (data) {

                resetError(currentForm);
                $('#loginErrors').html('An error occured !');
                stopSpinning();

            }).always(function () {
                // stopSpinning();
            });
        }
    });

    $('#accountPopupContainer, #corporateAccountPopupContainer').delegate('#registerForm', 'submit', function (event) {

        event.preventDefault();
        resetError(currentForm);
        var currentForm = $(this);

        if (currentForm.valid()) {

            startSpinning();

            var frmData = currentForm.serialize();

            $.post($(this).attr('action'), frmData).done(function (data) {
                //debugger;

                if (data.status == false && data.errors) {

                    $('#registerErrorContainer').html('<div class="alert alert-warning fade in alert-dismissable" style="margin-bottom:0">' +
                                                       '<a href="#" class="close" data-dismiss="alert" aria-label="close" title="close">×</a>' +
                                                            '<div class="red margintop10" id="registerErrors"></div>'+
                                                       '</div>');

                    $('#registerErrors').html(data.errors);
                }
                else if (data.status == false) {

                    $('#registerErrorContainer').html('<div class="alert alert-warning fade in alert-dismissable" style="margin-bottom:0">' +
                                                     '<a href="#" class="close" data-dismiss="alert" aria-label="close" title="close">×</a>'+
                                                          '<div class="red margintop10" id="registerErrors"></div>'+
                                                     '</div>');

                    $('#registerErrors').html('<li>' + data.message + '</li>');
                    $('#ConfirmPassword').val('');
                    $('#registerPassword').val('');
                }
                else {

                    //debugger;

                    if (data.isCorporateRequested) {

                        $.removeCookie('ofr.corporate.required.refresh.click');

                        $.cookie("ofr.corporate.required.refresh.click", true, { expires: 1, path: '/' });

                        location.reload();

                        return false;
                    }
                    else {
                        $('#accountPopupContainer').html(data);
                    }

                    $.validator.unobtrusive.parse($('#verifyPhoneNumberForm'));

                    var completeInfo = 'IP Address :' + ipAddress + ', Place Details : ' + placeDetail + ', Url : ' + window.location.href;

                    ga('send', {
                        hitType: 'event',
                        eventCategory: 'Register Form Submit',
                        eventAction: completeInfo,
                        eventLabel: 'Page View'
                    });

                }

            }).fail(function (data) {

                resetError(currentForm);
                $('#registerErrors').html('An error occured !');

            }).always(function () {
                stopSpinning();
            });
        }

    });

    $('#accountPopupContainer, #corporateAccountPopupContainer').delegate('#forgotPasswordForm', 'submit', function (event) {

        event.preventDefault();
        var currentForm = $(this);

        if (currentForm.valid()) {

            startSpinning();

            var frmData = currentForm.serialize();
            $.post($(this).attr('action'), frmData).done(function (data) {

                if (data && data.errors) {

                    //$('#forgotPasswordError').html(data.errors);
                    //$('#corporateAccountPopupContainer #forgotPasswordError').html(data.errors);
                    toastr.error(data.errors);

                }
                else if (data && data.status == false) {
                    //$('#forgotPasswordError').html('<li>' + data.message + '</li>');
                    //$('#corporateAccountPopupContainer #forgotPasswordError').html('<li>' + data.message + '</li>');
                    toastr.error(data.message);
                }

                else if (data && data.status == true) {
                    //debugger;
                    toastr.success(data.message);
                    currentForm.each(function () {
                        this.reset();
                    });

                    //$('#accountPopupContainer').html(data);

                    //$('#forgotPasswordError').html('');
                    //$('#forgotPasswordSuccess').text(data.message);

                }
                else {
                    $('#accountPopupContainer').html(data);

                    console.log('Go to implementation block')

                }

            }).fail(function (data) {

                $('#forgotPasswordError').html('Error in data loading...');
                $('#corporateAccountPopupContainer #forgotPasswordError').html('Error in data loading...');

            }).always(function () {
                stopSpinning();
            });
        }
    });

    $('#accountPopupContainer, #corporateAccountPopupContainer').delegate('#lnkReferralCode', 'click', function (event) {

        $('#txtReferralCode').slideToggle(function () {
            if ($(this).is(":hidden")) {
                $('#txtReferralCode').val(null);
                $('#txtReferralCode').rules('remove');
                $('#txtReferralCode-error').text('');
            }
            else {
                $("#txtReferralCode").rules("add", {
                    minlength: 8,
                    messages: {
                        minlength: jQuery.validator.format("Code must be of 8 digit.")
                    }
                });
            }
        });

    });

    $('#accountPopupContainer').delegate('#verifyPhoneNumberForm', 'submit', function (event) {

        event.preventDefault();
        var currentForm = $(this);
        if (currentForm.valid()) {

            startSpinning();

            var frmData = currentForm.serialize();
            $.post($(this).attr('action'), frmData).done(function (data) {

                if (data.status == false && data.errors) {

                    $('#phoneVerificationErrors').html(data.errors);
                }
                else if (data.status == false) {
                    $('#otpMessage').text(data.message);
                }
                else if (data.status == true && data.isCorporateRequested) {

                    CheckCorporateAndOpenModal();

                    //// Get cookie details and go to corporate search
                    //var searchDetails = JSON.parse($.cookie('ofr.corporate.searchdata'));
                    ////window.location.href = '/Corporate/Search?cid=' + searchDetails.cid + '&ctype=' + searchDetails.ctype + '&cname=' + searchDetails.cname + '&sCheckIn=' + searchDetails.sCheckIn + '&sCheckOut=' + searchDetails.sCheckOut + '&sRoomData=' + searchDetails.sRoomData;
                    //window.location.href = '/Search/Index?cid=' + searchDetails.cid + '&ctype=' + searchDetails.ctype + '&cname=' + searchDetails.cname + '&sCheckIn=' + searchDetails.sCheckIn + '&sCheckOut=' + searchDetails.sCheckOut + '&sRoomData=' + searchDetails.sRoomData;

                }
                else if (data.status == true) {

                    window.location = "/";
                }

                else {
                    currentForm.each(function () {
                        this.reset();
                    });
                    $('#phoneVerificationErrors').html('<li>An error has occured !</li>');
                }

            }).fail(function (data) {

                $('#phoneVerificationErrors').html('Error in data loading!');

            }).always(function () {
                stopSpinning(currentForm);
            });
        }
    });

    $('#accountPopupContainer').delegate('#lnkResendVerificationCode', 'click', function (event) {

        event.preventDefault();
        var $this = $(this);
        $('#txtPhoneNumber-error').text('');
        startSpinning();

        $.post($this.attr('href'), {}).done(function (data) {

            if (data && data.status == false) {
                $('#phoneVerificationErrors').html('<li>' + data.message + '</li>');
            }
            else {
                $('#txtPhoneNumber').val('');
                $('#otpMessage').text('OTP resent to your phone !');
                $('#phoneVerificationEncoded').val(data.encodedCode);
            }

        }).fail(function (error) {

            $('#phoneVerificationErrors').html(error.message);

        }).always(function () {
            stopSpinning();
        });
    });

    $('#accountPopupContainer, #corporateAccountPopupContainer').delegate('#frmExternalLogin', 'submit', function (event) {
        startSpinning();

        return true;
    });

    $('#lnkResendOTP').click(function (e) {

        e.preventDefault();

        $('#PhoneVerificationCode').val('');

        $.post('/Account/ResendVerification', { phone: $('#txtPhoneNumber').val() }).done(function (data) {

            if (data && data.status == false) {
                $('#confirmExternalLoginErrors').html('<li>' + data.message + '</li>');
            }
            else {

                $('#confirmExternalLoginErrors').text('OTP Resent to your phone (' + $('#txtPhoneNumber').val() + ') !');
                $('#phoneVerificationEncoded').val(data.encodedCode);
            }

        }).fail(function (error) {

            $('#confirmExternalLoginErrors').html(error.message);

        }).always(function () {
            stopSpinning();
        });
    })

    $('#lnkReferralCodeExt, #corporateAccountPopupContainer').click(function (event) {

        $('#txtReferralCodeExt').slideToggle(function () {
            if ($(this).is(":hidden")) {
                $('#txtReferralCodeExt').val(null);
                $('#txtReferralCodeExt').rules('remove');
                $('#txtReferralCodeExt-error').text('');
            }
            else {
                $("#txtReferralCodeExt").rules("add", {
                    minlength: 8,
                    messages: {
                        minlength: jQuery.validator.format("Code must be of 8 digit.")
                    }
                });
            }
        });

    });

    $('#frmConfirmExternalLogin').submit(function (event) {

        event.preventDefault();

        var currentForm = $(this);

        if (currentForm.valid()) {

            startSpinning();
            var returnUrl = window.location.href;
            var frmData = currentForm.serialize();

            $.post($(this).attr('action'), frmData).done(function (data) {

                stopSpinning();

                if (data.status == true && data.phoneVerificationCode) {

                    $('#confirmExternalLoginErrors').html(data.message);
                    $("#PhoneNumber").attr("readonly", "true");
                    $('#PhoneVerificationEncoded').val(data.phoneVerificationCode);
                    $('#PhoneVerificationCode').slideDown();
                    $('#lnkReferralCodeExt').slideDown();
                    $('#PhoneVerificationCode').prop('required', true);
                    $('#lblResendOTP').show();
                }
                else if (data.status == false) {

                    $('#confirmExternalLoginErrors').html(data.message);
                }
                else if (data.status == true && data.returnUrl) {

                    window.location.href = data.returnUrl;
                }
                else if (data.status == true) {

                    window.location.href = '/';
                }

            }).fail(function (data) {

                stopSpinning();
                $('#confirmExternalLoginErrors').html('An error occured !');

            }).always(function () {
                //stopSpinning();
            });
        }
    });


    function startSpinning(form) {
        $('#spinnerContainer').waitMe({
            effect: 'ios',
            text: 'Please wait...',
            bg: 'rgba(255,255,255,0.6)',
            color: '#000',
            opacity: 0.8
        });
    }

    function stopSpinning(form) {
        $('#spinnerContainer').waitMe('hide')
    }

});

function resetError() {

    try {
        $('#loginErrors').html('');
        $('#registerErrors').html('');

    } catch (e) { }

    $('form').each(function (index, element) {
        clearValidation(element);
    });
};

function clearValidation(formElement) {
    //Internal $.validator is exposed through $(form).validate()
    var validator = $(formElement).validate();
    //Iterate through named elements inside of the form, and mark them as error free
    $('[name]', formElement).each(function () {
        validator.successList.push(this);//mark as error free
        validator.showErrors();//remove error messages if present
    });
    validator.resetForm();//remove error class on name elements and clear history
    validator.reset();//remove all error and success data
}

function getParameterByName(name, url) {
    if (!url) {
        url = window.location.href;
    }
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}