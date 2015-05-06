<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintCylinderCard.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.Printing.PrintCylinderCard" EnableViewState="false" %>

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
            padding-top: 5px;
            padding-bottom: 5px;
        }

        small {
            color: #000!important;
        }

        @media all {
            h3, h4, h5 {
                font-size: 12px;
                margin-top: 2px;
                margin-bottom: 2px;
            }

                h4 small, h3 small, h5 small {
                    font-size: 12px;
                }

            .cylinderTable tr td:first-child, .cylinderTable tr th:first-child {
                border-left-width: 0;
            }

            .cylinderTable tr th:last-child,
            .cylinderTable tr td:last-child {
                border-right-width: 0;
            }

            .cylinderTable tr:last-child td {
                border-bottom-width: 0;
            }

            body {
                font-size: 12px;
            }
        }

        @page {
            size: A5;
            margin: 3mm 0mm 3mm 0mm;
            size: landscape;
        }

        @media print {
            body {
                margin: 0;
                font-size: 10px;
            }

            .no-border-left {
                border-left-color: #fff !important;
                border-left-color: transparent !important;
            }

            .no-border-right {
                border-right-color: #fff !important;
                border-right-color: transparent !important;
            }

            label {
                margin-top: 7px;
            }

            table {
                margin-bottom: 5px !important;
            }

            .table-bordered > thead > tr > th, .table-bordered > tbody > tr > th, .table-bordered > tfoot > tr > th, .table-bordered > thead > tr > td, .table-bordered > tbody > tr > td, .table-bordered > tfoot > tr > td {
                padding-top: 1px !important;
                padding-bottom: 1px !important;
                background: #fff !important;
            }

            h4 small, h3 small, h5 small {
                font-size: 9px !important;
            }

            h3, h4, h5 {
                font-size: 10px !important;
                margin-top: 2px;
                margin-bottom: 2px;
            }
        }
    </style>
</head>
<body style="background: #fff">
    <form id="form1" runat="server">
        <div class="container-fluid" id="wrapPrint">
            <div class="printContent">
                <asp:Repeater runat="server" ID="rptPrintCylinderCard" OnItemDataBound="rptPrintCylinderCard_ItemDataBound">
                    <ItemTemplate>
                        <div style="page-break-before: always">
                            <div class="row">
                                <div class="col-xs-4" style="margin-bottom: 15px">
                                    <asp:Image runat="server" ID="barcodeImage" EnableViewState="false" />
                                </div>
                                <div class="col-xs-8 text-right" style="margin-bottom: 15px">
                                    <span class="form-control-static">
                                        <strong>
                                            <asp:Literal Text="Asia-Pacific Engravers (Malaysia) Sdn. Bhd." runat="server" EnableViewState="false" ID="ltrCompanyName" />
                                        </strong>
                                    </span>
                                    <img src="/img/apem-logo-print.png" class="img-responsive" style="height: 12mm; vertical-align: bottom; display: inline-block" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12">
                                    <div style="display: table; width: 101%; left: -3px; right: -3px; border-spacing: 3px 0; position: relative; margin-bottom: 3px">
                                        <div style="display: table-row">
                                            <div style="display: table-cell; border: 1px solid #000; border-right: 1px solid #000; width: 54mm; background: #000; color: #fff">
                                                <h4 style="color: #fff; margin: 0; font-size: 13px; text-align: center"><strong style="color: #fff">Cylinder Card</strong>
                                                </h4>
                                            </div>
                                            <div style="display: table-cell; border: 1px solid #000;">
                                                <div style="display: table; width: 100%">
                                                    <div style="display: table-cell; border-right: 1px solid #000;">
                                                        <h5>
                                                            <small>Client: </small>
                                                            <strong>
                                                                <asp:Literal ID="ltrClient" runat="server" EnableViewState="false" />
                                                            </strong>
                                                        </h5>
                                                    </div>
                                                    <div style="display: table-cell; border-right: 1px solid #000;">
                                                        <h5>
                                                            <small>Job Nr : </small>
                                                            <strong>
                                                                <asp:Literal Text="text" ID="ltrJobNr" runat="server" EnableViewState="false" /></strong>
                                                        </h5>
                                                    </div>
                                                    <div style="display: table-cell; width: 70px">
                                                        <h5>
                                                            <small>R:</small>
                                                            <strong>
                                                                <asp:Literal Text="text" ID="ltrRevNumber" runat="server" EnableViewState="false" /></strong>
                                                        </h5>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div style="display: table; width: 101%; left: -3px; right: -3px; border-spacing: 3px; position: relative; margin-bottom: 3px">
                                        <div style="display: table-row; width: 100%;">
                                            <div style="display: table-cell; border: 1px solid #000">
                                                <div style="display: table; width: 100%; border-spacing: 3px 0">
                                                    <div style="display: table-cell;">
                                                        <h4>
                                                            <small>Job Name: </small>
                                                            <strong>
                                                                <asp:Literal Text="text" ID="ltrJobName" runat="server" EnableViewState="false" />
                                                            </strong>
                                                        </h4>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div style="display: table; width: 101%; left: -3px; right: -3px; border-spacing: 3px 0; position: relative; margin-bottom: 3px">
                                        <div style="display: table-row; width: 100%">
                                            <div style="display: table-cell; border: 1px solid #000">
                                                <div style="display: table; width: 100%; border-spacing: 3px 0">
                                                    <div style="display: table-cell;">
                                                        <h4>
                                                            <small>Design: </small>
                                                            <strong>
                                                                <asp:Literal Text="text" ID="ltrJobDesign" runat="server" EnableViewState="false" />
                                                            </strong>
                                                        </h4>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                    <div style="display: table; width: 101%; left: -3px; right: -3px; border-spacing: 3px 0; position: relative; margin-bottom: 3px">
                                        <div style="display: table-row;">
                                            <div style="display: table-cell; border: 1px solid #000; width: 57mm; padding: 3px">
                                                <h4>
                                                    <small>Circumference:</small>
                                                    <br />
                                                    <strong style="font-size:15px">
                                                        <asp:Literal Text="text" ID="ltrCircumference" runat="server" EnableViewState="false" />
                                                        mm</strong>
                                                </h4>
                                                <h4>
                                                    <small>Facewidth:</small>
                                                    <br />
                                                    <strong style="font-size:15px;">
                                                        <asp:Literal Text="text" ID="ltrFacewidt" runat="server" EnableViewState="false" />
                                                        mm</strong>
                                                </h4>
                                            </div>
                                            <div style="display: table-cell;">
                                                <div style="display: table; width: 100%; border-spacing: 0px; min-height: 90px; border-collapse: collapse;">
                                                    <div style="display: table-row;">
                                                        <div style="display: table-cell; border: 1px solid #000; padding: 3px">
                                                            <h3 style="margin: 3px 0">
                                                                <small>Vickers Hardness</small>
                                                            </h3>
                                                        </div>
                                                    </div>
                                                    <div style="display: table-row;">
                                                        <div style="display: table-cell; border: 1px solid #000">
                                                            <div style="display: table; width: 100%; border-spacing: 0">
                                                                <div style="display: table-row;">
                                                                    <div style="display: table-cell; padding: 3px">
                                                                        <h4>
                                                                            <small>Create By:</small>
                                                                            <br />
                                                                            <strong>
                                                                                <asp:Literal Text="text" ID="ltrCreateBy" runat="server" EnableViewState="false" />
                                                                            </strong>
                                                                        </h4>

                                                                    </div>
                                                                    <div style="display: table-cell; padding: 3px">
                                                                        <h4>
                                                                            <small>Date:</small>
                                                                            <br />
                                                                            <strong>
                                                                                <asp:Literal Text="text" ID="ltrDate" runat="server" EnableViewState="false" />
                                                                            </strong>
                                                                        </h4>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div style="display: table-cell; border: 1px solid #000; width: 67mm; padding: 3px">
                                                <h4>
                                                    <small>Remarks:</small>
                                                </h4>
                                                <asp:Literal ID="ltrRemark" runat="server" EnableViewState="false" />
                                            </div>
                                        </div>
                                    </div>
                                    <div style="display: table; width: 101%; left: -3px; right: -3px; border-spacing: 3px; position: relative; margin-bottom: 3px">
                                        <div style="display: table-row;">
                                            <div style="display: table-cell;">
                                                <table class="table table-bordered">
                                                    <thead>
                                                        <tr>
                                                            <th style="background:#ccc !important;">
                                                                <h4>
                                                                    <small>Cyl Seq</small>
                                                                    <br />
                                                                    <strong style="font-size:15px;">
                                                                        <asp:Literal Text="text" ID="ltrHeaderCylSeq" runat="server" EnableViewState="false" />
                                                                    </strong>
                                                                </h4>
                                                            </th>
                                                            <th style="background:#ccc !important;">
                                                                <h4>
                                                                    <small>Cyl Barcode</small>
                                                                    <br />
                                                                    <strong style="font-size:15px;">
                                                                        <asp:Literal Text="text" ID="ltrHeaderCylBarcode" runat="server" EnableViewState="false" />
                                                                    </strong>
                                                                </h4>
                                                            </th>
                                                            <%--<th>
                                                                <h4>
                                                                    <small>Cyl Nr</small>
                                                                    <br />
                                                                    <strong>
                                                                        <asp:Literal Text="text" ID="ltrHeaderCylNr" runat="server" EnableViewState="false" />
                                                                    </strong>
                                                                </h4>
                                                            </th>--%>
                                                             <th style="background:#ccc !important;">
                                                                <h4>
                                                                    <small>Cus Cyl No</small>
                                                                    <br />
                                                                    <strong style="font-size:15px;">
                                                                        <asp:Literal Text="text" ID="ltrHeaderCylCus" runat="server" EnableViewState="false" />
                                                                    </strong>
                                                                </h4>
                                                            </th>
                                                            <th style="background:#ccc !important;">
                                                                <h4>
                                                                    <small>Cus SteelBase ID</small>
                                                                    <br />
                                                                    <strong style="font-size:15px;">
                                                                        <asp:Literal Text="text" ID="ltrHeaderCustSteelBase" runat="server" EnableViewState="false" />
                                                                    </strong>
                                                                </h4>
                                                            </th>
                                                            <th style="background:#ccc !important;">
                                                                <h4>
                                                                    <small>Color Separation</small>
                                                                    <br />
                                                                    <strong style="font-size:15px;">
                                                                        <asp:Literal Text="text" ID="ltrHeaderColor" runat="server" EnableViewState="false" />
                                                                    </strong>
                                                                </h4>
                                                            </th>

                                                            <th style="background:#ccc !important;">
                                                                <h4>
                                                                    <small>Status</small>
                                                                    <br />
                                                                    <strong style="font-size:15px;">
                                                                        <asp:Literal Text="text" ID="ltrHeaderStatus" runat="server" EnableViewState="false" />
                                                                    </strong>
                                                                </h4>
                                                            </th>
                                                            <th style="background:#ccc !important;">
                                                                <h4>
                                                                    <small>Diameter</small>
                                                                    <br />
                                                                    <strong style="font-size:15px;">
                                                                        <asp:Literal Text="text" ID="ltrHeaderDiameter" runat="server" EnableViewState="false" />
                                                                    </strong>
                                                                </h4>
                                                            </th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <asp:Repeater runat="server" ID="rptCylinder" OnItemDataBound="rptCylinder_ItemDataBound">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Literal Text="text" ID="ltrCylSeq" runat="server" EnableViewState="false" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Literal Text="text" ID="ltrCylBarcode" runat="server" EnableViewState="false" />
                                                                    </td>
                                                                    <%--<td>
                                                                        <asp:Literal Text="text" ID="ltrCylNr" runat="server" EnableViewState="false" />
                                                                    </td>--%>
                                                                     <td>
                                                                        <asp:Literal Text="text" ID="ltrCylID" runat="server" EnableViewState="false" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Literal Text="text" ID="ltrCustSteelBase" runat="server" EnableViewState="false" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Literal Text="text" ID="ltrColor" runat="server" EnableViewState="false" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Literal Text="text" ID="ltrStatus" runat="server" EnableViewState="false" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Literal Text="text" ID="ltrDiameter" runat="server" EnableViewState="false" />
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <hr style="border-color: #b2b2b2; border-style: dashed" class="hidden-print" />
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </form>
</body>
</html>
