<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintDeliveryOrder.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.Printing.PrintDeliveryOrder" EnableViewState="false" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, user-scalable=no">
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
            margin: 55mm 6mm 15mm 6mm;
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
                <div class="row" style="display: none">
                    <div class="col-xs-3">
                        <img src="/img/logo-print1.png" style="vertical-align: bottom; display: inline-block; width: 40mm" />
                    </div>
                    <div class="col-xs-9">
                        <h5 style="margin: 0">
                            <strong>
                                <asp:Literal Text="Asia Pacific Engravers (Malaysia) Sdn Bhd" ID="ltrCompany" EnableViewState="false" runat="server" />
                            </strong>
                            <br />
                        </h5>
                        <table style="width: 100%">
                            <tr>
                                <td style="vertical-align: top; width: 100mm">
                                    <asp:TextBox ID="txtAddress" ReadOnly="true" CssClass="CompanyAddress" runat="server" TextMode="MultiLine" Rows="4" Text="Asia-Pacific Engravers (Malaysia) Sdn. Bhd. Lot121, Jalan Permata 2, Arab-Malaysian Industrial Park, 71800 Nilai, Negeri Sebilan D.K - Malaysia" />
                                </td>
                                <td style="vertical-align: top">
                                    <h5 style="margin-top: 0px">
                                        <span style="width: 13mm; display: inline-block">Phone</span>:
                            <asp:Literal Text="+60 (06) 7997812." EnableViewState="false" ID="ltrCompanyPhone" runat="server" />
                                        <br />
                                        <span style="width: 13mm; display: inline-block">Fax</span>:
                            <asp:Literal Text="60 (06) 799 7816" EnableViewState="false" ID="ltrCompanyFax" runat="server" />
                                        <br />
                                        <span style="width: 13mm; display: inline-block">Website</span>:
                            <asp:Literal Text="www.ape.com.my" ID="ltrCompanyWebsite" EnableViewState="false" runat="server" />
                                    </h5>
                                </td>
                            </tr>
                        </table>

                    </div>
                    <div class="col-xs-12">
                        <hr style="border-style: solid; border-color: #000;" />
                    </div>
                </div>

                <div class="row">
                    <div class="col-xs-12">
                        <h3 style="font-weight: 700; margin-top: 0">Delivery Order
                        </h3>
                    </div>
                </div>

                <div class="row">
                    <div class="col-xs-6">
                        <asp:Literal Text="" ID="ltrAddress" EnableViewState="false" runat="server" />
                    </div>
                    <div class="col-xs-4 col-xs-offset-2 information-do">
                        <div class="form-group">
                            <label class="control-label" style="font-weight: 300">APE Delivery Note No. / Date</label>
                            <p class="form-control-static">
                                <strong>
                                    <asp:Literal ID="ltrDeliveryOrder" Text="" runat="server" EnableViewState="false" /></strong>
                                /
                                <asp:Literal ID="ltrDate" runat="server" EnableViewState="false" />
                            </p>
                            <label class="control-label" style="font-weight: 300">APE Order No. / Date</label>
                            <p class="form-control-static">
                                <asp:Literal ID="ltrOrderNo" Text="" runat="server" EnableViewState="false" />
                                /
                                <asp:Literal ID="ltrODate" runat="server" EnableViewState="false" />
                            </p>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-xs-12">
                        <ul class="pull-left list-unstyled">
                            <li>Your Reference</li>
                            <li>Job Name</li>
                            <li>Design</li>
                        </ul>
                        <ul class="pull-left list-unstyled" style="margin-left: 15px;">
                            <li>:
                                <asp:Literal Text="" ID="ltrReference" runat="server" EnableViewState="false" /></li>
                            <li>:
                                <asp:Literal Text="" ID="ltrJobName" runat="server" EnableViewState="false" /></li>
                            <li>:
                                <asp:Literal Text="" ID="ltrDesign" EnableViewState="false" runat="server" /></li>
                        </ul>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-xs-12">
                        <asp:GridView ID="gvClinders" runat="server" AutoGenerateColumns="false"
                            CssClass="table table-bordered" GridLines="None"
                            OnRowDataBound="gvClinders_RowDataBound" ShowFooter="true"
                            AllowPaging="false" AllowSorting="false" DataKeyNames="CylinderID">
                            <Columns>
                                <asp:TemplateField HeaderText="Seq">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblNo" Text='<%#Eval("Sequence")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("CylDescription")%>' ID="lblCylDescription"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="Cyl Barcode">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("CylBarcode")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cyl No">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("CylinderNo")%>' ID="lblCylNo"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Cus Cyl No">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("CusCylID")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cus Steelbase ID">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%#Eval("CusSteelBaseID")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Steel Base">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("SteelBaseName") %>' ID="lblSteelBase"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Width (mm)">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#ShowNumberFormat(Eval("FaceWidth"))%>' ID="lblFaceWidth"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Circf (mm)">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#ShowNumberFormat(Eval("Circumference"))%>' ID="lblCircumfere"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Qty">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("Quantity")%>' ID="lblQty"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                    </FooterTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="col-xs-5">
                        <div class="col-xs-12">
                            <label class="control-label"><strong>Remark:</strong></label>
                        </div>
                        <div class="col-xs-12">
                            <asp:Literal ID="ltrRemark" runat="server" />
                        </div>
                    </div>
                    <div class="col-xs-7">
                        <table class="table table-bordered">
                            <tr>
                                <td>Packing
                                </td>
                                <td colspan="2" style="border: 0">
                                    <asp:Literal ID="ltrPackingType" runat="server"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td>Packing Dimension 
                                </td>
                                <td colspan="2" style="border: 0; padding: 0">
                                    <table class="table table-bordered" style="background: none; border: 0; margin: 0; padding: 0">
                                        <asp:Repeater ID="rptPackingList" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td style="border: 0px; border-right: 1px solid #000; border-top: 1px solid #000">
                                                        <asp:Literal ID="ltrPackingName" Text='<%#Eval("Name")%>' runat="server"></asp:Literal>
                                                    </td>
                                                    <td style="border: 0; border-top: 1px solid #000">
                                                        <asp:Literal ID="ltrPackingValue" runat="server" Text='<%#Eval("Quantity")%>'></asp:Literal>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                        <tr>
                                            <td style="border: 0; border-right: 1px solid #000; border-top: 1px solid #000"><strong>Total Create</strong></td>

                                            <td style="border: 0; border-top: 1px solid #000">
                                                <asp:Literal ID="ltrTotalPacking" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                            <tr>
                                <td>Gross Weight (kg)</td>
                                <td colspan="2">
                                    <asp:Literal ID="ltrGrossWeight" runat="server"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td>Net Weight (kg)</td>
                                <td>
                                    <asp:Literal ID="ltrNetWeight" runat="server"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td>Country of Origin</td>
                                <td colspan="2">
                                    <asp:Literal ID="ltrCountryOrigin" runat="server" />
                                </td>
                            </tr>
                            <%--<tr>
                                <td>Remark
                                </td>
                                <td colspan="2">
                                    <asp:Literal Text="" ID="ltrRemark" runat="server" />
                                </td>
                            </tr>--%>
                        </table>
                    </div>
                </div>
                <div class="row">

                    <div class="col-xs-5 text-center">
                        <br />
                        <br />
                        <br />
                        <br />
                        <asp:Literal Text="Asia Pacific Engravers (Malaysia) Sdn Bhd" ID="ltr_CompanyName" EnableViewState="false" runat="server" />
                        <br />
                        <asp:Literal ID="ltrDCCreator" runat="server"></asp:Literal>
                    </div>
                    <div class="col-xs-5 col-xs-offset-2">
                        Goods received in good condition
                        <br />
                        <br />
                        <br />
                        <br />
                        <hr style="border-color: #000!important; border-style: solid; margin: 15px auto;" />
                        Company Rubber Stamp:<br />
                        Name:<br />
                        Designation:<br />
                        Date:
                    </div>
                </div>
                <div class="row" style="display: none">
                    <div class="col-xs-12">
                        <label class="control-label">
                            <strong>Additional items</strong>
                        </label>
                    </div>
                    <div class="col-xs-12" style="min-height: 50px">
                        <asp:Literal Text="" ID="ltrAdditional" runat="server" />
                    </div>

                </div>
            </div>
        </div>
    </form>
</body>
</html>
