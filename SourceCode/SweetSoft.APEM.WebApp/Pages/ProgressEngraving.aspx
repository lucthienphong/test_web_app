<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/ModalMasterPage.Master" AutoEventWireup="true" CodeBehind="ProgressEngraving.aspx.cs" Inherits="SweetSoft.APEM.WebApp.Pages.ProgressEngraving" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="SweetSoft" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="button-control col-md-12 col-sm-12 affix">
            <div class="row">
                <div class="col-md-6 col-sm-6">
                    <div class="form-inline">
                        <div class="form-group">
                            <asp:UpdatePanel ID="upExport" runat="server" RenderMode="Inline">
                                <ContentTemplate>
                                    <asp:LinkButton ID="btnExcel" runat="server"
                                        OnClick="btnExcel_Click" class="btn btn-transparent"> 
                                        <span class="flaticon-xlsx"></span>
                                        Excel</asp:LinkButton>
                                    <div class='btn-group'>
                                        <button type='button' class='btn btn-transparent dropdown-toggle' data-toggle='dropdown' aria-expanded='false'>
                                            <span class='flaticon-xlsx'></span>&nbsp;Production Schedule&nbsp;<span class='caret'></span>
                                        </button>
                                        <ul class='dropdown-menu openPrinting' role='menu'>
                                            <li>
                                                <asp:LinkButton ID="btnEngraving" runat="server"
                                                    OnClick="btnEngraving_Click"> 
                                                    Engraving</asp:LinkButton></li>
                                            <li>
                                                <asp:LinkButton ID="btnEmbossing" runat="server"
                                                    OnClick="btnEmbossing_Click"> 
                                                    Embossing</asp:LinkButton></li>
                                        </ul>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnExcel" />
                                    <asp:PostBackTrigger ControlID="btnEngraving" />
                                    <asp:PostBackTrigger ControlID="btnEmbossing" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row row_content">
        <div class="col-sm-10 col-sm-offset-1">
            <div class="row">
                <div class="col-md-3 col-sm-3">
                    <label class="control-label">Job Number:</label>
                    <div class="form-group">
                        <asp:TextBox ID="txtJobNumber" runat="server" class="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-3 col-sm-3">
                    <label class="control-label">Delivery Date:</label>
                    <div class="row">
                        <div class="col-xs-3">
                            <div class="form-group">
                                <p class="form-control-static">From</p>
                            </div>
                            <div class="form-group">
                                <p class="form-control-static">To</p>
                            </div>
                        </div>
                        <div class="col-xs-8">
                            <div class="form-group">
                                <div class="wrap-datepicker">
                                    <SweetSoft:CustomExtraTextbox ID="txtDeliveryB" runat="server"
                                        RenderOnlyInput="true" data-format="dd-MM-yyyy"
                                        CssClass="datepicker form-control mask-date">
                                    </SweetSoft:CustomExtraTextbox>
                                    <span class="fa fa-calendar in-mask-date"></span>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="wrap-datepicker">
                                    <SweetSoft:CustomExtraTextbox ID="txtDeliveryE" runat="server"
                                        RenderOnlyInput="true" data-format="dd-MM-yyyy"
                                        CssClass="datepicker form-control mask-date">
                                    </SweetSoft:CustomExtraTextbox>
                                    <span class="fa fa-calendar in-mask-date"></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-3 col-sm-3">
                    <label class="control-label">Engraving Date:</label>
                    <div class="row">
                        <div class="col-xs-3">
                            <div class="form-group">
                                <p class="form-control-static">From</p>
                            </div>
                            <div class="form-group">
                                <p class="form-control-static">To</p>
                            </div>
                        </div>
                        <div class="col-xs-8">
                            <div class="form-group">
                                <div class="wrap-datepicker">
                                    <SweetSoft:CustomExtraTextbox ID="txtEngravingB" runat="server"
                                        RenderOnlyInput="true" data-format="dd-MM-yyyy"
                                        CssClass="datepicker form-control mask-date">
                                    </SweetSoft:CustomExtraTextbox>
                                    <span class="fa fa-calendar in-mask-date"></span>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="wrap-datepicker">
                                    <SweetSoft:CustomExtraTextbox ID="txtEngravingE" runat="server"
                                        RenderOnlyInput="true" data-format="dd-MM-yyyy"
                                        CssClass="datepicker form-control mask-date">
                                    </SweetSoft:CustomExtraTextbox>
                                    <span class="fa fa-calendar in-mask-date"></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-3 col-sm-3">
                    <label class="control-label">RePro Status:</label>
                    <div class="form-group">
                        <asp:DropDownList ID="ddlReProStatus" runat="server"
                            data-style="btn btn-info"
                            data-width="100%" Required="true"
                            data-toggle="dropdown"
                            CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
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
    <div class="row" style="margin-top:15px;">
        <div class="col-md-12">
            <div class="table-responsive">
                <SweetSoft:GridviewExtension ID="grvProgressEngraving" runat="server" AutoGenerateColumns="false"
                    CssClass="table table-striped table-bordered table-checkable dataTable" GridLines="None"
                    AllowPaging="true" AllowSorting="true" DataKeyNames="JobID"
                    OnRowCommand="grvProgressEngraving_RowCommand"
                    OnPageIndexChanging="grvProgressEngraving_PageIndexChanging"
                    OnSorting="grvProgressEngraving_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="Code" SortExpression="0" HeaderStyle-CssClass="sorting column-80" ItemStyle-CssClass="column-80">
                            <ItemTemplate>
                                <asp:Label ID="lbCode" runat="server" Text='<%#Eval("Code")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="JobNumber" SortExpression="1" HeaderStyle-CssClass="sorting column-100" ItemStyle-CssClass="column-100">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnUpdateByNo" runat="server"
                                    CommandArgument='<%#Eval("JobID")%>' CommandName="UpdateEngraving" Text='<%#Eval("JobNumber")%>'>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="RevNumber" SortExpression="2" HeaderStyle-CssClass="sorting column-60" ItemStyle-CssClass="column-60">
                            <ItemTemplate>
                                <asp:Label ID="lbRevNumber" runat="server"
                                    Text='<%#Eval("RevNumber")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="JobName" SortExpression="3" HeaderStyle-CssClass="sorting column-250" ItemStyle-CssClass="column-250">
                            <ItemTemplate>
                                <asp:Label ID="lbJobName" runat="server"
                                    Text='<%#Eval("JobName")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Design" SortExpression="4" HeaderStyle-CssClass="sorting column-250" ItemStyle-CssClass="column-250">
                            <ItemTemplate>
                                <asp:Label ID="lbDesign" runat="server"
                                    Text='<%#Eval("Design")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Qty" SortExpression="5" HeaderStyle-CssClass="sorting column-60" ItemStyle-CssClass="column-60">
                            <ItemTemplate>
                                <asp:Label ID="lbQty" runat="server"
                                    Text='<%#Eval("Qty")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DeliveryDate" SortExpression="6" HeaderStyle-CssClass="sorting column-100" ItemStyle-CssClass="column-100">
                            <ItemTemplate>
                                <asp:Label ID="lbDeliveryDate" runat="server"
                                    Text='<%#Eval("DeliveryDate")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="EngravingDate" SortExpression="7" HeaderStyle-CssClass="sorting column-100" ItemStyle-CssClass="column-100">
                            <ItemTemplate>
                                <asp:Label ID="lbEngravingDate" runat="server"
                                    Text='<%#Eval("EngravingDate")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ReproDate" SortExpression="8" HeaderStyle-CssClass="sorting column-100" ItemStyle-CssClass="column-100">
                            <ItemTemplate>
                                <asp:Label ID="lbReproDate" runat="server"
                                    Text='<%#Eval("ReproDate")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ReproStatusName" SortExpression="9" HeaderStyle-CssClass="sorting column-150" ItemStyle-CssClass="column-150">
                            <ItemTemplate>
                                <asp:Label ID="lbReproStatusName" runat="server"
                                    Text='<%#Eval("ReproStatusName")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CylinderDate" SortExpression="10" HeaderStyle-CssClass="sorting column-120" ItemStyle-CssClass="column-120">
                            <ItemTemplate>
                                <asp:Label ID="lbCylinderDate" runat="server"
                                    Text='<%#Eval("CylinderDate")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CylinderStatusName" SortExpression="11" HeaderStyle-CssClass="sorting column-150" ItemStyle-CssClass="column-180">
                            <ItemTemplate>
                                <asp:Label ID="lbCylinderStatusName" runat="server"
                                    Text='<%#Eval("CylinderStatusName")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Note" SortExpression="12" HeaderStyle-CssClass="sorting column-150" ItemStyle-CssClass="column-150">
                            <ItemTemplate>
                                <asp:Label ID="lbNote" runat="server"
                                    Text='<%#Eval("Note")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-CssClass="column-one">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnUpdate" runat="server" CssClass="btn btn-primary btnEdit"
                                    CommandArgument='<%#Eval("JobID")%>' CommandName="UpdateEngraving">
                                    <span class="glyphicon glyphicon-edit"></span>
                                </asp:LinkButton>
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
                        <SweetSoft:GridViewPager ID="GridViewPager1" runat="server" />
                    </PagerTemplate>
                    <EmptyDataTemplate>
                        There are currently no items in this table.
                    </EmptyDataTemplate>
                </SweetSoft:GridviewExtension>
                <asp:HiddenField ID="hJobID" runat="server" />
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ModalPlaceHolder" runat="server">
    <!-- Comfirm Modal Starts here-->
    <div id="dialog-form-repro" title="Edit Engraving Schedule">
        <asp:UpdatePanel ID="upRepro" runat="server">
            <ContentTemplate>
                <div class="container-fluid">
                    <div class="row" style="background: #5bc0de">
                        <div class="col-md-6 col-sm-6">
                            <div class="form-inline">
                                <div class="form-group">
                                    <asp:LinkButton runat="server" CssClass="btn btn-transparent"
                                        ID="btnAccept" OnClick="btnAccept_Click" Text="Save">
                                        <span class="flaticon-floppy1"></span>
                                        Save
                                    </asp:LinkButton>
                                </div>
                                <div class="form-group">
                                    <asp:LinkButton runat="server" ID="btnCloseModal"
                                        CssClass="btn btn-transparent" OnClientClick="CloseRepro(); return false;">                                        
                                        <span class="flaticon-back57"></span>
                                        Close
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row" style="margin-top: 15px;">
                        <div class="form-horizontal">
                            <div class="col-sm-12">
                                <div class="form-group">
                                    <div class="col-sm-12">
                                        <div class="col-sm-6">
                                            <label class="control-label">Delivery</label>
                                            <div class="wrap-datepicker">
                                                <SweetSoft:CustomExtraTextbox ID="txtDelivery" runat="server"
                                                    RenderOnlyInput="true" data-format="dd-MM-yyyy"
                                                    CssClass="datepicker form-control mask-date">
                                                </SweetSoft:CustomExtraTextbox>
                                                <span class="fa fa-calendar in-mask-date"></span>
                                            </div>
                                        </div>
                                        <div class="col-sm-6">
                                            <label class="control-label">Engraving</label>
                                            <div class="wrap-datepicker">
                                                <SweetSoft:CustomExtraTextbox ID="txtEngravingDate" runat="server"
                                                    RenderOnlyInput="true" data-format="dd-MM-yyyy"
                                                    CssClass="datepicker form-control mask-date">
                                                </SweetSoft:CustomExtraTextbox>
                                                <span class="fa fa-calendar in-mask-date"></span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="form-group">
                                    <div class="col-sm-12">
                                        <div class="col-sm-6">
                                            <label class="control-label">Repro Date</label>
                                            <div class="wrap-datepicker">
                                                <SweetSoft:CustomExtraTextbox ID="txtReproDate" runat="server"
                                                    RenderOnlyInput="true" data-format="dd-MM-yyyy"
                                                    CssClass="datepicker form-control mask-date">
                                                </SweetSoft:CustomExtraTextbox>
                                                <span class="fa fa-calendar in-mask-date"></span>
                                            </div>
                                        </div>
                                        <div class="col-sm-6">
                                            <label class="control-label">Status</label>
                                            <asp:DropDownList ID="ddlProgressReproStatus" runat="server"
                                                data-style="btn btn-info" data-width="100%" Required="true"
                                                data-toggle="dropdown" CssClass="form-control"
                                                OnSelectedIndexChanged="ddlProgressReproStatus_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                            <label class="control-label" style="display:none;">Status Desc</label>
                                            <asp:TextBox ID="txtProgressReproStatusDesc" CssClass="form-control" Visible="false"
                                                runat="server" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="form-group">
                                    <div class="col-sm-12">
                                        <div class="col-sm-6">
                                            <label class="control-label">Cylinder Date</label>
                                            <div class="wrap-datepicker">
                                                <SweetSoft:CustomExtraTextbox ID="txtCylinderDate" runat="server"
                                                    RenderOnlyInput="true" data-format="dd-MM-yyyy"
                                                    CssClass="datepicker form-control mask-date">
                                                </SweetSoft:CustomExtraTextbox>
                                                <span class="fa fa-calendar in-mask-date"></span>
                                            </div>
                                        </div>
                                        <div class="col-sm-6">
                                            <label class="control-label">Status</label>
                                            <asp:DropDownList ID="ddlProgressCylinderStatus" runat="server"
                                                data-style="btn btn-info" data-width="100%" Required="true"
                                                data-toggle="dropdown" CssClass="form-control"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlProgressCylinderStatus_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <label class="control-label" style="display:none;">Status Desc</label>
                                            <asp:TextBox ID="txtProgressCylinderStatusDesc" CssClass="form-control" Visible="false"
                                                runat="server" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="form-group">
                                    <div class="col-sm-8">
                                        <div class="col-sm-12">
                                            <label class="control-label">Note</label>
                                            <SweetSoft:CustomExtraTextbox ID="txtNote" runat="server"
                                                RenderOnlyInput="true">
                                            </SweetSoft:CustomExtraTextbox>
                                        </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div>&nbsp;</div>
                                        <asp:Label ID="lbMessage" runat="server" Visible="false"
                                            Style="text-align: left; color: #31708f; font-weight: bold;">Data saved successfully!</asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <!--Comfirm Modal Ends here -->
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            CreateRepro();
        });

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CreateRepro);
        function CreateRepro(sender, args) {
            $("#dialog-form-repro").dialog({
                autoOpen: false,
                height: 'auto',
                width: '800',
                appendTo: "form",
                modal: true,
                resizable: false
            });
        }

        function OpenRepro() {
            $("#dialog-form-repro").dialog("open");
        }

        function CloseRepro() {
            $("#dialog-form-repro").dialog("close");
        }
    </script>
</asp:Content>
