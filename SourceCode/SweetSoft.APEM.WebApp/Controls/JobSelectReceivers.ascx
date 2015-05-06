<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="SweetSoft.APEM.WebApp.Controls.JobSelectReceivers"
     CodeBehind="JobSelectReceivers.ascx.cs" %>
<%@ Register Src="~/Controls/GridViewPager.ascx" TagPrefix="SweetSoft" TagName="GridViewPager" %>

<asp:UpdatePanel ID="upMainSelect" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="form-group">
            <label class="control-label">
                Departments will receive this message:
            </label>
            <div class="chkdept ">
                <div class="all col-md-12" style="border-bottom:1px dashed #000">
                    <input id="chkselnone" runat="server" type="checkbox" name="" /><label for='<%=chkselnone.ClientID %>'>None</label>
                    <input id="chkselall" runat="server" type="checkbox" name="" /><label for='<%=chkselall.ClientID %>'>All department</label>
                </div>
                <asp:CheckBoxList ID="chkDepartment" runat="server" CssClass="col-md-12 item"
                    RepeatLayout="Flow" RepeatColumns="2"></asp:CheckBoxList>
                <br />
                <asp:Button ID="btnSelect" CssClass="btn btn-primary btn-sm hide" style="margin-left:30px" OnClick="btnSelect_Click" runat="server" Text="OK" />
            </div>
        </div>
        <div class="form-group">
            <label class="control-label">
                Staff list:
            </label>
            <div class="">
                <%--<input type="hidden" id="hdfUser" name="hdfUser" runat="server" />--%>

                <SweetSoft:GridviewExtension ID="grvUserList" runat="server" AutoGenerateColumns="false"
                    CssClass="table table-striped table-bordered table-checkable dataTable" GridLines="None"
                    AllowPaging="true" AllowSorting="true"
                    OnPageIndexChanging="grvUserList_PageIndexChanging"
                    OnSorting="grvUserList_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="StaffNo" SortExpression="0" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <a href="User.aspx?id=<%#Eval("StaffID")%>"><%#Eval("StaffNo")%></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="FirstName" SortExpression="1" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <%#Eval("FirstName")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="LastName" SortExpression="2" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <%#Eval("LastName")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Email" SortExpression="3" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <%#Eval("Email")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-CssClass="column-one" HeaderStyle-CssClass="checkbox-column chk">
                            <HeaderTemplate>
                                <input type="checkbox" id="chkStaffAll" runat="server" class="uniform" value="" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input type="checkbox" value='<%#Eval("StaffID")%>'
                                     ID="chkSelect" class="uniform" runat="server" />
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
    </ContentTemplate>
</asp:UpdatePanel>
