<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Disabilities.aspx.cs" Inherits="HRISWeb.SocialResponsability.Disabilities" %>

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
                                        <a class="nav-link active" id="disabilities-tab" data-toggle="tab" href="#disabilities" role="tab" aria-controls="disabilities" aria-selected="true"><%= GetLocalResourceObject("disabilities-tab") %></a>
                                    </li>
                                </ul>

                                <div class="tab-content" id="survey">
                                    <div class="tab-pane fade in active" id="disabilities" role="tabpanel" aria-labelledby="disabilities-tab">
                                        <p>
                                            <br />
                                        </p>

                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-6">
                                                            <asp:Label ID="lblFamilyMembersWithDisabilities" meta:resourcekey="lblFamilyMembersWithDisabilities" AssociatedControlID="chkFamilyDisability" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:CheckBox ID="chkFamilyDisability" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>' AutoPostBack="true" OnCheckedChanged="chkFamilyDisability_CheckedChanged"></asp:CheckBox>
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
                                                            <asp:Label ID="lblNumberFamilyMembersWithDisabilities" meta:resourcekey="lblNumberFamilyMembersWithDisabilities" AssociatedControlID="cboNumberFamilyMembersWithDisabilities" runat="server" Text="" CssClass="control-label text-right"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:DropDownList ID="cboNumberFamilyMembersWithDisabilities" runat="server" CssClass="form-control selectpicker" data-live-search="true" AutoPostBack="true" OnSelectedIndexChanged="cboNumberFamilyMembersWithDisabilities_SelectedIndexChanged"></asp:DropDownList>
                                                            <label id="cboNumberFamilyMembersWithDisabilitiesValidation" for="<%= cboNumberFamilyMembersWithDisabilities.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgNumberFamilyMembersWithDisabilitiesValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-11 col-sm-offset-1">
                                                            <asp:Label ID="lblDisabilityTypes" meta:resourcekey="lblDisabilityTypes" AssociatedControlID="hdfNumberFamilyMembersDisabilities" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                            <asp:HiddenField ID="hdfNumberFamilyMembersDisabilities" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-sm-9 col-sm-offset-1">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-12">
                                                            <div class="table-responsive">
                                                                <asp:Repeater ID="rptMembersWithDisabilities" runat="server" OnItemDataBound="rptMembersWithDisabilities_ItemDataBound">
                                                                    <HeaderTemplate>
                                                                        <table class="table table-bordered table-striped table-hover" id="tbMembersWithDisabilities">
                                                                            <thead>
                                                                                <tr>
                                                                                    <th style="width: 2%!important;"><%= GetLocalResourceObject("rptMembersWithDisabilities.Number.Header") %></th>
                                                                                    <th style="width: 38%!important"><%= GetLocalResourceObject("rptMembersWithDisabilities.Relationship.Header") %></th>
                                                                                    <th style="width: 60%!important"><%= GetLocalResourceObject("rptMembersWithDisabilities.Disability.Header") %></th>
                                                                                </tr>
                                                                            </thead>
                                                                            <tbody>
                                                                    </HeaderTemplate>

                                                                    <ItemTemplate>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lblFamilyMemberId" CssClass="control-label text-left" runat="server" Text='<%#Eval("FamiliarId")%>'></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="cboFamilyRelationship" runat="server" CssClass="form-control" AutoPostBack="false"></asp:DropDownList>
                                                                                <label id="cboFamilyRelationshipValidation" for="<%# Container.FindControl("cboFamilyRelationship").ClientID %>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgFamilyRelationshipValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="cboMemberDisabilityType" runat="server" CssClass="form-control" AutoPostBack="false"></asp:DropDownList>
                                                                                <label id="cboMemberDisabilityTypeValidation" for="<%# Container.FindControl("cboMemberDisabilityType").ClientID %>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgMemberDisabilityTypeValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>

                                                                    <FooterTemplate>
                                                                        </tbody>
                                                                        </table>
                                                                    </FooterTemplate>
                                                                </asp:Repeater>
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
                                                        <div class="col-sm-12 text-left">
                                                            <asp:Label ID="lblDiscapacityGrade" meta:resourcekey="lblDiscapacityGrade" AssociatedControlID="hdfQuestion33" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                            <asp:HiddenField ID="hdfQuestion33" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-10">
                                                            <div class="table-responsive">
                                                                <asp:Repeater ID="rptDiseaseByFamilyMembers" runat="server" OnItemDataBound="rptDiseaseByFamilyMembers_ItemDataBound">
                                                                    <HeaderTemplate>
                                                                        <table class="table table-bordered table-striped table-hover" id="tbDiseaseByFamilyMembers">
                                                                            <thead>
                                                                                <tr>
                                                                                    <th style="border-bottom-color: transparent;" class="hidden"></th>
                                                                                    <th style="border-bottom-color: transparent;"></th>
                                                                                    <th colspan="2" class="text-center"><%= GetLocalResourceObject("rptDiseaseByFamilyMembers.QuantityOf.Header") %></th>
                                                                                </tr>
                                                                                <tr>
                                                                                    <th class="hidden"></th>
                                                                                    <th style="width: 50%!important; border-top-color: transparent;"></th>
                                                                                    <th class="text-center" style="width: 10%!important"><%= GetLocalResourceObject("rptDiseaseByFamilyMembers.Mens.Header") %></th>
                                                                                    <th class="text-center" style="width: 10%!important"><%= GetLocalResourceObject("rptDiseaseByFamilyMembers.Women.Header") %></th>
                                                                                </tr>
                                                                            </thead>
                                                                            <tbody>
                                                                    </HeaderTemplate>

                                                                    <ItemTemplate>
                                                                        <tr>
                                                                            <td class="hidden">
                                                                                <asp:Label ID="lblDiseaseCode" CssClass="control-label text-left" runat="server" Text='<%#Eval("OtherDiseaseCode")%>'></asp:Label>
                                                                            </td>

                                                                            <td>
                                                                                <asp:Label ID="lblDiseaseType" CssClass="control-label text-left" runat="server" Text='<%#Eval("OtherDiseaseDescriptionByCurrentCulture")%>'></asp:Label>
                                                                            </td>

                                                                            <td>
                                                                                <asp:DropDownList ID="cboMenNumber" runat="server" CssClass="form-control" AutoPostBack="false" data-diseasecode='<%#Eval("OtherDiseaseCode")%>'></asp:DropDownList>
                                                                                <label id="cboMenNumberValidation" for="<%# Container.FindControl("cboMenNumber").ClientID %>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgMenNumberValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                                            </td>

                                                                            <td>
                                                                                <asp:DropDownList ID="cboWomenNumber" runat="server" CssClass="form-control" AutoPostBack="false" data-diseasecode='<%#Eval("OtherDiseaseCode")%>'></asp:DropDownList>
                                                                                <label id="cboWomenNumberValidation" for="<%# Container.FindControl("cboWomenNumber").ClientID %>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgWomenNumberValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>

                                                                    <FooterTemplate>
                                                                        </tbody>
                                                                        </table>
                                                                    </FooterTemplate>
                                                                </asp:Repeater>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                       <%-- <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-5 col-sm-offset-1">
                                                            <asp:Label ID="lblChronicDisease" meta:resourcekey="lblChronicDisease" AssociatedControlID="txtChronicDisease" runat="server" Text="" CssClass="control-label"></asp:Label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-9 col-sm-offset-1">
                                                            <asp:TextBox ID="txtChronicDisease" meta:resourcekey="txtChronicDisease" runat="server" CssClass="form-control control-validation cleanPasteText" MaxLength="100" placeholder="" disabled="disabled" onkeypress="blockEnterKey();return isNumberOrLetter(event);"></asp:TextBox>
                                                            <label id="txtChronicDiseaseValidation" for="<%= txtChronicDisease.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgChronicDiseaseValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>--%>
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

            $('#<%= chkFamilyDisability.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });

            $('#<%= chkFamilyDisability.ClientID %>').change(function () {
                var checkBoxValue = $(this).prop('checked');
                __doPostBack('<%= chkFamilyDisability.UniqueID %>', 'Checked');
            });

            $('select[data-diseasecode="3"]').change(function () {
                SetChronicDiseaseTextBoxConfiguration();
            });
        }

       <%-- function SetChronicDiseaseTextBoxConfiguration() {
            /// <summary>Enable or disbaled the chronic disease texbox</summary>
            var menNumber = $('select[name*="cboMenNumber"][data-diseasecode="3"]').val();
            var womenNumber = $('select[name*="cboWomenNumber"][data-diseasecode="3"]').val();

            menNumber = menNumber != undefined && menNumber != '-1' ? parseInt(menNumber) : 0;
            womenNumber = womenNumber != undefined && womenNumber != '-1' ? parseInt(womenNumber) : 0;

            if (menNumber + womenNumber <= 0) {
                $('#<%=txtChronicDisease.ClientID%>').prop('disabled', true);
                $('#<%=txtChronicDisease.ClientID%>').val("");
            }

            else {
                $('#<%=txtChronicDisease.ClientID%>').prop('disabled', false);
            }
        }--%>

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
                            "<%=cboNumberFamilyMembersWithDisabilities.UniqueID%>": {
                                required: {
                                    depends: function (element) {
                                        return $('#<%=chkFamilyDisability.ClientID%>').prop('checked');
                                    }
                                }
                                , validSelection: true
                            },
                            <%--"<%=txtChronicDisease.UniqueID%>": {
                                normalizer: function (value) {
                                    return $.trim(value);
                                }
                                , minlength: 1
                                , maxlength: 100
                                , required: {
                                    depends: function (element) {
                                        var menNumber = $('select[name*="cboMenNumber"][data-diseasecode="3"]').val();
                                        var womenNumber = $('select[name*="cboWomenNumber"][data-diseasecode="3"]').val();

                                        menNumber = menNumber != undefined && menNumber != '-1' ? parseInt(menNumber) : 0;
                                        womenNumber = womenNumber != undefined && womenNumber != '-1' ? parseInt(womenNumber) : 0;

                                        return (menNumber + womenNumber) > 0;
                                    }
                                }
                            }--%>
                        }
                    });

                $('[name*="cboFamilyRelationship"]').each(function () {
                    $(this).rules('add', {
                        required: true,
                        validSelection: true
                    });
                });

                $('[name*="cboMemberDisabilityType"]').each(function () {
                    $(this).rules('add', {
                        required: true,
                        validSelection: true
                    });
                });

                $('[name*="cboMenNumber"]').each(function () {
                    $(this).rules('add', {
                        required: true,
                        validSelection: true
                    });
                });

                $('[name*="cboWomenNumber"]').each(function () {
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
