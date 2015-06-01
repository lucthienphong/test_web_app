<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master" AutoEventWireup="true" CodeBehind="refCurrencyRateExchange.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.refCurrencyRateExchange" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="SweetSoft" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .column-large
        {
            width:200px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="button-control col-md-12 col-sm-12 affix">
            <div class="row">
                <div class="col-xs-12">
                    <div class="form-horizontal">
                        <asp:LinkButton ID="btnAdd" runat="server"
                            class="btn btn-transparent new"
                            OnClick="btnAdd_Click">
                                <span class="flaticon-new10"></span>
                                New</asp:LinkButton>
                        <asp:LinkButton ID="btnDelete" runat="server"
                            class="btn btn-transparent new"
                            OnClick="btnDelete_Click">
                                <span class="flaticon-delete41"></span>
                                Delete</asp:LinkButton>
                        <asp:LinkButton ID="btnCancel" Visible="false" Enabled="false" runat="server"
                            class="btn btn-transparent new pull-right" OnClick="btnCancel_Click">
                                <span class="flaticon-back57"></span>
                                Cancel</asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row row_content">
        <div class="col-md-12">
            <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="id"
                Text="" Width="100%"
                CssClass="form-control hidden" runat="server">
            </SweetSoft:CustomExtraTextbox>
            <div class="dataTables_wrapper form-inline sweet-input-mask">
                <SweetSoft:GridviewExtension ID="gvCurrency" runat="server" AutoGenerateColumns="false"
                    CssClass="gvCurrency table table-striped table-bordered table-checkable dataTable" GridLines="None"
                    AllowPaging="true" AllowSorting="true" DataKeyNames="CurrencyID"
                    OnRowCommand="gvCurrency_RowCommand"
                    OnPageIndexChanging="gvCurrency_PageIndexChanging"
                    OnSorting="gvCurrency_Sorting"
                    OnRowDeleting="gvCurrency_RowDeleting"
                    OnRowEditing="gvCurrency_RowEditing"
                    OnRowUpdating="gvCurrency_RowUpdating"
                    OnRowCreated="gvCurrency_RowCreated"
                    OnRowCancelingEdit="gvCurrency_RowCancelingEdit">
                    <Columns>
                        <asp:TemplateField HeaderText="Currency Name" SortExpression="0" HeaderStyle-CssClass="sorting">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server"
                                    Text='<%#Eval("CurrencyName")%>'
                                    CommandName="Edit"></asp:LinkButton>
                                <a href='#' data-href='CurrencyChangedLog.aspx?ID=<%#Eval("CurrencyID") %>' class="OpenPopUpCurrencyLog btn btn-primary btn-xs pull-right" data-toggle="tooltip" data-placement="left" title="View history changed">
                                    <span class="glyphicon glyphicon-time"></span>
                                </a>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div class="form-group" style="width: 100%">
                                    <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtCurrencyName"
                                        Text='<%#Eval("CurrencyName")%>' Width="100%"
                                        CssClass="form-control" runat="server">
                                    </SweetSoft:CustomExtraTextbox>
                                </div>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Currency Value" SortExpression="2"
                            HeaderStyle-CssClass="sorting" ItemStyle-CssClass="column-large">
                            <ItemTemplate>
                                <div class="col-md-12">
                                    <asp:Label ID="lbCurrencyValue" runat="server" Text='<%# ((decimal)Eval("CurrencyValue")).ToString("N2") %>'></asp:Label>
                                </div>
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="RM Value" SortExpression="1" HeaderStyle-CssClass="sorting"
                            ItemStyle-CssClass="column-large">
                            <ItemTemplate>
                                <asp:Label ID="lbRMValue" runat="server" Text='<%# ((decimal)Eval("RMValue")).ToString("N4") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div class="col-md-12">
                                    <SweetSoft:ExtraInputMask ID="txtRMValue" runat="server"
                                        RenderOnlyInput="true" Required="true"
                                        RequiredText="Require field"
                                        MaskType="Numeric" GroupSeparator="," RadixPoint="." AutoGroup="true" Digits="4"
                                        ShowMaskOnHover="false" Greedy="false"
                                        Text='<%#Eval("RMValue")%>' Width="100%">
                                    </SweetSoft:ExtraInputMask>
                                </div>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        

                        <asp:TemplateField
                            HeaderText="Obsolete" SortExpression="3" HeaderStyle-CssClass="sorting"
                            ItemStyle-CssClass="column-one">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsObsoleteView" CssClass="uniform"
                                    Checked='<%#Eval("IsObsolete")%>' Enabled="false" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkIsObsolete" CssClass="uniform"
                                    Checked='<%#Eval("IsObsolete")%>' runat="server"></asp:CheckBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-CssClass="column-one" HeaderStyle-CssClass="checkbox-column">
                            <HeaderTemplate>
                                <input type="checkbox" id="chkSelectAll" class="uniform" value="" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsDelete" CssClass="uniform"
                                    runat="server"></asp:CheckBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="UpdateButton"
                                    runat="server"
                                    CssClass="btn btn-primary"
                                    CommandName="Update"
                                    Text="Update" Font-Underline="false">
                                                        <span class="fa fa-check"></span>
                                </asp:LinkButton>
                                <asp:LinkButton ID="Cancel"
                                    runat="server"
                                    CssClass="btn btn-danger"
                                    CommandName="Cancel"
                                    Text="Cancel">
                                                        <span class="fa fa-ban"></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
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
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptPlaceHolder" runat="server">
    <script>

        $(function () {
            InitTooltip();

            InitCurrencyHistoryLink();
        })
        addRequestHanlde(InitTooltip);
        function InitTooltip() {
            $('[data-toggle="tooltip"]').bstooltip()
        }

        addRequestHanlde(InitCheckAll);
        function InitCheckAll() {
            $("#chkSelectAll").change(function () {
                var isChecked = $(this).is(':checked');
                var checkboxother = $(this).closest('table').find('tbody tr td input[type="checkbox"]:not(:disabled)');
                //checkboxother.trigger('click');
                if (isChecked) {
                    checkboxother.prop('checked', true).trigger('change');
                }
                else
                    checkboxother.each(function () {
                        $(this).prop('checked', false).trigger('change');
                    });
            });
        }
        InitCheckAll();

        addRequestHanlde(HistoryDialog);
        function HistoryDialog() {

        }

        addRequestHanlde(InitCurrencyHistoryLink);
        function InitCurrencyHistoryLink() {
            $('.gvCurrency .OpenPopUpCurrencyLog').each(function () {
                $(this).on('click', function (e) {

                    var hrefLink = $(this).data("href");
                    var iframe = $("#dialog-history-currency").find('iframe');

                    iframe.attr("src", hrefLink);

                    $("#dialog-history-currency").dialog({
                        autoOpen: false,
                        height: 'auto',
                        width: '750',
                        modal: true,
                        appendTo: "form",
                        resizable: false

                    });

                    $("#dialog-history-currency").dialog("open");
                    return false;
                });
            })
        }

    </script>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ModalPlaceHolder">
    <asp:UpdatePanel runat="server" ID="upnlHistoryCurrency" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="dialog-history-currency" title="Currency History" style="background: #f8f8f8">
                <iframe src="" frameborder="0" width="100%" height="100%" style="min-height: 500px"></iframe>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
