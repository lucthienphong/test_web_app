<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master" AutoEventWireup="true" CodeBehind="PurchaseOrder.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.PurchaseOrder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .widthPriceGridView
        {
            width: 160px;
        }

        .width50px
        {
            width: 57px;
            ;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="button-control col-md-12 col-sm-12">
            <div class="form-inline">
                <div class="form-group" style="margin-bottom: 0; width: 100%">
                    <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click"
                        class="btn btn-transparent new">
                                <span class="flaticon-new10"></span> Save
                    </asp:LinkButton>
                    <asp:LinkButton ID="btnDelete" runat="server" Visible="false"
                        class="btn btn-transparent new" OnClick="btnDelete_Click">
                                <span class="flaticon-delete41"></span>
                                Delete</asp:LinkButton>
                    <asp:UpdatePanel runat="server" ID="upnPrinting" RenderMode="Inline" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Literal ID="ltrPrint" runat="server" EnableViewState="false"></asp:Literal>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:LinkButton ID="btnCancel" runat="server" class="btn btn-transparent new" OnClick="btnCancel_Click">
                        <span class="flaticon-back57"></span>
                        <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.RETURN)%>
                    </asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <div class="row row_content">
        <div class="col-md-12 sweet-input-mask">
            <div class="row">
                <div class="col-md-2 col-sm-2">
                    <div class="form-group">
                        <label class="control-label">
                            <strong><%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.ORDER_NUMBER)%></strong>
                        </label>
                        <p class="form-control-static">
                            <asp:Label ID="lblOrderNumber" runat="server"></asp:Label>
                        </p>
                    </div>
                </div>
                <div class="col-md-1 col-sm-1">
                    <div class="form-group">
                        <label class="control-label">
                            <strong><%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.CUSTOMER)%></strong>
                        </label>
                        <SweetSoft:ExtraInputMask ID="txtCode" RenderOnlyInput="true" Required="true"
                                        runat="server" Repeat="5" ShowMaskOnHover="true" MaxLength="5" Enabled="false"
                                        Greedy="true" RightAlign="false"></SweetSoft:ExtraInputMask>
                    </div>
                </div>
                <div class="col-md-9 col-sm-9">
                    <div class="form-group">
                        <label class="control-label">
                            &nbsp;
                        </label>
                        <SweetSoft:CustomExtraTextbox ID="txtName" RenderOnlyInput="true" Placeholder="Customer's name"
                            runat="server" AutoCompleteType="Search"></SweetSoft:CustomExtraTextbox>
                        <asp:HiddenField ID="hCustomerID" runat="server" />
                        <asp:LinkButton ID="btnLoadContacts" runat="server" OnClick="btnLoadContacts_Click" Style="display: none;"></asp:LinkButton>
                    </div>
                </div>
                <div class="col-md-12">
                    <div class="row">
                        <div class="col-sm-3 col-md-3">
                            <div class="row">
                                <asp:UpdatePanel runat="server" ID="upnlJobRev" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="col-sm-8">
                                            <div class="form-group" style="margin-bottom: 0">
                                                <label class="control-label">Job Nr</label>
                                                <asp:DropDownList runat="server" ID="ddlJobNumber"
                                                    data-style="btn btn-info"
                                                    data-width="100%" AutoPostBack="true"
                                                    data-toggle="dropdown" OnSelectedIndexChanged="ddlJobNumber_SelectedIndexChanged"
                                                    CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <div class="form-group">
                                                <label class="control-label">Rev</label>
                                                <asp:DropDownList ID="ddlRevNumber" runat="server"
                                                    data-style="btn btn-info"
                                                    data-width="100%" Required="true" AutoPostBack="true"
                                                    data-toggle="dropdown" OnSelectedIndexChanged="ddlRevNumber_SelectedIndexChanged"
                                                    CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="col-md-6 col-sm-6">
                            <div class="form-group">
                                <label class="control-label">
                                    Job Name
                                </label>
                                <asp:TextBox runat="server" CssClass="form-control" ID="txtJobName" ReadOnly="true" />
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-3">
                            <div class="form-group">
                                <label class="control-label">
                                    Design
                                </label>
                                <asp:TextBox runat="server" CssClass="form-control" ID="txtDesign" ReadOnly="true" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-3 col-sm-3">
                    <div class="form-group">
                        <label class="control-label">
                            To:
                        </label>
                        <asp:DropDownList ID="ddlSuplier" runat="server" AutoPostBack="true"
                            data-style="btn btn-info"
                            data-width="100%" Required="true"
                            data-toggle="dropdown" OnSelectedIndexChanged="ddlSuplier_SelectedIndexChanged"
                            CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-3 col-sm-3">
                    <div class="form-group">
                        <label class="control-label">
                            Currency:
                        </label>
                        <asp:DropDownList ID="ddlCurrency" runat="server" AutoPostBack="false"
                            data-style="btn btn-info"
                            data-width="100%" Required="true"
                            data-toggle="dropdown"
                            CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-3 col-sm-3">
                    <div class="form-group wrap-datepicker">
                        <label class="control-label">
                            <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.ORDER_DATE)%>
                        </label>
                        <SweetSoft:ExtraInputMask ID="txtOrderDate" RenderOnlyInput="true"
                            data-format="dd-mm-yyyy" Required="true"
                            CssClass="form-control mask-date a"
                            runat="server"></SweetSoft:ExtraInputMask>
                        <span class="fa fa-calendar in-mask-date"></span>
                    </div>
                </div>
                <div class="col-md-3 col-sm-3">
                    <div class="form-group wrap-datepicker">
                        <label class="control-label">
                            <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.BASE_DELIVERY_DATE)%>
                        </label>
                        <SweetSoft:ExtraInputMask ID="txtBaseDeliveryDate"
                            data-format="dd-mm-yyyy" Required="true"
                            CssClass="form-control mask-date b"
                            RenderOnlyInput="true" runat="server"></SweetSoft:ExtraInputMask>
                        <span class="fa fa-calendar in-mask-date"></span>
                    </div>
                </div>                
                <div class="col-md-3 col-sm-3">
                    <div class="form-group">
                        <label class="control-label">
                            Contact person name:
                        </label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtContactName" Enabled="false"/>
                    </div>
                </div>
                <div class="col-md-3 col-sm-3">
                    <div class="form-group">
                        <label class="control-label">
                            Phone number:
                        </label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtContactPhone" Enabled="false"/>
                    </div>
                </div>
                <div class="col-md-3 col-sm-3">
                    <div class="form-group">
                        <label class="control-label">
                            Fax Number:
                        </label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtContactFax" Enabled="false"/>
                    </div>
                </div>
                <div class="col-md-2 col-sm-2">
                    <div class="form-group">
                        <label class="control-label">
                            <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.TOTAL_NUMBER_OF_CYLINDER)%>
                        </label>
                        <SweetSoft:ExtraInputMask ID="txtTotalCylinder"
                            MaskType="Numeric" ReadOnly="true" Required="true"
                            RenderOnlyInput="true" runat="server"></SweetSoft:ExtraInputMask>
                    </div>
                </div>
                <div class="col-md-1 col-sm-1">
                    <div class="form-group">
                        <label class="control-label">&nbsp;</label>
                        <label style="display: block; font-weight: 300">
                            <asp:CheckBox ID="chkIsUrgent" runat="server" CssClass="uniform" />
                            <span>
                                <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.IS_URGENT)%>
                            </span>
                        </label>
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
                                    <SweetSoft:GridviewExtension ID="gvClinders"
                                        runat="server" AutoGenerateColumns="false"
                                        CssClass="table table-striped table-bordered table-checkable dataTable"
                                        OnRowEditing="gvClinders_RowEditing"
                                        OnRowCommand="gvCylinders_RowCommand"
                                        OnRowUpdating="gvClinders_RowUpdating"
                                        GridLines="None" AllowPaging="false" AllowSorting="false">

                                        <Columns>
                                            <asp:TemplateField HeaderText="Cylinder Code">
                                                <ItemTemplate>
                                                    <asp:HiddenField runat="server" ID="hdfCylinderID" Value='<%#((SweetSoft.APEM.WebApp.Pages.CylinderOderModel)Container.DataItem).objCylinder.CylinderID%>' />
                                                    <%#((SweetSoft.APEM.WebApp.Pages.CylinderOderModel)Container.DataItem).objCylinder.CylinderNo %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cylinder type">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%#((SweetSoft.APEM.WebApp.Pages.CylinderOderModel)Container.DataItem).CylinderType%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Width">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWidth" Text='<%#((SweetSoft.APEM.WebApp.Pages.CylinderOderModel)Container.DataItem).objCylinder.FaceWidth%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Circumference">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCircumference" Text='<%#((SweetSoft.APEM.WebApp.Pages.CylinderOderModel)Container.DataItem).objCylinder.Circumference%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Price (RM)" HeaderStyle-CssClass="widthPriceGridView" ItemStyle-Width="160px" HeaderStyle-Width="160px">
                                                <HeaderStyle Width="160px" />
                                                <ItemStyle Width="160px" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl4" Text='<%# ((SweetSoft.APEM.WebApp.Pages.CylinderOderModel)Container.DataItem).UnitPriceExtension.ToString("N3")%>' runat="server" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <SweetSoft:ExtraInputMask ID="txtPrice"
                                                        ShowMaskOnHover="false" Greedy="false"
                                                        MaskType="Numeric" Text='<%#((SweetSoft.APEM.WebApp.Pages.CylinderOderModel)Container.DataItem).UnitPriceExtension%>'
                                                        RenderOnlyInput="true" runat="server"></SweetSoft:ExtraInputMask>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quantity" HeaderStyle-CssClass="widthPriceGridView" ItemStyle-Width="160px" HeaderStyle-Width="160px">
                                                <ItemTemplate>

                                                    <asp:Label ID="lbl5" Text='<%#((SweetSoft.APEM.WebApp.Pages.CylinderOderModel)Container.DataItem).Quantity.ToString("N2")%>' runat="server" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <SweetSoft:ExtraInputMask ID="txtQuantity" CssClass="notNegativeValue"
                                                        MaskString="9" Repeat="6" ShowMaskOnHover="false" Greedy="false"
                                                        MaskType="Numeric" Text='<%#((SweetSoft.APEM.WebApp.Pages.CylinderOderModel)Container.DataItem).Quantity%>'
                                                        RenderOnlyInput="true" runat="server"></SweetSoft:ExtraInputMask>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl6" Text='<%# ((SweetSoft.APEM.WebApp.Pages.CylinderOderModel)Container.DataItem).Total.ToString("N3")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="60px" HeaderStyle-CssClass="width50px" HeaderStyle-Width="60px">
                                                <ItemStyle CssClass="width50px" />
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Edit" CssClass="btn btn-primary">
                                                        <span class="glyphicon glyphicon-edit"></span>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Update" CssClass="btn btn-primary">
                                                        <span class="glyphicon glyphicon-floppy-saved"></span>
                                                    </asp:LinkButton>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </SweetSoft:GridviewExtension>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="col-md-12">
                    <div class="form-group">
                        <label class="control-label">
                            <%= SweetSoft.APEM.Core.ResourceTextManager.GetApplicationText(SweetSoft.APEM.Core.ResourceText.REMARK)%>
                        </label>
                        <asp:TextBox TextMode="MultiLine" ID="txtRemark" runat="server" Rows="4" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ModalPlaceHolder" runat="server">
    <div id="dialog-printing" title="Printing" style="background: #fff">
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
            NotAllowInputNagativeValue();
        })

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
                            url: "PurchaseOrder.aspx/GetCustomerData",
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
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(DatePicker);

        function DatePicker() {
            var nowTemp = new Date();
            var now = new Date(nowTemp.getFullYear(), nowTemp.getMonth(), nowTemp.getDate(), 0, 0, 0, 0);

            var oDate = $('.a').datepicker({
                format: "dd/mm/yyyy"
            });

            var dDate = $('.b').datepicker({
                format: "dd/mm/yyyy",
            });

            oDate.on('changeDate', function (e) {
                var date = new Date(oDate.datepicker('getDate'));
                var _date = new Date(date.getFullYear(), date.getMonth(), date.getDate() + 1, 0, 0, 0, 0);

                dDate.datepicker('remove').datepicker({
                    format: "dd/mm/yyyy",
                    startDate: _date
                });
                dDate.datepicker('update', new Date(date.getFullYear(), date.getMonth(), date.getDate() + 1, 0, 0, 0, 0));
            })
        }

        addRequestHanlde(InitDialogPrintLink);
        function InitDialogPrintLink() {
            $('a#printing').each(function () {
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
            })
        }

        addRequestHanlde(NotAllowInputNagativeValue);
        function NotAllowInputNagativeValue() {
            $(".notNegativeValue").keypress(function (event) {
                if (event.which == 45 || event.which == 189) {
                    event.preventDefault();
                }
            });
        }
    </script>
</asp:Content>

