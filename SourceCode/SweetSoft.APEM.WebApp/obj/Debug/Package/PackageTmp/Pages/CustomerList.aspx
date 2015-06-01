<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master" AutoEventWireup="true" CodeBehind="CustomerList.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.CustomerList" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="SweetSoft" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .column-one {
            max-width: 40px;
            width: auto!important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="button-control col-md-12 col-sm-12 affix">
            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="form-horizontal">
                        <a id="btnAdd" href="Customer.aspx"
                            class="btn btn-transparent new">
                            <span class="flaticon-new10"></span>
                            New</a>
                        <asp:LinkButton ID="btnDelete" runat="server"
                            class="btn btn-transparent new"
                            OnClick="btnDelete_Click">
                                <span class="flaticon-delete41"></span>
                                Delete</asp:LinkButton>
                        <asp:UpdatePanel ID="upExport" runat="server" RenderMode="Inline">
                            <ContentTemplate>
                                <asp:LinkButton ID="btnExcel" runat="server" class="btn btn-transparent" OnClick="btnExcel_Click"> 
                                <span class="flaticon-xlsx"></span>
                                Excel</asp:LinkButton>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnExcel" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row row_content">
        <div class="col-md-12">
            <div class="form-group">
                <div class="row">
                    <div class="col-md-6 col-md-offset-3 col-sm-12">
                        <div class="form-horizontal">
                            <div class="form-group" style="margin-bottom: 0">
                                <div class="col-sm-8 col-sm-7 col-xs-7">
                                    <div class="form-group">
                                        <asp:TextBox ID="txtKeyword" runat="server" placeholder="Enter some keyword..." class="form-control"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <asp:DropDownList ID="ddlStatus" runat="server"
                                            data-style="btn btn-info btn-block"
                                            data-width="100%"
                                            data-toggle="dropdown"
                                            CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md4 col-sm-5 col-xs-5">
                                    <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-primary" OnClick="btnSearch_Click">
                                            <span class="glyphicon glyphicon-search"></span>&nbsp;
                                            <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.SEARCH)%>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row" style="padding-top: 15px;">
        <div class="col-md-12">
            <div class="dataTables_wrapper form-inline">
                <SweetSoft:GridviewExtension ID="grvCustomerList" runat="server" AutoGenerateColumns="false"
                    CssClass="table table-striped table-bordered table-checkable dataTable" GridLines="None"
                    AllowPaging="true" AllowSorting="true" DataKeyNames="CustomerID"
                    OnRowCommand="grvCustomerList_RowCommand"
                    OnPageIndexChanging="grvCustomerList_PageIndexChanging"
                    OnSorting="grvCustomerList_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="CustomerCode" SortExpression="0" HeaderStyle-CssClass="sorting"
                            ItemStyle-CssClass="column-one">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server"
                                    CommandArgument='<%#Eval("CustomerID")%>' CommandName="Detail"
                                    Text='<%#Eval("Code")%>' data-id='<%#Eval("CustomerID")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name" SortExpression="1" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <asp:Label ID="lbName" runat="server"
                                    Text='<%#Eval("Name")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Address" SortExpression="2" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <asp:Label ID="lbAddress" runat="server"
                                    Text='<%#Eval("Address")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tel" SortExpression="3" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <asp:Label ID="lbTel" runat="server"
                                    Text='<%#Eval("Tel")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fax" SortExpression="4" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <asp:Label ID="lbFax" runat="server"
                                    Text='<%#Eval("Fax")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText="Obsolete" SortExpression="5" HeaderStyle-CssClass="sorting"
                            ItemStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsObsoleteView" CssClass="uniform"
                                    Checked='<%#Eval("IsObsolete")%>' Enabled="false" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Pricing master" ItemStyle-CssClass="column-one  none-underline">
                            <HeaderStyle CssClass="text-center" />
                            <ItemTemplate>
                                <asp:LinkButton ID="btnPriceQuotation" runat="server"
                                    CommandArgument='<%#Eval("CustomerID")%>' CommandName="Quotation"
                                    data-id='<%#Eval("CustomerID")%>'>
                                    <span style="text-decoration:none;" class="flaticon-medical50"></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-CssClass="column-one" HeaderStyle-CssClass="checkbox-column">
                            <HeaderStyle CssClass="column-one text-center" />
                            <ItemStyle CssClass="column-one" />
                            <HeaderTemplate>
                                <input type="checkbox" id="chkSelectAll" class="uniform" value="" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsDelete" CssClass="uniform"
                                    runat="server"></asp:CheckBox>
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
                        <SweetSoft:GridViewPager runat="server" />
                    </PagerTemplate>
                    <EmptyDataTemplate>
                        There are currently no items in this table.
                    </EmptyDataTemplate>
                </SweetSoft:GridviewExtension>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptPlaceHolder" runat="server">
    <script>
        addRequestHanlde(InitCheckAll);
        //addRequestHanlde(InitDetail);


        InitCheckAll();
        //InitDetail();

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

        //function InitDetail() {
        //    var linkColl = $('div[id$="grvCustomerList"] a[id$="btnEdit"]');
        //    if (linkColl.length > 0) {
        //        linkColl.click(function () {
        //            parent.openWindow($('a[data-title]:eq(0)'), 'Customer', '/Pages/Customer.aspx?ID=' + $(this).attr('data-id'));
        //            return false;
        //        });
        //    }

        //    var linkColl = $('div[id$="grvCustomerList"] a[id$="btnPriceQuotation"]');
        //    if (linkColl.length > 0) {
        //        linkColl.click(function () {
        //            parent.openWindow($('a[data-title]:eq(0)'), 'Quotation', '/Pages/CustomerQuotation.aspx?ID=' + $(this).attr('data-id'));
        //            return false;
        //        });
        //    }
        //}

        //$('button[id="btnAdd"]').click(function () {
        //    parent.openWindow($('a[data-title]:eq(0)'), 'Customer', '/Pages/Customer.aspx');
        //    return false;
        //});
    </script>
</asp:Content>
