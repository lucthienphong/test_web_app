<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="SweetSoft.APEM.WebApp.Pages.CylinderTimeline"
    CodeBehind="CylinderTimeline.aspx.cs" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Cylinder Timeline</title>
    <meta name="description" content="Content Timeline HTML5/jQuery plugin">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <%--<link href="../Timeline/css/reset.css" rel="stylesheet" />--%>
    <link href="/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/css/sweetstyle.min.css" rel="stylesheet" />
    <link href="../Timeline/css/jquery.mCustomScrollbar.css" rel="stylesheet" type="text/css" />
    <link href="../Timeline/css/jquery.qtip.css" rel="stylesheet" />
    <%--<link href="../css/table.css" rel="stylesheet" />--%>
    <link href="../Timeline/css/ui.notify.css" rel="stylesheet" />
    <script type="text/javascript" src="../Timeline/js/jquery-1.11.1.min.js"></script>
    <link href="/css/bootstrap-select.min.css" rel="stylesheet" />

    <!--[if gte IE 9]>
      <style type="text/css">
        .gradient {
           filter: none;
        }
      </style>
    <![endif]-->
    <link href="../Timeline/css/newtimeline.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .btn-inprogress {
        }
        .btn-finish {
        }
        .btn-notprocess {
            background:#808080; color:#fff!important
        }
        #gvdonhang{ margin-bottom:0}
        #gvdonhang td,.responstable.table th{ width:25% !important}
         div.btn,span.btn{ cursor:default!important}
         span.btn{ width:100%; display:inline-block}
         .btn-group div.btn{ width:100px}
    </style>
</head>
<body>
    <div id='page_spinner' class='page_spinner'>
        <span></span>
    </div>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" ID="scriptMain"></asp:ScriptManager>
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
                <div class="col-md-6">
                    <div class="topdept">
                        <asp:DropDownList ID="ddlDepartment" runat="server"
                            data-style="btn btn-info btn-block"
                            data-width="100%" data-live-search="true"
                            data-toggle="dropdown" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged"
                            CssClass="form-control" AutoPostBack="true">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="topdept">
                        <span>Information:</span>
                        <div class="btn-group">
                            <div class="btn btn-warning btn-inprogress">Inprogress</div>
                            <div class="btn btn-success btn-finish">Finish</div>
                            <div class="btn btn-notprocess">Not process</div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-6">
                    <div id="maindonhang">
                        <h4 class="text-primary text-center">
                            <label class="control-label" style="text-transform: uppercase">Cylinder List</label>
                        </h4>
                        <div class="product">
                            <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
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
                                                    <input type="text" class="form-control" id="txtBarcode" placeholder='Job name' />
                                                </td>
                                                <td class="jobname">
                                                    <input type="text" id="txtJobName" class="form-control" placeholder='<%=SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CYL_NO) %>' />
                                                </td>
                                                <td class="scustomer">
                                                    <input type="text" id="txtCustomer" class="form-control" placeholder='Barcode' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <asp:Literal ID="ltrProduct" runat="server" EnableViewState="false"></asp:Literal>
                                            </tr>
                                        </tbody>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlDepartment" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
                <div class="col-xs-6">
                    <h4 class="text-primary text-center">
                        <label class="control-label" style="text-transform: uppercase">Timeline</label>
                    </h4>
                    <div id="mainview">
                        <table class="table table-bordered table-striped" cellspacing="0"
                            rules="all" border="1" id="gvdetail" style="border-collapse: collapse;">
                            <tbody>
                                <tr>
                                    <th scope="col" style="width: 15%;">Department</th>
                                    <th scope="col" style="width: 5%;">Status</th>
                                    <th scope="col" style="width: 25%;">Started on</th>
                                    <th scope="col" style="width: 25%;">Finished on</th>
                                    <th scope="col" style="width: 15%;">Machine</th>
                                    <th scope="col" style="width: 15%;">Time</th>
                                </tr>
                                <%--<tr>
                                    <td class="first">12</td>
                                    <td><span class="btn btn-danger"></span></td>
                                    <td>11/11/2222 22:22:22 AM</td>
                                    <td>11/11/2222 22:22:22 AM</td>
                                    <td>12</td>
                                    <td>12.11 (hours)</td>
                                </tr>--%>
                            </tbody>
                        </table>
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
    
    <script src="/js/plugins/bootstrap-plugins/bootstrap-select.min.js"></script>

    <script type="text/javascript" src="../Timeline/js/SweetSoftScript.js"></script>
    <script type="text/javascript" src="../Timeline/js/filter.js"></script>
    <script type="text/javascript" src="../Timeline/js/SweetSoftScript-<%=SweetSoft.CMS.Common.LanguageHelper.CurrentLanguageCode==1?"en":"vi"%>.js"></script>
    <script src="/js/core/bootstrap.min.js"></script>
    <script type="text/javascript">
        SweetSoftScript.Data.isCylinderTimeline = true;
        $("[data-toggle='dropdown']").selectpicker();
    </script>
</body>
</html>
