<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JobInvoiceControl.ascx.cs"
    Inherits="SweetSoft.APEM.WebApp.Controls.JobInvoiceControl" %>
<div class="panel panel-default" style="margin-bottom: 5px;">
    <div class="panel-heading" role="tab" id="headingOne" style="cursor: pointer;">
        <asp:Literal runat="server" ID="ltrCollapTitle"></asp:Literal>
    </div>

    <asp:Literal runat="server" ID="ltrCollapseBody"></asp:Literal>
    <div class="panel-body">
        <div class="row">
            <div class="col-md-6 col-sm-6">
                <div class="form-group">
                    <label class="control-label">
                        Job Name
                    </label>
                    <asp:TextBox runat="server" ID="txtJobNameValue" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-6 col-sm-6">
                <div class="form-group">
                    <label class="control-label">
                        Design
                    </label>
                    <asp:TextBox runat="server" ID="txtDesignValue" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 col-sm-12">
                <div class="form-group">
                    <div class="row">
                        <div class="form-group">
                            <div class="col-md-3 col-sm-3">
                                <label class="control-label">
                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CUSTOMER_PO)%>
                                </label>
                                <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtCustomerPO1" runat="server" CssClass="form-control" ReadOnly="true"
                                    RequiredText='<%# SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.THIS_FIELD_IS_REQUIRED)%>'></SweetSoft:CustomExtraTextbox>
                            </div>
                            <div class="col-md-3 col-sm-3">
                                <label class="control-label">&nbsp;</label>
                                <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtCustomerPO2" runat="server" CssClass="form-control" ReadOnly="true"
                                    RequiredText='<%# SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.THIS_FIELD_IS_REQUIRED)%>'></SweetSoft:CustomExtraTextbox>
                            </div>

                            <div class="col-md-3 col-sm-3">
                                <label class="control-label">
                                    Sub Total
                                </label>
                                <asp:TextBox runat="server" ID="txtTotal" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                            </div>

                            <div class="col-md-3 col-sm-3">
                                <label class="control-label">
                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.TOTAL_TITLE)%>
                                </label>
                                <SweetSoft:ExtraInputMask ID="txtNetTotal" RenderOnlyInput="true" Required="false" ReadOnly="true"
                                    runat="server" MaskType="Decimal" GroupSeparator="," RadixPoint="." Text="0" Digits="3" AutoGroup="true"></SweetSoft:ExtraInputMask>
                            </div>

                        </div>
                    </div>
                </div>
            </div>

        </div>

        <div class="form-group" runat="server" id="divCylinder">
            <label class="control-label"><%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CYLINDERS)%></label>
            <asp:UpdatePanel runat="server" ID="upnlCylinder" UpdateMode="Conditional">
                <ContentTemplate>
                    <SweetSoft:GridviewExtension ID="gvClinders"
                        runat="server" AutoGenerateColumns="false"
                        CssClass="table table-striped table-bordered table-checkable dataTable"
                        DataKeyNames="CylinderID"
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
                                    <asp:Label runat="server" Text='<%#ShowNumberFormat(Eval("Circumference"))%>' ID="lblCircumfere"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Face width">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#ShowNumberFormat(Eval("FaceWidth"))%>' ID="lblFaceWidth"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Unit Price">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# ShowNumberFormat(Eval("UnitPrice"))%>' ID="lblUnitPrice"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Qty">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("Quantity")%>' ID="lblQty"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Total Price">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#string.Format("{0}",ShowNumberFormat(Eval("TotalPrice"))) %>' ID="lblPriceTaxed"></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdfPriceTaxed" Value='<%#Eval("TotalPrice") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </SweetSoft:GridviewExtension>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div class="form-group" runat="server" id="divOtherCharger">
            <label class="control-label">
                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.OTHER_CHARGES)%></label>
            <asp:UpdatePanel runat="server" ID="upnlOtherCharges" UpdateMode="Conditional">
                <ContentTemplate>
                    <SweetSoft:GridviewExtension ID="grvOtherCharges"
                        runat="server" AutoGenerateColumns="false"
                        CssClass="table table-striped table-bordered table-checkable dataTable"
                        DataKeyNames="OtherChargesID"
                        GridLines="None" AllowPaging="false" AllowSorting="false">
                        <Columns>
                            <asp:TemplateField HeaderText="Description">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblDescription" Text='<%#Eval("Description")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div style="position: relative;">
                                        <asp:TextBox runat="server" CssClass="form-control" ID="txtDescription" Text='<%#Eval("Description")%>'></asp:TextBox>
                                    </div>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="column-large" HeaderText="Quantity">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblQuantity" Text='<%#Eval("Quantity")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div style="position: relative;">
                                        <SweetSoft:ExtraInputMask ID="txtQuantity" RenderOnlyInput="true" Required="false"
                                            runat="server" MaskType="Integer" GroupSeparator="," Text='<%#Eval("Quantity")%>' AutoGroup="true"></SweetSoft:ExtraInputMask>
                                    </div>
                                </EditItemTemplate>

                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Charge" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="column-large">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCharge" Text='<%#ShowNumberFormat(Eval("Charge"))%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div style="position: relative;">
                                        <SweetSoft:ExtraInputMask ID="txtCharge" RenderOnlyInput="true" Required="false"
                                            runat="server" MaskType="Decimal" GroupSeparator="," RadixPoint="." Text='<%#ShowNumberFormat(Eval("Charge"))%>' Digits="2" AutoGroup="true"></SweetSoft:ExtraInputMask>
                                    </div>
                                </EditItemTemplate>

                            </asp:TemplateField>
                        </Columns>
                    </SweetSoft:GridviewExtension>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>



        <div class="form-group" runat="server" id="divJobService">
            <label class="control-label">
                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.SERVICE_JOB)%></label>
            <asp:GridView ID="grvServiceJobDetail" runat="server"
                AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-checkable dataTable"
                GridLines="None"
                AllowPaging="false"
                AllowSorting="false"
                ShowFooter="false"
                DataKeyNames="ServiceJobID"
                OnRowDataBound="grvServiceJobDetail_RowDataBound"
                OnDataBound="grvServiceJobDetail_DataBound">
                <Columns>
                    <asp:TemplateField HeaderText="No">
                        <ItemTemplate>
                            <asp:Label ID="lblNo" Text="1" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Work order number">
                        <ItemTemplate>
                            <asp:Label ID="lblWorkOrderNumber" runat="server"
                                Text='<%#Eval("WorkOrderNumber")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-CssClass="column-one" HeaderText="ProductID">
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
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>
</div>

<asp:HiddenField runat="server" ID="hdfJobID" />
