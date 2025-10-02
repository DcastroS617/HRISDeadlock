<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="FamilyWork.aspx.cs" Inherits="HRISWeb.SocialResponsability.FamilyWork" %>

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
                                        <a class="nav-link active" id="familygroup-tab" data-toggle="tab" href="#familygroup" role="tab" aria-controls="familygroup" aria-selected="true"><%= GetLocalResourceObject("familyWork-tab") %></a>
                                    </li>
                                </ul>
                                <div class="tab-content" id="survey">
                                    <div class="tab-pane fade in active" id="familygroup" role="tabpanel" aria-labelledby="familygroup-tab">
                                        <p>
                                            <br />
                                        </p>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <asp:Label ID="lblFamilyGroupWork" meta:resourcekey="lblFamilyGroupWork"  runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                            </div>
                                        </div>
                                          
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="">
                                                    <div class="table-responsive">
                                                        <asp:Repeater ID="rptFamilyWork" runat="server" OnItemDataBound="rptFamilyWork_ItemDataBound">
                                                            <HeaderTemplate>
                                                                <table class="table table-bordered table-striped table-hover" style="max-width: none!important; width: auto!important" id="tbFamilyGroup">
                                                                    <thead>
                                                                        <tr>
                                                                            <th style="width: 10px!important; min-width: 10px!important;">
                                                                                <div >
                                                                                    <img  src="../Content/images/bulb_icon.png" />
                                                                                </div>
                                                                                <div>
                                                                                    <%= GetLocalResourceObject("rptFamilyWork.Number.Header") %>
                                                                                </div>
                                                                            </th>
                                                                            <th style="width: 20px!important; min-width: 20px!important;">
                                                                                <%= GetLocalResourceObject("rptFamilyWork.Guide.Header") %>
                                                                            </th>
                                                                            <th style="width: 100px!important; min-width: 100px!important;">
                                                                                <div >
                                                                                    <img  src="../Content/images/bulb_icon.png" />
                                                                                </div>
                                                                                <div>
                                                                                <%= GetLocalResourceObject("rptFamilyWork.Working.Header") %>
                                                                                </div>
                                                                            </th>
                                                                            <th style="width: 100px!important; min-width: 100px!important">
                                                                                <div >
                                                                                    <img  src="../Content/images/bulb_icon.png" />
                                                                                </div>
                                                                                <div>
                                                                                <%= GetLocalResourceObject("rptFamilyWork.Dole.Header") %>
                                                                                </div>
                                                                            </th>
                                                                            <th style="width: 100px!important; min-width: 100px!important;">
                                                                                <div >
                                                                                    <img  src="../Content/images/bulb_icon.png" />
                                                                                </div>
                                                                                <div>
                                                                                <%= GetLocalResourceObject("rptFamilyWork.SocialMedic.Header") %>
                                                                                </div>
                                                                            </th>
                                                                            <th style="width: 100px!important; min-width: 100px!important;">
                                                                                <div >
                                                                                    <img  src="../Content/images/bulb_icon.png" />
                                                                                </div>
                                                                                <div>
                                                                                <%= GetLocalResourceObject("rptFamilyWork.HaveHelp.Header") %>
                                                                                </div>
                                                                            </th>
                                                                            <th style="width: 140px!important; min-width: 140px!important;">
                                                                                <div >
                                                                                    <img  src="../Content/images/bulb_icon.png" />
                                                                                </div>
                                                                                <div> 
                                                                                <%= GetLocalResourceObject("rptFamilyWork.CurrentlyPaying.Header") %>
                                                                                </div>
                                                                            </th>
                                                                            <th style="width: 100px!important; min-width: 100px!important;">
                                                                                <div >
                                                                                    <img  src="../Content/images/bulb_icon.png" />
                                                                                </div>
                                                                                <div> 
                                                                                <%= GetLocalResourceObject("rptFamilyWork.Retired.Header") %>
                                                                                </div>
                                                                            </th>
                                                                            <th style="width: 100px!important; min-width: 100px!important;">
                                                                                <%= GetLocalResourceObject("rptFamilyWork.OthersHelp.Header") %>
                                                                            </th>
                                                                            <th style="width: 110px!important; min-width: 110px!important;">
                                                                                <div >
                                                                                    <img  src="../Content/images/bulb_icon.png" />
                                                                                </div>
                                                                                <div>
                                                                                <%= GetLocalResourceObject("rptFamilyWork.Profession.Header") %>
                                                                                </div>
                                                                            </th>
                                                                            <th style="width: 160px!important; min-width: 160px!important;">
                                                                                <div >
                                                                                    <img  src="../Content/images/bulb_icon.png" />
                                                                                </div>
                                                                                <div> 
                                                                                <%= GetLocalResourceObject("rptFamilyWork.DoBussines.Header") %>
                                                                                </div>
                                                                            </th>
                                                                            <th style="width: 170px!important; min-width: 170px!important;">
                                                                                <div >
                                                                                    <img  src="../Content/images/bulb_icon.png" />
                                                                                </div>
                                                                                <div> 
                                                                                <%= GetLocalResourceObject("rptFamilyWork.ReasonNotWork.Header") %>
                                                                                </div>
                                                                            </th>   
                                                                        </tr>
                                                                    </thead>
                                                                    <tbody>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblFamilyId" CssClass="control-label text-left" runat="server" Text='<%#Eval("FamiliarId")%>'></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblGuide" CssClass="control-label text-left" runat="server" ></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <input id="chkWork" runat="server" enabled="false" type="checkbox" class="checkbox-toggle" /> 
                                                                    </td>
                                                                    <td>
                                                                        <input id="chkDole" runat="server" type="checkbox" class="checkbox-toggle" /> 
                                                                    </td>
                                                                    <td>
                                                                        <input id="chkSocialMedic" runat="server" type="checkbox" class="checkbox-toggle" />
                                                                    </td>
                                                                    <td>
                                                                        <input id="chkHaveHelp" runat="server" type="checkbox" class="checkbox-toggle" />
                                                                    </td>
                                                                    <td>
                                                                        <input id="chkCurrentlyPaying" runat="server" type="checkbox" class="checkbox-toggle" />
                                                                    </td>
                                                                     <td>
                                                                       <input id="chkRetired" runat="server" type="checkbox" class="checkbox-toggle" />
                                                                     </td>
                                                                    <td>
                                                                        <input id="chkHouseHelp" runat="server" type="checkbox" class="checkbox-toggle" />
                                                                        <asp:DropDownList ID="cboHouseHelp" runat="server" CssClass="form-control" AutoPostBack="false" disabled="disabled"></asp:DropDownList>
                                                                        <label id="cboHouseHelprValidation" for="<%# Container.FindControl("cboHouseHelp").ClientID %>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgHouseHelpValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="cboProfession" runat="server" Enabled="false" CssClass="form-control" AutoPostBack="false"></asp:DropDownList>
                                                                        <label id="cboProfessionValidation" for="<%# Container.FindControl("cboProfession").ClientID %>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgProfesionValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                                    </td>
                                                                    <td>
                                                                        <input id="chkDoBussines" runat="server" type="checkbox" class="checkbox-toggle" />
                                                                    </td>
                                                                    <td>
                                                                         <asp:DropDownList ID="cboReasonNotwork" runat="server" Enabled="false" CssClass="form-control" AutoPostBack="false"></asp:DropDownList>
                                                                        <label id="cboReasonNotworkValidation" for="<%# Container.FindControl("cboReasonNotwork").ClientID %>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgReasonNotWorkValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                                     </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                </tbody>
                                                            </table>
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                        <asp:HiddenField ID="hdfNumberOfMen" runat="server" />
                                                        <asp:HiddenField ID="hdfNumberOfWomen" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row" style="width:100%;">
                                            <asp:ListView runat="server" ID="lvHouseholdContributionRanges" GroupItemCount="6" >
                                                <LayoutTemplate>
                                                    <table runat="server" style="width:100%;" id="tbHouseholdContributionRanges">
                                                        <tr runat="server" id="groupPlaceholder"></tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <GroupTemplate>
                                                    <tr runat="server" id="tableRow" class="row">
                                                        <td runat="server" id="itemPlaceholder" />
                                                    </tr>
                                                </GroupTemplate>
                                                <ItemTemplate>
                                                    <td runat="server" class="col-sm-2" style="padding-top:20px;">                                                        
                                                        <asp:Label ID="lblHouseholdContributionRange" runat="server" Text='<%#Eval("RangeFormated") %>' />
                                                    </td>
                                                </ItemTemplate>
                                            </asp:ListView>
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

            $(".checkbox-toggle").bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });

            $('.checkbox-toggle[id*="chkWork"]').change(function () {
                
                var toggleValue = $(this).prop('checked');
                $('#' + $(this).closest('tr').find('td:eq(3) input[type=checkbox]')[0].id).bootstrapToggle('off');
                $('#' + $(this).closest('tr').find('td:eq(3) input[type=checkbox]')[0].id).attr('checked', false);

                $(this).closest('tr').find('td:eq(9) select[id*="cboProfession"]').prop('disabled', !toggleValue);
                $(this).closest('tr').find('td:eq(9) select[id*="cboProfession"] option[value=-1]').prop("selected", true);
                
                             
                $('#' + $(this).closest('tr').find('td:eq(10) input[type=checkbox]')[0].id).bootstrapToggle('off');
                $('#' + $(this).closest('tr').find('td:eq(10) input[type=checkbox]')[0].id).attr('checked', false);

                $(this).closest('tr').find('td:eq(3) input[type=checkbox]').prop('disabled', !toggleValue);
                $(this).closest('tr').find('td:eq(10) input[type=checkbox]').prop('disabled', toggleValue);
                $(this).closest('tr').find('td:eq(11) select[id*="cboReasonNotwork"]').prop('disabled', toggleValue);

                if (toggleValue) {
                    $(this).closest('tr').find('td:eq(3) input[type=checkbox]').parent().removeAttr("disabled");
                    SetControlValid($(this).closest('tr').find('td:eq(9) select[id*="cboProfession"]')[0].id);

                } else {
                    $(this).closest('tr').find('td:eq(10) input[type=checkbox]').parent().removeAttr("disabled");                    
                }                               
            });

            $('.checkbox-toggle[id*="chkDoBussines"]').change(function () {
                var toggleValue = $(this).prop('checked');
                $(this).closest('tr').find('td:eq(11) select[id*="cboReasonNotwork"]').prop('disabled', toggleValue);
                $(this).closest('tr').find('td:eq(11) select[id*="cboReasonNotwork"] option[value=-1]').prop("selected", true);
                SetControlValid($(this).closest('tr').find('td:eq(11) select[id*="cboReasonNotwork"]')[0].id);
            });

            $('.checkbox-toggle[id*="chkHouseHelp"]').change(function () {
                var toggleValue = $(this).prop('checked');

                $(this).closest('tr').find('td:eq(8) select[id*="cboHouseHelp"]').prop('disabled', !toggleValue);
                $(this).closest('tr').find('td:eq(8) select[id*="cboHouseHelp"]')[0].selectedIndex = -1;
                SetControlValid($(this).closest('tr').find('td:eq(8) select[id*="cboHouseHelp"]')[0].id);
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
            jQuery.validator.addMethod("validNumberOfPeopleByGender", function (value, element) {
                var numberOfMenGrid = 0, numberOfWomenGrid = 0;
                var numberOfMen = parseInt($('#<%= hdfNumberOfMen.ClientID %>').val());
                var numberOfWomen = parseInt($('#<%= hdfNumberOfWomen.ClientID %>').val());

                $('[name*="cboGender"]').each(function () {
                    var genderValue = $(this).val();
                    if (genderValue === 'F') {
                        numberOfWomenGrid++;
                    }
                    else if (genderValue === 'M') {
                        numberOfMenGrid++;
                    }
                });

                return !(numberOfMenGrid !== numberOfMen || numberOfWomenGrid !== numberOfWomen);
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
                        /*rules: {                            
                            cboRelationship: {
                                required: true
                                , validSelection: true
                            }
                        }*/
                    });

                $('[name*="cboProfession"]').each(function () {
                    $(this).rules('add', {
                        required: true,
                        validSelection: true
                    });                 
                });

                $('[name*="cboReasonNotwork"]').each(function () {
                    $(this).rules('add', {
                        required: true,
                        validSelection: true
                    });
                });
                $('[name*="cboHouseHelp"]').each(function () {
                    $(this).rules('add', {
                        required: true,
                        validSelection: true
                    });
                });
            }
            //get the results            
            var result = validatorSurvey.form();
            return result;
        }
    </script>
</asp:Content>