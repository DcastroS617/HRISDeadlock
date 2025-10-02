<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="DiseasesFrequency.aspx.cs" Inherits="HRISWeb.SocialResponsability.DiseasesFrequency" %>
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
                                        <a class="nav-link active" id="diseasesfrequency-tab" data-toggle="tab" href="#diseasesfrequency" role="tab" aria-controls="diseasesfrequency" aria-selected="true"><%= GetLocalResourceObject("diseasesfrequency-tab") %></a>
                                    </li>
                                </ul>
                                <div class="tab-content" id="survey">
                                    <div class="tab-pane fade in active" id="diseasesfrequency" role="tabpanel" aria-labelledby="diseasesfrequency-tab">
                                        <p>
                                            <br />
                                        </p>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-12">
                                                            <asp:Label ID="lblDiseaseFrequency" meta:resourcekey="lblDiseaseFrequency" AssociatedControlID="hdfDiseasesFrequency" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                            <asp:HiddenField ID="hdfDiseasesFrequency" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-6 ">
                                                         </div>
                                                        <div class="col-sm-6 ">                                                         
                                                            <div class="col-sm-2 ">
                                                                <asp:Label ID="lblMen"  runat="server" Text="" CssClass="control-label text-left"><%= GetLocalResourceObject("lblMen") %></asp:Label>
                                                            </div>  
                                                            <div class="col-sm-4 ">
                                                                <asp:Label ID="lblSpecifyMen"  runat="server" Text="" CssClass="control-label text-left"><%= GetLocalResourceObject("lblSpecify") %></asp:Label>
                                                            </div>
                                                            <div class="col-sm-2 ">
                                                                <asp:Label ID="lblWomen"  runat="server" Text="" CssClass="control-label text-left"><%= GetLocalResourceObject("lblWomen") %></asp:Label>
                                                            </div>
                                                             <div class="col-sm-4 ">
                                                                <asp:Label ID="lblSpecifyWoMen"  runat="server" Text="" CssClass="control-label text-left"><%= GetLocalResourceObject("lblSpecify") %></asp:Label>
                                                            </div>
                                                        </div> 
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                         <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                       <div class="col-sm-5  col-sm-offset-1">
                                                            <asp:Label ID="lblQuestionDisease" meta:resourcekey="lblQuestionDisease" AssociatedControlID="cboDiseaseChroniqueMen" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-6 "> 
                                                            <div class="col-sm-2 ">
                                                              <asp:DropDownList ID="cboMenNumber" runat="server" CssClass="form-control" AutoPostBack="false" ></asp:DropDownList>
                                                               <label id="cboMenNumberValidation" for="<%# Container.FindControl("cboMenNumber").ClientID %>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgMenNumberValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                            </div>
                                                            <div class="col-sm-4">
                                                                <div class="col-sm-11">
                                                                    <asp:DropDownList ID="cboDiseaseChroniqueMen" runat="server" CssClass="form-control selectpicker" data-live-search="true" multiple="multiple"   AutoPostBack="false"></asp:DropDownList>
                                                                    <label id="cboDiseaseChroniqueMenValidation" for="<%= cboDiseaseChroniqueMen.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgDiseaseValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                                    <asp:HiddenField ID="hdfDiseaseSelectedMen" runat="server" />
                                                                </div>
                                                                <div class="col-sm-0">
                                                                    <span class="glyphicon glyphicon-info-sign text-center" data-toggle="tooltip" title='<%=GetLocalResourceObject("MultiSelectionDropDownListTooltip")%>' style="cursor:pointer"></span>
                                                                </div>                                                                
                                                            </div>
                                                            <div class="col-sm-2">
                                                                <asp:DropDownList ID="cboWomenNumber" runat="server" CssClass="form-control" AutoPostBack="false" ></asp:DropDownList>
                                                                <label id="cboWomenNumberValidation" for="<%# Container.FindControl("cboWomenNumber").ClientID %>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgWomenNumberValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                            </div>
                                                            <div class="col-sm-4">
                                                                <div class="col-sm-11">
                                                                    <asp:DropDownList ID="cboDiseaseChroniqueWomen" runat="server" CssClass="form-control selectpicker" data-live-search="true" multiple="multiple" AutoPostBack="false"></asp:DropDownList>
                                                                    <label id="cboDiseaseChroniqueWomenValidation" for="<%= cboDiseaseChroniqueWomen.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgDiseaseValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                                    <asp:HiddenField ID="hdfDiseaseSelectedWomen" runat="server" />
                                                                </div>
                                                                <div class="col-sm-0">
                                                                     <span class="glyphicon glyphicon-info-sign text-center" data-toggle="tooltip" title='<%=GetLocalResourceObject("MultiSelectionDropDownListTooltip")%>' style="cursor:pointer"></span>
                                                                </div> 
                                                            </div>
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
                                                            <asp:Label ID="lblWithoutFood" meta:resourcekey="lblWithoutFood" AssociatedControlID="chkWithoutFood" runat="server" Text="" CssClass="control-label"></asp:Label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-1 col-sm-offset-4">
                                                            <asp:CheckBox ID="chkWithoutFood" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>' AutoPostBack="false"></asp:CheckBox>
                                                        </div>
                                                        <div class="col-sm-3 text-right">
                                                            <asp:Label ID="lblQuantityWithoutFood" meta:resourcekey="lblQuantityWithoutFood" AssociatedControlID="cboQuantityWithoutFood" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:DropDownList ID="cboQuantityWithoutFood" runat="server" CssClass="form-control selectpicker" data-live-search="true" AutoPostBack="false" disabled="disabled"></asp:DropDownList>
                                                            <label id="cboQuantityWithoutFoodValidation" for="<%= cboQuantityWithoutFood.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgQuantityWithoutFoodValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
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

            $('#<%= chkWithoutFood.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });

            $("#<%= cboMenNumber.ClientID %>").change(function () {
                if (Number($(this).val())  > 0) {
                    SetControlValid($('#<%= cboDiseaseChroniqueMen.ClientID %>').attr("id"));
                }
            });

            $("#<%= cboDiseaseChroniqueMen.ClientID %>").on('changed.bs.select', function (e) {
                /// <summary>Handles the select's value changed event for the cboDiseaseChronique control.</summary>
                MultiSelectDropdownListSaveSelectedItems($("#<%= cboDiseaseChroniqueMen.ClientID %>"), $("#<%= hdfDiseaseSelectedMen.ClientID %>"));                
            });

            $("#<%= cboDiseaseChroniqueWomen.ClientID %>").on('changed.bs.select', function (e) {
                /// <summary>Handles the select's value changed event for the cboDiseaseChronique control.</summary>
                MultiSelectDropdownListSaveSelectedItems($("#<%= cboDiseaseChroniqueWomen.ClientID %>"), $("#<%= hdfDiseaseSelectedWomen.ClientID %>"));
            });

            $('#<%= chkWithoutFood.ClientID %>').change(function () {
                var checkBoxValue = $(this).prop('checked');
                $('#<%= cboQuantityWithoutFood.ClientID %>').prop('disabled', !checkBoxValue);

                if (checkBoxValue) {
                        $('#<%= cboQuantityWithoutFood.ClientID %>').selectpicker("val", "1");  
                } else {
                    $('#<%= cboQuantityWithoutFood.ClientID %>').val("-1");  
                }
                //$('#<%= cboQuantityWithoutFood.ClientID %>').selectpicker("val", "-1");   
                
                SetControlValid($('#<%= cboQuantityWithoutFood.ClientID %>').attr("id"));
                $('#<%= cboQuantityWithoutFood.ClientID %>').selectpicker("refresh");
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

        function RestoreSelectedDiseases() {
            /// <summary>Recover the selected items that were stored and assign them to the control to be selected.</summary>
            MultiSelectDropdownListRestoreSelectedItems($('#<%= cboDiseaseChroniqueMen.ClientID %>'), $('#<%= hdfDiseaseSelectedMen.ClientID %>'))
            MultiSelectDropdownListRestoreSelectedItems($('#<%= cboDiseaseChroniqueWomen.ClientID %>'), $('#<%= hdfDiseaseSelectedWomen.ClientID %>'))
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

            jQuery.validator.addMethod("validSelectionMenDisease", function (value, element) {
                
                if (Number($('#<%=cboMenNumber.ClientID %>').val()) == 0) {
                    return true;
                } else {
                    if (value == "-1") {
                        return true;
                    } else {
                        return value.length > 0;
                    }
                }
                    
            }, "Please select a valid option");

            jQuery.validator.addMethod("validSelectionWomenDisease", function (value, element) {
                if (Number($('#<%=cboWomenNumber.ClientID%>').val()) == 0) {
                    return true;
                } else {
                    if (value == "-1") {
                        return true;
                    } else {
                        return value.length > 0;
                    }
                }
            }, "Please select a valid option");
            jQuery.validator.addMethod("validSelectionMenNumberDisease", function (value, element) {
                
                if ((Number($('#<%=cboDiseaseChroniqueMen.ClientID %>').val().length) == 0) || (Number($('#<%=cboDiseaseChroniqueMen.ClientID %>').val() == -1)) ) {
                    return !Number(value) > 0;
                } else {
                    if (Number(value) > 0) {
                        return true;
                    } else {
                        return false;
                    }
                }
            }, "Please select a valid option");

            jQuery.validator.addMethod("validSelectionWomenNumberDisease", function (value, element) {
                
                if ((Number($('#<%=cboDiseaseChroniqueWomen.ClientID %>').val().length) == 0) || (Number($('#<%=cboDiseaseChroniqueWomen.ClientID %>').val() == -1))) {
                    return !Number(value) > 0;
                } else {
                    if (Number(value) > 0) {
                        return true;
                    } else {
                        return false;
                    }
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
                            <%=cboQuantityWithoutFood.UniqueID%>: {
                                required:{
                                    depends: function(element) {                                  
                                        var checkValue = $('#<%=chkWithoutFood.ClientID%>').prop('checked');                                        
                                        return checkValue;
                                    }
                                }
                                , validSelection: {
                                    depends: function(element) {                                  
                                        var checkValue = $('#<%=chkWithoutFood.ClientID%>').prop('checked');                                        
                                        return checkValue;
                                    }
                                }
                            },
                            "<%= cboMenNumber.UniqueID %>": {
                                validSelectionMenNumberDisease: true
                            },
                             "<%= cboWomenNumber.UniqueID %>": {
                                   validSelectionWomenNumberDisease: true
                            },
                             "<%= cboDiseaseChroniqueMen.UniqueID %>": {
                                    validSelectionMenDisease: true
                            },
                            "<%= cboDiseaseChroniqueWomen.UniqueID %>": {
                                validSelectionWomenDisease: true
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