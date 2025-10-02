<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Profiles.aspx.cs" Inherits="HRISWeb.Security.Privileges" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntHeader" runat="server">
    <title><%=Convert.ToString(GetLocalResourceObject("lblScreenTitle")) %></title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cntBody" runat="server">
    <div class="main-content">
        <h1 class="text-left text-primary">
            <label class="PageTitle"><%= GetLocalResourceObject("lblScreenTitle") %></label>
        </h1>

        <asp:UpdatePanel runat="server" ID="uppControls">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSave" />
                <asp:AsyncPostBackTrigger ControlID="btnDelete" />
            </Triggers>

            <ContentTemplate>
                <div class="form-horizontal">
                    <div class="form-group" style="margin-bottom: 5px;">
                        <div class="col-sm-12 col-md-3 text-left">
                            <label for="<%=cboDivisionFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblDivisionCode")%></label>
                        </div>

                        <div class="col-sm-12 col-md-4">
                            <asp:DropDownList ID="cboDivisionFilter" CssClass="form-control" AutoPostBack="true" runat="server"></asp:DropDownList>
                        </div>
                    </div>
                </div>

                <asp:HiddenField runat="server" ID="hdfSelectedRowIndex" />

                <asp:GridView ID="grvProfile" 
                    Width="100%" 
                    runat="server" 
                    AutoGenerateColumns="false" 
                    CssClass="Grid"
                    DataKeyNames="ProfileID, DivisionCode">
                    <Columns>
                        <asp:TemplateField ItemStyle-CssClass="HiddeControl" HeaderStyle-CssClass="HiddeControl" FooterStyle-CssClass="HiddeControl" ControlStyle-CssClass="HiddeControl" >
                            <ItemTemplate>
                                <span id="dvProfileID" data-id="ProfileID" data-value="<%# Eval("ProfileID") %>"><%# Eval("ProfileID") %></span>
                            </ItemTemplate>
                        </asp:TemplateField> 

                        <asp:TemplateField ItemStyle-CssClass="HiddeControl" HeaderStyle-CssClass="HiddeControl" FooterStyle-CssClass="HiddeControl" ControlStyle-CssClass="HiddeControl" >
                            <ItemTemplate>
                                <span id="dvDivisionCode" data-id="DivisionCode" data-value="<%# Eval("DivisionCode") %>"><%# Eval("DivisionCode") %></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" meta:resourcekey="DivisionName">
                            <ItemTemplate>
                                <span id="dvDivisionName" data-id="DivisionName" data-value="<%# Eval("DivisionName") %>"><%# Eval("DivisionName") %></span>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" meta:resourcekey="ProfileDescription">
                            <ItemTemplate>
                                <span id="dvProfileDescription" data-id="ProfileDescription" data-value="<%# Eval("ProfileDescription") %>"><%# Eval("ProfileDescription") %></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>  
                
                <nav>
                    <asp:BulletedList ID="blstPager" runat="server" CssClass="pagination" DisplayMode="LinkButton" OnClick="BlstPager_Click">
                    </asp:BulletedList>
                </nav>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="ButtonsActions">
            <asp:UpdatePanel ID="uppBotonera" runat="server">
                <ContentTemplate>
                    <div class="btn-group" role="group" aria-label="main-buttons">
                        <button id="btnAdd" type="button" class="btn btn-default">
                            <span class="glyphicon glyphicon-plus glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnAdd") %>
                        </button>

                        <button id="btnEdit" type="button" class="btn btn-default">
                            <span class="glyphicon glyphicon-edit glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnEdit") %>
                        </button>

                        <button id="btnConfirmDelete" type="button" class="btn btn-default " onclick="ConfirmDelete();">
                            <span class="glyphicon glyphicon-trash glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnDelete") %>
                        </button>
                        <div style="display: none">
                            <asp:Button runat="server" ID="btnDelete" OnClick="BtnDelete_Click" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <%--  Modal  --%>
    <div class="modal fade" id="ProfilesDialog" tabindex="-1" role="dialog" aria-labelledby="ProfilesDialogTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title text-primary" id="dialogTitle"></h3>
                </div>

                <asp:UpdatePanel runat="server" ID="uppDialogControl">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label for="<%=cboDivisions.ClientID%>" class="col-sm-4 control-label"><%= Convert.ToString(GetLocalResourceObject("lblDivisionCode")) %></label>
                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="cboDivisions" CssClass="form-control" DataTextField="DivisionName" DataValueField="DivisionCode" AutoPostBack="false" runat="server"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label for="<%=txtProfileDescription.ClientID%>" class="col-sm-4 control-label"><%= Convert.ToString(GetLocalResourceObject("lblProfileDescription")) %></label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtProfileDescription" CssClass="form-control" runat="server" onkeypress="return isNumberOrLetter(event)" MaxLength="50" TextMode="MultiLine" Columns="2"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4"></div>
                                    <div class="col-sm-8">
                                        <asp:CheckBox ID="chkAddAllUser" runat="server" />
                                         
                                        <div class="btn-group">
                                            <label for="<%=chkAddAllUser.ClientID%>" class="btn btn-info">
                                                <span class="glyphicon glyphicon-ok"></span>
                                                <span> </span>
                                            </label>
                                            <label for="<%=chkAddAllUser.ClientID%>" class="btn btn-default active">
                                                <%=GetLocalResourceObject("lblAsign")%>
                                            </label>
                                        </div>
                                    </div>
                                </div>

                                <div class="tableRowDiv" style="display: none;">
                                    <div class="divCellTitle">
                                        <asp:TextBox ID="txtProfileID" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="modal-footer">
                            <button id="btnAcept" runat="server" class="btn btn-primary" onclick="IsValidSave(); return false;">
                                <%=Convert.ToString(GetLocalResourceObject("btnDialogSave")) %>
                            </button>

                            <button id="btnCancel" class="btn btn-default"><%=Convert.ToString(GetLocalResourceObject("btnDialogCancel")) %></button>

                            <div style="display: none">
                                <asp:Button runat="server" ID="btnSave" OnClick="BtnSave_Click" />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {
            // bind events related to controls within ajax sections (update panels)
            EnlazarEventosJqueryEnUpdatePanels();

            // re-binds events when the end of request event is executed
            var pageRqstMgr = Sys.WebForms.PageRequestManager.getInstance();
            //
            pageRqstMgr.add_endRequest(function (sender, args) {
                EnlazarEventosJqueryEnUpdatePanels();
            });

        });

        function EnlazarEventosJqueryEnUpdatePanels() {
            /// <summary>Binds jquery (client side) events from controls within update panels.</summary>

            $('.btn').mouseup(function () {
                /// <summary>Handles the mouse up event for all buttons in page.</summary>

                $(this).blur();
            });

            $('#btnCancel').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>
                DisableButtonsDialog();
                $('#ProfilesDialog').modal('hide');
                EnableButtonsDialog();
                return false;
            });

            $('#btnAdd').on('click', function (ev) {
                /// <summary>Handles the click event for button add.</summary>

                DisableToolBar();
                $("#<%=txtProfileID.ClientID%>").val(0);
                $("#<%=cboDivisions.ClientID%>").val(0);
                $("#<%=txtProfileDescription.ClientID%>").val("");

                $("#<%=chkAddAllUser.ClientID%>").prop("disabled", false);
                $("#<%=chkAddAllUser.ClientID%>").prop('checked', false);
                $('#ProfilesDialogTitle').html('<%= GetLocalResourceObject("DialogProfileAdd") %>');
                $('#dialogTitle').html('<%= GetLocalResourceObject("DialogProfileAdd") %>');
                $('#ProfilesDialog').modal('show');
                EnableToolBar();
                return false;
            });
            $('#btnEdit').on('click', function (ev) {
                //// <summary>Handles the click event for button edit.</summary>
                DisableToolBar();
                $('#ProfilesDialogTitle').html('<%= GetLocalResourceObject("DialogProfileEdit") %>');
                $('#dialogTitle').html('<%= GetLocalResourceObject("DialogProfileEdit") %>');
                
                if (IsProfileSelected()) {
                    var selectedRow = $("#<%= grvProfile.ClientID %> .SelectTR")[0];
                 
                    var ProfileIDValue = GetValueInRow(selectedRow, "ProfileID");
                    var DivisionCodeValue = GetValueInRow(selectedRow, "DivisionCode");
                    var ProfileDescriptionValue = GetValueInRow(selectedRow, "ProfileDescription");

                    $("#<%=txtProfileID.ClientID%>").val(ProfileIDValue);
                    $("#<%=cboDivisions.ClientID%>").val(DivisionCodeValue);
                    $("#<%=txtProfileDescription.ClientID%>").val(ProfileDescriptionValue);
                    $("#<%=chkAddAllUser.ClientID%>").prop('checked', false);
                    $("#<%=chkAddAllUser.ClientID%>").prop("disabled", true);
                    $('#ProfilesDialog').modal('show');
                }
                EnableToolBar();
                return false;
            });

            $(".ItemSelector").on('click', function (ev) {
                //// <summary>Handles the click event for button edit.</summary>
                var element = ev.target;
                var row = $(element).closest("tr")[0];
                var rowIndex = row.rowIndex - 1;
                var grid = $(row).closest(".Grid")[0];
                $("#" + grid.id + " .SelectTR").removeClass("SelectTR");
                $(row).addClass("SelectTR");

                if (grid.id == '<%=grvProfile.ClientID%>') {
                    $("#<%=hdfSelectedRowIndex.ClientID%>").val(rowIndex);
                }
                return false;
            });
        }

        $("#ProfilesDialog").on("hidden.bs.modal", function () {
            $("#<%=txtProfileID.ClientID%>").val('');
            $("#<%=cboDivisions.ClientID%>").val('0');
            $("#<%=txtProfileDescription.ClientID%>").val('');
            $("#<%=chkAddAllUser.ClientID%>").prop('checked', false);
        });

        function IsProfileSelected() {
            /// <summary>Validates a profile has been selected.</summary>
            /// <returns type="bool" />
            if ($("#<%= grvProfile.ClientID %> .SelectTR")[0]) {
                return true;
            }
            else
            {
                 MostrarMensaje(
                    TipoMensaje.VALIDACION
                    , '<%= GetLocalResourceObject("msjSelectAProfile") %>'
                    , null);

                return false;
            }
            return true;
        }

        function IsValidSave() {
            /// <summary>Determines if the save action is valid.</summary>
            /// <returns type="bool" />
            DisableButtonsDialog();
            var cboDivisionValue = $.trim($("#<%=cboDivisions.ClientID%>").val());
            if (!cboDivisionValue
                || cboDivisionValue == '0') {

                MostrarMensaje(
                    TipoMensaje.VALIDACION
                    , '<%= GetLocalResourceObject("msjDivisionInvalid") %>'
                    , null); 
                EnableButtonsDialog();
                return;
            }

            var txtProfileDescriptionText = $.trim($("#<%=txtProfileDescription.ClientID%>").val());
            if (!ValidateOnlyCharactersValid(txtProfileDescriptionText
                , ''
                , "<%=txtProfileDescription.ClientID%>"))
            {
                $("#<%=txtProfileDescription.ClientID%>").val('');
                EnableButtonsDialog();
                return false;
            }
            if (!txtProfileDescriptionText) {
                MostrarMensaje(
                    TipoMensaje.VALIDACION
                    , '<%= GetLocalResourceObject("msjProfileDescriptionInvalid") %>'
                    , null);
                EnableButtonsDialog();
                return;
            }

            if ($("#<%=txtProfileID.ClientID%>").val() == '0') {
                if ($('#<%=chkAddAllUser.ClientID%>').prop('checked'))
                {
                    MostrarConfirmacion(
                        '<%= GetLocalResourceObject("msjCheckSave") %>'
                        , '<%= GetLocalResourceObject("Si") %>'
                        , function (){
                            $("#<%=btnSave.ClientID%>").click();
                        }
                        , '<%= GetLocalResourceObject("No") %>'
                        , null
                    );

                }
                else {
                    $("#<%=btnSave.ClientID%>").click();
                }
                EnableButtonsDialog();
            }
            else {
                $("#<%=btnSave.ClientID%>").click();
                EnableButtonsDialog();
            }
            EnableButtonsDialog();
        }

        function ConfirmDelete() {
            /// <summary>Handles the Delete button click client event.</summary>
            /// <returns type="bool" />
            DisableToolBar();
            if (IsProfileSelected())
            {
                MostrarConfirmacion(
                    '<%= GetLocalResourceObject("msjDelete") %>'
                    , '<%= GetLocalResourceObject("Si") %>'
                    , function (){
                        $("#<%=btnDelete.ClientID%>").click();
                    }
                    , '<%= GetLocalResourceObject("No") %>'
                    , null
                );
            }
            EnableToolBar();
        }

        function CloseUserDialog() {
            /// <summary>Close a user dialog.</summary>
            $('#ProfilesDialog').modal('hide');
        }

        function DisableToolBar()
        {
            ///<summary>Disables all buttons toolbar</summary>
            disableButton($('#btnAdd'));
            disableButton($('#btnEdit'));
            disableButton($('#btnConfirmDelete'));
        }

        function EnableToolBar()
        {
            ///<summary>Enabled all buttons toolbar</summary>
            enableButton($('#btnAdd'));
            enableButton($('#btnEdit'));
            enableButton($('#btnConfirmDelete'));
        }

        function DisableButtonsDialog() {
            ///<summary>Disables all buttons toolbar</summary>
            disableButton($('#btnAcept'));
            disableButton($('#btnCancel'));
        }

        function EnableButtonsDialog() {
            ///<summary>Enabled all buttons toolbar</summary>
            enableButton($('#btnAcept'));
            enableButton($('#btnCancel'));
        }
    </script>
</asp:Content>
