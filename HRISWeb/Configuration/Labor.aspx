<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Labor.aspx.cs" Inherits="HRISWeb.Configuration.Labor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntHeader" runat="server">
    <title><%=Convert.ToString(GetLocalResourceObject("lblScreenTitle")) %></title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cntBody" runat="server">
    <style>
        .bootstrap-select > .dropdown-toggle.bs-placeholder, .bootstrap-select > .dropdown-toggle.bs-placeholder:active,
        .bootstrap-select > .dropdown-toggle.bs-placeholder:focus, .bootstrap-select > .dropdown-toggle.bs-placeholder:hover {
            color: #333;
        }
    </style>

    <div class="main-content">
        <asp:Panel ID="pnlMainContent" runat="server" DefaultButton="btnSearchDefault">
            <h1 class="text-left text-primary">
                <label class="PageTitle"><%= GetLocalResourceObject("lblScreenTitle") %></label>
            </h1>
            <br />

            <asp:UpdatePanel runat="server" ID="main">
                <Triggers>
                </Triggers>

                <ContentTemplate>
                    <div class="container" style="width: 100%">
                        <h4 class="text-left text-primary"><%= GetLocalResourceObject("lblFiltersSubtitle") %></h4>
                        <br />

                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-horizontal">
                                    <div class="row">
                                        <div class="form-group col-sm-12 col-md-6">
                                            <div class="col-sm-5 text-left">
                                                <label for="<%= LaborCodeFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("CodeLbl")%></label>
                                            </div>

                                            <div class="col-sm-7">
                                                <input id="LaborCodeFilter" type="text" class="form-control" runat="server" data-id="laborFilter" data-value="isPermitted"/>
                                            </div>
                                        </div>

                                        <div class="form-group col-sm-12 col-md-6">
                                            <div class="col-sm-5 text-left">
                                                <label for="<%= LaborNameFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("NameLbl")%></label>
                                            </div>

                                            <div class="col-sm-7">
                                                <input id="LaborNameFilter" type="text" class="form-control" runat="server" data-id="LaborName" data-value="isPermitted"/>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="form-group col-sm-12 col-md-6">
                                            <div class="col-sm-5 text-left">
                                                <label for="<%= LaborRegionalFilter.ClientID%>" class="control-label"><%= GetLocalResourceObject("LblLaborRegionalCode") %></label>
                                            </div>

                                            <div class="col-sm-7">
                                                <input id="LaborRegionalFilter" type="text" class="form-control" runat="server" data-id="LaborRegional" data-value="isPermitted"/>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-12 text-center">
                                <button id="btnSearch" type="button" runat="server" class="btn btn-default btnAjaxAction" onserverclick="BtnSearch_ServerClick" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>'>
                                    <span class="glyphicon glyphicon-search glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%= GetLocalResourceObject("btnSearch") %>
                                </button>
                                <asp:Button ID="btnSearchDefault" runat="server" OnClick="BtnSearch_ServerClick" Style="display: none;" />
                            </div>
                        </div>
                        <br />
                        <br />

                        <div class="row">
                            <div class="col-sm-12 text-center">
                                <asp:HiddenField runat="server" ID="hdfSelectedRowIndex" Value="-1" />

                                <div>
                                    <asp:GridView ID="grvList"
                                        Width="100%"
                                        runat="server"
                                        EmptyDataText='<%$ Code:GetLocalResourceObject("lblNoData") %>' EmptyDataRowStyle-CssClass="emptyRow"
                                        AllowPaging="false" PagerSettings-Visible="false" AllowSorting="true"
                                        AutoGenerateColumns="false" ShowHeader="true"
                                        CssClass="table table-striped table-hover table-bordered"
                                        DataKeyNames="LaborId"
                                        OnPreRender="GrvList_PreRender" OnSorting="GrvList_Sorting">
                                        <Columns>
                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="LaborCodeSort" runat="server" CommandName="Sort" CommandArgument="LaborCode"
                                                            OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("CodeLbl") %></span>&nbsp;
                                                            <i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "LaborCode") %> sorterDirection' 
                                                                aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvLaborCode" data-id="LaborCode" data-value="<%# Eval("LaborCode") %>"><%# Eval("LaborCode") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="LaborNameSort" runat="server" CommandName="Sort" CommandArgument="LaborName" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("NameLbl") %></span>&nbsp;
                                                            <i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "LaborName") %> sorterDirection'
                                                                 aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvLaborName" data-id="LaborName" data-value="<%# Eval("LaborName") %>"><%# Eval("LaborName") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="OrdersSort" runat="server" CommandName="Sort" CommandArgument="Orders" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("lblOrders") %> </span>&nbsp;
                                                            <i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "Orders") %> sorterDirection'
                                                                 aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvOrders" data-id="Orders" data-value="<%# Eval("Orders") %>"><%# Eval("Orders") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="LaborRegionalCodeSort" runat="server" CommandName="Sort" CommandArgument="LaborRegionalCode" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("LblLaborRegionalCode") %> </span>&nbsp;
                                                            <i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "LaborRegionalCode") %> sorterDirection'
                                                                 aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvLaborRegionalCode" data-id="LaborRegionalCode" data-value="<%# Eval("LaborRegionalCode") %>"><%# Eval("LaborRegionalCode") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>

                                    <div id="grvListWaiting" style="position: absolute; margin: auto; display: none; bottom: 0px; top: 0px; left: 50%; transform: translate(-50%, 35%);">
                                        <span class='fa fa-spinner fa-spin' style="font-size: 50px;"></span>
                                    </div>
                                </div>

                                <h4 class="text-left text-primary" id="htmlResultsSubtitle" runat="server"></h4>

                                <nav>
                                    <asp:BulletedList ID="blstPager" runat="server" CssClass="pagination" DisplayMode="LinkButton" OnClick="BlstPager_Click">
                                    </asp:BulletedList>
                                </nav>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>

        <div class="ButtonsActions">
            <asp:UpdatePanel ID="uppActions" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                <Triggers>
                </Triggers>

                <ContentTemplate>
                    <div class="btn-group" role="group" aria-label="main-buttons">
                        <button id="btnAdd" type="button" runat="server" class="btn btn-default btnAjaxAction"
                            data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnAdd"))%>'
                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnAdd"))%>'>
                            <span class="glyphicon glyphicon-plus glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnAdd") %>
                        </button>

                        <button id="btnaddEven" onserverclick="BtnAddClick_ServerClick"
                            runat="server" style="display: none;">
                        </button>

                        <button id="btnEdit" type="button" runat="server" class="btn btn-default btnAjaxAction"
                            onserverclick="BtnEdit_ServerClick" onclick="return ProcessEditRequest(this.id);"
                            data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnEdit"))%>'
                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnEdit"))%>'>
                            <span class="glyphicon glyphicon-edit glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnEdit") %>
                        </button>

                        <button id="btnDelete" type="button" runat="server" class="btn btn-default btnAjaxAction"
                            onserverclick="BtnDelete_ServerClick" onclick="return ProcessDeleteRequest(this.id);"
                            data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnDelete"))%>'
                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnDelete"))%>'>
                            <span class="glyphicon glyphicon-trash glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnDelete") %>
                        </button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <%--  Modal for Add and Edit MapPosistions Carrer--%>
    <div class="modal fade" id="MaintenanceDialog" tabindex="-1" role="dialog" aria-labelledby="MaintenanceDialogTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog " style="width: 55% !important;" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" id="btnClose" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title text-primary" id="dialogTitle"></h3>
                </div>

                <asp:UpdatePanel runat="server" ID="uppDialogControl">
                    <ContentTemplate>
                        <div class="modal-body" style="padding: 43px;">
                            <div class="form-horizontal">
                                <asp:HiddenField ID="LaborIdEdit" runat="server" Value="" />

                                <div class="form-group">
                                    <label for="<%=LaborCodeEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("CodeLbl")%></label>
                                    <input type="text" id="LaborCodeEdit" runat="server" class="form-control limpiarCampos cleanPasteText" onkeypress="return isNumberOrLetter(event);" onkeyup="return isNumberOrLetter(event);" maxlength="10" value="" required />
                                    <label id="LaborCodeEditValidation" for="<%= LaborCodeEdit.ClientID%>" class="label label-danger label-validation"
                                        data-toggle="tooltip" data-container="body" data-placement="left"
                                        data-content="<%= GetLocalResourceObject("msjCampo10EditValidation") %>"
                                        style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">
                                        !</label>
                                </div>

                                <div class="form-group">
                                    <label for="<%=LaborNameEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblEmployeeNameHeader")%></label>
                                    <input type="text" id="LaborNameEdit" runat="server" class="form-control limpiarCampos cleanPasteText" onkeypress="return isNumberOrLetter(event);" onkeyup="return isNumberOrLetter(event);" maxlength="500" required />
                                    <label id="LaborNameEditValidation" for="<%= LaborNameEdit.ClientID%>" class="label label-danger label-validation"
                                        data-toggle="tooltip" data-container="body" data-placement="left"
                                        data-content="<%= GetLocalResourceObject("msjCampo500EditValidation") %>"
                                        style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">
                                        !</label>
                                </div>

                                <div class="form-group">
                                    <label for="<%=OrdersEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblOrders")%></label>
                                    <input type="number" id="OrdersEdit" runat="server" class="form-control limpiarCampos cleanPasteText" max="2147483647"
                                        oninput="javascript: if (this.value.length > this.maxLength) this.value = this.value.slice(0, this.maxLength);"
                                        maxlength="10" onkeypress="return isNumberOrLetter(event);" onkeyup="return isNumberOrLetter(event);" min="0" value="" />
                                    <label id="OrdersEditValidation" for="<%= OrdersEdit.ClientID%>" class="label label-danger label-validation"
                                        data-toggle="tooltip" data-container="body" data-placement="left"
                                        data-content="<%= GetLocalResourceObject("msjCampoNumberEditValidation") %>"
                                        style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">
                                        !</label>
                                </div>

                                <div class="form-group">
                                    <label for="<%=LaborRegionalCodeEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("LblLaborRegionalCode")%></label>
                                    <select id="LaborRegionalCodeEdit" runat="server" class="form-control limpiarCampos cleanPasteText">
                                    </select>
                                    <label id="LaborRegionalCodeEditValidation" for="<%= LaborRegionalCodeEdit.ClientID%>" class="label label-danger label-validation"
                                        data-toggle="tooltip" data-container="body" data-placement="left"
                                        data-content="<%= GetLocalResourceObject("msjSelectEditValidation") %>"
                                        style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">
                                        !</label>
                                </div>

                                <div class="form-group">
                                    <label>
                                        <%=GetLocalResourceObject("lblSearchEnabled")%>
                                        <asp:CheckBox ID="SearchEnabledEdit" CssClass="limpiarCampos" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                    </label>
                                </div>
                            </div>
                        </div>

                        <div class="modal-footer">
                            <button id="btnAccept" type="button" runat="server" class="btn btn-primary btnAjaxAction btnedit"
                                onserverclick="BtnAccept_ServerClick" onclick="return ProcessAcceptRequest(this.id);"
                                data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>'
                                data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>'>
                                <span class="glyphicon glyphicon-ok glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnDialogSave")) %>
                            </button>

                            <button id="btnCancel" type="button" class="btn btn-default">
                                <span class="glyphicon glyphicon-remove glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnDialogCancel")) %>
                            </button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <%--  Modal for Duplicated value  --%>
    <div class="modal fade" id="DuplicatedDialog" tabindex="-1" role="dialog" aria-labelledby="DuplicatedDialogTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button id="btnDuplicatedClose" type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title text-primary"><%=Convert.ToString(GetLocalResourceObject("TitleDuplicatedDialog")) %></h3>
                </div>

                <asp:UpdatePanel runat="server" ID="uppDuplicatedDialog">
                    <Triggers>
                    </Triggers>

                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="form-horizontal">
                                <div id="divDuplicatedDialogText" runat="server">
                                </div>

                                <asp:Panel ID="pnlDuplicatedDialogDataDetail" runat="server">
                                    <div class="form-group">
                                        <div class="col-sm-4 text-left">
                                            <label for="<%= txtDuplicatedLaborCode.ClientID%>" class="control-label"><%=GetLocalResourceObject("CodeLbl")%></label>
                                        </div>

                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtDuplicatedLaborCode" CssClass="form-control cleanPasteText ignoreValidation" runat="server" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-4 text-left">
                                            <label for="<%= txtDuplicatedLaborName.ClientID%>" class="control-label"><%=GetLocalResourceObject("NameLbl")%></label>
                                        </div>

                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtDuplicatedLaborName" CssClass="form-control cleanPasteText ignoreValidation" runat="server" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>

                        <div class="modal-footer">
                            <button id="btnDuplicatedAccept" type="button" class="btn btn-default">
                                <span class="glyphicon glyphicon-remove glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnAccept")) %>
                            </button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <nav class="navbar-fixed-bottom">
        <div class="container center-block text-center">
            <b>
                <div class="alert alert-autocloseable-msg" style="display: none;"></div>
            </b>
        </div>
    </nav>

    <script type="text/javascript">
        //*******************************//
        //          VARIABLES            // 
        //*******************************//
        var dataSortAttribute, dataSortType, dataSortDirection;
        var validator = null;

        //*******************************//
        //       EVENT BINDING           // 
        //*******************************//
        function pageLoad(sender, args) {
            $('#MaintenanceDialog').on('keyup keypress', '.enterkey', function (e) {
                var keyCode = e.keyCode || e.which;
                if (keyCode === 13) {
                    e.preventDefault();
                    return false;
                }
            });
            //////////////////////////////////////////////////////////////////////////////////////////////////

            //In this section we initialize the checkbox toogles
            $('#<%= SearchEnabledEdit.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });
            //////////////////////////////////////////////////////////////////////////////////////////////////

            //In this section we set the button state for btnAjaxAction
            $('.btnAjaxAction').on('click', function () {
                var $this = $(this);
                $this.button('loading');
                setTimeout(function () { $this.button('reset'); }, 30000);
            });
            //////////////////////////////////////////////////////////////////////////////////////////////////

            //In this section we set the grvList functionality
            $('#<%= grvList.ClientID %>').on('click', 'tbody tr', function (event) {
                if (!$(this).hasClass('emptyRow')) {
                    $(this).toggleClass('info').siblings().removeClass('info');
                    $('#<%=hdfSelectedRowIndex.ClientID%>').val($(this).hasClass("info") ? $(this).index() : -1);
                }
            });

            $('#<%= blstPager.ClientID %>').on('click', 'a', function (event) {
                SetWaitingGrvList(true);
            });

            $('#<%= btnSearchDefault.ClientID %>').on('click', function (event) {
                var $this = $('#<%= btnSearch.ClientID %>');
                $this.button('loading');
                setTimeout(function () { $this.button('reset'); }, 30000);
            });
            //////////////////////////////////////////////////////////////////////////////////////////////////

            //In this section we set the button generics functionality
            $('#btnCancel, #btnClose').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                event.preventDefault();

                ClearModalForm();

                DisableButtonsDialog();
                $('#MaintenanceDialog').modal('hide');
                EnableButtonsDialog();
            });

            $('#btnActivateDeletedCancel, #btnActivateDeletedClose').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                event.preventDefault();
                $('#MaintenanceDialog').modal('show');
                $('#ActivateDeletedDialog').modal('hide');
            });

            $('#btnDuplicatedAccept, #btnDuplicatedClose').on('click', function (event) {
                /// <summary>Handles the click event for button accept in user dialog.</summary>            
                event.preventDefault();
                $('#DuplicatedDialog').modal('hide');
                $('#MaintenanceDialog').modal('show');
            });
            //////////////////////////////////////////////////////////////////////////////////////////////////

            //In this section we set the others controls functionality 
            $('#<%= btnAdd.ClientID %>').on('click', function (ev) {
                /// <summary>Handles the click event for button add.</summary>
                ev.preventDefault();

                $(".limpiarCampos").val("");
                $(".limpiarCampos").prop("disabled", false);

                $('#<%=SearchEnabledEdit.ClientID%>').bootstrapToggle('<%= GetLocalResourceObject("Yes") %>');
                $('#<%= LaborCodeEdit.ClientID %>').prop('disabled', false);

                setTimeout(function () {
                    $("#<%=btnAdd.ClientID%>").button('reset');
                }, 500);

                DisableToolBar();
                ClearModalForm();

                __doPostBack('<%= btnaddEven.UniqueID %>', '');

                $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceAdd") %>');

                return false;
            });

            $('#<%= LaborCodeFilter.ClientID %>').on('keyup keypress', function (e) {
                ReturnFromBtnSearchClickPostBack(e);

                return isNumberOrLetter(e);
            });

            $('#<%= LaborNameFilter.ClientID %>').on('keyup keypress', function (e) {
                ReturnFromBtnSearchClickPostBack(e);

                return isNumberOrLetter(e);
            });

            $('#<%= LaborRegionalFilter.ClientID %>').on('keyup keypress', function (e) {
                ReturnFromBtnSearchClickPostBack(e);

                return isNumberOrLetter(e);
            });

            $("#btnCancelLimpiar").click(function () {
                $('#dialogTitle').html('');
                $(".limpiarCampos").val("");
                $(".limpiarCampos").prop("disabled", true);
                $(".btnedit").prop("disabled", true);
            });

            //////////////////////////////////////////////////////////////////////////////////////////////////

            //In this section we set the clean paste manager functionality
            $('.cleanPasteText').on('paste', function (e) {
                var $this = $(this);

                setTimeout(function (e) {
                    replacePastedInvalidCharacters($this);
                    var ml = $this.attr("maxlength");
                    if (ml) {
                        checkMaxLength($this[0], e, parseInt(ml));
                    }
                }, 50);
            });

            $('.cleanPasteDigits').on('paste', function (e) {
                var $this = $(this);

                setTimeout(function (e) {
                    replacePastedInvalidDigits($this);
                    var ml = $this.attr("maxlength");
                    if (ml) {
                        checkMaxLength($this[0], e, parseInt(ml));
                    }
                }, 50);
            });

            //////////////////////////////////////////////////////////////////////////////////////////////////

            InitializeTooltipsPopovers();

            $(".sorter").click(function () {
                var $table = $(this).closest('table');
                var $dataSortSrc = $(this).find()
                dataSortAttribute = $(this).attr("data-sort-attr");
                dataSortType = $(this).attr("data-sort-type");
                dataSortDirection = $(this).attr("data-sort-direction");

                if (isEmptyOrSpaces(dataSortDirection)) {
                    dataSortDirection = "0";
                }
                else if (dataSortDirection == "0") {
                    dataSortDirection = "1";
                }
                else if (dataSortDirection == "1") {
                    dataSortDirection = "0";
                }

                $table.children('tbody').sortChildren(function (A, B) {

                    var a, b;
                    if (dataSortType == "int") {
                        a = parseInt($(A).find(".data-sort-src").attr(dataSortAttribute));
                        b = parseInt($(B).find(".data-sort-src").attr(dataSortAttribute));
                    }
                    if (dataSortType == "string") {
                        a = $(A).find(".data-sort-src").attr(dataSortAttribute);
                        b = $(B).find(".data-sort-src").attr(dataSortAttribute);
                    }

                    var orderA, orderB;
                    if (dataSortDirection == "0") {
                        orderA = -1;
                        orderB = 1;
                    }
                    else if (dataSortDirection == "1") {
                        orderA = 1;
                        orderB = -1;
                    }

                    return a < b ? orderA : a > b ? orderB : 0;
                });

                $table.find(".sorter").each(function () {
                    $(this).attr("data-sort-direction", "");
                    $(this).find(".sorterDirection").removeClass("fa-sort-asc");
                    $(this).find(".sorterDirection").removeClass("fa-sort-desc");
                });

                $(this).attr("data-sort-direction", dataSortDirection);
                if (dataSortDirection == "0") {
                    $(this).find(".sorterDirection").addClass("fa-sort-asc");
                }
                else if (dataSortDirection == "1") {
                    $(this).find(".sorterDirection").addClass("fa-sort-desc");
                }
            });

            SetRowSelected();

            $(".trim").change(function () {
                var text = $.trim($(this).val());
                $(this).val(text);
            });
        }

        //*******************************//
        //          VALIDATION           // 
        //*******************************//
        function ValidateForm() {
            jQuery.validator.addMethod("validSelection", function (value, element) {
                return this.optional(element) || value != "-1";
            }, "Please select a valid option");

            if (validator != null) {
                validator.destroy();
            }

            validator = $('#' + document.forms[0].id).validate({
                debug: true, ignore: ".ignoreValidation, :hidden",
                highlight: function (element, errorClass, validClass) {
                    SetControlInvalid($(element).attr('id'));
                },
                unhighlight: function (element, errorClass, validClass) {
                    SetControlValid($(element).attr('id'));
                },
                errorPlacement: function (error, element) { },
                rules: {
                    "<%= LaborCodeEdit.UniqueID%>": {
                        required: true,
                        normalizer: function (value) {
                            return $.trim(value);
                        },
                        minlength: 1,
                        maxlength: 10
                    },
                    "<%= LaborNameEdit.UniqueID%>": {
                        required: true,
                        normalizer: function (value) {
                            return $.trim(value);
                        },
                        minlength: 1,
                        maxlength: 500
                    }
                    ,
                    "<%= OrdersEdit.UniqueID%>": {
                        required: true, min: 0, max: 2147483647
                    }
                    ,
                    "<%= LaborRegionalCodeEdit.UniqueID%>": {
                        required: true, validSelection: true
                    }
                }
            });

            var result = validator.form();
            return result;
        }

        //*******************************//
        //             LOGIC             //
        //*******************************//
        function IsRowSelected() {
            /// <summary>Validate if there is a selected row</summary>  
            var selectedRowIndex = $('#<%=hdfSelectedRowIndex.ClientID%>').val();
            if (!isEmptyOrSpaces(selectedRowIndex) && selectedRowIndex != "-1") {
                return true;
            }
            return false;
        }

        function SetRowSelected() {
            /// <summary>Set the class of the selected row</summary>  
            var selectedRowIndex = $('#<%=hdfSelectedRowIndex.ClientID%>').val();
            if (!isEmptyOrSpaces(selectedRowIndex) && selectedRowIndex != "-1") {
                var selectedRow = $('#<%= grvList.ClientID %> tbody tr:eq(' + selectedRowIndex + ')');
                selectedRow.siblings().removeClass('info');
                if (!selectedRow.hasClass('info')) {
                    selectedRow.addClass('info');
                }
            }
        }

        function UnselectRow() {
            /// <summary>Unselect rows</summary>  
            $('#<%=hdfSelectedRowIndex.ClientID%>').val("-1");
            $('#<%= grvList.ClientID %> tbody tr').removeClass('info');
        }

        function ClearModalForm() {
            $(".emptyinput").val("");

            $("#<%=LaborIdEdit.ClientID%>").val("-1");

            if (validator != null) {
                validator.resetForm();
            }
        }

        function SetWaitingGrvList(flag) {
            /// <summary>Process the request of set the grid as waiting style</summary>
            if (flag) {
                $('#<%= grvList.ClientID %>').find("input,button,textarea,select").attr("disabled", "disabled");
                $('#grvListWaiting').fadeIn('fast');
                $('#<%= grvList.ClientID %>').fadeTo('fast', 0.5);
            } else {
                $('#<%= grvList.ClientID %>').find("input,button,textarea,select").removeAttr("disabled");
                $('#grvListWaiting').fadeOut('fast');
                $('#<%= grvList.ClientID %>').fadeIn('fast', 0);
                $('#<%= grvList.ClientID %>').removeAttr("style opacity");
            }
        }

        function DisableToolBar() {
            ///<summary>Disables all buttons toolbar</summary>
            disableButton($('#<%= btnAdd.ClientID %>'));
            disableButton($('#<%= btnEdit.ClientID %>'));
            disableButton($('#<%= btnDelete.ClientID %>'));
        }

        function EnableToolBar() {
            ///<summary>Enabled all buttons toolbar</summary>
            enableButton($('#<%= btnAdd.ClientID %>'));
            enableButton($('#<%= btnEdit.ClientID %>'));
            enableButton($('#<%= btnDelete.ClientID %>'));
        }

        function DisableButtonsDialog() {
            ///<summary>Disables all buttons toolbar</summary>
            disableButton($('#<%=btnAccept.ClientID%>'));
            disableButton($('#btnCancel'));
        }

        function EnableButtonsDialog() {
            ///<summary>Enabled all buttons toolbar</summary>
            enableButton($('#<%=btnAccept.ClientID%>'));
            enableButton($('#btnCancel'));
        }

        function SetControlInvalid(controlId) {
            /// <summary>Set the appearance of the control as invalid </summary>
            /// <param name="controlId" type="String">Id of the control</param>            
            if ($("#" + controlId).is(".selectpicker")) {
                $('button[data-id=' + controlId + '].dropdown-toggle').addClass("Invalid");
                $('#' + controlId).addClass("Invalid");
                $('label[for=' + controlId + '].label-validation').show();
            }
            else {
                $('#' + controlId).addClass("Invalid");
                $('label[for=' + controlId + '].label-validation').show();
            }
        }

        function SetControlValid(controlId) {
            /// <summary>Set the appearance of the control as valid </summary>
            /// <param name="controlId" type="String">Id of the control</param>
            if ($("#" + controlId).is(".selectpicker")) {
                $('button[data-id=' + controlId + '].dropdown-toggle').removeClass("Invalid");
                $('#' + controlId).removeClass("Invalid");
                $('label[for=' + controlId + '].label-validation').hide();
            }
            else {
                $('#' + controlId).removeClass("Invalid");
                $('label[for=' + controlId + '].label-validation').hide();
            }
        }

        //*******************************//
        //           PROCESS             //
        //*******************************//
        function ProcessEditRequest(resetId) {
            /// <summary>Process the edit request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            if (IsRowSelected()) {
                __doPostBack('<%= btnEdit.UniqueID %>', '');
                return true;
            }

            else {
                ShowFooterError('<%= GetLocalResourceObject("msgInvalidSelection") %>');
                ErrorButton(resetId);
                return false;
            }
        }

        function ProcessAcceptRequest(resetId) {
            DisableButtonsDialog();

            if (!ValidateForm()) {
                setTimeout(function () {
                    ResetButton(resetId);
                    enableButton($('#btnCancel'));
                    EnableButtonsDialog();
                }, 150);

                MostrarMensaje(TipoMensaje.VALIDACION, '<%= GetLocalResourceObject("msgInvalidEntry") %>', null);
            }

            else {
                __doPostBack('<%= btnAccept.UniqueID %>', '');
            }

            return false;
        }

        function ProcessDeleteRequest(resetId) {
            /// <summary>Process the delete request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            if (IsRowSelected()) {
                ShowConfirmationMessageDelete(resetId);
                return false;
            }

            else {
                ShowFooterError('<%= GetLocalResourceObject("msgInvalidSelection") %>');
                ErrorButton(resetId);
                return false;
            }
        }

        //*******************************//
        //  RETURN FROM POSTBACKS LOGIC  // 
        //*******************************//
        function ReturnFromBtnSearchClickPostBack(e) {
            var keyCode = e.keyCode || e.which;

            if (keyCode === 13) {
                __doPostBack('<%= btnSearchDefault.UniqueID %>', '');
                return false;
            }
        }

        function ReturnRequestBtnEditOpen() {
            SetRowSelected();
            DisableToolBar();

            $('.limpiarCampos').prop('disabled', false);
            $('#<%= LaborCodeEdit.ClientID %>').prop('disabled', true);

            $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#dialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#MaintenanceDialog').modal('show');

            EnableToolBar();
        }

        function ReturnPostBackAcceptClickSave() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            DisableButtonsDialog();
            $("#<%=LaborIdEdit.ClientID%>").val("");

            $(".limpiarCampos").val("");
            $(".limpiarCampos").prop("disabled", true);

            SetRowSelected();
            ClearModalForm();

            $('#MaintenanceDialog').modal('hide');

            ShowFooterSuccess('<%= GetLocalResourceObject("msgOperationSuccesfull") %>');
            EnableButtonsDialog();
        }

        function ReturnPostBackAcceptClickSave2(edit) {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            DisableButtonsDialog();
            $(".limpiarCampos").prop("disabled", false);

            if (edit == 1) {
                $("#<%=LaborCodeEdit.ClientID%>").prop("disabled", true);
            }

            $('#MaintenanceDialog').modal('hide');

            EnableButtonsDialog();
        }

        function ReturnFromBtnAcceptActivateDeletedClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            SetRowSelected();
            DisableButtonsDialog();

            $('#ActivateDeletedDialog').modal('hide');
            $('#MaintenanceDialog').modal('hide');

            EnableButtonsDialog();

            ShowFooterSuccess('<%= GetLocalResourceObject("msgOperationSuccesfull") %>');
        }

        function ReturnRequestBtnAddOpen() {
            $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceAdd") %>');
            $('#dialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceAdd") %>');
            $('#MaintenanceDialog').modal('show');

            EnableToolBar();
        }

        function ReturnFromBtnAcceptClickPostBackDeleted() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $('#MaintenanceDialog').modal('hide');
            $('#ActivateDeletedDialog').modal('show');
        }

        function ReturnFromBtnAcceptClickPostBackDuplicated() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $('#MaintenanceDialog').modal('hide');
            $('#DuplicatedDialog').modal('show');
        }

        function ReturnFromBtnDeleteClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            UnselectRow();

            ShowFooterSuccess('<%= GetLocalResourceObject("msgOperationSuccesfull") %>');
        }

        //*******************************//
        // MESSAGING AND CONFIRMATION    // 
        //*******************************//
        function ShowConfirmationMessageDelete(resetId) {
            /// <summary>Show confirmation message for Save and Close funtionality</summary>
            /// <param name="resetId" type="String">Id of the button to reset if cancel</param>
            /// <returns>False</returns>
            MostrarConfirmacion(
                '<%= GetLocalResourceObject("msjDelete") %>',
                '<%=GetLocalResourceObject("Yes")%>', function () {
                    __doPostBack('<%= btnDelete.UniqueID %>', '');
            }, '<%=GetLocalResourceObject("No")%>', function () {
                $("#" + resetId).button('reset');
            }
            );

            return false;
        }

        //*******************************//
        //AJAX CONCURRENCY ADMINISTRATION// 
        //*******************************//
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(initializeRequest);
        prm.add_endRequest(endingRequest);

        function initializeRequest(sender, args) {
            if (prm.get_isInAsyncPostBack()) {
                args.set_cancel(true);
                //this line was initially included but causes that the original postback being cancel too
                //prm.abortPostBack();
                ShowFooterAlert('<%=GetLocalResourceObject("msgWaitWhileProcessing")%>');
                AddTemporaryClass($("#" + args.get_postBackElement().id), "btn-warning", 1500);

                setTimeout(function () {
                    $("#" + args.get_postBackElement().id).button('error');
                    setTimeout(function () {
                        $("#" + args.get_postBackElement().id).button('reset');
                    }, 1500)
                }, 100);
            }
        }

        function endingRequest(sender, args) {
            if (prm.get_isInAsyncPostBack()) {
                SetWaitingGrvList(false)
            }
        }
    </script>
</asp:Content>
