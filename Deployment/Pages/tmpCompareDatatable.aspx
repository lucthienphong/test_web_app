<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="tmpCompareDatatable.aspx.cs" MasterPageFile="~/MasterPages/ModalMasterPage.Master" Inherits="SweetSoft.APEM.WebApp.Pages.tmpCompareDatatable" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="SweetSoft" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p><strong>Data before edit</strong></p>
    <div class="col-sm-12 col-md-12">
        <div class="table-responsive" id="grvOld" runat="server">
            
        </div>
    </div>
    <p><strong>Data after edit</strong></p>
    <div class="col-sm-12 col-md-12">
        <div class="table-responsive" id="grvNew" runat="server">
            
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptPlaceHolder" runat="server">
    <script type="text/javascript">
        var subCatContainer = $(".table-responsive");

        subCatContainer.scroll(function () {
            subCatContainer.scrollLeft($(this).scrollLeft());
            subCatContainer.scrollTop($(this).scrollTop());
        });

    </script>
</asp:Content>
