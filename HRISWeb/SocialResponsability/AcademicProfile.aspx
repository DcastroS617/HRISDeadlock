<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AcademicProfile.aspx.cs" Inherits="HRISWeb.SocialResponsability.AcademicProfile" %>
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
                                        <a class="nav-link active" id="academicprofile-tab" data-toggle="tab" href="#academicprofile" role="tab" aria-controls="academicprofile" aria-selected="true"><%= GetLocalResourceObject("academicprofile-tab") %></a>
                                    </li>
                                </ul>
                                <div class="tab-content" id="survey">
                                    <div class="tab-pane fade in active" id="academicprofile" role="tabpanel" aria-labelledby="academicprofile-tab">
                                        <p>
                                            <br />
                                        </p>
                                        
                                        <div class="row">
	                                        <div class="col-sm-12">
		                                        <div class="form-horizontal">
			                                        <div class="form-group">
				                                        <div class="col-sm-4">                                                            
                                                            <strong>
                                                                <asp:Label ID="lblAcademicLevel" meta:resourcekey="lblAcademicLevel"  runat="server" Text="" CssClass="control-label text-left "></asp:Label>
                                                            </strong>
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
                                                             <div class="col-xs-1 ml-auto mt-auto mb-auto">
                                                                <img  src="../Content/images/bulb_icon.png" />
                                                            </div>
                                                            <div class="col-sm-10 mr-auto mt-auto mb-auto"> 
                                                                <asp:Label ID="lblDoyouKnow" meta:resourcekey="lblDoyouKnow" AssociatedControlID="chkRead" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                            </div>
                                                        </div>                                                        
                                                        <div class="col-sm-2 clearfix">
                                                            <asp:Label ID="lblRead" meta:resourcekey="lblRead" AssociatedControlID="chkRead" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                            <asp:CheckBox ID="chkRead" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                                        </div>
                                                        <div class="col-sm-2 text-right">
                                                            <asp:Label ID="lblWrite" meta:resourcekey="lblWrite" AssociatedControlID="chkWrite" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                            <asp:CheckBox ID="chkWrite" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                                        </div>                                                        
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-5">
                                                             <div class="col-xs-1 ml-auto mt-auto mb-auto">
                                                                <img  src="../Content/images/bulb_icon.png" />
                                                            </div>
                                                            <div class="col-sm-10"> 
                                                                <asp:Label ID="lblGrade" meta:resourcekey="lblGrade" AssociatedControlID="cboGrade" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-4">                                                            
                                                            <asp:DropDownList ID="cboGrade" runat="server"  CssClass="form-control selectpicker" data-live-search="true" AutoPostBack="true" OnSelectedIndexChanged="cboGrade_SelectedIndexChanged" ></asp:DropDownList>
                                                            <label id="cboGradeValidation" for="<%= cboGrade.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgLastGradeReachedValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>                                            
                                        </div> 
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-4 col-sm-offset-1">
                                                             <div class="col-xs-1 ml-auto mt-auto mb-auto">
                                                                <img  src="../Content/images/bulb_icon.png" />
                                                            </div>
                                                            <div class="col-sm-10"> 
                                                                <asp:Label ID="lblStudyCarrers"  AssociatedControlID="cboStudyYears" runat="server" Text="" CssClass="control-label text-left"><%=GetLocalResourceObject("lblStudyYears")%> </asp:Label>
                                                            </div>
                                                        </div>                                                        
                                                        <div class="col-sm-4 clearfix">
                                                            <asp:DropDownList ID="cboStudyYears" runat="server" CssClass="form-control" ></asp:DropDownList>
                                                              <label id="cboStudyYearsValidation" for=" <%= cboStudyYears.ClientID %> " class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgStudyYearValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                                <asp:HiddenField ID="hdfStudy" runat="server"/>
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
                                                            <div class="col-xs-1 ml-auto mt-auto mb-auto">
                                                                <img  src="../Content/images/bulb_icon.png" />
                                                            </div>
                                                            <div class="col-sm-10 mr-auto mt-auto mb-auto">                                                                        
                                                                <asp:Label ID="lblGraduate" meta:resourcekey="lblGraduate" AssociatedControlID="chkGraduate" runat="server" Text="" CssClass=" text-left"></asp:Label>
                                                            </div>
                                                        </div>  
                                                        <div class="col-sm-4" >
                                                            <asp:CheckBox ID="chkGraduate" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                         <div class="row">                                            
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-5 ">
                                                             <div class="col-xs-1 ml-auto mt-auto mb-auto">
                                                                <img  src="../Content/images/bulb_icon.png" />
                                                            </div>
                                                            <div class="col-sm-10 mr-auto mt-auto mb-auto"> 
                                                                <asp:Label ID="lblCurrentlyStudying" meta:resourcekey="lblCurrentlyStudying" AssociatedControlID="chkCurrentlyStudying" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <asp:CheckBox ID="chkCurrentlyStudying" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
	                                        <div class="col-sm-12">
		                                        <div class="form-horizontal">
			                                        <div id="education" class="form-group">
				                                        <div class="col-sm-4 col-sm-offset-1">
                                                            <strong>
                                                                 <div class="col-xs-1 ml-auto mt-auto mb-auto">
                                                                    <img  src="../Content/images/bulb_icon.png" />
                                                                </div>
                                                            <div class="col-sm-10 mr-auto mt-auto mb-auto"> 
                                                                <asp:Label ID="lblStudyCarrer" meta:resourcekey="lblStudyCarrer"  runat="server" Text="" CssClass="control-label text-left "><%=GetLocalResourceObject("lblStudyCarrer")%></asp:Label>
                                                            </div>
                                                            </strong>
				                                        </div>
                                                        <div >
                                                            <div class="col-sm-2">
                                                                <asp:DropDownList ID="cboDegreeFormationType" OnSelectedIndexChanged="cboDegreeFormationType_SelectedIndexChanged"  AutoPostBack="true" runat="server" CssClass="form-control selectpicker" data-live-search="true" Enabled="false" ></asp:DropDownList>
                                                                <label id="cboDegreeFormationTypeValidation" for="<%= cboDegreeFormationType.ClientID%>" class="label label-danger label-validation" disabled="disabled" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgLanguagesValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                            </div> 
                                                            <div class="col-sm-2">
                                                                <asp:DropDownList ID="cboOtherEducation" runat="server" CssClass="form-control selectpicker" data-live-search="true" Enabled="false"  AutoPostBack="true" OnSelectedIndexChanged="cboOtherEducation_SelectedIndexChanged"></asp:DropDownList>
                                                                <label id="cboOtherEducationValidation" for="<%= cboOtherEducation.ClientID%>" class="label label-danger label-validation" disabled="disabled" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgLanguagesValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
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
				                                        <div class="col-sm-4 col-sm-offset-1">
                                                            <strong>
                                                                 <div class="col-xs-1 ml-auto mt-auto mb-auto">
                                                                <img  src="../Content/images/bulb_icon.png" />
                                                            </div>
                                                            <div class="col-sm-10 mr-auto mt-auto mb-auto"> 
                                                                <asp:Label ID="lblYearStudyCarrer" runat="server" Text="" AssociatedControlID="cboStudyYearCarrer" CssClass="control-label text-left "><%=GetLocalResourceObject("lblYearStudyCarrer")%></asp:Label>
                                                           </div>
                                                                </strong>
				                                        </div>
                                                        <div class="col-sm-4">
                                                            <asp:DropDownList ID="cboStudyYearCarrer" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="cboStudyYearCarrer_SelectedIndexChanged" Enabled="false"></asp:DropDownList>
                                                            <label id="cboStudyYearCarrerValidation" for="<%= cboStudyYearCarrer.ClientID%>" class="label label-danger label-validation" disabled="disabled" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgLanguagesValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
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
                                                            <asp:Label ID="lblNativeLanguage" meta:resourcekey="lblNativeLanguage" AssociatedControlID="cboNativeLanguages" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <asp:DropDownList ID="cboNativeLanguages" runat="server" CssClass="form-control selectpicker" data-live-search="true" AutoPostBack="false"></asp:DropDownList>
                                                            <label id="cboNativeLanguagesValidation" for="<%= cboNativeLanguages.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgNativeLanguagesValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
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
                                                            <asp:Label ID="lblLanguages" meta:resourcekey="lblLanguages" AssociatedControlID="cboLanguages" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <asp:DropDownList ID="cboLanguages" runat="server" CssClass="form-control selectpicker" data-live-search="true" multiple="multiple" data-selected-text-format="count > 4"  AutoPostBack="false"></asp:DropDownList>
                                                            <label id="cboLanguagesValidation" for="<%= cboLanguages.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgLanguagesValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                            <asp:HiddenField ID="hdfEmployeeSelectedLanguages" runat="server" />
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <span class="glyphicon glyphicon-info-sign text-center" data-toggle="tooltip" title='<%=GetLocalResourceObject("MultiSelectionDropDownListTooltip")%>' style="cursor:pointer"></span>
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

            $('#<%= chkRead.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });
            $('#<%= chkWrite.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });
          
            $('#<%= chkGraduate.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });
            $('#<%= chkCurrentlyStudying.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });
            $('#<%= chkCurrentlyStudying.ClientID %>').change(function () {
                ConfigureQuestionStudy();
            });

            $('#<%= chkWrite.ClientID %>').change(function () {
                
                $('#<%= cboStudyYears.ClientID %>').val(-1);
                $('#<%= cboGrade.ClientID %>').val(-1);

                $('#<%= cboGrade.ClientID %>').selectpicker('refresh');
            });

            $('#<%= chkRead.ClientID %>').change(function () {
                $('#<%= cboStudyYears.ClientID %>').val(-1);
                $('#<%= cboGrade.ClientID %>').val(-1);

                $('#<%= cboGrade.ClientID %>').selectpicker('refresh');
            });

            $("#<%= cboLanguages.ClientID %>").on('changed.bs.select', function (e) {
                /// <summary>Handles the select's value changed event for the cboLanguages control.</summary>
                MultiSelectDropdownListSaveSelectedItems($("#<%= cboLanguages.ClientID %>"), $("#<%= hdfEmployeeSelectedLanguages.ClientID %>"));
            });

            $("#<%= cboStudyYears.ClientID %>").change( function (e) {
                ValidateAcademicYear();
            });
          
            RestoreSelectedLanguages();
            ConfigureQuestionStudy();
            
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

        function ValidateAcademicYear() {
            var read = $("#<%= chkRead.ClientID %>").prop('checked');
            var write = $("#<%= chkWrite.ClientID %>").prop('checked');
                var degreeComplete = $.trim($("#<%= cboGrade.ClientID %>").val());
            var yearComplete = $.trim($("#<%= cboStudyYears.ClientID %>").val());

            var listYears = JSON.parse(localStorage.getItem('YearDegrees'));
            var divisionCode = localStorage.getItem('DivisionCode');

            var yearObject = listYears.filter(z => z.AcademicDegreeCode == degreeComplete && z.DivisionCode == divisionCode && z.Coursing == 0 && z.AcademicYear == yearComplete);

            if (read && write) {
                if (!yearObject[0].ReadAndWrite) {
                    MostrarMensaje(TipoMensaje.ERROR, '<%= GetLocalResourceObject("msjAcademicDegreeInvalidRead").ToString()%>', null);
                    $("#<%= cboStudyYears.ClientID %>").val("-1");
                    return false;
                }
            } else {
                if (yearObject[0].ReadAndWrite) {
                    MostrarMensaje(TipoMensaje.ERROR, '<%= GetLocalResourceObject("msjAcademicDegreeInvalidUnRead").ToString()%>', null);
                    $("#<%= cboStudyYears.ClientID %>").val("-1");
                    return false;
                }
            }
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
                ConfigureQuestionStudy();
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

            jQuery.validator.addMethod("ValidStudy", function (value, element) {
                /// <summary>Validate the spouse what work does</summary>            
                /// <returns> True if is valid. False otherwise. </returns>
                var areStudy = $("#<%= chkCurrentlyStudying.ClientID %>").prop('checked');
                if (areStudy) {
                    var areStudyDoes = $.trim($("#<%= cboOtherEducation.ClientID %>").val());
                    if (areStudyDoes == null || areStudyDoes == undefined || areStudyDoes.length === 0 || areStudyDoes === "" || areStudyDoes === "-1") {
                        return false
                    }
                }
                return true;
            }, "Please select a valid option");

            if (validatorSurvey == null) {               
                //declare the validator\
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
                                error.appendTo(element.parents('.radioclass'));
                            
                        },
                        rules: {                           
                           "<%= cboGrade.UniqueID %>": {
                                required: true
                                , validSelection: true
                            },
                            "<%= cboOtherEducation.UniqueID %>": {
                                ValidStudy: true
                            },                            
                            "<%= cboNativeLanguages.UniqueID %>": {
                                required: true
                                , validSelection: true
                            },
                             "<%= cboStudyYearCarrer.UniqueID %>": {
                                required: true
                                , validSelection: true
                            },                             
                            "<%= cboStudyYears.UniqueID %>": {
                                required: true
                                , validSelection: true
                            }

                        }
                });
            }
            var result = validatorSurvey.form();
            return result;
        }

        function RestoreSelectedLanguages() {
            /// <summary>Recover the selected items that were stored and assign them to the control to be selected.</summary>
            MultiSelectDropdownListRestoreSelectedItems($('#<%= cboLanguages.ClientID %>'), $('#<%= hdfEmployeeSelectedLanguages.ClientID %>'))
        }

        function ConfigureQuestionStudy() {
            /// <summary>Configure the screen controls for the question by the spouse work value</summary>
            var areStudy = $('#<%= chkCurrentlyStudying.ClientID %>').prop('checked');
            if (areStudy) {
                 $('#<%= cboStudyYearCarrer.ClientID %>').prop('disabled', false);
                $('#<%= cboOtherEducation.ClientID %>').prop('disabled', false);
                $('#<%= cboDegreeFormationType.ClientID %>').prop('disabled', false);                
            }
            else {

                $('#<%= cboStudyYearCarrer.ClientID %>').prop('disabled', true);
                $('#<%= cboOtherEducation.ClientID %>').prop('disabled', true);
                $('#<%= cboDegreeFormationType.ClientID %>').prop('disabled', true);

                $('#<%= cboStudyYearCarrer.ClientID %>').val("-1");
                $('#<%= cboOtherEducation.ClientID %>').val("-1");
                $('#<%= cboDegreeFormationType.ClientID %>').val("-1");

                $('#<%= cboStudyYearCarrer.ClientID %>').removeClass("Invalid");
                $('label[for=' + '<%= cboStudyYearCarrer.ClientID %>' + '].label-validation').hide();

                $('#<%= cboOtherEducation.ClientID %>').removeClass("Invalid");
                $('label[for=' + '<%= cboOtherEducation.ClientID %>' + '].label-validation').hide();
                $("#education").find('button').data("id",<%= cboOtherEducation.ClientID %>).removeClass("Invalid");

                $('#<%= cboDegreeFormationType.ClientID %>').removeClass("Invalid");
                $('label[for=' + '<%= cboDegreeFormationType.ClientID %>' + '].label-validation').hide();
            }

            $("#<%= cboOtherEducation.ClientID %>").selectpicker('refresh');
            $("#<%= cboStudyYearCarrer.ClientID %>").selectpicker('refresh');
            $("#<%= cboDegreeFormationType.ClientID %>").selectpicker('refresh');

         }
    </script>
</asp:Content>