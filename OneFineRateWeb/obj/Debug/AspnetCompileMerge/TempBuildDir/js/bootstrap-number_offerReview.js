
(function ($) {
    var RoomRate = 0;
    var Data = [];
    var roomdata = [];

    $.fn.bootstrapNumberOffer = function (options) {

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
            var occupancy = self.attr('occupancy');
            var ACstring = self.attr('ACstring');
            var roomid = self.attr('roomid');





            function setText(n, ctype) {
                var TotalRoomNo = n - 1;
                if (TotalRoomNo == max) {
                    toastr.warning('No more room available.');
                    return false;
                }

                if ((min && n < min) || (max && n > max)) {
                    return false;
                }

                Data = OccuData;
                var catalog = Data.filter(function (item) {
                    if (item.Occupancy == occupancy)
                        return item;
                });
                var iroom = 0;
                var itotal = 0;
                iroom = parseInt(catalog[0].Rooms);
                itotal = parseInt(catalog[0].Total);
                if (ctype == 'Forward') {

                    var data = roomdata.filter(function (item) {
                        if (item.roomid == roomid)
                            return item;
                    });
                 

                    if (data.length > 0) {
                        var count = data[0].Count;
                        if (count == max) {
                            toastr.warning('No more room available.');
                            return false;
                        }
                        count = count + 1;

                        var index = roomdata.indexOf(data);
                        roomdata.splice(index, 1);

                        roomdata.push({
                            roomid: roomid,
                            Count: count,
                        });
                    }
                    else {
                        roomdata.push({
                            roomid: roomid,
                            Count: 1,
                        });
                    }

                    if (itotal < iroom) {
                        catalog[0].Total = catalog[0].Total + 1;
                    }
                    else {
                        toastr.error('The room with ' + ACstring + ' has already been selected.');
                        return false;
                    }
                }
                else {

                    var data = roomdata.filter(function (item) {
                        if (item.roomid == roomid)  
                            return item;
                    });


                    if (data.length > 0) {
                        var count = data[0].Count;

                        var index = roomdata.indexOf(data);
                        roomdata.splice(index, 1);

                        count = count - 1;
                        roomdata.push({
                            roomid: roomid,
                            Count: count,
                        });
                    }
                    else {
                        roomdata.push({
                            roomid: roomid,
                            Count: 1,
                        });
                    }


                    if (itotal != 0) {
                        catalog[0].Total = catalog[0].Total - 1;
                    }
                    else {
                        toastr.error('The room with ' + ACstring + ' has already been selected.');
                        return false;
                    }
                }

                $('#hdnoccudata').val(JSON.stringify(Data));
                setTimeout(function () {
                    getdata();
                    //var result=getdata();
                }, 100);
                //if (result)
                clone.focus().val(n);
                return true;
            }

            var group = $("<div class='input-group'></div>");
            var down = $("<button type='button'><i class='fa fa-minus'></i></button>").attr('class', 'btn btn-' + settings.downClass).click(function () {
                setText(parseInt(clone.val()) - 1, 'Back');
            });
            var up = $("<button type='button'><i class='fa fa-plus'></i></button>").attr('class', 'btn btn-' + settings.upClass).click(function () {
                setText(parseInt(clone.val()) + 1, 'Forward');
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

            clone.prop('type', 'text').blur(function (e) {
                var c = String.fromCharCode(e.which);
                var n = parseInt(clone.val() + c);
                if ((min && n < min)) {
                    setText(min);
                }
                else if (max && n > max) {
                    setText(max);
                }
            });


            self.replaceWith(group);
        });
    };
}(jQuery));
function getdata() {
    try {
        var RoomRatePrice = 0;
        var RoomRateTaxes = 0;
        var ExtraBedCharges = 0;
        var taxarray = [];
        var tax = '';
        var tarray = '';
        var tvalue = '';
        var pricetype = ''
        var pAmt = 0;
        var taxAmt = 0;
        var GrandTotal = 0;
        var totalrooms = 0;
        var leftcount = 0;
        var Occudata = [];
        var map = {};
        var dd = '';
        var mm = '';
        var yy = '';
        var checkin = $('#txtfrom').val();
        var checkout = $('#txtto').val();
        var dcheckin = new Date(checkin.split("/").reverse().join("-"));
        var dcheckout = new Date(checkout.split("/").reverse().join("-"));
        var diff = Date.parse(dcheckout) - Date.parse(dcheckin);
        var daysDiff = Math.floor(diff / 86400000);
        var rcount = 0;



        $('.txtRooms').each(function (i, txtRoom) {
            debugger;
            var RoomsNo = parseFloat($(txtRoom).val() == undefined ? 0 : $(txtRoom).val());

            if (RoomsNo > 0) {
                rcount++;
            }

            var NoOfRooms = parseFloat(RoomsNo) * daysDiff;
            var price = $(this).attr('data-price');
            var taxString = $(this).attr('data-tax');
            var ExBedNumber = $(this).attr('data-extrabednumber');
            var ExBedCharge = $(this).attr('data-extrabedcharges');
            var TotalRoomAccept = $(this).attr('totalCount');
            var TotalOccupancy = $(this).attr('occupancy');
            var roomid = $(this).attr('roomid');
            var tax = $(this).attr('dTaxes');

            RoomRatePrice += parseFloat(price) * NoOfRooms;

            $('#lblRoomRate').val(ReplaceNumberWithCommas(RoomRatePrice.toFixed(2)));
            $('#lblRoomRate_hidden').val(RoomRatePrice.toFixed(2));

            ExtraBedCharges += parseFloat(ExBedCharge) * parseFloat(NoOfRooms * ExBedNumber);
            $('#lblextrabedcharge').val(ReplaceNumberWithCommas(ExtraBedCharges.toFixed(2)));
            $('#lblextrabedcharge_hidden').val(ExtraBedCharges.toFixed(2));
            var ActualPrice = RoomRatePrice + ExtraBedCharges;
            var RoomTax = tax * parseFloat(RoomsNo);

            RoomRateTaxes = parseFloat(RoomRateTaxes) + parseFloat(RoomTax);
            //taxString.split(",").forEach(function (d) {

            //    if (d != "") {
            //        tax = d.trim();
            //        tarray = tax.split(":");
            //        tvalue = tarray[1].trim();

            //        pricetype = tvalue.slice(-1);
            //        if (tvalue.slice(-1) == "%") {
            //            pAmt = tvalue.replace('%', '');
            //            tvalue = pAmt.trim();
            //            taxAmt = parseFloat(ActualPrice) * parseFloat(tvalue) / 100;
            //            RoomRateTaxes = parseFloat(RoomRateTaxes) + (NoOfRooms * taxAmt);
            //        }
            //        else {
            //            pAmt = tvalue.substr(1);
            //            tvalue = pAmt.trim();
            //            taxAmt = parseFloat(tvalue);
            //            RoomRateTaxes = parseFloat(RoomRateTaxes) + (NoOfRooms * taxAmt);
            //        }
            //    }
            //});

            //}

        });
        var totaltax = parseFloat(RoomRateTaxes) + parseFloat(dOFRServiceCharge);
        $('#lbltaxes').val(ReplaceNumberWithCommas(totaltax.toFixed(2)));
        $('#lbltaxes_hidden').val(totaltax.toFixed(2));
        GrandTotal = parseFloat(totaltax) + parseFloat(RoomRatePrice) + parseFloat(ExtraBedCharges);
        GrandTotal = GrandTotal.toFixed(2);
        $('#lbltotal').val(ReplaceNumberWithCommas(GrandTotal));
        $('#lbltotal_hidden').val(GrandTotal);
        if (rcount == 0) {
            $('#lbltaxes').val('0');
            $('#lbltotal').val('0');
            $('#lbltaxes_hidden').val('0');
            $('#lbltotal_hidden').val('0');
        }
    }
    catch (exception) {
        console.log(exception);
    }
}
function ReplaceNumberWithCommas(yourNumber) {
    //Seperates the components of the number
    yourNumber = Math.round(yourNumber);
    var n = yourNumber.toString().split(".");
    //Comma-fies the first part
    n[0] = n[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    //Combines the two sections
    return n.join(".");
}

