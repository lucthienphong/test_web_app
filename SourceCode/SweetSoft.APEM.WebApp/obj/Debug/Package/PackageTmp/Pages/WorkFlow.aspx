<%@ Page Language="C#" AutoEventWireup="true"
     Inherits="SweetSoft.APEM.WebApp.Pages.WorkFlow" Codebehind="WorkFlow.aspx.cs" %>
<!DOCTYPE html>
<%@ Register Src="~/mxgraph/WorkFlow.ascx" TagName="WorkFlow" TagPrefix="SweetSoftCMS" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Thiết kế sản xuất</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />      
    <style type="text/css">
        body *{-webkit-box-sizing: initial!important;
-moz-box-sizing: initial!important;
box-sizing:initial!important;}
    </style>
</head>
<body>
    <div id='page_spinner' class='page_spinner'>
        <div class="modal fade in" style="display:inline; background-color:black; opacity: 0.1"></div>
        <span></span>
    </div>
    <asp:Literal ID="ltrAuthorize" runat="server"></asp:Literal>
    <%--special graphictype to show--%>
    <%--1=kinh doanh,63=Đồ hoạ làm mẫu, 64=đồ họa chép khắc--%>
    <input type="hidden" id="hdfExternalType" name="hdfExternalType" value="[{ sid: 63, eid: '' }, { sid: 64, eid:'' }]" />
    <form id="Form1" runat="server">
        <SweetSoftCMS:WorkFlow id="WorkFlow1" runat="server" EnableViewState="false"></SweetSoftCMS:WorkFlow>
    </form>
    <div class="fixed-table-container" id="divtemplate" style="display:none">
        <div class="header-background"></div>
        <div class="fixed-table-container-inner">
            <table class="selectprop" cellspacing="0">
                <thead>
                    <tr><th class="first"><div class="th-inner">Chọn</div></th>
                        <th class="second"><div class="th-inner">STT</div></th>
                        <th class="third"><div class="th-inner">Tên trường cần lưu trữ</div></th>
                        <th class="four"><div class="th-inner">Kiểu dữ liệu</div></th></tr>
                </thead>
                <tbody></tbody>
            </table>
        </div>
    </div>
</body>
</html>