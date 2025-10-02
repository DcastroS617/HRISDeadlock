<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="TrainingLogbookRegister.aspx.cs" Inherits="HRISWeb.Training.TrainingLogbookRegister" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntHeader" runat="server">
    <title><%=Convert.ToString(GetLocalResourceObject("lblScreenTitle")) %></title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cntBody" runat="server">
    <div class="main-content">
        <h1 class="text-left text-primary">
            <label class="PageTitle"><%= GetLocalResourceObject("lblScreenTitle") %></label>
        </h1>
        <br />

        <asp:HiddenField ID="hdfGeographicDivisionCode" runat="server" Value="" />
        <asp:HiddenField ID="hdfUserAccount" runat="server" Value="" />
        <asp:HiddenField ID="hdfTypeLogbook" runat="server" Value="" />
        <asp:HiddenField ID="hdfIsFormEnabled" runat="server" Value="true" />
        <asp:HiddenField ID="hdfIsLogbookClosed" runat="server" Value="true" />
        <asp:HiddenField ID="hdfLogBookProcces" runat="server" Value="New" />
        <asp:HiddenField ID="hdfTypeEmployeesSearch" runat="server" Value="" />
        <asp:HiddenField ID="hdfMinGrade" runat="server" Value="" />
        <asp:HiddenField ID="hdfCreatedBy" runat="server" Value="true" />

        <input type="hidden" name="LabelParticipantesBuscar" value="" />

        <div class="container" style="width: 100%">
            <div class="row">
                <h3><%= GetLocalResourceObject("lblLogbookInformationSubtitle") %></h3>
                <br />

                <asp:UpdatePanel runat="server" ID="uppControls" UpdateMode="Conditional">
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnSearchLogBook" />
                        <asp:PostBackTrigger ControlID="btnNew" />
                        <asp:PostBackTrigger ControlID="btnNewFromThis" />
                        <asp:PostBackTrigger ControlID="btnEdit" />
                        <asp:PostBackTrigger ControlID="btnDraft" />
                        <asp:PostBackTrigger ControlID="btnSave" />
                        <asp:PostBackTrigger ControlID="btnDelete" />
                        <asp:PostBackTrigger ControlID="lnkbtnNewLogbook" />
                        <asp:PostBackTrigger ControlID="lnkbtnNewLogbookFromThis" />
                        <asp:PostBackTrigger ControlID="lnkbtnEditLogbook" />
                        <asp:PostBackTrigger ControlID="lnkbtnDraftLogbook" />
                        <asp:PostBackTrigger ControlID="lnkbtnSaveLogbook" />
                        <asp:PostBackTrigger ControlID="lnkbtnDeleteLogbook" />
                    </Triggers>

                    <ContentTemplate>
                        <div class="col-sm-6">
                            <!-- GENERIC CONTROLS' COLUMN-->
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtLogbookNumber.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblLogbookNumber")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <div class="input-group" style="white-space: nowrap;">
                                            <asp:TextBox ID="txtLogbookNumber" CssClass="form-control control-validation cleanPasteDigits" runat="server" autocomplete="off" onkeypress="return isNumber(event) && checkMinValue(this, event, 0) && checkMaxLength(this,event,9);" MaxLength="9" type="number" min="0" Enabled="false"></asp:TextBox>
                                            <label id="txtLogbookNumberValidation" for="<%= txtLogbookNumber.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjLogbookNumberValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>

                                            <div class="input-group-btn">
                                                <button id="btnSearchLogbook" type="button" runat="server" class="btn btn-default btnAjaxAction" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin\"></span>&nbsp;", GetLocalResourceObject("btnSearchLogbook"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign\"></span>&nbsp;", GetLocalResourceObject("btnSearchLogbook"))%>' onserverclick="BtnSearchLogbook_ServerClick">
                                                    <span class='glyphicon glyphicon-search'></span>&nbsp;<%=GetLocalResourceObject("btnSearchLogbook")%>
                                                </button>

                                                <button type="button" id="btnDeferredSearchButton" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" data-loading-text='<span class="fa fa-spinner fa-spin"></span>' data-error-text='<span class="glyphicon glyphicon-exclamation-sign"></span>'><span class="glyphicon glyphicon-menu-down"></span></button>
                                                <ul class="dropdown-menu dropdown-menu-right">
                                                    <li>
                                                        <asp:LinkButton ID="lnkbtnNewLogbook" runat="server" CssClass="btnAjaxDeferredActionOnSearchButton" OnClick="BtnNew_ServerClick" OnClientClick="return ShowConfirmationMessageNew('btnDeferredSearchButton');"><%=GetLocalResourceObject("lnkbtnNewLogbook")%></asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkbtnNewLogbookFromThis" runat="server" CssClass="btnAjaxDeferredActionOnSearchButton" OnClick="BtnNewFromThis_ServerClick" OnClientClick="return ShowConfirmationMessageNewFromThis('btnDeferredSearchButton');"><%=GetLocalResourceObject("lnkbtnNewLogbookFromThis")%></asp:LinkButton>
                                                    </li>
                                                    <li role="separator" class="divider"></li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkbtnEditLogbook" runat="server" CssClass="btnAjaxDeferredActionOnSearchButton" OnClick="BtnEdit_ServerClick" OnClientClick="return ProcessEditRequestLogbook('btnDeferredSearchButton');"><%=GetLocalResourceObject("lnkbtnEditLogbook")%></asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkbtnDraftLogbook" runat="server" CssClass="btnAjaxDeferredActionOnSearchButton" OnClick="BtnDraft_ServerClick" OnClientClick="if(!ValidateForm()) { ResetButton('btnDeferredSearchButton'); return false; }"><%=GetLocalResourceObject("lnkbtnDraftLogbook")%></asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkbtnSaveLogbook" runat="server" CssClass="btnAjaxDeferredActionOnSearchButton" OnClick="BtnSave_ServerClick" OnClientClick="return ProcessSaveRequestLogbook(this.id);"><%=GetLocalResourceObject("lnkbtnCloseLogbook")%></asp:LinkButton>
                                                    </li>
                                                    <li role="separator" class="divider"></li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkbtnDeleteLogbook" runat="server" CssClass="btnAjaxDeferredActionOnSearchButton" OnClick="BtnDelete_ServerClick" OnClientClick="return ShowConfirmationMessageDelete('btnDeferredSearchButton');"><%=GetLocalResourceObject("lnkbtnDeleteLogbook")%></asp:LinkButton>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%=txtLogbookStatus.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblLogbookStatus")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtLogbookStatus" CssClass="form-control" runat="server" onkeypress="return isNumberOrLetterNoEnter(event);" autocomplete="off" type="text" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%=cboCourse.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCourse")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="cboCourse" CssClass="form-control cboAjaxAction control-validation" data-live-search="true" AutoPostBack="true" runat="server" OnSelectedIndexChanged="CboCourse_SelectedIndexChanged"></asp:DropDownList>

                                        <span id="cboCourseWaiting" class='fa fa-spinner fa-spin waitingNotification' style="display: none; float: right; margin-right: 25px; margin-top: -23px; position: relative; z-index: 2;"></span>
                                        <label id="cboCourseValidation" for="<%= cboCourse.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjCourseValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                    </div>
                                </div>

                                <div id="cycleTranining" class="form-group" runat="server">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%=cboCycleTranining.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCycleTranining")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="cboCycleTranining" CssClass="form-control cboAjaxAction control-validation" data-live-search="true" AutoPostBack="true" runat="server" OnSelectedIndexChanged="CboCycleTranining_SelectedIndexChanged"></asp:DropDownList>

                                        <span id="cboCycleTraniningWaiting" class='fa fa-spinner fa-spin waitingNotification' style="display: none; float: right; margin-right: 25px; margin-top: -23px; position: relative; z-index: 2;"></span>
                                        <label id="cboCycleTraniningValidation" for="<%= cboCycleTranining.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjCycleTraniningValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%=cboTrainer.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblTrainer")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="cboTrainer" CssClass="form-control control-validation" data-live-search="true" runat="server" OnChange="UpdateTrainer()"></asp:DropDownList>
                                        <span id="cboTrainerWaiting" class='fa fa-spinner fa-spin waitingNotification' style="display: none; float: right; margin-right: 25px; margin-top: -23px; position: relative; z-index: 2;"></span>
                                        <label id="cboTrainerValidation" for="<%= cboTrainer.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjTrainerValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%=cboClassificationCourseId.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblClassification")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <select id="cboClassificationCourseId" class="form-control  control-validation" runat="server"></select>

                                        <label id="cboClassificationCourseIdValidation" for="<%= cboClassificationCourseId.ClientID%>"
                                            class="label label-danger label-validation" data-toggle="tooltip" data-container="body"
                                            data-placement="left" data-content="<%= GetLocalResourceObject("msjCourseValidation") %>"
                                            style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">
                                            !</label>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="dtpStartDate" class="control-label"><%=GetLocalResourceObject("lblStartDate")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <div class="input-group">
                                            <asp:TextBox runat="server" ID="dtpStartDate" class="form-control date control-validation cleanPasteDigits" placeholder='<%$ Code:GetLocalResourceObject("lblDateFormatPlaceHolder") %>' type="text" autocomplete="off" />
                                            <label id="dtpStartDateValidation" for="<%= dtpStartDate.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjStartDateValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                            <div class="input-group-addon">
                                                <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                            </div>

                                            <asp:TextBox runat="server" ID="tpcStarTime" class="form-control control-validation cleanPasteDigits" placeholder='<%$ Code:GetLocalResourceObject("lblTimeFormatPlaceHolder") %>' type="time" autocomplete="off" />
                                            <label id="tpcStarTimeValidation" for="<%= tpcStarTime.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjStarTimeValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                            <div class="input-group-addon">
                                                <span class="glyphicon glyphicon-time" aria-hidden="true"></span>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="dtpEndDate" class="control-label"><%=GetLocalResourceObject("lblEndDate")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <div class="input-group">
                                            <asp:TextBox runat="server" ID="dtpEndDate" class="form-control date control-validation cleanPasteDigits" placeholder='<%$ Code:GetLocalResourceObject("lblDateFormatPlaceHolder") %>' type="text" autocomplete="off" />
                                            <label id="dtpEndDateValidation" for="<%= dtpEndDate.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjEndDateValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>

                                            <div class="input-group-addon">
                                                <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%=cboTrainingCenter.ClientID%>" class="control-label" style="text-align: left"><%=GetLocalResourceObject("lblTrainingCenter")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="cboTrainingCenter" CssClass="form-control cboAjaxAction control-validation" data-live-search="true" runat="server" AutoPostBack="true" OnSelectedIndexChanged="CboTrainingCenter_SelectedIndexChanged"></asp:DropDownList>
                                        <span id="cboTrainingCenterWaiting" class='fa fa-spinner fa-spin waitingNotification' style="display: none; float: right; margin-right: 25px; margin-top: -23px; position: relative; z-index: 2;"></span>
                                        <label id="cboTrainingCenterValidation" for="<%= cboTrainingCenter.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjTrainingCenterValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%=cboClassroom.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblClassroom")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="cboClassroom" CssClass="form-control cboAjaxAction control-validation" data-live-search="true" AutoPostBack="true" runat="server" OnSelectedIndexChanged="CboClassroom_SelectedIndexChanged"></asp:DropDownList>
                                        <span id="cboClassroomWaiting" class='fa fa-spinner fa-spin waitingNotification' style="display: none; float: right; margin-right: 25px; margin-top: -23px; position: relative; z-index: 2;"></span>
                                        <label id="cboClassroomValidation" for="<%= cboClassroom.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjClassroomValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />
                        <br />

                        <div class="col-sm-6">
                            <div class="form-horizontal">
                                <div class="panel panel-info">
                                    <div class="panel-heading">
                                        <h3 class="panel-title"><%=GetLocalResourceObject("lblCourseSubtitle")%></h3>
                                    </div>

                                    <div class="panel-body">
                                        <asp:HiddenField ID="hdfCourseCostByParticipant" runat="server" Value="" />
                                        <asp:HiddenField ID="hdfNoteRequired" runat="server" Value="" />                                      
                                        <asp:HiddenField ID="hdfCyclesRefreshment" runat="server" Value="" />

                                        <table class="table table-user-information">
                                            <tbody>
                                                <tr>
                                                    <td class="col-sm-3"><%=GetLocalResourceObject("lblCourseCode")%></td>
                                                    <td class="col-sm-9">
                                                        <div runat="server" id="txtCourseCode"></div>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td class="col-sm-3"><%=GetLocalResourceObject("lblCourseName")%></td>
                                                    <td class="col-sm-9">
                                                        <div runat="server" id="txtCourseName"></div>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td class="col-sm-3"><%=GetLocalResourceObject("lblCourseDuration")%></td>
                                                    <td class="col-sm-9">
                                                        <span runat="server" id="txtCourseDuration"></span>&nbsp;h
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td class="col-sm-3"><%=GetLocalResourceObject("lblTrainner")%></td>
                                                    <td class="col-sm-9">
                                                        <div runat="server" id="txtTrainer"></div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>

                                <div id="panelCycleTranining" class="panel panel-info" runat="server">
                                    <div class="panel-heading">
                                        <h3 class="panel-title"><%=GetLocalResourceObject("lblCycleTraniningSubtitle")%></h3>
                                    </div>

                                    <div iclass="panel-body">
                                        <table class="table table-user-information">
                                            <tbody>
                                                <tr>
                                                    <td class="col-sm-3"><%=GetLocalResourceObject("lblCycleTrainingName")%></td>
                                                    <td class="col-sm-9">
                                                        <span runat="server" id="txtCycleTrainingName"></span>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td class="col-sm-3"><%=GetLocalResourceObject("lblCycleTrainingStartDate")%></td>
                                                    <td class="col-sm-9">
                                                        <span runat="server" id="txtCycleTrainingStartDate"></span>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td class="col-sm-3"><%=GetLocalResourceObject("lblCycleTrainingEndDate")%></td>
                                                    <td class="col-sm-9">
                                                        <span runat="server" id="txtCycleTrainingEndDate"></span>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>

                                <div class="panel panel-info">
                                    <div class="panel-heading">
                                        <h3 class="panel-title"><%=GetLocalResourceObject("lblClassroomSubtitle")%></h3>
                                    </div>

                                    <div class="panel-body">
                                        <table class="table table-user-information">
                                            <tbody>
                                                <tr>
                                                    <td class="col-sm-3"><%=GetLocalResourceObject("lblClassroomCapacity")%></td>
                                                    <td class="col-sm-9"><span runat="server" id="txtClassroomCapacity"></span>&nbsp;<%=GetLocalResourceObject("lblPersons")%></td>
                                                </tr>
                                                <tr>
                                                    <td><%=GetLocalResourceObject("lblClassroomComments")%></td>
                                                    <td><span runat="server" id="txtClassroomComments"></span></td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>

                                <div class="col-sm-12 text-center">
                                    <div class="ButtonsActions">
                                        <asp:UpdatePanel ID="uppBotonera" runat="server">
                                            <ContentTemplate>
                                                <div class="btn-group" role="group" aria-label="main-buttons">
                                                    <button id="btnNew" type="button" runat="server" class="btn btn-default btnAjaxAction"
                                                        onserverclick="BtnNew_ServerClick" onclick="return ShowConfirmationMessageNew(this.id);"
                                                        data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnNew"), "<br />&nbsp;")%>'
                                                        data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnNew"), "<br />&nbsp;")%>'>
                                                        <span class="glyphicon glyphicon-file glyphicon-main-button"></span>
                                                        <br />
                                                        <%= GetLocalResourceObject("btnNew") %><br />
                                                        &nbsp;
                                                    </button>

                                                    <button id="btnNewFromThis" type="button" runat="server" class="btn btn-default btnAjaxAction"
                                                        onserverclick="BtnNewFromThis_ServerClick" onclick="return ShowConfirmationMessageNewFromThis(this.id);"
                                                        data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnNewFromThis"))%>'
                                                        data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnNewFromThis"))%>'>
                                                        <span class="glyphicon glyphicon-duplicate glyphicon-main-button"></span>
                                                        <br />
                                                        <%= GetLocalResourceObject("btnNewFromThis") %>
                                                    </button>

                                                    <button id="btnEdit" type="button" runat="server" class="btn btn-default btnAjaxAction"
                                                        onserverclick="BtnEdit_ServerClick" onclick="return ProcessEditRequestLogbook(this.id);"
                                                        data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnEdit"), "<br />&nbsp;")%>'
                                                        data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnEdit"), "<br />&nbsp;")%>'>
                                                        <span class="glyphicon glyphicon-edit glyphicon-main-button"></span>
                                                        <br />
                                                        <%= GetLocalResourceObject("btnEdit") %><br />
                                                        &nbsp;
                                                    </button>

                                                    <button id="btnDraft" type="button" runat="server" class="btn btn-default btnAjaxAction" 
                                                        onserverclick="BtnDraft_ServerClick" onclick="if(!ValidateForm()) { ResetButton(this.id); return false; }"
                                                        data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnDraft"))%>'
                                                        data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnDraft"))%>'>
                                                        <span class="glyphicon glyphicon-time glyphicon-main-button"></span>
                                                        <br />
                                                        <%= GetLocalResourceObject("btnDraft") %>
                                                    </button>

                                                    <button id="btnSave" type="button" runat="server" class="btn btn-default btnAjaxAction"
                                                        onserverclick="BtnSave_ServerClick" onclick="return ProcessSaveRequestLogbook(this.id);"
                                                        data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnSave"))%>'
                                                        data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnSave"))%>'>
                                                        <span class="glyphicon glyphicon-save glyphicon-main-button"></span>
                                                        <br />
                                                        <%= GetLocalResourceObject("btnSave") %>
                                                    </button>

                                                    <button id="btnDelete" type="button" runat="server" class="btn btn-default btnAjaxAction"
                                                        onserverclick="BtnDelete_ServerClick" onclick="return ShowConfirmationMessageDelete(this.id);"
                                                        data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnDelete"), "<br />&nbsp;")%>'
                                                        data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnDelete"), "<br />&nbsp;")%>'>
                                                        <span class="glyphicon glyphicon-trash glyphicon-main-button"></span>
                                                        <br />
                                                        <%= GetLocalResourceObject("btnDelete") %><br />
                                                        &nbsp;
                                                    </button>

                                                    <button id="btnPrint" type="button" runat="server" class="btn btn-default btnAjaxAction"
                                                        onclick="return ProcessPrintLogbook(this.id);"
                                                        data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("lblprint"), "<br />&nbsp;")%>'
                                                        data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnDelete"), "<br />&nbsp;")%>'>
                                                        <span class="glyphicon glyphicon-print glyphicon-main-button"></span>
                                                        <br />
                                                        <%= GetLocalResourceObject("lblprint") %><br />
                                                        &nbsp;
                                                    </button>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-sm-12">
                            <div class="col-6 col-md-12">
                                <!-- PERSONS' COLUMN -->
                                <h3><%= GetLocalResourceObject("lblParticipantsInformationSubtitle") %></h3>
                            </div>
                            <br />

                            <div class="col-6 col-md-6">
                                <h4 class="lblRegistrationGradesInformationSubtitle"><%= GetLocalResourceObject("lblAddParticipantsInformationSubtitle") %></h4>

                                <asp:UpdatePanel runat="server" ID="uppEmployees" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="txtSearchEmployees" EventName="TextChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSearchEmployees" EventName="Click" />
                                    </Triggers>

                                    <ContentTemplate>
                                        <div class="col-sm-12 text-left">
                                            <div class="col-6 col-md-12">
                                                <span class="label label-danger classroom-capacity-alert" style="display: none; float: right; margin-right: 6px; margin-top: -28px; position: relative; z-index: 2;"><%= GetLocalResourceObject("msjClassroomCapacityExceeded") %></span>
                                                <asp:Button ID="btnRefreshParticipants" runat="server" OnClick="BtnRefreshParticipants_Click" Style="display: none;" />

                                                <asp:Panel runat="server" DefaultButton="btnSearchEmployees">
                                                    <div class="col-sm-12 text-left">
                                                        <div class="input-group" style="white-space: nowrap;">
                                                            <asp:TextBox ID="txtSearchEmployees" runat="server" CssClass="form-control cleanPasteText" onkeypress="return isNumberOrLetter(event)" placeholder='<%$ Code:GetLocalResourceObject("lblSearchEmployeesPlaceHolder") %>' autocomplete="off" Width="100%" OnTextChanged="TxtSearchEmployees_TextChanged"></asp:TextBox>
                                                            <asp:Button ID="btnSearchEmployees" runat="server" Text="Button" Style="display: none" OnClick="TxtSearchEmployees_TextChanged" />

                                                            <div class="input-group-btn">
                                                                <button id="btnSearchEmployeesInfo" type="button" runat="server" class="btn btn-default btnAjaxAction" disabled="disabled">
                                                                    <span id="SearchEmployeesInfo"></span>&nbsp;<%=GetLocalResourceObject("btnSearchLogbook")%>
                                                                </button>

                                                                <button type="button" id="btnTypeEmployeesSearchButton" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" data-loading-text='<span class="fa fa-spinner fa-spin"></span>' data-error-text='<span class="glyphicon glyphicon-exclamation-sign"></span>'><span class="glyphicon glyphicon-menu-down"></span></button>
                                                                <ul class="dropdown-menu dropdown-menu-right">
                                                                    <li>
                                                                        <asp:LinkButton ID="lnkbtnEmployeesActive" runat="server" OnClientClick="return ProcessEmployeesSearch('Active');"><%=GetLocalResourceObject("lnkbtnEmployeesActive")%></asp:LinkButton></li>
                                                                    <li>
                                                                        <asp:LinkButton ID="lnkbtnEmployeesInactive" runat="server" OnClientClick="return ProcessEmployeesSearch('Inactive');"><%=GetLocalResourceObject("lnkbtnEmployeesInactive")%></asp:LinkButton></li>
                                                                </ul>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <br />
                                                    <br />

                                                    <div class="col-sm-12 text-right">
                                                        <a id="lnkbtnAdvancedSearch" onclick="OpenAdvancedSearch();" style="cursor: pointer"><%= GetLocalResourceObject("DialogAdvancedSearch") %> </a>
                                                        <span id="txtSearchEmployeesWaiting" class='fa fa-spinner fa-spin' style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;"></span>
                                                    </div>
                                                </asp:Panel>
                                            </div>

                                            <div class="col-6 col-sm-12 text-left">
                                                <label id="lblSearchEmployeesResults" runat="server" class="control-label"></label>
                                            </div>
                                            <br />
                                        </div>

                                        <asp:Repeater ID="rptEmployees" runat="server">
                                            <HeaderTemplate>
                                                <table id="tableAddParticipants" class="table table-hover table-striped">
                                                    <thead>
                                                        <tr>
                                                            <th>
                                                                <div class="col-xs-3 col-sm-3 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-id" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblEmployeeCodeHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                <div class="col-xs-7 col-sm-7 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-name" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblEmployeeNameHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                <div class="col-xs-2 col-sm-2 text-primary">&nbsp;</div>
                                                            </th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <tr>
                                                    <td class="row-fluid">
                                                        <asp:UpdatePanel runat="server" ID="uppEmployee" UpdateMode="Conditional">
                                                            <Triggers></Triggers>

                                                            <ContentTemplate>
                                                                <div class="data-sort-src" data-sort-id='<%# Eval("EmployeeCode") %>' data-sort-name='<%# Eval("EmployeeName") %>'>
                                                                    <div class="col-xs-3 col-sm-3">
                                                                        <span>
                                                                            <%# Eval("EmployeeCode") %>
                                                                        </span>
                                                                    </div>

                                                                    <div class="col-xs-7 col-sm-7">
                                                                        <span>
                                                                            <%# Eval("EmployeeName") %>
                                                                        </span>
                                                                    </div>

                                                                    <div id="divAddEmployeeControls" runat="server" class="col-xs-2 col-sm-2 text-center controls">
                                                                        <asp:HiddenField ID="hdfEmployeeCode" runat="server" Value='<%#Eval("EmployeeCode") %>' />
                                                                        <asp:HiddenField ID="hdfEmployeeName" runat="server" Value='<%#Eval("EmployeeName") %>' />
                                                                        <asp:HiddenField ID="hdfEmployeeNominalClass" runat="server" Value='<%#Eval("NominalClassId") %>' />
                                                                        <asp:HiddenField ID="hdfEmployeeCostCenter" runat="server" Value='<%#Eval("CostCenter") %>' />

                                                                        <button id="btnAddParticipant" type="button" runat="server" class="btn btn-default btnAjaxAction btnAddParticipant" data-loading-text="<span class='fa fa-spinner fa-spin '></span>" data-error-text="<span class='glyphicon glyphicon-exclamation-sign'></span>" onserverclick="BtnAddParticipant_ServerClick">
                                                                            <span class="glyphicon glyphicon-plus glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                                                                        </button>
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>

                                            <FooterTemplate>
                                                </tbody>
                                            </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                            <div class="col-6 col-md-6">
                                <h4 class="lblRegistrationGradesInformationSubtitle"><%= GetLocalResourceObject("lblRegistrationGradesInformationSubtitle") %></h4>

                                <asp:UpdatePanel runat="server" ID="uppParticipants" UpdateMode="Conditional">
                                    <Triggers></Triggers>

                                    <ContentTemplate>
                                        <div class="col-sm-12 text-left">
                                            <div class="col-6 col-md-12">
                                                <div class="col-sm-12 text-left">
                                                    <input class="form-control cleanPasteText" id="txtSearchParticipants" name="txtSearchParticipants" type="text" placeholder='<%= GetLocalResourceObject("lblSearchParticipantsPlaceHolder") %>' autocomplete="off" onkeypress="return isNumberOrLetter(event)" />
                                                </div>

                                                <div class="col-sm-12 text-right">
                                                    <br />
                                                    <span id="txtSearchParticipantsWaiting" class='fa fa-spinner fa-spin' style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;"></span>
                                                </div>
                                            </div>

                                            <div class="col-6 col-sm-12 text-left">
                                                <div class="col-sm-12">
                                                    <label id="lblSearchParticipantsResults" runat="server" class="control-label"></label>
                                                </div>

                                                <div class="col-6 col-sm-12 text-left">
                                                    <div class="col-6 col-sm-6 text-left">
                                                        <label id="lblOperatives" runat="server" class="control-label"></label>
                                                    </div>

                                                    <div class="col-6 col-sm-6 text-left">
                                                        <label id="lblAdministratives" runat="server" class="control-label"></label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <asp:Repeater ID="rptParticipants" runat="server">
                                            <HeaderTemplate>
                                                <table id="tableParticipants" class="table table-hover table-striped">
                                                    <thead>
                                                        <tr>
                                                            <th>                                                  
                                                                <div class="col-xs-2 col-sm-2 text-primary isPresentAll" style="cursor: pointer;">
                                                                    <asp:CheckBox ID="chkIsPresentAll" runat="server"  AutoPostBack="false"></asp:CheckBox>

                                                                    <%= GetLocalResourceObject("lblParticipantAttendedHeader") %>
                                                                </div>          
                                                                
                                                                <div class="col-xs-2 col-sm-2 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-id" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblParticipantCodeHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                <div class="col-xs-2 col-sm-3 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-name" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblParticipantNameHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                <div class="col-xs-2 col-sm-2 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-name" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblParticipantNominalClassHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                <div class="col-xs-2 col-sm-2 text-primary sorter grade" style="cursor: pointer;" data-sort-attr="data-sort-grade" data-sort-type="int" data-sort-direction=""><%= GetLocalResourceObject("lblParticipantGradeHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                <div class="col-xs-1 col-sm-1 text-primary" style="cursor: pointer;"><%= GetLocalResourceObject("lblParticipantRemoveParticipantHeader") %> </div>
                                                            </th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <tr>
                                                    <td class="row participantEntry">
                                                        <asp:UpdatePanel runat="server" ID="uppParticipant" UpdateMode="Always" ChildrenAsTriggers="true">
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="txtGrade"/>
                                                            </Triggers>

                                                            <ContentTemplate>
                                                                <div class="data-sort-src" data-sort-id='<%# Eval("ParticipantCode") %>' data-sort-name='<%# Eval("ParticipantName") %>' data-sort-grade='<%# Eval("Grade") %>'>
                                                                    <div class="col-xs-2 col-sm-2 isPresent" style="text-align:center;">
                                                                        <asp:CheckBox ID="chkIsPresent" runat="server" Checked='<%#Eval("IsPresent")%>' AutoPostBack="false"></asp:CheckBox>
                                                                    </div>

                                                                    <div class="col-xs-2 col-sm-2">
                                                                        <span>
                                                                            <%# Eval("ParticipantCode") %>
                                                                        </span>
                                                                    </div>

                                                                    <div class="col-xs-3 col-sm-3 name">
                                                                        <span>
                                                                            <%# Eval("ParticipantName") %>
                                                                        </span>
                                                                    </div>

                                                                    <div class="col-xs-2 col-sm-2 nominalClass">
                                                                        <span>
                                                                            <%# Eval("NominalClassType") %>
                                                                        </span>
                                                                    </div>

                                                                    <div class="col-xs-2 col-sm-2 grade">
                                                                        <asp:TextBox ID="txtOriginalGrade" runat="server" CssClass="originalGrade" Text='<%# Eval("OriginalGrade") %>' Style="display: none;"></asp:TextBox>
                                                                        <asp:HiddenField ID="hdfApproved" runat="server" Value='<%# Eval("Approved") %>' />

                                                                        <div class="edit-on-click grades" tabindex="0">
                                                                            <asp:Label ID="lblGrade" runat="server" Text='<%# Eval("Grade") %>' CssClass="label edit-on-click-label label-default grade" Style="font-size: 15px; display: block; width: 100%"></asp:Label>
                                                                            <asp:TextBox ID="txtGrade" runat="server" AutoPostBack="false" OnTextChanged="TxtGrade_TextChanged" CssClass="input-group cleanPasteDigits edit-on-click-input txtGrade" onkeypress="return ReviewGrade(this, event);" type="text" autocomplete="off" Text='<%# Eval("Grade") %>' Style="display: none; width: 100%" ReadOnly="false"></asp:TextBox>
                                                                        </div>

                                                                        <span class="label label-warning originalGradeNotification" style="display: none; float: right; margin-right: -20px; margin-top: -19px; position: relative; z-index: 2;" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("tooltipUnsavedDataMark") %>">!</span>
                                                                    </div>

                                                                    <div id="divParticipantControls" runat="server" class="col-xs-1 col-sm-1 controls">
                                                                        <div class="row">
                                                                            <asp:HiddenField ID="hdfParticipantCode" runat="server" Value='<%# Eval("ParticipantCode") %>' />
                                                                            <asp:HiddenField ID="hdfParticipantNominalClass" runat="server" Value='<%#Eval("NominalClassId") %>' />

                                                                            <div class="col-xs-1 col-sm-1 text-center">
                                                                                <button id="btnRemoveParticipant" type="button" runat="server" class="btn btn-default btnAjaxAction btnRemoveParticipant" data-loading-text="<span class='fa fa-spinner fa-spin '></span>" data-error-text="<span class='glyphicon glyphicon-exclamation-sign'></span>" onserverclick="BtnRemoveParticipant_Click" onclick="return ShowConfirmationMessageRemoveParticipant(this.id);">
                                                                                    <span class="glyphicon glyphicon-remove glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                                                                                </button>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>

                                            <FooterTemplate>
                                                </tbody>
                                            </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <%--  Modal for Add and Edit  --%>
    <div class="modal fade" id="AdvancedSearchDialog" tabindex="-1" role="dialog" aria-labelledby="AdvancedSearchDialogTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" id="btnClose" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title text-primary" id="dialogTitle"></h3>
                </div>

                <asp:UpdatePanel runat="server" ID="uppDialogControl" UpdateMode="Always">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-12 text-left">
                                        <div class="col-sm-12 text-right">
                                            <div class="form-group">
                                                <div class="col-sm-2 text-left">
                                                    <label for="<%=StructByEdit.ClientID%>" class="control-label" style="text-align: left"><%=GetLocalResourceObject("lblStructByEdit")%></label>
                                                </div>

                                                <div class="col-sm-10">
                                                    <button type="button" id="btnStructByEdit" runat="server" style="display: none;"
                                                        onserverclick="BtnStructByEdit_ServerClick">
                                                    </button>

                                                    <select class="form-control onChangeStruct" runat="server" id="StructByEdit" multiple="false" data-live-search="true" data-actions-box="true" required="required"></select>

                                                    <label id="StructByEditValidation" for="<%= StructByEdit.ClientID%>" 
                                                        class="label label-danger label-validation"
                                                        data-toggle="tooltip" data-container="body" data-placement="left"
                                                        data-content="<%= GetLocalResourceObject("msjStructByEditValidation") %>"
                                                        style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                </div>
                                            </div>

                                            <div class="form-group structByFarm">
                                                <div class="col-sm-2 text-left">
                                                    <label for="<%=CostZoneIdEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCostZoneIdEdit")%></label>
                                                </div>

                                                <div class="col-sm-10">
                                                    <button type="button" id="btnCostZoneIdEdit" runat="server" style="display: none;"
                                                        onserverclick="BtnCostZoneIdEdit_ServerClick">
                                                    </button>

                                                    <select class="form-control selectpicker onChangeCostZoneId" runat="server" id="CostZoneIdEdit" multiple="false" data-live-search="true" data-actions-box="true" required="required"></select>
                                                    
                                                    <label id="CostZoneIdEditValidation" for="<%= CostZoneIdEdit.ClientID%>" 
                                                        class="label label-danger label-validation"
                                                        data-toggle="tooltip" data-container="body" data-placement="left"
                                                        data-content="<%= GetLocalResourceObject("msjCostZoneIdEditValidation") %>"
                                                        style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                </div>
                                            </div>

                                            <div class="form-group structByFarm">
                                                <div class="col-sm-2 text-left">
                                                    <label for="<%=CostMiniZoneIdEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCostMiniZoneIdEdit")%></label>
                                                </div>

                                                <div class="col-sm-10">
                                                    <button type="button" id="btnCostMiniZoneIdEdit" runat="server" style="display: none;"
                                                        onserverclick="BtnCostMiniZoneIdEdit_ServerClick">
                                                    </button>

                                                    <select class="form-control selectpicker onChangeCostMiniZoneId" runat="server" id="CostMiniZoneIdEdit" multiple="false" data-live-search="true" data-actions-box="true" required="required"></select>

                                                    <label id="CostMiniZoneIdEditValidation" for="<%= CostMiniZoneIdEdit.ClientID%>" 
                                                        class="label label-danger label-validation"
                                                        data-toggle="tooltip" data-container="body" data-placement="left"
                                                        data-content="<%= GetLocalResourceObject("msjCostMiniZoneIdEditEditValidation") %>"
                                                        style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                </div>
                                            </div>

                                            <div class="form-group structByFarm">
                                                <div class="col-sm-2 text-left">
                                                    <label for="<%=CostFarmsIdEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCostFarmsIdEdit")%></label>
                                                </div>

                                                <div class="col-sm-10">
                                                    <button type="button" id="btnCostFarmsIdEdit" runat="server" style="display: none;"
                                                        onserverclick="BtnCostCenterByStructByFarm_ServerClick">
                                                    </button>

                                                    <select class="form-control selectpicker onChangeCostFarmId" runat="server" id="CostFarmsIdEdit" multiple="false" data-live-search="true" data-actions-box="true" required="required"></select>

                                                    <label id="CostFarmsIdEditValidation" for="<%= CostFarmsIdEdit.ClientID%>" 
                                                        class="label label-danger label-validation"
                                                        data-toggle="tooltip" data-container="body" data-placement="left"
                                                        data-content="<%= GetLocalResourceObject("msjCostFarmsIdEditValidation") %>"
                                                        style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                </div>
                                            </div>

                                            <div class="form-group structByNominalClass">
                                                <div class="col-sm-2 text-left">
                                                    <label for="<%=CompanyIdEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCompanyIdEdit")%></label>
                                                </div>

                                                <div class="col-sm-10">
                                                    <button type="button" id="btnCompanyIdEdit" runat="server" style="display: none;"
                                                        onserverclick="BtnCompanyIdEdit_ServerClick">
                                                    </button>

                                                    <select class="form-control selectpicker onChangeCompanies" runat="server" id="CompanyIdEdit" multiple="false" data-live-search="true" data-actions-box="true" required="required"></select>
                                                    
                                                    <label id="CompanyIdEditValidation" for="<%= CompanyIdEdit.ClientID%>" 
                                                        class="label label-danger label-validation"
                                                        data-toggle="tooltip" data-container="body" data-placement="left"
                                                        data-content="<%= GetLocalResourceObject("msjCompanyIdEditValidation") %>"
                                                        style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                </div>
                                            </div>

                                            <div class="form-group structByNominalClass">
                                                <div class="col-sm-2 text-left">
                                                    <label for="<%=NominalClassIdEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblNominalClassIdEdit")%></label>
                                                </div>

                                                <div class="col-sm-10">
                                                    <button type="button" id="btnNominalClassIdEdit" runat="server" style="display: none;"
                                                        onserverclick="BtnCostCenterByStructByNominalClass_ServerClick">
                                                    </button>

                                                    <select class="form-control selectpicker onChangeNominalClass" runat="server" id="NominalClassIdEdit" multiple="false" data-live-search="true" data-actions-box="true" required="required"></select>
                                                    
                                                    <label id="NominalClassIdEditValidation" for="<%= NominalClassIdEdit.ClientID%>" 
                                                        class="label label-danger label-validation"
                                                        data-toggle="tooltip" data-container="body" data-placement="left"
                                                        data-content="<%= GetLocalResourceObject("msjNominalClassIdEditValidation") %>"
                                                        style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <div class="col-sm-2 text-left">
                                                    <label for="<%= CostCenterIdEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCenterCost")%></label>
                                                </div>

                                                <div class="col-sm-10">
                                                    <select class="form-control selectpicker onChangeCostCenter" runat="server" id="CostCenterIdEdit" multiple="true" data-live-search="true" data-actions-box="true"></select>

                                                    <input type="hidden" runat="server" class="limpiarCampos" id="CostCenterIdEditMultiple" value="" />

                                                    <label id="CostCenterIdEditValidation" for="<%= CostCenterIdEdit.ClientID%>" 
                                                        class="label label-danger label-validation"
                                                        data-toggle="tooltip" data-container="body" data-placement="left"
                                                        data-content="<%= GetLocalResourceObject("msjCostCenterIdEditValidation") %>"
                                                        style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-sm-12 text-right">
                                        <button id="btnAdvancedSearch" type="button" runat="server" class="btn btn-default btnAjaxAction btnAdvancedSearch"
                                            onserverclick="BtnAdvancedSearch_ServerClick" onclick="return ProcessAdvancedSearchRequest(this.id);"
                                            data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearchLogbook"))%>'
                                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearchLogbook"))%>'>
                                            <span class="glyphicon glyphicon-search glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnSearchLogbook")) %>
                                        </button>
                                    </div>
                                </div>

                                <div class="col-6 col-md-12">
                                    <span class="label label-danger employee-capacity-alert" style="display: none; float: right; margin-right: 6px; margin-top: -28px; position: relative; z-index: 2;"><%= GetLocalResourceObject("msjEmployeeCapacityExceeded") %></span>
                                    <asp:Button ID="BtnRefreshAdvancedSearch" runat="server" OnClick="BtnRefreshAdvancedSearch_Click" Style="display: none;" />

                                    <asp:Panel runat="server" DefaultButton="btnAdvancedSearchEmployees">
                                        <div class="col-sm-12 text-left">
                                            <div style="white-space: nowrap;">
                                                <asp:TextBox ID="txtAdvancedSearchEmployees" runat="server" CssClass="form-control cleanPasteText" onkeypress="return isNumberOrLetter(event)" placeholder='<%$ Code:GetLocalResourceObject("lblSearchEmployeesPlaceHolder") %>' autocomplete="off" Width="100%" OnTextChanged="TxtAdvancedSearch_TextChanged"></asp:TextBox>
                                                <asp:Button ID="btnAdvancedSearchEmployees" runat="server" Text="Button" Style="display: none" OnClick="TxtAdvancedSearch_TextChanged" />
                                            </div>
                                        </div>

                                        <div class="col-sm-12 text-right">
                                            <span id="txtAdvancedSearchWaiting" class='fa fa-spinner fa-spin' style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;"></span>
                                        </div>
                                    </asp:Panel>
                                </div>
                                <br />
                                <br />

                                <asp:GridView ID="grvAdvancedSearchEmployees"
                                    Width="100%"
                                    runat="server"
                                    EmptyDataText='<%$ Code:GetLocalResourceObject("lblNoData") %>' EmptyDataRowStyle-CssClass="emptyRow"
                                    AllowPaging="false" PagerSettings-Visible="false" AllowSorting="true"
                                    AutoGenerateColumns="false" ShowHeader="true"
                                    CssClass="table table-striped table-hover table-bordered"
                                    DataKeyNames="EmployeeCode, EmployeeName"
                                    OnPreRender="GrvAdvancedSearchEmployees_PreRender" OnSorting="GrvAdvancedSearchEmployees_Sorting">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="3%">
                                            <HeaderTemplate>
                                                <div id="AllEmployee" style="width: 100%; text-align: center;">
                                                    <asp:CheckBox ID="chkAdvancedSearchSelectedAllEmployee" runat="server" AutoPostBack="true" OnCheckedChanged="ChkAdvancedSearchSelectedAllEmployee_CheckedChanged" />
                                                </div>
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <div id="divAdvancedSearchControls" runat="server" class="col-xs-2 col-sm-2 text-center">
                                                    <asp:HiddenField ID="hdfEmployeeCode" runat="server" Value='<%#Eval("EmployeeCode") %>' />
                                                    <asp:HiddenField ID="hdfEmployeeName" runat="server" Value='<%#Eval("EmployeeName") %>' />
                                                    <asp:HiddenField ID="hdfEmployeeNominalClass" runat="server" Value='<%#Eval("NominalClassId") %>' />
                                                    <asp:HiddenField ID="hdfEmployeeCostCenter" runat="server" Value='<%#Eval("CostCenter") %>' />

                                                    <asp:CheckBox ID="chkAdvancedSearchSelectedEmployee" runat="server" AutoPostBack="true" OnCheckedChanged="ChkAdvancedSearchSelectedEmployee_CheckedChanged" />
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="EmployeeCodeSort" runat="server" CommandName="Sort" CommandArgument="EmployeeCode"
                                                        OnClientClick="SetWaitingGrvList(true);">          
                                                            
                                                        <span><%= GetLocalResourceObject("lblEmployeeCodeHeader") %></span>&nbsp;
                                                            <i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvAdvancedSearchEmployees.ClientID, "EmployeeCode") %> sorterDirection'  aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <span id="dvEmployeeCode" data-id="EmployeeCode" data-value="<%# Eval("EmployeeCode") %>"><%# Eval("EmployeeCode") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="EmployeeNameSort" runat="server" CommandName="Sort" CommandArgument="EmployeeName"
                                                        OnClientClick="SetWaitingGrvList(true);">          
                                                           
                                                        <span><%= GetLocalResourceObject("lblEmployeeNameHeader") %></span>&nbsp;
                                                            <i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvAdvancedSearchEmployees.ClientID, "EmployeeName") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <span id="dvEmployeeName" data-id="EmployeeName" data-value="<%# Eval("EmployeeName") %>"><%# Eval("EmployeeName") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>

                                <div id="grvListWaiting" style="position: absolute; margin: auto; display: none; bottom: 0px; top: 0px; left: 50%; transform: translate(-50%, 35%);">
                                    <span class='fa fa-spinner fa-spin' style="font-size: 50px;"></span>
                                </div>

                                <h4 class="text-left text-primary" id="htmlResultsSubtitle" runat="server"></h4>
                                <nav class="text-center">
                                    <asp:BulletedList ID="blstPager" runat="server" CssClass="pagination" DisplayMode="LinkButton" OnClick="BlstPager_Click">
                                    </asp:BulletedList>
                                </nav>
                            </div>
                        </div>

                        <div class="modal-footer">
                            <button id="btnAccept" type="button" runat="server" class="btn btn-primary btnAjaxAction btnAccept"
                                onserverclick="BtnAccept_ServerClick" onclick="return ProcessAdvancedSearchRequest(this.id, '<%= btnAccept.UniqueID %>');"
                                data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>'
                                data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>'>
                                <span class="glyphicon glyphicon-ok glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnDialogSave")) %>
                            </button>

                            <button id="btnCancel" type="button" class="btn btn-default" data-dismiss="modal">
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
            <div class="alert alert-autocloseable-msg" style="display: none;"></div>
        </div>
    </nav>

    <%--  Modal  --%>
    <script type="text/javascript">
        //*******************************//
        //          VARIABLES            // 
        //*******************************//
        //Variables for table ordening
        var dataSortAttribute, dataSortType, dataSortDirection;

        //*******************************//
        //       EVENT BINDING           // 
        //*******************************//
        
        function pageLoad(sender, args) {

            var $checkAll = $('.isPresentAll input:checkbox');
            var $checkboxes = $('.isPresent input:checkbox');

            updateCheckAll();
            syncCheckAll();

            function updateCheckAll() {
                var anyPresent = $checkboxes.length > 0;
                var allChecked = $checkboxes.filter(':checked').length === $checkboxes.length;
                $checkAll.prop('checked', anyPresent && allChecked);
            }

            function syncCheckAll() {
                var allChecked = true;
                var anyPresent = false;
                $checkboxes.each(function () {
                    anyPresent = true;
                    if (!this.checked) {
                        allChecked = false;
                        return false;
                    }
                });
                $checkAll.prop('checked', anyPresent && allChecked);
            }

            function handleIndividualCheckboxClick() {
                syncCheckAll();
            }

            function handleCheckAllClick() {
                var isChecked = $(this).is(':checked');
                $checkboxes.prop('checked', isChecked);
                syncCheckAll();
            }

            $checkboxes.on('click', handleIndividualCheckboxClick);
            $checkAll.on('click', handleCheckAllClick);
     
            DisableAlGrades();
            SelectedStructBy();
            //////////////////////////////////////////////////////////////////////////////////////////////////

            $('#body').on('keyup keypress', '.enterkey', function (e) {
                var keyCode = e.keyCode || e.which;
                if (keyCode === 13) {
                    e.preventDefault();
                    return false;
                }
            });
            //////////////////////////////////////////////////////////////////////////////////////////////////

            $('#<%= txtSearchEmployees.ClientID %>').prop("disabled", $('#<%= hdfIsFormEnabled.ClientID %>').val() == "false");
            $('#btnTypeEmployeesSearchButton').prop("disabled", $('#<%= hdfIsFormEnabled.ClientID %>').val() == "false");
            $('#<%= lnkbtnEmployeesActive.ClientID %>').prop("disabled", $('#<%= hdfIsFormEnabled.ClientID %>').val() == "false");
            $('#<%= lnkbtnEmployeesInactive.ClientID %>').prop("disabled", $('#<%= hdfIsFormEnabled.ClientID %>').val() == "false");
            $('.isPresentAll input:checkbox').prop("disabled", $('#<%= hdfIsFormEnabled.ClientID %>').val() == "false");

            if ($('#<%= hdfIsFormEnabled.ClientID %>').val() == "false") {
                $('#lnkbtnAdvancedSearch').prop("disabled", "true");
                $("#lnkbtnAdvancedSearch").removeAttr('href');

                $('#lnkbtnAdvancedSearch').css('cursor', 'not-allowed');
                $('#lnkbtnAdvancedSearch').css('color', 'gray');
            }

            $('.btnRemoveParticipant').prop("disabled", $('#<%= hdfIsFormEnabled.ClientID %>').val() == "false");
            $('.txtGrade').prop("disabled", $('#<%= hdfIsFormEnabled.ClientID %>').val() == "false");
            $('.btnAddParticipant').prop("disabled", $('#<%= hdfIsFormEnabled.ClientID %>').val() == "false");
            $('.isPresent input:checkbox').prop("disabled", $('#<%= hdfIsFormEnabled.ClientID %>').val() == "false");

            if ($('#<%= hdfIsFormEnabled.ClientID %>').val() == "false") {
                $('.edit-on-click.grades').removeClass("edit-on-click");
            }

            if ($('#<%= hdfTypeEmployeesSearch.ClientID %>').val() == "A") {
                $('#<%=btnSearchEmployeesInfo.ClientID%>').text('<%=GetLocalResourceObject("lnkbtnEmployeesActive")%>');
            }
            else if ($('#<%= hdfTypeEmployeesSearch.ClientID %>').val() == "I") {
                $('#<%=btnSearchEmployeesInfo.ClientID%>').text('<%=GetLocalResourceObject("lnkbtnEmployeesInactive")%>');
            }
            //////////////////////////////////////////////////////////////////////////////////////////////////

            //In this section we set the button state for btnAjaxAction
            $('.btnAjaxAction').on('click', function () {
                var $this = $(this);
                $this.button('loading');
                setTimeout(function () { $this.button('reset'); }, 30000);
            });

            $('.btnAjaxDeferredActionOnSearchButton').on('click', function () {
                var $this = $("#btnDeferredSearchButton")
                $this.button('loading');
                setTimeout(function () { $this.button('reset'); }, 30000);
            });

            $('.cboAjaxAction').on('change', function () {
                var $this = $(this);
                $this.siblings(".waitingNotification").show();
            });
            //////////////////////////////////////////////////////////////////////////////////////////////////

            //In this section we set the button generics functionality
            $('#btnCancel, #btnClose').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                $('#AdvancedSearchDialog').hide();
            });
            //////////////////////////////////////////////////////////////////////////////////////////////////

            //In this section we set the edit-on-click logic for interchange between label and input
            $('.edit-on-click').on("click focus", function () {
                var $span = $(this);
                var $input = $span.children(".edit-on-click-input");
                var $label = $span.children(".edit-on-click-label");

                $label.hide();
                $input.show().focus()
                    .focusout(function () {
                        $input.hide();
                        if (isEmptyOrSpaces($input.val())) {
                            $input.val('');
                        }
                        $label.html($input.val()).show();
                    })
            });

            $('.edit-on-click').on('keydown', function (event) {
                if (event.which == 9 || event.which == 13) {
                    event.preventDefault();

                    var $span = $(this);
                    var $input = $span.children(".edit-on-click-input");
                    $input.trigger("change");

                    var step = 1;
                    if (event.shiftKey) {
                        step = -1;
                    }

                    var $edits = $('.edit-on-click');
                    var i = $edits.index(this);

                    // Obtener todos los campos .txtGrade
                    const grades = $('.txtGrade');

                    // Si es el último, enfocar el botón
                    if (i === grades.length - 1) {
                        $('#ctl00_cntBody_btnDraft').focus();
                    } else {
                        // Si no, enfocar el siguiente campo
                        $edits.eq(i + step).focus();
                    }
                }
            });
            //////////////////////////////////////////////////////////////////////////////////////////////////

            //In this section we set the  controls functionality for search advanced             
            $(".onChangeStruct").change(function () {
                SelectedStructBy();

                ClearAdvancedSearch();

                __doPostBack('<%= btnStructByEdit.UniqueID %>', '');
            });

            $(".onChangeCostZoneId").change(function () {
                __doPostBack('<%= btnCostZoneIdEdit.UniqueID %>', '');
            });

            $(".onChangeCostMiniZoneId").change(function () {
                __doPostBack('<%= btnCostMiniZoneIdEdit.UniqueID %>', '');
            });

            $(".onChangeCostFarmId").change(function () {
                __doPostBack('<%= btnCostFarmsIdEdit.UniqueID %>', '');
            });

            $(".onChangeCompanies").change(function () {
                __doPostBack('<%= btnCompanyIdEdit.UniqueID %>', '');
            });

            $(".onChangeNominalClass").change(function () {
                __doPostBack('<%= btnNominalClassIdEdit.UniqueID %>', '');
            });

            var current = null;        
            $(".onChangeCostCenter").on("changed.bs.select", function (e, clickedIndex, isSelected, previousValue) {
                MultiSelectDropdownListSaveSelectedItems($("#<%=CostCenterIdEdit.ClientID %>"), $("#<%=CostCenterIdEditMultiple.ClientID%>"));

                isSelected = isSelected == null || undefined ? false : isSelected;

                if (current != previousValue) {
                    current = previousValue;
                }
            });

            $(".onChangeCostCenter .bs-actionsbox .bs-select-all").on("click", function () {
                MultiSelectDropdownListSaveSelectedItems($("#<%=CostCenterIdEdit.ClientID %>"), $("#<%=CostCenterIdEditMultiple.ClientID%>"));

                setTimeout(function () { __doPostBack('<%= CostCenterIdEdit.UniqueID %>', ''); }, 1000);
            });

            //In this section we set the txtGrade event for change in order to Test grade state
            $('.txtGrade').on('keyup', function (e) {
                if (e.keyCode == 27 || e.keyCode == 13) {
                    var $this = $(this);

                    if ($this.val() != $this.parent().siblings(".originalGrade").val()) {
                        $this.val($this.parent().siblings(".originalGrade").val());
                        AddTemporaryClass($this, "btn-info", 1000);

                        $this.parent().siblings(".originalGradeNotification").hide();
                        TestGradeState($(this).attr("id"));
                    }
                }
            });

            $('.txtGrade').on('paste', function (e) {
                var txtGrade = $(this);
                var row = txtGrade.closest('tr');
                var isChecked = row.find('.isPresent input:checkbox').is(':checked');

                if (!isChecked) {
                    e.preventDefault(); // Evita el pegado si no está marcado el checkbox
                    return;
                }

                var oldValue = txtGrade.val();

                setTimeout(function () {
                    var isnum = /^\d+$/.test(txtGrade.val());
                    var newValue = parseInt(txtGrade.val());
                    var returnValue = isnum && newValue >= 0 && newValue <= 100;

                    if (!returnValue) {
                        txtGrade.val(oldValue);
                    }
                }, 100);
            });

            $('.txtGrade').on('paste', function (e) {
                var txtGrade = $(this);
                var row = txtGrade.closest('tr');
                var isChecked = row.find('.isPresent input:checkbox').is(':checked');

                if (!isChecked) {
                    e.preventDefault(); 
                    return;
                }

                var oldValue = txtGrade.val();

                setTimeout(function () {
                    var isnum = /^\d+$/.test(txtGrade.val());
                    var newValue = parseInt(txtGrade.val());
                    var returnValue = isnum && newValue >= 0 && newValue <= 100;

                    if (!returnValue) {
                        txtGrade.val('');
                    }
                }, 100);
            });

            $('.txtGrade').on('paste', function (e) {
                var txtGrade = $(this);
                var row = txtGrade.closest('tr');
                var isChecked = row.find('.isPresent input:checkbox').is(':checked');

                if (!isChecked) {
                    e.preventDefault();

                    return;
                }

                e.preventDefault();

                var gradeSpan = txtGrade.closest('tr').find('span.grade');
                var pastedData = e.originalEvent.clipboardData.getData('text');
                var sanitizedData = pastedData.replace(/[^0-9]/g, '');

                if (sanitizedData.length > 3) {
                    sanitizedData = sanitizedData.substring(0, 3);
                }

                var numericValue = parseInt(sanitizedData, 10);

                if (!isNaN(numericValue)) {
                    if (numericValue > 100) {
                        numericValue = 100;
                    }
                    txtGrade.val(numericValue);

                    if (numericValue >= 70) {
                        gradeSpan.removeClass('label-danger').addClass('label-success');
                    } else {
                        gradeSpan.removeClass('label-success').addClass('label-danger');
                    }
                } else {
                    txtGrade.val('');
                    gradeSpan.removeClass('label-success').addClass('label-danger');
                }
            });





            $('.txtGrade').on('change', function (e) {
                TestGradeState($(this).attr("id"));

                //let controlId = e.target.toString().replace(/\_/g, "$");
                //__doPostBack(controlId, '');
            });

            $('.isPresent input:checkbox').on('click', function (e) {
                var listInput = $(this).closest('tr').find('input[type="text"]');
                var grade = $(this).closest('tr').find('span.grade');
                var originalGradeNotification = $(this).closest('tr').find('span.originalGradeNotification');
             
                if (!e.target.checked) {
                    $.map(listInput, function (txt, count) {
                        $(txt).val("");
                        $(txt).focus();
                    });

                    grade[0].innerHTML = '';
                    grade[0].innerText = '';
                    grade.removeClass("label-success");
                    grade.addClass("label-danger");
                    grade[0].focus();

                    originalGradeNotification.css('display', 'none');
                }
            });

            $('.isPresentAll input:checkbox').on('click', function (e) {
                var table = $(e.target).closest('table');

                $('td input:checkbox', table).prop('checked', this.checked);
               
                if (!e.target.checked) {
                    $('td input[type="text"]', table).prop('value', "0");

                    $('td span.grade', table).prop('innerHTML', "0");
                    $('td span.grade', table).prop('innerText', "0");
                    $('td span.grade', table).removeClass("label-success");
                    $('td span.grade', table).addClass("label-danger");

                    $('td span.originalGradeNotification', table).css('display', 'none');
                }               
            });

            //In this section we set the client side event for filter participants table
            $("#txtSearchParticipants").on("keyup", function () {
                var value = $(this).val().toLowerCase();

                $("#tableParticipants tbody tr").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });

            //In this section we set the server side event for search for employees setting a delay between 
            $("#<%= txtSearchEmployees.ClientID %>").keyup(function (e) {
                SetDelayForSearchEmployeesPostBack();
            });

            $("#<%= txtAdvancedSearchEmployees.ClientID %>").keyup(function (e) {
                SetDelayForAdvancedSearchEmployeesPostBack();
            });

            //In this section we initialize the date time pickers
            $('#<%= dtpStartDate.ClientID %>').datetimepicker({
                format: 'MM/DD/YYYY'
            });

            $('#<%= dtpEndDate.ClientID %>').datetimepicker({
                format: 'MM/DD/YYYY'
            });

            $('#<%= tpcStarTime.ClientID %>').datetimepicker({
                format: 'HH:mm'
            });

            //In this section we set the date time pickers reviews            
            $('#<%= dtpStartDate.ClientID %>').on("blur", function () {
                var ValidDateVal = $(this).data("DateTimePicker").date().format('MM/DD/YYYY');
                $(this).val(ValidDateVal);

                ReviewDates();
            });

            $('#<%= dtpEndDate.ClientID %>').on("blur", function () {
                var ValidDateVal = $(this).data("DateTimePicker").date().format('MM/DD/YYYY');
                $(this).val(ValidDateVal);

                ReviewDates();
            });

            $('#<%= tpcStarTime.ClientID %>').on("blur", function () {
                var ValidDateVal = $(this).data("DateTimePicker").date().format('HH:mm');
                $(this).val(ValidDateVal);
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

            UpdateGrades();

        }

        //*******************************//
        //          VALIDATION           // 
        //*******************************//
        var validator = null;
        function ValidateForm() {
            /// <summary>Validate the form with jquey validation plugin </summary>
            /// <returns> True if form is valid. False otherwise. </returns>

            if (validator == null) {
                //add custom validation methods
                jQuery.validator.addMethod("validDate", function (value, element) {
                    return this.optional(element) || moment(value, "MM/DD/YYYY").isValid();
                }, "Please enter a valid date in the format MM/DD/YYYY");

                jQuery.validator.addMethod("validTime", function (value, element) {
                    return this.optional(element) || moment(value, "HH:mm").isValid();
                }, "Please enter a valid time in the format HH:mm");

                jQuery.validator.addMethod("validClassroomCapacity", function (value, element) {
                    return ValidateClassroomCapacity();
                }, "The classroom capacity has been exceeded");

                jQuery.validator.addMethod("validateStartEndDates", function (value, element) {
                    return ValidateDates();
                }, "The start and end day must be set and end day must be greater or equal than start date");

                jQuery.validator.addMethod("validSelect", function (value, element) {
                    return ValidateSelect(value);
                }, "Please Select a Trainer");

                jQuery.validator.addMethod("validCycleTraining", function (value, element) {
                    return (this.optional(element) || value != "-1") && $('#<%= hdfCyclesRefreshment.ClientID %>').val() == "True";
                }, "Please Select a Cycle Training");

                //declare the validator
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
                            "<%= cboCourse.UniqueID %>": {
                                required: true,
                                validSelect: true
                            },
                            "<%= cboCycleTranining.UniqueID %>": {
                                validCycleTraining: true,
                            },
                            "<%= cboTrainer.UniqueID %>": {
                                required: true,
                                validSelect: true
                            },
                            "<%= cboClassificationCourseId.UniqueID %>": {
                                required: true,
                                validSelect: true
                            },
                            "<%= dtpStartDate.UniqueID %>": {
                                required: true,
                                validDate: true
                            },
                            "<%= tpcStarTime.UniqueID %>": {
                                required: true,
                                validTime: true
                            },
                            "<%= dtpEndDate.UniqueID %>": {
                                required: true,
                                validDate: true,
                                validateStartEndDates: true
                            },
                            "<%= cboTrainingCenter.UniqueID %>": {
                                required: true,
                                validSelect: true
                            },
                            "<%= cboClassroom.UniqueID %>": {
                                required: true,
                                validSelect: true
                            },
                            "txtSearchParticipants": {
                                validClassroomCapacity: true
                            },
                        }
                    });
            }

            //get the results
            var result = validator.form();
            return result;
        }

        function ValidateFormStruct() {
            jQuery.validator.addMethod("validSelection", function (value, element) {
                return this.optional(element) || value != "-1";
            }, "Please select a valid option");

            jQuery.validator.addMethod("structByFarm", function (value, element) {
                var structBy = $("#<%=StructByEdit.ClientID%>").val();

                if (structBy == "1") {
                    var costZone = $("#<%=CostZoneIdEdit.ClientID %>").selectpicker('val').length > 0;
                    if (costZone) {
                        return true;
                    }

                    var costMiniZone = $("#<%=CostMiniZoneIdEdit.ClientID %>").selectpicker('val').length > 0;
                    if (costMiniZone) {
                        return true;
                    }

                    var costFarm = $("#<%=CostFarmsIdEdit.ClientID %>").selectpicker('val').length > 0;
                    if (costFarm) {
                        return true;
                    }

                    return false;
                } else {
                    return true;
                }
            }, "<%=GetLocalResourceObject("msgInputValid").ToString()%>");

            jQuery.validator.addMethod("structByNominalClass", function (value, element) {
                var structBy = $("#<%=StructByEdit.ClientID%>").val();

                if (structBy == "2") {
                    var nominalClass = $("#<%=NominalClassIdEdit.ClientID %>").selectpicker('val').length > 0;
                    if (nominalClass) {
                        return true;
                    }

                    var company = $("#<%=CompanyIdEdit.ClientID %>").selectpicker('val').length > 0;
                    if (company) {
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
                   "<%= StructByEdit.UniqueID%>": {
                        required: true,
                        validSelection: true
                    },
                   "<%= CostZoneIdEdit.UniqueID%>": {
                        structByFarm: true
                    },
                   "<%= CostMiniZoneIdEdit.UniqueID%>": {
                        structByFarm: true
                    },
                    "<%= CostFarmsIdEdit.UniqueID%>": {
                        structByFarm: true
                    },
                    "<%= CompanyIdEdit.UniqueID%>": {
                        structByNominalClass: true
                    },
                    "<%= NominalClassIdEdit.UniqueID%>": {
                        structByNominalClass: true
                    }
                }
            });

            var result = validator.form();
            return result;
        }

        function EnabledRequired(controlId) {
            $('#' + controlId).attr("aria-invalid", "true");
            $('#' + controlId).addClass('Invalid');
        }

        //*******************************//
        //             LOGIC             // 
        //*******************************//
        function UpdateTrainer() {
            var texto = $('#<%= cboTrainer.ClientID %> option:selected').text();
            texto = texto.substring(texto.indexOf("-") + 1, texto.lastIndexOf("-") - 1);
            $('#<%= txtTrainer.ClientID %>').text(texto);
        }

        function CalculateSetDays() {
            /// <summary>Manage the logic between start and end date and also re calculate days by Course duration</summary>
            if ($('#<%= dtpStartDate.ClientID %>').data("DateTimePicker").date() != null) {
                var starDate = $('#<%= dtpStartDate.ClientID %>').data("DateTimePicker").date();
                var endDate = starDate;

                if ($('#<%= dtpEndDate.ClientID %>').data("DateTimePicker").date() != null) {
                    var endDate = $('#<%= dtpEndDate.ClientID %>').data("DateTimePicker").date();
                }

                var dateAdd = 0;
                if ($.isNumeric($.trim($('#<%= txtCourseDuration.ClientID %>').text().replace(",", ".")))) {
                    var courseDuration = parseFloat($.trim($('#<%= txtCourseDuration.ClientID %>').text().replace(",", ".")));
                    if (courseDuration == 0) {
                        dateAdd = 1;
                    } else {
                        dateAdd = Math.floor((courseDuration - 0.1) / 8.0);
                    }
                    starDate = starDate.add(dateAdd, 'day');
                }

                $('#<%= dtpEndDate.ClientID %>').data("DateTimePicker").date(starDate);

                // Se muestra el mensaje al usuario
                AddTemporaryClass($('#<%= dtpStartDate.ClientID %>'), "btn-info", 1500);
                AddTemporaryClass($('#<%= dtpEndDate.ClientID %>'), "btn-info", 1500);
            }
        }

        function DisableButtons(state) {
            if (state == "disabled") {
                $("#btnDeferredSearchButton").prop("disabled", true);
            }
            else {
                $("#btnDeferredSearchButton").prop("disabled", false);
            }
        }

        function TestGradeState(txtGradeId) {
            /// <summary>Test a grade state based on changes between original and new grade</summary>
            /// <param name="txtGradeId" type="String">Id of the input of the grade</param>
            //update the sorter
            $("#" + txtGradeId).closest('.data-sort-src').attr("data-sort-grade", $("#" + txtGradeId).val());

            //is modificated notification
            if ($("#" + txtGradeId).parent().siblings(".originalGrade").val() == $("#" + txtGradeId).val()) {
                $("#" + txtGradeId).parent().siblings(".originalGradeNotification").hide();
            }

            //else {
            //    $("#" + txtGradeId).parent().siblings(".originalGradeNotification").show();
            //}

            //label approved
            $("#" + txtGradeId).siblings(".edit-on-click-label").removeClass("label-success");
            $("#" + txtGradeId).siblings(".edit-on-click-label").removeClass("label-danger");

            var approved = $("#" + txtGradeId).parent().siblings(".approvedGrade").val();

            if (approved != null && "TRUE" === approved.toUpperCase()) {
                $("#" + txtGradeId).siblings(".edit-on-click-label").addClass("label-success");
            }

            else if (approved != null && "FALSE" === approved.toUpperCase()) {
                $("#" + txtGradeId).siblings(".edit-on-click-label").addClass("label-danger");
            }

            else if (parseInt($("#" + txtGradeId).val()) >= parseInt($("#" + "<%= hdfMinGrade.ClientID %>").val())) {
                $("#" + txtGradeId).siblings(".edit-on-click-label").addClass("label-success");
            }

            else {
                $("#" + txtGradeId).siblings(".edit-on-click-label").addClass("label-danger");
            }
        }

        function TestZeroGradeState(txtGradeId) {
            /// <summary>Test a grade state if zero based
            /// <param name="txtGradeId" type="String">Id of the input of the grade</param>
            if ($("#" + txtGradeId).val() == "" || parseInt($("#" + txtGradeId).val()) == 0) {
                return 1;
            }

            else {
                return 0;
            }
        }

        function TestForZeroGrades() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            var zeroGrades = 0;

            $('.txtGrade').each(function (index) {
                var txtGrade = $(this);
                var row = txtGrade.closest('tr');
                var isChecked = row.find('.isPresent input:checkbox').is(':checked');
                
                if (isChecked) {
                    zeroGrades = zeroGrades + TestZeroGradeState($(this).attr("id"));
                }
            });

            return zeroGrades;
        }

        function UpdateGrades() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $('.txtGrade').each(function (index) {
                TestGradeState($(this).attr("id"));
            });
        }

        function DisableAlGrades() {
            var evaluationRequired = $("#<%=hdfNoteRequired.ClientID %>").val();

            if (evaluationRequired == 'False') {
                $('.grade').hide();

                $('.name').removeClass('col-sm-5');
                $('.name').addClass('col-sm-7');
            }
        }

        function OpenAdvancedSearch() {
            /// <summary>Handles the click event for button add.</summary>
            if ($('#<%= hdfIsFormEnabled.ClientID %>').val() == "false") {
                return;
            }

            $("#<%= StructByEdit.ClientID%>").val("1");
            SelectedStructBy();

            ClearAdvancedSearch();

            __doPostBack('<%= btnStructByEdit.UniqueID %>', '');

            $('#DialogAdvancedSearchTitle').html('<%= GetLocalResourceObject("DialogAdvancedSearch") %>');
            $('#dialogTitle').html('<%= GetLocalResourceObject("DialogAdvancedSearch") %>');
            $('#AdvancedSearchDialog').modal('show');

            return false;
        }

        function SelectedStructBy() {
            if ($("#<%= StructByEdit.ClientID%>").val() == "1") {
                $(".structByFarm").show();
                $(".structByNominalClass").hide();
            } else {
                $(".structByFarm").hide();
                $(".structByNominalClass").show();
            }
        }

        function ClearAdvancedSearch() {
            $("#<%=txtAdvancedSearchEmployees.ClientID%>").val("");

            ///Others
            $("#<%=CostCenterIdEdit.ClientID%>").html("");
            if ($("#<%=CostCenterIdEdit.ClientID %>").selectpicker("val").length > 0) {
                $("#<%=CostCenterIdEdit.ClientID %>").selectpicker("val", "");
            }

            $("#<%=CostCenterIdEdit.ClientID %>").selectpicker('refresh');
            $("#<%=CostCenterIdEditMultiple.ClientID %>").val('');
        }

        function IsPresentAll(flag) {
            let checked = (flag == "True") ? true : false;
            if (checked) {
                $("#ctl00_cntBody_grvAdvancedSearchEmployees_ctl01_chkAdvancedSearchSelectedAllEmployee").prop('checked', true);
            } else {
                $("#ctl00_cntBody_grvAdvancedSearchEmployees_ctl01_chkAdvancedSearchSelectedAllEmployee").removeAttr('checked');
                $("#ctl00_cntBody_grvAdvancedSearchEmployees_ctl01_chkAdvancedSearchSelectedAllEmployee").prop('checked', false);
            }
        }

        function ReviewDates() {
            /// <summary>Manage the logic between start and end date and also re calculate days by Course duration</summary>
            // if both days are setting
            if (!ValidateDates()) {
                // Recalculate it
                CalculateSetDays();
            }
        }

        function ReviewGrade(txt, event) {
            if (!isNumber(event)) {
                return false;
            }

            var chk = $(txt).closest('tr').find('input[type="checkbox"]');
            if (!chk[0].checked) {
                return false;
            }

            return checkMaxLength(txt, event, 3) && checkMaxValue(txt, event, 100) && checkMinValue(txt, event, 0);
        }

        function ValidateEditLogbook() {
            /// <summary>Validate if the logbook is editable</summary>            
            /// <returns> True if is logbook is editable. False otherwise. </returns>
            if ($('#<%= hdfIsLogbookClosed.ClientID %>').val() != null &&
                $('#<%= hdfIsLogbookClosed.ClientID %>').val().toUpperCase() === "TRUE") {

                return false;
            }

            return true;
        }

        function ValidateSelect(value) {
            if (value != 0) {
                return true;
            }

            return false;
        }

        function ValidateClassroomCapacity() {
            /// <summary>Validate the classroom capacity is not exceeded</summary>
            /// <returns> True if is valid. False otherwise. </returns>
            if ($.isNumeric($.trim($('#<%= txtClassroomCapacity.ClientID %>').text()))) {
                var classroomCapacity = parseInt($.trim($('#<%= txtClassroomCapacity.ClientID %>').text()));
                var participantCount = $('.participantEntry').length;

                if (participantCount > classroomCapacity) {
                    return false;
                }
            }

            return true;
        }

        function ValidateDates() {
            /// <summary>Validate the dates of start and end</summary>            
            /// <returns> True if is valid. False otherwise. </returns>
            // if both days are setting
            if ($('#<%= dtpEndDate.ClientID %>').data("DateTimePicker").date() != null
                && $('#<%= dtpStartDate.ClientID %>').data("DateTimePicker").date() != null) {
                // Get the values
                var endDate = $('#<%= dtpEndDate.ClientID %>').data("DateTimePicker").date();
                var starDate = $('#<%= dtpStartDate.ClientID %>').data("DateTimePicker").date();

                //if start date is after end date
                if (starDate.isAfter(endDate, 'day')) {
                    // Recalculate it
                    return false;
                }
            }

            if ($('#<%= dtpEndDate.ClientID %>').data("DateTimePicker").date() == null
                && $('#<%= dtpStartDate.ClientID %>').data("DateTimePicker").date() != null) {
                // Recalculate it
                return false;
            }

            return true;
        }

        function ReturnBitacoraPage() {
            window.location.href = "TrainingLogbooks.aspx?Type=N";
        }

        function SetControlInvalid(controlId) {
            /// <summary>Set the appearance of the control as invalid </summary>
            /// <param name="controlId" type="String">Id of the control</param>
            if (controlId == "txtSearchParticipants") {
                $('.classroom-capacity-alert').show();
                $('.lblRegistrationGradesInformationSubtitle').addClass("Invalid");
            }

            else {
                $('#' + controlId).addClass("Invalid");
                $('label[for=' + controlId + '].label-validation').show();
            }
        }

        function SetControlValid(controlId) {
            /// <summary>Set the appearance of the control as valid </summary>
            /// <param name="controlId" type="String">Id of the control</param>
            if (controlId == "txtSearchParticipants") {
                $('.classroom-capacity-alert').hide();
                $('.lblRegistrationGradesInformationSubtitle').removeClass("Invalid");
            }

            else {
                $('#' + controlId).removeClass("Invalid");
                $('label[for=' + controlId + '].label-validation').hide();
            }
        }

        //*******************************//
        //           PROCESS             //
        //*******************************//
        var delayForSearchEmployeesPostBack = null;
        function SetDelayForSearchEmployeesPostBack() {
            /// <summary>Set a timer for delay the search employees post back while users writes</summary>
            if (delayForSearchEmployeesPostBack != null) {
                clearTimeout(delayForSearchEmployeesPostBack);
            }

            delayForSearchEmployeesPostBack = setTimeout("SearchEmployeesPostBack()", 1800);
        }

        var delayForAdvancedSearchEmployeesPostBack = null;
        function SetDelayForAdvancedSearchEmployeesPostBack() {
            /// <summary>Set a timer for delay the search employees post back while users writes</summary>
            if (delayForAdvancedSearchEmployeesPostBack != null) {
                clearTimeout(delayForAdvancedSearchEmployeesPostBack);
            }

            delayForAdvancedSearchEmployeesPostBack = setTimeout("AdvancedSearchEmployeesPostBack()", 1800);
        }

        function ProcessEditRequestLogbook(resetId) {
            /// <summary>Process the edit request according to the validation of logbook editable state</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            if (ValidateEditLogbook()) {
                __doPostBack('<%= btnEdit.UniqueID %>', '')
            }

            else {
                setTimeout(function () { $("#" + resetId).button('reset'); }, 100);

                MostrarMensaje(
                    TipoMensaje.VALIDACION, '<%= GetLocalResourceObject("msjLogbookNotEditableClosed") %>', null);

                return false;
            }
        }

        function ProcessSaveRequestLogbook(resetId) {
            /// <summary>Process the save request according to the validation of logbook</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            var zeroGrades = TestForZeroGrades();
            var evaluationRequired = $("#<%=hdfNoteRequired.ClientID %>").val();

            if (!ValidateForm()) {
                ResetButton(resetId);
                return false;
            }

            else if (zeroGrades > 0 && evaluationRequired == 'True') {
                return ShowConfirmationMessageSaveWithZeroGrades(resetId);
            }

            else {
                return ShowConfirmationMessageSave(resetId);
            }
        }

        function ProcessPrintLogbook(resetId) {
            /// <summary>Process the save request according to the validation of logbook</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            let geographicDivisionCode = $("#<%=hdfGeographicDivisionCode.ClientID%>").val();
            let logbook = $("#<%=txtLogbookNumber.ClientID%>").val();
            let type = $("#<%=hdfTypeLogbook.ClientID%>").val();
            let user = $("#<%=hdfUserAccount.ClientID%>").val();

            var request = geographicDivisionCode + "|" + logbook + "|" + type + "|" + user;

            window.open('../../api/LogbookFiles/DownloadPrintLogbook/' + request, '_blank');
        }

        function ProcessEmployeesSearch(resetId) {
            if (resetId === 'Active') {
                $('#<%=btnSearchEmployeesInfo.ClientID%>').text('<%=GetLocalResourceObject("lnkbtnEmployeesActive")%>');
                $('#<%=hdfTypeEmployeesSearch.ClientID%>').val("A");
                __doPostBack('<%= btnSearchEmployees.UniqueID %>', '')
            }
            else if (resetId === 'Inactive') {
                $('#<%=btnSearchEmployeesInfo.ClientID%>').text('<%=GetLocalResourceObject("lnkbtnEmployeesInactive")%>');
                $('#<%=hdfTypeEmployeesSearch.ClientID%>').val("I");
                __doPostBack('<%= btnSearchEmployees.UniqueID %>', '')
            }

            $('#tableAddParticipants').find("tr:gt(0)").remove();

            return false;
        }

        function ProcessValidateClassroomCapacity() {
            /// <summary>>Process the UI according to classroom capacity validation</summary>
            $('.lblRegistrationGradesInformationSubtitle').removeClass("Invalid");

            if (ValidateClassroomCapacity()) {
                $('.classroom-capacity-alert').hide();
            }

            else {
                $('.classroom-capacity-alert').show();
            }
        }

        function ProcessAdvancedSearchRequest(resetId) {
            var uniqueId = resetId.replace(/_/g, '$');

            if (!ValidateFormStruct()) {
                setTimeout(function () {
                    ResetButton(resetId);
                }, 150);
            }

            else {
                __doPostBack(uniqueId, '');
            }

            return false;
        }

        //*******************************//
        //  RETURN FROM POSTBACKS LOGIC  // 
        //*******************************//
        function ReturnFromBtnEditClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>    
            ShowFooterSuccess('<%= GetLocalResourceObject("msgOperationSuccesfull") %>');
        }

        function ReturnFromSearchLogbookPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>            
            ShowFooterSuccess('<%= GetLocalResourceObject("msgOperationSuccesfull") %>');
        }

        function ReturnFromBtnDeleteClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>          
            ReturnBitacoraPage();
        }

        function ReturnFromBtnSaveClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>            
            ShowFooterSuccess('<%= GetLocalResourceObject("msgOperationSuccesfull") %>');
        }

        function ReturnFromSearchEmployeesPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>            
            $('#<%= txtSearchEmployees.ClientID %>').prop("disabled", false);
            $("#txtSearchEmployeesWaiting").hide();
        }

        function ReturnFromAddParticipantPostBack(btnParticipantId) {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            var count = parseInt($("#countParticipants").html()) - 1;
            var countmin = parseInt($("#countminParticipants").html());

            if (count < countmin) {
                $("#countminParticipants").html(count);
            }

            $("#countParticipants").html(count);

            $("#LabelParticipantesBuscar").val($("#<%=lblSearchEmployeesResults.ClientID%>").html())

            $("#" + btnParticipantId).closest("tr").fadeTo("slow", 0.2, function () {
                $(this).remove();
            })

            ProcessValidateClassroomCapacity();
            DisableAlGrades();
        }

        function ReturnFromRemoveParticipantPostBack(hdfParticipantId) {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            setTimeout(function () {
                __doPostBack('<%= btnSearchEmployees.UniqueID %>', '')
            }, 100);

            $("#" + hdfParticipantId).closest("tr").fadeTo("slow", 0.2, function () {
                $(this).remove();
                ProcessValidateClassroomCapacity();
            });

            setTimeout(function () { __doPostBack('<%= btnRefreshParticipants.UniqueID %>', ''); }, 1000);
        }

        function ReturnFromSaveGradePostBack(txtGradeId) {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            TestGradeState(txtGradeId)
        }

        function ReturnFromCourseChangedPostBack(senderId) {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            ClearGrades();
            setTimeout(function () {
                CalculateSetDays();
                ReviewDates();
            }, 100);
        }
        function ClearGrades() {
            /// <summary>Clear the grades</summary>
            $('.txtGrade').each(function () {
                $(this).val(''); 
                var gradeSpan = $(this).closest('tr').find('span.grade');
                gradeSpan.removeClass('label-success').addClass('label-danger'); 
                gradeSpan.text(''); 
            });
        }


        function ReturnFromClassroomChangedPostBack(senderId) {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            ProcessValidateClassroomCapacity();
        }

        function ReturnFromBtnAcceptClickPostBackDuplicated() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $('#DuplicatedDialog').modal('show');
        }

        function ReturnPostBackAcceptClickSave() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $('#AdvancedSearchDialog').modal('hide');
            ValidateForm();

            ShowFooterSuccess('<%= GetLocalResourceObject("msgOperationSuccesfull") %>');
        }

        function AdvancedSearchEmployeesPostBack() {
            /// <summary>Execues the search employees post back</summary>
            $("#txtAdvancedSearchEmployeesWaiting").show();

            $('#<%= txtAdvancedSearchEmployees.ClientID %>').prop("disabled", true);
            __doPostBack("<%= txtAdvancedSearchEmployees.UniqueID %>", '');
        }

        function SearchEmployeesPostBack() {
            /// <summary>Execues the search employees post back</summary>
            $("#txtSearchEmployeesWaiting").show();

            $('#<%= txtSearchEmployees.ClientID %>').prop("disabled", true);
            __doPostBack("<%= txtSearchEmployees.UniqueID %>", '');
        }

        function ReturnRefreshAdvancedSearch() {
            MultiSelectDropdownListRestoreSelectedItems($("#<%=CostCenterIdEdit.ClientID %>"), $("#<%=CostCenterIdEditMultiple.ClientID%>"));
        }

        //*******************************//
        // MESSAGING AND CONFIRMATION    // 
        //*******************************//
        function ShowConfirmationMessageSave(resetId) {
            /// <summary>Show confirmation message for Save and Close funtionality</summary>
            /// <param name="resetId" type="String">Id of the button to reset if cancel</param>
            /// <returns>False</returns>
            MostrarConfirmacion(
                '<%= GetLocalResourceObject("msgConfirmationSave") %>', '<%=GetLocalResourceObject("Si")%>', function () {
                    __doPostBack('<%= btnSave.UniqueID %>', '')
            }, '<%=GetLocalResourceObject("No")%>', function () {
                $("#" + resetId).button('reset');
            });

            return false;
        }

        function ShowConfirmationMessageSaveWithZeroGrades(resetId) {
            /// <summary>Show message for Save and Close funtionality with grades in zero</summary>
            /// <param name="resetId" type="String">Id of the button to reset if cancel</param>
            /// <returns>False</returns>

            setTimeout(function () { $("#" + resetId).button('reset'); }, 100);

            MostrarMensaje(
                TipoMensaje.ERROR, '<%= GetLocalResourceObject("msgConfirmationSaveWithZeroGrades") %>', null);

            return false;
        }

        function ShowConfirmationMessageDelete(resetId) {
            /// <summary>Show confirmation message for Delete funtionality</summary>
            /// <param name="resetId" type="String">Id of the button to reset if cancel</param>
            /// <returns>False</returns>
            MostrarConfirmacion(
                '<%= GetLocalResourceObject("msjDelete") %>', '<%=GetLocalResourceObject("Si")%>', function () {
                    __doPostBack('<%= btnDelete.UniqueID %>', '')
            }, '<%=GetLocalResourceObject("No")%>', function () {
                $("#" + resetId).button('reset');
            });

            return false;
        }

        function ShowConfirmationMessageNew(resetId) {
            /// <summary>Show confirmation message for New logbook funtionality</summary>
            /// <param name="resetId" type="String">Id of the button to reset if cancel</param>
            /// <returns>False</returns>
            MostrarConfirmacion(
                '<%= GetLocalResourceObject("msgConfirmationNew") %>', '<%=GetLocalResourceObject("Si")%>', function () {
                    __doPostBack('<%= btnNew.UniqueID %>', '')
            }, '<%=GetLocalResourceObject("No")%>', function () {
                $("#" + resetId).button('reset');
            });

            return false;
        }

        function ShowConfirmationMessageNewFromThis(resetId) {
            /// <summary>Show confirmation message for New from this logbook funtionality</summary>
            /// <param name="resetId" type="String">Id of the button to reset if cancel</param>
            /// <returns>False</returns>
            MostrarConfirmacion(
                '<%= GetLocalResourceObject("msgConfirmationNewFromThis") %>', '<%=GetLocalResourceObject("Si")%>', function () {
                    __doPostBack('<%= btnNewFromThis.UniqueID %>', '')
            }, '<%=GetLocalResourceObject("No")%>', function () {
                $("#" + resetId).button('reset');
            });

            return false;
        }

        function ShowConfirmationMessageRemoveParticipant(resetId) {
            /// <summary>Show confirmation message for remove participant funtionality</summary>
            /// <param name="resetId" type="String">Id of the button to reset if cancel</param>
            /// <returns>False</returns>
            MostrarConfirmacion(
                '<%= GetLocalResourceObject("msgConfirmationRemoveParticipant") %>', '<%=GetLocalResourceObject("Si")%>', function () {
                    var postbackArg = resetId.replace(/_/g, "$");
                    __doPostBack(postbackArg, '')
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

                console.log(args.get_postBackElement().id);

                //this line was initially included but causes that the original postback being cancel too
                if (args.get_postBackElement().id != "ctl00_cntBody_lnkbtnAdvancedSearch" && !args.get_postBackElement().id.includes("IdEdit")) {
                    ShowFooterAlert('<%=GetLocalResourceObject("msgWaitWhileProcessing")%>');
                }

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
