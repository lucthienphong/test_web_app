<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintAdditionalJobServices.aspx.cs"
    Inherits="SweetSoft.APEM.WebApp.Pages.Printing.PrintAdditionalJobServices" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="SweetSoft" %>
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
            margin: 20mm 6mm 20mm 6mm;
            margin-top: 20mm;
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
<body>
    <form id="form1" runat="server">
        <div class="container-fluid" id="wrapPrint">
            <div class="printContent">
                <div class="row" style="margin-bottom:20px;">
                    <div class="col-sm-2 col-xs-2">
                        <img src="/img/apem-logo-print.png" class="img-responsive" />
                    </div>
                    <div class="col-sm-10 col-xs-10">
                        <asp:Literal runat="server" ID="ltrCompany" EnableViewState="false"></asp:Literal>
                    </div>
                </div>
                <asp:Literal runat="server" ID="ltrInvoiceAndDate" EnableViewState="false"></asp:Literal>
                <div class="row">
                    <div class="col-xs-12">
                        <h3 style="font-weight: 700; margin-top: 0">Quotation
                        </h3>
                    </div>
                    <div class="col-xs-6">
                        <asp:Literal runat="server" ID="ltrCustomerInfo" EnableViewState="false"></asp:Literal>
                    </div>
                    <div class="col-xs-4 col-xs-offset-2 information-do">
                        <div class="form-group">
                            <label class="control-label" style="font-weight: 300">Number / Date:</label>
                            <p class="form-control-static">
                                <strong>
                                    <asp:Literal ID="ltrOCNumber" Text="" runat="server" EnableViewState="false" /></strong>
                                /
                                <asp:Literal ID="ltrOCDate" runat="server" EnableViewState="false" />
                            </p>
                            <label class="control-label" style="font-weight: 300">APE Order No. / Date</label>
                            <p class="form-control-static">
                                <strong>
                                    <asp:Literal ID="ltrJobNumber" Text="" runat="server" EnableViewState="false" />
                                    /
                                <asp:Literal ID="ltrJobDate" runat="server" EnableViewState="false" />
                                </strong>
                            </p>
                            <label class="control-label" style="font-weight: 300">
                                Billing Currency:
                                <asp:Literal ID="ltrCurr" runat="server" EnableViewState="false" /></label>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12">
                        <label class="control-label"><strong>Requirement:</strong></label>
                        <div class="clearfix">
                            <div class="form-group clearfix">
                                <ul class="pull-left list-unstyled">
                                    <li>Terms of delivery
                                    </li>
                                    <li>Terms of payment
                                    </li>
                                    <li>
                                        <label class="control-label"></label>
                                    </li>
                                    <li>Attn
                                    </li>
                                    <li>Your Reference
                                    </li>
                                    <li>Job Name
                                    </li>
                                    <li>Design
                                    </li>
                                </ul>
                                <ul class="list-unstyled pull-left" style="margin-left: 15px;">
                                    <li>: <asp:Literal ID="ltrDeliveryTerm" Text="" EnableViewState="false" runat="server" />
                                    </li>
                                    <li>: <asp:Literal ID="ltrPaymentTerm" Text="" EnableViewState="false" runat="server" />
                                    </li>
                                    <li>
                                        <label class="control-label"></label>
                                    </li>
                                    <li>: <asp:Literal Text="" ID="ltrContact" runat="server" EnableViewState="false" />
                                    </li>
                                    <li>: <asp:Literal Text="" ID="ltrReferences" runat="server" EnableViewState="false" />
                                    </li>
                                    <li>: <asp:Literal Text="" ID="ltrJobName" runat="server" EnableViewState="false" />
                                    </li>
                                    <li>: <asp:Literal Text="" ID="ltrDesign" EnableViewState="false" runat="server" />
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-12">
                        <%--<asp:GridView ID="grvServiceJobDetail" runat="server"
                            AutoGenerateColumns="false" CssClass="table table-bordered"
                            GridLines="None"
                            AllowPaging="false"
                            AllowSorting="false"
                            ShowFooter="true"
                            DataKeyNames="ServiceJobID"
                            OnRowDataBound="grvServiceJobDetail_RowDataBound"
                            OnDataBound="grvServiceJobDetail_DataBound"
                            HeaderStyle-CssClass="header">
                            <Columns>
                                <asp:TemplateField HeaderText="No" ItemStyle-Width="50px" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="header-item">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNo" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Work order number" HeaderStyle-CssClass="header-item">
                                    <ItemTemplate>
                                        <asp:Label ID="lblWorkOrderNumber" runat="server"
                                            Text='<%#Eval("WorkOrderNumber")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="column-one" HeaderText="ProductID" HeaderStyle-CssClass="header-item">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProductID" runat="server"
                                            Text='<%#Eval("ProductID")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description" FooterStyle-CssClass="total" HeaderStyle-CssClass="header-item">
                                    <ItemStyle Width="300px" />
                                    <HeaderStyle Width="300px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescription" runat="server"
                                            Text='<%#Eval("Description")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Tax code" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="header-item">
                                     <ItemStyle Width="70px"/>
                                    <HeaderStyle Width="70px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblTaxCode" runat="server"
                                            Text='<%#ShowTaxCode(Eval("TaxID"))%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Tax rate" FooterStyle-CssClass="text-center" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="header-item">
                                     <ItemStyle Width="70px" HorizontalAlign="Right" />
                                    <HeaderStyle Width="70px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblTaxRate" runat="server"
                                            Text='<%#ShowNumberFormat(Eval("TaxPercentage"))%>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label runat="server" ID="lblTotalText" Text='<%# SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.TOTAL)%>'></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField FooterStyle-CssClass="text-right" HeaderStyle-CssClass="header-item" ItemStyle-CssClass="total" HeaderText="Work order values(in USD)">
                                    <ItemStyle HorizontalAlign="Right"/>
                                    <ItemTemplate>
                                        <asp:Label ID="lblWorkOrderValues" runat="server"
                                            Text='<%#ShowPriceTaxed(Eval("WorkOrderValues"),Eval("TaxPercentage"))%>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label runat="server" ID="lblTotalPrice"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>--%>
                        <table class="table table-bordered">
                            <thead>
                                <tr>
                                    <th>No</th>
                                    <th>Work order number</th>
                                    <th>ProductID</th>
                                    <th>Description</th>
                                    <th>Job Category</th>
                                    <th>Work order values</th>
                                </tr>
                            </thead>
                            <asp:Repeater ID="rptServiceJob" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" Text='<%#Eval("No")%>'></asp:Label></td>
                                        <td>
                                            <asp:Label runat="server" Text='<%#Eval("WorkOrderNumber")%>'></asp:Label></td>
                                        <td>
                                            <asp:Label runat="server" Text='<%#Eval("ProductID")%>'></asp:Label></td>
                                        <td>
                                            <asp:Label runat="server" Text='<%#Eval("Description")%>'></asp:Label></td>
                                        <td>
                                            <asp:Label runat="server" Text='<%#Eval("CategoryName")%>'></asp:Label></td>
                                        <td style="text-align:right;">
                                            <asp:Label runat="server" Text='<%#Eval("WorkOrderValues")%>'></asp:Label></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            
                            <%--Other charges--%>
                            <asp:Repeater ID="rptOtherCharges" runat="server">
                                <HeaderTemplate>
                                    <tr>
                                        <td colspan="6"><strong>Other charges</strong></td>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td colspan="1"><%#((SweetSoft.APEM.WebApp.Pages.Printing.TblOtherChargeExtension)Container.DataItem).No%></td>
                                        <td colspan="4">
                                            <%#((SweetSoft.APEM.WebApp.Pages.Printing.TblOtherChargeExtension)Container.DataItem).tblOtherCharges.Description%>
                                        </td>
                                        <%--<td style="text-align: right">
                                            <%#((SweetSoft.APEM.WebApp.Pages.Printing.TblOtherChargeExtension)Container.DataItem).tblOtherCharges.Quantity%>
                                        </td>
                                        <td style="text-align: right">
                                            <%#((decimal)((SweetSoft.APEM.WebApp.Pages.Printing.TblOtherChargeExtension)Container.DataItem).tblOtherCharges.Charge).ToString("N2")%>
                                        </td>--%>
                                        <td style="text-align: right"><%#(((SweetSoft.APEM.WebApp.Pages.Printing.TblOtherChargeExtension)Container.DataItem).TotalPrice).ToString("N2")%></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            <tr>
                                <%--<td colspan="3"></td>--%>
                                <td colspan="5" style="text-align:right"><strong>Sub Total</strong></td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblSubTotal" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <%--<td colspan="3"></td>--%>
                                <td colspan="5" style="text-align:right"><strong>Discount </strong>(<asp:Literal ID=ltrDiscountRate runat="server"></asp:Literal>)</td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblDiscount" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <%--<td colspan="3"></td>--%>
                                <td colspan="5" style="text-align:right"><strong>Sub Total before GST</strong></td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblSubTotalBefore" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <%--<td colspan="3"></td>--%>
                                <td colspan="5" style="text-align:right"><strong>GST </strong>(<asp:Literal ID=ltrTaxRate runat="server"></asp:Literal>)</td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblGST" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <%--<td colspan="3"></td>--%>
                                <td colspan="5" style="text-align:right"><strong>Total</strong></td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblTotal" runat="server"></asp:Label></td>
                            </tr>
                        </table>
                    </div>
                </div>

                <div class="row">
                    <asp:Literal runat="server" ID="ltrSignature" EnableViewState="false"></asp:Literal>
                </div>
                
                <div class="row text-center" style="margin-top:30px;">
                    <span>This is a computer generated printout and is valid without signature</span>
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
            //$("tr:last").find("td").each(function (index) {
            //    switch (index) {
            //        case 0:
            //            $(this).attr("style", "border-bottom-color: transparent !important;border-right-color: transparent !important;border-left-color: transparent !important;")
            //            break;
            //        case 1:
            //            $(this).attr("style", " text-align: right;border-bottom: 4px double #000 !important;border-left-color: transparent !important;border-right-color: transparent !important;")
            //            break;
            //        case 2:
            //            $(this).attr("style", " direction: rtl !important;border-bottom: 4px double #000 !important;border-left-color: transparent !important;border-right-color: transparent !important;")
            //            break;
            //    }
            //});
        })
    </script>
</body>
</html>
