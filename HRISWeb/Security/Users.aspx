<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="HRISWeb.Security.Users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntHeader" runat="server">
    <title><%=Convert.ToString(GetLocalResourceObject("lblScreenTitle")) %></title>
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
                    <h4 class="text-left text-primary"><%= GetLocalResourceObject("lblFiltersSubtitle") %></h4>
                    <br />

                    <div class="row">
                        <div class="col-sm-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-5 text-left">
                                        <label for="<%= txtUserNameFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblUserName")%></label>
                                    </div>

                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtUserNameFilter" CssClass="form-control control-validation cleanPasteText" onkeypress="blockEnterKey(); return isNumberOrLetter(event)" runat="server" autocomplete="off" type="text"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-sm-12 text-center">
                            <button id="btnSearch" type="button" runat="server" class="btn btn-default btnAjaxAction"
                                onserverclick="BtnSearch_ServerClick"
                                data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>'
                                data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>'>
                                <span class="glyphicon glyphicon-search glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                                &nbsp;<%= GetLocalResourceObject("btnSearch") %>
                            </button>
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
                                    AllowPaging="true" PagerSettings-Visible="false" AllowSorting="true"
                                    AutoGenerateColumns="false" ShowHeader="true"
                                    CssClass="table table-striped table-hover table-bordered" DataKeyNames="UserCode"
                                    OnPreRender="GrvList_PreRender" OnSorting="GrvList_Sorting">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" Visible="false">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="lbtnUserCodeSort" runat="server" CommandName="Sort" CommandArgument="UserCode">                
                                                        <span>UserCode</span><i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "UserCode") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvUserCode" data-id="UserCode" data-value="<%# Eval("UserCode") %>"><%# Eval("UserCode") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="lbtnActiveDirectoryUserAccountSort" runat="server" CommandName="Sort" CommandArgument="ActiveDirectoryUserAccount" OnClientClick="SetWaitingGrvList(true);">          
                                                        <span><%= GetLocalResourceObject("ActiveDirectoryUserAccount.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "ActiveDirectoryUserAccount") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvActiveDirectoryUserAccount" data-id="ActiveDirectoryUserAccount" data-value="<%# Eval("ActiveDirectoryUserAccount") %>"><%# Eval("ActiveDirectoryUserAccount") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="lbtnUserNameSort" runat="server" CommandName="Sort" CommandArgument="UserName" OnClientClick="SetWaitingGrvList(true);">          
                                                        <span><%= GetLocalResourceObject("UserName.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "UserName") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvUserName" data-id="UserName" data-value="<%# Eval("UserName") %>"><%# Eval("UserName") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="lbtnEmailAddressSort" runat="server" CommandName="Sort" CommandArgument="EmailAddress" OnClientClick="SetWaitingGrvList(true);">          
                                                        <span><%= GetLocalResourceObject("EmailAddress.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "EmailAddress") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvEmailAddress" data-id="EmailAddress" data-value="<%# Eval("EmailAddress") %>"><%# Eval("EmailAddress") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="ltbnIsActiveSort" runat="server" CommandName="Sort" CommandArgument="IsActive" OnClientClick="SetWaitingGrvList(true);">      
                                                        <span><%= GetLocalResourceObject("IsActive.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "IsActive") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvIsActive" data-id="IsActive" aria-hidden="true" data-value="<%# Eval("IsActive") %>" class="fa <%# ((DOLE.HRIS.Shared.Entity.UserEntity)Container.DataItem).IsActive ? "fa-check-circle" : "fa-times-circle" %> "></span>
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
            <asp:UpdatePanel ID="uppActions" runat="server">
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
                            onserverclick="BtnEdit_ServerClick" onclick="return ProcessEditRequest(this.id);"
                            data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnEdit"))%>'
                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnEdit"))%>'>
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

                        <button id="btnDeleteH" type="button" style="display: none;" runat="server" onserverclick="BtnDelete_ServerClick"></button>
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
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title text-primary" id="dialogTitle"></h3>
                </div>

                <asp:UpdatePanel runat="server" ID="uppDialogControl">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnEdit" EventName="ServerClick" />
                        <asp:AsyncPostBackTrigger ControlID="btnAcceptAddNewDivision" EventName="ServerClick" />
                        <asp:AsyncPostBackTrigger ControlID="btnDeleteDivision" EventName="ServerClick" />
                    </Triggers>

                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="form-horizontal">
                                <asp:HiddenField ID="hdfUserCodeEdit" runat="server" Value="" />
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtActiveDirectoryUserAccount.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblActiveDirectoryUserAccount")%></label>
                                    </div>

                                    <div class="col-sm-5">
                                        <asp:TextBox ID="txtActiveDirectoryUserAccount" CssClass="form-control cleanPasteText EnterKey" runat="server" MaxLength="30" onkeypress="return isNumberOrLetter(event)" disabled="disabled"></asp:TextBox>
                                        <label id="txtActiveDirectoryUserAccountValidation" for="<%= txtActiveDirectoryUserAccount.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjActiveDirectoryUserAccountValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                    </div>

                                    <div class="col-sm-3">
                                        <button id="btnSearchUser" type="button" runat="server" class="btn btn-primary" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>'>
                                            <span class="glyphicon glyphicon-search glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnSearch")) %>
                                        </button>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtUserName.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblUserName")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtUserName" Enabled="false" CssClass="form-control cleanPasteText" runat="server" MaxLength="50" onkeypress="blockEnterKey();return isNumberOrLetter(event)" TextMode="SingleLine"></asp:TextBox>
                                        <label id="txtUserNameValidation" for="<%= txtUserName.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjUserNameValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtEmailAddress.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblEmailAddress")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtEmailAddress" CssClass="form-control cleanPasteText" runat="server" MaxLength="100" onkeypress="blockEnterKey();return isNumberOrLetter(event)" TextMode="SingleLine"></asp:TextBox>
                                        <label id="txtEmailAddressValidation" for="<%= txtEmailAddress.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjEmailAddressValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= chkIsActive.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblIsActive")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:CheckBox ID="chkIsActive" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                    </div>
                                </div>
                            </div>
                            <hr />

                            <h4 class="text-left text-secondary">&nbsp;<%=Convert.ToString(GetLocalResourceObject("lblUserAssignedDivisions")) %></h4>

                            <div class="scrolling-table-container" style="height: 250px!important;">
                                <input type="hidden" name="SelectedUserCodeGet" id="SelectedUserCodeGet" runat="server" value="" />

                                <asp:GridView ID="grvDetails"
                                    Width="100%"
                                    runat="server"
                                    EmptyDataText='<%$ Code:GetLocalResourceObject("lblNoData") %>'
                                    AllowPaging="false" AllowSorting="true"
                                    AutoGenerateColumns="False" ShowHeader="true"
                                    CssClass="table table-bordered table-striped table-hover"
                                    DataKeyNames="DivisionCode"
                                    OnPreRender="GrvDetails_PreRender" OnSorting="GrvList_SortingDivision"
                                    ShowHeaderWhenEmpty="false">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-CssClass="elem-checkbox" ItemStyle-Width="5%">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <span>&nbsp;</span>
                                                </div>
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelectedDetail" runat="server"></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" Visible="false">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="lbtnDivisionCodeSort" runat="server" OnClientClick="return false;">                
                                                        <span>DivisionCode</span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <span id="dvDivisionCode" data-id="DivisionCode" data-value="<%# Eval("DivisionCode") %>"><%# Eval("DivisionCode") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" SortExpression="DivisionName">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="lbtnDivisionNameSort" OnClientClick="SetWaitingGrvList(true);" CommandName="Sort" CommandArgument="DivisionName" runat="server">          
                                                        <span><%= GetLocalResourceObject("DivisionName.HeaderText") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <span id="dvDivisionName" data-id="DivisionName" data-value="<%# Eval("DivisionName") %>"><%# Eval("DivisionName") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>

                            <div>
                                </br>
                                <button id="btnOpenDialodAddDivision" type="button" runat="server" class="btn btn-primary btnAjaxAction"
                                    onserverclick="BtnOpenDialodAddDivision_ServerClick" onclick="return ProcessOpenDialodAddDivisionRequest(this.id);"
                                    data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnOpenDialodAddDivision"))%>'
                                    data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnOpenDialodAddDivision"))%>'>
                                    <span class="glyphicon glyphicon-plus glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnOpenDialodAddDivision")) %>
                                </button>

                                <button id="btnDeleteDivision" type="button" runat="server" class="btn btn-primary btnAjaxAction "
                                    onserverclick="BtnDeleteDivision_ServerClick" onclick="return ProcessDeleteRequest(this.id);"
                                    data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDelete"))%>'
                                    data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDelete"))%>'>
                                    <span class="glyphicon glyphicon-trash glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnDelete")) %>
                                </button>
                            </div>
                        </div>

                        <div class="modal-footer">
                            <button id="btnAccept" type="button" runat="server" class="btn btn-primary btnAjaxAction btnAccept btnformdisabled"
                                onserverclick="BtnAccept_ServerClick" ondblclick="return ProcessAcceptRequest(this.id);" onclick="return ProcessAcceptRequest(this.id);"
                                data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>'
                                data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>'>
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

    <!-- Modal for new detail item-->
    <div class="modal fade" id="adddetailmodal" tabindex="-1" role="dialog" aria-labelledby="adddetailmodaltitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title text-primary" id="adddetailmodaltitle"><%= GetLocalResourceObject("lblDialogAddNewDivisionTitle") %></h4>
                </div>

                <asp:UpdatePanel ID="upnAvailableDetails" runat="server">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnOpenDialodAddDivision" EventName="ServerClick" />
                    </Triggers>

                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="scrolling-table-container-horizontal">
                                <asp:GridView ID="grvAvailableDetails"
                                    Width="100%"
                                    runat="server"
                                    EmptyDataText='<%$ Code:GetLocalResourceObject("lblNoData") %>'
                                    AllowPaging="false"
                                    CssClass="table table-bordered table-striped table-hover"
                                    ShowHeader="true"
                                    DataKeyNames="DivisionCode,DivisionName,CountryID"
                                    OnPreRender="GrvAvailableDetails_PreRender" AutoGenerateColumns="False"
                                    ShowHeaderWhenEmpty="false">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-CssClass="elem-checkbox" ItemStyle-Width="5%">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <span>&nbsp;</span>
                                                </div>
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelectedNewDetail" runat="server"></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" Visible="false">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="lbtnDivisionCodeAvailableSort" runat="server" OnClientClick="return false;">                
                                                        <span>DivisionCode</span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvDivisionAvailableCode" data-id="DivisionCode" data-value="<%# Eval("DivisionCode") %>"><%# Eval("DivisionCode") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="lbtnDivisionNameAvailableSort" runat="server" OnClientClick="return false;">          
                                                        <span><%= GetLocalResourceObject("DivisionName.HeaderText") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <span id="dvDivisionNameAvailable" data-id="DivisionName" data-value="<%# Eval("DivisionName") %>"><%# Eval("DivisionName") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>

                        <div class="modal-footer">
                            <button id="btnAcceptAddNewDivision" runat="server" class="btn btn-primary btnAjaxAction"
                                onserverclick="BtnAcceptAddNewDivision_ServerClick" onclick="return ProcessAcceptAddNewDivisionRequest(this.id);"
                                data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>'
                                data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>'>
                                <span class="glyphicon glyphicon-ok glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnDialogSave")) %>
                            </button>

                            <button id="btnCancelAddDivision" class="btn btn-default">
                                <span class="glyphicon glyphicon-remove glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnDialogCancel")) %>
                            </button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <!-- Modal for search user AD-->
    <div class="modal fade" id="searchmodal" tabindex="-1" role="dialog" aria-labelledby="searchmodaltitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title text-primary" id="searchmodaltitle">&nbsp;<%= GetLocalResourceObject("lblModalSearchUserActiveDirectoryTitle") %></h4>
                </div>

                <asp:UpdatePanel ID="uppActiveDirectoryUsersSearch" runat="server">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSearchActiveDirectoryUser" EventName="ServerClick" />
                    </Triggers>

                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label for="<%= txtUserNameActiveDirectorySearch.ClientID %>" class="col-sm-4 col-control-label">&nbsp;<%= GetLocalResourceObject("lblUserName") %></label>
                                    <div class="col-sm-5">
                                        <asp:TextBox runat="server" ID="txtUserNameActiveDirectorySearch" CssClass="form-control control-validation cleanPasteText" onkeypress="blockEnterKey();return isNumberOrLetter(event)" MaxLength="50" TextMode="SingleLine"></asp:TextBox>
                                        <label id="txtUserNameActiveDirectorySearchValidation" for="<%= txtUserNameActiveDirectorySearch.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjUserNameValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                    </div>

                                    <div class="col-sm-3">
                                        <button id="btnSearchActiveDirectoryUser" type="button" runat="server" class="btn btn-default btnAjaxAction"
                                            onclick="return ProcessSearchActiveDirectoryUserRequest(this.id);" onserverclick="BtnSearchActiveDirectoryUser_ServerClick"
                                            data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>'
                                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>'>
                                            <span class="glyphicon glyphicon-search glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                                            &nbsp;<%= GetLocalResourceObject("btnSearch") %>
                                        </button>
                                    </div>
                                </div>
                            </div>
                            <hr />

                            <div class="scrolling-table-container-vertical">
                                <asp:GridView ID="grvActiveDirectoryUsers"
                                    Width="100%"
                                    runat="server"
                                    EmptyDataText='<%$ Code:GetLocalResourceObject("lblNoData") %>'
                                    AllowPaging="false" AllowSorting="true"
                                    AutoGenerateColumns="False" ShowHeader="true"
                                    DataKeyNames="CompleteUserCode"
                                    CssClass="table table-bordered table-striped table-hover"
                                    OnPreRender="GrvActiveDirectoryUsers_PreRender"
                                    OnSorting="GrvActiveDirectoryUsersSort"
                                    ShowHeaderWhenEmpty="True">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-CssClass="elem-checkbox">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelectedSearchUser" runat="server"></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" SortExpression="CompleteUserCode">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="lbtnActiveDirectoryUserAccountSearch" runat="server" CommandName="Sort" CommandArgument="CompleteUserCode">          
                                                        <span><%= GetLocalResourceObject("ActiveDirectoryUserAccount.HeaderText") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvActiveDirectoryUserAccountSearch" data-id="ActiveDirectoryUserAccountSearch" data-value="<%# Eval("CompleteUserCode") %>"><%# Eval("CompleteUserCode") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" SortExpression="UserFullName">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="lbtnUserNameSort" runat="server" CommandName="Sort" CommandArgument="UserFullName">          
                                                        <span><%= GetLocalResourceObject("UserName.HeaderText") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvUserNameSearch" data-id="UserNameSearch" data-value="<%# Eval("UserFullName") %>"><%# Eval("UserFullName") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" SortExpression="EmailAddress">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="lbtnEmailAddressSort" runat="server" CommandName="Sort" CommandArgument="EmailAddress">          
                                                        <span><%= GetLocalResourceObject("EmailAddress.HeaderText") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <span id="dvEmailAddressSearch" data-id="EmailAddressSearch" data-value="<%# Eval("EmailAddress") %>"><%# Eval("EmailAddress") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>

                        <div class="modal-footer">
                            <button id="btnAddUser" runat="server" class="btn btn-primary btnAjaxAction"
                                onclick="return ProcessBtnAddUserRequest(this.id);"
                                data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>'
                                data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>'>
                                <span class="glyphicon glyphicon-ok glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnDialogSave")) %>
                            </button>

                            <button id="btnCancelSearchUser" class="btn btn-default">
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
            /// <summary>Execute at load even at partial and ajax requests</summary>
            /// We prefer to bind the events here over do it on document ready event in order to auto rebind the events in page and ajax request each time
            ReturnDeleteSucess = function () {
                UnselectRow();
                ShowFooterSuccess('<%= GetLocalResourceObject("msgOperationSuccesfull") %>');
            }

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

                    MostrarConfirmacion('<%= GetLocalResourceObject("msjDelete") %>', '<%=GetLocalResourceObject("Yes")%>', function () {
                        <%= ClientScript.GetPostBackEventReference(btnDeleteH, String.Empty) %>;
                    }, '<%=GetLocalResourceObject("No")%>', function () {
                        $($this).button('reset');
                    });
                } else {
                    ShowFooterError('<%= GetLocalResourceObject("msgInvalidSelection") %>');
                }
            });

            //In this section we set the button state for btnAjaxAction
            $('.btnAjaxAction').on('click', function () {
                var $this = $(this);
                $this.button('loading');
                setTimeout(function () { $this.button('reset'); }, 30000);
            });

            //And the grvList selection row functionality
            $('#<%= grvList.ClientID %>').on('click', 'tbody tr', function (event) {
                if (!$(this).hasClass('emptyRow')) {
                    $(this).toggleClass('info').siblings().removeClass('info');
                    $('#<%=hdfSelectedRowIndex.ClientID%>').val($(this).hasClass("info") ? $(this).index() : -1);
                }
            });

            //And the grvList pager event for client side
            $('#<%= blstPager.ClientID %>').on('click', 'a', function (event) {
                SetWaitingGrvList(true);
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

                $("#<%=hdfUserCodeEdit.ClientID%>").val("");
                $("#<%=txtActiveDirectoryUserAccount.ClientID%>").val("");
                $("#<%=txtUserName.ClientID%>").val("");
                $("#<%=txtEmailAddress.ClientID%>").val("");
                $("#<%=chkIsActive.ClientID%>").bootstrapToggle('on');
                $("#<%=grvDetails.ClientID%>").html('');

                if (validator != null) {
                    validator.resetForm();
                    validator.destroy();
                }

                $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceAdd") %>');
                $('#dialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceAdd") %>');
                $('#MaintenanceDialog').modal('show');

                enableButton($('#<%= btnSearchUser.ClientID %>'));
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

            $('#<%= btnSearchUser.ClientID %>').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                event.preventDefault();

                DisableButtonsDialog();

                $("#<%=txtUserNameActiveDirectorySearch.ClientID%>").val("");
                $("#<%=grvActiveDirectoryUsers.ClientID%>").html("");

                $('#MaintenanceDialog').modal('hide');
                $('#searchmodal').modal('show');

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
                    disableButton($("#<%=btnSearchUser.ClientID%>"));
                }
                else {
                    enableButton($("#<%=btnSearchUser.ClientID%>"));
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
            $('#<%= chkIsActive.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });

            //In this section we initialize the popovers and tooltips for the elements.
            //We have both (popovers and tooltips) in order to accomplish information alert over desktop 
            //and mobiles devices
            InitializeTooltipsPopovers();
        }

        var validator = null;

        function validateEmail(email) {
            var regex = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            return regex.test(email);
        }

        function ValidateForm() {
            jQuery.validator.addMethod("ValidateEmail", function (value, element) {
                return validateEmail(value);
            }, "<%= GetLocalResourceObject("msjEmailAddressValidation") %>");

            /// <summary>Validate the form with jquey validation plugin </summary>
            /// <returns> True if form is valid. False otherwise. </returns>
            $('#' + document.forms[0].id).validate().destroy();
            if (validator == null) {
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
                            '<%= txtActiveDirectoryUserAccount.UniqueID %>': {
                                required: true
                                , normalizer: function (value) {
                                    return $.trim(value);
                                }
                            },
                            '<%= txtUserName.UniqueID %>': {
                                required: true
                                , normalizer: function (value) {
                                    return $.trim(value);
                                }
                            },
                            '<%= txtEmailAddress.UniqueID %>': {
                                required: true, ValidateEmail: true
                                , email: true
                                , normalizer: function (value) {
                                    return $.trim(value);
                                }
                            }
                        }
                    });
            }

            else {
                validator.validate();
            }

            //get the results            
            var result = validator.form();
            return result;
        }

        var validatorSearchUser = null;
        function ValidateSearchUser() {
            /// <summary>Validate the form with jquey validation plugin </summary>
            /// <returns> True if form is valid. False otherwise. </returns>
            $('#' + document.forms[0].id).validate().destroy();

            if (validatorSearchUser == null) {
                //declare the validator
                var validatorSearchUser =
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
                            '<%= txtUserNameActiveDirectorySearch.UniqueID %>': {
                                required: true,
                                minlength: 1,
                                normalizer: function (value) {
                                    return $.trim(value);
                                }
                            }
                        }
                    });
            }

            else {
                validatorSearchUser.validate();
            }

            //get the results            
            var result = validatorSearchUser.form();
            return result;
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
            $("#<%=txtActiveDirectoryUserAccount.ClientID%>").prop('disabled', false);

            if (!ValidateForm()) {
                setTimeout(function () {
                    ResetButton(resetId);
                    enableButton($('#btnCancel'));
                    $(".btnAccept").prop("disabled", false);
                }, 150);
            }

            else {
                __doPostBack('<%= btnAccept.UniqueID %>', '');
            }

            $("#<%=txtActiveDirectoryUserAccount.ClientID%>").prop('disabled', true);

            return false;
        }

        function ProcessOpenDialodAddDivisionRequest(resetId) {
            /// <summary>Process the open dialog for add new division request</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            DisableButtonsDialog();
            enableButton($('#<%= btnAcceptAddNewDivision.ClientID %>'));
            enableButton($('#btnCancelAddDivision'));

            $('#MaintenanceDialog').modal('hide');
            $('#adddetailmodal').modal('show');

            disableButton($("#<%=btnSearchUser.ClientID%>"));
            EnableButtonsDialog();
            ResetButton(resetId);

            if ($('#MaintenanceDialog').data('windowmode') === 'edit') {
                disableButton($("#<%=btnSearchUser.ClientID%>"));
            }

            else {
                enableButton($("#<%=btnSearchUser.ClientID%>"));
            }

            __doPostBack('<%= btnOpenDialodAddDivision.UniqueID %>', '');
        }

        function ProcessSearchActiveDirectoryUserRequest(resetId) {
            /// <summary>Process the open dialog for add new division request</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            if (!ValidateSearchUser()) {
                ErrorButton(resetId);
                ResetButton(resetId);
            }

            else {
                __doPostBack('<%= btnSearchActiveDirectoryUser.UniqueID %>', '');
            }

            return false;
        }

        function ProcessBtnAddUserRequest(resetId) {
            /// <summary>Process the selected user search</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            if (!IsOnlyOneRowCheked($("#<%= grvActiveDirectoryUsers.ClientID%>"))) {
                ErrorButton(resetId);

                MostrarMensaje(TipoMensaje.VALIDACION, '<%= GetLocalResourceObject("msgInvalidSelection") %>', function () {
                    ResetButton(resetId);
                });
            }

            else {
                var checks = $('#<%= grvActiveDirectoryUsers.ClientID%>').find('input:checkbox[id*="chkSelected"]:checked:not(#chkSelectedAll)');
                // fill the fields with the info
                $('#<%= txtActiveDirectoryUserAccount.ClientID %>').val(checks.closest('tr').find('#dvActiveDirectoryUserAccountSearch').html());
                $('#<%= txtUserName.ClientID %>').val(checks.closest('tr').find('#dvUserNameSearch').html());
                $('#<%= txtEmailAddress.ClientID %>').val(checks.closest('tr').find('#dvEmailAddressSearch').html().trim().replace(/^\&nbsp+|\&nbsp+$/g, ''));

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
                disableButton($("#<%=btnSearchUser.ClientID%>"));
            }

            else {
                enableButton($("#<%=btnSearchUser.ClientID%>"));
            }

            if (IsRowCheked($("#<%= grvDetails.ClientID%>"))) {
                ShowConfirmationMessageDelete(resetId);
                return false;
            }

            else {
                MostrarMensaje(TipoMensaje.VALIDACION, '<%= GetLocalResourceObject("msgInvalidSelection") %>', null);
                ErrorButton(resetId);
                return false;
            }
        }

        function ProcessAcceptAddNewDivisionRequest(resetId) {
            /// <summary>Process the Add new division request according to the validation of row cheched</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            disableButton($('#<%= btnAcceptAddNewDivision.ClientID %>'));
            disableButton($('#btnCancelAddDivision'));

            if (IsRowCheked($("#<%= grvAvailableDetails.ClientID%>"))) {
                $('#adddetailmodal').modal('hide');

                __doPostBack('<%= btnAcceptAddNewDivision.UniqueID %>', '');
                return true;
            }

            else {
                MostrarMensaje(TipoMensaje.VALIDACION, '<%= GetLocalResourceObject("msgInvalidSelection") %>', null);

                ResetButton(resetId);
                ErrorButton(resetId);

                enableButton($('#<%= btnAcceptAddNewDivision.ClientID %>'));
                enableButton($('#btnCancelAddDivision'));
                return false;
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
            disableButton($('#<%= btnSearchUser.ClientID%>'));
            disableButton($('#<%= btnOpenDialodAddDivision.ClientID%>'));
            disableButton($('#<%= btnDeleteDivision.ClientID%>'));
        }

        function EnableButtonsDialog() {
            ///<summary>Enabled all buttons toolbar</summary>
            enableButton($('#btnAccept'));
            enableButton($('#btnCancel'));
            enableButton($('#<%= btnSearchUser.ClientID%>'));
            enableButton($('#<%= btnOpenDialodAddDivision.ClientID%>'));
            enableButton($('#<%= btnDeleteDivision.ClientID%>'));
        }

        function ReturnFromBtnEditClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>            
            DisableToolBar();

            $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#dialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#MaintenanceDialog').modal('show');

            disableButton($('#<%= btnSearchUser.ClientID %>'));

            EnableToolBar();
        }

        function ReturnFromBtnAcceptClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>      
            $(".btnAccept").prop("disabled", true);
            DisableButtonsDialog();

            $('#MaintenanceDialog').modal('hide');
            $(".btnAccept").prop("disabled", false);

            EnableButtonsDialog();
            ShowFooterSuccess('<%= GetLocalResourceObject("msgOperationSuccesfull") %>');
        }

        function ReturnFromBtnAcceptClickPostBackError() {
            /// <summary>Manage the events, ui and logic after the postback</summary>            
            $(".btnformdisabled").prop("disabled", false);
            $("#<%=btnAccept.ClientID%>").prop("disabled", false);
        }

        function ReturnFromAcceptAddNewDivisionRequest() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            DisableButtonsDialog();

            $('#adddetailmodal').modal('hide');
            $('#MaintenanceDialog').modal('show');

            EnableButtonsDialog();

            if ($('#MaintenanceDialog').data('windowmode') === 'edit') {
                disableButton($("#<%=btnSearchUser.ClientID%>"));
            }

            else {
                enableButton($("#<%=btnSearchUser.ClientID%>"));
            }
        }

        function ReturnFromBtnDeleteClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            if ($('#MaintenanceDialog').data('windowmode') === 'edit') {
                disableButton($("#<%=btnSearchUser.ClientID%>"));
            }

            else {
                enableButton($("#<%=btnSearchUser.ClientID%>"));
            }
        }

        function ReturnFromBtnDeleteFailClickPostBack(message) {
            MostrarMensaje(TipoMensaje.ERROR, message, null);
        }

        function ShowConfirmationMessageDelete(resetId) {
            /// <summary>Show confirmation message for Save and Close funtionality</summary>
            /// <param name="resetId" type="String">Id of the button to reset if cancel</param>
            /// <returns>False</returns>
            MostrarConfirmacion(
                '<%= GetLocalResourceObject("msjDelete") %>', '<%=GetLocalResourceObject("Yes")%>', function () {
                    if ($('#MaintenanceDialog').data('windowmode') === 'edit') {
                        disableButton($("#<%=btnSearchUser.ClientID%>"));
                    }

                    else {
                        enableButton($("#<%=btnSearchUser.ClientID%>"));
                    }

                    __doPostBack('<%= btnDeleteDivision.UniqueID %>', '');
            }, '<%=GetLocalResourceObject("No")%>', function () {
                $("#" + resetId).button('reset');
            }
            );

            return false;
        }

        function ProcessEnterKeySearchUser(event) {
            /// <summary>Process the enter key for the textbox txtUserNameActiveDirectorySearch</summary>
            if (event.keyCode === 10 || event.keyCode === 13) {
                $('#<%=btnSearchActiveDirectoryUser.ClientID%>').click();
            }
        }

        //In this section we set the control for multiple simultaneous ajax request
        //We cancel the ajax request executed when another one is in progress
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(initializeRequest);
        prm.add_endRequest(endingRequest);


        function initializeRequest(sender, args) {
            if (prm.get_isInAsyncPostBack()) {
                args.set_cancel(true);

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
