<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="OvertimeClassificationList.aspx.cs" Inherits="HRISWeb.Overtime.OvertimeClassificationList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cntHeader" runat="server">
    <title><%=Convert.ToString(GetLocalResourceObject("lblScreenTitle")) %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cntBody" runat="server">
    <div class="main-content">
        <h1 class="text-left text-primary">
            <label class="PageTitle"><%= GetLocalResourceObject("lblScreenTitle") %></label>
        </h1>
        <br />
        <asp:UpdatePanel ID="uppMain" runat="server">
            <Triggers>
            </Triggers>
            <ContentTemplate>
                <div class="container" style="width: 100%">
                    <div class="row">
                        <div class="col-sm-12">
                            <h4 class="text-left text-primary"><%= GetLocalResourceObject("lblFiltersSubtitle") %></h4>
                            <button id="btnCleanFilters" type="button" runat="server" class="btn btn-default btnAjaxAction" onserverclick="btnCleanFilters_Click" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnClearFilters"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnClearFilters"))%>'>
                                <span class="glyphicon glyphicon-erase glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%= GetLocalResourceObject("btnClearFilters") %>
                            </button>
                        </div>
                    </div>
                    <br />
                    <div class="row">

                        <div class="col-sm-3">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%=txtClassificationCode.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblOvertimeClassificationCode")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtClassificationCode" CssClass="form-control control-validation cleanPasteText EnterKey " runat="server" autocomplete="off" MaxLength="10" type="number" min="0"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%=cboTypeCode.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblOvertimeTypeCode")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="cboTypeCode" CssClass="form-control control-validation" data-live-search="true" AutoPostBack="false" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%=txtClassificationName.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblOvertimeClassificationName")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtClassificationName" CssClass="form-control cleanPasteText EnterKey" onkeypress="return isNumberOrLetter(event);"  runat="server" autocomplete="off" MaxLength="250" type="text"></asp:TextBox>

                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%=cboDayType.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblOvertimeDayType")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="cboDayType" CssClass="form-control control-validation" data-live-search="true" AutoPostBack="false" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        
                    </div>
                    <div class="row">
                        <div class="col-sm-12 text-right">
                            <button id="btnSearchFilter" type="button" runat="server" class="btn btn-default btnAjaxAction" onserverclick="btnSearchFilter_ServerClick" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>'>
                                <span class="glyphicon glyphicon-search glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%= GetLocalResourceObject("btnSearch") %>
                            </button>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-sm-12 text-center">
                            <asp:HiddenField runat="server" ID="hdfSelectedRowIndex" Value="-1" />
                            <div>
                                <asp:GridView ID="grvList"
                                    Width="100%" runat="server" EmptyDataText='<%$ Code:GetLocalResourceObject("lblNoData") %>'
                                    EmptyDataRowStyle-CssClass="emptyRow"
                                    AllowPaging="true" PagerSettings-Visible="false" AllowSorting="true" AutoGenerateColumns="false" ShowHeader="true"
                                    CssClass="table table-striped table-hover table-bordered" DataKeyNames="OvertimeClassificationCode"
                                    OnPreRender="grvList_PreRender" OnSorting="grvList_Sorting">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; vertical-align: middle; text-align: center;">
                                                    <asp:LinkButton ID="lbtnOvertimeClassificationCodeSort" runat="server" CommandName="Sort" CommandArgument="OvertimeClassificationCode">                
                                                        <span><%= GetLocalResourceObject("lblOvertimeClassificationCode") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvOvertimeClassificationCode" data-id="OvertimeClassificationCode" data-value="<%# Eval("OvertimeClassificationCode") %>"><%# Eval("OvertimeClassificationCode") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" HeaderStyle-VerticalAlign="Middle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="lbtnOvertimeClassificationNameSort" runat="server" CommandName="Sort" CommandArgument="OvertimeClassificationName">                
                                                        <span><%= GetLocalResourceObject("lblOvertimeClassificationName") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvOvertimeClassificationName" data-id="OvertimeClassificationName" data-value="<%# Eval("OvertimeClassificationName") %>"><%# Eval("OvertimeClassificationName") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" HeaderStyle-VerticalAlign="Middle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="lbtnOvertimeTypeCodeSort" runat="server" CommandName="Sort" CommandArgument="OvertimeTypeCode">                
                                                        <span><%= GetLocalResourceObject("lblOvertimeTypeCode") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvOvertimeTypeCode" data-id="OvertimeTypeCode" data-value="<%# Eval("OvertimeTypeName") %>"><%# Eval("OvertimeTypeName") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" HeaderStyle-VerticalAlign="Middle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="lbtnWorkingSort" runat="server" CommandName="Sort" CommandArgument="WorkingTimeTypeCode">                
                                                        <span><%= GetLocalResourceObject("lblWorkingTimeTypeName") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvWorkingTimeTypeCode" data-id="WorkingTimeTypeCode" data-value="<%# Eval("WorkingTimeTypeName") %>"><%# Eval("WorkingTimeTypeName") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" HeaderStyle-VerticalAlign="Middle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="lbtnDayTypeSort" runat="server" CommandName="Sort" CommandArgument="DayTypesName">                
                                                        <span><%= GetLocalResourceObject("lblOvertimeDayType") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="DayTypeName" data-id="DayTypeName" data-value="<%# Eval("DayTypesName") %>"><%# Eval("DayTypesName") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <div id="grvListWaiting" style="position: absolute; margin: auto; display: none; bottom: 0px; top: 0px; left: 50%; transform: translate(-50%, 35%);">
                                    <span class='fa fa-spinner fa-spin' style="font-size: 50px;"></span>
                                </div>
                            </div>
                            <h4 class="text-left text-primary" id="htmlResultsSubtitle" runat="server"></h4>
                            <br />
                            <nav>
                                <asp:BulletedList ID="blstPager" runat="server" CssClass="pagination" DisplayMode="LinkButton" OnClick="blstPager_Click">
                                </asp:BulletedList>
                            </nav>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="ButtonsActions">
            <asp:UpdatePanel ID="uppActions" runat="server">
                <Triggers>
                </Triggers>
                <ContentTemplate>
                    <div class="btn-group" role="group" aria-label="main-buttons">
                        <button id="btnAdd" type="button" runat="server" class="btn btn-default btnAjaxAction" onserverclick="btnAdd_ServerClick" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnAdd"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnAdd"))%>'>
                            <span class="glyphicon glyphicon-plus glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnAdd") %>
                        </button>
                        <button id="btnEdit" type="button" runat="server" class="btn btn-default btnAjaxAction" onserverclick="btnEdit_ServerClick" onclick="return ProcessEditRequest(this.id);" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnEdit"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnEdit"))%>'>
                            <span class="glyphicon glyphicon-edit glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnEdit") %>
                        </button>
                        <button id="btnDelete" type="button" class="btn btn-default btnAjaxAction btnDelete" runat="server"
                            data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnDelete"))%>'
                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnDelete"))%>'>
                            <span class="glyphicon glyphicon-trash glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnDelete") %>
                        </button>
                        <button id="btnDeleteH" type="button" style="display: none;" runat="server" onclick="" onserverclick="btnDelete_ServerClick"></button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <%--  Modal for Add and Edit  --%>
    <div class="modal fade" id="MaintenanceDialog" tabindex="-1" role="dialog" aria-labelledby="MaintenanceDialogTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title text-primary" id="dialogTitle"></h3>
                </div>
                <asp:UpdatePanel runat="server" ID="uppDialogControl">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnEdit" EventName="ServerClick" />
                    </Triggers>
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="form-horizontal">
                                <asp:HiddenField ID="hdfOvertimeTypeCode" runat="server" Value="" />
                                <div class="form-group" style="display:none;">
                                    <div class="col-sm-5 text-left">
                                        <label for="<%= txtOvertimeClassificationCode.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblOvertimeClassificationCode")%></label>
                                    </div>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtOvertimeClassificationCode" ReadOnly="true" CssClass="form-control control-validation cleanPasteText EnterKey" runat="server" autocomplete="off" MaxLength="10" type="number" min="0"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-5 text-left">
                                        <label for="<%= txtOvertimeClassificationName.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblOvertimeClassificationName")%></label>
                                    </div>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtOvertimeClassificationName" CssClass="form-control cleanPasteText EnterKey" onkeypress="return isNumberOrLetter(event);"  runat="server" autocomplete="off" MaxLength="250" type="text"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-5 text-left">
                                        <label for="<%= droOvertimeTypeCode.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblOvertimeTypeCode")%></label>
                                    </div>
                                    <div class="col-sm-7">
                                        <asp:DropDownList ID="droOvertimeTypeCode" CssClass="form-control control-validation" data-live-search="true" AutoPostBack="false" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-5 text-left">
                                        <label for="<%= droWorkingTimeTypeCode.ClientID%>" class="control-label" style="text-align:left;"><%=GetLocalResourceObject("lblWorkingTimeTypeCode")%></label>
                                    </div>
                                    <div class="col-sm-7">
                                        <asp:DropDownList ID="droWorkingTimeTypeCode" CssClass="form-control control-validation" data-live-search="true" AutoPostBack="false" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-5 text-left">
                                        <label for="<%= ddlDayType.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblOvertimeDayType")%></label>
                                    </div>
                                    <div class="col-sm-7">
                                        <asp:DropDownList ID="ddlDayType" CssClass="form-control control-validation" data-live-search="true" AutoPostBack="false" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-5 text-left">
                                        <label for="<%= chkIsExtra.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblIsExtra")%></label>
                                    </div>
                                    <div class="col-sm-7">
                                       <asp:CheckBox ID="chkIsExtra" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button id="btnAccept" type="button" runat="server" class="btn btn-primary btnAjaxAction btnAccept btnformdisabled" onserverclick="btnAccept_ServerClick" ondblclick="return ProcessAcceptRequest(this.id);" onclick="return ProcessAcceptRequest(this.id);" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>'>
                                <span class="glyphicon glyphicon-ok glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnDialogSave")) %>
                            </button>
                            <button id="btnCancel" type="button" class="btn btn-default btnformdisabled">
                                <span class="glyphicon glyphicon-remove glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnDialogCancel")) %>
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
                <div class="alert alert-autocloseable-msg" style="display: none;">
                </div>
            </b>
        </div>
    </nav>
    <script type="text/javascript">     

        //Variables for table ordening
        var dataSortAttribute, dataSortType, dataSortDirection;
        $(document).ready(function () {


            $(document).on('keyup keypress', '.EnterKey', function (e) {

                var keyCode = e.keyCode || e.which;
                if (keyCode === 13) {
                    e.preventDefault();
                    return false;
                }
            });
        });

        var ReturnDeleteSucess;
        function pageLoad(sender, args) {
            ReturnDeleteSucess = function () {
                UnselectRow();
                ShowFooterSuccess('<%= GetLocalResourceObject("msgOperationSuccesfull") %>');
            }


            function UnselectRow() {
                /// <summary>Unselect rows</summary>  
                $('#<%=hdfSelectedRowIndex.ClientID%>').val("-1");
                $('#<%= grvList.ClientID %> tbody tr').removeClass('info');
            }
            $(".btnDelete").click(function (ev) {
                ev.preventDefault();

                var $this = this;

                var selectedRowIndex = $('#<%=hdfSelectedRowIndex.ClientID%>').val();

                if (!isEmptyOrSpaces(selectedRowIndex) && selectedRowIndex != "-1") {

                    MostrarConfirmacion(
                '<%= GetLocalResourceObject("msjDelete") %>'
                        , '<%=GetLocalResourceObject("Yes")%>'

                        , function () {
                   <%= ClientScript.GetPostBackEventReference(btnDeleteH, String.Empty) %>;
                        }
                        , '<%=GetLocalResourceObject("No")%>'
                        , function () {
                            $($this).button('reset');
                        }
                    );
                } else {
                    ShowFooterError('<%= GetLocalResourceObject("msgInvalidSelection") %>');
                }
            });

            //In this section we set the button state for btnAjaxAction
            $('.btnAjaxAction').on('click', function () {
                var $this = $(this);
                $this.button('loading');
                setTimeout(function () { $this.button('reset'); }, 300);
            });

            $('#<%= chkIsExtra.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });

            //And the grvList selection row functionality
            $('#<%= grvList.ClientID %>').on('click', 'tbody tr', function (event) {

                if (!$(this).hasClass('emptyRow')) {
                    $(this).toggleClass('info').siblings().removeClass('info');
                    $('#<%=hdfSelectedRowIndex.ClientID%>').val($(this).hasClass("info") ? $(this).index() : -1);
                    console.log($('#<%=hdfSelectedRowIndex.ClientID%>').val());
                }
            });

            //And the grvList pager event for client side
            $('#<%= blstPager.ClientID %>').on('click', 'a', function (event) {
                SetWaitingGrvList();
            });

            //others buttons 
            $('#<%= btnAdd.ClientID %>').on('click', function (ev) {
                /// <summary>Handles the click event for button add.</summary>
                ev.preventDefault();
                $('#' + document.forms[0].id).validate().destroy();
                setTimeout(function () {
                    $("#<%=btnAdd.ClientID%>").button('reset');
                }, 500);

                // set the mode of the window
                $('#MaintenanceDialog').data('windowmode', 'add');

                DisableToolBar();
                $(".btnAccept").prop("disabled", false);
                $('.btnAccept').removeAttr('disabled');
                $("#<%=txtOvertimeClassificationCode.ClientID%>").val("");
                $("#<%=txtOvertimeClassificationName.ClientID%>").val("");
                $("#<%=droOvertimeTypeCode.ClientID%>").val("");
                $("#<%=droWorkingTimeTypeCode.ClientID%>").val("");
                if (validator != null) {
                    validator.resetForm();
                    validator.destroy();
                }

                $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceAdd") %>');
                $('#dialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceAdd") %>');
                $('#MaintenanceDialog').modal('show');

                EnableToolBar();
                return false;
            });

            $('#btnCancel').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                event.preventDefault();
                if (validator != null) {
                    validator.destroy();
                }

                DisableButtonsDialog();
                $('#MaintenanceDialog').modal('hide');
                EnableButtonsDialog();
            });



            $("#btnCancelAddDivision").on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                event.preventDefault();
                DisableButtonsDialog();
                $('#adddetailmodal').modal('hide');
                $('#MaintenanceDialog').modal('show');
                EnableButtonsDialog();
            });

            $("#btnCancelSearchUser").on('click', function (event) {
                /// <summary>Handles the click event for button btnCancelSearchUser in user dialog.</summary>            
                event.preventDefault();
                DisableButtonsDialog();
                $('#searchmodal').modal('hide');
                $('#MaintenanceDialog').modal('show');
                EnableButtonsDialog();
            });

            $('#MaintenanceDialog').on("shown.bs.modal", function () {
                /// <summary>Handles the shown event for MaintenanceDialog dialog.</summary>
                if ($('#MaintenanceDialog').data('windowmode') === 'edit') {

                }
                else {

                }
            });

            //And the clean paste manager
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

            //In this section we initialize the checkbox toogles
            //In this section we initialize the popovers and tooltips for the elements.
            //We have both (popovers and tooltips) in order to accomplish information alert over desktop 
            //and mobiles devices
            InitializeTooltipsPopovers();
        }

        var validator = null;

        var validatorSearchUser = null;

        function SetControlInvalid(controlId) {
            /// <summary>Set the appearance of the control as invalid </summary>
            /// <param name="controlId" type="String">Id of the control</param>            
            $('#' + controlId).addClass("Invalid");
            $('label[for=' + controlId + '].label-validation').show();
        }

        function SetControlValid(controlId) {
            /// <summary>Set the appearance of the control as valid </summary>
            /// <param name="controlId" type="String">Id of the control</param>                        
            $('#' + controlId).removeClass("Invalid");
            $('label[for=' + controlId + '].label-validation').hide();
        }

        function IsRowSelected() {
            /// <summary>Validate if there is a selected row</summary>  
            var selectedRowIndex = $('#<%=hdfSelectedRowIndex.ClientID%>').val();
            if (!isEmptyOrSpaces(selectedRowIndex) && selectedRowIndex != "-1") {
                return true;
            }
            return false;
        }

        function IsRowCheked(gridId) {
            /// <summary>Validate if there is a cheked row </summary>
            /// <param name="gridId" type="String">Id of the control</param>
            var checks = gridId.find('input:checkbox[id*="chkSelected"]:checked:not(#chkSelectedAll)');
            return checks.length > 0 ? true : false;
        }

        function IsOnlyOneRowCheked(gridId) {
            /// <summary>Validate if there is a cheked row </summary>
            /// <param name="gridId" type="String">Id of the control</param>
            var checks = gridId.find('input:checkbox[id*="chkSelected"]:checked:not(#chkSelectedAll)');
            return checks.length === 1 ? true : false;
        }

        function SetWaitingGrvList() {
            /// <summary>Process the request of set the grid as waiting style</summary>

            setTimeout(function () {
                $('#<%= grvList.ClientID %>').find("input,button,textarea,select").attr("disabled", "disabled");
                $('#<%= grvList.ClientID %>').find("a").removeAttr('href');
                $('#<%= blstPager.ClientID %>').find("a").removeAttr('href');

                $('#grvListWaiting').fadeIn('fast');
                $('#<%= grvList.ClientID %>').fadeTo('fast', 0.5);

            }, 100);
        }

        function ProcessEditRequest(resetId) {
            /// <summary>Process the edit request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>

            // set the mode of the window
            $('#MaintenanceDialog').data('windowmode', 'edit');
            $(".btnAccept").prop("disabled", false);
            $('.btnAccept').removeAttr('disabled');
            disableButton($("#btnSearchUser"));
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
            /// <summary>Process the accept request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>

            disableButton($('#btnCancel'));

            $(".btnAccept").prop("disabled", true);
            $("#<%=txtOvertimeClassificationCode.ClientID%>").prop('disabled', false);
            if (!ValidateForm()) {
                setTimeout(function () {
                    ResetButton(resetId);
                    enableButton($('#btnCancel'));
                    $(".btnAccept").prop("disabled", false);
                }, 150);
            }
            else {
                __doPostBack('<%= btnAccept.UniqueID %>', '');
            }
            $("#<%=txtOvertimeClassificationCode.ClientID%>").prop('disabled', true);
            return false;
        }

        var validator = null;
        function ValidateForm() {
            /// <returns> True if form is valid. False otherwise. </returns>
            $('#' + document.forms[0].id).validate().destroy();

            //add custom validation methods
            jQuery.validator.addMethod("validSelection", function (value, element) {
                return this.optional(element) || value != "-1";
            }, "Please select a valid option");

            if (validator == null) {
                //declare the validator
                var drpvalue = document.getElementById("<%=droOvertimeTypeCode.ClientID%>");
                drpvalue.value = drpvalue.value === "0" ? "" : drpvalue.value;

                var drpWorkingTime = document.getElementById("<%=droWorkingTimeTypeCode.ClientID%>");
                drpWorkingTime.value = drpWorkingTime.value === "0" ? "" : drpWorkingTime.value;
                var validator =
                    $('#' + document.forms[0].id).validate({
                        debug: true,
                        highlight: function (element, errorClass, validClass) {
                            SetControlInvalid($(element).attr('id'));
                        },
                        unhighlight: function (element, errorClass, validClass) {
                            SetControlValid($(element).attr('id'));
                        },
                        errorPlacement: function (error, element) { },
                        rules: {
                            <%= txtOvertimeClassificationCode.UniqueID %>: {
                    required: true
                        , normalizer: function(value) {
                            return $.trim(value);
                        }
                },
                            "<%= droOvertimeTypeCode.UniqueID %>": {
                            required: true
                            , validSelection: true
                },
                "<%= droWorkingTimeTypeCode.UniqueID %>": {
                    required: true
                        , validSelection: true
                },
                "<%= ddlDayType.UniqueID %>": {
                    required: true
                        , validSelection: true
                },
                        <%= txtOvertimeClassificationName.UniqueID %>: {
                    required: true
                        , normalizer: function(value) {
                            return $.trim(value);
                        }
                },

            }
        });
        }
         else
        {
            validator.validate();
        }
        //get the results            
        var result = validator.form();
        drpvalue.value = drpvalue.value === "" ? "0" : drpvalue.value;
        drpWorkingTime.value = drpWorkingTime.value === "" ? "0" : drpWorkingTime.value;
        return result;
        }

        function ProcessBtnAddUserRequest(resetId) {
            /// <summary>Process the selected user search</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            if (!IsOnlyOneRowCheked($("#<%= txtOvertimeClassificationCode.ClientID%>"))) {
                ErrorButton(resetId);
                MostrarMensaje(TipoMensaje.VALIDACION, '<%= GetLocalResourceObject("msgInvalidSelection") %>', function () {
                    ResetButton(resetId);
                });

                <%--ShowFooterError('<%= GetLocalResourceObject("msgInvalidSelection") %>');--%>
            }
            else {
                var checks = $('#<%= txtOvertimeClassificationCode.ClientID%>').find('input:checkbox[id*="chkSelected"]:checked:not(#chkSelectedAll)');
                // fill the fields with the info

                setTimeout(function () { ResetButton(resetId) }, 200);
                $('#searchmodal').modal('hide');
                $('#MaintenanceDialog').modal('show');
            }
            return false;
        }

        function ProcessDeleteRequest(resetId) {
            /// <summary>Process the delete request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>

            if ($('#MaintenanceDialog').data('windowmode') === 'edit') {

            }
            else {

            }

        }


        function CloseUserDialog() {
            /// <summary>Close a user dialog.</summary>
            $('#MaintenanceDialog').modal('hide');
        }

        function DisableToolBar() {
            ///<summary>Disables all buttons toolbar</summary>
            disableButton($('#<%= btnAdd.ClientID %>'));
            disableButton($('#<%= btnEdit.ClientID %>'));
        }

        function EnableToolBar() {
            ///<summary>Enabled all buttons toolbar</summary>
            enableButton($('#<%= btnAdd.ClientID %>'));
            enableButton($('#<%= btnEdit.ClientID %>'));
        }

        function DisableButtonsDialog() {
            ///<summary>Disables all buttons toolbar</summary>
            disableButton($('#btnAccept'));
            disableButton($('#btnCancel'));
        }

        function EnableButtonsDialog() {
            ///<summary>Enabled all buttons toolbar</summary>
            enableButton($('#btnAccept'));
            enableButton($('#btnCancel'));
        }

        function ReturnFromBtnEditClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>            
            DisableToolBar();
            $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#dialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#MaintenanceDialog').modal('show');
            EnableToolBar();
        }

        function ReturnFromBtnAcceptClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>      
            $(".btnAccept").prop("disabled", true);
            DisableButtonsDialog();


            // $('.btnAccept').removeAttr('disabled');

            $('#MaintenanceDialog').modal('hide');
            $(".btnAccept").prop("disabled", false);

            EnableButtonsDialog();


            ShowFooterSuccess('<%= GetLocalResourceObject("msgOperationSuccesfull") %>');


        }

        function ReturnFromAcceptAddNewDivisionRequest() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            DisableButtonsDialog();
            $('#adddetailmodal').modal('hide');
            //$('#MaintenanceDialog').modal('show');
            EnableButtonsDialog();
        }

        //In this section we set the control for multiple simultaneous ajax request
        //We cancel the ajax request executed when another one is in progress
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(initializeRequest);
        function initializeRequest(sender, args) {

            if (prm.get_isInAsyncPostBack()) {
                args.set_cancel(true);
                <%--ShowFooterAlert('<%=GetLocalResourceObject("msgWaitWhileProcessing")%>');--%>
                AddTemporaryClass($("#" + args.get_postBackElement().id), "btn-warning", 1500);
                setTimeout(function () {
                    $("#" + args.get_postBackElement().id).button('error');
                    setTimeout(function () {
                        $("#" + args.get_postBackElement().id).button('reset');
                    }, 1500)
                }, 100);
            }
        }
    </script>
</asp:Content>
