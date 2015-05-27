<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master"
    AutoEventWireup="true" CodeBehind="InvoiceList.aspx.cs"
    Inherits="SweetSoft.APEM.WebApp.Pages.InvoiceList" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="SweetSoft" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .minWidth {
            width: 419px !important;
            text-align: left !important;
        }

        /*[class^="flaticon-"]:before,
        [class*=" flaticon-"]:before
        {
            margin-left: 10px !important;
        }*/

        .flaticon-padlock19:before, 
        .flaticon-padlock21:before
        {
            font-size: 16px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="button-control col-md-12 col-sm-12 affix">
            <div class="row">
                <div class="col-md-6 col-sm-6">
                    <div class="form-inline">
                        <div class="form-group">
                            <a href="Invoice.aspx" id="btnAdd"
                                class="btn btn-transparent new">
                                <span class="flaticon-new10"></span>
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.NEW)%></a>
                            <asp:UpdatePanel ID="upExport" runat="server" RenderMode="Inline">
                                <ContentTemplate>
                                    <div class='btn-group'>
                                        <button type='button' class='btn btn-transparent dropdown-toggle' data-toggle='dropdown' aria-expanded='false'>
                                            <span class='flaticon-xlsx'></span>&nbsp;Export SAP File&nbsp;<span class='caret'></span>
                                        </button>
                                        <ul class='dropdown-menu openPrinting' role='menu'>
                                            <li>
                                                <asp:LinkButton ID="btnExcel" runat="server" OnClick="btnExcel_Click">
                                                    Excel
                                                </asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="btnExportHead" runat="server" OnClick="btnExportHead_Click">
                                                    Head File
                                                </asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="tblExportPotisions" runat="server" OnClick="tblExportPotisions_Click">
                                                    Position File
                                                </asp:LinkButton>
                                            </li>
                                        </ul>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnExcel" />
                                    <asp:PostBackTrigger ControlID="btnExportHead" />
                                    <asp:PostBackTrigger ControlID="tblExportPotisions" />
                                </Triggers>
                            </asp:UpdatePanel>
                            <asp:LinkButton ID="btnDelete" runat="server"
                                class="btn btn-transparent new" OnClick="btnDelete_Click">
                                <span class="flaticon-delete41"></span>
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.DELETE)%></asp:LinkButton>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>
    <div class="row row_content">
        <div class="form-group" style="overflow: hidden">
            <div class="col-md-3 col-sm-3">
                <div class="form-group">
                    <label class="control-label">
                        <strong><%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CUSTOMER)%></strong>
                    </label>
                    <SweetSoft:CustomExtraTextbox ID="txtName" RenderOnlyInput="true" Placeholder="Customer's name"
                        runat="server" AutoCompleteType="Search"></SweetSoft:CustomExtraTextbox>
                </div>
            </div>
            <div class="col-md-8 col-sm-8">
                <div class="col-md-3 col-sm-3">
                    <div class="form-group">
                        <label class="control-label">
                            <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.INVOICE_NUMBER)%>
                        </label>
                        <asp:TextBox runat="server" ID="txtOrderNumber" CssClass="form-control" Placeholder=".........."></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-3 col-sm-3">
                    <div class="form-group">
                        <label class="control-label">
                            <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.JOB_NUMBER)%>
                        </label>
                        <asp:TextBox runat="server" ID="txtJobName" CssClass="form-control" Placeholder="----/-----"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-3 col-sm-3">
                    <div class="form-group wrap-datepicker">
                        <label class="control-label">
                            <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.FROM_DATE)%>
                        </label>
                        <SweetSoft:ExtraInputMask ID="txtFromDate" RenderOnlyInput="true"
                            data-format="dd-mm-yyyy"
                            CssClass="form-control mask-date a datepicker"
                            runat="server"></SweetSoft:ExtraInputMask>
                        <span class="glyphicon glyphicon-calendar in-mask-date"></span>
                    </div>
                </div>
                <div class="col-md-3 col-sm-3">
                    <div class="form-group wrap-datepicker">
                        <label class="control-label">
                            <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.TO_DATE)%>
                        </label>

                        <SweetSoft:ExtraInputMask ID="txtToDate" RenderOnlyInput="true"
                            data-format="dd-mm-yyyy"
                            CssClass="form-control mask-date a datepicker"
                            runat="server"></SweetSoft:ExtraInputMask>
                        <span class="glyphicon glyphicon-calendar in-mask-date"></span>
                    </div>
                </div>
            </div>
            <div class="col-md-1 col-sm-1">
                <label class="control-label">&nbsp</label>
                <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-primary btn-block" OnClick="btnSearch_Click">
                            <span class="glyphicon glyphicon-search"></span>
                </asp:LinkButton>
            </div>
        </div>

        <div class="col-md-12">
            <label class="control-label">
                <asp:Label Text="Invoice" runat="server" />
            </label>
            <div class="dataTables_wrapper form-inline">
                <SweetSoft:GridviewExtension ID="grvInvoiceList" runat="server" AutoGenerateColumns="false"
                    CssClass="grvInvoiceList table table-striped table-bordered table-checkable dataTable" GridLines="None"
                    AllowPaging="true" AllowSorting="true" DataKeyNames="InvoiceID"
                    OnPageIndexChanging="grvInvoiceList_PageIndexChanging"
                    OnSorting="grvInvoiceList_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="Invoice No" SortExpression="0" HeaderStyle-CssClass="sorting"
                            ItemStyle-CssClass="column-one maxWitdh">
                            <ItemTemplate>
                                <a href='Invoice.aspx?ID=<%#Eval("InvoiceID")%>'>
                                    <%#Eval("InvoiceNo")%>
                                </a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Invoice date" SortExpression="1" HeaderStyle-CssClass="sorting"
                            ItemStyle-CssClass="column-one maxWitdh">
                            <ItemTemplate>
                                <asp:Label ID="lblInvoiceDate" Text='<%#Eval("InvoiceDate")%>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CustomerCode" SortExpression="2" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <asp:Label ID="lblCustomerCode" runat="server" Text='<%#Eval("Code")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CustomerName" SortExpression="3" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <asp:Label ID="lblCustomerName" runat="server" Text='<%#Eval("Name")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="JobNumber" SortExpression="4" HeaderStyle-CssClass="sorting" ItemStyle-CssClass="maxWitdh">
                            <ItemTemplate>
                                <asp:Label ID="lblJobNumber" runat="server" Text='<%# Eval("JobNumber") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="JobName" SortExpression="5" HeaderStyle-CssClass="sorting" ItemStyle-CssClass="maxWitdh">
                            <ItemTemplate>
                                <asp:Label ID="lblJobName" runat="server" Text='<%# Eval("JobName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Lock" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                            <ItemTemplate>
                                <span data-toggle="tooltip" data-placement="right" title='<%#Eval("Lock") == DBNull.Value ? "" : (((bool)Eval("Lock")) == false ?  "Unlock" : "Lock")%>' class='<%#Eval("Lock") == DBNull.Value ? "" : (((bool)Eval("Lock")) == false ?  "flaticon-padlock21 blue" : "flaticon-padlock19 red")%>'></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-CssClass="column-one" HeaderStyle-CssClass="checkbox-column">
                            <HeaderTemplate>
                                <input type="checkbox" id="chkSelectAll" class="uniform" value="" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsDelete" CssClass="uniform"
                                    runat="server"></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-CssClass="column-one">
                            <ItemTemplate>
                                <%--<div class="openPrinting" role="menu" style="right: 0; left: auto">
                                    <a href="javascript:;" class="btn btn-primary" data-href='Printing/PrintDetailInvoice.aspx?ID=<%#Eval("InvoiceID") %>'>
                                        <span class="fa fa-print"></span>
                                    </a>
                                </div>--%>
                                <div class="btn-group">
                                    <button type="button" class="btn btn-primary btn-block dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                                        <span class="fa fa-print"></span>&nbsp;<span class="caret"></span>
                                    </button>
                                    <ul class="dropdown-menu openPrinting" role="menu" style="right: 0; left: auto">
                                        <%--<li>
                                            <a href="#" data-href='Printing/PrintDetailInvoice.aspx?ID=<%#Eval("InvoiceID") %>'>Detail Invoice</a>
                                        </li>
                                        <li>
                                            <a href="#" data-href='Printing/PrintInvoice.aspx?ID=<%#Eval("InvoiceID") %>'>Combine Invoice</a>
                                        </li>--%>
                                        <li>
                                            <a href="#" data-href='Printing/PrintTaxInvoicePDF.aspx?ID=<%#Eval("InvoiceID") %>'>Tax Invoice</a>
                                        </li>                                        
                                       <%-- <li>
                                            <a href="#" data-href='Printing/PrintTaxInvoiceByCustomer.aspx?ID=<%#Eval("InvoiceID") %>'>Customer Invoice</a>
                                        </li>--%>
                                        <li>
                                            <a href="#" data-href='Printing/PrintTaxInvoiceServicePDF.aspx?ID=<%#Eval("InvoiceID") %>'>Service Job Invoice</a>
                                        </li>
                                    </ul>
                                </div>
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
                        <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.THERE_ARE_CURRENTLY_NO_ITEMS_IN_THIS_TABLE)%>
                    </EmptyDataTemplate>
                </SweetSoft:GridviewExtension>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ModalPlaceHolder" runat="server">
    <div id="dialog-printing" title="Printing" style="background: #fff">
        <iframe src="" frameborder="0" width="100%" height="100%" style="min-height: 500px"></iframe>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptPlaceHolder" runat="server">
    <script src="/js/plugins/printThis.js"></script>
    <script>
        $(document).ready(function () {
            //DatePicker();
            $('#dialog-printing').hide();
            InitDialogPrintLink();
            SearchText();
            InitCheckAll();
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
                            url: "InvoiceList.aspx/GetCustomerData",
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
                        $("input[type='text'][id$='txtCode']").val(ui.item.Code);
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
        addRequestHanlde(InitCheckAll);
        function InitCheckAll() {
            $("#chkSelectAll").change(function () {
                var isChecked = $(this).is(':checked');
                var checkboxother = $(this).closest('table').find('tbody tr td input[type="checkbox"]:not(:disabled)');
                if (isChecked) {
                    checkboxother.prop('checked', true).trigger('change');
                }
                else
                    checkboxother.each(function () {
                        $(this).prop('checked', false).trigger('change');
                    });
            });
        }

        addRequestHanlde(InitDialogPrintLink);
        function InitDialogPrintLink() {
            $('.grvInvoiceList .openPrinting a').each(function () {
                $(this).on('click', function (e) {

                    var hrefLink = $(this).data("href");
                    var iframe = $("#dialog-printing").find('iframe');

                    iframe.attr("src", hrefLink);

                    $("#dialog-printing").dialog({
                        autoOpen: false,
                        height: 'auto',
                        width: '850',
                        modal: true,
                        appendTo: "form",
                        resizable: false,
                        buttons: [
                            {
                                text: "Close",
                                Class: 'btn btn-default',
                                click: function () {
                                    iframe.attr("src", "");
                                    $("#dialog-printing").dialog("close");

                                }
                            }
                        ]
                    });
                    $("#dialog-printing").show();
                    $("#dialog-printing").dialog("open");
                    return false;
                });
            })
        }

        $(function () {
            $('[data-toggle="tooltip"]').bstooltip()
        })


    </script>
</asp:Content>
