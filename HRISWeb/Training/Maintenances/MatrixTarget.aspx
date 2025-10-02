<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="MatrixTarget.aspx.cs" Inherits="HRISWeb.Training.Maintenances.MatrixTarget" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntHeader" runat="server">
    <title><%=Convert.ToString(GetLocalResourceObject("lblScreenTitle")) %></title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cntBody" runat="server">
    <style>
        .bootstrap-select > .dropdown-toggle.bs-placeholder, .bootstrap-select > .dropdown-toggle.bs-placeholder:active,
        .bootstrap-select > .dropdown-toggle.bs-placeholder:focus, .bootstrap-select > .dropdown-toggle.bs-placeholder:hover {
            color: #333;
        }

        .redcolor {
            color: red;
        }

        .greencolor {
            color: forestgreen;
        }
    </style>

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
                            <div class="col-md-12">
                                <div class="form-horizontal">
                                    <div class="row">
                                        <div class="form-group col-sm-12 col-md-6">
                                            <div class="col-sm-5 text-left">
                                                <label for="<%= MatrixTargetCodeFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("CodeLbl")%></label>
                                            </div>

                                            <div class="col-sm-7">
                                                <input id="MatrixTargetCodeFilter" type="text" class="form-control" runat="server" data-id="MatrixTargetCode" data-value="isPermitted" />
                                            </div>
                                        </div>

                                        <div class="form-group col-sm-12 col-md-6">
                                            <div class="col-sm-5 text-left">
                                                <label for="<%= MatrixTargetNameFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("NameLbl")%></label>
                                            </div>

                                            <div class="col-sm-7">
                                                <input id="MatrixTargetNameFilter" type="text" class="form-control" runat="server" data-id="MatrixTargetName" data-value="isPermitted" />
                                            </div>
                                        </div>

                                        <div class="form-group col-sm-12 col-md-6">
                                            <div class="col-sm-5 text-left">
                                                <label for="<%= StructByFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblStructByEdit")%></label>
                                            </div>

                                            <div class="col-sm-7">
                                                <select class="form-control selectpicker" runat="server" id="StructByFilter" data-live-search="true"></select>
                                            </div>
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
                                        DataKeyNames="MatrixTargetId"
                                        OnPreRender="GrvList_PreRender" OnSorting="GrvList_Sorting">
                                        <Columns>
                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="MatrixTargetCodeSort" runat="server" CommandName="Sort" CommandArgument="MatrixTargetCode" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("CodeLbl") %></span>&nbsp;
                                                            <i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "MatrixTargetCode") %> sorterDirection' 
                                                                aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvMatrixTargetCode" data-id="MatrixTargetCode" data-value="<%# Eval("MatrixTargetCode") %>"><%# Eval("MatrixTargetCode") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="MatrixTargetNameSort" runat="server" CommandName="Sort" CommandArgument="MatrixTargetName" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("NameLbl") %></span>&nbsp;
                                                            <i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "MatrixTargetName") %> sorterDirection'
                                                                 aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvMatrixTargetName" data-id="MatrixTargetName" data-value="<%# Eval("MatrixTargetName") %>"><%# Eval("MatrixTargetName") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="DivisionNameSort" runat="server" CommandName="Sort" CommandArgument="DivisionName" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("DivisionLbl") %></span>&nbsp;
                                                            <i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "DivisionName") %> sorterDirection'
                                                                 aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvDivisionName" data-id="DivisionName" data-value="<%# Eval("DivisionName") %>"><%# Eval("DivisionName") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="StructBySort" runat="server" CommandName="Sort" CommandArgument="StructBy" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("lblStructByEdit") %></span>&nbsp;
                                                            <i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "StructBy") %> sorterDirection'
                                                                 aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvStructBy" data-id="StructBy" data-value="<%# Eval("StructBy").ToString().Equals("1") ? GetLocalResourceObject("StructByFarm").ToString() : GetLocalResourceObject("StructByNominalClass").ToString() %>"><%# Eval("StructBy").ToString().Equals("1") ? GetLocalResourceObject("StructByFarm").ToString() : GetLocalResourceObject("StructByNominalClass").ToString() %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="SearchEnabledSort" runat="server" CommandName="Sort" CommandArgument="SearchEnabled" OnClientClick="SetWaitingGrvList(true);">      
                                                        <span>Regional</span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "IsRegional") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvSearchEnabled" data-id="SearchEnabled" aria-hidden="true" data-value="<%# Eval("IsRegional") %>" class="fa <%# ((DOLE.HRIS.Shared.Entity.MatrixTargetEntity)Container.DataItem).IsRegional ? "fa-check-circle greencolor" : "fa-times-circle redcolor" %> "></span>
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
                            onserverclick="BtnAdd_ServerClick" onclick="return ProcessAddRequest(this.id);"
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
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <%--  Modal for Add and Edit MapPosistions Carrer--%>
    <div class="modal fade" id="MaintenanceDialog" tabindex="-1" role="dialog" aria-labelledby="MaintenanceDialogTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog " style="width: 90% !important;" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" id="btnClose" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title text-primary" id="dialogTitle"></h3>
                </div>

                <asp:UpdatePanel runat="server" ID="uppDialogControl">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="form-horizontal">
                                <asp:HiddenField ID="hdfMatrixTargetIdEdit" runat="server" Value="" />

                                <div class="form-group">
                                    <div class="col-sm-2 text-left">
                                        <label for="<%=MatrixTargetCodeEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("CodeLbl")%></label>
                                    </div>

                                    <div class="col-sm-10">
                                        <input type="text" id="MatrixTargetCodeEdit" runat="server" class="form-control limpiarCampos cleanPasteText ValidacionRegional" onkeypress="return isNumberOrLetter(event);" onkeyup="return isNumberOrLetter(event);" maxlength="10" value="" required />
                                        <label id="MatrixTargetCodeEditValidation" for="<%= MatrixTargetCodeEdit.ClientID%>" class="label label-danger label-validation"
                                            data-toggle="tooltip" data-container="body" data-placement="left"
                                            data-content="<%= GetLocalResourceObject("msjMatrixTargetCodeEditValidation") %>"
                                            style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">
                                            !</label>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-2 text-left">
                                        <label for="<%=MatrixTargetNameEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblEmployeeNameHeader")%></label>
                                    </div>

                                    <div class="col-sm-10">
                                        <input type="text" id="MatrixTargetNameEdit" runat="server" class="form-control limpiarCampos cleanPasteText ValidacionRegional" onkeypress="return isNumberOrLetter(event);" onkeyup="return isNumberOrLetter(event);" maxlength="500" value="" required />
                                        <label id="MatrixTargetNameEditValidation" for="<%= MatrixTargetNameEdit.ClientID%>" class="label label-danger label-validation"
                                            data-toggle="tooltip" data-container="body" data-placement="left"
                                            data-content="<%= GetLocalResourceObject("msjMatrixTargetNameEditValidation") %>"
                                            style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">
                                            !</label>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-2 text-left">
                                        <label for="<%=DivisionCodeEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblDivisionCodeEdit")%></label>
                                    </div>

                                    <div class="col-sm-10">
                                        <button type="button" id="btnDivisionCodeEdit" runat="server" style="display: none;"
                                            onserverclick="BtnDivisionCodeEdit_ServerClick">
                                        </button>

                                        <select class="form-control selectpicker onChangeDivisionCode" runat="server" id="DivisionCodeEdit" multiple="true" data-live-search="true" data-actions-box="true" required="required"></select>

                                        <input type="hidden" runat="server" id="DivisionCodeEditMultiple" value="" />
                                        
                                        <label id="DivisionCodeEditValidation" for="<%= DivisionCodeEdit.ClientID%>" class="label label-danger label-validation"
                                            data-toggle="tooltip" data-container="body" data-placement="left"
                                            data-content="<%= GetLocalResourceObject("msjDivisionCodeEditValidation") %>"
                                            style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">
                                            !</label>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-2 text-left">
                                        <label for="<%=StructByEdit.ClientID%>" class="control-label" style="text-align: left"><%=GetLocalResourceObject("lblStructByEdit")%></label>
                                    </div>

                                    <div class="col-sm-10">
                                        <button type="button" id="btnStructByEdit" runat="server" style="display: none;"
                                            onserverclick="BtnStructByEdit_ServerClick">
                                        </button>

                                        <select class="form-control onChangeStruct" runat="server" id="StructByEdit" multiple="false" data-live-search="true" data-actions-box="true" required="required"></select>

                                        <label id="StructByEditValidation" for="<%= StructByEdit.ClientID%>" class="label label-danger label-validation"
                                            data-toggle="tooltip" data-container="body" data-placement="left"
                                            data-content="<%= GetLocalResourceObject("msjStructByEditValidation") %>"
                                            style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">
                                            !</label>
                                    </div>
                                </div>

                                <div class="form-group structByFarm">
                                    <div class="col-sm-2 text-left">
                                        <label for="<%=CostZoneIdEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCostZoneIdEdit")%></label>
                                    </div>

                                    <div class="col-sm-10">
                                        <button type="button" id="btnCostZoneIdEdit" runat="server" style="display: none;"
                                            onserverclick="BtnCostZoneIdEdit_ServerClick">
                                        </button>

                                        <select class="form-control selectpicker onChangeCostZoneId" runat="server" id="CostZoneIdEdit" multiple="true" data-live-search="true" data-actions-box="true" required="required"></select>

                                        <input type="hidden" runat="server" class="limpiarCampos" id="CostZoneIdEditMultiple" value="" required="required" />
                                        <label id="CostZoneIdEditValidation" for="<%= CostZoneIdEdit.ClientID%>" class="label label-danger label-validation"
                                            data-toggle="tooltip" data-container="body" data-placement="left"
                                            data-content="<%= GetLocalResourceObject("msjCostZoneIdEditValidation") %>"
                                            style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">
                                            !</label>
                                    </div>
                                </div>

                                <div class="form-group structByFarm">
                                    <div class="col-sm-2 text-left">
                                        <label for="<%=CostMiniZoneIdEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCostMiniZoneIdEdit")%></label>
                                    </div>

                                    <div class="col-sm-10">
                                        <button type="button" id="btnCostMiniZoneIdEdit" runat="server" style="display: none;"
                                            onserverclick="BtnCostMiniZoneIdEdit_ServerClick">
                                        </button>

                                        <select class="form-control selectpicker onChangeCostMiniZoneId" runat="server" id="CostMiniZoneIdEdit" multiple="true" data-live-search="true" data-actions-box="true" required="required"></select>

                                        <input type="hidden" runat="server" class="limpiarCampos" id="CostMiniZoneIdEditMultiple" value="" required="required" />
                                        <label id="CostMiniZoneIdEditValidation" for="<%= CostMiniZoneIdEdit.ClientID%>" class="label label-danger label-validation"
                                            data-toggle="tooltip" data-container="body" data-placement="left"
                                            data-content="<%= GetLocalResourceObject("msjCostMiniZoneIdEditEditValidation") %>"
                                            style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">
                                            !</label>
                                    </div>
                                </div>

                                <div class="form-group structByFarm">
                                    <div class="col-sm-2 text-left">
                                        <label for="<%=CostFarmsIdEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCostFarmsIdEdit")%></label>
                                    </div>

                                    <div class="col-sm-10">
                                        <select class="form-control selectpicker onChangeCostFarmId" runat="server" id="CostFarmsIdEdit" multiple="true" data-live-search="true" data-actions-box="true" required="required"></select>

                                        <input type="hidden" runat="server" class="limpiarCampos" id="CostFarmsIdEditMultiple" value="" required="required" />
                                        <label id="CostFarmsIdEditValidation" for="<%= CostFarmsIdEdit.ClientID%>" class="label label-danger label-validation"
                                            data-toggle="tooltip" data-container="body" data-placement="left"
                                            data-content="<%= GetLocalResourceObject("msjCostFarmsIdEditValidation") %>"
                                            style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">
                                            !</label>
                                    </div>
                                </div>

                                <div class="form-group structByNominalClass">
                                    <div class="col-sm-2 text-left">
                                        <label for="<%=CompanyIdEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCompanyIdEdit")%></label>
                                    </div>

                                    <div class="col-sm-10">
                                        <button type="button" id="btnCompanyIdEdit" runat="server" style="display: none;"
                                            onserverclick="BtnCompanyIdEdit_ServerClick">
                                        </button>

                                        <select class="form-control selectpicker onChangeCompanies" runat="server" id="CompanyIdEdit" multiple="true" data-live-search="true" data-actions-box="true" required="required"></select>

                                        <input type="hidden" runat="server" class="limpiarCampos" id="CompanyIdEditMultiple" value="" required="required" />
                                        <label id="CompanyIdEditValidation" for="<%= CompanyIdEdit.ClientID%>" class="label label-danger label-validation"
                                            data-toggle="tooltip" data-container="body" data-placement="left"
                                            data-content="<%= GetLocalResourceObject("msjCompanyIdEditValidation") %>"
                                            style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">
                                            !</label>
                                    </div>
                                </div>

                                <div class="form-group structByNominalClass">
                                    <div class="col-sm-2 text-left">
                                        <label for="<%=NominalClassIdEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblNominalClassIdEdit")%></label>
                                    </div>

                                    <div class="col-sm-10">
                                        <select class="form-control selectpicker onChangeNominalClass" runat="server" id="NominalClassIdEdit" multiple="true" data-live-search="true" data-actions-box="true" required="required"></select>

                                        <input type="hidden" runat="server" class="limpiarCampos" id="NominalClassIdEditMultiple" value="" required="required" />
                                        <label id="NominalClassIdEditValidation" for="<%= NominalClassIdEdit.ClientID%>" class="label label-danger label-validation"
                                            data-toggle="tooltip" data-container="body" data-placement="left"
                                            data-content="<%= GetLocalResourceObject("msjNominalClassIdEditValidation") %>"
                                            style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">
                                            !</label>
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
                            <button id="btnAccept" type="button" runat="server" class="btn btn-primary btnAjaxAction ValidacionRegional"
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
                                <div id="divDuplicatedDialogText" runat="server">
                                </div>

                                <asp:Panel ID="pnlDuplicatedDialogDataDetail" runat="server">
                                    <div class="form-group">
                                        <div class="col-sm-4 text-left">
                                            <label for="<%= txtDuplicatedMatrixtargetCode.ClientID%>" class="control-label"><%=GetLocalResourceObject("CodeLbl")%></label>
                                        </div>

                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtDuplicatedMatrixtargetCode" CssClass="form-control cleanPasteText ignoreValidation" runat="server" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-4 text-left">
                                            <label for="<%= txtDuplicatedMatrixtargetName.ClientID%>" class="control-label"><%=GetLocalResourceObject("NameLbl")%></label>
                                        </div>

                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtDuplicatedMatrixtargetName" CssClass="form-control cleanPasteText ignoreValidation" runat="server" ReadOnly="true"></asp:TextBox>
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
            <div class="alert alert-autocloseable-msg" style="display: none;"></div>
        </div>
    </nav>

    <script type="text/javascript">
        //*******************************//
        //          VARIABLES            // 
        //*******************************//
        var dataSortAttribute, dataSortType, dataSortDirection;

        var DivisionCodeGlobal =<%=DivisionCodeGlobal%>;

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

            SelectedStructBy();
            //////////////////////////////////////////////////////////////////////////////////////////////////

            //In this section we initialize the checkbox toogles
            $('#<%= SearchEnabledEdit.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });
            //////////////////////////////////////////////////////////////////////////////////////////////////

            $('.selectpicker').selectpicker();
            $(".selectpicker2").selectpicker();
            $(".selectpicker1").selectpicker();
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
            $('#<%= MatrixTargetCodeFilter.ClientID %>').on('keyup keypress', function (e) {
                ReturnFromBtnSearchClickPostBack(e);

                return isNumberOrLetter(e);
            });

            $('#<%= MatrixTargetNameFilter.ClientID %>').on('keyup keypress', function (e) {
                ReturnFromBtnSearchClickPostBack(e);

                return isNumberOrLetter(e);
            });

            var current = null;
            $(".onChangeDivisionCode").on("changed.bs.select", function (e, clickedIndex, isSelected, previousValue) {
                MultiSelectDropdownListSaveSelectedItems($("#<%=DivisionCodeEdit.ClientID %>"), $("#<%=DivisionCodeEditMultiple.ClientID%>"));

                if (current != previousValue) {
                    current = previousValue;
                    if ($("#<%=DivisionCodeEditMultiple.ClientID%>").val() == '') 
                        $("#<%=DivisionCodeEditMultiple.ClientID %>").val('0');
                    __doPostBack('<%= btnDivisionCodeEdit.UniqueID %>', '');
                }
            });

            $(".onChangeDivisionCode .bs-actionsbox .bs-select-all").on("click", function () {
                MultiSelectDropdownListSaveSelectedItems($("#<%=CostZoneIdEdit.ClientID %>"), $("#<%=CostZoneIdEditMultiple.ClientID%>"));

                setTimeout(function () { __doPostBack('<%= btnCostZoneIdEdit.UniqueID %>', ''); }, 1000);
            });

            //In this section we set the  controls functionality for search advanced
            $(".onChangeStruct").change(function () {
                SelectedStructBy();

                $("#<%=CostZoneIdEditMultiple.ClientID %>").val('');
                $("#<%=CompanyIdEditMultiple.ClientID %>").val('');

                ClearStructBy();

                __doPostBack('<%= btnStructByEdit.UniqueID %>', '');
            });

            $(".onChangeCostZoneId").on("changed.bs.select", function (e, clickedIndex, isSelected, previousValue) {
                MultiSelectDropdownListSaveSelectedItems($("#<%=CostZoneIdEdit.ClientID %>"), $("#<%=CostZoneIdEditMultiple.ClientID%>"));

                if (current != previousValue) {
                    current = previousValue;

                    if ($("#<%=CostZoneIdEditMultiple.ClientID%>").val() == '') 
                        $("#<%=CostZoneIdEditMultiple.ClientID %>").val('0');

                    setTimeout(function () { __doPostBack('<%= btnCostZoneIdEdit.UniqueID %>', ''); }, 1000);
                }
            });

            $(".onChangeCostZoneId .bs-actionsbox .bs-select-all").on("click", function () {
                MultiSelectDropdownListSaveSelectedItems($("#<%=CostZoneIdEdit.ClientID %>"), $("#<%=CostZoneIdEditMultiple.ClientID%>"));

                setTimeout(function () { __doPostBack('<%= btnCostZoneIdEdit.UniqueID %>', ''); }, 1000);
            });

            $(".onChangeCostMiniZoneId").on("changed.bs.select", function (e, clickedIndex, isSelected, previousValue) {
                MultiSelectDropdownListSaveSelectedItems($("#<%=CostMiniZoneIdEdit.ClientID %>"), $("#<%=CostMiniZoneIdEditMultiple.ClientID%>"));

                if (current != previousValue) {
                    current = previousValue;

                    if ($("#<%=CostMiniZoneIdEditMultiple.ClientID%>").val() == '') 
                        $("#<%=CostMiniZoneIdEditMultiple.ClientID %>").val('0');

                    setTimeout(function () { __doPostBack('<%= btnCostMiniZoneIdEdit.UniqueID %>', ''); }, 1000);
                }
            });

            $(".onChangeCostMiniZoneId .bs-actionsbox .bs-select-all").on("click", function () {
                MultiSelectDropdownListSaveSelectedItems($("#<%=CostMiniZoneIdEdit.ClientID %>"), $("#<%=CostMiniZoneIdEditMultiple.ClientID%>"));

                setTimeout(function () { __doPostBack('<%= btnCostMiniZoneIdEdit.UniqueID %>', ''); }, 1000);
            });

            $(".onChangeCostFarmId").on("changed.bs.select", function (e, clickedIndex, isSelected, previousValue) {
                MultiSelectDropdownListSaveSelectedItems($("#<%=CostFarmsIdEdit.ClientID %>"), $("#<%=CostFarmsIdEditMultiple.ClientID%>"));

                if (current != previousValue) {
                    current = previousValue;

                    if ($("#<%=CostFarmsIdEditMultiple.ClientID%>").val() == '') 
                        $("#<%=CostFarmsIdEditMultiple.ClientID %>").val('0');

                    setTimeout(function () { __doPostBack('<%= CostFarmsIdEdit.UniqueID %>', ''); }, 1000);
                }
            });

            $(".onChangeCostFarmId .bs-actionsbox .bs-select-all").on("click", function () {
                MultiSelectDropdownListSaveSelectedItems($("#<%=CostFarmsIdEdit.ClientID %>"), $("#<%=CostFarmsIdEditMultiple.ClientID%>"));
            });

            $(".onChangeCompanies").on("changed.bs.select", function (e, clickedIndex, isSelected, previousValue) {
                MultiSelectDropdownListSaveSelectedItems($("#<%=CompanyIdEdit.ClientID %>"), $("#<%=CompanyIdEditMultiple.ClientID%>"));

                if (current != previousValue) {
                    current = previousValue;

                    if ($("#<%=CompanyIdEditMultiple.ClientID%>").val() == '') 
                        $("#<%=CompanyIdEditMultiple.ClientID %>").val('0');

                    setTimeout(function () { __doPostBack('<%= btnCompanyIdEdit.UniqueID %>', ''); }, 1000);
                }
            });

            $(".onChangeCompanies .bs-actionsbox .bs-select-all").on("click", function () {
                MultiSelectDropdownListSaveSelectedItems($("#<%=CompanyIdEdit.ClientID %>"), $("#<%=CompanyIdEditMultiple.ClientID%>"));

                setTimeout(function () { __doPostBack('<%= btnCompanyIdEdit.UniqueID %>', ''); }, 1000);
            });

            $(".onChangeNominalClass").on("changed.bs.select", function (e, clickedIndex, isSelected, previousValue) {
                MultiSelectDropdownListSaveSelectedItems($("#<%=NominalClassIdEdit.ClientID %>"), $("#<%=NominalClassIdEditMultiple.ClientID%>"));

                if (current != previousValue) {
                    current = previousValue;

                    if ($("#<%=NominalClassIdEditMultiple.ClientID%>").val() == '') 
                        $("#<%=NominalClassIdEditMultiple.ClientID %>").val('0');

                    setTimeout(function () { __doPostBack('<%= NominalClassIdEdit.UniqueID %>', ''); }, 1000);
                }
            });

            $(".onChangeNominalClass .bs-actionsbox .bs-select-all").on("click", function () {
                MultiSelectDropdownListSaveSelectedItems($("#<%=NominalClassIdEdit.ClientID %>"), $("#<%=NominalClassIdEditMultiple.ClientID%>"));
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

            $(".trim").change(function () {
                var text = $.trim($(this).val());
                $(this).val(text);
            });

            SetRowSelected();
        }

        //*******************************//
        //          VALIDATION           // 
        //*******************************//
        var validator = null;
        function ValidateForm() {
            jQuery.validator.addMethod("validSelection", function (value, element) {
                return this.optional(element) || value != "-1";
            }, "Please select a valid option");

            jQuery.validator.addMethod("divisionCode", function (value, element) {
                var divisions = $("#<%=DivisionCodeEdit.ClientID %>").selectpicker('val').length > 0;
                if (divisions) {
                    return true;
                }

                return false;
            }, "<%=GetLocalResourceObject("msgInputValid").ToString()%>");

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
                    "<%= MatrixTargetCodeEdit.UniqueID%>": {
                        required: true,
                        normalizer: function (value) {
                            return $.trim(value);
                        },
                        minlength: 1,
                        maxlength: 500
                    },
                    "<%= MatrixTargetNameEdit.UniqueID%>": {
                        required: true,
                        normalizer: function (value) {
                            return $.trim(value);
                        },
                        minlength: 1,
                        maxlength: 500
                    },
                    "<%= DivisionCodeEdit.UniqueID%>": {
                        required: true,
                        divisionCode: true
                    }
                }
            });

            var result = validator.form();
            return result;
        }

        function ValidateFormStruct() {
            jQuery.validator.addMethod("validSelection", function (value, element) {
                return this.optional(element) || value != "-1";
            }, "Please select a valid option");

            jQuery.validator.addMethod("structByFarm", function (value, element) {
                var structBy = $("#<%=StructByEdit.ClientID%>").val();

                if (structBy == "1") {
                    var costZone = $("#<%=CostZoneIdEdit.ClientID %>").selectpicker('val').length > 0;
                    if (costZone) {
                        return true;
                    }

                    var costMiniZone = $("#<%=CostMiniZoneIdEdit.ClientID %>").selectpicker('val').length > 0;
                    if (costMiniZone) {
                        return true;
                    }

                    var costFarm = $("#<%=CostFarmsIdEdit.ClientID %>").selectpicker('val').length > 0;
                    if (costFarm) {
                        return true;
                    }

                    return false;
                } else {
                    return true;
                }
            }, "<%=GetLocalResourceObject("msgInputValid").ToString()%>");

            jQuery.validator.addMethod("structByNominalClass", function (value, element) {
                var structBy = $("#<%=StructByEdit.ClientID%>").val();

                if (structBy == "2") {
                    var nominalClass = $("#<%=NominalClassIdEdit.ClientID %>").selectpicker('val').length > 0;
                    if (nominalClass) {
                        return true;
                    }

                    var company = $("#<%=CompanyIdEdit.ClientID %>").selectpicker('val').length > 0;
                    if (company) {
                        return true;
                    }

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
                   "<%= StructByEdit.UniqueID%>": {
                        required: true,
                        validSelection: true
                    },
                   "<%= CostZoneIdEdit.UniqueID%>": {
                        structByFarm: true
                    },
                   "<%= CostMiniZoneIdEdit.UniqueID%>": {
                        structByFarm: true
                    },
                    "<%= CostFarmsIdEdit.UniqueID%>": {
                        structByFarm: true
                    },
                    "<%= CompanyIdEdit.UniqueID%>": {
                        structByNominalClass: true
                    },
                    "<%= NominalClassIdEdit.UniqueID%>": {
                        structByNominalClass: true
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

        function SelectedStructBy() {
            if ($("#<%= StructByEdit.ClientID%>").val() == "1") {
                $(".structByFarm").show();
                $(".structByNominalClass").hide();
            }

            else {
                $(".structByFarm").hide();
                $(".structByNominalClass").show();
            }
        }

        function ClearModalForm() {
            $("#<%= StructByEdit.ClientID%>").val("1");
            $("#<%=hdfMatrixTargetIdEdit.ClientID%>").val("-1");

            $("#<%=MatrixTargetCodeEdit.ClientID%>").val("");
            $("#<%=MatrixTargetNameEdit.ClientID%>").val("");

            ClearStructBy();

            if (validator != null) {
                validator.resetForm();
            }
        }

        function ClearStructBy() {
            if ($("#<%= StructByEdit.ClientID%>").val() == "1") {
                $("#<%=CostZoneIdEdit.ClientID%>").html("");
                if ($("#<%=CostZoneIdEdit.ClientID %>").selectpicker("val").length > 0) {
                    $("#<%=CostZoneIdEdit.ClientID %>").selectpicker("val", "");
                }

                $("#<%=CostZoneIdEditMultiple.ClientID %>").val('');
                ////////////////////////////////////////////////////////////////////

                $("#<%=CostMiniZoneIdEdit.ClientID%>").html("");
                if ($("#<%=CostMiniZoneIdEdit.ClientID %>").selectpicker("val").length > 0) {
                    $("#<%=CostMiniZoneIdEdit.ClientID %>").selectpicker("val", "");
                }

                $("#<%=CostMiniZoneIdEditMultiple.ClientID %>").val('');
                ////////////////////////////////////////////////////////////////////

                $("#<%=CostFarmsIdEdit.ClientID%>").html("");
                if ($("#<%=CostFarmsIdEdit.ClientID %>").selectpicker("val").length > 0) {
                    $("#<%=CostFarmsIdEdit.ClientID %>").selectpicker("val", "");
                }

                $("#<%=CostFarmsIdEditMultiple.ClientID %>").val('');
            }

            if ($("#<%= StructByEdit.ClientID%>").val() == "2") {
                $("#<%=CompanyIdEdit.ClientID%>").html("");
                if ($("#<%=CompanyIdEdit.ClientID %>").selectpicker("val").length > 0) {
                    $("#<%=CompanyIdEdit.ClientID %>").selectpicker("val", "");
                }

                $("#<%=CompanyIdEditMultiple.ClientID %>").val('');
                ////////////////////////////////////////////////////////////////////

                $("#<%=NominalClassIdEdit.ClientID%>").html("");
                if ($("#<%=NominalClassIdEdit.ClientID %>").selectpicker("val").length > 0) {
                    $("#<%=NominalClassIdEdit.ClientID %>").selectpicker("val", "");
                }

                $("#<%=NominalClassIdEditMultiple.ClientID %>").val('');
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

        //*******************************//
        //           PROCESS             //
        //*******************************//   
        var delayForAdvancedSearchEmployeesPostBack = null;
        function SetDelayForAdvancedSearchEmployeesPostBack() {
            /// <summary>Set a timer for delay the search employees post back while users writes</summary>
            if (delayForAdvancedSearchEmployeesPostBack != null) {
                clearTimeout(delayForAdvancedSearchEmployeesPostBack);
            }

            delayForAdvancedSearchEmployeesPostBack = setTimeout("AdvancedSearchEmployeesPostBack()", 1800);
        }

        function ProcessAddRequest(resetId) {
            ///// <summary>Handles the click event for button add.</summary>
            ClearModalForm();

            $(".ValidacionRegional").prop("disabled", false);

            $('#<%=MatrixTargetCodeEdit.ClientID %>').prop('disabled', false);
            $('#<%=SearchEnabledEdit.ClientID%>').bootstrapToggle('off');

            $("#<%=DivisionCodeEdit.ClientID %>").selectpicker("val", "" + DivisionCodeGlobal);
            $("#<%=DivisionCodeEditMultiple.ClientID %>").val(DivisionCodeGlobal);

            $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceAdd") %>');
            $('#dialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceAdd") %>');
            $('#MaintenanceDialog').modal('show');

            __doPostBack('<%= btnAdd.UniqueID %>', '');
            return true;
        }

        function ProcessEditRequest(resetId) {
            /// <summary>Process the edit request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            $(".ValidacionRegional").prop("disabled", false);

            if (IsRowSelected()) {
                $('#<%= MatrixTargetCodeEdit.ClientID %>').prop('disabled', true);
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

            if (!ValidateForm() && !ValidateFormStruct()) {
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

        function ProcessAdvancedSearchRequest(resetId) {
            var uniqueId = resetId.replace(/_/g, '$');

            if (!ValidateFormStruct()) {
                setTimeout(function () {
                    ResetButton(resetId);
                }, 150);
            }

            else {
                __doPostBack(uniqueId, '');
            }

            return false;
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

        function ReturnRequestBtnEditOpen(ValidacionRegional) {
            SetRowSelected();
            DisableToolBar();

            $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#dialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#MaintenanceDialog').modal('show');

            $('#<%= MatrixTargetCodeEdit.ClientID %>').prop('disabled', true);

            MultiSelectDropdownListRestoreSelectedItems($("#<%=DivisionCodeEdit.ClientID %>"), $("#<%=DivisionCodeEditMultiple.ClientID%>"));

            ReturnRefreshDropdownList();

            $('.selectpicker').selectpicker('render');

            SelectedStructBy();

            if (ValidacionRegional != 0) {
                $(".ValidacionRegional").prop("disabled", true);
            }

            EnableToolBar();
        }

        function ReturnPostBackAcceptClickSave() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            SetRowSelected();
            DisableButtonsDialog();

            SelectedStructBy();

            $('#MaintenanceDialog').modal('hide');
            ClearModalForm();

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

        function ReturnRefreshDropdownList() {
            MultiSelectDropdownListRestoreSelectedItems($("#<%=DivisionCodeEdit.ClientID %>"), $("#<%=DivisionCodeEditMultiple.ClientID%>"));

            MultiSelectDropdownListRestoreSelectedItems($("#<%=CostZoneIdEdit.ClientID %>"), $("#<%=CostZoneIdEditMultiple.ClientID%>"));
            MultiSelectDropdownListRestoreSelectedItems($("#<%=CostMiniZoneIdEdit.ClientID %>"), $("#<%=CostMiniZoneIdEditMultiple.ClientID%>"));
            MultiSelectDropdownListRestoreSelectedItems($("#<%=CostFarmsIdEdit.ClientID %>"), $("#<%=CostFarmsIdEditMultiple.ClientID%>"));

            MultiSelectDropdownListRestoreSelectedItems($("#<%=CompanyIdEdit.ClientID %>"), $("#<%=CompanyIdEditMultiple.ClientID%>"));
            MultiSelectDropdownListRestoreSelectedItems($("#<%=NominalClassIdEdit.ClientID %>"), $("#<%=NominalClassIdEditMultiple.ClientID%>"));
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
                '<%=GetLocalResourceObject("Yes")%>',
                function () {
                    __doPostBack('<%= btnDelete.UniqueID %>', '');
                },
                '<%=GetLocalResourceObject("No")%>',
                function () {
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
                if (!args.get_postBackElement().id.includes("IdEdit")) {
                    ShowFooterAlert('<%=GetLocalResourceObject("msgWaitWhileProcessing")%>');
                }

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
