<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintInvoice.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.Printing.PrintInvoice" %>

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

            .textAddress
            {
                resize: none;
                display: block;
                border: 0;
                border-color: #fff;
                min-height: 84px;
                width: 100%;
                background: transparent;
                outline: none;
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
                <div class="row">
                    <div class="col-xs-offset-9 col-xs-3">
                        <img src="/img/logo-print1.png" class="img-responsive" style="vertical-align: bottom; display: inline-block" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12">
                        <h3 style="font-weight: 700;">INVOICE
                        </h3>
                    </div>
                    <div class="col-xs-6">
                        <asp:Literal Text="" ID="ltrCustomer" EnableViewState="false" runat="server" />
                    </div>
                    <div class="col-xs-6">
                        <div class="form-horizontal" style="direction: ltr;">
                            <div class="form-group">
                                <label class="control-label col-xs-8 text-right" style="font-weight: 300">No</label>
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
                                <label class="control-label col-xs-4 text-left" style="font-weight: 700">Attn</label>
                                <label class="control-label text-left col-xs-7">
                                    :&nbsp;&nbsp;
                                    <asp:Literal Text="" ID="ltrContact" runat="server" EnableViewState="false" /></label>
                                <label class="control-label col-xs-4 text-left">Your Reference</label>
                                <label class="control-label text-left col-xs-7">
                                    :&nbsp;&nbsp;
                                    <asp:Literal Text="" ID="ltrReference" runat="server" EnableViewState="false" /></label>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12">
                        <table class="table table-bordered">
                            <thead>
                                <tr>
                                    <th>Job Information</th>
                                    <th class="text-center">Description</th>
                                    <th class="text-center">Width (mm)</th>
                                    <th class="text-center">Circumf (mm)</th>
                                    <th class="text-center">Qty Pcs.</th>
                                    <th class="text-center">Unit Price (USD).</th>
                                    <th class="text-center">Total (USD).</th>
                                </tr>
                            </thead>
                            <asp:Repeater runat="server" ID="rptInvoiceDetails" OnItemDataBound="rptInvoiceDetails_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <asp:Literal ID="ltrTrHeading" EnableViewState="false" runat="server" />
                                        <td>Steel Base</td>
                                        <td class="text-center"><asp:Literal Text="" ID="ltrSTWidth" EnableViewState="false" runat="server" /></td>
                                        <td class="text-center"><asp:Literal Text="" ID="ltrSTCirf" EnableViewState="false" runat="server" /></td>
                                        <td class="text-center"><asp:Literal Text="" ID="ltrSTQty" EnableViewState="false" runat="server" /></td>
                                        <td class="text-center"><asp:Literal Text="" ID="ltrSTUnitPrice" EnableViewState="false" runat="server" /></td>
                                        <td class="text-center"><asp:Literal Text="" ID="ltrSTTotal" EnableViewState="false" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td>Engraving</td>
                                        <td></td>
                                        <td></td>
                                        <td class="text-center"><asp:Literal Text="" ID="ltrEGQty" EnableViewState="false" runat="server" /></td>
                                        <td class="text-center"><asp:Literal Text="" ID="ltrEGUnitPrice" EnableViewState="false" runat="server" /></td>
                                        <td class="text-center"><asp:Literal Text="" ID="ltrEGTotal" EnableViewState="false" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td>Services</td>
                                        <td></td>
                                        <td></td>
                                        <td class="text-center"><asp:Literal Text="" ID="ltrSVQty" EnableViewState="false" runat="server" /></td>
                                        <td class="text-center"><asp:Literal Text="" ID="ltrSVUnitPrice" EnableViewState="false" runat="server" /></td>
                                        <td class="text-center"><asp:Literal Text="" ID="ltrSVTotal" EnableViewState="false" runat="server" /></td>
                                    </tr>
                                    <asp:Repeater runat="server" id="rptOtherCharges" OnItemDataBound="rptOtherCharges_ItemDataBound">
                                        <ItemTemplate>
                                            <tr>
                                                <td><asp:Literal Text="" ID="ltrODChargeDescription" EnableViewState="false" runat="server" /></td>
                                                <td></td>
                                                <td></td>
                                                <td class="text-center"><asp:Literal Text="" ID="ltrODChargeQty" EnableViewState="false" runat="server" /></td>
                                                <td class="text-center"><asp:Literal Text="" ID="ltrODChargeUnitPrice" EnableViewState="false" runat="server" /></td>
                                                <td class="text-center"><asp:Literal Text="" ID="ltrODChargeTotal" EnableViewState="false" runat="server" /></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ItemTemplate>
                            </asp:Repeater>
                            <tr>
                                <td colspan="5" style="border-right-color:#fff!important">
                                    GRAND TOTAL
                                </td>
                                <td style="border-left-color:#fff!important;" class="text-center">
                                    <strong>Total</strong>
                                </td>
                                <td class="text-center">
                                    <asp:Literal Text="" EnableViewState="false" id="ltrGrandTotal" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-7">
                        Please make payment to:<br />
                        <strong>
                            <asp:Literal Text="Asia Pacific Engravers (Malaysia) Sdn Bhd" ID="ltrCompanyName" EnableViewState="false" runat="server" /></strong><br />
                        <strong>
                            <asp:Literal Text="Account number: 70-5103-000-167" ID="ltrAccountNumber" EnableViewState="false" runat="server" /></strong><br />
                        <asp:Literal Text="Maybank Berhad" ID="ltrBankName" EnableViewState="false" runat="server" /><br />
                        <asp:TextBox runat="server" ID="txtAddress" TextMode="MultiLine" ReadOnly="true" CssClass="textAddress" />
                        <asp:Literal Text="Swift code: MBBEMYKL" ID="ltrSwiftCode" runat="server" />
                    </div>
                    <div class="col-xs-5 text-center">
                        <br />
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
</body>
</html>
