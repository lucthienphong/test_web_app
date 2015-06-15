<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintPurchaseOrder.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.Printing.PrintPurchaseOrder" %>

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

        .info-company
        {
            font-size: 11px;
        }
    </style>
</head>
<body style="background: #fff">
    <form id="form1" runat="server">
        <div class="container-fluid" id="wrapPrint">
            <div class="printContent">
                <div class="row" style="margin-bottom: 20px;">
                    <div class="col-sm-2 col-xs-2">
                        <img src="/img/apem-logo-print.png" class="img-responsive" />
                    </div>
                    <div class="col-sm-10 col-xs-10">
                        <strong>
                            <asp:Literal runat="server" ID="ltrCompanyName" EnableViewState="false"></asp:Literal></strong>
                        <asp:Label runat="server" ID="lblCompanyInfo" CssClass="info-company"></asp:Label>
                    </div>
                </div>

                <div class="row">
                    <div class="col-xs-6">
                        <h3 style="text-transform: capitalize"><strong><%=SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.PURCHASE_ORDER_PRINT) %></strong></h3>
                    </div>

                </div>
                <div class="row">
                    <div class="col-xs-6">
                        <div class="form-group" style="margin-top: 15px;">
                            <asp:Literal ID="ltrSupplier" EnableViewState="false" runat="server" />
                        </div>
                        <div class="form-group" style="margin-top: 15px;">
                            Please Deliver to:
                            <br />
                            <strong>
                                <asp:Literal runat="server" ID="ltrCompany" EnableViewState="false"></asp:Literal></strong>
                            <br />
                            <asp:Literal ID="ltrCompanyAddress" Text="Lot121, Jalan Permata 2, Arab-Malaysian Industrial Park, 71800 Nilai, Negeri Sebilan D.K." runat="server" />
                        </div>
                    </div>
                    <div class="col-xs-4 col-xs-offset-2 information-do">
                        <div class="form-group" style="margin-top: 15px;">
                            <label class="control-label">PO number/ Date: </label>
                            <p class="form-control-static">
                                <strong>
                                    <asp:Literal ID="ltrOrderNumber" Text="Order number " runat="server" EnableViewState="false" />
                                    /
                                    <asp:Literal ID="ltrOrderDate" EnableViewState="false" runat="server" />
                                </strong>
                            </p>
                            <label class="control-label">Contact Person/Telephone:</label>
                            <p class="form-control-static">
                                <asp:Literal ID="ltrContact" runat="server" EnableViewState="false" />
                            </p>
                            <label class="control-label">Email address:</label>
                            <p class="form-control-static">
                                <asp:Literal ID="ltrContactEmail" Text="Order number " runat="server" EnableViewState="false" />
                            </p>
                            <label class="control-label">Our Tel No/ Fax No:</label>
                            <p class="form-control-static">
                                <asp:Literal Text="" EnableViewState="false" ID="ltrCompanyFax" runat="server" />
                            </p>
                            <label class="control-label">
                                Billing Currency:
                                <asp:Literal ID="ltrCurr" runat="server" EnableViewState="false" /></label>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12">
                        <div class="form-horizontal" style="margin-top: 20px">
                            <div class="form-group">
                                <label for="" class="col-xs-3 control-label text-left">Customer</label>
                                <div class="col-xs-9">
                                    <p class="form-control-static">
                                        : 
                                    <asp:Literal ID="ltrCustomer" Text="text customer" EnableViewState="false" runat="server" />
                                    </p>
                                </div>
                                <br />
                                <label for="" class="col-xs-3 control-label text-left">Our job number</label>
                                <div class="col-xs-9">
                                    <p class="form-control-static">
                                        : 
                                    <asp:Literal ID="ltrJobNumber" Text="text job number" EnableViewState="false" runat="server" />
                                    </p>
                                </div>
                                <br />
                                <label for="" class="col-xs-3 control-label text-left">Job name</label>
                                <div class="col-xs-9">
                                    <p class="form-control-static">
                                        : 
                                    <asp:Literal ID="ltrJobName" Text="text job name" EnableViewState="false" runat="server" />
                                    </p>
                                </div>
                                <br />
                                <label for="" class="col-xs-3 control-label text-left">Design Name</label>
                                <div class="col-xs-9">
                                    <p class="form-control-static">
                                        : 
                                    <asp:Literal ID="ltrJobDesign" Text="text job design" EnableViewState="false" runat="server" />
                                    </p>
                                </div>
                                <br />
                                <label for="" class="col-xs-3 control-label text-left">Cylinder type</label>
                                <div class="col-xs-9">
                                    <p class="form-control-static">
                                        : 
                                    <asp:Literal ID="ltrCylinderType" Text="text job design" EnableViewState="false" runat="server" />
                                    </p>
                                </div>
                                <br />
                                <label for="" class="col-xs-3 control-label text-left">Base delivery date</label>
                                <div class="col-xs-9">
                                    <p class="form-control-static">
                                        : 
                                    <asp:Literal ID="ltrDeliveryDate" Text="text base delivery date" EnableViewState="false" runat="server" />
                                    </p>
                                </div>
                            </div>
                            <div class="form-group" style="display: none">
                                <label for="" class="col-xs-3 control-label text-left">ORDER MADE BY</label>
                                <div class="col-xs-9">
                                    <p class="form-control-static">
                                        : 
                                    <asp:Literal ID="ltrMadeBy" Text="text made by" EnableViewState="false" runat="server" />
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-xs-12">
                        <SweetSoft:GridviewExtension ID="gvClinders"
                            runat="server" AutoGenerateColumns="false"
                            OnRowDataBound="gvClinders_RowDataBound"
                            CssClass="table table-striped table-bordered table-checkable dataTable"
                            GridLines="None" AllowPaging="false" AllowSorting="false" ShowFooter="true"
                            ItemType="CylinderOderModel.objCylinder">
                            <Columns>
                                <asp:TemplateField HeaderText="No">
                                    <ItemTemplate>
                                        <%#((SweetSoft.APEM.DataAccess.TblCylinderCollectionModel)Container.DataItem).objCylinder.Sequence %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cyl Barcode">
                                    <ItemTemplate>
                                        <%#((SweetSoft.APEM.DataAccess.TblCylinderCollectionModel)Container.DataItem).objCylinder.CylinderBarcode %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cylinder Id">
                                    <ItemTemplate>
                                        <%#((SweetSoft.APEM.DataAccess.TblCylinderCollectionModel)Container.DataItem).objCylinder.CylinderNo %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cus Cyl ID">
                                    <ItemTemplate>
                                        <%#((SweetSoft.APEM.DataAccess.TblCylinderCollectionModel)Container.DataItem).objCylinder.CusCylinderID%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Width (mm)">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl3" Text='<%#((SweetSoft.APEM.DataAccess.TblCylinderCollectionModel)Container.DataItem).objCylinder.FaceWidth%>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Circumference (mm)">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl3" Text='<%#((SweetSoft.APEM.DataAccess.TblCylinderCollectionModel)Container.DataItem).objCylinder.Circumference%>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Qty Pcs.">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl5" Text='<%#((SweetSoft.APEM.DataAccess.TblCylinderCollectionModel)Container.DataItem).Quantity.ToString("N2")%>' runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate></FooterTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="Unit Price (RM)" HeaderStyle-CssClass="widthPriceGridView" ItemStyle-Width="160px" HeaderStyle-Width="160px">
                                    <HeaderStyle Width="160px" />
                                    <ItemStyle Width="160px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lbl4" Text='<%# decimal.Parse(((SweetSoft.APEM.DataAccess.TblCylinderCollectionModel)Container.DataItem).UnitPriceExtension).ToString("N2")%>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total (RM)">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl6" Text='<%# ((SweetSoft.APEM.DataAccess.TblCylinderCollectionModel)Container.DataItem).Total.ToString("N2")%>' runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate></FooterTemplate>
                                </asp:TemplateField>--%>
                            </Columns>
                        </SweetSoft:GridviewExtension>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12">
                        <label class="control-label">
                            Remark
                        </label>
                    </div>
                    <div class="col-xs-12">
                        <div style="border: 1px solid #000; min-height: 5mm; min-height: 50px; padding: 5px;">
                            <asp:Literal Text="text" ID="ltrRemark" runat="server" />
                        </div>
                    </div>
                    <div class="col-xs-12" style="margin-top: 10px;">
                        <strong style="vertical-align: top;">URGENT !!! </strong>
                        <asp:Literal ID="ltrUrgent" Text="" runat="server" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-5 text-center">
                        <br />
                        <br />
                        <br />
                        <br />
                        <br />
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-5">
                        <asp:Literal ID="ltrInfoUser" Text="" runat="server" />
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
