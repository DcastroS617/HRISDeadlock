<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="MinorChildren.aspx.cs" Inherits="HRISWeb.SocialResponsability.MinorChildren"  %>
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
                                        <a class="nav-link active" id="minorchildren-tab" data-toggle="tab" href="#minorchildren" role="tab" aria-controls="minorchildren" aria-selected="true"><%= GetLocalResourceObject("minorchildren-tab") %></a>
                                    </li>
                                </ul>
                                <div class="tab-content" id="survey">
                                    <div class="tab-pane fade in active" id="minorchildren" role="tabpanel" aria-labelledby="minorchildren-tab">
                                        <p>
                                            <br />
                                        </p>
                                        <div class="row">
                                            <div class="col-sm-6">
                                                <asp:Label ID="lblMinorChildren" meta:resourcekey="lblMinorChildren" AssociatedControlID="cboNumberOfMinorChildren" runat="server" Text="" CssClass="control-label text-left"></asp:Label>                                                
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:DropDownList ID="cboNumberOfMinorChildren" runat="server" CssClass="form-control selectpicker" data-live-search="true" AutoPostBack="true" OnSelectedIndexChanged="cboNumberOfMinorChildren_SelectedIndexChanged"></asp:DropDownList>
                                                <label id="cboNumberOfMinorChildrenValidation" for="<%= cboNumberOfMinorChildren.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgNumberOfMinorChildrenValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                            </div>
                                        </div>
                                        <div class="row" style="padding-top:10px">
                                            <div class="col-sm-12">
                                                <div class="">
                                                    <div class="table-responsive">
                                                        <asp:Repeater ID="rptMinorChildren" runat="server" OnItemDataBound="rptMinorChildren_ItemDataBound">
                                                            <HeaderTemplate>
                                                                <table class="table table-bordered table-striped table-hover" style="max-width:none!important; width:auto!important" id="tbMinorChildren">
                                                                    <thead>
                                                                        <tr>
                                                                            <th style="width: 10px!important; min-width:10px!important;"><%= GetLocalResourceObject("rptMinorChildren.Number.Header") %></th>
                                                                            <th style="width: 70px!important; min-width: 70px!important;"><%= GetLocalResourceObject("rptMinorChildren.Relationship.Header") %></th>
                                                                            <th style="width: 75px!important; min-width: 75px!important"><%= GetLocalResourceObject("rptMinorChildren.Gender.Header") %></th>
                                                                            <th style="width: 80px!important; min-width: 80px!important;"><%= GetLocalResourceObject("rptMinorChildren.Age.Header") %></th>
                                                                            <th style="width: 110px!important; min-width: 110px!important;"><%= GetLocalResourceObject("rptMinorChildren.MaritalStatus.Header") %></th>
                                                                            <th style="width: 45px!important; min-width: 45px!important;"><%= GetLocalResourceObject("rptMinorChildren.ReadWrite.Header") %></th>
                                                                            <th style="width: 120px!important; min-width:120px!important;"><%= GetLocalResourceObject("rptMinorChildren.LastAcademicDegreeApproved.Header") %></th>
                                                                            <th style="width: 70px!important; min-width: 70px!important;"><%= GetLocalResourceObject("rptMinorChildren.LastStudyYearApproved.Header") %></th> 
                                                                            <th style="width: 35px!important; min-width: 35px!important;"><%= GetLocalResourceObject("rptMinorChildren.CurrentlyStudying.Header") %></th>
                                                                            <th style="width: 120px!important; min-width:120px!important;"><%= GetLocalResourceObject("rptMinorChildren.CoursingAcademicDegree.Header") %></th>
                                                                            <th style="width: 80px!important; min-width: 80px!important;"><%= GetLocalResourceObject("rptMinorChildren.CoursingYear.Header") %></th>
                                                                            <th style="width: 45px!important; min-width: 45px!important;"><%= GetLocalResourceObject("rptMinorChildren.SocialSecurity.Header") %></th>
                                                                            <th style="width: 50px!important; min-width: 50px!important;"><%= GetLocalResourceObject("rptMinorChildren.Aid.Header") %></th>
                                                                            <th style="width: 45px!important; min-width: 45px!important;"><%= GetLocalResourceObject("rptMinorChildren.Work.Header") %></th>
                                                                            <th style="width: 75px!important; min-width: 75px!important;"><%= GetLocalResourceObject("rptMinorChildren.HelpHouse.Header") %></th>
                                                                            
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
                                                                        <asp:DropDownList ID="cboRelationship" runat="server" CssClass="form-control" AutoPostBack="false"></asp:DropDownList>
                                                                        <label id="cboRelationshipValidation" for="<%# Container.FindControl("cboRelationship").ClientID %>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgRelationshipValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="cboGender" runat="server" CssClass="form-control" AutoPostBack="false"></asp:DropDownList>
                                                                        <label id="cboGenderValidation" for="<%# Container.FindControl("cboGender").ClientID %>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgGenderValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="cboAge" runat="server" CssClass="form-control" AutoPostBack="false"></asp:DropDownList>
                                                                        <label id="cboAgeValidation" for="<%# Container.FindControl("cboAge").ClientID %>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgAgeValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="cboMaritalStatus" runat="server" CssClass="form-control" AutoPostBack="false"></asp:DropDownList>
                                                                        <label id="cboMaritalStatusValidation" for="<%# Container.FindControl("cboMaritalStatus").ClientID %>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgMaritalStatusValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                                    </td>
                                                                    <td>
                                                                        <input id="chkReadWrite" runat="server" type="checkbox" class="checkbox-toggle" />
                                                                    </td>
                                                                     <td>
                                                                        <asp:DropDownList ID="cboLastAcademicAproved" runat="server" CssClass="form-control" AutoPostBack="false"></asp:DropDownList>
                                                                        <label id="cboLAstAcademicAprovedValidation" for="<%# Container.FindControl("cboLastAcademicAproved").ClientID %>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgAcademicDegreeValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                                        <asp:HiddenField ID="hdfLastAcademicDegreeAproved" runat="server" />
                                                                     </td>
                                                                     <td>
                                                                        <asp:DropDownList ID="cboLastStudyYearAproved" runat="server" CssClass="form-control" AutoPostBack="false"></asp:DropDownList>
                                                                        <label id="cboLastStudyYearAprovedValidation" for="<%# Container.FindControl("cboLastStudyYearAproved").ClientID %>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgStudyYearValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                                        <asp:HiddenField ID="hdfLastYearAproved" runat="server" />
                                                                     </td>
                                                                    <td>
                                                                        <input id="chkCurrentlyStudying" runat="server" type="checkbox" class="checkbox-toggle" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="cboAcademicDegree" runat="server" CssClass="form-control" AutoPostBack="false"  disabled="disabled"></asp:DropDownList>
                                                                        <label id="cboAcademicDegreeValidation" for="<%# Container.FindControl("cboAcademicDegree").ClientID %>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgAcademicDegreeValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="cboStudyYear" runat="server" CssClass="form-control" AutoPostBack="false" disabled="disabled"></asp:DropDownList>
                                                                        <label id="cboStudyYearValidation" for="<%# Container.FindControl("cboStudyYear").ClientID %>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgStudyYearValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                                        <asp:HiddenField ID="hdfStudyYear" runat="server" />
                                                                    </td>  
                                                                    <td>
                                                                        <input id="chkSocialSecurity" runat="server" type="checkbox" class="checkbox-toggle" />
                                                                    </td>
                                                                    <td>
                                                                        <input id="chkHelps" runat="server" type="checkbox" class="checkbox-toggle" />
                                                                    </td>
                                                                    <td>
                                                                        <input id="chkWorks" runat="server" type="checkbox" class="checkbox-toggle" />
                                                                    </td>
                                                                    <td>
                                                                        <input id="chkHouseHelpt" runat="server" type="checkbox" class="checkbox-toggle" />
                                                                        <asp:DropDownList ID="cboHouseHelp" runat="server" CssClass="form-control" AutoPostBack="false" disabled="disabled"></asp:DropDownList>
                                                                        <label id="cboHouseHelprValidation" for="<%# Container.FindControl("cboHouseHelp").ClientID %>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgHouseHelpValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                                      
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
                                        <br/> 
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

            $('.checkbox-toggle[id*="chkCurrentlyStudying"]').change(function () {
                var toggleValue = $(this).prop('checked');
                
                $(this).closest('tr').find('td:eq(9) select[id*="cboAcademicDegree"]').prop('disabled', !toggleValue);
                $(this).closest('tr').find('td:eq(9) select[id*="cboAcademicDegree"]')[0].selectedIndex = -1;
                SetControlValid($(this).closest('tr').find('td:eq(9) select[id*="cboAcademicDegree"]')[0].id);

                $(this).closest("td").next().next().find('select[id*="cboStudyYear"]').prop('disabled', !toggleValue);
                $(this).closest('tr').find('td:eq(10) select[id*="cboStudyYear"]')[0].selectedIndex = -1;
                SetControlValid($(this).closest("td").next().next().find('select[id*="cboStudyYear"]')[0].id);
            });

            $('.checkbox-toggle[id*="chkHouseHelpt"]').change(function () {
                var toggleValue = $(this).prop('checked');
                
                $(this).closest('tr').find('td:eq(14) select[id*="cboHouseHelp"]').prop('disabled', !toggleValue);
                $(this).closest('tr').find('td:eq(14) select[id*="cboHouseHelp"]')[0].selectedIndex = -1;
                SetControlValid($(this).closest('tr').find('td:eq(14) select[id*="cboHouseHelp"]')[0].id);
            });


            $('.checkbox-toggle[id*="chkReadWrite"]').change(function () {
                $(this).closest("tr").find('td:eq(6) select[id*="cboLastAcademicAproved"] option[value=-1]').prop("selected", true);
                $(this).closest("tr").find('td:eq(7) select[id*="cboLastStudyYearAproved"] option[value=-1]').prop("selected", true);

                $(this).closest("tr").find('td:eq(9) select[id*="cboAcademicDegree"] option[value=-1]').prop("selected", true);
                $(this).closest("tr").find('td:eq(10) select[id*="cboStudyYear"] option[value=-1]').prop("selected", true);

                $('#' + $(this).closest('tr').find('td:eq(7) select[id*="cboLastStudyYearAproved"]')[0].id).empty();
            });


            $('[name*="cboAge"]').change(function () {
                var selectedAge = Number($(this).val());
                if (selectedAge < 1) {
                    $(this).closest('tr').find('td:eq(6) select[id*="cboLastAcademicAproved"] option[value=-1]').prop("selected", true);
                    $(this).closest('tr').find('td:eq(6) select[id*="cboLastAcademicAproved"]').prop('disabled', true);

                    $(this).closest('tr').find('td:eq(7) select[id*="cboLastStudyYearAproved"] option[value=-1]').prop("selected", true);
                    $(this).closest('tr').find('td:eq(7) select[id*="cboLastStudyYearAproved"]').prop('disabled', true);
                } else {
                    $(this).closest('tr').find('td:eq(6) select[id*="cboLastAcademicAproved"]').prop('disabled', false);
                    $(this).closest('tr').find('td:eq(7) select[id*="cboLastStudyYearAproved"]').prop('disabled', false);
                }
                if (selectedAge < 12) {
                    $(this).closest('tr').find('td:eq(4) select[id*="cboMaritalStatus"] option[value=1]').prop("selected", true);
                }
            });

            $('[name*="cboLastStudyYearAproved"]').change(function () {
                var selectedYear = Number($(this).val());
                var readAndWrite = $(this).closest('tr').find('td:eq(5) input[type=checkbox]').prop('checked');

                var degreeComplete = Number($('#' + $(this).closest('tr').find('td:eq(6) select[id*="cboLastAcademicAproved"]')[0].id).val());
                var listYears = JSON.parse(localStorage.getItem('YearDegrees'));
                var divisionCode = localStorage.getItem('DivisionCode');

                var yearObject = listYears.filter(z => z.AcademicDegreeCode == degreeComplete && z.DivisionCode == divisionCode && z.Coursing == 0 && z.AcademicYear == selectedYear);

                if (readAndWrite) {
                    if (!yearObject[0].ReadAndWrite) {
                        MostrarMensaje(TipoMensaje.ERROR, '<%= GetLocalResourceObject("msjAcademicDegreeInvalidRead").ToString()%>', null);
                        $(this).val("-1");
                    }
                } else {
                    if (yearObject[0].ReadAndWrite) {
                        MostrarMensaje(TipoMensaje.ERROR, '<%= GetLocalResourceObject("msjAcademicDegreeInvalidUnRead").ToString()%>', null);
                        $(this).val("-1");
                    }
                }
                $('#' + $(this).closest('tr').find('td:eq(7)  input[type=Hidden]')[0].id).val(Number($(this).val()));
            });

            $('[name*="cboStudyYear"]').change(function () {
                var divisionCode = localStorage.getItem('DivisionCode');
                var selectedYear = Number($(this).val());
                var yearComplete = Number($('#' + $(this).closest('tr').find('td:eq(7) select[id*="cboLastStudyYearAproved"]')[0].id).val());
                var actualDegree = Number($('#' + $(this).closest('tr').find('td:eq(9) select[id*="cboAcademicDegree"]')[0].id).val());
                var degreeComplete = Number($('#' + $(this).closest('tr').find('td:eq(6) select[id*="cboLastAcademicAproved"]')[0].id).val());
                var listAcademicDegres = JSON.parse(localStorage.getItem('AcademicDegrees'));
                var actualDegreeEntity = listAcademicDegres.filter(x => x.AcademicDegreeCode == actualDegree);
                var lastAcademicDegreeValidation = <%= Convert.ToInt32(ConfigurationManager.AppSettings["LastValidationAcademicDegree"].ToString()) %>;

                var listYears = JSON.parse(localStorage.getItem('YearDegrees'));
                var yearResult = listYears.filter(x => x.AcademicDegreeCode == degreeComplete && x.DivisionCode == divisionCode && x.Coursing == false);

                if (actualDegreeEntity != null) {
                    if (actualDegreeEntity[0].Orderlist < lastAcademicDegreeValidation) {
                        if (actualDegree == degreeComplete) {
                            if (!(yearResult.length === 1 && yearResult[0].AcademicYear === 11)) {
                                if (yearComplete > selectedYear) {
                                    $(this).closest('tr').find('td:eq(10) select[id*="cboStudyYear"] option[value=-1]').prop("selected", true);
                                    MostrarMensaje(TipoMensaje.ERROR, '<%= GetLocalResourceObject("msjActualYearInvalid").ToString()%>', null);
                                }
                            }

                        }
                    }
                }

                $('#' + $(this).closest('tr').find('td:eq(10)  input[type=Hidden]')[0].id).val(Number($(this).val()));
            });

            //Años de estudio acanzados
            $('[name*="cboLastAcademicAproved"]').change(function () {

                var selectedDegree = Number($(this).val());
                var $this = this;
                var listYears = JSON.parse(localStorage.getItem('YearDegrees'));
                var divisionCode = localStorage.getItem('DivisionCode');
                var loadYearFlag = true;

                $(this).closest('tr').find('td:eq(9) select[id*="cboAcademicDegree"] option[value=-1]').prop("selected", true);
                $('#' + $(this).closest('tr').find('td:eq(10) select[id*="cboStudyYear"]')[0].id).empty();            

                if (selectedDegree != -1) {
                    
                    $('#' + $($this).closest('tr').find('td:eq(7) select[id*="cboLastStudyYearAproved"]')[0].id).empty();
                    var yearResult = listYears.filter(x => x.AcademicDegreeCode == selectedDegree && x.DivisionCode == divisionCode && x.Coursing == false);

                    if (yearResult.length > 1) {
                        $('#' + $($this).closest('tr').find('td:eq(7) select[id*="cboLastStudyYearAproved"]')[0].id).append($('<option>', { value: -1, text: "" }));
                    }else{  
                        var readAndWrite = $(this).closest('tr').find('td:eq(5) input[type=checkbox]').prop('checked');
                        var degreeComplete = Number($('#' + $(this).closest('tr').find('td:eq(6) select[id*="cboLastAcademicAproved"]')[0].id).val());
                        var divisionCode = localStorage.getItem('DivisionCode');
                        
                        if (readAndWrite) {
                            if (!yearResult[0].ReadAndWrite) {
                                MostrarMensaje(TipoMensaje.ERROR, '<%= GetLocalResourceObject("msjAcademicDegreeInvalidRead").ToString()%>', null);
                                $(this).val("-1");
                                $('#' + $(this).closest('tr').find('td:eq(7) select[id*="cboLastStudyYearAproved"]')[0].id).empty();   
                                loadYearFlag=false;
                            }
                        } else {
                            if (yearResult[0].ReadAndWrite) {
                                MostrarMensaje(TipoMensaje.ERROR, '<%= GetLocalResourceObject("msjAcademicDegreeInvalidUnRead").ToString()%>', null);
                                $(this).val("-1");
                                $('#' + $(this).closest('tr').find('td:eq(7) select[id*="cboLastStudyYearAproved"]')[0].id).empty(); 
                                loadYearFlag=false; 
                            }                        
                        }
                    }
                    if(loadYearFlag){
                        yearResult.forEach(function (item) {
                            $('#' + $($this).closest('tr').find('td:eq(7) select[id*="cboLastStudyYearAproved"]')[0].id).append($('<option>', { value: item.AcademicYear, text: item.AcademicYear }));
                        });
                    }
                }
            });

            $('[name*="cboAcademicDegree"]').change(function () {
                var selectedDegree = Number($(this).val());
                var degreeComplete = Number($('#' + $(this).closest('tr').find('td:eq(6) select[id*="cboLastAcademicAproved"]')[0].id).val());
                var lastAcademicDegreeValidation = <%= Convert.ToInt32(ConfigurationManager.AppSettings["LastValidationAcademicDegree"].ToString()) %>;
                $('#' + $(this).closest('tr').find('td:eq(10) select[id*="cboStudyYear"]')[0].id).empty();                

                if (selectedDegree != -1 && degreeComplete != -1) {

                    var listAcademicDegres = JSON.parse(localStorage.getItem('AcademicDegrees'));
                    var listYears = JSON.parse(localStorage.getItem('YearDegrees'));
                    var divisionCode = localStorage.getItem('DivisionCode');

                    var actualDegreeEntity = listAcademicDegres.filter(x => x.AcademicDegreeCode == selectedDegree);
                    var degreeCompleteEntity = listAcademicDegres.filter(x => x.AcademicDegreeCode == degreeComplete);

                    if (actualDegreeEntity != null && degreeCompleteEntity != null) {
                        if (degreeCompleteEntity[0].Orderlist > actualDegreeEntity[0].Orderlist) {
                            $(this).closest('tr').find('td:eq(9) select[id*="cboAcademicDegree"] option[value=-1]').prop("selected", true);
                            MostrarMensaje(TipoMensaje.ERROR, '<%= GetLocalResourceObject("msjInvalidDegrees").ToString()%>', null);
                            selectedDegree = -1;
                        }
                        if (actualDegreeEntity[0].Orderlist < lastAcademicDegreeValidation && degreeCompleteEntity[0].Orderlist > actualDegreeEntity[0].Orderlist) {
                            $(this).closest('tr').find('td:eq(9) select[id*="cboAcademicDegree"] option[value=-1]').prop("selected", true);
                            MostrarMensaje(TipoMensaje.ERROR, '<%= GetLocalResourceObject("msjInvalidDegrees").ToString()%>', null);
                            selectedDegree = -1;
                        }

                        if (selectedDegree != -1) {
                            var yearResult = listYears.filter(x => x.AcademicDegreeCode == selectedDegree && x.DivisionCode == divisionCode && x.Coursing == true);
                            var idControl = '#' + $(this).closest('tr').find('td:eq(10) select[id*="cboStudyYear"]')[0].id;
                            
                            if (yearResult.length > 1) {
                                $('#' + $(this).closest('tr').find('td:eq(10) select[id*="cboStudyYear"]')[0].id).append($('<option>', { value: -1, text: "" }));
                            }

                            yearResult.forEach(function (item) {
                                $(idControl).append($('<option>', { value: item.AcademicYear, text: item.AcademicYear }));
                            });
                        }

                        //$(idControl).selectpicker('refresh');
                    }
                } else {
                    if (degreeComplete == -1) {
                        MostrarMensaje(TipoMensaje.ERROR, '<%= GetLocalResourceObject("msjSelectDegreeValid").ToString()%>', null);
                    }
                }
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
                            <%= cboNumberOfMinorChildren.UniqueID %>: {
                                required: true
                                , validSelection: true
                            }
                        }
                    });

                $('[name*="cboRelationship"]').each(function () {
                    $(this).rules('add', {
                        required: true,
                        validSelection: true
                    });
                });

                $('[name*="cboGender"]').each(function () {
                    $(this).rules('add', {
                        required: true,
                        validSelection: true
                    });
                });
                $('[name*="cboAge"]').each(function () {
                    $(this).rules('add', {
                        required: true,
                        validSelection: true
                    });
                });
                $('[name*="cboMaritalStatus"]').each(function () {
                    $(this).rules('add', {
                        required: true,
                        validSelection: true
                    });
                });
                $('[name*="cboStudyYear"]').each(function () {
                    $(this).rules('add', {
                        required: true,
                        validSelection: true
                    });
                });
                $('[name*="cboAcademicDegree"]').each(function () {
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
                $('[name*="cboLastStudyYearAproved"]').each(function () {
                    $(this).rules('add', {
                        required: true,
                        validSelection: true
                    });
                });
                $('[name*="cboLastAcademicAproved"]').each(function () {
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