<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master" AutoEventWireup="true" CodeBehind="DeliveryOrderList.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.DeliveryOrderList" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="SweetSoft" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        ._minwidth {
            width: 148px!important;
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
                        <div class="form-group" style="margin-bottom: 0">
                            <a href="DeliveryOrder.aspx" id="btnAdd"
                                class="btn btn-transparent new">
                                <span class="flaticon-new10"></span>
                                New</a>

                            <asp:LinkButton ID="btnDelete" runat="server"
                                class="btn btn-transparent new" OnClick="btnDelete_Click">
                                <span class="flaticon-delete41"></span>
                                Delete</asp:LinkButton>
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
                <div class="row">
                    <div class="col-md-3 col-sm-3">
                        <div class="form-group">
                            <label class="control-label">
                                DONumber
                            </label>
                            <SweetSoft:CustomExtraTextbox ID="txtDeliveryOrder" RenderOnlyInput="true" Placeholder="..........."
                                runat="server"></SweetSoft:CustomExtraTextbox>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3">
                        <div class="form-group">
                            <label class="control-label">
                                Job Number
                            </label>
                            <SweetSoft:CustomExtraTextbox ID="txtJobNumber" RenderOnlyInput="true" Placeholder="----/-----"
                                runat="server"></SweetSoft:CustomExtraTextbox>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3">
                        <div class="form-group wrap-datepicker">
                            <label class="control-label">
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.FROM_DATE)%>
                            </label>
                            <SweetSoft:ExtraInputMask ID="txtfromDate" RenderOnlyInput="true"
                                data-format="dd-mm-yyyy"
                                CssClass="form-control mask-date datepicker"
                                runat="server"></SweetSoft:ExtraInputMask>
                            <span class="fa fa-calendar in-mask-date"></span>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3">
                        <div class="form-group wrap-datepicker">
                            <label class="control-label">
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.TO_DATE)%>
                            </label>

                            <SweetSoft:ExtraInputMask ID="txtToDate"
                                data-format="dd-mm-yyyy"
                                CssClass="form-control mask-date datepicker"
                                RenderOnlyInput="true" runat="server"></SweetSoft:ExtraInputMask>
                            <span class="fa fa-calendar in-mask-date"></span>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-sm-1 col-md-1">
                <label class="control-label">&nbsp</label>
                <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-primary btn-block" OnClick="btnLoadContacts_Click">
                    <span class="glyphicon glyphicon-search"></span>
                </asp:LinkButton>
            </div>
        </div>
        <div class="col-md-12">
            <label class="control-label">
                <asp:Label ID="Label1" Text="Delivery Orders" runat="server" />
            </label>
            <div class="dataTables_wrapper form-inline">
                <SweetSoft:GridviewExtension ID="gvDeliveryOrder" runat="server" AutoGenerateColumns="false"
                    CssClass="gvDeliveryOrder table table-striped table-bordered table-checkable dataTable" GridLines="None"
                    AllowPaging="true" AllowSorting="true" DataKeyNames="JobID"
                    OnPageIndexChanging="gvDeliveryOrder_PageIndexChanging"
                    OnSorting="gvDeliveryOrder_Sorting">
                    <Columns>                        
                        <asp:TemplateField HeaderText="Order Number" SortExpression="0" HeaderStyle-CssClass="sorting _minwidth"
                            ItemStyle-CssClass="column-one minWidth">
                            <ItemTemplate>
                                <a href='DeliveryOrder.aspx?ID=<%#Eval("JobID")%>'>
                                    <%#Eval("DONumber")%>
                                </a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Order Date" SortExpression="1" HeaderStyle-CssClass="sorting"
                            ItemStyle-CssClass="column-one maxWitdh">
                            <ItemTemplate>
                                <asp:Label ID="lbOrderDate" Text='<%#Eval("OrderDate")%>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Customer code" SortExpression="2" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <asp:Label ID="lbCode" Text='<%#Eval("Code")%>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Customer name" SortExpression="3" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <asp:Label ID="lbName" Text='<%#Eval("Name")%>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Job number" SortExpression="4" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <asp:Label ID="lbJobNumber" Text='<%#string.Format("{0} (Rev {1})", Eval("JobNumber"), Eval("RevNumber"))%>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Job name" SortExpression="5" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <asp:Label ID="lbJobName" Text='<%#Eval("JobName")%>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Invoice Created" ItemStyle-CssClass="column-one">
                            <ItemTemplate>
                                <span data-toggle="tooltip" data-placement="right" title='<%# Eval("InvoiceStatus").ToString() == "1" ? "Created" : "Not Created Yet"%>' class='<%# Eval("InvoiceStatus").ToString() == "1" ? "glyphicon glyphicon-ok green" : "glyphicon glyphicon-ban-circle red" %>'></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Lock" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                            <ItemTemplate>
                                <span data-toggle="tooltip" data-placement="right"  title='<%#Eval("Lock") == DBNull.Value ? "" : (((bool)Eval("Lock")) == false ?  "Unlock" : "Lock")%>' class='<%#Eval("Lock") == DBNull.Value ? "" : (((bool)Eval("Lock")) == false ?  "flaticon-padlock21 blue" : "flaticon-padlock19 red")%> '></span>
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
                        <asp:TemplateField HeaderStyle-CssClass="maxWitdh" ItemStyle-CssClass="column-one maxWitdh">
                            <ItemTemplate>
                                <div class="btn-group">
                                    <button type="button" class="btn btn-primary btn-block dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                                        <span class="fa fa-print"></span>&nbsp;<span class="caret"></span>
                                    </button>
                                    <ul class="dropdown-menu openPrinting" role="menu" style="right: 0; left: auto">
                                        <li>
                                            <a href="#" data-href='Printing/PrintDeliveryOrderPDF.aspx?ID=<%#Eval("JobID") %>'>Delivery order</a>
                                        </li>
                                        <li>
                                            <a href="#" data-href='Printing/PrintDeliveryOrderNotePDF.aspx?ID=<%#Eval("JobID") %>'>Delivery note</a>
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
                        There are currently no items in this table.
                    </EmptyDataTemplate>
                </SweetSoft:GridviewExtension>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ModalPlaceHolder" runat="server">
    <div id="dialog-printing" title="Printing" style="background: #f8f8f8">
        <iframe src="" frameborder="0" width="100%" height="100%" style="min-height: 500px"></iframe>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptPlaceHolder" runat="server">
    <script src="/js/plugins/printThis.js"></script>
    <script>
        $(document).ready(function () {
            $('#dialog-printing').hide();
            SearchText();
            InitCheckAll();
            InitDialogPrintLink();
            InitMaskAPE();
        })

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SearchText);
        function SearchText(s, a) {

            $("input[type='text'][id$='txtName']").focus(function () { $(this).select(); });

            var btn = document.getElementById('<%= btnSearch.ClientID %>')
            if ($("input[type='text'][id$='txtName']").length > 0) {
                $(".ui-autocomplete,.ui-dialog, .ui-helper-hidden-accessible").remove();
                $("input[type='text'][id$='txtName']").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "PurchaseOrderList.aspx/GetCustomerData",
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
                        //document.getElementById('<%= btnSearch.ClientID %>').click();
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
            $('.gvDeliveryOrder .openPrinting a').each(function () {
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

        addRequestHanlde(InitMaskAPE);
        function InitMaskAPE() {
            $('.maskAPE').mask('APEXXXXXXXX', {
                'translation': {
                    A: { pattern: /[a,A]/ },
                    P: { pattern: /[p,P]/ },
                    E: { pattern: /[e,E]/ },
                    X: { pattern: /[0-9]/ }
                }
            });

            $('.maskJobNumber').mask('XXXX/XXXXX', {
                'translation': {
                    X: { pattern: /[0-9]/ }
                }
            });
        }

        $(function () {
            $('[data-toggle="tooltip"]').bstooltip()
        })
    </script>
</asp:Content>