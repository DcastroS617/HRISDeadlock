<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="HousingInformation.aspx.cs" Inherits="HRISWeb.SocialResponsability.HousingInformation" %>
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
                                        <a class="nav-link active" id="housinginformation-tab" data-toggle="tab" href="#housinginformation" role="tab" aria-controls="housinginformation" aria-selected="true"><%= GetLocalResourceObject("housinginformation-tab") %></a>
                                    </li>
                                </ul>
                                <div class="tab-content" id="survey">
                                    <div class="tab-pane fade in active" id="housinginformation" role="tabpanel" aria-labelledby="housinginformation-tab">
                                        <p>
                                            <br />
                                        </p>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-12">
                                                            <img alt="" src="../Content/images/bulb_icon.png" /> 
                                                            <asp:Label ID="lblHousingLocation" meta:resourcekey="lblHousingLocation" AssociatedControlID="cboProvince" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>                                        
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-3">
                                                            <img alt="" src="../Content/images/bulb_icon.png" /> 
                                                            <asp:Label ID="lblProvince" AssociatedControlID="cboProvince" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <img alt="" src="../Content/images/bulb_icon.png" /> 
                                                            <asp:Label ID="lblCanton" AssociatedControlID="cboCanton" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <img alt="" src="../Content/images/bulb_icon.png" /> 
                                                            <asp:Label ID="lblDistrict" AssociatedControlID="cboDistrict" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <img alt="" src="../Content/images/bulb_icon.png" /> 
                                                            <asp:Label ID="lblNeighborhood" AssociatedControlID="cboNeighborhood" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-3">
                                                            <asp:DropDownList ID="cboProvince" runat="server" CssClass="form-control selectpicker" data-live-search="true" AutoPostBack="true" OnSelectedIndexChanged="cboProvince_SelectedIndexChanged"></asp:DropDownList>
                                                            <asp:Label ID="cboProvinceValidation" runat="server" for="cboProvince" AssociatedControlID="cboProvince" Text="" CssClass="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</asp:Label>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:DropDownList ID="cboCanton" runat="server" CssClass="form-control selectpicker" data-live-search="true" AutoPostBack="true" OnSelectedIndexChanged="cboCanton_SelectedIndexChanged"></asp:DropDownList>
                                                            <asp:Label ID="cboCantonValidation" runat="server" for="cboCanton" AssociatedControlID="cboCanton" Text="" CssClass="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</asp:Label>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:DropDownList ID="cboDistrict" runat="server" CssClass="form-control selectpicker" data-live-search="true" AutoPostBack="true" OnSelectedIndexChanged="cboDistrict_SelectedIndexChanged"></asp:DropDownList>
                                                            <asp:Label ID="cboDistrictValidation" runat="server" for="cboDistrict" AssociatedControlID="cboDistrict" Text="" CssClass="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</asp:Label>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:DropDownList ID="cboNeighborhood" runat="server" CssClass="form-control selectpicker" data-live-search="true" AutoPostBack="false"></asp:DropDownList>
                                                            <asp:Label ID="cboNeighborhoodValidation" runat="server" for="cboNeighborhood" AssociatedControlID="cboNeighborhood" Text="" CssClass="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</asp:Label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                     <div class="col-sm-3">
                                                         <img alt="" src="../Content/images/bulb_icon.png" /> 
                                                            <asp:Label ID="lblDirection" AssociatedControlID="txtDirection" runat="server" Text="" CssClass="control-label text-left"><%= GetLocalResourceObject("lblDirection.Text") %></asp:Label>
                                                     </div>
                                                     <div class="col-sm-9">
                                                            <asp:TextBox ID="txtDirection" meta:resourcekey="txtDirection" runat="server" CssClass="form-control control-validation cleanPasteText" MaxLength="255" onkeypress="blockEnterKey();return isNumberOrLetter(event);" placeholder="" ></asp:TextBox>
                                                            <label id="txtDirectionValidation" for="<%= txtDirection.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgDirectionValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </div>
                                                </div>
                                            </div>
                                        </div>
                                       <div>
                                           <p>
                                               
                                           </p>
                                       </div>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-2 col-sm-offset-1">
                                                            <img alt="" src="../Content/images/bulb_icon.png" /> 
                                                            <asp:Label ID="lblSector" meta:resourcekey="lblSector" AssociatedControlID="cboSector" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:DropDownList ID="cboSector" runat="server" CssClass="form-control selectpicker" data-live-search="true" AutoPostBack="false"></asp:DropDownList>
                                                            <label id="cboSectorValidation" for="<%= cboSector.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgSectorValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-3">
                                                            <asp:Label ID="lblWorkerLivesOnFarm" meta:resourcekey="lblWorkerLivesOnFarm" AssociatedControlID="chkWorkerLivesOnFarm" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:CheckBox ID="chkWorkerLivesOnFarm" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>' AutoPostBack="false"></asp:CheckBox>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:Label ID="lblHousingType" meta:resourcekey="lblHousingType" AssociatedControlID="cboHousingType" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:DropDownList ID="cboHousingType" runat="server" CssClass="form-control selectpicker" data-live-search="true" AutoPostBack="false"></asp:DropDownList>
                                                            <label id="cboHousingTypeValidation" for="<%= cboHousingType.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgHousingTypeValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-3">
                                                            <asp:Label ID="lblHousingTenure" meta:resourcekey="lblHousingTenure" AssociatedControlID="cboHousingTenure" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:DropDownList ID="cboHousingTenure" runat="server" CssClass="form-control selectpicker" data-live-search="true" AutoPostBack="false"></asp:DropDownList>
                                                            <label id="cboHousingTenureValidation" for="<%= cboHousingTenure.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgHousingTenureValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:Label ID="lblAcquisitionWay" meta:resourcekey="lblAcquisitionWay" AssociatedControlID="cboAcquisitionWay" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:DropDownList ID="cboAcquisitionWay" runat="server" CssClass="form-control selectpicker" data-live-search="true" AutoPostBack="false"></asp:DropDownList>
                                                            <label id="cboAcquisitionWayValidation" for="<%= cboAcquisitionWay.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgAcquisitionWayValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
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
                                                            <asp:Label ID="lblLegalDocToItsName" meta:resourcekey="lblLegalDocToItsName" AssociatedControlID="chkLegalDocToItsName" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:CheckBox ID="chkLegalDocToItsName" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>' AutoPostBack="false"></asp:CheckBox>
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
                                                            <asp:Label ID="lblAdditionalTerrain" meta:resourcekey="lblAdditionalTerrain" AssociatedControlID="chkAdditionalTerrain" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:CheckBox ID="chkAdditionalTerrain" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>' AutoPostBack="false"></asp:CheckBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-4 col-sm-offset-1">
                                                            <asp:Label ID="lblDescriptionAdditionalTerrain" meta:resourcekey="lblDescriptionAdditionalTerrain" AssociatedControlID="txtDescriptionAdditionalTerrain" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtDescriptionAdditionalTerrain" meta:resourcekey="txtDescriptionAdditionalTerrain" runat="server" CssClass="form-control control-validation cleanPasteText" MaxLength="80" onkeypress="blockEnterKey();return isNumberOrLetter(event);" placeholder="" disabled="disabled"></asp:TextBox>
                                                            <label id="txtDescriptionAdditionalTerrainValidation" for="<%= txtDescriptionAdditionalTerrain.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgDescriptionAdditionalTerrainValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
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

            $('#<%= chkWorkerLivesOnFarm.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });

            $('#<%= chkLegalDocToItsName.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });

            $('#<%= chkAdditionalTerrain.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });

            $('#<%= chkAdditionalTerrain.ClientID %>').change(function () {
                var checkBoxValue = $(this).prop('checked');
                $('#<%= txtDescriptionAdditionalTerrain.ClientID %>').prop('disabled', !checkBoxValue);
                if (!checkBoxValue) {
                    $('#<%= txtDescriptionAdditionalTerrain.ClientID %>').val("");
                }
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


              


              jQuery.validator.addMethod("validSelectionQuestion41", function (value, element) {
                  var TendenciaVivienda =parseInt( $("#<%=cboHousingTenure.ClientID%>").val());
                  // nueva validacion
                if ( TendenciaVivienda==2 ||  TendenciaVivienda==5 || TendenciaVivienda==6) {
                    return true
                } else {
                    return this.optional(element) || value != "-1";
                  }

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
                            <%= cboProvince.UniqueID %>: {
                                required: true
                                , validSelection: true
                            },
                            <%= cboCanton.UniqueID %>: {
                                required: true
                                , validSelection: true
                            },
                            <%= cboDistrict.UniqueID %>: {
                                required: true
                                , validSelection: true
                            },
                            <%= cboNeighborhood.UniqueID %>: {
                                required: true
                                , validSelection: true
                            },
                            <%= cboSector.UniqueID %>: {
                                required: true
                                , validSelection: true
                            },
                            <%= cboHousingType.UniqueID %>: {
                                required: true
                                , validSelection: true
                            },
                            <%= cboHousingTenure.UniqueID %>: {
                                required: true
                                , validSelection: true
                            },
                            <%= cboAcquisitionWay.UniqueID %>: {
                                required: true
                                , validSelectionQuestion41: true
                            },
                            "<%= txtDirection.UniqueID %>" : {
                                required:true
                            },
                            <%= txtDescriptionAdditionalTerrain.UniqueID %>: {
                                required: {
                                    depends: function(element) {
                                        return $("#<%= chkAdditionalTerrain.ClientID %>").prop('checked');
                                    }
                                }
                                , normalizer: function(value) {                                 
                                    return $.trim(value);
                                    }
                                , minlength: 0
                                , maxlength: 80
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