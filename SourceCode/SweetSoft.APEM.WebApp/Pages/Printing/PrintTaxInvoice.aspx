<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintTaxInvoice.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.Printing.PrintTaxInvoice" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
    <link href="/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/css/uniform.default.css" rel="stylesheet" />

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

        #PageNumber:after {
            counter-increment: page;
            content: counter(page);
        }
<<<<<<< HEAD
=======

        .table > tbody > tr > td
        {
            border-top: none;
        }
>>>>>>> backup_1

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
            margin: 55mm 6mm 15mm 6mm;
        }

        @media screen {
            #PageNumber {
                display:none;
            }
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

            #PageNumber {
                position: fixed;
                top: 200px;
            }
        }
    </style>
</head>
<body style="background: #fff">
    <form id="form1" runat="server">
        <div class="container-fluid" id="wrapPrint">
            <div class="printContent">
                <div class="row" style="display: none">
                    <div class="col-xs-12 text-right" style="margin-bottom: 15px">
                        <span class="form-control-static">
                            <strong>
                                <asp:Literal Text="Asia-Pacific Engravers (Malaysia) Sdn. Bhd." ID="ltrCompanyName" EnableViewState="false" runat="server" /></strong>
                        </span>
                        <img src="/img/logo-print1.png" class="img-responsive" style="height: 12mm; vertical-align: bottom; display: inline-block" />
                        <h6></h6>
                    </div>
                </div>

                <div class="row">
                    <div class="col-xs-6">
                        <h3 style="text-transform: capitalize; margin-top:0px;"><strong>Tax Invoice</strong></h3>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-6">
                        <p class="form-control-static">
                            <br />
                            <asp:Literal ID="ltrCustomerName" runat="server" />
                            <br />
                            <asp:Literal ID="ltrCustomerAddress" runat="server" />
                            <%--<br />--%>
                            <asp:Literal ID="ltrGSTID" runat="server" />
                        </p>
                        <%--<div class="form-group">
                        </div>--%>
                    </div>
                    <div class="col-xs-4 col-xs-offset-2 information-do">
                        <div class="form-group" style="margin-top: 15px;">
                            <label class="control-label">Number/ Date: </label>
                            <p class="form-control-static">
                                <strong>
                                    <asp:Literal ID="ltrInvoiceNumber" runat="server" EnableViewState="false" />
                                </strong>
                                /
                                    <asp:Literal ID="ltrInvoiceDate" EnableViewState="false" runat="server" />
                            </p>
                            <label class="control-label">
                                Customer number:
                                <asp:Literal ID="ltrSAPCode" runat="server" EnableViewState="false" /></label><br />
                            <br />
                        </div>
                    </div>
                </div>
                <div class="row" style="margin-top: 7px;">
                    <div class="col-xs-6">
                        <div class="row">
                            <div class="col-xs-12">
                                <label class="control-label"><strong>Ship to Address:</strong></label>
                            </div>
                            <div class="col-xs-12">
                                <p class="form-control-static">
                                    <asp:Literal ID="ltrShipToAddress" Text="" EnableViewState="false" runat="server" />
                                </p>
                            </div>
                        </div>
                    </div>  
                    <div class="col-xs-4 col-xs-offset-2 information-do">
                        <div id="PageNumber">
                            <label class="control-label">Page:</label>
                        </div>
                        <label class="control-label">
                            Curr: <%=CurrencyName %></label><br />
                        <label class="control-label">
                            Exchange Rate: 
                                <asp:Literal ID="ltrExchangeRate" runat="server" EnableViewState="false" /></label>
                    </div>
                </div>
                <div class="row" style="margin-top: 15px; margin-bottom: 5px;">
                    <div class="col-xs-12">
                        <label class="control-label"><strong>Requirement</strong></label>
                        <div class="clearfix">
                            <ul class="list-unstyled pull-left">
                                <li>Terms of delivery
                                </li>
                                <li>Terms of payment
                                </li>
                            </ul>
                            <ul class="list-unstyled pull-left" style="margin-left: 15px;">
                                <li>:
                                    <asp:Literal ID="ltrDeliveryTerm" Text="" EnableViewState="false" runat="server" />
                                </li>
                                <li>:
                                    <asp:Literal ID="ltrPaymentTerms" Text="" EnableViewState="false" runat="server" />
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
                <%--Tempate Cylinder in Jobs--%>
                <asp:Repeater ID="rptTemplateJob" runat="server" OnItemDataBound="rptTemplateJob_ItemDataBound">
                    <ItemTemplate>
                        <div class="row" style="page-break-inside: avoid">
                            <div class="col-xs-12">
                                <ul class="list-unstyled pull-left">
                                    <li>Job name
                                    </li>
                                    <li>Design Name
                                    </li>
                                    <li>APE DO No / Date
                                    </li>
                                    <li>APE Order no / Date
                                    </li>
                                </ul>
                                <ul class="list-unstyled pull-left">
                                    <li>:
                                        <asp:Literal ID="ltrJobName" Text="text job name" EnableViewState="false" runat="server" />
                                    </li>
                                    <li>:
                                        <asp:Literal ID="ltrJobDesign" Text="text job design" EnableViewState="false" runat="server" />
                                    </li>
                                    <li>:
                                        <asp:Literal ID="ltrDONumber" runat="server" EnableViewState="false" />
                                    </li>
                                    <li>:
                                        <asp:Literal ID="ltrJobNumber" runat="server" EnableViewState="false" />
                                    </li>
                                </ul>
                            </div>
                            <%--Cylinder Grid--%>
                            <div class="col-xs-12">
                                <table class="table table-bordered">
                                    <thead>
                                        <tr>
                                            <th>No</th>
                                            <%--<th>Cyl Barcode</th>--%>
                                            <th style="min-width:160px;">Description</th>
                                            <%--<th>Cylinder ID/ Product ID</th>--%>
                                            <th style="min-width:90px;">Cus Cyl No</th>
                                            <th style="min-width:90px;">Cus Steelbase ID</th>
                                            <th>Width (mm)</th>
                                            <th>Circumf. (mm)</th>
                                            <th>SB</th>
                                            <th>Qty Pcs.</th>
                                            <th>Unit Price</th>
                                            <th>Total</th>
                                            <%--<th>Total (MYR)</th>--%>
                                        </tr>
                                    </thead>
                                    <asp:Repeater ID="rptCylinder" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" Text='<%#Eval("No")%>'></asp:Label></td>
                                                <%--<td>
                                                    <asp:Label runat="server" Text='<%#Eval("CylBarcode")%>'></asp:Label></td>--%>
                                                <td>
                                                    <asp:Label runat="server" Text='<%#Eval("Description")%>'></asp:Label></td>
                                                <%--<td>
                                                    <asp:Label runat="server" Text='<%#Eval("CylinderID")%>'></asp:Label></td>--%>
                                                <td>
                                                    <asp:Label runat="server" Text='<%#Eval("CusCylID")%>'></asp:Label></td>
                                                <td>
                                                    <asp:Label runat="server" Text='<%#Eval("CusSteelBaseID")%>'></asp:Label></td>
                                                <td style="text-align: right">
                                                    <asp:Label runat="server" Text='<%#ShowNumberFormat(Eval("Width"), "N2", "")%>'></asp:Label></td>
                                                <td style="text-align: right">
                                                    <asp:Label runat="server" Text='<%#ShowNumberFormat(Eval("Cirf"), "N2", "")%>'></asp:Label></td>
                                                <td style="text-align: center">
                                                    <asp:Label runat="server" Text='<%#Eval("SteelBase")%>'></asp:Label></td>
                                                <td style="text-align: right">
                                                    <asp:Label runat="server" Text='<%#Eval("Qty")%>'></asp:Label></td>
                                                <td style="text-align: right">
                                                    <asp:Label runat="server" Text='<%#Convert.ToDecimal(Eval("UnitPrice")).ToString("N3")%>'></asp:Label></td>
                                                <td style="text-align: right">
                                                    <asp:Label runat="server" Text='<%#Convert.ToDecimal(Eval("TotalPrice")).ToString("N2")%>'></asp:Label>
                                                </td>
                                                <%--<td style="text-align:right">
                                                    <asp:Label runat="server" Text='<%#Convert.ToDecimal(Eval("TotalPriceMY")).ToString("N2")%>'></asp:Label></td>--%>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
<<<<<<< HEAD
=======
                                    <tr>
                                        <td colspan="7">
                                            <strong>Total</strong>
                                        </td>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblTotalQty" runat="server" Text="0"></asp:Label>
                                        </td>
                                        <td></td>
                                        <td></td>
                                    </tr>
>>>>>>> backup_1
                                    <asp:Repeater ID="rptOtherCharges" runat="server" Visible="false">
                                        <HeaderTemplate>
                                            <tr>
                                                <td colspan="10">
                                                    <strong>Other Charges</strong>
                                                </td>
                                            </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" Text='<%#Eval("No")%>'></asp:Label>
                                                </td>
                                                <td colspan="6">
                                                    <asp:Label runat="server" Text='<%#Eval("Description")%>'></asp:Label>
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label runat="server" Text='<%#Eval("Qty")%>'></asp:Label>
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label runat="server" Text='<%#Convert.ToDecimal(Eval("UnitPrice")).ToString("N2")%>'></asp:Label></td>
                                                <td style="text-align: right">
                                                    <asp:Label runat="server" Text='<%#Convert.ToDecimal(Eval("TotalPrice")).ToString("N2")%>'></asp:Label>
                                                </td>
                                                <%--<td style="text-align:right">
                                                    <asp:Label runat="server" Text='<%#Convert.ToDecimal(Eval("TotalPriceMY")).ToString("N3")%>'></asp:Label></td>--%>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <tr>
                                        <%--<td colspan="7" style="border: none;"></td>--%>
                                        <td colspan="9" style="text-align:right;"><strong>Sub Total</strong></td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblSubTotal" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%--<td colspan="7" style="border: none;"></td>--%>
                                        <td colspan="9" style="text-align:right;"><strong>Discount </strong>(<asp:Literal ID="ltrDiscountRate" runat="server"></asp:Literal>)</td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblDiscount" runat="server"></asp:Label></td>
                                        <%--<td style="text-align:right">
                                            <asp:Label ID="lblDiscountMY" runat="server"></asp:Label></td>--%>
                                    </tr>
                                    <tr>
                                        <%--<td colspan="7" style="border: none;"></td>--%>
                                        <td colspan="9" style="text-align:right;"><strong>Sub Total before GST</strong></td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblSubTotalBefore" runat="server"></asp:Label></td>
                                        <%--<td style="text-align:right">
                                            <asp:Label ID="lblSubTotalBeforeMY" runat="server"></asp:Label></td>--%>
                                    </tr>
                                    <tr>
                                        <%--<td colspan="7" style="border: none;"></td>--%>
                                        <td colspan="9" style="text-align:right;"><strong>GST </strong>(<asp:Literal ID="ltrTaxRate" runat="server"></asp:Literal>%)</td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblGST" runat="server"></asp:Label></td>
                                        <%--<td style="text-align:right">
                                            <asp:Label ID="lblGSTMY" runat="server"></asp:Label></td>--%>
                                    </tr>
                                    <tr>
                                        <%--<td colspan="7" style="border: none;"></td>--%>
                                        <td colspan="9" style="text-align:right;"><strong>Total</strong></td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblTotal" runat="server"></asp:Label></td>
                                        <%--<td style="text-align:right">
                                            <asp:Label ID="lblTotalMY" runat="server"></asp:Label></td>--%>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>

                <asp:Panel runat="server" ID="pnlSubTotal">
                    <div class="row" style="page-break-inside: avoid">
                        <div class="col-xs-12">
<<<<<<< HEAD
                            <table class="table table-bordered">
=======
                            <table class="table" border="0" style="border-top: none !important;">
>>>>>>> backup_1
                                <tr>
                                    <td><strong>Total</strong></td>
                                    <td><strong>Total (<%=CurrencyName %>)</strong></td>
                                    <%--<td><strong>Total (MYR)</strong></td>--%>
                                </tr>
                                <tr>
                                    <td>Sub total before GST
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Literal ID="ltrSubTotalBeforeGST" EnableViewState="false" runat="server" /></td>
                                    <td style="text-align: right">
                                        <asp:Literal ID="ltrSubTotalBeforeGSTMY" EnableViewState="false" runat="server" /></td>
                                </tr>
                                <tr>
<<<<<<< HEAD
                                    <td>Total invoice with GST 0%</td>
                                    <td style="text-align: right">0.00</td>
                                    <td style="text-align: right">0.00</td>
=======
                                    <td>Total GST <%=TotalTaxRate.ToString() %>%</td>
                                    <td style="text-align: right">
                                        <asp:Literal ID="ltrTotalGST" EnableViewState="false" runat="server" /></td>
>>>>>>> backup_1
                                </tr>
                                <tr>
                                    <td>Total invoice with GST <%=taxRate.TaxPercentage.ToString() %>%</td>
                                    <td style="text-align: right">
                                        <asp:Literal ID="ltrTotalInvoiceWithGST" runat="server" EnableViewState="false" /></td>
                                    <td style="text-align: right">
                                        <asp:Literal ID="ltrTotalInvoiceWithGSTMY" runat="server" EnableViewState="false" /></td>
                                </tr>
                                <tr>
                                    <td>Final amount</td>
                                    <td style="text-align: right">
                                        <asp:Literal ID="ltrFinalAmount" EnableViewState="false" runat="server" /></td>
                                    <td style="text-align: right">
                                        <asp:Literal ID="ltrFinalAmountMY" EnableViewState="false" runat="server" /></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </asp:Panel>
                <div class="row">
                    <div class="col-xs-12">
                        <label class="control-label"><strong>Remark:</strong>
                            <asp:Literal ID="ltrRemark" runat="server" /></label>
                        <br />
                        <br />
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-5 text-center">
                        <br />
                        Best regards
                        <br />
                        <br />
                        <br />
                        <br />
                        <hr style="border-color: #000!important; border-style: dashed; width: 80%; margin: 15px auto;" />
                        <asp:Literal Text="Asia Pacific Engravers (Malaysia) Sdn Bhd" ID="ltr_CompanyName" EnableViewState="false" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script src="/js/core/jquery.min.js"></script>
    <script src="/js/plugins/migrate.js"></script>
    <script src="/js/plugins/uniform/jquery.uniform.js"></script>
    <script>
        $(document).ready(function () {
            $('.uniform input[type=checkbox]').uniform();
        })
    </script>
</body>
</html>
