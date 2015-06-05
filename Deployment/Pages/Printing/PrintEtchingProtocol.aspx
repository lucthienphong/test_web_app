<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintEtchingProtocol.aspx.cs"
    Inherits="SweetSoft.APEM.WebApp.Pages.Printing.PrintEtchingProtocol" EnableViewState="false" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title></title>
    <link href="/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body {
            margin-top: 20px;
        }

        .table-bordered > thead > tr > th, .table-bordered > tbody > tr > th, .table-bordered > tfoot > tr > th, .table-bordered > thead > tr > td, .table-bordered > tbody > tr > td, .table-bordered > tfoot > tr > td {
            border-color: #000;
        }

        small {
            color: #000!important;
        }

        @media all {
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
        }

        @page {
            size: auto;
            margin: 50mm 6mm 20mm 6mm;
            margin-top: 50mm;
        }

        @media print {
            body {
                margin: 0;
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
        }
    </style>
</head>
<body style="background: #fff">
    <form id="form1" runat="server">
        <div class="container-fluid" id="wrapPrint">
            <div class="printContent">
                <div class="row" style="display: none;">
                    <div class="col-xs-12 text-right" style="margin-bottom: 15px;">
                        <span class="form-control-static">
                            <strong>
                                <asp:Literal Text="Asia-Pacific Engravers (Malaysia) Sdn. Bhd." ID="ltrCompanyName" EnableViewState="false" runat="server" />
                            </strong>
                        </span>
                        <img src="/img/logo-print1.png" class="img-responsive" style="height: 12mm; vertical-align: bottom; display: inline-block" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12">
                        <div style="display: table; width: 101%; left: -3px; right: -3px; border-spacing: 3px 0; position: relative; margin-bottom: 3px">
                            <div style="display: table-row">
                                <div style="display: table-cell; border: 1px solid #000; border-right: 1px solid #000; width: 54mm; background: #4569F2; color: #fff">
                                    <h5 style="color: #fff; text-align: center"><strong style="color: #fff">Laser Etching Protocol</strong>
                                    </h5>
                                </div>
                                <div style="display: table-cell; border: 1px solid #000;">
                                    <div style="display: table; width: 100%">
                                        <div style="display: table-cell; border-right: 1px solid #000;">
                                            <h5>
                                                <small>Client: </small>
                                                <strong>
                                                    <asp:Literal Text="text" ID="ltrClient" runat="server" EnableViewState="false" />
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
                                        <div style="width: 70px; display: none">
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
                                            <h5>
                                                <small>Job Name: </small>
                                                <strong>
                                                    <asp:Literal Text="text" ID="ltrJobName" runat="server" EnableViewState="false" />
                                                </strong>
                                            </h5>
                                        </div>
                                        <div style="display: table-cell">
                                            <h5>
                                                <small>Chrome Thickness: </small>
                                                <strong>
                                                    <asp:Literal ID="ltrChromeThickness" runat="server" />
                                                </strong>
                                            </h5>
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
                                            <h5>
                                                <small>Design: </small>
                                                <strong>
                                                    <asp:Literal Text="text" ID="ltrJobDesign" runat="server" EnableViewState="false" />
                                                </strong>
                                            </h5>
                                        </div>
                                        <div style="display: table-cell">
                                            <h5>
                                                <small>Roughness: </small>
                                                <strong>
                                                    <asp:Literal ID="ltrRoughness" runat="server" EnableViewState="false" />
                                                </strong>
                                            </h5>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div style="display: table; width: 101%; left: -3px; right: -3px; border-spacing: 3px 0; position: relative; margin-bottom: 3px">
                            <div style="display: table-row">
                                <div style="display: table-cell; border: 1px solid #000">
                                    <div style="display: table; width: 100%; border-spacing: 3px 0">
                                        <div style="display: table-cell; border-right: 1px solid #000; width: 54mm">
                                            <h5>
                                                <small>Root Job Nr:</small>
                                                <strong>
                                                    <asp:Literal Text="text" ID="ltrRootJobNr" runat="server" EnableViewState="false" /></strong>
                                            </h5>
                                        </div>
                                        <div style="display: table-cell; border-right: 1px solid #000">
                                            <h5>
                                                <small>Common Job Nr: </small>
                                                <strong>
                                                    <asp:Literal Text="text" ID="ltrCommonJobNr" runat="server" EnableViewState="false" /></strong>
                                            </h5>
                                        </div>
                                        <div style="display: table-cell; border-right: 1px solid #000">
                                            <h5>
                                                <small>Mother set: </small>
                                                <strong></strong>
                                            </h5>
                                        </div>
                                        <div style="display: table-cell; width: 35mm">
                                            <h6 style="margin: 0;">
                                                <small>Create By: </small>
                                                <strong>
                                                    <asp:Literal Text="text" ID="ltrCreateBy" runat="server" EnableViewState="false" /></strong>
                                            </h6>
                                            <h6 style="margin: 0; margin-bottom: 3px; display: none">
                                                <small>Date: </small>
                                                <strong>
                                                    <asp:Literal Text="text" ID="ltrDate" runat="server" EnableViewState="false" /></strong>
                                            </h6>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div style="display: table; width: 101%; left: -3px; right: -3px; border-spacing: 3px 0; position: relative;">
                            <div style="display: table-row;">
                                <div style="display: table-cell; border: 1px solid #000;">
                                    <div style="display: table; width: 100%; border-spacing: 3px;">
                                        <div style="display: table-row;">
                                            <div style="display: table-cell; padding: 3px 0">
                                                <h5 style="margin-top: 3px; margin-bottom: 0">
                                                    <strong>Circumference</strong>
                                                </h5>
                                            </div>
                                            <div style="display: table-cell; padding: 3px 0">
                                                <h5 style="margin-top: 3px; margin-bottom: 0">
                                                    <strong>Facewidth</strong>
                                                </h5>
                                            </div>
                                            <div style="display: table-cell; padding: 3px 0">
                                                <h5 style="margin-top: 3px; margin-bottom: 0">
                                                    <strong>Start Position</strong>
                                                </h5>
                                            </div>
                                            <div style="display: table-cell; padding: 3px 0">
                                                <h5 style="margin-top: 3px; margin-bottom: 0">
                                                    <strong>Laser Width</strong>
                                                </h5>
                                            </div>
                                            <div style="display: table-cell; padding: 3px 0">
                                                <h5 style="margin-top: 3px; margin-bottom: 0">
                                                    <strong>File Size Vertical</strong>
                                                </h5>
                                            </div>
                                            <div style="display: table-cell; padding: 3px 0">
                                                <h5 style="margin-top: 3px; margin-bottom: 0">
                                                    <strong>File Size Horizandal </strong>
                                                </h5>
                                            </div>
                                        </div>
                                        <div style="display: table-row;">
                                            <div style="display: table-cell; padding: 3px; border: 1px solid #000">
                                                <strong>
                                                    <asp:Literal EnableViewState="false" ID="ltrCircumference" Text="Circumference" runat="server"></asp:Literal>
                                                </strong>
                                            </div>
                                            <div style="display: table-cell; padding: 3px; border: 1px solid #000">
                                                <strong>
                                                    <asp:Literal EnableViewState="false" ID="ltrFaceWidth" Text="Facewidth" runat="server"></asp:Literal>
                                                </strong>
                                            </div>
                                            <div style="display: table-cell; padding: 3px; border: 1px solid #000">
                                                <strong>
                                                    <asp:Literal EnableViewState="false" ID="ltrEngravingStart" Text=" " runat="server"></asp:Literal>
                                                </strong>
                                            </div>
                                            <div style="display: table-cell; padding: 3px; border: 1px solid #000">
                                                <strong>
                                                    <asp:Literal EnableViewState="false" ID="ltrEngravingWidth" Text=" " runat="server"></asp:Literal>
                                                </strong>
                                            </div>
                                            <div style="display: table-cell; padding: 3px; border: 1px solid #000">
                                                <strong>
                                                    <span style="min-height: 17px; display: block">
                                                        <asp:Literal EnableViewState="false" ID="ltrUnitSizeVer" Text="" runat="server"></asp:Literal>
                                                    </span>
                                                </strong>
                                            </div>
                                            <div style="display: table-cell; padding: 3px; border: 1px solid #000">
                                                <strong>
                                                    <asp:Literal EnableViewState="false" ID="ltrUnitSizeHor" Text="" runat="server"></asp:Literal>
                                                </strong>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div style="display: table-cell; width: 54mm; border: 1px solid #000;">
                                    <div style="display: table; width: 100%; border-spacing: 3px;">
                                        <div style="display: table-row;">
                                            <div style="display: table-cell; padding: 3px 0">
                                                <h5 style="margin-top: 3px">
                                                    <strong>Printing</strong>
                                                </h5>
                                            </div>
                                        </div>
                                        <div style="display: table-row;">
                                            <div style="display: table-cell; padding: 3px 0">
                                                <h3 style="margin-top: 3px;">
                                                    <strong>
                                                        <asp:Literal EnableViewState="false" ID="ltrPrinting" runat="server"></asp:Literal>
                                                    </strong>
                                                </h3>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div style="display: table-row;">
                                <div style="display: table-cell;">
                                    <div style="margin-top: 20px;">
                                        Laser start keyway at right positions: <span><asp:Literal ID="ltrLaserStart" runat="server"></asp:Literal></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div style="display: table; width: 101%; left: -3px; right: -3px; border-spacing: 3px 0; position: relative; margin-bottom: 3px">
                            <div style="display: table-row">
                                <div style="display: table-cell">
                                    <asp:Repeater runat="server" ID="rptCylinder">
                                        <HeaderTemplate>
                                            <table class="table table-bordered" style="margin-top: 10px;">
                                                <tr>
                                                    <td colspan="7" style="border-left: 1px solid #fff; border-top: 1px solid #fff">
                                                        <div style="display: table; width: 101%; left: -3px; right: -3px; border-spacing: 3px 0; position: relative; margin-bottom: 3px">
                                                            <div style="display: table-row">
                                                                <div style="display: table-cell">
                                                                    <h5 style="margin: 0"><strong>Parameters for job</strong></h5>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td colspan="3" class="text-center">
                                                        <span style="margin-top: 5px; display: block">
                                                            <label class="control-label">Target Cell Value</label>
                                                        </span>
                                                    </td>
                                                    <td style="border-right-color: #fff; border-top-color: #fff"></td>
                                                    <td style="border-top-color: #fff"></td>
                                                    <td colspan="3" class="text-center">
                                                        <span style="margin-top: 5px; display: block">
                                                            <label class="control-label">Actual value in Chrome</label>
                                                        </span></td>
                                                </tr>
                                                <tr>
                                                    <td class="text-center">Seq</td>
                                                    <td class="text-center">Cyl.Nr</td>
                                                    <td class="text-center">Colour/Sepration</td>
                                                    <td class="text-center">Screen Lpi</td>
                                                    <td class="text-center">Cell type</td>
                                                    <td class="text-center">Angle</td>
                                                    <td class="text-center">Gamma</td>
                                                    <td class="text-center">Cell size µ</td>
                                                    <td class="text-center">Cell wall µ</td>
                                                    <td class="text-center">Cell dept µ</td>
                                                    <td class="text-center">Developing time/sec</td>
                                                    <td class="text-center">Etching time/sec</td>
                                                    <td class="text-center">Cell size µ</td>
                                                    <td class="text-center">Cell wall µ</td>
                                                    <td class="text-center">Cell dept µ</td>
                                                </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td><span><%# Eval("Sequence") %></span></td>
                                                <td><span><%# Eval("CylinderNo") %></span></td>
                                                <td><span><%# Eval("Color") %></span></td>
                                                <td><span><%# Eval("ScreenLpi") %></span></td>
                                                <td><span><%# Eval("CellType") %></span></td>
                                                <td><span><%# Eval("Angle") %></span></td>
                                                <td><span><%# Eval("Gamma") %></span></td>
                                                <td><span><%# Eval("TargetCellSize") %></span></td>
                                                <td><span><%# Eval("TargetCellWall") %></span></td>
                                                <td><span><%# Eval("TargetCellDepth") %></span></td>
                                                <td><span><%# Eval("DevelopingTime") %></span></td>
                                                <td><span><%# Eval("EtchingTime") %></span></td>
                                                <td><span><%# Eval("ChromeCellSize") %></span></td>
                                                <td><span><%# Eval("ChromeCellWall") %></span></td>
                                                <td><span><%# Eval("ChromeCellDepth") %></span></td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </div>
                        
                        <div style="display: table; width: 101%; left: -3px; right: -3px; border-spacing: 3px 0; position: relative; margin-bottom: 3px">
                            <div style="display: table-row">
                                <div style="display: table-cell; border: 1px solid #000; padding: 3px">
                                    <h5><strong>Remark</strong></h5>
                                    <span style="min-height: 10mm; display: block">
                                        <asp:Literal Text="text" ID="ltrRemark" EnableViewState="false" runat="server" />
                                    </span>
                                </div>
                            </div>
                        </div>
                        <div style="display: table; width: 101%; left: -3px; right: -3px; border-spacing: 3px 0; position: relative; margin-bottom: 3px">
                            <div style="display: table-row">
                                <div style="display: none">
                                    <h5 style="display: inline-block"><strong>Date</strong></h5>
                                    <span style="min-height: 10mm; min-width: 20mm; display: inline-block; border-bottom: 1px solid #000; padding-bottom: 3px"></span>
                                </div>
                                <div style="display: table-cell">
                                    <h5 style="display: inline-block"><strong>Laser Operator</strong></h5>
                                    <%--<span style="min-height: 10mm; min-width: 20mm; display: inline-block; border-bottom: 1px solid #000; padding-bottom: 3px"></span>--%>
                                    <span><asp:Literal ID="ltrLaserOperator" runat="server"></asp:Literal></span>
                                </div>
                                <div style="display: table-cell">
                                    <h5 style="display: inline-block"><strong>Final Control (QA) </strong></h5>
                                    <%--<span style="min-height: 10mm; min-width: 20mm; display: inline-block; border-bottom: 1px solid #000; padding-bottom: 3px"></span>--%>
                                    <span><asp:Literal ID="ltrFinalControl" runat="server"></asp:Literal></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
