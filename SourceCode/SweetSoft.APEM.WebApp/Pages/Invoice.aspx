<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master"
    AutoEventWireup="true" CodeBehind="Invoice.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.Invoice" %>

<%@ Register Src="~/Controls/JobInvoiceControl.ascx" TagName="JobInvoiceControl" TagPrefix="SweetSoftCMS" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .widthPriceGridView
        {
            width: 160px;
        }

        .center
        {
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="button-control col-md-12 col-sm-12">
            <div class="form-inline">
                <div class="form-group" style="margin-bottom: 0">
                    <asp:LinkButton runat="server" ID="btnSave" 
                        OnClick="btnSave_Click"
                        CssClass="btn btn-transparent new"><span class="flaticon-new10"></span> Save</asp:LinkButton>

                    <asp:LinkButton ID="btnDelete" runat="server"
                        class="btn btn-transparent new" OnClick="btnDelete_Click">
                                <span class="flaticon-delete41"></span>
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.DELETE)%></asp:LinkButton>
                </div>
                <asp:UpdatePanel ID="upExport" runat="server" RenderMode="Inline">
                    <ContentTemplate>
                        <div class='btn-group'>
                            <button type='button' class='btn btn-transparent dropdown-toggle' data-toggle='dropdown' aria-expanded='false'>
                                <span class='flaticon-xlsx'></span>&nbsp;Export SAP File&nbsp;<span class='caret'></span>
                            </button>
                            <ul class='dropdown-menu' role='menu'>
                                <li>
                                    <asp:LinkButton runat="server" ID="btnExcel" OnClick="btnExcel_Click">
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
                <asp:UpdatePanel runat="server" ID="upnPrinting" RenderMode="Inline" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Literal ID="ltrPrint" runat="server" EnableViewState="false"></asp:Literal>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <%--Trunglc Add - 22-04-2015--%>
                <asp:LinkButton ID="btnLock" runat="server" Visible="false"
                    OnClick="btnLock_Click" CssClass="btn btn-transparent">
                                <span class="flaticon-padlock19"></span>
                            Lock
                </asp:LinkButton>
                <asp:LinkButton ID="btnUnlock" runat="server" Visible="false"
                    OnClick="btnUnlock_Click" CssClass="btn btn-transparent">
                                <span class="flaticon-padlock21"></span>
                            Unlock
                </asp:LinkButton>
                <asp:Literal ID="ltrView" runat="server" EnableViewState="false"></asp:Literal>
                <%--End--%>
                <asp:LinkButton ID="btnCancel" runat="server" class="btn btn-transparent new" OnClick="btnCancel_Click">
                    <span class="flaticon-back57"></span>
                     <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.RETURN)%>
                </asp:LinkButton>
            </div>
        </div>
    </div>
    <div class="row row_content">
        <div class="col-md-12 sweet-input-mask">
            <div class="row">
                <div class="col-md-2 col-sm-2">
                    <div class="form-group">
                        <label class="control-label">
                            <strong><%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.INVOICE_NUMBER)%></strong>
                        </label>
                        <asp:TextBox runat="server" ID="txtInvoiceNumber" ReadOnly="true" CssClass="form-control" ToolTip="Invoice Number"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-1 col-sm-1">
                    <div class="form-group">
                        <label class="control-label">
                            <strong><%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CUSTOMER)%></strong>
                        </label>
                        <SweetSoft:ExtraInputMask ID="txtCode" RenderOnlyInput="true" Required="true" ToolTip="Customer Code"
                            runat="server" Repeat="5" ShowMaskOnHover="true" MaxLength="5" Enabled="false"
                            Greedy="true" RightAlign="false"></SweetSoft:ExtraInputMask>
                    </div>
                </div>
                <div class="col-md-3 col-sm-3">
                    <div class="form-group">
                        <label class="control-label">
                            &nbsp;
                        </label>
                        <SweetSoft:CustomExtraTextbox ID="txtName" RenderOnlyInput="true" Placeholder="Customer's name"
                            runat="server" AutoCompleteType="Search" ToolTip="Customer Name"
                            RequiredText='<%# SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.THIS_FIELD_IS_REQUIRED)%>'></SweetSoft:CustomExtraTextbox>

                        <asp:HiddenField ID="hCustomerID" runat="server" />
                        <asp:LinkButton ID="btnLoadContacts" runat="server" OnClick="btnLoadContacts_Click" Style="display: none;"></asp:LinkButton>
                    </div>
                </div>
                <div class="col-sm-3 col-md-3">
                    <div class="row">
                        <div class="form-group">
                            <div class="col-md-12 col-sm-12">
                                <label class="control-label">
                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CONTACT_PERSON)%>
                                </label>

                                <asp:DropDownList ID="ddlContact" runat="server" AutoPostBack="false"
                                    data-style="btn btn-info" ToolTip="Contact Person"
                                    data-width="100%"
                                    data-toggle="dropdown"
                                    CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4 col-sm-4">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-3 col-sm-3">
                    <div class="form-group wrap-datepicker">
                        <label class="control-label">
                            <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.INVOICE_DATE)%>
                        </label>
                        <SweetSoft:ExtraInputMask ID="txtInvoiceDate" RenderOnlyInput="true"
                            data-format="dd-mm-yyyy" ToolTip="Invoice Date"
                            CssClass="form-control mask-date a"
                            runat="server"></SweetSoft:ExtraInputMask>
                        <span class="fa fa-calendar in-mask-date"></span>
                    </div>
                </div>
            </div>

            <div class="row" runat="server" id="divJobNumber">
                <div class="col-md-12">
                    <asp:UpdatePanel runat="server" ID="upnlJobRev" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-sm-3 col-md-3">
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div class="form-group" style="margin-bottom: 0">
                                                <label class="control-label">
                                                    Delivery Order
                                                </label>
                                                <asp:DropDownList runat="server" ID="ddlDeliveryOrder"
                                                    data-style="btn btn-info" ToolTip="Delivery Order"
                                                    data-width="100%" AutoPostBack="true"
                                                    data-toggle="dropdown" OnSelectedIndexChanged="ddlDeliveryOrder_SelectedIndexChanged"
                                                    CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3 col-sm-3">
                                    <div class="form-group">
                                        <label class="control-label">
                                            <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.JOB_NAME)%>
                                        </label>
                                        <asp:TextBox runat="server" CssClass="form-control" ID="txtJobName" ReadOnly="true" ToolTip="Job Name"/>
                                    </div>
                                </div>
                                <div class="col-md-3 col-sm-3">
                                    <div class="form-group">
                                        <label class="control-label">
                                            <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.DESIGN)%>
                                        </label>
                                        <asp:TextBox runat="server" CssClass="form-control" ID="txtDesign" ReadOnly="true" ToolTip="Design"/>
                                    </div>
                                </div>
                                <div class="col-md-2 col-sm-2">
                                    <div class="form-group">
                                        <label class="control-label">
                                            Job number
                                        </label>
                                        <asp:TextBox runat="server" CssClass="form-control" ID="txtJobNumber" ReadOnly="true" ToolTip="Job Number"/>
                                    </div>
                                </div>
                                <div class="col-md-1 col-sm-1">
                                    <div class="form-group">
                                        <label class="control-label col-md-12">
                                            &nbsp;
                                        </label>
                                        <asp:LinkButton ID="btnAddJob" runat="server" OnClick="btnAddJob_Click"
                                            CssClass="btn btn-primary"><i class="glyphicon glyphicon-plus"></i>
                                             <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.ADD)%>
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 col-sm-12">
            <div class="form-group">
                <asp:UpdatePanel runat="server" ID="upnlJobInvoice" UpdateMode="Always">
                    <ContentTemplate>
                        <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
                            <asp:Repeater runat="server" ID="rpnlJobInvoice" OnItemDataBound="rpnlJobInvoice_ItemDataBound">
                                <ItemTemplate>
                                    <SweetSoftCMS:JobInvoiceControl runat="server" ID="JobInvoice1" />
                                </ItemTemplate>
                            </asp:Repeater>

                            <asp:HiddenField runat="server" ID="hdfJobIdRemove" />
                            <asp:Button runat="server" ID="btnRemoveJob" Style="display: none" OnClick="btnRemoveJob_Click" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <div class="row">

        <div class="col-sm-8">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="control-label col-sm-2">
                        <span class="pull-left"></span>
                        <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CURRENCY)%>
                    </label>
                    <div class="col-sm-4">
                        <div class="input-group">
                            <SweetSoft:ExtraInputMask ID="txtCurrencyValue" RenderOnlyInput="true" Enabled="false" Required="false" ToolTip="Currency Value"
                                runat="server" MaskType="Decimal" GroupSeparator="," RadixPoint="." Text="1.0000" Digits="4" AutoGroup="true"></SweetSoft:ExtraInputMask>
                            <span class="input-group-addon">
                                <asp:Literal ID="ltrCurrency" runat="server"></asp:Literal></span>
                            <span class="input-group-addon">=</span>
                            <SweetSoft:ExtraInputMask ID="txtRMValue" RenderOnlyInput="true" Required="false" ToolTip="RM Value"
                                runat="server" MaskType="Decimal" GroupSeparator="," RadixPoint="." Text="1" Digits="4" AutoGroup="true"></SweetSoft:ExtraInputMask>
                            <span class="input-group-addon">RM
                            </span>
                            <asp:HiddenField ID="hCurrencyID" runat="server" />
                        </div>
                    </div>
                    <label class="control-label col-sm-2">
                        <span class="pull-left"></span>
                        <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.TAX)%>
                    </label>
                    <div class="col-sm-4">
                        <asp:DropDownList ID="ddlTax" runat="server" ToolTip="Tax"
                            data-style="btn btn-info" Enabled="false"
                            data-width="100%" Width="100%"
                            data-toggle="dropdown"
                            CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="form-group">
                    <label class="control-label col-sm-2"><%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.REMARK)%></label>
                    <div class="col-sm-10">
                        <asp:TextBox TextMode="MultiLine" ID="txtRemark" runat="server" Rows="2" CssClass="form-control" ToolTip="Remark"></asp:TextBox>
                    </div>

                    <%--<label class="control-label col-sm-2">
                        <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.REMARK_SCREEN)%>
                    </label>
                    <div class="col-sm-4">
                        <asp:TextBox TextMode="MultiLine" ID="txtRemarkScreen" runat="server" Rows="2" CssClass="form-control"></asp:TextBox>
                    </div>--%>
                </div>

                <div class="form-group">
                    <label class="control-label col-sm-2">
                        <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.DELIVERY_TERMS)%>
                    </label>
                    <div class="col-sm-10">
                        <asp:TextBox runat="server" ID="txtYourReference" class="form-control" ToolTip="Delivery Terms"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group">
                    <label class="control-label col-sm-2">
                        <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.PAYMENT_TERMS)%>
                    </label>
                    <div class="col-sm-10">
                        <asp:TextBox runat="server" ID="txtPaymentTerms" class="form-control" ToolTip="Payment Terms"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-sm-4">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="control-label col-sm-6">
                        Sub total before GTS
                    </label>
                    <div class="col-sm-6">
                        <SweetSoft:ExtraInputMask ID="txtTotalPrice" RenderOnlyInput="true" Required="false" ToolTip="Sub Total Before GTS"
                            runat="server" MaskType="Decimal" GroupSeparator="," RadixPoint="." Text="0" Digits="3" AutoGroup="true"></SweetSoft:ExtraInputMask>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-6">
                        Final amount
                    </label>
                    <div class="col-sm-6">
                        <SweetSoft:ExtraInputMask ID="txtNetTotal" RenderOnlyInput="true" Required="false" ToolTip="Final Amount"
                            runat="server" MaskType="Decimal" GroupSeparator="," RadixPoint="." Text="0" Digits="3" AutoGroup="true"></SweetSoft:ExtraInputMask>
                    </div>
                </div>
            </div>
        </div>

        <asp:HiddenField runat="server" ID="hdfJobIDTemp" />
        <asp:HiddenField runat="server" ID="hdfJobID" />
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
            $('#dialog-printing').hide();
            InitDialogPrintLink();
            DatePicker();
            SearchText();
        })

        function SaveClick() {
            $("button[type='submit'][id$='btnSubmit']").click();
        }

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SearchText);
        function SearchText(s, a) {
            if ($("input[type='text'][id$='txtName']").length > 0) {
                $(".ui-autocomplete, .ui-helper-hidden-accessible").remove();
                $("input[type='text'][id$='txtName']").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "Invoice.aspx/GetCustomerData",
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
                        $("input[type='hidden'][id$='hCustomerID']").val(ui.item.ID);
                        document.getElementById('<%= btnLoadContacts.ClientID %>').click();
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
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(DatePicker);

        function DatePicker() {
            var nowTemp = new Date();
            var now = new Date(nowTemp.getFullYear(), nowTemp.getMonth(), nowTemp.getDate(), 0, 0, 0, 0);

            var oDate = $('.a').datepicker({
                format: "dd/mm/yyyy"
            });

            var dDate = $('.b').datepicker({
                format: "dd/mm/yyyy",
            });

            oDate.on('changeDate', function (e) {
                //console.log(oDate.datepicker('getUTCDate'))
                //console.log(oDate.datepicker('getDates'))
                //console.log(oDate.datepicker('getUTCDates'))
                var date = new Date(oDate.datepicker('getDate'));
                var _date = new Date(date.getFullYear(), date.getMonth(), date.getDate() + 1, 0, 0, 0, 0);
                console.log(_date)

                dDate.datepicker('remove').datepicker({
                    format: "dd/mm/yyyy",
                    startDate: _date
                });
                dDate.datepicker('update', new Date(date.getFullYear(), date.getMonth(), date.getDate() + 1, 0, 0, 0, 0));
            })
        }

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitDialogPrintLink);
        function InitDialogPrintLink() {
            $('.openPrinting a').each(function () {
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

        function DoRemoveJob(id) {
            console.log(id);
            $("input[type='hidden'][id$='hdfJobIdRemove']").val(id);
            $("input[type='submit'][id$='btnRemoveJob']").click();
        }
    </script>
</asp:Content>
