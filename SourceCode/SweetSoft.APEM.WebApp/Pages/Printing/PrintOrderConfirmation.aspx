<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintOrderConfirmation.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.Printing.PrintOrderConfirmation" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
    <link href="/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body {
            margin-top: 20px;
        }

        table,
        .table-bordered > thead > tr > th, .table-bordered > tbody > tr > th, .table-bordered > tfoot > tr > th, .table-bordered > thead > tr > td, .table-bordered > tbody > tr > td, .table-bordered > tfoot > tr > td {
            border-color: #000 !important;
        }

        label {
            font-weight: 300!important;
        }

        small {
            color: #000!important;
        }

        @media all {
            body {
                font-size: 12px !important;
            }

            .information-do label.control-label, .information-do p.form-control-static {
                margin-bottom: 3px!important;
            }

            .form-group {
                margin-bottom: 0px;
            }

            .control-label {
                padding-top: 0px !important;
                margin-bottom: 0 !important;
            }

            .form-control-static {
                padding-top: 0;
                padding-bottom: 0;
            }
        }

        @page {
            size: auto;
            margin: 20mm 6mm 20mm 6mm;
            margin-top: 20mm;
        }

        @media print {
            body {
                margin: 0;
            }

            .form-group {
                margin-bottom: 0;
            }

            h5 {
                margin-top: 1mm;
                margin-bottom: 1mm;
                font-size: 0.7em;
            }

            [class^="col-xs-"] {
                /*padding-left: 0mm;
                padding-right: 0mm;*/
            }

            .no-border-left {
                border-left-color: #fff !important;
                border-left-color: transparent !important;
            }

            .no-border-right {
                border-right-color: #fff !important;
                border-right-color: transparent !important;
            }

            div.uniform-checker.uniform-disabled span.uniform-checked {
                background-position: -114px -260px;
            }
        }
    </style>
</head>
<body style="background: #fff">
    <form id="form1" runat="server">
        <div class="container-fluid" id="wrapPrint">
            <div class="printContent">
                <div class="row" style="margin-bottom:20px;">
                    <div class="col-sm-2 col-xs-2">
                        <img src="/img/apem-logo-print.png" class="img-responsive" />
                    </div>
                    <div class="col-sm-10 col-xs-10">
                        <asp:Literal runat="server" ID="ltrCompany" EnableViewState="false"></asp:Literal>
                    </div>
                </div>
                <div class="row" style="display: none">
                    <div class="col-xs-12 text-right" style="margin-bottom: 15px">
                        <span class="form-control-static">
                            <strong>
                                <asp:Literal Text="Asia-Pacific Engravers (Malaysia) Sdn. Bhd. Lot121, Jalan Permata 2, Arab-Malaysian Industrial Park, 71800 Nilai, Negeri Sebilan D.K - Malaysia" ID="ltrCompanyName" EnableViewState="false" runat="server" /></strong>
                        </span>
                        <img src="/img/apem-logo-print.png" class="img-responsive" style="height: 12mm; vertical-align: bottom; display: inline-block" />
                        <h6>
                            <asp:Literal ID="ltrCompanyAddress" Text="Lot121, Jalan Permata 2, Arab-Malaysian Industrial Park, 71800 Nilai, Negeri Sebilan D.K." runat="server" />
                            <br />
                            Tel:
                            <asp:Literal Text="+60 (06) 7997812." EnableViewState="false" ID="ltrCompanyPhone" runat="server" />
                            . Fax:
                            <asp:Literal Text="60 (06) 799 7816" EnableViewState="false" ID="ltrCompanyFax" runat="server" />
                            . ISDN:
                            <asp:Literal Text="+60 (06) 798 1125" ID="ltrISDN" EnableViewState="false" runat="server" />
                            . Email:
                            <asp:Literal Text="ape@ape.com.my" ID="ltrCompanyEmail" EnableViewState="false" runat="server" />
                        </h6>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12">
                        <h3 style="font-weight: 700; margin-top: 0">Order Confirmation
                        </h3>
                    </div>
                    <div class="col-xs-6">
                        <asp:Literal Text="" ID="ltrCustomer" EnableViewState="false" runat="server" />
                    </div>
                    <div class="col-xs-4 col-xs-offset-2 information-do">
                        <div class="form-group">
                            <label class="control-label" style="font-weight: 300">Number / Date:</label>
                            <p class="form-control-static">
                                <strong>
                                    <asp:Literal ID="ltrOCNumber" Text="" runat="server" EnableViewState="false" /></strong>
                                /
                                <asp:Literal ID="ltrOCDate" runat="server" EnableViewState="false" />
                            </p>
                            <label class="control-label" style="font-weight: 300">APE Order No. / Date</label>
                            <p class="form-control-static">
                                <strong>
                                    <asp:Literal ID="ltrJobNumber" Text="" runat="server" EnableViewState="false" />
                                    /
                                <asp:Literal ID="ltrJobDate" runat="server" EnableViewState="false" />
                                </strong>
                            </p>
                            <label class="control-label" style="font-weight: 300">
                                Billing Currency:
                                <asp:Literal ID="ltrCurr" runat="server" EnableViewState="false" /></label>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12">
                        <label class="control-label"><strong>Requirement:</strong></label>
                        <div class="clearfix">
                            <div class="form-group clearfix">
                                <ul class="pull-left list-unstyled">

                                    <li>Terms of delivery
                                    </li>
                                    <li>Terms of payment
                                    </li>
                                    <li>
                                        <label class="control-label"></label>
                                    </li>
                                    <li>Attn
                                    </li>
                                    <li>Your Reference
                                    </li>
                                    <li>Job Name
                                    </li>
                                    <li>Design
                                    </li>
                                </ul>
                                <ul class="list-unstyled pull-left" style="margin-left: 15px;">
                                    <li>:
                                            <asp:Literal ID="ltrDeliveryTerm" Text="" EnableViewState="false" runat="server" />
                                    </li>
                                    <li>:
                                            <asp:Literal ID="ltrPaymentTerms" Text="" EnableViewState="false" runat="server" />
                                    </li>
                                    <li>
                                        <label class="control-label"></label>
                                    </li>
                                    <li>:
                                        <asp:Literal Text="" ID="ltrContact" runat="server" EnableViewState="false" />
                                    </li>
                                    <li>:
                                            <asp:Literal Text="" ID="ltrReferences" runat="server" EnableViewState="false" />
                                    </li>
                                    <li>:
                                            <asp:Literal Text="" ID="ltrJobName" runat="server" EnableViewState="false" />
                                    </li>
                                    <li>:
                                            <asp:Literal Text="" ID="ltrDesign" EnableViewState="false" runat="server" />
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-12">
                        Based on our general terms and conditions of sales we confirm your order as follows.
                        <br />
                        <table class="table table-bordered">
                            <thead>
                                <tr>
                                    <th>No</th>
                                    <%--<th>Cyl Barcode</th>--%>
                                    <th>Description</th>
                                    <%--<th>Cylinder ID/ Product ID</th>--%>
                                    <th>Cus Cyl No</th>
                                    <th>Cus Steelbase ID</th>
                                    <th>Steelbase</th>
                                    <th>Width (mm)</th>
                                    <th>Circumf. (mm)</th>
                                    <th>Qty Pcs.</th>
                                    <th>Unit Price</th>
                                    <th>Total</th>
                                </tr>
                            </thead>
                            <asp:Repeater ID="rptCylinder" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" Text='<%#Eval("Sequence")%>'></asp:Label></td>
                                        <%--<td>
                                            <asp:Label runat="server" Text='<%#Eval("CylBarcode")%>'></asp:Label></td>--%>
                                        <td>
                                            <asp:Label runat="server" Text='<%#Eval("CylDescription")%>'></asp:Label></td>
                                        <%--<td>
                                            <asp:Label runat="server" Text='<%#Eval("CylinderNo")%>'></asp:Label></td>--%>
                                        <td>
                                            <asp:Label runat="server" Text='<%#Eval("CusCylID")%>'></asp:Label></td>
                                        <td>
                                            <asp:Label runat="server" Text='<%#Eval("CusSteelBaseID")%>'></asp:Label></td>
                                        <td>
                                            <asp:Label runat="server" Text='<%#Eval("SteelBaseName")%>'></asp:Label></td>
                                        <td style="text-align: right">
                                            <asp:Label runat="server" Text='<%#ShowNumberFormat(Eval("FaceWidth"), "N2")%>'></asp:Label></td>
                                        <td style="text-align: right">
                                            <asp:Label runat="server" Text='<%#ShowNumberFormat(Eval("Circumference"), "N2")%>'></asp:Label></td>
                                        <td style="text-align: right">
                                            <asp:Label runat="server" Text='<%#Eval("Quantity")%>' ID="lblQty"></asp:Label></td>
                                        <td style="text-align: right">
                                            <asp:Label runat="server" Text='<%#Convert.ToDecimal(Eval("UnitPrice")).ToString("N3")%>'></asp:Label></td>
                                        <td style="text-align: right">
                                            <asp:Label runat="server" Text='<%#Convert.ToDecimal(Eval("TotalPrice")).ToString("N2")%>'></asp:Label></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            <%--Services charges--%>
                            <asp:Repeater ID="rptServicesCharges" runat="server">
                                <HeaderTemplate>
                                    <tr>
                                        <td colspan="10"><strong>Services charges</strong></td>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td colspan="9">
                                            <asp:Label Text='<%#Eval("WorkOrderNumber") %>' runat="server" />
                                            <asp:Label Text='<%#Eval("Description") %>' runat="server" />
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label Text='<%#Convert.ToDecimal(Eval("WorkOrderValues")).ToString("N2") %>' runat="server" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            <%--Other charges--%>
                            <asp:Repeater ID="rptOtherCharges" runat="server">
                                <HeaderTemplate>
                                    <tr>
                                        <td colspan="10"><strong>Other charges</strong></td>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td colspan="7">
                                            <%#((SweetSoft.APEM.WebApp.Pages.Printing.TblOtherChargeExtension)Container.DataItem).tblOtherCharges.Description%>
                                        </td>
                                        <td style="text-align: right">
                                            <%#((SweetSoft.APEM.WebApp.Pages.Printing.TblOtherChargeExtension)Container.DataItem).tblOtherCharges.Quantity%>
                                        </td>
                                        <td style="text-align: right">
                                            <%#((decimal)((SweetSoft.APEM.WebApp.Pages.Printing.TblOtherChargeExtension)Container.DataItem).tblOtherCharges.Charge).ToString("N2")%>
                                        </td>
                                        <td style="text-align: right"><%#(((SweetSoft.APEM.WebApp.Pages.Printing.TblOtherChargeExtension)Container.DataItem).TotalPrice).ToString("N2")%></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            <tr>
                                <%--<td colspan="7"></td>--%>
                                <td colspan="9" style="text-align: right;"><strong>Sub Total</strong></td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblSubTotal" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <%--<td colspan="7"></td>--%>
                                <td colspan="9" style="text-align: right;"><strong>Discount </strong>(<%=ocDiscount%>%)</td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblDiscount" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <%--<td colspan="7"></td>--%>
                                <td colspan="9" style="text-align: right;"><strong>Sub Total before GST</strong></td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblSubTotalBefore" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <%--<td colspan="7"></td>--%>
                                <td colspan="9" style="text-align: right;"><strong>GST </strong>(<%=ocTaxRate%>%)</td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblGST" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <%--<td colspan="7"></td>--%>
                                <td colspan="9" style="text-align: right;"><strong>Total</strong></td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblTotal" runat="server"></asp:Label></td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="row" style="display: none">
                    <div class="col-xs-7">
                        <div class="form-group">
                            <label class="control-label" style="font-weight: 700">
                                Additional Charges
                            </label>
                            <br />
                            IRIS Proof: &nbsp; 
                            <asp:Literal Text="IRIS Proof" EnableViewState="false" ID="ltrIris" runat="server" />
                        </div>

                    </div>
                    <div class="col-xs-5">
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
                    <div class="col-xs-12">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div class="col-xs-12">
                                    <p class="form-control-static">
                                        Remark:
                                        <asp:Literal Text="" ID="ltrRemark" EnableViewState="false" runat="server" />
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
                <div class="row">
                    <div class="col-xs-5">
                        <br />
                        <br />
                        <br />
                        <br />
                        <br />
                        <span style="font-size: 12px; text-transform: uppercase">Asia Pacific Engravers (Malaysia) Sdn Bhd</span><br />
                        <asp:Literal ID="ltrOCCreator" runat="server"></asp:Literal>

                    </div>
                    <div class="col-xs-5 col-xs-offset-2">
                        Kindly acknowledge receipt of this order confirmation. Thank you
                        <br />
                        <br />
                        <br />
                        <br />
                        <hr style="border-color: #000!important; margin: 0 auto;" />
                        Name:
                        <br />
                        Date: 
                    </div>
                </div>
                <div class="row text-center" style="margin-top:30px;">
                    <span>This is a computer generated printout and is valid without signature</span>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
