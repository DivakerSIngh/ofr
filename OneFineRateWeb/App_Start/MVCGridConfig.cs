[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(OneFineRateWeb.MVCGridConfig), "RegisterGrids")]

namespace OneFineRateWeb
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Linq;
    using System.Collections.Generic;

    using MVCGrid.Models;
    using MVCGrid.Web;
    using OneFineRateBLL.Entities;
    using OneFineRateBLL;
    using Microsoft.AspNet.Identity;

    public static class MVCGridConfig
    {
        public static void RegisterGrids()
        {
        //    var columnDefault = new ColumnDefaults();

        //    MVCGridDefinitionTable.Add("CurrentNegotiations", new MVCGridBuilder<etblBookingTx>(columnDefault)
        //     .WithAuthorizationType(AuthorizationType.AllowAnonymous)
        //     .WithPaging(paging: true, itemsPerPage: 5, allowChangePageSize: false, maxItemsPerPage: 50)
        //     .WithQueryStringPrefix("c")
        //     .AddColumns(cols =>
        //     {
        //         cols.Add("HotelName").WithHeaderText("Hotel Name, City").WithHtmlEncoding(false)
        //             .WithValueTemplate("{Model.sHotelName}, <span class='bluetext'>{Model.sPropertyCityName}</span>");

        //         cols.Add("PublishedRate").WithHeaderText("Published Rate")
        //             .WithValueTemplate("{Model.dTotalAmount}");

        //         cols.Add("YourAmount").WithHeaderText("Your Amount")
        //             .WithValueTemplate("{Model.dTotalNegotiateAmount}");

        //         cols.Add("Status").WithHeaderText("Status")
        //             .WithValueTemplate("{Model.cBookingStatus}");
        //     })
        //     .WithAdditionalSetting(MVCGrid.Rendering.BootstrapRenderingEngine.SettingNameTableClass, "table table-bordered table-hover table-striped") // Example of changing table css class
        //     .WithRetrieveDataMethod((context) =>
        //     {
        //         var options = context.QueryOptions;

        //         int totalRecords;

        //         string globalSearch = options.GetAdditionalQueryOptionString("search");

        //         string sortColumn = options.GetSortColumnData<string>();

        //         int pageNumber = (options.GetLimitOffset().Value / options.GetLimitRowcount().Value) + 1;

        //         var items = BL_WebsiteUser.GetCurrentNegotialions(out totalRecords, HttpContext.Current.User.Identity.GetUserId<long>(),pageNumber);

        //         return new QueryResult<etblBookingTx>()
        //         {
        //             Items = items,
        //             TotalRecords = totalRecords
        //         };
        //     })
        // );


        //    MVCGridDefinitionTable.Add("PastSavings", new MVCGridBuilder<etblBookingTx>(columnDefault)
        //    .WithAuthorizationType(AuthorizationType.AllowAnonymous)
        //    .WithPaging(paging: true, itemsPerPage: 5, allowChangePageSize: false, maxItemsPerPage: 50)
        //    .WithQueryStringPrefix("p")
        //    .AddColumns(cols =>
        //    {
        //        cols.Add("HotelName").WithHeaderText("Hotel Name, City").WithHtmlEncoding(false)                    
        //            .WithValueTemplate("{Model.sHotelName}, <span class='bluetext'>{Model.sPropertyCityName}</span>");

        //        cols.Add("PublishedRate").WithHeaderText("Published Rate")
        //            .WithValueTemplate("{Model.dTotalAmount}");

        //        cols.Add("YourAmount").WithHeaderText("Your Amount")
        //            .WithValueTemplate("{Model.dTotalNegotiateAmount}");

        //        cols.Add("BidAmount").WithHeaderText("Bid Amount")
        //           .WithValueTemplate("{Model.dBidAmount}");

        //        cols.Add("YouSaved").WithHeaderText("You Saved")
        //            .WithValueTemplate("{Model.sSavingsInPercentage} %");
        //    })
        //    .WithAdditionalSetting(MVCGrid.Rendering.BootstrapRenderingEngine.SettingNameTableClass, "table table-bordered table-hover table-striped") // Example of changing table css class
        //    .WithRetrieveDataMethod((context) =>
        //    {
        //        var options = context.QueryOptions;

        //        int totalRecords;

        //        string globalSearch = options.GetAdditionalQueryOptionString("search");

        //        string sortColumn = options.GetSortColumnData<string>();

        //         int pageNumber =  (options.GetLimitOffset().Value / options.GetLimitRowcount().Value) + 1;

        //        var items = BL_WebsiteUser.GetPastSavings(out totalRecords, HttpContext.Current.User.Identity.GetUserId<long>(),pageNumber);

        //        return new QueryResult<etblBookingTx>()
        //        {
        //            Items = items,
        //            TotalRecords = totalRecords
        //        };
        //    })
        //);
        }
    }
}