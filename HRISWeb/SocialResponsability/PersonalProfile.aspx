<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="PersonalProfile.aspx.cs" Inherits="HRISWeb.SocialResponsability.PersonalProfile" %>

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
                                        <a class="nav-link active" id="personalprofile-tab" data-toggle="tab" href="#personalprofile" role="tab" aria-controls="personalprofile" aria-selected="true"><%= GetLocalResourceObject("personalprofile-tab") %></a>
                                    </li>
                                </ul>
                                <div class="tab-content" id="survey">
                                    <div class="tab-pane fade in active" id="personalprofile" role="tabpanel" aria-labelledby="personalprofile-tab">
                                        <p>
                                            <br />
                                        </p>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-4">
                                                            <asp:Label ID="lblMaritalStatus" meta:resourcekey="lblMaritalStatus" AssociatedControlID="rdbMaritalStatus" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-7">                                                            
                                                            <asp:RadioButtonList ID="rdbMaritalStatus" runat="server" CssClass="control-validation" RepeatDirection="Horizontal" TextAlign="Right" CellSpacing="50" RepeatLayout="Table" Width="100%" CellPadding="50"></asp:RadioButtonList>
                                                            <label id="rdbMaritalStatusValidation" for="<%= rdbMaritalStatus.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjMaritalStatusValidation") %>" style="display:none; float:right;margin-right:0px;margin-top:-23px;position:relative;z-index: 2;">!</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lblSpouseWorks" meta:resourcekey="lblSpouseWorks" AssociatedControlID="chkSpouseWorks" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-4 text-left">
                                                            <asp:CheckBox ID="chkSpouseWorks" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                                        </div>                                                        
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-4 col-xs-offset-1">
                                                            <asp:Label ID="lblYesSpouseWorks" meta:resourcekey="lblYesSpouseWorks" AssociatedControlID="cboSpouseWorkplace" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-4">
                                                          
                                                            <select runat="server" id="cboSpouseWorkplace" meta:resourcekey="cboSpouseWorkplace" class="form-control cleanPasteText control-validation">
                                                               
                                                            </select>
                                                              <label id="txtSpouseWorkplaceValidation" for="<%= cboSpouseWorkplace.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjSpouseWorkplaceValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-3 col-xs-offset-2">
                                                            <asp:Label ID="lblWhatWorkDoes" meta:resourcekey="lblWhatWorkDoes" AssociatedControlID="cboWhatWorkDoes" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            
                                                            
                                                           <select runat="server" id="cboWhatWorkDoes" meta:resourcekey="cboWhatWorkDoes" class="form-control cleanPasteText control-validation">
                                                              
                                                            </select>
                                                             <label id="txtWhatWorkDoesValidation" for="<%= cboWhatWorkDoes.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjWhatWorkDoesValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-4 col-xs-offset-1">
                                                            <asp:Label ID="lblNoSpouseWorks" meta:resourcekey="lblNoSpouseWorks" AssociatedControlID="txtWhatDoForLiving" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtWhatDoForLiving" meta:resourcekey="txtWhatDoForLiving" runat="server" CssClass="form-control cleanPasteText control-validation" onkeypress="blockEnterKey();return isNumberOrLetter(event);" MaxLength="100" placeholder=""></asp:TextBox>
                                                            <label id="txtWhatDoForLivingValidation" for="<%= txtWhatDoForLiving.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjWhatDoForLivingValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lblMainSupplier" meta:resourcekey="lblMainSupplier" AssociatedControlID="chkMainSupplier" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-3 text-left">
                                                            <asp:CheckBox ID="chkMainSupplier" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lblChildrenNumber" meta:resourcekey="lblChildrenNumber" AssociatedControlID="cboChildrenNumber" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <asp:DropDownList ID="cboChildrenNumber" runat="server" CssClass="form-control selectpicker" data-live-search="true" AutoPostBack="false"></asp:DropDownList>
                                                            <label id="cboChildrenNumberValidation" for="<%= cboChildrenNumber.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjChildrenNumberValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">                                            
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lblChildrenLivingOutsideHome" meta:resourcekey="lblChildrenLivingOutsideHome" AssociatedControlID="cboChildrenLivingOutsideHome" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <asp:DropDownList ID="cboChildrenLivingOutsideHome" runat="server" CssClass="form-control selectpicker" data-live-search="true" AutoPostBack="false"></asp:DropDownList>
                                                            <label id="cboChildrenLivingOutsideHomeValidation" for="<%= cboChildrenLivingOutsideHome.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjChildrenLivingOutsideHomeValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">                                            
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lblNativeLanguage" meta:resourcekey="lblNativeLanguage" AssociatedControlID="cboNativeLanguage" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <asp:DropDownList ID="cboNativeLanguage" runat="server" CssClass="form-control selectpicker control-validation" data-live-search="true" AutoPostBack="false"></asp:DropDownList>
                                                            <label id="cboNativeLanguageValidation" for="<%= cboNativeLanguage.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjNativeMotherLanguageValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
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
                    <br /><hr/>
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
                    <br /><br />
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

            $('#<%= chkSpouseWorks.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });
            $('#<%= chkMainSupplier.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });                        
            $('#<%= chkSpouseWorks.ClientID %>').change(function () {
                ConfigureQuestionSpouseWorks();
            });

            $("#<%= rdbMaritalStatus.ClientID %> input").change(function () {
                ConfigureByMaritalStatus();
            });

            $("#<%= cboChildrenNumber.ClientID %>").on('changed.bs.select', function (e) {
                /// <summary>Handles the select's value changed event for the cboChildrenNumber control.</summary>                
                ConfigureNumberOfChildren();
            });
        }

        function ConfigureNumberOfChildren(){
            /// <summary>Enable or disable the question #12 according of response of question 11</summary>
            var selectedItem = $("#<%= cboChildrenNumber.ClientID %>").selectpicker('val');
            if(selectedItem == '0'){
                $('#<%= cboChildrenLivingOutsideHome.ClientID %>').selectpicker('val', '0');              
            }
            $('#<%= cboChildrenLivingOutsideHome.ClientID %>').attr("disabled", selectedItem == '0' ? true : false); 
            $("#<%= cboChildrenLivingOutsideHome.ClientID %>").selectpicker('refresh');
        }

        function SetControlInvalid(controlId) {
            /// <summary>Set the appearance of the control as invalid </summary>
            /// <param name="controlId" type="String">Id of the control</param>            
            if ($("#"+controlId).is(".selectpicker")) {                 
                $('button[data-id='+controlId+'].dropdown-toggle').addClass("Invalid"); 
                $('#' + controlId).addClass("Invalid");            
                $('label[for=' + controlId + '].label-validation').show();
            }
            else if ($("#"+controlId).is(":radio")) {                                                 
                $('#<%= rdbMaritalStatus.ClientID %>').addClass("Invalid");            
                $('label[for=' + '<%= rdbMaritalStatus.ClientID %>' + '].label-validation').show();
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
            else if ($("#"+controlId).is(":radio")) {
                $('#<%= rdbMaritalStatus.ClientID %>').removeClass("Invalid");            
                $('label[for=' + '<%= rdbMaritalStatus.ClientID %>' + '].label-validation').hide();
            }
            else {
                $('#' + controlId).removeClass("Invalid");
                $('label[for=' + controlId + '].label-validation').hide();
            }
        }

        function ConfigureQuestionSpouseWorks() {
            /// <summary>Configure the screen controls for the question by the spouse work value</summary>
            var spouseWorks = $('#<%= chkSpouseWorks.ClientID %>').prop('checked');
             var maritalStatusCode = $("#<%= rdbMaritalStatus.ClientID %> input:checked").val();
            var disableControls = maritalStatusCode == '2' || maritalStatusCode == '6' ? false : true;
            
            if (spouseWorks) {
                $('#<%= cboSpouseWorkplace.ClientID %>').prop('disabled', false);
                $('#<%= cboWhatWorkDoes.ClientID %>').prop('disabled', false);
                $('#<%= txtWhatDoForLiving.ClientID %>').prop('disabled', true);
                $('#<%= txtWhatDoForLiving.ClientID %>').val('');

                $('#<%= txtWhatDoForLiving.ClientID %>').removeClass("Invalid");
                $('label[for=' + '<%= txtWhatDoForLiving.ClientID %>' + '].label-validation').hide();
            }
            else {

                $('#<%= cboSpouseWorkplace.ClientID %>').prop('disabled', true);
                $('#<%= cboWhatWorkDoes.ClientID %>').prop('disabled', true);
                $('#<%= txtWhatDoForLiving.ClientID %>').prop('disabled', false);
                $('#<%= cboSpouseWorkplace.ClientID %>').val('');
                $('#<%= cboWhatWorkDoes.ClientID %>').val('');

                $('#<%= cboSpouseWorkplace.ClientID %>').removeClass("Invalid");
                $('label[for=' + '<%= cboSpouseWorkplace.ClientID %>' + '].label-validation').hide();

                $('#<%= cboWhatWorkDoes.ClientID %>').removeClass("Invalid");
                $('label[for=' + '<%= cboWhatWorkDoes.ClientID %>' + '].label-validation').hide();
            }
             

        }

        function ConfigureByMaritalStatus(){
            var maritalStatusCode = $("#<%= rdbMaritalStatus.ClientID %> input:checked").val();
            var disableControls = maritalStatusCode == '2' || maritalStatusCode == '6' ? false : true;
            if(disableControls){
                $('#<%= chkSpouseWorks.ClientID %>').bootstrapToggle('off');                                
                $('#<%= txtWhatDoForLiving.ClientID %>').val('');
                $('#<%= txtWhatDoForLiving.ClientID %>').prop('disabled', disableControls);
                $('#<%= txtWhatDoForLiving.ClientID %>').removeClass("Invalid");
                $('label[for=' + '<%= txtWhatDoForLiving.ClientID %>' + '].label-validation').hide();
            }
            else if(maritalStatusCode !== undefined){
                ConfigureQuestionSpouseWorks();
            }
            
            $('#<%= chkSpouseWorks.ClientID %>').bootstrapToggle(disableControls ? 'disable':'enable');
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
            setTimeout(function () {
                ConfigureByMaritalStatus()
            },500);

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
            jQuery.validator.addMethod("validSpouseWorkplace", function (value, element) {
                return ValidateSpouseWorkplace();
            }, "You must indicate where does he or she works");
            jQuery.validator.addMethod("validWhatWorkDoes", function (value, element) {
                return ValidateSpouseWorkDoes();
            }, "You must indicate what kind of work he or she does");
            jQuery.validator.addMethod("validWhatDoesForLiving", function (value, element) {
                return ValidateSpouseWhatDoesForLiving();
            }, "You must indicate what do he or she does");
            jQuery.validator.addMethod("validCivilStatus", function (value, element) {
                return ValidateCivilStatus();
            }, "You must select a valid civil status");
            jQuery.validator.addMethod("validChildrenLivingOutsideHomeVsChildrenNum", function (value, element) {
                return ValidateChildrenLivingOutsideHomeVsChildrenNum();
            }, "You must select a number of children who live outside the home minor or equals than the number of children");

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
                        errorPlacement: function (error, element) { 
                            if ( element.is(":radio") )   
                            {  
                                error.appendTo( element.parents('.radioclass') );  
                            }
                        },
                        rules: {
                            <%= rdbMaritalStatus.UniqueID %>: {
                                validCivilStatus: true
                                , required:true
                            },
                            <%= cboSpouseWorkplace.UniqueID %>: {
                                validSpouseWorkplace: true
                            },
                            <%= cboWhatWorkDoes.UniqueID %>: {
                                validWhatWorkDoes: true
                            },
                            <%= txtWhatDoForLiving.UniqueID %>: {
                                validWhatDoesForLiving: true
                            },
                            <%= cboChildrenNumber.UniqueID %>: {
                                required: true
                                , validSelection: true
                            },
                            <%= cboChildrenLivingOutsideHome.UniqueID %>: {
                                required: true
                                , validSelection: true
                                , validChildrenLivingOutsideHomeVsChildrenNum: true
                            },
                            <%= cboNativeLanguage.UniqueID %>: {
                                required: true
                                , validSelection: true
                            }
                        }
                    });
            }
            //else
            //{
            //    validatorSurvey.validate();
            //}
            //get the results            
            var result = validatorSurvey.form();
            return result;
        }
        function ValidateCivilStatus(){
            /// <summary>Validate the civil status</summary>            
            /// <returns> True if is valid. False otherwise. </returns>
            var civilStatusSelectedValue = $("#<%= rdbMaritalStatus.ClientID %> input:checked").val();
            if(civilStatusSelectedValue == null || civilStatusSelectedValue.length === 0 || civilStatusSelectedValue === ''){                
                return false
            }
            return true;
        }
        function ValidateSpouseWorkplace(){
            /// <summary>Validate the spouse workplace</summary>            
            /// <returns> True if is valid. False otherwise. </returns>
            var spouseWorks = $('#<%= chkSpouseWorks.ClientID %>').prop('checked');
            if (spouseWorks) {
                var spouseWorkPlace = $.trim($('#<%= cboSpouseWorkplace.ClientID %>').val());
                if(spouseWorkPlace == null || spouseWorkPlace.length === 0 || spouseWorkPlace === ''){
                    return false
                }
            }
            return true;
        }
        function ValidateSpouseWorkDoes(){
            /// <summary>Validate the spouse what work does</summary>            
            /// <returns> True if is valid. False otherwise. </returns>
            var spouseWorks = $('#<%= chkSpouseWorks.ClientID %>').prop('checked');
            if (spouseWorks) {
                var spouseWorkDoes = $.trim($('#<%= cboWhatWorkDoes.ClientID %>').val());
                if(spouseWorkDoes == null || spouseWorkDoes.length === 0 || spouseWorkDoes === ''){
                    return false
                }
            }
            return true;
        }
        function ValidateSpouseWhatDoesForLiving(){
            /// <summary>Validate the spouse what does for living</summary>            
            /// <returns> True if is valid. False otherwise. </returns>
            var spouseWorks = $('#<%= chkSpouseWorks.ClientID %>').prop('checked');
            if (!spouseWorks) {
                var spouseWhatkDoesForLiving = $.trim($('#<%= txtWhatDoForLiving.ClientID %>').val());
                if(spouseWhatkDoesForLiving == null || spouseWhatkDoesForLiving.length === 0 || spouseWhatkDoesForLiving === ''){
                    return false
                }
            }
            return true;
        }
        function ValidateChildrenLivingOutsideHomeVsChildrenNum(){
            /// <summary>Validate the number of children that live outside home must be minor or equal than chindren number</summary>            
            /// <returns> True if is valid. False otherwise. </returns>
            var childrenNumber = $('#<%= cboChildrenNumber.ClientID %>').selectpicker('val');
            if (childrenNumber === null || childrenNumber.length === 0 || childrenNumber === '-1') {
                return false;
            }
            var childrenOutsideHome = $('#<%= cboChildrenLivingOutsideHome.ClientID %>').selectpicker('val');
            if (childrenOutsideHome === null || childrenOutsideHome.length === 0 || childrenOutsideHome === '-1') {
                return false;
            }
            else if(parseInt(childrenNumber) < parseInt(childrenOutsideHome)){
                return false;
            }
            return true;
        }
    </script>
</asp:Content>