<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Courses.aspx.cs" Inherits="HRISWeb.Training.Courses" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntHeader" runat="server">
    <title><%=Convert.ToString(GetLocalResourceObject("lblScreenTitle")) %></title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cntBody" runat="server">
    <style>
        .modal {
            -ms-overflow-style: none;
        }

            .modal ::-webkit-scrollbar {
                /* This is the magic bit for WebKit */
                display: none !important;
            }
    </style>

    <div class="main-content">
        <asp:Panel ID="pnlMainContent" runat="server" DefaultButton="btnSearchDefault">
            <h1 class="text-left text-primary">
                <label class="PageTitle"><%= GetLocalResourceObject("lblScreenTitle") %></label>
            </h1>
            <br />

            <asp:UpdatePanel runat="server" ID="main">
                <Triggers></Triggers>

                <ContentTemplate>
                    <div class="container" style="width: 100%">
                        <h4 class="text-left text-primary"><%= GetLocalResourceObject("lblFiltersSubtitle") %></h4>
                        <br />

                        <asp:HiddenField ID="hdfDivisionCode" runat="server" />
                        <asp:HiddenField ID="hdfGeographicDivisionCode" runat="server" />

                        <div class="row">
                            <div class="col-sm-12">
                                <div class="form-horizontal">
                                    <div class="row">
                                        <div class="col-sm-12 col-md-6">
                                            <div class="form-group">
                                                <div class="col-sm-5 text-left">
                                                    <label for="<%= txtCourseCodeFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCourseCode")%></label>
                                                </div>

                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtCourseCodeFilter" CssClass="form-control control-validation cleanPasteText ignoreValidation" runat="server" MaxLength="10" autocomplete="off" type="text" data-id="CourseCode" data-value="isPermitted"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfCourseCodeFilter" runat="server" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-sm-12 col-md-6">
                                            <div class="form-group">
                                                <div class="col-sm-5 text-left">
                                                    <label for="<%= txtCourseNameFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCourseName")%></label>
                                                </div>

                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtCourseNameFilter" CssClass="form-control control-validation cleanPasteText ignoreValidation" runat="server" MaxLength="500" autocomplete="off" type="text" data-id="CourseName" data-value="isPermitted"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfCourseNameFilter" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-12 col-md-6">
                                            <div class="form-group">
                                                <div class="col-sm-5 text-left">
                                                    <label for="<%= cboCourseState.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCourseState")%></label>
                                                </div>

                                                <div class="col-sm-7">
                                                    <select id="cboCourseState" class="form-control  control-validation" runat="server">
                                                    </select>
                                                    <asp:HiddenField ID="hdfCourseStateFilter" runat="server" />
                                                </div>
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
                                        DataKeyNames="CourseCode"
                                        OnPreRender="GrvList_PreRender" OnSorting="GrvList_Sorting">
                                        <Columns>
                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="CourseCodeSort" runat="server" CommandName="Sort" CommandArgument="CourseCode" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("CourseCode.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "CourseCode") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvCourseCode" data-id="CourseCode" data-value="<%# Eval("CourseCode") %>"><%# Eval("CourseCode") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="CourseNameSort" runat="server" CommandName="Sort" CommandArgument="CourseName" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("CourseName.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "CourseName") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvCourseName" style="overflow-wrap: break-word; word-wrap: break-word; word-break: break-all;" data-id="CourseName" data-value="<%# Eval("CourseName") %>"><%# Eval("CourseName") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="TypeTrainingNameSort" runat="server" CommandName="Sort" CommandArgument="TypeTrainingName" OnClientClick="SetWaitingGrvList(true);">                
                                                            <span><%= GetLocalResourceObject("lblTypeTraining") %></span>
                                                            <i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "TypeTrainingName") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvTypeTrainingName" data-id="TypeTrainingName" data-value="<%# Eval("TypeTrainingName") %>"><%# Eval("TypeTrainingName") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="CourseCostByParticipantSort" runat="server" CommandName="Sort" CommandArgument="CourseCostByParticipant" OnClientClick="SetWaitingGrvList(true);">      
                                                            <span><%= GetLocalResourceObject("CourseCostByParticipant.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "CourseCostByParticipant") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvCourseCostByParticipant" data-id="CourseCostByParticipant" data-value="<%# Eval("CourseCostByParticipant") %>"><%# Eval("CourseCostByParticipant", String.Format("$ {{0:{0}}}", DOLE.HRIS.Shared.Utility.GetDisplayFormatAmount())) %> </span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="CourseDurationSort" runat="server" CommandName="Sort" CommandArgument="CourseDuration" OnClientClick="SetWaitingGrvList(true);">      
                                                            <span><%= GetLocalResourceObject("CourseDuration.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "CourseDuration") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvCourseDuration" data-id="CourseDuration" data-value="<%# Eval("CourseDuration") %>"><%# Eval("CourseDuration", String.Format("{{0:{0}}} hrs", DOLE.HRIS.Shared.Utility.GetDisplayFormatAmount())) %> </span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" Visible="false">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="CourseStateSort" runat="server" CommandName="Sort" CommandArgument="CourseState" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("CourseState.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "State") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="divCourseState" data-id="State" data-value="<%# Eval("State") %>"><%# Eval("State") %></span>
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
                <Triggers></Triggers>

                <ContentTemplate>
                    <input type="hidden" id="CodeCourseSelected" runat="server" name="CodeCourseSelected" value="" />

                    <div class="btn-group" role="group" aria-label="main-buttons">
                        <button id="btnAdd" type="button" runat="server" class="btn btn-default btnAjaxAction"
                            onclick="return false;"
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

                        <button id="btnEditTrainers" type="button" runat="server" class="btn btn-default btnAjaxAction"
                            onserverclick="BtnEditTrainers_ServerClick" onclick="return ProcessEditTrainersRequest(this.id);"
                            data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnEditTrainers"))%>'
                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnEditTrainers"))%>'>
                            <span class="glyphicon glyphicon-link glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnEditTrainers") %>
                        </button>

                        <button id="btnEditTrainingPrograms" type="button" runat="server" class="btn btn-default btnAjaxAction"
                            onserverclick="BtnEditTrainingPrograms_ServerClick" onclick="return ProcessEditTrainingProgramsRequest(this.id);"
                            data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnEditTrainingPrograms"))%>'
                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnEditTrainingPrograms"))%>'>
                            <span class="glyphicon glyphicon-link glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnEditTrainingPrograms") %>
                        </button>

                        <button id="btnEditThematicAreas" type="button" runat="server" class="btn btn-default btnAjaxAction"
                            onserverclick="BtnEditThematicAreas_ServerClick" onclick="return ProcessEditThematicAreasRequest(this.id);"
                            data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnEditThematicAreas"))%>'
                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnEditThematicAreas"))%>'>
                            <span class="glyphicon glyphicon-link glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnEditAreaThamatic") %>
                        </button>
                        
                        <button id="btnEditSchoolsTraining" type="button" runat="server" class="btn btn-default btnAjaxAction btns"
                            onserverclick="BtnEditSchoolsTraining_ServerClick" onclick="return ProcessEditSchoolsTrainingRequest(this.id);"
                            data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnEditSchoolsTraining"))%>'
                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnEditSchoolsTraining"))%>'>
                            <span class="glyphicon glyphicon-link glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnEditSchoolsTraining") %>
                        </button>

                        <button id="btnExport" type="button" runat="server" class="btn btn-default btnAjaxAction"
                            onserverclick="BtnExport_ServerClick" onclick="return ProcessDownloadRequest(this.id);"
                            data-loading-text='<%$ Code:String.Concat("<span class=\" fas fa-file-download\"></span><br />", GetLocalResourceObject("btnExport"))%>'
                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnExport"))%>'>
                            <span class="glyphicon glyphicon-download-alt" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnExport") %>
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
                                <asp:HiddenField ID="hdfCourseCodeEdit" runat="server" Value="" />

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtCourseCode.ClientID%>" class="control-label code"><%=GetLocalResourceObject("lblCourseCode")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtCourseCode" CssClass="form-control cleanPasteText" runat="server" onkeypress="return isNumberOrLetter(event)" MaxLength="10"></asp:TextBox>
                                        
                                        <label id="txtCourseCodeValidation" for="<%= txtCourseCode.ClientID%>" 
                                            class="label label-danger label-validation" data-toggle="tooltip" 
                                            data-container="body" data-placement="left" 
                                            data-content="<%= GetLocalResourceObject("msjCourseCodeValidation") %>" 
                                            style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtCourseName.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCourseName")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtCourseName" CssClass="form-control cleanPasteText" runat="server" onkeypress="return isNumberOrLetter(event) && checkMaxLength(this,event,500);" onkeyUp="return isNumberOrLetter(event) && checkMaxLength(this,event,500);" MaxLength="500" TextMode="MultiLine" Columns="3"></asp:TextBox>
                                        
                                        <label id="txtCourseNameValidation" for="<%= txtCourseName.ClientID%>" 
                                            class="label label-danger label-validation" data-toggle="tooltip" 
                                            data-container="body" data-placement="left" 
                                            data-content="<%= GetLocalResourceObject("msjCourseNameValidation") %>" 
                                            style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtTypeTrainingEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblTypeTraining")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <select id="txtTypeTrainingEdit" class="form-control" required="required" runat="server"></select>
                                        
                                        <label id="txtTypeTrainingEditValidation" for="<%= txtTypeTrainingEdit.ClientID%>"
                                            class="label label-danger label-validation" 
                                            data-toggle="tooltip" data-container="body" data-placement="left"
                                            data-content="<%= GetLocalResourceObject("msjTypeTrainingValidation") %>"
                                            style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtCourseCostByParticipant.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCourseCostByParticipant")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <div class="input-group">
                                            <span class="input-group-addon" id="basic-addon1">$</span>
                                            
                                            <asp:TextBox ID="txtCourseCostByParticipant" CssClass="form-control control-validation cleanPasteDecimalDigits" runat="server" onkeypress="return isDecimalNumber(event) && checkMaxLength(this,event,11);" MaxLength="11" autocomplete="off"></asp:TextBox>
                                            
                                            <label id="txtCourseCostByParticipantValidation" for="<%= txtCourseCostByParticipant.ClientID%>" 
                                                class="label label-danger label-validation" data-toggle="tooltip" 
                                                data-container="body" data-placement="left" 
                                                data-content="<%= GetLocalResourceObject("msjCourseCostByParticipantValidation") %>" 
                                                style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtCourseDuration.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCourseDuration")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtCourseDuration" CssClass="form-control control-validation cleanPasteDecimalDigits" runat="server" onkeypress="return isDecimalNumber(event) && checkMaxLength(this,event,11);" MaxLength="11" autocomplete="off"></asp:TextBox>
                                            
                                            <label id="txtCourseDurationValidation" for="<%= txtCourseDuration.ClientID%>" 
                                                class="label label-danger label-validation" data-toggle="tooltip" 
                                                data-container="body" data-placement="left" 
                                                data-content="<%= GetLocalResourceObject("msjCourseDurationValidation") %>" 
                                                style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                            
                                            <span class="input-group-addon" id="basic-addon2">hrs.</span>
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= chbNoteRequired.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblNoteRequired")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:CheckBox ID="chbNoteRequired" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= chbCyclesRefreshment.ClientID%>" class="control-label" style="text-align:left"><%=GetLocalResourceObject("lblCyclesRefreshment")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:CheckBox ID="chbCyclesRefreshment" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= chbForMatrix.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblForMatrix")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:CheckBox ID="chbForMatrix" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                    </div>
                                </div>

                                <div class="form-group ismandatory">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtMaxDaysTrainEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblMaxDayTrain")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtMaxDaysTrainEdit" CssClass="form-control control-validation cleanPasteDigits" runat="server" onkeypress="return isNumber(event) && checkMaxLength(this, event, 3);" autocomplete="off" type="number" min="0" max="999" MaxLength="3" Style="display: inline; float: left;"></asp:TextBox>

                                            <label id="MaxDaysTrainEditValidation" for="<%= txtMaxDaysTrainEdit.ClientID%>"
                                                class="label label-danger label-validation" data-toggle="tooltip"
                                                data-container="body" data-placement="left"
                                                data-content="<%= GetLocalResourceObject("msjCourseDurationValidation") %>"
                                                style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group ismandatory">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtDaysRenewCourseEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblDaysRenewCourse")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtDaysRenewCourseEdit" CssClass="form-control control-validation cleanPasteDigits" runat="server" onkeypress="return isNumber(event) && checkMaxLength(this, event, 3);" autocomplete="off" type="number" min="0" max="999" MaxLength="3" Style="display: inline; float: left;"></asp:TextBox>

                                            <label id="DaysRenewCourseValidation" for="<%= txtDaysRenewCourseEdit.ClientID%>"
                                                class="label label-danger label-validation" data-toggle="tooltip"
                                                data-container="body" data-placement="left"
                                                data-content="<%= GetLocalResourceObject("msjCourseDurationValidation") %>"
                                                style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= chbExternalCourse.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblExternalCourse")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:CheckBox ID="chbExternalCourse" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= chbSearchEnabled.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblSearchEnabled")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:CheckBox ID="chbSearchEnabled" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
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

    <%--  Modal for Associated ThematicAreas --%>
    <div class="modal fade" id="ThematicAreasDialog" tabindex="-1" role="dialog" aria-labelledby="ThematicAreasDialogTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button id="btnThematicAreasClose" type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title text-primary"><%=Convert.ToString(GetLocalResourceObject("TitleThematicAreasDialog")) %></h3>
                </div>

                <div class="modal-body">
                    <div class="form-horizontal">
                        <asp:Button ID="btnRefreshAssociatedThematicAreas" runat="server" OnClick="BtnRefreshAssociatedThematicAreas_Click" Style="display: none;" />

                        <asp:UpdatePanel runat="server" ID="uppAssociatedThematicAreas" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnRefreshAssociatedThematicAreas" EventName="Click" />
                            </Triggers>

                            <ContentTemplate>
                                <div class="form-group">
                                    <div class="col-sm-12 text-left">
                                        <h4>
                                            <label id="lblAssociatedThematicAreas" runat="server" class="control-label"><%=GetLocalResourceObject("lblAssociatedThematicAreas")%></label></h4>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-12 text-left">
                                        <asp:Repeater ID="rptAssociatedThematicAreas" runat="server">
                                            <HeaderTemplate>
                                                <div style="width: 100%; overflow-y: scroll !important; -ms-overflow-style: scrollbar;">
                                                    <table id="tableSelectAssociatedThematicAreas" class="table table-hover table-striped">
                                                        <thead>
                                                            <tr>
                                                                <th>
                                                                    <div>
                                                                        <div class="col-xs-4 col-sm-4 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-code" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblThematicAreaCodeHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                        <div class="col-xs-6 col-sm-6 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-name" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblThematicAreaNameHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                        <div class="col-xs-2 col-sm-2 text-primary">&nbsp;</div>
                                                                    </div>
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                        <tbody id="tableBodySelectThematicArea">
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <tr>
                                                    <td class="row-fluid">
                                                        <asp:UpdatePanel runat="server" ID="uppAssociatedThematicArea" UpdateMode="Conditional">
                                                            <Triggers>
                                                            </Triggers>

                                                            <ContentTemplate>
                                                                <div class="data-sort-src" data-sort-code='<%# DataBinder.Eval(((RepeaterItem)Container).DataItem, "ThematicAreaCode") %>' data-sort-name='<%# DataBinder.Eval(((RepeaterItem)Container).DataItem, "ThematicAreaName") %>'>
                                                                    <div class="col-xs-4 col-sm-4">
                                                                        <span>
                                                                            <%# DataBinder.Eval(((RepeaterItem)Container).DataItem, "ThematicAreaCode") %>
                                                                        </span>
                                                                    </div>

                                                                    <div class="col-xs-6 col-sm-6">
                                                                        <span>
                                                                            <%# DataBinder.Eval(((RepeaterItem)Container).DataItem, "ThematicAreaName") %>
                                                                        </span>
                                                                    </div>

                                                                    <div id="divDeleteControlsThematicAreas" runat="server" class="col-xs-2 col-sm-2 text-center">
                                                                        <button id="btnDeleteAssociatedThematicArea" type="button" runat="server" class="btn btn-default btnAjaxAction btnRemoveThematicArea" data-loading-text="<span class='fa fa-spinner fa-spin '></span>" data-error-text="<span class='glyphicon glyphicon-exclamation-sign'></span>" onserverclick="BtnDeleteAssociatedThematicArea_ServerClick">
                                                                            <span class="glyphicon glyphicon-remove-circle glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                                                                        </button>
                                                                        <asp:TextBox ID="hdfAssociatedThematicAreaCode" CssClass="ThematicAreaCode" runat="server" Text='<%#DataBinder.Eval(((RepeaterItem)Container).DataItem, "ThematicAreaCode") %>' Style="display: none;" />
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
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <div class="form-group">
                            <div class="col-sm-12 text-left">
                                <h4>
                                    <label id="lblAddThematicAreas" runat="server" class="control-label"><%=GetLocalResourceObject("lblAddThematicArea")%></label></h4>
                            </div>
                        </div>

                        <asp:UpdatePanel runat="server" ID="uppSearchBarThematicArea" UpdateMode="Conditional">
                            <Triggers>
                            </Triggers>

                            <ContentTemplate>
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%=txtSearchThematicAreas.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblSearchThematicAreasPlaceHolder")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:Panel runat="server" DefaultButton="btnSearchThematicAreas">
                                            <asp:TextBox ID="txtSearchThematicAreas" runat="server" CssClass="form-control cleanPasteText" onkeypress="return isNumberOrLetter(event)" placeholder='<%$ Code:GetLocalResourceObject("lblSearchThematicAreasPlaceHolder") %>' autocomplete="off" Width="100%" OnTextChanged="TxtSearchThematicAreas_TextChanged"></asp:TextBox>
                                            <asp:Button ID="btnSearchThematicAreas" runat="server" Text="Button" Style="display: none" OnClick="TxtSearchThematicAreas_TextChanged" />
                                        </asp:Panel>
                                        <span id="txtSearchThematicAreasWaiting" class='fa fa-spinner fa-spin' style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;"></span>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <asp:UpdatePanel runat="server" ID="uppSearchThematicAreas" UpdateMode="Conditional" ChildrenAsTriggers="true">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="txtSearchThematicAreas" EventName="TextChanged" />
                            </Triggers>

                            <ContentTemplate>
                                <div class="form-group">
                                    <div class="col-sm-12 text-left">
                                        <label id="lblSearchThematicAreasResults" runat="server" class="control-label"></label>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-12 text-left">
                                        <asp:Repeater ID="rptThematicAreas" runat="server">
                                            <HeaderTemplate>
                                                <div style="width: 100%; max-height: 300px; overflow-y: scroll; -ms-overflow-style: scrollbar;">
                                                    <table id="tableSelectThematicArea" class="table table-hover table-striped">
                                                        <thead>
                                                            <tr>
                                                                <th>
                                                                    <div>
                                                                        <div class="col-xs-4 col-sm-4 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-code" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblThematicAreaCodeHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                        <div class="col-xs-6 col-sm-6 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-name" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblThematicAreaNameHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                        <div class="col-xs-2 col-sm-2 text-primary">&nbsp;</div>
                                                                    </div>
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                        <tbody id="tableBodySelectThematicArea">
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <tr>
                                                    <td class="row-fluid">
                                                        <asp:UpdatePanel runat="server" ID="uppThematicArea" UpdateMode="Conditional">
                                                            <Triggers>
                                                            </Triggers>

                                                            <ContentTemplate>
                                                                <div class="data-sort-src" data-sort-code='<%# Eval("ThematicAreaCode") %>' data-sort-name='<%# Eval("ThematicAreaName") %>'>
                                                                    <div class="col-xs-4 col-sm-4">
                                                                        <span>
                                                                            <%# Eval("ThematicAreaCode") %>
                                                                        </span>
                                                                    </div>

                                                                    <div class="col-xs-6 col-sm-6">
                                                                        <span>
                                                                            <%# Eval("ThematicAreaName") %>
                                                                        </span>
                                                                    </div>

                                                                    <div id="divAddControlsThematicAreas" runat="server" class="col-xs-2 col-sm-2 text-center">
                                                                        <button id="btnAddThematicArea" type="button" runat="server" class="btn btn-default btnAjaxAction btnAddThematicArea" data-loading-text="<span class='fa fa-spinner fa-spin '></span>" data-error-text="<span class='glyphicon glyphicon-exclamation-sign'></span>" onserverclick="BtnAddThematicArea_ServerClick">
                                                                            <span class="glyphicon glyphicon-plus-sign glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                                                                        </button>
                                                                        <asp:TextBox ID="hdfThematicAreaCode" CssClass="ThematicAreaCode" runat="server" Text='<%#Eval("ThematicAreaCode") %>' Style="display: none;" />
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
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>

                <div class="modal-footer">
                    <button id="btnThematicAreasAccept" type="button" class="btn btn-default">
                        <span class="glyphicon glyphicon-ok glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnAccept")) %>
                    </button>
                </div>
            </div>
        </div>
    </div>

    <%--  Modal for Associated Positions --%>
    <div class="modal fade" id="PositionsDialog" tabindex="-1" role="dialog" aria-labelledby="PositionsDialogTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button id="btnPositionsClose" type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title text-primary"><%=Convert.ToString(GetLocalResourceObject("TitlePositionsDialog")) %></h3>
                </div>

                <div class="modal-body">
                    <div class="form-horizontal">
                        <asp:Button ID="btnRefreshAssociatedPositions" runat="server" OnClick="BtnRefreshAssociatedPositions_Click" Style="display: none;" />

                        <asp:UpdatePanel runat="server" ID="uppAssociatedPositions" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnRefreshAssociatedPositions" EventName="Click" />
                            </Triggers>

                            <ContentTemplate>
                                <div class="form-group">
                                    <div class="col-sm-12 text-left">
                                        <h4>
                                            <label id="lblAssociatedPositions" runat="server" class="control-label"><%=GetLocalResourceObject("lblAssociatedPositions")%></label></h4>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-12 text-left">
                                        <asp:Repeater ID="rptAssociatedPositions" runat="server">
                                            <HeaderTemplate>
                                                <div style="width: 100%; overflow-y: scroll;">
                                                    <table id="tableSelectAssociatedPositions" class="table table-hover table-striped">
                                                        <thead>
                                                            <tr>
                                                                <th>
                                                                    <div>
                                                                        <div class="col-xs-4 col-sm-4 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-code" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblPositionCodeHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                        <div class="col-xs-6 col-sm-6 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-name" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblPositionNameHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                        <div class="col-xs-2 col-sm-2 text-primary">&nbsp;</div>
                                                                    </div>
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                        <tbody id="tableBodySelectPosition">
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <tr>
                                                    <td class="row-fluid">
                                                        <asp:UpdatePanel runat="server" ID="uppAssociatedPosition" UpdateMode="Conditional">
                                                            <Triggers>
                                                            </Triggers>

                                                            <ContentTemplate>
                                                                <div class="data-sort-src" data-sort-code='<%# DataBinder.Eval(((RepeaterItem)Container).DataItem, "PositionCode") %>' data-sort-name='<%# DataBinder.Eval(((RepeaterItem)Container).DataItem, "PositionName") %>'>
                                                                    <div class="col-xs-4 col-sm-4">
                                                                        <span>
                                                                            <%# DataBinder.Eval(((RepeaterItem)Container).DataItem, "PositionCode") %>
                                                                        </span>
                                                                    </div>

                                                                    <div class="col-xs-6 col-sm-6">
                                                                        <span>
                                                                            <%# DataBinder.Eval(((RepeaterItem)Container).DataItem, "PositionName") %>
                                                                        </span>
                                                                    </div>

                                                                    <div id="divDeleteControlsPositions" runat="server" class="col-xs-2 col-sm-2 text-center">
                                                                        <button id="btnDeleteAssociatedPosition" type="button" runat="server" class="btn btn-default btnAjaxAction btnRemovePosition" data-loading-text="<span class='fa fa-spinner fa-spin '></span>" data-error-text="<span class='glyphicon glyphicon-exclamation-sign'></span>" onserverclick="BtnDeleteAssociatedPosition_ServerClick">
                                                                            <span class="glyphicon glyphicon-remove-circle glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                                                                        </button>
                                                                        <asp:TextBox ID="hdfAssociatedPositionCode" CssClass="PositionCode" runat="server" Text='<%#DataBinder.Eval(((RepeaterItem)Container).DataItem, "PositionCode") %>' Style="display: none;" />
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
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <div class="form-group">
                            <div class="col-sm-12 text-left">
                                <h4>
                                    <label id="lblAddPositions" runat="server" class="control-label"><%=GetLocalResourceObject("lblAddPosition")%></label></h4>
                            </div>
                        </div>

                        <asp:UpdatePanel runat="server" ID="uppSearchBarPosition" UpdateMode="Conditional">
                            <Triggers>
                            </Triggers>

                            <ContentTemplate>
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%=txtSearchPositions.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblSearchPositionsPlaceHolder")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:Panel runat="server" DefaultButton="btnSearchPositions">
                                            <asp:TextBox ID="txtSearchPositions" runat="server" CssClass="form-control cleanPasteText" onkeypress="return isNumberOrLetter(event)" placeholder='<%$ Code:GetLocalResourceObject("lblSearchPositionsPlaceHolder") %>' autocomplete="off" Width="100%" OnTextChanged="TxtSearchPositions_TextChanged"></asp:TextBox>
                                            <asp:Button ID="btnSearchPositions" runat="server" Text="Button" Style="display: none" OnClick="TxtSearchPositions_TextChanged" />
                                        </asp:Panel>
                                        <span id="txtSearchPositionsWaiting" class='fa fa-spinner fa-spin' style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;"></span>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <asp:UpdatePanel runat="server" ID="uppSearchPositions" UpdateMode="Conditional" ChildrenAsTriggers="true">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="txtSearchPositions" EventName="TextChanged" />
                            </Triggers>

                            <ContentTemplate>
                                <div class="form-group">
                                    <div class="col-sm-12 text-left">
                                        <label id="lblSearchPositionsResults" runat="server" class="control-label"></label>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-12 text-left">
                                        <asp:Repeater ID="rptPositions" runat="server">
                                            <HeaderTemplate>
                                                <div style="width: 100%; max-height: 300px; overflow-y: scroll;">
                                                    <table id="tableSelectPosition" class="table table-hover table-striped">
                                                        <thead>
                                                            <tr>
                                                                <th>
                                                                    <div>
                                                                        <div class="col-xs-4 col-sm-4 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-code" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblPositionCodeHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                        <div class="col-xs-6 col-sm-6 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-name" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblPositionNameHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                        <div class="col-xs-2 col-sm-2 text-primary">&nbsp;</div>
                                                                    </div>
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                        <tbody id="tableBodySelectPosition">
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <tr>
                                                    <td class="row-fluid">
                                                        <asp:UpdatePanel runat="server" ID="uppPosition" UpdateMode="Conditional">
                                                            <Triggers>
                                                            </Triggers>

                                                            <ContentTemplate>
                                                                <div class="data-sort-src" data-sort-code='<%# Eval("PositionCode") %>' data-sort-name='<%# Eval("PositionName") %>'>
                                                                    <div class="col-xs-4 col-sm-4">
                                                                        <span>
                                                                            <%# Eval("PositionCode") %>
                                                                        </span>
                                                                    </div>

                                                                    <div class="col-xs-6 col-sm-6">
                                                                        <span>
                                                                            <%# Eval("PositionName") %>
                                                                        </span>
                                                                    </div>

                                                                    <div id="divAddControlsPositions" runat="server" class="col-xs-2 col-sm-2 text-center">
                                                                        <button id="btnAddPosition" type="button" runat="server" class="btn btn-default btnAjaxAction btnAddPosition btnAjaxDisable" data-loading-text="<span class='fa fa-spinner fa-spin '></span>" data-error-text="<span class='glyphicon glyphicon-exclamation-sign'></span>" onserverclick="BtnAddPosition_ServerClick">
                                                                            <span class="glyphicon glyphicon-plus-sign glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                                                                        </button>
                                                                        <asp:TextBox ID="hdfPositionCode" CssClass="PositionCode" runat="server" Text='<%#Eval("PositionCode") %>' Style="display: none;" />
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
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>

                <div class="modal-footer">
                    <button id="btnPositionsAccept" type="button" class="btn btn-default">
                        <span class="glyphicon glyphicon-ok glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnAccept")) %>
                    </button>
                </div>
            </div>
        </div>
    </div>

    <%--  Modal for Associated training programs  --%>
    <div class="modal fade" id="TrainingProgramsDialog" tabindex="-1" role="dialog" aria-labelledby="TrainingProgramsDialogTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button id="btnTrainingProgramsClose" type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title text-primary"><%=Convert.ToString(GetLocalResourceObject("TitleTrainingProgramsDialog")) %></h3>
                </div>

                <div class="modal-body">
                    <div class="form-horizontal">
                        <asp:Button ID="btnRefreshAssociatedTrainingPrograms" runat="server" OnClick="BtnRefreshAssociatedTrainingPrograms_Click" Style="display: none;" />

                        <asp:UpdatePanel runat="server" ID="uppAssociatedTrainingPrograms" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnRefreshAssociatedTrainingPrograms" EventName="Click" />
                            </Triggers>

                            <ContentTemplate>
                                <div class="form-group">
                                    <div class="col-sm-12 text-left">
                                        <h4>
                                            <label id="lblAssociatedTrainingPrograms" runat="server" class="control-label"><%=GetLocalResourceObject("lblAssociatedTrainingPrograms")%></label></h4>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-12 text-left">
                                        <asp:Repeater ID="rptAssociatedTrainingPrograms" runat="server">
                                            <HeaderTemplate>
                                                <div style="width: 100%; overflow-y: scroll; max-height: 300px;">
                                                    <table id="tableSelectAssociatedTrainingPrograms" class="table table-hover table-striped">
                                                        <thead>
                                                            <tr>
                                                                <th>
                                                                    <div>
                                                                        <div class="col-xs-4 col-sm-4 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-code" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblTrainingProgramCodeHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                        <div class="col-xs-6 col-sm-6 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-name" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblTrainingProgramNameHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                        <div class="col-xs-2 col-sm-2 text-primary">&nbsp;</div>
                                                                    </div>
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                        <tbody id="tableBodySelectTrainingProgram">
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <tr>
                                                    <td class="row-fluid">
                                                        <asp:UpdatePanel runat="server" ID="uppAssociatedTrainingProgram" UpdateMode="Conditional">
                                                            <Triggers>
                                                            </Triggers>

                                                            <ContentTemplate>
                                                                <div class="data-sort-src" data-sort-code='<%# DataBinder.Eval(((RepeaterItem)Container).DataItem, "TrainingProgramCode") %>' data-sort-name='<%# DataBinder.Eval(((RepeaterItem)Container).DataItem, "TrainingProgramName") %>'>
                                                                    <div class="col-xs-4 col-sm-4">
                                                                        <span>
                                                                            <%# DataBinder.Eval(((RepeaterItem)Container).DataItem, "TrainingProgramCode") %>
                                                                        </span>
                                                                    </div>
                                                                    <div class="col-xs-6 col-sm-6">
                                                                        <span>
                                                                            <%# DataBinder.Eval(((RepeaterItem)Container).DataItem, "TrainingProgramName") %>
                                                                        </span>
                                                                    </div>
                                                                    <div id="divDeleteControlsTrainingPrograms" runat="server" class="col-xs-2 col-sm-2 text-center">
                                                                        <button id="btnDeleteAssociatedTrainingProgram" type="button" runat="server" class="btn btn-default btnAjaxAction btnRemoveTrainingProgram" data-loading-text="<span class='fa fa-spinner fa-spin '></span>" data-error-text="<span class='glyphicon glyphicon-exclamation-sign'></span>" onserverclick="BtnDeleteAssociatedTrainingProgram_ServerClick">
                                                                            <span class="glyphicon glyphicon-remove-circle glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                                                                        </button>
                                                                        <asp:TextBox ID="hdfAssociatedTrainingProgramCode" CssClass="TrainingProgramCode" runat="server" Text='<%#DataBinder.Eval(((RepeaterItem)Container).DataItem, "TrainingProgramCode") %>' Style="display: none;" />
                                                                        <asp:TextBox ID="hdfAssociatedTrainingProgramName" CssClass="TrainingProgramName" runat="server" Text='<%#DataBinder.Eval(((RepeaterItem)Container).DataItem, "TrainingProgramName") %>' Style="display: none;" />
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
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <div class="form-group">
                            <div class="col-sm-12 text-left">
                                <h4>
                                    <label id="lblAddTrainingPrograms" runat="server" class="control-label"><%=GetLocalResourceObject("lblAddTrainingProgram")%></label></h4>
                            </div>
                        </div>

                        <asp:UpdatePanel runat="server" ID="uppSearchBarTrainingProgram" UpdateMode="Conditional">
                            <Triggers>
                            </Triggers>

                            <ContentTemplate>
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%=txtSearchTrainingPrograms.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblSearchTrainingProgramsPlaceHolder")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:Panel runat="server" DefaultButton="btnSearchTrainingPrograms">
                                            <asp:TextBox ID="txtSearchTrainingPrograms" runat="server" CssClass="form-control cleanPasteText" onkeypress="return isNumberOrLetter(event)" placeholder='<%$ Code:GetLocalResourceObject("lblSearchTrainingProgramsPlaceHolder") %>' autocomplete="off" Width="100%" OnTextChanged="TxtSearchTrainingPrograms_TextChanged"></asp:TextBox>
                                            <asp:Button ID="btnSearchTrainingPrograms" runat="server" Text="Button" Style="display: none" OnClick="TxtSearchTrainingPrograms_TextChanged" />
                                        </asp:Panel>
                                        <span id="txtSearchTrainingProgramsWaiting" class='fa fa-spinner fa-spin' style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;"></span>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <asp:UpdatePanel runat="server" ID="uppSearchTrainingPrograms" UpdateMode="Conditional" ChildrenAsTriggers="true">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="txtSearchTrainingPrograms" EventName="TextChanged" />
                            </Triggers>

                            <ContentTemplate>
                                <div class="form-group">
                                    <div class="col-sm-12 text-left">
                                        <label id="lblSearchTrainingProgramsResults" runat="server" class="control-label"></label>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-12 text-left">
                                        <asp:Repeater ID="rptTrainingPrograms" runat="server">
                                            <HeaderTemplate>
                                                <div style="width: 100%; max-height: 300px; overflow-y: scroll;">
                                                    <table id="tableSelectTrainingProgram" class="table table-hover table-striped">
                                                        <thead>
                                                            <tr>
                                                                <th>
                                                                    <div>
                                                                        <div class="col-xs-4 col-sm-4 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-code" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblTrainingProgramCodeHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                        <div class="col-xs-6 col-sm-6 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-name" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblTrainingProgramNameHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                        <div class="col-xs-2 col-sm-2 text-primary">&nbsp;</div>
                                                                    </div>
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                        <tbody id="tableBodySelectTrainingProgram">
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <tr>
                                                    <td class="row-fluid">
                                                        <asp:UpdatePanel runat="server" ID="uppTrainingProgram" UpdateMode="Conditional">
                                                            <Triggers>
                                                            </Triggers>

                                                            <ContentTemplate>
                                                                <div class="data-sort-src" data-sort-code='<%# Eval("TrainingProgramCode") %>' data-sort-name='<%# Eval("TrainingProgramName") %>'>
                                                                    <div class="col-xs-4 col-sm-4">
                                                                        <span>
                                                                            <%# Eval("TrainingProgramCode") %>
                                                                        </span>
                                                                    </div>

                                                                    <div class="col-xs-6 col-sm-6">
                                                                        <span>
                                                                            <%# Eval("TrainingProgramName") %>
                                                                        </span>
                                                                    </div>

                                                                    <div id="divAddControlsTrainingPrograms" runat="server" class="col-xs-2 col-sm-2 text-center">
                                                                        <button id="btnAddTrainingProgram" type="button" runat="server" class="btn btn-default btnAjaxAction btnAddTrainingProgram btnAjaxDisable" data-loading-text="<span class='fa fa-spinner fa-spin '></span>" data-error-text="<span class='glyphicon glyphicon-exclamation-sign'></span>" onserverclick="BtnAddTrainingProgram_ServerClick">
                                                                            <span class="glyphicon glyphicon-plus-sign glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                                                                        </button>
                                                                        <asp:TextBox ID="hdfTrainingProgramCode" CssClass="TrainingProgramCode" runat="server" Text='<%#Eval("TrainingProgramCode") %>' Style="display: none;" />
                                                                        <asp:TextBox ID="hdfTrainingProgramName" CssClass="TrainingProgramName" runat="server" Text='<%#Eval("TrainingProgramName") %>' Style="display: none;" />
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
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>

                <div class="modal-footer">
                    <button id="btnTrainingProgramsAccept" type="button" class="btn btn-default">
                        <span class="glyphicon glyphicon-ok glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnAccept")) %>
                    </button>
                </div>
            </div>
        </div>
    </div>

    <%--  Modal for Associated Trainers  --%>
    <div class="modal fade" id="TrainersDialog" tabindex="-1" role="dialog" aria-labelledby="TrainersDialogTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button id="btnTrainersClose" type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title text-primary"><%=Convert.ToString(GetLocalResourceObject("TitleTrainersDialog")) %></h3>
                </div>

                <div class="modal-body">
                    <div class="form-horizontal">
                        <asp:Button ID="btnRefreshAssociatedTrainers" runat="server" OnClick="BtnRefreshAssociatedTrainers_Click" Style="display: none;" />

                        <asp:UpdatePanel runat="server" ID="uppAssociatedTrainers" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnRefreshAssociatedTrainers" EventName="Click" />
                            </Triggers>

                            <ContentTemplate>
                                <div class="form-group">
                                    <div class="col-sm-12 text-left">
                                        <h4>
                                            <label id="lblAssociatedTrainers" runat="server" class="control-label"><%=GetLocalResourceObject("lblAssociatedTrainers")%></label></h4>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-12 text-left">
                                        <asp:Repeater ID="rptAssociatedTrainers" runat="server">
                                            <HeaderTemplate>
                                                <div style="width: 100%; overflow-y: scroll; max-height: 200px;">
                                                    <table id="tableSelectAssociatedTrainer" class="table table-hover table-striped">
                                                        <thead>
                                                            <tr>
                                                                <th>
                                                                    <div>
                                                                        <div class="col-xs-2 col-sm-2 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-type" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblTrainerTypeHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                        <div class="col-xs-2 col-sm-2 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-code" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblTrainerCodeHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                        <div class="col-xs-6 col-sm-6 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-name" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblTrainerNameHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                        <div class="col-xs-2 col-sm-2 text-primary">&nbsp;</div>
                                                                    </div>
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                        <tbody id="tableBodySelectTrainer">
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <tr>
                                                    <td class="row-fluid">
                                                        <asp:UpdatePanel runat="server" ID="uppAssociatedTrainer" UpdateMode="Conditional">
                                                            <Triggers>
                                                            </Triggers>

                                                            <ContentTemplate>
                                                                <div class="data-sort-src" data-sort-code='<%# DataBinder.Eval(((RepeaterItem)Container).DataItem, "TrainerCode") %>' data-sort-name='<%# DataBinder.Eval(((RepeaterItem)Container).DataItem, "TrainerName") %>' data-sort-type='<%# DataBinder.Eval(((RepeaterItem)Container).DataItem, "TrainerType") %>'>
                                                                    <div class="col-xs-2 col-sm-2">
                                                                        <span>
                                                                            <%# GetTrainerTypeLocalizatedDescription(Convert.ToString(DataBinder.Eval(((RepeaterItem)Container).DataItem, "TrainerType"))) %>
                                                                        </span>
                                                                    </div>

                                                                    <div class="col-xs-2 col-sm-2">
                                                                        <span>
                                                                            <%# DataBinder.Eval(((RepeaterItem)Container).DataItem, "TrainerCode") %>
                                                                        </span>
                                                                    </div>

                                                                    <div class="col-xs-6 col-sm-6">
                                                                        <span>
                                                                            <%# DataBinder.Eval(((RepeaterItem)Container).DataItem, "TrainerName") %>
                                                                        </span>
                                                                    </div>

                                                                    <div id="divDeleteControlsTrainers" runat="server" class="col-xs-2 col-sm-2 text-center">
                                                                        <button id="btnDeleteAssociatedTrainer" type="button" runat="server" class="btn btn-default btnAjaxAction btnRemoveTrainer" data-loading-text="<span class='fa fa-spinner fa-spin '></span>" data-error-text="<span class='glyphicon glyphicon-exclamation-sign'></span>" onserverclick="BtnDeleteAssociatedTrainer_ServerClick">
                                                                            <span class="glyphicon glyphicon-remove-circle glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                                                                        </button>
                                                                        <asp:TextBox ID="hdfAssociatedTrainerCode" CssClass="TrainerCode" runat="server" Text='<%#DataBinder.Eval(((RepeaterItem)Container).DataItem, "TrainerCode") %>' Style="display: none;" />
                                                                        <asp:TextBox ID="hdfAssociatedTrainerType" CssClass="TrainerType" runat="server" Text='<%#DataBinder.Eval(((RepeaterItem)Container).DataItem, "TrainerType") %>' Style="display: none;" />
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
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <asp:UpdatePanel runat="server" ID="uppSearchBarTrainer" UpdateMode="Conditional">
                            <Triggers>
                            </Triggers>

                            <ContentTemplate>
                                <div class="form-group">
                                    <div class="col-sm-3 text-left">
                                        <label for="<%=txtSearchTrainers.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblSearchTrainersPlaceHolder")%></label>
                                    </div>

                                    <div class="col-sm-9">
                                        <div class="form-group row">
                                            <div class="col-sm-5 text-left">
                                                <asp:Panel runat="server" DefaultButton="btnSearchTrainers">
                                                    <asp:TextBox ID="txtSearchTrainers" runat="server" CssClass="form-control cleanPasteText" onkeypress="return isNumberOrLetter(event)" placeholder='<%$ Code:GetLocalResourceObject("lblSearchTrainersPlaceHolder") %>' autocomplete="off" Width="100%" OnTextChanged="TxtSearchTrainers_TextChanged"></asp:TextBox>
                                                    <asp:Button ID="btnSearchTrainers" runat="server" Text="Button" Style="display: none" OnClick="TxtSearchTrainers_TextChanged" />
                                                </asp:Panel>

                                                <span id="txtSearchTrainersWaiting" class='fa fa-spinner fa-spin' style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;"></span>
                                            </div>

                                            <div class="col-sm-3">
                                                <label for="<%=cboTrainerType.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblTrainerType")%></label>
                                            </div>

                                            <div class="col-sm-4">
                                                <asp:DropDownList ID="cboTrainerType" CssClass="form-control cboAjaxAction control-validation" AutoPostBack="true" runat="server" OnSelectedIndexChanged="CboTrainerType_SelectedIndexChanged"></asp:DropDownList>
                                                <label id="cboTrainerTypeValidation" for="<%= cboTrainerType.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjTrainerTypeValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <asp:UpdatePanel runat="server" ID="uppSearchTrainers" UpdateMode="Conditional" ChildrenAsTriggers="true">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="txtSearchTrainers" EventName="TextChanged" />
                            </Triggers>

                            <ContentTemplate>
                                <div class="form-group row">
                                    <div class="col-sm-7 text-left">
                                        <label id="lblSearchTrainersResults" runat="server" class="control-label"></label>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-12 text-left">
                                        <asp:Repeater ID="rptTrainers" runat="server">
                                            <HeaderTemplate>
                                                <div style="width: 100%; max-height: 200px; overflow-y: scroll;">
                                                    <table id="tableSelectTrainer" class="table table-hover table-striped">
                                                        <thead>
                                                            <tr>
                                                                <th>
                                                                    <div>
                                                                        <div class="col-xs-2 col-sm-2 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-type" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblTrainerTypeHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                        <div class="col-xs-2 col-sm-2 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-code" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblTrainerCodeHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                        <div class="col-xs-6 col-sm-6 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-name" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblTrainerNameHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                        <div class="col-xs-2 col-sm-2 text-primary">&nbsp;</div>
                                                                    </div>
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                        <tbody id="tableBodySelectTrainer">
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <tr>
                                                    <td class="row-fluid">
                                                        <asp:UpdatePanel runat="server" ID="uppTrainer" UpdateMode="Conditional">
                                                            <Triggers>
                                                            </Triggers>

                                                            <ContentTemplate>
                                                                <div class="data-sort-src" data-sort-code='<%# Eval("TrainerCode") %>' data-sort-name='<%# Eval("TrainerName") %>' data-sort-type='<%# Eval("TrainerType") %>'>
                                                                    <div class="col-xs-2 col-sm-2">
                                                                        <span>
                                                                            <%# GetTrainerTypeLocalizatedDescription(Convert.ToString(Eval("TrainerType"))) %>
                                                                        </span>
                                                                    </div>

                                                                    <div class="col-xs-2 col-sm-2">
                                                                        <span>
                                                                            <%# Eval("TrainerCode") %>
                                                                        </span>
                                                                    </div>
                                                                    <div class="col-xs-6 col-sm-6">
                                                                        <span>
                                                                            <%# Eval("TrainerName") %>
                                                                        </span>
                                                                    </div>
                                                                    <div id="divAddControlsTrainers" runat="server" class="col-xs-2 col-sm-2 text-center">
                                                                        <button id="btnAddTrainer" type="button" runat="server" class="btn btn-default btnAjaxAction btnAddTrainer btnAjaxDisable" data-loading-text="<span class='fa fa-spinner fa-spin '></span>" data-error-text="<span class='glyphicon glyphicon-exclamation-sign'></span>" onserverclick="BtnAddTrainer_ServerClick">
                                                                            <span class="glyphicon glyphicon-plus-sign glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                                                                        </button>
                                                                        <asp:TextBox ID="hdfTrainerCode" CssClass="TrainerCode" runat="server" Text='<%#Eval("TrainerCode") %>' Style="display: none;" />
                                                                        <asp:TextBox ID="hdfTrainerType" CssClass="TrainerType" runat="server" Text='<%#Eval("TrainerType") %>' Style="display: none;" />
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
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>

                <div class="modal-footer">
                    <button id="btnTrainersAccept" type="button" class="btn btn-default">
                        <span class="glyphicon glyphicon-ok glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnAccept")) %>
                    </button>
                </div>
            </div>
        </div>
    </div>
  
    <%--  Modal for Associated School Trainings  --%>
    <div class="modal fade" id="SchoolsTrainingDialog" tabindex="-1" role="dialog" aria-labelledby="SchoolsTrainingDialogTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button id="btnSchoolsTrainingClose" type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title text-primary"><%=Convert.ToString(GetLocalResourceObject("TitleSchoolsTrainingDialog")) %></h3>
                </div>

                <div class="modal-body">
                    <div class="form-horizontal">
                        <asp:Button ID="btnRefreshAssociatedSchoolsTraining" runat="server" OnClick="BtnRefreshAssociatedSchoolsTraining_Click" Style="display: none;" />

                        <asp:UpdatePanel runat="server" ID="uppAssociatedSchoolsTraining" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnRefreshAssociatedSchoolsTraining" EventName="Click" />
                            </Triggers>

                            <ContentTemplate>
                                <div class="form-group">
                                    <div class="col-sm-12 text-left">
                                        <h4><label id="lblAssociatedSchoolsTraining" runat="server" class="control-label"><%=GetLocalResourceObject("lblAssociatedSchoolsTraining")%></label></h4>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-12 text-left">
                                        <asp:Repeater ID="rptAssociatedSchoolsTraining" runat="server">
                                            <HeaderTemplate>
                                                <div style="width: 100%; overflow-y: scroll !important; -ms-overflow-style: scrollbar; height: 250px">
                                                    <table id="tableSelectAssociatedSchoolsTraining" class="table table-hover table-striped">
                                                        <thead>
                                                            <tr>
                                                                <th>
                                                                    <div>
                                                                        <div class="col-xs-4 col-sm-4 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-code" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblSchoolsTrainingCodeHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                        <div class="col-xs-6 col-sm-6 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-name" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblSchoolsTrainingNameHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                        <div class="col-xs-2 col-sm-2 text-primary">&nbsp;</div>
                                                                    </div>
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                        <tbody id="tableBodySelectSchoolTraining">
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <tr>
                                                    <td class="row-fluid">
                                                        <asp:UpdatePanel runat="server" ID="uppAssociatedSchoolTraining" UpdateMode="Conditional">
                                                            <Triggers></Triggers>

                                                            <ContentTemplate>
                                                                <div class="data-sort-src" data-sort-code='<%# DataBinder.Eval(((RepeaterItem)Container).DataItem, "SchoolTrainingCode") %>' data-sort-name='<%# DataBinder.Eval(((RepeaterItem)Container).DataItem, "SchoolTrainingName") %>'>
                                                                    <div class="col-xs-4 col-sm-4">
                                                                        <span>
                                                                            <%# DataBinder.Eval(((RepeaterItem)Container).DataItem, "SchoolTrainingCode") %>
                                                                        </span>
                                                                    </div>

                                                                    <div class="col-xs-6 col-sm-6">
                                                                        <span>
                                                                            <%# DataBinder.Eval(((RepeaterItem)Container).DataItem, "SchoolTrainingName") %>
                                                                        </span>
                                                                    </div>

                                                                    <div id="divDeleteControlsSchoolsTraining" runat="server" class="col-xs-2 col-sm-2 text-center">
                                                                        <button id="btnDeleteAssociatedSchoolTraining" type="button" runat="server" class="btn btn-default btnAjaxAction btnRemoveSchoolTraining" data-loading-text="<span class='fa fa-spinner fa-spin '></span>" data-error-text="<span class='glyphicon glyphicon-exclamation-sign'></span>" onserverclick="BtnDeleteAssociatedSchoolTraining_ServerClick">
                                                                            <span class="glyphicon glyphicon-remove-circle glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                                                                        </button>
                                                                        
                                                                        <asp:TextBox ID="hdfAssociatedSchoolTrainingCode" CssClass="SchoolTrainingCode" runat="server" Text='<%#DataBinder.Eval(((RepeaterItem)Container).DataItem, "SchoolTrainingCode") %>' Style="display: none;" />
                                                                        <asp:TextBox ID="hdfAssociatedSchoolTrainingName" CssClass="SchoolTrainingName" runat="server" Text='<%#DataBinder.Eval(((RepeaterItem)Container).DataItem, "SchoolTrainingName") %>' Style="display: none;" />
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
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <div class="form-group">
                            <div class="col-sm-12 text-left">
                                <h4><label id="lblAddSchoolsTraining" runat="server" class="control-label"><%=GetLocalResourceObject("lblAddSchoolTraining")%></label></h4>
                            </div>
                        </div>

                        <asp:UpdatePanel runat="server" ID="uppSearchBarSchoolTraining" UpdateMode="Conditional">
                            <Triggers></Triggers>

                            <ContentTemplate>
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%=txtSearchSchoolsTraining.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblSearchSchoolsTrainingPlaceHolder")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:Panel runat="server" DefaultButton="btnSearchSchoolsTraining">
                                            <asp:TextBox ID="txtSearchSchoolsTraining" runat="server" CssClass="form-control cleanPasteText" onkeypress="return isNumberOrLetter(event)" placeholder='<%$ Code:GetLocalResourceObject("lblSearchSchoolsTrainingPlaceHolder") %>' autocomplete="off" Width="100%" OnTextChanged="TxtSearchSchoolsTraining_TextChanged"></asp:TextBox>
                                            <asp:Button ID="btnSearchSchoolsTraining" runat="server" Text="Button" Style="display: none" OnClick="TxtSearchSchoolsTraining_TextChanged" />
                                        </asp:Panel>

                                        <span id="txtSearchSchoolsTrainingWaiting" class='fa fa-spinner fa-spin' style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;"></span>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <asp:UpdatePanel runat="server" ID="uppSearchSchoolsTraining" UpdateMode="Conditional" ChildrenAsTriggers="true">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="txtSearchSchoolsTraining" EventName="TextChanged" />
                            </Triggers>

                            <ContentTemplate>
                                <div class="form-group">
                                    <div class="col-sm-12 text-left">
                                        <label id="lblSearchSchoolsTrainingResults" runat="server" class="control-label"></label>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-12 text-left">
                                        <asp:Repeater ID="rptSchoolsTraining" runat="server">
                                            <HeaderTemplate>
                                                <div style="width: 100%; max-height: 300px; overflow-y: scroll;">
                                                    <table id="tableSelectSchoolTraining" class="table table-hover table-striped">
                                                        <thead>
                                                            <tr>
                                                                <th>
                                                                    <div>
                                                                        <div class="col-xs-4 col-sm-4 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-code" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblSchoolsTrainingCodeHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                        <div class="col-xs-6 col-sm-6 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-name" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblSchoolsTrainingNameHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                        <div class="col-xs-2 col-sm-2 text-primary">&nbsp;</div>
                                                                    </div>
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                        <tbody id="tableBodySelectSchoolTraining">
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <tr>
                                                    <td class="row-fluid">
                                                        <asp:UpdatePanel runat="server" ID="uppSchoolTraining" UpdateMode="Conditional">
                                                            <Triggers></Triggers>

                                                            <ContentTemplate>
                                                                <div class="data-sort-src" data-sort-code='<%# Eval("SchoolTrainingCode") %>' data-sort-name='<%# Eval("SchoolTrainingName") %>'>
                                                                    <div class="col-xs-4 col-sm-4">
                                                                        <span>
                                                                            <%# Eval("SchoolTrainingCode") %>
                                                                        </span>
                                                                    </div>

                                                                    <div class="col-xs-6 col-sm-6">
                                                                        <span>
                                                                            <%# Eval("SchoolTrainingName") %>
                                                                        </span>
                                                                    </div>

                                                                    <div id="divAddControlsSchoolsTraining" runat="server" class="col-xs-2 col-sm-2 text-center">
                                                                        <button id="btnAddSchoolTraining" type="button" runat="server" class="btn btn-default btnAjaxAction btnAddSchoolTraining" data-loading-text="<span class='fa fa-spinner fa-spin '></span>" data-error-text="<span class='glyphicon glyphicon-exclamation-sign'></span>" onserverclick="BtnAddSchoolTraining_ServerClick">
                                                                            <span class="glyphicon glyphicon-plus-sign glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                                                                        </button>
                                                                        
                                                                        <asp:TextBox ID="hdfSchoolTrainingCode" CssClass="SchoolTrainingCode" runat="server" Text='<%#Eval("SchoolTrainingCode") %>' Style="display: none;" />
                                                                        <asp:TextBox ID="hdfSchoolTrainingName" CssClass="SchoolTrainingName" runat="server" Text='<%#Eval("SchoolTrainingName") %>' Style="display: none;" />
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
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>

                <div class="modal-footer">
                    <button id="btnSchoolsTrainingAccept" type="button" class="btn btn-default">
                        <span class="glyphicon glyphicon-ok glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnAccept")) %>
                    </button>
                </div>
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
                                            <label for="<%= txtDuplicatedCourseCode.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCourseCode")%></label>
                                        </div>

                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtDuplicatedCourseCode" CssClass="form-control cleanPasteText ignoreValidation" runat="server" onkeypress="return isNumberOrLetter(event)" MaxLength="15" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-4 text-left">
                                            <label for="<%= txtDuplicatedCourseName.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCourseName")%></label>
                                        </div>

                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtDuplicatedCourseName" CssClass="form-control cleanPasteText ignoreValidation" runat="server" onkeypress="return isNumberOrLetter(event) && checkMaxLength(this,event,500);" onkeyUp="return isNumberOrLetter(event) && checkMaxLength(this,event,500);" MaxLength="500" TextMode="MultiLine" Columns="3" ReadOnly="true"></asp:TextBox>
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
                                <asp:HiddenField ID="hdfActivateDeletedCourseCode" runat="server" Value="" />

                                <%=Convert.ToString(GetLocalResourceObject("lblTextActivateDeletedDialog")) %>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtActivateDeletedCourseCode.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCourseCode")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtActivateDeletedCourseCode" CssClass="form-control cleanPasteText ignoreValidation" runat="server" onkeypress="return isNumberOrLetter(event)" MaxLength="15" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtActivateDeletedCourseName.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCourseName")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtActivateDeletedCourseName" CssClass="form-control cleanPasteText ignoreValidation" runat="server" onkeypress="return isNumberOrLetter(event) && checkMaxLength(this,event,500);" onkeyUp="return isNumberOrLetter(event) && checkMaxLength(this,event,500);" MaxLength="500" TextMode="MultiLine" Columns="3" ReadOnly="true"></asp:TextBox>
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

    <script src="<%=ResolveUrl("~/Scripts/Excel/table2excel.js") %>" type="text/javascript"></script>
    <script src="<%=ResolveUrl("~/Scripts/Excel/xlsx.full.min.js") %>" type="text/javascript"></script>

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

            ShowModalPanelForMandatory();
            //////////////////////////////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////////////////////////////
            //In this section we initialize the checkbox toogles
            $('#<%= chbSearchEnabled.ClientID %>').bootstrapToggle({
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

            $('#<%= chbForMatrix.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });

            $('#<%= chbNoteRequired.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });

            $('#<%= chbCyclesRefreshment.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });

            $('#<%= chbExternalCourse.ClientID %>').bootstrapToggle({
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

            //In this section we set up the external modal functionality
            $('#btnThematicAreasAccept, #btnThematicAreasCancel, #btnThematicAreasClose').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                event.preventDefault();
                $('#ThematicAreasDialog').modal('hide');
                EnableButtonsDialog();
            });

            $('#btnPositionsAccept, #btnPositionsCancel, #btnPositionsClose').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                event.preventDefault();
                $('#PositionsDialog').modal('hide');
                EnableButtonsDialog();
            });

            $('#btnTrainersAccept, #btnTrainersCancel, #btnTrainersClose').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                event.preventDefault();
                $('#TrainersDialog').modal('hide');
                EnableButtonsDialog();
            });

            $('#btnTrainingProgramsAccept, #btnTrainingProgramsCancel, #btnTrainingProgramsClose').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                event.preventDefault();
                $('#TrainingProgramsDialog').modal('hide');
                EnableButtonsDialog();
            });

            $('#btnSchoolsTrainingAccept, #btnSchoolsTrainingCancel, #btnSchoolsTrainingClose').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                event.preventDefault();
                $('#SchoolsTrainingDialog').modal('hide');
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
                $('#MaintenanceDialog').modal('show');
                $('#DuplicatedDialog').modal('hide');

                $(".code").prop('disabled', false);
            });
            //////////////////////////////////////////////////////////////////////////////////////////////////

            //In this section we set the others controls functionality 
            $('#<%= btnAdd.ClientID %>').on('click', function (ev) {
                /// <summary>Handles the click event for button add.</summary>
                ev.preventDefault();

                $('#<%=chbForMatrix.ClientID%>').bootstrapToggle('<%= GetLocalResourceObject("No") %>');
                $('#<%=chbNoteRequired.ClientID%>').bootstrapToggle('<%= GetLocalResourceObject("No") %>');
                $('#<%=chbCyclesRefreshment.ClientID%>').bootstrapToggle('<%= GetLocalResourceObject("No") %>');
                $('#<%=txtDaysRenewCourseEdit.ClientID%>').val("");
                $('#<%=txtMaxDaysTrainEdit.ClientID%>').val("");
                $(".ismandatory").hide();

                setTimeout(function () {
                    $("#<%=btnAdd.ClientID%>").button('reset');
                }, 500);

                DisableToolBar();

                ClearModalForm();

                $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceAdd") %>');
                $('#dialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceAdd") %>');
                $('#MaintenanceDialog').modal('show');

                EnableToolBar();
                return false;
            });

            $('#<%= txtCourseCodeFilter.ClientID %>').on('keyup keypress', function (e) {
                ReturnFromBtnSearchClickPostBack(e);

                return isNumberOrLetter(e);
            });

            $('#<%= txtCourseNameFilter.ClientID %>').on('keyup keypress', function (e) {
                ReturnFromBtnSearchClickPostBack(e);

                return isNumberOrLetter(e);
            });

            $("#<%=chbForMatrix.ClientID%>").change(function () {
                ShowModalPanelForMandatory();
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

            $("#<%= txtSearchThematicAreas.ClientID %>").keyup(function (e) {
                if (isNumberOrLetterNoEnter(e)) {
                    SetDelayForSearchThematicAreasPostBack();
                }
            });

            $("#<%= txtSearchPositions.ClientID %>").keyup(function (e) {
                if (isNumberOrLetterNoEnter(e)) {
                    SetDelayForSearchPositionsPostBack();
                }
            });

            $("#<%= txtSearchTrainers.ClientID %>").keyup(function (e) {
                if (isNumberOrLetterNoEnter(e)) {
                    SetDelayForSearchTrainersPostBack();
                }
            });

            $("#<%= txtSearchTrainingPrograms.ClientID %>").keyup(function (e) {
                if (isNumberOrLetterNoEnter(e)) {
                    SetDelayForSearchTrainingProgramsPostBack();
                }
            });

            $("#<%= txtSearchSchoolsTraining.ClientID %>").keyup(function (e) {
                if (isNumberOrLetterNoEnter(e)) {
                    SetDelayForSearchSchoolsTrainingPostBack();
                }
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

            $('.cleanPasteDecimalDigits').on('paste keyup', function (e) {
                var $this = $(this);

                setTimeout(function (e) {
                    replacePastedInvalidDecimalDigits($this);
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
        //          VALIDATION           // 
        //*******************************//
        var validator = null;
        function ValidateForm() {
            /// <summary>Validate the form with jquey validation plugin </summary>
            /// <returns> True if form is valid. False otherwise. </returns>
            //add custom validation methods
            jQuery.validator.addMethod("validSelection", function (value, element) {
                return this.optional(element) || value != "-1";
            }, "Please select a valid option");

            jQuery.validator.addMethod("decimal2places", function (value, element) {
                return this.optional(element) || /^\d{1,8}(\.\d{1,2})?$/i.test(value);
            }, "You must include two decimal places");

            jQuery.validator.addMethod("validateMatrix", function (value, element) {
                var option = $("#<%=chbForMatrix.ClientID%>").is(":checked");
                
                if (option) {
                    return value != "" && value != null;
                } else {
                    return true;
                }
            }, "<%=GetLocalResourceObject("msgInputValid").ToString()%>");

            if (validator != null) {
                validator.destroy();
            }

            //declare the validator
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
                        "<%= txtCourseCode.UniqueID %>": {
                            required: true,
                            normalizer: function (value) {
                                return $.trim(value);
                            },
                            minlength: 1,
                            maxlength: 150
                        },

                        "<%= txtCourseName.UniqueID %>": {
                            required: true,
                            normalizer: function (value) {
                                return $.trim(value);
                            },
                            minlength: 1,
                            maxlength: 500
                        },

                        "<%= txtTypeTrainingEdit.UniqueID %>": {
                            required: true,
                        },

                         "<%= txtCourseCostByParticipant.UniqueID %>": {
                            required: true, decimal2places: true,
                            min: 0
                        },

                        "<%= txtCourseDuration.UniqueID %>": {
                            required: true, decimal2places: true,
                            min: 0
                        },

                        "<%= txtMaxDaysTrainEdit.UniqueID %>": {
                            required: true, validateMatrix: true,
                        },

                        "<%= txtDaysRenewCourseEdit.UniqueID %>": {
                            required: true, validateMatrix: true,
                    },
                }
            });

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

        function ShowModalPanelForMandatory() {
            var isChecked = $("#<%=chbForMatrix.ClientID%>").is(":checked");

            if (isChecked) {
                $(".ismandatory").show();
            } else {
                $(".ismandatory").hide();
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

        function SelectInternalThematicArea(btnId) {
            setTimeout(function () { $("#" + btnId).button('reset'); }, 100);
        }

        function SelectInternalPosition(btnId) {
            setTimeout(function () { $("#" + btnId).button('reset'); }, 100);
        }

        function SelectInternalTrainer(btnId) {
            setTimeout(function () { $("#" + btnId).button('reset'); }, 100);
        }

        function SelectInternalTrainingProgram(btnId) {
            setTimeout(function () { $("#" + btnId).button('reset'); }, 100);
        }

        function SelectInternalSchoolsTraining(btnId) {
            setTimeout(function () { $("#" + btnId).button('reset'); }, 100);
        }

        function ClearModalForm() {
            /// <summary>Clear the modal form</summary>
            $("#<%=hdfCourseCodeEdit.ClientID%>").val("");

            $("#<%=txtCourseCode.ClientID%>").val("");
            $("#<%=txtCourseCode.ClientID%>").removeAttr("disabled");
            $("#<%=txtCourseName.ClientID%>").val("");
            $("#<%=txtTypeTrainingEdit.ClientID%>").val("");
            $("#<%=txtCourseCostByParticipant.ClientID%>").val("");
            $("#<%=txtCourseDuration.ClientID%>").val("");

            $("#<%=chbForMatrix.ClientID%>").bootstrapToggle('off');
            $("#<%=chbNoteRequired.ClientID%>").bootstrapToggle('off');
            $("#<%=chbCyclesRefreshment.ClientID%>").bootstrapToggle('off');
            $("#<%=txtMaxDaysTrainEdit.ClientID%>").val("");
            $("#<%=txtDaysRenewCourseEdit.ClientID%>").val("");

            $("#<%=chbExternalCourse.ClientID%>").bootstrapToggle('off')
            $("#<%=chbSearchEnabled.ClientID%>").bootstrapToggle('off');

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
        var delayForSearchThematicAreasPostBack = null;
        function SetDelayForSearchThematicAreasPostBack() {
            /// <summary>Set a timer for delay the search persons post back while users writes</summary>
            if (delayForSearchThematicAreasPostBack != null) {
                clearTimeout(delayForSearchThematicAreasPostBack);
            }

            delayForSearchThematicAreasPostBack = setTimeout("SearchThematicAreasPostBack()", 500);
        }

        var delayForSearchPositionsPostBack = null;
        function SetDelayForSearchPositionsPostBack() {
            /// <summary>Set a timer for delay the search persons post back while users writes</summary>
            if (delayForSearchPositionsPostBack != null) {
                clearTimeout(delayForSearchPositionsPostBack);
            }

            delayForSearchPositionsPostBack = setTimeout("SearchPositionsPostBack()", 500);
        }

        var delayForSearchTrainersPostBack = null;
        function SetDelayForSearchTrainersPostBack() {
            /// <summary>Set a timer for delay the search persons post back while users writes</summary>
            if (delayForSearchTrainersPostBack != null) {
                clearTimeout(delayForSearchTrainersPostBack);
            }

            delayForSearchTrainersPostBack = setTimeout("SearchTrainersPostBack()", 500);
        }

        var delayForSearchTrainingProgramsPostBack = null;
        function SetDelayForSearchTrainingProgramsPostBack() {
            /// <summary>Set a timer for delay the search training programs post back while users writes</summary>
            if (delayForSearchTrainingProgramsPostBack != null) {
                clearTimeout(delayForSearchTrainingProgramsPostBack);
            }

            delayForSearchTrainingProgramsPostBack = setTimeout("SearchTrainingProgramsPostBack()", 500);
        }

        var delayForSearchSchoolsTrainingPostBack = null;
        function SetDelayForSearchSchoolsTrainingPostBack() {
            /// <summary>Set a timer for delay the search training programs post back while users writes</summary>
            if (delayForSearchSchoolsTrainingPostBack != null) {
                clearTimeout(delayForSearchSchoolsTrainingPostBack);
            }

            delayForSearchSchoolsTrainingPostBack = setTimeout("SearchSchoolsTrainingPostBack()", 500);
        }

        function ProcessEditThematicAreasRequest(resetId) {
            /// <summary>Process the edit paymentRates request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            if (IsRowSelected()) {
                __doPostBack('<%= btnEditThematicAreas.UniqueID %>', '');
                return true;
            }

            else {
                ShowFooterError('<%= GetLocalResourceObject("msgInvalidSelection") %>');
                ErrorButton(resetId);
                return false;
            }
        }

        function ProcessEditPositionsRequest(resetId) {
            /// <summary>Process the edit positions request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            if (IsRowSelected()) {

                return true;
            }

            else {
                ShowFooterError('<%= GetLocalResourceObject("msgInvalidSelection") %>');
                ErrorButton(resetId);
                return false;
            }
        }

        function ProcessEditTrainingProgramsRequest(resetId) {
            /// <summary>Process the edit training programs request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            if (IsRowSelected()) {
                __doPostBack('<%= btnEditTrainingPrograms.UniqueID %>', '');
                return true;
            }

            else {
                ShowFooterError('<%= GetLocalResourceObject("msgInvalidSelection") %>');
                ErrorButton(resetId);
                return false;
            }
        }

        function ProcessEditTrainersRequest(resetId) {
            /// <summary>Process the edit trainers request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            if (IsRowSelected()) {
                __doPostBack('<%= btnEditTrainers.UniqueID %>', '');
                return true;
            }

            else {
                ShowFooterError('<%= GetLocalResourceObject("msgInvalidSelection") %>');
                ErrorButton(resetId);
                return false;
            }
        }

        function ProcessEditSchoolsTrainingRequest(resetId) {
            /// <summary>Process the edit training programs request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            if (IsRowSelected()) {
                __doPostBack('<%= btnEditSchoolsTraining.UniqueID %>', '');
                return true;
            }

            else {
                ShowFooterError('<%= GetLocalResourceObject("msgInvalidSelection") %>');
                ErrorButton(resetId);
                return false;
            }
        }

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

        function ProcessDownloadRequest(resetId) {
            /// <summary>Process the edit request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            ShowFooterSuccess('<%= GetLocalResourceObject("msgOperationExportSuccesfull") %>');
            __doPostBack('<%= btnExport.UniqueID %>', '');
            return true;
        }

        function ProcessAcceptRequest(resetId) {
            /// <summary>Process the accept request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
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

        function ReturnFromBtnEditClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>           
            SetRowSelected();
            DisableToolBar();

            $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#dialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#MaintenanceDialog').modal('show');

            if ($("#<%=chbForMatrix.ClientID%>").is(":checked")) {
                $(".ismandatory").show();
            } else {
                $(".ismandatory").hide();
            }

            $("#<%=txtCourseCode.ClientID%>").attr("disabled", "disabled");

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
            DisableButtonsDialog();
            SetRowSelected();

            $('#ActivateDeletedDialog').modal('hide');
            $('#MaintenanceDialog').modal('hide');

            ShowFooterSuccess('<%= GetLocalResourceObject("msgOperationSuccesfull") %>');
            EnableButtonsDialog();
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

        function ReturnFromBtnDeleteThematicAreaClickPostBack(btnId) {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            setTimeout(function () { __doPostBack('<%= btnRefreshAssociatedThematicAreas.UniqueID %>', ''); }, 100);

            $("#" + btnId).closest("tr").fadeTo("slow", 0.2, function () {
                $(this).remove();
            });
        }

        function ReturnFromBtnAddThematicAreaClickPostBack(btnId) {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $("#" + btnId).closest("tr").fadeTo("slow", 0.2, function () {
                $(this).remove();
            });
        }

        function ReturnFromBtnEditThematicAreasClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            SetRowSelected();
            DisableToolBar();

            $('#ThematicAreasDialog').modal('show');

            EnableToolBar();
        }

        function ReturnFromSearchThematicAreasPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $("#txtSearchThematicAreasWaiting").hide();
            $('#<%= txtSearchThematicAreas.ClientID %>').prop("disabled", false);
        }

        function ReturnFromBtnDeletePositionClickPostBack(btnId) {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            setTimeout(function () { __doPostBack('<%= btnRefreshAssociatedPositions.UniqueID %>', ''); }, 100);

            $("#" + btnId).closest("tr").fadeTo("slow", 0.2, function () {
                $(this).remove();
            });
        }

        function ReturnFromBtnAddPositionClickPostBack(btnId) {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $("#" + btnId).closest("tr").fadeTo("slow", 0.2, function () {
                $(this).remove();
            });
        }

        function ReturnFromBtnEditPositionsClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            SetRowSelected();
            DisableToolBar();

            $('#PositionsDialog').modal('show');

            EnableToolBar();
        }

        function ReturnFromSearchPositionsPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $("#txtSearchPositionsWaiting").hide();
            $('#<%= txtSearchPositions.ClientID %>').prop("disabled", false);
            //$('#MaintenanceInternalPositionPanel').show();
        }

        function ReturnFromBtnDeleteTrainingProgramClickPostBack(btnId) {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            setTimeout(function () { __doPostBack('<%= btnRefreshAssociatedTrainingPrograms.UniqueID %>', ''); }, 100);

            $("#" + btnId).closest("tr").fadeTo("slow", 0.2, function () {
                $(this).remove();
            });
        }

        function ReturnFromBtnAddTrainingProgramClickPostBack(btnId) {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $("#" + btnId).closest("tr").fadeTo("slow", 0.2, function () {
                $(this).remove();
                var Mens ="<%= GetLocalResourceObject("lblSearchTrainingProgramsResultsCount").ToString()%>"
                var rowCount = $('table#tableSelectTrainingProgram tr:last').index() + 1;

                Mens = Mens.replace("{0}", rowCount);
                Mens = Mens.replace("{1}", rowCount);

                $("#<%=lblSearchTrainingProgramsResults.ClientID%>").text("<%=GetLocalResourceObject("lblSearchTrainingProgramsResults").ToString()%> " + Mens);
            });
        }

        function ReturnFromBtnEditTrainingProgramsClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            SetRowSelected();
            DisableToolBar();

            $('#TrainingProgramsDialog').modal('show');

            EnableToolBar();
        }

        function ReturnFromSearchTrainingProgramsPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $("#txtSearchTrainingProgramsWaiting").hide();
            $('#<%= txtSearchTrainingPrograms.ClientID %>').prop("disabled", false);
            //$('#MaintenanceInternalTrainerPanel').show();
        }

        function ReturnFromBtnDeleteTrainerClickPostBack(btnId) {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            setTimeout(function () { __doPostBack('<%= btnRefreshAssociatedTrainers.UniqueID %>', ''); }, 100);

            $("#" + btnId).closest("tr").fadeTo("slow", 0.2, function () {
                $(this).remove();
            });
        }

        function ReturnFromBtnAddTrainerClickPostBack(btnId) {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $("#" + btnId).closest("tr").fadeTo("slow", 0.2, function () {
                $(this).remove();
            });
        }

        function ReturnFromBtnEditTrainersClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            SetRowSelected();
            DisableToolBar();

            $('#TrainersDialog').modal('show');

            EnableToolBar();
        }

        function ReturnFromSearchTrainersPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $("#txtSearchTrainersWaiting").hide();
            $('#<%= txtSearchTrainers.ClientID %>').prop("disabled", false);
            //$('#MaintenanceInternalTrainerPanel').show();
        }

        function ReturnFromBtnEditSchoolsTrainingClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            SetRowSelected();
            DisableToolBar();

            $('#SchoolsTrainingDialog').modal('show');
            EnableToolBar();
        }

        function ReturnFromBtnDeleteSchoolsTrainingClickPostBack(btnId) {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            setTimeout(function () { __doPostBack('<%= btnRefreshAssociatedSchoolsTraining.UniqueID %>', ''); }, 100);
            $("#" + btnId).closest("tr").fadeTo("slow", 0.2, function () {
                $(this).remove();
            });
        }

        function ReturnFromSearchSchoolsTrainingPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $("#txtSearchSchoolsTrainingWaiting").hide();
            $('#<%= txtSearchSchoolsTraining.ClientID %>').prop("disabled", false);
        }

        function ReturnFromBtnAddSchoolsTrainingClickPostBack(btnId) {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $("#" + btnId).closest("tr").fadeTo("slow", 0.2, function () {
                $(this).remove();
                var Mens ="<%= GetLocalResourceObject("lblSearchSchoolsTrainingResultsCount").ToString()%>"
                var rowCount = $('table#tableSelectSchoolsTraining tr:last').index() + 1;

                Mens = Mens.replace("{0}", rowCount);
                Mens = Mens.replace("{1}", rowCount);

                ReturnFromBtnSearchClickPostBack

                $("#<%=lblSearchSchoolsTrainingResults.ClientID%>").text("<%=GetLocalResourceObject("lblSearchSchoolsTrainingResults").ToString()%> " + Mens);
            });
        }

        function ReturnFromBtnExportClickPostBack(jsonData) {
            var courses = [];
            filename = '<%= GetLocalResourceObject("ExcelName") %>.xlsx';

            jsonData.forEach(row => {
                var obj = new Object({
                    "<%= GetLocalResourceObject("CourseCode.HeaderText") %>": row.Codigo,
                    "<%= GetLocalResourceObject("CourseName.HeaderText") %>": row.Nombre,
                    "<%= GetLocalResourceObject("CourseTypeForm.HeaderText") %>": row.TipoFormacion,
                    "<%= GetLocalResourceObject("CourseCostByParticipant.HeaderText") %>": "$ " + row.Costo,
                    "<%= GetLocalResourceObject("CourseDuration.HeaderText") %>": row.Duracion,
                    "<%= GetLocalResourceObject("CourseThematicAreaName.HeaderText") %>": row.AreaTematica,
                    "<%= GetLocalResourceObject("SchoolTrainingCode.HeaderText") %>": row.EscuelaEntrenamientoCodigo,
                    "<%= GetLocalResourceObject("SchoolTrainingName.HeaderText") %>": row.EscuelaEntrenamientoNombre,
                    "<%= GetLocalResourceObject("CourseTrainingProgramName.HeaderText") %>": row.ProgramaEntrenamiento,
                    "<%= GetLocalResourceObject("CourseMatrix.HeaderText") %>": row.Matrix,
                    "<%= GetLocalResourceObject("CourseNote.HeaderText") %>": row.NoteRequired,
                    "<%= GetLocalResourceObject("CourseCyclesRefreshment.HeaderText") %>": row.TieneCiclosRefrescamiento,
                    "<%= GetLocalResourceObject("CourseExtern.HeaderText") %>": row.Externo,
                    "<%= GetLocalResourceObject("CourseState.HeaderText") %>": row.Estado
                });

                courses.push(obj);
            });
            console.log("<%= GetLocalResourceObject("SchoolTrainingCode.HeaderText") %>");
            let objectMaxLength = []

            // Get columns length
            courses.map(arr => {
                Object.keys(arr).map(key => {
                    let value = arr[key] === null ? '' : arr[key];

                    if (typeof value === 'number') {
                        return objectMaxLength[key] = 10
                    }

                    objectMaxLength[key] = objectMaxLength[key] >= value.length ? objectMaxLength[key] : value.length
                })
            });

            // Get columns header
            const header = Object.keys(courses[0]);

            // Add columns length
            let wsCols = [];
            let col = 0;

            for (var i = 0; i < header.length; i++) {
                col = objectMaxLength[header[i]];

                if (objectMaxLength[header[i]] <= 10) {
                    col = header[i].length + 13
                }

                wsCols.push({ wch: col });
            }

            let workSheet = XLSX.utils.json_to_sheet(courses);
            workSheet['!autofilter'] = { ref: `A1:L${courses.length + 1}` }

            workSheet["!cols"] = wsCols;

            var workBook = XLSX.utils.book_new();
            XLSX.utils.book_append_sheet(workBook, workSheet, "<%= GetLocalResourceObject("ExcelName") %>");
            XLSX.writeFile(workBook, filename);
        }

        function SearchPositionsPostBack() {
            /// <summary>Executes the search persons post back</summary>
            $("#txtSearchPositionsWaiting").show();
            $('#<%= txtSearchPositions.ClientID %>').prop("disabled", true);

            __doPostBack("<%= btnSearchPositions.UniqueID %>", '');
        }

        function SearchTrainersPostBack() {
            /// <summary>Executes the search persons post back</summary>
            $("#txtSearchTrainersWaiting").show();
            $('#<%= txtSearchTrainers.ClientID %>').prop("disabled", true);

            __doPostBack("<%= btnSearchTrainers.UniqueID %>", '');
        }

        function SearchTrainingProgramsPostBack() {
            /// <summary>Executes the search persons post back</summary>
            $("#txtSearchTrainingProgramsWaiting").show();
            $('#<%= txtSearchTrainingPrograms.ClientID %>').prop("disabled", true);

            __doPostBack("<%= btnSearchTrainingPrograms.UniqueID %>", '');
        }

        function SearchThematicAreasPostBack() {
            /// <summary>Executes the search persons post back</summary>
            $("#txtSearchThematicAreasWaiting").show();
            $('#<%= txtSearchThematicAreas.ClientID %>').prop("disabled", true);

            __doPostBack("<%= btnSearchThematicAreas.UniqueID %>", '');
        }

        function SearchSchoolsTrainingPostBack() {
            /// <summary>Executes the search persons post back</summary>
            $("#txtSearchSchoolsTrainingWaiting").show();
            $('#<%= txtSearchSchoolsTraining.ClientID %>').prop("disabled", true);

            __doPostBack("<%= btnSearchSchoolsTraining.UniqueID %>", '');
        }

        //*******************************//
        // MESSAGING AND CONFIRMATION    // 
        //*******************************//
        function ShowConfirmationMessageDelete(resetId) {
            /// <summary>Show confirmation message for Save and Close funtionality</summary>
            /// <param name="resetId" type="String">Id of the button to reset if cancel</param>
            /// <returns>False</returns>
            var cod = $.trim($("#<%=grvList.ClientID%> tr.info td").eq(0).text());
            console.log(cod, 'codigos');
            var msg1 ='<%= GetLocalResourceObject("msjDelete") %>';
            var msg2 ='<%= GetLocalResourceObject("msgDeletedAnyrecords") %>';

            $.ajax({
                type: "POST",
                url: "./Courses.aspx/AsociatedRecordsCourses",
                data: JSON.stringify({ CodCourses: cod }),
                contentType: "application/json;charset=utf-8;",
                dataType: 'json',
                async: false,
                success: function (data) {
                    var result = data.d;

                    var resultmsg = result ? msg2 : msg1;
                    MostrarConfirmacion(
                        resultmsg,
                        '<%=GetLocalResourceObject("Yes")%>', function () {
                            __doPostBack('<%= btnDelete.UniqueID %>', '');
                    }, '<%=GetLocalResourceObject("No")%>', function () {
                        $("#" + resetId).button('reset');
                    }
                    );
                },
                error: function () {

                }
            });

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
