<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AttachedDocuments.aspx.cs" Inherits="HRISWeb.Absenteeism.AttachedDocuments" %>

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
                            <div class="col-sm-2">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-sm-3 text-left">
                                            <label for="<%= txtAttachedDocumentCodeFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblAttachedDocumentCode")%></label>
                                        </div>

                                        <div class="col-sm-9">
                                            <asp:TextBox ID="txtAttachedDocumentCodeFilter" CssClass="form-control control-validation cleanPasteText ignoreValidation" runat="server" onkeypress="return isNumberOrLetter(event)" MaxLength="50" autocomplete="off" type="text"></asp:TextBox>
                                            <asp:HiddenField ID="hdfAttachedDocumentCodeFilter" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-6">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-sm-2 text-left">
                                            <label for="<%= txtAttachedDocumentNameFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblAttachedDocumentName")%></label>
                                        </div>

                                        <div class="col-sm-10">
                                            <asp:TextBox ID="txtAttachedDocumentNameFilter" CssClass="form-control control-validation cleanPasteText ignoreValidation" runat="server" onkeypress="return isNumberOrLetter(event)" MaxLength="500" autocomplete="off" type="text"></asp:TextBox>
                                            <asp:HiddenField ID="hdfAttachedDocumentNameFilter" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-4">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=cboDocumentState.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblDocumentState")%></label>
                                        </div>

                                        <div class="col-sm-7">
                                            <asp:DropDownList ID="cboDocumentState" AutoPostBack="true" CssClass="form-control control-validation" data-live-search="true" runat="server" OnSelectedIndexChanged="CboDocumentState_OnSelectedIndexChanged"></asp:DropDownList>
                                            <asp:HiddenField runat="server" ID="hdfDocumentLoaded" Value="0" />
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
                                        DataKeyNames="DocumentCode,DocumentName,DocumentDescription "
                                        OnPreRender="GrvList_PreRender" OnSorting="GrvList_Sorting">
                                        <Columns>
                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="DocumentCodeSort" runat="server" CommandName="Sort" CommandArgument="DocumentCode" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("AttachedDocumentCode.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "DocumentCode") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvDocumentCode" data-id="DocumentCode" data-value="<%# Eval("DocumentCode") %>"><%# Eval("DocumentCode") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="DocumentNameSort" runat="server" CommandName="Sort" CommandArgument="DocumentName" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("AttachedDocumentName.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "DocumentName") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvDocumentName" data-id="DocumentName" data-value="<%# Eval("DocumentName") %>"><%# Eval("DocumentName") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="DocumentDescriptionSort" runat="server" CommandName="Sort" CommandArgument="DocumentDescription" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("DocumentDescription.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "DocumentDescription") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvDocumentDescription" data-id="DocumentDescription" data-value="<%# Eval("DocumentDescription") %>"><%# Eval("DocumentDescription") %></span>
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

                        <button id="btnActivate" style="display: none;" type="button" runat="server" class="btn btn-default btnAjaxAction"
                            onserverclick="BtnActivate_ServerClick" onclick="" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnActivate"))%>'
                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnActivate"))%>'>
                            <span class="glyphicon glyphicon-ok glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnActivate") %>
                        </button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
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
                                <asp:HiddenField ID="hdfActivateDeletedAttachedDocumentCode" runat="server" Value="" />
                                <asp:HiddenField ID="hdfActivateDeletedAttachedDocumentDivisionCode" runat="server" Value="" />

                                <%=Convert.ToString(GetLocalResourceObject("lblTextActivateDeletedDialog")) %>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtActivateDeletedAttachedDocumentCode.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblAttachedDocumentCode")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtActivateDeletedAttachedDocumentCode" CssClass="form-control cleanPasteText ignoreValidation" runat="server" onkeypress="return isNumberOrLetter(event)" MaxLength="50" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtActivateDeletedAttachedDocumentName.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblAttachedDocumentName")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtActivateDeletedAttachedDocumentName" CssClass="form-control cleanPasteText ignoreValidation" runat="server" onkeypress="return isNumberOrLetter(event) && checkMaxLength(this,event,500);" onkeyUp="return isNumberOrLetter(event) && checkMaxLength(this,event,500);" MaxLength="500" TextMode="MultiLine" Columns="3" ReadOnly="true"></asp:TextBox>
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
                                <asp:HiddenField ID="hdfAttachedDocumentCodeEdit" runat="server" Value="" />
                                <asp:HiddenField ID="hdfAttachedDocumentDivisionCodeEdit" runat="server" Value="" />

                                <div id="documentCodeDiv" class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label id="txtAttachedDocumentCodeText" for="<%= txtAttachedDocumentCode.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblAttachedDocumentCode")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtAttachedDocumentCode" CssClass="form-control cleanPasteText" runat="server" onkeypress="return isNumberOrLetter(event)" MaxLength="50"></asp:TextBox>
                                        <label id="txtAttachedDocumentCodeValidation" for="<%= txtAttachedDocumentCode.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjAttachedDocumentCodeValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtAttachedDocumentName.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblAttachedDocumentName")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtAttachedDocumentName" CssClass="form-control cleanPasteText" runat="server" onkeypress="return isNumberOrLetter(event) && checkMaxLength(this,event,500);" onkeyUp="return isNumberOrLetter(event) && checkMaxLength(this,event,500);" MaxLength="500" TextMode="MultiLine" Columns="3"></asp:TextBox>
                                        <label id="txtAttachedDocumentNameValidation" for="<%= txtAttachedDocumentName.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjAttachedDocumentNameValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtComments.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblComments")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtComments" CssClass="form-control cleanPasteText" runat="server" onkeypress="return isNumberOrLetter(event) && checkMaxLength(this,event,1000);" onkeyUp="return isNumberOrLetter(event) && checkMaxLength(this,event,1000);" MaxLength="1000" TextMode="MultiLine" Columns="3"></asp:TextBox>
                                        <label id="txtCommentsValidation" for="<%= txtComments.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjCommentsValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
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

                            <label class="control-label"><%=GetLocalResourceObject("lblDocumentsByDivision")%></label>

                            <div class="form-horizontal">
                                <asp:Repeater ID="rptDocumentsByDivision" runat="server">
                                    <HeaderTemplate>
                                        <table id="tableDocumentsByDivision" class="table table-hover table-striped">
                                            <thead>
                                                <tr>
                                                    <th>
                                                        <div>
                                                            <div class="col-xs-4 col-sm-4 text-primary sorter" style="cursor: pointer; white-space: nowrap; text-align: center" data-sort-attr="data-sort-id" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblDivisionEnabledHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                            <div class="col-xs-4 col-sm-4 text-primary sorter" style="cursor: pointer; white-space: nowrap; text-align: center" data-sort-attr="data-sort-name" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblDivisionNameHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                            <div class="col-xs-4 col-sm-4 text-primary sorter" style="cursor: pointer; white-space: nowrap; text-align: center" data-sort-attr="data-sort-division" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblDivisionCode") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                        </div>
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <tr>
                                            <td class="row-fluid">
                                                <div class="data-sort-src" data-sort-id='<%# Eval("Enabled") %>' data-sort-name='<%# Eval("Division") %>' data-sort-mapping='<%# Eval("DivisionName") %>'>
                                                    <div class="col-xs-4 col-sm-4" style="text-align: center;">
                                                        <span>
                                                            <asp:CheckBox ID="chbEnabled" runat="server" CssClass="<%# SetCssCheckButton( Container.ItemIndex)  %>" Checked='<%# Eval("Enabled") %>' />
                                                        </span>
                                                    </div>

                                                    <div class="col-xs-4 col-sm-4" style="text-align: center;">
                                                        <asp:HiddenField ID="hdfDivisionCode" runat="server" Value='<%# Eval("Division") %>' />
                                                        <asp:HiddenField ID="hdfDivisionName" runat="server" Value='<%# Eval("DivisionName") %>' />
                                                        <span>
                                                            <%# Eval("DivisionName") %>                                                            
                                                        </span>
                                                    </div>

                                                    <div class="col-xs-4 col-sm-4" style="text-align: center;">
                                                        <span>
                                                            <%# Eval("Division") %>                                                            
                                                        </span>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>

                        <div class="modal-footer">
                            <button id="btnAccept" type="button" runat="server" class="btn btn-primary btnAjaxAction"
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
                                <asp:HiddenField ID="hdfDuplicatedAttachedDocumentCode" runat="server" Value="" />

                                <div id="divDuplicatedDialogText" runat="server"></div>

                                <asp:Panel ID="pnlDuplicatedDialogDataDetail" runat="server">
                                    <div class="form-group">
                                        <div class="col-sm-4 text-left">
                                            <label for="<%= txtDuplicatedAttachedDocumentCode.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblAttachedDocumentCode")%></label>
                                        </div>

                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtDuplicatedAttachedDocumentCode" CssClass="form-control cleanPasteText ignoreValidation" runat="server" onkeypress="return isNumberOrLetter(event)" MaxLength="50" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-4 text-left">
                                            <label for="<%= txtDuplicatedAttachedDocumentName.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblAttachedDocumentName")%></label>
                                        </div>

                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtDuplicatedAttachedDocumentName" CssClass="form-control cleanPasteText ignoreValidation" runat="server" onkeypress="return isNumberOrLetter(event) && checkMaxLength(this,event,500);" onkeyUp="return isNumberOrLetter(event) && checkMaxLength(this,event,500);" MaxLength="500" TextMode="MultiLine" Columns="3" ReadOnly="true"></asp:TextBox>
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

                $('input:checkbox').removeAttr('checked');

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
                $("#<%=txtAttachedDocumentCode.ClientID%>").prop("disabled", false);
                $('#DuplicatedDialog').modal('hide');
            });
            //////////////////////////////////////////////////////////////////////////////////////////////////

            //In this section we set the others controls functionality 
            $('.labelMappingValidation').each(function () {
                var $this = $(this);
                $this.prop("for", $this.parent().find('input:first').attr('id'));
            });

            $('.enableDivisionMappingSpan > input').addClass("enableDivisionMapping");

            $('.enableDivisionMapping').each(function () {
                var $this = $(this);

                if ($this.is(":checked")) {
                    $this.closest('tr').find(':input').removeAttr('disabled');
                }

                else {
                    $this.closest('tr').find(':input').attr('disabled', 'disabled');
                }

                $this.prop('disabled', false);
            });

            $('.enableDivisionMapping').on('click', function () {
                var $this = $(this);
                if ($this.is(":checked")) {
                    $this.closest('tr').find(':input').removeAttr('disabled');
                }

                else {
                    $this.closest('tr').find(':input').attr('disabled', 'disabled');
                    $this.closest('tr').find(':input').each(function () {
                        SetControlValid($(this).attr('id'));
                    });
                }

                $this.prop('disabled', false);
            });

            $("#<%=txtComments.ClientID %>").change(function () {
                $(this).val($.trim($(this).val()))
            });
  
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
                return false;
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
        var validator = null;
        function ValidateForm() {
            /// <summary>Validate the form with jquey validation plugin </summary>
            /// <returns> True if form is valid. False otherwise. </returns>
            if (validator == null) {
                //add custom validation methods
                jQuery.validator.addMethod("validSelection", function (value, element) {
                    return this.optional(element) || value != "-1";
                }, "Please select a valid option");

                //declare the validator
                validator =
                    $('#' + document.forms[0].id).validate({
                        debug: true,
                        ignore: ".ignoreValidation, :hidden",
                        highlight: function (element, errorClass, validClass) {
                            SetControlInvalid($(element).attr('id'));
                        },
                        unhighlight: function (element, errorClass, validClass) {
                            SetControlValid($(element).attr('id'));
                        },
                        errorPlacement: function (error, element) { },
                        rules: {
                            "<%= txtAttachedDocumentCode.UniqueID %>": {
                                required: true,
                                normalizer: function (value) {
                                    return $.trim(value);
                                },
                                minlength: 1,
                                maxlength: 50
                            },
                            "<%= txtAttachedDocumentName.UniqueID %>": {
                                required: true,
                                normalizer: function (value) {
                                    return $.trim(value);
                                },
                                minlength: 1,
                                maxlength: 500
                            },
                            "<%= txtComments.UniqueID %>": {
                                required: false,
                                normalizer: function (value) {
                                    return $.trim(value);
                                },
                                minlength: 1,
                                maxlength: 1000
                            }
                        }
                    });

                $('.txtMapping').each(function () {
                    $(this).rules("add",
                        {
                            required: true,
                            normalizer: function (value) {
                                return $.trim(value);
                            },
                            minlength: 1,
                            maxlength: 50

                        });
                });

            }

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
            DisableToolBar();

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
            disableButton($('#btnAccept'));
            disableButton($('#btnCancel'));
        }

        function EnableButtonsDialog() {
            ///<summary>Enabled all buttons toolbar</summary>
            enableButton($('#btnAccept'));
            enableButton($('#btnCancel'));
        }

        function ClearModalForm() {
            /// <summary>Clear the modal form</summary>
            $("#<%=hdfAttachedDocumentCodeEdit.ClientID%>").val("");
            $("#<%=hdfAttachedDocumentDivisionCodeEdit.ClientID%>").val("");
            $("#<%=txtAttachedDocumentCode.ClientID%>").val("");
            $("#<%=txtAttachedDocumentName.ClientID%>").val("");
            $("#<%=txtComments.ClientID%>").val("");
            $("#<%=chbSearchEnabled.ClientID%>").bootstrapToggle('on');

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
        //             LOGIC             // 
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
            /// <summary>Process the accept request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            disableButton($('#btnCancel'));

            if (!ValidateForm()) {
                setTimeout(function () {
                    ResetButton(resetId);
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

        function ProcessActivateRequest(resetId) {
            /// <summary>Process the accept request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            __doPostBack('<%= btnActivate.UniqueID %>', '');

            return false;
        }

        //*******************************//
        //  RETURN FROM POSTBACKS LOGIC  // 
        //*******************************//
        function ReturnFromBtnEditClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            ReturnFromCboDocumentChangePostBack();
            SetRowSelected();
            DisableToolBar();

            $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#dialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#MaintenanceDialog').modal('show');
            $("#<%=txtAttachedDocumentCode.ClientID%>").attr("disabled", "disabled");

            EnableToolBar();
        }

        function ReturnFromBtnAcceptClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            SetRowSelected();
            DisableButtonsDialog();
            $('#MaintenanceDialog').modal('hide');
            EnableButtonsDialog();

            ShowFooterSuccess('<%= GetLocalResourceObject("msgOperationSuccesfull") %>');
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
            $('#ActivateDeletedDialog').modal('show');
            $('#MaintenanceDialog').modal('hide');
        }

        function ReturnFromBtnAcceptClickPostBackDuplicated() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $('#DuplicatedDialog').modal('show');
            $('#MaintenanceDialog').modal('hide');
        }

        function ReturnFromBtnDeleteClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            UnselectRow();
            ShowFooterSuccess('<%= GetLocalResourceObject("msgOperationSuccesfull") %>');
        }

        function ReturnFromCboDocumentChangePostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            var isInactive = document.getElementById('<%=hdfDocumentLoaded.ClientID%>').value;

            if (isInactive == 1) {
                document.getElementById('<%= btnDelete.ClientID %>').style.display = "none";
                document.getElementById('<%= btnActivate.ClientID %>').style.display = "block";
            } else {
                document.getElementById('<%= btnDelete.ClientID %>').style.display = "block";
                document.getElementById('<%= btnActivate.ClientID %>').style.display = "none";
            }
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
