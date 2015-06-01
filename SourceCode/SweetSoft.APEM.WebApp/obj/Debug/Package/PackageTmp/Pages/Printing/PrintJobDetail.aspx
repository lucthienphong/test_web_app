<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintJobDetail.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.Printing.PrintJobDetail" EnableViewState="false" %>

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
            margin: 0mm;
            margin-top: 5mm;
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
                <div class="row">
                    <div class="col-xs-4" style="margin-bottom: 15px">
                        <asp:Image runat="server" ID="barcodeImage" EnableViewState="false" />
                    </div>
                    <div class="col-xs-8 text-right" style="margin-bottom: 15px">
                        <span class="form-control-static">
                            <strong>
                                <asp:Literal Text="Asia-Pacific Engravers (Malaysia) Sdn. Bhd." ID="ltrCompanyName" EnableViewState="false" runat="server" />
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
                                    <h5 style="color: #fff; text-align: center"><strong style="color: #fff">Job Sheet</strong>
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
                                            <h5>
                                                <small>Job Name: </small>
                                                <strong>
                                                    <asp:Literal Text="text" ID="ltrJobName" runat="server" EnableViewState="false" />
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
                                                <small>Sales Rep: </small>
                                                <strong>
                                                    <asp:Literal Text="text" ID="ltrSaleRep" runat="server" EnableViewState="false" /></strong>
                                            </h5>
                                        </div>
                                        <div style="display: table-cell; width: 35mm">
                                            <h5>
                                                <small>Job Co-Ord: </small>
                                                <strong>
                                                    <asp:Literal Text="text" ID="ltrJobCoop" runat="server" EnableViewState="false" /></strong>
                                            </h5>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                        <div style="display: table; width: 101%; left: -3px; right: -3px; border-spacing: 3px 0; position: relative;">
                            <div style="display: table-row">
                                <div style="display: table-cell; border: 1px solid #000">
                                    <div style="display: table; width: 100%; border-spacing: 3px 0">
                                        <div style="display: table-cell; border-right: 1px solid #000; width: 54mm">
                                            <h5>
                                                <small>Customer Contact:</small>
                                                <strong>
                                                    <asp:Literal Text="text" ID="ltrCustomerContact" runat="server" EnableViewState="false" /></strong>
                                            </h5>
                                        </div>
                                        <div style="display: table-cell; border-right: 1px solid #000">
                                            <h5>
                                                <small>Customer Ref: </small>
                                                <strong><asp:Literal Text="text" ID="ltrCustomerRef" runat="server" EnableViewState="false" /></strong>
                                            </h5>
                                        </div>
                                        <div style="display: table-cell; width: 35mm">
                                            <h6 style="margin: 0;">
                                                <small>Create By: </small>
                                                <strong>
                                                    <asp:Literal Text="text" ID="ltrCreateBy" runat="server" EnableViewState="false" /></strong>
                                            </h6>
                                            <h6 style="margin: 0; margin-bottom: 3px;">
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
                            <div style="display: table-row">
                                <div style="display: table-cell;">
                                    <h5>
                                        <strong>Delivery Schedule</strong>
                                    </h5>
                                </div>

                            </div>
                            <div style="display: table-row;">
                                <div style="display: table-cell; border: 1px solid #000;">
                                    <div style="display: table; width: 100%; border-spacing: 3px;">
                                        <div style="display: table-row">
                                            <div style="display: table-cell; width: 54mm;">
                                                <h5 style="margin-top: 3px">
                                                    <strong>Repro Delivery</strong>
                                                </h5>
                                            </div>
                                            <div style="display: table-cell;">
                                                <h5 style="margin-top: 3px">
                                                    <strong>Cylinder Shipping</strong>
                                                </h5>
                                            </div>
                                        </div>
                                        <div style="display: table-row">
                                            <div style="display: table-cell; width: 54mm; border: 1px solid #000;">
                                                <div style="display: table; width: 100%; border-spacing: 3px;">
                                                    <div style="display: table-row">
                                                        <div style="display: table-cell; border: 1px solid #000; padding: 3px">
                                                            <h5>
                                                                <small>Date</small>
                                                                <strong>
                                                                    <asp:Literal Text="text" ID="ltrReproDate" runat="server" EnableViewState="false" /></strong>
                                                            </h5>
                                                        </div>
                                                    </div>
                                                    <div style="display: table-row">
                                                        <div style="display: table-cell; padding: 0 3px">
                                                            <h5 style="margin-top: 3px;">
                                                                <small>IRIS Proof</small>
                                                                <strong>
                                                                    <asp:Literal Text="text" ID="ltrIrisProof" runat="server" EnableViewState="false" /></strong>
                                                            </h5>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div style="display: table-cell; border: 1px solid #000; padding: 3px 0">

                                                <div style="display: table; width: 100%; border-spacing: 0;">
                                                    <div style="display: table-row">
                                                        <div style="display: table-cell;">
                                                            <div style="display: table; width: 100%; border-spacing: 3px 0">
                                                                <div style="display: table-row">
                                                                    <div style="display: table-cell; border: 1px solid #000; padding: 3px">
                                                                        <h5>
                                                                            <small>Date:</small>
                                                                            <strong>
                                                                                <asp:Literal Text="text" ID="ltrCylinderDate" runat="server" EnableViewState="false" /></strong>
                                                                        </h5>
                                                                    </div>
                                                                    <div style="display: table-cell; border: 1px solid #000; padding: 3px">
                                                                        <h5>
                                                                            <strong>
                                                                                <asp:Literal Text="text" ID="ltrLeaving" runat="server" EnableViewState="false" /></strong>
                                                                        </h5>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div style="display: table-row">
                                                        <div style="display: table-cell; padding: 0 3px;">
                                                            <h5 style="margin-top: 3px;">
                                                                <small>Notes</small>
                                                            </h5>
                                                            <span style="min-height: 5mm; display: block">
                                                                <asp:Literal Text="text" ID="ltrDeliveryNotes" runat="server" EnableViewState="false" />
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                        <div style="display: table; width: 101%; left: -3px; right: -3px; border-spacing: 3px 0; position: relative;">
                            <div style="display: table-row">
                                <div style="display: table-cell;">
                                    <h5>
                                        <strong>General Job Info</strong>
                                    </h5>
                                </div>
                            </div>
                            <div style="display: table-row;">
                                <div style="display: table-cell; border: 1px solid #000;">
                                    <div style="display: table; width: 100%; border-spacing: 3px;">
                                        <div style="display: table-row">
                                            <div style="display: table-cell; width: 90mm;">
                                                <h5 style="margin-top: 3px">
                                                    <strong>Repro</strong>
                                                </h5>
                                            </div>
                                            <div style="display: table-cell;">
                                                <h5 style="margin-top: 3px">
                                                    <strong>Proofing</strong>
                                                </h5>
                                            </div>
                                            <div style="display: table-cell; width: 55mm">
                                                <h5 style="margin-top: 3px">
                                                    <strong>S & R</strong>
                                                </h5>
                                            </div>
                                        </div>
                                        <div style="display: table-row">
                                            <div style="display: table-cell; vertical-align: top">
                                                <div style="display: table; width: 100%; border-spacing: 0px;">
                                                    <div style="display: table-row">
                                                        <div style="display: table-cell; border: 1px solid #000; padding: 3px">
                                                            <div style="display: table; width: 100%; border-spacing: 0">
                                                                <div style="display: table-row">
                                                                    <div style="display: table-cell;">
                                                                        <h5>
                                                                            <small>Eye Mark</small>
                                                                        </h5>
                                                                        <h5>
                                                                            <small>Size: </small>
                                                                            <strong>
                                                                                <asp:Literal Text="text" ID="ltrEMSH" runat="server" EnableViewState="false" />
                                                                                x
                                                                                <asp:Literal Text="text" ID="ltrEMSV" runat="server" EnableViewState="false" />&nbsp;mm</strong>
                                                                        </h5>
                                                                        <h5>
                                                                            <small>Colour: </small>
                                                                            <strong>
                                                                                <asp:Literal Text="text" ID="ltrEMColour" runat="server" EnableViewState="false" /></strong>
                                                                        </h5>
                                                                        <h5>
                                                                            <small>Backing</small>
                                                                            <strong>
                                                                                <asp:Literal Text="text" ID="ltrBacking" runat="server" EnableViewState="false" /></strong>
                                                                        </h5>
                                                                    </div>
                                                                    <div style="display: table-cell">
                                                                        <h5 style="display: inline-block">
                                                                            <small style="display: block; margin-bottom: 2mm">Eye Mark Position</small>
                                                                            <asp:Literal ID="ltrEyeImageImage" runat="server" EnableViewState="false" />
                                                                        </h5>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div style="display: table-row">
                                                        <div style="display: table-cell;">
                                                            <div style="display: table; width: 100%; border-spacing: 0px; margin-top: 3px; border-collapse: collapse">
                                                                <div style="display: table-row">
                                                                    <div style="display: table-cell; border: 1px solid #000; padding: 3px">
                                                                        <h5>
                                                                            <small>Barcode</small>
                                                                        </h5>
                                                                        <h5>
                                                                            <small>Size %: </small>
                                                                            <strong>
                                                                                <asp:Literal Text="text" ID="ltrBarcodeSize" runat="server" EnableViewState="false" /></strong>
                                                                        </h5>
                                                                        <h5>
                                                                            <small>Colour: </small>
                                                                            <strong>
                                                                                <asp:Literal Text="text" ID="ltrBarcodeColour" runat="server" EnableViewState="false" /></strong>
                                                                        </h5>
                                                                        <h5>
                                                                            <small>No: </small>
                                                                            <strong>
                                                                                <asp:Literal Text="text" ID="ltrBarcodeNumber" runat="server" EnableViewState="false" /></strong>
                                                                        </h5>
                                                                        <h5>
                                                                            <small>Supply: </small>
                                                                            <strong>
                                                                                <asp:Literal Text="text" ID="ltrSupply" runat="server" EnableViewState="false" /></strong>
                                                                        </h5>
                                                                        <h5>
                                                                            <small>BWR: </small>
                                                                            <strong>
                                                                                <asp:Literal Text="text" ID="ltrBW" runat="server" EnableViewState="false" /></strong>
                                                                        </h5>
                                                                    </div>
                                                                    <div style="display: table-cell; border: 1px solid #000; padding: 3px">
                                                                        <h5>
                                                                            <small>Unit size: </small>
                                                                            <strong>V:
                                                                                <asp:Literal Text="text" ID="ltrUnitSizeV" runat="server" EnableViewState="false" />
                                                                                x H:
                                                                                <asp:Literal Text="text" ID="ltrUnitSizeH" runat="server" EnableViewState="false" /></strong>
                                                                        </h5>
                                                                        <h5>
                                                                            <small>Printing Direction: </small>
                                                                            <asp:Literal ID="ltrDirection" runat="server" EnableViewState="false" />
                                                                        </h5>
                                                                        <h5>
                                                                            <small>Traps size: </small>
                                                                            <strong>
                                                                                <asp:Literal Text="text" ID="ltrTrapSize" runat="server" EnableViewState="false" /></strong>
                                                                        </h5>
                                                                        <h5>
                                                                            <small>Opaque Ink: </small>
                                                                            <strong>
                                                                                <asp:Literal Text="text" ID="ltrOpaqueInkRate" runat="server" EnableViewState="false" /></strong>
                                                                        </h5>
                                                                        <h5>
                                                                            <small>Color Target: </small>
                                                                            <strong>
                                                                                <asp:Literal Text="text" ID="ltrColorTarget" runat="server" EnableViewState="false" /></strong>
                                                                        </h5>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div style="display: table-cell; border: 1px solid #000; padding: 3px">

                                                <h5>
                                                    <small>Proofing Material: </small>
                                                    <strong>
                                                        <asp:Literal Text="text" ID="ltrProofingMaterial" runat="server" EnableViewState="false" /></strong>
                                                </h5>
                                                <h5>
                                                    <small>Actual Printing Material: </small>
                                                    <strong>
                                                        <asp:Literal Text="" ID="ltrActualPrintingMaterial" runat="server" EnableViewState="false" /></strong>
                                                </h5>
                                                <h5>
                                                    <small>Material Width: </small>
                                                    <strong>
                                                        <asp:Literal Text="text" ID="ltrMaterialWidth" runat="server" EnableViewState="false" /></strong>
                                                </h5>
                                            </div>
                                            <div style="display: table-cell; border: 1px solid #000; padding: 3px">
                                                <h5>
                                                    <small>Nr of repeats: </small>
                                                    <strong>H:<asp:Literal Text="text" ID="ltrNumberS_H" runat="server" EnableViewState="false" />
                                                        x V:<asp:Literal Text="text" ID="ltrNumberS_V" runat="server" EnableViewState="false" /></strong>
                                                </h5>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div style="display: table; width: 101%; left: -3px; right: -3px; border-spacing: 3px 0; position: relative; margin-bottom: 3px">
                            <div style="display: table-row">
                                <div style="display: table-cell;">
                                    <h5><strong>Cylinder Parameters</strong></h5>
                                </div>
                            </div>
                            <div style="display: table-row;">
                                <div style="display: table-cell; border: 1px solid #000">
                                    <div style="display: table; width: 100%; border-spacing: 0px">
                                        <div style="display: table-row;">
                                            <div style="display: table-cell;">
                                                <div style="display: table; width: 100%; border-spacing: 3px">
                                                    <div style="display: table-row;">
                                                        <div style="display: table-cell;">
                                                            <h5>
                                                                <small>Facewidth:</small>
                                                                <strong>
                                                                    <asp:Literal Text="text" ID="ltrFacewidt" runat="server" EnableViewState="false" />
                                                                    mm</strong>
                                                            </h5>
                                                        </div>
                                                        <div style="display: table-cell;">
                                                            <h5>
                                                                <small>Circumference:</small>
                                                                <strong>
                                                                    <asp:Literal Text="text" ID="ltrCircumference" runat="server" EnableViewState="false" />
                                                                    mm</strong>
                                                            </h5>
                                                        </div>
                                                        <div style="display: table-cell;">
                                                            <h5>
                                                                <small>Type of cylinder:</small>
                                                                <strong>
                                                                    <asp:Literal Text="text" ID="ltrCylinderType" runat="server" EnableViewState="false" /></strong>
                                                            </h5>
                                                        </div>
                                                        <div style="display: table-cell;">
                                                            <h5>
                                                                <small>Printing:</small>
                                                                <strong>
                                                                    <asp:Literal Text="text" ID="ltrPrinting" runat="server" EnableViewState="false" /></strong>
                                                            </h5>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div style="display: table-row;">
                                            <div style="display: table-cell; border-bottom-color: #fff; border-bottom-color: transparent;">
                                                <asp:GridView runat="server" AllowSorting="false" ID="gvCylinder" AutoGenerateColumns="false" CssClass="cylinderTable table table-bordered" Style="margin-bottom: 0">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Seq" HeaderStyle-CssClass="no-border-left">
                                                            <ItemStyle CssClass="no-border-left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label1" Text='<%#((SweetSoft.APEM.DataAccess.TblCylinderCollectionModel)Container.DataItem).objCylinder.Sequence%>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Base">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label2" Text='<%#((SweetSoft.APEM.DataAccess.TblCylinderCollectionModel)Container.DataItem).Base%>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cyl Barcode">
                                                            <ItemTemplate>
                                                                <asp:Label Text='<%#((SweetSoft.APEM.DataAccess.TblCylinderCollectionModel)Container.DataItem).objCylinder.CylinderBarcode%>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <%--<asp:TemplateField HeaderText="Cyl Nr">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label3" Text='<%#((SweetSoft.APEM.DataAccess.TblCylinderCollectionModel)Container.DataItem).objCylinder.CylinderNo%>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                                        <asp:TemplateField HeaderText="Cus Cyl No">
                                                            <ItemTemplate>
                                                                <asp:Label Text='<%#((SweetSoft.APEM.DataAccess.TblCylinderCollectionModel)Container.DataItem).objCylinder.CusCylinderID%>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Colour/Separation">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label4" Text='<%#((SweetSoft.APEM.DataAccess.TblCylinderCollectionModel)Container.DataItem).objCylinder.Color%>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                       <%-- <asp:TemplateField HeaderText="Pricing">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label5" Text='<%#((SweetSoft.APEM.DataAccess.TblCylinderCollectionModel)Container.DataItem).PricingName%>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                                        <asp:TemplateField HeaderText="Status">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label6" Text='<%#((SweetSoft.APEM.DataAccess.TblCylinderCollectionModel)Container.DataItem).Status%>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Process Type">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label6" Text='<%#((SweetSoft.APEM.DataAccess.TblCylinderCollectionModel)Container.DataItem).ProcessType%>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Diameter" HeaderStyle-CssClass="no-border-right">
                                                            <ItemStyle CssClass="no-border-right" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label7" Text='<%# ConvertToDecimal(((SweetSoft.APEM.DataAccess.TblCylinderCollectionModel)Container.DataItem).objCylinder.Dirameter, "N2")%>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div style="display: table; width: 101%; left: -3px; right: -3px; border-spacing: 3px 0; position: relative; margin-bottom: 3px">
                            <div style="display: table-row">
                                <div style="display: table-cell">
                                    <h5><strong>Remark</strong></h5>
                                </div>
                            </div>
                            <div style="display: table-row;">
                                <div style="display: table-cell; border: 1px solid #000; padding: 3px">
                                    <span style="min-height: 10mm; display: block">
                                        <asp:Literal Text="text" ID="ltrRemark" EnableViewState="false" runat="server" />
                                    </span>
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
