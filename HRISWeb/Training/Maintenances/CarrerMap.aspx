<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="CarrerMap.aspx.cs" Inherits="HRISWeb.Training.Maintenances.CarrerMap" %>

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
                                                <label for="<%= PositionCodeFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblPositionCodeEdit")%></label>
                                            </div>

                                            <div class="col-sm-7">
                                                <select class="form-control" runat="server" id="PositionCodeFilter"></select>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-12 text-center">
                                <button id="btnSearch" type="button" runat="server" class="btn btn-default btnAjaxAction" 
                                    onserverclick="BtnSearch_ServerClick" 
                                    data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>' 
                                    data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>'>
                                    <span class="glyphicon glyphicon-search glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%= GetLocalResourceObject("btnSearch") %>
                                </button>
                                <asp:Button ID="btnSearchDefault" runat="server" OnClick="BtnSearch_ServerClick" Style="display: none;" />
                            </div>
                        </div>
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
                                        DataKeyNames="MapCatalogPositionsId"
                                        OnPreRender="GrvList_PreRender" OnSorting="GrvList_Sorting">
                                        <Columns>
                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="CategoryFTEDescripcionSort" runat="server" CommandName="Sort" CommandArgument="CategoryFTE.CategoryFTEDescripcion"
                                                            OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("CategoryFTEHeaderText") %></span>&nbsp;
                                                            <i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "CategoryFTE.CategoryFTEDescripcion") %> sorterDirection' 
                                                                aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvCategoryFTEDescripcion" data-id="CategoryFTE.CategoryFTEDescripcion" data-value="<%# Eval("CategoryFTE.CategoryFTEDescripcion") %>"><%# Eval("CategoryFTE.CategoryFTEDescripcion") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="PositionSort" runat="server" CommandName="Sort" CommandArgument="Position.PositionName" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("PositionNameHeaderText") %></span>&nbsp;
                                                            <i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "Position.PositionName") %> sorterDirection'
                                                                 aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvPositionName" data-id="Position.PositionName" data-value="<%# Eval("Position.PositionName") %>"><%# Eval("Position.PositionName") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="CompanyIDsSort" runat="server" CommandName="Sort" CommandArgument="CompanyIDs" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("lblCompanyIDEdit") %></span>&nbsp;
                                                            <i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "CompanyIDs") %> sorterDirection'
                                                                 aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvCompanyIDs" data-id="CompanyIDs" style="word-break: break-all" data-value="<%# Eval("CompanyIDs") %>"><%# Eval("CompanyIDs") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="NominalClassIdsSort" runat="server" CommandName="Sort" CommandArgument="NominalClassIds" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("NominalClassIdsHeaderText") %></span>&nbsp;
                                                            <i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "NominalClassIds") %> sorterDirection'
                                                                 aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvNominalClassIds" data-id="NominalClassIds" style="word-break: break-all" data-value="<%# Eval("NominalClassIds") %>"><%# Eval("NominalClassIds") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="PaymentRateNameSort" runat="server" CommandName="Sort" CommandArgument="PaymentRateName" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("PaymentRateCodeHeaderText") %></span>&nbsp;
                                                            <i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "PaymentRateCode") %> sorterDirection'
                                                                 aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvPaymentRateCode" data-id="PaymentRateCode" data-value="<%# Eval("PaymentRateCode") %>"><%# Eval("PaymentRateCode") %></span>
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
        <div class="modal-dialog " style="width: 90% !important;" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" id="btnClose" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title text-primary" id="dialogTitle"></h3>
                </div>

                <asp:UpdatePanel runat="server" ID="uppDialogControl">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="form-horizontal">
                                <asp:HiddenField ID="hdfMapCatalogPositionsIdEdit" runat="server" Value="" />

                                <div class="form-group">
                                    <div class="col-sm-2 text-left">
                                        <label for="<%=CategoryFTEIdEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCategoryFTEId")%></label>
                                    </div>

                                    <div class="col-sm-10">
                                        <select class="form-control selectpicker2 limpiarCampos" runat="server" data-live-search="true" id="CategoryFTEIdEdit"></select>

                                        <label id="CategoryFTEIdEditValidation" for="<%= CategoryFTEIdEdit.ClientID%>" 
                                            class="label label-danger label-validation" data-toggle="tooltip" 
                                            data-container="body" data-placement="left"
                                            data-content="<%= GetLocalResourceObject("msjCategoryFTEIdEditValidation") %>"
                                            style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-2 text-left">
                                        <label for="<%=PositionCodeEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblPositionCodeEdit")%></label>
                                    </div>

                                    <div class="col-sm-10">
                                        <select class="form-control selectpicker2 limpiarCampos" runat="server" data-live-search="true" id="PositionCodeEdit"></select>

                                        <label id="PositionCodeEditValidation" for="<%= PositionCodeEdit.ClientID%>" 
                                            class="label label-danger label-validation" data-toggle="tooltip" 
                                            data-container="body" data-placement="left"
                                            data-content="<%= GetLocalResourceObject("msjPositionCodeEditValidation") %>"
                                            style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-2 text-left">
                                        <label for="<%=CompanyIDEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCompanyIDEdit")%></label>
                                    </div>

                                    <div class="col-sm-10">
                                        <select class="form-control selectpicker onchangeCBOcompany" runat="server" id="CompanyIDEdit" multiple="true" data-live-search="true" data-actions-box="true"></select>

                                        <input type="hidden" runat="server" class="limpiarCampos" id="CompanyIDEditMultiple" value="" />

                                        <label id="CompanyIDEditValidation" for="<%= CompanyIDEdit.ClientID%>" 
                                            class="label label-danger label-validation" data-toggle="tooltip" 
                                            data-container="body" data-placement="left"
                                            data-content="<%= GetLocalResourceObject("msjCompanyIDEditValidation") %>"
                                            style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-2 text-left">
                                        <label for="<%=NominalClassIdEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblNominalClassIdEdit")%></label>
                                    </div>

                                    <div class="col-sm-10">
                                        <select class="form-control selectpicker onchangeCBOclassNomina" runat="server" id="NominalClassIdEdit" multiple="true" data-live-search="true" data-actions-box="true"></select>

                                        <input type="hidden" runat="server" class="limpiarCampos" id="NominalClassIdEditMultiple" value="" />

                                        <label id="NominalClassIdEditValidation" for="<%= NominalClassIdEdit.ClientID%>" 
                                            class="label label-danger label-validation" data-toggle="tooltip" 
                                            data-container="body" data-placement="left"
                                            data-content="<%= GetLocalResourceObject("msjNominalClassIdEditValidation") %>"
                                            style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                    </div>
                                </div>

                                <div class="form-group PaymentRateCodeEdit" style="display: none;">
                                    <div class="col-sm-2 text-left">
                                        <label for="<%=PaymentRateCodeEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblPaymentRateCodeEdit")%></label>
                                    </div>

                                    <div class="col-sm-10">
                                        <select class="form-control selectpicker2 limpiarCampos" runat="server" id="PaymentRateCodeEdit" data-live-search="true"></select>

                                        <input type="hidden" runat="server" class="limpiarCampos" id="PaymentRateCodeEditMultiple" value="" />

                                        <label id="PaymentRateCodeEditValidation" for="<%= PaymentRateCodeEdit.ClientID%>" 
                                            class="label label-danger label-validation" data-toggle="tooltip" 
                                            data-container="body" data-placement="left"
                                            data-content="<%= GetLocalResourceObject("msjPaymentRateCodeEditValidation") %>"
                                            style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="modal-footer">
                            <button id="btnAccept" type="button" runat="server" class="btn btn-primary btnAjaxAction"
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
                                <div id="divDuplicatedDialogText" runat="server"></div>

                                <asp:Panel ID="pnlDuplicatedDialogDataDetail" runat="server">
                                    <div class="form-group">
                                        <div class="col-sm-4 text-left">
                                            <label for="<%= txtDuplicatedPositionName.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblPositionCodeEdit")%></label>
                                        </div>

                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtDuplicatedPositionName" CssClass="form-control cleanPasteText ignoreValidation" runat="server" ReadOnly="true"></asp:TextBox>
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
        var dataSortAttribute, dataSortType, dataSortDirection;
        var validator = null;
        var CompanyIDEdit;
        var CompanyIDEditMultiple;
        var NominalClassIdEdit;
        var NominalClassIdEditMultiple;

        function pageLoad(sender, args) {
            //checkbox SearchEnabledEdit
            $('.selectpicker').selectpicker();
            $(".selectpicker2").selectpicker();

            CompanyIDEdit = $("#<%=CompanyIDEdit.ClientID %>");
            CompanyIDEditMultiple = $("#<%=CompanyIDEditMultiple.ClientID%>");

            NominalClassIdEdit = $("#<%=NominalClassIdEdit.ClientID %>");
            NominalClassIdEditMultiple = $("#<%=NominalClassIdEditMultiple.ClientID%>");

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

            $(".trim").change(function () {
                var text = $.trim($(this).val());
                $(this).val(text);
            });

            $('.btnAjaxAction').on('click', function () {
                var $this = $(this);
                $this.button('loading');
                setTimeout(function () { $this.button('reset'); }, 30000);
            });

            $("#<%= CategoryFTEIdEdit.ClientID%>").change(function () {
                if ($(this).val() == 2) {
                    $(".PaymentRateCodeEdit").show();
                } else {
                    $(".PaymentRateCodeEdit").hide();
                }
            });

            //And the grvList selection row functionality
            $('#<%= grvList.ClientID %>').on('click', 'tbody tr', function (event) {
                if (!$(this).hasClass('emptyRow')) {
                    $(this).toggleClass('info').siblings().removeClass('info');
                    $('#<%=hdfSelectedRowIndex.ClientID%>').val($(this).hasClass("info") ? $(this).index() : -1);
                }
            });

            //And the grvList pager event for client side
            $('#<%= blstPager.ClientID %>').on('click', 'a', function (event) {
                SetWaitingGrvList(true);
            });

            $('#<%= btnSearchDefault.ClientID %>').on('click', function (event) {
                var $this = $('#<%= btnSearch.ClientID %>');
                $this.button('loading');
                setTimeout(function () { $this.button('reset'); }, 30000);
            });

            $(".onchangeCBOcompany").on("changed.bs.select", function () {
                MultiSelectDropdownListSaveSelectedItems($("#<%=CompanyIDEdit.ClientID %>"), $("#<%=CompanyIDEditMultiple.ClientID%>"));
            });

            $(".onchangeCBOclassNomina").on("changed.bs.select", function () {
                MultiSelectDropdownListSaveSelectedItems($("#<%=NominalClassIdEdit.ClientID %>"), $("#<%=NominalClassIdEditMultiple.ClientID%>"));
            });

            // btn accions
            $('#<%= btnAdd.ClientID %>').on('click', function (ev) {
                /// <summary>Handles the click event for button add.</summary>
                $(".btnAjaxAction").prop("disabled", true);
                ev.preventDefault();

                $("#<%=CompanyIDEdit.ClientID %>").selectpicker("val", "");
                $("#<%=NominalClassIdEdit.ClientID %>").selectpicker("val", "");
                $(".limpiarCampos").val("");
                $(".limpiarCampos").prop("disabled", false);
                $('.selectpicker2').selectpicker('render');

                $('#<%= PositionCodeEdit.ClientID %>').prop('disabled', false);
                $('#<%= PositionCodeEdit.ClientID %>').selectpicker('refresh');
                $(".PaymentRateCodeEdit").hide();

                setTimeout(function () {
                    $("#<%=btnAdd.ClientID%>").button('reset');
                }, 500);

                DisableToolBar();
                ClearModalForm();

                $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceAdd") %>');
                $('#dialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceAdd") %>');
                $('#MaintenanceDialog').modal('show');

                EnableToolBar();

                $(".btnAjaxAction").prop("disabled", false);

                return false;
            });

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

            //And the clean paste manager
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

            $(document).on('keyup keypress', '.enterkey', function (e) {
                var keyCode = e.keyCode || e.which;
                if (keyCode === 13) {
                    e.preventDefault();
                    return false;
                }
            });

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
        }

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
                    "<%= CategoryFTEIdEdit.UniqueID%>": {
                        required: true,
                        validSelection: true
                    },

                    "<%= PositionCodeEdit.UniqueID%>": {
                        required: true, validSelection: true
                    },

                    "<%= CompanyIDEdit.UniqueID%>": {
                        required: true, validSelection: true
                    },

                    "<%= NominalClassIdEdit.UniqueID%>": {
                        required: true, validSelection: true
                    }
                }
            });

            var result = validator.form();
            return result;
        }

        function ClearModalForm() {
            $(".emptyinput").val("");

            $("#<%=hdfMapCatalogPositionsIdEdit.ClientID%>").val("-1");

            if (validator != null) {
                validator.resetForm();
            }
        }

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

        function SetWaitingGrvList(true) {
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

        function ProcessEditRequest(resetId) {
            /// <summary>Process the edit request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            $(".btnAjaxAction").prop("disabled", true);

            if (IsRowSelected()) {
                $('#<%= PositionCodeEdit.ClientID %>').prop('disabled', true);
                __doPostBack('<%= btnEdit.UniqueID %>', '');
                return true;
            }

            else {
                ShowFooterError('<%= GetLocalResourceObject("msgInvalidSelection") %>');
                ErrorButton(resetId);
                $(".btnAjaxAction").prop("disabled", false);
                return false;
            }
        }

        function ProcessAcceptRequest(resetId) {
            disableButton($('#btnCancel'));
            disableButton($("#" + resetId));

            if (!ValidateForm()) {
                setTimeout(function () {
                    ResetButton(resetId);
                    enableButton($("#" + resetId));
                    enableButton($('#btnCancel'));
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

        function ReturnRequestBtnEditOpen() {
            SetRowSelected();
            DisableToolBar();

            $('#<%= PositionCodeEdit.ClientID %>').prop('disabled', true);
            $('#<%= PositionCodeEdit.ClientID %>').selectpicker('refresh');
            MultiSelectDropdownListRestoreSelectedItems($("#<%=CompanyIDEdit.ClientID %>"), $("#<%=CompanyIDEditMultiple.ClientID%>"));
            MultiSelectDropdownListRestoreSelectedItems($("#<%=NominalClassIdEdit.ClientID %>"), $("#<%=NominalClassIdEditMultiple.ClientID%>"));

            $('.selectpicker2').selectpicker('render');

            if ($('#<%= CategoryFTEIdEdit.ClientID %>').val() == 2) {
                $(".PaymentRateCodeEdit").show();
            } else {
                $(".PaymentRateCodeEdit").hide();
            }

            $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#dialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#MaintenanceDialog').modal('show');

            EnableToolBar();
            $(".btnAjaxAction").prop("disabled", false);
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
            disableButton($('#btnAccept'));
            disableButton($('#btnCancel'));
        }

        function EnableButtonsDialog() {
            ///<summary>Enabled all buttons toolbar</summary>
            enableButton($('#btnAccept'));
            enableButton($('#btnCancel'));
        }

        function ReturnPostBackAcceptClickSave() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            DisableButtonsDialog();
            SetRowSelected();
            ClearModalForm();

            $('#MaintenanceDialog').modal('hide');
            ShowFooterSuccess('<%= GetLocalResourceObject("msgOperationSuccesfull") %>');

            EnableButtonsDialog();

            $(".btnAjaxAction").prop("disabled", false);
        }

        function ReturnFromBtnAcceptActivateDeletedClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            SetRowSelected();
            DisableButtonsDialog();

            $('#ActivateDeletedDialog').modal('hide');
            $('#MaintenanceDialog').modal('hide');

            EnableButtonsDialog();

            $(".btnAjaxAction").prop("disabled", false);
            ShowFooterSuccess('<%= GetLocalResourceObject("msgOperationSuccesfull") %>');
        }

        function ReturnFromBtnAcceptClickPostBackDeleted() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $('#MaintenanceDialog').modal('hide');
            $('#ActivateDeletedDialog').modal('show');
            $(".btnAjaxAction").prop("disabled", false);
        }

        function ReturnFromBtnAcceptClickPostBackDuplicated() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $('#MaintenanceDialog').modal('hide');
            $('#DuplicatedDialog').modal('show');
            $(".btnAjaxAction").prop("disabled", false);
        }

        function ReturnFromBtnDeleteClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            UnselectRow();

            ShowFooterSuccess('<%= GetLocalResourceObject("msgOperationSuccesfull") %>');
            $(".btnAjaxAction").prop("disabled", false);
        }

        function ShowConfirmationMessageDelete(resetId) {
            /// <summary>Show confirmation message for Save and Close funtionality</summary>
            /// <param name="resetId" type="String">Id of the button to reset if cancel</param>
            /// <returns>False</returns>
            MostrarConfirmacion(
                '<%= GetLocalResourceObject("msjDelete") %>',
                '<%=GetLocalResourceObject("Yes")%>', function () {
                    __doPostBack('<%= btnDelete.UniqueID %>', '');
                },
                '<%=GetLocalResourceObject("No")%>', function () {
                    $("#" + resetId).button('reset');
                }
            );

            return false;
        }

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
