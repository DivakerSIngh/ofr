

$(function () {


    $(".accordion").click(function () {
        $(this).next(".lightbluebg").slideToggle(500);
    });


    $(".profileDatepicker").datepicker({
        dateFormat: 'dd/mm/yy',
        endDate: '+0d',
        autoclose: true,
        todayHighlight: true
    });

    $('#txtExpiryDate').datepicker({
        dateFormat: 'dd/mm/yy',
        autoclose: true,
        todayHighlight: true
    });


    $("#governmentApprovedIdImageInput").change(function () {

        var fileExtension = ['jpeg', 'jpg', 'png', 'gif', 'bmp'];
        if ($.inArray($(this).val().split('.').pop().toLowerCase(), fileExtension) == -1) {
            $(this).val(null);
            alert("Only '.jpeg','.jpg', '.png', '.gif', '.bmp' formats are allowed.");
            return false;
        }
        readURL(this, 'governmentApprovedIdImagePreview');
    });

    $("#profileImageInput").change(function () {

        var fileExtension = ['jpeg', 'jpg', 'png', 'gif', 'bmp'];
        if ($.inArray($(this).val().split('.').pop().toLowerCase(), fileExtension) == -1) {
            $(this).val(null);
            alert("Only '.jpeg','.jpg', '.png', '.gif', '.bmp' formats are allowed.");
            return false;
        }
        readURL(this, 'profilepicPreview');
    });


    $('#frmUserProfile').submit(function (event) {

        event.preventDefault();

        var form = $(this);

        SubmitData(form, function (data) {

            if (data.phoneVerify == true) {
                GetPartialsView('/Manage/VerifyPhoneNumber', { phoneNumber: $('#PhoneNumber').val() }, '#frmUserProfile', function (data) {

                    if (data) {
                        $('#genericModalPopupContainer').html(data);
                        $('#genericModalPopupDiv').modal('show');
                        $.validator.unobtrusive.parse($('#frmVerifyPhoneNumber'));
                    }
                    else {
                        toastr.error(error.responseText, '', toasterOptions)
                    }
                });
            }
            else if(data.status==false)
            {
                toastr.error(data.message, '', toasterOptions)
            }
            else {
                toastr.success(data.message,'',toasterOptions)
                $('#userProfileImage').attr('src', data.profileImageUrl);
            }
        });
    });

    $('#genericModalPopupContainer').delegate('#frmVerifyPhoneNumber', 'submit', function (event) {

        event.preventDefault();
        var form = $(this);

        SubmitData(form, function (data) {
            if (data.status == true) {

                SubmitData($('#frmUserProfile'), function (profileData) {
                    $('#genericModalPopupDiv').modal('hide');
                    if (profileData.status == true) {
                        toastr.success(profileData.message)
                        $('#userProfileImage').attr('src', data.profileImageUrl);
                    }
                    else if (profileData.status == false) {
                        toastr.error(profileData.message)
                    }
                });
            }
            else if (data.status == false) {
                $('#verifyPhoneNumberError').html(data.message)
                toastr.error(data.message)
            }
        });
    });


    $('#frmGovernmentApprovedId').submit(function (e) {
        e.preventDefault();
        switch ($('#governMentIdType').val()) {
            case '2': { $('#txtExpiryDate').rules("add", "required"); break; };
            case '3': { $('#txtExpiryDate').rules("add", "required"); break; };
            default: { $('#txtExpiryDate').rules("remove", "required"); $('#txtExpiryDate-error').html(''); break; };
        }

        SubmitData($(this), function (data) {
            if (data.status == true) {
                toastr.success(data.message)
            }
            else if (data.status == false) {
                toastr.error(data.message)
            }
        });
    });

    $('#frmUserPreference').submit(function (e) {
        e.preventDefault();
        var form = $(this);
        SubmitData(form, function (data) {
            if (data.status == true) {
                toastr.success(data.message)
            }
            else if (data.status == false) {
                toastr.error(data.message)
            }
        });
    });


    $('#frmSetPassword').submit(function (e) {
        e.preventDefault();
        var form = $(this);
        SubmitData(form, function (data) {
            form[0].reset();
            if (data.status == true) {
                toastr.success(data.message)
            }
            else if (data.status == false) {
                toastr.error(data.message)
            }
        });
    });

    $('#frmChangePassword').submit(function (e) {
        e.preventDefault();
        var form = $(this);
        SubmitData(form, function (data) {
            form[0].reset();
            if (data.status == true) {
                toastr.success(data.message)
            }
            else if (data.status == false) {
                toastr.error(data.message)
            }
        });
    });


    $("#ddlCountry").change(function () {

        var ddlCountry = "#ddlCountry";
        var ddlState = "#ddlState";
        var ddlCity = "#ddlCity";

        if ($(ddlCountry).val() != '') {

            var url = '/Manage/BindStates';
            $.getJSON(url, { countryId: $(ddlCountry).val() }, function (data) {

                $(ddlState).empty();
                $(ddlCity).empty();
                $(ddlState).append("<option value='-1'>Select State</option>");
                $.each(data.results, function (index, optionData) {
                    $(ddlState).append("<option value='" + optionData.iStateId + "'>" + optionData.sState + "</option>");
                });
            });
        }
        else {

            $(ddlState).empty();
            $(ddlCity).empty();
        }
    });

    $("#ddlState").change(function () {

        var ddlState = "#ddlState";
        var ddlCity = "#ddlCity";

        if ($(ddlState).val() != '') {
            var url = '/Manage/BindCity';
            $.getJSON(url, { stateId: $(ddlState).val() }, function (data) {

                $(ddlCity).empty();
                $(ddlCity).append("<option value='-1'>Select City</option>");
                $.each(data.results, function (index, optionData) {
                    $(ddlCity).append("<option value='" + optionData.sCity + "'>" + optionData.sCity + "</option>");
                });
            });
        }
        else {
            $(ddlCity).empty();
        }
    });

    $('.cancel').click(function () {

        $(this).resetValidation();
    })


    $('#governMentIdType').change(function () {

        switch ($(this).val()) {
            case '2':
            case '3': { $('#dvExpiryDate').show(); break; };
            default: { $('#dvExpiryDate').hide(); break; };
        }
    });

});
