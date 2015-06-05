<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master" AutoEventWireup="true" CodeBehind="CustomerQuotation.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.CustomerQuotation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="button-control col-md-12 col-sm-12 affix">
            <div class="row">
                <div class="col-md-6 col-sm-6">
                    <div class="form-inline">
                        <div class="form-group">
                            <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click" class="btn btn-transparent">
                                <span class="flaticon-floppy1"></span>
                                 <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.SAVE)%></asp:LinkButton>
                        </div>
                        <div class="form-group">
                            <asp:UpdatePanel ID="upnPrinting" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                <ContentTemplate>
                                    <asp:Literal ID="ltrPrint" runat="server" EnableViewState="false"></asp:Literal>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="form-group">
                            <asp:LinkButton ID="btnCancel" runat="server" OnClick="btnCancel_Click" class="btn btn-transparent">
                                <span class="flaticon-back57"></span>
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.RETURN)%>
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row row_content">
        <div class="col-sm-12">
            <div class="row">
                <div class="col-sm-12">
                    <div class="form-group">
                        <label class="control-label">
                            Customer name
                        </label>
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="form-horizontal">
                                    <div class="form-group" style="margin-bottom: 0">
                                        <div class="col-sm-2">
                                            <asp:TextBox ID="txtCode" runat="server" ReadOnly="true"
                                                CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="txtName" runat="server" ReadOnly="true"
                                                CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-12">
                    <div class="form-group">
                        <div class="row">
                            <div class="form-group">
                                <div class="col-sm-2">
                                    <div class="wrap-datepicker">
                                        <label class="control-label">Last Update Date</label>
                                        <SweetSoft:CustomExtraTextbox ID="txtQuotationDate" runat="server"
                                            RenderOnlyInput="true" data-format="dd-MM-yyyy"
                                            CssClass="datepicker form-control mask-date"></SweetSoft:CustomExtraTextbox>
                                        <span class="fa fa-calendar in-mask-date"></span>
                                    </div>
                                </div>
                                <div class="col-sm-4">
                                    <label class="control-label">Customer Contact</label>
                                    <asp:DropDownList ID="ddlContacts" runat="server"
                                        data-style="btn btn-info"
                                        data-width="100%"
                                        data-live-search="true"
                                        data-toggle="dropdown"
                                        CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-sm-4">
                                    <label class="control-label">
                                        Contact Designation
                                    </label>
                                    <SweetSoft:CustomExtraTextbox ID="txtContactDesignation" runat="server"
                                        RenderOnlyInput="true" CssClass="form-control"></SweetSoft:CustomExtraTextbox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-sm-12">
            <div class="form-group">
                <label class="control-label DTTT_button_save">
                    Pricing master
                </label>
                <div id="tablesContacts" class="dataTables_wrapper form-inline no-footer">
                    <div class="dt_header">
                        <div class="row">
                            <div class="col-md-12 col-sm-12">
                                <div class="DTTT btn-group pull-right">
                                    <asp:LinkButton ID="btnAddDetail" runat="server"
                                        CssClass="btn btn-default btn-sm DTTT_button_add"
                                        title="Add new item" OnClick="btnAddDetail_Click">
                                                        <span><i class="icol-add"></i>Add</span>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:GridView ID="grvPrices" runat="server" AutoGenerateColumns="false"
                        CssClass="table table-striped table-bordered table-checkable dataTable" GridLines="None"
                        AllowPaging="false" AllowSorting="false" DataKeyNames="ID"
                        OnRowDeleting="grvPrices_RowDeleting"
                        OnRowEditing="grvPrices_RowEditing"
                        OnRowUpdating="grvPrices_RowUpdating"
                        OnRowCancelingEdit="grvPrices_RowCancelingEdit"
                        OnRowDataBound="grvPrices_RowDataBound"
                        OnRowCommand="grvPrices_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="Pricing Name" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" Text='<%# Eval("PricingName") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div style="position: relative;">
                                        <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtPricingName"
                                            Text='<%#Eval("PricingName")%>' Width="100%"
                                            CssClass="form-control" runat="server">
                                        </SweetSoft:CustomExtraTextbox>
                                    </div>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Product Type" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lbProductTypeName" runat="server" Text='<%# Eval("ProductTypeName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div style="position: relative;">
                                        <asp:DropDownList ID="ddlProductType" runat="server"
                                            data-style="btn btn-info" data-width="100%" Required="true"
                                            data-toggle="dropdown" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Process Type" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lbProcessTypeName" runat="server" Text='<%# Eval("ProcessTypeName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div style="position: relative;">
                                        <asp:DropDownList ID="ddlProcessType" runat="server"
                                            data-style="btn btn-info" data-width="100%" Required="true"
                                            data-toggle="dropdown" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="GLCode">
                                <ItemTemplate>
                                    <asp:Label ID="lbPricingName" runat="server" Text='<%# Eval("GLCode") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div style="position: relative;">
                                        <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtGLCode"
                                            Text='<%#Eval("GLCode")%>' Width="100%"
                                            CssClass="form-control" runat="server">
                                        </SweetSoft:CustomExtraTextbox>
                                    </div>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description">
                                <ItemTemplate>
                                    <asp:Label ID="lbDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div style="position: relative;">
                                        <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtDescription"
                                            Text='<%#Eval("Description")%>' Width="100%"
                                            CssClass="form-control" runat="server">
                                        </SweetSoft:CustomExtraTextbox>
                                    </div>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="OLD" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lbOldSteelBase" runat="server" Text='<%# Convert.ToDecimal(Eval("OldSteelBase")).ToString("N3") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div class="col-md-12">
                                        <SweetSoft:ExtraInputMask ID="txtOldSteelBase" runat="server" Style="width: 100%;"
                                            RenderOnlyInput="true" Required="true"
                                            RequiredText="Require field"
                                            MaskType="Decimal" GroupSeparator="," RadixPoint="." AutoGroup="true" Digits="3"
                                            Text='<%# Convert.ToDecimal(Eval("OldSteelBase")).ToString("N3") %>'></SweetSoft:ExtraInputMask>
                                    </div>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="NEW" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lbNewSteelBase" runat="server" Text='<%# Convert.ToDecimal(Eval("NewSteelBase")).ToString("N3") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div class="col-md-12">
                                        <SweetSoft:ExtraInputMask ID="txtNewSteelBase" runat="server" Style="width: 100%;"
                                            RenderOnlyInput="true" Required="true"
                                            RequiredText="Require field"
                                            MaskType="Decimal" GroupSeparator="," RadixPoint="." AutoGroup="true" Digits="3"
                                            Text='<%# Convert.ToDecimal(Eval("NewSteelBase")).ToString("N3") %>'></SweetSoft:ExtraInputMask>
                                    </div>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CURR">
                                <ItemTemplate>
                                    <asp:Label ID="lbCurrencyName" runat="server" Text='<%# Eval("CurrencyName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div style="position: relative;">
                                        <asp:DropDownList ID="ddlCurrency" runat="server"
                                            data-style="btn btn-info" data-width="100%" Required="true"
                                            data-toggle="dropdown" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Unit">
                                <ItemTemplate>
                                    <asp:Label ID="lbUnit" runat="server" Text='<%# Eval("UnitOfMeasure") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div style="position: relative;">
                                        <asp:DropDownList ID="ddlUnit" runat="server"
                                            data-style="btn btn-info" data-width="100%" Required="true"
                                            data-toggle="dropdown" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-CssClass="column-one">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnDelete"
                                        runat="server" CssClass="btn btn-primary" CommandName="delete">
                                    <span class="glyphicon glyphicon-remove"></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div class="btn-group">
                                        <asp:LinkButton ID="UpdateButton"
                                            runat="server"
                                            CssClass="btn btn-primary"
                                            CommandName="Update"
                                            Text="Update" Font-Underline="false">
                                        <span class="fa fa-check"></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="Cancel"
                                            runat="server"
                                            CssClass="btn btn-danger"
                                            CommandName="Cancel"
                                            Text="Cancel">
                                        <span class="fa fa-ban"></span>
                                        </asp:LinkButton>
                                    </div>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
        <div class="col-sm-12">
            <div class="form-group">
                <label class="control-label DTTT_button_save">
                    Additional Services
                </label>
                <div id="divAdditionService" class="dataTables_wrapper form-inline no-footer">
                    <div class="dt_header">
                        <div class="row">
                            <div class="col-md-12 col-sm-12">
                                <div class="DTTT btn-group pull-right">
                                    <asp:LinkButton ID="tblAddService" runat="server"
                                        CssClass="btn btn-default btn-sm DTTT_button_add"
                                        title="Add new item" OnClick="tblAddService_Click">
                                        <span><i class="icol-add"></i>Add</span>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:GridView ID="grvAdditionalService" runat="server" AutoGenerateColumns="false"
                        CssClass="table table-striped table-bordered table-checkable dataTable" GridLines="None"
                        AllowPaging="false" AllowSorting="false" DataKeyNames="ID"
                        OnRowDeleting="grvAdditionalService_RowDeleting"
                        OnRowEditing="grvAdditionalService_RowEditing"
                        OnRowUpdating="grvAdditionalService_RowUpdating"
                        OnRowCancelingEdit="grvAdditionalService_RowCancelingEdit"
                        OnRowDataBound="grvAdditionalService_RowDataBound"
                        OnRowCommand="grvAdditionalService_RowCommand">
                        <Columns>                        
                            <asp:TemplateField HeaderText="Additional Services">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" Text='<%# Eval("Description") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div style="position: relative;">
                                        <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtDescription"
                                            Text='<%#Eval("Description")%>' Width="100%"
                                            CssClass="form-control" runat="server">
                                        </SweetSoft:CustomExtraTextbox>
                                    </div>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Category" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lbCategory" runat="server" Text='<%# Eval("CategoryName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div style="position: relative;">
                                        <asp:DropDownList ID="ddlCategory" runat="server"
                                            data-style="btn btn-info" data-width="100%" Required="true"
                                            data-toggle="dropdown" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="GLCode">
                                <ItemTemplate>
                                    <asp:Label ID="lbPricingName" runat="server" Text='<%# Eval("GLCode") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div style="position: relative;">
                                        <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtGLCode"
                                            Text='<%#Eval("GLCode")%>' Width="100%"
                                            CssClass="form-control" runat="server">
                                        </SweetSoft:CustomExtraTextbox>
                                    </div>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Prices" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lbValue" runat="server" Text='<%# Convert.ToDecimal(Eval("Price")).ToString("N3") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div class="col-md-12">
                                        <SweetSoft:ExtraInputMask ID="txtPrice" runat="server" Style="width: 100%;"
                                            RenderOnlyInput="true" Required="true"
                                            RequiredText="Require field"
                                            MaskType="Decimal" GroupSeparator="," RadixPoint="." AutoGroup="true" Digits="3"
                                            Text='<%# Convert.ToDecimal(Eval("Price")).ToString("N3") %>'></SweetSoft:ExtraInputMask>
                                    </div>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CURR">
                                <ItemTemplate>
                                    <asp:Label ID="lbCurrencyName" runat="server" Text='<%# Eval("CurrencyName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div style="position: relative;">
                                        <asp:DropDownList ID="ddlCurrency" runat="server"
                                            data-style="btn btn-info" data-width="100%" Required="true"
                                            data-toggle="dropdown" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-CssClass="column-one">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnDelete"
                                        runat="server" CssClass="btn btn-primary" CommandName="delete">
                                    <span class="glyphicon glyphicon-remove"></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div class="btn-group">
                                        <asp:LinkButton ID="UpdateButton"
                                            runat="server"
                                            CssClass="btn btn-primary"
                                            CommandName="Update"
                                            Text="Update" Font-Underline="false">
                                        <span class="fa fa-check"></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="Cancel"
                                            runat="server"
                                            CssClass="btn btn-danger"
                                            CommandName="Cancel"
                                            Text="Cancel">
                                        <span class="fa fa-ban"></span>
                                        </asp:LinkButton>
                                    </div>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
        <div class="col-sm-12">
            <div class="form-group">
                <label class="control-label DTTT_button_save">
                    Other charges
                </label>
                <div id="div1" class="dataTables_wrapper form-inline no-footer">
                    <div class="dt_header">
                        <div class="row">
                            <div class="col-md-12 col-sm-12">
                                <div class="DTTT btn-group pull-right">
                                    <asp:LinkButton ID="btnOtherCharges" runat="server"
                                        CssClass="btn btn-default btn-sm DTTT_button_add"
                                        title="Add new item" OnClick="btnOtherCharges_Click">
                                        <span><i class="icol-add"></i>Add</span>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:GridView ID="grvOtherCharges" runat="server" AutoGenerateColumns="false"
                        CssClass="table table-striped table-bordered table-checkable dataTable" GridLines="None"
                        AllowPaging="false" AllowSorting="false" DataKeyNames="ID"
                        OnRowDeleting="grvOtherCharges_RowDeleting"
                        OnRowEditing="grvOtherCharges_RowEditing"
                        OnRowUpdating="grvOtherCharges_RowUpdating"
                        OnRowCancelingEdit="grvOtherCharges_RowCancelingEdit"
                        OnRowDataBound="grvOtherCharges_RowDataBound"
                        OnRowCommand="grvOtherCharges_RowCommand">
                        <Columns>                        
                            <asp:TemplateField HeaderText="Other charges">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" Text='<%# Eval("Description") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div style="position: relative;">
                                        <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtDescription"
                                            Text='<%#Eval("Description")%>' Width="100%"
                                            CssClass="form-control" runat="server">
                                        </SweetSoft:CustomExtraTextbox>
                                    </div>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="GLCode">
                                <ItemTemplate>
                                    <asp:Label ID="lbPricingName" runat="server" Text='<%# Eval("GLCode") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div style="position: relative;">
                                        <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtGLCode"
                                            Text='<%#Eval("GLCode")%>' Width="100%"
                                            CssClass="form-control" runat="server">
                                        </SweetSoft:CustomExtraTextbox>
                                    </div>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Prices" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lbValue" runat="server" Text='<%# Convert.ToDecimal(Eval("Price")).ToString("N3") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div class="col-md-12">
                                        <SweetSoft:ExtraInputMask ID="txtPrice" runat="server" Style="width: 100%;"
                                            RenderOnlyInput="true" Required="true"
                                            RequiredText="Require field"
                                            MaskType="Decimal" GroupSeparator="," RadixPoint="." AutoGroup="true" Digits="3"
                                            Text='<%# Convert.ToDecimal(Eval("Price")).ToString("N3") %>'></SweetSoft:ExtraInputMask>
                                    </div>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CURR">
                                <ItemTemplate>
                                    <asp:Label ID="lbCurrencyName" runat="server" Text='<%# Eval("CurrencyName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div style="position: relative;">
                                        <asp:DropDownList ID="ddlCurrency" runat="server"
                                            data-style="btn btn-info" data-width="100%" Required="true"
                                            data-toggle="dropdown" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-CssClass="column-one">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnDelete"
                                        runat="server" CssClass="btn btn-primary" CommandName="delete">
                                    <span class="glyphicon glyphicon-remove"></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div class="btn-group">
                                        <asp:LinkButton ID="UpdateButton"
                                            runat="server"
                                            CssClass="btn btn-primary"
                                            CommandName="Update"
                                            Text="Update" Font-Underline="false">
                                        <span class="fa fa-check"></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="Cancel"
                                            runat="server"
                                            CssClass="btn btn-danger"
                                            CommandName="Cancel"
                                            Text="Cancel">
                                        <span class="fa fa-ban"></span>
                                        </asp:LinkButton>
                                    </div>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
        <div class="col-sm-12">
            <div class="form-group">
                <label class="control-label">Remark</label>
                <asp:TextBox ID="txtQuotationNote" runat="server"
                    TextMode="MultiLine" Rows="6" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptPlaceHolder" runat="server">
    <script src="/js/plugins/printThis.js"></script>
    <script>
        $(document).ready(function () {
            $('#dialog-printing').hide();
            InitDialogPrintLink();
        })
        addRequestHanlde(InitDialogPrintLink);
        function InitDialogPrintLink() {
            $('a#printing').each(function () {
                $(this).on('click', function (e) {

                    var hrefLink = $(this).data("href");
                    var iframe = $("#dialog-printing").find('iframe');

                    iframe.attr("src", hrefLink);

                    $("#dialog-printing").dialog({
                        autoOpen: false,
                        height: 'auto',
                        width: '900',
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

    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ModalPlaceHolder" runat="server">
    <div id="dialog-printing" title="Printing" style="background: #fff">
        <iframe src="" frameborder="0" width="100%" height="100%" style="min-height: 500px"></iframe>
    </div>
</asp:Content>
