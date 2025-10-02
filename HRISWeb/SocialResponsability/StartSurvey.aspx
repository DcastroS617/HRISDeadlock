<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="StartSurvey.aspx.cs" Inherits="HRISWeb.SocialResponsability.StartSurvey" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntHeader" runat="server">
    <title><%=Convert.ToString(GetLocalResourceObject("lblScreenTitle")) %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cntBody" runat="server">
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
                        <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
                            <div class="panel panel-default">
                                <div class="panel-heading" role="tab" id="headingOne">
                                    <h4 class="panel-title">
                                        <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><%= GetLocalResourceObject("ObjectiveText") %></a>
                                    </h4>
                                </div>
                                <div id="collapseOne" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
                                    <div class="panel-body">
                                        <div class="form-group row">
                                            <asp:Label ID="lblObjetive" runat="server" meta:resourcekey="lblObjetive" Text=""></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-4">
                                        <asp:Label ID="lblStartDateTime" meta:resourcekey="lblStartDateTime" AssociatedControlID="txtStartDateTime" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtStartDateTime" meta:resourcekey="txtStartDateTime" runat="server" CssClass="form-control" disabled="disabled" placeholder=""></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-4">
                                        <asp:Label ID="lblCompany" meta:resourcekey="lblCompany" AssociatedControlID="txtCompany" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtCompany" meta:resourcekey="txtCompany" runat="server" CssClass="form-control" disabled="disabled" placeholder=""></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-4">
                                        <asp:Label ID="lblFarm" meta:resourcekey="lblFarm" AssociatedControlID="txtFarm" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtFarm" meta:resourcekey="txtFarm" runat="server" CssClass="form-control" disabled="disabled" placeholder=""></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-4">
                                        <asp:Label ID="lblDepartment" meta:resourcekey="lblDepartment" AssociatedControlID="txtDepartment" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtDepartment" meta:resourcekey="txtDepartment" runat="server" CssClass="form-control" disabled="disabled" placeholder=""></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-4">
                                        <asp:Label ID="lblPosition" meta:resourcekey="lblPosition" AssociatedControlID="txtPosition" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtPosition" meta:resourcekey="txtPosition" runat="server" CssClass="form-control" disabled="disabled" placeholder=""></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-4">
                                        <asp:Label ID="lblHireDate" meta:resourcekey="lblHireDate" AssociatedControlID="txtHireDate" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtHireDate" meta:resourcekey="txtHireDate" runat="server" CssClass="form-control" disabled="disabled" placeholder=""></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-4">
                                        <asp:Label ID="lblTelephone" meta:resourcekey="lblTelephone" AssociatedControlID="txtTelephone" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtTelephone" meta:resourcekey="txtTelephone" runat="server" CssClass="form-control" disabled="disabled" placeholder=""></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-4">
                                    </div>
                                    <div class="col-sm-8">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="form-horizontal">
                                <ul class="nav nav-tabs" id="surveyTab">
                                    <li class="nav-item active">
                                        <a class="nav-link active" id="personalprofile-tab" data-toggle="tab" href="#personalprofile" role="tab" aria-controls="personalprofile" aria-selected="true"><%= GetLocalResourceObject("personalprofile-tab") %></a>
                                    </li>
                                </ul>
                                <div class="tab-content" id="survey">
                                    <div class="tab-pane fade in active" id="personalprofile" role="tabpanel" aria-labelledby="personalprofile-tab">
                                        <p>
                                            <br />
                                        </p>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-3">
                                                            <asp:Label ID="lblIdentificationDocumentNumber" meta:resourcekey="lblIdentificationDocumentNumber" AssociatedControlID="txtIdentificationDocumentNumber" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtIdentificationDocumentNumber" meta:resourcekey="txtIdentificationDocumentNumber" runat="server" CssClass="form-control" disabled="disabled" placeholder=""></asp:TextBox>
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
                                                            <asp:Label ID="lblFullName" meta:resourcekey="lblFullName" AssociatedControlID="txtName" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <asp:TextBox ID="txtName" meta:resourcekey="txtName" runat="server" CssClass="form-control" disabled="disabled" placeholder=""></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-6">
                                                            <asp:Label ID="lblBirthdate" meta:resourcekey="lblBirthdate" AssociatedControlID="txtBirthdate" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txtBirthdate" meta:resourcekey="txtBirthdate" runat="server" CssClass="form-control" disabled="disabled" placeholder=""></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-6">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-4">
                                                            <asp:Label ID="lblAge" meta:resourcekey="lblAge" AssociatedControlID="txtAge" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <asp:TextBox ID="txtAge" meta:resourcekey="txtAge" runat="server" CssClass="form-control" disabled="disabled" placeholder=""></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-6">
                                                            <asp:Label ID="lblNationality" meta:resourcekey="lblNationality" AssociatedControlID="txtNationality" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txtNationality" meta:resourcekey="txtNationality" runat="server" CssClass="form-control" disabled="disabled" placeholder=""></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-6">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-4">
                                                            <asp:Label ID="lblBirthProvince" AssociatedControlID="cboProvince" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <asp:DropDownList ID="cboProvince" runat="server" CssClass="form-control selectpicker" data-live-search="true" AutoPostBack="false"></asp:DropDownList>
                                                            <label id="cboProvinceValidation" for="<%= cboProvince.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjBirthProvinceValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-6">
                                                            <asp:Label ID="lblSex" meta:resourcekey="lblSex" AssociatedControlID="txtGender" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txtGender" meta:resourcekey="txtGender" runat="server" CssClass="form-control" disabled="disabled" placeholder=""></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-6">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-6">
                                                        </div>
                                                        <div class="col-sm-6">
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
                                                            <asp:Label ID="lblMaritalStatus" meta:resourcekey="lblMaritalStatus" AssociatedControlID="rdbMaritalStatus" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-8">
                                                            <asp:RadioButtonList ID="rdbMaritalStatus" runat="server" CssClass="control-validation" RepeatDirection="Horizontal" TextAlign="Right" CellSpacing="50" RepeatLayout="Table" Width="100%" CellPadding="50"></asp:RadioButtonList>
                                                            <label id="rdbMaritalStatusValidation" for="<%= rdbMaritalStatus.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjMaritalStatusValidation") %>" style="display: none; float: right; margin-right: 0px; margin-top: -23px; position: relative; z-index: 2;">!</label>
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
                        <div class="col-sm-12 text-right">
                            <div class="btn-group" role="group" aria-label="main-buttons">
                                <asp:LinkButton ID="lbtnStartSurvey" runat="server" CssClass="btn btn-default btnAjaxAction" OnClick="lbtnStartSurvey_Click"
                                    data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("lbtnStartSurvey.Text"))%>'
                                    data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("lbtnStartSurvey.Text"))%>'>
                                    <span class="glyphicon glyphicon-arrow-right glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span><br />
                                    <%= GetLocalResourceObject("lbtnStartSurvey.Text") %>
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                    <br />
                    <br />
                    <asp:HiddenField ID="hdfCurrentUserIsModuleAdmin" runat="server" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <%--  Modal for Informed Consent  --%>
    <div class="modal fade" id="InformedConsentDialog" tabindex="-1" role="dialog" aria-labelledby="InformedConsentTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <strong>
                        <h3 class="modal-title text-primary text-center" id="dialogTitle"><%= GetLocalResourceObject("lblInformedConsentTitle.Text") %></h3>
                    </strong>
                </div>
                <asp:UpdatePanel runat="server" ID="uppDialogControl">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="col-sm-12">
                                                <asp:Label ID="lblInformedConsentText" meta:resourcekey="lblInformedConsentText" AssociatedControlID="" runat="server" Text="" CssClass="text-justify"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="col-sm-12 text-center">
                                                <asp:CheckBox ID="chkInformedConsent" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <br />
                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton ID="lbtnAccept" runat="server" CssClass="btn btn-default btnAjaxAction"
                                data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("lbtnAccept.Text"))%>'
                                data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("lbtnAccept.Text"))%>'>
                                <span class="glyphicon glyphicon-ok glyphicon-main-button" aria-hidden="true" aria-label="main-buttons" style="pointer-events:none"></span><br />
                                <%= GetLocalResourceObject("lbtnAccept.Text") %>
                            </asp:LinkButton>
                            <asp:LinkButton ID="lbtnCancel" runat="server" CssClass="btn btn-default btnAjaxAction" OnClick="lbtnCancel_Click"
                                data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("lbtnCancel.Text"))%>'
                                data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("lbtnCancel.Text"))%>'>
                                <span class="glyphicon glyphicon-remove glyphicon-main-button" aria-hidden="true" aria-label="main-buttons" style="pointer-events:none"></span><br />
                                <%= GetLocalResourceObject("lbtnCancel.Text") %>
                            </asp:LinkButton>
                        </div>
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
                                                <asp:TextBox ID="txtEmployeeCodeSearch" MaxLength="20" TabIndex="1" CssClass="form-control SpecialCaracter" onkeypress="return isNumberOrLetterNoEnter(event);" runat="server" meta:resourcekey="txtEmployeeCodeSearch" placeholder=""></asp:TextBox>
                                                <label id="lblSearchEmployeeCodeValidation" for="<%= txtEmployeeCodeSearch.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjEmployeeSearchValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                            </div>
                                            <div class="col-sm-2">
                                                <button id="btnEmployeeSearch" type="button" runat="server" tabindex="2" class="btn btn-primary btnAjaxAction" onclick="return ProcessEmployeeSearchRequest(this.id);" onserverclick="btnEmployeeSearch_ServerClick"
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
                                <asp:HiddenField runat="server" ID="hdfSelectedRowIndex" Value="-1" />
                                <div class="scrolling-table-container-vertical">
                                    <asp:GridView ID="grvEmployees" CssClass="table table-bordered table-striped table-hover" ShowHeader="true"
                                        OnPreRender="grvEmployees_PreRender" Width="100%" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                        EmptyDataRowStyle-CssClass="emptyRow" PagerSettings-Visible="false" AllowSorting="false"
                                        DataKeyNames="EmployeeCode,GeographicDivisionCode,CurrentState" ShowHeaderWhenEmpty="True" EmptyDataText='<%$ Code:GetLocalResourceObject("lblNoData") %>'>
                                        <Columns>
                                            <asp:BoundField DataField="EmployeeCode" ItemStyle-Width="25%" HeaderText='<%$ Code:GetLocalResourceObject("grvEmployeesEmployeeCodeHeaderText") %>' />
                                            <asp:BoundField DataField="GeographicDivisionCode" HeaderText="GeographicDivisionCode" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" FooterStyle-CssClass="hidden" />
                                            <asp:BoundField DataField="ID" ItemStyle-Width="25%" HeaderText='<%$ Code:GetLocalResourceObject("grvEmployeesEmployeeIDHeaderText") %>' />
                                            <asp:BoundField DataField="EmployeeName" ItemStyle-Width="50%" HeaderText='<%$ Code:GetLocalResourceObject("grvEmployeesEmployeeFullNameHeaderText") %>' />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div class="col-sm-10" style="">
                                    <asp:Label ID="lblScopeEmployee" Visible="false" meta:resourcekey="msjSurveyEmployeeScope" AssociatedControlID="" runat="server" Text="" CssClass="text-justify">
                                    </asp:Label>
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
                            <asp:LinkButton ID="lbtnCancelEmployeeSearch" runat="server" TabIndex="7" CssClass="btn btn-default btnAjaxAction" OnClick="lbtnCancelEmployeeSearch_Click" OnClientClick="return ProcessCancelEmployeeSearchRequest(this.id);"
                                data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("lbtnCancelEmployeeSearch.Text"))%>'
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


            $(".SpecialCaracter").change(function () {
                var val = $(this).val();
                val = val.replace(/[^a-zA-Z 0-9.]+/g, '');
                $(this).val(val);
            });


            /// <summary>Execute at load even at partial and ajax requests</summary>
            $('.btnAjaxAction').on('click', function () {
                var $this = $(this);
                $this.button('loading');
                setTimeout(function () { $this.button('reset'); }, 30000);
            });

            $('#<%= chkInformedConsent.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });

            //In this section we initialize the popovers and tooltips for the elements.
            //We have both (popovers and tooltips) in order to accomplish information alert over desktop 
            //and mobiles devices
            InitializeTooltipsPopovers();


            //And the grvList selection row functionality
            $('#<%= grvEmployees.ClientID %>').on('click', 'tbody tr', function (event) {
                if (!$(this).hasClass('emptyRow')) {
                    $(this).toggleClass('info').siblings().removeClass('info');
                    console.log($(this).index());
                    $('#<%=hdfSelectedRowIndex.ClientID%>').val($(this).hasClass("info") ? $(this).index() : -1);

                    $( "#<%= lbtnAcceptSelectedEmployee.ClientID %>").focus();



                }
            });

            $('#<%= grvEmployees.ClientID %>').on('keypress', 'tbody tr', function (event) {
                $(this).click();
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

            $('#<%= chkInformedConsent.ClientID %>').change(function () {
                var checkBoxValue = $(this).prop('checked');
                if (checkBoxValue === true) {
                    disableButton($('#<%= lbtnCancel.ClientID %>'));
                    setTimeout(function () {
                        enableButton($('#<%= lbtnAccept.ClientID %>'));
                    }, 200);
                }
                else {
                    disableButton($('#<%= lbtnAccept.ClientID %>'));
                    setTimeout(function () {
                        enableButton($('#<%= lbtnCancel.ClientID %>'));
                    }, 200);
                }
            });

            $('#<%= lbtnAccept.ClientID %>').on('click', function (ev) {
                /// <summary>Handles the click event for button btnAccept.</summary>
                ev.preventDefault();
                disableButton($('#<%= lbtnAccept.ClientID %>'));
                disableButton($('#<%= lbtnCancel.ClientID %>'));

                var checkBoxValue = $('#<%= chkInformedConsent.ClientID %>').prop('checked');
                if (!checkBoxValue) {
                    MostrarMensaje(TipoMensaje.VALIDACION, '<%= GetLocalResourceObject("msgInvalidInformedConsent") %>', function () {
                        $('#<%= lbtnAccept.ClientID %>').button('reset');
                        disableButton($('#<%= lbtnAccept.ClientID %>'));
                        enableButton($('#<%= lbtnCancel.ClientID %>'));
                    });
                }
                else {
                    var currentUserIsModuleAdmin = $('#<%= hdfCurrentUserIsModuleAdmin.ClientID %>').val();
                    $('#<%= lbtnAccept.ClientID %>').button('reset');
                    $('#InformedConsentDialog').modal('hide');

                    if (currentUserIsModuleAdmin == true) {
                        $('#employeeSearchDialog').modal('show');
                        setTimeout(function () { $('#<%= txtEmployeeCodeSearch.ClientID %>').focus(); }, 500);
                    }
                    else {
                        MostrarMensaje(TipoMensaje.INFORMACION, '<%= GetLocalResourceObject("SaveAsDraftText") %>', function () { });
                    }
                }
            });

            $('#<%= lbtnAcceptSelectedEmployee.ClientID%>').on('click', function (ev) {
                /// <summary>Handles the click event for button lbtnAcceptSelectedEmployee.</summary>
                ev.preventDefault();
                disableButton($('#<%= lbtnAcceptSelectedEmployee.ClientID %>'));
                disableButton($('#<%= lbtnCancelEmployeeSearch.ClientID %>'));
                disableButton($('#<%= btnEmployeeSearch.ClientID %>'));
                $('#<%= txtEmployeeCodeSearch.ClientID %>').prop('disabled', true);

                if (IsRowSelected()) {
                    __doPostBack('<%= lbtnAcceptSelectedEmployee.UniqueID %>', 'OnClick');
                }
                else {
                    MostrarMensaje(TipoMensaje.VALIDACION, '<%= GetLocalResourceObject("msgInvalidSelection") %>', function () {
                        $('#<%= lbtnAcceptSelectedEmployee.ClientID %>').button('reset');
                        enableButton($('#<%= lbtnAcceptSelectedEmployee.ClientID %>'));
                        enableButton($('#<%= lbtnCancelEmployeeSearch.ClientID %>'));
                        enableButton($('#<%= btnEmployeeSearch.ClientID %>'));
                        $('#<%= txtEmployeeCodeSearch.ClientID %>').prop('disabled', false);
                    });
                }
            });

            $('#<%= lbtnStartSurvey.ClientID %>').on('click', function (ev) {
                /// <summary>Handles the click event for button lbtnStartSurvey.</summary>
                ev.preventDefault();
                disableButton($('#<%= lbtnStartSurvey.ClientID %>'));

                if (!ValidateSurvey()) {
                    MostrarMensaje(TipoMensaje.VALIDACION, '<%= GetLocalResourceObject("msgInvalidEntry") %>', function () {
                        $('#<%= lbtnStartSurvey.ClientID %>').button('reset');
                        enableButton($('#<%= lbtnStartSurvey.ClientID %>'));
                    });
                }
                else {
                    setTimeout(function () {
                        $('#<%= lbtnStartSurvey.ClientID %>').button('reset');
                        enableButton($('#<%= lbtnStartSurvey.ClientID %>'));
                    }, 30000);
                    __doPostBack('<%= lbtnStartSurvey.UniqueID %>', 'OnClick');
                }
            });

            $('#<%=txtEmployeeCodeSearch.ClientID%>').on('keypress', function (event) {
                if (event.keyCode === 10 || event.keyCode === 13) {
                    var employeeSearchText = $.trim($('#<%= txtEmployeeCodeSearch.ClientID %>').val());
                    console.log(employeeSearchText);
                    if (employeeSearchText.length > 0) {
                        SetControlValid($('#<%=txtEmployeeCodeSearch.ClientID%>').attr('id'));
                        $('#<%=btnEmployeeSearch.ClientID%>').click();
                    }
                    else {
                        SetControlInvalid($('#<%=txtEmployeeCodeSearch.ClientID%>').attr('id'));
                    }
                }
            });

            //each time a ajax or page load execue we need to sync the selected row with its value
            SetRowSelected();
        }

        function ProcessAcceptSelectedEmployeeResponse(hideSearchDialog) {
            /// <summary>Handles the on click resopnse for button lbtnAcceptSelectedEmployee.</summary>

            setTimeout(function () {
                if (hideSearchDialog === 1) {
                    $('#employeeSearchDialog').modal('hide');
                }
                $('#<%= lbtnAcceptSelectedEmployee.ClientID %>').button('reset');
                enableButton($('#<%= lbtnAcceptSelectedEmployee.ClientID %>'));
                enableButton($('#<%= lbtnCancelEmployeeSearch.ClientID %>'));
                enableButton($('#<%= btnEmployeeSearch.ClientID %>'));

            }, 500);
        }

        function ProcessCancelEmployeeSearchRequest(resetId) {
            /// <summary>Handles the on client click event for button lbtnCancelEmployeeSearch.</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            disableButton($('#<%= lbtnAcceptSelectedEmployee.ClientID %>'));
            disableButton($('#<%= lbtnCancelEmployeeSearch.ClientID %>'));
            disableButton($('#<%= btnEmployeeSearch.ClientID %>'));
            $('#<%= txtEmployeeCodeSearch.ClientID %>').prop('disabled', true);
            return true;
        }

        function IsRowSelected() {
            /// <summary>Validate if there is a selected row</summary>  
            var selectedRowIndex = $('#<%=hdfSelectedRowIndex.ClientID%>').val();
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
                if (!selectedRow.hasClass('info')) {
                    selectedRow.addClass('info');
                }
            }
        }

        function UnselectRow() {
            /// <summary>Unselect rows</summary>  
            $('#<%=hdfSelectedRowIndex.ClientID%>').val("-1");
            $('#<%= grvEmployees.ClientID %> tbody tr').removeClass('info');
        }

        function ShowInformedConsentDialog() {
            /// <summary>Show the Informed Consent Dialog</summary>                                    
            setTimeout(function () {
                $('#InformedConsentDialog').modal('show');
                $('#<%= chkInformedConsent.ClientID %>').bootstrapToggle('off');
                disableButton($('#<%= lbtnAccept.ClientID %>'));
            }, 200);
        }

        function ProcessEmployeeSearchRequest(resetId) {
            /// <summary>Process the request for the employee search</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            disableButton($('#<%= lbtnAcceptSelectedEmployee.ClientID %>'));
            disableButton($('#<%= lbtnCancelEmployeeSearch.ClientID %>'));
            if (!ValidateEmployeeSearch()) {
                $('#<%= txtEmployeeCodeSearch.ClientID %>').prop('disabled', true);
                MostrarMensaje(TipoMensaje.VALIDACION, '<%= GetLocalResourceObject("msgInvalidEntry") %>', function () {
                    setTimeout(function () {
                        ResetButton(resetId);
                        enableButton($('#<%= lbtnAcceptSelectedEmployee.ClientID %>'));
                        enableButton($('#<%= lbtnCancelEmployeeSearch.ClientID %>'));
                        $('#<%= txtEmployeeCodeSearch.ClientID %>').prop('disabled', false);
                    }, 200);
                });
                return false;
            }
            else {
                $('#<%= txtEmployeeCodeSearch.ClientID %>').prop('disabled', true);
                __doPostBack('<%= btnEmployeeSearch.UniqueID %>', '');
            }
            return false;
        }
        var setTAbIndex = function () {

            $("tbody tr").attr("tabIndex", function (i) {
                return 4;
            });
        }

        function DisableModalSearchEmployes() {
            setTimeout(function () {
                if (hideSearchDialog === 1) {
                    $('#employeeSearchDialog').modal('hide');
                }
            }
            )
        };



        function ProcessEmployeeSearchResponse() {
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

            if ($("#" + controlId).is(".selectpicker")) {
                $('button[data-id=' + controlId + '].dropdown-toggle').addClass("Invalid");
                $('#' + controlId).addClass("Invalid");
                $('label[for=' + controlId + '].label-validation').show();
            }
            else if ($("#" + controlId).is(":radio")) {
                $('#<%= rdbMaritalStatus.ClientID %>').addClass("Invalid");
                $('label[for=' + '<%= rdbMaritalStatus.ClientID %>' + '].label-validation').show();
            } else {
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
            else if ($("#" + controlId).is(":radio")) {
                $('#<%= rdbMaritalStatus.ClientID %>').removeClass("Invalid");
                $('label[for=' + '<%= rdbMaritalStatus.ClientID %>' + '].label-validation').hide();
            }
            else {
                $('#' + controlId).removeClass("Invalid");
                $('label[for=' + controlId + '].label-validation').hide();
            }
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
                                , normalizer: function(value) {

                                    return $.trim(value);
                                }
                    }
                }
            
        });
            }
        //else
        //{
        //    validatorEmployeeSearch.validate();
        //}
        //get the results            
        var result = validatorEmployeeSearch.form();
        return result;
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

            jQuery.validator.addMethod("validCivilStatus", function (value, element) {
                return ValidateCivilStatus();
            }, "You must select a valid civil status");

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
                             <%= rdbMaritalStatus.UniqueID %>: {
                                validCivilStatus: true
                                , required: true
                }
            }
        });
            }
            else
        {
            validatorSurvey.validate();
        }
        //get the results            
        var result = validatorSurvey.form();
        return result;
        }

        function ValidateCivilStatus() {
            /// <summary>Validate the civil status</summary>            
            /// <returns> True if is valid. False otherwise. </returns>
            var civilStatusSelectedValue = $("#<%= rdbMaritalStatus.ClientID %> input:checked").val();
            if (civilStatusSelectedValue == null || civilStatusSelectedValue.length === 0 || civilStatusSelectedValue === '') {
                return false
            }
            return true;
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


        $(document).ready(function () {

        });



        function ProcessSaveAsDraftResponse() {
            /// <summary>Process the response for the employee search</summary>

            setTimeout(function () {
                enableButton($('#<%= lbtnStartSurvey.ClientID %>'));

            }, 200);

        }


    </script>
</asp:Content>
