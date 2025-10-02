<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="FamilyWelfare.aspx.cs" Inherits="HRISWeb.SocialResponsability.FamilyWelfare" %>
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
                                        <a class="nav-link active" id="familywelfare-tab" data-toggle="tab" href="#familywelfare" role="tab" aria-controls="familywelfare" aria-selected="true"><%= GetLocalResourceObject("familywelfare-tab") %></a>
                                    </li>
                                </ul>
                            </div>
                            <div>
                                <br />
                                 <label class="question24"><%= GetLocalResourceObject("familywelfareQuestion") %></label>
                            </div>

                                <div class="tab-content" id="survey">
                                    <div class="tab-pane fade in active" id="familywelfare" role="tabpanel" aria-labelledby="familywelfare-tab">
                                        <p>
                                            <br />
                                        </p>
                                           <div class="row">
                                            <div class="col-sm-12">
                                                <div class="">
                                                    <div class="table-responsive" id="familyTable">
                                                        <asp:Repeater ID="rptFamilyWelfare" runat="server" OnItemDataBound="rptFamilyWelfare_ItemDataBound">
                                                            <HeaderTemplate>
                                                                <table class="table table-bordered table-striped table-hover" style="max-width: none!important; width: auto!important" id="tbFamilyGroup">
                                                                    <thead>
                                                                        <tr>
                                                                            <th style="width: 10px!important; min-width: 10px!important;"><%= GetLocalResourceObject("rptFamilyWelfare.Number.Header") %></th>
                                                                            <th style="width: 20px!important; min-width: 20px!important;">                                                                                  
                                                                                <div>
                                                                                <%= GetLocalResourceObject("rptFamilyWelfare.Guide.Header") %>
                                                                                </div>
                                                                            </th>
                                                                            <th style="width: 200px!important; min-width:200px!important;">
                                                                                  <div >
                                                                                    <img  src="../Content/images/bulb_icon.png" />
                                                                                </div>
                                                                                <div>
                                                                                <%= GetLocalResourceObject("rptFamilyWelfare.Visit.Header") %>
                                                                                </div>
                                                                            </th>
                                                                            <th style="width: 160px!important; min-width:160px!important">
                                                                                  <div >
                                                                                    <img  src="../Content/images/bulb_icon.png" />
                                                                                </div>
                                                                                <div>
                                                                                <%= GetLocalResourceObject("rptFamilyWelfare.Help.Header") %>
                                                                                </div>
                                                                            </th>
                                                                            <th style="width: 260px!important; min-width: 260px!important;">
                                                                                  <div >
                                                                                    <img  src="../Content/images/bulb_icon.png" />
                                                                                </div>
                                                                                <div>
                                                                                <%= GetLocalResourceObject("rptFamilyWelfare.ReasonNotMedic.Header") %>
                                                                                </div>
                                                                            </th>
                                                                            <th style="width: 130px!important; min-width: 130px!important;">
                                                                                  <div >
                                                                                    <img  src="../Content/images/bulb_icon.png" />
                                                                                </div>
                                                                                <div>
                                                                                <%= GetLocalResourceObject("rptFamilyWelfare.HaveSick.Header") %>
                                                                                </div>
                                                                            </th>
                                                                            <th style="width: 210px!important; min-width: 210px!important;">
                                                                                  <div >
                                                                                    <img  src="../Content/images/bulb_icon.png" />
                                                                                </div>
                                                                                <div>
                                                                                <%= GetLocalResourceObject("rptFamilyWelfare.HaveAccident.Header") %>
                                                                                </div>
                                                                            </th>
                                                                            <th style="width: 280px!important; min-width: 280px!important;">
                                                                                  <div >
                                                                                    <img  src="../Content/images/bulb_icon.png" />
                                                                                </div>
                                                                                <div>
                                                                                <%= GetLocalResourceObject("rptFamilyWelfare.ReasonNotAttention.Header") %>
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
                                                                        <asp:Label ID="lblGuide" CssClass="control-label text-left" Enabled="false" runat="server" ></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="cboTreatmentPlacesCare" runat="server" CssClass="form-control"  AutoPostBack="false"></asp:DropDownList>
                                                                         <label id="cboTreatmentPlacesCarePlacesValidation" for="<%# Container.FindControl("cboTreatmentPlacesCare").ClientID %>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgDiseaseTreatmentPlacesValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                                     </td>
                                                                    <td>
                                                                        <input id="chkVisitMedic" runat="server" type="checkbox" class="checkbox-toggle" /> 
                                                                    </td>
                                                                     <td>
                                                                         <asp:DropDownList ID="cboReasonNotMedic" runat="server"  CssClass="form-control" AutoPostBack="false"></asp:DropDownList>
                                                                        <label id="cboReasonNotMedicValidation" for="<%# Container.FindControl("cboReasonNotMedic").ClientID %>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgReasonNotMedicValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                                     </td>
                                                                    <td>
                                                                        <input id="chkHAveAccident" runat="server" type="checkbox"  class="checkbox-toggle" /> 
                                                                    </td>
                                                                    <td>
                                                                        <input id="chkHAveAttention" runat="server" type="checkbox" disabled="disabled" class="checkbox-toggle" />
                                                                    </td>
                                                                   <td>
                                                                         <asp:DropDownList ID="cboReasonNotAttention" runat="server"  CssClass="form-control" AutoPostBack="false"></asp:DropDownList>
                                                                        <label id="cboReasonNotAttentionValidation" for="<%# Container.FindControl("cboReasonNotAttention").ClientID %>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgcboReasonNotAttentionValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
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
                                       <p>
                                            <br />
                                        </p>
                                            <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-6">
                                                            <asp:Label ID="lblDisability" meta:resourcekey="lblDisability" AssociatedControlID="chkDisability" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <asp:CheckBox ID="chkDisability" runat="server" data-toggle="toggle"  data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                                        </div>                                                        
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row"  runat="server" >
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-5 col-sm-offset-1">
                                                            <asp:Label ID="lblTypeDisability" meta:resourcekey="lblTypeDisability" AssociatedControlID="cboTypeDisability" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <asp:DropDownList ID="cboTypeDisability" runat="server" CssClass="form-control selectpicker" data-live-search="true" disabled="disabled" AutoPostBack="false"></asp:DropDownList>
                                                            <label id="cboTypeDisabilityValidation" for="<%= cboTypeDisability.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgTypeDisabilityValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </div>                                                        
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" runat="server" id="Question25_2Text">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-5 col-sm-offset-1">
                                                            <asp:Label ID="lblDiscapacityGrade" meta:resourcekey="lblDiscapacityGrade" AssociatedControlID="chkDiscapacityGrade" runat="server" Text="" CssClass="text-"></asp:Label>
                                                        </div> 
                                                         <div class="col-sm-4 ">
                                                            <asp:CheckBox ID="chkDiscapacityGrade" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>' disabled="disabled"></asp:CheckBox>
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

            $('.checkbox-toggle[id*="chkVisitMedic"]').change(function () {
                var toggleValue = $(this).prop('checked');
                $(this).closest("tr").find('td:eq(4) select[id*="cboReasonNotMedic"]').prop('disabled', toggleValue);
                $(this).closest("tr").find('td:eq(4) select[id*="cboReasonNotMedic"] option[value=-1]').prop("selected", true);
                SetControlValid($(this).closest("td").next().find('select[id*="cboReasonNotMedic"]')[0].id);
            });

             $('.checkbox-toggle[id*="chkHAveAccident"]').change(function () {
                var toggleValue = $(this).prop('checked');
                
                $('#' + $(this).closest('tr').find('td:eq(6) input[type=checkbox]')[0].id).bootstrapToggle('off');
                $('#' + $(this).closest('tr').find('td:eq(6) input[type=checkbox]')[0].id).attr('checked', false);                

                if (toggleValue) {
                    $(this).closest('tr').find('td:eq(6) input[type=checkbox]').parent().removeAttr("disabled");
                    $(this).closest("tr").find('td:eq(7) select[id*="cboReasonNotAttention"]').prop('disabled', !toggleValue);
                } else {
                    $(this).closest("tr").find('td:eq(7) select[id*="cboReasonNotAttention"]').prop('disabled', !toggleValue);
                    $(this).closest("tr").find('td:eq(7) select[id*="cboReasonNotAttention"] option[value=-1]').prop("selected", true);
                }
                $(this).closest('tr').find('td:eq(6) input[type=checkbox]').prop('disabled', !toggleValue);
            });

            $('.checkbox-toggle[id*="chkHAveAttention"]').change(function () {
                var toggleValue = $(this).prop('checked');
                $(this).closest("tr").find('td:eq(7) select[id*="cboReasonNotAttention"]').prop('disabled', toggleValue);
                $(this).closest("tr").find('td:eq(7) select[id*="cboReasonNotAttention"] option[value=-1]').prop("selected", true);

               
                SetControlValid($(this).closest("td").next().find('select[id*="cboReasonNotAttention"]')[0].id);
            });

            $(".checkbox-toggle").bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                  off: '<%= GetLocalResourceObject("No") %>'
            });


            $('#<%= chkDisability.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });

            $('#<%= chkDiscapacityGrade.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });

            $('#<%= chkDisability.ClientID %>').change(function () {
                
                /// <summary>Handles the select's value changed event for the chkDisability control.</summary>  
                var checkBoxValue = $(this).prop('checked');
                $('#<%= cboTypeDisability.ClientID %>').prop('disabled', !checkBoxValue);
                SetControlValid('<%= cboTypeDisability.ClientID %>');
                $('#<%= cboTypeDisability.ClientID %>').selectpicker("val", "-1");
                $('#<%= cboTypeDisability.ClientID %>').selectpicker("refresh");
                $('#<%= chkDiscapacityGrade.ClientID %>').prop('disabled', !checkBoxValue);
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
                $('#<%= rptFamilyWelfare.ClientID %>').refresh();
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
                            <%= cboTypeDisability.UniqueID %>: {
                                required: true
                                , validSelection: true
                            }
                        }
                    });
                $('[name*="cboTreatmentPlacesCare"]').each(function () {
                    $(this).rules('add', {
                        required: true,
                        validSelection: true
                    });
                });
                $('[name*="cboReasonNotMedic"]').each(function () {
                        $(this).rules('add', {
                            required: true,
                            validSelection: true
                        });
                });
                $('[name*="cboReasonNotAttention"]').each(function () {
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
