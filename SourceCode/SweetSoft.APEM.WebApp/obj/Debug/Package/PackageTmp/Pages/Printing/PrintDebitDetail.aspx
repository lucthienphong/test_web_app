<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintDebitDetail.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.Printing.PrintDebitDetail" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="SweetSoft" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
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
        }

        label
        {
            font-weight: 300!important;
        }

        small
        {
            color: #000!important;
        }

        @media all
        {
            body
            {
                font-size: 12px !important;
            }

            .information-do label.control-label, .information-do p.form-control-static
            {
                margin-bottom: 3px!important;
            }

            .form-group
            {
                margin-bottom: 0px;
            }

            .control-label
            {
                padding-top: 0px !important;
                margin-bottom: 0 !important;
            }

            .form-control-static
            {
                padding-top: 0;
                padding-bottom: 0;
            }
        }

        @page
        {
            size: auto;
            margin: 20mm 6mm 20mm 6mm;
            margin-top: 20mm;
        }

        @media print
        {
            body
            {
                margin: 0;
            }

            .form-group
            {
                margin-bottom: 0;
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

            div.uniform-checker.uniform-disabled span.uniform-checked
            {
                background-position: -114px -260px;
            }
        }

        .column1
        {
            border: 1px solid #000;
            border-right: 1px solid #000;
            width: 10%;
        }

        .column2
        {
            border: 1px solid #000;
            border-left: none;
            border-right: 1px solid #000;
            width: 30%;
        }

        .column3
        {
            border: 1px solid #000;
            border-left: none;
            border-right: 1px solid #000;
            width: 20%;
        }

        .column4
        {
            border: 1px solid #000;
            border-left: none;
            border-right: 1px solid #000;
            width: 10%;
        }

        .column5
        {
            border: 1px solid #000;
            border-left: none;
            border-right: 1px solid #000;
            width: 15%;
        }

        .column6
        {
            border: 1px solid #000;
            border-left: none;
            border-right: 1px solid #000;
            width: 15%;
        }

        .row-gridview
        {
            width: 101%;
            left: -3px;
            right: -3px;
            position: relative;
            border-top: none;
        }

        h5
        {
            margin-top: 5px !important;
            margin-bottom: 5px !important;
        }

        .info-company
        {
            font-size: 11px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid" id="wrapPrint">
            <div class="printContent">
                <div class="row" style="margin-bottom: 20px;">
                    <div class="col-sm-2 col-xs-2">
                        <img src="/img/apem-logo-print.png" class="img-responsive" />
                    </div>
                    <div class="col-sm-10 col-xs-10">
                        <strong><asp:Literal runat="server" ID="ltrCompanyName" EnableViewState="false"></asp:Literal></strong>
                        <asp:Label runat="server" ID="lblCompanyInfo" CssClass="info-company"></asp:Label>
                    </div>
                </div>
                <asp:Literal runat="server" ID="ltrInvoiceAndDate" EnableViewState="false"></asp:Literal>
                <div class="row">
                    <div class="col-xs-12">
                        <h3 style="font-weight: 700; margin-top: 0">Debit Memo
                        </h3>
                    </div>
                    <div class="col-xs-4">
                        <strong>
                            <asp:Literal runat="server" ID="ltrCustomerName" EnableViewState="false"></asp:Literal></strong>
                        </br>
                            <asp:Literal runat="server" ID="ltrCustomerAddress" EnableViewState="false"></asp:Literal>
                    </div>
                    <div class="col-xs-2">
                        <asp:Literal runat="server" ID="Literal1" EnableViewState="false"></asp:Literal>
                    </div>
                    <div class="col-xs-4 col-xs-offset-2 information-do">
                        <div class="form-group">
                            <label class="control-label" style="font-weight: 300">Number / Date:</label>
                            <p class="form-control-static">
                                <strong>
                                    <asp:Literal ID="ltrCNumber" Text="" runat="server" EnableViewState="false" />
                                </strong>
                                /
                                    <asp:Literal ID="ltrCDate" runat="server" EnableViewState="false" />
                            </p>
                            <label class="control-label" style="font-weight: 300">Customer No.</label>
                            <p class="form-control-static">
                                <asp:Literal ID="ltrCustomerNumber" Text="" runat="server" EnableViewState="false" />
                            </p>
                            <label class="control-label" style="font-weight: 300">
                                Currency: 
                                <asp:Literal ID="ltrCurr" runat="server" EnableViewState="false" /></label>
                        </div>
                    </div>
                </div>
                </br>
                    <div class="row">
                        <div class="col-xs-12">
                            <label class="control-label"><strong>Requirement:</strong></label>
                            <div class="clearfix">
                                <div class="form-group clearfix">
                                    <ul class="pull-left list-unstyled">
                                        <li>Terms of payment
                                        </li>
                                        <li>
                                            <label class="control-label"></label>
                                        </li>
                                    </ul>
                                    <ul class="list-unstyled pull-left" style="margin-left: 15px;">
                                        <li>:
                                            <asp:Literal ID="ltrPaymentTerms" Text="" EnableViewState="false" runat="server" />
                                        </li>
                                        <li>
                                            <label class="control-label"></label>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                <div class="row">
                    <div class="col-xs-12">
                        <div style="display: table; width: 101%; left: -3px; right: -3px; position: relative;">
                            <div style="display: table-row">
                                <div style="display: table-cell; border: 1px solid #000; border-right: 1px solid #000; width: 10%;">
                                    <h5 style="text-align: center"><strong>No</strong>
                                    </h5>
                                </div>
                                <div style="display: table-cell; border: 1px solid #000; border-left: none; border-right: 1px solid #000; width: 30%;">
                                    <h5 style="text-align: center"><strong>Description</strong>
                                    </h5>
                                </div>
                                <div style="display: table-cell; border: 1px solid #000; border-left: none; border-right: 1px solid #000; width: 20%;">
                                    <h5 style="text-align: center"><strong>Job Order no.</strong>
                                    </h5>
                                </div>
                                <div style="display: table-cell; border: 1px solid #000; border-left: none; border-right: 1px solid #000; width: 10%;">
                                    <h5 style="text-align: center"><strong>Qty
                                        <br />
                                        Pcs.</strong>
                                    </h5>
                                </div>
                                <div style="display: table-cell; border: 1px solid #000; border-left: none; border-right: 1px solid #000; width: 15%;">
                                    <h5 style="text-align: center"><strong>Unit Price</strong>
                                    </h5>
                                </div>
                                <div style="display: table-cell; border: 1px solid #000; border-left: none; border-right: 1px solid #000; width: 15%;">
                                    <h5 style="text-align: center"><strong>Total</strong>
                                    </h5>
                                </div>
                            </div>
                        </div>
                        <asp:Repeater ID="rptDebitDetail" runat="server" OnItemCreated="RepeaterItemCreated">
                            <ItemTemplate>
                                <div style="display: table; width: 101%; left: -3px; right: -3px; position: relative;">
                                    <div style="display: table-row">
                                        <div style="display: table-cell; border: 1px solid #000; border-top: none; border-right: 1px solid #000; width: 10%;">
                                            <h5 style="text-align: center">
                                                <asp:Label ID="lblSequence" runat="server" Text=''></asp:Label>
                                            </h5>
                                        </div>
                                        <div style="display: table-cell; border: 1px solid #000; border-top: none; border-left: none; border-right: 1px solid #000; width: 30%; padding-left: 5px;">
                                            <h5 style="text-align: left">
                                                <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("Description")%>'></asp:Label>
                                            </h5>
                                        </div>
                                        <div style="display: table-cell; border: 1px solid #000; border-top: none; border-left: none; border-right: 1px solid #000; width: 20%;">
                                            <h5 style="text-align: center">
                                                <asp:Label ID="lblJobOrderNo" runat="server" Text='<%#Eval("JobOrderNo")%>'></asp:Label>
                                            </h5>
                                        </div>
                                        <div style="display: table-cell; border: 1px solid #000; border-top: none; border-left: none; border-right: 1px solid #000; width: 10%;">
                                            <h5 style="text-align: center">
                                                <asp:Label ID="lblQuantity" runat="server" Text='<%#Convert.ToDecimal(Eval("Quantity")).ToString("N0") %>'></asp:Label>
                                            </h5>
                                        </div>
                                        <div style="display: table-cell; border: 1px solid #000; border-top: none; border-left: none; border-right: 1px solid #000; width: 15%;">
                                            <h5 style="text-align: center">
                                                <asp:Label ID="lblUnitPrice" runat="server" Text='<%#Convert.ToDecimal(Eval("UnitPrice")).ToString("N2") %>'></asp:Label>
                                            </h5>
                                        </div>
                                        <div style="display: table-cell; border: 1px solid #000; border-top: none; border-left: none; border-right: 1px solid #000; width: 15%;">
                                            <h5 style="text-align: center">
                                                <asp:Label ID="lblTotal" runat="server" Text='<%#(Convert.ToDecimal(Eval("Quantity")) * Convert.ToDecimal(Eval("UnitPrice"))).ToString("N2") %>'></asp:Label>
                                            </h5>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <div style="display: table; width: 101%; left: -3px; right: -3px; position: relative;">
                            <div style="display: table-row">
                                <div style="display: table-cell; border: 1px solid #000; border-top: none; border-right: 1px solid #000; width: 10%;">
                                    <h5 style="text-align: center"><strong style="color: white">&nbsp;</strong>
                                    </h5>
                                </div>
                                <div style="display: table-cell; border: 1px solid #000; border-top: none; border-left: none; border-right: 1px solid #000; width: 30%;">
                                    <h5 style="text-align: center"><strong></strong>
                                    </h5>
                                </div>
                                <div style="display: table-cell; border: 1px solid #000; border-top: none; border-left: none; border-right: 1px solid #000; width: 20%;">
                                    <h5 style="text-align: center"><strong></strong>
                                    </h5>
                                </div>
                                <div style="display: table-cell; border: 1px solid #000; border-top: none; border-left: none; border-right: 1px solid #000; width: 10%;">
                                    <h5 style="text-align: center"><strong></strong>
                                    </h5>
                                </div>
                                <div style="display: table-cell; border: 1px solid #000; border-top: none; border-left: none; border-right: 1px solid #000; width: 15%;">
                                    <h5 style="text-align: center"><strong></strong>
                                    </h5>
                                </div>
                                <div style="display: table-cell; border: 1px solid #000; border-top: none; border-left: none; border-right: 1px solid #000; width: 15%;">
                                    <h5 style="text-align: center"><strong></strong>
                                    </h5>
                                </div>
                            </div>
                        </div>
                        <div style="display: table; width: 101%; left: -3px; right: -3px; position: relative;">
                            <div style="display: table-row">
                                <div style="display: table-cell; border: 1px solid #000; border-top: none; border-right: 1px solid #000; width: 10%;">
                                    <h5 style="text-align: center"><strong></strong>
                                    </h5>
                                </div>
                                <div style="display: table-cell; border: 1px solid #000; border-top: none; border-left: none; border-right: 1px solid #000; width: 30%;">
                                    <h5 style="text-align: center"><strong></strong>
                                    </h5>
                                </div>
                                <div style="display: table-cell; border: 1px solid #000; border-top: none; border-left: none; border-right: 1px solid #000; width: 20%;">
                                    <h5 style="text-align: center"><strong></strong>
                                    </h5>
                                </div>
                                <div style="display: table-cell; border: 1px solid #000; border-top: none; border-left: none; border-right: 1px solid #000; width: 10%;">
                                    <h5 style="text-align: left; padding-left: 5px;">Sub Total
                                    </h5>
                                </div>
                                <div style="display: table-cell; border: 1px solid #000; border-top: none; border-left: none; border-right: 1px solid #000; width: 15%;">
                                    <h5 style="text-align: center"><strong></strong>
                                    </h5>
                                </div>
                                <div style="display: table-cell; border: 1px solid #000; border-top: none; border-left: none; border-right: 1px solid #000; width: 15%;">
                                    <h5 style="text-align: center">
                                        <asp:Label ID="lblSubTotal" runat="server"></asp:Label>
                                    </h5>
                                </div>
                            </div>
                        </div>
                        <div style="display: table; width: 101%; left: -3px; right: -3px; position: relative;">
                            <div style="display: table-row">
                                <div style="display: table-cell; border: 1px solid #000; border-top: none; border-right: 1px solid #000; width: 10%;">
                                    <h5 style="text-align: center"><strong></strong>
                                    </h5>
                                </div>
                                <div style="display: table-cell; border: 1px solid #000; border-top: none; border-left: none; border-right: 1px solid #000; width: 30%;">
                                    <h5 style="text-align: center"><strong></strong>
                                    </h5>
                                </div>
                                <div style="display: table-cell; border: 1px solid #000; border-top: none; border-left: none; border-right: 1px solid #000; width: 20%;">
                                    <h5 style="text-align: center"><strong></strong>
                                    </h5>
                                </div>
                                <div style="display: table-cell; border: 1px solid #000; border-top: none; border-left: none; border-right: 1px solid #000; width: 10%;">
                                    <h5 style="text-align: left; padding-left: 5px;">GST(<asp:Label ID="lblTax" runat="server"></asp:Label>)
                                    </h5>
                                </div>
                                <div style="display: table-cell; border: 1px solid #000; border-top: none; border-left: none; border-right: 1px solid #000; width: 15%;">
                                    <h5 style="text-align: center"><strong></strong>
                                    </h5>
                                </div>
                                <div style="display: table-cell; border: 1px solid #000; border-top: none; border-left: none; border-right: 1px solid #000; width: 15%;">
                                    <h5 style="text-align: center">
                                        <asp:Label ID="lblTaxAmount" runat="server"></asp:Label>
                                    </h5>
                                </div>
                            </div>
                        </div>
                        <div style="display: table; width: 101%; left: -3px; right: -3px; position: relative;">
                            <div style="display: table-row">
                                <div style="display: table-cell; border: 1px solid #000; border-top: none; border-right: 1px solid #000; width: 10%;">
                                    <h5 style="text-align: center"><strong></strong>
                                    </h5>
                                </div>
                                <div style="display: table-cell; border: 1px solid #000; border-top: none; border-left: none; border-right: 1px solid #000; width: 30%;">
                                    <h5 style="text-align: center"><strong></strong>
                                    </h5>
                                </div>
                                <div style="display: table-cell; border: 1px solid #000; border-top: none; border-left: none; border-right: 1px solid #000; width: 20%;">
                                    <h5 style="text-align: center"><strong></strong>
                                    </h5>
                                </div>
                                <div style="display: table-cell; border: 1px solid #000; border-top: none; border-left: none; border-right: 1px solid #000; width: 10%;">
                                    <h5 style="text-align: left; padding-left: 5px;">TOTAL
                                    </h5>
                                </div>
                                <div style="display: table-cell; border: 1px solid #000; border-top: none; border-left: none; border-right: 1px solid #000; width: 15%;">
                                    <h5 style="text-align: center"><strong></strong>
                                    </h5>
                                </div>
                                <div style="display: table-cell; border: 1px solid #000; border-top: none; border-left: none; border-right: 1px solid #000; width: 15%;">
                                    <h5 style="text-align: center">
                                        <asp:Label ID="lblAllTotal" runat="server"></asp:Label>
                                    </h5>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <br />
                <br />
                <br />
                <div class="row">
                    <div class="col-xs-6">
                        <span style="font-size: 10pt;">Remark: </span>
                        <br />
                        <asp:Literal runat="server" ID="ltrRemark" EnableViewState="false"></asp:Literal>
                    </div>
                </div>
                <br />
                <br />
                <br />

                <div class="row">
                    <div class="col-xs-7">
                        <span style="font-size: 10pt;">Best Regards</span><br />
                        <hr style="border-color: #000!important; width: 50%; margin-left: 0; border-style: dashed;" />
                        <span style="font-size: 10pt; text-transform: uppercase">Asia Pacific Engravers (Malaysia) Sdn Bhd</span>

                        <br />
                        <br />
                        <br />
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
