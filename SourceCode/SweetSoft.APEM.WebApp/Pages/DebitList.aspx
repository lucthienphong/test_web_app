<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master" AutoEventWireup="true" CodeBehind="DebitList.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.DebitList" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="SweetSoft" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="button-control col-md-12 col-sm-12">
            <div class="form-inline">
                <div class="form-group" style="margin-bottom: 0; width: 100%">
                    <a href="Debit.aspx" id="btnAdd"
                        class="btn btn-transparent new">
                        <span class="flaticon-new10"></span>
                        <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.NEW)%></a>
                    <asp:LinkButton ID="btnDelete" runat="server"
                        class="btn btn-transparent new" OnClick="btnDelete_Click">
                            <span class="flaticon-delete41"></span>Delete
                    </asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <div class="row row_content">
        <div class="col-md-12">
            <div class="row">
                <div class="col-md-3 col-sm-3">
                    <div class="form-group">
                        <label>Debit No:</label>
                        <div>
                            <asp:TextBox ID="txtDebitNo" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-3 col-sm-3">
                    <div class="form-group">
                        <label>Customer:</label>
                        <div>
                            <asp:TextBox ID="txtCustomer" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-2 col-sm-2">
                    <div class="form-group">
                        <label>From date:</label>
                        <div>
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
                <div class="col-md-2 col-sm-2">
                    <div class="form-group">
                        <label>To date:</label>
                        <div>
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
                <div class="col-md-2 col-sm-2">
                    <label class="control-label">&nbsp;</label>
                    <div class="form-group">
                        <div class="col-md-12">
                            <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-primary pull-left"
                                OnClick="btnSearch_Click">
                                        <span class="glyphicon glyphicon-search"></span>&nbsp;
                                        <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.SEARCH)%>
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row" style="padding-top: 15px;">
        <div class="col-md-12">
            <div class="dataTables_wrapper form-inline">
                <SweetSoft:GridviewExtension ID="grvCrebitList" runat="server" AutoGenerateColumns="false"
                    CssClass="grvJobList table table-striped table-bordered table-checkable dataTable" GridLines="None"
                    AllowPaging="true" AllowSorting="true" DataKeyNames="DebitID"
                    OnRowCommand="grvCrebitList_RowCommand"
                    OnPageIndexChanging="grvCrebitList_PageIndexChanging"
                    OnSorting="grvCrebitList_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="DebitNumber" SortExpression="0" HeaderStyle-CssClass="sorting"
                            ItemStyle-CssClass="column-one">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server"
                                    CommandArgument='<%#Eval("DebitID")%>' CommandName="Detail"
                                    Text='<%#Eval("DebitNo")%>' data-id='<%#Eval("DebitID")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-CssClass="column-one" HeaderText="DebitDate"
                            SortExpression="1" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <asp:Label ID="lbDebitDate" runat="server"
                                    Text='<%#Eval("DebitDate")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CustomerCode" SortExpression="2" HeaderStyle-CssClass="sorting"
                            ItemStyle-CssClass="column-one">
                            <ItemTemplate>
                                <asp:Label ID="lbCustomerCode" runat="server"
                                    Text='<%#Eval("Code")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CustomerName" SortExpression="3" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <asp:Label ID="lbCustomerName" runat="server"
                                    Text='<%#Eval("Name")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total" SortExpression="4" HeaderStyle-CssClass="sorting"
                            ItemStyle-CssClass="column-large">
                            <ItemTemplate>
                                <asp:Label ID="lbTotal" runat="server"
                                    Text='<%#Convert.ToDecimal(Eval("Total")).ToString("N3") + " " + Eval("CurrencyName")%>'></asp:Label>
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
                                <div class="btn-group">
                                    <button type="button" class="btn btn-primary dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                                        <div class="fa fa-print"></div>
                                        <span class="caret"></span>
                                    </button>
                                    <ul class="dropdown-menu openPrinting" role="menu" style="right: 0; left: auto">
                                        <li><a href="javascript:;" data-href='Printing/PrintDebitDetail.aspx?ID=<%#Eval("DebitID") %>'>Debit Note</a></li>
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
        })
        addRequestHanlde(SearchText);
        SearchText();
        function SearchText(s, a) {
            if ($("input[type='text'][id$='txtCustomer']").length > 0) {
                $(".ui-autocomplete, .ui-dialog, .ui-helper-hidden-accessible").remove();
                $("input[type='text'][id$='txtCustomer']").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "JobList.aspx/GetCustomerData",
                            data: "{'Keyword':'" + $("input[type='text'][id$='txtCustomer']").val() + "'}",
                            dataType: "json",
                            async: true,
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
                        $("input[type='text'][id$='txtCustomer']").val(ui.item.Name);
                        return false;
                    }
                }).data("ui-autocomplete")._renderItem = function (ul, item) {
                    return $("<li>")
                        .data("ui-autocomplete-item", item)
                        .append("<a><span style='width:30px;'>" + item.Code + '</span> --- ' + item.Name + "</a>")
                        .appendTo(ul);
                };
            }
            else {
                $("input[type='hidden'][id$='hCustomerID']").val("");
            }
        }

        InitCheckAll();
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
            $('.openPrinting a').on('click', function (e) {

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
        }
    </script>
</asp:Content>
