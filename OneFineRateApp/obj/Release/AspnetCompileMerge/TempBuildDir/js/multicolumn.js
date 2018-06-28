
(function ($) {
    $.fn.multicolumndropdown = function (options) {

        function multicol(options, elem) {
            this.settings = $.extend({
                method: 'POST',
                dataType: 'json',
                data: {},
                container: elem,
                textField: "engine", // Give Column Name
                valueField: "browser", // Give Column Name             
                "bProcessing": true,
                sAjaxSource: "",
                aoColumnUrl: "",
                onSelect: function (aData) {
                },
                aoColumns: []
            }, options);
        }
        multicol.prototype.settings = {};
        multicol.prototype.tblElm = document.createElement("TABLE");
        multicol.prototype.tblWraper = document.createElement("DIV");
        multicol.prototype.mltClmnWraper = document.createElement("DIV");

        multicol.prototype.oTable = null;
        multicol.prototype.init = function () {

            $(call.mltClmnWraper).addClass('mltClmnWraper');

            $(call.tblWraper).attr({ 'id': 'container_' + $(this.settings.container).attr('id') });

            $(call.tblWraper).addClass('multicolcontainer');
            $(call.tblWraper).hide();

            $(call.tblElm).attr({ 'id': 'data_' + $(this.settings.container).attr('id') });
            $(call.tblElm).addClass("display");
            $(call.tblWraper).html(multicol.prototype.tblElm);
            $(call.mltClmnWraper).insertBefore(this.settings.container);
            $(call.mltClmnWraper).append(this.settings.container);
            $(call.mltClmnWraper).append($(call.tblWraper));





            var options = this.settings;

            $.ajax({
                type: this.settings.method,
                async: true,
                url: this.settings.aoColumnUrl,
                contentType: "application/json; charset=utf-8",
                dataType: this.settings.dataType,
                cache: false,
                success: function (data) {
                    options.aoColumns = $.parseJSON(data.d);

                    $.map(options.aoColumns, function (obj) {
                        if (obj.is_text === true) {
                            options.textField = obj.mData;
                        }
                        if (obj.is_value === true) {
                            options.valueField = obj.mData;
                        }
                    });


                    call.oTable = $(call.tblElm).dataTable({
                        "bDeferRender": true,
                        "bProcessing": true,
                        "bServerSide": true,
                        "sAjaxSource": options.sAjaxSource,
                        "aoColumns": options.aoColumns,
                        "sServerMethod": options.method,
                        "sPaginationType": "two_button",
                        "sDom": '<"top">rt<"bottom"ip><"clear">',
                        //"sScrollY": "300px",
                        //"sScrollX": "400px",
                        // "sScrollXInner": "600px",
                        "bPaginate": true,
                        "bAutoWidth": true,
                        "bScrollCollapse": true,
                        "oLanguage": {
                            "oPaginate": {
                                "sNext": "",
                                "sPrevious": ""
                            }
                        },
                        "fnServerData": function (Url, aoData, fnCallback, oSettings) {
                            oSettings.jqXHR = $.ajax({
                                "url": Url,
                                "data": aoData,
                                "success": function (json) {

                                    var jsondata = $.parseJSON(json.d);

                                    if (jsondata.sError) {
                                        oSettings.oApi._fnLog(oSettings, 0, jsondata.sError);
                                    }

                                    $(oSettings.oInstance).trigger('xhr', [oSettings, jsondata]);
                                    fnCallback(jsondata);
                                },
                                "dataType": "json",
                                "cache": false,
                                "type": "POST",
                                "error": function (xhr, error, thrown) {
                                    if (error == "parsererror") {
                                        oSettings.oApi._fnLog(oSettings, 0, "DataTables warning: JSON data from " +
							"server could not be parsed. This is caused by a JSON formatting error.");
                                    }
                                }
                            });

                        },
                        "fnInitComplete": function () {
                            $(this).css('width', '100%');
                            IECss();
                            //                            $('body').find('.dataTables_scrollBody').bind('jsp-scroll-x', function (event, scrollPositionX, isAtLeft, isAtRight) {
                            //                                table_header.css('right', scrollPositionX);
                            //                            })
                        }
                    });

                    
                }
            });



            $(this.settings.container).focus(function () { /** Toggle The DataTable  **/
                //call.oTable.fnAdjustColumnSizing();
                $('.multicolcontainer').hide();
                $(this).parent().find(call.tblWraper).show();
            });
            $(document).click(function (event) {
                var curCls = $(event.target).parents(".mltClmnWraper").attr("class");
                if (!curCls) {
                    $(this).find(call.tblWraper).hide();
                }

            });
            $(this.settings.container).keyup(function () {
                /* Filter on the column (the index) of this element */
                call.oTable.fnFilter($(this).val(), null);
            });

            $(multicol.prototype.tblElm).delegate('tbody tr', 'click', function () {
                call.onSelect(this);
            });
        };

        multicol.prototype.onSelect = function (elem) {

            var aPos = this.oTable.fnGetPosition(elem);
            var aData = this.oTable.fnGetData(aPos);
            var txtFld = this.settings.textField;
            var txtVal = this.settings.valueField;

            $(this.settings.container).val(aData[txtFld]);
            $(this.settings.container).attr('hidden_field', aData[txtVal]);

            $(this.tblWraper).hide();
            this.settings.onSelect(aData);
        }


        var call = new multicol(options, this);
        call.init();

    }

})(jQuery);

