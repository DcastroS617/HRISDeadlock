<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Classroom.ascx.cs" Inherits="HRISWeb.UserControls.Classroom" %>

<div class="modal-body">
    <div class="form-horizontal">
        <asp:HiddenField ID="hdfClassroomCodeEdit" runat="server" Value="" />
        <asp:HiddenField ID="hdfClassroomGeographicDivisionCodeEdit" runat="server" Value="" />

        <div class="form-group">
            <div class="col-sm-4 text-left">
                <label for="<%=cboTrainingCenter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblTrainingCenter")%></label>
            </div>

            <div class="col-sm-8">
                <asp:DropDownList ID="cboTrainingCenter" CssClass="form-control cboAjaxAction control-validation" AutoPostBack="false" runat="server"></asp:DropDownList>
                <label id="cboTrainingCenterValidation" for="<%= cboTrainingCenter.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjTrainingCenterValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
            </div>
        </div>

        <div class="form-group">
            <div class="col-sm-4 text-left">
                <label for="<%= txtClassroomCode.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblClassroomCode")%></label>
            </div>

            <div class="col-sm-8">
                <asp:TextBox ID="txtClassroomCode" CssClass="form-control cleanPasteText" runat="server" onkeypress="return isNumberOrLetter(event)" MaxLength="10"></asp:TextBox>
                <label id="txtClassroomCodeValidation" for="<%= txtClassroomCode.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjClassroomCodeValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
            </div>
        </div>

        <div class="form-group">
            <div class="col-sm-4 text-left">
                <label for="<%= txtClassroomDescription.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblClassroomDescription")%></label>
            </div>

            <div class="col-sm-8">
                <asp:TextBox ID="txtClassroomDescription" CssClass="form-control cleanPasteText" runat="server" onkeypress="return isNumberOrLetter(event) && checkMaxLength(this,event,500);" onkeyUp="return isNumberOrLetter(event) && checkMaxLength(this,event,500);" MaxLength="500" TextMode="MultiLine" Columns="3"></asp:TextBox>
                <label id="txtClassroomDescriptionValidation" for="<%= txtClassroomDescription.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjClassroomDescriptionValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
            </div>
        </div>

        <div class="form-group">
            <div class="col-sm-4 text-left">
                <label for="<%= txtCapacity.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCapacity")%></label>
            </div>

            <div class="col-sm-8">
                <asp:TextBox ID="txtCapacity" CssClass="form-control control-validation cleanPasteDigits" runat="server" onkeypress="return isNumber(event) && checkMaxLength(this,event,3);" MaxLength="3" autocomplete="off" type="number" min="0" max="100"></asp:TextBox>
                <label id="txtCapacityValidation" for="<%= txtCapacity.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjCapacityValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
            </div>
        </div>

        <div class="form-group">
            <div class="col-sm-4 text-left">
                <label for="<%= txtComments.ClientID%>" class="control-label" style="text-align: left"><%=GetLocalResourceObject("lblComments")%></label>
            </div>

            <div class="col-sm-8">
                <asp:TextBox ID="txtComments" CssClass="form-control cleanPasteText" runat="server" onkeypress="return isNumberOrLetter(event) && checkMaxLength(this,event,1000);" onkeyUp="return isNumberOrLetter(event) && checkMaxLength(this,event,1000);" MaxLength="1000" TextMode="MultiLine" Columns="3"></asp:TextBox>
                <label id="txtCommentsValidation" for="<%= txtComments.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjCommentsValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
            </div>
        </div>

        <div class="form-group">
            <div class="col-sm-4 text-left">
                <label for="<%= chbSearchEnabled.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblSearchEnabled")%></label>
            </div>

            <div class="col-sm-6">
                <asp:CheckBox ID="chbSearchEnabled" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
            </div>
        </div>
    </div>
</div>

<script type="text/javascript">
    //*******************************//
    //          VALIDATION           // 
    //*******************************//
    var validator = null;
    function ValidateFormClassroom() {
        /// <summary>Validate the form with jquey validation plugin </summary>
        /// <returns> True if form is valid. False otherwise. </returns>          
        if (validator == null) {
            //add custom validation methods
            jQuery.validator.addMethod("validSelection", function (value, element) {
                return this.optional(element) || value != "-1";
            }, "Please select a valid option");

            jQuery.validator.addMethod("validSelectioncbo", function (value, element) {
                return this.optional(element) || value != "--1";
            }, "Please select a valid option");

            //declare the validator
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
                        "<%= txtClassroomCode.UniqueID %>": {
                            required: true,
                            normalizer: function (value) {
                                return $.trim(value);
                            },
                            minlength: 1,
                            maxlength: 10
                        },
                        "<%= txtClassroomDescription.UniqueID %>": {
                            required: true,
                            normalizer: function (value) {
                                return $.trim(value);
                            },
                            minlength: 1,
                            maxlength: 500
                        },
                        "<%= cboTrainingCenter.UniqueID %>": {
                            required: true,
                            validSelectioncbo: true
                        },
                        "<%= txtCapacity.UniqueID %>": {
                            required: true,
                            digits: true,
                            min: 0,
                            max: 100
                        },
                        "<%= txtComments.UniqueID %>": {
                            required: false,
                            normalizer: function (value) {
                                return $.trim(value);
                            },
                            minlength: 1,
                            maxlength: 1000
                        }
                    }
                });
        }

        //get the results

        var result = validator.form();
        return result;
    }

    function EnabledRequired(controlId) {
        $('#' + controlId).attr("aria-invalid", "true");
        $('#' + controlId).addClass('Invalid');
    }

    //*******************************//
    //           PROCESS             //
    //*******************************//
    function ProcessAddClassroomRequest(resetId) {
        /// <summary>Process the accept request according to the validation of row selected</summary>
        /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
        var clientID = "#" + resetId + "";
        var uniqueID = resetId.replace("_", "$").replace('_', '$').replace('_', '$')

        $("#<%=txtComments.ClientID %>").val($.trim($("#<%=txtComments.ClientID %>").val()));
        disableButton($('#btnCancel'));
        disableButton($(clientID))

        if (!ValidateFormClassroom()) {
            setTimeout(function () {
                ResetButton(resetId);
                enableButton($('#btnCancel'));
                enableButton($(clientID));
            }, 150);

            MostrarMensaje(TipoMensaje.VALIDACION, '<%= GetLocalResourceObject("msgInvalidEntry") %>', null);
        }

        else {
            __doPostBack(uniqueID, '');
        }

        return false;
    }
</script>
