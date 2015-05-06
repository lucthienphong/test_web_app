<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintDetailInvoice.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.Printing.PrintDetailInvoice" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
    <link href="/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        @media all
        {
            .control-label
            {
                font-weight: normal;
            }
        }

        @media print
        {
            table,
            .table-bordered > thead > tr > th, .table-bordered > tbody > tr > th, .table-bordered > tfoot > tr > th, .table-bordered > thead > tr > td, .table-bordered > tbody > tr > td, .table-bordered > tfoot > tr > td
            {
                border-color: #000 !important;
                padding-top: 5px;
                padding-bottom: 5px;
                font-size: 9pt!important;
            }

            body
            {
                font-size: 9pt !important;
            }

            label, p
            {
                font-size: 9pt!important;
            }

                p.form-control-static
                {
                    padding: 0;
                    margin: 0;
                }
        }

        @page
        {
            margin: 0mm;
            margin-top: 5mm;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid" id="wrapPrint">
            <div class="printContent">
                <asp:Repeater runat="server" ID="rptJobInvoice" OnItemDataBound="rptJobInvoice_ItemDataBound">
                    <ItemTemplate>
                        <hr class="hidden-print" style="border-style: dashed; border-color: #C5C5C5; margin: 40px 0 30px; "/>
                        <div style="page-break-before: always;">
                            <div class="row">
                                <div class="col-xs-offset-9 col-xs-3">
                                    <img src="/img/logo-print1.png" class="img-responsive" style="vertical-align: bottom; display: inline-block" />
                                </div>
                                <div class="col-xs-offset-1 col-xs-8" style="margin-bottom: 0px">
                                    <h6>
                                        <asp:Literal Text="Asia-Pacific Engravers (Malaysia) Sdn. Bhd. Lot121, Jalan Permata 2, Arab-Malaysian Industrial Park, 71800 Nilai, Negeri Sebilan D.K - Malaysia" ID="ltrCompany" EnableViewState="false" runat="server" />
                                    </h6>
                                </div>
                                <div class="col-xs-3">
                                    <h6><span style="width: 13mm; display: inline-block">Phone</span>:
                            <asp:Literal Text="+60 (06) 7997812." EnableViewState="false" ID="ltrCompanyPhone" runat="server" />
                                        <br />
                                        <span style="width: 13mm; display: inline-block">Fax</span>:
                            <asp:Literal Text="60 (06) 799 7816" EnableViewState="false" ID="ltrCompanyFax" runat="server" />
                                        <br />
                                        <span style="letter-spacing: 0.5mm">
                                            <asp:Literal Text="www.ape.com.my" ID="ltrCompanyWebsite" EnableViewState="false" runat="server" />
                                        </span>
                                    </h6>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12">
                                    <h3 style="font-weight: 700;">DETAIL INVOICE
                                    </h3>
                                </div>
                                <div class="col-xs-6">
                                    <asp:Literal Text="" ID="ltrCustomer" EnableViewState="false" runat="server" />
                                </div>
                                <div class="col-xs-6">
                                    <div class="form-horizontal" style="direction: ltr;">
                                        <div class="form-group">
                                            <label class="control-label col-xs-8 text-right" style="font-weight: 300">APE Invoice No</label>
                                            <label class="control-label col-xs-4 text-left" style="font-weight: 300">
                                                : 
                                    <asp:Literal ID="ltrInvoiceNo" Text="" runat="server" EnableViewState="false" /></label>

                                            <label class="control-label col-xs-8 text-right" style="font-weight: 300">
                                                Date</label>
                                            <label class="control-label col-xs-4 text-left" style="font-weight: 300">
                                                : 
                                    <asp:Literal ID="ltrDate" runat="server" EnableViewState="false" /></label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-6">
                                    <div class="form-horizontal">
                                        <br />
                                        <div class="row">
                                            <label class="control-label col-xs-4 text-left">Attn</label>
                                            <label class="control-label text-left col-xs-7">
                                                :&nbsp;&nbsp;
                                    <asp:Literal Text="" ID="ltrContact" runat="server" EnableViewState="false" /></label>
                                            <label class="control-label col-xs-4 text-left">Your Reference</label>
                                            <label class="control-label text-left col-xs-7">
                                                :&nbsp;&nbsp;
                                    <asp:Literal Text="" ID="ltrReference" runat="server" EnableViewState="false" /></label>

                                            <label class="control-label col-xs-4 text-left">Job Name</label>
                                            <label class="control-label text-left col-xs-7">
                                                :&nbsp;&nbsp;
                                    <asp:Literal Text="" ID="ltrJobName" runat="server" EnableViewState="false" /></label>

                                            <label class="control-label col-xs-4 text-left">Design</label>
                                            <label class="control-label text-left col-xs-7">
                                                :&nbsp;&nbsp;
                                    <asp:Literal Text="" ID="ltrDesign" EnableViewState="false" runat="server" /></label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <br />
                                    <asp:GridView ID="gvClinders" runat="server" AutoGenerateColumns="false"
                                        CssClass="table table-bordered" GridLines="None"
                                        OnRowDataBound="gvClinders_RowDataBound" ShowFooter="true"
                                        AllowPaging="false" AllowSorting="false" DataKeyNames="CylinderID">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Seq.">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblNo" Text='<%#Eval("Sequence")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cyl. No">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblNo" Text='<%#Eval("CylinderNo")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Description">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#Eval("CylDescription")%>' ID="lblCylDescription"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Steel Base">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#Eval("SteelBaseName")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Width (mm)">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#ShowNumberFormat(Eval("FaceWidth"))%>' ID="lblFaceWidth"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cirf (mm)">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#ShowNumberFormat(Eval("Circumference"))%>' ID="lblCircumfere"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Qty (pcs)">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#Eval("Quantity")%>' ID="lblQty"></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Price (RM/sqcm)">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#Eval("UnitPrice")%>' ID="lblQty"></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total Price (RM)">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#Eval("TotalPrice")%>' ID="lblQty"></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12">
                                    <asp:Literal ID="ltrServiceJob" runat="server" />
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

                                            <asp:TemplateField HeaderText="Tax code">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTaxCode" runat="server"
                                                        Text='<%#ShowTaxCode(Eval("TaxID"))%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax rate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTaxRate" runat="server"
                                                        Text='<%#ShowNumberFormat(Eval("TaxPercentage"))%>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalText" Text='<%# SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.TOTAL)%>'></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="column-large" HeaderText="Work order values">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWorkOrderValues" runat="server"
                                                        Text='<%#Eval("WorkOrderValues")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="column-large" HeaderText="Taxed">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWorkOrderValuesTaxed" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-7">
                                    <div class="row">
                                        <div class="col-xs-12">
                                            <div class="form-group">
                                                <label class="control-label" style="font-weight: 700">
                                                    Other Charges
                                                </label>
                                                <asp:GridView runat="server" ID="gvOtherCharges" AutoGenerateColumns="false"
                                                    CssClass="table table-bordered" GridLines="None"
                                                    AllowPaging="false" AllowSorting="false">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Description">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label1" runat="server" Text='<%#Eval("Description")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Charge (RM)">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label1" runat="server" Text='<%#Eval("Charge")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Quantity">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label2" runat="server" Text='<%#Eval("Quantity")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-xs-5">
                                    <label class="control-label">
                                        &nbsp;
                                    </label>
                                    <div class="well" style="border-radius: 0">
                                        <div class="row">
                                            <div class="col-xs-5">
                                                <strong style="font-size: 9pt">Net Total</strong>
                                            </div>
                                            <div class="col-xs-7 text-right" style="direction: rtl">
                                                <strong style="font-size: 9pt">RM
                                        <asp:Literal ID="ltrNetTotal" EnableViewState="false" runat="server" />
                                                    <br />
                                                    <asp:Literal ID="ltrCurrency" EnableViewState="false" runat="server" />
                                                    <br />
                                                </strong>
                                            </div>
                                            <div class="col-xs-12 text-right">
                                                <strong><span style="font-size: 7pt">
                                                    <asp:Literal Text="" ID="ltrBaseCurrency" EnableViewState="false" runat="server" /></span></strong>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <label class="col-xs-2 control-label text-left" style="font-weight: 700">Delivery Terms: </label>
                                            <div class="col-xs-10">
                                                <p class="form-control-static">
                                                    <asp:Literal ID="ltrDeliveryTerm" Text="" EnableViewState="false" runat="server" />
                                                </p>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-xs-2 control-label text-left" style="font-weight: 700">Payment Terms: </label>
                                            <div class="col-xs-10">
                                                <p class="form-control-static">
                                                    <asp:Literal ID="ltrPaymentTerms" Text="" EnableViewState="false" runat="server" />
                                                </p>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-push-7 col-xs-5">
                                    <span style="font-size: 10pt;">Yours faithfully,</span><br />
                                    <span style="font-size: 10pt; text-transform: uppercase">Asia Pacific Engravers (Malaysia) Sdn Bhd</span>
                                    <br />
                                    <br />
                                    <br />
                                    <br />
                                    <hr style="border-color: #000!important; width: 80%; margin-left: 0; margin-bottom: 0" />
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </form>
</body>
</html>
