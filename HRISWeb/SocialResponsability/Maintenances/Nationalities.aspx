<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Nationalities.aspx.cs" Inherits="HRISWeb.SocialResponsability.Maintenances.Nationalities" %>

<asp:Content ID="Content" ContentPlaceHolderID="cntHeader" runat="server">
    <title><%=Convert.ToString(GetLocalResourceObject("lblScreenTitle")) %></title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cntBody" runat="server">
    <style>
        .bootstrap-select > .dropdown-toggle.bs-placeholder, .bootstrap-select > .dropdown-toggle.bs-placeholder:active,
        .bootstrap-select > .dropdown-toggle.bs-placeholder:focus, .bootstrap-select > .dropdown-toggle.bs-placeholder:hover {
            color: #333;
        }
    </style>
    <div class="main-content">
        <asp:Panel ID="pnlMainContent" runat="server" DefaultButton="btnSearchDefault">
            <h1 class="text-left text-primary">
                <label class="PageTitle"><%= GetLocalResourceObject("lblScreenTitle") %></label>
            </h1>
            <br />
            <asp:UpdatePanel runat="server" ID="main">
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
                                                <label for="<%= txtAplhaCode.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblAlphaCode")%></label>
                                            </div>

                                            <div class="col-sm-7">
                                                <asp:TextBox ID="txtAplhaCode" CssClass="form-control control-validation cleanPasteText ignoreValidation EnterKeypress" runat="server" onkeypress="blockEnterKey(); return isNumberOrLetter(event)" MaxLength="50" autocomplete="off" type="text"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group col-sm-12 col-md-6">
                                            <div class="col-sm-5 text-left">
                                                <label for="<%= txtDescriptionFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("DescriptionLbl")%></label>
                                            </div>

                                            <div class="col-sm-7">
                                                <asp:TextBox ID="txtDescriptionFilter" CssClass="form-control control-validation cleanPasteText ignoreValidation EnterKeypress" runat="server" onkeypress="blockEnterKey(); return isNumberOrLetter(event)" MaxLength="50" autocomplete="off" type="text"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12 text-center">
                                <button id="btnSearch" type="button" runat="server" class="btn btn-default btnAjaxAction"
                                    onserverclick="btnSearch_ServerClick"
                                    data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>'
                                    data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>'>
                                    <span class="glyphicon glyphicon-search glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%= GetLocalResourceObject("btnSearch") %>
                                </button>
                                <asp:Button ID="btnSearchDefault" runat="server" OnClick="btnSearch_ServerClick" Style="display: none;" />
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
                                        DataKeyNames="Alpha3Code"
                                        OnPreRender="grvList_PreRender" OnSorting="grvList_Sorting">
                                        <Columns>
                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="Alpha3CodeSort" runat="server" CommandName="Sort" CommandArgument="Alpha3Code" OnClientClick="SetWaitingGrvList(true);">
                                                            <span><%= GetLocalResourceObject("lblAlphaCode") %></span>&nbsp;
                                                            <i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "Alpha3Code") %> sorterDirection' aria-hidden="true" style="float: right; margin-right: -6px; margin-top: 4px; position: relative; z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvAlpha3Code" data-id="Alpha3Code" data-value="<%# Eval("Alpha3Code") %>"><%# Eval("Alpha3Code") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="NationalityLabelSort" runat="server" CommandName="Sort" CommandArgument="NationalityLabel" OnClientClick="SetWaitingGrvList(true);">
                                                            <span><%= GetLocalResourceObject("DescriptionLbl") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "NationalityLabel") %> sorterDirection' aria-hidden="true" style="float: right; margin-right: -6px; margin-top: 4px; position: relative; z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvNationalityLabel" data-id="NationalityLabel" aria-hidden="true" data-value="<%# Eval("NationalityName") %>"><%# Eval("NationalityName") %></span>
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
                                    <asp:BulletedList ID="blstPager" runat="server" CssClass="pagination" DisplayMode="LinkButton" OnClick="blstPager_Click">
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
                        <button id="btnAddProvince" type="button" runat="server" class="btn btn-default btnAjaxAction"
                             onserverclick="BtnAddProvince_ServerClick" onclick="return ProcessAddProvinceRequest(this.id);"
                            data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnAddProvince"))%>'
                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnAddProvince"))%>'>
                            <span class="glyphicon glyphicon-plus glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnAddProvince") %>
                        </button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <%--  Modal for Add and Edit Provinces--%>
    <div class="modal fade" id="MaintenanceDialog" tabindex="-1" role="dialog" aria-labelledby="MaintenanceDialogTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog  " role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" id="btnClose" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title text-primary" id="dialogTitle"></h3>
                </div>
                <asp:UpdatePanel runat="server" ID="uppDialogControl">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="form-horizontal">
                                <asp:HiddenField ID="hdfPoliticalDivisionID" runat="server" Value="" /> 
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">                                        
                                       <label for="<%= txtNationality.ClientID%>" class="control-label "><%=GetLocalResourceObject("lblNationality")%></label></div>
                                    <div class="col-sm-8 text-left">
                                         <asp:TextBox ID="txtNationality" CssClass="form-control cleanPasteText" runat="server"  TextMode="SingleLine" ReadOnly="true" ></asp:TextBox>                                      
                                    </div>                                    
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">                                        
                                       <label for="<%= txtProvince.ClientID%>" class="control-label  "><%=GetLocalResourceObject("lblProvince")%></label></div>
                                    <div class="col-sm-8 text-left">
                                         <asp:TextBox ID="txtProvince" CssClass="form-control cleanPasteText" runat="server"  TextMode="SingleLine" onkeypress="blockEnterKey(); return isNumberOrLetter(event);" MaxLength="150"></asp:TextBox>                                      
                                    </div>                                    
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">                                        
                                       <label for="<%= txtOrderList.ClientID%>" class="control-label "><%=GetLocalResourceObject("lblOrderList")%></label></div>
                                    <div class="col-sm-8 text-left">
                                         <asp:TextBox ID="txtOrderList" CssClass="form-control cleanPasteDigits" required="true" runat="server" MaxLength="3"  TextMode="SingleLine" onkeypress="return isNumber(event);" ></asp:TextBox>                                      
                                    </div>                                    
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= chkSearchEnabled.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblSearchEnabled")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:CheckBox ID="chkSearchEnabled" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                    </div>
                                </div>
                                <%--Controles de botones para agregar provincias--%>
                                 <div class="modal-footer">
                                    <button id="btnSaveProvince" type="button" runat="server" class="btn btn-primary btnAjaxAction"
                                        onserverclick="btnAccept_ServerClick" onclick="return ProcessSaveProvinceRequest(this.id);"
                                        data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSaveProvince"))%>'
                                        data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSaveProvince"))%>'>
                                        <span class="glyphicon glyphicon-save glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnSaveProvince")) %>
                                    </button>

                                    <button id="btnClean" type="button" class="btn btn-default" onclick="CleanModalProvince();">
                                        <span class="glyphicon glyphicon-erase glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnCleanProvince")) %>
                                    </button>
                                     <div style="visibility:hidden">
                                        <button id="btnLoadProvince" type="button" runat="server" class="btn btn-default" onserverclick="BtnLoadProvince_ServerClick">
                                           <span class="glyphicon glyphicon-erase glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnCleanProvince")) %>
                                        </button>
                                     </div>
                                </div>

                                <div class="form-group" >
                                    <div class="col-sm-12">
                                        <asp:GridView ID="grvProvince" Width="100%" runat="server" EmptyDataText='<%$ Code:GetLocalResourceObject("lblNoData") %>' 
                                                EmptyDataRowStyle-CssClass="emptyRow" AllowPaging="false" PagerSettings-Visible="false" AllowSorting="false"
                                                AutoGenerateColumns="false" ShowHeader="true" CssClass="table table-striped table-hover table-bordered"
                                                DataKeyNames="PoliticalDivisionID" OnPreRender="GrvProvince_PreRender" >                             
                                                <Columns>                                        
                                                     <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle hidden" HeaderStyle-CssClass="hidden" FooterStyle-CssClass="hidden" >
                                                        <HeaderTemplate>
                                                            <div style="width:100%; text-align:center;">
                                                                <asp:LinkButton ID="PoliticalDivisionIDSort" runat="server" CommandName="Sort" CommandArgument="PoliticalDivisionID" OnClientClick="SetWaitingGrvList(true);">          
                                                                    <span><%= GetLocalResourceObject("PoliticalDivisionID.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "PoliticalDivisionID") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <div style="width:100%; text-align:center;">
                                                                <span id="dvPoliticalDivisionID" data-id="PoliticalDivisionID" data-value="<%# Eval("PoliticalDivisionID") %>"><%# Eval("PoliticalDivisionID") %></span>
                                                             </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField> 
                                                    <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" >
                                                        <HeaderTemplate>
                                                            <div style="width:100%; text-align:center;">
                                                                <asp:LinkButton ID="PoliticalDivisionNameSort" runat="server" CommandName="Sort" CommandArgument="PoliticalDivisionName" OnClientClick="SetWaitingGrvList(true);">          
                                                                    <span><%= GetLocalResourceObject("PoliticalDivisionName.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "PoliticalDivisionName") %> sorterDirection' style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <div style="width:100%; text-align:center;">
                                                                <span id="dvPoliticalDivisionName" data-id="PoliticalDivisionName" data-value="<%# Eval("PoliticalDivisionName") %>"><%# Eval("PoliticalDivisionName") %></span>
                                                             </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField> 
                                                    <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" >
                                                        <HeaderTemplate>
                                                            <div style="width:100%; text-align:center;">
                                                                <asp:LinkButton ID="PoliticalDivisionCodeSort" runat="server" CommandName="Sort" CommandArgument="PoliticalDivisionCode" OnClientClick="SetWaitingGrvList(true);">          
                                                                    <span><%= GetLocalResourceObject("PoliticalDivisionCode.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "PoliticalDivisionCode") %> sorterDirection' style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <div style="width:100%; text-align:center;">
                                                                <span id="dvPoliticalDivisionCode" data-id="PoliticalDivisionCode" data-value="<%# Eval("PoliticalDivisionCode") %>"><%# Eval("PoliticalDivisionCode") %></span>
                                                             </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField> 
                                                     <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle hidden" HeaderStyle-CssClass="hidden" FooterStyle-CssClass="hidden">
                                                        <HeaderTemplate>
                                                            <div style="width:100%; text-align:center;">
                                                                <asp:LinkButton ID="SearchEnabledSort" runat="server" CommandName="Sort" CommandArgument="SearchEnabled" OnClientClick="SetWaitingGrvList(true);">          
                                                                    <span><%= GetLocalResourceObject("SearchEnabled.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "SearchEnabled") %> sorterDirection' style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span id="dvSearchEnabled" data-id="SearchEnabled" data-value="<%# Eval("SearchEnabled") %>"><%# Eval("SearchEnabled") %></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                        </asp:GridView>           
                                        <div id="grvProvinceWaiting" style="position: absolute; margin: auto; display: none; bottom: 0px; top: 0px; left: 50%;transform: translate(-50%, 35%);">
                                            <span class='fa fa-spinner fa-spin' style="font-size:50px;"></span>
                                        </div> 
                                    </div>
                                </div>                               
                            </div>
                        </div>

                        <div class="modal-footer">
                            <button id="btnAccept" type="button" runat="server" class="btn btn-primary btnAjaxAction"
                                onserverclick="btnAccept_ServerClick" onclick="return ProcessAcceptRequest(this.id);"
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

                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="form-horizontal">
                                <div id="divDuplicatedDialogText" runat="server"></div>

                                <asp:Panel ID="pnlDuplicatedDialogDataDetail" runat="server">
                                    <div class="form-group">
                                        <div class="col-sm-4 text-left">
                                            <label for="<%= txtDuplicatedProvinceId.ClientID%>" class="control-label"><%=GetLocalResourceObject("PoliticalDivisionID.HeaderText")%></label>
                                        </div>

                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtDuplicatedProvinceId" CssClass="form-control cleanPasteText ignoreValidation" runat="server" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-sm-4 text-left">
                                            <label for="<%= txtDuplicatedProvinceDescription.ClientID%>" class="control-label"><%=GetLocalResourceObject("DescriptionLbl")%></label>
                                        </div>

                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtDuplicatedProvinceDescription" CssClass="form-control cleanPasteText ignoreValidation" runat="server" ReadOnly="true"></asp:TextBox>
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
                <div class="alert alert-autocloseable-msg" style="display: none;">
                </div>
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
        $(document).ready(function () {
        });

        function pageLoad(sender, args) {
            //////////////////////////////////////////////////////////////////////////////////////////////////
            //In this section we initialize the checkbox toogles
            $('#<%= chkSearchEnabled.ClientID %>').bootstrapToggle({
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

            //And the grvYearCoursing selection row functionality
            $('#<%= grvProvince.ClientID %>').on('click', 'tbody tr', function (event) {
                if (!$(this).hasClass('emptyRow')) {
                    $(this).toggleClass('info').siblings().removeClass('info');
                    $('#<%=hdfPoliticalDivisionID.ClientID%>').val($(this).hasClass("info") ? $(this).index() : "");
                    //LoadInfoProvince();
                   __doPostBack('<%= btnLoadProvince.UniqueID %>', '');

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

            $('#<%= txtDescriptionFilter.ClientID %>').on('keyup keypress', function (e) {
                ReturnFromBtnSearchClickPostBack(e);

                return isNumberOrLetter(e);
            });

            $(".EnterKeypress").on('keyup keypress', function (e) {
                var keyCode = e.keyCode || e.which;
                if (keyCode === 13) {
                    e.preventDefault();
                    return false;
                }
            });

            $(document).on('keyup keypress', '.enterkey', function (e) {
                var keyCode = e.keyCode || e.which;
                if (keyCode === 13) {
                    e.preventDefault();
                    return false;
                }
            });

            $(".trim").change(function () {
                var text = $.trim($(this).val());
                $(this).val(text);
            });
            //////////////////////////////////////////////////////////////////////////////////////////////////

            //In this section we set the clean paste manager functionality
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
                    "<%= txtProvince.UniqueID %>": {
                                required: true,
                                normalizer: function(value) {
                                    return $.trim(value);
                                },
                                minlength: 1,
                                maxlength: 150
                            },
                    "<%= txtOrderList.UniqueID %>": {
                    required: true
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
            disableButton($('#<%= btnAddProvince.ClientID %>'));
        }

        function EnableToolBar() {
            ///<summary>Enabled all buttons toolbar</summary>
            enableButton($('#<%= btnAddProvince.ClientID %>'));
        }

        function CleanModalProvince() {
            ///<summary>Clean Modal Province</summary>
            $('#<%= txtProvince.ClientID %>').val("");
            $('#<%= txtOrderList.ClientID %>').val("");
            $('#<%= hdfPoliticalDivisionID.ClientID %>').val("-1");
            $('#<%=chkSearchEnabled.ClientID%>').bootstrapToggle('off');
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
            $(".emptyinput").val("");
            if (validator != null) {
                validator.resetForm();
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

        function ProcessAddProvinceRequest(resetId) {
            /// <summary>Process the edit request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param> 
            $(".btns").prop("disabled", true);
            if (IsRowSelected()) {
                __doPostBack('<%= btnAddProvince.UniqueID %>', '');
                return true;
            }
            else {
               ShowFooterError('<%= GetLocalResourceObject("msgInvalidSelection") %>');
               ErrorButton(resetId);
               $(".btns").prop("disabled", false);
               return false;
           }
        }

        function LoadInfoProvince() {
            var politicalDivisionID = $("#<%= hdfPoliticalDivisionID.ClientID %>").val().trim();

            if (politicalDivisionID != "-1") {
                var provinceList = JSON.parse(localStorage.getItem('ProvinceList'));
                var provinceObject = provinceList.filter(z => z.PoliticalDivisionID == politicalDivisionID );
                $("#<%= txtProvince.ClientID %>").val(provinceObject.PoliticalDivisionName);
                $("#<%= txtOrderList.ClientID %>").val(provinceObject.PoliticalDivisionCode);
                if (provinceObject.SearchEnabled) {
                    $('#<%=chkSearchEnabled.ClientID%>').bootstrapToggle('on');
                } else {
                    $('#<%=chkSearchEnabled.ClientID%>').bootstrapToggle('off');
                }
            }
        }

        //*******************************//
        //           PROCESS             //
        //*******************************//

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

        function ProcessSaveProvinceRequest(resetId) {
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
                __doPostBack('<%= btnSaveProvince.UniqueID %>', '');
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
        function ReturnPostBackAcceptClickSave() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            DisableButtonsDialog();
            SetRowSelected();
            ClearModalForm();

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
        
        function ReturnFromBtnAddProvinceClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $(".btns").prop("disabled", true);
            SetRowSelected();
            DisableToolBar();
            $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("btnAddProvince") %>');
            $('#dialogTitle').html('<%= GetLocalResourceObject("btnAddProvince") %>');
            $('#MaintenanceDialog').modal('show');
            EnableToolBar();
            $(".btns").prop("disabled", false);
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
