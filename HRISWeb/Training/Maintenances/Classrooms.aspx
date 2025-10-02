<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Classrooms.aspx.cs" Inherits="HRISWeb.Training.Classrooms" %>

<%@ Register Src="~/UserControls/Classroom.ascx" TagPrefix="uc1" TagName="Classroom" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntHeader" runat="server">
    <title><%=Convert.ToString(GetLocalResourceObject("lblScreenTitle")) %></title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cntBody" runat="server">
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
                            <div class="col-sm-6">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%= txtClassroomCodeFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblClassroomCode")%></label>
                                        </div>

                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtClassroomCodeFilter" CssClass="form-control control-validation cleanPasteText ignoreValidation" runat="server" MaxLength="10" autocomplete="off" type="text" data-id="ClassroomCode" data-value="isPermitted"></asp:TextBox>
                                            <asp:HiddenField ID="hdfClassroomCodeFilter" runat="server" />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%= txtClassroomDescriptionFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblClassroomDescription")%></label>
                                        </div>

                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtClassroomDescriptionFilter" CssClass="form-control control-validation cleanPasteText ignoreValidation" runat="server" MaxLength="500" autocomplete="off" type="text" data-id="ClassroomDescription" data-value="isPermitted"></asp:TextBox>
                                            <asp:HiddenField ID="hdfClassroomDescriptionFilter" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-6">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=cboTrainingCenterFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblTrainingCenter")%></label>
                                        </div>

                                        <div class="col-sm-7">
                                            <asp:DropDownList ID="cboTrainingCenterFilter" CssClass="form-control cboAjaxAction control-validation ignoreValidation" AutoPostBack="false" runat="server" data-id="TrainingCenter" data-value="isPermitted"></asp:DropDownList>
                                            <asp:HiddenField ID="hdfTrainingCenterValueFilter" runat="server" />
                                            <asp:HiddenField ID="hdfTrainingCenterTextFilter" runat="server" />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%= txtMinCapacityFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCapacityRange")%></label>
                                        </div>

                                        <div class="col-sm-7">
                                            <div class="input-group" style="width: 100%;">
                                                <asp:TextBox ID="txtMinCapacityFilter" CssClass="form-control control-validation cleanPasteDigits ignoreValidation" runat="server" autocomplete="off" type="number" min="0" max="100" MaxLength="3" Style="display: inline; width: 45%; float: left;" data-id="MinCapacity" data-value="isPermitted"></asp:TextBox>
                                                <asp:HiddenField ID="hdfMinCapacityFilter" runat="server" />
                                                
                                                <asp:TextBox ID="txtMaxCapacityFilter" CssClass="form-control control-validation cleanPasteDigits ignoreValidation" runat="server" autocomplete="off" type="number" min="0" max="100" MaxLength="3" Style="display: inline; width: 45%; float: right;" data-id="MaxCapacity" data-value="isPermitted"></asp:TextBox>
                                                <asp:HiddenField ID="hdfMaxCapacityFilter" runat="server" />

                                                <div style="margin-left: auto; margin-right: auto; text-align: center;">-</div>
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
                                        DataKeyNames="GeographicDivisionCode, ClassroomCode"
                                        OnPreRender="GrvList_PreRender" OnSorting="GrvList_Sorting">

                                        <Columns>
                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="ClassroomCodeSort" runat="server" CommandName="Sort" CommandArgument="ClassroomCode" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("ClassroomCode.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "ClassroomCode") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvClassroomCode" data-id="ClassroomCode" data-value="<%# Eval("ClassroomCode") %>"><%# Eval("ClassroomCode") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="ClassroomDescriptionSort" runat="server" CommandName="Sort" CommandArgument="ClassroomDescription" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("ClassroomDescription.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "ClassroomDescription") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvClassroomDescription" style="overflow-wrap: break-word; word-wrap: break-word; word-break: break-all;" data-id="ClassroomDescription" data-value="<%# Eval("ClassroomDescription") %>"><%# Eval("ClassroomDescription") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="TrainingCenterCodeSort" runat="server" CommandName="Sort" CommandArgument="TrainingCenterCode" OnClientClick="SetWaitingGrvList(true);">                
                                                            <span><%= GetLocalResourceObject("TrainingCenterCode.HeaderText") %></span><i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "TrainingCenterCode") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvTrainingCenterCode" data-id="TrainingCenterCode" data-value="<%# DataBinder.Eval(Container.DataItem, "TrainingCenter.TrainingCenterCode") %>"><%# DataBinder.Eval(Container.DataItem, "TrainingCenter.TrainingCenterCode") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="TrainingCenterDescriptionSort" runat="server" CommandName="Sort" CommandArgument="TrainingCenterDescription" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("TrainingCenterDescription.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "TrainingCenterDescription") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvTrainingCenterDescription" data-id="TrainingCenterDescription" data-value="<%# DataBinder.Eval(Container.DataItem, "TrainingCenter.TrainingCenterDescription") %>"><%# DataBinder.Eval(Container.DataItem, "TrainingCenter.TrainingCenterDescription") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="CapacitySort" runat="server" CommandName="Sort" CommandArgument="Capacity" OnClientClick="SetWaitingGrvList(true);">      
                                                            <span><%= GetLocalResourceObject("Capacity.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "Capacity") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvCapacity" data-id="Capacity" data-value="<%# Eval("Capacity") %>"><%# Eval("Capacity") %></span>
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
                        <button id="btnAdd" type="button" runat="server" class="btn btn-default btnAjaxAction btns"
                            data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnAdd"))%>'
                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnAdd"))%>'>
                            <span class="glyphicon glyphicon-plus glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnAdd") %>
                        </button>

                        <button id="btnEdit" type="button" runat="server" class="btn btn-default btnAjaxAction btns"
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
                        <uc1:Classroom runat="server" ID="ucClassroom" />

                        <div class="modal-footer">
                            <button id="btnAccept" type="button" runat="server" class="btn btn-primary btnAjaxAction btnAccept" 
                                onserverclick="BtnAccept_ServerClick" onclick="return ProcessAddClassroomRequest(this.id);" 
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
                                <asp:HiddenField ID="hdfDuplicatedClassroomGeographicDivisionCode" runat="server" Value="" />
                                <asp:HiddenField ID="hdfDuplicatedClassroomCode" runat="server" Value="" />

                                <div id="divDuplicatedDialogText" runat="server"></div>

                                <asp:Panel ID="pnlDuplicatedDialogDataDetail" runat="server">
                                    <div class="form-group">
                                        <div class="col-sm-4 text-left">
                                            <label for="<%= txtDuplicatedClassroomCode.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblClassroomCode")%></label>
                                        </div>

                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtDuplicatedClassroomCode" CssClass="form-control cleanPasteText ignoreValidation" runat="server" onkeypress="return isNumberOrLetter(event)" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-4 text-left">
                                            <label for="<%= txtDuplicatedClassroomDescription.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblClassroomDescription")%></label>
                                        </div>

                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtDuplicatedClassroomDescription" CssClass="form-control cleanPasteText ignoreValidation" runat="server" onkeypress="return isNumberOrLetter(event) && checkMaxLength(this,event,500);" onkeyUp="return isNumberOrLetter(event) && checkMaxLength(this,event,500);" MaxLength="500" TextMode="MultiLine" Columns="3" ReadOnly="true"></asp:TextBox>
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
                    <h3 class="modal-title text-primary"><%= Convert.ToString(GetLocalResourceObject("TitleActivateDeletedDialog")) %></h3>
                </div>

                <asp:UpdatePanel runat="server" ID="uppActivateDeletedDialog">
                    <Triggers>
                    </Triggers>

                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="form-horizontal">
                                <asp:HiddenField ID="hdfActivateDeletedClassroomGeographicDivisionCode" runat="server" Value="" />
                                <asp:HiddenField ID="hdfActivateDeletedClassroomCode" runat="server" Value="" />

                                <div id="divActivateDeletedDialog" runat="server"></div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtActivateDeletedClassroomCode.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblClassroomCode")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtActivateDeletedClassroomCode" CssClass="form-control cleanPasteText ignoreValidation" runat="server" onkeypress="return isNumberOrLetter(event)" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtActivateDeletedClassroomDescription.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblClassroomDescription")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtActivateDeletedClassroomDescription" CssClass="form-control cleanPasteText ignoreValidation" runat="server" onkeypress="return isNumberOrLetter(event) && checkMaxLength(this,event,500);" onkeyUp="return isNumberOrLetter(event) && checkMaxLength(this,event,500);" MaxLength="500" TextMode="MultiLine" Columns="3" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="modal-footer">
                            <div class="form-horizontal">
                                <%=Convert.ToString(GetLocalResourceObject("lblTextActionActivateDeletedDialog")) %>

                                <div class="form-group">
                                    <div class="col-sm-2">
                                        <asp:CheckBox ID="chbActivateDeleted" CssClass="groupedCheckbox" runat="server" Checked="true" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                    </div>

                                    <div class="col-sm-10 text-left">
                                        <label for="<%= chbActivateDeleted.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblActivateDeleted")%></label>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-2">
                                        <asp:CheckBox ID="chbUpdateActivateDeleted" CssClass="groupedCheckbox" runat="server" Checked="false" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                    </div>

                                    <div class="col-sm-10 text-left">
                                        <label for="<%= chbUpdateActivateDeleted.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblUpdateActivateDeleted")%></label>
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
                <div class="alert alert-autocloseable-msg" style="display: none;">
                </div>
            </b>
        </div>
    </nav>

    <%--  Modal  --%>
    <script type="text/javascript">    
        //*******************************//
        //          VARIABLES            // 
        //*******************************//
        var dataSortAttribute, dataSortType, dataSortDirection;

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
            $('#<%= ucClassroom.chbSearchEnabled.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });

            $('#<%= chbActivateDeleted.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });

            $('#<%= chbUpdateActivateDeleted.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });
            //////////////////////////////////////////////////////////////////////////////////////////////////

            //In this section we set the button state for btnAjaxAction
            $('.btnAjaxAction').on('click', function () {
                var $this = $(this);
                $this.button('loading');
                setTimeout(function () { $this.button('reset'); }, 50000);
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

                ClearModalFormClassroom();

                DisableButtonsDialog();
                $('#MaintenanceDialog').modal('hide');
                EnableButtonsDialog();
            });

            $('#btnActivateDeletedCancel, #btnActivateDeletedClose').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                event.preventDefault();
                $('#MaintenanceDialog').modal('show');
                $("#<%=ucClassroom.txtClassroomCode.ClientID%>").prop("disabled", false);
                $('#ActivateDeletedDialog').modal('hide');
            });

            $('#btnDuplicatedAccept, #btnDuplicatedClose').on('click', function (event) {
                /// <summary>Handles the click event for button accept in user dialog.</summary>            
                event.preventDefault();

                $('#MaintenanceDialog').modal('show');
                $("#<%=ucClassroom.txtClassroomCode.ClientID%>").prop("disabled", false);
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
                ClearModalFormClassroom();

                $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceAdd") %>');
                $('#dialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceAdd") %>');
                $('#MaintenanceDialog').modal('show');

                EnableToolBar();

                return false;
            });

            $('#<%= txtClassroomCodeFilter.ClientID %>').on('keyup keypress', function (e) {
                ReturnFromBtnSearchClickPostBack(e);

                return isNumberOrLetter(e);
            });

            $('#<%= txtClassroomDescriptionFilter.ClientID %>').on('keyup keypress', function (e) {
                ReturnFromBtnSearchClickPostBack(e);

                return isNumberOrLetter(e);
            });

            $('#<%= cboTrainingCenterFilter.ClientID %>').on('keyup keypress', function (e) {
                ReturnFromBtnSearchClickPostBack(e);
            });

            $('#<%= txtMinCapacityFilter.ClientID %>').on('keyup keypress', function (e) {
                ReturnFromBtnSearchClickPostBack(e);

                return isNumber(e) && checkMaxLength(this, e, 3);
            });

            $('#<%= txtMaxCapacityFilter.ClientID %>').on('keyup keypress', function (e) {
                ReturnFromBtnSearchClickPostBack(e);

                return isNumber(e) && checkMaxLength(this, e, 3);
            });

            $("#<%= ucClassroom.txtComments.ClientID %>").change(function () {
                $(this).val($.trim($(this).val()))
            });

            $('#<%= chbActivateDeleted.ClientID %>').on('change', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>                            
                setTimeout(function (e) {
                    if ($('#<%= chbActivateDeleted.ClientID %>').is(":checked")) {
                        if ($('#<%= chbUpdateActivateDeleted.ClientID %>').is(":checked")) {
                            $('#<%= chbUpdateActivateDeleted.ClientID %>').prop('checked', false).change();
                        }
                    }
                    else {
                        if (!$('#<%= chbUpdateActivateDeleted.ClientID %>').is(":checked")) {
                            $('#<%= chbUpdateActivateDeleted.ClientID %>').prop('checked', true).change();
                        }
                    }
                }, 50);
            });

            $('#<%= chbUpdateActivateDeleted.ClientID %>').on('change', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>                            
                setTimeout(function (e) {
                    if ($('#<%= chbUpdateActivateDeleted.ClientID %>').is(":checked")) {
                        if ($('#<%= chbActivateDeleted.ClientID %>').is(":checked")) {
                            $('#<%= chbActivateDeleted.ClientID %>').prop('checked', false).change();
                        }
                    }
                    else {
                        if (!$('#<%= chbActivateDeleted.ClientID %>').is(":checked")) {
                            $('#<%= chbActivateDeleted.ClientID %>').prop('checked', true).change();
                        }
                    }
                }, 50);
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
            })

            SetRowSelected();
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
            DisableToolBar();

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
            EnableToolBar();
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

        function ClearModalFormClassroom() {
            /// <summary>Clear the modal form</summary>                       
            $("#<%=ucClassroom.hdfClassroomCodeEdit.ClientID%>").val("");
            $("#<%=ucClassroom.txtClassroomCode.ClientID%>").val("");
            $("#<%=ucClassroom.txtClassroomCode.ClientID%>").removeAttr("disabled");
            $("#<%=ucClassroom.txtClassroomDescription.ClientID%>").val("");
            $("#<%=ucClassroom.cboTrainingCenter.ClientID%>").val("-1");
            $("#<%=ucClassroom.txtCapacity.ClientID%>").val("0");
            $("#<%=ucClassroom.txtComments.ClientID%>").val("");
            $("#<%=ucClassroom.chbSearchEnabled.ClientID%>").bootstrapToggle('<%= GetLocalResourceObject("Yes") %>');

            if (validator != null) {
                validator.resetForm();
            }
        }

        function CloseUserDialog() {
            /// <summary>Close a user dialog.</summary>
            $('#MaintenanceDialog').modal('hide');
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

        function ReturnFromBtnEditClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $('#MaintenanceDialog').modal('show');
            SetRowSelected();
            DisableToolBar();

            $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#dialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');

            $("#<%=ucClassroom.txtClassroomCode.ClientID%>").attr("disabled", "disabled");

            EnableToolBar();
        }

        function ReturnFromBtnAcceptClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            DisableButtonsDialog();
            SetRowSelected();

            $('#MaintenanceDialog').modal('hide');

            ShowFooterSuccess('<%= GetLocalResourceObject("msgOperationSuccesfull") %>');
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
            },
                '<%=GetLocalResourceObject("No")%>', function () {
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
