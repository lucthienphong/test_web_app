<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintCylinderCertificate.aspx.cs"
    Inherits="SweetSoft.APEM.WebApp.Pages.Printing.PrintCylinderCertificate" EnableViewState="false" %>

<!DOCTYPE html>

<html>
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
            padding-top: 5px;
            padding-bottom: 5px;
        }

        table
        {
            margin-bottom: 2px !important;
        }

        small
        {
            color: #000!important;
        }

        th
        {
            background: #4569F2 !important;
            color: white !important;
        }

        @media all
        {
            .cylinderTable tr td:first-child, .cylinderTable tr th:first-child
            {
                border-left-width: 0;
            }

            .cylinderTable tr th:last-child,
            .cylinderTable tr td:last-child
            {
                border-right-width: 0;
            }

            .cylinderTable tr:last-child td
            {
                border-bottom-width: 0;
            }
        }

        @page
        {
            margin: 0mm;
            margin-top: 5mm;
        }

        @media print
        {
            body
            {
                margin: 0;
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

            label
            {
                margin-top: 7px;
            }

            table
            {
                margin-bottom: 5px !important;
            }

            .table-bordered > tfoot > tr > th, .table-bordered > thead > tr > td, .table-bordered > tbody > tr > td, .table-bordered > tfoot > tr > td
            {
                padding-top: 4px !important;
                padding-bottom: 4px !important;
                background: #fff !important;
            }

            .table-bordered > thead > tr > th, .table-bordered > tbody > tr > th
            {
                background: #4569F2 !important;
                color: white !important;
            }
        }
    </style>
</head>
<body style="background: #fff">
    <form id="form1" runat="server">
        <div class="container-fluid" id="wrapPrint">
            <div class="printContent">
                 <div class="row">
                    <div class="col-xs-12 text-right" style="margin-bottom: 15px">
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
                        <h3 style="font-weight: 700;">
                            <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CYLINDER_CERTIFICATE)%>
                        </h3>
                    </div>
                </div>

                <div class="row">
                    <div class="col-xs-6">
                        <p style="font-weight: bold; font-style: italic;">
                            <asp:Literal Text="" ID="ltrAddress" EnableViewState="false" runat="server" />
                        </p>
                    </div>

                </div>

                <div class="row">
                    <div class="col-xs-12">
                        <div class="form-horizontal">
                            <div class="row">
                                <span class="control-label col-xs-4 text-left">
                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CUSTOMER)%>
                                </span>
                                <span class="control-label text-left col-xs-8">:&nbsp;&nbsp;
                                    <asp:Literal Text="" ID="ltrCustomer" runat="server" EnableViewState="false" /></span>

                                <span class="control-label col-xs-4 text-left">
                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.JOB_NUMBER)%>
                                </span>
                                <span class="control-label text-left col-xs-8">:&nbsp;&nbsp;
                                    <asp:Literal Text="" ID="ltrJobNumber" runat="server" EnableViewState="false" /></span>

                                <span class="control-label col-xs-4 text-left">
                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.DESIGN)%>
                                </span>
                                <span class="control-label text-left col-xs-8">:&nbsp;&nbsp;
                                    <asp:Literal Text="" ID="ltrDesign" EnableViewState="false" runat="server" /></span>

                                <span class="control-label col-xs-4 text-left">
                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CERT_PREPARED_BY)%>
                                </span>
                                <span class="control-label text-left col-xs-8">:&nbsp;&nbsp;
                                    <asp:Literal Text="" ID="ltrCertPreparedBy" EnableViewState="false" runat="server" /></span>

                                <span class="control-label col-xs-4 text-left">
                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.DATE)%>
                                </span>
                                <span class="control-label text-left col-xs-8">:&nbsp;&nbsp;
                                    <asp:Literal Text="" ID="ltrDate" EnableViewState="false" runat="server" /></span>

                                <span class="control-label col-xs-4 text-left">
                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CYL_CIRCUMFRRENCE_MM)%>
                                </span>
                                <span class="control-label text-left col-xs-8">:&nbsp;&nbsp;
                                    <asp:Literal Text="" ID="ltrCylCircumferenceMM" EnableViewState="false" runat="server" /></span>

                                <span class="control-label col-xs-4 text-left">
                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CYL_FACEWIDTH_MM)%>
                                </span>
                                <span class="control-label text-left col-xs-8">:&nbsp;&nbsp;
                                    <asp:Literal Text="" ID="ltrCylFacewidthMM" EnableViewState="false" runat="server" /></span>
                            </div>
                        </div>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-xs-12">
                        <asp:GridView ID="grvCylinder" runat="server" AutoGenerateColumns="false"
                            CssClass="table table-striped table-bordered table-checkable dataTable" GridLines="None"
                            AllowPaging="false" AllowSorting="false" DataKeyNames="EngravingID">
                            <Columns>
                                <asp:TemplateField HeaderText="Seq" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center">
                                    <ItemTemplate>
                                        <asp:Label ID="lbSequence" runat="server"
                                            Text='<%#Eval("Sequence")%>'></asp:Label>
                                        <asp:HiddenField runat="server" ID="hdfCylinderID" Value='<%#Eval("CylinderID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Colour" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblColour" runat="server" Text='<%#Eval("Color")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="CylinderNo" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center">
                                    <ItemTemplate>
                                        <asp:Label ID="lbCylinderNo" runat="server"
                                            Text='<%#Eval("CylinderNo")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Engraving Type" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCylinderStatusName" runat="server"
                                            Text='<%#Eval("CylinderStatusName")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Stylus" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center">
                                    <ItemTemplate>
                                        <asp:Label ID="lbStylus" runat="server"
                                            Text='<%#Eval("Stylus")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Screen" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center">
                                    <ItemTemplate>
                                        <asp:Label ID="lbScreen" runat="server"
                                            Text='<%#Eval("Screen")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Angle*" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center">
                                    <ItemTemplate>
                                        <asp:Label ID="lbAngle" runat="server"
                                            Text='<%#Eval("Angle")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Wall" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center">
                                    <ItemTemplate>
                                        <asp:Label ID="lbWall" runat="server"
                                            Text='<%#Eval("Wall")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cell depth" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCellDepth" runat="server"
                                            Text='<%#ShowNumberFormat(Eval("CellDepth"))%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                There are currently no items in this table.
                            </EmptyDataTemplate>
                        </asp:GridView>
                        <span>*For electromechanical engraving, Angle is refering to "Hell Klischograph code" 0=compressed, 2=elongated, 3=coarse, 4=fine
                        </span>
                    </div>

                </div>
                <div class="row" style="margin-top: 25px;">
                    <div class="col-xs-12">
                        <div class="form-horizontal">
                            <div class="row">
                                <span class="control-label col-xs-12 text-left" style="text-decoration: underline; font-weight: bold">Barcode</span>

                                <span class="control-label col-xs-3 text-left">Digits</span>
                                <span class="control-label text-left col-xs-9">:&nbsp;&nbsp;<asp:Literal runat="server" ID="ltrDigits" EnableViewState="false"></asp:Literal></span>

                                <span class="control-label col-xs-3 text-left">Colour</span>
                                <span class="control-label text-left col-xs-9">:&nbsp;&nbsp;<asp:Literal runat="server" ID="ltrColour" EnableViewState="false"></asp:Literal></span>

                                <span class="control-label col-xs-3 text-left">Readability</span>
                                <span class="control-label text-left col-xs-9">:  </span>

                                <span class="control-label col-xs-12 text-left">Each cylinder is measured and checked to follow these standard specifications:</span>

                            </div>
                        </div>
                    </div>
                    <div class="col-xs-12">
                        <div class="form-horizontal">
                            <div class="row">
                                <br />

                                <span class="control-label col-xs-4 text-left">Surface Roughness of chrome(Rz)</span>
                                <span class="control-label text-left col-xs-8">:&nbsp;&nbsp;0.35 - 0.45</span>

                                <span class="control-label col-xs-4 text-left">Copper Hardness(Hv)</span>
                                <span class="control-label text-left col-xs-8">:&nbsp;&nbsp;19 - 230</span>

                                <span class="control-label col-xs-4 text-left">Chrome Hardness(Hv)</span>
                                <span class="control-label text-left col-xs-8">:&nbsp;&nbsp;950 - 1050</span>

                                <span class="control-label col-xs-4 text-left">Chrome Thickness(&mu;v)</span>
                                <span class="control-label text-left col-xs-8">:&nbsp;&nbsp;7 - 10</span>


                            </div>
                            <div class="row">
                                <br />
                                <p class="control-label col-xs-12 text-left">
                                    The measuring devices are tesed and calibrated on a regular basis to assure accurate results.<br />
                                    The above mentioned cylinders have been proofed on our proof press and the prints have been carefuly checked for errors, damages and any other discrepancies.
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>

</body>
</html>
