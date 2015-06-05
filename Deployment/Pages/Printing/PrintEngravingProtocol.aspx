<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintEngravingProtocol.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.Printing.PrintEngravingProtocol" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
    <link href="/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        .column-20 {
            min-width: 25px;
            max-width: 25px;
            width: 25px;
            text-align: center;
        }

        .column-60 {
            min-width: 75px;
            max-width: 75px;
            width: 75px;
            text-align: center;
        }

        .column-80 {
            min-width: 80px;
            max-width: 80px;
            width: 80px;
        }

        .column-100 {
            min-width: 100px;
            max-width: 100px;
            width: 100px;
        }

        .column-120 {
            min-width: 120px;
            max-width: 120px;
            width: 120px;
        }

        .column-150 {
            min-width: 150px;
            max-width: 150px;
            width: 150px;
        }

        .column-180 {
            min-width: 180px;
            max-width: 180px;
            width: 180px;
        }

        .column-250 {
            min-width: 250px;
            max-width: 250px;
            width: 250px;
        }

        .cell-height-115 {
            height: 115px;
        }

        .cell-height-160 {
            height: 115px;
        }

        .wrap-rotate {
            width: 30px;
            text-align: center;
        }

        .rotate90 {
            -webkit-transform: rotate(270deg);
            -moz-transform: rotate(270deg);
            -o-transform: rotate(270deg);
            writing-mode: lr-tb;
            -webkit-transform: rotate(270deg);
            -moz-transform: rotate(270deg);
            -o-transform: rotate(270deg);
            -webkit-transform: rotate(270deg) translateX(100%) translateY(33%);
            -webkit-transform-origin: 100% 100%;
            white-space: nowrap;
        }

        .vheader {
            display: table-cell;
            vertical-align: middle;
        }

        @media screen {
            @media all {
                body {
                    font-size: 11px !important;
                }

                .form-control-static {
                    padding: 0 !important;
                    font-size: 11px !important;
                }

                .control-label {
                    padding-top: 0 !important;
                    padding-bottom: 0 !important;
                    font-size: 11px !important;
                }

                .form-control-static {
                    border-bottom: 1px dashed #000 !important;
                    padding-bottom: 2px !important;
                    font-size: 11px !important;
                }

                    .form-control-static:before {
                        content: ": ";
                    }

                .form-group {
                    margin-bottom: 8px !important;
                    font-size: 11px !important;
                }

                .head .control-label {
                    text-align: left;
                    white-space: nowrap;
                    padding-right: 0;
                    font-weight: bold;
                    font-size: 11px !important;
                }

                td, th {
                    vertical-align: middle !important;
                }
            }

            @media print {
                .form-group {
                    margin-bottom: 3px !important;
                }

                body {
                    font-size: 11px !important;
                }

                .form-control-static {
                    font-size: 10px !important;
                }
            }

            @page {
                margin: 0;
                margin-top: 5mm;
                size: landscape;
            }
    </style>
</head>
<body style="background: #fff">
    <form id="form1" runat="server">
        <div class="container-fluid" id="wrapPrint">
            <div class="printContent">
                <div class="row">
                    <div class="col-xs-10">
                        <div class="row">
                            <div class="col-xs-3">
                                <h3><strong>Engraving Protocol</strong></h3>
                            </div>
                            <div class="col-xs-7">
                                <h3>Tobacco</h3>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-2" style="text-align:right;">
                        <img src="/img/apem-logo-print.png" class="img-responsive" style="vertical-align: bottom; display: inline-block; width: 100px;" />
                    </div>
                    <div class="col-xs-12" style="margin-top: 15px;">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="form-horizontal head">
                                    <div class="row">
                                        <div class="col-xs-4">
                                            <div class="form-group">
                                                <label class="col-xs-2 control-label">Client</label>
                                                <div class="col-xs-10">
                                                    <p style="border-bottom: 1px dashed #000 !important;" class="form-control-static">
                                                        <asp:Literal ID="ltrCustomer" EnableViewState="false" runat="server"></asp:Literal>
                                                    </p>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-xs-8">
                                            <div class="form-group">
                                                <label class="col-xs-2 control-label">Job Number</label>
                                                <div class="col-xs-2">
                                                    <p style="border-bottom: 1px dashed #000 !important;" class="form-control-static">
                                                        <asp:Literal ID="ltrJobNumber" EnableViewState="false" runat="server"></asp:Literal>
                                                    </p>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-4">
                                <div class="form-horizontal head">                                    
                                    <div class="form-group">
                                        <label class="col-xs-2 control-label">Job Name</label>
                                        <div class="col-xs-10">
                                            <p style="border-bottom: 1px dashed #000 !important;" class="form-control-static">
                                                <asp:Literal ID="ltrJobName" EnableViewState="false" runat="server"></asp:Literal>
                                            </p>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-xs-2 control-label">Design Name</label>
                                        <div class="col-xs-10">
                                            <p style="border-bottom: 1px dashed #000 !important;" class="form-control-static">
                                                <asp:Literal ID="ltrDesign" EnableViewState="false" runat="server"></asp:Literal>
                                            </p>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-xs-2 control-label">Root Job No.</label>
                                        <div class="col-xs-4">
                                            <p style="border-bottom: 1px dashed #000 !important;" class="form-control-static">
                                                <asp:Literal ID="ltrRootJob" EnableViewState="true" runat="server"></asp:Literal>
                                            </p>
                                        </div>
                                        <label class="col-xs-2 control-label">Common Job</label>
                                        <div class="col-xs-4">
                                            <p style="border-bottom: 1px dashed #000 !important;" class="form-control-static">
                                                <asp:Literal ID="ltrCommonJob" EnableViewState="true" runat="server"></asp:Literal>
                                            </p>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-xs-2 control-label">Repro Size</label>
                                        <div class="col-xs-10">
                                            <p style="border-bottom: 1px dashed #000 !important;" class="form-control-static">
                                                <asp:Literal ID="ltrReproSize" EnableViewState="false" runat="server"></asp:Literal>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-3">
                                <div class="form-horizontal head">
                                    <div class="form-group" style="margin-top:2px;">
                                        <label class="col-xs-6 control-label">Engrave On Nut</label>
                                        <asp:Literal ID="ltrEngraveOnNut" EnableViewState="false" runat="server"></asp:Literal>
                                    </div>
                                    <div class="form-group" style="margin-top:10px;">
                                        <label class="col-xs-6 control-label">Engrave Close to Boarder</label>
                                        <asp:Literal ID="ltrEngraveCloseToBoarder" EnableViewState="false" runat="server"></asp:Literal>
                                    </div>
                                    <div class="form-group" style="margin-top:11px;">
                                        <label class="col-xs-5 control-label">Circumference</label>
                                        <div class="col-xs-7">
                                            <p style="border-bottom: 1px dashed #000 !important;" class="form-control-static">
                                                <asp:Literal ID="ltrCircumference" EnableViewState="false" runat="server"></asp:Literal>mm
                                            </p>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-xs-5 control-label">Operator</label>
                                        <div class="col-xs-7">
                                            <p style="border-bottom: 1px dashed #000 !important;" class="form-control-static">
                                                <asp:Literal ID="ltrJobOperator" EnableViewState="false" runat="server"></asp:Literal>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-3">
                                <div class="form-horizontal head">
                                    <div class="form-group" style="margin-top: 27px">
                                        <label class="col-xs-2 control-label">&nbsp</label>
                                        <div class="col-xs-10">
                                            <p></p>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-xs-4 control-label">Chrome Thickness</label>
                                        <div class="col-xs-8">
                                            <p class="form-control-static" style="border-bottom: 1px dashed #000 !important;">
                                                <asp:Literal ID="ltrChromeThickness" EnableViewState="false" runat="server"></asp:Literal>
                                            </p>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-xs-4 control-label">Roughness</label>
                                        <div class="col-xs-8">
                                            <p class="form-control-static" style="border-bottom: 1px dashed #000 !important;">
                                                <asp:Literal ID="ltrRoughness" EnableViewState="false" runat="server"></asp:Literal> 
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-2">
                                <div class="form-horizontal head">
                                    <div class="form-group" style="margin-top: 2px">
                                        <label class="col-xs-2 control-label">&nbsp</label>
                                        <div class="col-xs-10">
                                            <p></p>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-xs-4 control-label">Date</label>
                                        <div class="col-xs-8">
                                            <p class="form-control-static" style="border-bottom: 1px dashed #000 !important;">
                                                <asp:Literal ID="ltrCreatedOn" EnableViewState="false" runat="server"></asp:Literal>
                                            </p>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-xs-4 control-label">Cyl. Width</label>
                                        <div class="col-xs-8">
                                            <p class="form-control-static" style="border-bottom: 1px dashed #000 !important;">
                                                <asp:Literal ID="ltrFaceWidth" EnableViewState="false" runat="server"></asp:Literal>
                                            </p>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-xs-12 control-label">Surface 
                                            <asp:Literal ID="ltrSurface" EnableViewState="false" runat="server"></asp:Literal>
                                             / Reserse 
                                            <asp:Literal ID="ltrReserse" EnableViewState="false" runat="server"></asp:Literal>
                                        </label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12">
                        <div>
                            <table class="table table-bordered">
                                <tr>
                                    <th rowspan="3" class="column-20">
                                        <%--<div class="vheaders">Sep.</div>--%>
                                        <img alt="Engraving Spec" src="/img/dls/DLS_Sep.png" />
                                    </th>
                                    <th class="column-150">Position</th>
                                    <th class="column-60">(1)</th>
                                    <th class="column-60">(2)</th>
                                    <th class="column-60">(3)</th>
                                    <th class="column-60">(4)</th>
                                    <th class="column-60">(5)</th>
                                    <th class="column-60">(6)</th>
                                    <th class="column-60">(7)</th>
                                    <th class="column-60">(8)</th>
                                    <th class="column-60">(9)</th>
                                    <th class="column-60">(10)</th>
                                    <th class="column-60">(11)</th>
                                    <th class="column-60">(12)</th>
                                    <th class="column-60">(13)</th>
                                    <th class="column-60">(14)</th>
                                    <th class="column-60">(15)</th>
                                    <th class="column-60">(16)</th>
                                    <th class="column-60">(17) Barcodes</th>
                                </tr>
                                <tr>
                                    <td class="column-150">Colour</td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrColor1" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrColor2" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrColor3" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrColor4" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrColor5" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrColor6" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrColor7" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrColor8" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrColor9" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrColor10" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrColor11" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrColor12" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrColor13" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrColor14" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrColor15" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrColor16" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrColor17" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <td class="column-150">Cylinder ID</td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCylID1" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCylID2" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCylID3" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCylID4" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCylID5" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCylID6" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCylID7" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCylID8" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCylID9" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCylID10" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCylID11" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCylID12" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCylID13" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCylID14" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCylID15" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCylID16" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCylID17" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <td rowspan="5" style="width:20px;">
                                        <%--<div class="vheader">Engraving Spec</div>--%>
                                        <img alt="Engraving Spec" src="/img/dls/DLS_EngravingSpec.png" />
                                    </td>
                                    <td class="column-150">Screen</td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrScreen1" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrScreen2" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrScreen3" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrScreen4" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrScreen5" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrScreen6" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrScreen7" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrScreen8" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrScreen9" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrScreen10" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrScreen11" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrScreen12" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrScreen13" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrScreen14" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrScreen15" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrScreen16" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrScreen17" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <td class="column-150">Master screen</td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrMasterScreen1" runat="server"
                                            Text="<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrMasterScreen2" runat="server"
                                            Text="<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrMasterScreen3" runat="server"
                                            Text="<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrMasterScreen4" runat="server"
                                            Text="<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrMasterScreen5" runat="server"
                                            Text="<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrMasterScreen6" runat="server"
                                            Text="<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrMasterScreen7" runat="server"
                                            Text="<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrMasterScreen8" runat="server"
                                            Text="<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrMasterScreen9" runat="server"
                                            Text="<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrMasterScreen10" runat="server"
                                            Text="<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrMasterScreen11" runat="server"
                                            Text="<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrMasterScreen12" runat="server"
                                            Text="<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrMasterScreen13" runat="server"
                                            Text="<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrMasterScreen14" runat="server"
                                            Text="<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrMasterScreen15" runat="server"
                                            Text="<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrMasterScreen16" runat="server"
                                            Text="<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrMasterScreen17" runat="server"
                                            Text="<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <td class="column-150">Angle</td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrAngle1" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrAngle2" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrAngle3" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrAngle4" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrAngle5" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrAngle6" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrAngle7" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrAngle8" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrAngle9" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrAngle10" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrAngle11" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrAngle12" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrAngle13" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrAngle14" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrAngle15" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrAngle16" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrAngle17" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <td class="column-150">Elongation  [ 0 shape ]</td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrElongation1" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrElongation2" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrElongation3" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrElongation4" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrElongation5" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrElongation6" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrElongation7" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrElongation8" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrElongation9" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrElongation10" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrElongation11" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrElongation12" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrElongation13" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrElongation14" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrElongation15" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrElongation16" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrElongation17" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <td class="column-150">Distortion [ mm ]</td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrDistortion1" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrDistortion2" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrDistortion3" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrDistortion4" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrDistortion5" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrDistortion6" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrDistortion7" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrDistortion8" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrDistortion9" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrDistortion10" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrDistortion11" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrDistortion12" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrDistortion13" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrDistortion14" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrDistortion15" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrDistortion16" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrDistortion17" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <td rowspan="2" style="width:20px;">
                                        <%--<div class="vheader">M.S.</div>--%>
                                        <img alt="Engraving Spec" src="/img/dls/DLS_MS.png" 
                                    </td>
                                    <td class="column-150">Resolution (Lines)</td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrResolution1" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrResolution2" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrResolution3" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrResolution4" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrResolution5" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrResolution6" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrResolution7" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrResolution8" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrResolution9" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrResolution10" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrResolution11" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrResolution12" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrResolution13" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrResolution14" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrResolution15" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrResolution16" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrResolution17" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <td class="column-150">Hexagotnal Dot (Shape)</td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrHexagotnal1" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrHexagotnal2" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrHexagotnal3" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrHexagotnal4" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrHexagotnal5" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrHexagotnal6" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrHexagotnal7" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrHexagotnal8" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrHexagotnal9" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrHexagotnal10" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrHexagotnal11" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrHexagotnal12" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrHexagotnal13" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrHexagotnal14" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrHexagotnal15" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrHexagotnal16" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrHexagotnal17" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <td rowspan="4" style="width:20px;">
                                        <%--<div class="vheader">Sharpness</div>--%>
                                        <img alt="Engraving Spec" src="/img/dls/DLS_Sharpness.png" />
                                    </td>
                                    <td class="column-150">Image smoothness</td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrImageSmoothness1" runat="server"
                                            Text="<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrImageSmoothness2" runat="server"
                                            Text="<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrImageSmoothness3" runat="server"
                                            Text="<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrImageSmoothness4" runat="server"
                                            Text="<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrImageSmoothness5" runat="server"
                                            Text="<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrImageSmoothness6" runat="server"
                                            Text="<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrImageSmoothness7" runat="server"
                                            Text="<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrImageSmoothness8" runat="server"
                                            Text="<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrImageSmoothness9" runat="server"
                                            Text="<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrImageSmoothness10" runat="server"
                                            Text="<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrImageSmoothness11" runat="server"
                                            Text="<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrImageSmoothness12" runat="server"
                                            Text="<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrImageSmoothness13" runat="server"
                                            Text="<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrImageSmoothness14" runat="server"
                                            Text="<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrImageSmoothness15" runat="server"
                                            Text="<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrImageSmoothness16" runat="server"
                                            Text="<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrImageSmoothness17" runat="server"
                                            Text="<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <td class="column-150">Unsharp Masking (1-10)</td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrUnsharpMarking1" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrUnsharpMarking2" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrUnsharpMarking3" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrUnsharpMarking4" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrUnsharpMarking5" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrUnsharpMarking6" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrUnsharpMarking7" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrUnsharpMarking8" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrUnsharpMarking9" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrUnsharpMarking10" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrUnsharpMarking11" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrUnsharpMarking12" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrUnsharpMarking13" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrUnsharpMarking14" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrUnsharpMarking15" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrUnsharpMarking16" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrUnsharpMarking17" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <td class="column-150">Antialiasing</td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrAntialiasing1" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrAntialiasing2" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrAntialiasing3" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrAntialiasing4" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrAntialiasing5" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrAntialiasing6" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrAntialiasing7" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrAntialiasing8" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrAntialiasing9" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrAntialiasing10" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrAntialiasing11" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrAntialiasing12" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrAntialiasing13" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrAntialiasing14" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrAntialiasing15" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrAntialiasing16" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrAntialiasing17" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <td class="column-150">LineWork Widening</td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrLineWorkWidening1" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrLineWorkWidening2" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrLineWorkWidening3" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrLineWorkWidening4" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrLineWorkWidening5" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrLineWorkWidening6" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrLineWorkWidening7" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrLineWorkWidening8" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrLineWorkWidening9" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrLineWorkWidening10" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrLineWorkWidening11" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrLineWorkWidening12" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrLineWorkWidening13" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrLineWorkWidening14" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrLineWorkWidening15" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrLineWorkWidening16" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrLineWorkWidening17" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <td rowspan="6" style="width:20px;">
                                        <%--<div class="vheader">Engrave</div--%>
                                        <img alt="Engraving Spec" src="/img/dls/DLS_Engrave.png" />
                                    </td>
                                    <td class="column-150">Engraving Start (mm)</td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingStart1" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingStart2" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingStart3" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingStart4" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingStart5" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingStart6" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingStart7" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingStart8" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingStart9" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingStart10" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingStart11" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingStart12" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingStart13" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingStart14" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingStart15" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingStart16" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingStart17" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <td class="column-150">Engraving Width (mm)</td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingWidth1" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingWidth2" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingWidth3" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingWidth4" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingWidth5" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingWidth6" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingWidth7" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingWidth8" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingWidth9" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingWidth10" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingWidth11" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingWidth12" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingWidth13" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingWidth14" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingWidth15" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingWidth16" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingWidth17" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <td class="column-150">Cell Shape</td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellShape1" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellShape2" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellShape3" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellShape4" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellShape5" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellShape6" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellShape7" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellShape8" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellShape9" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellShape10" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellShape11" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellShape12" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellShape13" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellShape14" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellShape15" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellShape16" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellShape17" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <td class="column-150">Gradation</td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrGradation1" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrGradation2" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrGradation3" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrGradation4" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrGradation5" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrGradation6" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrGradation7" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrGradation8" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrGradation9" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrGradation10" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrGradation11" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrGradation12" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrGradation13" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrGradation14" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrGradation15" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrGradation16" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrGradation17" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <td class="column-150">Gamma</td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrGamma1" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrGamma2" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrGamma3" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrGamma4" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrGamma5" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrGamma6" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrGamma7" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrGamma8" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrGamma9" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrGamma10" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrGamma11" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrGamma12" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrGamma13" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrGamma14" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrGamma15" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrGamma16" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrGamma17" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <td class="column-150">Laser</td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrLaser1" runat="server"
                                            Text="A&nbsp;<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>&nbsp;B&nbsp;<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrLaser2" runat="server"
                                            Text="A&nbsp;<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>&nbsp;B&nbsp;<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrLaser3" runat="server"
                                            Text="A&nbsp;<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>&nbsp;B&nbsp;<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrLaser4" runat="server"
                                            Text="A&nbsp;<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>&nbsp;B&nbsp;<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrLaser5" runat="server"
                                            Text="A&nbsp;<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>&nbsp;B&nbsp;<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrLaser6" runat="server"
                                            Text="A&nbsp;<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>&nbsp;B&nbsp;<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrLaser7" runat="server"
                                            Text="A&nbsp;<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>&nbsp;B&nbsp;<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrLaser8" runat="server"
                                            Text="A&nbsp;<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>&nbsp;B&nbsp;<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrLaser9" runat="server"
                                            Text="A&nbsp;<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>&nbsp;B&nbsp;<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrLaser10" runat="server"
                                            Text="A&nbsp;<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>&nbsp;B&nbsp;<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrLaser11" runat="server"
                                            Text="A&nbsp;<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>&nbsp;B&nbsp;<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrLaser12" runat="server"
                                            Text="A&nbsp;<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>&nbsp;B&nbsp;<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrLaser13" runat="server"
                                            Text="A&nbsp;<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>&nbsp;B&nbsp;<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrLaser14" runat="server"
                                            Text="A&nbsp;<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>&nbsp;B&nbsp;<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrLaser15" runat="server"
                                            Text="A&nbsp;<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>&nbsp;B&nbsp;<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrLaser16" runat="server"
                                            Text="A&nbsp;<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>&nbsp;B&nbsp;<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrLaser17" runat="server"
                                            Text="A&nbsp;<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>&nbsp;B&nbsp;<span style='border: solid 1px; padding: 1px;'>&nbsp;&nbsp;&nbsp;&nbsp;</span>"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <td rowspan="3" style="width:20px;">
                                        <%--<div class="vheader">Cell size</div>--%>
                                        <img alt="Engraving Spec" src="/img/dls/DLS_CellSize.png" />
                                    </td>
                                    <td class="column-150">Cell Width [µ]</td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellWidth1" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellWidth2" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellWidth3" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellWidth4" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellWidth5" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellWidth6" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellWidth7" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellWidth8" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellWidth9" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellWidth10" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellWidth11" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellWidth12" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellWidth13" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellWidth14" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellWidth15" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellWidth16" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellWidth17" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <td class="column-150">Channel width [µ]</td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrChannelWidth1" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrChannelWidth2" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrChannelWidth3" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrChannelWidth4" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrChannelWidth5" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrChannelWidth6" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrChannelWidth7" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrChannelWidth8" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrChannelWidth9" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrChannelWidth10" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrChannelWidth11" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrChannelWidth12" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrChannelWidth13" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrChannelWidth14" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrChannelWidth15" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrChannelWidth16" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrChannelWidth17" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <td class="column-150">Cell Dept [µ]</td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellDepth1" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellDepth2" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellDepth3" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellDepth4" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellDepth5" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellDepth6" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellDepth7" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellDepth8" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellDepth9" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellDepth10" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellDepth11" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellDepth12" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellDepth13" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellDepth14" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellDepth15" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellDepth16" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCellDepth17" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <td colspan="2">Engraving Time</td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingTime1" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingTime2" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingTime3" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingTime4" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingTime5" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingTime6" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingTime7" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingTime8" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingTime9" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingTime10" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingTime11" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingTime12" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingTime13" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingTime14" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingTime15" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingTime16" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrEngravingTime17" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <td colspan="2">Beam</td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrBeam1" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrBeam2" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrBeam3" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrBeam4" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrBeam5" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrBeam6" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrBeam7" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrBeam8" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrBeam9" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrBeam10" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrBeam11" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrBeam12" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrBeam13" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrBeam14" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrBeam15" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrBeam16" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrBeam17" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <td colspan="2">Threshold</td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrThreshold1" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrThreshold2" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrThreshold3" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrThreshold4" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrThreshold5" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrThreshold6" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrThreshold7" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrThreshold8" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrThreshold9" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrThreshold10" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrThreshold11" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrThreshold12" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrThreshold13" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrThreshold14" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrThreshold15" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrThreshold16" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrThreshold17" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <td rowspan="2" style="width:20px;">
                                        <%--<div class="vheader">Checking</div>--%>
                                        <img alt="Engraving Spec" src="/img/dls/DLS_Checking.png" />
                                    </td>
                                    <td class="column-150">Checked by</td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCheckedBy1" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCheckedBy2" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCheckedBy3" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCheckedBy4" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCheckedBy5" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCheckedBy6" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCheckedBy7" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCheckedBy8" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCheckedBy9" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCheckedBy10" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCheckedBy11" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCheckedBy12" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCheckedBy13" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCheckedBy14" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCheckedBy15" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCheckedBy16" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCheckedBy17" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <td class="column-150">Date</td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCheckedOn1" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCheckedOn2" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCheckedOn3" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCheckedOn4" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCheckedOn5" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCheckedOn6" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCheckedOn7" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCheckedOn8" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCheckedOn9" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCheckedOn10" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCheckedOn11" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCheckedOn12" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCheckedOn13" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCheckedOn14" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCheckedOn15" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCheckedOn16" runat="server"></asp:Literal></td>
                                    <td class="column-60">
                                        <asp:Literal ID="ltrCheckedOn17" runat="server"></asp:Literal></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="col-xs-12">
                        <label class="control-label">Remark: </label>
                        <asp:Literal ID="ltrRemark" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
