<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintMechanicalEngraving.aspx.cs"
    Inherits="SweetSoft.APEM.WebApp.Pages.Printing.PrintMechanicalEngraving" EnableViewState="false" %>

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

        .table-bordered > thead > tr > th, .table-bordered > tbody > tr > th, .table-bordered > tfoot > tr > th, .table-bordered > thead > tr > td, .table-bordered > tbody > tr > td, .table-bordered > tfoot > tr > td
        {
            border-color: #000 !important;
        }

        small
        {
            color: #000!important;
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
                        <img src="/img/apem-logo-print.png" class="img-responsive" style="height: 12mm; vertical-align: bottom; display: inline-block" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12">
                        <div style="display: table; width: 101%; left: -3px; right: -3px; border-spacing: 3px 0; position: relative; margin-bottom: 3px">
                            <div style="display: table-row">
                                <div style="display: table-cell; border: 1px solid #000; border-right: 1px solid #000; width: 54mm; background: #4569F2; color: #fff">
                                    <h5 style="color: #fff; text-align: center"><strong style="color: #fff">
                                        <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.ENGRAVING_PROTOCOL)%></strong>
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
                        <div style="display: table; width: 101%; left: -3px; right: -3px; border-spacing: 3px 0; position: relative;">
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
                                                <small>Job Co-Ord: </small>
                                                <strong>
                                                    <asp:Literal Text="" ID="ltrJobCoop" runat="server" EnableViewState="false" /></strong>
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
                                        <strong>Dimensions</strong>
                                    </h5>
                                </div>

                            </div>

                            <div style="display: table-row">
                                <div style="display: table-cell; border: 1px solid #000; padding: 3px 0">

                                    <div style="display: table; width: 100%; border-spacing: 0;">
                                        <div style="display: table-row">
                                            <div style="display: table-cell; padding-left: 8px; padding-bottom: 6px; padding-top: 5px;">
                                                <h6 style="margin-top: 0px; margin-bottom: 0px;">
                                                    <strong>Circumference</strong>
                                                    <asp:TextBox runat="server" ID="txtCircumference" ReadOnly="true" Style="width: 100%; height: 25px; border: solid; border-width: 1px; font-weight: bold; padding-left: 5px;"></asp:TextBox>
                                                </h6>
                                            </div>

                                            <div style="display: table-cell; padding-left: 5px;">
                                                <h6 style="margin-top: 0px; margin-bottom: 0px;">
                                                    <strong>Facewidth</strong>
                                                    <asp:TextBox runat="server" ID="txtFacewidth" ReadOnly="true" Style="width: 100%; height: 25px; border: solid; border-width: 1px; font-weight: bold; padding-left: 5px;"></asp:TextBox>
                                                </h6>
                                            </div>

                                            <div style="display: table-cell; padding-left: 5px;">
                                                <h6 style="margin-top: 0px; margin-bottom: 0px;">
                                                    <strong>Engraving Start</strong>
                                                    <asp:TextBox runat="server" ID="txtEngravingStart" ReadOnly="true" Style="width: 100%; height: 25px; border: solid; border-width: 1px; font-weight: bold; padding-left: 5px;"></asp:TextBox>
                                                </h6>
                                            </div>

                                            <div style="display: table-cell; padding-left: 5px; padding-right: 8px;">
                                                <h6 style="margin-top: 0px; margin-bottom: 0px;">
                                                    <strong>Engraving Width</strong>
                                                    <asp:TextBox runat="server" ReadOnly="true" ID="txtEngravingWidth" Style="width: 100%; height: 25px; border: solid; border-width: 1px; font-weight: bold; padding-left: 5px;"></asp:TextBox>
                                                </h6>
                                            </div>

                                        </div>
                                        <div style="display: table-row">
                                            <div style="display: table-cell; padding-left: 8px; padding-bottom: 6px;">
                                                <h6 style="margin-top: 0px; margin-bottom: 0px;">
                                                    <strong>Unit Size Vertical</strong>
                                                    <asp:TextBox runat="server" ID="txtUnitSizeVertical" ReadOnly="true" Style="width: 100%; height: 25px; border: solid; border-width: 1px; font-weight: bold; padding-left: 5px;"></asp:TextBox>
                                                </h6>
                                            </div>

                                            <div style="display: table-cell; padding-left: 5px;">
                                                <h6 style="margin-top: 0px; margin-bottom: 0px;">
                                                    <strong>Unit Size Horizontal</strong>
                                                    <asp:TextBox runat="server" ID="txtUnitSizeHorizontal" ReadOnly="true" Style="width: 100%; height: 25px; border: solid; border-width: 1px; font-weight: bold; padding-left: 5px;"></asp:TextBox>
                                                </h6>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div style="display: table-cell; width: 54mm; border: 1px solid #000;">
                                    <div style="display: table; width: 100%; border-spacing: 3px;">
                                        <div style="display: table-row">
                                            <h6 style="margin-top: 0px; margin-bottom: 0px;"><strong>Printing</strong></h6>
                                            <div style="margin-top: 17px; margin-left: 5px;">
                                                <asp:Literal runat="server" ID="ltrPrint" EnableViewState="false"></asp:Literal>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>


                        <div style="display: table; width: 101%; left: -3px; right: -3px; border-spacing: 3px 0; position: relative; margin-bottom: 3px; margin-top: 25px;">
                            <div style="display: table-row">
                                <div style="display: table-cell;">
                                    <table class="table table-bordered" border="1" id="Table1" style="border-collapse: collapse; margin-bottom: 0">
                                        <tbody>
                                            <tr>
                                                <td colspan="7" style="border-top-color: #fff !important; border-left-color: #fff !important;" class="no-border-left text-left">
                                                    <h5><strong>Parameters for job</strong>
                                                        <asp:TextBox runat="server" Width="100px" Height="25px" ReadOnly="true" ID="txtNumber" Text="AM27042" Style="border-color: black; border-style: solid; border-width: 1px; text-align: center; font-weight: bold;"></asp:TextBox>
                                                        R:
                                                        <asp:TextBox runat="server" ID="txtR" Width="50px" Height="25px" ReadOnly="true" Text="0" Style="border-color: black; border-style: solid; border-width: 1px; text-align: center; font-weight: bold;"></asp:TextBox>
                                                    </h5>
                                                </td>
                                                <td colspan="4" class="no-border-right text-center">
                                                    <strong>Target Engraving</strong>
                                                </td>
                                                <td colspan="2" class="no-border-right text-center">
                                                    <strong>Actual Copper </strong>
                                                </td>
                                                <td colspan="2" class="no-border-right text-center">
                                                    <strong>Actual Chrome</strong>
                                                </td>
                                            </tr>

                                            <tr>
                                                <th class="no-border-left" scope="col">Seq</th>
                                                <th scope="col">Cyl Nr</th>
                                                <th scope="col">Colour/Separation</th>
                                                <th scope="col">Stylus</th>
                                                <th scope="col">Screen</th>
                                                <th class="no-border-right" scope="col">Angle</th>
                                                <th class="no-border-right text-center" scope="col">Gamma</th>
                                                <th class="no-border-right text-center" scope="col">SH</th>
                                                <th class="no-border-right text-center" scope="col">HL</th>
                                                <th class="no-border-right text-center" scope="col">CH</th>
                                                <th class="no-border-right text-center" scope="col">MT</th>
                                                <th class="no-border-right text-center" scope="col">SH</th>
                                                <th class="no-border-right text-center" scope="col">CH</th>
                                                <th class="no-border-right text-center" scope="col">SH</th>
                                                <th class="no-border-right text-center" scope="col">CH</th>
                                            </tr>

                                            <asp:Repeater runat="server" ID="rptCylinder">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td class="no-border-left">
                                                            <span><%#Eval("Sequence")%></span>
                                                        </td>
                                                        <td>
                                                            <span><%#Eval("CylinderNo")%></span>
                                                        </td>
                                                        <td>
                                                            <span><%#Eval("Color")%></span>
                                                        </td>
                                                        <td style="width: 65px;" class="text-center">
                                                            <span><%#Eval("Stylus")%></span>
                                                        </td>
                                                        <td>
                                                            <span><%#Eval("Screen")%></span>
                                                        </td>
                                                        <td style="width: 65px;" class="text-center">
                                                            <span><%#Eval("Angle")%></span>
                                                        </td>
                                                        <td style="width: 65px;" class="text-center">
                                                            <span><%#Eval("Gamma")%></span>
                                                        </td>
                                                        <td style="width: 65px;" class="text-center">
                                                            <span><%#Eval("Sh")%></span>
                                                        </td>
                                                        <td style="width: 65px;" class="text-center">
                                                            <span><%#Eval("Hl")%></span>
                                                        </td>
                                                        <td style="width: 65px;" class="text-center">
                                                            <span><%#Eval("Ch")%></span>
                                                        </td>
                                                        <td style="width: 65px;" class="text-center">
                                                            <span><%#Eval("Mt")%></span>
                                                        </td>
                                                        <td style="width: 65px;" class="text-center">
                                                            <span><%#Eval("CopperSh")%></span>
                                                        </td>
                                                        <td style="width: 65px;" class="text-center">
                                                            <span><%#Eval("CopperCh")%></span>
                                                        </td>
                                                        <td style="width: 65px;" class="text-center">
                                                            <span><%#Eval("ChromeSh")%></span>
                                                        </td>
                                                        <td style="width: 65px;" class="text-center">
                                                            <span><%#Eval("ChromeCh")%></span>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <div style="display: table; width: 101%; left: -3px; right: -3px; border-spacing: 3px 0; position: relative; margin-bottom: 3px">
                            <div style="display: table-row">
                                <div style="display: table-cell">
                                    <h5><strong>S & R Remark</strong></h5>
                                </div>
                            </div>
                            <div style="display: table-row;">
                                <div style="display: table-cell; border: 1px solid #000; padding: 3px">
                                    <span style="min-height: 20mm; display: block">
                                        <asp:Literal Text="" ID="ltrRemark" EnableViewState="false" runat="server" />
                                    </span>
                                </div>
                            </div>
                        </div>
                        <div style="display: table; width: 101%; left: -3px; right: -3px; border-spacing: 3px 0; position: relative; margin-bottom: 3px">
                            <div style="display: table-row;">
                                <div style="display: table-cell; border: 1px solid #000; padding: 3px">
                                    <div style="display: table; width: 100%; border-spacing: 0;">
                                        <div style="display: table-row">
                                            <div style="display: table-cell; padding-left: 8px; padding-bottom: 6px; padding-top: 5px;">
                                                <h6 style="margin-top: 0px; margin-bottom: 0px;">
                                                    <strong>Date</strong>
                                                    <asp:TextBox runat="server" ID="txtDateCheck" ReadOnly="true" Style="width: 100%; height: 25px; border: solid; border-width: 1px; font-weight: bold; padding-left: 5px;"></asp:TextBox>
                                                </h6>
                                            </div>
                                            <div style="display: table-cell; padding-left: 8px; padding-bottom: 6px; padding-top: 5px;">
                                                <h6 style="margin-top: 0px; margin-bottom: 0px;">
                                                    <strong>Repro</strong>
                                                    <asp:TextBox runat="server" ID="txtReproCheck" ReadOnly="true" Style="width: 100%; height: 25px; border: solid; border-width: 1px; font-weight: bold; padding-left: 5px;"></asp:TextBox>
                                                </h6>
                                            </div>
                                            <div style="display: table-cell; padding-left: 8px; padding-bottom: 6px; padding-top: 5px;">
                                                <h6 style="margin-top: 0px; margin-bottom: 0px;">
                                                    <strong>Job Ticket</strong>
                                                    <asp:TextBox runat="server" ID="txtJobTicketCheck" ReadOnly="true" Style="width: 100%; height: 25px; border: solid; border-width: 1px; font-weight: bold; padding-left: 5px;"></asp:TextBox>
                                                </h6>
                                            </div>
                                            <div style="display: table-cell; padding-left: 8px; padding-bottom: 6px; padding-top: 5px;">
                                                <h6 style="margin-top: 0px; margin-bottom: 0px;">
                                                    <strong>Signature Check</strong>
                                                    <asp:TextBox runat="server" ID="txtSignatureCheck" ReadOnly="true" Style="width: 100%; height: 25px; border: none; border-width: 1px; font-weight: bold; padding-left: 5px; border-bottom: solid 1px"></asp:TextBox>
                                                </h6>
                                            </div>
                                        </div>
                                    </div>
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
