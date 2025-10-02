<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="TrainingPlan.aspx.cs" Inherits="HRISWeb.Training.Maintenances.TrainingPlan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntHeader" runat="server">
    <title><%=Convert.ToString(GetLocalResourceObject("lblScreenTitle")) %></title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cntBody" runat="server">
    <style>
        .bootstrap-select > .dropdown-toggle.bs-placeholder, .bootstrap-select > .dropdown-toggle.bs-placeholder:active,
        .bootstrap-select > .dropdown-toggle.bs-placeholder:focus, .bootstrap-select > .dropdown-toggle.bs-placeholder:hover {
            color: #333;
        }

        .redcolor {
            color: red;
        }

        .greencolor {
            color: forestgreen;
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
                                        <div class="form-group col-sm-12 col-md-4">
                                            <div class="col-sm-5 text-left">
                                                <label for="<%= MasterProgramCodeFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("CodeLbl")%></label>
                                            </div>

                                            <div class="col-sm-7">
                                                <input id="MasterProgramCodeFilter" type="text" class="form-control" runat="server" data-id="MasterProgramCode" data-value="isPermitted" />
                                            </div>
                                        </div>

                                        <div class="form-group col-sm-12 col-md-4">
                                            <div class="col-sm-5 text-left">
                                                <label for="<%= MasterProgramNameFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("NameLbl")%></label>
                                            </div>

                                            <div class="col-sm-7">
                                                <input id="MasterProgramNameFilter" type="text" class="form-control" runat="server" data-id="MasterProgramName" data-value="isPermitted" />
                                            </div>
                                        </div>

                                        <div class="form-group col-sm-12 col-md-4">
                                            <div class="col-sm-5 text-left">
                                                <label for="<%= MatrixTargetNameFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblMatrixTarget")%></label>
                                            </div>

                                            <div class="col-sm-7">
                                                <select class="form-control limpiarCampos enterkey" runat="server" id="MatrixTargetNameFilter" data-id="MatrixTargetName" data-value="isPermitted" data-live-search="true" data-actions-box="true"></select>
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
                                        DataKeyNames="MasterProgramId"
                                        OnPreRender="GrvList_PreRender" OnSorting="GrvList_Sorting">
                                        <Columns>
                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="MasterProgramCodeSort" runat="server" CommandName="Sort" CommandArgument="MasterProgramCode"
                                                            OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("CodeLbl") %></span>&nbsp;
                                                            <i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "MasterProgramCode") %> sorterDirection' 
                                                                aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvMasterProgramCode" style="word-wrap: break-word; word-break: break-all;" data-id="MasterProgramCode" data-value="<%# Eval("MasterProgramCode") %>"><%# Eval("MasterProgramCode") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="MasterProgramNameSort" runat="server" CommandName="Sort" CommandArgument="MasterProgramName" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("NameLbl") %></span>&nbsp;
                                                            <i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "MasterProgramName") %> sorterDirection'
                                                                 aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvMasterProgramName" style="overflow-wrap: break-word; word-wrap: break-word; word-break: break-all;" data-id="MasterProgramName" data-value="<%# Eval("MasterProgramName") %>"><%# Eval("MasterProgramName") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="MatrixTargetNameSort" runat="server" CommandName="Sort" CommandArgument="MatrixTargetName" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("lblMatrixTarget") %></span>&nbsp;
                                                            <i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "MatrixTargetName") %> sorterDirection'
                                                                 aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvMatrixTargetName" data-id="MatrixTargetName" data-value="<%# Eval("MatrixTargetName") %>"><%# Eval("MatrixTargetName") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="IsExpirationSort" runat="server" CommandName="Sort" CommandArgument="IsExpiration" OnClientClick="SetWaitingGrvList(true);">      
                                                        <span><%=GetLocalResourceObject("LblExpiration") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "IsExpiration") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvIsExpiration" data-id="IsExpiration" aria-hidden="true" data-value="<%# Eval("IsExpiration") %>" class="fa <%# ((DOLE.HRIS.Shared.Entity.MasterProgramEntity)Container.DataItem).IsExpiration ? "fa-check-circle greencolor" : "fa-times-circle redcolor" %> "></span>
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
                            onserverclick="BtnAdd_ServerClick" onclick="return ProcessAddRequest(this.id);"
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

                        <button id="btnRelationship" type="button" runat="server" class="btn btn-default btnAjaxAction"
                            onserverclick="BtnRelate_ServerClick" onclick="return ProcessRelateRequest(this.id);"
                            data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnRelation"))%>'
                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnRelation"))%>'>
                            <span class="glyphicon glyphicon-link glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnRelation") %>
                        </button>

                        <button id="btnRelatedSummary" type="button" runat="server" class="btn btn-default btnAjaxAction"
                            onserverclick="BtnRelateSummary_ServerClick" onclick="return ProcessRelateSummaryRequest(this.id);"
                            data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("lblRelatedSummary"))%>'
                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("lblRelatedSummary"))%>'>
                            <span class="glyphicon glyphicon-link glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("lblRelatedSummary") %>
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

    <%--  Modal for Add and Edit TrainingPlan --%>
    <div class="modal fade" id="MaintenanceDialog" tabindex="-1" role="dialog" aria-labelledby="MaintenanceDialogTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog " style="width: 60% !important;" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" id="btnClose" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title text-primary" id="dialogTitle"></h3>
                </div>

                <asp:UpdatePanel runat="server" ID="uppDialogControl">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="form-horizontal">
                                <asp:HiddenField ID="hdfMasterProgramIdEdit" runat="server" Value="" />
                                <asp:HiddenField ID="hdfMasterProgramIdEditExisted" runat="server" Value="" />

                                <div class="form-group">
                                    <div class="col-sm-10 text-left">
                                        <label for="<%= ExistingMasterProgramEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblExistingMasterProgram")%></label>
                                    </div>

                                    <div class="col-sm-2">
                                        <asp:CheckBox ID="ExistingMasterProgramEdit" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                    </div>
                                </div>

                                <div class="form-group MasterProgramList">
                                    <div class="col-sm-6 text-left">
                                        <label for="<%=MasterProgramListEdit.ClientID%>" class="control-label" style="text-align: left"><%=GetLocalResourceObject("lblMasterProgramList")%></label>
                                    </div>

                                    <div class="col-sm-6">
                                        <asp:DropDownList ID="MasterProgramListEdit" CssClass="form-control cboAjaxAction control-validation selectpicker" AutoPostBack="true" runat="server" OnSelectedIndexChanged="MasterProgramListEdit_ServerChange" data-live-search="true" data-actions-box="true"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-2 text-left">
                                        <label for="<%=MasterProgramCodeEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("CodeLbl")%></label>
                                    </div>

                                    <div class="col-sm-10">
                                        <input type="text" id="MasterProgramCodeEdit" runat="server" class="form-control limpiarCampos cleanPasteText ValidacionRegional enterkey" onkeypress="return isNumberOrLetter(event);" onkeyup="return isNumberOrLetter(event);" maxlength="10" value="" required />
                                       
                                        <label id="MasterProgramCodeEditValidation" for="<%= MasterProgramCodeEdit.ClientID%>" 
                                            class="label label-danger label-validation"
                                            data-toggle="tooltip" data-container="body" data-placement="left"
                                            data-content="<%= GetLocalResourceObject("msjMatrixTargetCodeEditValidation") %>"
                                            style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-2 text-left">
                                        <label for="<%=MasterProgramNameEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("NameLbl")%></label>
                                    </div>

                                    <div class="col-sm-10">
                                        <input type="text" id="MasterProgramNameEdit" runat="server" class="form-control limpiarCampos cleanPasteText ValidacionRegional enterkey" onkeypress="return isNumberOrLetter(event);" onkeyup="return isNumberOrLetter(event);" maxlength="100" value="" required />
                                        
                                        <label id="MasterProgramNameEditValidation" for="<%= MasterProgramNameEdit.ClientID%>" 
                                            class="label label-danger label-validation"
                                            data-toggle="tooltip" data-container="body" data-placement="left"
                                            data-content="<%= GetLocalResourceObject("msjMatrixTargetNameEditValidation") %>"
                                            style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-2 text-left">
                                        <label for="<%=MatrixTargetIdEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblMatrixTarget")%></label>
                                    </div>

                                    <div class="col-sm-10">
                                        <select class="form-control limpiarCampos enterkey" runat="server" id="MatrixTargetIdEdit" data-live-search="true" data-actions-box="true"></select>
                                        
                                        <label id="MatrixTargetIdEditValidation" for="<%= MatrixTargetIdEdit.ClientID%>" 
                                            class="label label-danger label-validation"
                                            data-toggle="tooltip" data-container="body" data-placement="left"
                                            data-content="<%= GetLocalResourceObject("msjSelectEditValidation") %>"
                                            style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-2 text-left">
                                        <label for="<%=TrainingPlanProgramEdit.ClientID%>" class="control-label text-left" style="text-align: left"><%=GetLocalResourceObject("lblTrainingPlanProgram")%></label>
                                    </div>

                                    <div class="col-sm-10">
                                        <select class="form-control limpiarCampos enterkey" runat="server" id="TrainingPlanProgramEdit" data-live-search="true" data-actions-box="true"></select>
                                       
                                        <label id="TrainingPlanProgramEditValidation" for="<%= TrainingPlanProgramEdit.ClientID%>" 
                                            class="label label-danger label-validation"
                                            data-toggle="tooltip" data-container="body" data-placement="left"
                                            data-content="<%= GetLocalResourceObject("msjSelectEditValidation") %>"
                                            style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= IsExpirationEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("LblExpiration")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:CheckBox ID="IsExpirationEdit" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                    </div>
                                </div>

                                <div class="form-group IsExpirationEdit">
                                    <div class="col-sm-2 text-left">
                                        <label for="<%=CycleTrainingIdEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCycleTranningEdit")%></label>
                                    </div>

                                    <div class="col-sm-10">
                                        <select class="form-control selectpicker onChangeCycleTrainingIdEdit" runat="server" id="CycleTrainingIdEdit" multiple="false" data-live-search="true" data-actions-box="true"></select>
                                    </div> 
                                </div>

                                <div class="form-group IsExpirationEdit">
                                    <div class="col-sm-2 text-left">
                                        <label for="<%= ApplyRuleRecruitmentDateEdit.ClientID%>" class="control-label" style="text-align:left"><%=GetLocalResourceObject("lblApplyRuleRecruitmentDate")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:CheckBox ID="ApplyRuleRecruitmentDateEdit" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                    </div>
                                </div>

                                <div class="form-group ApplyRuleRecruitmentDate">
                                    <div class="col-sm-2 text-left">
                                        <label for="<%=FromDateEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("LblFromDate")%></label>
                                    </div>

                                    <div class="col-sm-10">
                                        <input type="text" id="FromDateEdit" runat="server" class="enterkey FromDateEdit dateinput form-control date control-validation cleanPasteDigits limpiarCampos" onkeypress="return isNumberOrLetter(event);" onkeyup="return isNumberOrLetter(event);" maxlength="500" value="" required />
                                        <label id="FromDateEditValidation" for="<%= FromDateEdit.ClientID%>" class="label label-danger label-validation"
                                            data-toggle="tooltip" data-container="body" data-placement="left"
                                            data-content="<%= GetLocalResourceObject("msjDateValidation") %>"
                                            style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">
                                            !</label>
                                    </div>
                                </div>

                                <div class="form-group ApplyRuleRecruitmentDate">
                                    <div class="col-sm-2 text-left">
                                        <label for="<%=ToDateEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblToDate")%></label>
                                    </div>

                                    <div class="col-sm-10">
                                        <input type="text" id="ToDateEdit" runat="server" class="enterkey ToDateEdit dateinput form-control date control-validation cleanPasteDigits limpiarCampos" onkeypress="return isNumberOrLetter(event);" onkeyup="return isNumberOrLetter(event);" maxlength="500" value="" required />
                                        <label id="ToDateEditValidation" for="<%= ToDateEdit.ClientID%>" class="label label-danger label-validation"
                                            data-toggle="tooltip" data-container="body" data-placement="left"
                                            data-content="<%= GetLocalResourceObject("msjDateValidation") %>"
                                            style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">
                                            !</label>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= SearchEnabledEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblSearchEnabled")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:CheckBox ID="SearchEnabledEdit" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="modal-footer">
                            <button id="btnAccept" type="button" runat="server" class="btn btn-primary btnAjaxAction btnAccept"
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

    <%--  Modal for Add and Edit Relate --%>
    <div class="modal fade" id="MaintenanceDialogRelate" tabindex="-1" role="dialog" aria-labelledby="MaintenanceDialogTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog " style="width: 60% !important;" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" id="btnCloseRealate" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title text-primary" id="dialogTitleRelate"><%= GetLocalResourceObject("btnRelation") %></h3>
                </div>

                <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="form-horizontal">
                                <asp:HiddenField ID="MasterProgramIdRelate" runat="server" Value="" />

                                <div class="form-group">
                                    <div class="col-sm-2 text-left">
                                        <label for="<%=RelatedByEdit.ClientID%>" class="control-label" style="text-align: left"><%=GetLocalResourceObject("lbRelateBy")%></label>
                                    </div>

                                    <div class="col-sm-10">
                                        <select class="form-control limpiarCampos" runat="server" id="RelatedByEdit" data-live-search="true" data-actions-box="true"></select>

                                        <label id="RelatedByEditValidation" for="<%= RelatedByEdit.ClientID%>" class="label label-danger label-validation"
                                            data-toggle="tooltip" data-container="body" data-placement="left"
                                            data-content="<%= GetLocalResourceObject("msjCompanyIDEditValidation") %>"
                                            style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">
                                            !</label>
                                    </div>
                                </div>

                                <div class="form-group relateBy1">
                                    <div class="col-sm-2 text-left">
                                        <label for="<%=PositionsIdEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblPositions")%></label>
                                    </div>

                                    <div class="col-sm-10">
                                        <select class="form-control selectpicker onChangePositionsIdEdit" runat="server" id="PositionsIdEdit" multiple="true" data-live-search="true" data-actions-box="true"></select>

                                        <input type="hidden" runat="server" class="limpiarCampos" id="PositionsIdEditMultiple" value="" />
                                        <label id="PositionsIdEditValidation" for="<%= PositionsIdEdit.ClientID%>" class="label label-danger label-validation"
                                            data-toggle="tooltip" data-container="body" data-placement="left"
                                            data-content="<%= GetLocalResourceObject("msjSelectEditValidation") %>"
                                            style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">
                                            !</label>
                                    </div>
                                </div>

                                <div class="form-group relateBy1">
                                    <div class="col-sm-2 text-left">
                                        <label for="<%=LaborsIdEdit.ClientID%>" class="control-label">Labor</label>
                                    </div>

                                    <div class="col-sm-10">
                                        <select class="form-control selectpicker onChangeLaborsIdEdit" runat="server" id="LaborsIdEdit" multiple="true" data-live-search="true" data-actions-box="true"></select>

                                        <input type="hidden" runat="server" class="limpiarCampos" id="LaborsIdEditMultiple" value="" />
                                        <label id="LaborsIdEditValidation" for="<%= LaborsIdEdit.ClientID%>" class="label label-danger label-validation"
                                            data-toggle="tooltip" data-container="body" data-placement="left"
                                            data-content="<%= GetLocalResourceObject("msjSelectEditValidation") %>"
                                            style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">
                                            !</label>
                                    </div>
                                </div>

                                <div class="form-group relateBy2">
                                    <div class="col-sm-2 text-left">
                                        <label for="<%=EmployeesIdEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblEmployee")%></label>
                                    </div>

                                    <div class="col-sm-10">
                                        <select class="form-control selectpicker onChangeEmployeesIdEdit" runat="server" id="EmployeesIdEdit" multiple="true"
                                            data-live-search="true" data-actions-box="true">
                                        </select>

                                        <input type="hidden" runat="server" class="limpiarCampos" id="EmployeesIdEditMultiple" value="" />
                                        <label id="EmployeesIdEditValidation" for="<%= EmployeesIdEdit.ClientID%>" class="label label-danger label-validation"
                                            data-toggle="tooltip" data-container="body" data-placement="left"
                                            data-content="<%= GetLocalResourceObject("msjSelectEditValidation") %>"
                                            style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">
                                            !</label>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="modal-footer">
                            <button id="btnAcceptRelate" type="button" runat="server" class="btn btn-primary btnAjaxAction btnAccept"
                                onserverclick="BtnAcceptRelate_ServerClick" onclick="return ProcessAcceptRelateRequest(this.id);"
                                data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>'
                                data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>'>
                                <span class="glyphicon glyphicon-ok glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnDialogSave")) %>
                            </button>
                            <button id="btnCancelRelate" type="button" class="btn btn-default">
                                <span class="glyphicon glyphicon-remove glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnDialogCancel")) %>
                            </button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <%--  Modal for Add and Edit Relate Summary --%>
    <div class="modal fade" id="MaintenanceDialogRelateSummary" tabindex="-1" role="dialog" aria-labelledby="MaintenanceDialogTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog " style="width: 60% !important;" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" id="btnCloseRealateSummary" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title text-primary" id="dialogTitleRelateSummary"><%= GetLocalResourceObject("lblRelatedSummary") %></h3>
                </div>

                <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                    <ContentTemplate>
                        <div class="modal-body">
                            <%if (ViewRelatedSummary == 1) { %>
                                <h3 class="text-primary"><%= GetLocalResourceObject("lblheaderPosition") %></h3>
                                <div class="row" style="overflow-y: scroll; height: 300px;">
                                    <div class="col-sm-12 col-md-12">
                                        <table class="table table-striped table-hover table-bordered">
                                            <thead>
                                                <tr>
                                                    <th><%= GetLocalResourceObject("lblCodigoTable") %></th>
                                                    <th><%= GetLocalResourceObject("lblDescriptionTable") %></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <% foreach (var item in GridRelatedPosition)
                                                    { %>
                                                <tr>
                                                    <td><%=item.PositionCode %></td>
                                                    <td><%=item.PositionName %></td>
                                                </tr>
                                                <% } %>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                                <br />

                                <h3 class="text-primary"><%= GetLocalResourceObject("lblheaderLabor") %></h3>

                                <div class="row" style="overflow-y: scroll; height: 300px;">

                                <div class="col-sm-12 col-md-12">
                                    <table class="table table-striped table-hover table-bordered">
                                        <thead>
                                            <tr>
                                                <th><%= GetLocalResourceObject("lblCodigoTable") %></th>
                                                <th><%= GetLocalResourceObject("lblDescriptionTable") %></th>
                                            </tr>
                                        </thead>

                                        <tbody>
                                            <% foreach (var item in GridRelatedLabor) { %>
                                                <tr>
                                                    <td><%=item.LaborCode %></td>
                                                    <td><%=item.LaborName %></td>
                                                </tr>
                                            <% } %>
                                        </tbody>
                                    </table>
                                </div>
                            </div>

                            <%} else  { %>
                                <h3 class="text-primary"><%= GetLocalResourceObject("lblheaderEmployees") %></h3>
                                <div class="row" style="overflow-y: scroll; height: 45vh;">
                                    <div class="col-sm-12 col-md-12">
                                        <table class="table table-striped table-hover table-bordered">
                                            <thead>
                                                <tr>
                                                    <th><%= GetLocalResourceObject("lblCodigoTable") %></th>
                                                    <th><%= GetLocalResourceObject("lblNameTable") %></th>
                                                </tr>
                                            </thead>

                                            <tbody>
                                                <% foreach (var item in GridRelatedEmployees) { %>
                                                    <tr>
                                                        <td><%=item.EmployeeCode %></td>
                                                        <td><%=item.EmployeeName %></td>
                                                    </tr>
                                                <% } %>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            <% } %>
                        </div>

                        <div class="modal-footer">
                            <button id="btnCancelRelateSummary" type="button" class="btn btn-default">
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
                                            <label for="<%= txtDuplicatedMasterProgramCode.ClientID%>" class="control-label"><%=GetLocalResourceObject("CodeLbl")%></label>
                                        </div>

                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtDuplicatedMasterProgramCode" CssClass="form-control cleanPasteText ignoreValidation" runat="server" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-4 text-left">
                                            <label for="<%= txtDuplicatedMasterProgramName.ClientID%>" class="control-label"><%=GetLocalResourceObject("NameLbl")%></label>
                                        </div>

                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtDuplicatedMasterProgramName" CssClass="form-control cleanPasteText ignoreValidation" runat="server" ReadOnly="true"></asp:TextBox>
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
            $('#<%= ExistingMasterProgramEdit.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });

            $('#<%= SearchEnabledEdit.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });

            $('#<%= IsExpirationEdit.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });

            $('#<%= ApplyRuleRecruitmentDateEdit.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });
            //////////////////////////////////////////////////////////////////////////////////////////////////

            ExistingMasterProgramChange();
            IsExpirationChange();
            ApplyRuleRecruitmentDateChange();
            EnableButtonsDialog();
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

            $('#<%=blstPager.ClientID %>').on('click', 'a', function (event) {
                SetWaitingGrvList(true);
            });

            $('#<%=btnSearchDefault.ClientID %>').on('click', function (event) {
                var $this = $('#<%= btnSearch.ClientID %>');
                $this.button('loading');
                setTimeout(function () { $this.button('reset'); }, 30000);
            });
            //////////////////////////////////////////////////////////////////////////////////////////////////

            //In this section we set up the external modal functionality
            $('#btnCancelRelate, #btnCloseRealate').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                event.preventDefault();

                ClearModalForm();
                DisableButtonsDialog();

                $('#MaintenanceDialogRelate').modal('hide');
                EnableButtonsDialog();
            });

            $('#btnCancelRelateSummary, #btnCloseRealateSummary').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                event.preventDefault();

                ClearModalForm();
                DisableButtonsDialog();

                $('#MaintenanceDialogRelateSummary').modal('hide');
                EnableButtonsDialog();
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
            $('#<%= MasterProgramCodeFilter.ClientID %>').on('keyup keypress', function (e) {
                ReturnFromBtnSearchClickPostBack(e);

                return isNumberOrLetter(e);
            });

            $('#<%= MasterProgramNameFilter.ClientID %>').on('keyup keypress', function (e) {
                ReturnFromBtnSearchClickPostBack(e);

                return isNumberOrLetter(e);
            });

            $('#<%=ExistingMasterProgramEdit.ClientID%>').change(function () {
                ExistingMasterProgramChange();
            });

            $('#<%=IsExpirationEdit.ClientID%>').change(function () {
                IsExpirationChange();
            });

            $('#<%=CycleTrainingIdEdit.ClientID%>').change(function (e) {
                if (e.currentTarget.value == "") {
                    $('.dateinput').val('');
                    return;
                }
                
                let cycleTraining = e.currentTarget.value;
                let cycleTrainingArray = cycleTraining.split('|');

                RecruitmentDate(cycleTrainingArray[1]);
            });

            $('#<%=ApplyRuleRecruitmentDateEdit.ClientID%>').change(function () {
                ApplyRuleRecruitmentDateChange();
            });

            $('#<%= FromDateEdit.ClientID %>').on("blur", function () {
                var ValidDateVal = $(this).data("DateTimePicker").date().format('MM/DD/YYYY');
                $(this).val(ValidDateVal);
            });

            $('#<%= ToDateEdit.ClientID %>').on("blur", function () {
                var ValidDateVal = $(this).data("DateTimePicker").date().format('MM/DD/YYYY');
                $(this).val(ValidDateVal);
            });

            $("#<%=RelatedByEdit.ClientID%>").change(function () {
                if ($(this).val() == "1") {
                    $(".relateBy1").show();
                    $(".relateBy2").hide();
                } else {
                    $(".relateBy1").hide();
                    $(".relateBy2").show();
                }
            });

            $(".onChangePositionsIdEdit").on("changed.bs.select", function () {
                MultiSelectDropdownListSaveSelectedItems($("#<%=PositionsIdEdit.ClientID %>"), $("#<%=PositionsIdEditMultiple.ClientID%>"));
            });

            $(".onChangeLaborsIdEdit").on("changed.bs.select", function () {
                MultiSelectDropdownListSaveSelectedItems($("#<%=LaborsIdEdit.ClientID %>"), $("#<%=LaborsIdEditMultiple.ClientID%>"));
            });

            $(".onChangeEmployeesIdEdit").on("changed.bs.select", function () {
                MultiSelectDropdownListSaveSelectedItems($("#<%=EmployeesIdEdit.ClientID %>"), $("#<%=EmployeesIdEditMultiple.ClientID%>"));
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

            $('.dateinput').datetimepicker({
                format: 'MM/DD/YYYY',
                minDate: '1970-01-01'
            });

            $(".trim").change(function () {
                var text = $.trim($(this).val());
                $(this).val(text);
            });

            $('.selectpicker').selectpicker();

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
            jQuery.validator.addMethod("validSelection", function (value, element) {
                return this.optional(element) || value != "-1";
            }, "Please select a valid option");

            jQuery.validator.addMethod("validSelectionIsExpiration", function (value, element) {
                var option = $("#<%=IsExpirationEdit.ClientID%>").is(":checked");
                
                if (option) {
                    return value != "" && value != null;
                } else {
                    return true;
                }
            }, "<%=GetLocalResourceObject("msgInputValid").ToString()%>");

            jQuery.validator.addMethod("validSelectionRuleRecruitmentDate", function (value, element) {
                var option = $("#<%=ApplyRuleRecruitmentDateEdit.ClientID%>").is(":checked");
                
                if (option) {
                    return value != "" && value != null;
                } else {
                    return true;
                }
            }, "<%=GetLocalResourceObject("msgInputValid").ToString()%>");

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
                    "<%= MasterProgramCodeEdit.UniqueID%>": {
                        required: true,
                        normalizer: function (value) {
                            return $.trim(value);
                        },
                        minlength: 1,
                        maxlength: 10
                    },

                    "<%= MasterProgramNameEdit.UniqueID%>": {
                        required: true,
                        normalizer: function (value) {
                            return $.trim(value);
                        },
                        minlength: 1,
                        maxlength: 500
                    },

                    "<%= MatrixTargetIdEdit.UniqueID%>": {
                        required: true, validSelection: true
                    },

                    "<%= TrainingPlanProgramEdit.UniqueID%>": {
                        required: true, validSelection: true
                    },

                    "<%= CycleTrainingIdEdit.UniqueID%>": {
                        required: true, validSelectionIsExpiration: true
                    },

                    "<%= FromDateEdit.UniqueID%>": {
                        required: true, validSelectionRuleRecruitmentDate: true
                    },

                    "<%= ToDateEdit.UniqueID%>": {
                        required: true, validSelectionRuleRecruitmentDate: true
                    },
                }
            });

            var result = validator.form();
            return result;
        }

        function ValidateFormRelate() {
            jQuery.validator.addMethod("validSelection", function (value, element) {
                return this.optional(element) || value != "-1";
            }, "Please select a valid option");

            jQuery.validator.addMethod("relateBy1", function (value, element) {
                var relateBy = $("#<%=RelatedByEdit.ClientID%>").val();

                if (relateBy == "1") {
                    var positions = $("#<%=PositionsIdEdit.ClientID %>").selectpicker('val').length > 0;
                    if (positions) {
                        return true;
                    }

                    var labor = $("#<%=LaborsIdEdit.ClientID %>").selectpicker('val').length > 0;
                    if (labor) {
                        return true;
                    }

                    return false;
                } else {
                    return true;
                }
            }, "<%=GetLocalResourceObject("msgInputValid").ToString()%>");

            jQuery.validator.addMethod("relateBy2", function (value, element) {
                var relateBy = $("#<%=RelatedByEdit.ClientID%>").val();

                if (relateBy == "2") {
                    var employees = $("#<%=EmployeesIdEdit.ClientID %>").selectpicker('val').length > 0;
                    if (employees) {
                        return true;
                    }

                    return false;
                } else {
                    return true;
                }
            }, "<%=GetLocalResourceObject("msgInputValid").ToString()%>");

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
                   "<%= RelatedByEdit.UniqueID%>": {
                        required: true, validSelection: true
                    },

                   "<%= PositionsIdEdit.UniqueID%>": {
                        relateBy1: true
                    },

                   "<%= LaborsIdEdit.UniqueID%>": {
                        relateBy1: true
                    },

                    "<%= EmployeesIdEdit.UniqueID%>": {
                        relateBy2: true
                    }
                }
            });

            var result = validator.form();
            return result;
        }

        function ValidateFormExisting() {
            jQuery.validator.addMethod("validSelection", function (value, element) {
                return this.optional(element) || value != "-1";
            }, "Please select a valid option");

            jQuery.validator.addMethod("existing", function (value, element) {
                var option = $("#<%=ExistingMasterProgramEdit.ClientID%>").is(":checked");

                if (option) {
                    return false;
                } else {
                    return true;
                }

            }, "<%=GetLocalResourceObject("msgInputValid").ToString()%>");

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
                    "<%= MasterProgramListEdit.UniqueID%>": {
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

        function SetWaitingGrvList(flag) {
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
        

        function MetodoPrueba(flag) {
            /// <summary>Process the request of set the grid as waiting style</summary>
            debugger;
            if (flag) {
                $('#<%= grvList.ClientID %>').find("input,button,textarea,select").attr("disabled", "disabled");
                $('#grvListWaiting').fadeIn('fast');
                $('#<%= grvList.ClientID %>').fadeTo('fast', 0.5);
            } else {
                $('#<%= grvList.ClientID %>').find("input,button,textarea,select").removeAttr("disabled");
                $('#grvListWaiting').fadeOut('fast');
                $('#<%= grvList.ClientID %>').fadeIn('fast',100);
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

        function ExistingMasterProgramChange() {
            if ($("#<%=ExistingMasterProgramEdit.ClientID%>").is(":checked")) {
                $(".MasterProgramList").show();
            } else {
                $(".MasterProgramList").hide();
            }
        }

        function IsExpirationChange() {
            if ($("#<%=IsExpirationEdit.ClientID%>").is(":checked")) {
                $(".IsExpirationEdit").show();
            } else {
                $(".IsExpirationEdit").hide();

                $("#<%=CycleTrainingIdEdit.ClientID%>").val("");

                $("#<%=ApplyRuleRecruitmentDateEdit.ClientID%>").bootstrapToggle('off');
                $(".ApplyRuleRecruitmentDate").hide();
                
                $('.dateinput').val('01/01/1970');
            }
        }

        function ApplyRuleRecruitmentDateChange() {
                        if ($("#<%=ApplyRuleRecruitmentDateEdit.ClientID%>").is(":checked")) {
                $(".ApplyRuleRecruitmentDate").show();

                if ($("#<%=hdfMasterProgramIdEdit.ClientID%>").val() == "-1" || $("#<%=hdfMasterProgramIdEdit.ClientID%>").val() == "") {
                    if ($("#<%=MasterProgramListEdit.ClientID%>").val() == "-1" || $("#<%=MasterProgramListEdit.ClientID%>").val() == "") {
                        $('.dateinput').val('01/01/1970');
                    }
                }
            } else {
                $(".ApplyRuleRecruitmentDate").hide();
            }
        }

        function RecruitmentDate(date) {            
            $('.FromDateEdit').data("DateTimePicker").maxDate(date);
            $('.ToDateEdit').data("DateTimePicker").maxDate(date);
        }

        function ClearModalForm() {
            /// <summary>Clear the modal form</summary>
            $("#<%=hdfMasterProgramIdEdit.ClientID%>").val("-1");
            $("#<%=hdfMasterProgramIdEditExisted.ClientID%>").val("");

            $("#<%=ExistingMasterProgramEdit.ClientID%>").bootstrapToggle('off');

            $("#<%=MasterProgramCodeEdit.ClientID%>").val("");
            $("#<%=MasterProgramNameEdit.ClientID%>").val("");

            $("#<%=IsExpirationEdit.ClientID%>").bootstrapToggle('off');
            $("#<%=CycleTrainingIdEdit.ClientID%>").val("");
            $("#<%=ApplyRuleRecruitmentDateEdit.ClientID%>").bootstrapToggle('off');

            $("#<%=SearchEnabledEdit.ClientID%>").bootstrapToggle('on');

            $(".ExistingMasterProgram").hide();
            $(".IsExpirationEdit").hide();
            $(".ApplyRuleRecruitmentDate").hide();

            if (validator != null) {
                validator.resetForm();
            }
        }

        function SelectedRelatedBy() {
            if ($("#<%= RelatedByEdit.ClientID%>").val() == "1") {
                $(".relateBy1").show();
                $(".relateBy2").hide();
            } else {
                $(".relateBy1").hide();
                $(".relateBy2").show();
            }
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
        function ProcessAddRequest(resetId) {
            ///// <summary>Handles the click event for button add.</summary>     
            $('#<%= MasterProgramCodeEdit.ClientID %>').prop('disabled', false);

            ClearModalForm();

            $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceAdd") %>');
            $('#dialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceAdd") %>');
            $('#MaintenanceDialog').modal('show');

            __doPostBack('<%= btnAdd.UniqueID %>', '');
            return true;
        }

        function ProcessEditRequest(resetId) {
            /// <summary>Process the edit request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            if (IsRowSelected()) {
                $('#<%= ExistingMasterProgramEdit.ClientID %>').prop('disabled', true);
                $('#<%= MasterProgramListEdit.ClientID %>').prop('disabled', true);
                $('#<%= MasterProgramCodeEdit.ClientID %>').prop('disabled', true);

                __doPostBack('<%= btnEdit.UniqueID %>', '');
                return true;
            }

            else {
                ShowFooterError('<%= GetLocalResourceObject("msgInvalidSelection") %>');
                ErrorButton(resetId);
                return false;
            }
        }

        function ProcessRelateSummaryRequest(resetId) {
            /// <summary>Process the edit request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            if (IsRowSelected()) {
                __doPostBack('<%= btnRelatedSummary.UniqueID %>', '');
                return true;
            }

            else {
                ShowFooterError('<%= GetLocalResourceObject("msgInvalidSelection") %>');
                ErrorButton(resetId);
                return false;
            }
        }

        function ProcessRelateRequest(resetId) {
            /// <summary>Process the edit request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            if (IsRowSelected()) {
                __doPostBack('<%= btnRelationship.UniqueID %>', '');
                return true;
            }

            else {
                ShowFooterError('<%= GetLocalResourceObject("msgInvalidSelection") %>');
                ErrorButton(resetId);
                return false;
            }
        }

        function ProcessAcceptRelateRequest(resetId) {
            disableButton($("#" + resetId));
            disableButton($('#btnCancelRelate'));

            if (!ValidateFormRelate()) {
                setTimeout(function () {
                    ResetButton(resetId);
                    enableButton($("#" + resetId));
                    enableButton($('#btnCancelRelate'));
                }, 150);

                MostrarMensaje(TipoMensaje.VALIDACION, '<%= GetLocalResourceObject("msgInvalidEntry") %>', null);
            }

            else {
                __doPostBack('<%= btnAcceptRelate.UniqueID %>', '');
            }

            return false;
        }

        function ProcessAcceptRequest(resetId) {
            /// <summary>Process the accept request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            DisableButtonsDialog();

            if (!ValidateForm()) {
                setTimeout(function () {
                    ResetButton(resetId);
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

        function ReturnRequestBtnEditOpen(ValidacionRegional) {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $('#MaintenanceDialog').modal('show');
            SetRowSelected();
            DisableToolBar();

            $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#dialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');

            $('#<%= ExistingMasterProgramEdit.ClientID %>').prop('disabled', true);
            $(".MasterProgramList").hide();

            $('#<%= MasterProgramListEdit.ClientID %>').prop('disabled', true);
            $('#<%= MasterProgramCodeEdit.ClientID %>').prop('disabled', true);

            if ($("#<%=this.IsExpirationEdit.ClientID%>").is(":checked")) {
                $(".IsExpirationEdit").show();
            } else {
                $(".IsExpirationEdit").hide();
            }

            if ($("#<%=this.ApplyRuleRecruitmentDateEdit.ClientID%>").is(":checked")) {
                $(".ApplyRuleRecruitmentDate").show();
            } else {
                $(".ApplyRuleRecruitmentDate").hide();
            }

            EnableToolBar();
        }

        function ReturnRequestBtnRelateOpen(ValidacionRegional) {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $('#MaintenanceDialogRelate').modal('show');
            SetRowSelected();
            DisableToolBar();

            MultiSelectDropdownListRestoreSelectedItems($("#<%=PositionsIdEdit.ClientID %>"), $("#<%=PositionsIdEditMultiple.ClientID%>"));
            MultiSelectDropdownListRestoreSelectedItems($("#<%=LaborsIdEdit.ClientID %>"), $("#<%=LaborsIdEditMultiple.ClientID%>"));
            MultiSelectDropdownListRestoreSelectedItems($("#<%=EmployeesIdEdit.ClientID %>"), $("#<%=EmployeesIdEditMultiple.ClientID%>"));

            $('.selectpicker').selectpicker('render');

            SelectedRelatedBy();

            EnableToolBar();
        }

        function ReturnRequestBtnRelateSummaryOpen() {
            SetRowSelected();
            DisableToolBar();

            $('#MaintenanceDialogRelateSummary').modal('show');

            EnableToolBar();
        }

        function ReturnPostBackAcceptClickSave() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            DisableButtonsDialog();
            SetRowSelected();

            $('#MaintenanceDialog').modal('hide');

            ShowFooterSuccess('<%= GetLocalResourceObject("msgOperationSuccesfull") %>');
            EnableButtonsDialog();
        }

        function ReturnPostBackAcceptRelateClickSave() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            DisableButtonsDialog();
            SetRowSelected();

            SelectedRelatedBy();

            $('#MaintenanceDialogRelate').modal('hide');

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
            if ($("#<%=this.IsExpirationEdit.ClientID%>").is(":checked")) {
                $(".IsExpirationEdit").show();
            } else {
                $(".IsExpirationEdit").hide();
            }

            if ($("#<%=this.ApplyRuleRecruitmentDateEdit.ClientID%>").is(":checked")) {
                $(".ApplyRuleRecruitmentDate").show();
            } else {
                $(".ApplyRuleRecruitmentDate").hide();
            }

            if ($("#<%=this.ExistingMasterProgramEdit.ClientID%>").is(":checked")) {
                $(".MasterProgramList").show();
            } else {
                $(".MasterProgramList").hide();
            }

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
