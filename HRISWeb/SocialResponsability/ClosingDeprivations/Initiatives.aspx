<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Initiatives.aspx.cs" Inherits="HRISWeb.SocialResponsability.ClosingDeprivations.Initiatives" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntHeader" runat="server">
    <title><%=Convert.ToString(GetLocalResourceObject("lblScreenTitle")) %></title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cntBody" runat="server">
    <div class="main-content">
        <asp:Panel ID="pnlMainContent" runat="server">
            <h1 class="text-left text-primary">
                <label class="PageTitle"><%=Convert.ToString(GetLocalResourceObject("lblScreenTitle")) %></label>
            </h1>
            <br />

            <asp:UpdatePanel runat="server" ID="main">
                <ContentTemplate>
                    <div class="container" style="width: 100%">
                        <h4 class="text-left text-primary"><%= GetLocalResourceObject("lblFiltersSubtitle") %></h4>
                        <br />

                        <div class="row">
                            <div class="col-sm-4">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=cboDivision.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblDivision")%></label>
                                        </div>

                                        <div class="col-sm-7">
                                            <asp:DropDownList ID="cboDivision" CssClass="form-control control-validation selectpicker" AutoPostBack="true" runat="server" data-live-search="true" data-actions-box="true" data-id="Division" data-value="isPermitted" OnSelectedIndexChanged="cboDivision_SelectedIndexChanged"></asp:DropDownList>
                                            <asp:HiddenField ID="hdfDivisionValueFilter" runat="server" />
                                        </div>
                                    </div>

                                    <div class="form-horizontal">
                                        <div class="form-group structByFarm">
                                            <div class="col-sm-5 text-left">
                                                <label for="<%=CostFarmsIdEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCostFarmsIdEdit")%></label>
                                            </div>

                                            <div class="col-sm-7">
                                                <select class="form-control selectpicker onChangeCostFarmId" runat="server" id="CostFarmsIdEdit" multiple="true" data-live-search="true" data-actions-box="true" required="required"></select>

                                                <input type="hidden" runat="server" class="limpiarCampos" id="CostFarmsIdEditMultiple" value="" required="required" />
                                                <label id="CostFarmsIdEditValidation" for="<%= CostFarmsIdEdit.ClientID%>" class="label label-danger label-validation"
                                                    data-toggle="tooltip" data-container="body" data-placement="left"
                                                    data-content="<%= GetLocalResourceObject("msjCostFarmsIdEditValidation") %>"
                                                    style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">
                                                    !</label>
                                            </div>
                                        </div>                                        
                                    </div>

                                </div>
                            </div>

                            <div class="col-sm-4">
                                <div class="form-horizontal">

                                    <div class="form-group" style="display:none">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=cboCompanies.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCompany")%></label>
                                        </div>

                                        <div class="col-sm-7">
                                            <asp:DropDownList ID="cboCompanies" CssClass="form-control control-validation selectpicker" AutoPostBack="false" runat="server" data-live-search="true" data-actions-box="true" data-id="Companies" data-value="isPermitted"></asp:DropDownList>
                                            <asp:HiddenField ID="hdfCompaniesValueFilter" runat="server" />
                                        </div>
                                    </div>

                                    <div class="form-group structByFarm">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=CostZoneIdEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCostZoneIdEdit")%></label>
                                        </div>

                                        <div class="col-sm-7">
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
                                    
                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=cboIndicator.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblIndicator")%></label>
                                        </div>

                                        <div class="col-sm-7">
                                            <asp:DropDownList ID="cboIndicator" CssClass="form-control control-validation selectpicker" AutoPostBack="false" runat="server" data-live-search="true" data-actions-box="true" data-id="Indicator" data-value="isPermitted"></asp:DropDownList>
                                            <asp:HiddenField ID="hdfIndicatorValueFilter" runat="server" />
                                        </div>
                                    </div>

                                </div>
                            </div>

                            <div class="col-sm-4">
                                <div class="form-group"  style="display:none">
                                    <div class="col-sm-5 text-left">
                                        <label for="<%=cboFarm.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblFarm")%></label>
                                    </div>

                                    <div class="col-sm-7">
                                        <asp:DropDownList ID="cboFarm" CssClass="form-control control-validation selectpicker" AutoPostBack="false" runat="server" data-live-search="true" data-actions-box="true" data-id="Farm" data-value="isPermitted"></asp:DropDownList>
                                        <asp:HiddenField ID="hdfFarmValueFilter" runat="server" />
                                    </div>
                                </div>

                                <div class="form-group structByFarm">
                                    <div class="col-sm-5 text-left">
                                        <label for="<%=CostMiniZoneIdEdit.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCostMiniZoneIdEdit")%></label>
                                    </div>

                                    <div class="col-sm-7">
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
                    </div>


                    <div class="row">
                        <div class="col-sm-12 text-center">
                            <asp:HiddenField runat="server" ID="hdfSelectedRowIndex" Value="-1" />
                            <asp:HiddenField runat="server" ID="hdfSelectedFileIndex" Value="" />
                            <asp:HiddenField runat="server" ID="hdfSelectedFileValues" Value="" />
                            <asp:HiddenField runat="server" ID="hdfCurrentUser" Value="" />

                            <div>
                                <asp:GridView ID="grvList"
                                    Width="100%"
                                    runat="server"
                                    EmptyDataText='<%$ Code:GetLocalResourceObject("lblNoData") %>'
                                    EmptyDataRowStyle-CssClass="emptyRow"
                                    AllowPaging="false" PagerSettings-Visible="false" AllowSorting="true"
                                    AutoGenerateColumns="false" ShowHeader="true"
                                    CssClass="table table-striped table-hover table-bordered"
                                    DataKeyNames="InitiativeCode"
                                    OnRowCommand="grvList_RowCommand"
                                    OnPreRender="GrvList_PreRender" OnSorting="GrvList_Sorting">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle hidden" HeaderStyle-CssClass="hidden" FooterStyle-CssClass="hidden">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="InitiativeCodeSort" runat="server" CommandName="Sort" CommandArgument="InitiativeCode" OnClientClick="SetWaitingGrvList(true);">          
                                                        <span><%= GetLocalResourceObject("InitiativeCode.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "InitiativeCode") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvInitiativeCode" data-id="PrincipalInitiativecode" data-value="<%# Eval("InitiativeCode") %>"><%# Eval("InitiativeCode") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="InitiativeName" runat="server" CommandName="Sort" CommandArgument="InitiativeName" OnClientClick="SetWaitingGrvList(true);">          
                                                        <span><%= GetLocalResourceObject("lblInitiative") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "InitiativeName") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvInitiativeName" data-id="Initiativename" data-value="<%# Eval("InitiativeName") %>"><%# Eval("InitiativeName")%></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="Dimension" runat="server" CommandName="Sort" CommandArgument="Dimension" OnClientClick="SetWaitingGrvList(true);">          
                                                        <span><%= GetLocalResourceObject("lblDimension") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "Dimension") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvDimension" data-id="Dimension" data-value="<%# Eval("Dimension") %>"><%# Eval("Dimension")%></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="IndicatorName" runat="server" CommandName="Sort" CommandArgument="IndicatorName" OnClientClick="SetWaitingGrvList(true);">          
                                                        <span><%= GetLocalResourceObject("lblIndicator") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "IndicatorName") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvIndicatorName" data-id="IndicatorName" data-value="<%# Eval("IndicatorName") %>"><%# Eval("IndicatorName")%></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="DivisionName" runat="server" CommandName="Sort" CommandArgument="DivisionName" OnClientClick="SetWaitingGrvList(true);">          
                                                        <span><%= GetLocalResourceObject("lblDivision") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "DivisionName") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvDivisionName" data-id="DivisionName" data-value="<%# Eval("DivisionName") %>"><%# Eval("DivisionName")%></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" Visible="false">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="CompanyName" runat="server" CommandName="Sort" CommandArgument="CompanyName" OnClientClick="SetWaitingGrvList(true);">          
                                                        <span><%= GetLocalResourceObject("lblCompany") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "CompanyName") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvCompanyName" data-id="CompanyName" data-value="<%# Eval("CompanyName") %>"><%# Eval("CompanyName")%></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" Visible="false">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="CostFarmName" runat="server" CommandName="Sort" CommandArgument="CostFarmName" OnClientClick="SetWaitingGrvList(true);">          
                                                        <span><%= GetLocalResourceObject("lblFarm") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "CostFarmName") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div> 
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvCostFarmName" data-id="CostFarmName" data-value="<%# Eval("CostFarmName") %>"><%# Eval("CostFarmName")%></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="CoordinatorName" runat="server" CommandName="Sort" CommandArgument="CoordinatorName" OnClientClick="SetWaitingGrvList(true);">          
                                                        <span><%= GetLocalResourceObject("lblCoordinator") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "CoordinatorName") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvCoordinatorName" data-id="CoordinatorName" data-value="<%# Eval("CoordinatorName") %>"><%# Eval("CoordinatorName")%></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="StartDate" runat="server" CommandName="Sort" CommandArgument="StartDate" OnClientClick="SetWaitingGrvList(true);">          
                                                        <span><%= GetLocalResourceObject("lblStartDate") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "StartDate") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvStartDate" data-id="StartDate" data-value="<%# Eval("StartDate") %>"><%# Eval("StartDate")%></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="EndDate" runat="server" CommandName="Sort" CommandArgument="EndDate" OnClientClick="SetWaitingGrvList(true);">          
                                                        <span><%= GetLocalResourceObject("lblEndDate") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "EndDate") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvEndDate" data-id="EndDate" data-value="<%# Eval("EndDate") %>"><%# Eval("EndDate")%></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="Budget" runat="server" CommandName="Sort" CommandArgument="Budget" OnClientClick="SetWaitingGrvList(true);">          
                                                        <span><%= GetLocalResourceObject("lblBudget") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "Budget") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvBudget" data-id="Budget" data-value="<%# Eval("Budget") %>"><%# Eval("Budget")%></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="Beneficiaries" runat="server" CommandName="Sort" CommandArgument="Beneficiaries" OnClientClick="SetWaitingGrvList(true);">          
                                                        <span><%= GetLocalResourceObject("lblBeneficiariesQty") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "Beneficiaries") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvBeneficiaries" data-id="Beneficiaries" data-value="<%# Eval("Beneficiaries") %>"><%# Eval("Beneficiaries")%></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkView" runat="server" class="btn btn-default" CommandName="ViewDetails" CommandArgument='<%# Eval("InitiativeCode") %>' >                                                    
                                                    <span><%= GetLocalResourceObject("lblManage") %></span>
                                                </asp:LinkButton>
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
                
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
        <div class="ButtonsActions">
            <asp:UpdatePanel ID="uppActions" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                <Triggers>
                </Triggers>
                <ContentTemplate>
                    <div class="btn-group" role="group" aria-label="main-buttons">
                        <button id="btnAdd" type="button" runat="server" class="btn btn-default btnAjaxAction btns" onserverclick="BtnAdd_ServerClick" onclick="return ProcessAddRequest(this.id);" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnAdd"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnAdd"))%>' >
                            <span class="glyphicon glyphicon-plus glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span><br /><%= GetLocalResourceObject("btnAdd") %>
                        </button>
                        <button id="btnEdit" type="button" runat="server" class="btn btn-default btnAjaxAction btns"  onserverclick="btnEdit_ServerClick" onclick="return ProcessEditRequest(this.id);" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnEdit"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnEdit"))%>' >
                            <span class="glyphicon glyphicon-edit glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span><br /><%= GetLocalResourceObject("btnEdit") %>
                        </button>
                        <button id="btnDelete" type="button" runat="server" class="btn btn-default btnAjaxAction btns" onserverclick="btnDelete_ServerClick"  onclick="return ProcessDeleteRequest(this.id);" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnDelete"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnDelete"))%>' >
                            <span class="glyphicon glyphicon-trash glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span><br /><%= GetLocalResourceObject("btnDelete") %>
                        </button>                        
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
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
        });

        function pageLoad(sender, args) {
            /// <summary>Execute at load even at partial and ajax requests</summary>
            /// We prefer to bind the events here over do it on document ready event in order to auto rebind the events in page and ajax request each time
            //////////////////////////////////////////////////////////////////////////////////////////////////
            //In this section we set the button state for btnAjaxAction
            $('.btnAjaxAction').on('click', function () {
                var $this = $(this);
                $this.button('loading');
                setTimeout(function () { $this.button('reset'); }, 30000);
            });
            //In this section we set the dropdown state (element) for cboAjaxAction
            $('.cboAjaxAction').on('change', function () {
                var $this = $(this);
                $this.siblings(".waitingNotification").show();
                SetWaitingGrvList(true);
            });
            //And the grvList selection row functionality
            $('#<%= grvList.ClientID %>').on('click', 'tbody tr', function (event) {
                if (! $(this).hasClass('emptyRow')) {
                    $(this).toggleClass('info').siblings().removeClass('info');
                    $('#<%=hdfSelectedRowIndex.ClientID%>').val($(this).hasClass("info") ? $(this).index() : -1);
                }
            });
            //And the grvList pager event for client side
            $('#<%= blstPager.ClientID %>').on('click', 'a', function (event) {
                SetWaitingGrvList(true);
            });
            $('#<%= btnSearch.ClientID %>').on('click', function (event) {
                var $this = $('#<%= btnSearch.ClientID %>');
                $this.button('loading');
                setTimeout(function () { $this.button('reset'); }, 30000);
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
                return false;
            });
            $('#btnCancel, #btnClose').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                event.preventDefault();                                
                ClearModalForm();
                DisableButtonsDialog();
                $('#MaintenanceDialog').modal('hide');
                EnableButtonsDialog();
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
            //And the clean paste manager
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

            var current = null;
            $(".onChangeCostZoneId").on("changed.bs.select", function (e, clickedIndex, isSelected, previousValue) {
                MultiSelectDropdownListSaveSelectedItems($("#<%=CostZoneIdEdit.ClientID %>"), $("#<%=CostZoneIdEditMultiple.ClientID%>"));

                isSelected = isSelected == null || isSelected == undefined ? false : isSelected;

                if (current != previousValue || isSelected) {
                    current = previousValue;

                    setTimeout(function () { __doPostBack('<%= btnCostZoneIdEdit.UniqueID %>', ''); }, 1000);
                }
            });

            $(".onChangeCostZoneId .bs-actionsbox .bs-select-all").on("click", function () {
               MultiSelectDropdownListSaveSelectedItems($("#<%=CostZoneIdEdit.ClientID %>"), $("#<%=CostZoneIdEditMultiple.ClientID%>"));

                setTimeout(function () { __doPostBack('<%= btnCostZoneIdEdit.UniqueID %>', ''); }, 1000);
            });

            $(".onChangeCostMiniZoneId").on("changed.bs.select", function (e, clickedIndex, isSelected, previousValue) {
               MultiSelectDropdownListSaveSelectedItems($("#<%=CostMiniZoneIdEdit.ClientID %>"), $("#<%=CostMiniZoneIdEditMultiple.ClientID%>"));

                isSelected = isSelected == null || isSelected == undefined ? false : isSelected;

                if (current != previousValue || isSelected) {
                    current = previousValue;

                    setTimeout(function () { __doPostBack('<%= btnCostMiniZoneIdEdit.UniqueID %>', ''); }, 1000);
                }
            });

            $(".onChangeCostMiniZoneId .bs-actionsbox .bs-select-all").on("click", function () {
                MultiSelectDropdownListSaveSelectedItems($("#<%=CostMiniZoneIdEdit.ClientID %>"), $("#<%=CostMiniZoneIdEditMultiple.ClientID%>"));

                setTimeout(function () { __doPostBack('<%= btnCostMiniZoneIdEdit.UniqueID %>', ''); }, 1000);
            });

            $(".onChangeCostFarmId").on("changed.bs.select", function (e, clickedIndex, isSelected, previousValue) {
                MultiSelectDropdownListSaveSelectedItems($("#<%=CostFarmsIdEdit.ClientID %>"), $("#<%=CostFarmsIdEditMultiple.ClientID%>"));

                isSelected = isSelected == null || isSelected == undefined ? false : isSelected;

                if (current != previousValue || isSelected) {
                    current = previousValue;
                }
            });

            $(".onChangeCostFarmId .bs-actionsbox .bs-select-all").on("click", function () {
                MultiSelectDropdownListSaveSelectedItems($("#<%=CostFarmsIdEdit.ClientID %>"), $("#<%=CostFarmsIdEditMultiple.ClientID%>"));
            });

            MultiSelectDropdownListRestoreSelectedItems($("#<%=CostZoneIdEdit.ClientID %>"), $("#<%=CostZoneIdEditMultiple.ClientID%>"));
            MultiSelectDropdownListRestoreSelectedItems($("#<%=CostMiniZoneIdEdit.ClientID %>"), $("#<%=CostMiniZoneIdEditMultiple.ClientID%>"));
            MultiSelectDropdownListRestoreSelectedItems($("#<%=CostFarmsIdEdit.ClientID %>"), $("#<%=CostFarmsIdEditMultiple.ClientID%>"));

            //////////////////////////////////////////////////////////////////////////////////////////////////
          

            //////////////////////////////////////////////////////////////////////////////////////////////////
            //In this section we initialize the popovers and tooltips for the elements.
            //We have both (popovers and tooltips) in order to accomplish information alert over desktop 
            //and mobiles devices
            InitializeTooltipsPopovers();            
            //////////////////////////////////////////////////////////////////////////////////////////////////
            //In this section we initialize the sorter elements for tables.

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
            //////////////////////////////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////////////////////////////
            //In this section we set the components enabled/disabled according to hdfIsFormEnabled indicator
            //Special attention to client side components that can not be enabled/disabled from server side
            //////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////
            //Other executions
            //each time a ajax or page load execue we need to synch the selected row with its value
            SetRowSelected();
        }

        //*******************************//
        //          VALIDATION           // 
        //*******************************//

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
        function SetRowSelected() {
            /// <summary>Set the class of the selected row</summary>  
            var selectedRowIndex = $('#<%=hdfSelectedRowIndex.ClientID%>').val();
            if (!isEmptyOrSpaces(selectedRowIndex) && selectedRowIndex != "-1") {
                var selectedRow = $('#<%= grvList.ClientID %> tbody tr:eq(' + selectedRowIndex + ')');
                selectedRow.siblings().removeClass('info');    
                if (!selectedRow.hasClass('info'))
                {
                    selectedRow.addClass('info');
                }
            }
        }
        function UnselectRow() {
            /// <summary>Unselect rows</summary>  
            $('#<%=hdfSelectedRowIndex.ClientID%>').val("-1");
            $('#<%= grvList.ClientID %> tbody tr').removeClass('info');            
        }
        //*******************************//
        //             LOGIC             // 
        //*******************************//                
    
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

        function ProcessAddRequest(resetId) {
            /// <summary>Process the edit request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param> 
            $(".btns").prop("disabled", true);
            __doPostBack('<%= btnAdd.UniqueID %>', '');
            return true;

        }

        function ProcessEditRequest(resetId) {
            /// <summary>Process the edit request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param> 
            $(".btns").prop("disabled",true);
            if (IsRowSelected()) {
                __doPostBack('<%= btnEdit.UniqueID %>', '');
                return true;
            }
            else {                
                ShowFooterError('<%= GetLocalResourceObject("msgInvalidSelection") %>');
                ErrorButton(resetId);
                $(".btns").prop("disabled",false);
                return false;
            }
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
            disableButton($('#btnAccept'));
            disableButton($('#btnCancel'));
        }
        function EnableButtonsDialog() {
            ///<summary>Enabled all buttons toolbar</summary>
            enableButton($('#btnAccept'));
            enableButton($('#btnCancel'));
        }
        //*******************************//
        //  RETURN FROM POSTBACKS LOGIC  // 
        //*******************************//        
        function ReturnFromBtnEditClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $(".btns").prop("disabled",true);
            SetRowSelected();
            DisableToolBar();
            $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#dialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#MaintenanceDialog').modal('show');
            EnableToolBar();
            $(".btns").prop("disabled",false);
        }
        function ReturnFromBtnAcceptClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            DisableButtonsDialog();
            SetRowSelected();
            
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
            EnableToolBar();
        } 

        function ReturnRefreshDropdownList() {
            MultiSelectDropdownListRestoreSelectedItems($("#<%=CostZoneIdEdit.ClientID %>"), $("#<%=CostZoneIdEditMultiple.ClientID%>"));
            MultiSelectDropdownListRestoreSelectedItems($("#<%=CostMiniZoneIdEdit.ClientID %>"), $("#<%=CostMiniZoneIdEditMultiple.ClientID%>"));
            MultiSelectDropdownListRestoreSelectedItems($("#<%=CostFarmsIdEdit.ClientID %>"), $("#<%=CostFarmsIdEditMultiple.ClientID%>"));
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
        //In this section we set the control for multiple simultaneous ajax request
        //We cancel the ajax request executed when another one is in progress
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(initializeRequest);
        prm.add_endRequest(endingRequest);


        function initializeRequest(sender, args) {            
            if (prm.get_isInAsyncPostBack()) {
                args.set_cancel(true);
                prm.abortPostBack();
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
