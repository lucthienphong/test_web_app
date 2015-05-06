<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintJobServiceDetail.aspx.cs"
    Inherits="SweetSoft.APEM.WebApp.Pages.Printing.PrintJobServiceDetail" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="SweetSoft" %>
<!DOCTYPE html>

<html>
<head runat="server">
    <title></title>
    <link href="/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/css/uniform.default.css" rel="stylesheet" />

    <style>
        body
        {
            margin-top: 0px;
        }

        /*table,
        .table-bordered > thead > tr > th, .table-bordered > tbody > tr > th, .table-bordered > tfoot > tr > th, .table-bordered > thead > tr > td, .table-bordered > tbody > tr > td, .table-bordered > tfoot > tr > td
        {
            border-color: #000 !important;
        }*/
        .column-one
        {
            width: 120px;
        }

        .total
        {
            text-align: right !important;
        }

        .header
        {
            background: #4569F2;
            color: #fff !important;
        }

        .header-item
        {
            background: #4569F2 !important;
            text-align: center;
            color: #fff !important;
        }


        small
        {
            color: #000!important;
        }

        @media all
        {
            .form-group
            {
                margin-bottom: 0px;
            }

            .form-horizontal .control-label
            {
                padding-top: 9px;
            }

            
        }

        @page
        {
            size: auto;
            margin: 55mm 6mm 15mm 6mm;
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

            div.uniform-checker.uniform-disabled span.uniform-checked
            {
                background-position: -114px -260px;
            }


             .style-td0
            {
                border-bottom-color: transparent !important;
                border-right-color: transparent !important;
                border-left-color: transparent !important;
            }

            .style-td1
            {
                text-align: right;
                border-bottom: 3px double #000 !important;
                border-left-color: transparent !important;
                border-right-color: transparent !important;
            }

            .style-td2
            {
                direction: rtl !important;
                border-bottom: 3px double #000 !important;
                border-left-color: transparent !important;
                border-right-color: transparent !important;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid" id="wrapPrint">
            <div class="printContent">
                <div class="row" style="display:none;">
                    <div class="col-sm-2 col-xs-2">
                        <img src="/img/logo-print1.png" class="img-responsive" />
                    </div>
                    <div class="col-sm-10 col-xs-10">
                        <asp:Literal runat="server" ID="ltrCompany" EnableViewState="false"></asp:Literal>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12  text-center">
                        <div class="form-horizontal" style="direction: rtl;">
                            <div class="form-group">
                                
                                    <div class='col-xs-5 col-sm-5 text-left'> 
                                        <h3><asp:Literal runat="server" ID="ltrInvoice" EnableViewState="false"></asp:Literal></h3>
                                    </div>
                                    <div class='col-xs-7 col-sm-7 text-left'>
                                        <h3><asp:Literal runat="server" ID="ltrTitle" EnableViewState="false"></asp:Literal></h3>
                                    </div>
                                
                            </div>
                        </div>
                    </div>
                </div>
                <br /><br />
                <div class="row">
                    <div class="col-sm-6 col-xs-6">
                        <asp:Literal runat="server" ID="ltrCustomerInfo" EnableViewState="false"></asp:Literal>
                    </div>
                    <div class="col-sm-6 col-xs-6">
                        <div class="form-horizontal" style="direction: rtl;">
                            <div class="form-group">
                                <asp:Literal runat="server" ID="ltrInvoiceAndDate" EnableViewState="false"></asp:Literal>
                            </div>
                        </div>
                        <div class="form-horizontal" style="direction: rtl;">
                            <div class="form-group">
                                <asp:Literal runat="server" ID="ltrJobAndDate" EnableViewState="false"></asp:Literal>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12">
                        <p class="form-control-static">
                            <asp:Literal runat="server" ID="ltrYourReforence" EnableViewState="false"></asp:Literal>
                            Your Reference: <strong></strong>
                        </p>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12">
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
                                    <HeaderStyle Width="300px" />
                                    <ItemStyle Width="300px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescription" runat="server"
                                            Text='<%#Eval("Description")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Tax code" ItemStyle-CssClass="column-one text-center" HeaderStyle-CssClass="header-item">
                                    <HeaderStyle Width="70px" />
                                    <ItemStyle Width="70px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblTaxCode" runat="server"
                                            Text='<%#ShowTaxCode(Eval("TaxID"))%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Tax rate" FooterStyle-CssClass="text-center" ItemStyle-CssClass="column-one text-center" HeaderStyle-CssClass="header-item">
                                    <HeaderStyle Width="70px" />
                                    <ItemStyle Width="70px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblTaxRate" runat="server"
                                            Text='<%#ShowNumberFormat(Eval("TaxPercentage"))%>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label runat="server" ID="lblTotalText" Text='<%# SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.TOTAL)%>'></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField FooterStyle-CssClass="text-right" HeaderStyle-CssClass="header-item" ItemStyle-CssClass="column-one total" HeaderText="Work order values(in USD)">
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
                                            <asp:Label ID="Label1" runat="server" Text='<%#Eval("No")%>'></asp:Label></td>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text='<%#Eval("WorkOrderNumber")%>'></asp:Label></td>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" Text='<%#Eval("ProductID")%>'></asp:Label></td>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" Text='<%#Eval("Description")%>'></asp:Label></td>
                                        <td>
                                            <asp:Label ID="Label5" runat="server" Text='<%#Eval("CategoryName")%>'></asp:Label></td>
                                        <td style="text-align:right;">
                                            <asp:Label ID="Label6" runat="server" Text='<%#ShowNumberFormat(Eval("WorkOrderValues"), "N2")%>'></asp:Label></td>
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
                    <div class="col-xs-12">
                        <div class="row">
                            <div class="form-horizontal">
                                <asp:Literal runat="server" ID="ltrPaymentTerms" EnableViewState="false"></asp:Literal>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-6">
                        <asp:Literal runat="server" ID="ltrCompanyInfo1" EnableViewState="false"></asp:Literal>
                        <asp:TextBox runat="server" ID="txtBankAddress" style="padding:0;background: none;border: none;resize: none;" Rows="4" TextMode="MultiLine"></asp:TextBox>
                        <asp:Literal runat="server" ID="ltrSwiftCode" EnableViewState="false"></asp:Literal>
                    </div>

                    <asp:Literal runat="server" ID="ltrSignature" EnableViewState="false"></asp:Literal>
                    
                </div>
                <div class="row">
                    <div class="col-sm-12">
                        <h6 style="font-size:6pt !important">
                        <asp:TextBox runat="server" ID="txtCompanyAddress" TextMode="MultiLine" style="padding:0;background: none;border: none;resize: none;width: 225px;" Rows="4"></asp:TextBox>
                        <br />
                        <asp:Literal runat="server" ID="ltrCompanyInfo2" EnableViewState="false"></asp:Literal>
                        </h6>
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
            //$("tr:last").find("td").each(function (index) {
            //    switch (index) {
            //        case 0:
            //            $(this).attr("style","border-bottom-color: transparent !important;border-right-color: transparent !important;border-left-color: transparent !important;")
            //            break;
            //        case 1:
            //            $(this).attr("style"," text-align: right;border-bottom: 4px double #000 !important;border-left-color: transparent !important;border-right-color: transparent !important;")
            //            break;
            //        case 2:
            //            $(this).attr("style"," direction: rtl !important;border-bottom: 4px double #000 !important;border-left-color: transparent !important;border-right-color: transparent !important;")
            //            break;
            //    }
            //});
        })
    </script>
</body>
</html>
