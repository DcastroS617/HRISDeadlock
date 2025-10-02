<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="HousingDistribution.aspx.cs" Inherits="HRISWeb.SocialResponsability.HousingDistribution" %>
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
                                        <a class="nav-link active" id="housingdistribution-tab" data-toggle="tab" href="#housingdistribution" role="tab" aria-controls="housingdistribution" aria-selected="true"><%= GetLocalResourceObject("housingdistribution-tab") %></a>
                                    </li>
                                </ul>
                                <div class="tab-content" id="survey">
                                    <div class="tab-pane fade in active" id="housingdistribution" role="tabpanel" aria-labelledby="housingdistribution-tab">
                                        <p>
                                            <br />
                                        </p>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-3">
                                                            <img alt="" src="../Content/images/bulb_icon.png" /> 
                                                            <asp:Label ID="lblHousingDistribution" meta:resourcekey="lblHousingDistribution" AssociatedControlID="cboHousingDistribution" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <asp:DropDownList ID="cboHousingDistribution" runat="server" CssClass="form-control selectpicker" data-live-search="true" AutoPostBack="false" multiple="multiple" data-selected-text-format="count > 5"></asp:DropDownList>
                                                            <label id="cboHousingDistributionValidation" for="<%= cboHousingDistribution.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgHousingDistributionValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                            <asp:HiddenField ID="hdfSelectedHousingDistributions" runat="server" />
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <span class="glyphicon glyphicon-info-sign text-center" data-toggle="tooltip" title='<%=GetLocalResourceObject("MultiSelectionDropDownListTooltip")%>' style="cursor:pointer"></span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>                                        
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-2 col-sm-offset-1">
                                                            <img alt="" src="../Content/images/bulb_icon.png" /> 
                                                            <asp:Label ID="lblBedroomsQuantity" meta:resourcekey="lblBedroomsQuantity" AssociatedControlID="cboBedroomsQuantity" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:DropDownList ID="cboBedroomsQuantity" runat="server" CssClass="form-control selectpicker" data-live-search="true" AutoPostBack="false"></asp:DropDownList>
                                                            <label id="cboBedroomsQuantityValidation" for="<%= cboBedroomsQuantity.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgBedroomsQuantityValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <div class="col-xs-1 ml-auto mt-auto mb-auto">
                                                                <img  src="../Content/images/bulb_icon.png" />
                                                            </div>
                                                            <div class="col-sm-10 mr-auto mt-auto mb-auto">
                                                                <asp:Label ID="lblBathroomsQuantity" meta:resourcekey="lblBathroomsQuantity" AssociatedControlID="cboBathroomsInternalQuantity" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:DropDownList ID="cboBathroomsInternalQuantity" runat="server" CssClass="form-control selectpicker" data-live-search="true" AutoPostBack="false"></asp:DropDownList>
                                                            <label id="cboBathroomsInternalQuantityValidation" for="<%= cboBathroomsInternalQuantity.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgBathroomsInternalQuantityValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                         <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-2 col-sm-offset-1">
                                                            <div class="col-xs-1 ml-auto mt-auto mb-auto">
                                                                <img  src="../Content/images/bulb_icon.png" />
                                                            </div>
                                                            <div class="col-sm-10 mr-auto mt-auto mb-auto">
                                                                <asp:Label ID="lblBatroomsExternal" meta:resourcekey="lblBatroomsExternal" AssociatedControlID="cboBathroomsExternalQuantity" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                            </div>
                                                        </div>                                                        
                                                        <div class="col-sm-3">
                                                            <asp:DropDownList ID="cboBathroomsExternalQuantity" runat="server" CssClass="form-control selectpicker" data-live-search="true" AutoPostBack="false"></asp:DropDownList>
                                                            <label id="cboBathroomsExternalQuantityValidation" for="<%= cboBathroomsExternalQuantity.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgBathroomsExternalQuantityValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-12">
                                                            <img alt="" src="../Content/images/bulb_icon.png" /> 
                                                            <asp:Label ID="lblTypeOfHousingConstruction" meta:resourcekey="lblTypeOfHousingConstruction" AssociatedControlID="cboFloor" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>                                                        
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-2 col-sm-offset-1">
                                                            <img alt="" src="../Content/images/bulb_icon.png" /> 
                                                            <asp:Label ID="lblFloor" meta:resourcekey="lblFloor" AssociatedControlID="cboFloor" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:DropDownList ID="cboFloor" runat="server" CssClass="form-control selectpicker" data-live-search="true" AutoPostBack="false"></asp:DropDownList>
                                                            <label id="cboFloorValidation" for="<%= cboFloor.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgFloorValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </div>                                                       
                                                    </div>
                                                </div>
                                            </div>
                                        </div>                                        
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-2 col-sm-offset-1">
                                                            <img alt="" src="../Content/images/bulb_icon.png" /> 
                                                            <asp:Label ID="lblWalls" meta:resourcekey="lblWalls" AssociatedControlID="cboWalls" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:DropDownList ID="cboWalls" runat="server" CssClass="form-control selectpicker" data-live-search="true" AutoPostBack="false"></asp:DropDownList>
                                                            <label id="cboWallsValidation" for="<%= cboWalls.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgWallsValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-2 col-sm-offset-1">
                                                            <img alt="" src="../Content/images/bulb_icon.png" /> 
                                                            <asp:Label ID="lblHouseRoof" meta:resourcekey="lblHouseRoof" AssociatedControlID="cboHouseRoof" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:DropDownList ID="cboHouseRoof" runat="server" CssClass="form-control selectpicker" data-live-search="true" AutoPostBack="false"></asp:DropDownList>
                                                            <label id="cboHouseRoofValidation" for="<%= cboHouseRoof.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgHouseRoofValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>                                        
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-2 col-sm-offset-1">
                                                            <img alt="" src="../Content/images/bulb_icon.png" /> 
                                                            <asp:Label ID="lblCeiling" meta:resourcekey="lblCeiling" AssociatedControlID="chkCeiling" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:CheckBox ID="chkCeiling" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>' AutoPostBack="false"></asp:CheckBox>
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

        const otherFloorCode = "6";
        const otherHouseRoofMaterialCode = "6";
        const mixWallMaterialCode = "10";
        const otherWallMaterialCode = "11";
        const internalBathroom = "6";
        const externalBathroom = "7";

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

            $("#<%= cboHousingDistribution.ClientID %>").on('changed.bs.select', function (e) {
                /// <summary>Handles the select's value changed event for the cboTransportMeansThatHas control.</summary>
                MultiSelectDropdownListSaveSelectedItems($("#<%= cboHousingDistribution.ClientID %>"), $("#<%= hdfSelectedHousingDistributions.ClientID %>"));

                if ($("#<%= cboHousingDistribution.ClientID %>").val().includes(internalBathroom)) {
                    $('#<%= cboBathroomsInternalQuantity.ClientID %>').prop('disabled', false);
                    SetControlValid($('#<%= cboBathroomsInternalQuantity.ClientID %>').attr("id"));
                } else {
                    $('#<%= cboBathroomsInternalQuantity.ClientID %>').val("-1");
                    $('#<%= cboBathroomsInternalQuantity.ClientID %>').prop('disabled', true);
                    
                }

                if ($("#<%= cboHousingDistribution.ClientID %>").val().includes(externalBathroom)) {
                    $('#<%= cboBathroomsExternalQuantity.ClientID %>').prop('disabled', false);
                    SetControlValid($('#<%= cboBathroomsExternalQuantity.ClientID %>').attr("id"));
                } else {
                    $('#<%= cboBathroomsExternalQuantity.ClientID %>').val("-1");
                    $('#<%= cboBathroomsExternalQuantity.ClientID %>').prop('disabled', true);                    
                }

                $('#<%= cboBathroomsInternalQuantity.ClientID %>').selectpicker('refresh');
                $('#<%= cboBathroomsExternalQuantity.ClientID %>').selectpicker('refresh');
            });     

            $('#<%= chkCeiling.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });

            RestoreSelectedHousingDistributions();
        }
        function RestoreSelectedHousingDistributions() {
            /// <summary>Recover the selected items that were stored and assign them to the control to be selected.</summary>
            MultiSelectDropdownListRestoreSelectedItems($('#<%= cboHousingDistribution.ClientID %>'), $('#<%= hdfSelectedHousingDistributions.ClientID %>'));
                        
            SetControlValid($('#<%= cboBathroomsExternalQuantity.ClientID %>').attr("id"));
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
                            <%= cboHousingDistribution.UniqueID %>: {
                                required: true
                                , validSelection: true
                            },
                            <%= cboBedroomsQuantity.UniqueID %>: {
                                required: true
                                , validSelection: true
                            },
                            <%= cboBathroomsInternalQuantity.UniqueID %>: {
                                required: true
                                , validSelection: true
                            },
                            <%= cboBathroomsExternalQuantity.UniqueID %>: {
                                required: true
                                , validSelection: true
                            },
                            <%= cboFloor.UniqueID %>: {
                                required: true
                                , validSelection: true
                            },
                            <%= cboWalls.UniqueID %>: {
                                required: true
                                , validSelection: true
                            },
                            <%= cboHouseRoof.UniqueID %>: {
                                required: true
                                , validSelection: true
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