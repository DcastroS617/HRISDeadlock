<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="MaterialWellness.aspx.cs" Inherits="HRISWeb.SocialResponsability.MaterialWellness" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntHeader" runat="server">
    <title><%=Convert.ToString(GetLocalResourceObject("lblScreenTitle")) %></title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cntBody" runat="server">
    <style>
        .bootstrap-select > .dropdown-toggle.bs-placeholder, .bootstrap-select > .dropdown-toggle.bs-placeholder:active,
        .bootstrap-select > .dropdown-toggle.bs-placeholder:focus, .bootstrap-select > .dropdown-toggle.bs-placeholder:hover {
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
                                        <a class="nav-link active" id="materialwellness-tab" data-toggle="tab" href="#materialwellness" role="tab" aria-controls="materialwellness" aria-selected="true"><%= GetLocalResourceObject("materialwellness-tab") %></a>
                                    </li>
                                </ul>

                                <div class="tab-content" id="survey">
                                    <div class="tab-pane fade in active" id="materialwellness" role="tabpanel" aria-labelledby="materialwellness-tab">
                                        <p>
                                            <br />
                                        </p>

                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-4">
                                                            <asp:Label ID="lblTransportMeansThatHas" meta:resourcekey="lblTransportMeansThatHas" AssociatedControlID="cboTransportMeansThatHas" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>

                                                        <div class="col-sm-7">
                                                            <asp:DropDownList ID="cboTransportMeansThatHas" runat="server" CssClass="form-control selectpicker" data-live-search="true" multiple="multiple" data-selected-text-format="count > 5" AutoPostBack="false"></asp:DropDownList>
                                                            <label id="cboTransportMeansThatHasValidation" for="<%= cboTransportMeansThatHas.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgTransportMeansThatHasValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                            <asp:HiddenField ID="hdfTransportMeansThatEmployeeHas" runat="server" />
                                                        </div>

                                                        <div class="col-sm-1">
                                                            <span class="glyphicon glyphicon-info-sign text-center" data-toggle="tooltip" title='<%=GetLocalResourceObject("MultiSelectionDropDownListTooltip")%>' style="cursor: pointer"></span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-4">
                                                            <asp:Label ID="lblTransportationForWorkplace" meta:resourcekey="lblTransportationForWorkplace" AssociatedControlID="cboTransportMeansThatHas" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>

                                                        <div class="col-sm-7">
                                                            <asp:DropDownList ID="cboTransportationForWorkplace" runat="server" CssClass="form-control selectpicker" data-live-search="true" multiple="multiple" data-selected-text-format="count > 6" AutoPostBack="false"></asp:DropDownList>
                                                            <label id="cboTransportationForWorkplaceValidation" for="<%= cboTransportationForWorkplace.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgTransportationForWorkplaceValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                            <asp:HiddenField ID="hdfTransportationForWorkplace" runat="server" />
                                                        </div>

                                                        <div class="col-sm-1">
                                                            <span class="glyphicon glyphicon-info-sign text-center" data-toggle="tooltip" title='<%=GetLocalResourceObject("MultiSelectionDropDownListTooltip")%>' style="cursor: pointer"></span>
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

            let validateSelection = true;
            let selectOptionNone = false;

            $("#<%= cboTransportMeansThatHas.ClientID %>").on('changed.bs.select', function (e) {
                /// <summary>Handles the select's value changed event for the cboTransportMeansThatHas control.</summary> 
                if (validateSelection === true) {
                    var selectedTransportMeans = $("#<%= cboTransportMeansThatHas.ClientID %>").selectpicker('val');
                    if (selectedTransportMeans.length > 0) {
                        for (let i = 0; i < selectedTransportMeans.length; i++) {
                            if (selectedTransportMeans[i] === '5') {
                                validateSelection = false;
                                selectOptionNone = true;

                                $("#<%= cboTransportMeansThatHas.ClientID %>").selectpicker('deselectAll');
                                break;
                            }
                        }
                    }
                }

                if (selectOptionNone === true) {
                    selectOptionNone = false;
                    setTimeout(function () { $("#<%= cboTransportMeansThatHas.ClientID %>").selectpicker('val', '5'); $("#<%= cboTransportMeansThatHas.ClientID %>").selectpicker('refresh'); validateSelection = true; }, 50);
                }

                MultiSelectDropdownListSaveSelectedItems($("#<%= cboTransportMeansThatHas.ClientID %>"), $("#<%= hdfTransportMeansThatEmployeeHas.ClientID %>"));
            });

            $("#<%= cboTransportationForWorkplace.ClientID %>").on('changed.bs.select', function (e) {
                /// <summary>Handles the select's value changed event for the cboTransportMeansThatHas control.</summary>                
                var selectedTransports = $("#<%= cboTransportationForWorkplace.ClientID %>").selectpicker('val');
                if (selectedTransports.length > 0) {
                    var enableOtherTransport = false;
                    for (let j = 0; j < selectedTransports.length; j++) {
                        if (selectedTransports[j] === '4') {
                            enableOtherTransport = true;
                            break;
                        }
                    }
                }

                MultiSelectDropdownListSaveSelectedItems($("#<%= cboTransportationForWorkplace.ClientID %>"), $("#<%= hdfTransportationForWorkplace.ClientID %>"));
            });

            RestoreSelectedTransports();
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

            if (!ValidateSurvey()) {
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
                            "<%= cboTransportMeansThatHas.UniqueID %>": {
                                required: true
                                , validSelection: true
                            },
                            "<%= cboTransportationForWorkplace.UniqueID %>": {
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

        function RestoreSelectedTransports() {
            /// <summary>Recover the selected items that were stored and assign them to the control to be selected.</summary>
            MultiSelectDropdownListRestoreSelectedItems($('#<%= cboTransportMeansThatHas.ClientID %>'), $('#<%= hdfTransportMeansThatEmployeeHas.ClientID %>'));
            MultiSelectDropdownListRestoreSelectedItems($('#<%= cboTransportationForWorkplace.ClientID %>'), $('#<%= hdfTransportationForWorkplace.ClientID %>'));
        }
    </script>
</asp:Content>
