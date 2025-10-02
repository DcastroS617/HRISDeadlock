<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="GtiPeriod.aspx.cs" Inherits="HRISWeb.GTI.GtiPeriod" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntHeader" runat="server">
    <title><%=Convert.ToString(GetLocalResourceObject("lblScreenTitle")) %></title>
    <link href="../../Content/css/custom-tabs.css" rel="stylesheet" />
    <link href="../../Content/css/dynamicTable.css" rel="stylesheet" />
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
                                    <div class="col-sm-2 text-left">
                                        <label for="<%= txtPeriodCampaignDescription.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblNameGtiPeriod")%></label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtPeriodCampaignDescription" CssClass="form-control control-validation cleanPasteText ignoreValidation" runat="server" onkeypress="blockEnterKey(); return isNumberOrLetter(event)" MaxLength="254" autocomplete="off" type="text"></asp:TextBox>
                                    </div>

                                    <div class="col-sm-2 text-left">
                                        <label for="<%= cboQuarterYearFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblYear")%></label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:DropDownList ID="cboQuarterYearFilter" CssClass="form-control control-validation" AutoPostBack="false" runat="server" data-id="Status" data-value="isPermitted"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-2 text-left">
                                        <label for="<%= cboQuarterIDFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblQuarterID")%></label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:DropDownList ID="cboQuarterIDFilter" CssClass="form-control control-validation" AutoPostBack="false" runat="server" data-id="Status" data-value="isPermitted"></asp:DropDownList>
                                    </div>

                                    <div class="col-sm-2 text-left">
                                        <label for="<%= cboPeriodStateFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblPeriodState")%></label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:DropDownList ID="cboPeriodStateFilter" CssClass="form-control control-validation" AutoPostBack="false" runat="server" data-id="Status" data-value="isPermitted"></asp:DropDownList>
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
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-sm-12 text-center">
                            <asp:HiddenField runat="server" ID="hdfSelectedRowIndex" Value="-1" />
                            <div>
                                <asp:GridView ID="grvList" Width="100%" runat="server" EmptyDataText='<%$ Code:GetLocalResourceObject("lblNoData") %>'
                                    EmptyDataRowStyle-CssClass="emptyRow" AllowPaging="false" PagerSettings-Visible="false" AllowSorting="true"
                                    AutoGenerateColumns="false" ShowHeader="true" CssClass="table table-striped table-hover table-bordered"
                                    DataKeyNames="PeriodCampaignId" OnPreRender="GrvList_PreRender" OnSorting="GrvList_Sorting" OnRowDataBound="GrvList_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle hidden" HeaderStyle-CssClass="hidden" FooterStyle-CssClass="hidden">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="PeriodCampaignIdSort" runat="server" CommandName="Sort" CommandArgument="PeriodCampaignId" OnClientClick="SetWaitingGrvList(true);">          
                                                        <span><%= GetLocalResourceObject("PeriodCampaignId.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "PeriodCampaignId") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvPeriodCampaignId" data-id="PrincipalPeriodCampaignId" data-value="<%# Eval("PeriodCampaignId") %>"><%# Eval("PeriodCampaignId") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="PeriodCampaignDescriptionSort" runat="server" CommandName="Sort" CommandArgument="PeriodCampaignDescription" OnClientClick="SetWaitingGrvList(true);">          
                                                        <span><%= GetLocalResourceObject("PeriodCampaignDescription.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "PeriodCampaignDescription") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvPeriodCampaignDescription" data-id="PeriodCampaignDescription" data-value="<%# Eval("PeriodCampaignDescription") %>"><%# Eval("PeriodCampaignDescription")%></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="QuarterIDSort" runat="server" CommandName="Sort" CommandArgument="QuarterID" OnClientClick="SetWaitingGrvList(true);">          
                                                        <span><%= GetLocalResourceObject("QuarterID.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "QuarterID") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvQuarterID" data-id="QuarterID" data-value="<%# Eval("QuarterPeriodName") %>"><%# Eval("QuarterPeriodName")%></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="QuarterYearSort" runat="server" CommandName="Sort" CommandArgument="QuarterYear" OnClientClick="SetWaitingGrvList(true);">          
                                                        <span><%= GetLocalResourceObject("QuarterYear.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "QuarterYear") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvQuarterYear" data-id="QuarterYear" data-value="<%# Eval("QuarterYear") %>"><%# Eval("QuarterYear")%></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="InitialDateSort" runat="server" CommandName="Sort" CommandArgument="InitialDate" OnClientClick="SetWaitingGrvList(true);">          
                                                        <span><%= GetLocalResourceObject("InitialDate.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "InitialDate") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvInitialDate" data-id="InitialDate" data-value="<%# Eval("InitialDate") %>"><%# Eval("InitialDate")%></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="FinalDateSort" runat="server" CommandName="Sort" CommandArgument="FinalDate" OnClientClick="SetWaitingGrvList(true);">          
                                                        <span><%= GetLocalResourceObject("FinalDate.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "FinalDate") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvFinalDate" data-id="FinalDate" data-value="<%# Eval("FinalDate") %>"><%# Eval("FinalDate")%></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="PeriodStateSort" runat="server" CommandName="Sort" CommandArgument="PeriodState" OnClientClick="SetWaitingGrvList(true);">          
                                                        <span><%= GetLocalResourceObject("PeriodState.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "PeriodState") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvPeriodState" data-id="PeriodState" data-value="<%# Eval("PeriodStateDescrition") %>"><%# Eval("PeriodStateDescrition")%></span>
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
                        <button id="btnAdd" type="button" runat="server" class="btn btn-default btnAjaxAction btns" onserverclick="btnAdd_ServerClick" onclick="return false;" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnAdd"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnAdd"))%>'>
                            <span class="glyphicon glyphicon-plus glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnAdd") %>
                        </button>
                        <button id="btnEdit" type="button" runat="server" class="btn btn-default btnAjaxAction btns"
                            onserverclick="btnEdit_ServerClick" onclick="return ProcessEditRequest(this.id);"
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
                        <button id="btnGtiConfig" type="button" runat="server" class="btn btn-default btnAjaxAction"
                            onserverclick="BtnGtiConfig_ServerClick" onclick="return ProcessConfigRequest(this.id);"
                            data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnGtiConfig"))%>'
                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnGtiConfig"))%>'>
                            <span class="glyphicon glyphicon-cog glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnGtiConfig") %>
                        </button>
                        <button id="btnManagers" type="button" runat="server" class="btn btn-default btnAjaxAction"
                            onserverclick="BtnManagers_ServerClick" onclick="return ProcessManagersRequest(this.id);"
                            data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnManagers"))%>'
                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnManagers"))%>'>
                            <span class="glyphicon glyphicon-user glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnManagers") %>
                        </button>
                        <button id="btnSummary" type="button" runat="server" class="btn btn-default btnAjaxAction"
                            onserverclick="BtnSummary_ServerClick" onclick="return ProcessSummaryRequest(this.id);"
                            data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnSummary"))%>'
                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnSummary"))%>'>
                            <span class="glyphicon glyphicon-list glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnSummary") %>
                        </button>
                        <button id="btnRunReview" type="button" runat="server" class="btn btn-default btnAjaxAction"
                            onserverclick="btnRunReview_Click" onclick="return ProcessRunReviewRequest(this.id);"
                            data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnReview"))%>'
                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnReview"))%>'>
                            <span class="glyphicon glyphicon-ok glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnReview") %>
                        </button>

                        <%--<button id="btnReview" type="button" runat="server" class="btn btn-default btnAjaxAction"
                            onserverclick="BtnReview_ServerClick" onclick="return ProcessReviewRequest(this.id);"
                            data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnReview"))%>'
                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnReview"))%>'>
                            <span class="glyphicon glyphicon-check glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnReview") %>
                        </button>--%>
                        <%-- <button id="BtnGtiReports" type="button" runat="server" class="btn btn-default btnAjaxAction"
                            onserverclick="BtnGtiReports_ServerClick" onclick="return ProcessReportsRequest(this.id);"
                            data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnGtiReports"))%>'
                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnGtiReports"))%>'>
                            <span class="glyphicon glyphicon-file glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnGtiReports") %>--%>
                        </button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <%--  Modal for Report --%>
    <%--<div class="modal fade" id="ReportDialog" tabindex="-1" role="dialog" aria-labelledby="ReportDialogTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg" style="width: 98%!important" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" id="btnReportClose" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title text-primary" id="dialogTitleReport"><%=GetLocalResourceObject("TitleReportDialog")%></h3>
                </div>

                <div class="modal-body">

                    <asp:UpdatePanel runat="server" ID="uppReportsGTI" UpdateMode="Always">
                        <Triggers>
                        </Triggers>

                        <ContentTemplate>
                            <div class="row">
                                <div class="col-sm-3 text-left">
                                    <label for="<%= txtPeriodNameReport.ClientID%>" class="control-label"><%= GetLocalResourceObject("lblNameGtiPeriod") %></label>
                                    <asp:TextBox ID="txtPeriodNameReport" CssClass="form-control control-validation cleanPasteText" runat="server" MaxLength="254" autocomplete="off" type="text" ReadOnly></asp:TextBox>
                                </div>

                                <div class="col-sm-3 text-left">
                                    <label for="<%= txtPeriodStateReport.ClientID%>" class="control-label"><%= GetLocalResourceObject("lblPeriodState") %></label>
                                    <asp:TextBox ID="txtPeriodStateReport" CssClass="form-control control-validation cleanPasteText" runat="server" MaxLength="254" autocomplete="off" type="text" ReadOnly></asp:TextBox>
                                </div>

                                <div class="col-sm-3 text-left">
                                    <label for="<%= txtQuarterYearReport.ClientID%>" class="control-label"><%= GetLocalResourceObject("lblYear") %></label>
                                    <asp:TextBox ID="txtQuarterYearReport" CssClass="form-control control-validation cleanPasteText" runat="server" MaxLength="254" autocomplete="off" type="text" ReadOnly></asp:TextBox>
                                </div>

                                <div class="col-sm-3 text-left">
                                    <label for="<%= txtQuarterIdReport.ClientID%>" class="control-label"><%= GetLocalResourceObject("lblQuarterID") %></label>
                                    <asp:TextBox ID="txtQuarterIdReport" CssClass="form-control control-validation cleanPasteText" runat="server" MaxLength="254" autocomplete="off" type="text" ReadOnly></asp:TextBox>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                    <!-- Tabs -->
                    <ul class="nav nav-tabs" id="myTab" role="tablist">
                        <li class="nav-item">
                            <a class="nav-link active" id="gti-tab" data-toggle="tab" href="#gti" role="tab" aria-controls="gti" aria-selected="true" onclick="showTab(this)">GTI</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" id="hmt-tab" data-toggle="tab" href="#hmt" role="tab" aria-controls="hmt" aria-selected="false" onclick="showTab(this)">HMT</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" id="hle-tab" data-toggle="tab" href="#hle" role="tab" aria-controls="hle" aria-selected="false" onclick="showTab(this)">HLE</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" id="dui-tab" data-toggle="tab" href="#dui" role="tab" aria-controls="dui" aria-selected="false" onclick="showTab(this)">DUI</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" id="qke-tab" data-toggle="tab" href="#qke" role="tab" aria-controls="qke" aria-selected="false" onclick="showTab(this)">QKE</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" id="sct-tab" data-toggle="tab" href="#sct" role="tab" aria-controls="sct" aria-selected="false" onclick="showTab(this)">SCT</a>
                        </li>
                    </ul>
                    <div class="tab-content" id="myTabContent">
                        <div class="tab-pane fade show active in" id="gti" role="tabpanel" aria-labelledby="gti-tab">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div id="tableContainer" class="table-container">
                                        <div class="button-container">
                                            <button id="btnStepOneDownload" type="button" runat="server" class="btn btn-default btnAjaxAction"
                                                onserverclick="BtnStepOneDownload_ServerClick" onclick=""
                                                data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnStepOneDownload"))%>'
                                                data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnStepOneDownload"))%>'>
                                                <span class="glyphicon glyphicon-cog glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                                                <br />
                                                <%= GetLocalResourceObject("btnStepOneDownload") %>
                                            </button>
                                        </div>
                                        <div id="tableWrapper" class="table-wrapper">
                                            <div id="gtiDiv" runat="server" style="width: 100%; overflow-x: auto; white-space: nowrap;"></div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </div>
                        <div class="tab-pane fade" id="hmt" role="tabpanel" aria-labelledby="hmt-tab">
                            <p>Content for HMT</p>
                        </div>
                        <div class="tab-pane fade" id="hle" role="tabpanel" aria-labelledby="hle-tab">
                            <p>Content for HLE</p>
                        </div>
                        <div class="tab-pane fade" id="dui" role="tabpanel" aria-labelledby="dui-tab">
                            <p>Content for DUI</p>
                        </div>
                        <div class="tab-pane fade" id="qke" role="tabpanel" aria-labelledby="qke-tab">
                            <p>Content for QKE</p>
                        </div>
                        <div class="tab-pane fade" id="sct" role="tabpanel" aria-labelledby="sct-tab">
                            <p>Content for SCT</p>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button id="btnAcceptReport" type="button" runat="server" class="btn btn-primary btnAjaxAction"
                            onserverclick="btnAccept_ServerClick" onclick="return ProcessAcceptRequest(this.id);"
                            data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>'
                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>'>
                            <span class="glyphicon glyphicon-ok glyphicon-main-button" aria-hidden="true"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnDialogSave")) %>
                        </button>

                        <button id="btnCancelReport" type="button" class="btn btn-default">
                            <span class="glyphicon glyphicon-remove glyphicon-main-button" aria-hidden="true"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnDialogCancel")) %>
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>--%>

    <%--  Modal for Configuration --%>
    <div class="modal fade" id="ConfigDialog" tabindex="-1" role="dialog" aria-labelledby="ConfigDialogTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg" style="width: 98%!important" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" id="btnConfigClose" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title text-primary" id="configDialogTitle"><%=GetLocalResourceObject("configDialogTitle")%></h3>
                </div>

                <div class="modal-body">
                    <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-sm-3 text-left">
                                    <label for="<%= txtPeriodNameConfig.ClientID%>" class="control-label"><%= GetLocalResourceObject("lblNameGtiPeriod") %></label>
                                    <asp:TextBox ID="txtPeriodNameConfig" CssClass="form-control control-validation cleanPasteText" runat="server" MaxLength="254" autocomplete="off" type="text" ReadOnly></asp:TextBox>
                                </div>

                                <div class="col-sm-3 text-left">
                                    <label for="<%= txtPeriodStateConfig.ClientID%>" class="control-label"><%= GetLocalResourceObject("lblPeriodState") %></label>
                                    <asp:TextBox ID="txtPeriodStateConfig" CssClass="form-control control-validation cleanPasteText" runat="server" MaxLength="254" autocomplete="off" type="text" ReadOnly></asp:TextBox>
                                </div>

                                <div class="col-sm-3 text-left">
                                    <label for="<%= txtQuarterYearConfig.ClientID%>" class="control-label"><%= GetLocalResourceObject("lblYear") %></label>
                                    <asp:TextBox ID="txtQuarterYearConfig" CssClass="form-control control-validation cleanPasteText" runat="server" MaxLength="254" autocomplete="off" type="text" ReadOnly></asp:TextBox>
                                </div>

                                <div class="col-sm-3 text-left">
                                    <label for="<%= txtQuarterIdConfig.ClientID%>" class="control-label"><%= GetLocalResourceObject("lblQuarterID") %></label>
                                    <asp:TextBox ID="txtQuarterIdConfig" CssClass="form-control control-validation cleanPasteText" runat="server" MaxLength="254" autocomplete="off" type="text" ReadOnly></asp:TextBox>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                    <!-- Tabs -->
                    <ul class="nav nav-tabs" id="ConfigTab" role="tablist" data-target="#configTabContent">
                        <li class="nav-item">
                            <a class="nav-link active" id="Gti-tab" data-toggle="tab" href="#gti" role="tab" aria-controls="gti" aria-selected="true">GTI</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" id="hmt-tab" data-toggle="tab" href="#hmt" role="tab" aria-controls="hmt" aria-selected="false">HMT</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" id="hle-tab" data-toggle="tab" href="#hle" role="tab" aria-controls="hle" aria-selected="false">HLE</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" id="dui-tab" data-toggle="tab" href="#dui" role="tab" aria-controls="dui" aria-selected="false">DUI</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" id="qke-tab" data-toggle="tab" href="#qke" role="tab" aria-controls="qke" aria-selected="false">QKE</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" id="sct-tab" data-toggle="tab" href="#sct" role="tab" aria-controls="sct" aria-selected="false">SCT</a>
                        </li>
                    </ul>
                    <div class="tab-content" id="configTabContent">
                        <!-- Tab Pane para GTI -->
                        <div class="tab-pane fade show active" id="gti" role="tabpanel" aria-labelledby="config-Gti-tab">
                            <asp:UpdatePanel ID="UpdatePanelGti" runat="server">
                                <ContentTemplate>
                                    <div id="gtiParametersContainer" runat="server">
                                        <asp:Repeater ID="rptGtiParameters" runat="server">
                                            <HeaderTemplate>
                                                <table class="table table-bordered">
                                                    <thead>
                                                        <tr>
                                                            <th>Habilitar configuración</th>
                                                            <th>Pre-aprobación</th>
                                                            <th>Responsable de proceso</th>
                                                            <th>Tipo de cambio</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <!-- Habilitar configuración -->
                                                    <td>
                                                        <asp:HiddenField ID="hdfParameterId" runat="server" Value='<%# Eval("PeriodParameterDivisionCurrencyId") %>' />
                                                        <asp:CheckBox ID="chkEnableConfig" runat="server" Checked='<%# Eval("IsEnabled") %>' />
                                                        <%# Eval("ParameterName") %>
                                                    </td>
                                                    <!-- Pre-aprobación -->
                                                    <td>
                                                        <asp:CheckBox ID="chkPreApproval" runat="server" Checked='<%# Eval("RequiresPreApproval") %>' />
                                                        Requiere preaprobación
                                                    </td>
                                                    <!-- Responsable de proceso -->
                                                    <td>
                                                        <asp:TextBox ID="txtResponsible" runat="server" Text='<%# Eval("ProcessResponsible") %>' CssClass="form-control control-validation cleanPasteText ignoreValidation" autocomplete="off" />
                                                    </td>
                                                    <!-- Tipo de cambio -->
                                                    <td>
                                                        <asp:TextBox ID="txtExchangeRate" runat="server" Text='<%# Eval("ExchangeRate") %>' CssClass="form-control control-validation cleanPasteText ignoreValidation" autocomplete="off" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </tbody>
                                                </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>

                        <!-- Tab Pane para HMT -->
                        <div class="tab-pane fade" id="hmt" role="tabpanel" aria-labelledby="config-hmt-tab">
                            <asp:UpdatePanel ID="UpdatePanelHmt" runat="server">
                                <ContentTemplate>
                                    <div id="hmtParametersContainer" runat="server">
                                        <asp:Repeater ID="rptHmtParameters" runat="server">
                                            <HeaderTemplate>
                                                <table class="table table-bordered">
                                                    <thead>
                                                        <tr>
                                                            <th>Habilitar configuración</th>
                                                            <th>Pre-aprobación</th>
                                                            <th>Responsable de proceso</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <!-- Habilitar configuración -->
                                                    <td>
                                                        <asp:HiddenField ID="hdfParameterId" runat="server" Value='<%# Eval("PeriodParameterDivisionCurrencyId") %>' />
                                                        <asp:CheckBox ID="chkEnableConfig" runat="server" Checked='<%# Eval("IsEnabled") %>' />
                                                        <%# Eval("ParameterName") %>
                                                    </td>
                                                    <!-- Pre-aprobación -->
                                                    <td>
                                                        <asp:CheckBox ID="chkPreApproval" runat="server" Checked='<%# Eval("RequiresPreApproval") %>' />
                                                        Requiere preaprobación
                                                    </td>
                                                    <!-- Responsable de proceso -->
                                                    <td>
                                                        <asp:TextBox ID="txtResponsible" runat="server" Text='<%# Eval("ProcessResponsible") %>' CssClass="form-control control-validation cleanPasteText ignoreValidation" autocomplete="off" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </tbody>
                        </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>

                        <!-- Tab Pane para HLE -->
                        <div class="tab-pane fade" id="hle" role="tabpanel" aria-labelledby="config-hle-tab">
                            <asp:UpdatePanel ID="UpdatePanelHle" runat="server">
                                <ContentTemplate>
                                    <div id="hleParametersContainer" runat="server">
                                        <asp:Repeater ID="rptHleParameters" runat="server">
                                            <HeaderTemplate>
                                                <table class="table table-bordered">
                                                    <thead>
                                                        <tr>
                                                            <th>Habilitar configuración</th>
                                                            <th>Pre-aprobación</th>
                                                            <th>Responsable de proceso</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <!-- Habilitar configuración -->
                                                    <td>
                                                        <asp:HiddenField ID="hdfParameterId" runat="server" Value='<%# Eval("PeriodParameterDivisionCurrencyId") %>' />
                                                        <asp:CheckBox ID="chkEnableConfig" runat="server" Checked='<%# Eval("IsEnabled") %>' />
                                                        <%# Eval("ParameterName") %>
                                                    </td>
                                                    <!-- Pre-aprobación -->
                                                    <td>
                                                        <asp:CheckBox ID="chkPreApproval" runat="server" Checked='<%# Eval("RequiresPreApproval") %>' />
                                                        Requiere preaprobación
                                                    </td>
                                                    <!-- Responsable de proceso -->
                                                    <td>
                                                        <asp:TextBox ID="txtResponsible" runat="server" Text='<%# Eval("ProcessResponsible") %>' CssClass="form-control control-validation cleanPasteText ignoreValidation" autocomplete="off" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </tbody>
                        </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>

                        <!-- Tab Pane para DUI -->
                        <div class="tab-pane fade" id="dui" role="tabpanel" aria-labelledby="config-dui-tab">
                            <asp:UpdatePanel ID="UpdatePanelDui" runat="server">
                                <ContentTemplate>
                                    <div id="duiParametersContainer" runat="server">
                                        <asp:Repeater ID="rptDuiParameters" runat="server">
                                            <HeaderTemplate>
                                                <table class="table table-bordered">
                                                    <thead>
                                                        <tr>
                                                            <th>Habilitar configuración</th>
                                                            <th>Pre-aprobación</th>
                                                            <th>Responsable de proceso</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <!-- Habilitar configuración -->
                                                    <td>
                                                        <asp:HiddenField ID="hdfParameterId" runat="server" Value='<%# Eval("PeriodParameterDivisionCurrencyId") %>' />
                                                        <asp:CheckBox ID="chkEnableConfig" runat="server" Checked='<%# Eval("IsEnabled") %>' />
                                                        <%# Eval("ParameterName") %>
                                                    </td>
                                                    <!-- Pre-aprobación -->
                                                    <td>
                                                        <asp:CheckBox ID="chkPreApproval" runat="server" Checked='<%# Eval("RequiresPreApproval") %>' />
                                                        Requiere preaprobación
                                                    </td>
                                                    <!-- Responsable de proceso -->
                                                    <td>
                                                        <asp:TextBox ID="txtResponsible" runat="server" Text='<%# Eval("ProcessResponsible") %>' CssClass="form-control control-validation cleanPasteText ignoreValidation" autocomplete="off" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </tbody>
                        </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>

                        <!-- Tab Pane para QKE -->
                        <div class="tab-pane fade" id="qke" role="tabpanel" aria-labelledby="config-qke-tab">
                            <asp:UpdatePanel ID="UpdatePanelQke" runat="server">
                                <ContentTemplate>
                                    <div id="qkeParametersContainer" runat="server">
                                        <asp:Repeater ID="rptQkeParameters" runat="server">
                                            <HeaderTemplate>
                                                <table class="table table-bordered">
                                                    <thead>
                                                        <tr>
                                                            <th>Habilitar configuración</th>
                                                            <th>Pre-aprobación</th>
                                                            <th>Responsable de proceso</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <!-- Habilitar configuración -->
                                                    <td>
                                                        <asp:HiddenField ID="hdfParameterId" runat="server" Value='<%# Eval("PeriodParameterDivisionCurrencyId") %>' />
                                                        <asp:CheckBox ID="chkEnableConfig" runat="server" Checked='<%# Eval("IsEnabled") %>' />
                                                        <%# Eval("ParameterName") %>
                                                    </td>
                                                    <!-- Pre-aprobación -->
                                                    <td>
                                                        <asp:CheckBox ID="chkPreApproval" runat="server" Checked='<%# Eval("RequiresPreApproval") %>' />
                                                        Requiere preaprobación
                                                    </td>
                                                    <!-- Responsable de proceso -->
                                                    <td>
                                                        <asp:TextBox ID="txtResponsible" runat="server" Text='<%# Eval("ProcessResponsible") %>' CssClass="form-control control-validation cleanPasteText ignoreValidation" autocomplete="off" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </tbody>
                        </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>

                        <!-- Tab Pane para SCT -->
                        <div class="tab-pane fade" id="sct" role="tabpanel" aria-labelledby="config-sct-tab">
                            <asp:UpdatePanel ID="UpdatePanelSct" runat="server">
                                <ContentTemplate>
                                    <div id="sctParametersContainer" runat="server">
                                        <asp:Repeater ID="rptSctParameters" runat="server">
                                            <HeaderTemplate>
                                                <table class="table table-bordered">
                                                    <thead>
                                                        <tr>
                                                            <th>Habilitar configuración</th>
                                                            <th>Pre-aprobación</th>
                                                            <th>Responsable de proceso</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <!-- Habilitar configuración -->
                                                    <td>
                                                        <asp:HiddenField ID="hdfParameterId" runat="server" Value='<%# Eval("PeriodParameterDivisionCurrencyId") %>' />
                                                        <asp:CheckBox ID="chkEnableConfig" runat="server" Checked='<%# Eval("IsEnabled") %>' />
                                                        <%# Eval("ParameterName") %>
                                                    </td>
                                                    <!-- Pre-aprobación -->
                                                    <td>
                                                        <asp:CheckBox ID="chkPreApproval" runat="server" Checked='<%# Eval("RequiresPreApproval") %>' />
                                                        Requiere preaprobación
                                                    </td>
                                                    <!-- Responsable de proceso -->
                                                    <td>
                                                        <asp:TextBox ID="txtResponsible" runat="server" Text='<%# Eval("ProcessResponsible") %>' CssClass="form-control control-validation cleanPasteText ignoreValidation" autocomplete="off" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </tbody>
                        </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <%--<button id="btnRunReview" type="button" runat="server" class="btn btn-primary btnAjaxAction"
                            onserverclick="btnRunReview_Click" onclick="return ProcessRunReviewRequest(this.id);"
                            data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnReview"))%>'
                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnReview"))%>'>
                            <span class="glyphicon glyphicon-ok glyphicon-main-button" aria-hidden="true"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnReview")) %>
                        </button>--%>
                        <button id="btnSaveDraft" type="button" runat="server" class="btn btn-primary btnAjaxAction"
                            onserverclick="btnSaveDraft_ServerClick" onclick="return ProcessSaveDraftRequest(this.id);"
                            data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>'
                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>'>
                            <span class="glyphicon glyphicon-time glyphicon-main-button" aria-hidden="true"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnSaveDraft")) %>
                        </button>

                        <button id="btnCancelConfig" type="button" class="btn btn-default">
                            <span class="glyphicon glyphicon-remove glyphicon-main-button" aria-hidden="true"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnDialogCancel")) %>
                        </button>
                    </div>
                </div>
            </div>
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
                            <asp:HiddenField ID="hdfGtiPeriodId" runat="server" Value="" />
                            <asp:HiddenField ID="hdfGtiPeriodIdExisted" runat="server" Value="" />
                            <asp:HiddenField ID="hdfQuarterYearSelectedExisted" runat="server" Value="" />
                            <div class="form-horizontal">

                                <div class="form-group">
                                    <div class="col-sm-10 text-left">
                                        <label for="<%= chkExistPeriod.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblExistingPeriod")%></label>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:CheckBox ID="chkExistPeriod" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                    </div>
                                </div>
                                <div class="form-group MasterPeriodList">
                                    <div class="col-sm-6 text-left">
                                        <label for="<%=cboGtiPeriodExisted.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblGtiPeriodExisted")%></label>
                                    </div>
                                    <div class="col-sm-6">
                                        <asp:DropDownList ID="cboGtiPeriodExisted" CssClass="form-control cboAjaxAction control-validation selectpicker" AutoPostBack="true" runat="server" OnSelectedIndexChanged="cboGtiPeriodExisted_SelectedIndexChanged" data-live-search="true" data-actions-box="true"></asp:DropDownList>
                                    </div>
                                </div>
                                <br />


                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtName.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblNameGtiPeriod")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtName" CssClass="form-control control-validation cleanPasteText " runat="server" onkeypress="blockEnterKey(); return isNumberOrLetter(event);" MaxLength="254" autocomplete="off" type="text"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= cboPeriodState.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblPeriodState")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="cboPeriodState" CssClass="form-control control-validation" AutoPostBack="false" runat="server" data-id="Status" data-value="isPermitted"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= cboQuarterYear.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblYear")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="cboQuarterYear" CssClass="form-control control-validation" AutoPostBack="false" runat="server" data-id="Status" data-value="isPermitted"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= dtpStartDate.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblDateIni")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:TextBox runat="server" ID="dtpStartDate" ReadOnly="true" class="form-control date control-validation cleanPasteDigits" placeholder='<%$ Code:GetLocalResourceObject("lblDateFormatPlaceHolder") %>' type="text" autocomplete="off" />
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= cboQuarterId.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblQuarterID")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="cboQuarterId" OnSelectedIndexChanged="cboQuarterId_SelectedIndexChanged" CssClass="form-control control-validation" AutoPostBack="true" runat="server" data-id="Status" data-value="isPermitted"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= dtpFinDate.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblDateFin")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:TextBox runat="server" ID="dtpFinDate" ReadOnly="true" class="form-control date control-validation cleanPasteDigits" placeholder='<%$ Code:GetLocalResourceObject("lblDateFormatPlaceHolder") %>' type="text" autocomplete="off" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%=ToDateEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblMaxDate")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <input type="text" id="ToDateEdit" runat="server" class="enterkey ToDateEdit dateinput form-control date control-validation cleanPasteDigits limpiarCampos" onkeypress="return isNumberOrLetter(event);" onkeyup="return isNumberOrLetter(event);" maxlength="500" value="" required />

                                        <label id="ToDateEditValidation" for="<%= ToDateEdit.ClientID%>"
                                            class="label label-danger label-validation" data-toggle="tooltip"
                                            data-container="body" data-placement="left"
                                            data-content="<%= GetLocalResourceObject("msjDateValidation") %>"
                                            style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">
                                            !</label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button id="btnAccept" type="button" runat="server" class="btn btn-primary btnAjaxAction"
                                onserverclick="btnAccept_ServerClick" onclick="return ProcessAcceptRequest(this.id);"
                                data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>'
                                data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>'>
                                <span class="glyphicon glyphicon-ok glyphicon-main-button" aria-hidden="true"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnDialogSave")) %>
                            </button>

                            <button id="btnCancel" type="button" class="btn btn-default">
                                <span class="glyphicon glyphicon-remove glyphicon-main-button" aria-hidden="true"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnDialogCancel")) %>
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

                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="form-horizontal">
                                <div id="divDuplicatedDialogText" runat="server"></div>

                                <asp:Panel ID="pnlDuplicatedDialogDataDetail" runat="server">
                                    <div class="form-group">
                                        <div class="col-sm-4 text-left">
                                            <label for="<%= txtDuplicatedPeriodId.ClientID%>" class="control-label"><%=GetLocalResourceObject("PeriodCampaignId.HeaderText")%></label>
                                        </div>

                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtDuplicatedPeriodId" CssClass="form-control cleanPasteText ignoreValidation" runat="server" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-sm-4 text-left">
                                            <label for="<%= txtDuplicatedPeriodDescription.ClientID%>" class="control-label"><%=GetLocalResourceObject("PeriodCampaignDescription.HeaderText")%></label>
                                        </div>

                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtDuplicatedPeriodDescription" CssClass="form-control cleanPasteText ignoreValidation" runat="server" ReadOnly="true"></asp:TextBox>
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

    </div>




    <nav class="navbar-fixed-bottom">
        <div class="container center-block text-center">
            <b>
                <div class="alert alert-autocloseable-msg" style="display: none;"></div>
            </b>
        </div>
    </nav>
    <script src="../Scripts/custom-scripts.js"></script>

    <script type="text/javascript">
        //*******************************//
        //          VARIABLES            // 
        //*******************************//
        var dataSortAttribute, dataSortType, dataSortDirection;
        var validator = null;


        $(document).ready(function () {

        });


        //*******************************//
        //       EVENT BINDING           //
        //*******************************//
        function pageLoad(sender, args) {

            //In this section we initialize the checkbox toogles
            $('#<%= chkExistPeriod.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });

            //////////////////////////////////////////////////////////////////////////////////////////////////

            //In this section we set the grvList functionality
            $('#<%= grvList.ClientID %>').on('click', 'tbody tr', function (event) {
                if (!$(this).hasClass('emptyRow')) {
                    $(this).toggleClass('info').siblings().removeClass('info');
                    $('#<%=hdfSelectedRowIndex.ClientID%>').val($(this).hasClass("info") ? $(this).index() : -1);
                }
            });

            //In this section we set the others controls functionality 
            $('#<%=chkExistPeriod.ClientID%>').change(function () {
                ExistingGtiPeriodChange();
            });

            //others buttons 
            $('#<%= btnAdd.ClientID %>').on('click', function (ev) {
                /// <summary>Handles the click event for button add.</summary>
                ev.preventDefault();
                setTimeout(function () {
                    $("#<%=btnAdd.ClientID%>").button('reset');
                }, 500);
                DisableToolBar();
                ClearModalForm();
                $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceAdd") %>');
                $('#dialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceAdd") %>');
                $('#MaintenanceDialog').modal('show');
                EnableToolBar();

                __doPostBack('<%= btnAdd.UniqueID %>', '');
            });

            $('#btnCancel, #btnClose').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                event.preventDefault();
                ClearModalForm();
                DisableButtonsDialog();
                $('#MaintenanceDialog').modal('hide');
                EnableButtonsDialog();
            });

            $('#btnCancelReport, #btnReportClose').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                event.preventDefault();
                ClearModalFormReport();
                DisableButtonsDialog();
                $('#ReportDialog').modal('hide');
                EnableButtonsDialog();
            });

            $('#btnCancelConfig, #btnConfigClose').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                event.preventDefault();
                ClearModalFormReport();
                DisableButtonsDialog();
                $('#ConfigDialog').modal('hide');
                EnableButtonsDialog();
            });

            $('#btnDuplicatedAccept, #btnDuplicatedClose').on('click', function (event) {
                /// <summary>Handles the click event for button accept in user dialog.</summary>            
                event.preventDefault();
                $('#MaintenanceDialog').modal('show');
                $('#DuplicatedDialog').modal('hide');

            });

            $('#<%= ToDateEdit.ClientID %>').on("blur", function () {
                var ValidDateVal = $(this).data("DateTimePicker").date().format('MM/DD/YYYY');
                $(this).val(ValidDateVal);
            });


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

            $('.dateinput').datetimepicker({
                format: 'MM/DD/YYYY',
                //minDate: new Date(),
                maxDate: new Date("9999-12-31")
            });

            ExistingGtiPeriodChange();
            SetRowSelected();


        }

        //*******************************//
        //          VALIDATION           // 
        //*******************************//
        function ValidateForm() {
            // Agregar método de validación personalizado para la comparación de fechas
            $.validator.addMethod("greaterThanFinDate", function (value, element) {
                // Obtener los valores de las fechas desde los controles
                var finDateStr = $('#<%= dtpFinDate.ClientID %>').val();
                var toDateStr = value;

                // Si alguno de los campos está vacío, no validar aquí (se manejará con 'required')
                if (!toDateStr || !finDateStr) {
                    return true;
                }

                // Convertir las cadenas de fecha a objetos Date
                var toDate = new Date(toDateStr);
                var finDate = new Date(finDateStr);

                // Verificar si las fechas son válidas
                if (isNaN(toDate.getTime()) || isNaN(finDate.getTime())) {
                    return false;
                }

                // Retornar true si toDate es mayor que finDate
                return toDate > finDate;
            }, "<%= GetLocalResourceObject("msgDateValidation") %>"); // Mensaje de error personalizado

            // Agregar método de validación existente para 'validSelection' si no se ha agregado previamente
            $.validator.addMethod("validSelection", function (value, element) {
                return this.optional(element) || value != "-1";
            }, "Please select a valid option");

            // Destruir y recrear el validador para evitar duplicados
            if (validator != null) {
                validator.destroy();
            }

            // Inicializar el validador
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
            "<%= txtName.UniqueID %>": {
                        required: true,
                        normalizer: function (value) {
                            return $.trim(value);
                        },
                        minlength: 1,
                        maxlength: 254
                    },
            "<%= cboPeriodState.UniqueID %>": {
                        required: true,
                        validSelection: true
                    },
            "<%= cboQuarterId.UniqueID %>": {
                        required: true,
                        validSelection: true
                    },
            "<%= cboQuarterYear.UniqueID %>": {
                        required: true,
                        validSelection: true
                    },
            "<%= ToDateEdit.UniqueID %>": { // Agregar validación para ToDateEdit
                        required: true,
                        date: true, // Asegura que el valor sea una fecha válida
                        greaterThanFinDate: true // Validación personalizada
                    }
                },
                messages: {
            "<%= ToDateEdit.UniqueID %>": {
                greaterThanFinDate: "<%= GetLocalResourceObject("msgDateValidation") %>"
                    }
                }
            });

            var result = validator.form();
            return result;
        }

        function ValidateFormExisting() {
            $.validator.addMethod("validSelection", function (value, element) {
                return this.optional(element) || value != "-1";
            }, "Please select a valid option");

            $.validator.addMethod("existing", function (value, element) {
                var option = $("#<%=chkExistPeriod.ClientID%>").is(":checked");

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
                    "<%= cboGtiPeriodExisted.UniqueID%>": {
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
            /// <summary>Clear the modal form</summary>                       

            $("#<%=hdfGtiPeriodId.ClientID%>").val("-1");
            $("#<%=hdfGtiPeriodIdExisted.ClientID%>").val("-1");

            $("#<%=chkExistPeriod.ClientID%>").bootstrapToggle('off');

            $("#<%=txtName.ClientID%>").val("");
            $("#<%=cboPeriodState.ClientID%>").val("-1");

            $("#<%=cboQuarterYear.ClientID%>").val("-1");
            $("#<%=cboQuarterYear.ClientID%>").removeAttr("disabled");

            $("#<%=cboQuarterId.ClientID%>").val("-1");

            $("#<%=dtpStartDate.ClientID%>").val("");

            $("#<%=dtpFinDate.ClientID%>").val("");

            $("#<%=ToDateEdit.ClientID%>").val("");
        }

        function ClearModalFormReport() {
            /// <summary>Clear the modal form</summary>                       
            /// <summary>Clear the modal form</summary>                       


            if (validator != null) {
                validator.resetForm();
            }
        }

        function ExistingGtiPeriodChange() {
            if ($("#<%=chkExistPeriod.ClientID%>").is(":checked")) {
                $(".MasterPeriodList").show();
            } else {
                $(".MasterPeriodList").hide();
            }
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

        function ProcessSaveDraftRequest(resetId) {
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
                __doPostBack('<%= btnSaveDraft.UniqueID %>', '');
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

        function ProcessReviewRequest(resetId) {
            /// <summary>Process the review request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            if (IsRowSelected()) {
                ShowConfirmationMessageReview(resetId);
                return false;
            }

            else {
                ShowFooterError('<%= GetLocalResourceObject("msgInvalidSelection") %>');
                ErrorButton(resetId);
                return false;
            }
        }

        function ProcessSummaryRequest(resetId) {
            /// <summary>Process the review request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            if (IsRowSelected()) {
                ShowConfirmationMessageSummary(resetId);
                return false;
            }

            else {
                ShowFooterError('<%= GetLocalResourceObject("msgInvalidSelection") %>');
                ErrorButton(resetId);
                return false;
            }
        }

        <%--function ProcessReportsRequest(resetId) { ojo revisar
            /// <summary>Process the create report request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            if (IsRowSelected()) {
                __doPostBack('<%= BtnGtiReports.UniqueID %>', '');
                return true;
            }

            else {
                ShowFooterError('<%= GetLocalResourceObject("msgInvalidSelection") %>');
                ErrorButton(resetId);
                return false;
            }
        }--%>

        function ProcessConfigRequest(resetId) {
            /// <summary>Process the create report request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            if (IsRowSelected()) {
                __doPostBack('<%= btnGtiConfig.UniqueID %>', '');
                return true;
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

        function ReturnFromBtnReportsClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            SetRowSelected();
            DisableToolBar();

            //ClearModalFormReport();
            //$('#ReportDialog').find('.modal-body').empty();

            $('#ReportDialog').modal('show');

            EnableToolBar();
        }

        function ReturnFromBtnConfigClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            SetRowSelected();
            DisableToolBar();

            //ClearModalFormReport();
            //$('#ReportDialog').find('.modal-body').empty();

            $('#ConfigDialog').modal('show');
            $('#ConfigTab a[href="#gti"]').tab('show');

            EnableToolBar();
        }

        function ReturnPostBackAcceptClickSave() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            DisableButtonsDialog();
            SetRowSelected();
            ClearModalForm();

            $('#MaintenanceDialog').modal('hide');

            ShowFooterSuccess('<%= GetLocalResourceObject("msgOperationSuccesfull") %>');
            EnableButtonsDialog();
        }

        function ReturnRequestBtnEditOpen() {
            SetRowSelected();
            DisableToolBar();

            $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#dialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');

            $('#MaintenanceDialog').modal('show');

            $('#<%= chkExistPeriod.ClientID %>').prop('disabled', true);
            $(".MasterPeriodList").hide();

            $('#<%= cboGtiPeriodExisted.ClientID %>').prop('disabled', true);
            $('#<%= txtName.ClientID %>').prop('disabled', true);

            var toDateEditId = '<%= ToDateEdit.ClientID %>'; // Obtener el ClientID del control
            //$('#' + toDateEditId).datetimepicker('destroy'); // Destruir el datetimepicker
            $('#' + toDateEditId).prop('disabled', true);

            EnableToolBar();
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

        function ReturnFromBtnReviewClickPostBack() {
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
            '<%= GetLocalResourceObject("msjDelete") %>'
                , '<%=GetLocalResourceObject("Yes")%>'
                , function () {

                    __doPostBack('<%= btnDelete.UniqueID %>', '');
                }
                , '<%=GetLocalResourceObject("No")%>'
                , function () {
                    $("#" + resetId).button('reset');
                }
            );
            return false;
        }

        <%--function ShowConfirmationMessageReview(resetId) {
            /// <summary>Show confirmation message for Save and Close funtionality</summary>
            /// <param name="resetId" type="String">Id of the button to reset if cancel</param>
            /// <returns>False</returns>

            MostrarConfirmacion(
            '<%= GetLocalResourceObject("msjReview") %>'
                , '<%=GetLocalResourceObject("Yes")%>'
                , function () {

                    __doPostBack('<%= btnReview.UniqueID %>', '');
                }
                , '<%=GetLocalResourceObject("No")%>'
                , function () {
                    $("#" + resetId).button('reset');
                }
            );
            return false;
        }--%>

        function ShowConfirmationMessageSummary(resetId) {
            /// <summary>Show confirmation message for Save and Close funtionality</summary>
            /// <param name="resetId" type="String">Id of the button to reset if cancel</param>
            /// <returns>False</returns>

            MostrarConfirmacion(
            '<%= GetLocalResourceObject("msjSummary") %>'
                , '<%=GetLocalResourceObject("Yes")%>'
                , function () {

                    __doPostBack('<%= btnSummary.UniqueID %>', '');
                }
                , '<%=GetLocalResourceObject("No")%>'
                , function () {
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
        prm.add_initializeRequest(endingRequest);
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
