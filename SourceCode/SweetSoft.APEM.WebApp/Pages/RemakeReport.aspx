<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master" AutoEventWireup="true" CodeBehind="RemakeReport.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.RemakeReport" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="SweetSoft" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row" style="margin-top: 20px;">
        <div class="col-sm-8 col-sm-offset-2">
            <div class="row">
                <div class="col-md-4 col-sm-4">
                    <label class="control-label">Remake date:</label>
                    <div class="row">
                        <div class="col-xs-2">
                            <div class="form-group">
                                <p class="form-control-static">From</p>
                            </div>
                        </div>
                        <div class="col-xs-10">
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
                </div>
                <div class="col-md-4 col-sm-4">
                    <label class="control-label">&nbsp;</label>
                    <div class="row">
                        <div class="col-xs-2">
                            <div class="form-group">
                                <p class="form-control-static">To</p>
                            </div>
                        </div>
                        <div class="col-xs-10">
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
                <div class="col-md-4 col-sm-4">
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
                                <asp:LinkButton ID="btnExport" runat="server" CssClass="btn btn-primary"
                                    OnClick="btnExport_Click">
                                    <span class="glyphicon glyphicon-save"></span>&nbsp;
                                Download
                                </asp:LinkButton>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnExport" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row" style="margin-top: 15px;">
        <div class="col-md-12">
            <div class="dataTables_wrapper form-inline">
                <SweetSoft:GridviewExtension ID="grvRemakeReport" runat="server" AutoGenerateColumns="false"
                    CssClass="table table-striped table-bordered table-checkable dataTable" GridLines="None"
                    AllowPaging="false" AllowSorting="false" DataKeyNames=""
                    OnRowCommand="grvRemakeReport_RowCommand"
                    OnPageIndexChanging="grvRemakeReport_PageIndexChanging"
                    OnSorting="grvRemakeReport_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="No" SortExpression="0">
                            <ItemTemplate>
                                <asp:Label ID="lbRowNumber" runat="server" Text='<%#Eval("RowNumber")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date" SortExpression="1">
                            <ItemTemplate>
                                <asp:Label ID="lbDate" runat="server"
                                    Text='<%#Eval("Date")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="S" SortExpression="1">
                            <ItemTemplate>
                                <asp:Label ID="lbSale" runat="server"
                                    Text='<%#Eval("S")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="C" SortExpression="1">
                            <ItemTemplate>
                                <asp:Label ID="lbCoordinator" runat="server"
                                    Text='<%#Eval("C")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="JobNumber" SortExpression="2">
                            <ItemTemplate>
                                <asp:Label ID="lbJobNumber" runat="server"
                                    Text='<%#Eval("JobNumber")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cust" SortExpression="2">
                            <ItemTemplate>
                                <asp:Label ID="lbCode" runat="server"
                                    Text='<%#Eval("Code")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="JobName" SortExpression="3">
                            <ItemTemplate>
                                <asp:Label ID="lbJobName" runat="server"
                                    Text='<%#Eval("JobName")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Design" SortExpression="4">
                            <ItemTemplate>
                                <asp:Label ID="lbDesign" runat="server"
                                    Text='<%#Eval("Design")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="I/E" SortExpression="5">
                            <ItemTemplate>
                                <asp:Label ID="lbIE" runat="server"
                                    Text='<%#Eval("IE")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="S" SortExpression="5">
                            <ItemTemplate>
                                <asp:Label ID="lbCByS" runat="server"
                                    Text='<%#Eval("CByS")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="R" SortExpression="6">
                            <ItemTemplate>
                                <asp:Label ID="lbCByR" runat="server"
                                    Text='<%#Eval("CByR")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="P" SortExpression="7">
                            <ItemTemplate>
                                <asp:Label ID="lbCByP" runat="server"
                                    Text='<%#Eval("CByP")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="O" SortExpression="8">
                            <ItemTemplate>
                                <asp:Label ID="lbCByO" runat="server"
                                    Text='<%#Eval("CByO")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Revision Details" SortExpression="9">
                            <ItemTemplate>
                                <asp:Label ID="lbRevisionDetail" runat="server"
                                    Text='<%#Eval("RevisionDetail")%>'></asp:Label>
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
</asp:Content>
