<%@ Page Title="All Logs" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master" AutoEventWireup="true" CodeBehind="AllLogs.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.AllLogs" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="SweetSoft" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        @page
        {
            margin: 0mm;
        }

        @media print
        {
            body
            {
                background: #f00;
            }
            /*h5
            {
                margin-top: 0mm;
                margin-bottom: 3mm;
            }*/

            [class^="flaticon-"]
            {
                padding-left: 3mm;
                padding-right: 3mm;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <div class="row">
        <div class="col-md-10 col-md-offset-1">
            <div class="form-group">
                <div class="row">
                    <div class="col-md-3 col-sm-3">
                        <div class="form-group">
                            <label class="control-label">User Name:</label>
                            <asp:TextBox ID="txtUserName" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3">
                        <div class="form-group">
                            <label class="control-label">User IP:</label>
                            <asp:TextBox ID="txtUserIP" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3">
                        <div class="form-group">
                            <label class="control-label">Action:</label>
                            <asp:TextBox ID="txtAction" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3">
                        <div class="form-group">
                            <label class="control-label">Object:</label>
                            <asp:TextBox ID="txtObjectName" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3">
                        <div class="form-group">
                            <label class="control-label text-right">From date:</label>
                            <div class="wrap-datepicker">
                                <SweetSoft:CustomExtraTextbox ID="txtFromDate" runat="server"
                                    RenderOnlyInput="true" data-format="dd-MM-yyyy"
                                    CssClass="datepicker form-control mask-date">
                                </SweetSoft:CustomExtraTextbox>
                                <span class="fa fa-calendar in-mask-date"></span>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3">
                        <div class="form-group">
                            <label class="control-label">To date:</label>
                            <div class="wrap-datepicker">
                                <SweetSoft:CustomExtraTextbox ID="txtToDate" runat="server"
                                    RenderOnlyInput="true" data-format="dd-MM-yyyy"
                                    CssClass="datepicker form-control mask-date">
                                </SweetSoft:CustomExtraTextbox>
                                <span class="fa fa-calendar in-mask-date"></span>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3">
                        <div class="form-group">
                            <label class="control-label">&nbsp;</label>
                            <div>
                                <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-primary" OnClick="btnSearch_Click">
                                            <span class="glyphicon glyphicon-search"></span>&nbsp;
                                            <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.SEARCH)%>
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row" style="padding-top: 15px;">
        <div class="col-md-12">
            <div class="dataTables_wrapper form-inline">
                <SweetSoft:GridviewExtension ID="grvLogs" runat="server" AutoGenerateColumns="false"
                    CssClass="grvLogs table table-striped table-bordered table-checkable dataTable" GridLines="None"
                    OnPageIndexChanging="grvLogs_PageIndexChanging"
                    OnSorting="grvLogs_Sorting" DataKeyNames="ID"
                    AllowPaging="true" AllowSorting="true">
                    <Columns>
                        <asp:TemplateField HeaderText="CustomerCode" Visible="false"
                            ItemStyle-CssClass="column-one">
                            <ItemTemplate>
                                <asp:Label ID="lbID" runat="server"
                                    Text='<%#Eval("ID")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="No" HeaderStyle-CssClass="text-center"
                            ItemStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lbNo" runat="server"
                                    Text='<%#Eval("No")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action Date" SortExpression="1" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <asp:Label ID="lbActionDate" runat="server"
                                    Text='<%#Eval("DateAction")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="User Name" SortExpression="2" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <asp:Label ID="lbUsername" runat="server"
                                    Text='<%#Eval("UserName")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="User IP" SortExpression="3" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <asp:Label ID="lbUserIP" runat="server"
                                    Text='<%#Eval("UserIP")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action" SortExpression="4" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <strong>
                                    <asp:Label ID="lbAction" runat="server"
                                        Text='<%#Eval("Action")%>'></asp:Label>
                                </strong>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Object" SortExpression="5" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <asp:Label ID="lbObject" runat="server"
                                    Text='<%#Eval("Object")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Contents">
                            <ItemTemplate>
                                <asp:Label ID="lblContents"
                                    runat="server" Text='<%#Eval("Contents")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle BorderStyle="None" />
                    <PagerSettings
                        Mode="NumericFirstLast"
                        PageButtonCount="5"
                        FirstPageText="&laquo;"
                        LastPageText="&raquo;"
                        NextPageText="&rsaquo;"
                        PreviousPageText="&lsaquo;"
                        Position="Bottom" />
                    <PagerTemplate>
                        <SweetSoft:GridViewPager runat="server" />
                    </PagerTemplate>
                    <EmptyDataTemplate>
                        There are currently no items in this table.
                    </EmptyDataTemplate>
                </SweetSoft:GridviewExtension>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="ModalContent" ContentPlaceHolderID="ModalPlaceHolder" runat="server">
    <div id="content-change-datatable" title="Detail" style="display: none;">
        <iframe src="" frameborder="0" width="100%" height="100%" style="min-height: 500px"></iframe>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptPlaceHolder" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            $(window).keydown(function (e) {
                if (e.keyCode == 13) //Enter
                {
                    if (typeof $("[id$='btnSearch']")[0] != "undefined") {
                        eval($("[id$='btnSearch']")[0].href);
                    }
                }
            });
        })

        addRequestHanlde(SearchUserName);
        SearchUserName();
        function SearchUserName(s, a) {
            if ($("input[type='text'][id$='txtUserName']").length > 0) {
                //$(".ui-autocomplete, .ui-dialog, .ui-helper-hidden-accessible").remove();
                $("input[type='text'][id$='txtUserName']").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AllLogs.aspx/GetUserNameData",
                            data: "{'Keyword':'" + $("input[type='text'][id$='txtUserName']").val() + "'}",
                            dataType: "json",
                            async: true,
                            cache: false,
                            success: function (result) {
                                response($.map($.parseJSON(result.d), function (item) {
                                    return { Name: item.UserName };
                                }));
                            },
                            error: function (msg) {
                                alert(msg);
                            }
                        });
                    },
                    messages: {
                        noResults: '',
                        results: function () { }
                    },
                    focus: function (event, ui) {
                        return false;
                    },
                    select: function (event, ui) {
                        $("input[type='text'][id$='txtUserName']").val(ui.item.Name);
                        return false;
                    }
                }).data("ui-autocomplete")._renderItem = function (ul, item) {
                    return $("<li>")
                        .data("ui-autocomplete-item", item)
                        .append("<a><span style='width:30px;'>" + item.Name + '</span>' + "</a>")
                        .appendTo(ul);
                };
            }
            else {
            }
        }

        addRequestHanlde(SearchUserIP);
        SearchUserIP();
        function SearchUserIP(s, a) {
            if ($("input[type='text'][id$='txtUserIP']").length > 0) {
                //$(".ui-autocomplete, .ui-dialog, .ui-helper-hidden-accessible").remove();
                $("input[type='text'][id$='txtUserIP']").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AllLogs.aspx/GetUserIPData",
                            data: "{'Keyword':'" + $("input[type='text'][id$='txtUserIP']").val() + "'}",
                            dataType: "json",
                            async: true,
                            cache: false,
                            success: function (result) {
                                response($.map($.parseJSON(result.d), function (item) {
                                    return { Name: item.UserIP };
                                }));
                            },
                            error: function (msg) {
                                alert(msg);
                            }
                        });
                    },
                    messages: {
                        noResults: '',
                        results: function () { }
                    },
                    focus: function (event, ui) {
                        return false;
                    },
                    select: function (event, ui) {
                        $("input[type='text'][id$='txtUserIP']").val(ui.item.Name);
                        return false;
                    }
                }).data("ui-autocomplete")._renderItem = function (ul, item) {
                    return $("<li>")
                        .data("ui-autocomplete-item", item)
                        .append("<a><span style='width:30px;'>" + item.Name + '</span>' + "</a>")
                        .appendTo(ul);
                };
            }
            else {
            }
        }

        addRequestHanlde(SearchAction);
        SearchAction();
        function SearchAction(s, a) {
            if ($("input[type='text'][id$='txtAction']").length > 0) {
                //$(".ui-autocomplete, .ui-dialog, .ui-helper-hidden-accessible").remove();
                $("input[type='text'][id$='txtAction']").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AllLogs.aspx/GetActionData",
                            data: "{'Keyword':'" + $("input[type='text'][id$='txtAction']").val() + "'}",
                            dataType: "json",
                            async: true,
                            cache: false,
                            success: function (result) {
                                response($.map($.parseJSON(result.d), function (item) {
                                    return { Name: item.Action };
                                }));
                            },
                            error: function (msg) {
                                alert(msg);
                            }
                        });
                    },
                    messages: {
                        noResults: '',
                        results: function () { }
                    },
                    focus: function (event, ui) {
                        return false;
                    },
                    select: function (event, ui) {
                        $("input[type='text'][id$='txtAction']").val(ui.item.Name);
                        return false;
                    }
                }).data("ui-autocomplete")._renderItem = function (ul, item) {
                    return $("<li>")
                        .data("ui-autocomplete-item", item)
                        .append("<a><span style='width:30px;'>" + item.Name + '</span>' + "</a>")
                        .appendTo(ul);
                };
            }
            else {
            }
        }

        addRequestHanlde(SearchObject);
        SearchObject();
        function SearchObject(s, a) {
            if ($("input[type='text'][id$='txtObjectName']").length > 0) {
                //$(".ui-autocomplete, .ui-dialog, .ui-helper-hidden-accessible").remove();
                $("input[type='text'][id$='txtObjectName']").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AllLogs.aspx/GetObjectData",
                            data: "{'Keyword':'" + $("input[type='text'][id$='txtObjectName']").val() + "'}",
                            dataType: "json",
                            async: true,
                            cache: false,
                            success: function (result) {
                                response($.map($.parseJSON(result.d), function (item) {
                                    return { Name: item.ObjectX };
                                }));
                            },
                            error: function (msg) {
                                alert(msg);
                            }
                        });
                    },
                    messages: {
                        noResults: '',
                        results: function () { }
                    },
                    focus: function (event, ui) {
                        return false;
                    },
                    select: function (event, ui) {
                        $("input[type='text'][id$='txtObjectName']").val(ui.item.Name);
                        return false;
                    }
                }).data("ui-autocomplete")._renderItem = function (ul, item) {
                    return $("<li>")
                        .data("ui-autocomplete-item", item)
                        .append("<a><span style='width:30px;'>" + item.Name + '</span>' + "</a>")
                        .appendTo(ul);
                };
            }
            else {
            }
        }

        function Detail(obj) {
            var dataid = $(obj).attr("data-id");
            var jdataindex = $(obj).attr("jdata-index");
            var iframe = $("#content-change-datatable").find('iframe');
            var hrefLink = "tmpCompareDatatable.aspx?ID=" + dataid + "&jIndex=" + jdataindex;
            iframe.attr("src", hrefLink);

            $("#content-change-datatable").dialog({
                autoOpen: false,
                height: 'auto',
                width: '850',
                modal: true,
                appendTo: "form",
                resizable: false,
            });
            $("#content-change-datatable").show();
            $("#content-change-datatable").dialog("open");
        }
    </script>
</asp:Content>
