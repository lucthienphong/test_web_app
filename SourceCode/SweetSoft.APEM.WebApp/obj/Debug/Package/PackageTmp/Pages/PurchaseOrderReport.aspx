<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master" AutoEventWireup="true" CodeBehind="PurchaseOrderReport.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.PurchaseOrderReport" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="SweetSoft" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row" style="margin-top: 20px;">
        <div class="col-sm-12">
            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="col-md-2 col-sm-2">
                    </div>
                    <div class="col-md-3 col-sm-3">
                        <label class="control-label">
                            <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CUSTOMER)%>:
                        </label>
                        <SweetSoft:CustomExtraTextbox ID="txtName" RenderOnlyInput="true" Placeholder="Customer's name"
                            runat="server" AutoCompleteType="Search"></SweetSoft:CustomExtraTextbox>
                        <asp:HiddenField ID="hCustomerID" runat="server" />
                    </div>
                    <div class="col-md-3 col-sm-3">
                        <label class="control-label">Suppliers:</label>
                        <div class="form-group">
                            <asp:DropDownList ID="ddlSuppliers" runat="server"
                                data-style="btn btn-info"
                                data-width="100%" Required="true"
                                data-toggle="dropdown"
                                CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-12 col-sm-12">
                    <div class="col-md-2 col-sm-2">
                    </div>
                    <div class="col-md-6 col-sm-6">
                        <label class="control-label">Order date:</label>
                        <div class="row">
                            <div class="col-xs-2">
                                <div class="form-group">
                                    <p class="form-control-static">From date</p>
                                </div>
                            </div>
                            <div class="col-xs-4">
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
                            <div class="col-xs-2">
                                <div class="form-group">
                                    <p class="form-control-static">To date</p>
                                </div>
                            </div>
                            <div class="col-xs-4">
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
                                                <asp:LinkButton ID="btnByCustomer" runat="server" Text="Purchase Order Report - By Customer"
                                                    OnClick="btnByCustomer_Click">
                                                </asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="btnBySuppliers" runat="server" Text="Purchase Order Report - By Suppliers"
                                                    OnClick="btnBySuppliers_Click">
                                                </asp:LinkButton>
                                            </li>
                                        </ul>
                                    </div>

                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnByCustomer" />
                                    <asp:PostBackTrigger ControlID="btnBySuppliers" />
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
                        <asp:TemplateField HeaderText="Job Name" HeaderStyle-CssClass="column-250 text-center" ItemStyle-CssClass="column-250">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%#Eval("JobName")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Design Name" HeaderStyle-CssClass="column-250 text-center" ItemStyle-CssClass="column-250">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%#Eval("Design")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Order Date" HeaderStyle-CssClass="column-120 text-center" ItemStyle-CssClass="column-120">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#Eval("CreatedOn")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cust" HeaderStyle-CssClass="column-120 text-center" ItemStyle-CssClass="column-120">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#Eval("Cust")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Customer Name" HeaderStyle-CssClass="column-250 text-center" ItemStyle-CssClass="column-250">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#Eval("Customer")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Supplier" HeaderStyle-CssClass="column-250 text-center" ItemStyle-CssClass="column-250">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#Eval("Supplier")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Steel Base(PCS)" HeaderStyle-CssClass="column-30 text-center" ItemStyle-CssClass="column-30">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#Eval("SteelBase")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Size(Cirf x Width)" HeaderStyle-CssClass="column-100 text-center" ItemStyle-CssClass="column-100">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#Eval("Size")%>'></asp:Label>
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
