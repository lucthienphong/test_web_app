<%@ Page Language="C#" AutoEventWireup="true"
    EnableViewState="false" Inherits="SweetSoft.APEM.WebApp.Pages.TimelinePage"
    CodeBehind="Timeline.aspx.cs" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Job Timeline</title>
    <meta name="description" content="Content Timeline HTML5/jQuery plugin">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <%--<link href="../Timeline/css/reset.css" rel="stylesheet" />--%>
    <link href="/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/css/sweetstyle.min.css" rel="stylesheet" />
    <link href="../Timeline/css/jquery.mCustomScrollbar.css" rel="stylesheet" type="text/css" />
    <link href="../Timeline/css/jquery.qtip.css" rel="stylesheet" />
    <%--<link href="../Timeline/css/table.css" rel="stylesheet" />--%>
    <link href="../css/table.css" rel="stylesheet" />
    <link href="../Timeline/css/ui.notify.css" rel="stylesheet" />
    <script type="text/javascript" src="../Timeline/js/jquery-1.11.1.min.js"></script>

    <!--[if gte IE 9]>
      <style type="text/css">
        .gradient {
           filter: none;
        }
      </style>
    <![endif]-->
    <link href="../Timeline/css/newtimeline.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div id='page_spinner' class='page_spinner'>
        <span></span>
    </div>
    <form id="form1" runat="server">
        <div class="container-fluid">
            <%--<div class="row">
                <div class="button-control col-md-12 col-sm-12">
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="form-horizontal">
                                <a class="btn btn-transparent new" href="javscript:void(0);" id="btnrefresh">
                                    <span class="fa fa-refresh" style="font-size: 28px; vertical-align: middle"></span>&nbsp; Refresh
                                </a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>--%>
            <div class="row row_content">
                <div class="col-xs-12">
                    <div class="topdept">
                        <table>
                            <tr id="maindept">
                                <asp:Literal ID="ltrDept" runat="server"></asp:Literal>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-6">
                    <div id="maindonhang">
                        <h4 class="text-primary text-center">
                            <label class="control-label" style="text-transform: uppercase">Job List</label>
                        </h4>
                        <div class="product">
                            <table class="responstable table table-striped table-bordered">
                                <thead>
                                    <tr>
                                        <asp:Literal ID="ltrHeader" runat="server" EnableViewState="false"></asp:Literal>
                                    </tr>
                                </thead>
                                <tfoot>
                                    <tr>
                                        <asp:Literal ID="ltrTotal" runat="server" EnableViewState="false"></asp:Literal>
                                    </tr>
                                </tfoot>
                                <tbody>
                                    <tr id="trfilter">
                                        <td>
                                            <input type="text" class="form-control" id="txtJobNumber" placeholder='Job number' />
                                        </td>
                                        <td class="jobbarcode">
                                            <input type="text" class="form-control" id="txtBarcode" placeholder='Job barcode' />
                                        </td>
                                        <td class="jobname">
                                            <input type="text" id="txtJobName" class="form-control" placeholder='Job name' />
                                        </td>
                                        <td class="scustomer">
                                            <input type="text" id="txtCustomer" class="form-control" placeholder='<%=SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CUSTOMER) %>' />
                                        </td>
                                    </tr>
                                    <tr>
                                        <asp:Literal ID="ltrProduct" runat="server" EnableViewState="false"></asp:Literal>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="col-xs-6">
                    <h4 class="text-primary text-center">
                        <label class="control-label" style="text-transform: uppercase">Timeline</label>
                    </h4>
                    <div id="mainview">
                    </div>
                </div>
            </div>
        </div>
    </form>
    <div id="container-message" style="display: none">
        <div id="error-message">
            <a class="ui-notify-cross ui-notify-close" href="#">x</a>
            <h3>#{title}</h3>
            <p>#{text}</p>
        </div>
    </div>

    <script type="text/javascript" src="../Timeline/js/jquery.mousewheel.min.js"></script>
    <script type="text/javascript" src="../Timeline/js/jquery.mCustomScrollbar.uncompress.js"></script>
    <script type="text/javascript" src="../Timeline/js/jquery.easing.1.3.js"></script>
    <script type="text/javascript" src="../Timeline/js/jquery.qtip.min.js"></script>
    <script type="text/javascript" src="../Timeline/js/jquery.ui.core.js"></script>
    <script type="text/javascript" src="../Timeline/js/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="../Timeline/js/jquery.notify.js"></script>

    <script type="text/javascript" src="../Timeline/js/SweetSoftScript.js"></script>
    <script type="text/javascript" src="../Timeline/js/filter.js"></script>
    <script type="text/javascript" src="../Timeline/js/SweetSoftScript-<%=SweetSoft.CMS.Common.LanguageHelper.CurrentLanguageCode==1?"en":"vi"%>.js"></script>

</body>
</html>
