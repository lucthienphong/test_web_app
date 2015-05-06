<%@ Control Language="C#" CodeBehind="GridViewPager.ascx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.GridViewPager" %>
<div class="form-inline" style="margin:10px 0 10px 0;">
    <span class="form-inline">
        <asp:Label ID="LabelRows" runat="server" Text="Show: " AssociatedControlID="DropDownListPageSize" />
        <asp:DropDownList ID="DropDownListPageSize"
            runat="server" AutoPostBack="true"
            data-style="form-control btn-info btn-sm"
            data-width="65"
            data-toggle="dropdown"
            OnSelectedIndexChanged="DropDownListPageSize_SelectedIndexChanged">
            <asp:ListItem Value="10" />
            <asp:ListItem Value="25" />
            <asp:ListItem Value="50" />
            <asp:ListItem Value="100" />
            <asp:ListItem Value="200" />
        </asp:DropDownList>
        <asp:Label ID="lbTotalRow" runat="server" Text=""/>
    </span>
    <ul class="pagination pagination-sm pull-right" style="margin:0;">
        <li id="liFirst" runat="server">
            <asp:LinkButton ToolTip="First page" ID="btnFirst"
                runat="server" CommandName="Page" CommandArgument="First"
                Style="text-decoration: none;">
                    <span class="glyphicon glyphicon-fast-backward"></span>
            </asp:LinkButton>
        </li>
        <li id="liPrev" runat="server">
            <asp:LinkButton ToolTip="Previous page" ID="btnPrev" runat="server"
                CommandName="Page" CommandArgument="Prev"
                Style="text-decoration: none;">
                    <span class="glyphicon glyphicon-backward"></span>
            </asp:LinkButton>
        </li>
        <li id="li1" runat="server">
            <asp:LinkButton ID="btn1" runat="server"
                CommandName="Page" CommandArgument="1"
                Style="text-decoration: none;">
            </asp:LinkButton>
        </li>
        <li id="li2" runat="server">
            <asp:LinkButton ID="btn2" runat="server"
                CommandName="Page" CommandArgument="2"
                Style="text-decoration: none;">
            </asp:LinkButton>
        </li>
        <li id="li3" runat="server">
            <asp:LinkButton ID="btn3" runat="server"
                CommandName="Page" CommandArgument="3"
                Style="text-decoration: none;">
            </asp:LinkButton>
        </li>
        <li id="li4" runat="server">
            <asp:LinkButton ID="btn4" runat="server"
                CommandName="Page" CommandArgument="4"
                Style="text-decoration: none;">
            </asp:LinkButton>
        </li>
        <li id="li5" runat="server">
            <asp:LinkButton ID="btn5" runat="server"
                CommandName="Page" CommandArgument="5"
                Style="text-decoration: none;">
            </asp:LinkButton>
        </li>
        <li id="liNext" runat="server">
            <asp:LinkButton ToolTip="Next page" ID="btnNext" runat="server"
                CommandName="Page" CommandArgument="Next"
                Style="text-decoration: none;">
                    <span class="glyphicon glyphicon-forward"></span>
            </asp:LinkButton>
        </li>
        <li id="liLast" runat="server">
            <asp:LinkButton ToolTip="Last page" ID="btnLast" runat="server"
                CommandName="Page" CommandArgument="Last"
                Style="text-decoration: none;">
                    <span class="glyphicon glyphicon-fast-forward"></span>
            </asp:LinkButton>
        </li>
    </ul>
</div>

