<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="VacationRequest.aspx.cs" Inherits="HRISWeb.Vacations.VacationRequest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntHeader" runat="server">
    <title><%=Convert.ToString(GetLocalResourceObject("lblScreenTitle")) %></title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cntBody" runat="server">



    <div class="main-content">
        <asp:Panel ID="pnlMainContent" runat="server">
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
                            <div class="col-sm-12">
                                <div class="form-horizontal">

                                    <div class="row isPersonHelper">
                                        <div class="col-sm-12 col-md-6">
                                            <div class="form-group">
                                                <div class="col-sm-5 text-left">
                                                    <label for="<%=cboSeatSuperior.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblSeatSuperior")%></label>
                                                </div>

                                                <div class="col-sm-7">
                                                    <asp:DropDownList ID="cboSeatSuperior" CssClass="form-control control-validation selectpicker" AutoPostBack="false" runat="server" data-live-search="true" data-actions-box="true" data-id="Trainer" data-value="isPermitted"></asp:DropDownList>
                                                    <asp:HiddenField ID="hdfTrainerValueFilter" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-12 col-md-6">
                                            <div class="form-group">
                                                <div class="col-sm-5 text-left">
                                                    <label for="<%= txtNameEmployeeSearch.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCourseCode")%></label>
                                                </div>

                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtNameEmployeeSearch" CssClass="form-control control-validation cleanPasteText ignoreValidation" runat="server" MaxLength="10" autocomplete="off" type="text" data-id="CourseCode" data-value="isPermitted"></asp:TextBox>
                                                    <asp:HiddenField ID="hdftxtNameEmployeeSearchFilter" runat="server" />
                                                </div>

                                            </div>
                                        </div>

                                        <div class="col-sm-12 col-md-6">
                                            <div class="form-group">
                                                <div class="col-sm-5 text-left">
                                                    <label for="<%=cboCenterCostFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCenterCost")%></label>
                                                </div>

                                                <div class="col-sm-7">
                                                    <asp:DropDownList ID="cboCenterCostFilter" CssClass="form-control cboAjaxAction control-validation ignoreValidation" AutoPostBack="false" runat="server" data-id="TrainerType" data-value="isPermitted"></asp:DropDownList>
                                                    <asp:HiddenField ID="hdfCenterCostValueFilter" runat="server" />
                                                    <asp:HiddenField ID="hdfCenterCostTextFilter" runat="server" />
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="row">
                                        <div class="col-sm-12 col-md-6">
                                            <div class="form-group">
                                                <div class="col-sm-5 text-left">
                                                    <label for="<%= txtEmployeeCode.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCodeEmployee")%></label>
                                                </div>

                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtEmployeeCode" CssClass="form-control control-validation cleanPasteText ignoreValidation" runat="server" MaxLength="10" autocomplete="off" type="text" data-id="CourseCode" data-value="isPermitted"></asp:TextBox>
                                                    <asp:HiddenField ID="hdftxtEmployeeCode" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-12 col-md-6"></div>
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
                                        DataKeyNames="EmployeeCode,EmployeeName"
                                        OnPreRender="GrvList_PreRender" OnSorting="GrvList_Sorting">
                                        <Columns>
                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="EmployeeCodeSort" runat="server" CommandName="Sort" CommandArgument="EmployeeCode" OnClientClick="SetWaitingGrvList(true);">          
                        <span><%= GetLocalResourceObject("vacationEmployeeCode.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "EmployeeCode") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;VacationDay: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <span id="dvEmployeeCode" data-id="EmployeeCode" data-value="<%# Eval("EmployeeCode") %>"><%# Eval("EmployeeCode") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="EmployeeNameSort" runat="server" CommandName="Sort" CommandArgument="EmployeeName" OnClientClick="SetWaitingGrvList(true);">          
                        <span><%= GetLocalResourceObject("vacationName.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "EmployeeName") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;VacationDay: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <span id="dvEmployeeName" data-id="EmployeeName" data-value="<%# Eval("EmployeeName") %>"><%# Eval("EmployeeName") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="FarmNameSort" runat="server" CommandName="Sort" CommandArgument="FarmName" OnClientClick="SetWaitingGrvList(true);">                
                        <span><%= GetLocalResourceObject("vacationFarmName.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "FarmName") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;VacationDay: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <span id="dvFarmName" data-id="FarmName" data-value="<%# Eval("FarmName") %>"><%# Eval("FarmName") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="CostCenterSort" runat="server" CommandName="Sort" CommandArgument="CostCenter" OnClientClick="SetWaitingGrvList(true);">                
                        <span><%= GetLocalResourceObject("vacationCostCenter.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "CostCenter") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;VacationDay: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <span id="dvCostCenter" data-id="CostCenter" data-value="<%# Eval("CenterCostCode") %>"><%# Eval("CenterCostCode") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="TotalVacationSort" runat="server" CommandName="Sort" CommandArgument="TotalVacation" OnClientClick="SetWaitingGrvList(true);">                
                        <span><%= GetLocalResourceObject("vacationTotal.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "TotalVacation") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;VacationDay: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <span id="dvTotalVacation" data-id="TotalVacation" data-value="<%# Eval("TotalVacation", "{0:N2}") %>"><%# Eval("TotalVacation", "{0:N2}") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <div id="grvListWaiting" style="vacationday: absolute; margin: auto; display: none; bottom: 0px; top: 0px; left: 50%; transform: translate(-50%, 35%);">
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

            <div class="ButtonsActions">
                <asp:UpdatePanel runat="server" ID="uppActions" UpdateMode="Conditional" ChildrenAsTriggers="true">
                    <Triggers></Triggers>
                    <ContentTemplate>
                        <div class="btn-group" role="group" aria-label="main-buttons">

                            <button id="btnRequest" type="button" runat="server" class="btn btn-default btnAjaxAction btndisabled"
                                onserverclick="BtnRequest_ServerClick" onclick="return ProcesRequestVacaction(this.id);"
                                data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnRequest"))%>'
                                data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnRequest"))%>'>
                                <span class="glyphicon glyphicon-copy glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                                <br />
                                <%= GetLocalResourceObject("btnRequest") %>
                            </button>

                            <button id="btnHistory" type="button" runat="server" class="btn btn-default btnAjaxAction btndisabled"
                                onserverclick="BtnHistory_ServerClick" onclick="return ProcesHistory(this.id);"
                                data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnHistory"))%>'
                                data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnHistory"))%>'>
                                <span class="glyphicon glyphicon-time glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                                <br />
                                <%= GetLocalResourceObject("btnHistory") %>
                            </button>

                            <button id="btnExport" type="button" runat="server" class="btn btn-default btnAjaxAction btndisabled"
                                onserverclick="BtnHistory_ServerClick"
                                data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnExport"))%>'
                                data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnExport"))%>'>
                                <span class="glyphicon glyphicon-download-alt glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                                <br />
                                <%= GetLocalResourceObject("btnExport") %>
                            </button>

                            <button id="btnBalanceDetails" type="button" runat="server" class="btn btn-default btnAjaxAction btndisabled"
                                onserverclick="BtnBalanceDetails_ServerClick" onclick="return ProcessBalanceDetail(this.id);"
                                data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnBalanceDetails"))%>'
                                data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnBalanceDetails"))%>'>
                                <span class="glyphicon glyphicon-list-alt glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                                <br />
                                <%= GetLocalResourceObject("btnBalanceDetails") %>
                            </button>

                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:Panel>
    </div>

    <%-- Modal for request Vacation --%>
    <div class="modal fade" id="VacationRequestDialog" tabindex="-1" role="dialog" aria-labelledby="VacationRequestDialogTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button id="btnVacationRequestClose" type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title text-primary"><%=Convert.ToString(GetLocalResourceObject("TitleVacationRequestDialog")) %></h3>
                </div>


                <div class="modal-body">
                    <div class="form-horizontal">
                        <asp:HiddenField ID="hdfVacationCodeEmployee" runat="server" Value="" />
                        <asp:UpdatePanel runat="server" ID="uppVacationRequestDialog">
                            <Triggers></Triggers>
                            <ContentTemplate>
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtCodeEmployee.ClientID%>" class="control-label code"><%=GetLocalResourceObject("lblCodeEmployee")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtCodeEmployee" CssClass="form-control cleanPasteText" runat="server" onkeypress="return isNumberOrLetter(event)" MaxLength="10"></asp:TextBox>

                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtNameEmployee.ClientID%>" class="control-label code"><%=GetLocalResourceObject("lblCourseCode")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtNameEmployee" CssClass="form-control cleanPasteText" runat="server" onkeypress="return isNumberOrLetter(event)" MaxLength="10"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group isNotmandatory">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= dtpSelectDay.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblSelectDay")%></label>
                                    </div>
                                    <div class="col-sm-7">
                                        <asp:TextBox runat="server" ID="dtpSelectDay" class="form-control date control-validation cleanPasteDigits" placeholder='<%$ Code:GetLocalResourceObject("lblDateFormatPlaceHolder") %>' autocomplete="off" />
                                        <label id="dtpSelectDayValidation" for="<%= dtpSelectDay.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; vacationday: relative; z-index: 2;">!</label>
                                    </div>
                                    <div class="col-sm-1">
                                        <button id="btnAddDayRequest" type="button" runat="server" class="btn btn-default btnAjaxAction btnAddDayRequest" data-loading-text="<span class='fa fa-spinner fa-spin '></span>" data-error-text="<span class='glyphicon glyphicon-exclamation-sign'></span>" onserverclick="BtnAddDay_ServerClick">
                                            <span class="glyphicon glyphicon-plus-sign glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                                        </button>
                                    </div>
                                </div>


                                <div class="form-group ismandatory">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= dtpStartDate.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblStartDate")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:TextBox runat="server" ID="dtpStartDate" class="form-control date control-validation cleanPasteDigits" placeholder='<%$ Code:GetLocalResourceObject("lblDateFormatPlaceHolder") %>' autocomplete="off" />
                                        <label id="dtpStartDateValidation" for="<%= dtpStartDate.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; vacationday: relative; z-index: 2;">!</label>
                                    </div>
                                </div>


                                <div class="form-group ismandatory">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= dtpEndDate.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblEndDate")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:TextBox runat="server" ID="dtpEndDate" class="form-control date control-validation cleanPasteDigits" placeholder='<%$ Code:GetLocalResourceObject("lblDateFormatPlaceHolder") %>' autocomplete="off" />
                                        <label id="dtpEndDateValidation" for="<%= dtpEndDate.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; vacationday: relative; z-index: 2;">!</label>
                                    </div>
                                </div>



                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= chbTypeRequest.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblTypeRequest")%></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <asp:CheckBox ID="chbTypeRequest" runat="server" class="check-toggle" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                    </div>
                                </div>


                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <asp:UpdatePanel runat="server" ID="uppVacationDay" UpdateMode="Conditional" ChildrenAsTriggers="true">
                            <Triggers>
                            </Triggers>

                            <ContentTemplate>
                                <div class="form-group isNotmandatory">
                                    <div class="col-sm-12 text-left">
                                        <h4>
                                            <label id="lblDaysRequest" runat="server" class="control-label"><%=GetLocalResourceObject("lblDaysRequest")%></label></h4>
                                    </div>
                                </div>

                                <div class="form-group isNotmandatory">
                                    <div class="col-sm-12 text-left">
                                        <asp:Repeater ID="rptVacationDays" runat="server">
                                            <HeaderTemplate>
                                                <div style="width: 100%; max-height: 300px; overflow-y: scroll;">
                                                    <table id="tableSelectVacationDay" class="table table-hover table-striped">
                                                        <thead>
                                                            <tr>
                                                                <th>
                                                                    <div>
                                                                        <div class="col-xs-4 col-sm-4 text-primary sorter" style="cursor: pointer;" data-sort-attr="data-sort-code" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblVacationDay") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                                        <div class="col-xs-2 col-sm-2 text-primary">&nbsp;</div>
                                                                    </div>
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                        <tbody id="tableBodySelectVacationDay">
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <tr>
                                                    <td class="row-fluid">
                                                        <asp:UpdatePanel runat="server" ID="uppVacationDay" UpdateMode="Conditional">
                                                            <Triggers>
                                                            </Triggers>

                                                            <ContentTemplate>
                                                                <div class="data-sort-src" data-sort-code='<%# Eval("RequestDay") %>' data-sort-name='<%# Eval("RequestDay") %>'>


                                                                    <div class="col-xs-6 col-sm-6">
                                                                        <span>
                                                                            <%# Eval("RequestDay", "{0:dd/MM/yyyy}") %>
                                                                        </span>
                                                                    </div>

                                                                    <div id="divAddControlsVacationDays" runat="server" class="col-xs-2 col-sm-2 text-center">
                                                                        <button id="btnRemoveVacationDay" type="button" runat="server" class="btn btn-default btnAjaxAction btnRemoveVacationDay btnAjaxDisable" data-loading-text="<span class='fa fa-spinner fa-spin '></span>" data-error-text="<span class='glyphicon glyphicon-exclamation-sign'></span>" onserverclick="BtnRemoveDay_ServerClick">
                                                                            <span class="glyphicon glyphicon-remove-circle glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                                                                        </button>
                                                                        <asp:TextBox ID="hdfVacationDayCode" CssClass="VacationDayCode" runat="server" Text='<%#Eval("RequestDay") %>' Style="display: none;" />
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
                    <button id="btnAccept" type="button" runat="server" class="btn btn-primary btnAccept "
                        onserverclick="BtnAccept_ServerClick" onclick="return ProcessExternalsAcceptRequest(this.id);"
                        data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>'
                        data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>'>
                        <span class="glyphicon glyphicon-ok glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnDialogSave")) %>
                    </button>

                    <button id="btnCancel" type="button" class="btn btn-default">
                        <span class="glyphicon glyphicon-remove glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnDialogCancel")) %>
                    </button>
                </div>
            </div>
        </div>
    </div>

    <%-- Modal for history vacation --%>
    <div class="modal fade" id="VacationHistoryDialog" tabindex="-1" role="dialog" aria-labelledby="VacationHistoryDialogTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button id="btnVacationHistoryClose" type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h3 class="modal-title text-primary"><%= Convert.ToString(GetLocalResourceObject("TitleVacationHistoryDialog")) %></h3>
                </div>

                <div class="modal-body">
                    <div class="form-horizontal">
                        <asp:UpdatePanel runat="server" ID="uppVacationHistorys">
                            <ContentTemplate>
                                <div class="form-group">
                                    <div class="col-sm-12 text-left">
                                        <h4>
                                            <label id="lblHistoryVacaction" runat="server" class="control-label"><%= GetLocalResourceObject("lblHistoryVacaction") %></label>
                                        </h4>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-12 text-left">
                                        <asp:GridView ID="grvHistory"
                                            Width="100%"
                                            runat="server"
                                            EmptyDataText='<%$ Code:GetLocalResourceObject("lblNoData") %>' EmptyDataRowStyle-CssClass="emptyRow"
                                            AllowPaging="false" PagerSettings-Visible="false" AllowSorting="true"
                                            AutoGenerateColumns="false" ShowHeader="true"
                                            CssClass="table table-striped table-hover table-bordered"
                                            DataKeyNames="RequestDate"
                                            OnPreRender="GrvHistory_PreRender" OnSorting="GrvHistory_Sorting">
                                            <Columns>

                                                <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                    <HeaderTemplate>
                                                        <div style="width: 100%; text-align: center;">
                                                            <asp:LinkButton ID="RequestDateSort" runat="server" CommandName="Sort" CommandArgument="RequestDate">          
                                                                <span><%= GetLocalResourceObject("lblDateRequest") %></span>
                                                            </asp:LinkButton>
                                                        </div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span id="dvRequestDate" data-id="RequestDate" data-value="<%# Eval("RequestDate") %>"><%# Eval("RequestDate") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                    <HeaderTemplate>
                                                        <div style="width: 100%; text-align: center;">
                                                            <asp:LinkButton ID="PeriodSort" runat="server" CommandName="Sort" CommandArgument="Period">          
                                                                <span><%= GetLocalResourceObject("lblPeriod") %></span>
                                                            </asp:LinkButton>
                                                        </div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span id="dvPeriod" data-id="Period" data-value="<%# Eval("Period") %>"><%# Eval("Period") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                    <HeaderTemplate>
                                                        <div style="width: 100%; text-align: center;">
                                                            <asp:LinkButton ID="DaysSort" runat="server" CommandName="Sort" CommandArgument="Days">          
                                                                <span><%= GetLocalResourceObject("lblDays") %></span>
                                                            </asp:LinkButton>
                                                        </div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span id="dvDays" data-id="Days" data-value="<%# Eval("Days") %>"><%# Eval("Days") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                    <HeaderTemplate>
                                                        <div style="width: 100%; text-align: center;">
                                                            <asp:LinkButton ID="StatusSort" runat="server" CommandName="Sort" CommandArgument="Status">          
                                                                <span><%= GetLocalResourceObject("lblStatus") %></span>
                                                            </asp:LinkButton>
                                                        </div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span id="dvStatus" data-id="Status" data-value="<%# Eval("Status") %>"><%# Eval("Status") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>

                <div class="modal-footer">
                    <button id="btnVacationHistoryAccept" type="button" class="btn btn-default">
                        <span class="glyphicon glyphicon-ok glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%= Convert.ToString(GetLocalResourceObject("btnAccept")) %>
                    </button>
                </div>
            </div>
        </div>
    </div>

    <%-- Modal for detail vacation --%>
    <div class="modal fade" id="VacationDetail" tabindex="-1" role="dialog" aria-labelledby="VacationDetailTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">

                <div class="modal-header">
                    <button id="btnVacationDetailClose" type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h3 class="modal-title text-primary"><%= Convert.ToString(GetLocalResourceObject("TitleVacationDetailDialog")) %></h3>
                </div>

                <div class="modal-body">
                    <div class="form-horizontal">
                        <asp:UpdatePanel runat="server" ID="UppDetail">
                            <ContentTemplate>
                                <div class="form-group">
                                    <div class="col-sm-12 text-left">
                                        <h4>
                                            <label id="Label1" runat="server" class="control-label"><%= GetLocalResourceObject("TitleVacationDetailDialog") %></label>
                                        </h4>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-12 text-left" style="max-height: 400px; overflow-y: auto; overflow-x: auto;">
                                        <asp:GridView ID="grvDetail"
                                            Width="100%"
                                            runat="server"
                                            EmptyDataText='<%$ Code:GetLocalResourceObject("lblNoData") %>' EmptyDataRowStyle-CssClass="emptyRow"
                                            AllowPaging="false" PagerSettings-Visible="false" AllowSorting="true"
                                            AutoGenerateColumns="false" ShowHeader="true"
                                            CssClass="table table-striped table-hover table-bordered"
                                            DataKeyNames="LaborCycle"
                                            OnPreRender="GrvDetail_PreRender" OnSorting="GrvDetail_Sorting">
                                            <Columns>

                                                <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                    <HeaderTemplate>
                                                        <div style="width: 100%; text-align: center;">
                                                            <asp:LinkButton ID="LaborCycleDateSort" runat="server" CommandName="Sort" CommandArgument="LaborCycle">          
                                                                 <span><%= GetLocalResourceObject("lblCycle") %></span>
                                                            </asp:LinkButton>
                                                        </div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span id="dvLaborCycle" data-id="LaborCycle" data-value="<%# Eval("LaborCycle") %>"><%# Eval("LaborCycle") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                    <HeaderTemplate>
                                                        <div style="width: 100%; text-align: center;">
                                                            <asp:LinkButton ID="CompanyCodeDateSort" runat="server" CommandName="Sort" CommandArgument="CompanyCode">          
                                                                <span><%= GetLocalResourceObject("lblCompany") %></span>
                                                            </asp:LinkButton>
                                                        </div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span id="dvCompanyCode" data-id="CompanyCode" data-value="<%# Eval("CompanyCode") %>"><%# Eval("CompanyCode") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                    <HeaderTemplate>
                                                        <div style="width: 100%; text-align: center;">
                                                            <asp:LinkButton ID="VacationsbyCycleDateSort" runat="server" CommandName="Sort" CommandArgument="VacationsbyCycle">
                                                                <span><%= GetLocalResourceObject("lblVacationsByCycle") %></span>
                                                            </asp:LinkButton>
                                                        </div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span id="dvVacationsbyCycle" data-id="VacationsbyCycle" data-value='<%# string.Format("{0:F2}", Eval("VacationPerCycle")) %>'>
                                                            <%# string.Format("{0:F2}", Eval("VacationPerCycle")) %>
                                                        </span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                    <HeaderTemplate>
                                                        <div style="width: 100%; text-align: center;">
                                                            <asp:LinkButton ID="EnjoyedVacationDateSort" runat="server" CommandName="Sort" CommandArgument="EnjoyedVacation">
                                                                <span><%= GetLocalResourceObject("lblEnjoyedVacation") %></span>
                                                            </asp:LinkButton>
                                                        </div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span id="dvEnjoyedVacation" data-id="EnjoyedVacation" data-value='<%# string.Format("{0:F2}", Eval("TakenVacations")) %>'>
                                                            <%# string.Format("{0:F2}", Eval("TakenVacations")) %>
                                                        </span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                    <HeaderTemplate>
                                                        <div style="width: 100%; text-align: center;">
                                                            <asp:LinkButton ID="SheduledVacationsDateSort" runat="server" CommandName="Sort" CommandArgument="SheduledVacations">
                                                                <span><%= GetLocalResourceObject("lblSheduledVacations") %></span>
                                                            </asp:LinkButton>
                                                        </div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span id="dvSheduledVacations" data-id="SheduledVacations" data-value='<%# string.Format("{0:F2}", Eval("ScheduledVacations")) %>'>
                                                            <%# string.Format("{0:F2}", Eval("ScheduledVacations")) %>
                                                        </span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                    <HeaderTemplate>
                                                        <div style="width: 100%; text-align: center;">
                                                            <asp:LinkButton ID="BalancePeriodDateSort" runat="server" CommandName="Sort" CommandArgument="BalancePeriod">
                                                                <span><%= GetLocalResourceObject("lblBalancePeriod") %></span>
                                                            </asp:LinkButton>
                                                        </div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span id="dvBalancePeriod" data-id="BalancePeriod" data-value='<%# string.Format("{0:F2}", Eval("BalancePeriod")) %>'>
                                                            <%# string.Format("{0:F2}", Eval("BalancePeriod")) %>
                                                        </span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>

                <div class="modal-footer">
                    <button id="btnVacationDetailAccept" type="button" class="btn btn-default">
                        <span class="glyphicon glyphicon-ok glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%= Convert.ToString(GetLocalResourceObject("btnAccept")) %>
                    </button>
                </div>

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

        //*******************************//
        //          VALIDATION           // 
        //*******************************//
        var validator = null;
        function ValidateForm() {
            /// <summary>Validate the form with jquey validation plugin </summary>
            /// <returns> True if form is valid. False otherwise. </returns>
            //add custom validation methods
            //add custom validation methods
            jQuery.validator.addMethod("validDate", function (value, element) {
                return this.optional(element) || moment(value, "MM/DD/YYYY").isValid();
            }, "Please enter a valid date in the format MM/DD/YYYY");

            if (validator != null) {
                validator.destroy();
            }


            var option = $("#<%=chbTypeRequest.ClientID%>").is(":checked");
            if (option) {
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
                  "<%= dtpStartDate.UniqueID %>": {
                            required: true,
                            validDate: true
                        },
                    "<%= dtpEndDate.UniqueID %>": {
                            required: true,
                            validDate: true
                        },
                    }
                });
            }
            else {
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
                 "<%= dtpSelectDay.UniqueID %>": {
                            required: true,
                            validDate: true
                        }
                    }
                });
            }



            //get the results
            var result = validator.form();
            return result;
        }

        //*******************************//
        //       EVENT BINDING           //
        //*******************************//
        function pageLoad(sender, args) {

            $('#VacationRequestDialog').on('keyup keypress', '.enterkey', function (e) {
                var keyCode = e.keyCode || e.which;
                if (keyCode === 13) {
                    e.preventDefault();
                    return false;
                }
            });

            //In this section we set the button generics functionality
            $('#btnCancel, #btnVacationRequestClose').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                event.preventDefault();

                $('#VacationRequestDialog').modal('hide');
                //EnableButtonsDialog();
            });

            $('#btnVacationHistoryClose, #btnVacationHistoryAccept').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                event.preventDefault();

                $('#VacationHistoryDialog').modal('hide');
                //EnableButtonsDialog();
            });

            $('#btnVacationDetailClose, #btnVacationDetailAccept').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                event.preventDefault();

                $('#VacationDetail').modal('hide');
                //EnableButtonsDialog();
            });



            $('#<%= dtpStartDate.ClientID %>').datetimepicker({
                format: 'MM/DD/YYYY'

            });

            $('#<%= dtpEndDate.ClientID %>').datetimepicker({
                format: 'MM/DD/YYYY'
            });

            $('#<%= dtpSelectDay.ClientID %>').datetimepicker({
                format: 'MM/DD/YYYY'
            });

            $('#<%= dtpStartDate.ClientID %>').on("blur", function () {
                var ValidDateVal = $(this).data("DateTimePicker").date().format('MM/DD/YYYY');
                $(this).val(ValidDateVal);
            });

            $('#<%= chbTypeRequest.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });

            $("#<%=chbTypeRequest.ClientID%>").change(function () {
                __doPostBack('<%= chbTypeRequest.ClientID %>', '');

            });

            $('#<%= txtNameEmployee.ClientID %>').on('keyup keypress', function (e) {
                // ReturnFromBtnSearchClickPostBack(e);

                return isNumberOrLetter(e);
            });

            $('#<%= txtNameEmployeeSearch.ClientID %>').on('keyup keypress', function (e) {
                // ReturnFromBtnSearchClickPostBack(e);

                return isNumberOrLetter(e);
            });
            $('#<%= txtEmployeeCode.ClientID %>').on('keyup keypress', function (e) {
                // ReturnFromBtnSearchClickPostBack(e);

                return isNumberOrLetter(e);
            });

            //In this section we set the button state for btnAjaxAction
            $('.btnAjaxAction').on('click', function () {
                var $this = $(this);
                $this.button('loading');
                setTimeout(function () { $this.button('reset'); }, 50000);
            });

            //In this section we set the grvList functionality
            $('#<%= grvList.ClientID %>').on('click', 'tbody tr', function (event) {
                if (!$(this).hasClass('emptyRow')) {
                    $(this).toggleClass('info').siblings().removeClass('info');
                    $('#<%=hdfSelectedRowIndex.ClientID%>').val($(this).hasClass("info") ? $(this).index() : -1);
                }
            });


            // GridView functionality for grvHistory
            $('#<%= grvHistory.ClientID %>').on('click', 'tbody tr', function (event) {
                if (!$(this).hasClass('emptyRow')) {
                    $(this).toggleClass('info').siblings().removeClass('info');
                    $('#<%= hdfSelectedRowIndex.ClientID %>').val($(this).hasClass("info") ? $(this).index() : -1);
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


            //In this section we set the others controls functionality 
            $('#<%= btnRequest.ClientID %>').on('click', function (event) {
                /// <summary>Handles the click event for button add.</summary>
                event.preventDefault();


             <%--   setTimeout(function () {
                    $("#<%=btnRequest.ClientID%>").button('reset');
                }, 500);--%>
                EnableToolBar();

                return false;
            });

            //In this section we set the others controls functionality 
            $('#<%= btnBalanceDetails.ClientID %>').on('click', function (event) {
                /// <summary>Handles the click event for button add.</summary>
                event.preventDefault();


             <%--   setTimeout(function () {
                    $("#<%=btnRequest.ClientID%>").button('reset');
                }, 500);--%>
                EnableToolBar();

                return false;
            });


            $('#<%= cboCenterCostFilter.ClientID %>').on('keyup keypress', function (e) {
                //ReturnFromBtnSearchClickPostBack(e);
            });

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
        //  RETURN FROM POSTBACKS LOGIC  //
        //*******************************//

        function ReturnFromBtnRequestPostBack() {
            $("#<%=txtCodeEmployee.ClientID%>").attr("disabled", "disabled");
            $("#<%=txtNameEmployee.ClientID%>").attr("disabled", "disabled");
            $('#<%=chbTypeRequest.ClientID%>').bootstrapToggle('<%= GetLocalResourceObject("Yes") %>');
            $("#<%=chbTypeRequest.ClientID%>").bootstrapToggle('on');
            ShowModalPanelForMandatory();
            $('#VacationRequestDialog').modal('show');
        }

        function ReturnFromBtnHistoryPostBack() {
            $('#VacationHistoryDialog').modal('show');

        }

        function ReturnFromBtnDetailPostBack() {
            $('#VacationDetail').modal('show');

        }

        function ReturnFromBtnValidatePostBack() {
            ShowModalPanelForMandatory();
            $("#<%=txtCodeEmployee.ClientID%>").attr("disabled", "disabled");
            $("#<%=txtNameEmployee.ClientID%>").attr("disabled", "disabled");
            HidenSeatSuperior();
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

        function EnableToolBar() {
            ///<summary>Enabled all buttons toolbar</summary>
            enableButton($('#<%= btnRequest.ClientID %>'));
        }
        function ShowModalPanelForMandatory() {
            var isChecked = $("#<%=chbTypeRequest.ClientID%>").is(":checked");

            if (isChecked) {
                $(".ismandatory").show();
                $(".isNotmandatory").hide();
            } else {
                $(".ismandatory").hide();
                $(".isNotmandatory").show();
            }

        }

        function SelectInternalEmployee(btnId) {
                                <%--  $("#<%=txtInternalEmployeeRequestDate.ClientID%>").val($("#" + btnId).siblings(".EmployeeCode").val());
//$("#<%=txtInternalEmployeeTrainerName.ClientID%>").val($("#" + btnId).siblings(".EmployeeName").val());
//$("#<%=txtInternalEmployeeTrainerTelephone.ClientID%>").focus();
--%>
            setTimeout(function () { $("#" + btnId).button('reset'); }, 100);
        }


        function ProcessExternalsAcceptRequest(resetId) {
<%--            /// <summary>Process the accept request according to the validation of row selected</summary>
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

            return false;--%>

            if (IsRowSelected()) {
                __doPostBack('<%= btnAccept.UniqueID %>', ''); // Corrected this line
                return true;
            } else {
                ShowFooterError('<%= GetLocalResourceObject("msgInvalidSelection") %>');
                ErrorButton(resetId);
                return false;
            }
        }

        function ProcesRequestVacaction(resetId) {
            if (IsRowSelected()) {
                __doPostBack('<%= btnRequest.UniqueID %>', ''); // Corrected this line
                return true;
            } else {
                ShowFooterError('<%= GetLocalResourceObject("msgInvalidSelection") %>');
                ErrorButton(resetId);
                return false;
            }
        }

        function ProcessBalanceDetail(resetId) {
            /// <summary>Process the edit request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            if (IsRowSelected()) {
                __doPostBack('<%= btnBalanceDetails.UniqueID %>', ''); // Corrected this line
                return true;
            } else {
                ShowFooterError('<%= GetLocalResourceObject("msgInvalidSelection") %>');
                ErrorButton(resetId);
                return false;
            }
        }

        function ProcesHistory(resetId) {
            if (IsRowSelected()) {
                __doPostBack('<%= btnHistory.UniqueID %>', ''); // Corrected this line
                return true;
            } else {
                ShowFooterError('<%= GetLocalResourceObject("msgInvalidSelection") %>');
                ErrorButton(resetId);
                return false;
            }
        }

                
        function HidenSeatSuperior() {
            $(".isPersonHelper").hide();
        }

         function ShowSeatSuperior() {
            $(".isPersonHelper").show();
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
                SetWaitingGrvList(false);
            }
        }


    </script>
</asp:Content>
