<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="EnableClosedSurveys.aspx.cs" Inherits="HRISWeb.SocialResponsability.EnableClosedSurveys" %>
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
        <div class="container" style="width: 100%">
            <div class="row">
                <div class="col-sm-12">
                    <h4 class="text-left text-primary"><%= GetLocalResourceObject("lblLastSurvey.Text") %></h4>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-12">
                    <asp:UpdatePanel ID="upnList" runat="server">
                        <Triggers>                            
                            <asp:AsyncPostBackTrigger ControlID="lbtnEnableSurvey" EventName="Click"/>
                        </Triggers>
                        <ContentTemplate>
                            <asp:GridView ID="grvEmployeeSurveys" Width="100%" runat="server" EmptyDataText='<%$ Code:GetLocalResourceObject("lblNoData") %>' EmptyDataRowStyle-CssClass="emptyRow"
                                AllowPaging="false" PagerSettings-Visible="false" AllowSorting="false" AutoGenerateColumns="false" ShowHeader="true"
                                CssClass="table table-striped table-hover table-bordered" DataKeyNames="SurveyCode" OnPreRender="grvEmployeeSurveys_PreRender">
                                <Columns>
                                    <asp:BoundField DataField="EmployeeCode" meta:resourcekey="EmployeeCode" />
                                    <asp:BoundField DataField="EmployeeID" meta:resourcekey="EmployeeID" />
                                    <asp:BoundField DataField="EmployeeName" meta:resourcekey="EmployeeName" />
                                    <asp:BoundField DataField="SurveyStartDateTime" meta:resourcekey="SurveyStartDateTime" DataFormatString="{0:yyyy/MM/dd HH:mm}" />
                                    <asp:BoundField DataField="SurveyEndDateTime" meta:resourcekey="SurveyEndDateTime" DataFormatString="{0:yyyy/MM/dd HH:mm}" />
                                </Columns>
                            </asp:GridView>
                            <div id="grvListWaiting" style="position: absolute; margin: auto; display: none; bottom: 0px; top: 0px; left: 50%; transform: translate(-50%, 35%);">
                                <span class='fa fa-spinner fa-spin' style="font-size: 50px;"></span>
                            </div>
                            <h4 class="text-left text-primary" id="htmlResultsSubtitle" runat="server"></h4>
                            <asp:HiddenField ID="hdfEmployeeCode" runat="server" />
                            <asp:HiddenField runat="server" ID="hdfEmployeeSurveysSelectedRowIndex" Value="-1"/>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="ButtonsActions">
                <asp:UpdatePanel ID="upnActions" runat="server">
                    <Triggers>
                    </Triggers>
                    <ContentTemplate>
                        <div class="btn-group" role="group" aria-label="main-buttons">
                            <asp:LinkButton ID="lbtnEnableSurvey" runat="server" CssClass="btn btn-default btnAjaxAction" OnClick="lbtnEnableSurvey_Click"
                                data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("lbtnEnableSurvey.Text"))%>'
                                data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("lbtnEnableSurvey.Text"))%>'>
                                    <span class="glyphicon glyphicon-ok glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span><br />
                                    <%= GetLocalResourceObject("lbtnEnableSurvey.Text") %>
                            </asp:LinkButton>
                            <asp:LinkButton ID="lbtnSearchEmployee" runat="server" CssClass="btn btn-default btnAjaxAction" OnClick="lbtnSearchEmployee_Click"
                                data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("lbtnSearchEmployee.Text"))%>'
                                data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("lbtnSearchEmployee.Text"))%>'>
                                    <span class="glyphicon glyphicon-search glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span><br />
                                    <%= GetLocalResourceObject("lbtnSearchEmployee.Text") %>
                            </asp:LinkButton>                            
                        </div>
                        <asp:HiddenField ID="hdfIsCancelEmployeeSearchEnabled" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <%--  Modal User Search  --%>
    <div class="modal fade" id="employeeSearchDialog" tabindex="-1" role="dialog" aria-labelledby="employeeSearchTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <strong>
                        <h3 class="modal-title text-primary text-center" id="employeeSearchDialogTitle"><%= GetLocalResourceObject("lblEmployeeSearchTitle.Text") %></h3>
                    </strong>
                </div>
                <asp:UpdatePanel runat="server" ID="upnEmployeeSearch">
                    <ContentTemplate>
                        <div class="modal-body">                            
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="form-horizontal">
                                        <div class="form-group">                                            
                                            <div class="col-sm-4">
                                                <asp:Label ID="lblSearchEmpoyeeCode" meta:resourcekey="lblSearchEmpoyeeCode" AssociatedControlID="txtEmployeeCodeSearch" runat="server" Text="" CssClass="text-left"></asp:Label>
                                            </div>
                                            <div class="col-sm-5">
                                                <asp:TextBox ID="txtEmployeeCodeSearch" TabIndex="1" MaxLength="20" CssClass="form-control control-validation cleanPasteText" onkeypress="return isNumberOrLetter(event);" runat="server" meta:resourcekey="txtEmployeeCodeSearch" placeholder=""></asp:TextBox>
                                                <label id="lblSearchEmployeeCodeValidation" for="<%= txtEmployeeCodeSearch.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjEmployeeSearchValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                            </div>
                                            <div class="col-sm-2">
                                                <button id="btnEmployeeSearch" tabindex="2" type="button" runat="server" class="btn btn-primary btnAjaxAction" onclick="return ProcessEmployeeSearchRequest(this.id);" onserverclick="btnEmployeeSearch_ServerClick"
                                                    data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>' 
                                                    data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>'>
                                                    <span class="glyphicon glyphicon-search glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnSearch")) %>
                                                </button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <br /> 
                            <div class="row">
                                <asp:HiddenField runat="server" ID="hdfSelectedRowIndex" Value="-1"/>
                                <div class="scrolling-table-container-vertical">
                                <asp:GridView ID="grvEmployees" CssClass="table table-bordered table-striped table-hover" ShowHeader="true"
                                    OnPreRender="grvEmployees_PreRender" Width="100%" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                    EmptyDataRowStyle-CssClass="emptyRow" PagerSettings-Visible="false" AllowSorting="false"
                                    DataKeyNames="EmployeeCode,GeographicDivisionCode" ShowHeaderWhenEmpty="True" EmptyDataText='<%$ Code:GetLocalResourceObject("lblNoData") %>'>
                                    <Columns>
                                        <asp:BoundField DataField="EmployeeCode" ItemStyle-Width="25%" HeaderText='<%$ Code:GetLocalResourceObject("grvEmployeesEmployeeCodeHeaderText") %>' />
                                        <asp:BoundField DataField="GeographicDivisionCode" HeaderText="GeographicDivisionCode" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" FooterStyle-CssClass="hidden"/>
                                        <asp:BoundField DataField="ID" ItemStyle-Width="25%" HeaderText='<%$ Code:GetLocalResourceObject("grvEmployeesEmployeeIDHeaderText") %>' />
                                        <asp:BoundField DataField="EmployeeName" ItemStyle-Width="50%" HeaderText='<%$ Code:GetLocalResourceObject("grvEmployeesEmployeeFullNameHeaderText") %>' />
                                    </Columns>
                                </asp:GridView>
                            </div>
                            </div>                           
                            <br />                            
                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton ID="lbtnAcceptSelectedEmployee" runat="server" CssClass="btn btn-default btnAjaxAction" OnClick="lbtnAcceptSelectedEmployee_Click" TabIndex="6"
                                data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("lbtnAcceptSelectedEmployee.Text"))%>'
                                data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("lbtnAcceptSelectedEmployee.Text"))%>'>
                                <span class="glyphicon glyphicon-ok glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span><br />
                                <%= GetLocalResourceObject("lbtnAcceptSelectedEmployee.Text") %>
                            </asp:LinkButton>
                            <asp:LinkButton ID="lbtnCancelEmployeeSearch" runat="server" CssClass="btn btn-default btnAjaxAction" OnClick="lbtnCancelEmployeeSearch_Click" OnClientClick="return ProcessCancelEmployeeSearchRequest(this.id);"
                                data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("lbtnCancelEmployeeSearch.Text"))%>' TabIndex="7"
                                data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("lbtnCancelEmployeeSearch.Text"))%>'>
                                <span class="glyphicon glyphicon-remove glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span><br />
                                <%= GetLocalResourceObject("lbtnCancelEmployeeSearch.Text") %>
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
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
            // Add the grvEmployees selection row functionality
            $('#<%= grvEmployees.ClientID %>').on('click', 'tbody tr', function (event) {
                if (! $(this).hasClass('emptyRow')) {
                    $(this).toggleClass('info').siblings().removeClass('info');
                    $('#<%=hdfSelectedRowIndex.ClientID%>').val($(this).hasClass("info") ? $(this).index() : -1);
                    $( "#<%= lbtnAcceptSelectedEmployee.ClientID %>" ).focus();
                }
            });
             $('#<%= grvEmployees.ClientID %>').on('keypress', 'tbody tr', function (event) {
                 $(this).click();
            });

            // Add the grvEmployeeSurveys selection row functionality
            $('#<%= grvEmployeeSurveys.ClientID %>').on('click', 'tbody tr', function (event) {
                if (! $(this).hasClass('emptyRow')) {
                    $(this).toggleClass('info').siblings().removeClass('info');
                    $('#<%=hdfEmployeeSurveysSelectedRowIndex.ClientID%>').val($(this).hasClass("info") ? $(this).index() : -1);
                }
            });
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
            $('#<%= lbtnAcceptSelectedEmployee.ClientID%>').on('click', function(ev){
                /// <summary>Handles the click event for button lbtnAcceptSelectedEmployee.</summary>
                ev.preventDefault();
                disableButton($('#<%= lbtnAcceptSelectedEmployee.ClientID %>'));
                disableButton($('#<%= lbtnCancelEmployeeSearch.ClientID %>'));
                disableButton($('#<%= btnEmployeeSearch.ClientID %>'));
                $('#<%= txtEmployeeCodeSearch.ClientID %>').prop('disabled', true);

                if(IsRowSelected()){                                        
                    __doPostBack('<%= lbtnAcceptSelectedEmployee.UniqueID %>', 'OnClick');
                }
                else{                    
                    MostrarMensaje(TipoMensaje.VALIDACION, '<%= GetLocalResourceObject("msgInvalidSelection") %>', function () {
                        $('#<%= lbtnAcceptSelectedEmployee.ClientID %>').button('reset');
                        enableButton($('#<%= lbtnAcceptSelectedEmployee.ClientID %>'));
                        enableButton($('#<%= lbtnCancelEmployeeSearch.ClientID %>'));
                        enableButton($('#<%= btnEmployeeSearch.ClientID %>'));
                        $('#<%= txtEmployeeCodeSearch.ClientID %>').prop('disabled', false);
                    });
                }
            });
            $('#<%= lbtnEnableSurvey.ClientID%>').on('click', function(ev){
                /// <summary>Handles the click event for button lbtnAcceptSelectedEmployee.</summary>
                ev.preventDefault();
                disableButton($('#<%= lbtnEnableSurvey.ClientID %>'));
                if(IsEmployeeSurveyRowSelected()){                                        
                    __doPostBack('<%= lbtnEnableSurvey.UniqueID %>', 'OnClick');
                }
                else{                    
                    MostrarMensaje(TipoMensaje.VALIDACION, '<%= GetLocalResourceObject("msgInvalidSelection") %>', function () {
                        $('#<%= lbtnEnableSurvey.ClientID %>').button('reset');
                        enableButton($('#<%= lbtnEnableSurvey.ClientID %>'));
                    });
                }
            });
            $('#<%=txtEmployeeCodeSearch.ClientID%>').on('keypress', function(event){
                if (event.keyCode === 10 || event.keyCode === 13) {
                    $('#<%=btnEmployeeSearch.ClientID%>').click();                    
                }
            });
            $('#<%= lbtnSearchEmployee.ClientID%>').on('click', function(ev){
                /// <summary>Handles the click event for button lbtnSearchEmployee.</summary>
                ev.preventDefault();
                disableButton($('#<%= lbtnSearchEmployee.ClientID %>'));
                disableButton($('#<%= lbtnEnableSurvey.ClientID %>'));                
                $('#<%= txtEmployeeCodeSearch.ClientID %>').val('');                
                $('#<%= hdfIsCancelEmployeeSearchEnabled.ClientID %>').val('1');
                __doPostBack('<%= lbtnSearchEmployee.UniqueID %>', 'OnClick');                
            });
            //each time a ajax or page load execue we need to sync the selected row with its value
            SetRowSelected();
            SetEmployeeSurveysRowSelected();
        }
        function showSearchModal(currentUserIsModuleAdmin){
            /// <summary>Show the search employees modal or a warning for non admin users.</summary>
            if(currentUserIsModuleAdmin == true){
                enableButton($('#<%= lbtnEnableSurvey.ClientID %>'));
                enableButton($('#<%= lbtnSearchEmployee.ClientID %>'));
                $('#employeeSearchDialog').modal('show');
                setTimeout(function () { $('#<%= txtEmployeeCodeSearch.ClientID %>').focus(); }, 500);
            }
            else{
                disableButton($('#<%= lbtnSearchEmployee.ClientID %>'));
                disableButton($('#<%= lbtnEnableSurvey.ClientID %>'));    
                setTimeout(function () {
                    MostrarMensaje(TipoMensaje.INFORMACION, '<%= GetLocalResourceObject("msgNoAdminUser") %>', function () {
                        window.location.href = '../Default.aspx';
                    });
                }, 100);
            }
        }
        function ProcessAcceptSelectedEmployeeResponse(hideSearchDialog) {
             disableButton($('#<%= lbtnAcceptSelectedEmployee.ClientID %>'));    
            /// <summary>Handles the on click resopnse for button lbtnAcceptSelectedEmployee.</summary>
              if(hideSearchDialog === 1){
                    $('#employeeSearchDialog').modal('hide');
                }

            setTimeout(function () {
              
                $('#<%= lbtnAcceptSelectedEmployee.ClientID %>').button('reset');
                enableButton($('#<%= lbtnAcceptSelectedEmployee.ClientID %>'));
                enableButton($('#<%= lbtnCancelEmployeeSearch.ClientID %>'));
                enableButton($('#<%= btnEmployeeSearch.ClientID %>'));
            }, 500);
        }
        function ProcessEnableEmployeeSurveyResponse(isEnabledSuccessful){
            /// <summary>Handles the on click response for button lbtnEnableSurvey.</summary>
            setTimeout(function () {  
                if(isEnabledSuccessful === 1){
                    MostrarMensaje(TipoMensaje.INFORMACION, '<%= HttpUtility.JavaScriptStringEncode(GetLocalResourceObject("msgEmployeeSurveyEnabledSuccessful").ToString()) %>', function () {
                        $('#<%= lbtnEnableSurvey.ClientID %>').button('reset');
                        enableButton($('#<%= lbtnEnableSurvey.ClientID %>'));
                    });
                }
                else{
                    $('#<%= lbtnEnableSurvey.ClientID %>').button('reset');
                    enableButton($('#<%= lbtnEnableSurvey.ClientID %>'));
                }
            }, 300);
        }
        function ProcessCancelEmployeeSearchRequest(resetId){
            /// <summary>Handles the on client click event for button lbtnCancelEmployeeSearch.</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            disableButton($('#<%= lbtnAcceptSelectedEmployee.ClientID %>'));
            disableButton($('#<%= lbtnCancelEmployeeSearch.ClientID %>'));
            disableButton($('#<%= btnEmployeeSearch.ClientID %>'));
            $('#<%= txtEmployeeCodeSearch.ClientID %>').prop('disabled', true);
            var isCancelEnabled = $('#<%= hdfIsCancelEmployeeSearchEnabled.ClientID %>').val();
            //if(isCancelEnabled==='0'){
            //    window.location.href = '../Default.aspx';
            //}
            //else{
            //    $('#employeeSearchDialog').modal('hide');
            //}
            $('#employeeSearchDialog').modal('hide');
            return false;
        }
        function ProcessSearchEmployeeResponse(){
            /// <summary>Handles the on click response for button lbtnSearchEmployee.</summary>                            
            showSearchModal(1);
            setTimeout(function(){                
                $('#<%= lbtnSearchEmployee.ClientID %>').button('reset');
                enableButton($('#<%= lbtnEnableSurvey.ClientID %>'));
                enableButton($('#<%= lbtnSearchEmployee.ClientID %>'));

                $('#<%= lbtnCancelEmployeeSearch.ClientID %>').button('reset');
                enableButton($('#<%= lbtnAcceptSelectedEmployee.ClientID %>'));
                enableButton($('#<%= lbtnCancelEmployeeSearch.ClientID %>'));
                enableButton($('#<%= btnEmployeeSearch.ClientID %>'));
                $('#<%= txtEmployeeCodeSearch.ClientID %>').prop('disabled', false);
            }, 150);
        }
        function IsRowSelected() {
            /// <summary>Validate if there is a selected row</summary>  
            var selectedRowIndex = $('#<%=hdfSelectedRowIndex.ClientID%>').val();
            if (!isEmptyOrSpaces(selectedRowIndex) && selectedRowIndex != "-1") {
                return true;
            }
            return false;
        }
        function IsEmployeeSurveyRowSelected() {
            /// <summary>Validate if there is a selected row</summary>  
            var selectedRowIndex = $('#<%=hdfEmployeeSurveysSelectedRowIndex.ClientID%>').val();
            if (!isEmptyOrSpaces(selectedRowIndex) && selectedRowIndex != "-1") {
                return true;
            }
            return false;
        }
        function SetRowSelected() {
            /// <summary>Set the class of the selected row</summary>  
            var selectedRowIndex = $('#<%=hdfSelectedRowIndex.ClientID%>').val();
            if (!isEmptyOrSpaces(selectedRowIndex) && selectedRowIndex != "-1") {
                var selectedRow = $('#<%= grvEmployees.ClientID %> tbody tr:eq(' + selectedRowIndex + ')');
                selectedRow.siblings().removeClass('info');    
                if (!selectedRow.hasClass('info'))
                {
                    selectedRow.addClass('info');
                }
            }
        }
        function SetEmployeeSurveysRowSelected() {
            /// <summary>Set the class of the selected row</summary>  
            var selectedRowIndex = $('#<%=hdfEmployeeSurveysSelectedRowIndex.ClientID%>').val();
            if (!isEmptyOrSpaces(selectedRowIndex) && selectedRowIndex != "-1") {
                var selectedRow = $('#<%= grvEmployeeSurveys.ClientID %> tbody tr:eq(' + selectedRowIndex + ')');
                selectedRow.siblings().removeClass('info');    
                if (!selectedRow.hasClass('info'))
                {
                    selectedRow.addClass('info');
                }
            }
        }
        function UnselectRow() {
            /// <summary>Unselect rows</summary>  
            $('#<%=hdfSelectedRowIndex.ClientID%>').val("-1");
            $('#<%= grvEmployees.ClientID %> tbody tr').removeClass('info');            
        }
        function UnselectEmployeeSurveyRow() {
            /// <summary>Unselect rows</summary>  
            $('#<%=hdfEmployeeSurveysSelectedRowIndex.ClientID%>').val("-1");
            $('#<%= grvEmployeeSurveys.ClientID %> tbody tr').removeClass('info');            
        }
        function ProcessEmployeeSearchRequest(resetId){
            /// <summary>Process the request for the employee search</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            disableButton($('#<%= lbtnAcceptSelectedEmployee.ClientID %>'));
            disableButton($('#<%= lbtnCancelEmployeeSearch.ClientID %>'));            
            if(!ValidateEmployeeSearch()){
                $('#<%= txtEmployeeCodeSearch.ClientID %>').prop('disabled', true);
                MostrarMensaje(TipoMensaje.VALIDACION, '<%= GetLocalResourceObject("msgInvalidEntry") %>', function(){
                    setTimeout(function () {                        
                        ResetButton(resetId);
                        enableButton($('#<%= lbtnAcceptSelectedEmployee.ClientID %>'));
                        enableButton($('#<%= lbtnCancelEmployeeSearch.ClientID %>'));
                        $('#<%= txtEmployeeCodeSearch.ClientID %>').prop('disabled', false);
                    }, 200);
                });
                return false;
            }
            else{
                $('#<%= txtEmployeeCodeSearch.ClientID %>').prop('disabled', true);
                __doPostBack('<%= btnEmployeeSearch.UniqueID %>', '');
            }
            return false;
        }

          var setTAbIndex = function () {
                
                $("tbody tr").attr( "tabIndex", function ( i ) {
                    return 4;
                });
        }   

        function ProcessEmployeeSearchResponse(){
            /// <summary>Process the response for the employee search</summary>
            setTimeout(function () {                    
                ResetButton($('#<%= btnEmployeeSearch.ClientID %>').id);
                enableButton($('#<%= lbtnAcceptSelectedEmployee.ClientID %>'));
                enableButton($('#<%= lbtnCancelEmployeeSearch.ClientID %>'));
                $('#<%= txtEmployeeCodeSearch.ClientID %>').prop('disabled', false);
                UnselectRow();
                setTAbIndex();
            }, 200);
        }

        function SetControlInvalid(controlId) {
            /// <summary>Set the appearance of the control as invalid </summary>
            /// <param name="controlId" type="String">Id of the control</param>            
            $('#' + controlId).addClass("Invalid");            
            $('label[for=' + controlId + '].label-validation').show();
        }
        function SetControlValid(controlId) {
            /// <summary>Set the appearance of the control as valid </summary>
            /// <param name="controlId" type="String">Id of the control</param>
            $('#' + controlId).removeClass("Invalid");
            $('label[for=' + controlId + '].label-validation').hide();
        }
        var validatorEmployeeSearch = null;
        function ValidateEmployeeSearch() {
            /// <summary>Validate the form with jquey validation plugin </summary>
            /// <returns> True if form is valid. False otherwise. </returns>
            $('#' + document.forms[0].id).validate().destroy();
            if (validatorEmployeeSearch == null) {               
                //declare the validator
                var validatorEmployeeSearch =
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
                            <%= txtEmployeeCodeSearch.UniqueID %>: {
                                required: true
                                , minlength: 1
                                , normalizer: function( value ) {                                    
                                    return $.trim( value );
                                }
                            }
                        }
                    });
            }
            //get the results            
            var result = validatorEmployeeSearch.form();
            return result;
        }                
        //*******************************//
        //AJAX CONCURRENCY ADMINISTRATION// 
        //*******************************//
        //In this section we set the control for multiple simultaneous ajax request
        //We cancel the ajax request executed when another one is in progress
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(initializeRequest);
        function initializeRequest(sender, args) {
            if (prm.get_isInAsyncPostBack()) {
                args.set_cancel(true);
                //this line was initially included but causes that the original postback being cancel too
                //prm.abortPostBack();

                ShowFooterAlert('<%=GetLocalResourceObject("msgWaitWhileProcessing")%>');
                AddTemporaryClass($("#" + args.get_postBackElement().id), "btn-warning", 1500);
                setTimeout(function () {
                    $("#" + args.get_postBackElement().id).button('error');
                    setTimeout(function () {
                        $("#" + args.get_postBackElement().id).button('reset');
                    }, 1500)
                }, 100);
            }
        }
    </script>
</asp:Content>
