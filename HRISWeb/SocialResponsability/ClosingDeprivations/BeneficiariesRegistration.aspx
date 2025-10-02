<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="BeneficiariesRegistration.aspx.cs" Inherits="HRISWeb.SocialResponsability.ClosingDeprivations.BeneficiariesRegistration" %>


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
                        <h4 class="text-left text-primary"><%= GetLocalResourceObject("lblInitiativeDetails") %></h4>
                        <br />

                        <div class="row">
                            <div class="col-sm-4">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=txtInitiativeName.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblInitiative")%></label>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtInitiativeName" onkeypress="return isNumberOrLetter(event)" CssClass="form-control cleanPasteText control-validation" runat="server"></asp:TextBox>
                                            <label id="txtInitiativeNameValidation" for="<%= txtInitiativeName.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjInitiativeNameValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=cboIndicator.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblIndicator")%></label>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:DropDownList ID="cboIndicator" CssClass="form-control control-validation selectpicker" AutoPostBack="false" runat="server" data-live-search="true" data-actions-box="true"></asp:DropDownList>
                                            <label id="cboIndicatorValidation" for="<%= cboIndicator.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjIndicatorValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=cboDivision.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblDivision")%></label>
                                        </div>

                                        <div class="col-sm-7">
                                            <asp:DropDownList ID="cboDivision" CssClass="form-control control-validation selectpicker" AutoPostBack="true" runat="server" data-live-search="true" data-actions-box="true" data-id="Division" data-value="isPermitted" OnSelectedIndexChanged="cboDivision_SelectedIndexChanged"></asp:DropDownList>
                                            <label id="cboDivisionValidation" for="<%= cboDivision.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgDivisionValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                            <asp:HiddenField ID="hdfDivisionValueFilter" runat="server" />
                                        </div>
                                    </div>

                                    <div class="form-group" style="display: none">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=cboCompanies.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCompany")%></label>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:DropDownList ID="cboCompanies" CssClass="form-control control-validation selectpicker" AutoPostBack="false" runat="server" data-live-search="true" data-actions-box="true"></asp:DropDownList>
                                            <label id="cboCompaniesValidation" for="<%= cboCompanies.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgCompanyValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
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

                            <div class="col-sm-4">
                                <div class="form-horizontal">

                                    <div class="form-group" style="display: none">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=cboFarm.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblFarm")%></label>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:DropDownList ID="cboFarm" CssClass="form-control control-validation selectpicker" AutoPostBack="false" runat="server" data-live-search="true" data-actions-box="true"></asp:DropDownList>
                                            <label id="cboFarmValidation" for="<%= cboFarm.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgFarmValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                        </div>
                                    </div>

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

                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=cboCoordinator.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCoordinator")%></label>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:DropDownList ID="cboCoordinator" CssClass="form-control control-validation selectpicker" AutoPostBack="false" runat="server" data-live-search="true" data-actions-box="true"></asp:DropDownList>
                                            <label id="cboCoordinatorValidation" for="<%= cboCoordinator.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgCoordinatorValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="dtpStartDate" class="control-label"><%=GetLocalResourceObject("lblStartDate")%></label>
                                        </div>

                                        <div class="col-sm-7">
                                            <div class="input-group">
                                                <asp:TextBox runat="server" ID="dtpStartDate" class="form-control date control-validation cleanPasteDigits" placeholder='<%$ Code:GetLocalResourceObject("lblDateFormatPlaceHolder") %>' type="text" autocomplete="off" />
                                                <label id="dtpStartDateValidation" for="<%= dtpStartDate.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgStartDateValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                <div class="input-group-addon">
                                                    <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="dtpEndDate" class="control-label"><%=GetLocalResourceObject("lblEndDate")%></label>
                                        </div>

                                        <div class="col-sm-7">
                                            <div class="input-group">
                                                <asp:TextBox runat="server" ID="dtpEndDate" class="form-control date control-validation cleanPasteDigits" placeholder='<%$ Code:GetLocalResourceObject("lblDateFormatPlaceHolder") %>' type="text" autocomplete="off" />
                                                <label id="dtpEndDateValidation" for="<%= dtpEndDate.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgEndDateValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>

                                                <div class="input-group-addon">
                                                    <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=txtBudget.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblBudget")%></label>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtBudget" type="number" onkeypress="return isDecimalNumber(event) && checkMaxLength(this,event,11);" CssClass="form-control control-validation cleanPasteDecimalDigits" min="0" max="99999999" MaxLength="8" autocomplete="off" runat="server"></asp:TextBox>
                                            <label id="txtBudgetValidation" for="<%= txtBudget.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgBudgetValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-4">
                                <div class="form-horizontal">


                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label  style="text-align:left" for="<%=txtBeneficiaries.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblBeneficiariesQtyPlanned")%></label>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtBeneficiaries" type="number" onkeypress="return isDecimalNumber(event) && checkMaxLength(this,event,11);" CssClass="form-control control-validation cleanPasteDecimalDigits" min="0" max="99999999" MaxLength="8" autocomplete="off" runat="server"></asp:TextBox>
                                            <label id="txtBeneficiariesValidation" for="<%= txtBeneficiaries.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgBeneficiariesValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=txtBeneficiariesReal.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblBeneficiariesQty")%></label>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtBeneficiariesReal" Enabled="false" type="number" onkeypress="return isDecimalNumber(event) && checkMaxLength(this,event,11);" CssClass="form-control control-validation cleanPasteDecimalDigits" min="0" max="99999999" MaxLength="8" autocomplete="off" runat="server"></asp:TextBox>
                                            <label id="txtBeneficiariesRealValidation" for="<%= txtBeneficiariesReal.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgBeneficiariesValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                        </div>
                                    </div>



                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=txtInvestedHours.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblInvestedPlanned")%></label>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtInvestedHours" type="number" onkeypress="return isDecimalNumber(event) && checkMaxLength(this,event,11);" CssClass="form-control control-validation cleanPasteDecimalDigits" min="0" max="99999999" MaxLength="8" autocomplete="off" runat="server"></asp:TextBox>
                                            <label id="txtInvestedHoursValidation" for="<%= txtInvestedHours.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgInvestedHoursValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=txtInvestedHoursReal.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblInvestedHours")%></label>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtInvestedHoursReal" Enabled="false" type="number" onkeypress="return isDecimalNumber(event) && checkMaxLength(this,event,11);" CssClass="form-control control-validation cleanPasteDecimalDigits" min="0" max="99999999" MaxLength="8" autocomplete="off" runat="server"></asp:TextBox>
                                            <label id="txtInvestedHoursRealValidation" for="<%= txtInvestedHoursReal.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgInvestedHoursValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=chkBMPIAssociated.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblBMPIAssociated")%></label>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:CheckBox ID="chkBMPIAssociated" CssClass="form-check-input control-validation" runat="server"></asp:CheckBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <div class="row">
                            <div class="col-sm-12 text-center" style="background-color: #808080; height: 2px">
                            </div>
                        </div>
                        <br />

                        <div class="row">
                            <div class="col-sm-2">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-sm-5 text-left"> 
                                            <label for="<%=txtEmloyeeCode.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblEmployeeCode")%></label>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtEmloyeeCode" CssClass="form-control  cleanPasteDigit EnterKey" runat="server" autocomplete="off" MaxLength="12" type="text"></asp:TextBox>
                                        </div>
                                    </div>                                      
                                </div>
                            </div> 
                            <div class="col-sm-4">
                                <div class="form-horizontal">
                                   <div class="form-group">
                                        <div class="col-sm-3 text-left"> 
                                            <label for="<%=txtEmloyeeName.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblEmployeeName")%></label>
                                        </div>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="txtEmloyeeName" CssClass="form-control  cleanPasteText EnterKey" runat="server" autocomplete="off" MaxLength="250" type="text"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div> 
                        </div>

                        <div class="row">
                            <div class="col-sm-2">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=txtSeniorityStart.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblStartSeniority")%></label>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtSeniorityStart" CssClass="form-control control-validation cleanPasteDecimalDigits" onkeypress="return isDecimalNumber(event) && checkMaxLength(this,event,3);" MaxLength="3" autocomplete="off" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=txtSeniorityEnd.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblEndSeniority")%></label>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtSeniorityEnd" CssClass="form-control control-validation cleanPasteDecimalDigits" onkeypress="return isDecimalNumber(event) && checkMaxLength(this,event,3);" MaxLength="3" autocomplete="off" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=txtAgeStart.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblStartAge")%></label>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtAgeStart" CssClass="form-control control-validation cleanPasteDecimalDigits" onkeypress="return isDecimalNumber(event) && checkMaxLength(this,event,3);" MaxLength="3" autocomplete="off" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=txtAgeEnd.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblEndAge")%></label>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtAgeEnd" CssClass="form-control control-validation cleanPasteDecimalDigits" onkeypress="return isDecimalNumber(event) && checkMaxLength(this,event,3);" MaxLength="3" autocomplete="off" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=txtPovertyScoreStart.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblStartPovertyScore")%></label>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtPovertyScoreStart" CssClass="form-control control-validation cleanPasteDecimalDigits" onkeypress="return isDecimalNumber(event) && checkMaxLength(this,event,8);" MaxLength="8" autocomplete="off" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=txtPovertyScoreEnd.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblEndPovertyScore")%></label>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtPovertyScoreEnd" CssClass="form-control control-validation cleanPasteDecimalDigits" onkeypress="return isDecimalNumber(event) && checkMaxLength(this,event,8);" MaxLength="8" autocomplete="off" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-2">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=rblGender.ClientID%>" class="control-label"><%=GetLocalResourceObject("Gender")%></label>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:RadioButtonList ID="rblGender" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="M" class="form-check-input" Value="Masculino" />
                                                <asp:ListItem Text="F" class="form-check-input" Value="Femenino" />
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <button id="btnSearch" type="button" runat="server" class="btn btn-default btnAjaxAction" onserverclick="BtnSearch_ServerClick" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>'>
                                    <span class="glyphicon glyphicon-search glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%= GetLocalResourceObject("btnSearch") %>
                                </button>

                                <asp:Button ID="btnSearchDefault" runat="server" OnClick="BtnSearch_ServerClick" Style="display: none;" />
                            </div>
                        </div>
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
                                    DataKeyNames="IndividualCode"
                                    OnRowCommand="grvList_RowCommand"
                                    OnPreRender="GrvList_PreRender" OnSorting="GrvList_Sorting">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="IndividualCodeSort" runat="server" CommandName="Sort" CommandArgument="IndividualCode" OnClientClick="SetWaitingGrvList(true);">          
                                                    <span><%= GetLocalResourceObject("IndividualCode") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "IndividualCode") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvIndividualCode" data-id="PrincipalIndividualCode" data-value="<%# Eval("IndividualCode") %>"><%# Eval("IndividualCode") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="IndividualNameSort" runat="server" CommandName="Sort" CommandArgument="IndividualName" OnClientClick="SetWaitingGrvList(true);">          
                                                    <span><%= GetLocalResourceObject("IndividualName") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "IndividualName") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvIndividualName" data-id="PrincipalIndividualName" data-value="<%# Eval("IndividualName") %>"><%# Eval("IndividualName") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="EmployeeSenioritySort" runat="server" CommandName="Sort" CommandArgument="EmployeeSeniority" OnClientClick="SetWaitingGrvList(true);">          
                                                    <span><%= GetLocalResourceObject("EmployeeSeniority") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "EmployeeSeniority") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvEmployeeSeniority" data-id="PrincipalEmployeeSeniority" data-value="<%# Eval("EmployeeSeniority") %>"><%# Eval("EmployeeSeniority") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="DeprivationScoreSort" runat="server" CommandName="Sort" CommandArgument="DeprivationScore" OnClientClick="SetWaitingGrvList(true);">          
                                                    <span><%= GetLocalResourceObject("DeprivationScore") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "DeprivationScore") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvDeprivationScore" data-id="PrincipalDeprivationScore" data-value="<%# Eval("DeprivationScore") %>"><%# Eval("DeprivationScore") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="GenderSort" runat="server" CommandName="Sort" CommandArgument="Gender" OnClientClick="SetWaitingGrvList(true);">          
                                                    <span><%= GetLocalResourceObject("Gender") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "Gender") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvGender" data-id="PrincipalGender" data-value="<%# Eval("Gender") %>"><%# Eval("Gender") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="AgeSort" runat="server" CommandName="Sort" CommandArgument="Age" OnClientClick="SetWaitingGrvList(true);">          
                                                        <span><%= GetLocalResourceObject("Age") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "Age") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvAge" data-id="PrincipalAge" data-value="<%# Eval("Age") %>"><%# Eval("Age") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div style="width: 100%; text-align: center;">
                                                    <asp:LinkButton ID="FamilyRelationshipSort" runat="server" CommandName="Sort" CommandArgument="FamilyRelationship" OnClientClick="SetWaitingGrvList(true);">          
                                                        <span><%= GetLocalResourceObject("FamilyRelationship") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "FamilyRelationship") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvFamilyRelationship" data-id="PrincipalFamilyRelationship" data-value="<%# Eval("FamilyRelationship") %>"><%# Eval("FamilyRelationship") %></span>
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
                        <div id="screenWaiting" style="position: absolute; margin: auto; display: none; bottom: 0px; top: 0px; left: 50%; transform: translate(-50%, 35%);">
                                    <span class='fa fa-spinner fa-spin' style="font-size: 50px;"></span>
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
                        <button id="btnAdd" type="button" runat="server" class="btn btn-default btnAjaxAction btns" onserverclick="BtnAdd_ServerClick" onclick="return ProcessAddRequest(this.id);" 
                            data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnSave"))%>' 
                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnAdd"))%>'>
                            <span class="glyphicon glyphicon-floppy-disk glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnSave") %>
                        </button>
                        <button id="btnEdit" type="button" runat="server" class="btn btn-default btnAjaxAction btns" onserverclick="btnEdit_ServerClick" onclick="return ProcessEditRequest(this.id);" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnCancel"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnEdit"))%>'>
                            <span class="glyphicon glyphicon-remove glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnCancel") %>
                        </button>
                        <button id="btnDelete" style="display: none" type="button" runat="server" class="btn btn-default btnAjaxAction btns" onserverclick="btnDelete_ServerClick" onclick="return ProcessDeleteRequest(this.id);" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnDelete"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnDelete"))%>'>
                            <span class="glyphicon glyphicon-trash glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnDelete") %>
                        </button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hndInitiativeCode" Value="-1" />
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
                console.log("Paste");
                setTimeout(function (e) {
                    replacePastedInvalidCharacters($this);
                    var ml = $this.attr("maxlength");
                    if (ml) {
                        checkMaxLength($this[0], e, parseInt(ml));
                    }
                }, 50);
            });
            //And the clean paste manager
            $('.cleanPasteDigits').on('paste keyup', function (e) {
                var $this = $(this);
                console.log("Paste");
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

            //In this section we initialize the date time pickers
            $('#<%= dtpStartDate.ClientID %>').datetimepicker({
                format: 'MM/DD/YYYY'
            });

            $('#<%= dtpEndDate.ClientID %>').datetimepicker({
                format: 'MM/DD/YYYY'
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
        var validator = null;
        function ValidateForm() {
            jQuery.validator.addMethod("validSelect", function (value, element) {
                return ValidateSelect(value);
            }, "Please Select a valid information");

            jQuery.validator.addMethod("validDate", function (value, element) {
                return this.optional(element) || moment(value, "MM/DD/YYYY").isValid();
            }, "Please enter a valid date in the format MM/DD/YYYY");

            if (validator != null) {
                validator.destroy();
            }

            validator = $('#' + document.forms[0].id).validate({
                debug: true,
                highlight: function (element, errorClass, validClass) {
                    SetControlInvalid($(element).attr('id'));
                },

                unhighlight: function (element, errorClass, validClass) {
                    SetControlValid($(element).attr('id'));
                },

                errorPlacement: function (error, element) { },
                rules: {
                    "<%= txtInitiativeName.UniqueID%>": {
                        required: true,
                        normalizer: function (value) {
                            return $.trim(value);
                        },
                        minlength: 1,
                        maxlength: 500
                    },
                    "<%= cboDivision.UniqueID %>": {
                        required: true,
                        validSelect: true
                    },
                    "<%= txtBudget.UniqueID%>": {
                        required: true,
                        normalizer: function (value) {
                            return $.trim(value);
                        }
                    },
                    "<%= txtBeneficiaries.UniqueID%>": {
                        required: true,
                        normalizer: function (value) {
                            return $.trim(value);
                        }
                    },
                    "<%= cboCompanies.UniqueID %>": {
                        required: true,
                        validSelect: true
                    },
                    "<%= cboFarm.UniqueID %>": {
                        required: true,
                        validSelection: true
                    },
                    "<%= cboIndicator.UniqueID %>": {
                        required: true,
                        validSelect: true
                    },
                    "<%= cboCoordinator.UniqueID %>": {
                        required: true,
                        validSelect: true
                    },
                    "<%= dtpEndDate.UniqueID %>": {
                        required: true,
                        validDate: true
                    },
                    "<%= dtpStartDate.UniqueID %>": {
                        required: true,
                        validDate: true
                    },
                    "<%= txtInvestedHours.UniqueID%>": {
                        required: true,
                        normalizer: function (value) {
                            return $.trim(value);
                        }
                    }
                }
            });

            var result = validator.form();
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
        //*******************************//
        //             LOGIC             // 
        //*******************************//                
        function ClearModalForm() {
            /// <summary>Clear the modal form</summary>                        

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
            $(".btns").prop("disabled", true);
            __doPostBack('<%= btnEdit.UniqueID %>', '');
            return true;
        }
        function ProcessAddRequest(resetId) {
            /// <summary>Process the edit request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param> 

            /// <summary>Process the accept request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            //disableButton($('#btnCancel'));
            DesplegarBackdrop();
            $('#screenWaiting').fadeIn('fast');
            $(".btns").prop("disabled", true);
            __doPostBack('<%= btnAdd.UniqueID %>', '');
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
        function ValidateSelect(value) {

            if (value != "-1") {
                return true;
            }

            return false;
        }

        function ReturnInitiativePage() {
            window.location.href = "InitiativeManagement.aspx";
        }
        //*******************************//
        //  RETURN FROM POSTBACKS LOGIC  // 
        //*******************************//        
        function ReturnFromBtnEditClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $(".btns").prop("disabled", true);
            SetRowSelected();
            DisableToolBar();
            $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#dialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#MaintenanceDialog').modal('show');
            EnableToolBar();
            $(".btns").prop("disabled", false);
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
