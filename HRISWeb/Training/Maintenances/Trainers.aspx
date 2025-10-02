<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Trainers.aspx.cs" Inherits="HRISWeb.Training.Trainers" %>

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
                            <div class="col-sm-6">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%= txtTrainerCodeFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblTrainerCode")%></label>
                                        </div>

                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtTrainerCodeFilter" CssClass="form-control control-validation cleanPasteText ignoreValidation" runat="server" MaxLength="10" autocomplete="off" type="text" data-id="TrainerCode" data-value="isPermitted"></asp:TextBox>
                                            <asp:HiddenField ID="hdfTrainerCodeFilter" runat="server" />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%= txtTrainerNameFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblTrainerName")%></label>
                                        </div>

                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtTrainerNameFilter" CssClass="form-control control-validation cleanPasteText ignoreValidation" runat="server" MaxLength="500" autocomplete="off" type="text" data-id="TrainerName" data-value="isPermitted"></asp:TextBox>
                                            <asp:HiddenField ID="hdfTrainerNameFilter" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-6">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=cboTrainerTypeFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblTrainerType")%></label>
                                        </div>

                                        <div class="col-sm-7">
                                            <asp:DropDownList ID="cboTrainerTypeFilter" CssClass="form-control cboAjaxAction control-validation ignoreValidation" AutoPostBack="false" runat="server" data-id="TrainerType" data-value="isPermitted"></asp:DropDownList>
                                            <asp:HiddenField ID="hdfTrainerTypeValueFilter" runat="server" />
                                            <asp:HiddenField ID="hdfTrainerTypeTextFilter" runat="server" />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                        </div>

                                        <div class="col-sm-7">
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
                                        DataKeyNames="TrainerCode, TrainerType"
                                        OnPreRender="GrvList_PreRender" OnSorting="GrvList_Sorting">
                                        <Columns>
                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="TrainerCodeSort" runat="server" CommandName="Sort" CommandArgument="TrainerCode" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("TrainerCode.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "TrainerCode") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvTrainerCode" data-id="TrainerCode" data-value="<%# Eval("TrainerCode") %>"><%# Eval("TrainerCode") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="TrainerNameSort" runat="server" CommandName="Sort" CommandArgument="TrainerName" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("TrainerName.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "TrainerName") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvTrainerName" style="overflow-wrap: break-word; word-wrap: break-word; word-break: break-all;" data-id="TrainerName" data-value="<%# Eval("TrainerName") %>"><%# Eval("TrainerName") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="TrainerTypeSort" runat="server" CommandName="Sort" CommandArgument="TrainerType" OnClientClick="SetWaitingGrvList(true);">                
                                                            <span><%= GetLocalResourceObject("TrainerType.HeaderText") %></span><i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "TrainerType") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvTrainerType" data-id="TrainerType" data-value="<%# Eval("TrainerType") %>"><%# GetTrainerTypeLocalizatedDescription(Convert.ToString(Eval("TrainerType"))) %></span>
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
                        <button id="btnAdd" type="button" runat="server" class="btn btn-default btnAjaxAction btndisabled"
                            onserverclick="BtnAdd_ServerClick"
                            data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnAdd"))%>'
                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnAdd"))%>'>
                            <span class="glyphicon glyphicon-plus glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnAdd") %>
                        </button>

                        <button id="btnEdit" type="button" runat="server" class="btn btn-default btnAjaxAction btndisabled"
                            onserverclick="BtnEdit_ServerClick" onclick="return ProcessEditRequest(this.id);"
                            data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnEdit"))%>'
                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnEdit"))%>'>
                            <span class="glyphicon glyphicon-edit glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnEdit") %>
                        </button>

                        <button id="btnDelete" type="button" runat="server" class="btn btn-default btnAjaxAction btndisabled"
                            onserverclick="BtnDelete_ServerClick" onclick="return ProcessDeleteRequest(this.id);"
                            data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnDelete"))%>'
                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnDelete"))%>'>
                            <span class="glyphicon glyphicon-trash glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnDelete") %>
                        </button>

                        <button id="btnCoursesByTrainers" type="button" runat="server" class="btn btn-default btnAjaxAction"
                            onserverclick="BtnCoursesByTrainers_ServerClick" onclick="return ProcessCoursesByTrainersRequest(this.id);"
                            data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnCourses"))%>'
                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnCourses"))%>'>
                            <span class="glyphicon glyphicon-link glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnCourses") %>
                        </button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <%--  Modal for Add and Edit External Companies & External Persons --%>
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
                                <asp:HiddenField ID="hdfTrainerCodeEdit" runat="server" Value="" />
                                <asp:HiddenField ID="hdfTrainerTypeEdit" runat="server" Value="" />

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%=cboTrainerType.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblTrainerType")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="cboTrainerType" CssClass="form-control cboAjaxAction control-validation" AutoPostBack="true" runat="server" OnSelectedIndexChanged="CboTrainerType_SelectedIndexChanged"></asp:DropDownList>
                                        
                                        <label id="cboTrainerTypeValidation" for="<%= cboTrainerType.ClientID%>" 
                                            class="label label-danger label-validation" data-toggle="tooltip" 
                                            data-container="body" data-placement="left" 
                                            data-content="<%= GetLocalResourceObject("msjTrainerTypeValidation") %>" 
                                            style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                    </div>
                                </div>

                                <div id="MaintenanceExternalsPanel" style="display: none;">
                                    <div class="form-group">
                                        <div class="col-sm-4 text-left">
                                            <label for="<%= txtExternalsTrainerCode.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblTrainerCode")%></label>
                                        </div>

                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtExternalsTrainerCode" CssClass="form-control cleanPasteText enterkey" runat="server" onkeypress="return isNumberOrLetter(event)" MaxLength="10"></asp:TextBox>
                                            
                                            <label id="txtExternalsTrainerCodeValidation" for="<%= txtExternalsTrainerCode.ClientID%>" 
                                                class="label label-danger label-validation" data-toggle="tooltip" 
                                                data-container="body" data-placement="left" 
                                                data-content="<%= GetLocalResourceObject("msjTrainerCodeValidation") %>" 
                                                style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-4 text-left">
                                            <label for="<%= txtExternalsTrainerName.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblTrainerName")%></label>
                                        </div>

                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtExternalsTrainerName" CssClass="form-control cleanPasteText enterkey" runat="server" onkeypress="return isNumberOrLetter(event) && checkMaxLength(this,event,500);" onkeyUp="return isNumberOrLetter(event) && checkMaxLength(this,event,500);" MaxLength="500" TextMode="MultiLine" Columns="3"></asp:TextBox>
                                            
                                            <label id="txtExternalsTrainerNameValidation" for="<%= txtExternalsTrainerName.ClientID%>" 
                                                class="label label-danger label-validation" data-toggle="tooltip" 
                                                data-container="body" data-placement="left" 
                                                data-content="<%= GetLocalResourceObject("msjTrainerNameValidation") %>" 
                                                style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-4 text-left">
                                            <label for="<%= txtExternalsTrainerTelephone.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblTrainerTelephone")%></label>
                                        </div>

                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtExternalsTrainerTelephone" CssClass="form-control cleanPasteText enterkey" runat="server" onkeypress="return isNumberOrLetter(event)" MaxLength="20"></asp:TextBox>
                                            
                                            <label id="txtExternalsTrainerTelephoneValidation" for="<%= txtExternalsTrainerTelephone.ClientID%>" 
                                                class="label label-danger label-validation" data-toggle="tooltip" 
                                                data-container="body" data-placement="left" 
                                                data-content="<%= GetLocalResourceObject("msjTrainerTelephoneValidation") %>" 
                                                style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-4 text-left">
                                            <label for="<%= chbExternalsSearchEnabled.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblSearchEnabled")%></label>
                                        </div>

                                        <div class="col-sm-3">
                                            <asp:CheckBox ID="chbExternalsSearchEnabled" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                        </div>
                                    </div>
                                </div>

                                <div id="MaintenanceInternalEmployeePanel" style="display: none;">
                                    <asp:UpdatePanel runat="server" ID="uppSearchEmployees" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="txtSearchEmployees" />
                                            <asp:AsyncPostBackTrigger ControlID="btnSearchEmployees" />
                                        </Triggers>

                                        <ContentTemplate>
                                            <div class="form-group">
                                                <div class="col-sm-4 text-left">
                                                    <label for="<%=txtSearchEmployees.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblSearchEmployeesPlaceHolder")%></label>
                                                </div>

                                                <div class="col-sm-8">
                                                    <asp:TextBox ID="txtSearchEmployees" runat="server" CssClass="form-control cleanPasteText enterkey" onkeypress="return isNumberOrLetter(event)" placeholder='<%$ Code:GetLocalResourceObject("lblSearchEmployeesPlaceHolder") %>' autocomplete="off" Width="100%" OnTextChanged="TxtSearchEmployees_TextChanged"></asp:TextBox>
                                                    <asp:Button ID="btnSearchEmployees" runat="server" Text="Button" Style="display: none" OnClick="TxtSearchEmployees_TextChanged" />

                                                    <span id="txtSearchEmployeesWaiting" class='fa fa-spinner fa-spin' style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;"></span>
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <div class="col-sm-12 text-left">
                                                    <label id="lblSearchEmployeesResults" runat="server" class="control-label"></label>
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <div class="col-sm-12 text-left">
                                                    <asp:Repeater ID="rptEmployees" runat="server">
                                                        <HeaderTemplate>
                                                            <div style="width: 100%; max-height: 300px; overflow-y: scroll;">
                                                                <table id="tableSelectEmployee" class="table table-hover table-striped">
                                                                    <thead>
                                                                        <tr>
                                                                            <th>
                                                                                <div>
                                                                                    <div class="col-xs-3 col-sm-3 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-code" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblEmployeeCodeHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                                    <div class="col-xs-7 col-sm-7 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-name" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblEmployeeNameHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                                    <div class="col-xs-2 col-sm-2 text-primary">&nbsp;</div>
                                                                                </div>
                                                                            </th>
                                                                        </tr>
                                                                    </thead>
                                                                    <tbody id="tableBodySelectEmployee">
                                                        </HeaderTemplate>

                                                        <ItemTemplate>
                                                            <tr>
                                                                <td class="row-fluid">
                                                                    <asp:UpdatePanel runat="server" ID="uppEmployee" UpdateMode="Conditional">
                                                                        <Triggers></Triggers>

                                                                        <ContentTemplate>
                                                                            <div class="data-sort-src" data-sort-code='<%# Eval("EmployeeCode") %>' data-sort-name='<%# Eval("EmployeeName") %>'>
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

                                                                                <div class="col-xs-2 col-sm-2 text-center">
                                                                                    <button id="btnAddEmployee" type="button" runat="server" class="btn btn-default btnAjaxAction btnAddEmployee" data-loading-text="<span class='fa fa-spinner fa-spin '></span>" data-error-text="<span class='glyphicon glyphicon-exclamation-sign'></span>" onclick="SelectInternalEmployee(this.id); return false;">
                                                                                        <span class="glyphicon glyphicon-check glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                                                                                    </button>
                                                                                    
                                                                                    <asp:TextBox ID="hdfEmployeeCode" CssClass="EmployeeCode" runat="server" Text='<%#Eval("EmployeeCode") %>' Style="display: none;" />
                                                                                    <asp:TextBox ID="hdfEmployeeName" CssClass="EmployeeName" runat="server" Text='<%#Eval("EmployeeName") %>' Style="display: none;" />
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

                                            <div class="form-group">
                                                <div class="col-sm-4 text-left">
                                                    <label for="<%= txtInternalEmployeeTrainerCode.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblTrainerCode")%></label>
                                                </div>

                                                <div class="col-sm-8">
                                                    <asp:TextBox ID="txtInternalEmployeeTrainerCode" CssClass="form-control cleanPasteText enterkey" runat="server" onkeypress="return isNumberOrLetter(event)" MaxLength="10"></asp:TextBox>
                                                    
                                                    <label id="txtInternalEmployeeTrainerCodeValidation" for="<%= txtInternalEmployeeTrainerCode.ClientID%>" 
                                                        class="label label-danger label-validation" data-toggle="tooltip" 
                                                        data-container="body" data-placement="left" 
                                                        data-content="<%= GetLocalResourceObject("msjTrainerCodeValidation") %>" 
                                                        style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <div class="col-sm-4 text-left">
                                                    <label for="<%= txtInternalEmployeeTrainerName.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblTrainerName")%></label>
                                                </div>

                                                <div class="col-sm-8">
                                                    <asp:TextBox ID="txtInternalEmployeeTrainerName" CssClass="form-control cleanPasteText enterkey" runat="server" onkeypress="return isNumberOrLetter(event) && checkMaxLength(this,event,500);" onkeyUp="return isNumberOrLetter(event) && checkMaxLength(this,event,500);" MaxLength="500" TextMode="MultiLine" Columns="3"></asp:TextBox>
                                                    
                                                    <label id="txtInternalEmployeeTrainerNameValidation" for="<%= txtInternalEmployeeTrainerName.ClientID%>" 
                                                        class="label label-danger label-validation" data-toggle="tooltip"
                                                        data-container="body" data-placement="left" 
                                                        data-content="<%= GetLocalResourceObject("msjTrainerNameValidation") %>" 
                                                        style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <div class="col-sm-4 text-left">
                                                    <label for="<%= txtInternalEmployeeTrainerTelephone.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblTrainerTelephone")%></label>
                                                </div>

                                                <div class="col-sm-8">
                                                    <asp:TextBox ID="txtInternalEmployeeTrainerTelephone" CssClass="form-control cleanPasteText enterkey" runat="server" onkeypress="return isNumberOrLetter(event)" MaxLength="20"></asp:TextBox>
                                                   
                                                    <label id="txtInternalEmployeeTrainerTelephoneValidation" for="<%= txtInternalEmployeeTrainerTelephone.ClientID%>" 
                                                        class="label label-danger label-validation" data-toggle="tooltip" 
                                                        data-container="body" data-placement="left" 
                                                        data-content="<%= GetLocalResourceObject("msjTrainerTelephoneValidation") %>" 
                                                        style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <div class="col-sm-4 text-left">
                                                    <label for="<%= chbInternalEmployeeSearchEnabled.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblSearchEnabled")%></label>
                                                </div>

                                                <div class="col-sm-3">
                                                    <asp:CheckBox ID="chbInternalEmployeeSearchEnabled" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>

                        <div class="modal-footer">
                            <button id="btnAccept" type="button" runat="server" class="btn btn-primary btnAjaxAction btnAccept"
                                onserverclick="BtnAccept_ServerClick" onclick="return ProcessExternalsAcceptRequest(this.id);"
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

    <%--  Modal for list course by Trainer  --%>
    <div class="modal fade" id="AssociatedTrainingCoursesDialog" tabindex="-1" role="dialog" aria-labelledby="AssociatedTrainingCoursesDialogTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button id="btnAssociatedTrainingCoursesClose" type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title text-primary"><%=Convert.ToString(GetLocalResourceObject("TitleAssociatedTrainingCoursesDialog")) %></h3>
                </div>

                <div class="modal-body">
                    <div class="form-horizontal">
                        <asp:HiddenField ID="hdfModeAddTraining" runat="server" Value="False" />
                        <asp:Button ID="btnRefreshAssociatedTrainingCourses" runat="server" OnClick="BtnRefreshAssociatedTrainingCourses_Click" Style="display: none;" />

                        <asp:UpdatePanel runat="server" ID="uppAssociatedTrainingCourses" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnRefreshAssociatedTrainingCourses" EventName="Click" />
                            </Triggers>

                            <ContentTemplate>
                                <div class="form-group">
                                    <div class="col-sm-12 text-left">
                                        <h4><label id="lblAssociatedTrainingCourses" runat="server" class="control-label"><%=GetLocalResourceObject("lblAssociatedTrainingCourses")%></label></h4>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-12 text-left">
                                        <asp:Repeater ID="rptAssociatedTrainingCourses" runat="server" OnItemDataBound="RptAssociatedTrainingCourses_ItemDataBound">
                                            <HeaderTemplate>
                                                <div style="width: 100%; overflow-y: scroll;">
                                                    <table id="tableSelectAssociatedTrainingCourses" class="table table-hover table-striped">
                                                        <thead>
                                                            <tr>
                                                                <th>
                                                                    <div>
                                                                        <div class="col-xs-4 col-sm-4 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-code" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblTrainingCoursesCodeHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                        <div class="col-xs-6 col-sm-6 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-name" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblTrainingCoursesNameHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
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
                                                                        <button id="btnDeleteAssociatedCourse" type="button" runat="server" class="btn btn-default btnAjaxAction btnRemoveCourse" 
                                                                            onserverclick="BtnDeleteAssociatedCourse_ServerClick"
                                                                            data-loading-text="<span class='fa fa-spinner fa-spin '></span>" 
                                                                            data-error-text="<span class='glyphicon glyphicon-exclamation-sign'></span>">
                                                                            <span class="glyphicon glyphicon-remove-circle glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                                                                        </button>

                                                                        <asp:TextBox ID="hdfAssociatedCourseCode" CssClass="CourseCode" runat="server" Text='<%#DataBinder.Eval(((RepeaterItem)Container).DataItem, "CourseCode") %>' Style="display: none;" />
                                                                        <asp:TextBox ID="hdfAssociatedCourseName" CssClass="CourseName" runat="server" Text='<%#DataBinder.Eval(((RepeaterItem)Container).DataItem, "CourseName") %>' Style="display: none;" />
                                                                        <asp:HiddenField ID="hdfAssociatedCourseSearchEnabled" runat="server" Value='<%#DataBinder.Eval(((RepeaterItem)Container).DataItem, "SearchEnabled") %>' />
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
                                        <asp:Repeater ID="rptCourses" runat="server" OnItemDataBound="RptCourses_ItemDataBound">
                                            <HeaderTemplate>
                                                <div style="width: 100%; max-height: 300px; overflow-y: scroll;">
                                                    <table id="tableSelectCourse" class="table table-hover table-striped">
                                                        <thead>
                                                            <tr>
                                                                <th>
                                                                    <div>
                                                                        <div class="col-xs-4 col-sm-4 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-code" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblCourseCodeHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                        <div class="col-xs-6 col-sm-6 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-name" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblCourseNameHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
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
                                                                        <button id="btnAddCourse" type="button" runat="server" class="btn btn-default btnAjaxAction btnAddCourse" 
                                                                            onserverclick="BtnAddCourse_ServerClick"
                                                                            data-loading-text="<span class='fa fa-spinner fa-spin '></span>" 
                                                                            data-error-text="<span class='glyphicon glyphicon-exclamation-sign'></span>">
                                                                            <span class="glyphicon glyphicon-plus-sign glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                                                                        </button>
                                                                        
                                                                        <asp:TextBox ID="hdfCourseCode" CssClass="CourseCode" runat="server" Text='<%#Eval("CourseCode") %>' Style="display: none;" />
                                                                        <asp:TextBox ID="hdfCourseName" CssClass="CourseName" runat="server" Text='<%#Eval("CourseName") %>' Style="display: none;" />
                                                                        <asp:HiddenField ID="hdfCourseSearchEnabled" runat="server" Value='<%#Eval("SearchEnabled") %>' />
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
                    <button id="btnAssociatedTrainingCoursesAccept" type="button" class="btn btn-default">
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
                                            <label for="<%= txtDuplicatedTrainerType.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblTrainerType")%></label>
                                        </div>

                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtDuplicatedTrainerType" CssClass="form-control cleanPasteText ignoreValidation" runat="server" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-4 text-left">
                                            <label for="<%= txtDuplicatedTrainerCode.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblTrainerCode")%></label>
                                        </div>

                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtDuplicatedTrainerCode" CssClass="form-control cleanPasteText ignoreValidation" runat="server" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-4 text-left">
                                            <label for="<%= txtDuplicatedTrainerName.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblTrainerName")%></label>
                                        </div>

                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtDuplicatedTrainerName" CssClass="form-control cleanPasteText ignoreValidation" runat="server" TextMode="MultiLine" Columns="3" ReadOnly="true"></asp:TextBox>
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
                                <asp:HiddenField ID="hdfActivateDeletedTrainerCode" runat="server" Value="" />
                                <asp:HiddenField ID="hdfActivateDeletedTrainerType" runat="server" Value="" />

                                <div id="divActivateDeletedDialog" runat="server"></div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtActivateDeletedTrainerType.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblTrainerType")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtActivateDeletedTrainerType" CssClass="form-control cleanPasteText ignoreValidation" runat="server" TextMode="MultiLine" Columns="3" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtActivateDeletedTrainerCode.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblTrainerCode")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtActivateDeletedTrainerCode" CssClass="form-control cleanPasteText ignoreValidation" runat="server" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtActivateDeletedTrainerName.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblTrainerName")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtActivateDeletedTrainerName" CssClass="form-control cleanPasteText ignoreValidation" runat="server" TextMode="MultiLine" Columns="3" ReadOnly="true"></asp:TextBox>
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
            $('#<%= chbExternalsSearchEnabled.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });

            $('#<%= chbInternalEmployeeSearchEnabled.ClientID %>').bootstrapToggle({
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

            ShowModalPanelForTrainerType();
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
            $('#btnAssociatedTrainingCoursesAccept, #btnAssociatedTrainingCoursesCancel, #btnAssociatedTrainingCoursesClose').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                event.preventDefault();
                $('#AssociatedTrainingCoursesDialog').modal('hide');
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

                $("#<%=cboTrainerType.ClientID%>").prop('disabled', false);
                $("#<%=txtExternalsTrainerCode.ClientID%>").prop('disabled', false);
            });
            //////////////////////////////////////////////////////////////////////////////////////////////////

            //In this section we set the others controls functionality 
            $('#<%= btnAdd.ClientID %>').on('click', function (event) {
                /// <summary>Handles the click event for button add.</summary>
                event.preventDefault();

                $("#<%=txtSearchEmployees.ClientID%>").prop("disabled", false);
                $("#<%=txtSearchEmployees.ClientID%>").attr("readonly", false);

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

            $('#<%= txtTrainerCodeFilter.ClientID %>').on('keyup keypress', function (e) {
                ReturnFromBtnSearchClickPostBack(e);

                return isNumberOrLetter(e);
            });

            $('#<%= txtTrainerNameFilter.ClientID %>').on('keyup keypress', function (e) {
                ReturnFromBtnSearchClickPostBack(e);

                return isNumberOrLetter(e);
            });

            $('#<%= cboTrainerTypeFilter.ClientID %>').on('keyup keypress', function (e) {
                ReturnFromBtnSearchClickPostBack(e);
            });

            $("#<%= txtSearchCourses.ClientID %>").keyup(function (event) {
                if (isNumberOrLetterNoEnter(event)) {
                    SetDelayForSearchCoursesPostBack();
                }
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

            $('#<%= cboTrainerType.ClientID %>').change(function (event) {
                /// <summary>Handles the change event for cboTrainerType in edit add dialog.</summary>
                $('#MaintenanceInternalEmployeePanel').hide();
                $('#MaintenanceExternalsPanel').hide();

                ClearModalFormContents();
                ShowModalPanelForTrainerType();
            });

            $("#<%= txtSearchEmployees.ClientID %>").keyup(function (e) {
                if (isNumberOrLetterNoEnter(e)) {
                    SetDelayForSearchEmployeesPostBack();
                }
            });

            $("#txtSearchParticipants").on("keyup", function () {
                var value = $(this).val().toLowerCase();
                $("#tableParticipants tbody tr").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
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
        function ValidateForm() {
            /// <summary>Validate the form with jquey validation plugin </summary>
            /// <returns> True if form is valid. False otherwise. </returns>

            //add custom validation methods
            jQuery.validator.addMethod("validSelection", function (value, element) {
                return this.optional(element) || value != "-1";
            }, "Please select a valid option");

            if (validator != null) {
                validator.destroy();
            }

            var trainerType = $('#<%= cboTrainerType.ClientID %>').val();

            if ("E" == trainerType) {
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
                            "<%= cboTrainerType.UniqueID %>": {
                                required: true, validSelection: true
                            },

                            "<%= txtInternalEmployeeTrainerCode.UniqueID %>": {
                                required: true,
                                normalizer: function (value) {
                                    return $.trim(value);
                                },
                                minlength: 1,
                                maxlength: 10
                            },

                            "<%= txtInternalEmployeeTrainerName.UniqueID %>": {
                                required: true,
                                normalizer: function (value) {
                                    return $.trim(value);
                                },
                                minlength: 1,
                                maxlength: 500
                            }
                        }
                    });
            }

            else if ("EC" == trainerType || "EP" == trainerType || "GA" == trainerType) {
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
                            "<%= cboTrainerType.UniqueID %>": {
                                required: true,
                                validSelection: true
                            },

                            "<%= txtExternalsTrainerCode.UniqueID %>": {
                                required: true,
                                normalizer: function (value) {
                                    return $.trim(value);
                                },
                                minlength: 1,
                                maxlength: 10
                            },

                            "<%= txtExternalsTrainerName.UniqueID %>": {
                                required: true,
                                normalizer: function (value) {
                                    return $.trim(value);
                                },
                                minlength: 1,
                                maxlength: 500
                            }
                        }
                    });
            }

            else {
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
                            "<%= cboTrainerType.UniqueID %>": {
                                required: true,
                                validSelection: true
                            }
                        }
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

        function SelectInternalEmployee(btnId) {
            $("#<%=txtInternalEmployeeTrainerCode.ClientID%>").val($("#" + btnId).siblings(".EmployeeCode").val());
            $("#<%=txtInternalEmployeeTrainerName.ClientID%>").val($("#" + btnId).siblings(".EmployeeName").val());
            $("#<%=txtInternalEmployeeTrainerTelephone.ClientID%>").focus();

            setTimeout(function () { $("#" + btnId).button('reset'); }, 100);
        }

        function ClearModalForm() {
            /// <summary>Clear the modal form</summary>
            $("#<%=hdfTrainerCodeEdit.ClientID%>").val("");

            $('#<%=cboTrainerType.ClientID%>').val('default');
            $('#<%=cboTrainerType.ClientID%>').selectpicker("refresh");
            $("#<%=cboTrainerType.ClientID%>").val("-1");

            $('#MaintenanceInternalEmployeePanel').hide();
            $('#MaintenanceExternalsPanel').hide();

            ClearModalFormContents();

            if (validator != null) {
                validator.resetForm();
            }
        }

        function ClearModalFormContents() {
            $("#<%=txtExternalsTrainerCode.ClientID%>").val("");
            $("#<%=txtExternalsTrainerName.ClientID%>").val("");
            $("#<%=txtExternalsTrainerTelephone.ClientID%>").val("");
            $("#<%=chbExternalsSearchEnabled.ClientID%>").bootstrapToggle('off');

            $("#<%=txtSearchEmployees.ClientID%>").prop("disabled", false);
            $("#<%=txtSearchEmployees.ClientID%>").val("");
            $("#tableBodySelectEmployee").empty();

            $("#<%=txtInternalEmployeeTrainerCode.ClientID%>").val("");
            $("#<%=txtInternalEmployeeTrainerName.ClientID%>").val("");
            $("#<%=txtInternalEmployeeTrainerTelephone.ClientID%>").val("");
            $("#<%=chbInternalEmployeeSearchEnabled.ClientID%>").bootstrapToggle('off');

            if (validator != null) {
                validator.resetForm();
            }
        }

        function ShowModalPanelForTrainerType() {
            if ("E" == $('#<%= cboTrainerType.ClientID %>').val()) {
                $('#MaintenanceInternalEmployeePanel').show();
                $('#MaintenanceExternalsPanel').hide();
            }
            else if ("EC" == $('#<%= cboTrainerType.ClientID %>').val()) {
                $('#MaintenanceInternalEmployeePanel').hide();
                $('#MaintenanceExternalsPanel').show();
            }
            else if ("EP" == $('#<%= cboTrainerType.ClientID %>').val()) {
                $('#MaintenanceInternalEmployeePanel').hide();
                $('#MaintenanceExternalsPanel').show();
            }
            else if ("GA" == $('#<%= cboTrainerType.ClientID %>').val()) {
                $('#MaintenanceInternalEmployeePanel').hide();
                $('#MaintenanceExternalsPanel').show();
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
        

        function CloseUserDialog() {
            /// <summary>Close a user dialog.</summary>
            $('#MaintenanceDialog').modal('hide');
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

        function DisableBtnAdd(btnId, status) {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $("#" + btnId).prop('disabled', Boolean.parse(status));
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
        var delayForSearchCoursesPostBack = null;
        function SetDelayForSearchCoursesPostBack() {
            /// <summary>Set a timer for delay the search thematic areas post back while users writes</summary>
            if (delayForSearchCoursesPostBack != null) {
                clearTimeout(delayForSearchCoursesPostBack);
            }

            delayForSearchCoursesPostBack = setTimeout("SearchCoursesPostBack()", 500);
        }

        var delayForSearchEmployeesPostBack = null;
        function SetDelayForSearchEmployeesPostBack() {
            /// <summary>Set a timer for delay the search persons post back while users writes</summary>
            if (delayForSearchEmployeesPostBack != null) {
                clearTimeout(delayForSearchEmployeesPostBack);
            }

            delayForSearchEmployeesPostBack = setTimeout("SearchEmployeesPostBack()", 500);
        }

        function ProcessEditRequest(resetId) {
            /// <summary>Process the edit request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            if (IsRowSelected()) {
                $("#<%=txtSearchEmployees.ClientID%>").val("");
                $("#tableBodySelectEmployee").empty();
                $("#<%=txtInternalEmployeeTrainerCode.ClientID%>").val("");
                $("#<%=txtSearchEmployees.ClientID%>").prop("disabled", true);

                __doPostBack('<%= btnEdit.UniqueID %>', '');
                return true;
            }

            else {
                ShowFooterError('<%= GetLocalResourceObject("msgInvalidSelection") %>');
                ErrorButton(resetId);
                return false;
            }
        }

        function ProcessCoursesByTrainersRequest(resetId) {
            /// <summary>Process the edit request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            if (IsRowSelected()) {
                __doPostBack('<%= btnCoursesByTrainers.UniqueID %>', '');
                return true;
            }

            else {
                $(".btndisabled").prop("disabled", false);
                ShowFooterError('<%= GetLocalResourceObject("msgInvalidSelection") %>');
                ErrorButton(resetId);
                return false;
            }
        }

        function ProcessExternalsAcceptRequest(resetId) {
            /// <summary>Process the accept request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            DisableButtonsDialog();

            if (!ValidateForm()) {
                setTimeout(function () {
                    ResetButton(resetId);
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
        function ReturnFromBtnAddClickPostBack() {
            ClearModalForm();
        }

        function ReturnFromBtnEditClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $('#MaintenanceDialog').modal('show');
            SetRowSelected();
            DisableToolBar();

            $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#dialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');

            $("#<%=txtExternalsTrainerCode.ClientID%>").attr("disabled", "disabled");

            ShowModalPanelForTrainerType();

            EnableToolBar();
        }

        function ReturnFromBtnSearchClickPostBack(e) {
            var keyCode = e.keyCode || e.which;

            if (keyCode === 13) {
                __doPostBack('<%= btnSearchDefault.UniqueID %>', '');
                return false;
            }
        }

        function ReturnFrombtnAcceptClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            DisableButtonsDialog();
            SetRowSelected();

            $('#MaintenanceDialog').modal('hide');

            ShowFooterSuccess('<%= GetLocalResourceObject("msgOperationSuccesfull") %>');
            EnableButtonsDialog();
        }

        function ReturnFromBtnCoursesClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            SetRowSelected();
            DisableToolBar();

            $('#AssociatedTrainingCoursesDialog').modal('show');

            EnableToolBar();
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
            ShowModalPanelForTrainerType();

            $('#MaintenanceDialog').modal('hide');
            $('#ActivateDeletedDialog').modal('show');
        }

        function ReturnFromBtnAcceptClickPostBackDuplicated() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            ShowModalPanelForTrainerType();

            $('#MaintenanceDialog').modal('hide');
            $('#DuplicatedDialog').modal('show');
        }

        function ReturnFromBtnDeleteClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            UnselectRow();
            ShowFooterSuccess('<%= GetLocalResourceObject("msgOperationSuccesfull") %>');

            $('.btndisabled').prop('disabled', false);
        }

        function ReturnFromBtnAddCourseClickPostBack(btnId) {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $("#" + btnId).closest("tr").fadeTo("slow", 0.2, function () {
                $(this).remove();
            });
        }

        function ReturnFromBtnDeleteCourseClickPostBack(btnId) {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            setTimeout(function () { __doPostBack('<%= btnRefreshAssociatedTrainingCourses.UniqueID %>', ''); }, 100);

            $("#" + btnId).closest("tr").fadeTo("slow", 0.2, function () {
                $(this).remove();
            });
        }

        function SearchEmployeesPostBack() {
            /// <summary>Executes the search persons post back</summary>
            $("#txtSearchEmployeesWaiting").show();
            $('#<%= txtSearchEmployees.ClientID %>').prop("disabled", true);
            __doPostBack("<%= btnSearchEmployees.UniqueID %>", '');
        }

        function SearchCoursesPostBack() {
            /// <summary>Executes the search persons post back</summary>
            $("#txtSearchCoursesWaiting").show();
            $('#<%= txtSearchCourses.ClientID %>').prop("disabled", true);
            __doPostBack("<%= btnSearchCourses.UniqueID %>", '');
        }

        function ReturnFromSearchEmployeesPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $("#txtSearchEmployeesWaiting").hide();
            $('#<%= txtSearchEmployees.ClientID %>').prop("disabled", false);
            $('#MaintenanceInternalEmployeePanel').show();
        }

        function ReturnFromSearchCoursesPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $("#txtSearchCoursesWaiting").hide();
            $('#<%= txtSearchCourses.ClientID %>').prop("disabled", false);
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
            },
                '<%=GetLocalResourceObject("No")%>', function () {
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
