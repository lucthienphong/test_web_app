<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master" AutoEventWireup="true" CodeBehind="Customer.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.Customer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="button-control col-md-12 col-sm-12 affix">
            <div class="row">
                <div class="col-md-6 col-sm-6">
                    <div class="form-inline">
                        <div class="form-group">
                            <asp:LinkButton class="btn btn-transparent" ID="btnSave"
                                runat="server" OnClick="btnSave_Click">
                                <span class="flaticon-floppy1"></span> <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.SAVE)%>
                            </asp:LinkButton>
                            <asp:LinkButton class="btn btn-transparent" ID="btnPricingMaster"
                                runat="server" OnClick="btnPricingMaster_Click">
                                <span class="flaticon-medical50"></span>&nbsp;Pricing master
                            </asp:LinkButton>
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
        <div class="col-md-12">
            <div class="form-group">
                <div class="row">
                    <div class="col-sm-12">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div class="col-sm-2 col-md-1">
                                    <label class="control-label">
                                        Short code
                                    </label>
                                    <SweetSoft:ExtraInputMask ID="txtCode" RenderOnlyInput="true" Required="true"
                                        runat="server" Repeat="5" ShowMaskOnHover="true" MaxLength="5"
                                        Greedy="true" RightAlign="false"></SweetSoft:ExtraInputMask>
                                </div>
                                <div class="col-sm-8 col-md-8">
                                    <label class="control-label">
                                        <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CUSTOMER_NAME)%>
                                    </label>
                                    <SweetSoft:CustomExtraTextbox ID="txtName" CssClass="ignore" RenderOnlyInput="true" runat="server"></SweetSoft:CustomExtraTextbox>
                                </div>
                                <div class="col-sm-2 col-md-2">
                                    <label class="control-label">
                                        &nbsp;
                                    </label>
                                    <div>
                                        <label class="control-label" style="display:none;">
                                            <asp:CheckBox ID="chkIsBrand" runat="server" AutoPostBack="true" OnCheckedChanged="chkIsBrand_CheckedChanged" CssClass="uniform" Visible="false"/>
                                            Brand Owner
                                        </label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 tabbable">
            <ul class="nav nav-tabs" role="tablist">
                <li class="active"><a href="#detail" role="tab" data-toggle="tab">
                    <strong>
                        <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CUSTOMER_DETAIL)%>
                    </strong>
                </a></li>
                <li runat="server" id="divShipping"><a href="#shipping" role="tab" data-toggle="tab">
                    <strong>
                        <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CUSTOMER_SHIPPING)%>
                    </strong>
                </a></li>
            </ul>
        </div>
    </div>
    <div class="tab-content">
        <div class="tab-pane active" id="detail">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-6 col-sm-6 sweet-input-mask">
                        <div class="form">
                            <div class="form-group">
                                <label class="control-label">
                                    Customer short name
                                </label>
                                <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtShortName" runat="server"
                                    MaxWords="64" MaxLength="64" CssClass="form-control"></SweetSoft:CustomExtraTextbox>
                            </div>

                            <div class="form-group" style="margin-bottom:0px;">
                                <div class="row">
                                    <div class="col-md-6 col-sm-6">
                                        <div class="form-group">
                                            <label class="control-label">
                                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CUSTOMER_TEL)%>
                                            </label>
                                            <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtTelNumber"
                                                runat="server" CssClass="form-control"></SweetSoft:CustomExtraTextbox>
                                        </div>
                                    </div>
                                    <div class="col-md-6 col-sm-6">
                                        <div class="form-group">
                                            <label class="control-label">
                                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CUSTOMER_FAX)%>
                                            </label>
                                            <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtFax"
                                                runat="server" CssClass="form-control"></SweetSoft:CustomExtraTextbox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label">
                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.EMAIL)%>
                                </label>
                                <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtEmail"
                                    EmailType="true" EmailErrorText="Wrong email format"
                                    runat="server" CssClass="form-control" MaxWords="200" MaxLength="200"></SweetSoft:CustomExtraTextbox>
                            </div>
                            <div class="form-group">
                                <label class="control-label">
                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CUSTOMER_ADDRESS)%>
                                </label>
                                <asp:TextBox ID="txtAddress" runat="server"
                                    TextMode="MultiLine" Rows="3"
                                    CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group" style="margin-bottom:0px;">
                                <div class="row">
                                    <div class="col-md-6 col-sm-6">
                                        <div class="form-group">
                                            <label class="control-label">
                                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CUSTOMER_CITY)%>
                                            </label>
                                            <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtCity"
                                                runat="server" CssClass="form-control"></SweetSoft:CustomExtraTextbox>
                                        </div>
                                    </div>
                                    <div class="col-md-6 col-sm-6">
                                        <div class="form-group">
                                            <label class="control-label">
                                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CUSTOMER_POSTCODE)%>
                                            </label>
                                            <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtPostcode" runat="server" CssClass="form-control"></SweetSoft:CustomExtraTextbox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label">
                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CUSTOMER_COUNTRY)%>
                                </label>
                                <asp:DropDownList ID="ddlCountry" runat="server"
                                    data-style="btn btn-info btn-block"
                                    data-width="100%" data-live-search="true"
                                    data-toggle="dropdown"
                                    CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 col-sm-6 sweet-input-mask">
                        <div class="form">
                            <div class="form-group">
                                <label class="control-label">
                                    GST Code
                                </label>
                                <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtGSTCode" runat="server" 
                                    MaxWords="12" MaxLength="12" CssClass="form-control"></SweetSoft:CustomExtraTextbox>
                            </div>
                            <div class="form-group">
                                <label class="control-label">
                                    TIN (Tax Information No.)
                                </label>
                                <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtTIN" runat="server" MaxWords="20" MaxLength="20" CssClass="form-control"></SweetSoft:CustomExtraTextbox>
                            </div>
                            <div class="form-group" style="margin-bottom:0px;">
                                <div class="row">
                                    <div class="col-md-6 col-sm-6">
                                        <div class="form-group">
                                            <label class="control-label">
                                                Customer code (SAP)
                                            </label>
                                            <SweetSoft:ExtraInputMask ID="txtSAPCode" MaxLength="5" RenderOnlyInput="true" Required="false"
                                                runat="server" MaskString="99999" AutoGroup="true"                                                
                                                Greedy="true" RightAlign="false"></SweetSoft:ExtraInputMask>
                                        </div>
                                    </div>
                                    <div class="col-md-6 col-sm-6">
                                        <div class="form-group">
                                            <label class="control-label">
                                                Internal Order No. (SAP)
                                            </label>
                                            <SweetSoft:ExtraInputMask ID="txtInternalOrderNo" MaxLength="6"  RenderOnlyInput="true" Required="true"
                                                runat="server" MaskString="999999" ShowMaskOnHover="true"
                                                Greedy="true" RightAlign="false"></SweetSoft:ExtraInputMask>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label">
                                    Group
                                </label>
                                <asp:DropDownList ID="ddlCustomerGroup" runat="server"
                                    data-style="btn btn-info btn-block"
                                    data-width="100%" data-live-search="true"
                                    data-toggle="dropdown"
                                    CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label>
                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CUSTOMER_SALE_REP)%>
                                </label>
                                <asp:DropDownList ID="ddlSaleRep" runat="server"
                                    data-style="btn btn-info btn-block"
                                    data-width="100%" data-live-search="true"
                                    data-toggle="dropdown"
                                    CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div runat="server" id="divRight">
                                <div class="form-group">
                                    <label class="control-label">
                                        <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CUSTOMER_DELIVERY)%>
                                    </label>
                                    <%--<asp:TextBox ID="txtCustomerDelivery" runat="server" CssClass="form-control"></asp:TextBox>--%>
                                    <asp:DropDownList ID="ddlDelivery" runat="server"
                                        data-style="btn btn-info btn-block"
                                        data-width="100%" data-live-search="true"
                                        data-toggle="dropdown"
                                        CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <label class="control-label">
                                        <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CUSTOMER_PAYMENT)%>
                                    </label>
                                    <%--<asp:TextBox ID="txtCustomerPayment" runat="server"
                                        data-ignore="1" CssClass="form-control"></asp:TextBox>--%>
                                    <asp:DropDownList ID="ddlPayment" runat="server"
                                        data-style="btn btn-info btn-block"
                                        data-width="100%" data-live-search="true"
                                        data-toggle="dropdown"
                                        CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label>
                                    <asp:CheckBox ID="chkIsObsolete" runat="server" CssClass="uniform" />
                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.IS_OBSOLETE)%>
                                </label>
                            </div>
                        </div>
                    </div>

                    <div id="divContact" class="col-md-12 col-sm-12">
                        <div class="form-group">
                            <label class="control-label DTTT_button_save">
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CUSTOMER_CONTACT)%>
                            </label>
                            <div id="tablesContacts" class="dataTables_wrapper form-inline no-footer">
                                <div class="dt_header">
                                    <div class="row">
                                        <div class="col-md-12 col-sm-12">
                                            <div class="DTTT btn-group pull-right">
                                                <asp:LinkButton ID="btnAddContact" runat="server"
                                                    CssClass="btn btn-default btn-sm DTTT_button_add"
                                                    title="Add new item" OnClick="btnAddContact_Click">
                                                        <span><i class="icol-add"></i>Add</span>
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnDeleteContact" runat="server"
                                                    CssClass="btn btn-default btn-sm DTTT_button_delete"
                                                    title="Delete selected rows" OnClick="btnDeleteContact_Click">
                                                        <span><i class="icol-delete"></i>Delete</span>
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <asp:GridView ID="grvContacts" runat="server" AutoGenerateColumns="false"
                                    CssClass="table table-striped table-bordered table-checkable dataTable" GridLines="None"
                                    AllowPaging="false" AllowSorting="false" DataKeyNames="ContactID"
                                    OnRowCommand="grvContacts_RowCommand"
                                    OnRowDeleting="grvContacts_RowDeleting"
                                    OnRowEditing="grvContacts_RowEditing"
                                    OnRowUpdating="grvContacts_RowUpdating"
                                    OnRowCancelingEdit="grvContacts_RowCancelingEdit">
                                    <Columns>
                                        <asp:TemplateField HeaderText="ContactName" SortExpression="0" HeaderStyle-CssClass="sorting">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit"
                                                    CommandArgument='<%#Eval("ContactID")%>' Text='<%#Eval("ContactName")%>' data-id='<%#Eval("ContactID")%>'></asp:LinkButton>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <div style="position: relative;">
                                                    <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtContactName"
                                                        Text='<%#Eval("ContactName")%>' Width="100%"
                                                        CssClass="form-control" runat="server">
                                                    </SweetSoft:CustomExtraTextbox>
                                                </div>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Honorific" SortExpression="1" HeaderStyle-CssClass="sorting">
                                            <ItemTemplate>
                                                <asp:Label ID="lbHonorific" runat="server"
                                                    Text='<%#Eval("Honorific")%>'
                                                    CommandName="Edit"></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <div style="position: relative;">
                                                    <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtHonorific"
                                                        Text='<%#Eval("Honorific")%>' Width="100%"
                                                        CssClass="form-control" runat="server">
                                                    </SweetSoft:CustomExtraTextbox>
                                                </div>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Designation" SortExpression="2" HeaderStyle-CssClass="sorting">
                                            <ItemTemplate>
                                                <asp:Label ID="lbDesignation" runat="server"
                                                    Text='<%#Eval("Designation")%>'
                                                    CommandName="Edit"></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <div style="position: relative;">
                                                    <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtDesignation"
                                                        Text='<%#Eval("Designation")%>' Width="100%"
                                                        CssClass="form-control" runat="server">
                                                    </SweetSoft:CustomExtraTextbox>
                                                </div>
                                            </EditItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Phone" SortExpression="2" HeaderStyle-CssClass="sorting">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPhone" runat="server"
                                                    Text='<%#Eval("Tel")%>'
                                                    CommandName="Edit"></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <div style="position: relative;">
                                                    <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtPhone"
                                                        Text='<%#Eval("Tel")%>' Width="100%"
                                                        CssClass="form-control" runat="server">
                                                    </SweetSoft:CustomExtraTextbox>
                                                </div>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Email" SortExpression="2" HeaderStyle-CssClass="sorting">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmail" runat="server"
                                                    Text='<%#Eval("Email")%>'
                                                    CommandName="Edit"></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <div style="position: relative;">
                                                    <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtEmail"
                                                        Text='<%#Eval("Email")%>' Width="100%"
                                                        CssClass="form-control" runat="server">
                                                    </SweetSoft:CustomExtraTextbox>
                                                </div>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-CssClass="column-one" HeaderStyle-CssClass="checkbox-column">
                                            <HeaderTemplate>
                                                <input type="checkbox" id="chkSelectAll" class="uniform" value="" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsDelete" CssClass="uniform"
                                                    runat="server"></asp:CheckBox>
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
                                    </Columns>
                                    <EmptyDataTemplate>
                                        There are currently no items in this table.
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="tab-pane" id="shipping">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-6 col-sm-6">
                        <div class="form">
                            <div class="form-group">
                                <label class="control-label">
                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CUSTOMER_SALES_RECORDS)%>
                                </label>
                                <asp:TextBox ID="txtSaleRecords" runat="server" TextMode="MultiLine" Rows="10" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label class="control-label">
                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CUSTOMER_PACKAGE_REQUIREMENTS)%>
                                </label>
                                <asp:TextBox ID="txtPackingRequirement" runat="server" TextMode="MultiLine" Rows="7" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 col-sm-6">
                        <div class="form">
                            <div class="form-group">
                                <label class="control-label">
                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CUSTOMER_TECH_DATA)%>
                                </label>
                                <asp:TextBox ID="txtTechData" runat="server" TextMode="MultiLine" Rows="5" CssClass="form-control"></asp:TextBox>
                            </div>

                            <div class="form-group">
                                <label class="control-label">
                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CUSTOMER_FORWARD_NAME)%>
                                </label>
                                <asp:TextBox ID="txtForwarderName" runat="server" TextMode="MultiLine" Rows="5" CssClass="form-control"></asp:TextBox>
                            </div>

                            <div class="form-group">
                                <label class="control-label">
                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CUSTOMER_FORWARD_ADDRESS)%>
                                </label>
                                <asp:TextBox ID="txtForwarderAddress" runat="server" TextMode="MultiLine" Rows="5" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptPlaceHolder" runat="server">
    <script>
        addRequestHanlde(InitCheckAll);
        function InitCheckAll() {
            $("#chkSelectAll").change(function () {
                var isChecked = $(this).is(':checked');
                var checkboxother = $(this).closest('table').find('tbody tr td input[type="checkbox"]:not(:disabled)');
                //checkboxother.trigger('click');
                if (isChecked) {
                    checkboxother.prop('checked', true).trigger('change');
                }
                else
                    checkboxother.each(function () {
                        $(this).prop('checked', false).trigger('change');
                    });
            });
        }
        InitCheckAll();
    </script>
</asp:Content>
