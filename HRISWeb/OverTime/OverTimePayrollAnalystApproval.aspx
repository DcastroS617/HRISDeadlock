<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="OverTimePayrollAnalystApproval.aspx.cs" Inherits="HRISWeb.Overtime.OverTimePayrollAnalystApproval" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntHeader" runat="server">
    <title><%=Convert.ToString(GetLocalResourceObject("lblScreenTitle")) %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cntBody" runat="server">
    <div class="main-content">
        <h1 class="text-left text-primary">
            <label class="PageTitle"><%= GetLocalResourceObject("lblScreenTitle") %></label>
        </h1>
        <br />
        <asp:UpdatePanel ID="uppMain" runat="server">
            <Triggers>
            </Triggers>
            <ContentTemplate>
                <div class="container" style="width: 100%">
                    <div class="row">
                        <div class="col-sm-12">
                            <h4 class="text-left text-primary"><%= GetLocalResourceObject("lblFiltersSubtitle") %></h4>
                            <button id="btnCleanFilters" type="button" runat="server" class="btn btn-default btnAjaxAction" onserverclick="btnCleanFilters_Click" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnClearFilters"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnClearFilters"))%>'>
                                <span class="glyphicon glyphicon-erase glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%= GetLocalResourceObject("btnClearFilters") %>
                            </button>
                        </div>
                    </div>

                    <br />
                    <div class="row">

                        <div class="col-sm-4">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-5 text-left">
                                        <label for="<%=cboDateType.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblDateType")%></label>
                                    </div>
                                    <div class="col-sm-7">
                                        <asp:DropDownList ID="cboDateType" CssClass="form-control control-validation" data-live-search="true" AutoPostBack="false" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-5 text-left">
                                        <label for="<%=dtpOvertimeDateFrom.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblSuspendedStartFilter")%></label>
                                    </div>
                                    <div class="col-sm-7">
                                        <asp:TextBox runat="server" ID="dtpOvertimeDateFrom" class="form-control date control-validation cleanPasteDigits" placeholder='<%$ Code:GetLocalResourceObject("lblDateFormatPlaceHolder") %>' type="text" autocomplete="off" />

                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-5 text-left">
                                        <label for="<%=dtpOvertimeDateto.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblSuspendedEndFilter")%></label>
                                    </div>
                                    <div class="col-sm-7">
                                        <asp:TextBox runat="server" ID="dtpOvertimeDateto" class="form-control date control-validation cleanPasteDigits" placeholder='<%$ Code:GetLocalResourceObject("lblDateFormatPlaceHolder") %>' type="text" autocomplete="off" />

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-sm-4" runat="server" id="divEmloyeeCode">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-5 text-left">
                                        <label for="<%=txtEmloyeeCode.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblEmployeeCode")%></label>
                                    </div>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtEmloyeeCode" CssClass="form-control cleanPasteText EnterKey" runat="server" autocomplete="off" MaxLength="250" type="text"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-5 text-left">
                                        <label for="<%=cboOvertimeStatu.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblOvertimeStatus")%></label>
                                    </div>
                                    <div class="col-sm-7">
                                        <asp:DropDownList ID="cboOvertimeStatu" CssClass="form-control control-validation" data-live-search="true" AutoPostBack="false" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-5 text-left">
                                        <label for="<%=txtovetimeNumber.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblOvertimeNumber")%></label>
                                    </div>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtovetimeNumber" CssClass="form-control control-validation  cleanPasteText EnterKey" runat="server" autocomplete="off" onkeypress="return isNumber(event);" MaxLength="10" type="number" min="0" oninput="maxLengthCheck(this)"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">

                            <div class="col-sm-4" style="display: none">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=txtStartHoursFrom.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblStartHourfrom")%></label>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtStartHoursFrom" CssClass="form-control control-validation cleanPasteDigits " runat="server" autocomplete="off" MaxLength="10" type="number" min="0"></asp:TextBox>

                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-4" style="display: none">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=txtStartHoursTo.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblTo")%></label>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtStartHoursTo" CssClass="form-control control-validation cleanPasteDigits " runat="server" autocomplete="off" MaxLength="10" type="number" min="0"></asp:TextBox>

                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>

                        <div class="col-sm-4" style="display: none">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-5 text-left">
                                        <label for="<%=txtEndHoursFrom.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblEndHoursFrom")%></label>
                                    </div>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtEndHoursFrom" CssClass="form-control control-validation  " runat="server" autocomplete="off" MaxLength="10" type="number" min="0"></asp:TextBox>

                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4" style="display: none">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-5 text-left">
                                        <label for="<%=txtEndHoursTo.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblTo")%></label>
                                    </div>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtEndHoursTo" CssClass="form-control control-validation " runat="server" autocomplete="off" MaxLength="10" type="number" min="0"></asp:TextBox>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 text-right">
                            <button id="btnSearchFilter" type="button" runat="server" class="btn btn-default btnAjaxAction" onserverclick="btnSearchFilter_ServerClick" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>'>
                                <span class="glyphicon glyphicon-search glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%= GetLocalResourceObject("btnSearch") %>
                            </button>
                        </div>
                    </div>
                    <br />
                    <div class="row" style="display: none;">
                        <div class="col-sm-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-5 text-left">
                                    </div>
                                    <div class="col-sm-7">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row" style="display: none;">
                        <div class="col-sm-12 text-center">
                            <button id="btnSearch" type="button" runat="server" class="btn btn-default btnAjaxAction" onserverclick="btnSearch_ServerClick" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>'>
                                <span class="glyphicon glyphicon-search glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                                &nbsp;<%= GetLocalResourceObject("btnSearch") %>
                            </button>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-sm-12 text-right">
                            <button id="btnExportExcel" type="button" runat="server" class="btn btn-default " onserverclick="btnExportExcel_Click"  data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnExportExcel"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnExportExcel"))%>'>
                                <span class="glyphicon glyphicon-export glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                                &nbsp;<%= GetLocalResourceObject("btnExportExcel") %>
                            </button>
                        </div>
                        <div class="col-sm-12 text-center">
                            <asp:HiddenField runat="server" ID="hdfSelectedRowIndex" Value="-1" />
                            <div>
                                <asp:GridView ID="grvList"
                                    Width="100%" runat="server" EmptyDataText='<%$ Code:GetLocalResourceObject("lblNoData") %>'
                                    EmptyDataRowStyle-CssClass="emptyRow"
                                    AllowPaging="true" PagerSettings-Visible="false" AllowSorting="true" AutoGenerateColumns="false" ShowHeader="true"
                                    CssClass="table table-striped table-hover table-bordered" DataKeyNames="OverTimeNumber"
                                    OnPreRender="grvList_PreRender" OnSorting="grvList_Sorting">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" HeaderStyle-VerticalAlign="Middle">
                                            <HeaderTemplate>
                                                <div class="gridHeader">
                                                    <asp:LinkButton ID="lbtnOverTimeNumberSort" runat="server" CommandName="Sort" CommandArgument="OverTimeNumberCode">                
                                                        <span><%= GetLocalResourceObject("htOvertimeNumber") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvOvertimeNumber" data-id="OverTimeNumber" data-value="<%# Eval("OvertimeNumber") %>"><%# Eval("OvertimeNumber") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div class="gridHeader">
                                                    <asp:LinkButton ID="lbtnOvertimeDateSort" runat="server" CommandName="Sort" CommandArgument="OvertimeDate">                
                                                        <span><%= GetLocalResourceObject("htOvertimeDate") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvOvertimeDate" data-id="OvertimeDate" data-value="<%# Eval("OvertimeDateFormatted") %>"><%# Eval("OvertimeDateFormatted") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" HeaderStyle-VerticalAlign="Middle">
                                            <HeaderTemplate>
                                                <div class="gridHeader">
                                                    <asp:LinkButton ID="lbtnCompanySort" runat="server" CommandName="Sort" CommandArgument="Company">                
                                                        <span><%= GetLocalResourceObject("htCompany") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvCompany" data-id="Company" data-value="<%# Eval("Company") %>"><%# Eval("Company") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" Visible="false" HeaderStyle-VerticalAlign="Middle">
                                            <HeaderTemplate>
                                                <div class="gridHeader">
                                                    <asp:LinkButton ID="lbtnGeographicDivisionCoderSort" runat="server" CommandName="Sort" CommandArgument="GeographicDivisionCode">                
                                                        <span><%= GetLocalResourceObject("htGeographicDivisionCode") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvGeographicDivisionCode" data-id="GeographicDivisionCode" data-value="<%# Eval("GeographicDivisionCode") %>"><%# Eval("GeographicDivisionCode") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" Visible="false" HeaderStyle-VerticalAlign="Middle">
                                            <HeaderTemplate>
                                                <div class="gridHeader">
                                                    <asp:LinkButton ID="lbtnDivisionCodeSort" runat="server" CommandName="Sort" CommandArgument="DivisionCode">                
                                                        <span><%= GetLocalResourceObject("htDivisionCode") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvDivisionCode" data-id="DivisionCode" data-value="<%# Eval("DivisionCode") %>"><%# Eval("DivisionCode") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" HeaderStyle-VerticalAlign="Middle">
                                            <HeaderTemplate>
                                                <div class="gridHeader">
                                                    <asp:LinkButton ID="lbtnEmployeeCodeSort" runat="server" CommandName="Sort" CommandArgument="Employee">                
                                                        <span><%= GetLocalResourceObject("htEmployee") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvEmployeeCode" data-id="EmployeeCode" data-value="<%# Eval("Employee") %>"><%# Eval("Employee") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" HeaderStyle-VerticalAlign="Middle">
                                            <HeaderTemplate>
                                                <div class="gridHeader">
                                                    <asp:LinkButton ID="lbtnStartHourSort" runat="server" CommandName="Sort" CommandArgument="StartHour">                
                                                        <span><%= GetLocalResourceObject("htStartHour") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvStartHour" data-id="StartHour" data-value="<%# Eval("StartHour") %>"><%# Eval("StartHour") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" HeaderStyle-VerticalAlign="Middle">
                                            <HeaderTemplate>
                                                <div class="gridHeader">
                                                    <asp:LinkButton ID="lbtnEndHourSort" runat="server" CommandName="Sort" CommandArgument="EndHour">                
                                                        <span><%= GetLocalResourceObject("htEndHour") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvEndHour" data-id="EndHour" data-value="<%# Eval("EndHour") %>"><%# Eval("EndHour") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" HeaderStyle-VerticalAlign="Middle">
                                            <HeaderTemplate>
                                                <div class="gridHeader">
                                                    <asp:LinkButton ID="lbtnTotalOvertimeHoursSort" runat="server" CommandName="Sort" CommandArgument="TotalOvertimeHours">                
                                                        <span><%= GetLocalResourceObject("htTotalOvertimeHours") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvTotalOvertimeHours" data-id="TotalOvertimeHours" data-value="<%# Eval("TotalOvertimeHours") %>"><%# Eval("TotalOvertimeHours") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" HeaderStyle-VerticalAlign="Middle">
                                            <HeaderTemplate>
                                                <div class="gridHeader">
                                                    <asp:LinkButton ID="lbtnJustificationForExtraTimeSort" runat="server" CommandName="Sort" CommandArgument="JustificationForExtraTime">                
                                                        <span><%= GetLocalResourceObject("htJustification") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvJustificationForExtraTime" data-id="JustificationForExtraTime" data-value="<%# Eval("JustificationForExtraTime") %>"><%# Eval("JustificationForExtraTime") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                            <HeaderTemplate>
                                                <div class="gridHeader">
                                                    <asp:LinkButton ID="lbtnJourneyTypeSort" runat="server" CommandName="Sort" CommandArgument="WorkingTimeTypeDescription">                
                                                        <span><%= GetLocalResourceObject("htJourneyType") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvJourneyType" data-id="WorkingTimeTypeDescription" data-value="<%# Eval("WorkingTimeTypeDescription") %>"><%# Eval("WorkingTimeTypeDescription") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" HeaderStyle-VerticalAlign="Middle">
                                            <HeaderTemplate>
                                                <div class="gridHeader">
                                                    <asp:LinkButton ID="lbtnOvertimeCreatedDateSort" runat="server" CommandName="Sort" CommandArgument="OvertimeCreatedDate">                
                                                        <span><%= GetLocalResourceObject("htOvertimeCreatedDate") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvOvertimeCreatedDate" data-id="OvertimeCreatedDate" data-value="<%# Eval("OvertimeCreatedDateFormatted") %>"><%# Eval("OvertimeCreatedDateFormatted") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" HeaderStyle-VerticalAlign="Middle">
                                            <HeaderTemplate>
                                                <div class="gridHeader">
                                                    <asp:LinkButton ID="lbtnOverTimeClassificationSort" runat="server" CommandName="Sort" CommandArgument="OverTimeClassification">                
                                                        <span><%= GetLocalResourceObject("htOvertimeClassification") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvOvertimeClassification" data-id="OvertimeClassificationName" data-value="<%# Eval("OvertimeClassificationName") %>"><%# Eval("OvertimeClassificationName") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" HeaderStyle-VerticalAlign="Middle">
                                            <HeaderTemplate>
                                                <div class="gridHeader">
                                                    <asp:LinkButton ID="lbtnOverTimeStatusCodeSort" runat="server" CommandName="Sort" CommandArgument="OverTimeStatus">                
                                                        <span><%= GetLocalResourceObject("htOvertimeStatusCode") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvOverTimeStatusCode" data-id="OverTimeStatusCode" data-value="<%# Eval("OverTimeStatus") %>"><%# Eval("OverTimeStatus") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" Visible="false" HeaderStyle-VerticalAlign="Middle">
                                            <HeaderTemplate>
                                                <div class="gridHeader">
                                                    <asp:LinkButton ID="lbtnIsExtraHourSort" runat="server" CommandName="Sort" CommandArgument="IsExtraHour">                
                                                        <span><%= GetLocalResourceObject("htIsExtraHour") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvIsExtraHour" data-id="IsExtraHour" aria-hidden="true" data-value="<%# Eval("IsExtraHour") %>" class="fa <%# ((DOLE.HRIS.Shared.Entity.OverTimeRecordsEntity)Container.DataItem).IsExtraHour ? "fa-check-circle" : "fa-times-circle" %> "></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <div id="grvListWaiting" style="position: absolute; margin: auto; display: none; bottom: 0px; top: 0px; left: 50%; transform: translate(-50%, 35%);">
                                    <span class='fa fa-spinner fa-spin' style="font-size: 50px;"></span>
                                </div>
                            </div>
                            <h4 class="text-left text-primary" id="htmlResultsSubtitle" runat="server"></h4>
                            <br />
                            <nav>
                                <asp:BulletedList ID="blstPager" runat="server" CssClass="pagination" DisplayMode="LinkButton" OnClick="blstPager_Click">
                                </asp:BulletedList>
                            </nav>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        
    </div>
    <nav class="navbar-fixed-bottom">
        <div class="container center-block text-center">
            <b>
                <div class="alert alert-autocloseable-msg" style="display: none;">
                </div>
            </b>
        </div>
    </nav>
    <script src="<%=ResolveUrl("~/Scripts/Excel/xlsx.full.min.js") %>" type="text/javascript"></script>
    <script type="text/javascript">     

        //Variables for table ordening
        var dataSortAttribute, dataSortType, dataSortDirection;
        $(document).ready(function () {


            $(document).on('keyup keypress', '.EnterKey', function (e) {

                var keyCode = e.keyCode || e.which;
                if (keyCode === 13) {
                    e.preventDefault();
                    return false;
                }
            });

        });

        function maxLengthCheck(object) {
            if (object.value.length > object.maxLength)
                object.value = object.value.slice(0, object.maxLength)
        }

        var ReturnDeleteSucess;
        function pageLoad(sender, args) {
            /// <summary>Execute at load even at partial and ajax requests</summary>
            /// We prefer to bind the events here over do it on document ready event in order to auto rebind the events in page and ajax request each time
            $('#<%= dtpOvertimeDateFrom.ClientID %>').datetimepicker({
                format: 'MM/DD/YYYY',

            });

            $('#<%= dtpOvertimeDateto.ClientID %>').datetimepicker({
                format: 'MM/DD/YYYY'
            });

            


            ReturnDeleteSucess = function () {
                UnselectRow();
                ShowFooterSuccess('<%= GetLocalResourceObject("msgOperationSuccesfull") %>');
            }


            function UnselectRow() {
                /// <summary>Unselect rows</summary>  
                $('#<%=hdfSelectedRowIndex.ClientID%>').val("-1");
                $('#<%= grvList.ClientID %> tbody tr').removeClass('info');
            }

           


            //In this section we set the button state for btnAjaxAction
            $('.btnAjaxAction').on('click', function () {
                var $this = $(this);
                $this.button('loading');
                setTimeout(function () { $this.button('reset'); }, 30000);
            });

            //And the grvList selection row functionality
            $('#<%= grvList.ClientID %>').on('click', 'tbody tr', function (event) {

                if (!$(this).hasClass('emptyRow')) {
                    $(this).toggleClass('info').siblings().removeClass('info');
                    $('#<%=hdfSelectedRowIndex.ClientID%>').val($(this).hasClass("info") ? $(this).index() : -1);
                    console.log($('#<%=hdfSelectedRowIndex.ClientID%>').val());
                }
            });

            //And the grvList pager event for client side
            $('#<%= blstPager.ClientID %>').on('click', 'a', function (event) {
                SetWaitingGrvList();
            });

            //others buttons 
            

            $('#btnCancel').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                event.preventDefault();
                if (validator != null) {
                    validator.destroy();
                }

                DisableButtonsDialog();
                $('#MaintenanceDialog').modal('hide');
                EnableButtonsDialog();
            });

            $('#<%= btnSearch.ClientID %>').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                event.preventDefault();

                DisableButtonsDialog();
                $('#MaintenanceDialog').modal('hide');
                $('#searchmodal').modal('show');
                EnableButtonsDialog();
            });

            $("#btnCancelAddDivision").on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                event.preventDefault();
                DisableButtonsDialog();
                $('#adddetailmodal').modal('hide');
                $('#MaintenanceDialog').modal('show');
                EnableButtonsDialog();
            });

            $("#btnCancelSearchUser").on('click', function (event) {
                /// <summary>Handles the click event for button btnCancelSearchUser in user dialog.</summary>            
                event.preventDefault();
                DisableButtonsDialog();
                $('#searchmodal').modal('hide');
                $('#MaintenanceDialog').modal('show');
                EnableButtonsDialog();
            });

            $('#MaintenanceDialog').on("shown.bs.modal", function () {
                /// <summary>Handles the shown event for MaintenanceDialog dialog.</summary>
                if ($('#MaintenanceDialog').data('windowmode') === 'edit') {
                    disableButton($("#<%=btnSearch.ClientID%>"));
                }
                else {
                    enableButton($("#<%=btnSearch.ClientID%>"));
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

            //In this section we initialize the checkbox toogles
            //In this section we initialize the popovers and tooltips for the elements.
            //We have both (popovers and tooltips) in order to accomplish information alert over desktop 
            //and mobiles devices
            InitializeTooltipsPopovers();
        }

        var validator = null;

        var validatorSearchUser = null;

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

        function IsRowSelected() {
            /// <summary>Validate if there is a selected row</summary>  
            var selectedRowIndex = $('#<%=hdfSelectedRowIndex.ClientID%>').val();
            if (!isEmptyOrSpaces(selectedRowIndex) && selectedRowIndex != "-1") {
                return true;
            }
            return false;
        }

        function IsRowCheked(gridId) {
            /// <summary>Validate if there is a cheked row </summary>
            /// <param name="gridId" type="String">Id of the control</param>
            var checks = gridId.find('input:checkbox[id*="chkSelected"]:checked:not(#chkSelectedAll)');
            return checks.length > 0 ? true : false;
        }

        function IsOnlyOneRowCheked(gridId) {
            /// <summary>Validate if there is a cheked row </summary>
            /// <param name="gridId" type="String">Id of the control</param>
            var checks = gridId.find('input:checkbox[id*="chkSelected"]:checked:not(#chkSelectedAll)');
            return checks.length === 1 ? true : false;
        }

        function SetWaitingGrvList() {
            /// <summary>Process the request of set the grid as waiting style</summary>

            setTimeout(function () {
                $('#<%= grvList.ClientID %>').find("input,button,textarea,select").attr("disabled", "disabled");
                $('#<%= grvList.ClientID %>').find("a").removeAttr('href');
                $('#<%= blstPager.ClientID %>').find("a").removeAttr('href');

                $('#grvListWaiting').fadeIn('fast');
                $('#<%= grvList.ClientID %>').fadeTo('fast', 0.5);

            }, 100);
        }

        function ProcessEditRequest(resetId) {
            /// <summary>Process the edit request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>

            // set the mode of the window
            $('#MaintenanceDialog').data('windowmode', 'edit');
            $(".btnAccept").prop("disabled", false);
            $('.btnAccept').removeAttr('disabled');
            disableButton($("#btnSearchUser"));
            
        }


        function ProcessDeleteRequest(resetId) {
            /// <summary>Process the delete request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>

            if ($('#MaintenanceDialog').data('windowmode') === 'edit') {
                disableButton($("#<%=btnSearch.ClientID%>"));
            }
            else {
                enableButton($("#<%=btnSearch.ClientID%>"));
            }

        }


        function CloseUserDialog() {
            /// <summary>Close a user dialog.</summary>
            $('#MaintenanceDialog').modal('hide');
        }

        

        function DisableButtonsDialog() {
            ///<summary>Disables all buttons toolbar</summary>
            disableButton($('#btnAccept'));
            disableButton($('#btnCancel'));
        }

        function EnableButtonsDialog() {
            ///<summary>Enabled all buttons toolbar</summary>
            enableButton($('#btnAccept'));
            enableButton($('#btnCancel'));
        }

        function ReturnFromBtnEditClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>            
            DisableToolBar();
            $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#dialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#MaintenanceDialog').modal('show');
            EnableToolBar();
        }

        function ReturnFromBtnAcceptClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>      
            $(".btnAccept").prop("disabled", true);
            DisableButtonsDialog();


            // $('.btnAccept').removeAttr('disabled');

            $('#MaintenanceDialog').modal('hide');
            $(".btnAccept").prop("disabled", false);

            EnableButtonsDialog();

            ShowFooterSuccess('<%= GetLocalResourceObject("msgOperationSuccesfull") %>');
        }

        function ReturnFromBtnExportClickPostBack(jsonData) {
            var cursos = [];
            filename = '<%= GetLocalResourceObject("ExcelName") %>.xlsx';

            jsonData.forEach(row => {
                var cursoTemp = new Object({
                    "<%= GetLocalResourceObject("htOvertimeNumber") %>": row.OvertimeNumber,
                    "<%= GetLocalResourceObject("htOvertimeDate") %>": row.OvertimeDate,
                    "<%= GetLocalResourceObject("htCompany") %>": row.Company,
                    "<%= GetLocalResourceObject("htEmployee") %>": "$ " + row.Employee,
                    "<%= GetLocalResourceObject("htStartHour") %>": row.StarHour,
                    "<%= GetLocalResourceObject("htEndHour") %>": row.EndHour,
                    "<%= GetLocalResourceObject("htTotalOvertimeHours") %>": row.TotalHours,
                    "<%= GetLocalResourceObject("htJustification") %>": row.Justification,
                    "<%= GetLocalResourceObject("htOvertimeClassification") %>": row.OvertimeClass,
                    "<%= GetLocalResourceObject("htOvertimeCreatedDate") %>": row.CreatedDate,
                    "<%= GetLocalResourceObject("htOvertimeStatusCode") %>": row.Status
                    
                });

                cursos.push(cursoTemp);
            });

            const header = Object.keys(jsonData[0]); // columns name

            var wscols = [];
            for (var i = 0; i < header.length; i++) {  // columns length added
                wscols.push({ wch: header[i].length + 13 })
            }

            var workSheet = XLSX.utils.json_to_sheet(cursos);
            workSheet['!autofilter'] = { ref: `A1:L${jsonData.length + 1}` }
            workSheet["!cols"] = wscols;

            var workBook = XLSX.utils.book_new();
            XLSX.utils.book_append_sheet(workBook, workSheet, "<%= GetLocalResourceObject("ExcelName") %>");
            XLSX.writeFile(workBook, filename);
        }

        function ReturnFromAcceptAddNewDivisionRequest() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            DisableButtonsDialog();
            $('#adddetailmodal').modal('hide');
            //$('#MaintenanceDialog').modal('show');
            EnableButtonsDialog();
        }

        //In this section we set the control for multiple simultaneous ajax request
        //We cancel the ajax request executed when another one is in progress
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(initializeRequest);
        function initializeRequest(sender, args) {

            if (prm.get_isInAsyncPostBack()) {
                args.set_cancel(true);
                <%--ShowFooterAlert('<%=GetLocalResourceObject("msgWaitWhileProcessing")%>');--%>
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
