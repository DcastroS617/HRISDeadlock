<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="WorkingTimeHoursByDepartmentEmployeesList.aspx.cs" Inherits="HRISWeb.Overtime.WorkingTimeHoursByDepartmentEmployeesList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cntHeader" runat="server">
     <title><%=Convert.ToString(GetLocalResourceObject("lblScreenTitle")) %></title>
     <style>
        .chkmargin {
            margin-left: 5px !important;
        }
    </style>
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

                        <div class="col-sm-5">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-5 text-left">
                                        <label for="<%=txtEmployeeId.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblEmployeeCode")%></label>
                                    </div>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtEmployeeId" CssClass="form-control control-validation  " runat="server" autocomplete="off" type="number"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-5">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-5 text-left">
                                        <label for="<%=cboDepartmentId.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblDepartmentCode")%></label>
                                    </div>
                                    <div class="col-sm-7">
                                        <asp:DropDownList ID="cboDepartmentId" CssClass="form-control control-validation" data-live-search="true" AutoPostBack="false" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-2">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-5 text-left">
                                    </div>
                                    <div class="col-sm-7 text-right">
                                        <button id="Button1" type="button" runat="server" class="btn btn-default btnAjaxAction" onserverclick="btnSearchFilter_ServerClick" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>'>
                                            <span class="glyphicon glyphicon-search glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%= GetLocalResourceObject("btnSearch") %>
                                        </button>
                                    </div>
                                </div>
                            </div>
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
                                    CssClass="table table-striped table-hover table-bordered" DataKeyNames="WorkingTimeHoursByDepartmentEmployeesID"
                                    OnPreRender="grvList_PreRender" OnSorting="grvList_Sorting">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" Visible="false">
                                            <HeaderTemplate>
                                                <div style="width: 100%; vertical-align: middle; text-align: center;">
                                                    <asp:LinkButton ID="lbtnWorkingTimeHoursByDepartmentEmployeesIDSort" runat="server" CommandName="Sort" CommandArgument="WorkingTimeHoursByDepartmentEmployeesID">                
                                                        <span><%= GetLocalResourceObject("lblWorkingTimeHoursByDepartmentEmployeesID") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvWorkingTimeHoursByDepartmentEmployeesID" data-id="WorkingTimeHoursByDepartmentEmployeesID" data-value="<%# Eval("WorkingTimeHoursByDepartmentEmployeesID") %>"><%# Eval("WorkingTimeHoursByDepartmentEmployeesID") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" HeaderStyle-VerticalAlign="Middle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="lbtnDepartmentCodeSort" runat="server" CommandName="Sort" CommandArgument="DepartmentCode">                
                                                        <span><%= GetLocalResourceObject("lblDepartmentCode") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvDepartmentCode" data-id="DepartmentCode" data-value="<%# Eval("Department") %>"><%# Eval("Department") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; vertical-align: middle; text-align: center;">
                                                    <asp:LinkButton ID="lbtnEmployeeCodeSort" runat="server" CommandName="Sort" CommandArgument="EmployeeCode">                
                                                        <span><%= GetLocalResourceObject("lblEmployeeName") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvEmployeeCode" data-id="EmployeeCode" data-value="<%# Eval("EmployeeName") %>"><%# Eval("EmployeeName") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" HeaderStyle-VerticalAlign="Middle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="lbtnStartDateSort" runat="server" CommandName="Sort" CommandArgument="StartDate">                
                                                        <span><%= GetLocalResourceObject("lblStartDate") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvStartDate" data-id="StartDate" data-value="<%# Eval("StartDateFormatted") %>"><%# Eval("StartDateFormatted") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" HeaderStyle-VerticalAlign="Middle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="lbtnEndDateSort" runat="server" CommandName="Sort" CommandArgument="EndDate">                
                                                        <span><%= GetLocalResourceObject("lblEndDate") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvEndDate" data-id="EndDate" data-value="<%# Eval("EndDateFormatted") %>"><%# Eval("EndDateFormatted") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" HeaderStyle-VerticalAlign="Middle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="lbtnStartHourSort" runat="server" CommandName="Sort" CommandArgument="StartHour">                
                                                        <span><%= GetLocalResourceObject("lblStartHour") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvStartHour" data-id="StartHour" data-value="<%# Eval("StartHour") %>"><%# Eval("StartHour") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" HeaderStyle-VerticalAlign="Middle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="lbtnEndHourSort" runat="server" CommandName="Sort" CommandArgument="EndHour">                
                                                        <span><%= GetLocalResourceObject("lblEndHour") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvEndHour" data-id="EndHour" data-value="<%# Eval("EndHour") %>"><%# Eval("EndHour") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" HeaderStyle-VerticalAlign="Middle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="lbtnSecondStartHourSort" runat="server" CommandName="Sort" CommandArgument="SecondStartHour">                
                                                        <span><%= GetLocalResourceObject("lblSecondStartHour") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvStartHour" data-id="StartHour" data-value="<%# Eval("SecondStartHour") %>"><%# Eval("SecondStartHour") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" HeaderStyle-VerticalAlign="Middle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="lbtnSecondEndHourSort" runat="server" CommandName="Sort" CommandArgument="SecondEndHour">                
                                                        <span><%= GetLocalResourceObject("lblSecondEndHour") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvEndHour" data-id="EndHour" data-value="<%# Eval("SecondEndHour") %>"><%# Eval("SecondEndHour") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" HeaderStyle-VerticalAlign="Middle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="lbtnlWorkingTimeTypeCodeSort" runat="server" CommandName="Sort" CommandArgument="lWorkingTimeTypeCode">                
                                                        <span><%= GetLocalResourceObject("lblWorkingTimeTypeCode") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvlWorkingTimeTypeCode" data-id="lWorkingTimeTypeCode" data-value="<%# Eval("WorkingTimeType") %>"><%# Eval("WorkingTimeType") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" HeaderStyle-VerticalAlign="Middle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="lbtnlRestDay" runat="server" CommandName="Sort" CommandArgument="lRestDay">                
                                                        <span><%= GetLocalResourceObject("lblRestDay") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvlRestDay" data-id="lRestDay" data-value="<%# Eval("RestDayDescription") %>"><%# Eval("RestDayDescription") %></span>
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
        <div class="modal-dialog " role="document">
            <div class="modal-content" style="width:185% !important;right:42% !important;">
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
                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="form-horizontal">
                                        <asp:HiddenField ID="hdfWorkDepartmentCode" runat="server" Value="" />
                                        <div class="form-group" style="display:none;">
                                            <div class="col-sm-5 text-left">
                                                <label for="<%= txtWorkingTimeHoursByDepartmentEmployeesID.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblWorkingTimeHoursByDepartmentEmployeesID")%></label>
                                            </div>
                                            <div class="col-sm-7">
                                                <asp:TextBox ID="txtWorkingTimeHoursByDepartmentEmployeesID" ReadOnly="true" CssClass="form-control control-validation" runat="server" autocomplete="off" type="number"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-sm-5 text-left">
                                                <label for="<%= cboDepartmentCode.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblDepartmentCode")%></label>
                                            </div>
                                            <div class="col-sm-7">
                                                <asp:DropDownList ID="cboDepartmentCode" OnSelectedIndexChanged="cboDepartmentCode_SelectedIndexChanged" CssClass="form-control control-validation selectpicker" data-live-search="true" AutoPostBack="true" runat="server"></asp:DropDownList>
                                                 <label id="lblcboDepartmentCode" for="<%= cboDepartmentCode.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-sm-5 text-left">
                                                <label for="<%= cboEmployeeCode.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblEmployeeCode")%></label>
                                            </div>
                                            <div class="col-sm-7">
                                                <asp:DropDownList ID="cboEmployeeCode" CssClass="form-control control-validation selectpicker" data-live-search="true" AutoPostBack="false" runat="server"></asp:DropDownList>
                                                 <label id="lblcboEmployeeCode" for="<%= cboEmployeeCode.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-sm-5 text-left">
                                                <label for="<%= cboWorkingTimeTypeCode.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblWorkingTimeTypeCode")%></label>
                                            </div>
                                            <div class="col-sm-7">
                                                <asp:DropDownList ID="cboWorkingTimeTypeCode" CssClass="form-control control-validation" data-live-search="true" AutoPostBack="false" runat="server"></asp:DropDownList>
                                                 <label id="lblcboWorkingTimeTypeCode" for="<%= cboWorkingTimeTypeCode.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-sm-5 text-left">
                                                <label for="<%= dtpStartDate.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblStartDate")%></label>
                                            </div>
                                            <div class="col-sm-7">
                                                <asp:TextBox runat="server" ID="dtpStartDate" class="form-control date control-validation cleanPasteDigits" placeholder='<%$ Code:GetLocalResourceObject("lblDateFormatPlaceHolder") %>' autocomplete="off" />
                                                <label id="dtpStartDateValidation" for="<%= dtpStartDate.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-sm-5 text-left">
                                                <label for="<%= dtpEndDate.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblEndDate")%></label>
                                            </div>
                                            <div class="col-sm-7">
                                                <asp:TextBox runat="server" ID="dtpEndDate" class="form-control date control-validation cleanPasteDigits" placeholder='<%$ Code:GetLocalResourceObject("lblDateFormatPlaceHolder") %>' autocomplete="off" />
                                                <label id="dtpEndDateValidation" for="<%= dtpEndDate.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                            </div>
                                        </div>
                                       
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="form-horizontal">
                                         <div class="form-group">
                                            <div class="col-sm-5 text-left">
                                                <label for="<%= cboStartHour.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblStartHour")%></label>
                                            </div>
                                            <div class="col-sm-7">
                                                <asp:DropDownList ID="cboStartHour"  runat="server" CssClass="form-control selectpicker" ></asp:DropDownList>
                                                <label id="cboStartHourValidation" for="<%= cboStartHour.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-sm-5 text-left">
                                                <label for="<%= cboEndHour.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblEndHour")%></label>
                                            </div>
                                            <div class="col-sm-7">
                                                <asp:DropDownList ID="cboEndHour" runat="server"   CssClass="form-control control-validation selectpicker" ></asp:DropDownList>
                                                <label id="cboEndHourValidation" for="<%= cboEndHour.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-sm-5 text-left">
                                                <label for="<%= cboSecondStartHour.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblSecondStartHour")%></label>
                                            </div>
                                            <div class="col-sm-7">
                                                <asp:DropDownList ID="cboSecondStartHour" runat="server"   CssClass="form-control selectpicker" ></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-sm-5 text-left">
                                                <label for="<%= cboSecondEndHour.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblSecondEndHour")%></label>
                                            </div>
                                            <div class="col-sm-7">
                                                <asp:DropDownList ID="cboSecondEndHour" runat="server"   CssClass="form-control selectpicker" ></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-sm-5 text-left">
                                                <label for="<%= txtTotalHours.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblTotalHours")%></label>
                                            </div>
                                            <div class="col-sm-7">
                                                 <asp:TextBox ID="txtTotalHours" ReadOnly="true" CssClass="form-control control-validation" runat="server" autocomplete="off"></asp:TextBox>
                                                <asp:HiddenField ID="hdnTotalHours" runat="server" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-sm-5 text-left">
                                                <label class="control-label"><%=GetLocalResourceObject("lblRestDay")%></label>
                                            </div>
                                            <div class="col-sm-7">
                                                <asp:DropDownList ID="cboRestDay" runat="server" CssClass="form-control control-validation selectpicker" data-live-search="true" multiple="multiple" data-selected-text-format="count > 6"></asp:DropDownList>                                                         
                                                <asp:HiddenField ID="hdnAllDays" runat="server" />
                                            </div>
                                        </div>
                                        
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
            $('#<%= dtpStartDate.ClientID %>').datetimepicker({
                format: 'MM/DD/YYYY',

            });

            $('#<%= dtpEndDate.ClientID %>').datetimepicker({
                format: 'MM/DD/YYYY'
            });

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
                setTimeout(function () { $this.button('reset'); }, 7000);
            });

            //And the grvList selection row functionality
            $('#<%= grvList.ClientID %>').on('click', 'tbody tr', function (event) {

                if (!$(this).hasClass('emptyRow')) {
                    $(this).toggleClass('info').siblings().removeClass('info');
                    $('#<%=hdfSelectedRowIndex.ClientID%>').val($(this).hasClass("info") ? $(this).index() : -1);
                }
            });

            $("#<%=cboStartHour.ClientID %>").on('changed.bs.select', function (e) {
                
                CalcularHoras();
            });
            $("#<%=cboEndHour.ClientID %>").on('changed.bs.select', function (e) {
                
                CalcularHoras();
            });
            $("#<%=cboSecondStartHour.ClientID %>").on('changed.bs.select', function (e) {
                
                CalcularHoras();
            });
            $("#<%=cboSecondEndHour.ClientID %>").on('changed.bs.select', function (e) {
                
                CalcularHoras();
            });

            $("#<%= cboRestDay.ClientID %>").on('changed.bs.select', function (e) {
                /// <summary>Handles the select's value changed event for the cboTransportMeansThatHas control.</summary>
                MultiSelectDropdownListSaveSelectedItems($("#<%= cboRestDay.ClientID %>"), $("#<%= hdnAllDays.ClientID %>"));
               /// RestoreSelectedRestDay();
            });

            //And the grvList pager event for client side
            $('#<%= blstPager.ClientID %>').on('click', 'a', function (event) {
                //SetWaitingGrvList();
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
                $("#<%=txtWorkingTimeHoursByDepartmentEmployeesID.ClientID%>").val("");
                $("#<%=cboDepartmentCode.ClientID%>").val("");
                $("#<%=cboEmployeeCode.ClientID%>").val("");
                $("#<%=dtpStartDate.ClientID%>").val("");
                $("#<%=dtpEndDate.ClientID%>").val("");
                $("#<%=cboStartHour.ClientID%>").val("");
                $("#<%=cboEndHour.ClientID%>").val("");
                $("#<%=cboWorkingTimeTypeCode.ClientID%>").val("");
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


        function CalcularHoras() {
            var startHour = $('#<%= cboStartHour.ClientID %>').val();
            var endHour = $('#<%= cboEndHour.ClientID %>').val() ;
            var secondStartHour = $('#<%= cboSecondStartHour.ClientID %>').val() ;
            var secondEndHour = $('#<%= cboSecondEndHour.ClientID %>').val();
            var diffHours = 0;
            var diffMinuts = 0;

            if (!isNullOrEmpty(startHour) && !isNullOrEmpty(endHour)) {
                var starHourArray = startHour.split(":");
                var endHourArray = endHour.split(":");
                diffHours +=  Math.abs(endHourArray[0] - starHourArray[0]);
                diffMinuts +=  Math.abs(endHourArray[1] - starHourArray[1]);
            }
            if (!isNullOrEmpty(secondStartHour) && !isNullOrEmpty(secondEndHour)) {
                var secondStarHourArray = secondStartHour.split(":");
                var secondEndHourArray = secondEndHour.split(":");
                diffHours +=  Math.abs(secondEndHourArray[0] - secondStarHourArray[0]);
                diffMinuts +=  Math.abs(secondEndHourArray[1] - secondStarHourArray[1]);
            }
            if (diffMinuts.toString().length == 1)
                diffMinuts = '0' + diffMinuts;
            if (diffMinuts == "60") {
                diffHours+=1;
                diffMinuts = "00";
            }

            $('#<%= txtTotalHours.ClientID %>').val(diffHours + ":" + diffMinuts);
            $('#<%= hdnTotalHours.ClientID %>').val(diffHours + ":" + diffMinuts);
        }

        function validateTimeJourney() {
            var listWorkingTime = JSON.parse(localStorage.getItem('WorkingType'));
            var selectedJourney =  $('#<%= cboWorkingTimeTypeCode.ClientID %>').val();
            var timeRegister = $('#<%= hdnTotalHours.ClientID %>').val();

            var workingTimeEntity = listWorkingTime.filter(z => z.WorkingTimeTypeCode == selectedJourney);
            var timeArray = timeRegister.split(":");
            if (parseInt(timeArray[0]) == workingTimeEntity[0].TotalWorkingTime) {
                if (parseInt(timeArray[1]) > 0) {
                    return false;
                }
            } else if (parseInt(timeArray[0]) > workingTimeEntity[0].TotalWorkingTime) {
                return false;
            }
            return true;
        }

        function isNullOrEmpty(value) {
            return value == null || value === "";
        }

        function RestoreSelectedRestDay() {
            /// <summary>Recover the selected items that were stored and assign them to the control to be selected.</summary>
            MultiSelectDropdownListRestoreSelectedItems($('#<%= cboRestDay.ClientID %>'), $('#<%= hdnAllDays.ClientID %>'));
        }
        
        var validator = null;

        var validatorSearchUser = null;

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
            $("#<%=txtWorkingTimeHoursByDepartmentEmployeesID.ClientID%>").prop('disabled', false);
            if (!ValidateForm()) {
                setTimeout(function () {
                    ResetButton(resetId);
                    enableButton($('#btnCancel'));
                    $(".btnAccept").prop("disabled", false);
                }, 150);
            }
            else {
                if (validateTimeJourney()) {
                    __doPostBack('<%= btnAccept.UniqueID %>', '');
                } else {
                    MostrarMensaje(TipoMensaje.ERROR, '<%= GetLocalResourceObject("msjBigTimeRegisterJourney").ToString()%>', null);
                    ResetButton(resetId);
                    enableButton($('#btnCancel'));
                    $(".btnAccept").prop("disabled", false);
                }
                
            }
            $("#<%=txtWorkingTimeHoursByDepartmentEmployeesID.ClientID%>").prop('disabled', true);
            return false;
        }

        var validator = null;
        function ValidateForm() {
            /// <returns> True if form is valid. False otherwise. </returns>
            $('#' + document.forms[0].id).validate().destroy();

            //add custom validation methods
            jQuery.validator.addMethod("validSelection", function (value, element) {
                debugger;
                return this.optional(element) || value != "-1";
            }, "Please select a valid option");

            if (validator == null) {
                //declare the validator
                var drpvalue = document.getElementById("<%=cboWorkingTimeTypeCode.ClientID%>");
                drpvalue.value = drpvalue.value === "0" ? "" : drpvalue.value;


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
                            "<%= txtWorkingTimeHoursByDepartmentEmployeesID.UniqueID %>": {
                                required: true
                                , normalizer: function (value) {
                                return $.trim(value);
                                }
                            },
                            "<%= cboDepartmentCode.UniqueID %>": {
                                required: true
                                , normalizer: function (value) {
                                    return $.trim(value);
                                }
                            },
                            "<%= dtpStartDate.UniqueID %>": {
                                required: true
                                , normalizer: function(value) {
                                    return $.trim(value);
                                }
                            },

                            "<%= dtpEndDate.UniqueID %>": {
                                required: true
                                , normalizer: function (value) {
                                    return $.trim(value);
                                }
                            },
                            "<%= cboStartHour.UniqueID %>": {
                                required: true
                                , validSelection: true
                            },
                            "<%= cboEndHour.UniqueID %>": {
                                required: true
                                , validSelection: true
                            },
                            "<%= cboWorkingTimeTypeCode.UniqueID %>": {
                                required: true
                                , normalizer: function(value) {
                                    return $.trim(value);
                                }
                            },
                            "<%= cboEmployeeCode.UniqueID %>": {
                                required: true
                                , normalizer: function (value) {
                                    return $.trim(value);
                                }
                            },
                            "<%= cboRestDay.UniqueID %>": {
                                required: true
                                , validSelection: true
                            },
                      }///Este es el fin del Rules
            });
        }
         else
        {
            validator.validate();
        }
        //get the results   
        var result = validator.form();
        drpvalue.value = drpvalue.value === "" ? "0" : drpvalue.value;
        
        return result;
        }



        function ProcessBtnAddUserRequest(resetId) {
            /// <summary>Process the selected user search</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            if (!IsOnlyOneRowCheked($("#<%= txtWorkingTimeHoursByDepartmentEmployeesID.ClientID%>"))) {
                ErrorButton(resetId);
                MostrarMensaje(TipoMensaje.VALIDACION, '<%= GetLocalResourceObject("msgInvalidSelection") %>', function () {
                    ResetButton(resetId);
                });

                <%--ShowFooterError('<%= GetLocalResourceObject("msgInvalidSelection") %>');--%>
            }
            else {
                var checks = $('#<%= txtWorkingTimeHoursByDepartmentEmployeesID.ClientID%>').find('input:checkbox[id*="chkSelected"]:checked:not(#chkSelectedAll)');
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
