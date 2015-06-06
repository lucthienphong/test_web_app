<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master" AutoEventWireup="true" CodeBehind="Credit.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.Credit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="button-control col-md-12 col-sm-12">
            <div class="form-inline">
                <div class="form-group" style="margin-bottom: 0; width: 100%;">
                    <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click"
                        class="btn btn-transparent new">
                                <span class="flaticon-floppy1"></span> Save
                    </asp:LinkButton>
                    <asp:LinkButton ID="btnDelete" runat="server"
                        class="btn btn-transparent new" OnClick="btnDelete_Click">
                                <span class="flaticon-delete41"></span> Delete</asp:LinkButton>
                    <asp:UpdatePanel runat="server" RenderMode="Inline" UpdateMode="Conditional" ID="upnlPrinting">
                        <ContentTemplate>
                            <asp:Literal ID="ltrPrinting" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <a href="CreditList.aspx" class="btn btn-transparent new">
                        <span class="flaticon-back57"></span>Return
                    </a>
                </div>
            </div>
        </div>
    </div>
    <div class="row row_content">
        <div class="col-md-12 col-sm-12 sweet-input-mask">
            <div class="row">
                <div class="col-md-6 col-sm-6">
                    <div class="row">
                        <div class="col-md-3 col-sm-3">
                            <div class="form-group">
                                <label class="control-label">
                                    Credit No
                                </label>
                                <p class="form-control-static">
                                    <asp:Label ID="lblCreditNumber" runat="server"></asp:Label>
                                </p>
                            </div>
                        </div>
                        <div class="col-md-2 col-sm-2">
                            <div class="form-group">
                                <label class="control-label">
                                    <strong><%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CUSTOMER)%></strong>
                                </label>
                                <SweetSoft:ExtraInputMask ID="txtCode" RenderOnlyInput="true" Required="true"
                                    runat="server" Repeat="5" ShowMaskOnHover="true" MaxLength="5" Enabled="false"
                                    Greedy="true" RightAlign="false"></SweetSoft:ExtraInputMask>
                            </div>
                        </div>
                        <div class="col-md-7 col-sm-7">
                            <div class="form-group">
                                <label class="control-label">
                                    &nbsp;
                                </label>
                                <SweetSoft:CustomExtraTextbox ID="txtName" RenderOnlyInput="true" Placeholder="Customer's name"
                                    runat="server" AutoCompleteType="Search"></SweetSoft:CustomExtraTextbox>
                                <asp:HiddenField ID="hCustomerID" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-3 col-sm-3">
                    <div class="form-group wrap-datepicker">
                        <label class="control-label">
                            Credit date
                        </label>
                        <div class="wrap-datepicker">
                            <SweetSoft:CustomExtraTextbox ID="txtCreditDate" runat="server"
                                RenderOnlyInput="true" data-format="dd-MM-yyyy"
                                CssClass="datepicker form-control mask-date">
                            </SweetSoft:CustomExtraTextbox>
                            <span class="fa fa-calendar in-mask-date"></span>
                        </div>
                    </div>
                </div>
                <div class="col-md-3 col-sm-3">
                    <div class="form-group">
                        <label class="control-label">
                            Currency
                        </label>
                        <asp:DropDownList ID="ddlCurrency" runat="server" AutoPostBack="true"
                            data-style="btn btn-info"
                            data-width="100%"
                            data-toggle="dropdown"
                            CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-4 col-sm-4">
                    <div class="form-group">
                        <label class="control-label">
                            Tax
                        </label>
                        <asp:DropDownList ID="ddlTax" runat="server" AutoPostBack="true"
                            data-style="btn btn-info"
                            data-width="100%"
                            data-toggle="dropdown" OnSelectedIndexChanged="ddlTax_SelectedIndexChanged"
                            CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-1 col-sm-1">
                    <div class="form-group">
                        <label class="control-label">
                            Tax Rate
                        </label>
                        <div>
                            <SweetSoft:ExtraInputMask ID="txtTaxRate" Enabled="false" RenderOnlyInput="true" Required="false" Suffix=" %"
                                runat="server" MaskType="Decimal" GroupSeparator="," RadixPoint="." Text="0" Digits="3" AutoGroup="true"></SweetSoft:ExtraInputMask>
                        </div>
                    </div>
                </div>
                <div class="col-md-12 col-sm-12">
                    <div class="form-group">
                        <label class="control-label">
                            Remark
                        </label>
                        <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine"
                            Rows="4" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-12">
                    <div class="form-group">
                        <label class="control-label DTTT_button_save">
                            Credit detail
                        </label>
                        <div id="tablesContacts" class="dataTables_wrapper form-inline no-footer">
                            <div class="dt_header">
                                <div class="row">
                                    <div class="col-md-12 col-sm-12">
                                        <div class="DTTT btn-group pull-right">
                                            <asp:LinkButton ID="btnAddDetail" runat="server"
                                                CssClass="btn btn-default btn-sm DTTT_button_add"
                                                title="Add new item" OnClick="btnAddDetail_Click">
                                                        <span><i class="icol-add"></i>Add</span>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnDeleteDetail" runat="server"
                                                CssClass="btn btn-default btn-sm DTTT_button_delete"
                                                title="Delete selected rows" OnClick="btnDeleteDetail_Click">
                                                        <span><i class="icol-delete"></i>Delete</span>
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <asp:GridView ID="grvDetail" runat="server" AutoGenerateColumns="false"
                                CssClass="table table-striped table-bordered table-checkable dataTable" GridLines="None"
                                AllowPaging="false" AllowSorting="false" DataKeyNames="CreditDetailID"
                                OnRowCommand="grvDetail_RowCommand"
                                OnRowDeleting="grvDetail_RowDeleting"
                                OnRowEditing="grvDetail_RowEditing"
                                OnRowUpdating="grvDetail_RowUpdating"
                                OnRowCancelingEdit="grvDetail_RowCancelingEdit">
                                <Columns>
                                    <asp:TemplateField HeaderText="Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lbDescription" Text='<%#Eval("Description") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <div class="form-group" style="width: 100%">
                                                <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtDescription"
                                                    Text='<%#Eval("Description")%>' Width="100%"
                                                    CssClass="form-control" runat="server">
                                                </SweetSoft:CustomExtraTextbox>
                                            </div>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="JOb Order No">
                                        <ItemTemplate>
                                            <asp:Label ID="lbJobOrderNo" Text='<%#Eval("JobOrderNo") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <div class="form-group" style="width: 100%">
                                                <SweetSoft:CustomExtraTextbox RenderOnlyInput="true" ID="txtJobOrderNo"
                                                    Text='<%#Eval("JobOrderNo")%>' Width="100%"
                                                    CssClass="form-control" runat="server">
                                                </SweetSoft:CustomExtraTextbox>
                                            </div>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qty Pcs.">
                                        <ItemTemplate>
                                            <asp:Label ID="lbQuantity" Text='<%#Convert.ToDecimal(Eval("Quantity")).ToString("N0") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <div class="form-group" style="width: 100%">
                                                <SweetSoft:ExtraInputMask MaskType="Integer" RenderOnlyInput="true" ID="txtQuantity"
                                                    Text='<%#Eval("Quantity")%>' Width="100%"
                                                    CssClass="form-control" runat="server">
                                                </SweetSoft:ExtraInputMask>
                                            </div>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unit price">
                                        <ItemTemplate>
                                            <asp:Label ID="lbUnitPrice" Text='<%#Convert.ToDecimal(Eval("UnitPrice")).ToString("N3") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <div class="form-group" style="width: 100%">
                                                <SweetSoft:ExtraInputMask MaskType="Decimal" RenderOnlyInput="true" ID="txtUnitPrice"
                                                    Text='<%#Eval("UnitPrice")%>' Required="false" runat="server" GroupSeparator="," RadixPoint="."
                                                    Digits="3" AutoGroup="true" Width="100%" CssClass="form-control">
                                                </SweetSoft:ExtraInputMask>
                                            </div>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unit price">
                                        <ItemTemplate>
                                            <asp:Label ID="lbTotal" Text='<%#(Convert.ToDecimal(Eval("Quantity")) * Convert.ToDecimal(Eval("UnitPrice"))).ToString("N3") %>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-CssClass="column-one" HeaderStyle-CssClass="checkbox-column">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-primary"
                                                CommandArgument='<%# Eval("CreditDetailID") %>' CommandName="Edit">
                                                            <span class="fa fa-edit"></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <div class="btn-group">
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
                                            </div>
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
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    There are currently no items in this table.
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="dialog-printing" title="Printing" style="background: #fff">
        <iframe src="" frameborder="0" width="100%" height="100%" style="min-height: 500px"></iframe>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ModalPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptPlaceHolder" runat="server">
    <script src="/js/plugins/printThis.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#dialog-printing').hide();
            SearchText();
            PrintCredit();
        });

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SearchText);
        function SearchText(s, a) {
            if ($("input[type='text'][id$='txtName']").length > 0) {
                //$(".ui-autocomplete, .ui-dialog, .ui-helper-hidden-accessible").remove();
                $("input[type='text'][id$='txtName']").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "Credit.aspx/GetCustomerData",
                            data: "{'Keyword':'" + $("input[type='text'][id$='txtName']").val() + "'}",
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
                        $("input[type='text'][id$='txtName']").val(ui.item.Name);
                        $("input[type='text'][id$='txtCode']").val(ui.item.Code);
                        $("input[type='hidden'][id$='hCustomerID']").val(ui.item.ID);
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
        InitCheckAll();
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
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(PrintCredit);
        function PrintCredit() {
            $('a#printing').on('click', function (e) {

                var hrefLink = $(this).data("href");
                var iframe = $("#dialog-printing").find('iframe');

                iframe.attr("src", hrefLink);

                $("#dialog-printing").dialog({
                    autoOpen: false,
                    height: 'auto',
                    width: '850',
                    modal: true,
                    appendTo: "form",
                    resizable: false,
                    buttons: [
                        {
                            text: "Print",
                            "class": 'btn btn-primary',
                            click: function () {
                                var content = $(this).find('iframe');
                                var clonePrint = content.contents().find("html");
                                var script = clonePrint.find('script').remove();
                                $(clonePrint).printThis();

                            }
                        }
                        ,
                        {
                            text: "Close",
                            Class: 'btn btn-default',
                            click: function () {
                                iframe.attr("src", "");
                                $("#dialog-printing").dialog("close");

                            }
                        }
                    ]
                });
                $("#dialog-printing").show();
                $("#dialog-printing").dialog("open");
                return false;
            });
        }
    </script>
</asp:Content>
