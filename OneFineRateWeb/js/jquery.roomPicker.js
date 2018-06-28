
// Author : Aditya kumar (senior developer, Futuresoft India Pvt .Ltd, Noida) -->

(function ($) {

    "use strict";

    $.fn.plusMinusNumber = function (options) {

        var settings = $.extend({
            upClass: 'btnPlus',
            downClass: 'btnMinus',
            min: 0,
            max: 10,
            onStep: function() { },
            stepUp: function() { },
            stepDown: function() { },
            onMax: function() { },
            onMin: function() { },
        }, options);

        return this.each(function (e) {

            var self = $(this);
            var clone = self.clone();

            var min = settings.min;
            var max = settings.max;

            var element_attr_min = self.attr('min');
            var element_attr_max = self.attr('max');

            if (element_attr_min && element_attr_max) {

                min = parseInt(element_attr_min);
                max = parseInt(element_attr_max);
            }

            //clone.attr('readonly', true);

            function setText(n) {

                if (n < min) {
                    return false;
                }
                else if (n > max) {
                    return false;
                }

                if (n == min) {
                    var result = settings.onMin(clone);
                    if (result == false) {
                        return false;
                    }
                }
                else if (n == max) {
                    var result = settings.onMax(clone);
                    if (result == false) {
                        return false;
                    }
                }

                clone.val(n);
                return true;
            }

            var group = $("<div class='input-group'></div>");

            var down = $("<button type='button'>-</button>").attr('class', 'btn btn-default ' + settings.downClass).click(function () {

                var value = clone.val() == '<1' ? 0 : clone.val();

                var result = setText(parseInt(value) - 1);
                if (result == false) {
                    $(this).prop('disabled', true);
                }
                else {
                    $(this).prop('disabled', false);
                }

                clone.parent().find(`.${settings.upClass}`).prop('disabled', false);

            settings.onStep(clone);

            if (result == true)
                settings.stepDown(clone);
        });

        var up = $("<button type='button'>+</button>").attr('class', 'btn btn-default ' + settings.upClass).click(function () {

            var value = clone.val() == '<1' ? 0 : clone.val();

            var result = setText(parseInt(value) + 1);
            if (result == false) {
                $(this).prop('disabled', true);
                return false;
            }
            else {
                $(this).prop('disabled', false);
            }

            clone.parent().find(`.${settings.downClass}`).prop('disabled', false);

        settings.onStep(clone);
        settings.stepUp(clone);
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

    self.replaceWith(group);
});
};

$.fn.roomPicker = function (options) {

    var settings = $.extend({
        id: 1,
        seedData: [],
        onDataUpdate : function(){}
    }, options);

    return this.each(function (e) {

        var self = $(this);

        //self.attr('readonly', true).css( 'cursor', 'pointer');
        self.css( 'cursor', 'pointer');

        var selfId = self.attr('id');
        

        if(settings.seedData)
        {
            var numberOfGuests = 0;

            var numberOfRooms = parseInt(settings.seedData.length);

            for (var i = 0; i < settings.seedData.length; i++) {
                  
                numberOfGuests +=  parseInt(settings.seedData[i].Adults.length) + parseInt(settings.seedData[i].Children.length);
            }
           
            var roomText = "Room";

            if(numberOfRooms > 1)
            {
                roomText = "Rooms";
            }

            self.html(`<span id="spnGuestCount${selfId}">${numberOfGuests}</span>
                      <span id="spnGuestText${selfId}">Guests</span> in
                      <span id="spnRoomCount${selfId}">${numberOfRooms}</span>
                      <span id="spnRoomText${selfId}">${roomText}</span>`);
        }
        else
        {
            self.html(`<span id="spnGuestCount${selfId}">3</span>
                      <span id="spnGuestText${selfId}">Guests</span> in
                      <span id="spnRoomCount${selfId}">1</span>
                      <span id="spnRoomText${selfId}">Room</span>`);
        }
      

        var guestDetailsData = {

            Rooms: [],

            GetRoomData: function () {

                this.Rooms = [];

                if(settings.seedData && settings.seedData.length > 0)
                {
                    for (var i = 0; i < settings.seedData.length; i++) {
    
                        this.Rooms.push({
                            RoomId: settings.seedData[i].RoomId,
                            Adults: settings.seedData[i].Adults,
                            Children: settings.seedData[i].Children
                        });
                    }
                }
                else if (this.Rooms.length < 1) {
                    this.InitializeRoom();
                }

                return this.Rooms;
            },

            InitializeRoom: function () {

                this.Rooms.push({
                    RoomId: 1,
                    Adults: [{ AdultId: 1, RoomId: 1 }, { AdultId: 2, RoomId: 1 }],
                    Children: []
                });

                settings.onDataUpdate(this.Rooms);
            },

            AddRoom: function (roomId) {

                if(roomId > 8){
                    return false;
                }

                this.Rooms.push({
                    RoomId: roomId,
                    Adults: [{ AdultId: 1, RoomId: roomId }],
                    Children: []
                });

                settings.onDataUpdate(this.Rooms);
            },

            RemoveRoom: function (roomId) {

                //TO DO
                roomId = roomId+1;
                var roomIndex = this.Rooms.findIndex((r => r.RoomId == roomId));
                var room = this.Rooms[roomIndex];
                this.Rooms.splice(roomIndex, 1);

                settings.onDataUpdate(this.Rooms);
            },

            AddAdult: function (roomId, adultId) {

                var roomIndex = this.Rooms.findIndex((r => r.RoomId == roomId));
                var room = this.Rooms[roomIndex];

                room.Adults.push({ AdultId: adultId, RoomId: roomId });

                settings.onDataUpdate(this.Rooms);
            },

            RemoveAdult: function (roomId, adultId) {

                var roomIndex = this.Rooms.findIndex((r => r.RoomId == roomId));
                var room = this.Rooms[roomIndex];

                var adultIndex = room.Adults.findIndex((r => r.AdultId == adultId));
                room.Adults.splice(adultIndex, 1);

                settings.onDataUpdate(this.Rooms);
            },

            AddChild: function (roomId, childId, childAge) {

                var roomIndex = this.Rooms.findIndex((r => r.RoomId == roomId));
                var room = this.Rooms[roomIndex];

                room.Children.push({ ChildId: childId, RoomId: roomId, Age: childAge });

                settings.onDataUpdate(this.Rooms);
            },

            UpdateChild: function (roomId, childId, childAge) {

                var roomIndex = this.Rooms.findIndex((r => r.RoomId == roomId));
                var room = this.Rooms[roomIndex];

                var childIndex = room.Children.findIndex((r => r.ChildId == childId));
                var child = room.Children[childIndex];
                child.Age = childAge;

                settings.onDataUpdate(this.Rooms);
            },

            RemoveChild: function (roomId, childId) {
                
                var roomIndex = this.Rooms.findIndex((r => r.RoomId == roomId));
                var room = this.Rooms[roomIndex];

                var childIndex = room.Children.findIndex((r => r.ChildId == childId));
                room.Children.splice(childIndex, 1);

                settings.onDataUpdate(this.Rooms);
            },
        };

        self.popover({

            html: true,
            content: function () {
                return GetPopoverTemplete(selfId);
            }
        });        

        self.on('shown.bs.popover', function () {

            InitializePopoverAdditionals(selfId);

        });

        function InitializePopoverAdditionals(roomPickerId) {

            var $txt_adult = $(`#rooms_container${roomPickerId} .plus_minus_input_adult`);
        var $txt_child = $(`#rooms_container${roomPickerId} .plus_minus_input_child`);
    var $txt_child_age = $(`#rooms_container${roomPickerId} .plus_minus_input_child_age`);

CreateAdultPlusMinus($txt_adult, roomPickerId);
CreateChildPlusMinus($txt_child, roomPickerId);
CreateChildAgePlusMinus($txt_child_age, roomPickerId);

$(`#spnGuestCount${roomPickerId}`).off('change').on('change', function () {

    var val = $(this).text();
    if (parseInt(val) > 1) {
        $(`#spnGuestText${roomPickerId}`).text('Guests')
}
else {
                        $(`#spnGuestText${roomPickerId}`).text('Guest')
}
});

$(`#spnRoomCount${roomPickerId}`).off('change').on('change', function () {
    var val = $(this).text();
    if (parseInt(val) > 1) {
        $(`#spnRoomText${roomPickerId}`).text('Rooms');
}
else {
                        $(`#spnRoomText${roomPickerId}`).text('Room');
}
});

$(`#btnDone${roomPickerId}`).off('click').on('click', function () {

    console.log(guestDetailsData.GetRoomData());
    $('#divPopoverGuestDetails').popover('hide');

});

$(`#addRoom${roomPickerId}`).off('click').on('click', function () {

    var numberOfRoomsAdded = $(`#rooms_container${roomPickerId} .room`).length;

if (numberOfRoomsAdded < 8) {

    var newRoomHtml = GetNewRoomTemplate(numberOfRoomsAdded + 1);

    $(`#rooms_container${roomPickerId}`).append(newRoomHtml);
}
else{
    return false;
}

$(`#removeRoom${roomPickerId}`).show();
$('#spnPipeSeperator').show();

var guestCountBefore = $(`#spnGuestCount${roomPickerId}`).text();

$(`#spnGuestCount${roomPickerId}`).text(parseInt(guestCountBefore) + 1).change();

var roomCountBefore = $(`#spnRoomCount${roomPickerId}`).text();

$(`#spnRoomCount${roomPickerId}`).text(parseInt(roomCountBefore) + 1).change();

guestDetailsData.AddRoom(parseInt(roomCountBefore) + 1);

var $child = $(`#rooms_container${roomPickerId} .room:last .plus_minus_input_child`);

var $adult = $(`#rooms_container${roomPickerId} .room:last .plus_minus_input_adult`);

CreateAdultPlusMinus($adult, roomPickerId);

CreateChildPlusMinus($child, roomPickerId);

$(`#rooms_container${roomPickerId}`).animate({ scrollTop: $(`#rooms_container${roomPickerId}`).prop("scrollHeight") }, 0);

});

$(`#removeRoom${roomPickerId}`).off('click').on('click', function () {

    var numberOfRoomsAdded = $(`#rooms_container${roomPickerId} .room`).length;

if (numberOfRoomsAdded) {

    if (numberOfRoomsAdded == 2) {
        $(`#removeRoom${roomPickerId}`).hide();
    $('#spnPipeSeperator').hide();
}

var roomCountBefore = $(`#spnRoomCount${roomPickerId}`).text();

$(`#spnRoomCount${roomPickerId}`).text(parseInt(roomCountBefore) - 1).change();

var number_of_child_in_last_room = $(`#rooms_container${roomPickerId} .room:last .plus_minus_input_child`).val();

var number_of_adult_in_last_room = $(`#rooms_container${roomPickerId} .room:last .plus_minus_input_adult`).val();

var totalGuestInThisRoom = parseInt(number_of_adult_in_last_room) + parseInt(number_of_child_in_last_room)

var guestCountBefore = $(`#spnGuestCount${roomPickerId}`).text();

$(`#spnGuestCount${roomPickerId}`).text(parseInt(guestCountBefore) - totalGuestInThisRoom).change();

guestDetailsData.RemoveRoom(parseInt(roomCountBefore) - 1);

$(`#rooms_container${roomPickerId} .room:last`).remove();

$(`#rooms_container${roomPickerId}`).animate({ scrollTop: $(`#rooms_container${roomPickerId}`).prop("scrollHeight") }, 0);
}
});

var roomCount = guestDetailsData.Rooms.length;
var guestCount = 0;

for (var i = 0; i < roomCount; i++) {

    guestCount += guestDetailsData.Rooms[i].Adults.length;
    guestCount += guestDetailsData.Rooms[i].Children.length;

}

$(`#spnRoomCount${roomPickerId}`).text(roomCount).change();

$(`#spnGuestCount${roomPickerId}`).text(guestCount).change();

}

function GetPopoverTemplete(roomPickerId) {

    var rooms = guestDetailsData.GetRoomData();

    var popover_template = `<div class="form-group"><div class="row roompicker" id="rooms_container${roomPickerId}" style="max-height:250px; overflow-y:auto">`;

    for (var i = 0; i < rooms.length; i++) {

        var room_templete = `<div class="room">

        <div class="col-md-12" style= "margin-top:10px" ><b> Room ${ rooms[i].RoomId} :</b></div>
            <div class="col-md-6 col-xs-6">
                <b> Adult </b>
                <br />
                <small>Above 12 years</small>
                <br />
                <input type="text" readonly data-roompickerid="${roomPickerId}" data-roomid="${rooms[i].RoomId}" class="form-control plus_minus_input_adult" value="${rooms[i].Adults.length}"/>
            </div>
            <div class="col-md-6 col-xs-6">
                <b>Children</b>
                <br />
                <small>Below 12 years</small>
                <br />
                <input type="text" data-roompickerid="${roomPickerId}" data-roomid="${ rooms[i].RoomId}" readonly class="form-control plus_minus_input_child" value="${rooms[i].Children.length}"/>
            </div>
            <div class="room_child_age_container" data-roomid="${ rooms[i].RoomId}" style="margin-top:10px;">`;

        for (var j = 0; j < rooms[i].Children.length; j++) {

            if (j == 0) {

                room_templete += `<div class="col-md-12  age_text_of_children_container" style="margin-top:10px;"><b>Age of Children</b></div>`;
            }

            var childAge = rooms[i].Children[j].Age == 0 ? '<1' : rooms[i].Children[j].Age;

            room_templete += `<div class="col-md-6 col-xs-6" style="margin-top:10px;"><input type="text" data-roompickerid="${roomPickerId}" data-childid="${rooms[i].Children[j].ChildId}" data-roomid="${rooms[i].Children[j].RoomId}" readonly class="form-control plus_minus_input_child_age" value="${childAge}" /></div>`;

        }

        room_templete += `</div></div >`;

        popover_template += room_templete;
    }

    popover_template += `</div >
     <div class="row">
         <div class="col-md-12 " style="margin-top:10px">
             <a href="javascript:void(0)" id="addRoom${roomPickerId}">Add Room</a>`;

    if (rooms.length > 1) {
        popover_template += `<span id="spnPipeSeperator"> | </span><a href="javascript:void(0)" id="removeRoom${roomPickerId}">Remove Room</a>`;
    }
    else {
        popover_template += `<span id="spnPipeSeperator" style="display:none"> | </span><a href="javascript:void(0)" style="display:none" id="removeRoom${roomPickerId}">Remove Room</a>`;
    }

    popover_template += `</div>
           <div class="col-md-12">
               <hr />
               <button type="button" id="btnDone${roomPickerId}" class="btn btn-info" style="width:100%">Done</button>
           </div>
       </div>
     </div>`;

    return popover_template;
};

function CreateAdultPlusMinus($element, roomPickerId) {

    $element.plusMinusNumber({
        min: 1,
        max: 6,
        onMax: function ($this) {

        },
        onMin: function ($this) {

        },
        stepDown: function ($this) {
            
            var value = $this.val();

            var guestCountBefore = $(`#spnGuestCount${roomPickerId}`).text();

    $(`#spnGuestCount${roomPickerId}`).text(parseInt(guestCountBefore) - 1).change();

var roomId = parseInt($this.data('roomid'));
var adultId = parseInt(value);

guestDetailsData.RemoveAdult(roomId, adultId + 1);
},
stepUp: function ($this) {

    var value = $this.val();

    var guestCountBefore = $(`#spnGuestCount${roomPickerId}`).text();

$(`#spnGuestCount${roomPickerId}`).text(parseInt(guestCountBefore) + 1).change();

var roomId = parseInt($this.data('roomid'));
var adultId = parseInt(value);

guestDetailsData.AddAdult(roomId, adultId);
}
});
}

function CreateChildPlusMinus($element, roomPickerId) {

    $element.plusMinusNumber({
        min: 0,
        max: 4,
        onMax: function ($this) {

        },
        onMin: function ($this) {

        },
        stepDown: function ($this) {

            var elem = $this.parent().parent().next('.room_child_age_container');

            var col6Div = elem.find('.col-md-6');

            var lastCol6Div = elem.find('.col-md-6:last');

            if (col6Div.length == 1) {

                elem.find('.age_text_of_children_container').remove();
            }

            lastCol6Div.remove();

            var guestCountBefore = $(`#spnGuestCount${roomPickerId}`).text();

    $(`#spnGuestCount${roomPickerId}`).text(parseInt(guestCountBefore) - 1).change();

var txtChildAge = elem.find('.plus_minus_input_child_age:last');

var roomId = parseInt($this.data('roomid'));
var childId = parseInt(txtChildAge.data('childid'));

guestDetailsData.RemoveChild(roomId, childId + 1);
},
stepUp: function ($this) {

    var guestCountBefore = $(`#spnGuestCount${roomPickerId}`).text();

$(`#spnGuestCount${roomPickerId}`).text(parseInt(guestCountBefore) + 1).change();

var elem = $this.parent().parent().next('.room_child_age_container');

var value = $this.val();

if (value == 1) {

    elem.append('<div class="col-md-12 age_text_of_children_container" style="margin-top:10px;"><b>Age of Children</b></div>')
}

elem.append('<div class="col-md-6 col-xs-6" style="margin-top:10px;"><input type="text" data-childid="' + value + '" data-roomid="' + elem.data('roomid') + '" readonly class="form-control plus_minus_input_child_age" value="0" /></div>');

var txtChildAge = elem.find('.plus_minus_input_child_age:last');

var roomId = parseInt($this.data('roomid'));
var childId = parseInt(txtChildAge.data('childid'));

var childAge = parseInt(txtChildAge.val());

guestDetailsData.AddChild(roomId, childId, childAge);

txtChildAge.val('<1');

CreateChildAgePlusMinus($(txtChildAge), roomPickerId);
}
});
}

function CreateChildAgePlusMinus($element, roomPickerId) {

    $element.plusMinusNumber({
        min: 0,
        max: 12,
        onMax: function ($this) {

        },
        onMin: function ($this) {
            $($this).val('<1');
            return false;
        },
        onStep: function ($this) {

            var roomId = parseInt($this.data('roomid'));
            var childId = parseInt($this.data('childid'));
            var childAge = parseInt($this.val());

            guestDetailsData.UpdateChild(roomId, childId, childAge);
        },
    });
}

function GetNewRoomTemplate(roomId) {

    var room_templete = `<div class="room">
     <div class="col-md-12" style= "margin-top:10px" ><b> Room ${ roomId} :</b></div >
         <div class="col-md-6 col-xs-6">
             <b> Adult </b>
             <br />
             <small>Above 12 years</small>
             <br />
             <input type="text" readonly data-roomid="${ roomId}" class="form-control plus_minus_input_adult" value="1" />
         </div>
         <div class="col-md-6 col-xs-6">
             <b>Children</b>
             <br />
             <small>Below 12 years</small>
             <br />
             <input type="text" data-roomid="${ roomId}" readonly class="form-control plus_minus_input_child" value="0" />
         </div>
         <div class="room_child_age_container" data-roomid="${ roomId}" style="margin-top:10px;">
         </div>
     </div >`;

    return room_templete;
}

});
};

}(jQuery));