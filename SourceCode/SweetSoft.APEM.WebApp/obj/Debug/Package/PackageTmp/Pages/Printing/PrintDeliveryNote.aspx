<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintDeliveryNote.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.Printing.PrintDeliveryNote" EnableViewState="false" %>

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
                    <div class="col-xs-offset-9 col-xs-3">
                        <img src="/img/logo-print1.png" class="img-responsive" style="vertical-align: bottom; display: inline-block" />
                    </div>
                    <div class="col-xs-offset-1 col-xs-8" style="margin-bottom: 0px">
                        <h6>
                            <asp:Literal Text="Asia-Pacific Engravers (Malaysia) Sdn. Bhd. Lot121, Jalan Permata 2, Arab-Malaysian Industrial Park, 71800 Nilai, Negeri Sebilan D.K - Malaysia" ID="ltrCompany" EnableViewState="false" runat="server" />
                        </h6>
                    </div>
                    <div class="col-xs-3">
                        <h6><span style="width: 13mm; display: inline-block">Phone</span>:
                            <asp:Literal Text="+60 (06) 7997812." EnableViewState="false" ID="ltrCompanyPhone" runat="server" />
                            <br />
                            <span style="width: 13mm; display: inline-block">Fax</span>:
                            <asp:Literal Text="60 (06) 799 7816" EnableViewState="false" ID="ltrCompanyFax" runat="server" />
                            <br />
                            <span style="letter-spacing: 0.5mm">
                                <asp:Literal Text="www.ape.com.my" ID="ltrCompanyWebsite" EnableViewState="false" runat="server" />
                            </span>
                        </h6>
                    </div>
                </div>

                <div class="row">
                    <div class="col-xs-12">
                        <h3 style="font-weight: 700;">DELIVERY NOTE
                        </h3>
                    </div>
                </div>

                <div class="row">
                    <div class="col-xs-6">
                        <asp:Literal Text="" ID="ltrAddress" EnableViewState="false" runat="server" />
                    </div>
                    <div class="col-xs-4 col-xs-offset-2 information-do">
                        <div class="form-group">
                            <label class="control-label" style="font-weight: 300">D/N No. / Date</label>
                            <p class="form-control-static">
                                <strong>
                                    <asp:Literal ID="ltrDeliveryOrder" Text="" runat="server" EnableViewState="false" /></strong>
                                /
                                <asp:Literal ID="ltrDate" runat="server" EnableViewState="false" />
                            </p>
                            <label class="control-label" style="font-weight: 300">APE Order No./ Date</label>
                            <p class="form-control-static">
                                <asp:Literal ID="ltrOrderNo" Text="" runat="server" EnableViewState="false" />
                                /
                                <asp:Literal ID="ltrODate" runat="server" EnableViewState="false" /></label>
                            </p>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-xs-12">
                        <ul class="list-unstyled pull-left">
                            <li>
                                Job Name
                            </li>
                            <li>Design</li>
                        </ul>
                        <ul class="list-unstyled pull-left" style="margin-left:15px">
                            <li>
                                : <asp:Literal Text="" ID="ltrJobName" runat="server" EnableViewState="false" />
                            </li>
                            <li>: <asp:Literal Text="" ID="ltrDesign" EnableViewState="false" runat="server" /></li>
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
                                </asp:TemplateField>--%>
                                <%--<asp:TemplateField HeaderText="Cyl No">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("CylinderNo")%>' ID="lblCylNo"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Cus Cyl No">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("CusCylID")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cus SteelBase ID">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("CusSteelBaseID")%>'></asp:Label>
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

                </div>
                <div class="row">
                    <div class="col-xs-6">
                        <div class="col-xs-12">
                            <label class="control-label"><strong>Remark:</strong></label>
                        </div>
                        <div class="col-xs-12">
                            <asp:Literal ID="ltrRemark" runat="server" />
                        </div>
                    </div>
                    <div class="col-xs-6">
                        <span style="font-size: 10pt;">Goods received in good condition,</span><br />
                        <br />
                        <br />
                        <br />
                        <br />
                        <hr style="border-color: #000!important; width: 80%; margin-left: 0; border-style: dashed" />
                        <br />
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
