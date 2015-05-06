<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master"
    AutoEventWireup="true" CodeBehind="ServiceJobList.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.ServiceJobList" %>

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

        /*[class^="flaticon-"]:before,
        [class*=" flaticon-"]:before
        {
            margin-left: 20px !important;
        }*/

        .flaticon-padlock19:before,
        .flaticon-padlock21:before
        {
            font-size: 16px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="button-control col-md-12 col-sm-12 affix">
            <div class="row">
                <div class="col-md-6 col-sm-6">
                    <div class="form-inline">
                        <div class="form-group" style="margin-bottom: 0;">
                            <a class="btn btn-transparent new" href="ServiceJob.aspx">
                                <span class="flaticon-new10"></span>
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.NEW)%>
                            </a>

                            <asp:LinkButton ID="btnDelete" runat="server"
                                class="btn btn-transparent new" OnClick="btnDelete_Click">
                                <span class="flaticon-delete41"></span>
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.DELETE)%></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row row_content">
        <div class="col-md-12">
            <div class="form-group">
                <div class="row">
                    <div class="col-md-3 col-sm-3">
                        <div class="form-group">
                            <label class="control-label">
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CUSTOMER)%>:</label>
                            <asp:TextBox ID="txtCustomer" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3">
                        <div class="form-group">
                            <label class="control-label">
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.JOB_NAME_DESIGN)%>:</label>
                            <asp:TextBox ID="txtJobInfo" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2 col-sm-2">
                        <div class="form-group">
                            <label class="control-label">
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.JOB_NUMBER)%>:</label>
                            <asp:TextBox ID="txtJobNumber" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2 col-sm-2">
                        <div class="form-group">
                            <label class="control-label">
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.FROM_DATE)%>:</label>
                            <div class="wrap-datepicker">
                                <SweetSoft:CustomExtraTextbox ID="txtFromDate" runat="server"
                                    RenderOnlyInput="true" data-format="dd-MM-yyyy"
                                    CssClass="datepicker form-control mask-date">
                                </SweetSoft:CustomExtraTextbox>
                                <span class="fa fa-calendar in-mask-date"></span>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2 col-sm-2">
                        <div class="form-group">
                            <label class="control-label">
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.TO_DATE)%>:</label>
                            <div class="wrap-datepicker">
                                <SweetSoft:CustomExtraTextbox ID="txtToDate" runat="server"
                                    RenderOnlyInput="true" data-format="dd-MM-yyyy"
                                    CssClass="datepicker form-control mask-date">
                                </SweetSoft:CustomExtraTextbox>
                                <span class="fa fa-calendar in-mask-date"></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3 col-sm-3">
                        <div class="form-group">
                            <label class="control-label">OC Status:</label>
                            <asp:DropDownList ID="ddlOCStatus" runat="server"
                                data-style="btn btn-info"
                                data-width="100%" Required="true"
                                data-toggle="dropdown" data-live-search="true"
                                CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3">
                        <div class="form-group">
                            <label class="control-label">DO Status:</label>
                            <asp:DropDownList ID="ddlDOStatus" runat="server"
                                data-style="btn btn-info"
                                data-width="100%" Required="true"
                                data-toggle="dropdown" data-live-search="true"
                                CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3">
                        <div class="form-group">
                            <label class="control-label">Invoice Status</label>
                            <asp:DropDownList ID="ddlInvoiceStatus" runat="server"
                                data-style="btn btn-info"
                                data-width="100%" Required="true"
                                data-toggle="dropdown" data-live-search="true"
                                CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div class="col-md-12">
                                    <div class="control-label">&nbsp;</div>
                                    <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-primary pull-right"
                                        OnClick="btnSearch_Click">
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
    </div>
    <div class="row" style="padding-top: 15px;">
        <div class="col-md-12">
            <div class="dataTables_wrapper form-inline">
                <SweetSoft:GridviewExtension ID="grvJobList" runat="server" AutoGenerateColumns="false"
                    CssClass="grvJobList table table-striped table-bordered table-checkable dataTable" GridLines="None"
                    AllowPaging="true" AllowSorting="true" DataKeyNames="JobID"
                    OnRowCommand="grvJobList_RowCommand"
                    OnPageIndexChanging="grvJobList_PageIndexChanging"
                    OnSorting="grvJobList_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="CustomerCode" SortExpression="0" HeaderStyle-CssClass="sorting"
                            ItemStyle-CssClass="column-one">
                            <ItemTemplate>
                                <asp:Label ID="lbCustomerCode" runat="server"
                                    Text='<%#Eval("Code")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="JobNumber" SortExpression="1" HeaderStyle-CssClass="sorting"
                            ItemStyle-CssClass="column-one">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server"
                                    CommandArgument='<%#Eval("JobID")%>' CommandName="Detail"
                                    Text='<%#Eval("JobNumber")%>' data-id='<%#Eval("JobID")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="JobName" SortExpression="2" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <asp:Label ID="lbJobName" runat="server"
                                    Text='<%#Eval("JobName")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Design" SortExpression="3" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <asp:Label ID="lbDesign" runat="server"
                                    Text='<%#Eval("Design")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-CssClass="column-one" HeaderText="CreatedOn"
                            SortExpression="4" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <asp:Label ID="lbCreatedOn" runat="server"
                                    Text='<%#Eval("CreatedOn")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Lock" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                            <ItemTemplate>
                                <span data-toggle="tooltip" data-placement="right" title='<%#Eval("Lock") == DBNull.Value ? "" : (((bool)Eval("Lock")) == false ?  "Unlock" : "Lock")%>' class='<%#Eval("Lock") == DBNull.Value ? "" : (((bool)Eval("Lock")) == false ?  "flaticon-padlock21 blue" : "flaticon-padlock19 red")%>'></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-CssClass="column-one" HeaderStyle-CssClass="checkbox-column">
                            <HeaderTemplate>
                                <input type="checkbox" id="chkSelectAll" class="uniform" value="" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsDelete" CssClass="uniform"
                                    runat="server"></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:TemplateField ItemStyle-CssClass="column-one">
                            <ItemTemplate>
                                <div class="openPrinting" role="menu" style="right: 0; left: auto">
                                    <a href="#" class="btn btn-primary" data-href='Printing/PrintJobServiceDetail.aspx?ID=<%#Eval("JobID") %>'>
                                        <span class="fa fa-print"></span>
                                    </a>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
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
                        <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.THERE_ARE_CURRENTLY_NO_ITEMS_IN_THIS_TABLE)%>
                    </EmptyDataTemplate>
                </SweetSoft:GridviewExtension>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ModalPlaceHolder" runat="server">
    <asp:UpdatePanel runat="server" ID="upnlPrint" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="dialog-printing" title="Printing" style="background: #f8f8f8">
                <iframe src="" frameborder="0" width="100%" height="100%" style="min-height: 500px"></iframe>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptPlaceHolder" runat="server">
    <script src="/js/plugins/printThis.js"></script>
    <script>
        addRequestHanlde(SearchText);
        SearchText();
        function SearchText(s, a) {
            if ($("input[type='text'][id$='txtCustomer']").length > 0) {
                $(".ui-autocomplete, .ui-dialog, .ui-helper-hidden-accessible").remove();
                $("input[type='text'][id$='txtCustomer']").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "JobList.aspx/GetCustomerData",
                            data: "{'Keyword':'" + $("input[type='text'][id$='txtCustomer']").val() + "'}",
                            dataType: "json",
                            async: true,
                            cache: false,
                            success: function (result) {
                                response($.map($.parseJSON(result.d), function (item) {
                                    return { ID: item.CustomerID, Name: item.Name, Code: item.Code };
                                }));
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
                        $("input[type='text'][id$='txtCustomer']").val(ui.item.Name);
                        return false;
                    }
                }).data("ui-autocomplete")._renderItem = function (ul, item) {
                    return $("<li>")
                        .data("ui-autocomplete-item", item)
                        .append("<a><span style='width:30px;'>" + item.Code + '</span> --- ' + item.Name + "</a>")
                        .appendTo(ul);
                };
            }
            else {
                $("input[type='hidden'][id$='hCustomerID']").val("");
            }
        }

        addRequestHanlde(InitCheckAll);
        //addRequestHanlde(InitDetail);

        //InitDialogPrintLink();
        InitCheckAll();
        //InitDetail();

        function InitCheckAll() {
            $("#chkSelectAll").change(function () {
                var isChecked = $(this).is(':checked');
                var checkboxother = $(this).closest('table').find('tbody tr td input[type="checkbox"]:not(:disabled)');
                if (isChecked) {
                    checkboxother.prop('checked', true).trigger('change');
                }
                else
                    checkboxother.each(function () {
                        $(this).prop('checked', false).trigger('change');
                    });
            });
        }

        //function InitDetail() {
        //    var linkColl = $('div[id$="grvJobList"] a[id$="btnEdit"]');
        //    if (linkColl.length > 0) {
        //        linkColl.click(function () {
        //            parent.openWindow($('a[data-title]:eq(0)'), 'Job', '/Pages/ServiceJob.aspx?ID=' + $(this).attr('data-id'));
        //            return false;
        //        });
        //    }
        //}

        //$('button[id="btnAdd"]').click(function () {
        //    parent.openWindow($('a[data-title]:eq(0)'), 'Job', '/Pages/ServiceJob.aspx');
        //    return false;
        //});

        //addRequestHanlde(InitDialogPrintLink);
        //function InitDialogPrintLink() {
        //    $('.grvJobList .openPrinting a').each(function () {
        //        $(this).on('click', function (e) {

        //            var hrefLink = $(this).data("href");
        //            var iframe = $("#dialog-printing").find('iframe');

        //            iframe.attr("src", hrefLink);

        //            $("#dialog-printing").dialog({
        //                autoOpen: false,
        //                height: 'auto',
        //                width: '850',
        //                modal: true,
        //                appendTo: "form",
        //                resizable: false,
        //                buttons: [
        //                    {
        //                        text: "Print",
        //                        "class": 'btn btn-primary',
        //                        click: function () {
        //                            var content = $(this).find('iframe');
        //                            var clonePrint = content.contents().find("html");
        //                            var script = clonePrint.find('script').remove();
        //                            $(clonePrint).printThis();

        //                        }
        //                    }
        //                    ,
        //                    {
        //                        text: "Close",
        //                        Class: 'btn btn-default',
        //                        click: function () {
        //                            iframe.attr("src", "");
        //                            $("#dialog-printing").dialog("close");

        //                        }
        //                    }
        //                ]
        //            });
        //            $("#dialog-printing").show();
        //            $("#dialog-printing").dialog("open");
        //            return false;
        //        });
        //    })
        //}


    </script>
</asp:Content>
