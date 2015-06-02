<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master" AutoEventWireup="true" CodeBehind="DeliveryOrder.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.DeliveryOrder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="button-control col-md-12 col-sm-12">
            <div class="form-inline">
                <div class="form-group" style="margin-bottom: 0; width: 100%;">
                    <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click" OnClientClick="SaveStateOfData('Now')"
                        class="waitforajax btn btn-transparent new">
                                <span class="flaticon-floppy1"></span> Save
                    </asp:LinkButton>
                    <asp:LinkButton ID="btnDelete" runat="server" Visible="false"
                        class="btn btn-transparent new" OnClick="btnDelete_Click">
                                <span class="flaticon-delete41"></span> Delete</asp:LinkButton>
                    <asp:UpdatePanel runat="server" RenderMode="Inline" UpdateMode="Conditional" ID="upnlPrinting">
                        <ContentTemplate>
                            <asp:Literal ID="ltrPrinting" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <%--Trunglc Add - 22-04-2015--%>
                    <asp:LinkButton ID="btnLock" runat="server" Visible="false"
                        OnClick="btnLock_Click" CssClass="btn btn-transparent">
                                <span class="flaticon-padlock19"></span>
                            Lock
                    </asp:LinkButton>
                    <asp:LinkButton ID="btnUnlock" runat="server" Visible="false"
                        OnClick="btnUnlock_Click" CssClass="btn btn-transparent">
                                <span class="flaticon-padlock21"></span>
                            Unlock
                    </asp:LinkButton>
                    <%--End--%>
                    <a href="DeliveryOrderList.aspx" class="btn btn-transparent new">
                        <span class="flaticon-back57"></span>
                        <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.RETURN)%>
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
                                    <strong><%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.DELIVERY_ORDER)%></strong>
                                </label>
                                <p class="form-control-static">
                                    <asp:Label ID="lblOrderNumber" runat="server"></asp:Label>
                                </p>
                            </div>
                        </div>
                        <div class="col-md-2 col-sm-2">
                            <div class="form-group">
                                <label class="control-label">
                                    <strong><%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CUSTOMER)%></strong>
                                </label>
                                <SweetSoft:ExtraInputMask ID="txtCode" RenderOnlyInput="true" Required="true" ToolTip="Customer Code"
                                    runat="server" Repeat="5" ShowMaskOnHover="true" MaxLength="5" Enabled="false"
                                    Greedy="true" RightAlign="false"></SweetSoft:ExtraInputMask>
                            </div>
                        </div>
                        <div class="col-md-7 col-sm-7">
                            <div class="form-group">
                                <label class="control-label">
                                    &nbsp;
                                </label>
                                <SweetSoft:CustomExtraTextbox ID="txtName" RenderOnlyInput="true" Placeholder="Customer's name" ToolTip="Customer Name"
                                    runat="server" AutoCompleteType="Search"></SweetSoft:CustomExtraTextbox>
                                <asp:HiddenField ID="hCustomerID" runat="server" />
                                <asp:LinkButton ID="btnLoadContacts" runat="server" OnClick="btnLoadContacts_Click" Style="display: none;"></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-3 col-sm-3">
                    <div class="form-group">
                        <label class="control-label">
                            Job Number
                        </label>
                        <asp:DropDownList ID="ddlJob" runat="server" AutoPostBack="true"
                            data-style="btn btn-info" ToolTip="Job Number"
                            data-width="100%" Required="true" data-size="5"
                            data-toggle="dropdown" data-live-search="true"
                            OnSelectedIndexChanged="ddlJob_SelectedIndexChanged"
                            CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-3 col-sm-3">
                    <div class="form-group">
                        <label class="control-label">
                            Contact Person
                        </label>
                        <asp:DropDownList ID="ddlContact" runat="server" AutoPostBack="false"
                            data-style="btn btn-info" ToolTip="Contact Person"
                            data-width="100%" Required="true"
                            data-toggle="dropdown"
                            CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="col-md-12">
                    <div class="row">
                        <div class="col-md-6 col-sm-6">
                            <div class="form-group">
                                <label class="control-label">
                                    Job Name
                                </label>
                                <asp:TextBox runat="server" CssClass="form-control" ID="txtJobName" ReadOnly="true" ToolTip="Job Name"/>
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-3">
                            <div class="form-group">
                                <label class="control-label">
                                    Design
                                </label>
                                <asp:TextBox runat="server" CssClass="form-control" ID="txtDesign" ReadOnly="true" ToolTip="Design"/>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3">
                            <div class="row">
                                <asp:UpdatePanel runat="server" ID="upnlJobRev" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="col-sm-8">
                                            <div class="form-group" style="margin-bottom: 0">
                                                <label class="control-label">Job Nr</label>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtJobNR" ReadOnly="true" ToolTip="Job Number"/>
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <div class="form-group">
                                                <label class="control-label">Rev</label>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtRev" ReadOnly="true" ToolTip="Job Rev"/>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="row">
                                <div class="form-group" style="overflow: hidden">
                                    <div class="col-md-6 col-sm-6">
                                        <label class="control-label">
                                            Customer P/O
                                        </label>
                                        <asp:TextBox ID="txtPO1" runat="server" CssClass="form-control" ToolTip="Customer P/O1"></asp:TextBox>
                                    </div>
                                    <div class="col-md-6 col-sm-6">
                                        <label class="control-label">&nbsp;</label>
                                        <asp:TextBox ID="txtPO2" runat="server" CssClass="form-control" ToolTip="Customer P/O2"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-3">
                            <div class="form-group wrap-datepicker">
                                <label class="control-label">
                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.DELIVERY_DATE)%>
                                </label>
                                <SweetSoft:ExtraInputMask ID="txtOrderDate" RenderOnlyInput="true"
                                    data-format="dd-mm-yyyy" ToolTip="Delivery Date"
                                    CssClass="form-control mask-date"
                                    runat="server"></SweetSoft:ExtraInputMask>
                                <span class="fa fa-calendar in-mask-date"></span>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-12">
                    <div class="form-group">
                        <label class="control-label">
                            <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CYLINDERS)%>
                        </label>
                        <asp:UpdatePanel runat="server" ID="upnlCylinder" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel runat="server" ID="pnRecord">
                                    <p class="text-muted text-center">
                                        No records found!
                                    </p>
                                </asp:Panel>
                                <asp:Panel runat="server" ID="pnListCylinder" Visible="false">
                                    <SweetSoft:GridviewExtension ID="gvClinders" ToolTip="Cylinders"
                                        runat="server" AutoGenerateColumns="false"
                                        CssClass="table table-striped table-bordered table-checkable dataTable"
                                        DataKeyNames="CylinderID"
                                        GridLines="None" AllowPaging="false" AllowSorting="false">
                                        <Columns>
                                            <asp:TemplateField HeaderText="No">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblNo" Text='<%#Eval("Sequence")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cyl Barcode">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#Eval("CylBarcode")%>' ID="lblCylBarcode"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cyl No">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#Eval("CylinderNo")%>' ID="lblCylNo"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cus Cyl ID">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#Eval("CusCylID")%>' ID="lblCusCylID"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Steel Base">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#Eval("SteelBaseName") %>' ID="lblSteelBase"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#Eval("CylDescription")%>' ID="lblCylDescription"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pricing">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#Eval("PricingName")%>' ID="lblPricing"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Circumfere">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#ShowNumberFormat(Eval("Circumference"))%>' ID="lblCircumfere"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Face width">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#ShowNumberFormat(Eval("FaceWidth"))%>' ID="lblFaceWidth"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Unit Price">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%# ShowNumberFormat(Eval("UnitPrice"))%>' ID="lblUnitPrice"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Qty">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#Eval("Quantity")%>' ID="lblQty"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total Price">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#string.Format("{0}",ShowNumberFormat(Eval("TotalPrice"))) %>' ID="lblPriceTexed"></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hdfPriceTaxed" Value='<%#Eval("TotalPrice") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </SweetSoft:GridviewExtension>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="col-md-12">
                    <div class="row">
                        <div class="col-md-9">
                            <div class="form-group">
                                <label class="control-label">
                                    <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.REMARK)%>
                                </label>
                                <asp:TextBox TextMode="MultiLine" ID="txtRemark" runat="server" Rows="3" CssClass="form-control" ToolTip="Remark"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label class="control-label DTTT_button_save">
                                    Packing Dimension
                                </label>
                                <div id="tablesContacts" class="dataTables_wrapper form-inline no-footer">
                                    <div class="dt_header">
                                        <div class="row">
                                            <div class="col-md-12 col-sm-12">
                                                <div class="DTTT btn-group pull-right">
                                                    <asp:LinkButton ID="btnAddPackingDimension" runat="server"
                                                        CssClass="btn btn-default btn-sm DTTT_button_add"
                                                        title="Add new item" OnClick="btnAddPackingDimension_Click">
                                                        <span><i class="icol-add"></i>Add</span>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnDeletePackingDimension" runat="server"
                                                        CssClass="btn btn-default btn-sm DTTT_button_delete"
                                                        title="Delete selected rows" OnClick="btnDeletePackingDimension_Click">
                                                        <span><i class="icol-delete"></i>Delete</span>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <asp:GridView ID="gvPackingDimension" runat="server" AutoGenerateColumns="false"
                                        CssClass="table table-striped table-bordered table-checkable dataTable" GridLines="None" ToolTip="Packing Dimension"
                                        AllowPaging="false" AllowSorting="false" DataKeyNames="VitualID, PackingDimensionID"
                                        OnRowCommand="gvPackingDimension_RowCommand"
                                        OnRowDeleting="gvPackingDimension_RowDeleting"
                                        OnRowEditing="gvPackingDimension_RowEditing"
                                        OnRowUpdating="gvPackingDimension_RowUpdating"
                                        OnRowCancelingEdit="gvPackingDimension_RowCancelingEdit"
                                        OnRowDataBound="gvPackingDimension_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Packing dimension">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit"
                                                        CommandArgument='<%#Eval("PackingDimensionID")%>' Text='<%#Eval("PackingDimensionName")%>'></asp:LinkButton>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DropDownList ID="ddlPackingDimension" runat="server" AutoPostBack="false"
                                                        data-style="btn btn-info"
                                                        data-width="100%" Required="true"
                                                        data-toggle="dropdown"
                                                        CssClass="form-control">
                                                    </asp:DropDownList>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quantity">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%#Eval("Quantity") %>' runat="server"></asp:Label>
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
                                        <EmptyDataTemplate>
                                            There are currently no items in this table.
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group">
                                <label class="control-label">
                                    Packing
                                </label>
                                <asp:DropDownList ID="ddlPacking" runat="server" AutoPostBack="false"
                                    data-style="btn btn-info" ToolTip="Packing"
                                    data-width="100%" Required="true"
                                    data-toggle="dropdown"
                                    CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label class="control-label">
                                    Gross Weight
                                </label>
                                <asp:TextBox ID="txtGrossWeight" runat="server" CssClass="form-control" ToolTip="Gross Weight"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label class="control-label">
                                    Net Weight
                                </label>
                                <asp:TextBox ID="txtNetWeight" runat="server" CssClass="form-control" ToolTip="Net Weight"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ModalPlaceHolder" runat="server">
    <div id="dialog-printing" title="Printing" style="background: #f8f8f8">
        <iframe src="" frameborder="0" width="100%" height="100%" style="min-height: 500px"></iframe>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptPlaceHolder" runat="server">
    <script src="/js/plugins/printThis.js"></script>
    <script>
        $(document).ready(function () {
            $('#dialog-printing').hide();
            DatePicker();
            SearchText();
            InitDialogPrintLink();
            InitCheckAll();
            SaveStateOfData('Before');
        })

        var viewstate = '<%=ViewState_PageID%>';

        function SaveStateOfData(time) {
            var obj = [
                {
                    key: 'gvClinders_' + time,
                    data: $("[id$='gvClinders']").parent().html() == undefined ? "<table></table>" : $("[id$='gvClinders']").parent().html(),
                    PageID: viewstate
                },
                {
                    key: 'gvPackingDimension_' + time,
                    data: $("[id$='gvPackingDimension']").parent().html() == undefined ? "<table></table>" : $("[id$='gvPackingDimension']").parent().html(),
                    PageID: viewstate
                }
            ];
            SaveStateOfDataForm("DeliveryOrder.aspx/SaveDataTable", obj, time);
        }

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(DatePicker);
        function DatePicker() {
            $('.mask-date').datepicker({ format: "dd/mm/yyyy" }).on('changeDate', function (e) {
                $('.mask-date').datepicker('hide', { format: "dd/mm/yyyy" })
            })
        }

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SearchText);
        function SearchText(s, a) {
            $("input[type='text'][id$='txtName']").focus(function () { $(this).select(); });
            if ($("input[type='text'][id$='txtName']").length > 0) {
                $(".ui-autocomplete,.ui-dialog, .ui-helper-hidden-accessible").remove();
                $("input[type='text'][id$='txtName']").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "DeliveryOrder.aspx/GetCustomerData",
                            data: "{'Keyword':'" + $("input[type='text'][id$='txtName']").val() + "'}",
                            dataType: "json",
                            async: false,
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
                        document.getElementById('<%= btnLoadContacts.ClientID %>').click();
                        return false;
                    }
                }).data("ui-autocomplete")._renderItem = function (ul, item) {
                    return $("<li>")
                        .data("ui-autocomplete-item", item)
                        .append("<a><span style='width:30px;'>" + item.Code + '</span> --- ' + item.Name + "</a>")
                        .appendTo(ul);
                };
            }
        }

        addRequestHanlde(InitDialogPrintLink);
        function InitDialogPrintLink() {
            $('.openPrinting a').each(function () {
                $(this).on('click', function (e) {

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
            })
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

    </script>
</asp:Content>
