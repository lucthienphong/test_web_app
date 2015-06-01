<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuotationPrinting.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.Printing.QuotationPrinting" EnableViewState="false" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title></title>
    <link href="/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body
        {
            margin-top: 20px;
        }

        table,
        .table-bordered > thead > tr > th, .table-bordered > tbody > tr > th, .table-bordered > tfoot > tr > th, .table-bordered > thead > tr > td, .table-bordered > tbody > tr > td, .table-bordered > tfoot > tr > td
        {
            border-color: #000 !important;
            padding-top: 5px;
            padding-bottom: 5px;
        }

        small
        {
            color: #000!important;
        }

        @media all
        {
            .cylinderTable tr td:first-child, .cylinderTable tr th:first-child
            {
                border-left-width: 0;
            }

            .cylinderTable tr th:last-child,
            .cylinderTable tr td:last-child
            {
                border-right-width: 0;
            }

            .cylinderTable tr:last-child td
            {
                border-bottom-width: 0;
            }
        }

        @page
        {
            margin: 0mm;
            margin-top: 5mm;
        }

        @media print
        {
            body
            {
                margin: 0;
            }

            h5
            {
                margin-top: 1mm;
                margin-bottom: 1mm;
                font-size: 0.7em;
            }

            [class^="col-xs-"]
            {
                /*padding-left: 0mm;
                padding-right: 0mm;*/
            }

            .no-border-left
            {
                border-left-color: #fff !important;
                border-left-color: transparent !important;
            }

            .no-border-right
            {
                border-right-color: #fff !important;
                border-right-color: transparent !important;
            }

            label
            {
                margin-top: 7px;
            }
            table{
                margin-bottom:5px !important;
            }
            .table-bordered > thead > tr > th, .table-bordered > tbody > tr > th, .table-bordered > tfoot > tr > th, .table-bordered > thead > tr > td, .table-bordered > tbody > tr > td, .table-bordered > tfoot > tr > td
            {
                padding-top: 4px !important;
                padding-bottom: 4px !important;
                background:#fff !important;
            }
        }
    </style>
</head>
<body style="background: #fff">
    <form id="form1" runat="server">
        <div class="container-fluid" id="wrapPrint">
            <div class="printContent">
                <div class="row">
                    <div class="col-xs-12 text-right" style="margin-bottom: 0px">
                        <span class="form-control-static">
                            <strong><asp:Literal Text="Asia-Pacific Engravers (Malaysia) Sdn. Bhd." ID="ltrCompanyName" EnableViewState="false" runat="server" /></strong>
                        </span>
                        <img src="/img/logo-print1.png" class="img-responsive" style="height: 12mm; vertical-align: bottom; display: inline-block" />
                        <h6><asp:Literal ID="ltrCompanyAddress" Text="Lot121, Jalan Permata 2, Arab-Malaysian Industrial Park, 71800 Nilai, Negeri Sebilan D.K." runat="server" />
                            <br />
                            Tel: <asp:Literal Text="+60 (06) 7997812." EnableViewState="false" ID="ltrCompanyPhone" runat="server" /> . Fax: <asp:Literal Text="60 (06) 799 7816" EnableViewState="false" id="ltrCompanyFax" runat="server" /> . ISDN: <asp:Literal Text="+60 (06) 798 1125" ID="ltrISDN" EnableViewState="false" runat="server" /> . Email: <asp:Literal Text="ape@ape.com.my" ID="ltrCompanyEmail" EnableViewState="false" runat="server" />
                        </h6>
                    </div>
                </div>

                <div class="row">
                    <div class="col-xs-12">
                        <h3 style="font-weight:700; ">
                            Pricing master
                        </h3>
                    </div>
                </div>

                <div class="row">
                    <div class="col-xs-6">
                        <asp:Literal Text="" ID="ltrAddress" EnableViewState="false" runat="server" />
                    </div>
                    <div class="col-xs-6">
                        <div class="form-horizontal" style="direction: ltr;">
                            <div class="form-group" style="margin-top: 15px;">
                                <label class="control-label col-xs-12 text-right">
                                    <asp:Literal ID="ltrDate" Text="Date" runat="server" EnableViewState="false" /></label>
                            </div>
                        </div>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-xs-12">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div class="col-xs-12">
                                    <p class="form-control-static">
                                        <asp:Literal ID="ltrContact" Text="" EnableViewState="false" runat="server" />
                                    </p>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-xs-12">
                                    <p class="form-control-static">
                                        Based on our general terms and conditions of sales we confirm our Quotation as follows
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-12" id="divPricing" runat="server">
                        <label class="control-label">Pricing master</label>
                        <asp:GridView ID="grvPrices" runat="server" AutoGenerateColumns="false"
                            CssClass="table table-striped table-bordered" GridLines="None"
                            AllowPaging="false" AllowSorting="false" DataKeyNames="CustomerID, Id">
                            <Columns>
                                <asp:TemplateField HeaderText="Pricing Name" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lbPricingName" runat="server" Text='<%# Eval("PricingName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="New Steel Base" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label Text='<%#Convert.ToDecimal(Eval("NewSteelBase")).ToString("N3") 
                                            + string.Format(" {0}/{1}", Eval("CurrencyName"), Eval("UnitOfMeasure")) %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Old Steel Base" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" Text='<%#Convert.ToDecimal(Eval("OldSteelBase")).ToString("N3") 
                                            + string.Format(" {0}/{1}", Eval("CurrencyName"), Eval("UnitOfMeasure")) %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>

                    <div class="col-xs-12" id="divAdditional" runat="server">
                        <label class="control-label">Additional Services</label>
                        <asp:GridView ID="grvAdditionalServices" runat="server" AutoGenerateColumns="false"
                            CssClass="table table-striped table-bordered" GridLines="None"
                            AllowPaging="false" AllowSorting="false" DataKeyNames="CustomerID, Id">
                            <Columns>
                                <asp:TemplateField HeaderText="Additional services" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lbDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Price" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lbPrice" Text='<%#Convert.ToDecimal(Eval("Price")).ToString("N3") 
                                            + string.Format(" {0}", Eval("CurrencyName")) %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>

                    <div class="col-xs-12" id="divOthers" runat="server">
                        <label class="control-label">Other charges</label>
                        <asp:GridView ID="grvOtherCharges" runat="server" AutoGenerateColumns="false"
                            CssClass="table table-striped table-bordered" GridLines="None"
                            AllowPaging="false" AllowSorting="false" DataKeyNames="CustomerID, Id">
                            <Columns>
                                <asp:TemplateField HeaderText="Additional services" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lbDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Price" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lbPrice" Text='<%#Convert.ToDecimal(Eval("Price")).ToString("N3") 
                                            + string.Format(" {0}", Eval("CurrencyName")) %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12">
                        <label class="control-label">
                            Note: 
                        </label>
                    </div>
                    <div class="col-xs-12">
                        <div style="border: 1px solid #000; min-height: 50mm; min-height: 100px; padding: 5px;">
                            <asp:Literal Text="" ID="ltrNotes" runat="server" />
                        </div>
                    </div>
                </div>
                <br />
                <div class="form-horizontal">
                    <div class="form-group">
                        <label for="" class="col-xs-2 control-label text-left">Delivery Terms: </label>
                        <div class="col-xs-9">
                            <p class="form-control-static">
                                <asp:Literal ID="ltrDeliveryTerms" Text="" EnableViewState="false" runat="server" />
                            </p>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="" class="col-xs-2 control-label text-left">Payment Terms: </label>
                        <div class="col-xs-9">
                            <p class="form-control-static">
                                <asp:Literal ID="ltrPaymentTerms" Text="" EnableViewState="false" runat="server" />
                            </p>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-6">
                        <span style="font-size: 10pt;">Yours faithfully,</span><br />
                        <span style="font-size: 10pt; text-transform: uppercase">Asia Pacific Engravers (Malaysia) Sdn Bhd</span>
                        <br />
                        <br />
                        <br />
                        <hr style="border-color: #000!important; width: 80%; margin-left: 0;" />
                        <br />
                    </div>
                    <div class="col-xs-push-2 col-xs-4">
                        Kindly acknowledge receipt of this quotation. Thank you.
                        <br />
                        <br />
                        <br />
                        <hr style="border-color: #000!important" />
                        Name:
                        <br />
                        Designation:
                        <asp:Literal ID="ltrDesignation" EnableViewState="false" runat="server" />
                        <br />
                        I/C No: 
                    </div>
                </div>
            </div>
        </div>
        </div>
    </form>

</body>
</html>
