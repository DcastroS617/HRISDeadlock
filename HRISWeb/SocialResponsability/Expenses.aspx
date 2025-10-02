<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Expenses.aspx.cs" Inherits="HRISWeb.SocialResponsability.Expenses" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntHeader" runat="server">
    <title><%=Convert.ToString(GetLocalResourceObject("lblScreenTitle")) %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cntBody" runat="server">
    <style>
        .bootstrap-select>.dropdown-toggle.bs-placeholder, .bootstrap-select>.dropdown-toggle.bs-placeholder:active,
        .bootstrap-select>.dropdown-toggle.bs-placeholder:focus, .bootstrap-select>.dropdown-toggle.bs-placeholder:hover{
color: #333;
        }
    </style>
    <div class="main-content">
        <h1 class="text-left text-primary">
            <label class="PageTitle"><%= GetLocalResourceObject("lblScreenTitle") %></label>
        </h1>
        <br />
        <asp:UpdatePanel runat="server" ID="upnMain">
            <Triggers>
            </Triggers>
            <ContentTemplate>
                <div class="container" style="width: 100%">
                    <div class="row">
                        <div class="btn-group" role="group" aria-label="main-buttons">
                            <asp:LinkButton ID="lbtnSaveAsDraft" CssClass="btn btn-default btnAjaxAction" runat="server" OnClick="lbtnSaveAsDraft_Click" OnClientClick="return ProcessSaveAsDraftRequest(this.id);"
                                data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("lbtnSaveAsDraft.Text"))%>'
                                data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("lbtnSaveAsDraft.Text"))%>'>
                                <span class="glyphicon glyphicon-erase glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span><br />
                                <%= GetLocalResourceObject("lbtnSaveAsDraft.Text") %>
                            </asp:LinkButton>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="form-horizontal">
                                <ul class="nav nav-tabs" id="surveyTab">
                                    <li class="nav-item active">
                                        <a class="nav-link active" id="expenses-tab" data-toggle="tab" href="#expenses" role="tab" aria-controls="expenses" aria-selected="true"><%= GetLocalResourceObject("expenses-tab") %></a>
                                    </li>
                                </ul>
                                <div class="tab-content" id="survey">
                                    <div class="tab-pane fade in active" id="expenses" role="tabpanel" aria-labelledby="expenses-tab">
                                        <p>
                                            <br />
                                        </p>
                                        <div class="row">
                                            <div class="col-sm-3">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-12">
                                                            <asp:Label ID="lblExtraIncome" meta:resourcekey="lblExtraIncome" AssociatedControlID="chkExtraIncome" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-9">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-1">
                                                            <asp:CheckBox ID="chkExtraIncome" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:Label ID="lblExtraIncomeComes" meta:resourcekey="lblExtraIncomeComes" AssociatedControlID="txtExtraIncomeComes" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <asp:TextBox ID="txtExtraIncomeComes" meta:resourcekey="txtExtraIncomeComes" runat="server" CssClass="form-control control-validation cleanPasteText" onkeypress="blockEnterKey();return isNumberOrLetter(event);" MaxLength="65" disabled="disabled" placeholder=""></asp:TextBox>
                                                            <label id="txtExtraIncomeComesValidation" for="<%= txtExtraIncomeComes.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgExtraIncomeComesFromValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-5 col-sm-offset-1">                                                             
                                                           
                                                                <asp:Label ID="lblPrincipalProvider" meta:resourcekey="lblPrincipalProvider" AssociatedControlID="chkPrincipalProvider" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                    
                                                        </div>
                                                        <div class="col-sm-1">                                                            
                                                           <asp:CheckBox ID="chkPrincipalProvider" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>                                            
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-2">
                                                            <asp:Label ID="lblExpensesDetails" meta:resourcekey="lblExpensesDetails" AssociatedControlID="hdfExpenses" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                            <asp:HiddenField ID="hdfExpenses" runat="server" />
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lblValuesCalculation" meta:resourcekey="lblValuesCalculation" AssociatedControlID="hdfExpenses" runat="server" Text="" CssClass="control-label text-left text-danger"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lblCurrency" AssociatedControlID="hdfExpenses" runat="server" Text="" CssClass="control-label text-left text-danger"></asp:Label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>                                            
                                        </div>                                         
                                        <div class="row">
                                            <div class="col-sm-3">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lblFeeding" meta:resourcekey="lblFeeding" AssociatedControlID="txtFeeding" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtFeeding" meta:resourcekey="txtFeeding" runat="server" CssClass="form-control numericInputMask control-validation cleanPasteText" onkeypress="blockEnterKey();" MaxLength="14" placeholder=""></asp:TextBox>
                                                            <label id="txtFeedingValidation" for="<%= txtFeeding.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgFeedingExpenseValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </div>                                                        
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-3">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lblElectricity" meta:resourcekey="lblElectricity" AssociatedControlID="txtElectricity" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtElectricity" meta:resourcekey="txtElectricity" runat="server" CssClass="form-control numericInputMask control-validation cleanPasteText" onkeypress="blockEnterKey();" MaxLength="14" placeholder=""></asp:TextBox>
                                                            <label id="txtElectricityValidation" for="<%= txtElectricity.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgElectricityExpenseValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-3">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lblCableTv" meta:resourcekey="lblCableTv" AssociatedControlID="txtCableTv" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtCableTv" meta:resourcekey="txtCableTv" runat="server" CssClass="form-control numericInputMask control-validation cleanPasteText" onkeypress="blockEnterKey();" MaxLength="14" placeholder=""></asp:TextBox>
                                                            <label id="txtCableTvValidation" for="<%= txtCableTv.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgCableTvExpenseValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-3">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lblLivingPlace" meta:resourcekey="lblLivingPlace" AssociatedControlID="txtLivingPlace" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtLivingPlace" meta:resourcekey="txtLivingPlace" runat="server" CssClass="form-control numericInputMask control-validation cleanPasteText" onkeypress="blockEnterKey();" MaxLength="14" placeholder=""></asp:TextBox>
                                                            <label id="txtLivingPlaceValidation" for="<%= txtLivingPlace.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgLivingPlaceExpenseValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </div>                                                        
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">                                            
                                            <div class="col-sm-3">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lblTransport" meta:resourcekey="lblTransport" AssociatedControlID="txtTransport" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtTransport" meta:resourcekey="txtTransport" runat="server" CssClass="form-control numericInputMask control-validation cleanPasteText" onkeypress="blockEnterKey();" MaxLength="14" placeholder=""></asp:TextBox>
                                                            <label id="txtTransportValidation" for="<%= txtTransport.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgTransportExpenseValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-3">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lblInternet" meta:resourcekey="lblInternet" AssociatedControlID="txtInternet" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtInternet" meta:resourcekey="txtInternet" runat="server" CssClass="form-control numericInputMask control-validation cleanPasteText" onkeypress="blockEnterKey();" MaxLength="14" placeholder=""></asp:TextBox>
                                                            <label id="txtInternetValidation" for="<%= txtInternet.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgInternetExpenseValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-3">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lblClothing" meta:resourcekey="lblClothing" AssociatedControlID="txtClothing" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtClothing" meta:resourcekey="txtClothing" runat="server" CssClass="form-control numericInputMask control-validation cleanPasteText" onkeypress="blockEnterKey();" MaxLength="14" placeholder=""></asp:TextBox>
                                                            <label id="txtClothingValidation" for="<%= txtClothing.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgClothingExpenseValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </div>                                                        
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-3">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lblWater" meta:resourcekey="lblWater" AssociatedControlID="txtWater" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtWater" meta:resourcekey="txtWater" runat="server" CssClass="form-control numericInputMask control-validation cleanPasteText" onkeypress="blockEnterKey();" MaxLength="14" placeholder=""></asp:TextBox>
                                                            <label id="txtWaterValidation" for="<%= txtWater.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgWaterExpenseValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>                                                                                       
                                        </div>
                                        <div class="row">                                            
                                            <div class="col-sm-3">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lblEducation" meta:resourcekey="lblEducation" AssociatedControlID="txtEducation" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtEducation" meta:resourcekey="txtEducation" runat="server" CssClass="form-control numericInputMask control-validation cleanPasteText" onkeypress="blockEnterKey();" MaxLength="14" placeholder=""></asp:TextBox>
                                                            <label id="txtEducationValidation" for="<%= txtEducation.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-placement="left" data-offset="10" data-content="<%= GetLocalResourceObject("msgEducationExpenseValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-3">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lblPhone" meta:resourcekey="lblPhone" AssociatedControlID="txtPhone" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtPhone" meta:resourcekey="txtPhone" runat="server" CssClass="form-control numericInputMask control-validation cleanPasteText" onkeypress="blockEnterKey();" MaxLength="14" placeholder=""></asp:TextBox>
                                                            <label id="txtPhoneValidation" for="<%= txtPhone.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgTelephoneExpenseValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </div>                                                        
                                                    </div>
                                                </div>
                                            </div>                                           
                                            <div class="col-sm-5">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-4" style="padding-right:0!important">
                                                            <asp:Label ID="lblGas" meta:resourcekey="lblGas" AssociatedControlID="txtGas" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-4" style="padding-left:0!important">
                                                            <asp:TextBox ID="txtGas" meta:resourcekey="txtGas" runat="server" CssClass="form-control numericInputMask control-validation cleanPasteText" onkeypress="blockEnterKey();" MaxLength="14" placeholder=""></asp:TextBox>
                                                            <label id="txtGasValidation" for="<%= txtGas.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgCookingGasExpenseValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">                                            
                                            <div class="col-sm-5">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-6">
                                                            <asp:Label ID="lblDoctor" meta:resourcekey="lblDoctor" AssociatedControlID="txtDoctor" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txtDoctor" meta:resourcekey="txtDoctor" runat="server" CssClass="form-control numericInputMask control-validation cleanPasteText" onkeypress="blockEnterKey();" MaxLength="14" placeholder=""></asp:TextBox>
                                                            <label id="txtDoctorValidation" for="<%= txtDoctor.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgDoctorExpenseValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">                                            
                                            <div class="col-sm-5">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-6">
                                                            <asp:Label ID="lblLoan" meta:resourcekey="lblLoan" AssociatedControlID="txtLoan" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txtLoan" meta:resourcekey="txtLoan" runat="server" CssClass="form-control numericInputMask control-validation cleanPasteText" onkeypress="blockEnterKey();" MaxLength="14" placeholder=""></asp:TextBox>
                                                            <label id="txtLoanValidation" for="<%= txtLoan.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgLoansExpenseValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-2">
                                                            <asp:Label ID="lblLoanDescription" meta:resourcekey="lblLoanDescription" AssociatedControlID="txtLoanDescription" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-10">
                                                            <asp:TextBox ID="txtLoanDescription" meta:resourcekey="txtLoanDescription" runat="server" CssClass="form-control control-validation cleanPasteText" onkeypress="blockEnterKey();return isNumberOrLetter(event);" MaxLength="60" placeholder=""></asp:TextBox>
                                                            <label id="txtLoanDescriptionValidation" for="<%= txtLoanDescription.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgLoansExpenseDescriptionValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>                                                                                        
                                        </div>
                                        <div class="row">                                            
                                            <div class="col-sm-5">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-6">
                                                            <asp:Label ID="lblLegalDeductions" meta:resourcekey="lblLegalDeductions" AssociatedControlID="txtLegalDeductions" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <asp:TextBox ID="txtLegalDeductions" meta:resourcekey="txtLegalDeductions" runat="server" CssClass="form-control numericInputMask control-validation cleanPasteText" onkeypress="blockEnterKey();" MaxLength="14" placeholder=""></asp:TextBox>
                                                            <label id="txtLegalDeductionsValidation" for="<%= txtLegalDeductions.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgLegalDeductionsValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <span class="glyphicon glyphicon-info-sign text-center" data-toggle="tooltip" title='<%=GetLocalResourceObject("txtLegalDeductions.Tooltip")%>' style="cursor:pointer"></span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-2">
                                                            <asp:Label ID="lblLegalDeductionsDescription" meta:resourcekey="lblLegalDeductionsDescription" AssociatedControlID="txtLegalDeductionsDescription" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-10">
                                                            <asp:TextBox ID="txtLegalDeductionsDescription" meta:resourcekey="txtLegalDeductionsDescription" runat="server" CssClass="form-control control-validation cleanPasteText" onkeypress="blockEnterKey();return isNumberOrLetter(event);" MaxLength="60" placeholder=""></asp:TextBox>
                                                            <label id="txtLegalDeductionsDescriptionValidation" for="<%= txtLegalDeductionsDescription.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgLegalDeductionsDescriptionValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>                                                                                        
                                        </div>
                                        <div class="row">                                            
                                            <div class="col-sm-5">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-6">
                                                            <asp:Label ID="lblOtherExpenses" meta:resourcekey="lblOtherExpenses" AssociatedControlID="txtOtherExpenses" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txtOtherExpenses" meta:resourcekey="txtOtherExpenses" runat="server" CssClass="form-control numericInputMask control-validation cleanPasteText" onkeypress="blockEnterKey();" MaxLength="14" placeholder=""></asp:TextBox>
                                                            <label id="txtOtherExpensesValidation" for="<%= txtOtherExpenses.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgOtherExpensesValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-2">
                                                            <asp:Label ID="lblOtherExpensesDescription" meta:resourcekey="lblOtherExpensesDescription" AssociatedControlID="txtOtherExpensesDescription" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-10">
                                                            <asp:TextBox ID="txtOtherExpensesDescription" meta:resourcekey="txtOtherExpensesDescription" runat="server" CssClass="form-control control-validation cleanPasteText" onkeypress="blockEnterKey();return isNumberOrLetter(event);" MaxLength="60" placeholder=""></asp:TextBox>
                                                            <label id="txtOtherExpensesDescriptionValidation" for="<%= txtOtherExpensesDescription.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgOtherExpensesDescriptionValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>                                                                                        
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <hr />
                    <div class="row">
                        <div class="col-sm-6 text-left">
                            <div class="btn-group" role="group" aria-label="main-buttons">
                                <asp:LinkButton ID="lbtnBack" runat="server" CssClass="btn btn-default btnAjaxAction" OnClick="lbtnBack_Click" OnClientClick="return ProcessBackRequest(this.id);"
                                    data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("lbtnBack.Text"))%>'
                                    data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("lbtnBack.Text"))%>'>
                                    <span class="glyphicon glyphicon-arrow-left glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span><br />
                                    <%= GetLocalResourceObject("lbtnBack.Text") %>
                                </asp:LinkButton>
                            </div>
                        </div>
                        <div class="col-sm-6 text-right">
                            <div class="btn-group" role="group" aria-label="main-buttons">
                                <asp:LinkButton ID="lbtnNext" runat="server" CssClass="btn btn-default btnAjaxAction" OnClick="lbtnNext_Click" OnClientClick="return ProcessNextRequest(this.id);"
                                    data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("lbtnNext.Text"))%>'
                                    data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("lbtnNext.Text"))%>'>
                                    <span class="glyphicon glyphicon-arrow-right glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span><br />
                                    <%= GetLocalResourceObject("lbtnNext.Text") %>
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                    <br />
                    <br />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script type="text/javascript">
        function pageLoad(sender, args) {
            /// <summary>Execute at load even at partial and ajax requests</summary>
            $('.btnAjaxAction').on('click', function () {
                var $this = $(this);
                $this.button('loading');
                setTimeout(function () { $this.button('reset'); }, 30000);
            });
            //In this section we initialize the popovers and tooltips for the elements.
            //We have both (popovers and tooltips) in order to accomplish information alert over desktop 
            //and mobiles devices
            InitializeTooltipsPopovers();
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

            $('#<%= chkExtraIncome.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });
            $('#<%= chkPrincipalProvider.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                 off: '<%= GetLocalResourceObject("No") %>'
            });

            $('#<%= chkExtraIncome.ClientID %>').change(function () {
                var checkBoxValue = $(this).prop('checked');
                $('#<%= txtExtraIncomeComes.ClientID %>').prop('disabled', !checkBoxValue);
                $('#<%= txtExtraIncomeComes.ClientID %>').val("");
            });
            //
            $('.numericInputMask').inputmask('decimal', {
                rightAlign: true
                , groupSeparator: ','
                , autoGroup: true
                , showMaskOnFocus: false
                , showMaskOnHover: true
                , removeMaskOnSubmit: true
                , autoUnmask: true
                , maxlength: 11 
                , unmaskAsNumber: true
            });
        }
        function ProcessBackRequest(resetId) {
            /// <summary>Process the request for the back button</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            disableButton($('#<%= lbtnSaveAsDraft.ClientID %>'));
            disableButton($('#<%= lbtnNext.ClientID %>'));
            setTimeout(function () {
                ResetButton(resetId);
                enableButton($('#<%= lbtnSaveAsDraft.ClientID %>'));
                enableButton($('#<%= lbtnNext.ClientID %>'));
            }, 10000);
            return true;
        }
        function ProcessNextRequest(resetId) {
            /// <summary>Process the request for the next button</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            disableButton($('#<%= lbtnSaveAsDraft.ClientID %>'));
            disableButton($('#<%= lbtnBack.ClientID %>'));

            if(!ValidateSurvey()){
                MostrarMensaje(TipoMensaje.VALIDACION, '<%= GetLocalResourceObject("msgInvalidEntry") %>', function () {
                    $('#<%= lbtnNext.ClientID %>').button('reset');                        
                    enableButton($('#<%= lbtnSaveAsDraft.ClientID %>'));
                    enableButton($('#<%= lbtnBack.ClientID %>'));
                    return false;
                });
                return false;
            }
            setTimeout(function () {
                ResetButton(resetId);
                enableButton($('#<%= lbtnSaveAsDraft.ClientID %>'));
                enableButton($('#<%= lbtnBack.ClientID %>'));
            }, 10000);
            return true;
        }
        function ProcessSaveAsDraftRequest(resetId) {
            /// <summary>Process the request for the save as draft button</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            disableButton($('#<%= lbtnBack.ClientID %>'));
            disableButton($('#<%= lbtnNext.ClientID %>'));            
            return true;
        }
        function ProcessSaveAsDraftResponse() {
            /// <summary>Process the response for the employee search</summary>
            setTimeout(function () {                
                ResetButton($('#<%= lbtnSaveAsDraft.ClientID %>').id);
                enableButton($('#<%= lbtnBack.ClientID %>'));
                enableButton($('#<%= lbtnNext.ClientID %>'));
            }, 200);
        }        
        function SetControlInvalid(controlId) {
            /// <summary>Set the appearance of the control as invalid </summary>
            /// <param name="controlId" type="String">Id of the control</param>            
            if ($("#"+controlId).is(".selectpicker")) {                 
                $('button[data-id='+controlId+'].dropdown-toggle').addClass("Invalid"); 
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
            if ($("#"+controlId).is(".selectpicker")) {                
                $('button[data-id='+controlId+'].dropdown-toggle').removeClass("Invalid");
                $('#' + controlId).removeClass("Invalid");
                $('label[for=' + controlId + '].label-validation').hide();
            }
            else {
                $('#' + controlId).removeClass("Invalid");
                $('label[for=' + controlId + '].label-validation').hide();
            }
        }
        var validatorSurvey = null;
        function ValidateSurvey() {
            /// <summary>Validate the form with jquey validation plugin </summary>
            /// <returns> True if form is valid. False otherwise. </returns>
            $('#' + document.forms[0].id).validate().destroy();

            //add custom validation methods
            jQuery.validator.addMethod("validSelection", function (value, element) {
                return this.optional(element) || value != "-1";
            }, "Please select a valid option");

            if (validatorSurvey == null) {               
                //declare the validator
                var validatorSurvey =
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
                            <%= txtExtraIncomeComes.UniqueID %>: {
                                required: {
                                    depends: function(element) {                                        
                                        return $('#<%=chkExtraIncome.ClientID%>').prop('checked');
                                    }
                                }
                                , normalizer: function(value) {                                 
                                    return $.trim(value);
                                    }
                                , minlength: 0
                                , maxlength: 65
                            },
                            <%= txtFeeding.UniqueID %>: {
                                required: true
                                , normalizer: function(value) {                                 
                                    return $.trim(value);
                                    }
                                , minlength: 1
                                , maxlength: 14
                                , min: 0
                                , max: 99999999999
                            },
                            <%= txtElectricity.UniqueID %>: {
                                required: true
                                , normalizer: function(value) {                                 
                                    return $.trim(value);
                                }
                                , minlength: 1
                                , maxlength: 14
                                , min: 0
                                , max: 99999999999
                            },
                            <%= txtCableTv.UniqueID %>: {
                                required: true
                                , normalizer: function(value) {                                 
                                    return $.trim(value);
                                }
                                , minlength: 1
                                , maxlength: 14
                                , min: 0
                                , max: 99999999999
                            },
                            <%= txtLivingPlace.UniqueID %>: {
                                required: true
                                , normalizer: function(value) {                                 
                                    return $.trim(value);
                                }
                                , minlength: 1
                                , maxlength: 14
                                , min: 0
                                , max: 99999999999
                            },
                            <%= txtTransport.UniqueID %>: {
                                required: true
                                , normalizer: function(value) {                                 
                                    return $.trim(value);
                                }
                                , minlength: 1
                                , maxlength: 14
                                , min: 0
                                , max: 99999999999
                            },
                            <%= txtInternet.UniqueID %>: {
                                required: true
                                , normalizer: function(value) {                                 
                                    return $.trim(value);
                                }
                                , minlength: 1
                                , maxlength: 14
                                , min: 0
                                , max: 99999999999
                            },
                            <%= txtClothing.UniqueID %>: {
                                required: true
                                , normalizer: function(value) {                                 
                                    return $.trim(value);
                                }
                                , minlength: 1
                                , maxlength: 14
                                , min: 0
                                , max: 99999999999
                            },
                            <%= txtWater.UniqueID %>: {
                                required: true
                                , normalizer: function(value) {                                 
                                    return $.trim(value);
                                }
                                , minlength: 1
                                , maxlength: 14
                                , min: 0
                                , max: 99999999999
                            },
                            <%= txtEducation.UniqueID %>: {
                                required: true
                                , normalizer: function(value) {                                 
                                    return $.trim(value);
                                }
                                , minlength: 1
                                , maxlength: 14
                                , min: 0
                                , max: 99999999999
                            },
                            <%= txtPhone.UniqueID %>: {
                                required: true
                                , normalizer: function(value) {                                 
                                    return $.trim(value);
                                }
                                , minlength: 1
                                , maxlength: 14
                                , min: 0
                                , max: 99999999999
                            },
                            <%= txtGas.UniqueID %>: {
                                required: true
                                , normalizer: function(value) {                                 
                                    return $.trim(value);
                                }
                                , minlength: 1
                                , maxlength: 14
                                , min: 0
                                , max: 99999999999
                            },
                            <%= txtDoctor.UniqueID %>: {
                                required: true
                                , normalizer: function(value) {                                 
                                    return $.trim(value);
                                }
                                , minlength: 1
                                , maxlength: 14
                                , min: 0
                                , max: 99999999999
                            },
                            <%= txtLoan.UniqueID %>: {
                                required: true
                                , normalizer: function(value) {                                 
                                    return $.trim(value);
                                }
                                , minlength: 1
                                , maxlength: 14
                                , min: 0
                                , max: 99999999999
                            },
                            <%= txtLegalDeductions.UniqueID %>: {
                                required: true
                                , normalizer: function(value) {                                 
                                    return $.trim(value);
                                }
                                , minlength: 1
                                , maxlength: 14
                                , min: 0
                                , max: 99999999999
                            },
                            <%= txtOtherExpenses.UniqueID %>: {
                                required: true
                                , normalizer: function(value) {                                 
                                    return $.trim(value);
                                }
                                , minlength: 1
                                , maxlength: 14
                                , min: 0
                                , max: 99999999999
                            },
                            <%= txtLoanDescription.UniqueID %>: {
                                required: {
                                    depends: function(element) {                                        
                                        return $('#<%=txtLoan.ClientID%>').val() !== '' && parseFloat($('#<%=txtLoan.ClientID%>').val()) > 0;
                                    }
                                }
                                , normalizer: function(value) {                                 
                                    return $.trim(value);
                                    }
                                , minlength: 0
                                , maxlength: 60
                            },
                            <%= txtLegalDeductionsDescription.UniqueID %>: {
                                required: {
                                    depends: function(element) {                                        
                                        return $('#<%=txtLegalDeductions.ClientID%>').val() !== '' && parseFloat($('#<%=txtLegalDeductions.ClientID%>').val()) > 0;
                                    }
                                }
                                , normalizer: function(value) {                                 
                                    return $.trim(value);
                                    }
                                , minlength: 0
                                , maxlength: 60
                            },
                            <%= txtOtherExpensesDescription.UniqueID %>: {
                                required: {
                                    depends: function(element) {                                        
                                        return $('#<%=txtOtherExpenses.ClientID%>').val() !== '' && parseFloat($('#<%=txtOtherExpenses.ClientID%>').val()) > 0;
                                    }
                                }
                                , normalizer: function(value) {                                 
                                    return $.trim(value);
                                    }
                                , minlength: 0
                                , maxlength: 60
                            }
                        }
                    });
            }
            //get the results            
            var result = validatorSurvey.form();
            return result;
        }
    </script>
</asp:Content>