<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CurrencyChangedLog.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.CurrencyChangedLog" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="SweetSoft" %>
<!DOCTYPE html>
<link href="/css/normalize.min.css" rel="stylesheet" />
<link href="/css/bootstrap.min.css" rel="stylesheet" />
<link href="/css/sweetstyle.min.css" rel="stylesheet" />
<link href="/css/datepicker.min.css" rel="stylesheet" />
<link href="/css/uniform.default.css" rel="stylesheet" />
<link href="/css/bootstrap-select.min.css" rel="stylesheet" />
<link href="/css/table.css" rel="stylesheet" />
<link href="/css/customui.css" rel="stylesheet" />
<script src="/js/core/jquery.min.js"></script>

<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>
        <div class="container-fluid">
            <div class="row hidden">
                <div class="col-md-12">
                    <h3>
                        <asp:Label Text="lblCurrencyName" ID="lblCurrencyName" runat="server" /></h3>
                    <hr />
                </div>
            </div>
            <div class="form-horizontal" style="margin-top:20px">
                <div class="form-group">
                    <label class="control-label col-xs-1" style="margin-top: 5px;">Date</label>
                    <div class="col-xs-5">
                        <div class="input-group">
                            <asp:TextBox runat="server" ID="txtSearchDate" CssClass="form-control mask-date datepicker" />
                            <span class="fa fa-calendar in-mask-date" style="right: 50px; z-index: 9;"></span>
                            <span class="input-group-btn">
                                <asp:LinkButton runat="server" CssClass="btn btn-primary" ID="btnSearch" onclick="btnSearch_Click">
                                    <span class="glyphicon glyphicon-search"></span>
                                </asp:LinkButton>
                            </span>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <SweetSoft:GridviewExtension ID="gvCurrency" runat="server" AutoGenerateColumns="false"
                                CssClass="gvCurrency table table-striped table-bordered table-checkable dataTable" GridLines="None"
                                AllowPaging="true" AllowSorting="true" DataKeyNames="CurrencyID"
                                OnPageIndexChanging="gvCurrency_PageIndexChanging"
                                OnSorting="gvCurrency_Sorting">
                        <AlternatingRowStyle CssClass="info" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Date changed" SortExpression="0" HeaderStyle-CssClass="sorting">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" Text='<%#Eval("DateChanged") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Currency name" SortExpression="1" HeaderStyle-CssClass="sorting">
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" Text='<%#Eval("NewName") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="New value" SortExpression="2" HeaderStyle-CssClass="sorting">
                                        <ItemTemplate>
                                            <asp:Label ID="Label3" Text='<%#Eval("NewValue") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="RM Value" SortExpression="3" HeaderStyle-CssClass="sorting">
                                        <ItemTemplate>
                                            <asp:Label ID="Label4" Text='<%#Eval("NewRMValue") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Changed by" SortExpression="4" HeaderStyle-CssClass="sorting">
                                        <ItemTemplate>
                                            <asp:Label ID="Label5" Text='<%#Eval("DisplayName") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status" SortExpression="5" HeaderStyle-CssClass="sorting">
                                        <ItemTemplate>
                                            <asp:Label ID="Label6" Text='<%#Eval("Status") %>' runat="server" />
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


            <script src="/js/core/bootstrap.min.js"></script>
        <script src="/js/plugins/jquery-ui/jquery-ui.min.js"></script>
        <!------------------>
        <script src="/js/plugins/migrate.js"></script>
        <!--bootstrap plugins-->
        <script src="/js/plugins/bootstrap-plugins/bootstrap-select.min.js"></script>
        <!--autocomplete-->
        <script src="/js/plugins/bootstrap-plugins/bootstrap-typeahead.min.js"></script>
        <!--Checkbox - radio-->
        <script src="/js/plugins/uniform/jquery.uniform.js"></script>
        <!--input mask-->
        <script src="/js/plugins/mask/jquery.mask.js"></script>
        <!--datepicker-->
        <script src="/js/plugins/bootstrap-plugins/bootstrap-datepicker.js"></script>
        <!--dialog-->
        <script src="/js/plugins/bootstrap-plugins/bootstrap-dialog.min.js"></script>
        <!--datatable, chi dung khi co table va sort-->
        <script src="/js/plugins/datatables/jquery.dataTables.js"></script>
        <script src="/js/plugins/datatables/TableTools/js/TableTools.js"></script>
        <script src="/js/plugins/datatables/dataTables.bootstrap.js"></script>
        <script src="/js/plugins/datatables/jquery.dataTables.columnFilter.js"></script>
        <script src="/js/plugins/datatables/TableTools/js/ZeroClipboard.js"></script>
        <!--Message box-->
        <script src="/js/messagebox.js"></script>
        <!--validation-->
        <script src="/js/validationEngine/jquery.validationEngine.js" type="text/javascript"></script>
        <script src="/js/validationEngine/jquery.validationEngine-vi.js" type="text/javascript"></script>
        <script src="/js/validationEngine/ValidationScript.js" type="text/javascript"></script>
        <%--<!--script chỉ chạy demo-->
            <script src="/demo_javascript/demo.js"></script>
            <script src="/demo_javascript/dataTables.js"></script>--%>
            <!------------------!>
        <!--run script-->
        <script src="/js/main.js"></script>
        <script src="/js/UIScript.js"></script>
        <script>
            $(document).ready(function () {
                SelectStyle();
            });

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SelectStyle);
            function SelectStyle() {
                //$('select').selectpicker();
            }
        </script>
    </form>


</body>
</html>
