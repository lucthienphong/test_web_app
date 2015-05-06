<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master" AutoEventWireup="true" CodeBehind="JobEngraving.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.JobEngraving" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="button-control col-md-12 col-sm-12 affix">
            <div class="row">
                <div class="col-sm-12">
                    <div class="form-horizontal">

                        <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click" CssClass="btn btn-transparent">
                                <span class="flaticon-floppy1"></span>
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.SAVE)%>
                        </asp:LinkButton>
                        <div class="btn-group">
                            <button type="button" class="btn btn-transparent dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                                <span class='flaticon-printer60'></span>
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.PRINT)%>
                                <span class="caret"></span>
                            </button>
                            <asp:Literal runat="server" ID="ltrPrint"></asp:Literal>
                        </div>
                        <asp:LinkButton ID="btnCancel" runat="server" OnClick="btnCancel_Click" CssClass="btn btn-transparent">
                            <span class="flaticon-back57"></span>
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.RETURN)%></asp:LinkButton>
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
                                    <SweetSoft:ExtraInputMask ID="txtCode" RenderOnlyInput="true" runat="server" ReadOnly="true"
                                        Enabled="false"></SweetSoft:ExtraInputMask>
                                </div>
                                <div class="col-sm-10 col-xs-10">
                                    <SweetSoft:CustomExtraTextbox ID="txtName" RenderOnlyInput="true" ReadOnly="true" Enabled="false"
                                        runat="server"></SweetSoft:CustomExtraTextbox>
                                </div>
                                <asp:HiddenField ID="hCustomerID" runat="server" />
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
                        <SweetSoft:CustomExtraTextbox ID="txtJobNumber" RenderOnlyInput="true" ReadOnly="true" Enabled="false"
                            runat="server"></SweetSoft:CustomExtraTextbox>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <label class="control-label">Rev</label>
                        <asp:DropDownList ID="ddlRevNumber" runat="server" ReadOnly="true" Enabled="false"
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
        <div class="container-fluid" style="padding-bottom: 20px;">
            <div class="row">
                <div class="col-md-4">
                    <div class="row">
                        <div class="form-group col-md-12">
                            <label class="control-label">Job Co-Ord:</label>
                            <SweetSoft:CustomExtraTextbox ID="txtJobCoOrd" RenderOnlyInput="true"
                                runat="server"></SweetSoft:CustomExtraTextbox>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="row">
                        <div class="form-group col-md-6">
                            <label class="control-label">Chrome Thickness:</label>
                            <SweetSoft:CustomExtraTextbox ID="txtChromeThickness" RenderOnlyInput="true"
                                runat="server"></SweetSoft:CustomExtraTextbox>
                        </div>
                        <div class="form-group col-md-6">
                            <label class="control-label">Roughness:</label>
                            <SweetSoft:CustomExtraTextbox ID="txtRoughness" RenderOnlyInput="true"
                                runat="server"></SweetSoft:CustomExtraTextbox>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row" id="divMechanical" runat="server">
                <div class="col-sm-12">
                    <div class="form-group col-md-12">
                        <div class="row">
                            <label class="control-label">Mechanical - Parameters for job</label>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="form-group col-md-3">
                                            <label class="control-label">Engraving Start</label>
                                            <SweetSoft:ExtraInputMask ID="txtEngravingStart" RenderOnlyInput="true"
                                                MaskType="Numeric" GroupSeparator="," AutoGroup="true" Digits="2"
                                                PlaceholderMask="0" runat="server"></SweetSoft:ExtraInputMask>
                                        </div>
                                        <div class="form-group col-md-3">
                                            <label class="control-label">Engraving Width</label>
                                            <SweetSoft:ExtraInputMask ID="txtEngravingWidth" RenderOnlyInput="true"
                                                MaskType="Numeric" GroupSeparator="," AutoGroup="true" Digits="2"
                                                PlaceholderMask="0" runat="server"></SweetSoft:ExtraInputMask>
                                        </div>
                                        <div class="form-group col-md-3">
                                            <label class="control-label">File Size H</label>
                                            <SweetSoft:ExtraInputMask ID="txtFileSizeHEMG" RenderOnlyInput="true"
                                                MaskType="Numeric" GroupSeparator="," AutoGroup="true" Digits="2"
                                                PlaceholderMask="0" runat="server"></SweetSoft:ExtraInputMask>
                                        </div>
                                        <div class="form-group col-md-3">
                                            <label class="control-label">File Size V</label>
                                            <SweetSoft:ExtraInputMask ID="txtFileSizeVEMG" RenderOnlyInput="true"
                                                MaskType="Numeric" GroupSeparator="," AutoGroup="true" Digits="2"
                                                PlaceholderMask="0" runat="server"></SweetSoft:ExtraInputMask>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group col-md-12">
                                            <label class="control-label">Remark (EMG)</label>
                                            <asp:TextBox ID="txtRemarkEMG" runat="server"
                                                CssClass="form-control" TextMode="MultiLine"
                                                Rows="4"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="tablesContacts" class="dataTables_wrapper form-inline no-footer">
                                <div class="table-responsive">
                                    <asp:GridView ID="grvCylinder" runat="server" AutoGenerateColumns="false"
                                        CssClass="table table-striped table-bordered table-checkable dataTable" GridLines="None"
                                        AllowPaging="false" AllowSorting="false" DataKeyNames="EngravingID"
                                        OnRowCommand="grvCylinder_RowCommand"
                                        OnRowDeleting="grvCylinder_RowDeleting"
                                        OnRowEditing="grvCylinder_RowEditing"
                                        OnRowUpdating="grvCylinder_RowUpdating"
                                        OnRowCancelingEdit="grvCylinder_RowCancelingEdit">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Seq">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnEditHead" runat="server" CssClass="btn btn-link"
                                                        CommandArgument='<%# Eval("EngravingID") %>' CommandName="Edit" Text='<%#Eval("Sequence")%>'>
                                                    </asp:LinkButton>
                                                    <asp:HiddenField runat="server" ID="hdfCylinderID" Value='<%#Eval("CylinderID") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CylinderNo" HeaderStyle-CssClass="column-60 text-center" ItemStyle-CssClass="column-60">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbSequence" runat="server"
                                                        Text='<%#Eval("CylinderNo")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Color" HeaderStyle-CssClass="column-180 text-center" ItemStyle-CssClass="column-180">
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
                                            <asp:TemplateField HeaderText="Stylus" HeaderStyle-CssClass="column-80 text-center" ItemStyle-CssClass="column-80">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbStylus" runat="server"
                                                        Text='<%#Eval("Stylus")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div style="position: relative;">
                                                        <SweetSoft:ExtraInputMask ID="txtStylus" RenderOnlyInput="true" Required="false" Width="100%"
                                                            runat="server" MaskType="Integer" GroupSeparator="," RadixPoint="." Text='<%#Eval("Stylus")%>' Digits="0" AutoGroup="true"></SweetSoft:ExtraInputMask>
                                                    </div>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Screen" HeaderStyle-CssClass="column-80 text-center" ItemStyle-CssClass="column-80">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbScreen" runat="server"
                                                        Text='<%#Eval("Screen")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div style="position: relative;">
                                                        <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtScreen"
                                                            Text='<%#Eval("Screen")%>' Width="100%"
                                                            CssClass="form-control" runat="server">
                                                        </SweetSoft:CustomExtraTextbox>
                                                    </div>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Angle" HeaderStyle-CssClass="column-80 text-center" ItemStyle-CssClass="column-80">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbAngle" runat="server"
                                                        Text='<%#Eval("Angle")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div style="position: relative;">
                                                        <SweetSoft:ExtraInputMask ID="txtAngle" RenderOnlyInput="true" Required="false" Width="100%"
                                                            runat="server" MaskType="Integer" GroupSeparator="," RadixPoint="." Text='<%#Eval("Angle")%>' Digits="0" AutoGroup="true"></SweetSoft:ExtraInputMask>
                                                    </div>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Gamma" HeaderStyle-CssClass="column-80 text-center" ItemStyle-CssClass="column-80">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbGamma" runat="server"
                                                        Text='<%#Eval("Gamma")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div style="position: relative;">
                                                        <SweetSoft:ExtraInputMask ID="txtGamma" RenderOnlyInput="true" Required="false" Width="100%"
                                                            runat="server" MaskType="Integer" GroupSeparator="," RadixPoint="." Text='<%#Eval("Gamma")%>' Digits="0" AutoGroup="true"></SweetSoft:ExtraInputMask>
                                                    </div>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sh" HeaderStyle-CssClass="column-80 text-center" ItemStyle-CssClass="column-80">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbSh" runat="server"
                                                        Text='<%#Eval("Sh")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div style="position: relative;">
                                                        <SweetSoft:ExtraInputMask ID="txtSh" RenderOnlyInput="true" Required="false" Width="100%"
                                                            runat="server" MaskType="Integer" GroupSeparator="," RadixPoint="." Text='<%#Eval("Sh")%>' Digits="0" AutoGroup="true"></SweetSoft:ExtraInputMask>
                                                    </div>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Hl" HeaderStyle-CssClass="column-80 text-center" ItemStyle-CssClass="column-80">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbHl" runat="server"
                                                        Text='<%#Eval("Hl")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div style="position: relative;">
                                                        <SweetSoft:ExtraInputMask ID="txtHl" RenderOnlyInput="true" Required="false" Width="100%"
                                                            runat="server" MaskType="Integer" GroupSeparator="," RadixPoint="." Text='<%#Eval("Hl")%>' Digits="0" AutoGroup="true"></SweetSoft:ExtraInputMask>

                                                    </div>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ch" HeaderStyle-CssClass="column-80 text-center" ItemStyle-CssClass="column-80">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbCh" runat="server"
                                                        Text='<%#Eval("Ch")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div style="position: relative;">
                                                        <SweetSoft:ExtraInputMask ID="txtCh" RenderOnlyInput="true" Required="false" Width="100%"
                                                            runat="server" MaskType="Integer" GroupSeparator="," RadixPoint="." Text='<%#Eval("Ch")%>' Digits="0" AutoGroup="true"></SweetSoft:ExtraInputMask>
                                                    </div>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Mt" HeaderStyle-CssClass="column-80 text-center" ItemStyle-CssClass="column-80">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbMt" runat="server"
                                                        Text='<%#Eval("Mt")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div style="position: relative;">
                                                        <SweetSoft:ExtraInputMask ID="txtMt" RenderOnlyInput="true" Required="false" Width="100%"
                                                            runat="server" MaskType="Integer" GroupSeparator="," RadixPoint="." Text='<%#Eval("Mt")%>' Digits="0" AutoGroup="true"></SweetSoft:ExtraInputMask>
                                                    </div>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cell depth" HeaderStyle-CssClass="column-100 text-center" ItemStyle-CssClass="column-100">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCellDepth" runat="server"
                                                        Text='<%#Convert.ToDouble(ShowNumberFormat(Eval("CellDepth"))).ToString("#####")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div style="position: relative;">
                                                        <SweetSoft:ExtraInputMask ID="txtCellDepth" RenderOnlyInput="true" Required="false" Width="100%"
                                                            runat="server" MaskType="Decimal" GroupSeparator="," RadixPoint="." Text='<%#Eval("CellDepth")%>' Digits="5" AutoGroup="true"></SweetSoft:ExtraInputMask>

                                                    </div>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sh<br/>(Actual Cooper)" HeaderStyle-CssClass="column-150 text-center" ItemStyle-CssClass="column-150">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbActualCopperSh" runat="server"
                                                        Text='<%#Eval("CopperSh")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div style="position: relative;">
                                                        <SweetSoft:ExtraInputMask ID="txtActualCopperSh" RenderOnlyInput="true" Required="false" Width="100%"
                                                            runat="server" MaskType="Integer" GroupSeparator="," RadixPoint="." Text='<%#Eval("CopperSh")%>'
                                                            Digits="0" AutoGroup="true"></SweetSoft:ExtraInputMask>
                                                    </div>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ch<br/>(Actual Cooper)" HeaderStyle-CssClass="column-150 text-center" ItemStyle-CssClass="column-150">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbActualCopperCh" runat="server"
                                                        Text='<%#Eval("CopperCh")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div style="position: relative;">
                                                        <SweetSoft:ExtraInputMask ID="txtActualCopperCh" RenderOnlyInput="true" Required="false" Width="100%"
                                                            runat="server" MaskType="Integer" GroupSeparator="," RadixPoint="." Text='<%#Eval("CopperCh")%>'
                                                            Digits="0" AutoGroup="true"></SweetSoft:ExtraInputMask>
                                                    </div>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sh<br/>(Actual Chrome)" HeaderStyle-CssClass="column-150 text-center" ItemStyle-CssClass="column-150">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbActualChromeSh" runat="server"
                                                        Text='<%#Eval("ChromeSh")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div style="position: relative;">
                                                        <SweetSoft:ExtraInputMask ID="txtActualChromeSh" RenderOnlyInput="true" Required="false" Width="100%"
                                                            runat="server" MaskType="Integer" GroupSeparator="," RadixPoint="." Text='<%#Eval("ChromeSh")%>'
                                                            Digits="0" AutoGroup="true"></SweetSoft:ExtraInputMask>
                                                    </div>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ch<br/>(Actual Chrome)" HeaderStyle-CssClass="column-150 text-center" ItemStyle-CssClass="column-150">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbActualChromeCh" runat="server"
                                                        Text='<%#Eval("ChromeCh")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div style="position: relative;">
                                                        <SweetSoft:ExtraInputMask ID="txtActualChromeCh" RenderOnlyInput="true" Required="false" Width="100%"
                                                            runat="server" MaskType="Integer" GroupSeparator="," RadixPoint="." Text='<%#Eval("ChromeCh")%>'
                                                            Digits="0" AutoGroup="true"></SweetSoft:ExtraInputMask>
                                                    </div>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-CssClass="column-100 text-center" ItemStyle-CssClass="column-100 text-center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-link"
                                                        CommandArgument='<%# Eval("EngravingID") %>' CommandName="Edit">
                                                            <span class="fa fa-edit" style="font-size: 21px;vertical-align: -7px;"></span>
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
                                            <asp:TemplateField  HeaderStyle-CssClass="column-80 text-center" ItemStyle-CssClass="column-80 text-center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="tbnDelete" Visible='<%#Convert.ToBoolean(Eval("IsCopy"))%>'
                                                        runat="server" CssClass="btn btn-link"
                                                        CommandName="DeleteDetail" CommandArgument='<%#Eval("EngravingID") %>' Text="Delete">
                                                        <span style="font-size:18px; vertical-align:middle " class="glyphicon glyphicon-remove"></span></span>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnGetCopy" Visible='<%#!Convert.ToBoolean(Eval("IsCopy")) %>'
                                                        runat="server" CssClass="btn btn-link"
                                                        CommandName="Copy" CommandArgument='<%#Eval("EngravingID") %>' Text="Copy">
                                                    <span style="font-size:18px; vertical-align:middle" class="flaticon-files6"></span>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            There are currently no items in this table.
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                    <asp:HiddenField runat="server" ID="hdfEngravingID" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row" id="divTobacco" runat="server">
                <div class="col-sm-12">
                    <div class="form-group col-md-12">
                        <div class="row">
                            <label class="control-label">Tobacco - Parameters for job</label>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="col-md-4 form-group">
                                            <div class="row">
                                                <div class="form-group col-md-12">
                                                    <label>
                                                        <asp:CheckBox ID="chkEngravingOnNut" runat="server" CssClass="uniform" />
                                                        Engraving On Nut
                                                    </label>
                                                </div>
                                                <div class="form-group col-md-12">
                                                    <label>
                                                        <asp:CheckBox ID="chkEngravingOnBoader" runat="server" CssClass="uniform" />
                                                        Engraving On Board
                                                    </label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group col-md-4">
                                            <label class="control-label">File Size H</label>
                                            <SweetSoft:ExtraInputMask ID="txtFileSizeHDLS" RenderOnlyInput="true"
                                                MaskType="Numeric" GroupSeparator="," AutoGroup="true" Digits="2"
                                                PlaceholderMask="0" runat="server"></SweetSoft:ExtraInputMask>
                                        </div>
                                        <div class="form-group col-md-4">
                                            <label class="control-label">File Size V</label>
                                            <SweetSoft:ExtraInputMask ID="txtFileSizeVDLS" RenderOnlyInput="true"
                                                MaskType="Numeric" GroupSeparator="," AutoGroup="true" Digits="2"
                                                PlaceholderMask="0" runat="server"></SweetSoft:ExtraInputMask>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group col-md-12">
                                            <label class="control-label">Remark (DLS)</label>
                                            <asp:TextBox ID="txtRemarkDLS" runat="server"
                                                CssClass="form-control" TextMode="MultiLine"
                                                Rows="4"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="divGridview" class="table-responsive">
                                <asp:GridView ID="grvTobacco" runat="server" AutoGenerateColumns="false"
                                    CssClass="table table-striped table-bordered table-checkable " GridLines="None"
                                    AllowPaging="false" AllowSorting="false" DataKeyNames="EngravingID"
                                    OnRowCommand="grvTobaccco_RowCommand"
                                    OnRowDeleting="grvTobaccco_RowDeleting"
                                    OnRowEditing="grvTobaccco_RowEditing"
                                    OnRowUpdating="grvTobaccco_RowUpdating"
                                    OnRowCancelingEdit="grvTobaccco_RowCancelingEdit"
                                    OnRowDataBound="grvTobaccco_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Seq" HeaderStyle-CssClass="text-center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEditHead" runat="server" CssClass="btn btn-link"
                                                    CommandArgument='<%# Eval("EngravingID") %>' CommandName="Edit" Text='<%#Eval("Sequence")%>'>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CylinderNo" HeaderStyle-CssClass="column-60 text-center" ItemStyle-CssClass="column-60">
                                            <ItemTemplate>                                                
                                                <asp:Label ID="lbCylinderNo" runat="server"
                                                    Text='<%#Eval("CylinderNo")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Color" HeaderStyle-CssClass="column-180 text-center" ItemStyle-CssClass="column-180">
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
                                        <asp:TemplateField HeaderText="Screen" HeaderStyle-CssClass="column-80 text-center" ItemStyle-CssClass="column-80">
                                            <ItemTemplate>
                                                <asp:Label ID="lbScreen" runat="server"
                                                    Text='<%#Eval("Screen")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <div style="position: relative;">
                                                    <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtScreen"
                                                        Text='<%#Eval("Screen")%>' Width="100%"
                                                        CssClass="form-control" runat="server">
                                                    </SweetSoft:CustomExtraTextbox>
                                                </div>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="MasterScreen" HeaderStyle-CssClass="column-120 text-center" ItemStyle-CssClass="column-120 text-center">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkMasterScreen" CssClass="uniform" Enabled="false"
                                                    Checked='<%#Convert.ToBoolean(Eval("MasterScreen"))%>' runat="server"></asp:CheckBox>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="chkMasterScreen" CssClass="uniform"
                                                    Checked='<%#Convert.ToBoolean(Eval("MasterScreen"))%>' runat="server"></asp:CheckBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Angle" HeaderStyle-CssClass="column-120 text-center" ItemStyle-CssClass="column-120">
                                            <ItemTemplate>
                                                <asp:Label ID="lbAngle" runat="server"
                                                    Text='<%#Eval("Angle")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <div style="position: relative;">
                                                    <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtAngle"
                                                        Text='<%#Eval("Angle")%>' Width="100%"
                                                        CssClass="form-control" runat="server">
                                                    </SweetSoft:CustomExtraTextbox>
                                                </div>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Elongation" HeaderStyle-CssClass="column-100 text-center" ItemStyle-CssClass="column-100">
                                            <ItemTemplate>
                                                <asp:Label ID="lbElongation" runat="server"
                                                    Text='<%#Eval("Elongation")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <div style="position: relative;">
                                                    <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtElongation"
                                                        Text='<%#Eval("Elongation")%>' Width="100%"
                                                        CssClass="form-control" runat="server">
                                                    </SweetSoft:CustomExtraTextbox>
                                                </div>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Distortion" HeaderStyle-CssClass="column-100 text-center" ItemStyle-CssClass="column-100">
                                            <ItemTemplate>
                                                <asp:Label ID="lbDistotion" runat="server"
                                                    Text='<%#Eval("Distotion")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <div style="position: relative;">
                                                    <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtDistotion"
                                                        Text='<%#Eval("Distotion")%>' Width="100%"
                                                        CssClass="form-control" runat="server">
                                                    </SweetSoft:CustomExtraTextbox>
                                                </div>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Resolution" HeaderStyle-CssClass="column-100 text-center" ItemStyle-CssClass="column-100">
                                            <ItemTemplate>
                                                <asp:Label ID="lbResolution" runat="server"
                                                    Text='<%#Eval("Resolution")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <div style="position: relative;">
                                                    <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtResolution"
                                                        Text='<%#Eval("Resolution")%>' Width="100%"
                                                        CssClass="form-control" runat="server">
                                                    </SweetSoft:CustomExtraTextbox>
                                                </div>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Hexagonal Dot" HeaderStyle-CssClass="column-120 text-center" ItemStyle-CssClass="column-120">
                                            <ItemTemplate>
                                                <asp:Label ID="lbHexagonal" runat="server"
                                                    Text='<%#Eval("HexaName")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <div style="position: relative;">
                                                    <asp:DropDownList ID="ddlHexagonal" runat="server"
                                                        data-style="btn btn-info" data-width="100%" Required="true"
                                                        data-container="body"
                                                        data-toggle="dropdown" CssClass="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Image Smoothness" HeaderStyle-CssClass="column-180 text-center" ItemStyle-CssClass="column-180 text-center">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkImageSmoothnessView" CssClass="uniform" Enabled="false"
                                                    Checked='<%#Convert.ToBoolean(Eval("ImageSmoothness"))%>' runat="server"></asp:CheckBox>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="chkImageSmoothness" CssClass="uniform"
                                                    Checked='<%#Convert.ToBoolean(Eval("ImageSmoothness"))%>' runat="server"></asp:CheckBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Unsharp Masking" HeaderStyle-CssClass="column-180 text-center" ItemStyle-CssClass="column-180">
                                            <ItemTemplate>
                                                <asp:Label ID="lbUnsharpMasking" runat="server"
                                                    Text='<%#Eval("UnsharpMasking")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <div style="position: relative;">
                                                    <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtUnsharpMasking"
                                                        Text='<%#Eval("UnsharpMasking")%>' Width="100%"
                                                        CssClass="form-control" runat="server">
                                                    </SweetSoft:CustomExtraTextbox>
                                                </div>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Antialiasing" HeaderStyle-CssClass="column-100 text-center" ItemStyle-CssClass="column-100">
                                            <ItemTemplate>
                                                <asp:Label ID="lbAntialiasing" runat="server"
                                                    Text='<%#Eval("Antialiasing")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <div style="position: relative;">
                                                    <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtAntialiasing"
                                                        Text='<%#Eval("Antialiasing")%>' Width="100%"
                                                        CssClass="form-control" runat="server">
                                                    </SweetSoft:CustomExtraTextbox>
                                                </div>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Linework Widening" HeaderStyle-CssClass="column-150 text-center" ItemStyle-CssClass="column-150">
                                            <ItemTemplate>
                                                <asp:Label ID="lbLineworkWidening" runat="server"
                                                    Text='<%#Eval("LineworkWidening")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <div style="position: relative;">
                                                    <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtLineworkWidening"
                                                        Text='<%#Eval("LineworkWidening")%>' Width="100%"
                                                        CssClass="form-control" runat="server">
                                                    </SweetSoft:CustomExtraTextbox>
                                                </div>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Engraving Start" HeaderStyle-CssClass="column-150 text-center" ItemStyle-CssClass="column-150">
                                            <ItemTemplate>
                                                <asp:Label ID="lbEngravingStart" runat="server"
                                                    Text='<%#Eval("EngravingStart")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <div style="position: relative;">
                                                    <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtEngravingStart"
                                                        Text='<%#Eval("EngravingStart")%>' Width="100%"
                                                        CssClass="form-control" runat="server">
                                                    </SweetSoft:CustomExtraTextbox>
                                                </div>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Engraving Width" HeaderStyle-CssClass="column-150 text-center" ItemStyle-CssClass="column-150">
                                            <ItemTemplate>
                                                <asp:Label ID="lbEngravingWidth" runat="server"
                                                    Text='<%#Eval("EngravingWidth")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <div style="position: relative;">
                                                    <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtEngravingWidth"
                                                        Text='<%#Eval("EngravingWidth")%>' Width="100%"
                                                        CssClass="form-control" runat="server">
                                                    </SweetSoft:CustomExtraTextbox>
                                                </div>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cell Shape" HeaderStyle-CssClass="column-120 text-center" ItemStyle-CssClass="column-120">
                                            <ItemTemplate>
                                                <asp:Label ID="lbCellShape" runat="server"
                                                    Text='<%#Eval("CellShapeName")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <div style="position: relative;">
                                                    <asp:DropDownList ID="ddlCellShape" runat="server"
                                                        data-style="btn btn-info" data-width="100%" Required="true"
                                                        data-container="body"
                                                        data-toggle="dropdown" CssClass="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Gradation" HeaderStyle-CssClass="column-120 text-center" ItemStyle-CssClass="column-120">
                                            <ItemTemplate>
                                                <asp:Label ID="lbGradation" runat="server"
                                                    Text='<%#Eval("GraName")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <div style="position: relative;">
                                                    <asp:DropDownList ID="ddlGradation" runat="server"
                                                        data-style="btn btn-info" data-width="100%" Required="true"
                                                        data-container="body"
                                                        data-toggle="dropdown" CssClass="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Gamma" HeaderStyle-CssClass="column-250 text-center" ItemStyle-CssClass="column-250">
                                            <ItemTemplate>
                                                <asp:Label ID="lbGamma" runat="server"
                                                    Text='<%#Eval("Gamma")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <div style="position: relative;">
                                                    <asp:TextBox ID="txtGamma" TextMode="MultiLine" Rows="3"
                                                        Text='<%#Eval("Gamma")%>' Width="100%"
                                                        CssClass="form-control" runat="server">
                                                    </asp:TextBox>
                                                </div>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Laser" HeaderStyle-CssClass="column-180 text-center" ItemStyle-CssClass="column-180">
                                            <ItemTemplate>
                                                <div class="col-md-6">
                                                    <label>
                                                        A
                                                            <asp:CheckBox ID="chkLaserAView" CssClass="uniform" Enabled="false"
                                                                Checked='<%#Convert.ToBoolean(Eval("LaserA"))%>' runat="server"></asp:CheckBox>
                                                    </label>
                                                </div>
                                                <div class="col-md-6">
                                                    <label>
                                                        B
                                                            <asp:CheckBox ID="chkLaserBView" CssClass="uniform" Enabled="false"
                                                                Checked='<%#Convert.ToBoolean(Eval("LaserB"))%>' runat="server"></asp:CheckBox>
                                                    </label>
                                                </div>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <div class="col-md-6">
                                                    <label>
                                                        A
                                                    <asp:CheckBox ID="chkLaserA" CssClass="uniform"
                                                        Checked='<%#Convert.ToBoolean(Eval("LaserA"))%>' runat="server"></asp:CheckBox>
                                                    </label>
                                                </div>
                                                <div class="col-md-6">
                                                    <label>
                                                        B
                                                    <asp:CheckBox ID="chkLaserB" CssClass="uniform"
                                                        Checked='<%#Convert.ToBoolean(Eval("LaserB"))%>' runat="server"></asp:CheckBox>
                                                    </label>
                                                </div>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cell Width" HeaderStyle-CssClass="column-120 text-center" ItemStyle-CssClass="column-120">
                                            <ItemTemplate>
                                                <asp:Label ID="lbCellWidth" runat="server"
                                                    Text='<%#Eval("CellWidth")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <div style="position: relative;">
                                                    <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtCellWidth"
                                                        Text='<%#Eval("CellWidth")%>' Width="100%"
                                                        CssClass="form-control" runat="server">
                                                    </SweetSoft:CustomExtraTextbox>
                                                </div>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Channel Width" HeaderStyle-CssClass="column-120 text-center" ItemStyle-CssClass="column-120">
                                            <ItemTemplate>
                                                <asp:Label ID="lbChannelWidth" runat="server"
                                                    Text='<%#Eval("ChannelWidth")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <div style="position: relative;">
                                                    <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtChannelWidth"
                                                        Text='<%#Eval("ChannelWidth")%>' Width="100%"
                                                        CssClass="form-control" runat="server">
                                                    </SweetSoft:CustomExtraTextbox>
                                                </div>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cell Depth" HeaderStyle-CssClass="column-120 text-center" ItemStyle-CssClass="column-120">
                                            <ItemTemplate>
                                                <asp:Label ID="lbCellDepth" runat="server"
                                                    Text='<%#Eval("CellDepth")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <div style="position: relative;">
                                                    <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtCellDepth"
                                                        Text='<%#Eval("CellDepth")%>' Width="100%"
                                                        CssClass="form-control" runat="server">
                                                    </SweetSoft:CustomExtraTextbox>
                                                </div>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Engraving Time" HeaderStyle-CssClass="column-180 text-center" ItemStyle-CssClass="column-180">
                                            <ItemTemplate>
                                                <asp:Label ID="lbEngravingTime" runat="server"
                                                    Text='<%#Eval("EngravingTime")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <div style="position: relative;">
                                                    <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtEngravingTime"
                                                        Text='<%#Eval("EngravingTime")%>' Width="100%"
                                                        CssClass="form-control" runat="server">
                                                    </SweetSoft:CustomExtraTextbox>
                                                </div>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Beam" HeaderStyle-CssClass="column-80 text-center" ItemStyle-CssClass="column-80">
                                            <ItemTemplate>
                                                <asp:Label ID="lbBeam" runat="server"
                                                    Text='<%#Eval("EngravingTime")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <div style="position: relative;">
                                                    <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtBeam"
                                                        Text='<%#Eval("Beam")%>' Width="100%"
                                                        CssClass="form-control" runat="server">
                                                    </SweetSoft:CustomExtraTextbox>
                                                </div>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Threshold" HeaderStyle-CssClass="column-100 text-center" ItemStyle-CssClass="column-100">
                                            <ItemTemplate>
                                                <asp:Label ID="lbThreshold" runat="server"
                                                    Text='<%#Eval("Threshold")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <div style="position: relative;">
                                                    <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtThreshold"
                                                        Text='<%#Eval("Threshold")%>' Width="100%"
                                                        CssClass="form-control" runat="server">
                                                    </SweetSoft:CustomExtraTextbox>
                                                </div>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Checked By" HeaderStyle-CssClass="column-250 text-center" ItemStyle-CssClass="column-250">
                                            <ItemTemplate>
                                                <asp:Label ID="lbCheckedBy" runat="server"
                                                    Text='<%#Eval("CheckedBy")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <div style="position: relative;">
                                                    <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtCheckedBy"
                                                        Text='<%#Eval("CheckedBy")%>' Width="100%"
                                                        CssClass="form-control" runat="server">
                                                    </SweetSoft:CustomExtraTextbox>
                                                </div>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Date" HeaderStyle-CssClass="column-150 text-center" ItemStyle-CssClass="column-150">
                                            <ItemTemplate>
                                                <asp:Label ID="lbCheckedOn" runat="server"
                                                    Text='<%#ShowDatetimeFormat(Eval("CheckedOn"))%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <div style="position: relative;" class="form-group wrap-datepicker">
                                                    <SweetSoft:ExtraInputMask ID="txtCheckedOn" RenderOnlyInput="true"
                                                        data-format="dd-mm-yyyy" Text='<%#ShowDatetimeFormat(Eval("CheckedOn"))%>'
                                                        CssClass="form-control mask-date"
                                                        runat="server"></SweetSoft:ExtraInputMask>
                                                    <span class="fa fa-calendar in-mask-date"></span>
                                                </div>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-CssClass="column-100 text-center" ItemStyle-CssClass="column-100 text-center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-link"
                                                    CommandArgument='<%# Eval("EngravingID") %>' CommandName="Edit">
                                                            <span class="fa fa-edit" style="font-size: 21px;vertical-align: -7px;"></span>
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
                                        <asp:TemplateField HeaderStyle-CssClass="column-80 text-center" ItemStyle-CssClass="column-80 text-center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="tbnDelete" Visible='<%#Convert.ToBoolean(Eval("IsCopy")) && Convert.ToInt32(Eval("CylinderID")) > 0 ? true : false%>'
                                                    runat="server" CssClass="btn btn-link"
                                                    CommandName="DeleteDetail" CommandArgument='<%#Eval("EngravingID") %>' Text="Delete">
                                            <span style="font-size:18px; vertical-align:middle " class="glyphicon glyphicon-remove"></span></span>
                                                </asp:LinkButton>

                                                <asp:LinkButton ID="btnGetCopy" Visible='<%#!Convert.ToBoolean(Eval("IsCopy"))  && Convert.ToInt32(Eval("CylinderID")) > 0 ? true : false%>'
                                                    runat="server" CssClass="btn btn-link"
                                                    CommandName="Copy" CommandArgument='<%#Eval("EngravingID") %>' Text="Copy">
                                                    <span style="font-size:18px; vertical-align:middle" class="flaticon-files6"></span>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        There are currently no items in this table.
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <asp:HiddenField runat="server" ID="HiddenField1" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row" id="divEtching" runat="server">
                <div class="col-sm-12">
                    <div class="form-group col-md-12">
                        <div class="row">
                            <label class="control-label">Etching - Parameters for job</label>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="form-group col-md-4">
                                            <div class="row">
                                                <div class="form-group col-md-12">
                                                    <label class="control-label">Laser start keyway at right position:</label>
                                                    <SweetSoft:CustomExtraTextbox ID="txtLaseStart" RenderOnlyInput="true"
                                                        runat="server"></SweetSoft:CustomExtraTextbox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group col-md-2">
                                            <label class="control-label">Start Position</label>
                                            <SweetSoft:ExtraInputMask ID="txtEngrStartEtching" RenderOnlyInput="true"
                                                MaskType="Numeric" GroupSeparator="," AutoGroup="true" Digits="2"
                                                PlaceholderMask="0" runat="server"></SweetSoft:ExtraInputMask>
                                        </div>
                                        <div class="form-group col-md-2">
                                            <label class="control-label">Laser Width</label>
                                            <SweetSoft:ExtraInputMask ID="txtEngrWidthEtching" RenderOnlyInput="true"
                                                MaskType="Numeric" GroupSeparator="," AutoGroup="true" Digits="2"
                                                PlaceholderMask="0" runat="server"></SweetSoft:ExtraInputMask>
                                        </div>
                                        <div class="form-group col-md-2">
                                            <label class="control-label">File Size H</label>
                                            <SweetSoft:ExtraInputMask ID="txtFileSizeHEtching" RenderOnlyInput="true"
                                                MaskType="Numeric" GroupSeparator="," AutoGroup="true" Digits="2"
                                                PlaceholderMask="0" runat="server"></SweetSoft:ExtraInputMask>
                                        </div>
                                        <div class="form-group col-md-2">
                                            <label class="control-label">File Size V</label>
                                            <SweetSoft:ExtraInputMask ID="txtFileSizeVEtching" RenderOnlyInput="true"
                                                MaskType="Numeric" GroupSeparator="," AutoGroup="true" Digits="2"
                                                PlaceholderMask="0" runat="server"></SweetSoft:ExtraInputMask>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="row">
                                                <div class="form-group col-md-12">
                                                    <label class="control-label">Laser Operator:</label>
                                                    <SweetSoft:CustomExtraTextbox ID="txtLaserOperator" RenderOnlyInput="true"
                                                        runat="server"></SweetSoft:CustomExtraTextbox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="row">
                                                <div class="form-group col-md-12">
                                                    <label class="control-label">Final Control(QA):</label>
                                                    <SweetSoft:CustomExtraTextbox ID="txtFinalControl" RenderOnlyInput="true"
                                                        runat="server"></SweetSoft:CustomExtraTextbox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group col-md-12">
                                            <label class="control-label">Remark (Etching)</label>
                                            <asp:TextBox ID="txtSRRemarkEtching" runat="server"
                                                CssClass="form-control" TextMode="MultiLine"
                                                Rows="4"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="Div2" class="dataTables_wrapper form-inline no-footer">
                                <div class="table-responsive">
                                    <asp:GridView ID="grvEtching" runat="server" AutoGenerateColumns="false"
                                        CssClass="table table-striped table-bordered table-checkable dataTable" GridLines="None"
                                        AllowPaging="false" AllowSorting="false" DataKeyNames="EngravingID"
                                        OnRowCommand="grvEtching_RowCommand"
                                        OnRowDeleting="grvEtching_RowDeleting"
                                        OnRowEditing="grvEtching_RowEditing"
                                        OnRowUpdating="grvEtching_RowUpdating"
                                        OnRowCancelingEdit="grvEtching_RowCancelingEdit">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Seq">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnEditHead" runat="server" CssClass="btn btn-link"
                                                        CommandArgument='<%# Eval("EngravingID") %>' CommandName="Edit" Text='<%#Eval("Sequence")%>'>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CylinderNo" HeaderStyle-CssClass="column-60 text-center" ItemStyle-CssClass="column-60">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbSequence" runat="server"
                                                        Text='<%#Eval("CylinderNo")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Color" HeaderStyle-CssClass="column-180 text-center" ItemStyle-CssClass="column-180">
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
                                            <asp:TemplateField HeaderText="Screen lbi" HeaderStyle-CssClass="column-100 text-center" ItemStyle-CssClass="column-100">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbScreen" runat="server"
                                                        Text='<%#Eval("ScreenLpi")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div style="position: relative;">
                                                        <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtScreen"
                                                            Text='<%#Eval("ScreenLpi")%>' Width="100%"
                                                            CssClass="form-control" runat="server">
                                                        </SweetSoft:CustomExtraTextbox>
                                                    </div>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cell type" HeaderStyle-CssClass="column-100 text-center" ItemStyle-CssClass="column-100">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbCellType" runat="server"
                                                        Text='<%#Eval("CellType")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div style="position: relative;">
                                                        <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtCellType"
                                                            Text='<%#Eval("CellType")%>' Width="100%"
                                                            CssClass="form-control" runat="server">
                                                        </SweetSoft:CustomExtraTextbox>
                                                    </div>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Angle" HeaderStyle-CssClass="column-100 text-center" ItemStyle-CssClass="column-100">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbAngle" runat="server"
                                                        Text='<%#Eval("Angle")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div style="position: relative;">
                                                        <SweetSoft:ExtraInputMask ID="txtAngle" RenderOnlyInput="true" Required="false" Width="100%"
                                                            runat="server" MaskType="Integer" GroupSeparator="," RadixPoint="."
                                                            Text='<%#Eval("Angle")%>' Digits="0" AutoGroup="true"></SweetSoft:ExtraInputMask>
                                                    </div>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Gamma" HeaderStyle-CssClass="column-100 text-center" ItemStyle-CssClass="column-100">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbGamma" runat="server"
                                                        Text='<%#Eval("Gamma")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div style="position: relative;">
                                                        <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtGamma"
                                                            Text='<%#Eval("Gamma")%>' Width="100%"
                                                            CssClass="form-control" runat="server">
                                                        </SweetSoft:CustomExtraTextbox>
                                                    </div>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cell Size µ(Target)" HeaderStyle-CssClass="column-100 text-center" ItemStyle-CssClass="column-100">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbTargetCellSize" runat="server"
                                                        Text='<%#Eval("TargetCellSize")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div style="position: relative;">
                                                        <SweetSoft:ExtraInputMask ID="txtTargetCellSize" RenderOnlyInput="true" Required="false" Width="100%"
                                                            runat="server" MaskType="Decimal" GroupSeparator="," RadixPoint="."
                                                            Text='<%#Eval("TargetCellSize")%>' Digits="2" AutoGroup="true"></SweetSoft:ExtraInputMask>
                                                    </div>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cell Wall µ(Target)" HeaderStyle-CssClass="column-100 text-center" ItemStyle-CssClass="column-100">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbTargetCellWall" runat="server"
                                                        Text='<%#Eval("TargetCellWall")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div style="position: relative;">
                                                        <SweetSoft:ExtraInputMask ID="txtTargetCellWall" RenderOnlyInput="true" Required="false" Width="100%"
                                                            runat="server" MaskType="Decimal" GroupSeparator="," RadixPoint="."
                                                            Text='<%#Eval("TargetCellWall")%>' Digits="2" AutoGroup="true"></SweetSoft:ExtraInputMask>
                                                    </div>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cell Depth µ(Target)" HeaderStyle-CssClass="column-100 text-center" ItemStyle-CssClass="column-100">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbTargetCellDepth" runat="server"
                                                        Text='<%#Eval("TargetCellDepth")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div style="position: relative;">
                                                        <SweetSoft:ExtraInputMask ID="txtTargetCellDepth" RenderOnlyInput="true" Required="false" Width="100%"
                                                            runat="server" MaskType="Decimal" GroupSeparator="," RadixPoint="."
                                                            Text='<%#Eval("TargetCellDepth")%>' Digits="2" AutoGroup="true"></SweetSoft:ExtraInputMask>
                                                    </div>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Developing time/sec" HeaderStyle-CssClass="column-100 text-center" ItemStyle-CssClass="column-100">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbDevelopingTime" runat="server"
                                                        Text='<%#Eval("DevelopingTime")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div style="position: relative;">
                                                        <SweetSoft:ExtraInputMask ID="txtDevelopingTime" RenderOnlyInput="true" Required="false" Width="100%"
                                                            runat="server" MaskType="Integer" GroupSeparator="," RadixPoint="."
                                                            Text='<%#Eval("DevelopingTime")%>' Digits="0" AutoGroup="true"></SweetSoft:ExtraInputMask>
                                                    </div>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Etching time/sec" HeaderStyle-CssClass="column-100 text-center" ItemStyle-CssClass="column-100">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbEtchingTime" runat="server"
                                                        Text='<%#Eval("EtchingTime")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div style="position: relative;">
                                                        <SweetSoft:ExtraInputMask ID="txtEtchingTime" RenderOnlyInput="true" Required="false" Width="100%"
                                                            runat="server" MaskType="Decimal" GroupSeparator="," RadixPoint="."
                                                            Text='<%#Eval("EtchingTime")%>' Digits="2" AutoGroup="true"></SweetSoft:ExtraInputMask>
                                                    </div>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cell Size µ(Actual Value)" HeaderStyle-CssClass="column-100 text-center" ItemStyle-CssClass="column-100">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbChromeCellSize" runat="server"
                                                        Text='<%#Eval("ChromeCellSize")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div style="position: relative;">
                                                        <SweetSoft:ExtraInputMask ID="txtChromeCellSize" RenderOnlyInput="true" Required="false" Width="100%"
                                                            runat="server" MaskType="Decimal" GroupSeparator="," RadixPoint="."
                                                            Text='<%#Eval("ChromeCellSize")%>' Digits="2" AutoGroup="true"></SweetSoft:ExtraInputMask>
                                                    </div>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cell Wall µ(Actual Value)" HeaderStyle-CssClass="column-100 text-center" ItemStyle-CssClass="column-100">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbChromeCellWall" runat="server"
                                                        Text='<%#Eval("ChromeCellWall")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div style="position: relative;">
                                                        <SweetSoft:ExtraInputMask ID="txtChromeCellWall" RenderOnlyInput="true" Required="false" Width="100%"
                                                            runat="server" MaskType="Decimal" GroupSeparator="," RadixPoint="."
                                                            Text='<%#Eval("ChromeCellWall")%>' Digits="2" AutoGroup="true"></SweetSoft:ExtraInputMask>
                                                    </div>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cell Depth µ(Actual Value)" HeaderStyle-CssClass="column-100 text-center" ItemStyle-CssClass="column-100">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbChromeCellDepth" runat="server"
                                                        Text='<%#Eval("ChromeCellDepth")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div style="position: relative;">
                                                        <SweetSoft:ExtraInputMask ID="txtChromeCellDepth" RenderOnlyInput="true" Required="false" Width="100%"
                                                            runat="server" MaskType="Decimal" GroupSeparator="," RadixPoint="."
                                                            Text='<%#Eval("ChromeCellDepth")%>' Digits="2" AutoGroup="true"></SweetSoft:ExtraInputMask>
                                                    </div>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-CssClass="column-120 text-center" ItemStyle-CssClass="column-120 text-center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-link"
                                                        CommandArgument='<%# Eval("EngravingID") %>' CommandName="Edit">
                                                            <span class="fa fa-edit" style="font-size: 21px;vertical-align: -7px;"></span>
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
                                            <asp:TemplateField HeaderStyle-CssClass="column-80 text-center" ItemStyle-CssClass="column-80 text-center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="tbnDelete" Visible='<%#Convert.ToBoolean(Eval("IsCopy")) && Convert.ToInt32(Eval("CylinderID")) > 0%>'
                                                        runat="server" CssClass="btn btn-link"
                                                        CommandName="DeleteDetail" CommandArgument='<%#Eval("EngravingID") %>' Text="Delete">
                                            <span style="font-size:18px; vertical-align:middle " class="glyphicon glyphicon-remove"></span></span>
                                                    </asp:LinkButton>

                                                    <asp:LinkButton ID="btnGetCopy" Visible='<%#!Convert.ToBoolean(Eval("IsCopy")) && Convert.ToInt32(Eval("CylinderID")) > 0%>'
                                                        runat="server" CssClass="btn btn-link"
                                                        CommandName="Copy" CommandArgument='<%#Eval("EngravingID") %>' Text="Copy">
                                                    <span style="font-size:18px; vertical-align:middle" class="flaticon-files6"></span>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            There are currently no items in this table.
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                    <asp:HiddenField runat="server" ID="HiddenField2" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ModalPlaceHolder" runat="server">
    <asp:UpdatePanel runat="server" ID="upnlPrint" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="dialog-printing" title="Printing" style="background: #f8f8f8">
                <iframe src="" frameborder="0" width="100%" height="100%" style="min-height: 500px"></iframe>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptPlaceHolder" runat="server">
    <script src="/js/plugins/printThis.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            InitDialogPrintLink();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(Innit);
        });

        var oldScreen, oldAngle;
        var oldStylus, oldSh;
        function Innit() {
            if ($("input[id$='txtScreen']").length > 0) {
                oldScreen = $("input[id$='txtScreen']").val();
                $("input[id$='txtScreen']").blur(function () {
                    SelectValues();
                });
            }
            if ($("input[id$='txtAngle']").length > 0) {
                oldAngle = $("input[id$='txtAngle']").val();
                $("input[id$='txtAngle']").blur(function () {
                    SelectValues();
                });
            }

            if ($("input[id$='txtSh']").length > 0) {
                oldSh = $("input[id$='txtSh']").val();
                $("input[id$='txtSh']").blur(function () {
                    SelectCellDepth();
                });
            }
            if ($("input[id$='txtStylus']").length > 0) {
                oldStylus = $("input[id$='txtStylus']").val();
                $("input[id$='txtStylus']").blur(function () {
                    SelectCellDepth();
                });
            }
        }

        function SelectValues() {
            var screen, angle;
            screen = $("input[id$='txtScreen']").val();
            angle = $("input[id$='txtAngle']").val();
            if (screen.length > 0 && angle.length > 0) {
                if (screen !== oldScreen || angle !== oldAngle) {
                    oldScreen = screen;
                    oldAngle = angle;

                    console.log("screen", screen, "  ", "angle", angle);
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "JobEngraving.aspx/SelectValues",
                        data: "{'Keyword':'" + screen + '|' + angle + "'}",
                        dataType: "json",
                        async: true,
                        cache: false,
                        success: function (result) {
                            if (result.d !== 'null') {
                                var myArray = $.parseJSON(result.d);
                                if (myArray.length > 0) {



                                    if ($("input[id$='txtSh']").length > 0) {
                                        $("input[id$='txtSh']").val(myArray[0]);
                                    }

                                    if ($("input[id$='txtHl']").length > 0) {
                                        $("input[id$='txtHl']").val(myArray[1]);
                                    }

                                    if ($("input[id$='txtCh']").length > 0) {
                                        $("input[id$='txtCh']").val(myArray[2]);
                                    }

                                    if ($("input[id$='txtMt']").length > 0) {
                                        $("input[id$='txtMt']").val(myArray[3]);
                                    }

                                    SelectCellDepth();
                                }
                                else {

                                    $("input[id$='txtSh']").val("");
                                    $("input[id$='txtHl']").val("");
                                    $("input[id$='txtCh']").val("");
                                    $("input[id$='txtMt']").val("");
                                }
                            }
                            else {
                                $("input[id$='txtSh']").val("");
                                $("input[id$='txtHl']").val("");
                                $("input[id$='txtCh']").val("");
                                $("input[id$='txtMt']").val("");
                            }
                        }
                    });
                }
            }
        }
        function SelectCellDepth() {
            var stylus, sh;
            stylus = $("input[id$='txtStylus']").val();
            sh = $("input[id$='txtSh']").val();
            if (stylus.length > 0 && sh.length > 0) {
                if (stylus !== oldStylus || sh !== oldSh) {
                    oldStylus = stylus;
                    oldSh = sh;

                    console.log("stylus", stylus, "SH", sh);

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "JobEngraving.aspx/SelectCellDepth",
                        data: "{'Keyword':'" + stylus + '|' + sh + "'}",
                        dataType: "json",
                        async: true,
                        cache: false,
                        success: function (result) {
                            if (result.d !== 'null') {
                                var array = $.parseJSON(result.d);
                                if (array.length > 0) {
                                    if ($("input[id$='txtCellDepth']").length > 0) {
                                        $("input[id$='txtCellDepth']").val(array[0]);
                                    }
                                }
                                else {
                                    $("input[id$='txtCellDepth']").val("");
                                }
                            }
                            else {
                                $("input[id$='txtCellDepth']").val("");
                            }
                        }
                    });
                }
            }
        }
        addRequestHanlde(InitDialogPrintLink);
        function InitDialogPrintLink() {
            $(".openPrinting a").on('click', function (e) {



                var hrefLink = $(this).data("href");
                console.log("hrefLink", hrefLink);

                var iframe = $("#dialog-printing").find('iframe');

                iframe.attr("src", hrefLink);

                $("#dialog-printing").dialog({
                    autoOpen: false,
                    height: 'auto',
                    width: '980',
                    modal: true,
                    appendTo: "form",
                    resizable: false,
                    buttons: [
                        {
                            text: "Print",
                            "class": 'btn btn-primary',
                            click: function () {
                                var content = $(this).find('iframe');

                                console.log("content", content);

                                var clonePrint = content.contents().find("html");
                                console.log("clonePrint", clonePrint);

                                var script = clonePrint.find('script').remove();
                                console.log("script", script);

                                console.log($(clonePrint));
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
        }

    </script>
</asp:Content>
