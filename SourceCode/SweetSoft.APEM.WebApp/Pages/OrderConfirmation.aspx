<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master"
    AutoEventWireup="true" CodeBehind="OrderConfirmation.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.OrderConfirmation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .widthPriceGridView
        {
            width: 160px;
        }

        .center
        {
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="button-control col-md-12 col-sm-12">
            <div class="form-inline">
                <div class="form-group" style="margin-bottom: 0; width: 100%">
                    <asp:LinkButton runat="server" ID="btnSave" OnClientClick="SaveStateOfData('Now')" OnClick="btnSave_Click" CssClass="waitforajax btn btn-transparent new"><span class="flaticon-new10"></span> Save</asp:LinkButton>
                    <asp:LinkButton ID="btnDelete" runat="server"
                        class="btn btn-transparent new" OnClick="btnDelete_Click">
                        <span class="flaticon-delete41"></span>
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.DELETE)%>
                    </asp:LinkButton>
                    <asp:UpdatePanel runat="server" ID="upnPrinting" RenderMode="Inline" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Literal ID="ltrPrint" runat="server" EnableViewState="false"></asp:Literal>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <%--Trunglc Add - 22-04-2015--%>
                    <asp:LinkButton ID="btnLock" runat="server" Visible="false"
                        OnClick="btnLock_Click" CssClass="btn btn-transparent">
                                <span class="flaticon-padlock19"></span>
                            Lock
                    </asp:LinkButton>
                    <asp:LinkButton ID="btnUnlock" runat="server" Visible="false"
                        OnClick="btnUnlock_Click" CssClass="btn btn-transparent">
                                <span class="flaticon-padlock21"></span>
                            Unlock
                    </asp:LinkButton>
                    <%--End--%>
                    <asp:LinkButton ID="btnCancel" runat="server" class="btn btn-transparent new" OnClick="btnCancel_Click">
                        <span class="flaticon-back57"></span> <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.RETURN)%>
                    </asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <div class="row row_content">
        <div class="col-md-12 sweet-input-mask">
            <div class="row">
                <div class="col-md-2 col-sm-2">
                    <div class="form-group">
                        <label class="control-label">
                            <strong><%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.ORDER_NUMBER)%></strong>
                        </label>
                        <p class="form-control-static">
                            <asp:Label ID="lblOrderNumber" runat="server" ToolTip="Order Number"></asp:Label>
                        </p>
                    </div>
                </div>
                <div class="col-md-1 col-sm-1">
                    <div class="form-group">
                        <label class="control-label">
                            <strong><%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CUSTOMER)%></strong>
                        </label>
                        <SweetSoft:ExtraInputMask ID="txtCode" RenderOnlyInput="true" Required="true" ToolTip="Customer Code"
                            runat="server" Repeat="5" ShowMaskOnHover="true" MaxLength="5" Enabled="false"
                            Greedy="true" RightAlign="false"></SweetSoft:ExtraInputMask>
                    </div>
                </div>
                <div class="col-md-9 col-sm-9">
                    <div class="form-group">
                        <label class="control-label">
                            &nbsp;
                        </label>
                        <SweetSoft:CustomExtraTextbox ID="txtName" RenderOnlyInput="true" Placeholder="Customer's name" ToolTip="Customer Name"
                            runat="server" AutoCompleteType="Search"
                            RequiredText='<%# SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.THIS_FIELD_IS_REQUIRED)%>'></SweetSoft:CustomExtraTextbox>

                        <asp:HiddenField ID="hCustomerID" runat="server" />
                        <asp:LinkButton ID="btnLoadContacts" runat="server" OnClick="btnLoadContacts_Click" Style="display: none;"></asp:LinkButton>
                    </div>
                </div>
                <div class="col-md-12">
                    <div class="row">
                        <div class="col-sm-3 col-md-3">
                            <div class="row">
                                <asp:UpdatePanel runat="server" ID="upnlJobRev" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="col-sm-8">
                                            <div class="form-group" style="margin-bottom: 0">
                                                <label class="control-label">
                                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.JOB_NR)%>
                                                </label>

                                                <asp:DropDownList runat="server" ID="ddlJobNumber" ToolTip="Job Number"
                                                    data-style="btn btn-info" data-size="5" data-live-search="true"
                                                    data-width="100%" AutoPostBack="true"
                                                    data-toggle="dropdown" OnSelectedIndexChanged="ddlJobNumber_SelectedIndexChanged"
                                                    CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <div class="form-group">
                                                <label class="control-label">
                                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.REV)%>
                                                </label>
                                                <asp:DropDownList ID="ddlRevNumber" runat="server"
                                                    data-style="btn btn-info" ToolTip="Job Rev"
                                                    data-width="100%" AutoPostBack="true"
                                                    data-toggle="dropdown" OnSelectedIndexChanged="ddlRevNumber_SelectedIndexChanged"
                                                    CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="col-md-6 col-sm-6">
                            <div class="form-group">
                                <label class="control-label">
                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.JOB_NAME)%>
                                </label>
                                <asp:TextBox runat="server" CssClass="form-control" ID="txtJobName" ReadOnly="true" ToolTip="Job Name" />
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-3">
                            <div class="form-group">
                                <label class="control-label">
                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.DESIGN)%>
                                </label>
                                <asp:TextBox runat="server" CssClass="form-control" ID="txtDesign" ReadOnly="true" ToolTip="Design" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-sm-3 col-md-3">
                    <div class="row">
                        <div class="col-md-12 col-sm-12">
                            <label class="control-label">
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CONTACT_PERSON)%>
                            </label>
                            <asp:DropDownList ID="ddlContact" runat="server" AutoPostBack="false"
                                data-style="btn btn-info" ToolTip="Contact Person"
                                data-width="100%" Required="true"
                                data-toggle="dropdown"
                                CssClass="form-control">
                            </asp:DropDownList>

                        </div>
                        <div class="col-md-4 col-sm-4">
                        </div>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6">
                    <div class="form-group">
                        <div class="row">
                            <div class="form-group">
                                <div class="col-md-6 col-sm-6">
                                    <label class="control-label">
                                        <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CUSTOMER_PO)%>
                                    </label>
                                    <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtCustomerPO1" runat="server" CssClass="form-control" ToolTip="Customer PO1"
                                        RequiredText='<%# SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.THIS_FIELD_IS_REQUIRED)%>'></SweetSoft:CustomExtraTextbox>
                                </div>
                                <div class="col-md-6 col-sm-6">
                                    <label class="control-label">&nbsp;</label>
                                    <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtCustomerPO2" runat="server" CssClass="form-control" ToolTip="Customer PO2"
                                        RequiredText='<%# SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.THIS_FIELD_IS_REQUIRED)%>'></SweetSoft:CustomExtraTextbox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-3 col-sm-3">
                    <div class="form-group wrap-datepicker">
                        <label class="control-label">
                            <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.ORDER_DATE)%>
                        </label>
                        <SweetSoft:ExtraInputMask ID="txtOrderDate" RenderOnlyInput="true"
                            data-format="dd-mm-yyyy" ToolTip="Order Date"
                            CssClass="form-control mask-date a"
                            runat="server"></SweetSoft:ExtraInputMask>
                        <span class="fa fa-calendar in-mask-date"></span>
                    </div>
                </div>

            </div>

            <div class="row">
                <div class="col-sm-2">
                    <div class="form-group">
                        <label class="control-label">
                            <strong><%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.TAX)%></strong>
                        </label>
                        <div>
                            <asp:DropDownList runat="server" ID="ddlTax"
                                data-style="btn btn-info" AutoPostBack="true" OnSelectedIndexChanged="ddlTax_SelectedIndexChanged"
                                data-width="100%" ToolTip="Tax"
                                data-toggle="dropdown"
                                CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-sm-1">
                    <div class="form-group">
                        <label class="control-label">
                            <strong><%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.TAX_RATE)%></strong>
                        </label>
                        <div>
                            <SweetSoft:ExtraInputMask ID="txtTaxRate" Enabled="false" RenderOnlyInput="true" Required="false" Suffix=" %" ToolTip="Tax Rate"
                                runat="server" MaskType="Decimal" GroupSeparator="," RadixPoint="." Text="0" Digits="3" AutoGroup="true"></SweetSoft:ExtraInputMask>
                        </div>
                    </div>
                </div>
                <div class="col-sm-9">
                    <div class="well">
                        <div class="form-horizontal">
                            <div class="row">
                                <div class="col-sm-10 col-sm-pull-1">

                                    <label class="control-label col-sm-4">
                                        <span class="pull-left"></span>
                                        <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CURRENCY)%>
                                    </label>
                                    <div class="input-group">
                                        <SweetSoft:ExtraInputMask ID="txtCurrencyValue" RenderOnlyInput="true" Enabled="false" Required="false" ToolTip="Currency"
                                            runat="server" MaskType="Decimal" GroupSeparator="," RadixPoint="." Text="1" Digits="4" AutoGroup="true"></SweetSoft:ExtraInputMask>
                                        <span class="input-group-addon">
                                            <asp:Literal ID="ltrCurrencyName" runat="server"></asp:Literal>
                                        </span>
                                        <span class="input-group-addon">=</span>
                                        <SweetSoft:ExtraInputMask ID="txtRMValue" RenderOnlyInput="true" Enabled="false" Required="false" ToolTip="RM Value"
                                            runat="server" MaskType="Decimal" GroupSeparator="," RadixPoint="." Text="0" Digits="4" AutoGroup="true"></SweetSoft:ExtraInputMask>
                                        <span class="input-group-addon">
                                            <asp:Literal ID="ltrBaseCurrency" runat="server"></asp:Literal>
                                        </span>
                                    </div>
                                    <asp:HiddenField ID="hCurrencyID" runat="server" />
                                </div>
                                <div class="col-sm-2">
                                    <asp:Button runat="server" ID="btnPricesLookup" CssClass="btn btn-primary btn-block pull-right"
                                        OnClientClick="return false" />

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row" runat="server" id="divCylinder">
        <div class="col-md-12">
            <label class="control-label"><%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CYLINDERS)%></label>
        </div>
        <div class="col-md-12">
            <div class="form-group">
                <asp:UpdatePanel runat="server" ID="upnlCylinder" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel runat="server" ID="pnRecord">
                            <p class="text-muted text-center">
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.NO_RECORDS_FOUND)%>
                            </p>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="pnListCylinder" Visible="false">
                            <SweetSoft:GridviewExtension ID="gvClinders" ToolTip="Cylinders"
                                runat="server" AutoGenerateColumns="false"
                                CssClass="table table-striped table-bordered table-checkable dataTable"
                                GridLines="None" AllowPaging="false" AllowSorting="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="No">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblNo" Text='<%#Eval("Sequence")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Steel Base">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%#Eval("SteelBaseName") %>' ID="lblSteelBase"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cyl No">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%#Eval("CylinderNo")%>' ID="lblCylNo"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%#Eval("CylDescription")%>' ID="lblCylDescription"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Pricing">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%#Eval("PricingName")%>' ID="lblPricing"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Circumfere">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%#ShowNumberFormat(Eval("Circumference"), "N2")%>' ID="lblCircumfere"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Face width">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%#ShowNumberFormat(Eval("FaceWidth"), "N2")%>' ID="lblFaceWidth"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unit Price">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# ShowNumberFormat(Eval("UnitPrice"), "N3")%>' ID="lblUnitPrice"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <SweetSoft:ExtraInputMask ID="txtUnitPrice" RenderOnlyInput="true" Required="false" Enabled='<%#Eval("ForTobaccoCustomers") %>'
                                                runat="server" MaskType="Decimal" GroupSeparator="," RadixPoint="." Text='<%#Eval("UnitPrice")%>' Digits="3" AutoGroup="true" Width="120px"></SweetSoft:ExtraInputMask>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qty">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%#Eval("Quantity")%>' ID="lblQty"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Price">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%#string.Format("{0}",ShowNumberFormat(Eval("TotalPrice"), "N2")) %>' ID="lblPriceTaxed"></asp:Label>
                                            <asp:HiddenField runat="server" ID="hdfPriceTaxed" Value='<%#ShowNumberFormat(Eval("TotalPrice"), "N2") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </SweetSoft:GridviewExtension>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <div class="row" runat="server" id="divServiceJob" style="margin-top: 10px;">
        <div class="col-md-12">
            <label class="control-label">
                Additional Services
            </label>

            <asp:LinkButton Visible="false" Style="margin-left: 55px;" ID="btnAddServiceDetail" runat="server" OnClick="btnAddServiceDetail_Click"
                CssClass="pull"><i class="glyphicon glyphicon-plus"></i>
                Add service detail
            </asp:LinkButton>

        </div>
        <div class="col-md-12">
            <div class="form-group">
                <asp:UpdatePanel runat="server" ID="upnlServiceJobDetail" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div runat="server" id="divJobService">
                            <asp:GridView ID="grvServiceJobDetail" runat="server" ToolTip="Service Job"
                                AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-checkable dataTable"
                                GridLines="None"
                                AllowPaging="false"
                                AllowSorting="false"
                                ShowFooter="false"
                                DataKeyNames="ServiceJobID"
                                OnRowDataBound="grvServiceJobDetail_RowDataBound"
                                OnRowDeleting="grvServiceJobDetail_RowDeleting"
                                OnRowCancelingEdit="grvServiceJobDetail_RowCancelingEdit"
                                OnRowUpdating="grvServiceJobDetail_RowUpdating"
                                OnRowEditing="grvServiceJobDetail_RowEditing"
                                OnDataBound="grvServiceJobDetail_DataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNo" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Work order number">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWorkOrderNumber" runat="server"
                                                Text='<%#Eval("WorkOrderNumber")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-CssClass="column-large" HeaderText="ProductID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProductID" runat="server"
                                                Text='<%#Eval("ProductID")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description" FooterStyle-CssClass="total">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescription" runat="server"
                                                Text='<%#Eval("Description")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="column-large" HeaderText="Work order values">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWorkOrderValues" runat="server"
                                                Text='<%#Eval("WorkOrderValues")%>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <SweetSoft:ExtraInputMask ID="txtWorkOrderValues" RenderOnlyInput="true" Required="false"
                                                runat="server" MaskType="Decimal" GroupSeparator="," RadixPoint="." Text='<%#Eval("WorkOrderValues")%>' Digits="2" AutoGroup="true"></SweetSoft:ExtraInputMask>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField ItemStyle-CssClass="column-one text" HeaderStyle-CssClass="checkbox-column">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEdit" runat="server"
                                                CommandArgument='<%# Eval("ServiceJobID") %>' CommandName="Edit" CssClass="btn btn-primary">
                                                            <span class="fa fa-edit"></span>
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
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <div class="row" style="margin-top: 10px; margin-bottom: 15px;" runat="server" id="divOrderCharger">
        <div class="col-md-12">
            <div class="control-group">
                <label class="control-label DTTT_button_save">
                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.OTHER_CHARGES)%>
                </label>
                <div id="tablesContacts" class="dataTables_wrapper form-inline no-footer">
                    <%--<div class="dt_header">
                        <div class="row">
                            <div class="col-md-12 col-sm-12">
                                <div class="DTTT btn-group pull-right">
                                    <asp:LinkButton ID="btnAddOtherCharges" runat="server"
                                        CssClass="btn btn-default btn-sm DTTT_button_add"
                                        title="Add new item" OnClick="btnAddOtherCharges_Click">
                                            <span><i class="icol-add"></i>Add</span>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>--%>
                    <SweetSoft:GridviewExtension ID="grvOtherCharges" ToolTip="Other Charges"
                        runat="server" AutoGenerateColumns="false"
                        CssClass="table table-striped table-bordered table-checkable dataTable"
                        OnRowEditing="grvOtherCharges_RowEditing"
                        OnRowCommand="grvOtherCharges_RowCommand"
                        OnRowUpdating="grvOtherCharges_RowUpdating"
                        DataKeyNames="OtherChargesID, PricingID"
                        OnRowDeleting="grvOtherCharges_RowDeleting"
                        OnRowCancelingEdit="grvOtherCharges_RowCancelingEdit"
                        OnRowDataBound="grvOtherCharges_RowDataBound"
                        GridLines="None" AllowPaging="false" AllowSorting="false">
                        <Columns>
                            <asp:TemplateField HeaderText="Pricing" HeaderStyle-CssClass="column-100 text-center" ItemStyle-CssClass="column-100">
                                <ItemTemplate>
                                    <asp:Label ID="lbPricing" runat="server"
                                        Text='<%#Eval("PricingName")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="column-150 text-center" ItemStyle-CssClass="column-150">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblDescription" Text='<%#Eval("Description")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Quantity" HeaderStyle-CssClass="column-60 text-center" ItemStyle-CssClass="column-60">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblQuantity" Text='<%#Eval("Quantity")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Charge" HeaderStyle-CssClass="column-80 text-center" ItemStyle-CssClass="column-80 text-right">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCharge" Text='<%#ShowNumberFormat(Eval("Charge"), "N2")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div style="position: relative;">
                                        <SweetSoft:ExtraInputMask ID="txtCharge" RenderOnlyInput="true" Required="false"
                                            runat="server" MaskType="Decimal" GroupSeparator="," RadixPoint="." Width="100%"
                                            Text='<%#ShowNumberFormat(Eval("Charge"), "N2")%>' Digits="2" AutoGroup="true"></SweetSoft:ExtraInputMask>
                                    </div>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-CssClass="column-80 text-center" ItemStyle-CssClass="column-80 text-center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-primary"
                                        CommandArgument='<%# Eval("OtherChargesID") %>' CommandName="Edit">
                                                            <span class="fa fa-edit"></span>
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
                    </SweetSoft:GridviewExtension>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="co-md-8 col-sm-8">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="control-label col-sm-2"><%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.REMARK)%></label>
                    <div class="col-sm-10">
                        <asp:TextBox TextMode="MultiLine" ID="txtRemark" runat="server" Rows="2" CssClass="form-control" ToolTip="Remark"></asp:TextBox>
                    </div>

                    <%--<label class="control-label col-sm-2">
                        <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.REMARK_SCREEN)%>
                    </label>
                    <div class="col-sm-4">
                        <asp:TextBox TextMode="MultiLine" ID="txtRemarkScreen" runat="server" Rows="2" CssClass="form-control"></asp:TextBox>
                    </div>--%>
                </div>
            </div>
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="control-label col-sm-2">
                        <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.PAYMENT_TERMS)%>
                    </label>
                    <div class="col-sm-10">
                        <asp:TextBox runat="server" ID="txtPaymentTerms" class="form-control" ToolTip="Payment Terms"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="control-label col-sm-2">
                        <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.DELIVERY_TERMS)%>
                    </label>
                    <div class="col-sm-10">
                        <asp:TextBox runat="server" ID="txtDeliveryTerms" CssClass="form-control" ToolTip="Delivery Terms"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-4 col-sm-4">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="control-label col-sm-6">
                        Sub Total
                    </label>
                    <div class="col-sm-6">
                        <SweetSoft:ExtraInputMask ID="txtSubTotal" RenderOnlyInput="true" Required="false" Enabled="false" ToolTip="Sub Total"
                            runat="server" MaskType="Decimal" GroupSeparator="," RadixPoint="." Text="0" Digits="3" AutoGroup="true"></SweetSoft:ExtraInputMask>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-6">
                        <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.DISCOUNT)%> (%)
                    </label>
                    <div class="col-sm-6">
                        <SweetSoft:ExtraInputMask ID="txtDiscount" RenderOnlyInput="true" Required="false" onblur="CallKeyClick();" ToolTip="Discount"
                            runat="server" MaskType="Decimal" GroupSeparator="," RadixPoint="." Text="0" Digits="3" AutoGroup="true"></SweetSoft:ExtraInputMask>
                    </div>
                    <asp:LinkButton ID="btnDiscountChanged" runat="server" OnClick="btnDiscountChanged_Click"></asp:LinkButton>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-6">
                        Sub Total before GST
                    </label>
                    <div class="col-sm-6">
                        <SweetSoft:ExtraInputMask ID="txtNetTotal" RenderOnlyInput="true" Required="false" Enabled="false" ToolTip="Not Total(Before GST)"
                            runat="server" MaskType="Decimal" GroupSeparator="," RadixPoint="." Text="0" Digits="3" AutoGroup="true"></SweetSoft:ExtraInputMask>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-6">
                        GST at
                    </label>
                    <div class="col-sm-6">
                        <SweetSoft:ExtraInputMask ID="txtGSTAt" RenderOnlyInput="true" Required="false" Enabled="false" ToolTip="GST"
                            runat="server" MaskType="Decimal" GroupSeparator="," RadixPoint="." Text="0" Digits="3" AutoGroup="true"></SweetSoft:ExtraInputMask>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-6">
                        TOTAL
                    </label>
                    <div class="col-sm-6">
                        <SweetSoft:ExtraInputMask ID="txtTotalPrice" RenderOnlyInput="true" Required="false" Enabled="false" ToolTip="TOTAL"
                            runat="server" MaskType="Decimal" GroupSeparator="," RadixPoint="." Text="0" Digits="3" AutoGroup="true"></SweetSoft:ExtraInputMask>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField runat="server" ID="hdfJobID" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ModalPlaceHolder" runat="server">
    <div id="dialog-form-quotes-prices" title="Quoted Prices">
        <asp:UpdatePanel runat="server" ID="upnlQuotedPrices" UpdateMode="Always">
            <ContentTemplate>
                <div class="container-fluid">
                    <div class="row" style="margin-top: 15px;">
                        <div class="col-sm-12">
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-sm-4">
                                        <label class="control-label">
                                            Quotation No.
                                        </label>
                                        <div class="col-md-12">
                                            <div class="row">
                                                <asp:TextBox ID="txtQuotationNo" runat="server" CssClass="form-control" Enabled="false" ToolTip="Quotation No"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <label class="control-label">
                                            Quotation Date
                                        </label>
                                        <div class="wrap-datepicker">
                                            <SweetSoft:CustomExtraTextbox ID="txtQuotationDate" runat="server" ToolTip="Quotation Date"
                                                RenderOnlyInput="true" data-format="dd-MM-yyyy" Enabled="false"
                                                CssClass="datepicker form-control mask-date">
                                            </SweetSoft:CustomExtraTextbox>
                                            <span class="fa fa-calendar in-mask-date"></span>
                                        </div>
                                    </div>
                                    <div class="col-sm-2">
                                        <label class="control-label">
                                            <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.REV)%>
                                        </label>
                                        <asp:DropDownList runat="server" ID="ddlQuotationRev" ToolTip="Quotation Rev"
                                            data-style="btn btn-info"
                                            data-width="100%" AutoPostBack="true"
                                            data-toggle="dropdown" OnSelectedIndexChanged="ddlQuotationRev_SelectedIndexChanged"
                                            CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-12">
                            <div class="form-group">
                                <label class="control-label">
                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.QUOTED_PRICES)%>
                                </label>
                                <asp:GridView ID="grvPrices" runat="server" AutoGenerateColumns="false" ToolTip="Quotation Prices"
                                    CssClass="table table-striped table-bordered table-checkable dataTable" GridLines="None"
                                    AllowPaging="false" AllowSorting="false" DataKeyNames="QuotationID,PricingID">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Pricing Name" SortExpression="0" HeaderStyle-CssClass="sorting">
                                            <ItemTemplate>
                                                <asp:Label ID="lbPricingName" runat="server" Text='<%# Eval("PricingName") %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfPricingID" Value='<%# Eval("PricingID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="New Steel Base" SortExpression="0" HeaderStyle-CssClass="sorting">
                                            <ItemTemplate>
                                                <div class="col-md-12">
                                                    <SweetSoft:ExtraInputMask ID="txtNewSteelBasePrice" RenderOnlyInput="true" Required="false"
                                                        Text='<%# ShowNumberFormat(Eval("NewSteelBasePrice"), "N3") %>'
                                                        runat="server" MaskType="Decimal" GroupSeparator="," RadixPoint="."
                                                        Digits="3" AutoGroup="true"></SweetSoft:ExtraInputMask>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Old Steel Base" SortExpression="0" HeaderStyle-CssClass="sorting">
                                            <ItemTemplate>
                                                <div class="col-md-12">
                                                    <SweetSoft:ExtraInputMask ID="txtOldSteelBasePrice" RenderOnlyInput="true" Required="false"
                                                        Text='<%# ShowNumberFormat(Eval("OldSteelBasePrice"), "N3") %>'
                                                        runat="server" MaskType="Decimal" GroupSeparator="," RadixPoint="."
                                                        Digits="3" AutoGroup="true"></SweetSoft:ExtraInputMask>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CURR" HeaderStyle-CssClass="sorting">
                                            <ItemTemplate>
                                                <asp:Label ID="lbCurr" runat="server" Text='<%# Eval("CurrencyName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Unit" HeaderStyle-CssClass="sorting">
                                            <ItemTemplate>
                                                <asp:Label ID="lbUnit" runat="server" Text='<%# Eval("UnitName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <div class="col-sm-12">
                            <div class="form-group">
                                <label class="control-label">
                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.QUOTATION_TEXT)%>
                                </label>
                                <asp:TextBox ID="txtQuotationNote" runat="server" ToolTip="Quotation Note"
                                    TextMode="MultiLine" Rows="4" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6">
                            <asp:Label Style="text-align: left; color: #31708f; font-weight: bold;" ID="lbQuotationMessage" runat="server"></asp:Label>
                        </div>
                        <div class="col-sm-6 pull-right">
                            <div class="form-group pull-right">
                                <asp:LinkButton CssClass="btn btn-default" runat="server" ID="btnReset" OnClick="btnReset_Click">
                                    <span class="glyphicon glyphicon-refresh"></span>
                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.RESET)%>
                                </asp:LinkButton>
                                <asp:LinkButton CssClass="btn btn-default" runat="server" ID="btnFixPrice" OnClick="btnFixPrice_Click">
                                    <span class="glyphicon glyphicon-floppy-saved"></span>
                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.FIX_PRICE)%>
                                </asp:LinkButton>
                                <asp:LinkButton CssClass="btn btn-default btn-close-form-prices" runat="server" ID="btnCancel1" OnClientClick="return false;">
                                    <span class="glyphicon glyphicon-remove"></span>
                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CANCEL)%>
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="dialog-printing" title="Printing" style="background: #fff">
        <iframe src="" frameborder="0" width="100%" height="100%" style="min-height: 500px"></iframe>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptPlaceHolder" runat="server">
    <script src="/js/plugins/printThis.js"></script>
    <script>
        var viewstate = '<%=ViewState_PageID%>';
        $(document).ready(function () {
            $('#dialog-printing').hide();
            InitDialogPrintLink();
            DatePicker();
            SearchText();
            ShowDialogJobQuotationPricing();
            SaveStateOfData('Before');
        });

        function SaveStateOfData(time) {
            var obj = [
                {
                    key: 'grvServiceJobDetail_' + time,
                    data: $("[id$='grvServiceJobDetail']").html() == undefined ? "<table></table>" : $("[id$='grvServiceJobDetail']").html(),
                    PageID: viewstate
                },
                {
                    key: 'grvOtherCharges_' + time,
                    data: $("[id$='grvOtherCharges']").html() == undefined ? "<table></table>" : $("[id$='grvOtherCharges']").html(),
                    PageID: viewstate
                },
                {
                    key: 'gvClinders_' + time,
                    data: $("[id$='gvClinders']").html() == undefined ? "<table></table>" : $("[id$='gvClinders']").html(),
                    PageID: viewstate
                }
            ];
            SaveStateOfDataForm("Job.aspx/SaveDataTable", obj, time);
        }

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(ShowDialogJobQuotationPricing);
        function ShowDialogJobQuotationPricing() {
            setTimeout(function () {
                $("#dialog-form-quotes-prices").dialog({
                    autoOpen: false,
                    appendTo: "form",
                    height: 'auto',
                    width: '800',
                    modal: true,
                    resizable: false
                });

                $("input[type='submit'][id$='btnPricesLookup']").on("click", function () {
                    $("#dialog-form-quotes-prices").dialog("open")
                });

                $('.btn-close-form-prices').click(function () {
                    $("#dialog-form-quotes-prices").dialog("close")
                });

            }, 10);
        }

        function SaveClick() {
            $("button[type='submit'][id$='btnSubmit']").click();
        }

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SearchText);
        function SearchText(s, a) {
            if ($("input[type='text'][id$='txtName']").length > 0) {
                $(".ui-autocomplete, .ui-helper-hidden-accessible").remove();
                $("input[type='text'][id$='txtName']").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "OrderConfirmation.aspx/GetCustomerData",
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
                        $("input[type='hidden'][id$='hCustomerID']").val(ui.item.ID);
                        document.getElementById('<%= btnLoadContacts.ClientID %>').click();
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
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(DatePicker);

        function DatePicker() {
            var nowTemp = new Date();
            var now = new Date(nowTemp.getFullYear(), nowTemp.getMonth(), nowTemp.getDate(), 0, 0, 0, 0);

            var oDate = $('.a').datepicker({
                format: "dd/mm/yyyy"
            });

            var dDate = $('.b').datepicker({
                format: "dd/mm/yyyy",
            });

            oDate.on('changeDate', function (e) {
                //console.log(oDate.datepicker('getUTCDate'))
                //console.log(oDate.datepicker('getDates'))
                //console.log(oDate.datepicker('getUTCDates'))
                var date = new Date(oDate.datepicker('getDate'));
                var _date = new Date(date.getFullYear(), date.getMonth(), date.getDate() + 1, 0, 0, 0, 0);
                console.log(_date)

                dDate.datepicker('remove').datepicker({
                    format: "dd/mm/yyyy",
                    startDate: _date
                });
                dDate.datepicker('update', new Date(date.getFullYear(), date.getMonth(), date.getDate() + 1, 0, 0, 0, 0));
            })
        }

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitDialogPrintLink);
        function InitDialogPrintLink() {
            $('.openPrinting a').each(function () {
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

        function CallKeyClick() {
            document.getElementById('<%= btnDiscountChanged.ClientID %>').click();
        };
    </script>
</asp:Content>

