<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master"
    AutoEventWireup="true" CodeBehind="OrderConfirmationList.aspx.cs"
    Inherits="SweetSoft.APEM.WebApp.Pages.OrderConfirmationList" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="SweetSoft" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .minWidth
        {
            width: 419px !important;
            text-align: left !important;
        }

        .column-one
        {
            width: auto !important;
            max-width: 40px !important;
        }

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
                            <a href="OrderConfirmation.aspx" id="btnAdd"
                                class="btn btn-transparent new">
                                <span class="flaticon-new10"></span>
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.NEW)%></a>

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
                <div class="row">
                    <div class="col-md-3 col-sm-3">
                        <div class="form-group">
                            <label class="control-label">
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.ORDER_NUMBER)%>
                            </label>
                            <asp:TextBox runat="server" ID="txtOrderNumber" Placeholder=".........." CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3">
                        <div class="form-group">
                            <label class="control-label">
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.JOB_NUMBER)%>
                            </label>
                            <asp:TextBox runat="server" ID="txtJobNameOrNumber" Placeholder="----/-----" CssClass="form-control"></asp:TextBox>
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
                            <span class="glyphicon glyphicon-calendar in-mask-date"></span>
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
                            <span class="glyphicon glyphicon-calendar in-mask-date"></span>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-1 col-sm-1">
                <div class="form-group">
                    <label class="control-label">
                        &nbsp;
                    </label>
                    <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-primary btn-block" OnClick="btnSearch_Click">
                                            <span class="glyphicon glyphicon-search"></span>
                    </asp:LinkButton>
                    <asp:LinkButton ID="btnViewAll" runat="server" Visible="false" CssClass="btn btn-block btn-primary" OnClick="btnViewAll_Click">
                        <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.VIEW_ALL)%>
                    </asp:LinkButton>
                </div>
            </div>
        </div>
        <div class="col-md-12">
            <label class="control-label">
                <asp:Label Text="Confirm order" runat="server" />
            </label>
            <div class="dataTables_wrapper form-inline">
                <SweetSoft:GridviewExtension ID="gvOrderConfiList" runat="server" AutoGenerateColumns="false"
                    CssClass="gvOrderConfiList table table-striped table-bordered table-checkable dataTable" GridLines="None"
                    AllowPaging="true" AllowSorting="true" DataKeyNames="JobID"
                    OnPageIndexChanging="gvOrderConfiList_PageIndexChanging"
                    OnSorting="gvOrderConfiList_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="OCNumber" SortExpression="0" HeaderStyle-CssClass="sorting"
                            ItemStyle-CssClass="column-one minWidth">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server"
                                    CommandArgument='<%#Eval("JobID")%>' 
                                    Text='<%#Eval("OCNumber")%>' data-id='<%#Eval("JobID")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Order Date" SortExpression="1" HeaderStyle-CssClass="sorting"
                            ItemStyle-CssClass="column-one maxWitdh">
                            <ItemTemplate>
                                <asp:Label ID="lblOrderDate" Text='<%#Eval("OrderDate")%>' runat="server" />
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
                                <asp:Label ID="lbJobNumber" Text='<%#string.Format("{0} (Rev {1})", Eval("JobNumber"), Eval("RevNumber"))%>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="JobName" SortExpression="5" HeaderStyle-CssClass="sorting" ItemStyle-CssClass="maxWitdh">
                            <ItemTemplate>
                                <asp:Label ID="lblJobName" runat="server" Text='<%# Eval("JobName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Lock" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                            <ItemTemplate>
                                <span data-toggle="tooltip" data-placement="right" title='<%#Eval("Lock") == DBNull.Value ? "" : (((bool)Eval("Lock")) == false ?  "Unlock" : "Lock")%>' class='<%#Eval("Lock") == DBNull.Value ? "" : (((bool)Eval("Lock")) == false ?  "flaticon-padlock21 blue" : "flaticon-padlock19 red")%> '></span>
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
                        <asp:TemplateField>
                            <ItemTemplate>
                                <div class="btn-group">
                                    <button type="button" class="btn btn-primary dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                                        <div class="fa fa-print"></div>
                                        <span class="caret"></span>
                                    </button>
                                    <ul class="dropdown-menu openPrinting" role="menu" style="right: 0; left: auto">
                                        <li><a href="javascript:;" data-href='Printing/PrintOrderConfirmation.aspx?ID=<%#Eval("JobID") %>'>OC - Normal Job</a></li>
                                        <li><a href="javascript:;" data-href='Printing/PrintAdditionalJobServices.aspx?ID=<%#Eval("JobID") %>'>OC - Service Job</a></li>
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


            $("input[type='text'][id$='txtOrderDate']").datepicker({
                format: "dd/mm/yyyy"
            }).on('changeDate', function (e) {
                $("input[type='text'][id$='txtOrderDate']").datepicker('hide')
                btn.click();
            })

            $("input[type='text'][id$='txtBaseDeliveryDate']").datepicker({
                format: "dd/mm/yyyy"
            }).on('changeDate', function (e) {
                $("input[type='text'][id$='txtBaseDeliveryDate']").datepicker('hide');
                btn.click();
            })
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
            $('.gvOrderConfiList .openPrinting a').each(function () {
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

        $(function () {
            $('[data-toggle="tooltip"]').bstooltip()
        })

        addRequestHanlde(InitDetail);
        InitDetail();
        function InitDetail() {
            var linkColl = $('div[id$="gvOrderConfiList"] a[id$="btnEdit"]');
            if (linkColl.length > 0) {
                linkColl.click(function () {
                    parent.openWindow($('a[data-title]:eq(0)'), 'Order Confirmation', '/Pages/OrderConfirmation.aspx?ID=' + $(this).attr('data-id'));
                    return false;
                });
            }
        }

    </script>
</asp:Content>
