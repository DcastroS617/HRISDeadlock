<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="TrainingLogbooks.aspx.cs" Inherits="HRISWeb.Training.TrainingLogbooks" %>

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
                            <div class="col-sm-4">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%= txtLogbookNumberFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblLogbookNumber")%></label>
                                        </div>

                                        <div class="col-sm-7">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtLogbookNumberFilter" Style="width: 40%;" CssClass="form-control control-validation cleanPasteDigits" runat="server" autocomplete="off" MaxLength="9" type="number" min="0" data-id="LogbookNumber" data-value="isPermitted"></asp:TextBox>
                                                <asp:HiddenField ID="hdfLogbookNumberFilter" runat="server" />
                                                
                                                <asp:DropDownList ID="cboStatus" Style="width: 60%;" CssClass="form-control control-validation" AutoPostBack="false" runat="server" data-id="Status" data-value="isPermitted"></asp:DropDownList>
                                                <asp:HiddenField ID="hdfStatusValueFilter" runat="server" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=cboTrainerFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblTrainer")%></label>
                                        </div>

                                        <div class="col-sm-7">
                                            <asp:DropDownList ID="cboTrainerFilter" CssClass="form-control control-validation selectpicker" AutoPostBack="false" runat="server" data-live-search="true" data-actions-box="true" data-id="Trainer" data-value="isPermitted"></asp:DropDownList>
                                            <asp:HiddenField ID="hdfTrainerValueFilter" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-4">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="dtpStartDate" class="control-label"><%=GetLocalResourceObject("lblStartDate")%></label>
                                        </div>

                                        <div class="col-sm-7">
                                            <div class="input-group">
                                                <asp:TextBox runat="server" ID="dtpStartDateFilter" class="form-control date control-validation cleanPasteDigits" placeholder='<%$ Code:GetLocalResourceObject("lblDateFormatPlaceHolder") %>' type="text" autocomplete="off" data-id="StartDate" data-value="isPermitted"/>
                                                <asp:HiddenField ID="hdfStartDateFilter" runat="server" />

                                                <label id="dtpEndStartValidation" for="<%= dtpStartDateFilter.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjStartDateValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                <div class="input-group-addon">
                                                    <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=cboCourseFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCourse")%></label>
                                        </div>

                                        <div class="col-sm-7">
                                            <asp:DropDownList ID="cboCourseFilter"  CssClass="form-control cboAjaxAction control-validation selectpicker" AutoPostBack="false" runat="server" data-live-search="true" data-actions-box="true" data-id="Course" data-value="isPermitted"></asp:DropDownList>
                                            <asp:HiddenField ID="hdfCourseValueFilter" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-4">
                                <div class="form-horizontal">                                    
                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="dtpEndDate" class="control-label"><%=GetLocalResourceObject("lblEndDate")%></label>
                                        </div>

                                        <div class="col-sm-7">
                                            <div class="input-group">
                                                <asp:TextBox runat="server" ID="dtpEndDateFilter" class="form-control date control-validation cleanPasteDigits" placeholder='<%$ Code:GetLocalResourceObject("lblDateFormatPlaceHolder") %>' type="text" autocomplete="off" data-id="EndDate" data-value="isPermitted"/>
                                                <asp:HiddenField ID="hdfEndDateFilter" runat="server" />
                                                
                                                <label id="dtpEndDateValidation" for="<%= dtpEndDateFilter.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjEndDateValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                <div class="input-group-addon">
                                                    <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=cboTrainingCenterFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblTrainingCenter")%></label>
                                        </div>

                                        <div class="col-sm-7">
                                            <asp:DropDownList ID="cboTrainingCenterFilter" CssClass="form-control cboAjaxAction control-validation selectpicker" AutoPostBack="false" runat="server" data-live-search="true" data-actions-box="true" data-id="TrainingCenter" data-value="isPermitted"></asp:DropDownList>
                                            <asp:HiddenField ID="hdfTrainingCenterValueFilter" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-4">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=cboExistFilesFilter.ClientID%>" class="control-label" style="text-align: left"><%=GetLocalResourceObject("lblExistFiles")%></label>
                                        </div>

                                        <div class="col-sm-7">
                                            <asp:DropDownList ID="cboExistFilesFilter" CssClass="form-control cboAjaxAction control-validation selectpicker" AutoPostBack="false" runat="server" data-live-search="true" data-actions-box="true"></asp:DropDownList>
                                            <asp:HiddenField ID="hdfExistFilesValueFilter" runat="server" />                                     
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
                                <asp:HiddenField runat="server" ID="hdfSelectedFileIndex" Value="" />
                                <asp:HiddenField runat="server" ID="hdfSelectedFileValues" Value="" />
                                <asp:HiddenField runat="server" ID="hdfCurrentUser" Value="" />

                                <div>
                                    <asp:GridView ID="grvList"
                                        Width="100%"
                                        runat="server"
                                        EmptyDataText='<%$ Code:GetLocalResourceObject("lblNoData") %>'
                                        EmptyDataRowStyle-CssClass="emptyRow"
                                        AllowPaging="false" PagerSettings-Visible="false" AllowSorting="true"
                                        AutoGenerateColumns="false" ShowHeader="true"
                                        CssClass="table table-striped table-hover table-bordered"
                                        DataKeyNames="LogbookNumber,IsClosed,Status,CreatedBy"
                                        OnPreRender="GrvList_PreRender" OnSorting="GrvList_Sorting">
                                        <Columns>
                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="LogbookNumberSort" runat="server" CommandName="Sort" CommandArgument="LogbookNumber" OnClientClick="SetWaitingGrvList(true);">                
                                                            <span><%= GetLocalResourceObject("LogbookNumber.HeaderText") %></span><i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "LogbookNumber") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvLogbookNumber" data-id="LogbookNumber" data-value="<%# Eval("LogbookNumber") %>"><%# Eval("LogbookNumber") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="TrainerSort" runat="server" CommandName="Sort" CommandArgument="Trainer" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("Trainer.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "Trainer") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvTrainer" data-id="Trainer" data-value="<%# Eval("TrainerName") %>"><%# Eval("TrainerName") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="CourseSort" runat="server" CommandName="Sort" CommandArgument="Course" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("Course.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "Course") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvCourse" data-id="Course" data-value="<%# Eval("CourseCodeName") %>"><%# Eval("CourseCodeName") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle hidden-xs" HeaderStyle-CssClass="hidden-xs">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="StartDateSort" runat="server" CommandName="Sort" CommandArgument="StartDate" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("StartDate.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "StartDate") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvStartDate" data-id="StartDate" data-value="<%# Eval("StartDateTimeFormated") %>"><%# Eval("StartDateTimeFormated") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="StatusSort" runat="server" CommandName="Sort" CommandArgument="Status" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("Status.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "Status") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvStatus" data-id="Status" data-value="<%# Eval("Status") %>"><%# Eval("Status") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle hidden-xs hidden-sm" HeaderStyle-CssClass="hidden-xs hidden-sm">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="ParticipantsSort" runat="server" CommandName="Sort" CommandArgument="Participants" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("Participants.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "Participants") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvParticipants" data-id="Participants" data-value="<%# Eval("ParticipantsCount") %>"><%# Eval("ParticipantsCount") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle hidden-xs hidden-sm" HeaderStyle-CssClass="hidden-xs hidden-sm">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="AverageGradeSort" runat="server" CommandName="Sort" CommandArgument="AverageGrade" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("AverageGrade.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "AverageGrade") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvAverageGrade" data-id="AverageGrade" data-value="<%# Eval("AverageGrade") %>"><%# Eval("AverageGrade") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle hidden-xs hidden-sm" HeaderStyle-CssClass="hidden-xs hidden-sm">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="LogbookFileCourseCodeName" runat="server" CommandName="Sort" CommandArgument="LogbookNumber" OnClientClick="SetWaitingGrvList(true);"></asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <div id="files" data-value="<%# Eval("LogbookNumber") %>" data-instructor="<%# Eval("TrainerName") %>" data-curso="<%# Eval("CourseCodeName") %>" data-estado="<%# Eval("Status") %>" data-archivos="<%#Eval("ExistFiles") %>">
                                                        <button id="btnFile" type="button" runat="server" class="btn btn-default btnAjaxAction" 
                                                            onclick="ShowModalFile(this);" onserverclick="BtnDownloadFile_ServerClick" 
                                                            data-loading-text='<%$ Code:String.Concat(GetLocalResourceObject("btnFile"), "&nbsp;<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>")%>' 
                                                            data-error-text='<%$ Code:String.Concat( GetLocalResourceObject("btnFile"), "&nbsp;<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>")%>'>
                                                            <span class="glyphicon glyphicon-file glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%= GetLocalResourceObject("btnFile") %>
                                                        </button>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle hidden-xs hidden-sm" HeaderStyle-CssClass="hidden-xs hidden-sm">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="LogbookFileGeographic" runat="server" CommandName="Sort" CommandArgument="LogbookNumber" OnClientClick="SetWaitingGrvList(true);"></asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <div id="files" data-value="<%# Eval("LogbookNumber") %>" data-geographic="<%# Eval("GeographicDivisionCode") %>" data-type="<%# Eval("LogbookType").ToString().Trim() %>">
                                                        <button id="btnPrint" type="button" runat="server" class="btn btn-default btnAjaxAction"
                                                            onclick="ProcessPrintLogbook(this.id);"
                                                            data-loading-text='<%$ Code:String.Concat(GetLocalResourceObject("btnPrint"), "&nbsp;<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>")%>' 
                                                            data-error-text='<%$ Code:String.Concat( GetLocalResourceObject("btnPrint"), "&nbsp;<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>")%>'>
                                                            <span class="glyphicon glyphicon-print glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%= GetLocalResourceObject("btnPrint") %>
                                                        </button>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="IsClosed" Visible="false" />
                                            <asp:BoundField DataField="CreatedBy" Visible="false" />
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
                            onserverclick="BtnAdd_ServerClick"
                            data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnAdd"))%>'
                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnAdd"))%>'>
                            <span class="glyphicon glyphicon-plus glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnAdd") %>
                        </button>

                        <button id="btnEdit" type="button" runat="server" class="btn btn-default btnAjaxAction"
                            onserverclick="BtnEdit_ServerClick" onclick="return ProcessEditRequestLogbook(this.id);"
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

    <nav class="navbar-fixed-bottom">
        <div class="container center-block text-center">
            <b>
                <div class="alert alert-autocloseable-msg" style="display: none;"></div>
            </b>
        </div>
    </nav>

    <%--  Modal  --%>
    <div class="modal fade" id="FileDialog" tabindex="-1" role="dialog" aria-labelledby="FileDialogTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg" style="margin-top: 4px;" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button id="btnFileClose" type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title text-primary"><%=Convert.ToString(GetLocalResourceObject("TitleFileDialog")) %></h3>
                </div>

                <asp:UpdatePanel runat="server" ID="uppDuplicatedDialog">
                    <Triggers>
                    </Triggers>

                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="form-horizontal">
                                <div id="divFileDialogText" runat="server"></div>

                                <div class="row">
                                    <h4 class="text-left text-primary" style="padding-left: 15px;"><%= GetLocalResourceObject("lblsubtitulofile") %></h4>
                                    <br />

                                    <div class="col-sm-12">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <div class="row">
                                                    <div class="col-sm-12 col-md-6">
                                                        <div class="col-sm-5 text-left">
                                                            <label for="<%=txtFileLogbookNumber.ClientID%>" class="control-label"><%=GetLocalResourceObject("LogbookNumber.HeaderText")%></label>
                                                        </div>

                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtFileLogbookNumber" CssClass="form-control control-validation" runat="server" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="col-sm-12 col-md-6">
                                                        <div class="col-sm-3 text-left">
                                                            <label for="<%=txtFileTrainer.ClientID%>" class="control-label"><%=GetLocalResourceObject("Trainer.HeaderText")%></label>
                                                        </div>

                                                        <div class="col-sm-9">
                                                            <asp:TextBox ID="txtFileTrainer" CssClass="form-control control-validation" runat="server" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <div class="row">
                                                    <div class="col-sm-12 col-md-6">
                                                        <div class="col-sm-5 text-left">
                                                            <label for="<%=txtFileCourse.ClientID%>" class="control-label"><%=GetLocalResourceObject("Course.HeaderText")%></label>
                                                        </div>

                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtFileCourse" CssClass="form-control control-validation" runat="server" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="col-sm-12 col-md-6">
                                                        <div class="col-sm-3 text-left">
                                                            <label for="<%=txFileStatus.ClientID%>" class="control-label"><%=GetLocalResourceObject("Status.HeaderText")%></label>
                                                        </div>

                                                        <div class="col-sm-9">
                                                            <asp:TextBox ID="txFileStatus" CssClass="form-control control-validation" runat="server" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <asp:HiddenField runat="server" ID="hdfSelectedFileUsing" />

                                    <div class="col-lg-12">
                                        <asp:Panel ID="pnlFileDialogDataDetail" runat="server">
                                            <asp:Repeater ID="rptFiles" runat="server">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; max-height: 38vh; overflow-y: auto; -ms-overflow-style: scrollbar;">
                                                        <table id="tableLogbookFiles" class="table table-hover table-striped">
                                                            <thead>
                                                                <tr>
                                                                    <th>
                                                                        <div>
                                                                            <div class="col-xs-2 col-sm-2 text-primary" style="cursor: pointer; text-align: left;"><%= GetLocalResourceObject("LogbookFileFecha") %> </div>
                                                                            <div class="col-xs-3 col-sm-3 text-primary" style="cursor: pointer; text-align: left;"><%= GetLocalResourceObject("LogbookFileDescripcion") %> </div>
                                                                            <div class="col-xs-3 col-sm-3 text-primary" style="cursor: pointer; text-align: left;"><%= GetLocalResourceObject("LogbookFileName") %> </div>                                                                          
                                                                            <div class="col-xs-2 col-sm-2 text-primary" style="cursor: pointer; text-align: center;"><%= GetLocalResourceObject("lblDownloadFileHeader") %>  </div>
                                                                            <div class="col-xs-2 col-sm-2 text-primary" style="cursor: pointer; text-align: center;"><%= GetLocalResourceObject("lblDeleteFileHeader") %>  </div>
                                                                        </div>
                                                                    </th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <tr>
                                                        <td class="row-fluid">
                                                            <asp:UpdatePanel runat="server" ID="uppFiles" UpdateMode="Conditional">
                                                                <Triggers>
                                                                </Triggers>
                                                                <ContentTemplate>
                                                                    <div class="data-sort-src">
                                                                        <div class="col-xs-2 col-sm-2">
                                                                            <span>
                                                                                <%# Eval("LastModifiedDate") %>
                                                                            </span>
                                                                        </div>

                                                                        <div class="col-xs-3 col-sm-3">
                                                                            <span>
                                                                                <%# Eval("Description") %>
                                                                            </span>
                                                                        </div>

                                                                        <div class="col-xs-3 col-sm-3">
                                                                            <span>
                                                                                <%# Eval("FileName") %>
                                                                            </span>
                                                                        </div>

                                                                        <div class="col-xs-2 col-sm-2" style="text-align: center;" data-value="<%# Eval("LogbooksFileId") %>">
                                                                            <button id="btnDownloadFile" runat="server" type="button" class="btn btn-success btnAjaxAction" 
                                                                                onclick="return ProcessDownloadFileRequest(this.id);" 
                                                                                data-loading-text='<%$ Code:String.Concat("", "&nbsp;<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>")%>' 
                                                                                data-error-text='<%$ Code:String.Concat("", "&nbsp;<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>")%>'>
                                                                                <span class="glyphicon glyphicon-download glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                                                                            </button>
                                                                        </div>

                                                                        <div class="col-xs-2 col-sm-2" style="text-align: center;" data-value="<%# Eval("LogbooksFileId") %>">
                                                                            <button id="btnDeleteFile" runat="server" type="button" class="btn btn-danger btnAjaxAction" 
                                                                                onserverclick="BtnDeleteFile_ServerClick"  onclick="return ProcessDeleteFileRequest(this.id);"
                                                                                data-loading-text='<%$ Code:String.Concat("", "&nbsp;<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>")%>' 
                                                                                data-error-text='<%$ Code:String.Concat( "", "&nbsp;<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>")%>'>
                                                                                <span class="glyphicon glyphicon-trash glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
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
                                                            </div>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                            <hr />

                                            <div class="form-horizontal">
                                                <div class="form-group">
                                                    <div class="col-sm-2 text-left">
                                                        <label for="<%=LogbookFileDescripcion.ClientID%>" class="control-label"><%=GetLocalResourceObject("LogbookFileDescripcion")%></label>
                                                    </div>

                                                    <div class="col-sm-10">
                                                        <asp:TextBox ID="LogbookFileDescripcion" CssClass="form-control control-validation" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <div class="col-sm-2 text-left">
                                                        <label for="<%=LogbookfileUp.ClientID%>" class="control-label"><%=GetLocalResourceObject("File")%></label>
                                                    </div>

                                                    <div class="col-sm-10">
                                                        <asp:FileUpload ID="LogbookfileUp" runat="server" data-input-type="archivo" CssClass="file" data-show-preview="false" data-show-upload="false" data-allowed-file-extensions='["pdf","doc", "docx","xlsx"]' />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <button id="btnAddFile" onserverclick="BtnAddFile_ServerClick" type="button" runat="server" class="btn btn-default btnAjaxAction" data-loading-text='<%$ Code:String.Concat(GetLocalResourceObject("btnAddFileValue"), "&nbsp;<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>")%>' data-error-text='<%$ Code:String.Concat( GetLocalResourceObject("btnAddFileValue"), "&nbsp;<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>")%>'>
                                                        <span class="glyphicon glyphicon-floppy-disk glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%= GetLocalResourceObject("btnAddFileValue") %>
                                                    </button>
                                                </div>
                                            </div>

                                            <div style="display: none;">
                                                <a id="filedownloadlogbook" href="" download></a>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="modal-footer">
                            <button id="btnCloseModal" type="button" class="btn btn-default">
                                <span class="glyphicon glyphicon-remove glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnClose")) %>
                            </button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

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

            ChangeTypeButtonForLogbookFile();
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

                ChangeTypeButtonForLogbookFile();
            });

            $('#<%= btnSearchDefault.ClientID %>').on('click', function (event) {
                var $this = $('#<%= btnSearch.ClientID %>');
                $this.button('loading');

                setTimeout(function () {
                    $this.button('reset');
                }, 30000);
            });
            //////////////////////////////////////////////////////////////////////////////////////////////////

            //In this section we set the button generics functionality
            $('#btnFileClose, #btnCloseModal').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                event.preventDefault();
                $('#FileDialog').modal('hide');
            });

            $('#<%= txtLogbookNumberFilter.ClientID %>').on('keyup keypress', function (e) {
                ReturnFromBtnSearchClickPostBack(e);

                return isNumber(e) && checkMinValue(this, e, 0) && checkMaxLength(this, e, 9);
            });

            $('#<%= dtpStartDateFilter.ClientID %>').on('keyup keypress', function (e) {
                ReturnFromBtnSearchClickPostBack(e);
            });

            $('#<%= dtpStartDateFilter.ClientID %>').on("blur", function () {
                var ValidDateVal = $(this).data("DateTimePicker").date().format('MM/DD/YYYY');

                $(this).val(ValidDateVal);
                ReviewDates();
            });

            $('#<%= dtpStartDateFilter.ClientID %>').datetimepicker({
                format: 'MM/DD/YYYY',
            });

            $('#<%= cboCourseFilter.ClientID %>').on('keyup keypress', function (e) {
                ReturnFromBtnSearchClickPostBack(e);
            });

            $('#<%= cboTrainerFilter.ClientID %>').on('keyup keypress', function (e) {
                ReturnFromBtnSearchClickPostBack(e);
            });

            $('#<%= dtpEndDateFilter.ClientID %>').on('keyup keypress', function (e) {
                ReturnFromBtnSearchClickPostBack(e);
            });

            $('#<%= dtpEndDateFilter.ClientID %>').datetimepicker({
                format: 'MM/DD/YYYY',
            });

            $('#<%= dtpEndDateFilter.ClientID %>').on("blur", function () {
                var ValidDateVal = $(this).data("DateTimePicker").date().format('MM/DD/YYYY');

                $(this).val(ValidDateVal);

                ReviewDates();
            });

            $('#<%= cboTrainingCenterFilter.ClientID %>').on('keyup keypress', function (e) {
                ReturnFromBtnSearchClickPostBack(e);
            });

            $('#<%= cboExistFilesFilter.ClientID %>').on('keyup keypress', function (e) {
                ReturnFromBtnSearchClickPostBack(e);
            });

            $('#<%= cboStatus.ClientID %>').on('keyup keypress', function (e) {
                ReturnFromBtnSearchClickPostBack(e);
            });

            //////////////////////////////////////////////////////////////////////////////////////////////////

            //In this section we set up the external modal functionality
            $(".file").fileinput({
                'showUpload': true,
                'showCancel': false,
                'showRemove': false,
                'allowedFileExtensions': ['pdf', 'docx', 'doc', 'xlsx', 'xls', 'jpeg', 'jpg', 'txt', 'png', 'csv', 'ppt', 'pptx', 'xlsm'],
                'hiddenThumbnailContent': false,
                'showPreview': false,
                'showUploadedThumbs': false,
                'uploadAsync': false,
                'showBrowse': true,
                'showCaption': true,
                browseOnZoneClick: true,
                'initialPreviewAsData': false,
                'maxFileCount': 1,
                'language': 'es'
            }).on("click", function (event) {
                return true;
            })
                .on("filebatchselected", function (event, files) {
                    var documentType = "1";
                    var company = "1";
                    var adamCase = "1";
                    var fileUpload = ($(this)).get(0);
                    var files = fileUpload.files;

                    var data = new FormData();
                    for (var i = 0; i < files.length; i++) {
                        data.append(files[i].name, files[i]);
                    }

                    data.append('company', company);
                    data.append('adamCase', adamCase);
                    if (documentType != "") {
                        $.ajax({
                            url: "/Shared/FileUploadHandler.ashx",
                            type: "POST",
                            data: data,
                            contentType: false,
                            processData: false,
                            success: function (result) { },
                            error: function (err) {
                                showClientMessage(MessageType.Error, err.responseText, null);
                            }
                        });
                    }
                })
                .on('fileclear', function () {
                    var documentType = "-1";
                    var company = "1";
                    var adamCase = "1";
                    var rutaArchivo = ($(this)).val();
                    var data = new FormData();

                    data.append('company', company);
                    data.append('adamCase', adamCase);

                    $.ajax({
                        url: "/Shared/FileUploadHandler.ashx",
                        type: "POST",
                        data: data,
                        contentType: false,
                        processData: false,
                        success: function (result) { },
                        error: function (err) {
                            showClientMessage(MessageType.Error, err.responseText, null);
                        }
                    });

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

            SetRowSelected();
        }

        //*******************************//
        //          VALIDATION           // 
        //*******************************//
        function ValidateDates() {
            /// <summary>Validate the dates of start and end</summary>            
            /// <returns> True if is valid. False otherwise. </returns>

            // if both days are setting
            if ($('#<%= dtpEndDateFilter.ClientID %>').data("DateTimePicker").date() != null
                && $('#<%= dtpStartDateFilter.ClientID %>').data("DateTimePicker").date() != null) {
                // Get the values
                var endDate = $('#<%= dtpEndDateFilter.ClientID %>').data("DateTimePicker").date();
                var starDate = $('#<%= dtpStartDateFilter.ClientID %>').data("DateTimePicker").date();

                //if start date is after end date
                if (starDate.isAfter(endDate, 'day')) {
                    // Recalculate it
                    return false;
                }
            }

            return true;
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

        function ReviewDates() {
            /// <summary>Manage the logic between start and end date and also re calculate days by subject duration</summary>
            if (!ValidateDates()) {
                CalculateSetDays();
            }
        }

        function CalculateSetDays() {
            /// <summary>Manage the logic between start and end date
            if ($('#<%= dtpEndDateFilter.ClientID %>').data("DateTimePicker").date() != null
                && $('#<%= dtpStartDateFilter.ClientID %>').data("DateTimePicker").date() != null) {

                var endDate = $('#<%= dtpEndDateFilter.ClientID %>').data("DateTimePicker").date();
                var starDate = $('#<%= dtpStartDateFilter.ClientID %>').data("DateTimePicker").date();

                $('#<%= dtpEndDateFilter.ClientID %>').data("DateTimePicker").date(starDate);

                // Se muestra el mensaje al usuario
                AddTemporaryClass($('#<%= dtpStartDateFilter.ClientID %>'), "btn-info", 1500);
                AddTemporaryClass($('#<%= dtpEndDateFilter.ClientID %>'), "btn-info", 1500);
            }
        }

        function ShowModalFile(btn) {
            var id = $(btn).parent().data("value");
            var instructor = $(btn).parent().data("instructor");
            var CourseCodeName = $(btn).parent().data("curso");
            var Status = $(btn).parent().data("estado");

            $("#<%=hdfSelectedFileValues.ClientID%>").val(instructor + "," + CourseCodeName + "," + Status);

            $('#FileDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceAdd") %>');
            $('#FileDialog').modal('show');

            $("#<%=hdfSelectedFileIndex.ClientID%>").val(id);
        }

        function ChangeTypeButtonForLogbookFile() {
            $('#<%= grvList.ClientID %> div#files').map(function (i, element) {
                let existFile = $(element).data("archivos");
                
                if (existFile == "True") {
                    $(element).find("button").removeClass('btn-default');
                    $(element).find("button").addClass('btn-info');
                } else {
                    $(element).find("button").removeClass('btn-info');
                    $(element).find("button").addClass('btn-default');
                }
            });
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
        function ProcessEditRequestLogbook(resetId) {
            /// <summary>Process the edit request according to the validation of logbook selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            if (IsRowSelected()) {
                __doPostBack('<%= btnEdit.UniqueID %>', '')
                return true;
            }

            else {
                ShowFooterError('<%= GetLocalResourceObject("msgInvalidSelection") %>')
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

        function ProcessPrintLogbook(resetId) {
            /// <summary>Process the save request according to the validation of logbook</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            setTimeout(function () {
                $("#" + resetId).button('reset');
            }, 500);

            let geographicDivisionCode = $("#" + resetId).parent().data("geographic");
            let logbook = $("#" + resetId).parent().data("value");
            let type = $("#" + resetId).parent().data("type");
            let user = $('#<%= hdfCurrentUser.ClientID %>').val();

            let request = geographicDivisionCode + "|" + logbook + "|" + type + "|" + user;

            window.open('../../api/LogbookFiles/DownloadPrintLogbook/' + request, '_blank');
        }

        function ProcessDownloadFileRequest(resetId) {
            /// <summary>Process the delete request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            setTimeout(function () {
                $("#" + resetId).button('reset');
            }, 500);

            let request = $("#" + resetId).parent().data("value");

            window.location.href = '../../api/LogbookFiles/DownloadFileLogbook/' + request;
        }

        function ProcessDeleteFileRequest(resetId) {
            /// <summary>Process the delete request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>            
            ShowConfirmationMessageDeleteFile(resetId);
            return false;
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
                '<%=GetLocalResourceObject("Si")%>', function () {
                    __doPostBack('<%= btnDelete.UniqueID %>', '')
                }, '<%=GetLocalResourceObject("No")%>', function () {
                    $("#" + resetId).button('reset');
                }
            );

            return false;
        }

        function ShowConfirmationMessageDeleteFile(resetId) {
            /// <summary>Show confirmation message for Save and Close funtionality</summary>
            /// <param name="resetId" type="String">Id of the button to reset if cancel</param>
            /// <returns>False</returns>
            MostrarConfirmacion(
                '<%= GetLocalResourceObject("msjDeleteFile") %>',
                '<%=GetLocalResourceObject("Si")%>', function () {
                    let logbooksFileId = $("#" + resetId).parent().data("value");
                   
                    $('#<%= hdfSelectedFileUsing.ClientID %>').val(logbooksFileId);
                    __doPostBack(resetId.replaceAll('_', '$'), '');
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
