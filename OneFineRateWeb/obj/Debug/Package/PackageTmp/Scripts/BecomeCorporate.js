
$("#sCompanyName").autocomplete({
    source: function (request, response) {
        debugger;
        var url = "/Corporate/GetCompaniesAutoComplete";
                $.ajax({
                    url: "/Corporate/GetCompaniesAutoComplete",
                    type: "GET",
                    dataType: "json",
                    data: { companySearchTxt: request.term },
                    success: function (data) {
                        debugger;
                        response($.map(data, function (item) {
                            return { label: item.sCompanyName, value: item.iCompanyId };
                        }))

                    }
                })
            },
            select: function (event, ui) {
                debugger;
                $("#sCompanyName").val(ui.item.label);
                $('#iCompanyID').val(ui.item.value);
                event.preventDefault();
            },error: function(jqXHR, textStatus, errorThrown){
                alert("error handler!");                        
            },
            focus: function (event, ui) {
                event.preventDefault();
                return false;
            }
        });
