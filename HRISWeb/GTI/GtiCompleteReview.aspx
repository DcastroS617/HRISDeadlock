<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="GtiCompleteReview.aspx.cs" Inherits="HRISWeb.GTI.GtiCompleteReview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntHeader" runat="server">
    <link href="../../Content/css/custom-tabs.css" rel="stylesheet" />
    <link href="../../Content/css/dynamicTable.css" rel="stylesheet" />
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
                                            <asp:DropDownList ID="cboQuarterYearFilter" Style="width: 60%;" CssClass="form-control control-validation" AutoPostBack="false" runat="server" data-id="Status" data-value="isPermitted"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-sm-2 text-left">
                                            <label for="<%= cboQuarterIDFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblQuarterID")%></label>
                                        </div>
                                        <div class="col-sm-3">
                                            <asp:DropDownList ID="cboQuarterIDFilter" Style="width: 60%;" CssClass="form-control control-validation" AutoPostBack="false" runat="server" data-id="Status" data-value="isPermitted"></asp:DropDownList>
                                        </div>

                                        <div class="col-sm-2 text-left">
                                            <label for="<%= cboPeriodStateFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblPeriodState")%></label>
                                        </div>
                                        <div class="col-sm-3">
                                            <asp:DropDownList ID="cboPeriodStateFilter" Style="width: 60%;" CssClass="form-control control-validation" AutoPostBack="false" runat="server" data-id="Status" data-value="isPermitted"></asp:DropDownList>
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
                                    <asp:GridView ID="grvList" Width="100%" runat="server" EmptyDataText='<%$ Code:GetLocalResourceObject("lblNoData") %>'
                                        EmptyDataRowStyle-CssClass="emptyRow" AllowPaging="false" PagerSettings-Visible="false" AllowSorting="true"
                                        AutoGenerateColumns="false" ShowHeader="true" CssClass="table table-striped table-hover table-bordered"
                                        DataKeyNames="PeriodCampaignId" OnPreRender="GrvList_PreRender" OnSorting="GrvList_Sorting" OnRowDataBound="GrvList_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="QuarterIDSort" runat="server" CommandName="Sort" CommandArgument="QuarterID" OnClientClick="SetWaitingGrvList(true);">
                                                            <span><%= GetLocalResourceObject("ParameterID.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "QuarterID") %> sorterDirection' aria-hidden="true" style="float: right; margin-right: -6px; margin-top: 4px; position: relative; z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <span id="dvQuarterID" data-id="QuarterID" data-value="<%# Eval("QuarterID") %>"><%# Eval("QuarterID")%></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="QuarterIDSort" runat="server" CommandName="Sort" CommandArgument="QuarterID" OnClientClick="SetWaitingGrvList(true);">
                                                            <span><%= GetLocalResourceObject("ParameterName.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "QuarterID") %> sorterDirection' aria-hidden="true" style="float: right; margin-right: -6px; margin-top: 4px; position: relative; z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <span id="dvQuarterID" data-id="QuarterID" data-value="<%# Eval("QuarterID") %>"><%# Eval("QuarterID")%></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="QuarterIDSort" runat="server" CommandName="Sort" CommandArgument="QuarterID" OnClientClick="SetWaitingGrvList(true);">
                                                            <span><%= GetLocalResourceObject("DivisionCode.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "QuarterID") %> sorterDirection' aria-hidden="true" style="float: right; margin-right: -6px; margin-top: 4px; position: relative; z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <span id="dvQuarterID" data-id="QuarterID" data-value="<%# Eval("QuarterID") %>"><%# Eval("QuarterID")%></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="QuarterYearSort" runat="server" CommandName="Sort" CommandArgument="QuarterYear" OnClientClick="SetWaitingGrvList(true);">
                                                            <span><%= GetLocalResourceObject("DivisionName.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "QuarterYear") %> sorterDirection' aria-hidden="true" style="float: right; margin-right: -6px; margin-top: 4px; position: relative; z-index: 2;"></i>
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
                                                            <span><%= GetLocalResourceObject("CurrencyCode.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "InitialDate") %> sorterDirection' aria-hidden="true" style="float: right; margin-right: -6px; margin-top: 4px; position: relative; z-index: 2;"></i>
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
                                                            <span><%= GetLocalResourceObject("CurrencyName.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "FinalDate") %> sorterDirection' aria-hidden="true" style="float: right; margin-right: -6px; margin-top: 4px; position: relative; z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <span id="dvFinalDate" data-id="FinalDate" data-value="<%# Eval("FinalDate") %>"><%# Eval("FinalDate")%></span>
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
        </asp:Panel>
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
                            <asp:HiddenField ID="hdfGtiPeriodId" runat="server" Value="" />
                            <asp:HiddenField ID="hdfGtiPeriodIdExisted" runat="server" Value="" />
                            <asp:HiddenField ID="hdfQuarterYearSelectedExisted" runat="server" Value="" />

                            <div class="form-group">
                                <div class="col-sm-4 text-left">
                                    <label for="<%= txtName.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblNameGtiPeriod")%></label>
                                </div>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtName" CssClass="form-control control-validation cleanPasteText" runat="server" onkeypress="blockEnterKey(); return isNumberOrLetter(event);" MaxLength="254" autocomplete="off" type="text"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group mb-3">
                                <br />
                            </div>
                            <div class="form-group">

                                <asp:GridView ID="grvPeriodParameters" Width="100%" runat="server" EmptyDataText='<%$ Code:GetLocalResourceObject("lblNoData") %>'
                                    EmptyDataRowStyle-CssClass="emptyRow" AllowPaging="true" PagerSettings-Visible="true" AllowSorting="true"
                                    AutoGenerateColumns="false" ShowHeader="true" CssClass="table table-striped table-hover table-bordered"
                                    DataKeyNames="PeriodCampaignId" OnPreRender="GrvParameterList_PreRender" OnSorting="GrvParameterList_Sorting" OnRowDataBound="GrvParameterList_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="QuarterIDSort" runat="server" CommandName="Sort" CommandArgument="QuarterID" OnClientClick="SetWaitingGrvList(true);">
                                    <span><%= GetLocalResourceObject("DivisionCode.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "QuarterID") %> sorterDirection' aria-hidden="true" style="float: right; margin-right: -6px; margin-top: 4px; position: relative; z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvQuarterID" data-id="QuarterID" data-value="<%# Eval("QuarterID") %>"><%# Eval("QuarterID")%></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="QuarterYearSort" runat="server" CommandName="Sort" CommandArgument="QuarterYear" OnClientClick="SetWaitingGrvList(true);">
                                    <span><%= GetLocalResourceObject("DivisionName.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "QuarterYear") %> sorterDirection' aria-hidden="true" style="float: right; margin-right: -6px; margin-top: 4px; position: relative; z-index: 2;"></i>
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
                                    <span><%= GetLocalResourceObject("CurrencyCode.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "InitialDate") %> sorterDirection' aria-hidden="true" style="float: right; margin-right: -6px; margin-top: 4px; position: relative; z-index: 2;"></i>
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
                                    <span><%= GetLocalResourceObject("CurrencyName.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "FinalDate") %> sorterDirection' aria-hidden="true" style="float: right; margin-right: -6px; margin-top: 4px; position: relative; z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvFinalDate" data-id="FinalDate" data-value="<%# Eval("FinalDate") %>"><%# Eval("FinalDate")%></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
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
                                            <label for="<%= txtDuplicatedPeriodId.ClientID%>" class="control-label"><%=GetLocalResourceObject("ParameterID.HeaderText")%></label>
                                        </div>

                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtDuplicatedPeriodId" CssClass="form-control cleanPasteText ignoreValidation" runat="server" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-sm-4 text-left">
                                            <label for="<%= txtDuplicatedPeriodDescription.ClientID%>" class="control-label"><%=GetLocalResourceObject("ParameterName.HeaderText")%></label>
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

        //*******************************//
        //       EVENT BINDING           //
        //*******************************//
        function pageLoad(sender, args) {


            //////////////////////////////////////////////////////////////////////////////////////////////////

            //In this section we set the grvList functionality
            $('#<%= grvList.ClientID %>').on('click', 'tbody tr', function (event) {
                if (!$(this).hasClass('emptyRow')) {
                    $(this).toggleClass('info').siblings().removeClass('info');
                    $('#<%=hdfSelectedRowIndex.ClientID%>').val($(this).hasClass("info") ? $(this).index() : -1);
                }
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

            ExistingGtiPeriodChange();
            SetRowSelected();
        }

        //*******************************//
        //          VALIDATION           // 
        //*******************************//
        function ValidateForm() {
            $.validator.addMethod("validSelection", function (value, element) {
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
            "<%= txtName.UniqueID %>": {
                        required: true,
                        normalizer: function (value) {
                            return $.trim(value);
                        },
                        minlength: 1,
                        maxlength: 254
                    }
                }
            });

            var result = validator.form();
            return result;
        }

        function ClearModalForm() {
        /// <summary>Clear the modal form</summary>                       


     <%--$("#<%=chkExistPeriod.ClientID%>").bootstrapToggle('off');
 $("#<%=cboPeriodState.ClientID%>").val("-1");
     $("#<%=cboQuarterYear.ClientID%>").val("-1");
     $("#<%=cboQuarterId.ClientID%>").val("-1");
     $("#<%=dtpStartDate.ClientID%>").val("");
     $("#<%=dtpFinDate.ClientID%>").val("");
 --%>

            $("#<%=txtName.ClientID%>").val("");

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

            $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#dialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#MaintenanceDialog').modal('show');

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


        <%--function ValidateFormExisting() {
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
        }--%>

        <%--function ExistingGtiPeriodChange() {
            if ($("#<%=chkExistPeriod.ClientID%>").is(":checked")) {
                $(".GtiPeriodList").show();
            } else {
                $(".GtiPeriodList").hide();
            }
        }--%>


    </script>
</asp:Content>
