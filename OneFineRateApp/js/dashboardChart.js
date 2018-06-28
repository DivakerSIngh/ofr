
$(".loadingOverlay").LoadingOverlay("show");

var currentSelectedType = '';

function DrawBookingOverviewChart(data) {

    //#region Inititializer

    var bookingColor = '#5ac639';
    var bidColor = '#db843d';
    var negotiationColor = '#5252ad';
    var corporateColor = "#8F69F5"

    var ourColor = '#5ac639';
    var compColor = '#5252ad';

    $('#divViews_Ours').height(250);
    $('#divViews_Competitors').height(250);
    $('#divBooking_Ours').height(250);
    $('#divBooking_Competitors').height(250);
    $('#divCommission_Ours').height(250);
    $('#divCommission_Competitors').height(250);

    //#endregion

    //#region Booking Overview => Views

    var bookingView_our = [];
    var bookingView_comp = [];

    var bookingView_our_overall = [{
        name: 0,
        y: 0,
        color: ourColor
    }];

    var bookingView_comp_overall = [{
        name: 0,
        y: 0,
        color: compColor
    }];


    try {


        if (data.TotalViews_Ours.Bookings > 0) {

            var item = {
                name: data.TotalViews_Ours.Bookings,
                pName: 'Book',
                y: data.TotalViews_Ours.Bookings,
                color: bookingColor
            };

            bookingView_our.push(item);
            bookingView_our_overall[0].name += item.y;
            bookingView_our_overall[0].y += item.y;
        }

        if (data.TotalViews_Ours.Negotiations > 0) {

            var item = {
                pName: 'Bargain',
                name: data.TotalViews_Ours.Negotiations,
                y: data.TotalViews_Ours.Negotiations,
                sliced: true,
                selected: true,
                color: negotiationColor
            }

            bookingView_our.push(item);
            bookingView_our_overall[0].name += item.y;
            bookingView_our_overall[0].y += item.y;
        }

        if (data.TotalViews_Ours.Bids > 0) {

            var item = {
                pName: 'Bid',
                name: data.TotalViews_Ours.Bids,
                y: data.TotalViews_Ours.Bids,
                color: bidColor
            }

            bookingView_our.push(item);
            bookingView_our_overall[0].name += item.y;
            bookingView_our_overall[0].y += item.y;
        }

        if (data.TotalViews_Ours.Corporate > 0) {

            var item = {
                pName: 'Corporate',
                name: data.TotalViews_Ours.Corporate,
                y: data.TotalViews_Ours.Corporate,
                color: corporateColor
            }

            bookingView_our.push(item);
            bookingView_our_overall[0].name += item.y;
            bookingView_our_overall[0].y += item.y;
        }

        if (data.TotalViews_Competitors.Bookings > 0) {

            var item = {
                name: data.TotalViews_Competitors.Bookings,
                pName: 'Book',
                y: data.TotalViews_Competitors.Bookings,
                color: bookingColor
            };

            bookingView_comp.push(item);
            bookingView_comp_overall[0].name += item.y;
            bookingView_comp_overall[0].y += item.y;
        }

        if (data.TotalViews_Competitors.Negotiations > 0) {

            var item = {
                pName: 'Bargain',
                name: data.TotalViews_Competitors.Negotiations,
                y: data.TotalViews_Competitors.Negotiations,
                sliced: true,
                selected: true,
                color: negotiationColor
            }

            bookingView_comp.push(item);
            bookingView_comp_overall[0].name += item.y;
            bookingView_comp_overall[0].y += item.y;
        }

        if (data.TotalViews_Competitors.Bids > 0) {

            var item = {
                pName: 'Bid',
                name: data.TotalViews_Competitors.Bids,
                y: data.TotalViews_Competitors.Bids,
                color: bidColor
            }

            bookingView_comp.push(item);
            bookingView_comp_overall[0].name += item.y;
            bookingView_comp_overall[0].y += item.y;
        }

        if (data.TotalViews_Competitors.Corporate > 0) {

            var item = {
                pName: 'Corporate',
                name: data.TotalViews_Competitors.Corporate,
                y: data.TotalViews_Competitors.Corporate,
                color: corporateColor
            }

            bookingView_comp.push(item);
            bookingView_comp_overall[0].name += item.y;
            bookingView_comp_overall[0].y += item.y;
        }
    } catch (e) {
        console.error(e);
    }

    //#endregion

    //#region Booking Overview => Bookings

    var bookingData_our = [];
    var bookingData_comp = [];

    var bookingData_our_overall = [{
        name: 0,
        y: 0,
        color: ourColor
    }];

    var bookingData_comp_overall = [{
        name: 0,
        y: 0,
        color: compColor
    }];


    try {

        if (data.TotalBookings_Ours.Bookings > 0) {

            var item = {
                name: data.TotalBookings_Ours.Bookings,
                pName: 'Book',
                y: data.TotalBookings_Ours.Bookings,
                color: bookingColor
            };

            bookingData_our.push(item);
            bookingData_our_overall[0].name += item.y;
            bookingData_our_overall[0].y += item.y;
        }

        if (data.TotalBookings_Ours.Negotiations > 0) {

            var item = {
                pName: 'Bargain',
                name: data.TotalBookings_Ours.Negotiations,
                y: data.TotalBookings_Ours.Negotiations,
                sliced: true,
                selected: true,
                color: negotiationColor
            }

            bookingData_our.push(item);
            bookingData_our_overall[0].name += item.y;
            bookingData_our_overall[0].y += item.y;
        }

        if (data.TotalBookings_Ours.Bids > 0) {

            var item = {
                pName: 'Bid',
                name: data.TotalBookings_Ours.Bids,
                y: data.TotalBookings_Ours.Bids,
                color: bidColor
            }

            bookingData_our.push(item);
            bookingData_our_overall[0].name += item.y;
            bookingData_our_overall[0].y += item.y;
        }

        if (data.TotalBookings_Ours.Corporate > 0) {

            var item = {
                pName: 'Corporate',
                name: data.TotalBookings_Ours.Corporate,
                y: data.TotalBookings_Ours.Corporate,
                color: corporateColor
            }

            bookingData_our.push(item);
            bookingData_our_overall[0].name += item.y;
            bookingData_our_overall[0].y += item.y;
        }


        if (data.TotalBookings_Competitors.Bookings > 0) {

            var item = {
                name: data.TotalBookings_Competitors.Bookings,
                pName: 'Book',
                y: data.TotalBookings_Competitors.Bookings,
                color: bookingColor
            };

            bookingData_comp.push(item);
            bookingData_comp_overall[0].name += item.y;
            bookingData_comp_overall[0].y += item.y;
        }

        if (data.TotalBookings_Competitors.Negotiations > 0) {

            var item = {
                pName: 'Bargain',
                name: data.TotalBookings_Competitors.Negotiations,
                y: data.TotalBookings_Competitors.Negotiations,
                sliced: true,
                selected: true,
                color: negotiationColor
            }

            bookingData_comp.push(item);
            bookingData_comp_overall[0].name += item.y;
            bookingData_comp_overall[0].y += item.y;
        }

        if (data.TotalBookings_Competitors.Bids > 0) {

            var item = {
                pName: 'Bid',
                name: data.TotalBookings_Competitors.Bids,
                y: data.TotalBookings_Competitors.Bids,
                color: bidColor
            }

            bookingData_comp.push(item);
            bookingData_comp_overall[0].name += item.y;
            bookingData_comp_overall[0].y += item.y;
        }

        if (data.TotalBookings_Competitors.Corporate > 0) {

            var item = {
                pName: 'Corporate',
                name: data.TotalBookings_Competitors.Corporate,
                y: data.TotalBookings_Competitors.Corporate,
                color: corporateColor
            }

            bookingData_comp.push(item);
            bookingData_comp_overall[0].name += item.y;
            bookingData_comp_overall[0].y += item.y;
        }

    } catch (e) {
        console.error(e)
    }

    //#endregion

    //#region Booking Overview => Conversion

    var conversionData_our = [];
    var conversionData_comp = [];

    var conversionData_our_overall = [{
        name: 0,
        y: 0,
        color: ourColor
    }];

    var conversionData_comp_overall = [{
        name: 0,
        y: 0,
        color: compColor
    }];

    try {


        if (data.TotalConversions_Ours.Bookings > 0) {

            var item = {
                name: data.TotalConversions_Ours.Bookings,
                pName: 'Book',
                y: data.TotalConversions_Ours.Bookings,
                color: bookingColor
            };

            conversionData_our.push(item);
            conversionData_our_overall[0].name += item.y;
            conversionData_our_overall[0].y += item.y;
        }

        if (data.TotalConversions_Ours.Negotiations > 0) {

            var item = {
                pName: 'Bargain',
                name: data.TotalConversions_Ours.Negotiations,
                y: data.TotalConversions_Ours.Negotiations,
                sliced: true,
                selected: true,
                color: negotiationColor
            }

            conversionData_our.push(item);
            conversionData_our_overall[0].name += item.y;
            conversionData_our_overall[0].y += item.y;
        }

        if (data.TotalConversions_Ours.Bids > 0) {

            var item = {
                pName: 'Bid',
                name: data.TotalConversions_Ours.Bids,
                y: data.TotalConversions_Ours.Bids,
                color: bidColor
            }

            conversionData_our.push(item);
            conversionData_our_overall[0].name += item.y;
            conversionData_our_overall[0].y += item.y;
        }

        if (data.TotalConversions_Ours.Corporate > 0) {

            var item = {
                pName: 'Corporate',
                name: data.TotalConversions_Ours.Corporate,
                y: data.TotalConversions_Ours.Corporate,
                color: corporateColor
            }

            conversionData_our.push(item);
            conversionData_our_overall[0].name += item.y;
            conversionData_our_overall[0].y += item.y;
        }


        if (data.TotalConversions_Competitors.Bookings > 0) {

            var item = {
                name: data.TotalConversions_Competitors.Bookings,
                pName: 'Book',
                y: data.TotalConversions_Competitors.Bookings,
                color: bookingColor
            };

            conversionData_comp.push(item);
            conversionData_comp_overall[0].name += item.y;
            conversionData_comp_overall[0].y += item.y;
        }

        if (data.TotalConversions_Competitors.Negotiations > 0) {

            var item = {
                pName: 'Bargain',
                name: data.TotalConversions_Competitors.Negotiations,
                y: data.TotalConversions_Competitors.Negotiations,
                sliced: true,
                selected: true,
                color: negotiationColor
            }

            conversionData_comp.push(item);
            conversionData_comp_overall[0].name += item.y;
            conversionData_comp_overall[0].y += item.y;
        }

        if (data.TotalConversions_Competitors.Bids > 0) {

            var item = {
                pName: 'Bid',
                name: data.TotalConversions_Competitors.Bids,
                y: data.TotalConversions_Competitors.Bids,
                color: bidColor
            }

            conversionData_comp.push(item);
            conversionData_comp_overall[0].name += item.y;
            conversionData_comp_overall[0].y += item.y;
        }

        if (data.TotalConversions_Competitors.Corporate > 0) {

            var item = {
                pName: 'Corporate',
                name: data.TotalConversions_Competitors.Corporate,
                y: data.TotalConversions_Competitors.Corporate,
                color: corporateColor
            }

            conversionData_comp.push(item);
            conversionData_comp_overall[0].name += item.y;
            conversionData_comp_overall[0].y += item.y;
        }
    } catch (e) {
        console.error(e);
    }

    //#endregion

    //#region Drawing Charts from above data

    try {

        var view_our_chart = Highcharts.chart({

            chart: {
                renderTo: 'divViews_Ours',
                type: 'column',
                events: {
                    load: function (event) {

                        $('#divViews_Ours').LoadingOverlay("hide");
                    }
                }
            },
            title: {
                text: 'My Hotel',
                style: { "color": "#333333", "fontSize": "12px", "font-weight": "bold" },
            },
            //tooltip: {
            //    pointFormat: '{point.pName}<br/> <b>{point.percentage:.1f}%</b>'
            //},
            xAxis: {
                lineWidth: 0,
                gridLineColor: 'transparent',
                minorGridLineWidth: 0,
                lineColor: 'transparent',
                labels: {
                    enabled: false
                },
                minorTickLength: 0,
                tickLength: 0
            },
            yAxis: {
                lineWidth: 0,
                gridLineColor: 'transparent',
                minorGridLineWidth: 0,
                lineColor: 'transparent',
                labels: {
                    enabled: false
                },
                title: {
                    text: '',
                },
                minorTickLength: 0,
                tickLength: 0
            },
            legend: {
                enabled: false,
                labelFormatter: function () {
                    return this.pName;
                },
                align: "center",
                symbolPadding: 1,
                padding: 5,
                itemDistance: 0,
                itemStyle: { "fontSize": "11px", "font-weight": "normal" },
            },
            plotOptions: {
                series: {
                    borderWidth: 0,
                    dataLabels: {
                        enabled: true,
                        formatter: function () {

                            return this.point.y;

                        }
                    }
                }
            },
            loading: {
                labelStyle: {
                    backgroundImage: 'url("/img/select2-spinner.gif")',
                    display: 'block',
                    width: '100px',
                    height: '100px',
                    backgroundColor: '#000'
                }
            },

            series: [{
                name: "Overall",
                colorByPoint: true,
                data: bookingView_our_overall
            }]
        });

        var view_comp_chart = Highcharts.chart({

            chart: {
                renderTo: 'divViews_Competitors',
                type: 'column',
                events: {
                    load: function (event) {
                        $("#divViews_Competitors").LoadingOverlay("hide");
                    }
                }
            },
            title: {
                text: 'Avg of your comp set',
                style: { "color": "#333333", "fontSize": "12px", "font-weight": "bold" },
            },
            //tooltip: {
            //    pointFormat: '{point.pName}<br/> <b>{point.percentage:.1f}%</b>'
            //},
            xAxis: {
                lineWidth: 0,
                gridLineColor: 'transparent',
                minorGridLineWidth: 0,
                lineColor: 'transparent',
                labels: {
                    enabled: false
                },
                minorTickLength: 0,
                tickLength: 0
            },
            yAxis: {
                lineWidth: 0,
                gridLineColor: 'transparent',
                minorGridLineWidth: 0,
                lineColor: 'transparent',
                labels: {
                    enabled: false
                },
                title: {
                    text: '',
                },
                minorTickLength: 0,
                tickLength: 0
            },
            legend: {
                enabled: false,
                labelFormatter: function () {
                    return this.pName;
                },
                align: "center",
                symbolPadding: 1,
                padding: 5,
                itemDistance: 0,
                itemStyle: { "fontSize": "11px", "font-weight": "normal" },
            },
            plotOptions: {
                series: {
                    borderWidth: 0,
                    dataLabels: {
                        enabled: true,
                        formatter: function () {
                            return this.point.y;
                        }
                    }
                }
            },

            series: [{
                name: "Overall",
                colorByPoint: true,
                data: bookingView_comp_overall
            }]
        });

        var booking_our_chart = Highcharts.chart({

            chart: {
                renderTo: 'divBooking_Ours',
                type: 'pie',
                events: {
                    load: function (event) {
                        $("#divBooking_Ours").LoadingOverlay("hide");
                    }
                }
            },
            title: {
                text: 'My Hotel',
                style: { "color": "#333333", "fontSize": "12px", "font-weight": "bold" },
            },
            tooltip: {
                pointFormat: '{point.pName}<br/> <b>{point.percentage:.1f}%</b>'
            },
            showInLegend: false,
            legend: {
                labelFormatter: function () {
                    return this.pName;
                },
                align: "center",
                symbolPadding: 1,
                padding: 5,
                itemDistance: 0,
                itemStyle: { "fontSize": "11px", "font-weight": "normal" },
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        distance: -20,
                        style: {
                            fontWeight: 'bold',
                            color: 'white'
                        }
                    },
                    showInLegend: true
                }
            },

            series: [{
                colorByPoint: true,
                data: bookingData_our
            }]
        });

        var booking_comp_chart = Highcharts.chart({

            chart: {
                renderTo: 'divBooking_Competitors',
                type: 'pie',
                events: {
                    load: function (event) {
                        $("#divBooking_Competitors").LoadingOverlay("hide");
                    }
                }
            },
            title: {
                text: 'Avg of your comp set',
                style: { "color": "#333333", "fontSize": "12px", "font-weight": "bold" },
            },
            tooltip: {
                pointFormat: '{point.pName}<br/> <b>{point.percentage:.1f}%</b>'
            },
            showInLegend: false,
            legend: {
                labelFormatter: function () {
                    return this.pName;
                },
                align: "center",
                symbolPadding: 1,
                padding: 5,
                itemDistance: 0,
                itemStyle: { "fontSize": "11px", "font-weight": "normal" },
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        distance: -20,
                        style: {
                            fontWeight: 'bold',
                            color: 'white'
                        }
                    },
                    showInLegend: true
                }
            },

            series: [{
                colorByPoint: true,
                data: bookingData_comp
            }]
        });

        var conversion_our_chart = Highcharts.chart({

            chart: {
                renderTo: 'divCommission_Ours',
                type: 'pie',
                events: {
                    load: function (event) {
                        $("#divCommission_Ours").LoadingOverlay("hide");
                    }
                }
            },
            title: {
                text: 'My Hotel',
                style: { "color": "#333333", "fontSize": "12px", "font-weight": "bold" },
            },
            tooltip: {
                pointFormat: '{point.pName}<br/> <b>{point.percentage:.1f}%</b>'
            },
            showInLegend: false,
            legend: {
                labelFormatter: function () {
                    return this.pName;
                },
                align: "center",
                symbolPadding: 1,
                padding: 5,
                itemDistance: 0,
                itemStyle: { "fontSize": "11px", "font-weight": "normal" },
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        distance: -20,
                        style: {
                            fontWeight: 'bold',
                            color: 'white'
                        }
                    },
                    showInLegend: true
                }
            },

            series: [{
                colorByPoint: true,
                data: conversionData_our
            }]
        });

        var conversion_comp_chart = Highcharts.chart({

            chart: {
                renderTo: 'divCommission_Competitors',
                type: 'pie',
                events: {
                    load: function (event) {
                        $("#divCommission_Competitors").LoadingOverlay("hide");
                    }
                }
            },
            title: {
                text: 'Avg of your comp set',
                style: { "color": "#333333", "fontSize": "12px", "font-weight": "bold" },
            },
            tooltip: {
                pointFormat: '{point.pName}<br/> <b>{point.percentage:.1f}%</b>'
            },
            showInLegend: false,
            legend: {
                labelFormatter: function () {
                    return this.pName;
                },
                align: "center",
                symbolPadding: 1,
                padding: 5,
                itemDistance: 0,
                itemStyle: { "fontSize": "11px", "font-weight": "normal" },
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        distance: -20,
                        style: {
                            fontWeight: 'bold',
                            color: 'white'
                        }
                    },
                    showInLegend: true
                }
            },

            series: [{
                colorByPoint: true,
                data: conversionData_comp
            }]
        });

        if (!view_our_chart.hasData() && !view_comp_chart.hasData() && !booking_our_chart.hasData() && !booking_comp_chart.hasData()
            && !conversion_our_chart.hasData() && !conversion_comp_chart.hasData()) {

            view_our_chart.setSize(view_our_chart.plotWidth, 100, true);
            view_comp_chart.setSize(view_comp_chart.plotWidth, 100, true);
            booking_our_chart.setSize(booking_our_chart.plotWidth, 100, true);
            booking_comp_chart.setSize(booking_comp_chart.plotWidth, 100, true);
            conversion_our_chart.setSize(conversion_our_chart.plotWidth, 100, true);
            conversion_comp_chart.setSize(conversion_comp_chart.plotWidth, 100, true);


            $('#divViews_Ours').height(100);
            $('#divViews_Competitors').height(100);
            $('#divBooking_Ours').height(100);
            $('#divBooking_Competitors').height(100);
            $('#divCommission_Ours').height(100);
            $('#divCommission_Competitors').height(100);
        }

    } catch (e) {

    }

    //#endregion
}


function DrawNegotiationOverviewChart(data) {

    //#region Inititializer

    var acceptedColor = '#5ac639';
    var rejectedColor = '#434348';
    var counterOfferColor = '#5252ad';
    var noActionColor = '#ff8000';

    $('#divNegotiation_Ours').height(250);
    $('#divNegotiation_Competitors').height(250);
    $('#divNegotiationAccepted_Ours').height(250);
    $('#divNegotiationAccepted_Competitors').height(250);

    //#endregion

    //#region Your responses to guest negotiation

    var negotiationData_our = [];
    var negotiationData_comp = [];

    try {

        if (data.TotalNegotiation_Ours.Accepted > 0) {

            var item = {
                name: data.TotalNegotiation_Ours.Accepted,
                pName: 'Accepted',
                color: acceptedColor,
                y: data.TotalNegotiation_Ours.Accepted
            };

            negotiationData_our.push(item);
        }

        if (data.TotalNegotiation_Ours.Rejected > 0) {

            var item = {
                pName: 'Rejected',
                name: data.TotalNegotiation_Ours.Rejected,
                y: data.TotalNegotiation_Ours.Rejected,
                sliced: true,
                selected: true,
                color: rejectedColor
            };
            negotiationData_our.push(item);
        }

        if (data.TotalNegotiation_Ours.CounterOffer > 0) {
            var item = {
                pName: 'Counter Offer',
                name: data.TotalNegotiation_Ours.CounterOffer,
                y: data.TotalNegotiation_Ours.CounterOffer,
                color: counterOfferColor
            };
            negotiationData_our.push(item);
        }



        if (data.TotalNegotiation_Competitors.Accepted > 0) {

            var item = {
                name: data.TotalNegotiation_Competitors.Accepted,
                pName: 'Accepted',
                color: acceptedColor,
                y: data.TotalNegotiation_Competitors.Accepted
            };

            negotiationData_comp.push(item);
        }

        if (data.TotalNegotiation_Competitors.Rejected > 0) {

            var item = {
                pName: 'Rejected',
                name: data.TotalNegotiation_Competitors.Rejected,
                y: data.TotalNegotiation_Competitors.Rejected,
                sliced: true,
                selected: true,
                color: rejectedColor
            };
            negotiationData_comp.push(item);
        }

        if (data.TotalNegotiation_Competitors.CounterOffer > 0) {
            var item = {
                pName: 'Counter Offer',
                name: data.TotalNegotiation_Competitors.CounterOffer,
                y: data.TotalNegotiation_Competitors.CounterOffer,
                color: counterOfferColor
            };
            negotiationData_comp.push(item);
        }

    } catch (e) {
        console.error(e);
    }

    //#endregion

    //#region Responses of guests to your counter offers

    var acceptedData_our = [];
    var acceptedData_comp = [];


    try {

        if (data.TotalAccepted_Ours.Accepted > 0) {

            var item = {
                name: data.TotalAccepted_Ours.Accepted,
                pName: 'Accepted',
                color: acceptedColor,
                y: data.TotalAccepted_Ours.Accepted
            }

            acceptedData_our.push(item);
        }

        if (data.TotalAccepted_Ours.Rejected > 0) {

            var item = {
                pName: 'Rejected',
                name: data.TotalAccepted_Ours.Rejected,
                y: data.TotalAccepted_Ours.Rejected,
                sliced: true,
                selected: true,
                color: rejectedColor
            };
            acceptedData_our.push(item);
        }

        if (data.TotalAccepted_Ours.NoAction > 0) {
            var item = {
                pName: 'No Action',
                name: data.TotalAccepted_Ours.NoAction,
                y: data.TotalAccepted_Ours.NoAction,
                color: noActionColor
            };
            acceptedData_our.push(item);
        }


        if (data.TotalAccepted_Competitors.Accepted > 0) {

            var item = {
                name: data.TotalAccepted_Competitors.Accepted,
                pName: 'Accepted',
                color: acceptedColor,
                y: data.TotalAccepted_Competitors.Accepted
            };

            acceptedData_comp.push(item);
        }

        if (data.TotalAccepted_Competitors.Rejected > 0) {

            var item = {
                pName: 'Rejected',
                name: data.TotalAccepted_Competitors.Rejected,
                y: data.TotalAccepted_Competitors.Rejected,
                sliced: true,
                selected: true,
                color: rejectedColor
            };
            acceptedData_comp.push(item);
        }

        if (data.TotalAccepted_Competitors.NoAction > 0) {
            var item = {
                pName: 'No Action',
                name: data.TotalAccepted_Competitors.NoAction,
                y: data.TotalAccepted_Competitors.NoAction,
                color: noActionColor
            };
            acceptedData_comp.push(item);
        }

    } catch (e) {
        console.error(e);
    }

    //#endregion

    //#region Drawing Charts from above data

    var nego_our_chart = Highcharts.chart({

        chart: {
            renderTo: 'divNegotiation_Ours',
            type: 'pie',
            events: {
                load: function (event) {
                    $("#divNegotiation_Ours").LoadingOverlay("hide");
                }
            }
        },
        title: {
            text: 'My Hotel',
            style: { "color": "#333333", "fontSize": "12px", "font-weight": "bold" },
        },
        tooltip: {
            pointFormat: '{point.pName}<br/> <b>{point.percentage:.1f}%</b>'
        },
        legend: {
            labelFormatter: function () {
                return this.pName;
            },
            align: "center",
            symbolPadding: 1,
            padding: 5,
            itemDistance: 0,
            itemStyle: { "fontSize": "11px", "font-weight": "normal" },
        },
        plotOptions: {

            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    distance: -20,
                    style: {
                        fontWeight: 'bold',
                        color: 'white'
                    }
                },
                showInLegend: true
            }
        },
        series: [{
            colorByPoint: true,
            data: negotiationData_our
        }]
    });

    var nego_comp_chart = Highcharts.chart({

        chart: {
            renderTo: 'divNegotiation_Competitors',
            type: 'pie',
            events: {
                load: function (event) {
                    $("#divNegotiation_Competitors").LoadingOverlay("hide");
                }
            }
        },
        title: {
            text: 'Best in your Competition Set',
            style: { "color": "#333333", "fontSize": "12px", "font-weight": "bold" },
        },
        tooltip: {
            pointFormat: '{point.pName}<br/> <b>{point.percentage:.1f}%</b>'
        },
        legend: {
            labelFormatter: function () {
                return this.pName;
            },
            align: "center",
            symbolPadding: 1,
            padding: 5,
            itemDistance: 0,
            itemStyle: { "fontSize": "11px", "font-weight": "normal" },
        },
        plotOptions: {

            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    distance: -20,
                    style: {
                        fontWeight: 'bold',
                        color: 'white'
                    }
                },
                showInLegend: true
            }
        },
        series: [{
            colorByPoint: true,
            data: negotiationData_comp
        }]
    });

    var nego_accepted_our_chart = Highcharts.chart({

        chart: {
            renderTo: 'divNegotiationAccepted_Ours',
            type: 'pie',
            events: {
                load: function (event) {
                    $("#divNegotiationAccepted_Ours").LoadingOverlay("hide");
                }
            }
        },
        title: {
            text: 'My Hotel',
            style: { "color": "#333333", "fontSize": "12px", "font-weight": "bold" },
        },
        tooltip: {
            pointFormat: '{point.pName}<br/> <b>{point.percentage:.1f}%</b>'
        },
        legend: {
            labelFormatter: function () {
                return this.pName;
            },
            align: "center",
            symbolPadding: 1,
            padding: 5,
            itemDistance: 0,
            itemStyle: { "fontSize": "11px", "font-weight": "normal" },
        },
        plotOptions: {

            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    distance: -20,
                    style: {
                        fontWeight: 'bold',
                        color: 'white'
                    }
                },
                showInLegend: true
            }
        },
        series: [{
            colorByPoint: true,
            data: acceptedData_our
        }]
    });

    var nego_accepted_comp_chart = Highcharts.chart({

        chart: {
            renderTo: 'divNegotiationAccepted_Competitors',
            type: 'pie',
            events: {
                load: function (event) {
                    $("#divNegotiationAccepted_Competitors").LoadingOverlay("hide");
                }
            }
        },
        title: {
            text: 'Best in your Competition Set',
            style: { "color": "#333333", "fontSize": "12px", "font-weight": "bold" },
        },
        tooltip: {
            pointFormat: '{point.pName}<br/> <b>{point.percentage:.1f}%</b>'
        },
        legend: {
            labelFormatter: function () {
                return this.pName;
            },
            align: "center",
            symbolPadding: 1,
            padding: 5,
            itemDistance: 0,
            itemStyle: { "fontSize": "11px", "font-weight": "normal" },
        },
        plotOptions: {

            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    distance: -20,
                    style: {
                        fontWeight: 'bold',
                        color: 'white'
                    }
                },
                showInLegend: true
            }
        },

        series: [{
            colorByPoint: true,
            data: acceptedData_comp
        }]
    });

    if (!nego_our_chart.hasData() && !nego_comp_chart.hasData() && !nego_accepted_our_chart.hasData() && !nego_accepted_comp_chart.hasData()) {

        nego_our_chart.setSize(nego_our_chart.plotWidth, 100, true);
        nego_comp_chart.setSize(nego_comp_chart.plotWidth, 100, true);
        nego_accepted_our_chart.setSize(nego_accepted_our_chart.plotWidth, 100, true);
        nego_accepted_comp_chart.setSize(nego_accepted_comp_chart.plotWidth, 100, true);

        $('#divNegotiation_Ours').height(100);
        $('#divNegotiation_Competitors').height(100);
        $('#divNegotiationAccepted_Ours').height(100);
        $('#divNegotiationAccepted_Competitors').height(100);
    }

    //#endregion

}


function DrawBookingInsightsChart(data) {

    var bookingInsightOurColor_overall = '#4572a7';
    var bookingInsightCompColor_overall = '#aa4643';

    var bookingInsightOurColor = '#4198af';
    var bookingInsightCompColor = '#db843d';

    //#region Initializer

    var bookingType = $("input:radio[name='radioBookingType']:checked").val();

    var isOnlyOverall = true;

    if (bookingType && bookingType != 'O')
        isOnlyOverall = false;


    //#endregion Initializer

    //#region LeadTime

    $('#divLeadTime').height(500);

    var leadTimeData_Ours,
      leadTimeData_Comp,
      leadTimeData_Ours_P,
      leadTimeData_Comp_P,

      leadTimeData_Ours_Overall,
      leadTimeData_Comp_Overall,
      leadTimeData_Ours_P_Overall,
      leadTimeData_Comp_P_Overall = [];

    try {

        //#region Overll

        leadTimeData_Ours_Overall = [
               data.LeadTime[0].Y6_Overall,
               data.LeadTime[0].Y5_Overall,
               data.LeadTime[0].Y4_Overall,
               data.LeadTime[0].Y3_Overall,
               data.LeadTime[0].Y2_Overall,
               data.LeadTime[0].Y1_Overall,
               data.LeadTime[0].Y0_Overall];

        var leadTimeData_Ours_Overall_Result = $.grep(leadTimeData_Ours_Overall, function (input) {
            return input > 0;
        });

        if (leadTimeData_Ours_Overall_Result.length == 0) {
            leadTimeData_Ours_Overall = [];
        }


        leadTimeData_Comp_Overall = [
           data.LeadTime[1].Y6_Overall,
           data.LeadTime[1].Y5_Overall,
           data.LeadTime[1].Y4_Overall,
           data.LeadTime[1].Y3_Overall,
           data.LeadTime[1].Y2_Overall,
           data.LeadTime[1].Y1_Overall,
           data.LeadTime[1].Y0_Overall];

        var leadTimeData_Comp_Overall_Result = $.grep(leadTimeData_Comp_Overall, function (input) {
            return input > 0;
        });

        if (leadTimeData_Comp_Overall_Result.length == 0) {
            leadTimeData_Comp_Overall = [];
        }


        leadTimeData_Ours_P_Overall = [
           data.LeadTime[2].Y6_Overall,
           data.LeadTime[2].Y5_Overall,
           data.LeadTime[2].Y4_Overall,
           data.LeadTime[2].Y3_Overall,
           data.LeadTime[2].Y2_Overall,
           data.LeadTime[2].Y1_Overall,
           data.LeadTime[2].Y0_Overall];

        var leadTimeData_Ours_P_Overall_Result = $.grep(leadTimeData_Ours_P_Overall, function (input) {
            return input > 0;
        });

        if (leadTimeData_Ours_P_Overall_Result.length == 0) {
            leadTimeData_Ours_P_Overall = [];
        }

        leadTimeData_Comp_P_Overall = [
          data.LeadTime[3].Y6_Overall,
          data.LeadTime[3].Y5_Overall,
          data.LeadTime[3].Y4_Overall,
          data.LeadTime[3].Y3_Overall,
          data.LeadTime[3].Y2_Overall,
          data.LeadTime[3].Y1_Overall,
          data.LeadTime[3].Y0_Overall];

        var leadTimeData_Comp_P_Overall_Result = $.grep(leadTimeData_Comp_P_Overall, function (input) {
            return input > 0;
        });

        if (leadTimeData_Comp_P_Overall_Result.length == 0) {
            leadTimeData_Comp_P_Overall = [];
        }

        //#endregion Overall

        //#region Other

        if (!isOnlyOverall) {

            leadTimeData_Ours = [
                 data.LeadTime[4].Y6,
                 data.LeadTime[4].Y5,
                 data.LeadTime[4].Y4,
                 data.LeadTime[4].Y3,
                 data.LeadTime[4].Y2,
                 data.LeadTime[4].Y1,
                 data.LeadTime[4].Y0];


            var leadTimeData_Ours_Result = $.grep(leadTimeData_Ours, function (input) {
                return input > 0;
            });

            if (leadTimeData_Ours_Result.length == 0) {
                leadTimeData_Ours = [];
            }


            leadTimeData_Comp = [
           data.LeadTime[5].Y6,
           data.LeadTime[5].Y5,
           data.LeadTime[5].Y4,
           data.LeadTime[5].Y3,
           data.LeadTime[5].Y2,
           data.LeadTime[5].Y1,
           data.LeadTime[5].Y0];

            var leadTimeData_Comp_Result = $.grep(leadTimeData_Comp, function (input) {
                return input > 0;
            });

            if (leadTimeData_Comp_Result.length == 0) {
                leadTimeData_Comp = [];
            }

            leadTimeData_Ours_P = [
           data.LeadTime[6].Y6,
           data.LeadTime[6].Y5,
           data.LeadTime[6].Y4,
           data.LeadTime[6].Y3,
           data.LeadTime[6].Y2,
           data.LeadTime[6].Y1,
           data.LeadTime[6].Y0];

            var leadTimeData_Ours_P_Result = $.grep(leadTimeData_Ours_P, function (input) {
                return input > 0;
            });

            if (leadTimeData_Ours_P_Result.length == 0) {
                leadTimeData_Ours_P = [];
            }

            leadTimeData_Comp_P = [
           data.LeadTime[7].Y6,
           data.LeadTime[7].Y5,
           data.LeadTime[7].Y4,
           data.LeadTime[7].Y3,
           data.LeadTime[7].Y2,
           data.LeadTime[7].Y1,
           data.LeadTime[7].Y0];

            var leadTimeData_Comp_P_Result = $.grep(leadTimeData_Comp_P, function (input) {
                return input > 0;
            });

            if (leadTimeData_Comp_P_Result.length == 0) {
                leadTimeData_Comp_P = [];
            }
        }

        //#endregion Other

    } catch (e) {
        console.error(e)
    }


    var leadTimeSeriesData = [{
        color: bookingInsightOurColor,
        data_arr: leadTimeData_Ours_Overall,
        name: 'My Hotel (Overall)',
        data: leadTimeData_Ours_P_Overall
    }, {
        color: bookingInsightCompColor,
        data_arr: leadTimeData_Comp_Overall,
        name: 'Average In Comp Set (Overall)',
        data: leadTimeData_Comp_P_Overall
    }];

    if (!isOnlyOverall) {

        leadTimeSeriesData.push({
            custom_type: 'M',
            color: bookingInsightOurColor_overall,
            data_arr: leadTimeData_Ours,
            name: 'My Hotel' + currentSelectedType,
            data: leadTimeData_Ours_P
        }, {
            custom_type: 'C',
            color: bookingInsightCompColor_overall,
            data_arr: leadTimeData_Comp,
            name: 'Average In Comp Set' + currentSelectedType,
            data: leadTimeData_Comp_P
        });
    }

    var leadTimeChart = Highcharts.chart({

        chart: {
            renderTo: 'divLeadTime',
            type: 'bar',
            events: {
                load: function (event) {
                    $("#divLeadTime").LoadingOverlay("hide");
                }
            }
        },
        title: {
            text: 'Lead Time'
        },

        xAxis: {
            categories: ['60+ days', '31-60 days', '15-30 days', '8-14 days', '4-7 days', '2-3 days', '0-1 days'],
            minorGridLineWidth: 0,
        },

        yAxis: {
            gridLineColor: 'transparent',
            min: 0,
            title: {
                text: '',
                align: 'high'
            },
            labels: {
                enabled: false,
                overflow: 'justify'
            }
        },

        tooltip: {

            formatter: function () {

                var s = '<b>' + this.x + '</b><table>';

                $.each(this.points, function (i, e) {
                    s += '<tr><td style="color:' + this.series.color + '">' + this.series.name + ': </td><td style="text-align: right"><b>' + ReplaceNumberWithCommas(this.series.options.data_arr[e.point.index]) + ' </b></td></tr>';
                });

                s += '</table>';
                return s;
            },
            shared: true,
            useHTML: true,
            followPointer: true,
        },

        plotOptions: {
            bar: {
                events: {
                    legendItemClick: function (e) {

                        ShowHideOverallSeries(leadTimeChart, e);
                    }
                },
                dataLabels: {
                    enabled: true,
                    style: {
                        fontWeight: 'bold'
                    },
                    formatter: function () {
                        if (this.y != 0) {
                            return this.y + '%';
                        } else {
                            return null;
                        }
                    }
                },
            }
        },
        legend: {
            layout: 'horizontal',
            align: 'center',
            borderWidth: 0,
            //backgroundColor: ((Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'),
            shadow: false
        },
        credits: {
            enabled: false
        },
        series: leadTimeSeriesData
    });


    //#endregion LeadTime

    //#region DayOfWeek

    $('#divDayOfWeek').height(500);

    var dayOfWeek_Ours,
        dayOfWeek_Comp,
        dayOfWeek_Ours_P,
        dayOfWeek_Comp_P,

        dayOfWeek_Ours_Overall,
        dayOfWeek_Comp_Overall,
        dayOfWeek_Ours_P_Overall,
        dayOfWeek_Comp_P_Overall = [];


    try {

        //#region Overall

        dayOfWeek_Ours_Overall = [
               //data.DayOfWeek[0].Y7_Overall,
               data.DayOfWeek[0].Y6_Overall,
               data.DayOfWeek[0].Y5_Overall,
               data.DayOfWeek[0].Y4_Overall,
               data.DayOfWeek[0].Y3_Overall,
               data.DayOfWeek[0].Y2_Overall,
               data.DayOfWeek[0].Y1_Overall,
               data.DayOfWeek[0].Y0_Overall];

        var DayOfWeekData_Ours_Overall_Result = $.grep(dayOfWeek_Ours_Overall, function (input) {
            return input > 0;
        });

        if (DayOfWeekData_Ours_Overall_Result.length == 0) {
            dayOfWeek_Ours_Overall = [];
        }

        dayOfWeek_Comp_Overall = [
           //data.DayOfWeek[1].Y7_Overall,
           data.DayOfWeek[1].Y6_Overall,
           data.DayOfWeek[1].Y5_Overall,
           data.DayOfWeek[1].Y4_Overall,
           data.DayOfWeek[1].Y3_Overall,
           data.DayOfWeek[1].Y2_Overall,
           data.DayOfWeek[1].Y1_Overall,
           data.DayOfWeek[1].Y0_Overall];

        var DayOfWeekData_Comp_Overall_Result = $.grep(dayOfWeek_Comp_Overall, function (input) {
            return input > 0;
        });

        if (DayOfWeekData_Comp_Overall_Result.length == 0) {
            dayOfWeek_Comp_Overall = [];
        }


        dayOfWeek_Ours_P_Overall = [
           //data.DayOfWeek[2].Y7_Overall,
           data.DayOfWeek[2].Y6_Overall,
           data.DayOfWeek[2].Y5_Overall,
           data.DayOfWeek[2].Y4_Overall,
           data.DayOfWeek[2].Y3_Overall,
           data.DayOfWeek[2].Y2_Overall,
           data.DayOfWeek[2].Y1_Overall,
           data.DayOfWeek[2].Y0_Overall];

        var DayOfWeekData_Ours_P_Overall_Result = $.grep(dayOfWeek_Ours_P_Overall, function (input) {
            return input > 0;
        });

        if (DayOfWeekData_Ours_P_Overall_Result.length == 0) {
            dayOfWeek_Ours_P_Overall = [];
        }

        dayOfWeek_Comp_P_Overall = [
          //data.DayOfWeek[3].Y7_Overall,
          data.DayOfWeek[3].Y6_Overall,
          data.DayOfWeek[3].Y5_Overall,
          data.DayOfWeek[3].Y4_Overall,
          data.DayOfWeek[3].Y3_Overall,
          data.DayOfWeek[3].Y2_Overall,
          data.DayOfWeek[3].Y1_Overall,
          data.DayOfWeek[3].Y0_Overall];

        var DayOfWeekData_Comp_P_Overall_Result = $.grep(dayOfWeek_Comp_P_Overall, function (input) {
            return input > 0;
        });

        if (DayOfWeekData_Comp_P_Overall_Result.length == 0) {
            dayOfWeek_Comp_P_Overall = [];
        }

        //#endregion Overall

        //#region Other

        if (!isOnlyOverall) {

            dayOfWeek_Ours = [
                 //data.DayOfWeek[4].Y7,
                 data.DayOfWeek[4].Y6,
                 data.DayOfWeek[4].Y5,
                 data.DayOfWeek[4].Y4,
                 data.DayOfWeek[4].Y3,
                 data.DayOfWeek[4].Y2,
                 data.DayOfWeek[4].Y1,
                 data.DayOfWeek[4].Y0];


            var DayOfWeekData_Ours_Result = $.grep(dayOfWeek_Ours, function (input) {
                return input > 0;
            });

            if (DayOfWeekData_Ours_Result.length == 0) {
                dayOfWeek_Ours = [];
            }


            dayOfWeek_Comp = [
          // data.DayOfWeek[5].Y7,
           data.DayOfWeek[5].Y6,
           data.DayOfWeek[5].Y5,
           data.DayOfWeek[5].Y4,
           data.DayOfWeek[5].Y3,
           data.DayOfWeek[5].Y2,
           data.DayOfWeek[5].Y1,
           data.DayOfWeek[5].Y0];

            var DayOfWeekData_Comp_Result = $.grep(dayOfWeek_Comp, function (input) {
                return input > 0;
            });

            if (DayOfWeekData_Comp_Result.length == 0) {
                dayOfWeek_Comp = [];
            }

            dayOfWeek_Ours_P = [
           //data.DayOfWeek[6].Y7,
           data.DayOfWeek[6].Y6,
           data.DayOfWeek[6].Y5,
           data.DayOfWeek[6].Y4,
           data.DayOfWeek[6].Y3,
           data.DayOfWeek[6].Y2,
           data.DayOfWeek[6].Y1,
           data.DayOfWeek[6].Y0];

            var DayOfWeekData_Ours_P_Result = $.grep(dayOfWeek_Ours_P, function (input) {
                return input > 0;
            });

            if (DayOfWeekData_Ours_P_Result.length == 0) {
                dayOfWeek_Ours_P = [];
            }

            dayOfWeek_Comp_P = [
           //data.DayOfWeek[7].Y7,
           data.DayOfWeek[7].Y6,
           data.DayOfWeek[7].Y5,
           data.DayOfWeek[7].Y4,
           data.DayOfWeek[7].Y3,
           data.DayOfWeek[7].Y2,
           data.DayOfWeek[7].Y1,
           data.DayOfWeek[7].Y0];

            var DayOfWeekData_Comp_P_Result = $.grep(dayOfWeek_Comp_P, function (input) {
                return input > 0;
            });

            if (DayOfWeekData_Comp_P_Result.length == 0) {
                dayOfWeek_Comp_P = [];
            }
        }

        //#endregion Other

    } catch (e) {
        console.error(e)
    }


    var dowSeriesData = [
           {

               color: bookingInsightOurColor,
               data_arr: dayOfWeek_Ours_Overall,
               name: 'My Hotel (Overall)',
               data: dayOfWeek_Ours_P_Overall
           },
          {

              color: bookingInsightCompColor,
              data_arr: dayOfWeek_Comp_Overall,
              name: 'Average In Comp Set (Overall)',
              data: dayOfWeek_Comp_P_Overall
          }
    ];

    if (!isOnlyOverall) {
        dowSeriesData.push({

            custom_type: 'M',
            color: bookingInsightOurColor_overall,
            data_arr: dayOfWeek_Ours,
            name: 'My Hotel' + currentSelectedType,
            data: dayOfWeek_Ours_P
        },
            {
                custom_type: 'C',
                color: bookingInsightCompColor_overall,
                data_arr: dayOfWeek_Comp,
                name: 'Average In Comp Set' + currentSelectedType,
                data: dayOfWeek_Comp_P
            });
    }

    var dayOfWeekChart = Highcharts.chart({

        chart: {
            renderTo: 'divDayOfWeek',
            type: 'bar',
            events: {
                load: function (event) {
                    $("#divDayOfWeek").LoadingOverlay("hide");
                }
            }
        },
        title: {
            text: 'Days Of The Week'
        },
        xAxis: {
            categories: ['Sun', 'Sat', 'Fri', 'Thu', 'Wed', 'Tue', 'Mon'],
            minorGridLineWidth: 0,
        },
        yAxis: {
            gridLineColor: 'transparent',
            min: 0,
            title: {
                text: '',
                align: 'high'
            },
            labels: {
                enabled: false,
                overflow: 'justify'
            }
        },
        tooltip: {

            formatter: function () {

                var s = '<b>' + this.x + '</b><table>';

                $.each(this.points, function (i, e) {
                    s += '<tr><td style="color:' + this.series.color + '">' + this.series.name + ': </td><td style="text-align: right"><b>' + ReplaceNumberWithCommas(this.series.options.data_arr[e.point.index]) + ' </b></td></tr>';
                });

                s += '</table>';
                return s;
            },
            shared: true,
            useHTML: true,
            followPointer: true,
        },
        plotOptions: {

            bar: {
                events: {
                    legendItemClick: function (e) {

                        ShowHideOverallSeries(dayOfWeekChart, e);
                    }
                },
                dataLabels: {
                    enabled: true,
                    style: {
                        fontWeight: 'bold'
                    },
                    formatter: function () {
                        if (this.y != 0) {
                            return this.y + '%';
                        } else {
                            return null;
                        }
                    }
                },
            }
        },
        legend: {
            layout: 'horizontal',
            align: 'center',
            borderWidth: 0,
            //backgroundColor: ((Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'),
            shadow: false
        },
        credits: {
            enabled: false
        },
        series: dowSeriesData
    });

    //#endregion DayOfWeek

    //#region LengthOfStay

    $('#divLengthOfStay').height(500);

    var lengthOfStay_Ours,
        lengthOfStay_Comp,
        lengthOfStay_Ours_P,
        lengthOfStay_Comp_P,

        lengthOfStay_Ours_Overall,
        lengthOfStay_Comp_Overall,
        lengthOfStay_Ours_P_Overall,
        lengthOfStay_Comp_P_Overall = [];


    try {

        //#region Overall

        lengthOfStay_Ours_Overall = [
               data.LengthOfStay[0].Y9_Overall,
               data.LengthOfStay[0].Y8_Overall,
               data.LengthOfStay[0].Y7_Overall,
               data.LengthOfStay[0].Y6_Overall,
               data.LengthOfStay[0].Y5_Overall,
               data.LengthOfStay[0].Y4_Overall,
               data.LengthOfStay[0].Y3_Overall,
               data.LengthOfStay[0].Y2_Overall,
               data.LengthOfStay[0].Y1_Overall,
               data.LengthOfStay[0].Y0_Overall];

        var LengthOfStayData_Ours_Overall_Result = $.grep(lengthOfStay_Ours_Overall, function (input) {
            return input > 0;
        });

        if (LengthOfStayData_Ours_Overall_Result.length == 0) {
            lengthOfStay_Ours_Overall = [];
        }

        lengthOfStay_Comp_Overall = [
           data.LengthOfStay[1].Y9_Overall,
           data.LengthOfStay[1].Y8_Overall,
           data.LengthOfStay[1].Y7_Overall,
           data.LengthOfStay[1].Y6_Overall,
           data.LengthOfStay[1].Y5_Overall,
           data.LengthOfStay[1].Y4_Overall,
           data.LengthOfStay[1].Y3_Overall,
           data.LengthOfStay[1].Y2_Overall,
           data.LengthOfStay[1].Y1_Overall,
           data.LengthOfStay[1].Y0_Overall];

        var LengthOfStayData_Comp_Overall_Result = $.grep(lengthOfStay_Comp_Overall, function (input) {
            return input > 0;
        });

        if (LengthOfStayData_Comp_Overall_Result.length == 0) {
            lengthOfStay_Comp_Overall = [];
        }


        lengthOfStay_Ours_P_Overall = [
           data.LengthOfStay[2].Y9_Overall,
           data.LengthOfStay[2].Y8_Overall,
           data.LengthOfStay[2].Y7_Overall,
           data.LengthOfStay[2].Y6_Overall,
           data.LengthOfStay[2].Y5_Overall,
           data.LengthOfStay[2].Y4_Overall,
           data.LengthOfStay[2].Y3_Overall,
           data.LengthOfStay[2].Y2_Overall,
           data.LengthOfStay[2].Y1_Overall,
           data.LengthOfStay[2].Y0_Overall];

        var LengthOfStayData_Ours_P_Overall_Result = $.grep(lengthOfStay_Ours_P_Overall, function (input) {
            return input > 0;
        });

        if (LengthOfStayData_Ours_P_Overall_Result.length == 0) {
            lengthOfStay_Ours_P_Overall = [];
        }

        lengthOfStay_Comp_P_Overall = [
          data.LengthOfStay[3].Y9_Overall,
          data.LengthOfStay[3].Y8_Overall,
          data.LengthOfStay[3].Y7_Overall,
          data.LengthOfStay[3].Y6_Overall,
          data.LengthOfStay[3].Y5_Overall,
          data.LengthOfStay[3].Y4_Overall,
          data.LengthOfStay[3].Y3_Overall,
          data.LengthOfStay[3].Y2_Overall,
          data.LengthOfStay[3].Y1_Overall,
          data.LengthOfStay[3].Y0_Overall];

        var LengthOfStayData_Comp_P_Overall_Result = $.grep(lengthOfStay_Comp_P_Overall, function (input) {
            return input > 0;
        });

        if (LengthOfStayData_Comp_P_Overall_Result.length == 0) {
            lengthOfStay_Comp_P_Overall = [];
        }

        //#endregion Overall

        //#region Other

        if (!isOnlyOverall) {

            lengthOfStay_Ours = [
                 data.LengthOfStay[4].Y9,
                 data.LengthOfStay[4].Y8,
                 data.LengthOfStay[4].Y7,
                 data.LengthOfStay[4].Y6,
                 data.LengthOfStay[4].Y5,
                 data.LengthOfStay[4].Y4,
                 data.LengthOfStay[4].Y3,
                 data.LengthOfStay[4].Y2,
                 data.LengthOfStay[4].Y1,
                 data.LengthOfStay[4].Y0];


            var LengthOfStayData_Ours_Result = $.grep(lengthOfStay_Ours, function (input) {
                return input > 0;
            });

            if (LengthOfStayData_Ours_Result.length == 0) {
                lengthOfStay_Ours = [];
            }


            lengthOfStay_Comp = [
               data.LengthOfStay[5].Y9,
               data.LengthOfStay[5].Y8,
               data.LengthOfStay[5].Y7,
               data.LengthOfStay[5].Y6,
               data.LengthOfStay[5].Y5,
               data.LengthOfStay[5].Y4,
               data.LengthOfStay[5].Y3,
               data.LengthOfStay[5].Y2,
               data.LengthOfStay[5].Y1,
               data.LengthOfStay[5].Y0];

            var LengthOfStayData_Comp_Result = $.grep(lengthOfStay_Comp, function (input) {
                return input > 0;
            });

            if (LengthOfStayData_Comp_Result.length == 0) {
                lengthOfStay_Comp = [];
            }

            lengthOfStay_Ours_P = [
               data.LengthOfStay[6].Y9,
               data.LengthOfStay[6].Y8,
               data.LengthOfStay[6].Y7,
               data.LengthOfStay[6].Y6,
               data.LengthOfStay[6].Y5,
               data.LengthOfStay[6].Y4,
               data.LengthOfStay[6].Y3,
               data.LengthOfStay[6].Y2,
               data.LengthOfStay[6].Y1,
               data.LengthOfStay[6].Y0];

            var LengthOfStayData_Ours_P_Result = $.grep(lengthOfStay_Ours_P, function (input) {
                return input > 0;
            });

            if (LengthOfStayData_Ours_P_Result.length == 0) {
                lengthOfStay_Ours_P = [];
            }

            lengthOfStay_Comp_P = [
               data.LengthOfStay[7].Y9,
               data.LengthOfStay[7].Y8,
               data.LengthOfStay[7].Y7,
               data.LengthOfStay[7].Y6,
               data.LengthOfStay[7].Y5,
               data.LengthOfStay[7].Y4,
               data.LengthOfStay[7].Y3,
               data.LengthOfStay[7].Y2,
               data.LengthOfStay[7].Y1,
               data.LengthOfStay[7].Y0];

            var LengthOfStayData_Comp_P_Result = $.grep(lengthOfStay_Comp_P, function (input) {
                return input > 0;
            });

            if (LengthOfStayData_Comp_P_Result.length == 0) {
                lengthOfStay_Comp_P = [];
            }
        }

        //#endregion Other

    } catch (e) {
        console.error(e)
    }


    var losChartSeriesData = [
             {
                 color: bookingInsightOurColor,
                 data_arr: lengthOfStay_Ours_Overall,
                 name: 'My Hotel (Overall)',
                 data: lengthOfStay_Ours_P_Overall
             },
            {
                color: bookingInsightCompColor,
                data_arr: lengthOfStay_Comp_Overall,
                name: 'Average In Comp Set (Overall)',
                data: lengthOfStay_Comp_P_Overall
            }
    ];

    if (!isOnlyOverall) {

        losChartSeriesData.push({

            custom_type: 'M',
            color: bookingInsightOurColor_overall,
            data_arr: lengthOfStay_Ours,
            name: 'My Hotel' + currentSelectedType,
            data: lengthOfStay_Ours_P
        },
            {

                custom_type: 'C',
                color: bookingInsightCompColor_overall,
                data_arr: lengthOfStay_Comp,
                name: 'Average In Comp Set' + currentSelectedType,
                data: lengthOfStay_Comp_P
            });
    }

    var lengthOfStayChartCategory = ['10+ days', '9 days', '8 days', '7 days', '6 days', '5 days', '4 days', '3 days', '2 days', '1 day'];

    var lengthOfStayToKeep = 0;

    var indexes = [];

    for (var i = 0; i < losChartSeriesData.length; i++) {

        losChartSeriesData[i].data.some(function (value, indx) {

            if (value != 0) {
                indexes.push(indx);
                return true;
            }
        });

        losChartSeriesData[i].data_arr.some(function (value, indx) {

            if (value != 0) {
                indexes.push(indx);
                return true;
            }
        });
    }

    var lengthToRemove = Math.min.apply(null, indexes);

    losChartSeriesData[0].data.splice(0, lengthToRemove)
    losChartSeriesData[0].data_arr.splice(0, lengthToRemove)

    losChartSeriesData[1].data.splice(0, lengthToRemove)
    losChartSeriesData[1].data_arr.splice(0, lengthToRemove)

    if (!isOnlyOverall) {

        losChartSeriesData[2].data.splice(0, lengthToRemove)
        losChartSeriesData[2].data_arr.splice(0, lengthToRemove)

        losChartSeriesData[3].data.splice(0, lengthToRemove)
        losChartSeriesData[3].data_arr.splice(0, lengthToRemove)
    }

    lengthOfStayChartCategory.splice(0, lengthToRemove)

    var lengthOfStayChart = Highcharts.chart({

        chart: {
            renderTo: 'divLengthOfStay',
            type: 'bar',
            events: {
                load: function (event) {
                    $("#divLengthOfStay").LoadingOverlay("hide");
                }
            }
        },
        title: {
            text: 'Length Of Stay'
        },
        xAxis: {
            categories: lengthOfStayChartCategory,
            //categories: ['10+ days', '9 days', '8 days', '7 days', '6 days', '5 days', '4 days', '3 days', '2 days', '1 day'],
            minorGridLineWidth: 0,
        },
        yAxis: {
            gridLineColor: 'transparent',
            min: 0,
            title: {
                text: '',
                align: 'high'
            },
            labels: {
                enabled: false,
                overflow: 'justify'
            }
        },
        tooltip: {

            formatter: function () {

                var s = '<b>' + this.x + '</b><table>';

                $.each(this.points, function (i, e) {
                    s += '<tr><td style="color:' + this.series.color + '">' + this.series.name + ': </td><td style="text-align: right"><b>' + ReplaceNumberWithCommas(this.series.options.data_arr[e.point.index]) + ' </b></td></tr>';
                });

                s += '</table>';
                return s;
            },
            shared: true,
            useHTML: true,
            followPointer: true,
        },
        plotOptions: {
            bar: {
                events: {
                    legendItemClick: function (e) {

                        ShowHideOverallSeries(lengthOfStayChart, e);
                    }
                },
                dataLabels: {
                    enabled: true,
                    style: {
                        fontWeight: 'bold'
                    },
                    formatter: function () {
                        if (this.y != 0) {
                            return this.y + '%';
                        }
                    }
                },
            }
        },
        legend: {
            layout: 'horizontal',
            align: 'center',
            borderWidth: 0,
            //backgroundColor: ((Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'),
            shadow: false
        },
        credits: {
            enabled: false
        },
        series: losChartSeriesData
    });

    //#endregion LengthOfStay

    //#region NumberOrRooms

    $('#divNumberOfRooms').height(500);

    var numberOfRooms_Ours,
       numberOfRooms_Comp,
       numberOfRooms_Ours_P,
       numberOfRooms_Comp_P,

       numberOfRooms_Ours_Overall,
       numberOfRooms_Comp_Overall,
       numberOfRooms_Ours_P_Overall,
       numberOfRooms_Comp_P_Overall = [];


    try {

        //#region Overall

        numberOfRooms_Ours_Overall = [
               data.NoOfRooms[0].Y7_Overall,
               data.NoOfRooms[0].Y6_Overall,
               data.NoOfRooms[0].Y5_Overall,
               data.NoOfRooms[0].Y4_Overall,
               data.NoOfRooms[0].Y3_Overall,
               data.NoOfRooms[0].Y2_Overall,
               data.NoOfRooms[0].Y1_Overall,
               data.NoOfRooms[0].Y0_Overall];

        var noOfRoomsData_Ours_Overall_Result = $.grep(numberOfRooms_Ours_Overall, function (input) {
            return input > 0;
        });

        if (noOfRoomsData_Ours_Overall_Result.length == 0) {
            numberOfRooms_Ours_Overall = [];
        }

        numberOfRooms_Comp_Overall = [
           data.NoOfRooms[1].Y7_Overall,
           data.NoOfRooms[1].Y6_Overall,
           data.NoOfRooms[1].Y5_Overall,
           data.NoOfRooms[1].Y4_Overall,
           data.NoOfRooms[1].Y3_Overall,
           data.NoOfRooms[1].Y2_Overall,
           data.NoOfRooms[1].Y1_Overall,
           data.NoOfRooms[1].Y0_Overall];

        var NoOfRoomsData_Comp_Overall_Result = $.grep(numberOfRooms_Comp_Overall, function (input) {
            return input > 0;
        });

        if (NoOfRoomsData_Comp_Overall_Result.length == 0) {
            NoOfRoomsData_Comp_Overall = [];
        }


        numberOfRooms_Ours_P_Overall = [
           data.NoOfRooms[2].Y7_Overall,
           data.NoOfRooms[2].Y6_Overall,
           data.NoOfRooms[2].Y5_Overall,
           data.NoOfRooms[2].Y4_Overall,
           data.NoOfRooms[2].Y3_Overall,
           data.NoOfRooms[2].Y2_Overall,
           data.NoOfRooms[2].Y1_Overall,
           data.NoOfRooms[2].Y0_Overall];

        var NoOfRoomsData_Ours_P_Overall_Result = $.grep(numberOfRooms_Ours_P_Overall, function (input) {
            return input > 0;
        });

        if (NoOfRoomsData_Ours_P_Overall_Result.length == 0) {
            numberOfRooms_Ours_P_Overall = [];
        }

        numberOfRooms_Comp_P_Overall = [
          data.NoOfRooms[3].Y7_Overall,
          data.NoOfRooms[3].Y6_Overall,
          data.NoOfRooms[3].Y5_Overall,
          data.NoOfRooms[3].Y4_Overall,
          data.NoOfRooms[3].Y3_Overall,
          data.NoOfRooms[3].Y2_Overall,
          data.NoOfRooms[3].Y1_Overall,
          data.NoOfRooms[3].Y0_Overall];

        var NoOfRoomsData_Comp_P_Overall_Result = $.grep(numberOfRooms_Comp_P_Overall, function (input) {
            return input > 0;
        });

        if (NoOfRoomsData_Comp_P_Overall_Result.length == 0) {
            numberOfRooms_Comp_P_Overall = [];
        }

        //#endregion Overall

        //#region Other

        if (!isOnlyOverall) {

            numberOfRooms_Ours = [
                 data.NoOfRooms[4].Y7,
                 data.NoOfRooms[4].Y6,
                 data.NoOfRooms[4].Y5,
                 data.NoOfRooms[4].Y4,
                 data.NoOfRooms[4].Y3,
                 data.NoOfRooms[4].Y2,
                 data.NoOfRooms[4].Y1,
                 data.NoOfRooms[4].Y0];


            var NoOfRoomsData_Ours_Result = $.grep(numberOfRooms_Ours, function (input) {
                return input > 0;
            });

            if (NoOfRoomsData_Ours_Result.length == 0) {
                numberOfRooms_Ours = [];
            }


            numberOfRooms_Comp = [
           data.NoOfRooms[5].Y7,
           data.NoOfRooms[5].Y6,
           data.NoOfRooms[5].Y5,
           data.NoOfRooms[5].Y4,
           data.NoOfRooms[5].Y3,
           data.NoOfRooms[5].Y2,
           data.NoOfRooms[5].Y1,
           data.NoOfRooms[5].Y0];

            var NoOfRoomsData_Comp_Result = $.grep(numberOfRooms_Comp, function (input) {
                return input > 0;
            });

            if (NoOfRoomsData_Comp_Result.length == 0) {
                numberOfRooms_Comp = [];
            }

            numberOfRooms_Ours_P = [
           data.NoOfRooms[6].Y7,
           data.NoOfRooms[6].Y6,
           data.NoOfRooms[6].Y5,
           data.NoOfRooms[6].Y4,
           data.NoOfRooms[6].Y3,
           data.NoOfRooms[6].Y2,
           data.NoOfRooms[6].Y1,
           data.NoOfRooms[6].Y0];

            var NoOfRoomsData_Ours_P_Result = $.grep(numberOfRooms_Ours_P, function (input) {
                return input > 0;
            });

            if (NoOfRoomsData_Ours_P_Result.length == 0) {
                numberOfRooms_Ours_P = [];
            }

            numberOfRooms_Comp_P = [
           data.NoOfRooms[7].Y7,
           data.NoOfRooms[7].Y6,
           data.NoOfRooms[7].Y5,
           data.NoOfRooms[7].Y4,
           data.NoOfRooms[7].Y3,
           data.NoOfRooms[7].Y2,
           data.NoOfRooms[7].Y1,
           data.NoOfRooms[7].Y0];

            var NoOfRoomsData_Comp_P_Result = $.grep(numberOfRooms_Comp_P, function (input) {
                return input > 0;
            });

            if (NoOfRoomsData_Comp_P_Result.length == 0) {
                numberOfRooms_Comp_P = [];
            }
        }

        //#endregion Other

    } catch (e) {
        console.error(e)
    }

    var numberOfRoomsSeriesData = [
             {
                 color: bookingInsightOurColor,
                 data_arr: numberOfRooms_Ours_Overall,
                 name: 'My Hotel (Overall)',
                 data: numberOfRooms_Ours_P_Overall
             },
            {
                color: bookingInsightCompColor,
                data_arr: numberOfRooms_Comp_Overall,
                name: 'Average In Comp Set (Overall)',
                data: numberOfRooms_Comp_P_Overall
            }];

    if (!isOnlyOverall) {

        numberOfRoomsSeriesData.push({
            showInLegend: !isOnlyOverall,
            custom_type: 'M',
            color: bookingInsightOurColor_overall,
            data_arr: numberOfRooms_Ours,
            name: 'My Hotel' + currentSelectedType,
            data: numberOfRooms_Ours_P
        },
          {
              showInLegend: !isOnlyOverall,
              custom_type: 'C',
              color: bookingInsightCompColor_overall,
              data_arr: numberOfRooms_Comp,
              name: 'Average In Comp Set' + currentSelectedType,
              data: numberOfRooms_Comp_P
          });
    }

    var numberOfRoomChartCategory = ['8 Rooms', '7 Rooms', '6 Rooms', '5 Rooms', '4 Rooms', '3 Rooms', '2 Rooms', '1 Room'];

    var numberOfRoomToKeep = 0;

    var indexes = [];

    for (var i = 0; i < numberOfRoomsSeriesData.length; i++) {

        numberOfRoomsSeriesData[i].data.some(function (value, indx) {

            if (value != 0) {
                indexes.push(indx);
                return true;
            }
        });

        numberOfRoomsSeriesData[i].data_arr.some(function (value, indx) {

            if (value != 0) {
                indexes.push(indx);
                return true;
            }
        });
    }

    var lengthToRemove = Math.min.apply(null, indexes);

    numberOfRoomsSeriesData[0].data.splice(0, lengthToRemove)
    numberOfRoomsSeriesData[0].data_arr.splice(0, lengthToRemove)

    numberOfRoomsSeriesData[1].data.splice(0, lengthToRemove)
    numberOfRoomsSeriesData[1].data_arr.splice(0, lengthToRemove)

    if (!isOnlyOverall) {

        numberOfRoomsSeriesData[2].data.splice(0, lengthToRemove)
        numberOfRoomsSeriesData[2].data_arr.splice(0, lengthToRemove)

        numberOfRoomsSeriesData[3].data.splice(0, lengthToRemove)
        numberOfRoomsSeriesData[3].data_arr.splice(0, lengthToRemove)
    }

    numberOfRoomChartCategory.splice(0, lengthToRemove);

    var numberOfRoomsChart = Highcharts.chart({

        chart: {
            renderTo: 'divNumberOfRooms',
            type: 'bar',
            events: {
                load: function (event) {
                    $("#divNumberOfRooms").LoadingOverlay("hide");
                }
            }
        },
        title: {
            text: 'Number Of Rooms'
        },
        xAxis: {
            categories: numberOfRoomChartCategory,
            //categories: ['8 Rooms', '7 Rooms', '6 Rooms', '5 Rooms', '4 Rooms', '3 Rooms', '2 Rooms', '1 Room'],
            minorGridLineWidth: 0,
        },
        yAxis: {
            gridLineColor: 'transparent',
            min: 0,
            title: {
                text: '',
                align: 'high'
            },
            labels: {
                enabled: false,
                overflow: 'justify'
            }
        },
        tooltip: {

            formatter: function () {

                var s = '<b>' + this.x + '</b><table>';

                $.each(this.points, function (i, e) {
                    s += '<tr><td style="color:' + this.series.color + '">' + this.series.name + ': </td><td style="text-align: right"><b>' + ReplaceNumberWithCommas(this.series.options.data_arr[e.point.index]) + ' </b></td></tr>';
                });

                s += '</table>';
                return s;
            },
            shared: true,
            useHTML: true,
            followPointer: true,
        },
        plotOptions: {
            bar: {
                events: {
                    legendItemClick: function (e) {

                        ShowHideOverallSeries(numberOfRoomsChart, e);
                    }
                },
                dataLabels: {
                    enabled: true,
                    style: {
                        fontWeight: 'bold'
                    },
                    formatter: function () {
                        if (this.y != 0) {
                            return this.y + '%';
                        } else {
                            return null;
                        }
                    }
                },
            }
        },
        legend: {
            layout: 'horizontal',
            align: 'center',
            borderWidth: 0,
            //backgroundColor: ((Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'),
            shadow: false
        },
        credits: {
            enabled: false
        },
        series: numberOfRoomsSeriesData
    });

    //#endregion NumberOfRooms

    //#region Room Productivity


    //$('#divRoomProductivity').height(350);

    //var roomProducitivityData = [];

    //$.each(data.RoomProductivities, function (i, element) {

    //    var item = {};
    //    item.name = element.RoomName;
    //    item.y = element.Value;
    //    if (item.y > 0)
    //        roomProducitivityData.push(item);
    //});


    //var roomProductivityChart = Highcharts.chart({

    //    chart: {
    //        renderTo: 'divRoomProductivity',
    //        type: 'column',
    //        events: {
    //            load: function (event) {
    //                $("#divRoomProductivity").LoadingOverlay("hide");
    //            }
    //        }
    //    },
    //    title: {
    //        text: 'Room Productivity'
    //    },
    //    xAxis: {
    //        type: 'category',

    //    },
    //    yAxis: {
    //        gridLineColor: '#197F07',
    //        gridLineWidth: 0,
    //        lineWidth: 1,
    //        plotLines: [{
    //            color: '#FF0000',
    //            width: 1,
    //            value: 0
    //        }],
    //        title: {
    //            enabled: false
    //        }
    //    },
    //    legend: {
    //        enabled: false
    //    },
    //    plotOptions: {
    //        series: {
    //            borderWidth: 0,
    //            dataLabels: {
    //                enabled: true,
    //                formatter: function () {

    //return ReplaceNumberWithCommas(this.point.y);

    //                }
    //            }
    //        }
    //    },

    //    tooltip: {
    //        shared: true,
    //        useHTML: true,
    //        followPointer: true,
    //        headerFormat: '',
    //        pointFormat: '<span style="color:{point.color}">{point.name}</span>:<b>{point.y}</b><br/>',
    //        valueDecimals: 0
    //    },

    //    series: [{
    //        name: '',
    //        colorByPoint: true,
    //        data: roomProducitivityData
    //    }]
    //});


    //#endregion Room Productivity

    //#region Drawing Charts from above data

    if (!leadTimeChart.hasData()) {
        var legend = leadTimeChart.legend;

        if (legend.display) {
            legend.group.hide();
            legend.box.hide();
            legend.display = false;
        }
    }

    if (!numberOfRoomsChart.hasData()) {

        var legend = numberOfRoomsChart.legend;

        if (legend.display) {
            legend.group.hide();
            legend.box.hide();
            legend.display = false;
        }
    }

    if (!lengthOfStayChart.hasData()) {
        var legend = lengthOfStayChart.legend;

        if (legend.display) {
            legend.group.hide();
            legend.box.hide();
            legend.display = false;
        }
    }

    if (!dayOfWeekChart.hasData()) {

        var legend = dayOfWeekChart.legend;

        if (legend.display) {
            legend.group.hide();
            legend.box.hide();
            legend.display = false;
        }
    }

    if (!leadTimeChart.hasData() && !numberOfRoomsChart.hasData()) {

        leadTimeChart.setSize(leadTimeChart.plotWidth, 100, true);
        numberOfRoomsChart.setSize(numberOfRoomsChart.plotWidth, 100, true);

        $('#divLeadTime').height(100);
        $('#divNumberOfRooms').height(100);
    }

    if (!lengthOfStayChart.hasData() && !dayOfWeekChart.hasData()) {

        lengthOfStayChart.setSize(lengthOfStayChart.plotWidth, 100, true);
        dayOfWeekChart.setSize(dayOfWeekChart.plotWidth, 100, true);

        $('#divLengthOfStay').height(100);
        $('#divDayOfWeek').height(100);

    }

    //if (!roomProductivityChart.hasData()) {
    //    roomProductivityChart.setSize(roomProductivityChart.plotWidth, 100, true);
    //    $('#divRoomProductivity').height(100);
    //}

    //#endregion
}


function ShowHideOverallSeries(chartObject, event) {

    //var series0 = chartObject.series[0];
    //var series1 = chartObject.series[1];

    //if (!series0.visible && !series1.visible) {

    //    if (event.target.userOptions.custom_type == 'M') {
    //        series0.show();
    //    }
    //    else {
    //        series1.show();
    //    }
    //}
    //else {

    //    if (event.target.userOptions.custom_type == 'M') {

    //        if (series0.visible) {
    //            series0.hide();
    //        } else {
    //            series0.show();
    //        }
    //    }
    //    else {

    //        if (series1.visible) {
    //            series1.hide();
    //        } else {
    //            series1.show();
    //        }
    //    }
    //}
}


function GetBookingOverviewiewData_AndDrawChart(iPropId, iDays) {

    $.ajax({
        url: '/Property/GetBookingOverviewData',
        dataType: 'json',
        async: true,
        data: { iPropId: iPropId, iDays: iDays },
        success: function (result) {

            try {

                DrawBookingOverviewChart(result.data);

            } catch (e) {
                console.error(e)
            }

        },
        error: function () {

        },
        always: function () {

        }
    });
}


function GetNegotiationOverviewData_AndDrawChart(iPropId, iDays) {

    $.ajax({
        url: '/Property/GetNegotiationOverViewData',
        dataType: 'json',
        async: true,
        data: { iPropId: iPropId, iDays: iDays },
        success: function (result) {

            try {

                DrawNegotiationOverviewChart(result.data);

            } catch (e) {
                console.error(e)
            }

        },
        error: function () {

        },
        always: function () {

        }
    });
}


function GetBookingInsightsData_AndDrawChart(iPropId, iDays, cType, bookingOrRevenue) {

    $.ajax({
        url: '/Property/GetBookingInsightsData',
        dataType: 'json',
        async: true,
        data: { iPropId: iPropId, iDays: iDays, cType: cType, bookingOrRevenue: bookingOrRevenue },
        success: function (result) {

            try {

                DrawBookingInsightsChart(result.data);

            } catch (e) {
                console.error(e)
            }

        },
        error: function () {

        },
        always: function () {

        }
    });
}


function GetPerformanceOverview_AndBind(iPropId, iDays) {

    $.ajax({
        url: '/Property/GetPerformanceOverView',
        dataType: 'html',
        async: true,
        data: { iPropId: iPropId, iDays: iDays },
        success: function (result) {
            $('#divPerformanceOverview').html(result);
        },
        error: function () {
        },
        always: function () {
        }
    });
}


try {

    var iPropId = $('#hdnPropId').val();
    GetBookingOverviewiewData_AndDrawChart(iPropId, 7);
    GetNegotiationOverviewData_AndDrawChart(iPropId, 7);
    GetBookingInsightsData_AndDrawChart(iPropId, 7, 'O', 'N');
    GetPerformanceOverview_AndBind(iPropId, 7);

} catch (e) {
    console.error(e)
}


$(function () {

    Highcharts.setOptions({
        lang: {
            thousandsSep: ','
        }
    });

    $('#ddlBookingOverview').change(function () {

        var value = $(this).val();

        var iPropId = $('#hdnPropId').val();

        try {
            GetBookingOverviewiewData_AndDrawChart(iPropId, value);
        } catch (e) {
            //console.error(e)

        }
    });

    $('#ddlNegotitaionOverview').change(function () {

        var value = $(this).val();

        var iPropId = $('#hdnPropId').val();

        try {

            GetNegotiationOverviewData_AndDrawChart(iPropId, value);

        } catch (e) {
            console.error(e)
        }
    });

    $('#ddlBookingInsights').change(function () {

        var value = $(this).val();
        var bookingType = $("input:radio[name='radioBookingType']:checked").val();
        var bookingOrRevenue = $('input[name=bookingOrRevenue]:checked').val();
        var iPropId = $('#hdnPropId').val();

        switch (bookingType) {

            case 'B': {
                currentSelectedType = '(Bid)';
                break;
            }
            case 'N': {
                currentSelectedType = '(Negotiate)';
                break;
            }
            case 'R': {
                currentSelectedType = '(Buy)';
                break;
            }
            case 'C': {
                currentSelectedType = '(Corporate)';
                break;
            }
            default: break;
        }

        try {

            GetBookingInsightsData_AndDrawChart(iPropId, value, bookingType, bookingOrRevenue);

        } catch (e) {
            console.error(e);
        }
    });

    $('#ddlPerformanceOverview').change(function () {


        var value = $(this).val();
        var iPropId = $('#hdnPropId').val();

        try {
            GetPerformanceOverview_AndBind(iPropId, value);

        } catch (e) {
            console.error(e);
        }
    });

    $('input:radio[name="radioBookingType"]').change(function () {

        $('#ddlBookingInsights').change();
    })

    $('.lnkBookingOrRevenue').click(function () {

        $('.lnkBookingOrRevenue').removeClass('booking-active');

        $(this).addClass('booking-active');

        var type = $(this).data('type');

        if (type == "B") {
            $("#rdBooking").prop("checked", true)
        }
        else if (type == "R") {
            $("#rdRevenue").prop("checked", true)
        }

        $('#ddlBookingInsights').change();
    });

    setTimeout(function () {
        $(".loadingOverlay").LoadingOverlay("hide");
    }, 10000)

});

function ReplaceNumberWithCommas(yourNumber) {
    //Seperates the components of the number
    yourNumber = Math.round(yourNumber);
    var n = yourNumber.toString().split(".");
    //Comma-fies the first part
    n[0] = n[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    //Combines the two sections
    return n.join(".");
}