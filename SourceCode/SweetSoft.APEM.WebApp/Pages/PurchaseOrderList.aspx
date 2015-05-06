<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master" AutoEventWireup="true" CodeBehind="PurchaseOrderList.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.PurchaseOrderList" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="SweetSoft" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .minWidth
        {
            width: 319px !important;
            text-align: left !important;
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
                            <a href="PurchaseOrder.aspx" id="btnAdd"
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
            <div class="col-md-1 col-sm-1 hidden">
                <div class="form-group">
                    <label class="control-label">
                        <strong><%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CUSTOMER)%></strong>
                    </label>
                    <SweetSoft:ExtraInputMask ID="txtCode" RenderOnlyInput="true" runat="server" CssClass="text-search"
                        MaskString="AAA"></SweetSoft:ExtraInputMask>
                </div>
            </div>
            <div class="col-md-3 col-sm-3">
                <div class="form-group">
                    <label class="control-label">
                        Customer name
                    </label>
                    <SweetSoft:CustomExtraTextbox ID="txtName" RenderOnlyInput="true" Placeholder="Customer's name" CssClass="text-search"
                        runat="server" AutoCompleteType="Search"></SweetSoft:CustomExtraTextbox>
                </div>
            </div>
            <div class="col-md-8 col-sm-8">
                <div class="row">
                    <div class="col-md-3 col-sm-3">
                        <div class="form-group">
                            <label class="control-label">
                                Purchase Order Number
                            </label>
                            <SweetSoft:CustomExtraTextbox ID="txtPurchaseOrder"  CssClass="text-search" RenderOnlyInput="true" Placeholder="APE........."
                                runat="server"></SweetSoft:CustomExtraTextbox>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3">
                        <div class="form-group">
                            <label class="control-label">
                                Job Number
                            </label>
                            <SweetSoft:CustomExtraTextbox ID="txtJobNumber" RenderOnlyInput="true" Placeholder="----/-----"
                                CssClass="text-search"
                                runat="server"></SweetSoft:CustomExtraTextbox>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3">
                        <div class="form-group wrap-datepicker">
                            <label class="control-label">
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.FROM_DATE)%>
                            </label>
                            <SweetSoft:ExtraInputMask ID="txtFromDate" RenderOnlyInput="true"
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
            <div class="col-md-1 col-sm-1">
                <label class="control-label">
                    &nbsp;
                </label>
                <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-primary btn-block"
                    OnClick="btnSearch_Click">
                    <span class="glyphicon glyphicon-search"></span>
                </asp:LinkButton>
            </div>
        </div>
        <div class="col-md-12">
            <label class="control-label">
                <asp:Label Text="Purchase Orders" runat="server" />
            </label>
            <div class="dataTables_wrapper form-inline">
                <SweetSoft:GridviewExtension ID="gvPurchaseOrder" runat="server" AutoGenerateColumns="false"
                    CssClass="gvPurchaseOrder table table-striped table-bordered table-checkable dataTable" GridLines="None"
                    AllowPaging="true" AllowSorting="true" DataKeyNames="PurchaseOrderID"
                    OnPageIndexChanging="gvPurchaseOrder_PageIndexChanging"
                    OnSorting="gvPurchaseOrder_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="Order No." SortExpression="0" HeaderStyle-CssClass="sorting"
                            ItemStyle-CssClass="column-one maxWidth">
                            <ItemTemplate>
                                <a href='PurchaseOrder.aspx?ID=<%#Eval("PurchaseOrderID")%>'>
                                    <%#Eval("OrderNumber")%>
                                </a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Order Date" SortExpression="1" HeaderStyle-CssClass="sorting"
                            ItemStyle-CssClass="column-one maxWitdh">
                            <ItemTemplate>
                                <asp:Label ID="lbOrderDate" Text='<%#Eval("OrderDate")%>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delivery Date" SortExpression="2" HeaderStyle-CssClass="sorting" 
                            ItemStyle-CssClass="column-one maxWitdh">
                            <ItemTemplate>
                                <asp:Label ID="lbRequiredDeliveryDate" runat="server" Text='<%#Eval("RequiredDeliveryDate")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CustomerCode" SortExpression="3" HeaderStyle-CssClass="sorting"
                            ItemStyle-CssClass="column-one maxWitdh">
                            <ItemTemplate>
                                <asp:Label ID="lblCustomerCode" runat="server" Text='<%#Eval("Code")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CustomerName" SortExpression="4" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <asp:Label ID="lblCustomerName" runat="server" Text='<%#Eval("Name")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Job No." SortExpression="5" HeaderStyle-CssClass="sorting" 
                            ItemStyle-CssClass="column-one maxWitdh">
                            <ItemTemplate>
                                <asp:Label ID="lblJobNumber" runat="server" Text='<%# Eval("JobNumber") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="JobName" SortExpression="6" HeaderStyle-CssClass="sorting" 
                            ItemStyle-CssClass="maxWitdh">
                            <ItemTemplate>
                                <asp:Label ID="lblJobName" runat="server" Text='<%# Eval("JobName") %>'></asp:Label>
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
                                <div class="openPrinting" role="menu" style="right: 0; left: auto">
                                    <a href="#" class="btn btn-primary" data-href='Printing/PrintPurchaseOrder.aspx?ID=<%#Eval("PurchaseOrderID") %>'>
                                        <span class="fa fa-print"></span>
                                    </a>
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
    <div id="dialog-printing" title="Printing" style="background: #fff">
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
            
            if ($("input[type='text'][id$='txtName']").length > 0) {
                $(".ui-autocomplete, .ui-helper-hidden-accessible").remove();
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
            $('.gvPurchaseOrder .openPrinting a').each(function () {
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
                                text: "Print",
                                "class": 'btn btn-primary',
                                click: function () {
                                    var content = $(this).find('iframe');
                                    var clonePrint = content.contents().find("html");
                                    var script = clonePrint.find('script').remove();
                                    $(clonePrint).printThis();

                                }
                            }
                            ,
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
            $('.maskAPE').mask('APEXX6XXXXX', {
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
    </script>
</asp:Content>

