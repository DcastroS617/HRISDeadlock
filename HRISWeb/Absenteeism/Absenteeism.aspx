<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Absenteeism.aspx.cs" Inherits="HRISWeb.Absenteeism.Absenteeism" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntHeader" runat="server">
    <title><%=Convert.ToString(GetLocalResourceObject("lblScreenTitle")) %></title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cntBody" runat="server">
    <style type="text/css">
        .treeNode {
            transition: all .3s;
            padding: 2px;
            text-align: center;
            margin: 0;
            text-decoration: none !important;
            color: black;
        }

        .rootNode {
            font-weight: bold;
        }

        .leafNode {
            font-weight: normal;
        }

        .treeNode a {
            padding-left: 5px;
            padding-top: 5px;
            padding-bottom: 5px;
        }

        .treeNode input {
            -ms-transform: scale(1.5); /* IE */
            -moz-transform: scale(1.5); /* FF */
            -webkit-transform: scale(1.5); /* Safari and Chrome */
            -o-transform: scale(1.5); /* Opera */
            transform: scale(1.5);
        }
    </style>

    <div class="main-content">
        <h1 class="text-left text-primary">
            <label class="PageTitle"><%= GetLocalResourceObject("lblScreenTitle") %></label>
        </h1>
        <br />

        <asp:UpdatePanel runat="server" ID="main" UpdateMode="Conditional">
            <Triggers>
            </Triggers>
            <ContentTemplate>
                <asp:HiddenField ID="searchSelected" runat="server" />

                <asp:Panel ID="pnlMainContent" runat="server">
                    <div class="container" style="width: 100%">

                        <h4 class="text-left text-primary"><%= GetLocalResourceObject("lblFiltersSubtitle") %></h4>
                        <div style="margin-top: -100px" class="pull-right">
                            <button id="btnPrevToPage0" type="button" runat="server" class="btn btn-default btnAjaxAction" disabled="disabled"
                                data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnPrevious"))%>'
                                data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnPrevious"))%>'>
                                <span class="glyphicon glyphicon-chevron-left glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%= GetLocalResourceObject("btnPrevious") %>
                            </button>
                            &nbsp;
                            <button id="btnNextToPage2" type="button" runat="server" class="btn btn-default btnAjaxAction"
                                onclick="ActivateModalProgress(true);" onserverclick="BtnNextToPage2_Click"
                                data-loading-text='<%$ Code:String.Concat(GetLocalResourceObject("btnNext"), "&nbsp;<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>")%>'
                                data-error-text='<%$ Code:String.Concat( GetLocalResourceObject("btnNext"), "&nbsp;<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>")%>'>
                                <%= GetLocalResourceObject("btnNext") %>&nbsp;<span class="glyphicon glyphicon-chevron-right glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            </button>
                        </div>
                        <br />

                        <div id="Tabs" role="tabpanel">
                            <!-- Nav tabs -->
                            <ul class="nav nav-tabs">
                                <li class="active">
                                    <button id="btnDirectSearch" runat="server" class="btn btn-default btnAjaxAction" onserverclick="BtnDirectSearch_ServerClick"><%= GetLocalResourceObject("pnlDirectSearch") %></button></li>
                                <li>
                                    <button id="btnAdvancedSearch" runat="server" class="btn btn-default btnAjaxAction" onserverclick="BtnAdvancedSearch_ServerClick"><%= GetLocalResourceObject("pnlAdvancedSearch") %></button></li>
                            </ul>

                            <!-- Tab panes -->
                            <asp:Panel ID="pnlDirectSearch" runat="server">
                                <br />
                                <div class="row">
                                    <div class="col-sm-5">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <div class="col-sm-5 text-left">
                                                    <label for="<%=txtEmployeeIdFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblEmployeeIdFilter")%></label>
                                                </div>

                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtEmployeeIdFilter" CssClass="form-control control-validation cleanPasteText EnterKeyPressSearch" onkeypress="return isNumber(event);" runat="server" autocomplete="off" MaxLength="10"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-sm-5">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <div class="col-sm-5 text-left">
                                                    <label for="<%=txtEmployeeNameFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblEmployeeNameFilter")%></label>
                                                </div>

                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtEmployeeNameFilter" CssClass="form-control control-validation cleanPasteText EnterKeyPressSearch" onkeypress="return isNumberOrLetterTFL(event);"  runat="server" autocomplete="off" MaxLength="250" ></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-sm-2">
                                        <div class="col-sm-12 text-center">
                                            <button id="btnSearchFilter" type="button" runat="server" class="btn btn-default btnAjaxAction"
                                                onserverclick="BtnSearchFilter_ServerClick"
                                                data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>'
                                                data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>'>
                                                <span class="glyphicon glyphicon-search glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%= GetLocalResourceObject("btnSearch") %>
                                            </button>
                                        </div>
                                    </div>
                                    <br />
                                    <br />

                                    <asp:HiddenField ID="hdfEmployeeSelectedDirect" runat="server" />                                    
                                    <asp:GridView ID="grvEmployeesByFilter" Width="100%" runat="server"
                                        EmptyDataRowStyle-CssClass="emptyRow" AllowPaging="false" PagerSettings-Visible="false"
                                        AutoGenerateColumns="false" ShowHeader="true" CssClass="table table-striped table-hover table-bordered"
                                        DataKeyNames="EmployeeCode" OnPreRender="GrvEmployeesByFilter_PreRender">
                                        <Columns>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdfUniqueKey" runat="server" Value='<%#Eval("UniqueKey") %>' />
                                                    <asp:CheckBox ID="chbEmployeeByFilter" runat="server" Checked='<%# Eval("IsSelected") %>' onclick="reviewOnlyOneCheck('ctl00_cntBody_grvEmployeesByFilter', this)" CssClass="employeeDirectSelected" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="EmployeeCodeSort" Enabled="false" runat="server" CommandName="Sort" CommandArgument="EmployeeCode">
                                                            <span><%= GetLocalResourceObject("lblEmployeeCodeHeader") %></span>&nbsp;
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvEmployeeCode" data-id="EmployeeCode" data-value="<%# Eval("EmployeeCode") %>"><%# Eval("EmployeeCode")%></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="EmployeeNameSort" Enabled="false" runat="server" CommandName="Sort" CommandArgument="EmployeeName">          
                                                            <span><%= GetLocalResourceObject("lblEmployeeNameHeader") %></span>&nbsp;
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvEmployeeName" data-id="EmployeeName" data-value="<%# Eval("EmployeeName") %>"><%# Eval("EmployeeName")%></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                <HeaderTemplate>
                                                    <div style="width: 100%; text-align: center;">
                                                        <asp:LinkButton ID="CompanyPayrollClassSort" Enabled="false" runat="server" CommandName="Sort" CommandArgument="Company-PayrollClass">          
                                                            <span><%= GetLocalResourceObject("lblEmployeeCompanyPayrollHeader") %></span>&nbsp;
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <span id="dvCompanyPayrollClass" data-id="Company-PayrollClass" data-value="<%# Eval("Company-PayrollClass") %>"><%# Eval("Company-PayrollClass")%></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>

                                    <div id="grvEmployeesByFilterListWaiting" style="position: absolute; margin: auto; display: none; bottom: 0px; top: 0px; left: 50%; transform: translate(-50%, 35%);">
                                        <span class='fa fa-spinner fa-spin' style="font-size: 50px;"></span>
                                    </div>
                                </div>
                            </asp:Panel>

                            <asp:Panel ID="pnlAdvancedSearch" runat="server" Visible="false">
                                <br />
                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <div class="col-sm-3 text-left">
                                                    <label for="<%=cboPayroll.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblPayroll")%></label>
                                                </div>

                                                <div class="col-sm-9">
                                                    <asp:DropDownList ID="cboPayroll" CssClass="form-control cboAjaxAction control-validation ignoreValidation" AutoPostBack="true" runat="server" OnSelectedIndexChanged="CboPayroll_SelectedIndexChanged"></asp:DropDownList>

                                                    <span id="cboPayrollWaiting" class='fa fa-spinner fa-spin waitingNotification' style="display: none; float: right; margin-right: 25px; margin-top: -23px; position: relative; z-index: 2;"></span>

                                                    <asp:HiddenField ID="hdfPayrollValue" runat="server" />
                                                    <asp:HiddenField ID="hdfPayrollText" runat="server" />
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <div class="col-sm-3 text-left">
                                                    <label for="<%=cboGroupType.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblGroupType")%></label>
                                                </div>

                                                <div class="col-sm-9">
                                                    <asp:DropDownList ID="cboGroupType" CssClass="form-control cboAjaxAction control-validation ignoreValidation" AutoPostBack="true" runat="server" OnSelectedIndexChanged="CboGroupType_SelectedIndexChanged"></asp:DropDownList>

                                                    <span id="cboGroupTypeWaiting" class='fa fa-spinner fa-spin waitingNotification' style="display: none; float: right; margin-right: 25px; margin-top: -23px; position: relative; z-index: 2;"></span>

                                                    <asp:HiddenField ID="hdfGroupTypeValue" runat="server" />
                                                    <asp:HiddenField ID="hdfGroupTypeText" runat="server" />
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <div class="col-sm-3 text-left">
                                                    <label for="<%=cboGroup.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblGroup")%></label>
                                                </div>

                                                <div class="col-sm-9">
                                                    <asp:DropDownList ID="cboGroup" CssClass="form-control cboAjaxAction control-validation ignoreValidation" AutoPostBack="true" runat="server" OnSelectedIndexChanged="CboGroup_SelectedIndexChanged"></asp:DropDownList>

                                                    <span id="cboGroupWaiting" class='fa fa-spinner fa-spin waitingNotification' style="display: none; float: right; margin-right: 25px; margin-top: -23px; position: relative; z-index: 2;"></span>

                                                    <asp:HiddenField ID="hdfGroupValue" runat="server" />
                                                    <asp:HiddenField ID="hdfGroupText" runat="server" />
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <div class="col-sm-3 text-left">
                                                    <label for="<%=chkMultiCause.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblMultiCause")%></label>
                                                </div>

                                                <div class="col-sm-9">
                                                    <asp:CheckBox ID="chkMultiCause" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <br />

                                <div class="row">
                                    <div class="col-sm-12 text-center">
                                        <div class="col-sm-3">
                                            <h4 class="text-left text-primary"><%= GetLocalResourceObject("lblGroupDataSubtitle") %></h4>

                                            <div>
                                                <asp:TreeView ID="trvGroupData" runat="server" ShowCheckBoxes="Leaf" OnTreeNodeCheckChanged="TrvGroupData_TreeNodeCheckChanged"
                                                    NodeStyle-CssClass="treeNode" RootNodeStyle-CssClass="rootNode" LeafNodeStyle-CssClass="leafNode" SelectedNodeStyle-CssClass="selectNode" ExpandDepth="1" Font-Size="Small">
                                                </asp:TreeView>

                                                <div id="trvGroupDataWaiting" style="position: absolute; margin: auto; display: none; bottom: 0px; top: 0px; left: 50%; transform: translate(-50%, 35%);">
                                                    <span class='fa fa-spinner fa-spin' style="font-size: 50px;"></span>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-sm-9" id="uppSearchEmployeesParent">
                                            <h4 class="text-left text-primary"><%= GetLocalResourceObject("lblWorkersSubtitle") %></h4>
                                            <br />

                                            <input class="form-control cleanPasteText EnterKeypress" id="txtSearchEmployees" name="txtSearchEmployees" type="text" placeholder='<%= GetLocalResourceObject("lblSearchEmployeesPlaceHolder") %>' autocomplete="off" onkeypress="return isNumberOrLetter(event)" />
                                            <span id="txtSearchEmployeesWaiting" class='fa fa-spinner fa-spin' style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;"></span>
                                            <br />

                                            <asp:UpdatePanel runat="server" ID="uppSearchEmployees" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                                <Triggers>
                                                </Triggers>

                                                <ContentTemplate>
                                                    <div class="col-sm-12 text-left">
                                                        <label id="lblSearchEmployeesResults" runat="server" class="control-label"></label>
                                                    </div>

                                                    <asp:HiddenField ID="hdfEmployeesSelected" runat="server" />
                                                    <asp:HiddenField ID="hdfSelectAll" runat="server" />
                                                    <asp:GridView ID="grvEmployees" Width="100%" runat="server"
                                                        EmptyDataRowStyle-CssClass="emptyRow" AllowPaging="false" PagerSettings-Visible="false"
                                                        AutoGenerateColumns="false" ShowHeader="true" CssClass="table table-striped table-hover table-bordered"
                                                        DataKeyNames="EmployeeCode" OnPreRender="GrvEmployees_PreRender" OnDataBound="GrvEmployees_DataBound"
                                                        AllowSorting="true" OnSorting="GrvEmployees_Sorting">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="">
                                                                <HeaderTemplate>
                                                                    <div style="width: 100%; text-align: center;">
                                                                        <input type="checkbox" id="selectAll" />
                                                                    </div>
                                                                </HeaderTemplate>

                                                                <ItemTemplate>
                                                                    <asp:HiddenField ID="hdfUniqueKey" runat="server" Value='<%#Eval("UniqueKey") %>' />
                                                                    <asp:CheckBox ID="chbEmployee" runat="server" Checked='<%# Eval("IsSelected") %>' CssClass="employeeSelected" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                                <HeaderTemplate>
                                                                    <div style="width: 100%; text-align: center;">
                                                                        <asp:LinkButton ID="EmployeeCodeSort" runat="server" CommandName="Sort" CommandArgument="EmployeeCode">                                                                        
                                                                            <span><%= GetLocalResourceObject("lblEmployeeCodeHeader") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvEmployees.ClientID, "EmployeeCode") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>                                                                       
                                                                        </asp:LinkButton>
                                                                    </div>
                                                                </HeaderTemplate>

                                                                <ItemTemplate>
                                                                    <span id="dvEmployeeCode" data-id="EmployeeCode" data-value="<%# Eval("EmployeeCode") %>"><%# Eval("EmployeeCode")%></span>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                                <HeaderTemplate>
                                                                    <div style="width: 100%; text-align: center;">
                                                                        <asp:LinkButton ID="EmployeeNameSort" runat="server" CommandName="Sort" CommandArgument="EmployeeName">          
                                                                            <span><%= GetLocalResourceObject("lblEmployeeNameHeader") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvEmployees.ClientID, "EmployeeName") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                                        </asp:LinkButton>
                                                                    </div>
                                                                </HeaderTemplate>

                                                                <ItemTemplate>
                                                                    <span id="dvEmployeeName" data-id="EmployeeName" data-value="<%# Eval("EmployeeName") %>"><%# Eval("EmployeeName")%></span>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                                <HeaderTemplate>
                                                                    <div style="width: 100%; text-align: center;">
                                                                        <asp:LinkButton ID="CompanyPayrollClassSort" runat="server" CommandName="Sort" CommandArgument="Company-PayrollClass">          
                                                                            <span><%= GetLocalResourceObject("lblEmployeeCompanyPayrollHeader") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvEmployees.ClientID, "Company-PayrollClass") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                                        </asp:LinkButton>
                                                                    </div>
                                                                </HeaderTemplate>

                                                                <ItemTemplate>
                                                                    <span id="dvCompanyPayrollClass" data-id="Company-PayrollClass" data-value="<%# Eval("Company-PayrollClass") %>"><%# Eval("Company-PayrollClass")%></span>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                                <HeaderTemplate>
                                                                    <div style="width: 100%; text-align: center;">
                                                                        <asp:LinkButton ID="GroupDataDescriptionSort" runat="server" CommandName="Sort" CommandArgument="GroupDataDescription">          
                                                                            <span><%= GetLocalResourceObject("lblEmployeeGroupHeader") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvEmployees.ClientID, "GroupDataDescription") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                                        </asp:LinkButton>
                                                                    </div>
                                                                </HeaderTemplate>

                                                                <ItemTemplate>
                                                                    <span id="dvGroupDataDescription" data-id="GroupDataDescription" data-value="<%# Eval("GroupDataDescription") %>"><%# Eval("GroupDataDescription")%></span>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>

                                                    <div id="grvListWaiting" style="position: absolute; margin: auto; display: none; bottom: 0px; top: 0px; left: 50%; transform: translate(-50%, 35%);">
                                                        <span class='fa fa-spinner fa-spin' style="font-size: 50px;"></span>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div id="trvSearchEmployeesWaiting" style="position: absolute; margin: auto; display: none; bottom: 0px; top: 0px; left: 50%; transform: translate(-50%, 35%);">
                                            <span class='fa fa-spinner fa-spin' style="font-size: 50px;"></span>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                </asp:Panel>

                <asp:Panel ID="pnlAbsenteeismContent" runat="server" Visible="false">
                    <div class="container" style="width: 100%">
                        <div style="margin-top: -60px" class="pull-right">
                            <button id="btnPrevToPage1" type="button" runat="server" class="btn btn-default btnAjaxAction"
                                onserverclick="BtnPrevToPage1_Click"
                                data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnPrevious"))%>'
                                data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnPrevious"))%>'>
                                <span class="glyphicon glyphicon-chevron-left glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%= GetLocalResourceObject("btnPrevious") %>
                            </button>
                            &nbsp;
                            <button id="btnNextToPage3" type="button" runat="server" class="btn btn-default btnAjaxAction"
                                onclick="if(!ProcessValidateAbsenteeismContent(this.id)){return false;}" onserverclick="BtnNextToPage3_ServerClick"
                                data-loading-text='<%$ Code:String.Concat(GetLocalResourceObject("btnNext"), "&nbsp;<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>")%>'
                                data-error-text='<%$ Code:String.Concat( GetLocalResourceObject("btnNext"), "&nbsp;<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>")%>'>
                                <%= GetLocalResourceObject("btnNext") %>&nbsp;<span class="glyphicon glyphicon-chevron-right glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            </button>
                        </div>

                        <asp:Panel ID="pnlNormalTfl" runat="server" Visible="true">
                            <div class="row">
                                <div class="col-sm-4" style="height: 600px; overflow-y: auto">
                                    <h4 class="text-left text-primary"><%= GetLocalResourceObject("lblWorkersSubtitle") %>
                                        <asp:Label ID="lblCount" runat="server"></asp:Label></h4>
                                    <br />

                                    <asp:UpdatePanel runat="server" ID="uppEmployeesSelected" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                        <Triggers>
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:HiddenField ID="hdfParticipantsSelected" runat="server" />

                                            <asp:GridView ID="grvEmployeesSelected" Width="100%" runat="server" EmptyDataText='<%$ Code:GetLocalResourceObject("lblNoData") %>'
                                                EmptyDataRowStyle-CssClass="emptyRow" AllowPaging="false" PagerSettings-Visible="false"
                                                AutoGenerateColumns="false" ShowHeader="true" CssClass="table table-striped table-hover table-borderless"
                                                DataKeyNames="EmployeeCode" AllowSorting="true" OnPreRender="GrvEmployeesSelected_PreRender" OnDataBound="GrvEmployeesSelected_DataBound"
                                                OnSorting="GrvEmployeesSelected_Sorting" BorderStyle="None" GridLines="None">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="">
                                                        <HeaderTemplate>
                                                        </HeaderTemplate>

                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chbParticipantSelected" runat="server" OnCheckedChanged="ChkEmployeesSelected_CheckedChanged" AutoPostBack="true" Checked='<%# Eval("IsSelected") %>' CssClass="participantSelected" />
                                                            <asp:HiddenField ID="hdfUniqueKey" runat="server" Value='<%# Eval("UniqueKey") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                        <HeaderTemplate>
                                                            <div style="width: 100%; text-align: center;">
                                                                <asp:LinkButton ID="EmployeeCodeSort" runat="server" CommandName="Sort" CommandArgument="EmployeeCode">
                                                                    <span><%= GetLocalResourceObject("lblEmployeeCodeHeader") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvEmployeesSelected.ClientID, "EmployeeCode") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </HeaderTemplate>

                                                        <ItemTemplate>
                                                            <span id="EmployeeCode" data-id="EmployeeCode" data-value="<%# Eval("EmployeeCode") %>"><%# Eval("EmployeeCode")%></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                        <HeaderTemplate>
                                                            <div style="width: 100%; text-align: center;">
                                                                <asp:LinkButton ID="EmployeeNameSort" runat="server" CommandName="Sort" CommandArgument="EmployeeName">
                                                                    <span><%= GetLocalResourceObject("lblEmployeeNameHeader") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvEmployeesSelected.ClientID, "EmployeeName") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </HeaderTemplate>

                                                        <ItemTemplate>
                                                            <span id="EmployeeName" data-id="EmployeeName" data-value="<%# Eval("EmployeeName") %>"><%# Eval("EmployeeName")%></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                        <HeaderTemplate>
                                                            <div style="width: 100%; text-align: center;">
                                                                <asp:LinkButton ID="CompanyCodeSort" runat="server" CommandName="Sort" CommandArgument="CompanyCode">
                                                                    <span><%= GetLocalResourceObject("lblEmployeeCompanyHeader") %> </span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvEmployeesSelected.ClientID, "CompanyCode") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </HeaderTemplate>

                                                        <ItemTemplate>
                                                            <span id="CompanyCode" data-id="CompanyCode" data-value="<%# Eval("CompanyCode") %>"><%# Eval("CompanyCode")%></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>

                                <div class="col-sm-8">
                                    <h4 class="text-left text-primary"><%= GetLocalResourceObject("lblAbsenteeismSubtitle") %></h4>
                                    <br />

                                    <div class="row">
                                        <div class="col-sm-6">
                                            <div class="form-horizontal">
                                                <div class="form-group">
                                                    <div class="col-sm-4 text-left">
                                                        <label for="<%=cboCause.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCause")%></label>
                                                    </div>

                                                    <div class="col-sm-8">
                                                        <asp:DropDownList ID="cboCause" CssClass="form-control cboAjaxAction control-validation cboCause" AutoPostBack="true" runat="server" OnSelectedIndexChanged="CboCause_SelectedIndexChanged"></asp:DropDownList>

                                                        <label id="cboCauseValidation" for="<%= cboCause.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjCauseValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>

                                                        <asp:HiddenField ID="hdfCauseValue" runat="server" />

                                                        <asp:HiddenField ID="hdfCauseText" runat="server" />
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <div class="col-sm-4 text-left">
                                                        <label for="<%=cboInterestGroups.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblInterestGroups")%></label>
                                                    </div>

                                                    <div class="col-sm-8">
                                                        <asp:DropDownList ID="cboInterestGroups" CssClass="form-control control-validation cboInterestGroups" AutoPostBack="false" runat="server"></asp:DropDownList>

                                                        <span id="cboInterestGroupsWaiting" class='fa fa-spinner fa-spin waitingNotification' style="display: none; float: right; margin-right: 25px; margin-top: -23px; position: relative; z-index: 2;"></span>

                                                        <asp:HiddenField ID="hdfInterestGroupsValue" runat="server" />

                                                        <asp:HiddenField ID="hdfInterestGroupsText" runat="server" />

                                                        <label id="cboInterestGroupsValidation" for="<%= cboInterestGroups.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjInterestGroupValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                    </div>
                                                </div>

                                                <div class="form-group" id="divAccidentCase" runat="server">
                                                    <div class="col-sm-4 text-left">
                                                        <asp:Label ID="lblDescriptionLabel" runat="server" CssClass="control-label" Font-Bold="true"><%=GetLocalResourceObject("lblDescription")%></asp:Label>
                                                    </div>

                                                    <div class="col-sm-8">
                                                        <asp:TextBox ID="txtDescription" CssClass="form-control cleanPasteText control-validation txtDescription" runat="server" onkeypress="return isNumberOrLetter(event) && checkMaxLength(this,event,15);" onkeyUp="return isNumberOrLetter(event) && checkMaxLength(this,event,15);" MaxLength="15"></asp:TextBox>

                                                        <label id="txtDescriptionValidation" for="<%= txtDescription.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjDescriptionValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>

                                                        <asp:HiddenField ID="hdfDescriptionValue" runat="server" />
                                                    </div>
                                                </div>
                                                <!-- Incident datetime -->
                                            </div>
                                        </div>

                                        <div class="col-sm-6">
                                            <div class="form-horizontal">
                                                <div class="form-group" id="divState" runat="server">
                                                    <div class="col-sm-4 text-left">
                                                        <label for="<%=cboCaseState.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCaseState")%></label>
                                                    </div>

                                                    <div class="col-sm-8">
                                                        <asp:DropDownList ID="cboCaseState" CssClass="form-control cboAjaxAction control-validation" AutoPostBack="false" runat="server"></asp:DropDownList>

                                                        <label id="cboCaseStateValidation" for="<%= cboCaseState.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjCaseStateValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>

                                                        <asp:HiddenField ID="hdfCaseStateValue" runat="server" />

                                                        <asp:HiddenField ID="hdfCaseStateText" runat="server" />
                                                    </div>
                                                </div>

                                                <div class="form-group" id="divRelated" runat="server">
                                                    <div class="col-sm-4 text-left">
                                                        <label for="<%=txtRelatedCase.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblRelatedCase")%></label>
                                                    </div>

                                                    <div class="col-sm-8">
                                                        <asp:TextBox ID="txtRelatedCase" runat="server" CssClass="form-control cleanPasteText control-validation" onkeypress="return isNumberOrLetter(event) && checkMaxLength(this,event,50);" onkeyUp="return isNumberOrLetter(event) && checkMaxLength(this,event,50);" MaxLength="50" ReadOnly="true"></asp:TextBox>

                                                        <label id="cboRelatedCaseValidation" for="<%= txtRelatedCase.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjRelatedCaseValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>

                                                        <asp:HiddenField ID="hdfRelatedCaseValue" runat="server" />

                                                        <asp:HiddenField ID="hdfRelatedCaseText" runat="server" />

                                                        <button id="btnSearchRelatedCase" type="button" runat="server" class="btn btn-default btnAjaxAction" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>'>
                                                            <span class="glyphicon glyphicon-search glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%= GetLocalResourceObject("btnSearch") %>
                                                        </button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-sm-12">
                                            <div class="form-horizontal">
                                                <div class="form-group">
                                                    <div class="col-sm-2 text-left">
                                                        <label for="<%=txtDetailedDescription.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblDetailedDescription")%></label>
                                                    </div>

                                                    <div class="col-sm-10">
                                                        <asp:TextBox ID="txtDetailedDescription" CssClass="form-control cleanPasteText control-validation" runat="server" onkeypress="return isNumberOrLetter(event) && checkMaxLength(this,event,2000);" onkeyUp="return isNumberOrLetter(event) && checkMaxLength(this,event,2000);" MaxLength="2000" TextMode="MultiLine" Rows="6"></asp:TextBox>

                                                        <label id="txtDetailedDescriptionValidation" for="<%= txtDetailedDescription.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjDetailedDescriptionValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>

                                                        <asp:HiddenField ID="hdfDetailedDescriptionValue" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <br />

                                    <h4 class="text-left text-primary"><%= GetLocalResourceObject("lblAbsenteeismTimeSubtitle") %></h4>
                                    <br />

                                    <div class="row">
                                        <div class="col-sm-6">
                                            <div class="form-horizontal">
                                                <div id="divHourStartDate" style="display: none; text-align: center;">
                                                    <div class="form-group">
                                                        <div class="col-sm-4 text-left">
                                                            <label for="<%=dtpIncidentDate.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblIncidentDate")%></label>
                                                        </div>

                                                        <div class="col-sm-8 input-group" style="padding-left: 15px; padding-right: 15px;">
                                                            <asp:TextBox runat="server" ID="dtpIncidentDate" class="form-control date control-validation cleanPasteDigits" placeholder='<%$ Code:GetLocalResourceObject("lblDateFormatPlaceHolder") %>' type="text" autocomplete="off" Style="width: 50%;" />

                                                            <asp:TextBox runat="server" ID="tpcIncidentDateTime" class="form-control control-validation cleanPasteDigits" placeholder='<%$ Code:GetLocalResourceObject("lblTimeFormatPlaceHolder") %>' type="time" autocomplete="off" Style="width: 50%;" />

                                                            <label id="dtpIncidentDateValidation" for="<%= dtpIncidentDate.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjIncidentDateValidation") %>" style="display: none; float: right; margin-right: 55%; margin-top: -23px; position: relative; z-index: 2;">!</label>

                                                            <label id="tpcIncidentDateValidation" for="<%= tpcIncidentDateTime.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjIncidentTimeValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>

                                                            <div class="input-group-addon">
                                                                <span class="glyphicon glyphicon-time" aria-hidden="true"></span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <div class="col-sm-4 text-left">
                                                        <label for="<%=dtpSuspendWorkDate.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblSuspendWork")%></label>
                                                    </div>

                                                    <div class="col-sm-8 input-group" style="padding-left: 15px; padding-right: 15px;">
                                                        <asp:HiddenField ID="hdfEmployeeDirectSuspendTime" runat="server" />

                                                        <asp:TextBox runat="server" ID="dtpSuspendWorkDate" class="form-control date control-validation cleanPasteDigits" placeholder='<%$ Code:GetLocalResourceObject("lblDateFormatPlaceHolder") %>' type="text" autocomplete="off" Style="width: 50%;" />

                                                        <asp:TextBox runat="server" ID="tpcSuspendWorkTime" class="form-control control-validation cleanPasteDigits" placeholder='<%$ Code:GetLocalResourceObject("lblTimeFormatPlaceHolder") %>' type="time" autocomplete="off" Style="width: 50%;" />

                                                        <label id="dtpSuspendWorkDateValidation" for="<%= dtpSuspendWorkDate.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjSuspendWorkValidation") %>" style="display: none; float: right; margin-right: 55%; margin-top: -23px; position: relative; z-index: 2;">!</label>

                                                        <label id="tpcSuspendWorkTimeValidation" for="<%= tpcSuspendWorkTime.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjSuspendWorkTimeValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>

                                                        <div class="input-group-addon">
                                                            <span class="glyphicon glyphicon-time" aria-hidden="true"></span>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <div class="col-sm-4 text-left">
                                                        <label for="<%=dtpFinalDate.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblFinalDate")%></label>
                                                    </div>

                                                    <div class="col-sm-8 input-group" style="padding-left: 15px; padding-right: 15px;">
                                                        <asp:TextBox runat="server" ID="dtpFinalDate" class="form-control date control-validation cleanPasteDigits" placeholder='<%$ Code:GetLocalResourceObject("lblDateFormatPlaceHolder") %>' type="text" autocomplete="off" />

                                                        <label id="dtpFinalDateValidation" for="<%= dtpFinalDate.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjFinalDateValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>

                                                        <div class="input-group-addon">
                                                            <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="form-group" id="divCloseDate" runat="server">
                                                    <div class="col-sm-4 text-left">
                                                        <label for="<%= chkCloseDate.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCloseDate")%></label>
                                                    </div>

                                                    <div class="col-sm-8">
                                                        <asp:CheckBox ID="chkCloseDate" runat="server" data-toggle="toggle" onclick="if(!ProcessCloseDate(this.id)){return false;}" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                                    </div>
                                                </div>

                                                <div class="form-group" runat="server">
                                                    <div class="col-sm-4 text-left">
                                                        &nbsp;
                                                    </div>

                                                    <div class="col-sm-8">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-sm-6">
                                            <div class="form-horizontal">
                                                <div class="form-group">
                                                    <div class="col-sm-4 text-left">
                                                        <label for="<%=cboTimeUnit.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblTimeUnit")%></label>
                                                    </div>

                                                    <div class="col-sm-8">
                                                        <asp:DropDownList ID="cboTimeUnit" CssClass="form-control cboAjaxAction control-validation ignoreValidation" AutoPostBack="false" runat="server"></asp:DropDownList>
                                                        
                                                        <label id="cboTimeUnitValidation" for="<%= cboTimeUnit.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjTimeUnitValidation") %>" style="display: none; float: right; margin-right: 26px; margin-top: -23px; position: relative; z-index: 2;">!</label>

                                                        <asp:HiddenField ID="hdfTimeUnitValue" runat="server" />

                                                        <asp:HiddenField ID="hdfTimeUnitText" runat="server" />
                                                    </div>
                                                </div>

                                                <div class="form-group" id="NaturalDaysData">
                                                    <div class="col-sm-4 text-left">
                                                        <label for="<%=txtNaturalDays.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblNaturalDays")%></label>
                                                    </div>

                                                    <div class="col-sm-8">
                                                        <asp:TextBox ID="txtNaturalDays" runat="server" CssClass="form-control cleanPasteDigits" onkeypress="return isNumber(event) && checkMaxLength(this,event,6) && checkMaxValue(this, event, 10000) && checkMinValue(this, event, 1);" type="number" min="1" max="10000" autocomplete="off" Style="width: 100%"></asp:TextBox>

                                                        <label id="txtNaturalDaysValidation" for="<%= txtNaturalDays.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjNaturalDaysValidation") %>" style="display: none; float: right; margin-right: 26px; margin-top: -20px; position: relative; z-index: 2;">!</label>

                                                        <asp:HiddenField ID="hdfNaturalDaysValue" runat="server" />
                                                    </div>
                                                </div>

                                                <div class="form-group" id="HoursData">
                                                    <div class="col-sm-4 text-left">
                                                        <label for="<%=txtFinalHours.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblFinalHours")%></label>
                                                    </div>

                                                    <div class="col-sm-8">
                                                        <asp:TextBox ID="txtFinalHours" runat="server" CssClass="input-group cleanPasteDigits" onkeypress="return isDecimalNumber(event) && checkMaxLength(this,event,5) && !checkAlreadyDecimalIfSeparator(this, event);" type="number" min="0.5" max="8" autocomplete="off" Style="width: 100%"></asp:TextBox>

                                                        <label id="txtFinalHoursValidation" for="<%= txtFinalHours.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjFinalHoursValidation") %>" style="display: none; float: right; margin-right: 26px; margin-top: -20px; position: relative; z-index: 2;">!</label>

                                                        <asp:HiddenField ID="hdfFinalHoursValue" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                </div>
                            </div>
                        </asp:Panel>

                        <asp:Panel ID="pnlMultipleTFL" runat="server">
                            <asp:Panel ID="pnlNewRecord" runat="server" Visible="false">
                                <div class="btn-group" role="group" aria-label="main-buttons">
                                    <button id="btnReturnMultipleTfl" type="button" runat="server" class="btn btn-default btnAjaxAction" 
                                        onserverclick="BtnReturnMultipleTfl_ServerClick" 
                                        data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnReturnToMain"))%>' 
                                        data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnReturnToMain"))%>'>
                                        <span class="glyphicon glyphicon-ok glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                                        <br />
                                        <%= GetLocalResourceObject("btnReturnToMain") %>
                                    </button>
                                </div>
                            </asp:Panel>

                            <div class="col-lg-12">
                                <asp:UpdatePanel ID="updMultiplTfl" runat="server">
                                    <Triggers>
                                    </Triggers>
                                    <ContentTemplate>
                                        <asp:Repeater ID="rptMultipleTfl" runat="server" OnItemDataBound="RptMultipleTfl_ItemDataBound" OnItemCreated="RptMultipleTfl_ItemCreated">
                                            <HeaderTemplate>
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <asp:Panel runat="server" ID="pnlMessage" Visible="false">
                                                    <asp:Label runat="server" ID="lblMessage"></asp:Label>
                                                </asp:Panel>

                                                <table class="table table-bordered table-striped">
                                                    <tr>
                                                        <asp:HiddenField runat="server" ID="hdnCompanyMultipleTfl" Value='<%# Eval("CompanyCode") %>' />
                                                        <asp:HiddenField runat="server" ID="hdnEmployeeCode" Value='<%# Eval("EmployeeCode") %>' />

                                                        <td>
                                                            <asp:CheckBox runat="server" ID="chkIsSelected" Checked="true" />
                                                        </td>

                                                        <td style="width: 10%;">
                                                            <%# Eval("EmployeeName") %>
                                                        </td>

                                                        <td style="width: 10%;">
                                                            <label class="control-label"><%=GetLocalResourceObject("lblCause")%></label>
                                                            <asp:DropDownList ID="cboCauseMultipleTfl" CssClass="form-control cboAjaxAction control-validation cboCauseMultipleTfl" AutoPostBack="true" OnSelectedIndexChanged="CboCauseMultipleTfl_SelectedIndexChanged" runat="server"></asp:DropDownList>
                                                        </td>

                                                        <td style="width: 11%;">
                                                            <label class="control-label"><%=GetLocalResourceObject("lblInterestGroups")%></label>
                                                            <asp:DropDownList ID="cboInterestGroupsMultipleTfl" CssClass="form-control control-validation cboInterestGroupsMultipleTfl" AutoPostBack="false" runat="server"></asp:DropDownList>
                                                        </td>

                                                        <td style="width: 20%;">
                                                            <label class="control-label"><%=GetLocalResourceObject("lblDetailedDescription")%></label>
                                                            <asp:TextBox ID="txtDescriptionMultipleTfl" CssClass="form-control cleanPasteText control-validation txtDescriptionMultipleTfl" runat="server" onkeypress="return isNumberOrLetter(event) && checkMaxLength(this,event,2000);" onkeyUp="return isNumberOrLetter(event) && checkMaxLength(this,event,2000);" MaxLength="2000"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 12%;">
                                                            <label class="control-label"><%=GetLocalResourceObject("lblTimeUnit")%></label>
                                                            <asp:DropDownList ID="cboTimeUnitMultipleTfl" CssClass="form-control cboAjaxAction control-validation ignoreValidation cboTimeUnitMultipleTfl" AutoPostBack="true" OnSelectedIndexChanged="CboTimeUnitMultipleTfl_SelectedIndexChanged" runat="server"></asp:DropDownList>
                                                        </td>

                                                        <td style="width: 15%;">
                                                            <label class="control-label"><%=GetLocalResourceObject("lblSuspendWork")%></label>

                                                            <div class="col-sm-12 input-group">
                                                                <asp:TextBox runat="server" ID="dtpSuspendWorkDateMultipleTfl" class="form-control date control-validation cleanPasteDigits dtpSuspendWorkDateMultipleTfl" placeholder='<%$ Code:GetLocalResourceObject("lblDateFormatPlaceHolder") %>' type="text" autocomplete="off" Style="width: 98px;" />
                                                                <asp:TextBox runat="server" ID="tpcSuspendWorkTimeMultipleTfl" class="form-control control-validation cleanPasteDigits tpcSuspendWorkTimeMultipleTfl" placeholder='<%$ Code:GetLocalResourceObject("lblTimeFormatPlaceHolder") %>' type="time" autocomplete="off" ReadOnly="true" Text="07:00" Style="padding: 0px!important; width: 99px; margin: 0px!important; max-width: 100px;" />
                                                            </div>
                                                        </td>

                                                        <td style="width: 13%;">
                                                            <label class="control-label"><%=GetLocalResourceObject("lblFinalDate")%></label>
                                                            <div class="col-sm-12 input-group">
                                                                <asp:TextBox runat="server" ID="dtpFinalDateMultipleTfl" class="form-control date control-validation cleanPasteDigits dtpFinalDateMultipleTfl" placeholder='<%$ Code:GetLocalResourceObject("lblDateFormatPlaceHolder") %>' type="text" autocomplete="off" />

                                                                <div class="input-group-addon">
                                                                    <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                                                </div>
                                                            </div>
                                                        </td>

                                                        <td style="width: 9% !important; padding-left: 5px; padding-right: 5px;">
                                                            <asp:Panel ID="pnlNaturalDaysDataMultipleTfl" runat="server">
                                                                <label class="control-label"><%=GetLocalResourceObject("lblNaturalDays")%></label>

                                                                <asp:TextBox ID="txtNaturalDaysMultipleTfl" runat="server" CssClass="form-control cleanPasteDigits" onkeypress="return isNumber(event) && checkMaxLength(this,event,6) && checkMaxValue(this, event, 10000) && checkMinValue(this, event, 1);" type="number" min="1" max="10000" autocomplete="off" Style="width: 100%" disabled="disabled"></asp:TextBox>
                                                            </asp:Panel>

                                                            <asp:Panel ID="pnlHoursDataMultipleTfl" runat="server" Visible="false">
                                                                <label class="control-label"><%=GetLocalResourceObject("lblFinalHours")%></label>

                                                                <asp:TextBox ID="txtFinalHoursMultipleTfl" runat="server" CssClass="form-control cleanPasteDigits" onkeypress="return isDecimalNumber(event) && checkMaxLength(this, event, 5) && !checkAlreadyDecimalIfSeparator(this, event)" type="number" min="0.5" max="8" autocomplete="off" Style="width: 100%"></asp:TextBox>

                                                                <asp:Label ID="txtFinalHoursMultipleTflValidation" runat="server" AssociatedControlID="txtFinalHoursMultipleTfl" CssClass="label label-danger label-validation" ToolTip='<%$ Code:GetLocalResourceObject("msjFinalHoursValidation") %>' Style="display: none; float: right; margin-right: 27px; margin-top: -24px; position: relative; z-index: 4;">!</asp:Label>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <hr />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </asp:Panel>

                        <%--Modal de búsqueda Caso Relacionado--%>
                        <div class="modal fade" id="RelatedCaseDialog" tabindex="-1" role="dialog" aria-labelledby="RelatedCaseDialogTitle" data-backdrop="static" data-keyboard="false">
                            <div class="modal-dialog" role="document">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" id="btnClose" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                        <h3 class="modal-title text-primary" id="dialogTitle"></h3>
                                    </div>

                                    <div class="modal-body">
                                        <div class="row">
                                            <div class="col-sm-12 text-center">
                                                <asp:Repeater ID="rptRelatedCase" runat="server">
                                                    <HeaderTemplate>
                                                        <table id="tableRelatedCase" class="table table-hover table-striped">
                                                            <thead>
                                                                <tr>
                                                                    <th>
                                                                        <div>
                                                                            <div class="col-xs-1 col-sm-1 text-primary">&nbsp;</div>
                                                                            <div class="col-xs-2 col-sm-2 text-primary " style="cursor: pointer; text-align: center;"><%= GetLocalResourceObject("lblEmployeeNameHeader") %></div>
                                                                            <div class="col-xs-3 col-sm-3 text-primary " style="cursor: pointer; text-align: center;"><%= GetLocalResourceObject("lblAdamCaseNumberHeader") %></div>
                                                                            <div class="col-xs-3 col-sm-3 text-primary " style="cursor: pointer; text-align: center;"><%= GetLocalResourceObject("lblAccidentCaseHeader") %></div>
                                                                            <div class="col-xs-3 col-sm-3 text-primary " style="cursor: pointer; text-align: center;"><%= GetLocalResourceObject("lblStartDateHeader") %></div>
                                                                        </div>
                                                                    </th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                    </HeaderTemplate>

                                                    <ItemTemplate>
                                                        <tr>
                                                            <td class="row-fluid">
                                                                <asp:UpdatePanel runat="server" ID="uppEmployee" UpdateMode="Conditional">
                                                                    <Triggers>
                                                                    </Triggers>

                                                                    <ContentTemplate>
                                                                        <div class="data-sort-src" data-sort-employeename='<%# Eval("EmployeeName") %>' data-sort-absteeismid='<%# Eval("AbsteeismId") %>' data-sort-startdate='<%# Eval("StartDate") %>'>
                                                                            <div id="divSelectRelatedCase" runat="server" class="col-xs-1 col-sm-1 text-center">
                                                                                <asp:HiddenField ID="hdfUniqueKeyRelated" runat="server" Value='<%#Eval("AbsteeismId") %>' />
                                                                                <asp:CheckBox ID="chbEmployeeRelatedCase" runat="server" Checked='<%# Eval("IsSelected") %>' onclick="reviewOnlyOneCheck('tableRelatedCase', this)" />
                                                                            </div>
                                                                            <div class="col-xs-2 col-sm-2">
                                                                                <span>
                                                                                    <%# Eval("EmployeeName") %>
                                                                                </span>
                                                                            </div>
                                                                            <div class="col-xs-3 col-sm-3">
                                                                                <span>
                                                                                    <%# Eval("AbsteeismId") %>
                                                                                </span>
                                                                            </div>
                                                                            <div class="col-xs-3 col-sm-3" style="word-wrap: break-word;">
                                                                                <span>
                                                                                    <%# Eval("Description") %>
                                                                                </span>
                                                                            </div>
                                                                            <div class="col-xs-3 col-sm-3">
                                                                                <span>
                                                                                    <%# Eval("StartDate") %>
                                                                                </span>
                                                                            </div>
                                                                        </div>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
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

                                    <div class="modal-footer">
                                        <button id="btnAcceptRelatedCase" type="button" runat="server" class="btn btn-primary btnAjaxAction"
                                            onserverclick="BtnAcceptRelatedCase_ServerClick"
                                            data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSelect"))%>'
                                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSelect"))%>'>
                                            <span class="glyphicon glyphicon-ok glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnDialogSelect")) %>
                                        </button>
                                        <button id="btnCancel" type="button" class="btn btn-default">
                                            <span class="glyphicon glyphicon-remove glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnDialogCancel")) %>
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%-- Fin Modal de búsqueda Caso Relacionado--%>
                </asp:Panel>

                <asp:Panel ID="pnlDetailsContent" runat="server" Visible="false">
                    <div class="container" style="width: 100%">
                        <h4 class="text-left text-primary"><%= GetLocalResourceObject("lblDetailsSubtitle") %></h4>

                        <div style="margin-top: -100px" class="pull-right">
                            <button id="btnPrevToPage2" type="button" runat="server" class="btn btn-default btnAjaxAction"
                                onserverclick="BtnPrevToPage2_ServerClick"
                                data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnPrevious"))%>'
                                data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnPrevious"))%>'>
                                <span class="glyphicon glyphicon-chevron-left glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%= GetLocalResourceObject("btnPrevious") %>
                            </button>
                            &nbsp;
                            <button id="btnNextToPage4" type="button" runat="server" class="btn btn-default btnAjaxAction"
                                onserverclick="BtnNextToPage4_ServerClick" data-loading-text='<%$ Code:String.Concat(GetLocalResourceObject("btnNext"), "&nbsp;<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>")%>'
                                data-error-text='<%$ Code:String.Concat( GetLocalResourceObject("btnNext"), "&nbsp;<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>")%>'>
                                <%= GetLocalResourceObject("btnNext") %>&nbsp;<span class="glyphicon glyphicon-chevron-right glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            </button>
                        </div>

                        <br />

                        <div class="row">
                            <div class="col-sm-6">
                                <div class="form-horizontal">

                                    <div class="form-group">
                                        <div class="col-sm-3 text-left">
                                            <label id="lblUnsafeActs" runat="server" for="<%=cboUnsafeActs.ClientID%>" class="control-label"></label>
                                        </div>

                                        <div class="col-sm-9">
                                            <asp:DropDownList ID="cboUnsafeActs" CssClass="form-control control-validation ignoreValidation" AutoPostBack="false" runat="server"></asp:DropDownList>

                                            <span id="cboUnsafeActsWaiting" class='fa fa-spinner fa-spin waitingNotification' style="display: none; float: right; margin-right: 25px; margin-top: -23px; position: relative; z-index: 2;"></span>

                                            <asp:HiddenField ID="hdfUnsafeActsValue" runat="server" />

                                            <asp:HiddenField ID="hdfUnsafeActsText" runat="server" />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-3 text-left">
                                            <label for="<%=cboMaterialAgents.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblMaterialAgents")%></label>
                                        </div>

                                        <div class="col-sm-9">
                                            <asp:DropDownList ID="cboMaterialAgents" CssClass="form-control control-validation ignoreValidation" AutoPostBack="false" runat="server"></asp:DropDownList>

                                            <span id="cboMaterialAgentsWaiting" class='fa fa-spinner fa-spin waitingNotification' style="display: none; float: right; margin-right: 25px; margin-top: -23px; position: relative; z-index: 2;"></span>

                                            <asp:HiddenField ID="hdfMaterialAgentsValue" runat="server" />

                                            <asp:HiddenField ID="hdfMaterialAgentsText" runat="server" />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-3 text-left">
                                            <label id="lblUnsafeConditions" runat="server" for="<%=cboUnsafeConditions.ClientID%>" class="control-label"></label>
                                        </div>

                                        <div class="col-sm-9">
                                            <asp:DropDownList ID="cboUnsafeConditions" CssClass="form-control control-validation ignoreValidation" AutoPostBack="false" runat="server"></asp:DropDownList>

                                            <span id="cboUnsafeConditionsWaiting" class='fa fa-spinner fa-spin waitingNotification' style="display: none; float: right; margin-right: 25px; margin-top: -23px; position: relative; z-index: 2;"></span>

                                            <asp:HiddenField ID="hdfUnsafeConditionsValue" runat="server" />

                                            <asp:HiddenField ID="hdfUnsafeConditionsText" runat="server" />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-3 text-left">
                                            <label id="lblEstablishments" runat="server" for="<%=cboEstablishments.ClientID%>" class="control-label"></label>
                                        </div>

                                        <div class="col-sm-9">
                                            <asp:DropDownList ID="cboEstablishments" CssClass="form-control control-validation ignoreValidation" AutoPostBack="false" runat="server"></asp:DropDownList>

                                            <span id="cboEstablishmentsWaiting" class='fa fa-spinner fa-spin waitingNotification' style="display: none; float: right; margin-right: 25px; margin-top: -23px; position: relative; z-index: 2;"></span>

                                            <asp:HiddenField ID="hdfEstablishmentsValue" runat="server" />

                                            <asp:HiddenField ID="hdfEstablishmentsText" runat="server" />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-3 text-left">
                                            <label for="<%=cboDayFactor.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblDayFactor")%></label>
                                        </div>

                                        <div class="col-sm-9">
                                            <asp:DropDownList ID="cboDayFactor" CssClass="form-control control-validation ignoreValidation" AutoPostBack="false" runat="server"></asp:DropDownList>

                                            <span id="cboDayFactorWaiting" class='fa fa-spinner fa-spin waitingNotification' style="display: none; float: right; margin-right: 25px; margin-top: -23px; position: relative; z-index: 2;"></span>

                                            <asp:HiddenField ID="hdfDayFactorValue" runat="server" />

                                            <asp:HiddenField ID="hdfDayFactorText" runat="server" />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-3 text-left">
                                            <label id="lblPersonalFactor" runat="server" for="<%=cboPersonalFactor.ClientID%>" class="control-label"></label>
                                        </div>

                                        <div class="col-sm-9">
                                            <asp:DropDownList ID="cboPersonalFactor" CssClass="form-control control-validation ignoreValidation" AutoPostBack="false" runat="server"></asp:DropDownList>

                                            <span id="cboPersonalFactorWaiting" class='fa fa-spinner fa-spin waitingNotification' style="display: none; float: right; margin-right: 25px; margin-top: -23px; position: relative; z-index: 2;"></span>

                                            <asp:HiddenField ID="hdfPersonalFactorValue" runat="server" />

                                            <asp:HiddenField ID="hdfPersonalFactorText" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-6">
                                <div class="form-horizontal">

                                    <div class="form-group">
                                        <div class="col-sm-3 text-left">
                                            <label for="<%=cboJourneys.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblJourneys")%></label>
                                        </div>

                                        <div class="col-sm-9">
                                            <asp:DropDownList ID="cboJourneys" CssClass="form-control control-validation ignoreValidation" AutoPostBack="false" runat="server"></asp:DropDownList>

                                            <span id="cboJourneysWaiting" class='fa fa-spinner fa-spin waitingNotification' style="display: none; float: right; margin-right: 25px; margin-top: -23px; position: relative; z-index: 2;"></span>

                                            <asp:HiddenField ID="hdfJourneysValue" runat="server" />

                                            <asp:HiddenField ID="hdfJourneysText" runat="server" />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-3 text-left">
                                            <label for="<%=cboLaborMade.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblLaborMade")%></label>
                                        </div>

                                        <div class="col-sm-9">
                                            <asp:DropDownList ID="cboLaborMade" CssClass="form-control control-validation ignoreValidation" AutoPostBack="false" runat="server"></asp:DropDownList>

                                            <span id="cboLaborMadeWaiting" class='fa fa-spinner fa-spin waitingNotification' style="display: none; float: right; margin-right: 25px; margin-top: -23px; position: relative; z-index: 2;"></span>

                                            <asp:HiddenField ID="hdfLaborMadeValue" runat="server" />

                                            <asp:HiddenField ID="hdfLaborMadeText" runat="server" />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-3 text-left">
                                            <label for="<%=cboBodyParts.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblBodyParts")%></label>
                                        </div>

                                        <div class="col-sm-9">
                                            <asp:DropDownList ID="cboBodyParts" CssClass="form-control control-validation ignoreValidation" AutoPostBack="false" runat="server"></asp:DropDownList>

                                            <span id="cboBodyPartsWaiting" class='fa fa-spinner fa-spin waitingNotification' style="display: none; float: right; margin-right: 25px; margin-top: -23px; position: relative; z-index: 2;"></span>

                                            <asp:HiddenField ID="hdfBodyPartsValue" runat="server" />

                                            <asp:HiddenField ID="hdfBodyPartsText" runat="server" />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-3 text-left">
                                            <label for="<%=cboAccidentType.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblAccidentType")%></label>
                                        </div>

                                        <div class="col-sm-9">
                                            <asp:DropDownList ID="cboAccidentType" CssClass="form-control control-validation ignoreValidation" AutoPostBack="false" runat="server"></asp:DropDownList>

                                            <span id="cboAccidentTypeWaiting" class='fa fa-spinner fa-spin waitingNotification' style="display: none; float: right; margin-right: 25px; margin-top: -23px; position: relative; z-index: 2;"></span>

                                            <asp:HiddenField ID="hdfAccidentTypeValue" runat="server" />

                                            <asp:HiddenField ID="hdfAccidentTypeText" runat="server" />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-3 text-left">
                                            <label for="<%=cboDiseaseType.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblDiseaseType")%></label>
                                        </div>

                                        <div class="col-sm-9">
                                            <asp:DropDownList ID="cboDiseaseType" CssClass="form-control control-validation ignoreValidation" AutoPostBack="false" runat="server"></asp:DropDownList>

                                            <span id="cboDiseaseTypeWaiting" class='fa fa-spinner fa-spin waitingNotification' style="display: none; float: right; margin-right: 25px; margin-top: -23px; position: relative; z-index: 2;"></span>

                                            <asp:HiddenField ID="hdfDiseaseTypeValue" runat="server" />

                                            <asp:HiddenField ID="hdfDiseaseTypeText" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlDocumentsContent" runat="server" Visible="false">
                    <div class="container" style="width: 100%">
                        <h4 class="text-left text-primary"><%= GetLocalResourceObject("lblDocumentsSubtitle") %></h4>

                        <div style="margin-top: -100px" class="pull-right">
                            <button id="btnPrevToPage3" type="button" runat="server" class="btn btn-default btnAjaxAction"
                                onserverclick="BtnPrevToPage3_ServerClick" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnPrevious"))%>'
                                data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnPrevious"))%>'>
                                <span class="glyphicon glyphicon-chevron-left glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%= GetLocalResourceObject("btnPrevious") %>
                            </button>
                            &nbsp;
                            <button id="btnNextToPage5" type="button" runat="server" class="btn btn-default btnAjaxAction" disabled="disabled"
                                data-loading-text='<%$ Code:String.Concat(GetLocalResourceObject("btnNext"), "&nbsp;<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>")%>'
                                data-error-text='<%$ Code:String.Concat( GetLocalResourceObject("btnNext"), "&nbsp;<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>")%>'>
                                <%= GetLocalResourceObject("btnNext") %>&nbsp;<span class="glyphicon glyphicon-chevron-right glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            </button>
                        </div>
                        <br />

                        <div class="row">
                            <div class="col-sm-6">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-sm-2 text-left">
                                            <label for="<%=cboDocuments.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblDocuments")%></label>
                                        </div>

                                        <div class="col-sm-10">
                                            <asp:DropDownList ID="cboDocuments" CssClass="form-control control-validation cboAjaxAction" AutoPostBack="true" OnSelectedIndexChanged="CboDocuments_SelectedIndexChanged" runat="server"></asp:DropDownList>

                                            <span id="cboDocumentsWaiting" class='fa fa-spinner fa-spin waitingNotification' style="display: none; float: right; margin-right: 25px; margin-top: -23px; position: relative; z-index: 2;"></span>

                                            <asp:HiddenField ID="hdfDocumentsValue" runat="server" />

                                            <asp:HiddenField ID="hdfDocumentsText" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-12">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-sm-1 text-left">
                                            <label for="<%=txtDocumentDescription.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblDocumentDescription")%></label>
                                        </div>

                                        <div class="col-sm-10">
                                            <asp:TextBox ID="txtDocumentDescription" CssClass="form-control cleanPasteText control-validation" runat="server" onkeypress="return isNumberOrLetter(event) && checkMaxLength(this,event,2000);" onkeyUp="return isNumberOrLetter(event) && checkMaxLength(this,event,2000);" MaxLength="2000" TextMode="MultiLine" Rows="10"></asp:TextBox>

                                            <label id="txtDocumentDescriptionValidation" for="<%= txtDetailedDescription.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjDocumentDescriptionValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>

                                            <asp:HiddenField ID="hdfDocumentDescriptionValue" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row" style="text-align: right;">
                            <div class="col-sm-12">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-sm-1 text-left"></div>

                                        <div class="col-sm-10" style="text-align: right;">
                                            <button id="btnSaveDocumentValue" type="button" runat="server" class="btn btn-default btnAjaxAction" 
                                                onclick="if(!ProcessSaveDocumentValue(this.id)){return false;}" onserverclick="BtnSaveDocumentValue_ServerClick" 
                                                data-loading-text='<%$ Code:String.Concat(GetLocalResourceObject("btnSaveDocumentValue"), "&nbsp;<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>")%>' 
                                                data-error-text='<%$ Code:String.Concat( GetLocalResourceObject("btnSaveDocumentValue"), "&nbsp;<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>")%>'>
                                                <span class="glyphicon glyphicon-floppy-disk glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%= GetLocalResourceObject("btnSaveDocumentValue") %>
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlResults" runat="server" Visible="false">
                    <div class="container" style="width: 100%">
                        <div class="row">
                            <div class="btn-group" role="group" aria-label="main-buttons">
                                <button id="btnReturnToMain" type="button" runat="server" class="btn btn-default btnAjaxAction" onserverclick="BtnReturnToMain_ServerClick" 
                                    data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnReturnToMain"))%>' 
                                    data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnReturnToMain"))%>'>
                                    <span class="glyphicon glyphicon-ok glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                                    <br />
                                    <%= GetLocalResourceObject("btnReturnToMain") %>
                                </button>
                            </div>
                        </div>

                        <div class="row" style="text-align: center;">
                            <div class="col-sm-10">
                                <h4 class="text-left text-primary"><%= GetLocalResourceObject("lblResults") %></h4>
                                <br />

                                <asp:Repeater ID="rptResults" runat="server">
                                    <HeaderTemplate>
                                        <table id="tableSelectedParticipants" class="table table-hover table-striped">
                                            <thead>
                                                <tr>
                                                    <th>
                                                        <div>
                                                            <div class="col-xs-3 col-sm-3 text-primary sorter" style="cursor: pointer; text-align: center;" data-sort-attr="data-sort-id" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblEmployeeCodeHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                            <div class="col-xs-3 col-sm-3 text-primary sorter" style="cursor: pointer; text-align: center;" data-sort-attr="data-sort-name" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblEmployeeNameHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                            <div class="col-xs-3 col-sm-3 text-primary sorter" style="cursor: pointer; text-align: center;" data-sort-attr="data-sort-cause" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblCauseHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                            <div class="col-xs-3 col-sm-3 text-primary sorter" style="cursor: pointer; text-align: center;" data-sort-attr="data-sort-case" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblEmployeeCaseNumber") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                        </div>
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <tr>
                                            <td class="row-fluid">
                                                <asp:UpdatePanel runat="server" ID="uppEmployee" UpdateMode="Conditional">
                                                    <Triggers>
                                                    </Triggers>

                                                    <ContentTemplate>
                                                        <div class="data-sort-src" data-sort-id='<%# Eval("EmployeeCode") %>' data-sort-name='<%# Eval("EmployeeName") %>' data-sort-cause='<%# Eval("CauseName") %>' data-sort-case='<%# Eval("CaseID") %>'>
                                                            <div class="col-xs-3 col-sm-3">
                                                                <span>
                                                                    <%# Eval("EmployeeCode") %>
                                                                </span>
                                                            </div>

                                                            <div class="col-xs-3 col-sm-3">
                                                                <span>
                                                                    <%# Eval("EmployeeName") %>
                                                                </span>
                                                            </div>

                                                            <div class="col-xs-3 col-sm-3">
                                                                <span>
                                                                    <%# Eval("CauseName") %>
                                                                </span>
                                                            </div>

                                                            <div class="col-xs-3 col-sm-3">
                                                                <span>
                                                                    <%# Eval("CaseID") %>
                                                                </span>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
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
                </asp:Panel>

                <div class="row" id="SaveAction" style="display: none; text-align: center;">
                    <div class="ButtonsActions">
                        <div class="btn-group" role="group" aria-label="main-buttons">
                            <button id="btnSave" type="button" runat="server" class="btn btn-default btnAjaxAction"
                                onclick="if(!ProcessSave(this.id)){return false;}" onserverclick="BtnSave_ServerClick"
                                data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnSave"))%>'
                                data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnSave"))%>'>
                                <span class="glyphicon glyphicon-floppy-disk glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                                <br />
                                <%= GetLocalResourceObject("btnSave") %>
                            </button>
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

    <%--  Modal  --%>

    <script type="text/javascript">
        //*******************************//
        //          VARIABLES            // 
        //*******************************//
        var dataSortAttribute, dataSortType, dataSortDirection;

        //*******************************//
        //       EVENT BINDING           // 
        //*******************************//
        function pageLoad(sender, args) {
            //////////////////////////////////////////////////////////////////////////////////////////////////

            //In this section we set the button state for btnAjaxAction
            $('.btnAjaxAction').on('click', function () {
                var $this = $(this);
                $this.button('loading');
                setTimeout(function () { $this.button('reset'); }, 36000);
            });

            $('.cboAjaxAction').on('change', function () {
                var $this = $(this);
                $this.siblings(".waitingNotification").show();
            });
            //////////////////////////////////////////////////////////////////////////////////////////////////

            //In this section we set the button generics functionality
            $('#btnCancel, #btnClose').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                event.preventDefault();
                $('#RelatedCaseDialog').modal('hide');
            });
            //////////////////////////////////////////////////////////////////////////////////////////////////

            //In this section we set the others controls functionality 
            

            $(".cboCause").change(function () {
                $(".cboInterestGroups").prop("disabled", true);

                $(".txtDescription").prop("disabled", true);
            });

            $(".EnterKeypress").on('keyup keypress', function (e) {
                var keyCode = e.keyCode || e.which;
                if (keyCode === 13) {
                    e.preventDefault();
                    return false;
                }
            });

            $(".EnterKeyPressSearch").keypress(function (event) {
                if (event.keyCode === 13) {
                    $("#ctl00_cntBody_btnSearchFilter").click();
                }
            });

            $('#<%= btnSearchRelatedCase.ClientID %>').on('click', function (ev) {
                /// <summary>Handles the click event for button add.</summary>
                ev.preventDefault();

                setTimeout(function () {
                    $("#<%=btnSearchRelatedCase.ClientID%>").button('reset');
                }, 500);

                $('#RelatedCaseDialogTitle').html('<%= GetLocalResourceObject("RelatedCaseDialogTitle") %>');
                $('#dialogTitle').html('<%= GetLocalResourceObject("RelatedCaseDialogTitle") %>');
                $('#RelatedCaseDialog').modal('show');

                return false;
            });

            $('#<%=btnAcceptRelatedCase.ClientID%>').on('click', function (event) {
                __doPostBack('<%= btnAcceptRelatedCase.UniqueID %>', 'OnClick');
                event.preventDefault();

                $('#RelatedCaseDialog').modal('hide');
            });

            $('#<%=chkCloseDate.ClientID%>').change(function () {
                if (this.checked) {
                    $('#<%=txtDescription.ClientID%>').attr("required", "true");
                } else {
                    $('#<%=txtDescription.ClientID%>').attr("required", "false");
                    $('#<%=txtDescription.ClientID%>').removeAttr('required');;
                }
            });

            $('#<%= dtpIncidentDate.ClientID %>').datetimepicker({
                format: 'MM/DD/YYYY'
            });

            var dateDtpIncident = $('#<%= dtpIncidentDate.ClientID %>').val();
            if (dateDtpIncident != undefined && dateDtpIncident != '') {
                $('#<%= dtpIncidentDate.ClientID %>').data("DateTimePicker").show()
                $('#<%= dtpIncidentDate.ClientID %>').data("DateTimePicker").hide()
            }

            $('#<%= tpcIncidentDateTime.ClientID %>').datetimepicker({
                format: 'HH:mm'
            });

            $('#<%= dtpSuspendWorkDate.ClientID %>').datetimepicker({
                format: 'MM/DD/YYYY'
            });

            var dateDtpSuspendWork = $('#<%= dtpSuspendWorkDate.ClientID %>').val();
            if (dateDtpSuspendWork != undefined && dateDtpSuspendWork != '') {
                $('#<%= dtpSuspendWorkDate.ClientID %>').data("DateTimePicker").show()
                $('#<%= dtpSuspendWorkDate.ClientID %>').data("DateTimePicker").hide()
            }

            $('#<%= tpcSuspendWorkTime.ClientID %>').datetimepicker({
                format: 'HH:mm'
            });

            $('.dtpSuspendWorkDateMultipleTfl').datetimepicker({
                format: 'MM/DD/YYYY'
            });

            $('.tpcSuspendWorkTimeMultipleTfl').datetimepicker({
                format: 'HH:mm'
            });

            $('.dtpFinalDateMultipleTfl').datetimepicker({
                format: 'MM/DD/YYYY'
            });

            $('#<%= dtpFinalDate.ClientID %>').datetimepicker({
                format: 'MM/DD/YYYY'
            });

            var dateDtpFinal = $('#<%= dtpFinalDate.ClientID %>').val();
            if (dateDtpFinal != undefined && dateDtpFinal != '') {
                $('#<%= dtpFinalDate.ClientID %>').data("DateTimePicker").show()
                $('#<%= dtpFinalDate.ClientID %>').data("DateTimePicker").hide()
            }

            $('#<%= dtpIncidentDate.ClientID %>').on("blur", function () {
                var ValidDateVal = $(this).data("DateTimePicker").date().format('MM/DD/YYYY');
                $(this).val(ValidDateVal);
            });

            $('#<%= tpcIncidentDateTime.ClientID %>').on("blur", function () {
                var ValidDateVal = $(this).data("DateTimePicker").date().format('HH:mm');
                $(this).val(ValidDateVal);
            });

            $('#<%= dtpSuspendWorkDate.ClientID %>').on("blur", function () {
                if ($('#<%= cboTimeUnit.ClientID %>').val() != undefined) {
                    if ($('#<%= cboTimeUnit.ClientID %>').val() == "Hours") {
                        calculatedHourManage();
                    }
                }
            });

            $('#<%= dtpSuspendWorkDate.ClientID %> .dtpSuspendWorkDateMultipleTfl').on("blur", function () {
                var ValidDateVal = $(this).data("DateTimePicker").date().format('MM/DD/YYYY');
                $(this).val(ValidDateVal);
            });

            $('#<%= tpcSuspendWorkTime.ClientID %> .tpcSuspendWorkTimeMultipleTfl').on("blur", function () {
                var ValidDateVal = $(this).data("DateTimePicker").date().format('HH:mm');
                $(this).val(ValidDateVal);
            });

            $('#<%= dtpFinalDate.ClientID %> .dtpFinalDateMultipleTfl').on("blur", function () {
                var ValidDateVal = $(this).data("DateTimePicker").date().format('MM/DD/YYYY');
                $(this).val(ValidDateVal);
            });

            $("#txtSearchEmployees").on("keyup", function () {
                var value = $(this).val().toLowerCase();
                $("#ctl00_cntBody_grvEmployees tbody tr").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
                });
            });


            $('#<%= cboTimeUnit.ClientID %>').on("change", function () {
                SetDaysHoursSelection();
            });

            $('.cboTimeUnitMultipleTfl').on("change", function () {
                SetDaysHoursSelectionMultipleTfl(this);
            });

            $('#<%= cboCaseState.ClientID %>').on("change", function () {
                SetRelatedCaseVisibility();
            });

            $('#<%= dtpSuspendWorkDate.ClientID %>').on("blur", function () {
                ReviewFinalDate($('#<%= dtpSuspendWorkDate.ClientID %>'));
            });

            $('.dtpSuspendWorkDateMultipleTfl').on("blur", function () {
                ReviewFinalDateMultipleTfl(this);
            });

            $('#<%= dtpFinalDate.ClientID %>').on("blur", function () {
                ReviewFinalDate($('#<%= dtpFinalDate.ClientID %>'));
            });

            $('.dtpFinalDateMultipleTfl').on("blur", function () {
                ReviewFinalDateMultipleTfl(this);
            });

            $('.tpcSuspendWorkTimeMultipleTfl').on("blur change", function () {
                CalculateDaysMultipleTfl(this);
            });

            $('#<%= tpcSuspendWorkTime.ClientID %>').on("blur change", function () {
                CalculateDays();
            });

            $('#<%= txtNaturalDays.ClientID %>').on("change", function () {

            });

            $('#<%= cboCause.ClientID%>').on('change', function () {
                SetSaveActionVisibility();
                SetHoursRequired();
                SetNextActionVisibility();
                CalculateDays();
            });

            $('#<%= txtDocumentDescription.ClientID %>').keypress(function () {
                var textLength = 2000;
                if (textLength == $('#<%= txtDocumentDescription.ClientID %>').val().length) {
                    $('#txtDocumentDescriptionValidation').show();
                }
                else {
                    $('#txtDocumentDescriptionValidation').hide();
                }
            });
            ///<summary>Method to check or uncheck ALL the employeess</summary>
            $('#selectAll').click(function (e) {
                var table = $(e.target).closest('table');
                $('td input:checkbox', table).prop('checked', this.checked);

                if (this.checked) {
                    var value = $('#<%= hdfEmployeesSelected.ClientID %>').val();
                    $('#<%= hdfSelectAll.ClientID %>').val(1);
                    
                    $('td input:checkbox', table).each(function (index, tr) {
                        const recordSelected = $($(this).parents('tr').find('input[type="hidden"]')[0]).val().split('/');
                        
                        value = value + ',' + recordSelected[4];
                        $('#<%= hdfEmployeesSelected.ClientID %>').val(value);
                    });
                } else {
                    $('#<%= hdfEmployeesSelected.ClientID %>').val('');
                    $('#<%= hdfSelectAll.ClientID %>').val('');
                }
            });

            $('.employeeSelected').click(function (e) {
                var value = $('#<%= hdfEmployeesSelected.ClientID %>').val();
                const recordSelected = $($(this).parents('tr').find('input[type="hidden"]')[0]).val().split('/');

                if ($($(this).parents('tr').find('input[type="checkbox"]')[0]).prop('checked') === false) {
                    value = value.replace(',' + recordSelected[4], '');
                    $('#<%= hdfSelectAll.ClientID %>').val('');
                } else {
                    value = value + ',' + recordSelected[4];
                }

                ValidateCheckAll();
                $('#<%= hdfEmployeesSelected.ClientID %>').val(value);
            });

            $('.participantSelected').click(function (e) {
                var value = $('#<%= hdfParticipantsSelected.ClientID %>').val();
                uniq = [...new Set(value.split(","))];
                value = uniq.toString();
                const recordSelected = $($(this).parents('tr').find('input[type="hidden"]')[0]).val().split('/');

                if ($($(this).parents('tr').find('input[type="checkbox"]')[0]).prop('checked') === false) {
                    value = value.replace(',' + recordSelected[4], '');
                } else {
                    value = value + ',' + recordSelected[4];
                }

                $('#<%= hdfParticipantsSelected.ClientID %>').val(value);
            });

            $('.employeeDirectSelected').click(function (e) {
                var value = $('#<%= hdfEmployeeSelectedDirect.ClientID %>').val();
                const recordSelected = $($(this).parents('tr').find('input[type="hidden"]')[0]).val().split('/');
                $('#<%= hdfEmployeeSelectedDirect.ClientID %>').val($($(this).parents('tr').find('span')[1]).html());
            });
            //////////////////////////////////////////////////////////////////////////////////////////////////

            //In this section we set the clean paste manager functionality
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

            $('.cleanPasteDigits').on('paste', function (e) {
                var $this = $(this);

                setTimeout(function (e) {
                    replacePastedInvalidDigits($this);
                    var ml = $this.attr("maxlength");
                    if (ml) {
                        checkMaxLength($this[0], e, parseInt(ml));
                    }
                }, 50);
            });
            //////////////////////////////////////////////////////////////////////////////////////////////////

            InitializeTooltipsPopovers();

            $(".sorter").click(function () {
                var $table = $(this).closest('table');
                var $dataSortSrc = $(this).find()
                dataSortAttribute = $(this).attr("data-sort-attr");
                dataSortType = $(this).attr("data-sort-type");
                dataSortDirection = $(this).attr("data-sort-direction");

                if (isEmptyOrSpaces(dataSortDirection)) {
                    dataSortDirection = "0";
                }
                else if (dataSortDirection == "0") {
                    dataSortDirection = "1";
                }
                else if (dataSortDirection == "1") {
                    dataSortDirection = "0";
                }

                $table.children('tbody').sortChildren(function (A, B) {
                    var a, b;
                    if (dataSortType == "int") {
                        a = parseInt($(A).find(".data-sort-src").attr(dataSortAttribute));
                        b = parseInt($(B).find(".data-sort-src").attr(dataSortAttribute));
                    }
                    if (dataSortType == "string") {
                        a = $(A).find(".data-sort-src").attr(dataSortAttribute);
                        b = $(B).find(".data-sort-src").attr(dataSortAttribute);
                    }

                    var orderA, orderB;
                    if (dataSortDirection == "0") {
                        orderA = -1;
                        orderB = 1;
                    }
                    else if (dataSortDirection == "1") {
                        orderA = 1;
                        orderB = -1;
                    }

                    return a < b ? orderA : a > b ? orderB : 0;
                });

                $table.find(".sorter").each(function () {
                    $(this).attr("data-sort-direction", "");
                    $(this).find(".sorterDirection").removeClass("fa-sort-asc");
                    $(this).find(".sorterDirection").removeClass("fa-sort-desc");
                });

                $(this).attr("data-sort-direction", dataSortDirection);
                if (dataSortDirection == "0") {
                    $(this).find(".sorterDirection").addClass("fa-sort-asc");
                }
                else if (dataSortDirection == "1") {
                    $(this).find(".sorterDirection").addClass("fa-sort-desc");
                }
            })

            SetRelatedCaseVisibility();
            SetDocumentDescriptionState();
            SetSaveActionVisibility();
            SetHoursRequired();
            SetNextActionVisibility();
            SetDaysHoursSelection();
        }

        //*******************************//
        //          VALIDATION           // 
        //*******************************//
        var validator = null;
        function ValidateAbsenteeismContent() {
            /// <summary>Validate the form with jquey validation plugin </summary>
            /// <returns> True if form is valid. False otherwise. </returns>
            if (validator != null) {
                validator.resetForm();
                validator = null;
            }

            if (validator == null) {
                //add custom validation methods
                jQuery.validator.addMethod("validSelection", function (value, element) {
                    return this.optional(element) || value != "-1";
                }, "Please select a valid option");

                jQuery.validator.addMethod("validDate", function (value, element) {
                    return this.optional(element) || moment(value, "MM/DD/YYYY").isValid();
                }, "Please enter a valid date in the format MM/DD/YYYY");

                jQuery.validator.addMethod("validTime", function (value, element) {
                    return this.optional(element) || moment(value, "HH:mm").isValid();
                }, "Please enter a valid time in the format HH:mm");

                jQuery.validator.addMethod("validRelatedCase", function (value, element) {
                    return this.optional(element) || (!($('#<%= btnSearchRelatedCase.ClientID %>').is(":disabled")) && $.isNumeric($('#<%= txtRelatedCase.ClientID %>').val()));
                }, "Please enter a valid related time");

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
                            "<%= cboInterestGroups.UniqueID %>": {
                                required: true,
                                validSelection: true
                            },
                            "<%= cboCaseState.UniqueID %>": {
                                required: true,
                                validSelection: true
                            },

                            "<%= cboCause.UniqueID %>": {
                                required: true,
                                validSelection: true
                            },

                            "<%= txtDetailedDescription.UniqueID %>": {
                                required: true,
                                normalizer: function (value) {
                                    return $.trim(value);
                                },
                                minlength: 1,
                                maxlength: 2000
                            },
                            "<%= dtpIncidentDate.UniqueID %>": {
                                required: true,
                                validDate: true
                            },
                            "<%= tpcIncidentDateTime.UniqueID %>": {
                                required: true,
                                validTime: true
                            },
                            "<%= dtpSuspendWorkDate.UniqueID %>": {
                                required: true,
                                validDate: true
                            },
                            "<%= tpcSuspendWorkTime.UniqueID %>": {
                                required: true,
                                validTime: true
                            },

                            "<%= txtNaturalDays.UniqueID %>": {
                                required: true,
                                digits: true,
                                min: 1,
                                max: 10000
                            },
                            "<%= cboTimeUnit.UniqueID %>": {
                                required: true,
                                validSelection: true
                            },
                            "<%= dtpFinalDate.UniqueID %>": {
                                required: true,
                                validDate: true
                            },
                            "<%= txtFinalHours.UniqueID %>": {
                                required: true,
                                normalizer: function (value) {
                                    return $.trim(value).replace(",", ".");
                                },
                                number: true,
                                min: 0.5,
                                max: 8
                            }
                        }
                    });
            }

            var result = validator.form();
            return result;
        }

        function ValidateSaveDocument() {
            /// <summary>Validate the form with jquey validation plugin </summary>
            /// <returns> True if form is valid. False otherwise. </returns>
            if (validator != null) {
                validator.resetForm();
                validator = null;
            }

            if (validator == null) {
                jQuery.validator.addMethod("validSelection", function (value, element) {
                    return this.optional(element) || value != "-1";
                }, "Please select a valid option");

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
                                "<%= cboDocuments.UniqueID %>": {
                                required: true,
                                validSelection: true
                            },
                                "<%= txtDocumentDescription.UniqueID %>": {
                                required: true,
                                normalizer: function (value) {
                                    return $.trim(value);
                                },
                                minlength: 1,
                                maxlength: 2000
                            }
                        }
                    });
            }

            var result = validator.form();
            return result;
        }

        //*******************************//
        //             LOGIC             // 
        //*******************************//
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

        function reviewCheckHeader(table) {

        }

        function reviewOnlyOneCheck(table, check) {
            var count = $('#' + table + ' tbody td input:checkbox').length;
            var checkedCount = 0;
            var i = 0;

            $('#' + table + ' tbody td input:checkbox').each(function () {
                $(this).prop('checked', false);
            });

            $('#' + $(check)[0].id).prop('checked', true);
        }

        function ProcessValidateAbsenteeismContent(resetId) {
            /// <summary>Process the validation of page 2 request according to the validation</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            if (!ValidateAbsenteeismContent()) {
                ResetButton(resetId);
                return false;
            }

            else {
                return true;
            }
        }

        function ProcessSaveDocumentValue(resetId) {
            /// <summary>Process the validation of page 4 request according to the validation</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            if (!ValidateSaveDocument()) {
                ResetButton(resetId);
                return false;
            }

            else {
                return true;
            }
        }

        function ProcessSave(resetId) {
            /// <summary>Process the save request according to the validation</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            if (!($('#<%= pnlAbsenteeismContent.ClientID %>').is(":hidden")) && !ValidateAbsenteeismContent()) {
                ResetButton(resetId);
                return false;
            } else if (!($('#<%= pnlDocumentsContent.ClientID %>').is(":hidden")) && false) {
            } else {
                return ShowConfirmationMessageSave(resetId);
            }
        }

        function ProcessCloseDate(check) {
            /// <summary>Process the save request according to the validation</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            return ShowConfirmationMessageCloseDate(check);
        }

        function ShowConfirmationMessageCloseDate(check) {
            /// <summary>Show confirmation message for Delete funtionality</summary>
            /// <param name="resetId" type="String">Id of the button to reset if cancel</param>
            /// <returns>False</returns>
            if ($('#' + check).prop('checked')) {
                MostrarConfirmacion(
                            '<%= GetLocalResourceObject("msjConfirmationCloseDate") %>'
                    , '<%=GetLocalResourceObject("Yes")%>'
                    , function () {
                        $('#' + check).prop('checked', true);
                    }
                    , '<%=GetLocalResourceObject("No")%>'
                    , function () {
                        $('#' + check).prop('checked', false);
                    }
                );
            }

            return true;
        }

        function postBackTrvGroupDataByObject() {
            /// <summary>Manage the checkbox change event for treeview</summary>
            var o = window.event.srcElement;

            if (o.tagName == "INPUT" && o.type == "checkbox") {
                $('#trvGroupDataWaiting').fadeIn('fast');
                $('#trvSearchEmployeesWaiting').fadeIn('fast');

                $('#<%= trvGroupData.ClientID %>').fadeTo('fast', 0.5);
                $('#uppSearchEmployeesParent').fadeTo('fast', 0.5);
                $('#txtSearchEmployees').attr('readonly', true);

                __doPostBack("", "");
                $('#<%= trvGroupData.ClientID %>').find("a").removeAttr('href');
                $('#<%= trvGroupData.ClientID %>').find("a,input,button,textarea,select").attr("disabled", "disabled");

                $('#uppSearchEmployeesParent').find("a").removeAttr('href');
                $('#uppSearchEmployeesParent').find("a,input,button,textarea,select").attr("disabled", "disabled");
            }

            return false;
        }

        function ReturnFromGridDataBound() {
            $('#txtSearchEmployees').attr('readonly', false);
        }
         ///<summary>Method to evaluate if the checkALL is marked</summary>
        function ValidateCheckAll() {
            if ($('#<%= hdfSelectAll.ClientID %>').val() == '1') {
                $('#selectAll').prop('checked', true);
            } else {
                $('#selectAll').prop('checked', false);
            }
        }
        function UpdateLblCountEmployes(value) {
            $('#<%= lblCount.ClientID %>').text(' ( ' + value + ' )');
        }

        function ValidateDataForDates() {
            /// <summary>Validate the dates of suspension and final are set</summary>            
            /// <returns> True if is valid. False otherwise. </returns>
            if ($('#<%= dtpSuspendWorkDate.ClientID %>').val() != ""
                && $('#<%= dtpSuspendWorkDate.ClientID %>').data("DateTimePicker") != null
                && $('#<%= dtpSuspendWorkDate.ClientID %>').data("DateTimePicker").date() != null
                && $('#<%= dtpFinalDate.ClientID %>').val() != ""
                && $('#<%= dtpFinalDate.ClientID %>').data("DateTimePicker") != null
                && $('#<%= dtpFinalDate.ClientID %>').data("DateTimePicker").date() != null) {

                return true;
            }

            return false;
        }

        function ValidateDataForDatesMultipleTfl(suspendedData, finalData) {
            /// <summary>Validate the dates of suspension and final are set</summary>            
            /// <returns> True if is valid. False otherwise. </returns>
            if ($(suspendedData).val() != ""
                && $(suspendedData).data("DateTimePicker") != null
                && $(suspendedData).data("DateTimePicker").date() != null
                && $(finalData).val() != ""
                && $(finalData).data("DateTimePicker") != null
                && $(finalData).data("DateTimePicker").date() != null) {

                return true;
            }

            return false;

        }

        function ReviewFinalDate(changedControl) {
            /// <summary>Manage the logic between suspend date and natural days and also re calculate days for final date</summary>
            if ($('#<%= dtpSuspendWorkDate.ClientID %>').val() != ""
                && $('#<%= cboTimeUnit.ClientID %>').val() != "Days"
                && $('#<%= dtpFinalDate.ClientID %>').val() == "") {
                $('#<%= dtpFinalDate.ClientID %>').val($('#<%= dtpSuspendWorkDate.ClientID %>').val());
                $('#<%= dtpFinalDate.ClientID %>').data("DateTimePicker").date($('#<%= dtpSuspendWorkDate.ClientID %>').val());

            } else if ($('#<%= dtpSuspendWorkDate.ClientID %>').val() != ""
                && $('#<%= cboTimeUnit.ClientID %>').val() != "Days"
                && $('#<%= dtpFinalDate.ClientID %>').val() != "") {
                $('#<%= dtpFinalDate.ClientID %>').val($('#<%= dtpSuspendWorkDate.ClientID %>').val());
                $('#<%= dtpFinalDate.ClientID %>').data("DateTimePicker").date($('#<%= dtpSuspendWorkDate.ClientID %>').val());

            } else if ($('#<%= dtpSuspendWorkDate.ClientID %>').val() != ""
                && $('#<%= cboTimeUnit.ClientID %>').val() != "Days"
                && $('#<%= dtpFinalDate.ClientID %>').val() != ""
                && $('#<%= dtpSuspendWorkDate.ClientID %>').val() != $('#<%= dtpFinalDate.ClientID %>').val()) {
                $('#<%= dtpFinalDate.ClientID %>').val($('#<%= dtpSuspendWorkDate.ClientID %>').val());
                $('#<%= dtpFinalDate.ClientID %>').data("DateTimePicker").date($('#<%= dtpSuspendWorkDate.ClientID %>').val());
            }

            if (ValidateDataForDates()) {
                // Recalculate it
                var suspendWorkDate = $('#<%= dtpSuspendWorkDate.ClientID %>').data("DateTimePicker").date().startOf('day');

                var finalDate = $('#<%= dtpFinalDate.ClientID %>').data("DateTimePicker").date().startOf('day');

                if (finalDate.diff(suspendWorkDate, 'days', true) < 0) {
                    //we have a problem with dates setup
                    ShowFooterAlert('<%=GetLocalResourceObject("msjDatesValidation")%>');
                    AddTemporaryClass(changedControl, "btn-warning", 1500);
                    changedControl.val("");

                    $('#<%= txtNaturalDays.ClientID %>').val("");
                }
                else {
                    CalculateDays();
                }
            }
        }

        function ReviewFinalDateMultipleTfl(changedControl) {
            /// <summary>Manage the logic between suspend date and natural days and also re calculate days for final date</summary>
            var control = $(changedControl);
            var controlFechaSuspende;
            var controlFechaFinal;
            var controlUnidadTiempo;
            var controlNaturalDays;

            if (control[0].id.split('_')[4] == "dtpSuspendWorkDateMultipleTfl") {
                controlFechaSuspende = $('#' + control[0].id);
                controlFechaFinal = $('#' + control[0].id.replace('dtpSuspendWorkDateMultipleTfl', 'dtpFinalDateMultipleTfl'));
                controlUnidadTiempo = $('#' + control[0].id.replace('dtpSuspendWorkDateMultipleTfl', 'cboTimeUnitMultipleTfl'));
                controlNaturalDays = $('#' + control[0].id.replace('dtpSuspendWorkDateMultipleTfl', 'txtNaturalDaysMultipleTfl'));
            } else {
                controlFechaFinal = $('#' + control[0].id);
                controlFechaSuspende = $('#' + control[0].id.replace('dtpFinalDateMultipleTfl', 'dtpSuspendWorkDateMultipleTfl'));
                controlUnidadTiempo = $('#' + control[0].id.replace('dtpFinalDateMultipleTfl', 'cboTimeUnitMultipleTfl'));
                controlNaturalDays = $('#' + control[0].id.replace('dtpFinalDateMultipleTfl', 'txtNaturalDaysMultipleTfl'));
            }

            if ($(controlFechaSuspende).val() != ""
                && $(controlUnidadTiempo).val() != "Days"
                && $(controlFechaFinal).val() == "") {
                $(controlFechaFinal).val($(controlFechaSuspende).val());
                $(controlFechaFinal).data("DateTimePicker").date($(controlFechaSuspende).val());

            } else if ($(controlFechaSuspende).val() != ""
                && $(controlUnidadTiempo).val() != "Days"
                && $(controlFechaFinal).val() != "") {
                $(controlFechaFinal).val($(controlFechaSuspende).val());
                $(controlFechaFinal).data("DateTimePicker").date($(controlFechaSuspende).val());

            } else if ($(controlFechaSuspende).val() != ""
                && $(controlUnidadTiempo).val() != "Days"
                && $(controlFechaFinal).val() != ""
                && $(controlFechaSuspende).val() != $(controlFechaFinal).val()) {
                $(controlFechaFinal).val($(controlFechaSuspende).val());
                $(controlFechaFinal).data("DateTimePicker").date($(controlFechaSuspende).val());
            }

            // if both days are setting
            if (ValidateDataForDatesMultipleTfl(controlFechaSuspende, controlFechaFinal)) {
                // Recalculate it
                var suspendWorkDate = $(controlFechaSuspende).data("DateTimePicker").date().startOf('day');

                var finalDate = $(controlFechaFinal).data("DateTimePicker").date().startOf('day');

                if (finalDate.diff(suspendWorkDate, 'days', true) < 0) {
                    //we have a problem with dates setup
                    ShowFooterAlert('<%=GetLocalResourceObject("msjDatesValidation")%>');
                    AddTemporaryClass(control, "btn-warning", 1500);
                    control.val("");

                    $(controlNaturalDays).val("");
                }
                else {
                    CalculateDaysMultipleTfl(controlFechaSuspende, controlFechaFinal, controlNaturalDays);
                }
            }
        }

        function CalculateDays() {
            /// <summary>Manage the logic between start and end date and also re calculate days</summary>
            if (ValidateDataForDates()) {
                var suspendWorkDate = $('#<%= dtpSuspendWorkDate.ClientID %>').data("DateTimePicker").date().startOf('day');
                var finalDate = $('#<%= dtpFinalDate.ClientID %>').data("DateTimePicker").date().startOf('day');

                var difference = finalDate.diff(suspendWorkDate, 'days')
                difference = difference + 1;

                $('#<%= txtNaturalDays.ClientID %>').val(difference);
            }
        }

        function CalculateDaysMultipleTfl(suspendedData, finalData, controlNaturalDays) {
            /// <summary>Manage the logic between start and end date and also re calculate days</summary>
            if (ValidateDataForDatesMultipleTfl(suspendedData, finalData)) {
                var suspendWorkDate = $(suspendedData).data("DateTimePicker").date().startOf('day');
                var finalDate = $(finalData).data("DateTimePicker").date().startOf('day');

                var difference = finalDate.diff(suspendWorkDate, 'days')
                difference = difference + 1;

                $(controlNaturalDays).val(difference);
            }
        }

        function CalculateFinalDate() {
            /// <summary>Manage the logic between start and end date and also re calculate days by Course duration</summary>
            var suspendWorkDate = $('#<%= dtpSuspendWorkDate.ClientID %>').data("DateTimePicker").date().startOf('day');
            var finalDate = suspendWorkDate.add($('#<%= txtNaturalDays.ClientID %>').val(), 'day');

            $('#<%= dtpFinalDate.ClientID %>').data("DateTimePicker").date(finalDate);
        }

        function SetDaysHoursSelection() {
            /// <summary>Manage the logic between days or hours selection</summary>
            if ($('#<%= cboTimeUnit.ClientID %>').val() != undefined) {
                if ($('#<%= cboTimeUnit.ClientID %>').val() == "Days") {

                    $('#<%= tpcSuspendWorkTime.ClientID %>').val("07:00");

                    if ($('#<%= cboCause.ClientID %>').val() != undefined) {
                        var causesDateRequired = [<%= GetCausesWithCategoryAccident() %>];
                        if (jQuery.inArray($('#<%= cboCause.ClientID %>').val(), causesDateRequired) < 0) {
                            $('#<%= tpcSuspendWorkTime.ClientID %>').attr("disabled", "disabled");
                        }
                    }

                    $('#<%= dtpFinalDate.ClientID %>').removeAttr("readonly");
                    $('#<%= dtpSuspendWorkDate.ClientID %>').removeAttr("readonly");

                    $('#<%= txtFinalHours.ClientID %>').val("0");

                    $('#HoursData').hide();
                    $('#NaturalDaysData').show();
                    $('#<%= txtNaturalDays.ClientID %>').val("")

                    if ($('#<%= dtpSuspendWorkDate.ClientID %>').val() != ""
                        && $('#<%= cboTimeUnit.ClientID %>').val() == "Days"
                        && $('#<%= dtpFinalDate.ClientID %>').val() != ""
                        && $('#<%= txtNaturalDays.ClientID %>').val() == "") {
                        var suspendWorkDate = $('#<%= dtpSuspendWorkDate.ClientID %>').data("DateTimePicker").date().startOf('day');

                        $('#<%= dtpFinalDate.ClientID %>').data("DateTimePicker").date($('#<%= dtpFinalDate.ClientID %>').val());

                        var dtpFinalDate = $('#<%= dtpFinalDate.ClientID %>').data("DateTimePicker").date().format('MM/DD/YYYY');

                        $('#<%= dtpFinalDate.ClientID %>').val(dtpFinalDate);

                        var finalDate;
                        var difference;

                        try {
                            finalDate = $('#<%= dtpFinalDate.ClientID %>').data("DateTimePicker").date().startOf('day');
                            difference = finalDate.diff(suspendWorkDate, 'days');
                        }
                        catch (ex) {
                            difference = 0;
                        }

                        difference = difference + 1;
                        $('#<%= txtNaturalDays.ClientID %>').val(difference);
                    }
                }

                else {
                    calculatedHourManage();

                    $('#<%= tpcSuspendWorkTime.ClientID %>').attr("disabled", false);
                    $('#<%= dtpFinalDate.ClientID %>').val($('#<%= dtpSuspendWorkDate.ClientID %>').val());
                    CalculateDays();

                    $('#<%= dtpFinalDate.ClientID %>').attr("readonly", "readonly");

                    $('#HoursData').show();
                    $('#NaturalDaysData').hide();
                }

                ReviewFinalDate();
            }
        }

        function calculatedHourManage() {
            var inputDate = $('#<%= dtpSuspendWorkDate.ClientID %>').val();
            var todayDate = moment(new Date()).format('MM/DD/YYYY');

            if (inputDate === todayDate) {
                var time = $('#<%= hdfEmployeeDirectSuspendTime.ClientID %>').val();
                $('#<%= tpcSuspendWorkTime.ClientID %>').val(time);
            } else {
                $('#<%= tpcSuspendWorkTime.ClientID %>').val("07:00");
            }
        }

        function SetDaysHoursSelectionMultipleTfl(control) {
            /// <summary>Manage the logic between days or hours selection</summary>
            var controlUnidad = $(control);
            var controlFechaSuspende;
            var controlFechaHoraSuspende;
            var controlFechaFinal;
            var controlHoras;
            var controlNaturalDays;

            controlFechaSuspende = $('#' + control.id.replace('cboTimeUnitMultipleTfl', 'dtpSuspendWorkDateMultipleTfl'));
            controlFechaHoraSuspende = $('#' + control.id.replace('cboTimeUnitMultipleTfl', 'tpcSuspendWorkTimeMultipleTfl'));
            controlFechaFinal = $('#' + control.id.replace('cboTimeUnitMultipleTfl', 'dtpFinalDateMultipleTfl'));
            controlNaturalDays = $('#' + control.id.replace('cboTimeUnitMultipleTfl', 'txtNaturalDaysMultipleTfl'));
            controlHoras = $('#' + control.id.replace('cboTimeUnitMultipleTfl', 'txtFinalHoursMultipleTfl'));

            if ($(controlUnidad).val() != undefined) {
                if ($(controlUnidad).val() == "Days") {

                    $(controlFechaHoraSuspende).val("07:00");

                    $(controlFechaFinal).removeAttr("disabled");
                    $(controlFechaSuspende).removeAttr("disabled");

                    $(controlHoras).val("0");

                    if ($(controlFechaSuspende).val() != ""
                        && $(controlUnidad).val() == "Days"
                        && $(controlFechaFinal).val() != ""
                        && $(controlHoras).val() == "0") {
                        var suspendWorkDate = $(controlFechaSuspende).data("DateTimePicker").date().startOf('day');

                        $(controlFechaFinal).data("DateTimePicker").date($(controlFechaFinal).val());

                        var dtpFinalDate = $(controlFechaFinal).data("DateTimePicker").date().format('MM/DD/YYYY');

                        $(controlFechaFinal).val(dtpFinalDate);

                        var finalDate;
                        var difference;
                        try {

                            finalDate = $(controlFechaFinal).data("DateTimePicker").date().startOf('day');
                            difference = finalDate.diff(suspendWorkDate, 'days');
                        }
                        catch (ex) {
                            difference = 0;
                        }

                        difference = difference + 1;
                        $(controlNaturalDays).val(difference);
                    }
                }

                else {

                    $(controlFechaHoraSuspende).removeAttr("disabled");
                    $(controlFechaFinal).val($(controlFechaSuspende).val());
                    CalculateDaysMultipleTfl(controlFechaSuspende, controlFechaFinal, controlNaturalDays);

                    $(controlFechaFinal).attr("disabled", "disabled");
                }

                ReviewFinalDateMultipleTfl(controlFechaSuspende);
            }
        }

        function SetRelatedCaseVisibility() {
            /// <summary>Manage the logic for the related case visibility</summary>
            //this validation is duplicated un code behind btnSave_ServerClick event
            if ($('#<%= cboCaseState.ClientID %>').val() == "RP" || $('#<%= cboCaseState.ClientID %>').val() == "AP") {

                $('#<%= btnSearchRelatedCase.ClientID %>').removeAttr("disabled");
                $('#<%= txtDescription.ClientID %>').attr('readonly', true);
                $('#<%= dtpIncidentDate.ClientID %>').prop('disabled', true);
                $('#<%= tpcIncidentDateTime.ClientID %>').prop('disabled', true);
            }
            else {
                if (validator != null) {
                    validator.resetForm();
                }

                $('#<%= btnSearchRelatedCase.ClientID %>').attr("disabled", true);
                $('#<%= txtRelatedCase.ClientID %>').val("");
                $('#<%= txtDescription.ClientID %>').attr('readonly', false);

                $('#<%= dtpIncidentDate.ClientID %>').prop('disabled', false);
                $('#<%= tpcIncidentDateTime.ClientID %>').prop('disabled', false);
            }

        }

        function SetDocumentDescriptionState() {
            /// <summary>Manage the logic for the document state</summary>
            if ($('#<%= cboDocuments.ClientID %>').val() == "") {
                $('#<%= txtDocumentDescription.ClientID %>').attr("readonly", "readonly");
                $('#<%= btnSaveDocumentValue.ClientID %>').attr("disabled", true);
            }

            else {

                $('#<%= txtDocumentDescription.ClientID %>').removeAttr("readonly");
                $('#<%= btnSaveDocumentValue.ClientID %>').removeAttr("disabled");
            }
        }

        function SetSaveActionVisibility() {
            /// <summary>Manage the logic for the save action visibility</summary>
            var causesWithAdditionalInformation = [<%= GetCausesWithAdditionalInformation() %>];

            $('#SaveAction').hide();

            if ($('#<%= pnlAbsenteeismContent.ClientID %>').is(":visible")) {
                if (jQuery.inArray($('#<%= cboCause.ClientID %>').val(), causesWithAdditionalInformation) <= -1) {
                    $('#SaveAction').show();
                }
            }

            if ($('#<%= pnlDocumentsContent.ClientID %>').is(":visible")) {
                $('#SaveAction').show();
            }
        }

        function SetStartDateVisibility(show) {
            /// <summary>Manage the logic for the start date visibility</summary>
            if (show) {
                $('#divHourStartDate').show();
            } else {
                $('#divHourStartDate').hide();
            }
        }

        function SetHoursRequired() {
            /// <summary>Manage the logic for the hours control required</summary>
            var causesDateRequired = [<%= GetCausesWithCategoryAccident() %>];
            if ($('#<%= cboCause.ClientID %>').val() != undefined) {
                if (jQuery.inArray($('#<%= cboCause.ClientID %>').val(), causesDateRequired) >= 0) {
                    if ($('#<%= cboTimeUnit.ClientID %>').val() == "Days") {
                        $("#divHour *").attr("disabled", false);
                        $('#<%= tpcSuspendWorkTime.ClientID %>').attr("disabled", false);
                        if ($('#<%= tpcSuspendWorkTime.ClientID %>').val() == "") {
                            $('#<%= tpcSuspendWorkTime.ClientID %>').val("");
                        }
                    }

                    SetStartDateVisibility(true);
                } else {
                    if ($('#<%= cboTimeUnit.ClientID %>').val() == "Days") {
                        $("#divHour *").attr("disabled", "disabled");
                        $('#<%= tpcSuspendWorkTime.ClientID %>').attr("disabled", "disabled");

                        if ($('#<%= cboCause.ClientID %>').val() != undefined) {
                            $('#<%= tpcSuspendWorkTime.ClientID %>').val("07:00");
                        }
                    }

                    SetStartDateVisibility(false);
                }
            }
        }

        function SetNextActionVisibility() {
            /// <summary>Manage the logic for the save action visibility</summary>
            var causesWithAdditionalInformation = [<%= GetCausesWithAdditionalInformation() %>];

            $('#<%= btnNextToPage3.ClientID %>').attr("disabled", true);

            if ($('#<%= pnlAbsenteeismContent.ClientID %>').is(":visible")) {
                if (jQuery.inArray($('#<%= cboCause.ClientID %>').val(), causesWithAdditionalInformation) > -1) {
                    $('#<%= btnNextToPage3.ClientID %>').removeAttr("disabled");
                }
            }
        }

        //*******************************//
        //  RETURN FROM POSTBACKS LOGIC  // 
        //*******************************//
        function ReturnFromBtnAcceptRelatedCaseClickPostBack() {

        }

        //*******************************//
        // MESSAGING AND CONFIRMATION    // 
        //*******************************//
        function ShowConfirmationMessageSave(resetId) {
            /// <summary>Show confirmation message for Save funtionality</summary>
            /// <param name="resetId" type="String">Id of the button to reset if cancel</param>
            /// <returns>False</returns>
            MostrarConfirmacion(
                '<%= GetLocalResourceObject("msjConfirmationSave") %>', '<%=GetLocalResourceObject("Yes")%>', function () {
                    ActivateModalProgress(true);
                    __doPostBack('<%= btnSave.UniqueID %>', '')
                },
                '<%=GetLocalResourceObject("No")%>', function () {
                    $("#" + resetId).button('reset');
                }
            );

            return false;
        }

        //*******************************//
        //AJAX CONCURRENCY ADMINISTRATION// 
        //*******************************//
        //In this section we set the control for multiple simultaneous ajax request
        //We cancel the ajax request executed when another one is in progress
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function (sender, args) {
            if (args.get_error() && args.get_error().name === 'Sys.WebForms.PageRequestManagerTimeoutException') {
                args.set_errorHandled(true);
            }
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_initializeRequest(initializeRequest);
        function initializeRequest(sender, args) {
            if (prm.get_isInAsyncPostBack()) {
                args.set_cancel(true);
                //this line was initially included but causes that the original postback being cancel too
                if (args.get_postBackElement().id != "ctl00_cntBody_btnDirectSearch" && args.get_postBackElement().id != "ctl00_cntBody_btnAdvancedSearch") {
                    ShowFooterAlert('<%=GetLocalResourceObject("msgWaitWhileProcessing")%>');
                }

                AddTemporaryClass($("#" + args.get_postBackElement().id), "btn-warning", 1500);

                setTimeout(function () {
                    $("#" + args.get_postBackElement().id).button('error');
                    setTimeout(function () {
                        $("#" + args.get_postBackElement().id).button('reset');
                    }, 4500)
                }, 100);
            }
        }

        function validateHourInput(txtBox, e, maxLength) {
            return isDecimalNumber(event) && checkMaxLength(this, event, maxLength) && !checkAlreadyDecimalIfSeparator(this, event);
        }
    </script>
</asp:Content>
