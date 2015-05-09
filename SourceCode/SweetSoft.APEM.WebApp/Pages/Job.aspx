<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master" AutoEventWireup="true" CodeBehind="Job.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.Job" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .column-one
        {
            width: auto !important;
            /*max-width:40px !important;*/
        }

        .total
        {
            text-align: right !important;
        }

        .checkbox-column
        {
            width: 51px !important;
        }

        .delete
        {
            width: 51px !important;
            text-align: center;
        }

        div.uniform-checker
        {
            margin-right: 0 !important;
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
                        <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" CssClass="btn btn-transparent">
                                <span class="flaticon-delete41"></span>
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.DELETE)%>
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnEngraving" runat="server"
                            CssClass="btn btn-transparent" OnClick="btnEngraving_Click">
                                <span class="glyphicon glyphicon-wrench" style="font-size: 19px; line-height: 28px; vertical-align: -3px;"></span>
                                Engraving
                        </asp:LinkButton>
                        <asp:Literal Text="" ID="ltrPrint" runat="server" />
                        <asp:LinkButton ID="btnSaveRevision" runat="server"
                            CssClass="btn btn-transparent" OnClick="btnSaveRevision_Click">
                                <span class="flaticon-duplicate3"></span>
                                Save Revision
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnGetCopy" runat="server"
                            CssClass="btn btn-transparent" OnClick="btnGetCopy_Click">
                                <span class="flaticon-files6"></span>
                                Get copy
                        </asp:LinkButton>
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
                                    <SweetSoft:ExtraInputMask ID="txtCode" RenderOnlyInput="true" runat="server" Enabled="false">
                                    </SweetSoft:ExtraInputMask>
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
                <div class="col-sm-8">
                    <div class="form-group" style="margin-bottom: 0">
                        <label class="control-label">Job Nr</label>
                        <SweetSoft:CustomExtraTextbox ID="txtJobNumber" RenderOnlyInput="true"
                            runat="server" Enabled="false"></SweetSoft:CustomExtraTextbox>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="control-label">Rev</label>
                        <asp:DropDownList ID="ddlRevNumber" runat="server"
                            data-style="btn btn-info"
                            data-width="100%" Required="true" AutoPostBack="true"
                            data-toggle="dropdown" OnSelectedIndexChanged="ddlRevNumber_SelectedIndexChanged"
                            CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6 col-sm-6">
            <div class="form-group">
                <div class="row">
                    <div class="col-md-12">
                        <label class="control-label">Job name</label>
                        <SweetSoft:CustomExtraTextbox ID="txtJobName"
                            RenderOnlyInput="true" runat="server"></SweetSoft:CustomExtraTextbox>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-sm-6 col-md-6">
            <div class="row">
                <div class="col-md-4 col-sm-4">
                    <div class="form-group">
                        <label class="control-label">Type of Job</label>
                        <asp:DropDownList ID="ddlTypeOfOrder" runat="server"
                            data-style="btn btn-info"
                            data-width="100%" Required="true" data-toggle="dropdown"
                            CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-2 col-sm-2">
                    <div class="form-group">
                        <label class="control-label">Currency</label>
                        <asp:DropDownList ID="ddlCurrency" runat="server"
                            data-style="btn btn-info" AutoPostBack="true"
                            data-width="100%" Required="true"
                            data-toggle="dropdown" data-live-search="true"
                            CssClass="form-control" OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-sm-6 col-md-6">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="control-label">Product type</label>
                                <asp:DropDownList ID="ddlMainProductType" runat="server"
                                    data-style="btn btn-info" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlMainProductType_SelectedIndexChanged"
                                    data-width="100%" Required="true" data-toggle="dropdown"
                                    CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-9 col-sm-9">
            <div class="form-group">
                <div class="row">
                    <div class="col-md-8">
                        <label class="control-label">Design</label>
                        <SweetSoft:CustomExtraTextbox ID="txtDesign"
                            RenderOnlyInput="true" runat="server"></SweetSoft:CustomExtraTextbox>
                    </div>
                    <div class="col-md-4">
                        <label class="control-label">Drawing Number</label>
                        <SweetSoft:CustomExtraTextbox ID="txtDrawing"
                            RenderOnlyInput="true" runat="server"></SweetSoft:CustomExtraTextbox>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-3 col-sm-3">
            <div class="form-group">
                <label class="control-label">Item code</label>
                <asp:TextBox ID="txtItemCode" runat="server"
                    CssClass="form-control">
                </asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 tabbable">
            <ul class="nav nav-tabs" role="tablist">
                <li class="active"><a href="#detail" role="tab" data-toggle="tab">Detail</a></li>
                <li><a href="#JobSheet" role="tab" data-toggle="tab">Job sheet</a></li>
                <%--<li><a href="#ServiceJob" role="tab" data-toggle="tab">Additional Services</a></li>--%>
                <li><a href="#OtherCharges" role="tab" data-toggle="tab">
                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.OTHER_CHARGES)%>
                </a></li>
            </ul>
        </div>
    </div>
    <div class="tab-content">
        <div class="tab-pane active" id="detail">
            <div class="container-fluid" style="padding-bottom: 20px;">
                <div class="row">
                    <div class="col-md-9">
                        <div class="row">
                            <div class="col-md-8 col-sm-8">
                                <div class="row">
                                    <div class="col-md-6 col-sm-6">
                                        <div class="form-group">
                                            <div class="row">
                                                <div class="form-group">
                                                    <div class="col-md-8 col-sm-8">
                                                        <label class="control-label">Root job Nr</label>
                                                        <SweetSoft:CustomExtraTextbox ID="txtRootJobNumber"
                                                            RenderOnlyInput="true" runat="server"></SweetSoft:CustomExtraTextbox>
                                                    </div>
                                                    <div class="col-md-4 col-sm-4">
                                                        <label class="control-label">R</label>
                                                        <SweetSoft:CustomExtraTextbox ID="txtRootJobRevNumber"
                                                            RenderOnlyInput="true" runat="server"></SweetSoft:CustomExtraTextbox>
                                                    </div>
                                                    <asp:HiddenField ID="hRootJobID" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6 col-sm-6">
                                        <div class="form-group">
                                            <label class="control-label">
                                                Common Job Nr
                                            </label>
                                            <SweetSoft:CustomExtraTextbox ID="txtCommonJobNumber"
                                                RenderOnlyInput="true" runat="server"></SweetSoft:CustomExtraTextbox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4 col-sm-4">
                                        <div class="row">
                                            <div class="col-md-12 com-sm-12">
                                                <label class="control-label">Contact person</label>
                                                <asp:DropDownList ID="ddlContacts" runat="server" AutoPostBack="false"
                                                    data-style="btn btn-info"
                                                    data-width="100%" Required="true"
                                                    data-toggle="dropdown"
                                                    CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-sm-4">
                                        <div class="row">
                                            <div class="col-md-12 com-sm-12">
                                                <label class="control-label">Sales Rep</label>
                                                <asp:DropDownList ID="ddlSaleRep" runat="server"
                                                    data-style="btn btn-info"
                                                    data-width="100%" Required="true"
                                                    data-toggle="dropdown" data-live-search="true"
                                                    CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-sm-4">
                                        <div class="form-group">
                                            <label class="control-label">Job Coordinator</label>
                                            <asp:DropDownList ID="ddlJobCoordinator" runat="server"
                                                data-style="btn btn-info"
                                                data-width="100%" Required="true"
                                                data-toggle="dropdown"
                                                CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="control-label">Customer P/O1</label>
                                            <SweetSoft:CustomExtraTextbox ID="txtCustomerPO1"
                                                RenderOnlyInput="true" runat="server"></SweetSoft:CustomExtraTextbox>
                                        </div>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="control-label">Customer P/O2</label>
                                            <SweetSoft:CustomExtraTextbox ID="txtCustomerPO2"
                                                RenderOnlyInput="true" runat="server"></SweetSoft:CustomExtraTextbox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form-group">
                            <label class="control-label">Job Barcode</label>
                            <div>
                                <asp:Image ID="imgBarcode" CssClass="visible-xs-inline visible-sm-inline visible-md-inline visible-lg-inline img-responsive"
                                    runat="server" AlternateText="Barcode" />
                                <asp:HiddenField ID="hBarcode" runat="server" />
                                <asp:HiddenField ID="hRevisionRootJob" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6 col-sm-6">
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
                    <div class="col-md-6 col-sm-6">
                        <div class="row">
                            <div class="col-md-6 col-sm-6">
                                <label class="control-label">
                                    Date created
                                </label>
                                <div class="wrap-datepicker">
                                    <SweetSoft:CustomExtraTextbox ID="txtCreatedDate" runat="server"
                                        RenderOnlyInput="true" data-format="dd-MM-yyyy" Enabled="false"
                                        CssClass="datepicker form-control mask-date">
                                    </SweetSoft:CustomExtraTextbox>
                                    <span class="fa fa-calendar in-mask-date"></span>
                                </div>
                            </div>
                            <div class="col-md-6 col-sm-6">
                                <label class="control-label">
                                    Created by
                                </label>
                                <SweetSoft:CustomExtraTextbox ID="txtCreatedBy" Enabled="false"
                                    RenderOnlyInput="true" runat="server"></SweetSoft:CustomExtraTextbox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6 col-sm-6">
                        <div class="form-group">
                            <label class="control-label">Ship to Party</label>
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="form-horizontal">
                                        <div class="form-group" style="margin-bottom: 0">
                                            <div class="col-sm-3 col-xs-3">
                                                <SweetSoft:ExtraInputMask ID="txtShipToPartyCode" RenderOnlyInput="true" runat="server" Enabled="false"></SweetSoft:ExtraInputMask>
                                            </div>
                                            <div class="col-sm-9 col-xs-9">
                                                <SweetSoft:CustomExtraTextbox ID="txtShipToParty" RenderOnlyInput="true"
                                                    runat="server"></SweetSoft:CustomExtraTextbox>
                                            </div>
                                            <asp:HiddenField ID="hdShipToPartyID" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 col-sm-6">
                        <div class="form-group">
                            <label class="control-label">Address</label>
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="form-horizontal">
                                        <div class="form-group" style="margin-bottom: 0">
                                        </div>
                                    </div>
                                    <asp:Label ID="lbShipToPartyAddress" runat="server">abc</asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6 col-sm-6">
                        <label class="control-label">Status</label>
                        <asp:DropDownList ID="ddlStatus" runat="server"
                            data-style="btn btn-info"
                            data-width="100%" Required="true"
                            data-toggle="dropdown"
                            CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-6 col-sm-6">
                        <label class="control-label">Outsource</label>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div class="col-md-3 col-sm-3">
                                    <div class="checkbox" style="margin-left: -20px;">
                                        <label>
                                            <asp:CheckBox ID="chkIsOutsource" runat="server" CssClass="uniform" AutoPostBack="true" OnCheckedChanged="chkIsOutsource_CheckedChanged" />&nbsp;Yes
                                        </label>
                                    </div>
                                </div>
                                <div class="col-md-9 col-sm-9">
                                    <asp:DropDownList ID="ddlSupplier" runat="server"
                                        data-style="btn btn-info"
                                        data-width="100%" Required="true"
                                        data-toggle="dropdown"
                                        CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-8 col-sm-8">
                        <div class="form-group">
                            <label class="control-label">Job Remark</label>
                            <asp:TextBox ID="txtJobRemark" TextMode="Multiline"
                                Rows="8" runat="server" class="form-control">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-4">
                        <div class="form-group">
                            <label class="control-label">Internal/External</label>
                            <asp:DropDownList ID="ddlInExternal" runat="server"
                                data-style="btn btn-info"
                                data-width="100%" Required="true"
                                data-toggle="dropdown"
                                CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <label class="control-label">Revision detail</label>
                            <asp:TextBox ID="txtViewRevisionDetail" TextMode="Multiline"
                                Rows="4" runat="server" class="form-control">
                            </asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <label class="control-label">Revision history</label>
                    </div>
                    <div class="col-md-12">
                        <asp:GridView ID="grvRevisionHistory" runat="server"
                            AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-checkable dataTable"
                            GridLines="None" AllowPaging="false" AllowSorting="false" DataKeyNames="JobID">
                            <Columns>
                                <asp:TemplateField HeaderText="RevNumber">
                                    <ItemTemplate>
                                        <asp:Label ID="lbRevNumber" runat="server"
                                            Text='<%#Eval("RevNumber")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="RootRevNumber">
                                    <ItemTemplate>
                                        <asp:Label ID="lbRootRevNumber" runat="server"
                                            Text='<%#Eval("RootRevNumber")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CreatedBy">
                                    <ItemTemplate>
                                        <asp:Label ID="lbCreatedBy" runat="server"
                                            Text='<%#Eval("CreatedBy")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CreatedDate">
                                    <ItemTemplate>
                                        <asp:Label ID="lbCreatedOn" runat="server"
                                            Text='<%#Eval("CreatedOn")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="InternalExternal">
                                    <ItemTemplate>
                                        <asp:Label ID="lbInternalExternal" runat="server"
                                            Text='<%#Eval("InternalExternal")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="RevisionDetail">
                                    <ItemTemplate>
                                        <asp:Label ID="lbRevisionDetail" runat="server"
                                            Text='<%#Eval("RevisionDetail")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
        <div class="tab-pane" id="JobSheet">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-sm-12">
                        <div class="form-group">
                            <div class="row">
                                <div class="col-sm-4">
                                    <label class="control-label">Repro Operator</label>
                                    <SweetSoft:CustomExtraTextbox ID="txtReproOperator" RenderOnlyInput="true"
                                        runat="server"></SweetSoft:CustomExtraTextbox>
                                </div>
                                <div class="col-sm-8">
                                    <div class="row">
                                        <div class="col-sm-3">
                                            <div class="form-group">
                                                <label class="control-label">Circumference</label>
                                                <SweetSoft:ExtraInputMask ID="txtCircumference" RenderOnlyInput="true"
                                                    MaskType="Numeric" GroupSeparator="," AutoGroup="true" Digits="2"
                                                    PlaceholderMask="0" runat="server" onkeyup="ChangeCircumference();"></SweetSoft:ExtraInputMask>
                                                <asp:HiddenField ID="hPiValue" runat="server" Value="3.1416" />
                                            </div>
                                        </div>
                                        <div class="col-sm-3">
                                            <div class="form-group">
                                                <label class="control-label">Face width</label>
                                                <SweetSoft:ExtraInputMask ID="txtFaceWidth" RenderOnlyInput="true"
                                                    MaskType="Numeric" GroupSeparator="," AutoGroup="true" Digits="2"
                                                    PlaceholderMask="0" runat="server"></SweetSoft:ExtraInputMask>
                                            </div>
                                        </div>
                                        <div class="col-sm-3">
                                            <div class="form-group">
                                                <label class="control-label">Diameter Nom</label>
                                                <SweetSoft:ExtraInputMask ID="txtDiameter" RenderOnlyInput="true"
                                                    MaskType="Numeric" GroupSeparator="," AutoGroup="true" Digits="2"
                                                    PlaceholderMask="0" runat="server"></SweetSoft:ExtraInputMask>
                                            </div>
                                        </div>
                                        <div class="col-sm-3">
                                            <div class="form-group">
                                                <label class="control-label">Diameter Diff</label>
                                                <SweetSoft:ExtraInputMask ID="txtDiameterDiff" RenderOnlyInput="true"
                                                    MaskType="Numeric" GroupSeparator="," AutoGroup="true" Digits="2"
                                                    PlaceholderMask="0" runat="server"></SweetSoft:ExtraInputMask>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-12">
                        <div class="tabbable2nd">
                            <ul class="nav nav-tabs" role="tablist">
                                <li class="active"><a href="#parameter" role="tab" data-toggle="tab">Parameter</a></li>
                                <li><a href="#Delivery" role="tab" data-toggle="tab">Delivery</a></li>
                                <li><a href="#Repro" role="tab" data-toggle="tab">Repro</a></li>
                                <li><a href="#ProofingandST" role="tab" data-toggle="tab">Cylinder, Proofing and S + T</a></li>
                            </ul>
                            <div class="tab-content" style="margin-bottom: 15px; padding: 15px; background: #f8f8f8">
                                <div id="parameter" class="tab-pane active">
                                    <div id="tablesContacts" class="dataTables_wrapper form-inline no-footer">
                                        <div class="dt_header">
                                            <div class="row">
                                                <div class="col-md-12 col-sm-12">
                                                    <div class="pull-left">
                                                        <asp:Label ID="lbCylsError" runat="server" Text=""></asp:Label>
                                                    </div>
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
                                                        <a href="javascript:;" runat="server" id="btnPrintCylinder" class="btn btn-default btn-sm DTTT_button_print">
                                                            <span><i class="icol-printer"></i>Print</span>
                                                        </a>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="table-responsive">
                                            <asp:GridView ID="grvCylinders" runat="server" AutoGenerateColumns="false"
                                                CssClass="table table-striped table-bordered table-checkable dataTable" GridLines="None"
                                                AllowPaging="false" AllowSorting="false" DataKeyNames="CylinderID, SteelBase, PricingID, Protocol, CylinderStatusID, Dept"
                                                OnRowCommand="grvCylinders_RowCommand" OnRowDataBound="grvCylinders_RowDataBound"
                                                OnRowDeleting="grvCylinders_RowDeleting"
                                                OnRowEditing="grvCylinders_RowEditing"
                                                OnRowUpdating="grvCylinders_RowUpdating"
                                                OnRowCancelingEdit="grvCylinders_RowCancelingEdit">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Seq" HeaderStyle-CssClass="column-60" ItemStyle-CssClass="column-60">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbSequence" runat="server"
                                                                Text='<%#Eval("Sequence")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <div style="position: relative;">
                                                                <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtSequence"
                                                                    Text='<%#Eval("Sequence")%>' Width="100%"
                                                                    CssClass="form-control" runat="server">
                                                                </SweetSoft:CustomExtraTextbox>
                                                            </div>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Barcode" HeaderStyle-CssClass="column-150" ItemStyle-CssClass="column-150">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnBarcode" runat="server" Text='<%#Eval("CylinderBarcode")%>'
                                                                CommandArgument="CylinderID" CommandName="Edit"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cylinder No." HeaderStyle-CssClass="column-150" ItemStyle-CssClass="column-150">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbCylinderNo" runat="server"
                                                                Text='<%#Eval("CylinderNo")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cus. Cyl. No" HeaderStyle-CssClass="column-250" ItemStyle-CssClass="column-250">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbCusCylinderID" runat="server"
                                                                Text='<%#Eval("CusCylinderID")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <div style="position: relative;">
                                                                <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtCusCylinderID"
                                                                    Text='<%#Eval("CusCylinderID")%>' Width="100%"
                                                                    CssClass="form-control" runat="server">
                                                                </SweetSoft:CustomExtraTextbox>
                                                            </div>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cus. Steelbase ID" HeaderStyle-CssClass="column-250" ItemStyle-CssClass="column-250">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbCusSteelBaseID" runat="server"
                                                                Text='<%#Eval("CusSteelBaseID")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <div style="position: relative;">
                                                                <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtCusSteelBaseID"
                                                                    Text='<%#Eval("CusSteelBaseID")%>' Width="100%"
                                                                    CssClass="form-control" runat="server">
                                                                </SweetSoft:CustomExtraTextbox>
                                                            </div>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="SteelBase" HeaderStyle-CssClass="column-100" ItemStyle-CssClass="column-100">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbSteelBase" runat="server"
                                                                Text='<%#(byte)Eval("SteelBase") == 1 ? "New" : "Old"%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <div style="position: relative;">
                                                                <asp:DropDownList ID="ddlSteelBase" runat="server"
                                                                    data-style="btn btn-info" data-width="100%" Required="true"
                                                                    data-container="body"
                                                                    data-toggle="dropdown" CssClass="form-control">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Color" HeaderStyle-CssClass="column-180" ItemStyle-CssClass="column-180">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbColor" runat="server"
                                                                Text='<%#Eval("Color")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <div style="position: relative;">
                                                                <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtColor"
                                                                    Text='<%#Eval("Color")%>' Width="100%"
                                                                    CssClass="form-control" runat="server">
                                                                </SweetSoft:CustomExtraTextbox>
                                                            </div>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="column-150" ItemStyle-CssClass="column-150">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbStatus" runat="server"
                                                                Text='<%#Eval("CylinderStatusName")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <div style="position: relative;">
                                                                <asp:DropDownList ID="ddlStatus" runat="server"
                                                                    data-style="btn btn-info" data-width="100%" Required="true"
                                                                    data-container="body" AutoPostBack="true"
                                                                    OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged"
                                                                    data-toggle="dropdown" CssClass="form-control">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Engraving Protocol" HeaderStyle-CssClass="column-180" ItemStyle-CssClass="column-180">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbProtocol" runat="server"
                                                                Text='<%#Eval("Protocol")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <div style="position: relative;">
                                                                <asp:DropDownList ID="ddlProtocol" runat="server"
                                                                    data-style="btn btn-info" data-width="100%" Required="true"
                                                                    data-container="body"
                                                                    data-toggle="dropdown" CssClass="form-control">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Pricing" HeaderStyle-CssClass="column-150" ItemStyle-CssClass="column-150">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbPricing" runat="server"
                                                                Text='<%#Eval("PricingName")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <div style="position: relative;">
                                                                <asp:DropDownList ID="ddlPricing" runat="server"
                                                                    data-style="btn btn-info" data-width="100%" Required="true"
                                                                    data-container="body"
                                                                    data-toggle="dropdown" CssClass="form-control">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Type" HeaderStyle-CssClass="column-100" ItemStyle-CssClass="column-100">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbCylType" runat="server"
                                                                Text='<%#Eval("CylType")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Diameter" HeaderStyle-CssClass="column-120" ItemStyle-CssClass="column-120 text-right">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbDiameter" runat="server"
                                                                Text='<%# ((double)Eval("Dirameter")).ToString("N2") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <div style="position: relative;">
                                                                <SweetSoft:ExtraInputMask ID="txtDirameter" RenderOnlyInput="true" Required="false" 
                                                                    Width="100%" runat="server" MaskType="Decimal" GroupSeparator="," RadixPoint="." Text='<%#Eval("Dirameter")%>' 
                                                                    Digits="2" AutoGroup="true"></SweetSoft:ExtraInputMask>
                                                            </div>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Dept" HeaderStyle-CssClass="column-80" ItemStyle-CssClass="column-80 text-center">
                                                        <%--<ItemTemplate>
                                                            <asp:Label ID="lbDept" runat="server"
                                                                Text='<%#Eval("Dept")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <div style="position: relative;">
                                                                <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtDept"
                                                                    Text='<%#Eval("Dept")%>' Width="100%"
                                                                    CssClass="form-control" runat="server">
                                                                </SweetSoft:CustomExtraTextbox>
                                                            </div>
                                                        </EditItemTemplate>--%>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbDept" runat="server"
                                                                Text='<%#Eval("Dept")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <div style="position: relative;">
                                                                <asp:DropDownList ID="ddlDept" runat="server"
                                                                    data-style="btn btn-info" data-width="100%" 
                                                                    data-container="body" 
                                                                    data-toggle="dropdown" CssClass="form-control">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Pivot" ItemStyle-CssClass="text-center column-80" HeaderStyle-CssClass="column-80">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkIsPivot" CssClass="uniform" onclick="javascript:ChangeIsPivot(this);"
                                                                Checked='<%# Convert.ToBoolean(Eval("IsPivotCylinder")) %>'
                                                                runat="server"></asp:CheckBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-CssClass="column-100" ItemStyle-CssClass="column-100 text-center">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-primary"
                                                                CommandArgument='<%# Eval("CylinderID") %>' CommandName="Edit">
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
                                                    <asp:TemplateField ItemStyle-CssClass="column-one" HeaderStyle-CssClass="checkbox-column">
                                                        <HeaderTemplate>
                                                            <input type="checkbox" id="chkSelectAll" class="uniform" value="" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkIsDelete" CssClass="uniform print-or-delete"
                                                                runat="server"></asp:CheckBox>
                                                            <input type="hidden" value='<%#Eval("CylinderID") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    There are currently no items in this table.
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </div>
                                        <asp:LinkButton ID="btnResetPivot" runat="server" OnClick="btnResetPivot_Click"></asp:LinkButton>
                                        <asp:HiddenField ID="hSelectedCylID" runat="server" />
                                    </div>
                                </div>
                                <div id="Delivery" class="tab-pane">
                                    <div class="row">
                                        <div class="col-sm-4">
                                            <div class="panel panel-default delivery-panel">
                                                <div class="panel-heading">
                                                    Repro
                                                </div>
                                                <div class="panel-body">
                                                    <div class="form-group">
                                                        <div class="form-horizontal">
                                                            <div class="row">
                                                                <label class="control-label col-sm-2">Date</label>
                                                                <div class="col-sm-10">
                                                                    <div class="wrap-datepicker">
                                                                        <SweetSoft:CustomExtraTextbox ID="txtReproDate" runat="server"
                                                                            RenderOnlyInput="true" data-format="dd-MM-yyyy"
                                                                            CssClass="datepicker form-control mask-date">
                                                                        </SweetSoft:CustomExtraTextbox>
                                                                        <span class="fa fa-calendar in-mask-date"></span>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-horizontal">
                                                        <div class="form-group">
                                                            <div class="col-sm-6">
                                                                <div class="checkbox" style="margin-left: -20px;">
                                                                    <label>
                                                                        <asp:CheckBox ID="chkHasIrisProof" runat="server" CssClass="uniform" />
                                                                        Digital Proof
                                                                    </label>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <SweetSoft:ExtraInputMask ID="txtIrisProof" RenderOnlyInput="true"
                                                                    MaskType="Numeric" GroupSeparator="," AutoGroup="true" Digits="0"
                                                                    PlaceholderMask="0" runat="server"></SweetSoft:ExtraInputMask>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-8">
                                            <div class="panel panel-default delivery-panel">
                                                <div class="panel-heading">
                                                    Cylinder/Shipping
                                                </div>
                                                <div class="panel-body">
                                                    <div class="row">
                                                        <div class="form-group">
                                                            <div class="col-sm-8">
                                                                <div class="form-horizontal">
                                                                    <div class="form-group">
                                                                        <label class="control-label col-sm-2">
                                                                            <span class="pull-left">Date
                                                                            </span>
                                                                        </label>
                                                                        <div class="col-sm-8">
                                                                            <div class="wrap-datepicker">
                                                                                <SweetSoft:CustomExtraTextbox ID="txtCylinderDate" runat="server"
                                                                                    RenderOnlyInput="true" data-format="dd-MM-yyyy"
                                                                                    CssClass="datepicker form-control mask-date">
                                                                                </SweetSoft:CustomExtraTextbox>
                                                                                <span class="fa fa-calendar in-mask-date"></span>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-4">
                                                                <div class="checkbox" style="margin-left: -20px;">
                                                                    <label>
                                                                        <asp:CheckBox ID="chkPreApproval" runat="server" CssClass="uniform" />
                                                                        Pre-Approval
                                                                    </label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-4">
                                                            <div class="form-group" style="margin-left: -20px">
                                                                <div class="radio">
                                                                    <label>
                                                                        <asp:RadioButton ID="radLeavingAPEM" GroupName="LeavingAPEM" runat="server" CssClass="uniform" />
                                                                        Leaving APE
                                                                    </label>
                                                                </div>

                                                            </div>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <div class="form-group" style="margin-left: -20px">
                                                                <div class="radio">
                                                                    <label>
                                                                        <asp:RadioButton ID="radExpected" GroupName="LeavingAPEM" runat="server" CssClass="uniform" />
                                                                        Expected at client
                                                                    </label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-12">
                                                            <label class="control-label">Notes</label>
                                                            <asp:TextBox ID="txtDeilveryNotes" runat="server" CssClass="form-control"
                                                                TextMode="MultiLine" Rows="5"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div id="Repro" class="tab-pane">
                                    <div class="row">
                                        <div class="col-sm-4">
                                            <div class="panel panel-default">
                                                <div class="panel-body">
                                                    <div class="form-group" style="margin-left: -20px">
                                                        <div class="checkbox">
                                                            <label>
                                                                <asp:CheckBox ID="chkEyeMark" runat="server" CssClass="uniform" />
                                                                Eye Mark
                                                            </label>
                                                        </div>
                                                    </div>
                                                    <div class="form-group ">
                                                        <div class="form-horizontal">
                                                            <div class="form-group">
                                                                <label class="control-label col-sm-3">
                                                                    <span class="pull-left">Size</span>
                                                                </label>
                                                                <div class="col-sm-9">
                                                                    <div class="input-group">
                                                                        <SweetSoft:ExtraInputMask ID="txtEMWidth" RenderOnlyInput="true"
                                                                            MaskType="Numeric" GroupSeparator="," AutoGroup="true" Digits="0"
                                                                            PlaceholderMask="0" runat="server"></SweetSoft:ExtraInputMask>
                                                                        <span class="input-group-addon">x</span>
                                                                        <SweetSoft:ExtraInputMask ID="txtEMHeight" RenderOnlyInput="true"
                                                                            MaskType="Numeric" GroupSeparator="," AutoGroup="true" Digits="0"
                                                                            PlaceholderMask="0" runat="server"></SweetSoft:ExtraInputMask>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <div class="form-horizontal">
                                                            <div class="form-group">
                                                                <label class="control-label col-sm-3">
                                                                    <span class="pull-left">Colour</span>
                                                                </label>
                                                                <div class="col-sm-9">
                                                                    <SweetSoft:CustomExtraTextbox ID="txtEMColor" RenderOnlyInput="true"
                                                                        runat="server"></SweetSoft:CustomExtraTextbox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <div class="form-horizontal">
                                                            <div class="form-group">
                                                                <label class="control-label col-sm-3">
                                                                    <span class="pull-left">Backing</span>
                                                                </label>
                                                                <div class="col-sm-9">
                                                                    <asp:DropDownList ID="ddlBacking" runat="server"
                                                                        data-style="btn btn-info"
                                                                        data-width="100%" Required="true"
                                                                        data-toggle="dropdown"
                                                                        CssClass="form-control">
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="control-label">Eye Mark Position</label>
                                                        <div class="row">
                                                            <div class="col-sm-7">
                                                                <img id="imgViewEyeMark" src="/img/eye-mark/eye-mark-3.png"
                                                                    runat="server" alt="" class="img-responsive" />
                                                            </div>
                                                            <div class="col-sm-5">
                                                                <div class="form-group">
                                                                    <asp:LinkButton class="btn btn-block btn-default" ID="btnMoreEyeMark" runat="server">...</asp:LinkButton>
                                                                </div>
                                                                <div class="form-group">
                                                                    <asp:LinkButton class="btn btn-block btn-default" ID="btnClearEyeMark" runat="server">
                                                                        <span class="glyphicon glyphicon-remove"></span>
                                                                        Clear</asp:LinkButton>
                                                                </div>
                                                            </div>
                                                            <asp:HiddenField ID="hEyeMarkPosition" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <div class="panel panel-default">
                                                <div class="panel-body">
                                                    <div class="form-group" style="margin-left: -20px">
                                                        <div class="checkbox">
                                                            <label>
                                                                <asp:CheckBox ID="chkBarcode" runat="server" CssClass="uniform" />
                                                                Barcode
                                                            </label>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <div class="form-horizontal">
                                                            <div class="form-group">
                                                                <label class="control-label col-sm-3">
                                                                    <span class="pull-left">Size</span>
                                                                </label>
                                                                <div class="col-sm-9">
                                                                    <div class="input-group">
                                                                        <SweetSoft:ExtraInputMask ID="txtBarcodeSize" RenderOnlyInput="true"
                                                                            MaskType="Numeric" GroupSeparator="," AutoGroup="true" Digits="0"
                                                                            PlaceholderMask="0" runat="server"></SweetSoft:ExtraInputMask>
                                                                        <span class="input-group-addon">%</span>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <div class="form-horizontal">
                                                            <div class="form-group">
                                                                <label class="control-label col-sm-3">
                                                                    <span class="pull-left">Colour</span>
                                                                </label>
                                                                <div class="col-sm-9">
                                                                    <SweetSoft:CustomExtraTextbox ID="txtBarcodeColor" RenderOnlyInput="true"
                                                                        runat="server"></SweetSoft:CustomExtraTextbox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <div class="form-horizontal">
                                                            <div class="form-group">
                                                                <label class="control-label col-sm-3">
                                                                    <span class="pull-left">No</span>
                                                                </label>
                                                                <div class="col-sm-9">
                                                                    <SweetSoft:CustomExtraTextbox ID="txtBarcodeNo" RenderOnlyInput="true"
                                                                        runat="server"></SweetSoft:CustomExtraTextbox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <div class="form-horizontal">
                                                            <div class="form-group">
                                                                <label class="control-label col-sm-3">
                                                                    <span class="pull-left">Supply</span>
                                                                </label>
                                                                <div class="col-sm-9">
                                                                    <asp:DropDownList ID="ddlSupply" runat="server"
                                                                        data-style="btn btn-info"
                                                                        data-width="100%" Required="true"
                                                                        data-toggle="dropdown"
                                                                        CssClass="form-control">
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <div class="form-horizontal">
                                                            <div class="form-group">
                                                                <label class="control-label col-sm-3">
                                                                    <span class="pull-left">BWR</span>
                                                                </label>
                                                                <div class="col-sm-9">
                                                                    <SweetSoft:ExtraInputMask ID="txtBWR" RenderOnlyInput="true"
                                                                        MaskType="Numeric" GroupSeparator="," AutoGroup="true" Digits="2"
                                                                        PlaceholderMask="0" runat="server"></SweetSoft:ExtraInputMask>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <div class="row">
                                                            <div class="col-sm-5">
                                                                <label class="checkbox" style="font-weight: 300">
                                                                    <asp:CheckBox ID="chkTraps" runat="server" CssClass="uniform" />
                                                                    Traps
                                                                </label>
                                                            </div>
                                                            <div class="col-sm-7">
                                                                <div class="form-horizontal">
                                                                    <div class="form-group">
                                                                        <label class="control-label col-sm-4">Size</label>
                                                                        <div class="col-sm-8">
                                                                            <SweetSoft:ExtraInputMask ID="txtSize" RenderOnlyInput="true"
                                                                                MaskType="Numeric" GroupSeparator="," AutoGroup="true" Digits="3"
                                                                                PlaceholderMask="0.000" runat="server"></SweetSoft:ExtraInputMask>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <div class="panel panel-default">
                                                <div class="panel-body">
                                                    <div class="form-group">
                                                        <div class="row">
                                                            <div class="col-sm-6">
                                                                <label class="control-label">Unit Size V</label>
                                                                <SweetSoft:ExtraInputMask ID="txtUNSizeV" RenderOnlyInput="true"
                                                                    MaskType="Numeric" GroupSeparator="," AutoGroup="true" Digits="2"
                                                                    PlaceholderMask="0" runat="server"></SweetSoft:ExtraInputMask>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <label class="control-label">Unit Size H</label>
                                                                <SweetSoft:ExtraInputMask ID="txtUNSizeH" RenderOnlyInput="true"
                                                                    MaskType="Numeric" GroupSeparator="," AutoGroup="true" Digits="2"
                                                                    PlaceholderMask="0" runat="server"></SweetSoft:ExtraInputMask>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <div class="row">
                                                            <div class="col-sm-6">
                                                                <label class="checkbox" style="font-weight: 300">
                                                                    <asp:CheckBox ID="chkOpaqueInk" runat="server" CssClass="uniform" />
                                                                    Opaque Ink
                                                                </label>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <SweetSoft:ExtraInputMask ID="txtOpaqueInkRate" RenderOnlyInput="true"
                                                                    MaskType="Numeric" GroupSeparator="," AutoGroup="true" Digits="2"
                                                                    PlaceholderMask="0" runat="server"></SweetSoft:ExtraInputMask>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <div class="row">
                                                            <div class="col-sm-12">
                                                                <label class="checkbox" style="font-weight: 300">
                                                                    <asp:CheckBox ID="chkIsEndless" runat="server" CssClass="uniform" />
                                                                    Endless/Continuous
                                                                </label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="control-label">Printing Direction</label>
                                                        <div class="row">
                                                            <div class="col-sm-3" style="text-align: center">
                                                                <label class="radio" style="font-weight: 300">
                                                                    <asp:RadioButton ID="radPrintingDirectionU" runat="server" GroupName="PrintingDirection" CssClass="uniform" />
                                                                    U
                                                                </label>
                                                                <img src="/img/up.png" alt="" />
                                                            </div>
                                                            <div class="col-sm-3" style="text-align: center">
                                                                <label class="radio" style="font-weight: 300">
                                                                    <asp:RadioButton ID="radPrintingDirectionD" runat="server" GroupName="PrintingDirection" CssClass="uniform" />
                                                                    D
                                                                </label>
                                                                <img src="/img/down.png" alt="" />
                                                            </div>
                                                            <div class="col-sm-3" style="text-align: center">
                                                                <label class="radio" style="font-weight: 300">
                                                                    <asp:RadioButton ID="radPrintingDirectionL" runat="server" GroupName="PrintingDirection" CssClass="uniform" />
                                                                    L
                                                                </label>
                                                                <img src="/img/left.png" alt="" />
                                                            </div>
                                                            <div class="col-sm-3" style="text-align: center">
                                                                <label class="radio" style="font-weight: 300">
                                                                    <asp:RadioButton ID="radPrintingDirectionR" runat="server" GroupName="PrintingDirection" CssClass="uniform" />
                                                                    R
                                                                </label>
                                                                <img src="/img/right.png" alt="" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <label class="control-label">
                                                Color Target
                                            </label>
                                            <asp:TextBox ID="txtColorTarget" runat="server" TextMode="MultiLine"
                                                Rows="2" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div id="ProofingandST" class="tab-pane">
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div class="panel panel-default">
                                                <div class="panel-heading">Cylinder</div>
                                                <div class="panel-body">
                                                    <div class="row">
                                                        <div class="col-sm-6">
                                                            <div class="form-horizontal">
                                                                <div class="form-group">
                                                                    <label class="control-label col-sm-4">
                                                                        <span class="pull-left">Type of Cylinder</span>
                                                                    </label>
                                                                    <div class="col-sm-6">
                                                                        <asp:DropDownList ID="ddlTypeOfCylinder" runat="server"
                                                                            data-style="btn btn-info"
                                                                            data-width="100%" Required="true"
                                                                            data-toggle="dropdown"
                                                                            CssClass="form-control">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <div class="form-horizontal">
                                                                <div class="form-group">
                                                                    <label class="control-label col-sm-4">
                                                                        <span class="pull-left">Printing</span>
                                                                    </label>
                                                                    <div class="col-sm-6">
                                                                        <asp:DropDownList ID="ddlPrinting" runat="server"
                                                                            data-style="btn btn-info"
                                                                            data-width="100%" Required="true"
                                                                            data-toggle="dropdown"
                                                                            CssClass="form-control">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-12">
                                            <div class="panel panel-default">
                                                <div class="panel-heading">Proofing</div>
                                                <div class="panel-body">
                                                    <div class="row">
                                                        <div class="col-sm-4">
                                                            <label class="control-label">
                                                                Proofing Material
                                                            </label>
                                                            <SweetSoft:CustomExtraTextbox ID="txtProofingMaterial" RenderOnlyInput="true"
                                                                runat="server"></SweetSoft:CustomExtraTextbox>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <label class="control-label">
                                                                Actual Printing Material
                                                            </label>
                                                            <SweetSoft:CustomExtraTextbox ID="txtActualPrintingMaterial" RenderOnlyInput="true"
                                                                runat="server"></SweetSoft:CustomExtraTextbox>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <label class="control-label">
                                                                Material Width
                                                            </label>
                                                            <SweetSoft:CustomExtraTextbox ID="txtMaterialWidth" RenderOnlyInput="true"
                                                                runat="server"></SweetSoft:CustomExtraTextbox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-12">
                                            <div class="panel panel-default">
                                                <div class="panel-heading">Step & Repeat</div>
                                                <div class="panel-body">
                                                    <div class="row">
                                                        <div class="col-sm-5">
                                                            <div class="form-horizontal">
                                                                <div class="form-group">
                                                                    <label class="control-label col-sm-6">
                                                                        <span class="pull-left">No of repeats Horizontal</span>
                                                                    </label>
                                                                    <div class="col-sm-6">
                                                                        <SweetSoft:ExtraInputMask ID="txtNumberOfRepeatH" RenderOnlyInput="true"
                                                                            MaskType="Numeric" GroupSeparator="," AutoGroup="true" Digits="0"
                                                                            PlaceholderMask="0" runat="server"></SweetSoft:ExtraInputMask>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <div class="form-horizontal">
                                                                <div class="form-group">
                                                                    <label class="control-label col-sm-6">
                                                                        <span class="pull-left">No of repeats Vertical</span>
                                                                    </label>
                                                                    <div class="col-sm-6">
                                                                        <SweetSoft:ExtraInputMask ID="txtNumberOfRepeatV" RenderOnlyInput="true"
                                                                            MaskType="Numeric" GroupSeparator="," AutoGroup="true" Digits="0"
                                                                            PlaceholderMask="0" runat="server"></SweetSoft:ExtraInputMask>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-12">
                                                            <label class="control-label">Remarks</label>
                                                            <asp:TextBox ID="txtSRRemark" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
        <%--<div class="tab-pane" id="ServiceJob">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <label class="control-label DTTT_button_save">
                                Additional Services
                            </label>
                            <div class="dataTables_wrapper form-inline no-footer">
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
                                        <asp:TemplateField HeaderText="Seq">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNo" Text="1" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Work No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWorkOrderNumber" runat="server"
                                                    Text='<%#Eval("WorkOrderNumber")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" ID="txtWorkOrderNumber" Text='<%#Eval("WorkOrderNumber") %>' 
                                                    CssClass="form-control" Width="100%"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-CssClass="column-large" HeaderText="Product ID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProductID" runat="server"
                                                    Text='<%#Eval("ProductID")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" ID="txtProductID" Text='<%#Eval("ProductID") %>' 
                                                    CssClass="form-control" Width="100%"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="GLCode" FooterStyle-CssClass="total" ItemStyle-CssClass="column-one">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGLCode" runat="server"
                                                    Text='<%#Eval("GLCode")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" ID="txtGLCode" Text='<%#Eval("GLCode") %>' 
                                                    CssClass="form-control" Width="100%"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description" FooterStyle-CssClass="total" ItemStyle-CssClass="column-one">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDescription" runat="server"
                                                    Text='<%#Eval("Description")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" ID="txtDescription" Text='<%#Eval("Description") %>' 
                                                    CssClass="form-control" Width="100%"></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalText" Text='<%# SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.TOTAL)%>'></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Pricing">
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
                                        <asp:TemplateField ItemStyle-CssClass="column-one" HeaderStyle-CssClass="checkbox-column"">
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
                                                    Text="Cancel"><span class="fa fa-ban"></span>
                                                </asp:LinkButton>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-CssClass="column-one">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-primary"
                                                    CommandName="delete" CommandArgument='<%#Eval("ServiceJobID") %>' Text="Delete">
                                                    <span class="fa fa-remove"></span>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>                            
                        </div>
                    </div>
                </div>
            </div>
        </div>--%>
        <div class="tab-pane" id="OtherCharges">
            <div class="container-fluid">
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
            </div>
        </div>
        <%--EyeMark Modal---%>
        <div id="dialog-form-eye-mark" title="Eye Mark">
            <div class="container-fluid" style="margin-top: 15px;">
                <div class="row">
                    <div class="col-sm-4 form-group">
                        <img src="/img/eye-mark/eye-mark-1.png" toggle-value="1" class="img-responsive btn btn-default" alt="" />
                    </div>
                    <div class="col-sm-4 form-group">
                        <img src="/img/eye-mark/eye-mark-2.png" toggle-value="2" class="img-responsive btn btn-default" alt="" />
                    </div>
                    <div class="col-sm-4 form-group">
                        <img src="/img/eye-mark/eye-mark-3.png" toggle-value="3" class="img-responsive btn btn-default" alt="" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-4 form-group">
                        <img src="/img/eye-mark/eye-mark-4.png" toggle-value="4" class="img-responsive btn btn-default" alt="" />
                    </div>
                    <div class="col-sm-4 form-group">
                        <img src="/img/eye-mark/eye-mark-5.png" toggle-value="5" class="img-responsive btn btn-default" alt="" />
                    </div>
                    <div class="col-sm-4 form-group">
                        <img src="/img/eye-mark/eye-mark-6.png" toggle-value="6" class="img-responsive btn btn-default" alt="" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-4 form-group">
                        <img src="/img/eye-mark/eye-mark-7.png" toggle-value="7" class="img-responsive btn btn-default" alt="" />
                    </div>
                    <div class="col-sm-4 form-group">
                        <img src="/img/eye-mark/eye-mark-8.png" toggle-value="8" class="img-responsive btn btn-default" alt="" />
                    </div>
                    <div class="col-sm-4 form-group">
                        <img src="/img/eye-mark/eye-mark-9.png" toggle-value="9" class="img-responsive btn btn-default" alt="" />
                    </div>
                </div>
            </div>
        </div>
        <%--EyeMark Modal---%>
    </div>
</asp:Content>
<asp:Content ID="ModalContent" ContentPlaceHolderID="ModalPlaceHolder" runat="server">
    <!-- Comfirm Revision Starts here-->
    <div id="dialog-form-revision" title="Revision">
        <asp:UpdatePanel runat="server" ID="upnlRevisions">
            <ContentTemplate>
                <div class="container-fluid">
                    <div class="row" style="background: #5bc0de">
                        <div class="col-md-6 col-sm-6">
                            <div class="form-inline">
                                <div class="form-group">
                                    <asp:LinkButton ID="btnSaveRevDetail" runat="server"
                                        CssClass="btn btn-transparent" OnClick="btnSaveRevDetail_Click">
                                                    <span class="flaticon-floppy1"></span>
                                                    Save
                                    </asp:LinkButton>
                                </div>
                                <div class="form-group">
                                    <asp:LinkButton runat="server" ID="btnRevisionCancel"
                                        CssClass="btn btn-transparent" OnClientClick="CloseRevision(); return false;">
                                                    <span class="flaticon-back57"></span>
                                                    Close
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row" style="margin-top: 15px;">
                        <div class="col-sm-12">
                            <div class="form-group">
                                <div class="form-horizontal">
                                    <div class="row">
                                        <div class="col-md-9 col-sm-9">
                                            <div class="form-horizontal">
                                                <div class="form-group">
                                                    <label class="control-label col-sm-2">
                                                        <span class="pull-left">Cust name</span>
                                                    </label>
                                                    <div class="col-sm-10">
                                                        <SweetSoft:CustomExtraTextbox ID="txtRevisionCustomerName" RenderOnlyInput="true"
                                                            Enabled="false" runat="server"></SweetSoft:CustomExtraTextbox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-3 col-sm-3">
                                            <div class="form-horizontal">
                                                <div class="form-group">
                                                    <label class="control-label col-sm-3">
                                                        <span class="pull-left">Rev</span>
                                                    </label>
                                                    <div class="col-sm-9">
                                                        <SweetSoft:CustomExtraTextbox ID="txtRevisionNumber" RenderOnlyInput="true"
                                                            Enabled="false" runat="server"></SweetSoft:CustomExtraTextbox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6 col-sm-6">
                                            <div class="form-horizontal">
                                                <div class="form-group">
                                                    <label class="control-label col-sm-3">
                                                        <span class="pull-left">Job Nr</span>
                                                    </label>
                                                    <div class="col-sm-9">
                                                        <SweetSoft:CustomExtraTextbox ID="txtRevisionJobNumber" RenderOnlyInput="true"
                                                            Enabled="false" runat="server"></SweetSoft:CustomExtraTextbox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6 col-sm-6">
                                            <div class="form-horizontal">
                                                <div class="form-group">
                                                    <div class="col-sm-6">
                                                        <asp:DropDownList ID="ddlIsInternal" runat="server"
                                                            data-style="btn btn-info"
                                                            data-width="100%" Required="true"
                                                            data-toggle="dropdown"
                                                            CssClass="form-control">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-9 col-sm-9">
                                            <div class="form-horizontal">
                                                <div class="form-group">
                                                    <label class="control-label col-sm-2">
                                                        <span class="pull-left">Remark</span>
                                                    </label>
                                                    <div class="col-sm-10">
                                                        <SweetSoft:CustomExtraTextbox ID="txtRevisionDetail" RenderOnlyInput="true"
                                                            runat="server"></SweetSoft:CustomExtraTextbox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label">
                                    Job sheets
                                </label>
                                <asp:GridView ID="grvRevisionDetail" runat="server"
                                    AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-checkable dataTable"
                                    GridLines="None" AllowPaging="false" AllowSorting="false" DataKeyNames="JobID">
                                    <Columns>
                                        <asp:TemplateField HeaderText="RevNumber">
                                            <ItemTemplate>
                                                <asp:Label ID="lbRevNumber" runat="server"
                                                    Text='<%#Eval("RevNumber")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CreatedDate">
                                            <ItemTemplate>
                                                <asp:Label ID="lbCreatedOn" runat="server"
                                                    Text='<%#Eval("CreatedOn")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="RevisionDetail">
                                            <ItemTemplate>
                                                <asp:Label ID="lbRevisionDetail" runat="server"
                                                    Text='<%#Eval("RevisionDetail")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="form-group">
                                <label class="control-label">
                                    Cylinders
                                </label>
                                <asp:GridView ID="grvCylinderView" runat="server" AutoGenerateColumns="false"
                                    CssClass="table table-striped table-bordered table-checkable dataTable" GridLines="None"
                                    AllowPaging="false" AllowSorting="false" DataKeyNames="CylinderID">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Seq" HeaderStyle-CssClass="sorting">
                                            <ItemTemplate>
                                                <asp:Label ID="lbSequence" runat="server"
                                                    Text='<%#Eval("Sequence")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CylinderNo" HeaderStyle-CssClass="sorting">
                                            <ItemTemplate>
                                                <asp:Label ID="lbCylinderNo" runat="server"
                                                    Text='<%#Eval("CylinderNo")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Color" HeaderStyle-CssClass="sorting">
                                            <ItemTemplate>
                                                <asp:Label ID="lbColor" runat="server"
                                                    Text='<%#Eval("Color")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Type" HeaderStyle-CssClass="sorting">
                                            <ItemTemplate>
                                                <asp:Label ID="lbCylType" runat="server"
                                                    Text='<%#Eval("CylType")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Dept" HeaderStyle-CssClass="sorting">
                                            <ItemTemplate>
                                                <asp:Label ID="lbDept" runat="server"
                                                    Text='<%#Eval("Dept")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        There are currently no items in this table.
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>
                            <div class="form-group" style="padding-top: 15px;">
                                <asp:Label Style="text-align: left; color: #31708f; font-weight: bold;" ID="lbMessage" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <!--Comfirm Revision Ends here -->

    <%--modal error of cylinder printer--%>
    <div class="modal fade" id="modalErrorPrint">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header alert alert-warning">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title" id="myModalLabel">
                        <span>Warning</span>
                    </h4>
                </div>
                <div class="modal-body">
                    No cylinder(s) selected
                </div>
                <div class="modal-footer">
                    <a class="btn btn-danger" data-dismiss="modal">Close</a>
                </div>
            </div>
        </div>
    </div>

    <div id="dialog-printing" title="Printing" style="background: #fff">
        <iframe src="" frameborder="0" width="100%" height="100%" style="min-height: 500px"></iframe>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptPlaceHolder" runat="server">
    <script src="/js/plugins/printThis.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#dialog-printing').hide();
            SearchText();
            SearchShipToParty();
            CreateEyemark();
            CreateRevision();
            RegisterButton();

            ChooseCylinderPrinter();
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

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(RegisterButton);
        function RegisterButton() {
            $('#<%= btnMoreEyeMark.ClientID %>').click(function () {
                $("#dialog-form-eye-mark").dialog("open");
                return false;
            });

            $('#<%= btnClearEyeMark.ClientID %>').click(function () {
                $('img[id$="imgViewEyeMark"]').attr('src', '');
                $("input[type='hidden'][id$='hEyeMarkPosition']").val('');
                return false;
            });

            $('#dialog-form-eye-mark img.btn').click(function () {
                var src = $(this).attr('src');
                var value = $(this).attr('toggle-value');
                $('img[id$="imgViewEyeMark"]').attr('src', src);
                $("input[type='hidden'][id$='hEyeMarkPosition']").val(value);
            });
        }

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SearchText);
        function SearchText(s, a) {
            if ($("input[type='text'][id$='txtName']").length > 0) {
                //$(".ui-autocomplete, .ui-dialog, .ui-helper-hidden-accessible").remove();
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

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SearchShipToParty);
        function SearchShipToParty(s, a) {
            if ($("input[type='text'][id$='txtShipToParty']").length > 0) {
                //$(".ui-autocomplete, .ui-dialog, .ui-helper-hidden-accessible").remove();
                $("input[type='text'][id$='txtShipToParty']").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "Job.aspx/GetCustomerData",
                            data: "{'Keyword':'" + $("input[type='text'][id$='txtShipToParty']").val() + "'}",
                            dataType: "json",
                            async: true,
                            cache: false,
                            success: function (result) {
                                response($.map($.parseJSON(result.d), function (item) {
                                    return { ID: item.CustomerID, Name: item.Name, Code: item.Code, Address: item.Address };
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
                        $("input[type='text'][id$='txtShipToParty']").val(ui.item.Name);
                        $("input[type='text'][id$='txtShipToPartyCode']").val(ui.item.Code);
                        $("#<%=lbShipToPartyAddress.ClientID%>").text(ui.item.Address);
                        $("input[type='hidden'][id$='hdShipToPartyID']").val(ui.item.ID);
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
                $("input[type='hidden'][id$='hdShipToPartyID']").val("");
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

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CreateRevision);
        function CreateRevision(sender, args) {
            $("#dialog-form-revision").dialog({
                autoOpen: false,
                height: 'auto',
                width: '800',
                appendTo: "form",
                modal: true,
                resizable: false
            });
        }

        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(OpenRevision);
        function OpenRevision() {
            $("#dialog-form-revision").dialog("open");
        }

        function CloseRevision() {
            $("#dialog-form-revision").dialog("close");
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

            $('#<%= btnResetPivot.ClientID%>')[0].click();
        }

        function ChangeCircumference() {
            var circum = parseFloat($('#<%= txtCircumference.ClientID %>').val().replace(/\,/g, ''));
            var pi = parseFloat($('#<%= hPiValue.ClientID%>').val());
            var diameter = (circum / pi).toFixed(2);
            $('#<%= txtDiameter.ClientID %>').val(diameter);
        }


        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ChooseCylinderPrinter);
        function ChooseCylinderPrinter() {
            $('#<%= btnPrintCylinder.ClientID %>').on('click', function () {
                cylinderPrint = "";
                var arrChk = $('[ID$=grvCylinders] .print-or-delete input[type=checkbox]').not('#chkSelectAll');
                arrChk.each(function () {
                    if ($(this).is(":checked") == true) {
                        var id = $(this).parents("td.column-one").find("input[type=hidden]");
                        cylinderPrint += id.val() + ",";

                    }
                })
                var newString = cylinderPrint.substring(0, cylinderPrint.length - 1);

                if (cylinderPrint.length == 0) {
                    $('#modalErrorPrint').modal('show');
                }
                else {
                    var iframe = $("#dialog-printing").find('iframe');

                    iframe.attr("src", "Printing/PrintCylinderCard.aspx?ID=" + newString + "&JobID=" + getParameterByName("ID"));

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
                }
            })

            $('a#printing').on('click', function (e) {

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
            });;

        }

        function getParameterByName(name) {
            name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
            var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                results = regex.exec(location.search);
            return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
        }
    </script>
</asp:Content>
