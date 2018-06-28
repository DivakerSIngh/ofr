
var notH = 1,
    $popcommon = $('#divwhole_Co').hover(function () { notH ^= 1; });

$(document).on('mousedown keydown', function (e) {
    if (notH || e.which == 27) $popcommon.stop().fadeOut();
});

/////// CALL POPUP 
$('#txttotalcount_Co').click(function () {
    var isDisabled = $('#txttotalcount_Co').attr('disabled');
    if (isDisabled != 'disabled') {
        $popcommon.stop().toggle('show');
    }
});

(function ($) {

    //jQuery('#divwhole').toggle('hide');
    //jQuery('#txttotalcount').on('click', function (event) {
    //    jQuery('#divwhole').toggle('show');
    //});

    $.fn.bootstrapNumber_Common = function (options) {

        var settings = $.extend({
            upClass: 'default',
            downClass: 'default',
            center: true
        }, options);

        return this.each(function (e) {

            var self = $(this);
            var clone = self.clone();

            var min = self.attr('min');
            var max = self.attr('max');
            var itype = self.attr('itype');
            var roomno = self.attr('roomno');
            var childhtml = '';
            var roomcount = '';
            var acount = 0;
            var ccount = 0;


            function setText(n, ctype) {

                if (itype == 'childage') {
                    if (n == 0) {
                        n = '<1';
                        clone.focus().val(n);
                        return true;
                    }
                }

                if ((min && n < min) || (max && n > max)) {
                    return false;
                }

                roomcount = roomno//$('#roomContainerDiv div.rooms').length;  //fetching total no of rooms

                if (itype == 'children' + roomcount + '') {     //on click event of children
                    var i = $('#Co_ulchilds' + roomcount + ' li.childli').length + 1;
                    if (ctype == 'Forward') {   //if adding no of chilrens
                        childhtml = '';
                        childhtml += ' <li id="childli' + i + '' + roomcount + '" class="childli"><input id="Co_child' + i + '' + roomcount + '" itype="childage" class="form-control child showdata" readonly="readonly" type="text" value="<1" min="1" max="11" /> </li>';
                        $('#Co_ulchilds' + roomcount + '').append(childhtml);
                        $('#Co_child' + i + '' + roomcount + '').bootstrapNumber_Common();
                    }
                    else {  //if removing no of chilrens
                        var d = i - 1;
                        $('#childli' + d + '' + roomcount + '').remove();
                    }

                    ////debugger;
                    i = $('#Co_ulchilds' + roomcount + ' li.childli').length;

                    //placing validation of maximum 6 persons in a room , having maximum 4 adults
                    var adultcount = 10 - i;
                    if (adultcount > 6) {
                        $('#Co_adult' + roomcount + '').removeAttr('max');
                        $('#Co_adult' + roomcount + '').attr('max', 6)
                    }
                    else {
                        $('#Co_adult' + roomcount + '').removeAttr('max');
                        $('#Co_adult' + roomcount + '').attr('max', adultcount)
                    }

                    //show child age div (Maping age of the childrens)
                    if (i > 0)
                        $('#Co_divChildAge' + roomcount + '').show();
                    else
                        $('#Co_divChildAge' + roomcount + '').hide();

                }

                ////debugger;
                //on click event of adults
                if (itype == 'adult' + roomcount + '') {

                    if (ctype == 'Forward') //if adding no of adults
                        adultcount = parseInt(clone.val()) + 1;
                    else
                        adultcount = parseInt(clone.val()) - 1;
                    i = $('#Co_ulchilds' + roomcount + ' li.childli').length;

                    ////debugger;
                    //placing validation of maximum 6 persons in a room 
                    var childcount = 10 - (adultcount);
                    if (childcount > 6) {
                        $('#Co_children' + roomcount + '').removeAttr('max');
                        $('#Co_children' + roomcount + '').attr('max', 6)
                    }
                    else {
                        $('#Co_children' + roomcount + '').removeAttr('max');
                        $('#Co_children' + roomcount + '').attr('max', childcount)
                    }
                }
                setTimeout(function () {
                    Co_GetTotalCount();
                }, 100);
                clone.focus().val(n);
                return true;
            }

            var group = $("<div class='input-group'></div>");


            var down = $("<button type='button'><i class='fa fa-minus'></i></button>").attr('class', 'btn btn-' + settings.downClass).click(function () {
                //if (itype == 'adult' + roomcount + '' || itype == 'children' + roomcount + '') {   //checking click event of number of adults and childrens
                //    max = clone[0].max;    //placing maximum value of childrens and adults 
                //}

                if (clone.val() == '<1') {
                    return false;
                }
                setText(parseInt(clone.val()) - 1, 'Back');
            });

            var up = $("<button type='button'> <i class='fa fa-plus'></i></button>").attr('class', 'btn btn-' + settings.upClass).click(function (e) {
                // //debugger;
                roomcount = roomno// $('#roomContainerDiv div.rooms').length;

                if (itype == 'adult' + roomcount + '' || itype == 'children' + roomcount + '') {   //checking click event of number of adults and childrens
                    max = clone[0].max;    //placing maximum value of childrens and adults 
                }

                if (clone.val() == '<1') {
                    setText(1, 'Forward');
                }
                else {
                    setText(parseInt(clone.val()) + 1, 'Forward');
                }
            });

            $("<span class='input-group-btn'></span>").append(down).appendTo(group);
            clone.appendTo(group);
            if (clone) {
                clone.css('text-align', 'center');
            }
            $("<span class='input-group-btn'></span>").append(up).appendTo(group);

            // remove spins from original
            clone.prop('type', 'text').keydown(function (e) {
                if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
					(e.keyCode == 65 && e.ctrlKey === true) ||
					(e.keyCode >= 35 && e.keyCode <= 39)) {
                    return;
                }
                if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                    e.preventDefault();
                }

                var c = String.fromCharCode(e.which);
                var n = parseInt(clone.val() + c);

                //if ((min && n < min) || (max && n > max)) {
                //    e.preventDefault();
                //}
            });

            //clone.prop('type', 'text').blur(function (e) {

            //    var c = String.fromCharCode(e.which);
            //    var n = parseInt(clone.val() + c);
            //    if ((min && n < min)) {
            //        setText(min);
            //    }
            //    else if (max && n > max) {
            //        setText(max);
            //    }
            //});


            self.replaceWith(group);
        });
    };
    $('#adult1').bootstrapNumber_Common();
    $('#children1').bootstrapNumber_Common();
}(jQuery));

//creating dynamic room on Add room click
function Co_AddRooms() {
  
    roomcount = $('#roomContainerDiv_Co div.rooms').length + 1;
    var myval = '';

    if (roomcount == 9) {
        $('#Co_btnaddroom').hide();
        $('#Co_sppipe').hide();
        return false;
    }

    myval += '<div class="rooms">';
    myval += "<div class='col-md-12  col-xs-12 pull-left'><h5><strong>Room" + roomcount + ":</strong></h5></div>";
    myval += "<div class='col-md-6 col-xs-6 '>";
    myval += '<h5><strong>Adult</strong></h5><h6>Above 12 years</h6>';
    myval += "<div class='input-group width110'>";
    myval += '<input id="Co_adult' + roomcount + '" class="form-control adult1" itype="adult' + roomcount + '" roomno="' + roomcount + '" type="number" readonly="readonly" value="1" min="1" max="6" />';
    myval += '</div> </div>';
    myval += "<div class='col-md-6 col-xs-6'>";
    myval += '<h5><strong>Children</strong></h5><h6>Below 12 years</h6>';
    myval += "<div class='input-group width110'>";
    myval += '<input id="Co_children' + roomcount + '" class="form-control children1" roomno="' + roomcount + '" readonly="readonly" itype="children' + roomcount + '" type="number" value="0" min="0" max="6" />';
    myval += '</div></div>';
    myval += "<div class='col-md-12 col-xs-12' id='Co_divChildAge" + roomcount + "' style='display:none;'>";
    myval += ' <h5><strong>Age of Childen</strong></h5>';
    myval += '<ul style="list-style:none; padding:0; margin:0; " id="Co_ulchilds' + roomcount + '"></ul>';
    myval += '</div></div>';

    $('#roomContainerDiv_Co').append(myval);

    $('#Co_adult' + roomcount + '').bootstrapNumber_Common();
    $('#Co_children' + roomcount + '').bootstrapNumber_Common();
    //placing validation to add maximum 8 rooms
    if (roomcount == 9) {
        $('#Co_btnaddroom').hide();
        $('#Co_sppipe').hide();
    }
    if (roomcount > 1) {
        $('#Co_btnremoveroom').show();
        if (roomcount == 2)
            $('#Co_sppipe').show();
    }
    else {
        $('#Co_btnremoveroom').hide(); $('#Co_sppipe').hide();
    }

    setTimeout(function () {
        Co_GetTotalCount();
    }, 100);

    $("#roomContainerDiv_Co").animate({ scrollTop: $('#roomContainerDiv_Co').prop("scrollHeight") }, 0);
}

//remove the latest added room 
function Co_RemoveRoom() {
    roomcount = $('#roomContainerDiv_Co div.rooms').length;
    if (roomcount == 2) {
        $('#Co_btnremoveroom').hide();
        $('#Co_sppipe').hide();
    }
    else {
        $('#Co_btnaddroom').show();
        $('#Co_sppipe').show();
    }
    $("#roomContainerDiv_Co div.rooms:last").remove();

    setTimeout(function () {
        Co_GetTotalCount();
    }, 100);
}
//get total count of peoples and rooms
function Co_GetTotalCount() {
    var acount = 0;
    var ccount = 0;
    $("#roomContainerDiv_Co div.rooms").each(function () {
        acount += parseInt($(this).find("input:text.adult1").val());
        ccount += parseInt($(this).find("input:text.children1").val());
    });
    roomcount = $('#roomContainerDiv_Co div.rooms').length;
    var tcount = acount + ccount;
    //Add placeholder to div 
   
    //Aditya

    var myval = '';

    if (tcount > 1 && roomcount > 1) {

        myval = '' + tcount + ' Guests in ' + roomcount + ' Rooms';
    }
    else if (tcount > 1 && roomcount == 1) {
        myval = '' + tcount + ' Guests in ' + roomcount + ' Room';
    }
    else if (tcount == 1) {
        myval = '' + tcount + ' Guest in ' + roomcount + ' Room';
    }
    //Aditya

    $('#txttotalcount_Co').html(myval);
}


function Co_BindRoomData(data) {

    var roomcount = 0;
    if (data.length > 0) {

        $('#roomContainerDiv_Co').empty();
        $.each(data, function (key, value) {
            roomcount++;
            var myval = '';
            myval += '<div class="rooms">';
            myval += "<div class='col-md-12 col-xs-12  pull-left'><h5><strong>Room" + roomcount + ":</strong></h5></div>";
            myval += "<div class='col-md-6 col-xs-6'>";
            myval += '<h5><strong>Adult</strong></h5><h6>Above 12 years</h6>';
            myval += "<div class='input-group width110'>";
            myval += '<input id="Co_adult' + roomcount + '" class="form-control adult1" itype="adult' + roomcount + '" roomno="' + roomcount + '" type="number" readonly="readonly" value="' + value.adult + '" min="1" max="6" />';
            myval += '</div> </div>';
            myval += "<div class='col-md-6 col-xs-6'>";
            myval += '<h5><strong>Children</strong></h5><h6>Below 12 years</h6>';
            myval += "<div class='input-group width110'>";
            myval += '<input id="Co_children' + roomcount + '" class="form-control children1" readonly="readonly" roomno="' + roomcount + '" itype="children' + roomcount + '" type="number" value="' + value.child + '" min="0" max="6" />';
            myval += '</div></div>';
            if (value.ChildAge.length > 0)
                myval += "<div class='col-md-12 col-xs-12' id='Co_divChildAge" + roomcount + "'>";
            else
                myval += "<div class='col-md-12 col-xs-12' id='Co_divChildAge" + roomcount + "' style='display:none;'>";
            myval += ' <h5><strong>Age of Childen</strong></h5>';
            myval += '<ul style="list-style:none; padding:0; margin:0; " id="Co_ulchilds' + roomcount + '">';

            myval += '</ul>';
            myval += '</div></div>';

            $('#roomContainerDiv_Co').append(myval);
            var i = 1;
            $.each(value.ChildAge, function (key, val) {
                if (val.Age == '0') {
                    val.Age = '<1';
                }
                var desc = '<li id="childli' + i + '' + roomcount + '" class="childli"><input id="Co_child' + i + '' + roomcount + '" itype="childage" class="form-control child showdata" readonly="readonly" type="text" value="' + val.Age + '" min="1" max="12" /> </li>';
                $('#Co_ulchilds' + roomcount + '').append(desc);
                $('#Co_child' + i + '' + roomcount + '').bootstrapNumber_Common();
                i++;
            });

            $('#Co_adult' + roomcount + '').bootstrapNumber_Common();
            $('#Co_children' + roomcount + '').bootstrapNumber_Common();

        });

        if (roomcount == 8) {
            $('#Co_btnaddroom').hide();
            $('#Co_sppipe').hide();
        }
        if (roomcount > 1) {
            $('#Co_btnremoveroom').show(); $('#Co_sppipe').show();
        }
        else {
            $('#Co_btnremoveroom').hide(); $('#Co_sppipe').hide();
        }

        setTimeout(function () {
            Co_GetTotalCount();
        }, 100);
    }

}


//get room data for search
function Co_FetchRoomDetails() {
    var Data = [];
    var acount = 0;
    var ccount = 0;
    var childage = '';
    var i = 1;
    $("#roomContainerDiv_Co div.rooms").each(function () {

        acount = parseInt($(this).find("input:text.adult1").val());
        ccount = parseInt($(this).find("input:text.children1").val());

        var AgeArray = [];
        $("#Co_ulchilds" + i + " li.childli").each(function () {



            childage = $(this).find("input:text.child").val();
            console.log(childage)

            var parserChildage = parseInt(childage) || 0;


            AgeArray.push({ Age: parserChildage.toString() });
        });

        i++;
        Data.push({
            room: i - 1,
            adult: acount,
            child: ccount,
            ChildAge: AgeArray
        });
    });
    $("#hdnJson").val(JSON.stringify(Data));
    $("#hdnJsonNew").val(JSON.stringify(Data));
    //jQuery('#divwhole').toggle('hide');
    $popcommon.stop().fadeOut();

    if (i > 4)
        return false;
}