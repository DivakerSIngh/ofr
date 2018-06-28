
$(document).ready(function () {

  
});

function bindDatatable(id) {
    $(id).dataTable({
        "bJQueryUI": true,
        "sPaginationType": "full_numbers",
     
        "aaSorting": [],
        "sScrollX": "100%",
        "sScrollXInner": "110%",
        "fnDrawCallback": function (oSettings) {
            var searchTerm = $('#searchbox').val();
            // remove any old highlighted terms
            $('tbody').removeHighlight();
            // disable highlighting if empty
            if (searchTerm) {
                // highlight the new term
                $('tbody').highlight(searchTerm);
            }
        }
    });
}


function DatatableServerSide(id, aoColumns, Path, sortcolumn) {
    var mydt = $(id).dataTable({
        "oLanguage": {
            "sZeroRecords": "No records to display"
        },
        "aLengthMenu": [[10, 50, 100, 150, 250, 500], [10, 50, 100, 150, 250, 500]],
        "iDisplayLength": 10,
        "pageLength": 7,
        "bSortClasses": false,
        //"aaSorting": sortcolumn == undefined ? [[0, "asc"]] : sortcolumn,
        "bStateSave": false,
        "bPaginate": true,
        "aoColumns": aoColumns,
        "bAutoWidth": false,
        "bProcessing": true,
        "bServerSide": true,
        "bDestroy": true,
        "sAjaxSource": Path,
        //"sAjaxSource": "http://192.168.0.202/OneFineRateApp" + Path,
        "bJQueryUI": true,
        "sPaginationType": "full_numbers",
        "bDeferRender": true,
        "fnServerData": function (sSource, aoData, fnCallback) {
            $.ajax({
                "dataType": 'json',
                "contentType": "application/json; charset=utf-8",
                "type": "GET",
                "url": sSource,
                "data": aoData,
                "success":
                                function (msg) {
                                    var json = jQuery.parseJSON(msg.d);
                                    fnCallback(json);
                                    $(id).show();
                                }
            });
        },
        "fnDrawCallback": function (oSettings) {
            var searchTerm = $('#searchbox').val();
            // remove any old highlighted terms
            //$('tbody').removeHighlight();
            //// disable highlighting if empty
            //if (searchTerm) {
            //    // highlight the new term
            //    $('tbody').highlight(searchTerm);
            //}
        }
    });
 }


function getNorDate(mydate) {
    if (mydate != undefined) {
        var dateString = mydate.substr(6);
        var currentTime = new Date(parseInt(dateString));
        var month = currentTime.getMonth() + 1;
        var day = currentTime.getDate();
        var year = currentTime.getFullYear();
        var date = day + "/" + month + "/" + year;

        return date;
    }
}

function getNorDateWithoutTime(mydate) {

    if (mydate != undefined) {

        var month = new Array();
        month[0] = "January";
        month[1] = "February";
        month[2] = "March";
        month[3] = "April";
        month[4] = "May";
        month[5] = "June";
        month[6] = "July";
        month[7] = "August";
        month[8] = "September";
        month[9] = "October";
        month[10] = "November";
        month[11] = "December";

        var dateString = mydate.substr(6);
        var currentTime = new Date(parseInt(dateString));
        //var month = currentTime.getMonth() + 1;
        var mnth = currentTime.getMonth();
        var day = currentTime.getDate();
        day = day < 10 ? "0" + day : day;
        var year = currentTime.getFullYear();
        //var hours24 = (currentTime.getHours() < 10) ? "0" + currentTime.getHours() : currentTime.getHours();
        //var hours = (hours24 === 0 ? 12 : hours24 > 12 ? hours24 - 12 : hours24);
        var hours = currentTime.getHours();
        var minuts = (currentTime.getMinutes() < 10) ? "0" + currentTime.getMinutes() : currentTime.getMinutes();
        //meridiem = hours24 >= 12 ? "PM" : "AM";

        var date = day + " " + month[mnth] + " " + year;// + meridiem;
        return date;
    }
}
function getNorDateTime(mydate) {
  
    if (mydate != undefined) {

        var month = new Array();
        month[0] = "January";
        month[1] = "February";
        month[2] = "March";
        month[3] = "April";
        month[4] = "May";
        month[5] = "June";
        month[6] = "July";
        month[7] = "August";
        month[8] = "September";
        month[9] = "October";
        month[10] = "November";
        month[11] = "December";

        var dateString = mydate.substr(6);
        var currentTime = new Date(parseInt(dateString));
        //var month = currentTime.getMonth() + 1;
        var mnth = currentTime.getMonth();
        var day = currentTime.getDate();
        day = day < 10 ? "0" + day : day;
        var year = currentTime.getFullYear();
        //var hours24 = (currentTime.getHours() < 10) ? "0" + currentTime.getHours() : currentTime.getHours();
        //var hours = (hours24 === 0 ? 12 : hours24 > 12 ? hours24 - 12 : hours24);
        var hours = currentTime.getHours();
        var minuts = (currentTime.getMinutes() < 10) ? "0" + currentTime.getMinutes() : currentTime.getMinutes();
        //meridiem = hours24 >= 12 ? "PM" : "AM";

        var date = day + " " + month[mnth] + " " + year + " " + hours + ":" + minuts + " ";// + meridiem;
        return date;
    }
}