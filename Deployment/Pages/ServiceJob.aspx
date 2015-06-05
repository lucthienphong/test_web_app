<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master"
    AutoEventWireup="true" CodeBehind="ServiceJob.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.ServiceJob" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .total
        {
            text-align: right !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="button-control col-md-12 col-sm-12 affix">
            <div class="row">
                <div class="col-xs-12">
                    <div class="form-horizontal">
                        <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click" CssClass="btn btn-transparent">
                                <span class="flaticon-floppy1"></span>
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.SAVE)%>
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnDelete" runat="server" fgrvs
                            class="btn btn-transparent new" OnClick="btnDelete_Click">
                                <span class="flaticon-delete41"></span>
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.DELETE)%>
                        </asp:LinkButton>
                        <%--Trunglc Add - 27-04-2015--%>
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
                        <asp:LinkButton ID="btnCancel" runat="server" OnClick="btnCancel_Click" CssClass="btn btn-transparent">
                            <span class="flaticon-back57"></span>
                            <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.RETURN)%>
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row row_content">
        <div class="col-md-9 col-sm-9">
            <div class="form-group">
                <label class="control-label">
                    <strong><%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CUSTOMER)%></strong>
                </label>
                <div class="row">
                    <div class="col-sm-12">
                        <div class="form-horizontal">
                            <div class="form-group" style="margin-bottom: 0">
                                <div class="col-sm-2 col-xs-2">
                                    <SweetSoft:ExtraInputMask ID="txtCode" RenderOnlyInput="true" Required="true"
                                        runat="server" Repeat="5" ShowMaskOnHover="true" MaxLength="5" Enabled="false"
                                        Greedy="true" RightAlign="false"></SweetSoft:ExtraInputMask>
                                </div>
                                <div class="col-sm-10 col-xs-10">
                                    <SweetSoft:CustomExtraTextbox ID="txtName" RenderOnlyInput="true"
                                        runat="server"></SweetSoft:CustomExtraTextbox>
                                </div>
                                <asp:HiddenField ID="hCustomerID" runat="server" />
                                <asp:LinkButton ID="btnLoadContacts" runat="server" OnClick="btnLoadContacts_Click" Style="display: none;"></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-sm-3 col-md-3">
            <div class="row">
                <div class="col-sm-12">
                    <div class="form-group" style="margin-bottom: 0">
                        <label class="control-label">
                            <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.JOB_NR)%>
                        </label>
                        <SweetSoft:CustomExtraTextbox ID="txtJobNumber" RenderOnlyInput="true"
                            runat="server" Enabled="false"></SweetSoft:CustomExtraTextbox>
                        <asp:HiddenField ID="hBarcode" runat="server" />
                    </div>
                </div>
            </div>
        </div>
        <%--<div class="col-sm-12 text-right">
            <asp:Image ID="imgBarcode" CssClass="visible-xs-inline visible-sm-inline visible-md-inline visible-lg-inline img-responsive" runat="server" AlternateText="Barcode" />
            <asp:HiddenField ID="hBarcode" runat="server" />
        </div>--%>
    </div>

    <div class="row">
        <div class="col-md-12 col-sm-12">
            <div class="row">
                <div class="col-md-6 col-sm-6">
                    <div class="form-group">
                        <label class="control-label">
                            <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.JOB_NAME)%>
                        </label>
                        <SweetSoft:CustomExtraTextbox ID="txtJobName"
                            RenderOnlyInput="true" runat="server"></SweetSoft:CustomExtraTextbox>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6">
                    <div class="form-group">
                        <label class="control-label">
                            <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.DESIGN)%>
                        </label>
                        <SweetSoft:CustomExtraTextbox ID="txtDesign"
                            RenderOnlyInput="true" runat="server"></SweetSoft:CustomExtraTextbox>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-3 col-sm-3">
            <div class="row">
                <div class="col-md-12 com-sm-12">
                    <label class="control-label">
                        <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CONTACT_PERSON)%>
                    </label>
                    <asp:DropDownList ID="ddlContacts" runat="server"
                        data-style="btn btn-info"
                        data-width="100%" Required="true"
                        data-toggle="dropdown"
                        CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="col-md-3 col-sm-3">
            <div class="row">
                <div class="col-md-12 com-sm-12">
                    <label class="control-label">
                        <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.SALES_REP)%>
                    </label>
                    <asp:DropDownList ID="ddlSaleRep" runat="server"
                        data-style="btn btn-info"
                        data-width="100%" Required="true"
                        data-toggle="dropdown" data-live-search="true"
                        CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="col-md-2 col-sm-2">
            <div class="form-group">
                <label class="control-label">
                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.JOB_COORDINATOR)%>
                </label>
                <asp:DropDownList ID="ddlJobCoordinator" runat="server"
                    data-style="btn btn-info"
                    data-width="100%" Required="true"
                    data-toggle="dropdown"
                    CssClass="form-control">
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-md-1 col-sm-1">
            <div class="form-group">
                <label class="control-label">
                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CURRENCY)%>
                </label>
                <asp:DropDownList ID="ddlCurrency" runat="server"
                    data-style="btn btn-info"
                    data-width="100%" Required="true"
                    data-toggle="dropdown" data-live-search="true"
                    CssClass="form-control">
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-md-3 col-sm-3">
            <div class="form-group">
                <label class="control-label">Product type</label>
                <asp:DropDownList ID="ddlProductType" runat="server"
                    data-style="btn btn-info"
                    data-width="100%" Required="true"
                    data-toggle="dropdown"
                    CssClass="form-control">
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6 col-sm-6">
            <div class="row">
                <div class="col-md-6 col-sm-6">
                    <div class="form-group">
                        <label class="control-label">
                            <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.PAYMENT_TERMS)%>
                        </label>
                        <asp:TextBox ID="txtPaymentTerms" runat="server" class="form-control">
                        </asp:TextBox>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6">
                    <div class="form-group">
                        <label class="control-label">
                            Item code
                        </label>
                        <asp:TextBox ID="txtItemCode" runat="server" class="form-control">
                        </asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-6 col-sm-6">
            <div class="row">
                <div class="col-md-4 col-sm-4">
                    <div class="form-group">
                        <label class="control-label">
                            <strong>Brand Owner</strong>
                        </label>
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="form-group" style="margin-bottom: 0">
                                    <asp:DropDownList ID="ddlBrandOwner" runat="server"
                                        data-style="btn btn-info"
                                        data-width="100%" Required="true"
                                        data-toggle="dropdown" data-live-search="true"
                                        CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-3 col-sm-3">
                    <div class="form-group wrap-datepicker">
                        <label class="control-label">
                            <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.DATE_CREATED)%>
                        </label>
                        <SweetSoft:CustomExtraTextbox ID="txtCreatedDate" runat="server"
                            RenderOnlyInput="true" data-format="dd-MM-yyyy" Enabled="false"
                            CssClass="datepicker form-control mask-date">
                        </SweetSoft:CustomExtraTextbox>
                        <span class="fa fa-calendar in-mask-date"></span>
                    </div>
                </div>
                <div class="col-md-5 col-sm-5">
                    <label class="control-label">
                        <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CREATED_BY)%>
                    </label>
                    <SweetSoft:CustomExtraTextbox ID="txtCreatedBy" Enabled="false"
                        RenderOnlyInput="true" runat="server"></SweetSoft:CustomExtraTextbox>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="form-group">
                <label class="control-label DTTT_button_save">
                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.SERVICE_JOB_DETAIL)%>
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
                    <asp:GridView ID="grvServiceJobDetail" runat="server"
                        AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-checkable dataTable"
                        GridLines="None"
                        AllowPaging="false"
                        AllowSorting="false"
                        ShowFooter="false"
                        DataKeyNames="ServiceJobID, PricingID"
                        OnRowDataBound="grvServiceJobDetail_RowDataBound"
                        OnRowCommand="grvServiceJobDetail_RowCommand"
                        OnRowDeleting="grvServiceJobDetail_RowDeleting"
                        OnRowCancelingEdit="grvServiceJobDetail_RowCancelingEdit"
                        OnRowUpdating="grvServiceJobDetail_RowUpdating"
                        OnRowEditing="grvServiceJobDetail_RowEditing"
                        OnDataBound="grvServiceJobDetail_DataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="No" HeaderStyle-CssClass="column-100 text-center" ItemStyle-CssClass="column-100">
                                <ItemTemplate>
                                    <asp:Label ID="lblNo" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Work order number" HeaderStyle-CssClass="column-120 text-center" ItemStyle-CssClass="column-120">
                                <ItemTemplate>
                                    <asp:Label ID="lblWorkOrderNumber" runat="server"
                                        Text='<%#Eval("WorkOrderNumber")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="txtWorkOrderNumber" Text='<%#Eval("WorkOrderNumber") %>'
                                        Width="100%" CssClass="form-control"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ProductID" HeaderStyle-CssClass="column-100 text-center" ItemStyle-CssClass="column-100">
                                <ItemTemplate>
                                    <asp:Label ID="lblProductID" runat="server"
                                        Text='<%#Eval("ProductID")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="txtProductID" Text='<%#Eval("ProductID") %>'
                                        Width="100%" CssClass="form-control"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description" FooterStyle-CssClass="total" HeaderStyle-CssClass="column-250 text-center" ItemStyle-CssClass="column-250">
                                <ItemTemplate>
                                    <asp:Label ID="lblDescription" runat="server"
                                        Text='<%#Eval("Description")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="txtDescription" Text='<%#Eval("Description") %>'
                                        Width="100%" CssClass="form-control"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Pricing" HeaderStyle-CssClass="column-120 text-center" ItemStyle-CssClass="column-120">
                                <ItemTemplate>
                                    <asp:Label ID="lbPricing" runat="server"
                                        Text='<%#Eval("PricingName")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div style="position: relative;">
                                        <asp:DropDownList ID="ddlPricing" runat="server"
                                            data-style="btn btn-info" data-width="100%" Required="true"
                                            data-toggle="dropdown" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-CssClass="column-80 text-center" ItemStyle-CssClass="column-80 text-center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-primary"
                                        CommandArgument='<%# Eval("ServiceJobID") %>' CommandName="Edit">
                                                            <span class="fa fa-edit"></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
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
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderStyle-CssClass="column-60 text-center" ItemStyle-CssClass="column-60 text-center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-primary"
                                        CommandName="delete" CommandArgument='<%#Eval("ServiceJobID") %>' Text="Delete">
                                            <span class="glyphicon glyphicon-remove"></span></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="form-group">
                <label class="control-label DTTT_button_save">
                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.OTHER_CHARGES)%>
                </label>
                <div class="dataTables_wrapper form-inline no-footer">
                    <div class="dt_header">
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
                    </div>
                    <SweetSoft:GridviewExtension ID="grvOtherCharges"
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
                                <EditItemTemplate>
                                    <div style="position: relative;">
                                        <asp:DropDownList ID="ddlPricing" runat="server"
                                            data-style="btn btn-info" data-width="100%" Required="true"
                                            data-container="body" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlPricing_SelectedIndexChanged"
                                            data-toggle="dropdown" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="column-150 text-center" ItemStyle-CssClass="column-150">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblDescription" Text='<%#Eval("Description")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div style="position: relative;">
                                        <asp:TextBox runat="server" CssClass="form-control" ID="txtDescription"
                                            Text='<%#Eval("Description")%>' Width="100%"></asp:TextBox>
                                    </div>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Quantity" HeaderStyle-CssClass="column-60 text-center" ItemStyle-CssClass="column-60">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblQuantity" Text='<%#Eval("Quantity")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div style="position: relative;">
                                        <SweetSoft:ExtraInputMask ID="txtQuantity" RenderOnlyInput="true" Required="false"
                                            runat="server" MaskType="Integer" GroupSeparator="," Text='<%#Eval("Quantity")%>'
                                            AutoGroup="true" Width="100%"></SweetSoft:ExtraInputMask>
                                    </div>
                                </EditItemTemplate>
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
                            <asp:TemplateField HeaderStyle-CssClass="column-60 text-center" ItemStyle-CssClass="column-60 text-center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnDelete"
                                        runat="server" CssClass="btn btn-primary"
                                        CommandArgument='<%#Eval("OtherChargesID")%>' CommandName="delete">
                                            <span class="glyphicon glyphicon-remove"></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </SweetSoft:GridviewExtension>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="ModalContent" ContentPlaceHolderID="ModalPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            SearchText();


            //$("tr:last").find("td").each(function (index) {
            //    switch (index) {
            //        case 0:
            //            $(this).attr("style", "border-bottom-color: transparent !important;border-right-color: transparent !important;border-left-color: transparent !important;")
            //            break;
            //        case 1:
            //            $(this).attr("style", " text-align: right;border-bottom: 4px double #000 !important;border-left-color: transparent !important;border-right-color: transparent !important;")
            //            break;
            //        case 2:
            //            $(this).attr("style", " direction: rtl !important;border-bottom: 4px double #000 !important;border-left-color: transparent !important;border-right-color: transparent !important;")
            //            break;
            //    }
            //});
        });



        addRequestHanlde(InitCheckAll);
        InitCheckAll();
        function InitCheckAll() {
            $("#chkSelectAll").change(function () {
                var isChecked = $(this).is(':checked');
                var columnIndex = $(this).closest('th').index();
                console.log(columnIndex);
                var checkboxother = $(this).closest('table').find('tr > td:nth-child(' + (columnIndex + 1) + ') input[type="checkbox"]');
                if (isChecked) {
                    checkboxother.prop('checked', true).trigger('change');
                }
                else
                    checkboxother.each(function () {
                        $(this).prop('checked', false).trigger('change');
                    });
            });
        }



        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SearchText);
        function SearchText(s, a) {
            if ($("input[type='text'][id$='txtName']").length > 0) {
                $(".ui-autocomplete, .ui-dialog, .ui-helper-hidden-accessible").remove();
                $("input[type='text'][id$='txtName']").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "Job.aspx/GetCustomerData",
                            data: "{'Keyword':'" + $("input[type='text'][id$='txtName']").val() + "'}",
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
            else {
                $("input[type='hidden'][id$='hCustomerID']").val("");
            }
        }

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CreateEyemark);
        function CreateEyemark() {
            $("#dialog-form-eye-mark").dialog({
                autoOpen: false,
                height: 'auto',
                width: '350',
                modal: true,
                resizable: false
            });
        }

        function ChangeIsPivot(rb) {
            var isChecked = rb.checked;
            var columnIndex = $(rb).closest('td').index();
            var other = $(rb).closest('table').find('tr > td:nth-child(' + (columnIndex + 1) + ') input[type="checkbox"]');
            $.each(other, function (i, o) {
                if (o.id != rb.id) {
                    if ($(o).is(':checked')) {
                        $(o).prop('checked', false).trigger('change');
                    }
                }
                else {
                    $(rb).prop('checked', true).trigger('change');
                }
            });

        }

    </script>
</asp:Content>
