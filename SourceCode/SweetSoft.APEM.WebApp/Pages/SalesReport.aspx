<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master" AutoEventWireup="true" CodeBehind="SalesReport.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.SalesReport" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="SweetSoft" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row" style="margin-top: 20px;">
        <div class="col-sm-12">
            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="col-md-3 col-sm-3">
                        <label class="control-label">
                            <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CUSTOMER)%>:
                        </label>
                        <SweetSoft:CustomExtraTextbox ID="txtName" RenderOnlyInput="true" Placeholder="Customer's name"
                            runat="server" AutoCompleteType="Search"></SweetSoft:CustomExtraTextbox>
                        <asp:HiddenField ID="hCustomerID" runat="server" />
                    </div>
                    <div class="col-md-3 col-sm-3">
                        <label class="control-label">Product type:</label>
                        <div class="form-group">
                            <asp:DropDownList ID="ddlProductType" runat="server"
                                data-style="btn btn-info"
                                data-width="100%" Required="true"
                                data-toggle="dropdown"
                                CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3">
                        <label class="control-label">Sale Persons:</label>
                        <div class="form-group">
                            <asp:DropDownList ID="ddlSale" runat="server"
                                data-style="btn btn-info"
                                data-width="100%" Required="true"
                                data-toggle="dropdown"
                                CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3">
                        <label class="control-label">Order Status:</label>
                        <div class="form-group">
                            <asp:DropDownList ID="ddlOrderStatus" runat="server"
                                data-style="btn btn-info"
                                data-width="100%" Required="true"
                                data-toggle="dropdown"
                                CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-12 col-sm-12">
                    <div class="col-md-3 col-sm-3">
                        <label class="control-label">Order date:</label>
                        <div class="row">
                            <div class="col-xs-4">
                                <div class="form-group">
                                    <p class="form-control-static">From date</p>
                                </div>
                            </div>
                            <div class="col-xs-8">
                                <div class="form-group">
                                    <div class="wrap-datepicker">
                                        <SweetSoft:CustomExtraTextbox ID="txtFromDate" runat="server"
                                            RenderOnlyInput="true" data-format="dd-MM-yyyy"
                                            CssClass="datepicker form-control mask-date">
                                        </SweetSoft:CustomExtraTextbox>
                                        <span class="fa fa-calendar in-mask-date"></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-4">
                                <div class="form-group">
                                    <p class="form-control-static">To date</p>
                                </div>
                            </div>
                            <div class="col-xs-8">
                                <div class="form-group">
                                    <div class="wrap-datepicker">
                                        <SweetSoft:CustomExtraTextbox ID="txtToDate" runat="server"
                                            RenderOnlyInput="true" data-format="dd-MM-yyyy"
                                            CssClass="datepicker form-control mask-date">
                                        </SweetSoft:CustomExtraTextbox>
                                        <span class="fa fa-calendar in-mask-date"></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3">
                        <label class="control-label">Invoice date:</label>
                        <div class="row">
                            <div class="col-xs-4">
                                <div class="form-group">
                                    <p class="form-control-static">From date</p>
                                </div>
                            </div>
                            <div class="col-xs-8">
                                <div class="form-group">
                                    <div class="wrap-datepicker">
                                        <SweetSoft:CustomExtraTextbox ID="txtFromDateInvoice" runat="server"
                                            RenderOnlyInput="true" data-format="dd-MM-yyyy"
                                            CssClass="datepicker form-control mask-date">
                                        </SweetSoft:CustomExtraTextbox>
                                        <span class="fa fa-calendar in-mask-date"></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-4">
                                <div class="form-group">
                                    <p class="form-control-static">To date</p>
                                </div>
                            </div>
                            <div class="col-xs-8">
                                <div class="form-group">
                                    <div class="wrap-datepicker">
                                        <SweetSoft:CustomExtraTextbox ID="txtToDateInvoice" runat="server"
                                            RenderOnlyInput="true" data-format="dd-MM-yyyy"
                                            CssClass="datepicker form-control mask-date">
                                        </SweetSoft:CustomExtraTextbox>
                                        <span class="fa fa-calendar in-mask-date"></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 col-sm-6">
                        <label class="control-label">&nbsp;</label>
                        <div class="form-group">
                            <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-primary"
                                OnClick="btnSearch_Click">
                                <span class="glyphicon glyphicon-search"></span>&nbsp;
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.SEARCH)%>
                            </asp:LinkButton>
                            <label class="control-label">&nbsp;</label>
                            <asp:UpdatePanel ID="upExport" runat="server" RenderMode="Inline">
                                <ContentTemplate>
                                    <div class="btn-group">
                                        <button type="button" class="btn btn-primary dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                                            Download&nbsp;<span class="caret"></span>
                                        </button>
                                        <ul class="dropdown-menu" role="menu">
                                            <li>
                                                <asp:LinkButton ID="btnByProductType" runat="server" Text="Sales Report - By Product Type"
                                                    OnClick="btnByProductType_Click">
                                                </asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="btnByCustomer" runat="server" Text="Sales Report - By Customer"
                                                    OnClick="btnByCustomer_Click">
                                                </asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="tblBySale" runat="server" Text="Sales Report - By Sales Person"
                                                    OnClick="tblBySale_Click">
                                                </asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="btnByBrandOwner" runat="server" Text="Sales Report - By Brand Owners"
                                                    OnClick="btnByBrandOwner_Click">
                                                </asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="btnGeneralSalesReport" runat="server" Text="General sales report" 
                                                    OnClick="btnGeneralSalesReport_Click">
                                            
                                                </asp:LinkButton>
                                            </li>
                                        </ul>
                                    </div>

                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnByProductType" />
                                    <asp:PostBackTrigger ControlID="btnByCustomer" />
                                    <asp:PostBackTrigger ControlID="tblBySale" />
                                    <asp:PostBackTrigger ControlID="btnByBrandOwner" />
                                    <asp:PostBackTrigger ControlID="btnGeneralSalesReport" />
                                </Triggers>
                            </asp:UpdatePanel>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row" style="margin-top: 15px;">
        <div class="col-md-12">
            <div class="table-responsive">
                <SweetSoft:GridviewExtension ID="grvSalesReport" runat="server" AutoGenerateColumns="false"
                    CssClass="table table-striped table-bordered table-checkable dataTable" GridLines="None"
                    AllowPaging="true" AllowSorting="false" DataKeyNames=""
                    OnPageIndexChanging="grvSalesReport_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="Job Number" HeaderStyle-CssClass="column-120 text-center" ItemStyle-CssClass="column-120">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#Eval("JobNumber")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sale Persons" HeaderStyle-CssClass="column-120 text-center" ItemStyle-CssClass="column-120">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#Eval("FirstName")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SAP Cust ID" HeaderStyle-CssClass="column-120 text-center" ItemStyle-CssClass="column-120">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#Eval("SAPCode")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sold to party" HeaderStyle-CssClass="column-250 text-center" ItemStyle-CssClass="column-250">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#Eval("CustomerName")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Country" HeaderStyle-CssClass="column-60 text-center" ItemStyle-CssClass="column-60">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#Eval("Code")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Job Name" HeaderStyle-CssClass="column-250 text-center" ItemStyle-CssClass="column-250">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#Eval("JobName")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Design Name" HeaderStyle-CssClass="column-250 text-center" ItemStyle-CssClass="column-250">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#Eval("Design")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Width" HeaderStyle-CssClass="column-80 text-center" ItemStyle-CssClass="column-80 text-right">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#ShowNumberFormat(Eval("FaceWidth"), "N2")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Circ" HeaderStyle-CssClass="column-80 text-center" ItemStyle-CssClass="column-80 text-right">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#ShowNumberFormat(Eval("Circumference"), "N2")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Currency" HeaderStyle-CssClass="column-60 text-center" ItemStyle-CssClass="column-60">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#Eval("CurrencyName")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Exchange Rate" HeaderStyle-CssClass="column-120 text-center" ItemStyle-CssClass="column-120 text-right">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#ShowNumberFormat(Eval("RMValue"), "N4")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="De/Re Qty" HeaderStyle-CssClass="column-100 text-center" ItemStyle-CssClass="column-100 text-right">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#ShowNumberFormat(Eval("DeReQty"), "N0")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="De/Re Value" HeaderStyle-CssClass="column-100 text-center" ItemStyle-CssClass="column-100 text-right">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#ShowNumberFormat(Eval("DeReTotalPrice"), "N2")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Engr Old Qty" HeaderStyle-CssClass="column-120 text-center" ItemStyle-CssClass="column-120 text-right">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#ShowNumberFormat(Eval("OldQty"), "N0")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Engr New Qty" HeaderStyle-CssClass="column-120 text-center" ItemStyle-CssClass="column-120 text-right">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#ShowNumberFormat(Eval("NewQty"), "N0")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Engr Total Qty" HeaderStyle-CssClass="column-120 text-center" ItemStyle-CssClass="column-120 text-right">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#ShowNumberFormat(Eval("TotalQty"), "N0")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Engr Value" HeaderStyle-CssClass="column-120 text-center" ItemStyle-CssClass="column-120 text-right">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#ShowNumberFormat(Eval("TotalPrice"), "N2")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Service Jobs" HeaderStyle-CssClass="column-120 text-center" ItemStyle-CssClass="column-120 text-right">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#ShowNumberFormat(Eval("ServiceJobs"), "N2")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Other Charges" HeaderStyle-CssClass="column-120 text-center" ItemStyle-CssClass="column-120 text-right">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#ShowNumberFormat(Eval("OtherCharges"), "N2")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Discount" HeaderStyle-CssClass="column-100 text-center" ItemStyle-CssClass="column-100 text-right">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#ShowNumberFormat(Eval("Discount"), "N3") + "%"%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Subtotal" HeaderStyle-CssClass="column-100 text-center" ItemStyle-CssClass="column-100 text-right">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#ShowNumberFormat(Eval("SubTotal"), "N2")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="InvoiceNo" HeaderStyle-CssClass="column-100 text-center" ItemStyle-CssClass="column-100">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#Eval("InvoiceNo")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="InvoiceDate" HeaderStyle-CssClass="column-100 text-center" ItemStyle-CssClass="column-100">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#Eval("InvoiceDate")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PostingDate" HeaderStyle-CssClass="column-100 text-center" ItemStyle-CssClass="column-100">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%#Eval("InvoicePostingDate")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tax Code" HeaderStyle-CssClass="column-100 text-center" ItemStyle-CssClass="column-100">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#Eval("TaxCode")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tax Rate" HeaderStyle-CssClass="column-100 text-center" ItemStyle-CssClass="column-100 text-right">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#ShowNumberFormat(Eval("TaxPercentage"), "N2") + "%"%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sales Value in Foreign Currency" HeaderStyle-CssClass="column-150 text-center" ItemStyle-CssClass="column-150 text-right">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#ShowNumberFormat(Eval("TotalOverseas"), "N2")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sales Value in MYR" HeaderStyle-CssClass="column-150 text-center" ItemStyle-CssClass="column-150 text-right">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#ShowNumberFormat(Eval("TotalRM"), "N2")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Payment Terms" HeaderStyle-CssClass="column-180 text-center" ItemStyle-CssClass="column-180">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#Eval("PaymentTern")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle BorderStyle="None" />
                    <PagerSettings
                        Mode="NumericFirstLast"
                        PageButtonCount="5"
                        FirstPageText="&laquo;"
                        LastPageText="&raquo;"
                        NextPageText="&rsaquo;"
                        PreviousPageText="&lsaquo;"
                        Position="Bottom" />
                    <PagerTemplate>
                        <SweetSoft:GridViewPager ID="GridViewPager1" runat="server" />
                    </PagerTemplate>
                    <EmptyDataTemplate>
                        There are currently no items in this table.
                    </EmptyDataTemplate>
                </SweetSoft:GridviewExtension>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ModalPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            SearchText();
        })

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SearchText);
        function SearchText(s, a) {

            $("input[type='text'][id$='txtName']").focus(function () { $(this).select(); });

            if ($("input[type='text'][id$='txtName']").length > 0) {
                $(".ui-autocomplete,.ui-dialog, .ui-helper-hidden-accessible").remove();
                $("input[type='text'][id$='txtName']").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "OrderConfirmationList.aspx/GetCustomerData",
                            data: "{'Keyword':'" + $("input[type='text'][id$='txtName']").val() + "'}",
                            dataType: "json",
                            async: false,
                            cache: false,
                            success: function (result) {
                                response($.map($.parseJSON(result.d), function (item) {
                                    return { ID: item.CustomerID, Name: item.Name, Code: item.Code };
                                }));
                            }
                        });
                    },
                    messages: {
                        noResults: '',
                        results: function () { }
                    },
                    focus: function (event, ui) {
                        return false;
                    },
                    select: function (event, ui) {
                        $("input[type='text'][id$='txtName']").val(ui.item.Name);
                        $("input[type='hidden'][id$='hCustomerID']").val(ui.item.ID);
                        return false;
                    }
                }).data("ui-autocomplete")._renderItem = function (ul, item) {
                    return $("<li>")
                        .data("ui-autocomplete-item", item)
                        .append("<a><span style='width:30px;'>" + item.Code + '</span> --- ' + item.Name + "</a>")
                        .appendTo(ul);
                };
            }
        }
    </script>
</asp:Content>
