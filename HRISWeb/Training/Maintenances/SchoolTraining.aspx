<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="SchoolTraining.aspx.cs" Inherits="HRISWeb.Training.Maintenances.SchoolTraining" %>

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
                <Triggers></Triggers>

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
                                                <label for="<%= SchoolTrainingCodeFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblSchoolTrainingCode")%></label>
                                            </div>

                                            <div class="col-sm-7">
                                                <asp:TextBox ID="SchoolTrainingCodeFilter" CssClass="form-control control-validation cleanPasteText ignoreValidation" runat="server" MaxLength="10" autocomplete="off" type="text" data-id="SchoolTrainingCode" data-value="isPermitted"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group col-sm-12 col-md-6">
                                            <div class="col-sm-5 text-left">
                                                <label for="<%= SchoolTrainingNameFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblSchoolTrainingName")%></label>
                                            </div>

                                            <div class="col-sm-7">
                                                <asp:TextBox ID="SchoolTrainingNameFilter" CssClass="form-control control-validation cleanPasteText ignoreValidation" runat="server" MaxLength="500" autocomplete="off" type="text" data-id="SchoolTrainingName" data-value="isPermitted"></asp:TextBox>
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
                                        DataKeyNames="SchoolTrainingId, SchoolTrainingCode"
                                        OnPreRender="GrvList_PreRender" OnSorting="GrvList_Sorting">
                                        <Columns>
                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="SchoolTrainingCodeSort" runat="server" CommandName="Sort" CommandArgument="SchoolTrainingCode"
                                                            OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("SchoolTrainingCodeHeaderText") %></span>&nbsp;
                                                            <i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "SchoolTrainingCode") %> sorterDirection' 
                                                                aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvSchoolTrainingCode" data-id="SchoolTrainingCode" data-value="<%# Eval("SchoolTrainingCode") %>"><%# Eval("SchoolTrainingCode") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="SchoolTrainingNameSort" runat="server" CommandName="Sort" CommandArgument="SchoolTrainingName" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("SchoolTrainingNameHeaderText") %></span>&nbsp;
                                                            <i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "SchoolTrainingName") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvSchoolTrainingName" data-id="SchoolTrainingName" style="overflow-wrap: break-word; word-wrap: break-word; word-break: break-all;" data-value="<%# Eval("SchoolTrainingName") %>"><%# Eval("SchoolTrainingName") %></span>
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
                        
                        <button id="btnEditCourses" type="button" runat="server" class="btn btn-default btnAjaxAction btns"
                            onserverclick="BtnEditCourses_ServerClick" onclick="return ProcessEditCoursesRequest(this.id);"
                            data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnEditCourses"))%>'
                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnEditCourses"))%>'>
                            <span class="glyphicon glyphicon-link glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnEditCourses") %>
                        </button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <%--  Modal for Add and Edit --%>
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
                                <asp:HiddenField ID="hdfSchoolTrainingIdEdit" runat="server" Value="" />

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%=SchoolTrainingCodeEdit.ClientID%>" class="control-label" style="text-align: left"><%=GetLocalResourceObject("lblSchoolTrainingCode")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:TextBox ID="SchoolTrainingCodeEdit" CssClass="form-control cleanPasteText enterkey emptyinput trim" runat="server" onkeypress="return isNumberOrLetter(event)" MaxLength="10"></asp:TextBox>
                                        
                                        <label id="SchoolTrainingCodeEditValidation" for="<%= SchoolTrainingCodeEdit.ClientID%>" 
                                            class="label label-danger label-validation" data-toggle="tooltip" 
                                            data-container="body" data-placement="left" 
                                            data-content="<%= GetLocalResourceObject("msjSchoolTrainingCodeValidation") %>" 
                                            style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%=SchoolTrainingNameEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblSchoolTrainingName")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:TextBox ID="SchoolTrainingNameEdit" CssClass="form-control cleanPasteText enterkey emptyinput trim" runat="server" onkeypress="return isNumberOrLetter(event)" MaxLength="500"></asp:TextBox>
                                        
                                        <label id="SchoolTrainingName" for="<%= SchoolTrainingNameEdit.ClientID%>" 
                                            class="label label-danger label-validation" data-toggle="tooltip" 
                                            data-container="body" data-placement="left" 
                                            data-content="<%= GetLocalResourceObject("msjSchoolTrainingNameValidation") %>" 
                                            style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
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

    <%--  Modal for Associated Courses --%>
    <div class="modal fade" id="CoursesDialog" tabindex="-1" role="dialog" aria-labelledby="CoursesDialogTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button id="btnCoursesClose" type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title text-primary"><%=Convert.ToString(GetLocalResourceObject("TitleCoursesDialog")) %></h3>
                </div>

                <div class="modal-body">
                    <div class="form-horizontal">
                        <asp:Button ID="btnRefreshAssociatedCourses" runat="server" OnClick="BtnRefreshAssociatedCourses_Click" Style="display: none;" />

                        <asp:UpdatePanel runat="server" ID="uppAssociatedCourses" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnRefreshAssociatedCourses" EventName="Click" />
                            </Triggers>

                            <ContentTemplate>
                                <div class="form-group">
                                    <div class="col-sm-12 text-left">
                                        <h4><label id="lblAssociatedCourses" runat="server" class="control-label"><%=GetLocalResourceObject("lblAssociatedCourses")%></label></h4>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-12 text-left">
                                        <asp:Repeater ID="rptAssociatedCourses" runat="server">
                                            <HeaderTemplate>
                                                <div style="width: 100%; overflow-y: scroll !important; -ms-overflow-style: scrollbar; height: 250px">
                                                    <table id="tableSelectAssociatedCourses" class="table table-hover table-striped">
                                                        <thead>
                                                            <tr>
                                                                <th>
                                                                    <div>
                                                                        <div class="col-xs-4 col-sm-4 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-code" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblCoursesCodeHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                        <div class="col-xs-6 col-sm-6 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-name" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblCoursesNameHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                        <div class="col-xs-2 col-sm-2 text-primary">&nbsp;</div>
                                                                    </div>
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                        <tbody id="tableBodySelectCourse">
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <tr>
                                                    <td class="row-fluid">
                                                        <asp:UpdatePanel runat="server" ID="uppAssociatedCourse" UpdateMode="Conditional">
                                                            <Triggers></Triggers>

                                                            <ContentTemplate>
                                                                <div class="data-sort-src" data-sort-code='<%# DataBinder.Eval(((RepeaterItem)Container).DataItem, "CourseCode") %>' data-sort-name='<%# DataBinder.Eval(((RepeaterItem)Container).DataItem, "CourseName") %>'>
                                                                    <div class="col-xs-4 col-sm-4">
                                                                        <span>
                                                                            <%# DataBinder.Eval(((RepeaterItem)Container).DataItem, "CourseCode") %>
                                                                        </span>
                                                                    </div>

                                                                    <div class="col-xs-6 col-sm-6">
                                                                        <span>
                                                                            <%# DataBinder.Eval(((RepeaterItem)Container).DataItem, "CourseName") %>
                                                                        </span>
                                                                    </div>

                                                                    <div id="divDeleteControlsCourses" runat="server" class="col-xs-2 col-sm-2 text-center">
                                                                        <button id="btnDeleteAssociatedCourse" type="button" runat="server" class="btn btn-default btnAjaxAction btnRemoveCourse" data-loading-text="<span class='fa fa-spinner fa-spin '></span>" data-error-text="<span class='glyphicon glyphicon-exclamation-sign'></span>" onserverclick="BtnDeleteAssociatedCourse_ServerClick">
                                                                            <span class="glyphicon glyphicon-remove-circle glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                                                                        </button>
                                                                        
                                                                        <asp:TextBox ID="hdfAssociatedCourseCode" CssClass="CourseCode" runat="server" Text='<%#DataBinder.Eval(((RepeaterItem)Container).DataItem, "CourseCode") %>' Style="display: none;" />
                                                                        <asp:TextBox ID="hdfAssociatedCourseGeographicDivisionCode" CssClass="CourseGeographicDivisionCode" runat="server" Text='<%#DataBinder.Eval(((RepeaterItem)Container).DataItem, "GeographicDivisionCode") %>' Style="display: none;" />
                                                                        <asp:TextBox ID="hdfAssociatedCourseName" CssClass="CourseName" runat="server" Text='<%#DataBinder.Eval(((RepeaterItem)Container).DataItem, "CourseName") %>' Style="display: none;" />
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
                                <h4><label id="lblAddCourses" runat="server" class="control-label"><%=GetLocalResourceObject("lblAddCourse")%></label></h4>
                            </div>
                        </div>

                        <asp:UpdatePanel runat="server" ID="uppSearchBarCourse" UpdateMode="Conditional">
                            <Triggers></Triggers>

                            <ContentTemplate>
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%=txtSearchCourses.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblSearchCoursesPlaceHolder")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:Panel runat="server" DefaultButton="btnSearchCourses">
                                            <asp:TextBox ID="txtSearchCourses" runat="server" CssClass="form-control cleanPasteText" onkeypress="return isNumberOrLetter(event)" placeholder='<%$ Code:GetLocalResourceObject("lblSearchCoursesPlaceHolder") %>' autocomplete="off" Width="100%" OnTextChanged="TxtSearchCourses_TextChanged"></asp:TextBox>
                                            <asp:Button ID="btnSearchCourses" runat="server" Text="Button" Style="display: none" OnClick="TxtSearchCourses_TextChanged" />
                                        </asp:Panel>

                                        <span id="txtSearchCoursesWaiting" class='fa fa-spinner fa-spin' style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;"></span>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <asp:UpdatePanel runat="server" ID="uppSearchCourses" UpdateMode="Conditional" ChildrenAsTriggers="true">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="txtSearchCourses" EventName="TextChanged" />
                            </Triggers>

                            <ContentTemplate>
                                <div class="form-group">
                                    <div class="col-sm-12 text-left">
                                        <label id="lblSearchCoursesResults" runat="server" class="control-label"></label>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-12 text-left">
                                        <asp:Repeater ID="rptCourses" runat="server">
                                            <HeaderTemplate>
                                                <div style="width: 100%; max-height: 300px; overflow-y: scroll;">
                                                    <table id="tableSelectCourse" class="table table-hover table-striped">
                                                        <thead>
                                                            <tr>
                                                                <th>
                                                                    <div>
                                                                        <div class="col-xs-4 col-sm-4 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-code" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblCoursesCodeHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                        <div class="col-xs-6 col-sm-6 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-name" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblCoursessNameHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                        <div class="col-xs-2 col-sm-2 text-primary">&nbsp;</div>
                                                                    </div>
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                        <tbody id="tableBodySelectCourse">
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <tr>
                                                    <td class="row-fluid">
                                                        <asp:UpdatePanel runat="server" ID="uppCourse" UpdateMode="Conditional">
                                                            <Triggers></Triggers>

                                                            <ContentTemplate>
                                                                <div class="data-sort-src" data-sort-code='<%# Eval("CourseCode") %>' data-sort-name='<%# Eval("CourseName") %>'>
                                                                    <div class="col-xs-4 col-sm-4">
                                                                        <span>
                                                                            <%# Eval("CourseCode") %>
                                                                        </span>
                                                                    </div>

                                                                    <div class="col-xs-6 col-sm-6">
                                                                        <span>
                                                                            <%# Eval("CourseName") %>
                                                                        </span>
                                                                    </div>

                                                                    <div id="divAddControlsCourses" runat="server" class="col-xs-2 col-sm-2 text-center">
                                                                        <button id="btnAddCourse" type="button" runat="server" class="btn btn-default btnAjaxAction btnAddCourse" data-loading-text="<span class='fa fa-spinner fa-spin '></span>" data-error-text="<span class='glyphicon glyphicon-exclamation-sign'></span>" onserverclick="BtnAddCourse_ServerClick">
                                                                            <span class="glyphicon glyphicon-plus-sign glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                                                                        </button>
                                                                        
                                                                        <asp:TextBox ID="hdfCourseCode" CssClass="CourseCode" runat="server" Text='<%#Eval("CourseCode") %>' Style="display: none;" />
                                                                        <asp:TextBox ID="hdfCourseGeographicDivisionCode" CssClass="CourseGeographicDivisionCode" runat="server" Text='<%#Eval("GeographicDivisionCode") %>' Style="display: none;" />
                                                                        <asp:TextBox ID="hdfCourseName" CssClass="CourseName" runat="server" Text='<%#Eval("CourseName") %>' Style="display: none;" />
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
                    <button id="btnCoursesAccept" type="button" class="btn btn-default">
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
                                <div id="divDuplicatedDialogText" runat="server"></div>

                                <asp:Panel ID="pnlDuplicatedDialogDataDetail" runat="server">
                                    <div class="form-group">
                                        <div class="col-sm-4 text-left">
                                            <label for="<%= txtDuplicatedSchoolTrainingCode.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblSchoolTrainingCode")%></label>
                                        </div>

                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtDuplicatedSchoolTrainingCode" CssClass="form-control cleanPasteText ignoreValidation" runat="server" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-4 text-left">
                                            <label for="<%= txtDuplicatedTrainerCode.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblSchoolTrainingName")%></label>
                                        </div>

                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtDuplicatedTrainerCode" CssClass="form-control cleanPasteText ignoreValidation" runat="server" ReadOnly="true"></asp:TextBox>
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
                                <asp:HiddenField ID="hdfActivateDeletedSchoolTrainingId" runat="server" Value="" />

                                <div id="divActivateDeletedDialog" runat="server"></div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtActivateDeletedSchoolTrainingCode.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblSchoolTrainingCode")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtActivateDeletedSchoolTrainingCode" CssClass="form-control cleanPasteText ignoreValidation" runat="server" TextMode="MultiLine" Columns="3" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtActivateDeletedSchoolTrainingName.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblSchoolTrainingName")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtActivateDeletedSchoolTrainingName" CssClass="form-control cleanPasteText ignoreValidation" runat="server" ReadOnly="true"></asp:TextBox>
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

                            <button id="btnActivateDeletedAccept" type="button" runat="server" class="btn btn-primary btnAjaxAction" onserverclick="BtnActivateDeletedAccept_ServerClick" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>'>
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
            $('#<%= chbActivateDeleted.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });

            $('#<%= chbUpdateActivateDeleted.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });

            $('#<%= SearchEnabledEdit.ClientID %>').bootstrapToggle({
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
            $('#btnCoursesAccept, #btnCoursesCancel, #btnCoursesClose').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                event.preventDefault();
                $('#CoursesDialog').modal('hide');
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
            $('#<%= btnAdd.ClientID %>').on('click', function (ev) {
                /// <summary>Handles the click event for button add.</summary>
                ev.preventDefault();

                $('#<%= SchoolTrainingCodeEdit.ClientID %>').prop('disabled', false);
                $('#<%=SearchEnabledEdit.ClientID%>').bootstrapToggle('<%= GetLocalResourceObject("Yes") %>')

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

            $('#<%= SchoolTrainingCodeFilter.ClientID %>').on('keyup keypress', function (e) {
                ReturnFromBtnSearchClickPostBack(e);

                return isNumberOrLetter(e);
            });

            $('#<%= SchoolTrainingNameFilter.ClientID %>').on('keyup keypress', function (e) {
                ReturnFromBtnSearchClickPostBack(e);

                return isNumberOrLetter(e);
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

            $("#<%= txtSearchCourses.ClientID %>").keyup(function (e) {
                if (isNumberOrLetterNoEnter(e)) {
                    SetDelayForSearchCoursesPostBack();
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
                    "<%= SchoolTrainingCodeEdit.UniqueID%>": {
                        required: true,
                        normalizer: function (value) {
                            return $.trim(value);
                        },
                        minlength: 1,
                        maxlength: 10
                    },

                    "<%= SchoolTrainingNameEdit.UniqueID%>": {
                        required: true,
                        normalizer: function (value) {
                            return $.trim(value);
                        },
                        minlength: 1,
                        maxlength: 500
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

        function ClearModalForm() {
            $(".emptyinput").val("");
            $("#<%=hdfSchoolTrainingIdEdit.ClientID%>").val("-1");

            $("#<%=SchoolTrainingCodeEdit.ClientID%>").val("");

            if (validator != null) {
                validator.resetForm();
            }
        }

        function SetControlInvalid(controlId) {
            /// <summary>Set the appearance of the control as invalid </summary>
            /// <param name="controlId" type="String">Id of the control</param>
            $('#' + controlId).addClass("Invalid");
            $('label[for=' + controlId + '].label-validation').show();

        }

        //*******************************//
        //           PROCESS             //
        //*******************************//
        var delayForSearchCoursesPostBack = null;
        function SetDelayForSearchCoursesPostBack() {
            /// <summary>Set a timer for delay the search training programs post back while users writes</summary>
            if (delayForSearchCoursesPostBack != null) {
                clearTimeout(delayForSearchCoursesPostBack);
            }

            delayForSearchCoursesPostBack = setTimeout("SearchCoursesPostBack()", 500);
        }

        function ProcessEditCoursesRequest(resetId) {
            /// <summary>Process the edit course request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            if (IsRowSelected()) {
                __doPostBack('<%= btnEditCourses.UniqueID %>', '');
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
                $('#<%= SchoolTrainingCodeEdit.ClientID %>').prop('disabled', true);
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
            SetRowSelected();
            DisableToolBar();

            $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#dialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#MaintenanceDialog').modal('show');

            $('#<%= SchoolTrainingCodeEdit.ClientID %>').prop('disabled', true);

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

            ShowFooterSuccess('<%= GetLocalResourceObject("msgOperationSuccesfull") %>');
            EnableButtonsDialog();
        }

        function ReturnFromBtnAcceptClickPostBackDeleted(tipo) {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $('#MaintenanceDialog').modal('hide');

            var textoMostrar = ""
            if (tipo == "-1") {
                textoMostrar = "<%=Convert.ToString(GetLocalResourceObject("lblTextDuplicatedDialog")) %>";
            } else {
                textoMostrar = "<%=Convert.ToString(GetLocalResourceObject("lblTextDuplicatedNameDialog")) %>";
            }

            $('#textMessage').append(textoMostrar);

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

        function ReturnFromBtnEditCoursesClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            SetRowSelected();
            DisableToolBar();

            $('#CoursesDialog').modal('show');
            EnableToolBar();
        }

        function ReturnFromBtnDeleteCourseClickPostBack(btnId) {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            setTimeout(function () { __doPostBack('<%= btnRefreshAssociatedCourses.UniqueID %>', ''); }, 100);
            $("#" + btnId).closest("tr").fadeTo("slow", 0.2, function () {
                $(this).remove();
            });
        }

        function ReturnFromSearchCoursesPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $("#txtSearchCoursesWaiting").hide();
            $('#<%= txtSearchCourses.ClientID %>').prop("disabled", false);
        }

        function ReturnFromBtnAddCourseClickPostBack(btnId) {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $("#" + btnId).closest("tr").fadeTo("slow", 0.2, function () {
                $(this).remove();

                var Mens ="<%= GetLocalResourceObject("lblSearchCoursesResultsCount").ToString()%>"
                var rowCount = $('table#tableSelectCourse tr:last').index() + 1;

                Mens = Mens.replace("{0}", rowCount);
                Mens = Mens.replace("{1}", rowCount);

                ReturnFromBtnSearchClickPostBack

                $("#<%=lblSearchCoursesResults.ClientID%>").text("<%=GetLocalResourceObject("lblSearchCoursesResults").ToString()%> " + Mens);
            });
        }

        function SearchCoursesPostBack() {
            /// <summary>Executes the search persons post back</summary>
            $("#txtSearchCoursesWaiting").show();
            $('#<%= txtSearchCourses.ClientID %>').prop("disabled", true);
            __doPostBack("<%= btnSearchCourses.UniqueID %>", '');
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
