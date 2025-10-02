<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="InitiativeCoordinator.aspx.cs" Inherits="HRISWeb.SocialResponsability.Maintenances.InitiativeCoordinator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntHeader" runat="server">
    <title><%=Convert.ToString(GetLocalResourceObject("lblScreenTitle")) %></title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cntBody" runat="server">
    <div class="main-content">
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
                        <div class="col-sm-12">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="form-group col-sm-12 col-md-8">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%= txtInitiativeCoordinatorDesSpanishFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblUserName")%></label>
                                        </div>

                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtInitiativeCoordinatorDesSpanishFilter" CssClass="form-control control-validation cleanPasteText ignoreValidation" runat="server" onkeypress="blockEnterKey(); return isNumberOrLetter(event)" MaxLength="50" autocomplete="off" type="text"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group col-sm-12 col-md-8">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%= cboDivisionFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblDivision")%></label>
                                        </div>

                                        <div class="col-sm-7">
                                            <asp:DropDownList ID="cboDivisionFilter" CssClass="form-control control-validation selectpicker" AutoPostBack="true" runat="server" data-live-search="true" data-actions-box="true" data-id="Division" data-value="isPermitted" OnSelectedIndexChanged="cboDivision_SelectedIndexChanged"></asp:DropDownList>
                                            <label id="cboDivisionValidationFilter" for="<%= cboDivisionFilter.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjCourseValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                            <asp:HiddenField ID="hdnDivisionValidationFilter" runat="server" />
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
                                    DataKeyNames="CoordinatorCode"
                                    OnPreRender="GrvList_PreRender" OnSorting="GrvList_Sorting" OnRowDataBound="GrvList_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle hidden" HeaderStyle-CssClass="hidden" FooterStyle-CssClass="hidden">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="InitiativeCoordinatorCodeSort" runat="server" CommandName="Sort" CommandArgument="CoordinatorCode" OnClientClick="SetWaitingGrvList(true);">
                                                        <span><%= GetLocalResourceObject("InitiativeCoordinatorCode.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "InitiativeCoordinatorCode") %> sorterDirection' aria-hidden="true" style="float: right; margin-right: -6px; margin-top: 4px; position: relative; z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <span id="dvInitiativeCoordinatorCode" data-id="InitiativeCoordinatorCode" data-value="<%# Eval("CoordinatorCode") %>"><%# Eval("CoordinatorCode") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="InitiativeCoordinatorName" runat="server" CommandName="Sort" CommandArgument="CoordinatorName" OnClientClick="SetWaitingGrvList(true);">
                                                        <span><%= GetLocalResourceObject("lblCoordinatorName") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "InitiativeCoordinatorDesEnglish") %> sorterDirection' aria-hidden="true" style="float: right; margin-right: -6px; margin-top: 4px; position: relative; z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <span id="dvInitiativeCoordinatorName" data-id="CoordinatorName" data-value="<%# Eval("CoordinatorName") %>"><%# Eval("CoordinatorName")%></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="UserNameSort" runat="server" CommandName="Sort" CommandArgument="UserName" OnClientClick="SetWaitingGrvList(true);">
                                                        <span><%= GetLocalResourceObject("lblUserName") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "UserName") %> sorterDirection' aria-hidden="true" style="float: right; margin-right: -6px; margin-top: 4px; position: relative; z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <span id="dvUserName" data-id="UserName" data-value="<%# Eval("UserName") %>"><%# Eval("UserName")%></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="DivisionNameSort" runat="server" CommandName="Sort" CommandArgument="DivisionName" OnClientClick="SetWaitingGrvList(true);">
                                                        <span><%= GetLocalResourceObject("lblDivision") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "DivisionName") %> sorterDirection' aria-hidden="true" style="float: right; margin-right: -6px; margin-top: 4px; position: relative; z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <span id="dvDivisionName" data-id="DivisionName" data-value="<%# Eval("DivisionName") %>"><%# Eval("DivisionName")%></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="SearchEnabledSort" runat="server" CommandName="Sort" CommandArgument="SearchEnabled" OnClientClick="SetWaitingGrvList(true);">      
                                                        <span><%= GetLocalResourceObject("SearchEnabledLbl") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "SearchEnabled") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <span id="dvSearchEnabled" data-id="SearchEnabled" aria-hidden="true" data-value="<%# Eval("SearchEnabled") %>" class="fa <%# ((DOLE.HRIS.Shared.Entity.InitiativeCoordinatorEntity)Container.DataItem).SearchEnabled ? "fa-check-circle" : "fa-times-circle" %> "></span>
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
                                <asp:BulletedList ID="blstPager" runat="server" CssClass="pagination" DisplayMode="LinkButton" OnClick="BlstPager_Click">
                                </asp:BulletedList>
                            </nav>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="ButtonsActions">
            <asp:UpdatePanel ID="uppActions" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                <Triggers>
                </Triggers>

                <ContentTemplate>
                    <div class="btn-group" role="group" aria-label="main-buttons">
                        <button id="btnAdd" type="button" runat="server" class="btn btn-default btnAjaxAction btns"
                            onclick="return false;"
                            data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnAdd"))%>'
                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnAdd"))%>'>
                            <span class="glyphicon glyphicon-plus glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnAdd") %>
                        </button>

                        <button id="btnEdit" type="button" runat="server" style="display:none" class="btn btn-default btnAjaxAction btns"
                            onserverclick="BtnEdit_ServerClick" onclick="return ProcessEditRequest(this.id);"
                            data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnEdit"))%>'
                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnEdit"))%>'>
                            <span class="glyphicon glyphicon-edit glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnEdit") %>
                        </button>

                        <button id="btnDelete" type="button" runat="server" class="btn btn-default btnAjaxAction btns"
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

    <%--  Modal for Add and Edit  --%>
    <div class="modal fade" id="MaintenanceDialog" tabindex="-1" role="dialog" aria-labelledby="MaintenanceDialogTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" id="btnClose" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title text-primary" id="dialogTitle"></h3>
                </div>

                <asp:UpdatePanel runat="server" ID="uppDialogControl">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="form-horizontal">
                                <asp:HiddenField ID="hdfInitiativeCoordinatorCodeEdit" runat="server" Value="" />

                               <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%=cboDivision.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblDivision")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="cboDivision" CssClass="form-control control-validation selectpicker" AutoPostBack="true" runat="server" data-live-search="true" data-actions-box="true" data-id="Division" data-value="isPermitted" OnSelectedIndexChanged="cboDivision_SelectedIndexChanged"></asp:DropDownList>
                                        <label id="cboDivisionValidation" for="<%= cboDivision.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgDivisionValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                        <asp:HiddenField ID="hdfDivisionValueFilter" runat="server" />
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%=cboUsers.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblUserName")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="cboUsers" CssClass="form-control control-validation selectpicker" AutoPostBack="true" runat="server" data-live-search="true" data-actions-box="true" data-id="Division" data-value="isPermitted" OnSelectedIndexChanged="cboDivision_SelectedIndexChanged"></asp:DropDownList>
                                        <label id="cboUsersValidation" for="<%= cboDivision.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgUserValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                        <asp:HiddenField ID="hdfUsers" runat="server" />
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
                                <div id="divDuplicatedDialogText" runat="server">
                                </div>

                                <asp:Panel ID="pnlDuplicatedDialogDataDetail" runat="server">
                                    <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%=cboDuplicatedDivision.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblDivision")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="cboDuplicatedDivision" CssClass="form-control control-validation selectpicker" AutoPostBack="true" runat="server" data-live-search="true" data-actions-box="true" data-id="Division" data-value="isPermitted" OnSelectedIndexChanged="cboDivision_SelectedIndexChanged"></asp:DropDownList>
                                        <label id="cboDuplicatedDivisionValidation" for="<%= cboDuplicatedDivision.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjCourseValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                        <asp:HiddenField ID="hdnDuplicatedDivision" runat="server" />
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%=cboDuplicatedUsers.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblUserName")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="cboDuplicatedUsers" CssClass="form-control control-validation selectpicker" AutoPostBack="true" runat="server" data-live-search="true" data-actions-box="true" data-id="Division" data-value="isPermitted" OnSelectedIndexChanged="cboDivision_SelectedIndexChanged"></asp:DropDownList>
                                        <label id="cboDuplicatedUsersValidation" for="<%= cboDuplicatedUsers.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjCourseValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                        <asp:HiddenField ID="hndDuplicatedUsers" runat="server" />
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

    <%--  Modal for ActivateDeleted value  --%>
    <div class="modal fade" id="ActivateDeletedDialog" tabindex="-1" role="dialog" aria-labelledby="ActivateDeletedDialogTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button id="btnActivateDeletedClose" type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title text-primary"><%=Convert.ToString(GetLocalResourceObject("TitleActivateDeletedDialog")) %></h3>
                </div>

                <asp:UpdatePanel runat="server" ID="uppActivateDeletedDialog">
                    <Triggers>
                    </Triggers>
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="form-horizontal">
                                <%=Convert.ToString(GetLocalResourceObject("lblTextActivateDeletedDialog")) %>
                                  <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%=cboDeletedDivision.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblDivision")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="cboDeletedDivision" CssClass="form-control control-validation selectpicker" AutoPostBack="true" runat="server" data-live-search="true" data-actions-box="true" data-id="Division" data-value="isPermitted" OnSelectedIndexChanged="cboDivision_SelectedIndexChanged"></asp:DropDownList>
                                        <label id="cboDeletedDivisionValidation" for="<%= cboDeletedDivision.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjCourseValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                        <asp:HiddenField ID="hdnDeletedDivision" runat="server" />
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%=cboDeletedUsers.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblUserName")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="cboDeletedUsers" CssClass="form-control control-validation selectpicker" AutoPostBack="true" runat="server" data-live-search="true" data-actions-box="true" data-id="Division" data-value="isPermitted" OnSelectedIndexChanged="cboDivision_SelectedIndexChanged"></asp:DropDownList>
                                        <label id="cboDeletedUsersValidation" for="<%= cboDeletedUsers.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjCourseValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                        <asp:HiddenField ID="hdnDeletedUsers" runat="server" />
                                    </div>
                                </div> 
                            </div>
                        </div>

                        <div class="modal-footer">
                            <div class="form-horizontal">
                                <%=Convert.ToString(GetLocalResourceObject("lblTextActionActivateDeletedDialog")) %>
                                <div class="form-group">
                                    <div class="col-sm-2">
                                        <asp:CheckBox ID="chkActivateDeleted" CssClass="groupedCheckbox" runat="server" Checked="true" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                    </div>

                                    <div class="col-sm-10 text-left">
                                        <label for="<%= chkActivateDeleted.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblActivateDeleted")%></label>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-2">
                                        <asp:CheckBox ID="chkUpdateActivateDeleted" CssClass="groupedCheckbox" runat="server" Checked="false" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                    </div>
                                    <div class="col-sm-10 text-left">
                                        <label for="<%= chkUpdateActivateDeleted.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblUpdateActivateDeleted")%></label>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <br />

                            <button id="btnActivateDeletedAccept" type="button" runat="server" class="btn btn-primary btnAjaxAction"
                                onserverclick="BtnActivateDeletedAccept_ServerClick"
                                data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>'
                                data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>'>
                                <span class="glyphicon glyphicon-ok glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnDialogSave")) %>
                            </button>

                            <button id="btnActivateDeletedCancel" type="button" class="btn btn-default">
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
                <div class="alert alert-autocloseable-msg" style="display: none;"></div>
            </b>
        </div>
    </nav>

    <script type="text/javascript">
        //*******************************//
        //          VARIABLES            // 
        //*******************************//
        var dataSortAttribute, dataSortType, dataSortDirection;

        //*******************************//
        //       EVENT BINDING           // 
        //*******************************//
        function pageLoad(sender, args) {
            var CultureGlobal = "<%= CultureGlobal %>";
            //In this section we initialize the textbox relations the culture
            /*if (CultureGlobal == "en-US") {
                $(".ES").remove();
            }
            else if (CultureGlobal == "es-CR") {
                $(".EN").remove();
            }*/
            //////////////////////////////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////////////////////////////
            //In this section we initialize the checkbox toogles
            

            $('#<%= chkActivateDeleted.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });

            $('#<%= chkUpdateActivateDeleted.ClientID %>').bootstrapToggle({
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

            $('#<%= btnSearch.ClientID %>').on('click', function (event) {
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
                $("#<%=hdfInitiativeCoordinatorCodeEdit.ClientID%>").val("-1");
                event.preventDefault();
                $('#MaintenanceDialog').modal('show');
                $('#ActivateDeletedDialog').modal('hide');
            });

            $('#btnDuplicatedAccept, #btnDuplicatedClose').on('click', function (event) {
                /// <summary>Handles the click event for button accept in user dialog.</summary>            
                event.preventDefault();
                $('#MaintenanceDialog').modal('show');
                $('#DuplicatedDialog').modal('hide');
            });
            //////////////////////////////////////////////////////////////////////////////////////////////////

            //In this section we set the others controls functionality 
            $('#<%= btnAdd.ClientID %>').on('click', function (ev) {
                /// <summary>Handles the click event for button add.</summary>
                ev.preventDefault();

                setTimeout(function () {
                    $("#<%=btnAdd.ClientID%>").button('reset');
                }, 500);

                DisableToolBar();
                ClearModalForm();

                $("#<%=hdfInitiativeCoordinatorCodeEdit.ClientID%>").val("-1");

                $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceAdd") %>');
                $('#dialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceAdd") %>');
                $('#MaintenanceDialog').modal('show');

                EnableToolBar();
                return false;
            });

            $('#<%= txtInitiativeCoordinatorDesSpanishFilter.ClientID %>').on('keyup keypress', function (e) {
                ReturnFromBtnSearchClickPostBack(e);

                return isNumberOrLetter(e);
            });
            

            $('#<%= chkActivateDeleted.ClientID %>').on('change', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>                            
                setTimeout(function (e) {
                    if ($('#<%= chkActivateDeleted.ClientID %>').is(":checked")) {
                        if ($('#<%= chkUpdateActivateDeleted.ClientID %>').is(":checked")) {
                            $('#<%= chkUpdateActivateDeleted.ClientID %>').prop('checked', false).change();
                        }
                    }
                    else {
                        if (!$('#<%= chkUpdateActivateDeleted.ClientID %>').is(":checked")) {
                            $('#<%= chkUpdateActivateDeleted.ClientID %>').prop('checked', true).change();
                        }
                    }
                }, 50);
            });

            $('#<%= chkUpdateActivateDeleted.ClientID %>').on('change', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>                            
                setTimeout(function (e) {
                    if ($('#<%= chkUpdateActivateDeleted.ClientID %>').is(":checked")) {
                        if ($('#<%= chkActivateDeleted.ClientID %>').is(":checked")) {
                            $('#<%= chkActivateDeleted.ClientID %>').prop('checked', false).change();
                        }
                    }
                    else {
                        if (!$('#<%= chkActivateDeleted.ClientID %>').is(":checked")) {
                            $('#<%= chkActivateDeleted.ClientID %>').prop('checked', true).change();
                        }
                    }
                }, 50);
            });

            $('.cboAjaxAction').on('change', function () {
                var $this = $(this);
                $this.siblings(".waitingNotification").show();
                SetWaitingGrvList(true);
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
        }

        //*******************************//
        //          VALIDATION           // 
        //*******************************//
        var validator = null;
        function ValidateForm() {
            jQuery.validator.addMethod("validSelect", function (value, element) {
                return ValidateSelect(value);
            }, "Please Select a valid information");
            /// <summary>Validate the form with jquey validation plugin </summary>
            /// <returns> True if form is valid. False otherwise. </returns>            
            if (validator == null) {
                //add custom validation methods
                //declare the validator
                validator =
                    $('#' + document.forms[0].id).validate({
                        debug: true,
                        ignore: ".ignoreValidation, :hidden",
                        highlight: function (element, errorClass, validClass) {
                            SetControlInvalid($(element).attr('id'));
                        },
                        unhighlight: function (element, errorClass, validClass) {
                            SetControlValid($(element).attr('id'));
                        },
                        errorPlacement: function (error, element) { },
                        rules: {
                            "<%= cboDivision.UniqueID %>": {
                                required: true,
                                validSelect: true
                            },
                            "<%= cboUsers.UniqueID %>": {
                                required: true,
                                validSelect: true
                            }
                        }
                    });
            }
            //get the results            
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
            disableButton($('#btnAccept'));
            disableButton($('#btnCancel'));
        }

        function EnableButtonsDialog() {
            ///<summary>Enabled all buttons toolbar</summary>
            enableButton($('#btnAccept'));
            enableButton($('#btnCancel'));
        }

        function ClearModalForm() {
            /// <summary>Clear the modal form</summary>                        
            $("#<%=hdfInitiativeCoordinatorCodeEdit.ClientID%>").val("-1");
            $("#<%=cboDivision.ClientID%>").val("-1");
            $("#<%= cboDivision.ClientID %>").selectpicker('refresh');
            $("#<%=cboUsers.ClientID%>").val("-1");
            $("#<%= cboUsers.ClientID %>").selectpicker('refresh');
        }

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

        function CloseUserDialog() {
            /// <summary>Close a user dialog.</summary>
            $('#MaintenanceDialog').modal('hide');
        }

        function ValidateSelect(value) {
            if (value != "-1") {
                return true;
            }
            return false;
        }

        //*******************************//
        //           PROCESS             //
        //*******************************//
        function ProcessEditRequest(resetId) {
            /// <summary>Process the edit request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param> 
            $(".btns").prop("disabled", true);

            if (IsRowSelected()) {
                __doPostBack('<%= btnEdit.UniqueID %>', '');
                return true;
            }

            else {
                ShowFooterError('<%= GetLocalResourceObject("msgInvalidSelection") %>');
                ErrorButton(resetId);
                $(".btns").prop("disabled", false);
                return false;
            }
        }

        function ProcessAcceptRequest(resetId) {
            /// <summary>Process the accept request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            disableButton($('#btnCancel'));

            if (!ValidateForm()) {
                setTimeout(function () {
                    ResetButton(resetId);
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

        //*******************************//
        //  RETURN FROM POSTBACKS LOGIC  // 
        //*******************************//        
        function ReturnFromBtnEditClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $(".btns").prop("disabled", true);

            SetRowSelected();
            DisableToolBar();

            $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#dialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#MaintenanceDialog').modal('show');

            EnableToolBar();
            $(".btns").prop("disabled", false);
        }

        function ReturnFromBtnAcceptClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            DisableButtonsDialog();
            SetRowSelected();

            $('#MaintenanceDialog').modal('hide');

            EnableButtonsDialog();

            ShowFooterSuccess('<%= GetLocalResourceObject("msgOperationSuccesfull") %>');
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
                '<%=GetLocalResourceObject("Yes")%>',
                function () {
                    __doPostBack('<%= btnDelete.UniqueID %>', '');
                }, '<%=GetLocalResourceObject("No")%>',
                function () {
                    $("#" + resetId).button('reset');
                }
            );

            return false;
        }

        //*******************************//
        //AJAX CONCURRENCY ADMINISTRATION// 
        //*******************************//
        //In this section we set the control for multiple simultaneous ajax request
        //We cancel the ajax request executed when another one is in progress
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(initializeRequest);
        prm.add_endRequest(endingRequest);

        function initializeRequest(sender, args) {
            if (prm.get_isInAsyncPostBack()) {
                args.set_cancel(true);
                prm.abortPostBack();

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
