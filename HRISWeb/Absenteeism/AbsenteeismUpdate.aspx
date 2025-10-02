<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AbsenteeismUpdate.aspx.cs" Inherits="HRISWeb.Absenteeism.AbsenteeismUpdate" %>

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
    </style>

    <div class="main-content">
        <h1 class="text-left text-primary">
            <label class="PageTitle"><%= GetLocalResourceObject("lblScreenTitle") %></label>
        </h1>
        <br />

        <!-- 5266 - Paolo Aguilar -->
        <asp:UpdatePanel runat="server" ID="main" UpdateMode="Conditional">
            <Triggers>
            </Triggers>

            <ContentTemplate>
                <asp:Panel ID="pnlMainContent" runat="server">
                    <div class="container" style="width: 100%">
                        <h4 class="text-left text-primary"><%= GetLocalResourceObject("lblFiltersSubtitle") %></h4>

                        <button id="btnCleanFilters" type="button" runat="server" class="btn btn-default btnAjaxAction" 
                            onserverclick="BtnCleanFilters_Click" 
                            data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnClearFilters"))%>' 
                            data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnClearFilters"))%>'>
                            <span class="glyphicon glyphicon-erase glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%= GetLocalResourceObject("btnClearFilters") %>
                        </button>

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
                        <br />

                        <div class="row">
                            <div class="col-sm-3">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=cboPayroll.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblPayroll")%></label>
                                        </div>

                                        <div class="col-sm-7">
                                            <asp:DropDownList ID="cboPayroll" runat="server" CssClass="form-control selectpicker" data-live-search="true" data-selected-text-format="count > 1" multiple="multiple" data-actions-box="true" data-select-all-text='<%# GetLocalResourceObject("cboPayrollSelectAll") %>' data-deselect-all-text='<%# GetLocalResourceObject("cboPayrollDeselectAll") %>' AutoPostBack="false"></asp:DropDownList>

                                            <asp:HiddenField ID="hdfEmployeeSelectedPayroll" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-3">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=txtEmployeeIdFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblEmployeeIdFilter")%></label>
                                        </div>

                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtEmployeeIdFilter" CssClass="form-control control-validation cleanPasteDigits EnterKeyPressSearchCases" onkeypress="return isNumber(event);" runat="server" autocomplete="off" MaxLength="10" type="number" min="0"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-3">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=txtEmployeeNameFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblEmployeeNameFilter")%></label>
                                        </div>

                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtEmployeeNameFilter" CssClass="form-control EnterKeyPressSearchCases cleanPasteText" runat="server" autocomplete="off" MaxLength="250" type="text" ></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-3">
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

                            <div class="col-sm-3">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=dtpStartDateFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblSuspendedStartFilter")%></label>
                                        </div>

                                        <div class="col-sm-7">
                                            <asp:TextBox runat="server" ID="dtpStartDateFilter" class="form-control date control-validation cleanPasteDigits" placeholder='<%$ Code:GetLocalResourceObject("lblDateFormatPlaceHolder") %>' type="text" autocomplete="off" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-3">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=dtpEndDateFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblSuspendedEndFilter")%></label>
                                        </div>

                                        <div class="col-sm-7">
                                            <asp:TextBox runat="server" ID="dtpEndDateFilter" class="form-control date control-validation cleanPasteDigits" placeholder='<%$ Code:GetLocalResourceObject("lblDateFormatPlaceHolder") %>' type="text" autocomplete="off" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%= txtIdAbsenteeimFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblIdAbsenteeism")%></label>
                                        </div>

                                        <div class="col-sm-7">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtIdAbsenteeimFilter" CssClass="form-control control-validation cleanPasteDigits EnterKeyPressSearchCases" onkeypress="return isNumber(event);" runat="server" autocomplete="off" MaxLength="9" type="number" min="0"></asp:TextBox>

                                                <asp:HiddenField ID="hdftxtIdAbsenteeimFilter" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-3" id="divDescriptionFilter" runat="server" visible="false">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <asp:Label ID="lblDescriptionLabel" runat="server" CssClass="control-label" Font-Bold="true"><%=GetLocalResourceObject("lblAccidentCase")%></asp:Label>
                                        </div>

                                        <div class="col-sm-7">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtDescriptionFilter" CssClass="form-control control-validation EnterKeyPressSearchCases cleanPasteDigits" onkeypress="return true;" runat="server" autocomplete="off" MaxLength="15" type="text"></asp:TextBox>

                                                <asp:HiddenField ID="hdftxtDescriptionFilter" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-3">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=cboPageSize.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblPageSize")%></label>
                                        </div>

                                        <div class="col-sm-7">
                                            <asp:DropDownList ID="cboPageSize" CssClass="form-control control-validation" data-live-search="true" AutoPostBack="false" runat="server"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-3">
                                <div class="col-sm-12 text-center">
                                    <button id="btnSearchFilter" type="button" runat="server" class="btn btn-default btnAjaxAction" 
                                        onclick="ActivateModalProgress(true);" onserverclick="BtnSearchFilter_ServerClick" 
                                        data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>' 
                                        data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>'>
                                        <span class="glyphicon glyphicon-search glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%= GetLocalResourceObject("btnSearch") %>
                                    </button>

                                    <asp:Button ID="btnSearchFilterDefault" runat="server" Style="display: none;" />
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-12 text-center">
                                <asp:UpdatePanel runat="server" ID="uppSearchAbsenteeism" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                    <Triggers>
                                    </Triggers>

                                    <ContentTemplate>
                                        <asp:GridView ID="grvCausas" Width="100%" runat="server" EmptyDataText=''
                                            EmptyDataRowStyle-CssClass="emptyRow" AllowPaging="false" PagerSettings-Visible="false"
                                            AutoGenerateColumns="false" ShowHeader="true" CssClass="table table-striped table-hover table-borderless"
                                            DataKeyNames="AbsteeismId" AllowSorting="true" OnPreRender="GrvCausas_PreRender" OnDataBound="GrvCausas_DataBound"
                                            OnSorting="GrvCausas_Sorting" BorderStyle="None" GridLines="None">

                                            <Columns>
                                                <asp:TemplateField HeaderText="">
                                                    <HeaderTemplate>
                                                    </HeaderTemplate>

                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chbEmployeeFilter" runat="server" Checked='<%# Eval("IsSelected") %>' onclick="reviewOnlyOneCheck('ctl00_cntBody_grvCausas', this)" />
                                                        <asp:HiddenField ID="hdnAbsteeismId" runat="server" Value='<%# Eval("AbsteeismId") %>' />
                                                        <asp:HiddenField ID="hdnCauseFile" runat="server" Value='<%# Eval("CauseDescription") %>' />
                                                        <asp:HiddenField ID="hdnCompany" runat="server" Value='<%# Eval("compania") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                    <HeaderTemplate>
                                                        <div style="width: 100%; text-align: center;">
                                                            <asp:LinkButton ID="trabajadorSort" runat="server" CommandName="Sort" CommandArgument="trabajador">
                                                        <span><%= GetLocalResourceObject("lblEmployeeIdHeader") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvCausas.ClientID, "trabajador") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                            </asp:LinkButton>
                                                        </div>
                                                    </HeaderTemplate>

                                                    <ItemTemplate>
                                                        <span id="trabajador" data-id="trabajador" data-value="<%# Eval("trabajador") %>"><%# Eval("trabajador")%></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                    <HeaderTemplate>
                                                        <div style="width: 100%; text-align: center;">
                                                            <asp:LinkButton ID="EmployeeNameSort" runat="server" CommandName="Sort" CommandArgument="EmployeeName">
                                                        <span><%= GetLocalResourceObject("lblEmployeeNameHeader") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvCausas.ClientID, "EmployeeName") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
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
                                                            <asp:LinkButton ID="casoSort" runat="server" CommandName="Sort" CommandArgument="caso">
                                                        <span><%= GetLocalResourceObject("lblAdamCaseNumberHeader") %> </span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvCausas.ClientID, "caso") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                            </asp:LinkButton>
                                                        </div>
                                                    </HeaderTemplate>

                                                    <ItemTemplate>
                                                        <span id="AbsteeismId" data-id="AbsteeismId" data-value="<%# Eval("caso") %>"><%# Eval("caso")%></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                    <HeaderTemplate>
                                                        <div style="width: 100%; text-align: center;">
                                                            <asp:LinkButton ID="CauseDescriptionSort" runat="server" CommandName="Sort" CommandArgument="CauseDescription">
                                                        <span><%= GetLocalResourceObject("lblCauseHeader") %> </span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvCausas.ClientID, "CauseDescription") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                            </asp:LinkButton>
                                                        </div>
                                                    </HeaderTemplate>

                                                    <ItemTemplate>
                                                        <span id="CauseDescription" data-id="CauseDescription" data-value="<%# Eval("CauseDescription") %>"><%# Eval("CauseDescription")%></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                    <HeaderTemplate>
                                                        <div style="width: 100%; text-align: center;">
                                                            <asp:LinkButton ID="fecha_suspendeSort" runat="server" CommandName="Sort" CommandArgument="fecha_suspende">
                                                        <span><%= GetLocalResourceObject("lblSuspendedHeader") %> </span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvCausas.ClientID, "fecha_suspende") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                            </asp:LinkButton>
                                                        </div>
                                                    </HeaderTemplate>

                                                    <ItemTemplate>
                                                        <span id="fecha_suspende" data-id="fecha_suspende" data-value="<%# Eval("fecha_suspende") %>"><%# Eval("fecha_suspende")%></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                    <HeaderTemplate>
                                                        <div style="width: 100%; text-align: center;">
                                                            <asp:LinkButton ID="fecha_finalSort" runat="server" CommandName="Sort" CommandArgument="fecha_final">
                                                        <span><%= GetLocalResourceObject("lblEndDateHeader") %> </span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvCausas.ClientID, "fecha_final") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                            </asp:LinkButton>
                                                        </div>
                                                    </HeaderTemplate>

                                                    <ItemTemplate>
                                                        <span id="fecha_final" data-id="fecha_final" data-value="<%# Eval("fecha_final") %>"><%# Eval("fecha_final")%></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                    <HeaderTemplate>
                                                        <div style="width: 100%; text-align: center;">
                                                            <asp:LinkButton ID="StateDescriptionSort" runat="server" CommandName="Sort" CommandArgument="StateDescription">
                                                        <span><%= GetLocalResourceObject("lblStateHeader") %> </span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvCausas.ClientID, "StateDescription") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                            </asp:LinkButton>
                                                        </div>
                                                    </HeaderTemplate>

                                                    <ItemTemplate>
                                                        <span id="StateDescription" data-id="StateDescription" data-value="<%# Eval("StateDescription") %>"><%# Eval("StateDescription")%></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                    <HeaderTemplate>
                                                    </HeaderTemplate>

                                                    <ItemTemplate>
                                                        <asp:Panel ID="pnlFile" runat="server" Visible='<%# Eval("ShowFileButton") %>'>
                                                            <button id="btnFileButton" type="button" runat="server" class="btn btn-default btnAjaxAction" 
                                                                onclick="ShowModalFile(this);" onserverclick="BtnFileButton_ServerClick" 
                                                                data-loading-text='<%$ Code:String.Concat(GetLocalResourceObject("btnFile"), "&nbsp;<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>")%>' 
                                                                data-error-text='<%$ Code:String.Concat( GetLocalResourceObject("btnFile"), "&nbsp;<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>")%>'>
                                                                <span class="glyphicon glyphicon-file glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%= GetLocalResourceObject("btnFile") %>
                                                            </button>
                                                        </asp:Panel>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>

                                        <h4 class="text-left text-primary" id="htmlResultsSubtitle" runat="server"></h4>
                                        <br />

                                        <nav>
                                            <asp:BulletedList ID="blstPager" runat="server" CssClass="pagination" DisplayMode="LinkButton" OnClick="BlstPager_Click">
                                            </asp:BulletedList>
                                        </nav>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <!-- 5266 - Paolo Aguilar -->
                            </div>
                        </div>
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

                        <div class="row">
                            <div class="col-lg-12">
                                <h4 class="text-left text-primary"><%= GetLocalResourceObject("lblEmployeeSubtitle") %></h4>
                                <br />

                                <div class="col-sm-6">
                                    <div class="form-horizontal">
                                        <asp:HiddenField ID="hdnClaveUnica" runat="server" />
                                        <asp:HiddenField ID="hdnTrabajador" runat="server" />

                                        <div class="form-group">
                                            <div class="col-sm-4 text-left">
                                                <label for="<%=txtEmployeeName.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblEmployeeName")%></label>
                                            </div>

                                            <div class="col-sm-8">
                                                <asp:TextBox ID="txtEmployeeName" CssClass="form-control control-validation" runat="server" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-sm-6">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="col-sm-4 text-left">
                                                <label for="<%=txtCompany.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCompany")%></label>
                                            </div>

                                            <div class="col-sm-8">
                                                <asp:TextBox ID="txtCompany" CssClass="form-control control-validation" runat="server" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-12">
                                <h4 class="text-left text-primary"><%= GetLocalResourceObject("lblAbsenteeismSubtitle") %>
                                    <asp:Label ID="lblIdAbsenteeism" runat="server"></asp:Label>
                                </h4>
                                <br />

                                <asp:HiddenField ID="hdnAbsenteeismId" runat="server" />

                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <div class="col-sm-4 text-left">
                                                    <label for="<%=cboCause.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCause")%></label>
                                                </div>

                                                <div class="col-sm-8">
                                                    <asp:DropDownList ID="cboCause" CssClass="form-control cboAjaxAction control-validation" AutoPostBack="false" runat="server"></asp:DropDownList>

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
                                                    <asp:DropDownList ID="cboInterestGroups" CssClass="form-control control-validation" AutoPostBack="false" runat="server"></asp:DropDownList>

                                                    <span id="cboInterestGroupsWaiting" class='fa fa-spinner fa-spin waitingNotification' style="display: none; float: right; margin-right: 25px; margin-top: -23px; position: relative; z-index: 2;"></span>

                                                    <asp:HiddenField ID="hdfInterestGroupsValue" runat="server" />

                                                    <asp:HiddenField ID="hdfInterestGroupsText" runat="server" />

                                                    <label id="cboInterestGroupsValidation" for="<%= cboInterestGroups.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjInterestGroupValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                </div>
                                            </div>

                                            <div class="form-group" id="divAccidentCase" runat="server">
                                                <div class="col-sm-4 text-left">
                                                    <asp:Label ID="lblDescriptionFormLabel" runat="server" CssClass="control-label" Font-Bold="true"><%=GetLocalResourceObject("lblDescription")%></asp:Label>
                                                </div>
                                                <div class="col-sm-8">
                                                    <asp:TextBox ID="txtDescription" CssClass="form-control cleanPasteText control-validation" runat="server" onkeypress="return isNumberOrLetter(event) && checkMaxLength(this,event,250);" onkeyUp="return isNumberOrLetter(event) && checkMaxLength(this,event,250);" MaxLength="250"></asp:TextBox>
                                                    <label id="txtDescriptionValidation" for="<%= txtDescription.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjDescriptionValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                    <asp:HiddenField ID="hdfDescriptionValue" runat="server" />
                                                </div>
                                            </div>
                                            <!-- Incident datetime -->
                                        </div>
                                    </div>

                                    <div class="col-sm-6">
                                        <div class="form-horizontal">
                                            <div class="form-group">
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

                                                    <button id="btnSearchRelatedCase" type="button" runat="server" class="btn btn-default btnAjaxAction" 
                                                        data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>' 
                                                        data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>'>
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
                                                                            <div class="col-xs-2 col-sm-2 text-danger " style="cursor: pointer; text-align: center;"><%= GetLocalResourceObject("lblEmployeeNameHeader") %></div>
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
                                onserverclick="BtnNextToPage4_ServerClick" 
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
                                onserverclick="BtnPrevToPage3_ServerClick" 
                                data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnPrevious"))%>' 
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
                                        <div class="col-sm-1 text-left">
                                        </div>
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
                                                            <div class="col-xs-6 col-sm-6 text-primary sorter" style="cursor: pointer; text-align: center;" data-sort-attr="data-sort-name" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblEmployeeNameHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
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
                                                        <div class="data-sort-src" data-sort-id='<%# Eval("EmployeeCode") %>' data-sort-name='<%# Eval("EmployeeName") %>' data-sort-case='<%# Eval("CaseID") %>'>
                                                            <div class="col-xs-3 col-sm-3">
                                                                <span>
                                                                    <%# Eval("EmployeeCode") %>
                                                                </span>
                                                            </div>
                                                            <div class="col-xs-6 col-sm-6">
                                                                <span>
                                                                    <%# Eval("EmployeeName") %>
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

                        <div class="btn-group" role="group" aria-label="main-buttons">
                            <button id="btnDelete" type="button" runat="server" class="btn btn-default btnAjaxAction" 
                                onclick="if(!ProcessDelete(this.id)){return false;}" onserverclick="BtnDelete_ServerClick" 
                                data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnDelete"))%>' 
                                data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnDelete"))%>'>
                                <span class="glyphicon glyphicon-trash glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                                <br />
                                <%= GetLocalResourceObject("btnDelete") %>
                            </button>
                        </div>
                    </div>
                </div>

                <%--  Modal for Files value  --%>
                <div class="modal fade" id="FileDialog" tabindex="-1" role="dialog" aria-labelledby="FileDialogTitle" data-backdrop="static" data-keyboard="false">
                    <div class="modal-dialog modal-lg" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button id="btnFileClose" type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                <h3 class="modal-title text-primary"><%=Convert.ToString(GetLocalResourceObject("TitleFileDialog")) %></h3>
                            </div>

                            <asp:UpdatePanel runat="server" ID="uppDuplicatedDialog">
                                <Triggers>
                                </Triggers>
                                <ContentTemplate>
                                    <div class="modal-body">
                                        <div class="form-horizontal">
                                            <div id="divFileDialogText" runat="server"></div>

                                            <div class="row">
                                                <div class="col-lg-12">
                                                    <h4 class="text-left text-primary"><%= GetLocalResourceObject("lblEmployeeSubtitle") %></h4>
                                                    <br />

                                                    <div class="col-sm-6">
                                                        <div class="form-horizontal">
                                                            <div class="form-group">
                                                                <div class="col-sm-5 text-left">
                                                                    <label for="<%=txtModalEmployeeName.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblEmployeeName")%></label>
                                                                </div>

                                                                <div class="col-sm-7">
                                                                    <asp:TextBox ID="txtModalEmployeeName" CssClass="form-control control-validation" runat="server" ReadOnly="true"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                            <div class="form-group">
                                                                <div class="col-sm-5 text-left">
                                                                    <label for="<%=txtModalCause.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblModalCause")%></label>
                                                                </div>

                                                                <div class="col-sm-7">
                                                                    <asp:TextBox ID="txtModalCause" CssClass="form-control control-validation" runat="server" ReadOnly="true"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                            <div class="form-group">
                                                                <div class="col-sm-5 text-left">
                                                                    <label for="<%=txtModalAdamCase.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblIdAbsenteeism")%></label>
                                                                </div>

                                                                <div class="col-sm-7">
                                                                    <asp:TextBox ID="txtModalAdamCase" CssClass="form-control control-validation" runat="server" ReadOnly="true"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-sm-6">
                                                        <div class="form-horizontal">
                                                            <div class="form-group">
                                                                <div class="col-sm-4 text-left">
                                                                    <label for="<%=txtModalEmployeeId.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblEmployeeId")%></label>
                                                                </div>

                                                                <div class="col-sm-8">
                                                                    <asp:TextBox ID="txtModalEmployeeId" CssClass="form-control control-validation" runat="server" ReadOnly="true"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                            <div class="form-group">
                                                                <div class="col-sm-4 text-left">
                                                                    <label for="<%=txtModalCompany.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCompany")%></label>
                                                                </div>

                                                                <div class="col-sm-8">
                                                                    <asp:TextBox ID="txtModalCompany" CssClass="form-control control-validation" runat="server" ReadOnly="true"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                            <div class="form-group" id="divModalInsCase" runat="server">
                                                                <div class="col-sm-4 text-left">
                                                                    <asp:Label ID="lblModalInsCaseLabel" runat="server" CssClass="control-label" Font-Bold="true"><%=GetLocalResourceObject("lblModalDescription")%></asp:Label>
                                                                </div>

                                                                <div class="col-sm-8">
                                                                    <asp:TextBox ID="txtModalInsCase" CssClass="form-control control-validation" runat="server" ReadOnly="true"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <asp:HiddenField runat="server" ID="hdnIdFileToDelete" />
                                                <div class="col-lg-12">
                                                    <asp:Panel ID="pnlFileDialogDataDetail" runat="server">
                                                        <asp:Repeater ID="rptFiles" runat="server">
                                                            <HeaderTemplate>
                                                                <table id="tableAbsenteeismsFiles" class="table table-hover table-striped">
                                                                    <thead>
                                                                        <tr>
                                                                            <th>
                                                                                <div>
                                                                                    <div class="col-xs-3 col-sm-3 text-primary" style="cursor: pointer; text-align: center;"><%= GetLocalResourceObject("lblDocumentTypeHeader") %> </div>
                                                                                    <div class="col-xs-3 col-sm-3 text-primary" style="cursor: pointer; text-align: center;"><%= GetLocalResourceObject("lblFileNameHeader") %> </div>
                                                                                    <div class="col-xs-3 col-sm-3 text-primary" style="cursor: pointer; text-align: center;"><%= GetLocalResourceObject("lblDeleteFileHeader") %>  </div>
                                                                                    <div class="col-xs-3 col-sm-3 text-primary" style="cursor: pointer; text-align: center;"><%= GetLocalResourceObject("lblDownloadFileHeader") %>  </div>
                                                                                </div>
                                                                            </th>
                                                                        </tr>
                                                                    </thead>
                                                                    <tbody>
                                                            </HeaderTemplate>

                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td class="row-fluid">
                                                                        <asp:UpdatePanel runat="server" ID="uppFiles" UpdateMode="Conditional">
                                                                            <Triggers>
                                                                            </Triggers>
                                                                            <ContentTemplate>
                                                                                <div class="data-sort-src">
                                                                                    <div class="col-xs-3 col-sm-3">
                                                                                        <span>
                                                                                            <%# Eval("DocumentTypeDescription") %>
                                                                                        </span>
                                                                                    </div>

                                                                                    <div class="col-xs-3 col-sm-3">
                                                                                        <span>
                                                                                            <%# Eval("FileName") %>
                                                                                        </span>
                                                                                    </div>

                                                                                    <div class="col-xs-3 col-sm-3" style="text-align: center;">
                                                                                        <button id="btnDeleteFile" runat="server" type="button" class="btn btn-default btnAjaxAction" 
                                                                                            onclick='<%# DeleteFile(Eval("IdFile")) %>' onserverclick="BtnDeleteFile_ServerClick" 
                                                                                            data-loading-text='<%$ Code:String.Concat(GetLocalResourceObject("btnDeleteFile"), "&nbsp;<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>")%>' 
                                                                                            data-error-text='<%$ Code:String.Concat( GetLocalResourceObject("btnDeleteFile"), "&nbsp;<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>")%>'>
                                                                                            <span class="glyphicon glyphicon-trash glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                                                                                        </button>
                                                                                    </div>

                                                                                    <div class="col-xs-3 col-sm-3" style="text-align: center;">
                                                                                        <button id="btnDownloadFile" runat="server" type="button" class="btn btn-default btnAjaxAction" 
                                                                                            onclick='<%# DownloadFile(Eval("IdFile")) %>' 
                                                                                            data-loading-text='<%$ Code:String.Concat(GetLocalResourceObject("btnDownloadFile"), "&nbsp;<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>")%>' 
                                                                                            data-error-text='<%$ Code:String.Concat( GetLocalResourceObject("btnDownloadFile"), "&nbsp;<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>")%>'>
                                                                                            <span class="glyphicon glyphicon-download glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                                                                                        </button>
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
                                                        <hr />

                                                        <div class="form-horizontal">
                                                            <div class="form-group">
                                                                <div class="col-sm-2 text-left">
                                                                    <label for="<%=cboDocuments.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblDocumentType")%></label>
                                                                </div>

                                                                <div class="col-sm-10">
                                                                    <asp:DropDownList ID="cboDocumentsAbsenteeism" CssClass="form-control control-validation" runat="server" AutoPostBack="true" OnSelectedIndexChanged="CboDocumentsAbsenteeism_SelectedIndexChanged"></asp:DropDownList>
                                                                </div>
                                                            </div>

                                                            <div class="form-group">
                                                                <div class="col-sm-2 text-left">
                                                                    <label for="<%=fupAbsenteeismFile.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblFile")%></label>
                                                                </div>

                                                                <div class="col-sm-10">
                                                                    <asp:FileUpload ID="fupAbsenteeismFile" runat="server" data-input-type="archivo" CssClass="file" data-show-preview="false" data-show-upload="false" data-allowed-file-extensions='["pdf", "docx"]' />
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="form-group">
                                                            <div class="col-sm-2 text-left"></div>

                                                            <div class="col-sm-10 text-right">
                                                                <button id="btnAddFile" type="button" runat="server" class="btn btn-default btnAjaxAction" 
                                                                    onserverclick="BtnAddFile_ServerClick" 
                                                                    data-loading-text='<%$ Code:String.Concat(GetLocalResourceObject("btnAddFileValue"), "&nbsp;<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>")%>' 
                                                                    data-error-text='<%$ Code:String.Concat( GetLocalResourceObject("btnAddFileValue"), "&nbsp;<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>")%>'>
                                                                    <span class="glyphicon glyphicon-floppy-disk glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%= GetLocalResourceObject("btnAddFileValue") %>
                                                                </button>
                                                            </div>
                                                        </div>
                                                        <br />
                                                        <br />

                                                        <div class="row col-sm-12">
                                                            <div>
                                                                <asp:Label ID="lblInformationFile" runat="server"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="modal-footer">
                                        <button id="btnCloseModal" type="button" class="btn btn-default">
                                            <span class="glyphicon glyphicon-remove glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnClose")) %>
                                        </button>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
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
        $(document).ready(function () {
            //Se utiliza para eliminar el enter en los campos e texto, select,textsrea
            $(function () {
                var keyStop = {
                    13: "input:text, input:file, input:password,input:checkbox,input:radio, select", // stop enter = submit
                    end: null
                };

                $(document).bind("keydown", function (event) {
                    var selector = keyStop[event.which];

                    if (selector !== undefined && $(event.target).is(selector)) {
                        event.preventDefault(); //stop event
                    }
                    return true;
                });
            });
        });

        function pageLoad(sender, args) {
            //////////////////////////////////////////////////////////////////////////////////////////////////

            //In this section we set the button state for btnAjaxAction
            $('.btnAjaxAction').on('click', function () {
                var $this = $(this);
                $this.button('loading');
                setTimeout(function () { $this.button('reset'); }, 30000);
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
            

            $('#<%= txtDescriptionFilter.ClientID %>').on('keyup keypress', function (e) {
                return isNumberOrLetter(e);
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

            $('#btnFileClose, #btnCloseModal').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                event.preventDefault();
                $('#FileDialog').modal('hide');
            });

            $(".EnterKeyPressSearchEmployees").on('keyup keypress', function (e) {
                var keyCode = e.keyCode || e.which;
                if (keyCode === 13) {
                    $("#ctl00_cntBody_btnSearchEmployeesFilter").click();
                }
            });

            $(".EnterKeyPressSearchCases").on('keyup keypress', function (e) {
                var keyCode = e.keyCode || e.which;
                if (keyCode === 13) {
                    $("#ctl00_cntBody_btnSearchFilter").click();
                }
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

            $('#<%= dtpFinalDate.ClientID %>').datetimepicker({
                format: 'MM/DD/YYYY'
            });

            var dateDtpFinal = $('#<%= dtpFinalDate.ClientID %>').val();
            if (dateDtpFinal != undefined && dateDtpFinal != '') {
                $('#<%= dtpFinalDate.ClientID %>').data("DateTimePicker").show()
                $('#<%= dtpFinalDate.ClientID %>').data("DateTimePicker").hide()
            }

            $('#<%= dtpStartDateFilter.ClientID %>').datetimepicker({
                format: 'MM/DD/YYYY'
            });

            $('#<%= dtpEndDateFilter.ClientID %>').datetimepicker({
                format: 'MM/DD/YYYY'
            });

            $('#<%= dtpIncidentDate.ClientID %>').on("blur", function () {
                var ValidDateVal = $(this).data("DateTimePicker").date().format('MM/DD/YYYY');
                $(this).val(ValidDateVal);
            });

            $('#<%= tpcIncidentDateTime.ClientID %>').on("blur", function () {
                var ValidDateVal = $(this).data("DateTimePicker").date().format('HH:mm');
                $(this).val(ValidDateVal);
            });

            $('#<%= dtpSuspendWorkDate.ClientID %>').on("blur", function () {
                var ValidDateVal = $(this).data("DateTimePicker").date().format('MM/DD/YYYY');
                $(this).val(ValidDateVal);
            });

            $('#<%= tpcSuspendWorkTime.ClientID %>').on("blur", function () {
                var ValidDateVal = $(this).data("DateTimePicker").date().format('HH:mm');
                $(this).val(ValidDateVal);
            });

            $('#<%= dtpFinalDate.ClientID %>').on("blur", function () {
                var ValidDateVal = $(this).data("DateTimePicker").date().format('MM/DD/YYYY');
                $(this).val(ValidDateVal);
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

            $("#txtSearchEmployees").on("keyup", function () {
                var value = $(this).val().toLowerCase();
                $("#tableAddParticipants tbody tr").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });

            $('#<%= cboTimeUnit.ClientID %>').on("change", function () {
                SetDaysHoursSelection();
            });

            $('#<%= cboCaseState.ClientID %>').on("change", function () {
                SetRelatedCaseVisibility();
            });

            $('#<%= dtpSuspendWorkDate.ClientID %>').on("blur", function () {
                ReviewFinalDate($('#<%= dtpSuspendWorkDate.ClientID %>'));
            });

            $('#<%= dtpFinalDate.ClientID %>').on("blur", function () {
                
                ReviewFinalDate($('#<%= dtpFinalDate.ClientID %>'));
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

            $('#selectAll').click(function (e) {
                var table = $(e.target).closest('table');
                $('td input:checkbox', table).prop('checked', this.checked);
            });

            $(".file").fileinput({
                'showUpload': true,
                'showCancel': false,
                'showRemove': false,
                'allowedFileExtensions': ['pdf', 'docx', 'doc', 'xlsx', 'xls', 'jpeg', 'jpg', 'txt', 'png', 'csv', 'ppt', 'pptx', 'xlsm'],
                'hiddenThumbnailContent': false,
                'showPreview': false,
                'showUploadedThumbs': false,
                'uploadAsync': false,
                'showBrowse': true,
                'showCaption': true,
                browseOnZoneClick: true,
                'initialPreviewAsData': false,
                'maxFileCount': 1,
                'language': 'es'
            })
                .on("click", function (event) {
                    var documentType = $('#<%= cboDocumentsAbsenteeism.ClientID %>').val();

                    if (documentType == "") {
                        MostrarMensaje(4,'<%= GetLocalResourceObject("msjDocumentTypeValidation") %>', null);
                        return false;
                    }

                    return true;

                })
                .on("filebatchselected", function (event, files) {
                    var documentType = $('#<%= cboDocumentsAbsenteeism.ClientID %>').val();
                    var company = $('#<%= txtModalCompany.ClientID %>').val();
                    var adamCase = $('#<%= txtModalAdamCase.ClientID %>').val();
                    var fileUpload = ($(this)).get(0);
                    var files = fileUpload.files;

                    var data = new FormData();
                    for (var i = 0; i < files.length; i++) {
                        data.append(files[i].name, files[i]);
                    }

                    data.append('company', company);
                    data.append('adamCase', adamCase);

                    if (documentType != "") {
                        $.ajax({
                            url: "/Shared/FileUploadHandler.ashx",
                            type: "POST",
                            data: data,
                            contentType: false,
                            processData: false,
                            success: function (result) { },
                            error: function (err) {
                                showClientMessage(MessageType.Error, err.responseText, null);
                            }
                        });
                    }
                })
                .on('fileclear', function () {
                    var documentType = $('#<%= cboDocumentsAbsenteeism.ClientID %>').val();
                    var company = $('#<%= txtModalCompany.ClientID %>').val();
                    var adamCase = $('#<%= txtModalAdamCase.ClientID %>').val();
                    var rutaArchivo = ($(this)).val();
                    var data = new FormData();

                    data.append('company', company);
                    data.append('adamCase', adamCase);

                    $.ajax({
                        url: "/Shared/FileUploadHandler.ashx",
                        type: "POST",
                        data: data,
                        contentType: false,
                        processData: false,
                        success: function (result) { },
                        error: function (err) {
                            showClientMessage(MessageType.Error, err.responseText, null);
                        }
                    });
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

            $("#<%= cboPayroll.ClientID %>").on('changed.bs.select', function (e, clickedIndex, isSelected, previousValue) {
                /// <summary>Handles the select's value changed event for the cboLanguages control.</summary>
                MultiSelectDropdownListSaveSelectedItems($("#<%= cboPayroll.ClientID %>"), $("#<%= hdfEmployeeSelectedPayroll.ClientID %>"));
                console.log($("#<%= cboPayroll.ClientID %>")[0].selectedOptions);
                console.log($("#<%= cboPayroll.ClientID %>").val());
                console.log($("#<%= cboPayroll.ClientID %>").selectpicker("val"));
                console.log($("#<%= hdfEmployeeSelectedPayroll.ClientID %>").val());
            });

            $("#<%= btnCleanFilters.ClientID %>").click(function () {
                $('#<%= cboPayroll.ClientID %>').selectpicker("val", "");
                $('#<%= cboPayroll.ClientID %>').val("");
                $('#<%= cboPayroll.ClientID %>')[0].selectedOptions = [];
                $('#<%= cboPayroll.ClientID %>').selectpicker("refresh");
                $('#<%= cboPayroll.ClientID %> option:selected').removeAttr('selected');

                MultiSelectDropdownListSaveSelectedItems($("#<%= cboPayroll.ClientID %>"), $("#<%= hdfEmployeeSelectedPayroll.ClientID %>"));
                MultiSelectDropdownListRestoreSelectedItems($('#<%= cboPayroll.ClientID %>'), $('#<%= hdfEmployeeSelectedPayroll.ClientID %>'));
            });

            if ($('#<%= cboPayroll.ClientID %>').is(":visible")) {

                MultiSelectDropdownListRestoreSelectedItems($('#<%= cboPayroll.ClientID %>'), $('#<%= hdfEmployeeSelectedPayroll.ClientID %>'));
            }

            SetRelatedCaseVisibility();
            SetDocumentDescriptionState();
            SetSaveActionVisibility();
            SetHoursRequired();
            SetNextActionVisibility();
            SetDaysHoursSelection();
        }

        function DeleteFile(btnId, idFile) {
            setTimeout(function () {
                $("#" + btnId.id).button('reset');
            }, 500);

            $('#<%= hdnIdFileToDelete.ClientID %>').val(idFile);
        }

        function RestoreFilter() {
            MultiSelectDropdownListRestoreSelectedItems($('#<%= cboPayroll.ClientID %>'), $('#<%= hdfEmployeeSelectedPayroll.ClientID %>'));
        }

        function DownloadFile(btnId, idFile) {
            setTimeout(function () {
                $("#" + btnId.id).button('reset');
            }, 500);

            DescargarArchivoAsync('<%=(Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath)%>', idFile);
        }

        function DescargarArchivoAsync(rutaBase, idFile) {
            /// <summary>Descarga un archivo abriendo otra ventana del navegador y haciendo una solicitud a un manejador genérico.</summary>
            var url = rutaBase + '/Shared/FileDownloadHandler.ashx?idAbsenteeismFile=' + idFile;

            var xhr = new XMLHttpRequest();
            xhr.open('POST', url, true);
            xhr.responseType = 'arraybuffer';

            xhr.onload = function () {
                if (this.status === 200) {
                    var filename = "";
                    var disposition = xhr.getResponseHeader('Content-Disposition');
                    if (disposition && disposition.indexOf('attachment') !== -1) {
                        var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                        var matches = filenameRegex.exec(disposition);
                        if (matches != null && matches[1]) filename = matches[1].replace(/['"]/g, '');
                    }

                    var type = xhr.getResponseHeader('Content-Type');

                    var blob = typeof File === 'function'
                        ? new File([this.response], filename, { type: type })
                        : new Blob([this.response], { type: type });

                    if (typeof window.navigator.msSaveOrOpenBlob !== 'undefined') {
                        // IE workaround for "HTML7007: One or more blob URLs were revoked by closing the blob for which they were created. These URLs will no longer resolve as the data backing the URL has been freed."
                        window.navigator.msSaveOrOpenBlob(blob, filename); //msSaveOrOpenBlob or msSaveBlob
                    } else {
                        var URL = window.URL || window.webkitURL;
                        var downloadUrl = URL.createObjectURL(blob);

                        if (filename) {
                            // use HTML5 a[download] attribute to specify filename
                            var a = document.createElement("a");
                            // safari doesn't support this yet
                            if (typeof a.download === 'undefined') {
                                window.location = downloadUrl;
                            } else {
                                a.href = downloadUrl;
                                a.download = filename;
                                document.body.appendChild(a);
                                a.click();
                            }
                        } else {
                            window.location = downloadUrl;
                        }
                        setTimeout(function () { URL.revokeObjectURL(downloadUrl); }, 100); // cleanup
                    }
                }
            };

            xhr.setRequestHeader('Content-type', 'application/x-www-form-urlencoded');
            xhr.send();
        }

        function ShowModalFile(btnId) {
            setTimeout(function () {
                $("#" + btnId.id).button('reset');
            }, 500);

            var count = $('#ctl00_cntBody_grvCausas tbody td input:checkbox').length;
            var checkedCount = 0;
            var i = 0;
            $('#ctl00_cntBody_grvCausas tbody td input:checkbox').each(function () {
                $(this).prop('checked', false);
            });

            $($('#' + btnId.id).closest('tr')[0]).find('[type=checkbox]').prop('checked', true);

            $('#FileDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceAdd") %>');

            $('#FileDialog').modal('show');

            return false;
        }

        function ReturnFromGridDataBound() {

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

                //declare the validator
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

            //get the results

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
                //add custom validation methods
                jQuery.validator.addMethod("validSelection", function (value, element) {
                    return this.optional(element) || value != "-1";
                }, "Please select a valid option");

                //declare the validator
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

            //get the results

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
            var count = $('#' + table + ' tbody tr td input:checkbox').length;
            var checkedCount = 0;
            var i = 0;

            $('#' + table + ' tbody tr td input:checkbox').each(function () {
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
            }

            else if (!($('#<%= pnlDocumentsContent.ClientID %>').is(":hidden")) && false) {
                //there is nos validation here
            }

            else {
                return ShowConfirmationMessageSave(resetId);
            }
        }

        function ProcessDelete(resetId) {
            /// <summary>Process the save request according to the validation</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>

            return ShowConfirmationMessageDelete(resetId);
        }

        function ProcessCloseDate(check) {
            /// <summary>Process the save request according to the validation</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            return ShowConfirmationMessageCloseDate(check);
        }

        function ValidateDataForDates() {
            /// <summary>Validate the dates of suspension and final are set</summary>            
            /// <returns> True if is valid. False otherwise. </returns>

            // if both days are setting
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

        function ReviewFinalDate(changedControl) {
            
            /// <summary>Manage the logic between suspend date and natural days and also re calculate days for final date</summary>
            if ($('#<%= dtpSuspendWorkDate.ClientID %>').val() != ""
                && $('#<%= cboTimeUnit.ClientID %>').val() != "Days"
                && $('#<%= dtpFinalDate.ClientID %>').val() == "") {
                $('#<%= dtpFinalDate.ClientID %>').val($('#<%= dtpSuspendWorkDate.ClientID %>').val());

            } else if ($('#<%= dtpSuspendWorkDate.ClientID %>').val() != ""
                && $('#<%= cboTimeUnit.ClientID %>').val() != "Days"
                && $('#<%= dtpFinalDate.ClientID %>').val() != "") {
                $('#<%= dtpFinalDate.ClientID %>').val($('#<%= dtpSuspendWorkDate.ClientID %>').val());

            } else if ($('#<%= dtpSuspendWorkDate.ClientID %>').val() != ""
                && $('#<%= cboTimeUnit.ClientID %>').val() != "Days"
                && $('#<%= dtpFinalDate.ClientID %>').val() != ""
                && $('#<%= dtpSuspendWorkDate.ClientID %>').val() != $('#<%= dtpFinalDate.ClientID %>').val()) {
                $('#<%= dtpFinalDate.ClientID %>').val($('#<%= dtpSuspendWorkDate.ClientID %>').val());
            }

            // if both days are setting
            if (ValidateDataForDates()) {
                // Recalculate it
                var suspendWorkDate = $('#<%= dtpSuspendWorkDate.ClientID %>').data("DateTimePicker").date().startOf('day');
                        var finalDate = $('#<%= dtpFinalDate.ClientID %>').data("DateTimePicker").date().startOf('day');
                        if ($('#<%= cboTimeUnit.ClientID %>').val() != "Days") {
                            finalDate = suspendWorkDate;
                        }
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
   
                    if ($('#<%= dtpSuspendWorkDate.ClientID %>').val() != ""
                        && $('#<%= cboTimeUnit.ClientID %>').val() == "Days"
                        && $('#<%= dtpFinalDate.ClientID %>').val() != ""
                        && $('#<%= txtNaturalDays.ClientID %>').val() == "") {
                        var suspendWorkDate = $('#<%= dtpSuspendWorkDate.ClientID %>').data("DateTimePicker").date().startOf('day');
                        var finalDate = $('#<%= dtpFinalDate.ClientID %>').data("DateTimePicker").date().startOf('day');

                        var difference = finalDate.diff(suspendWorkDate, 'days')
                        difference = difference + 1;
                        $('#<%= txtNaturalDays.ClientID %>').val(difference);
                    }
                }

                else {
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

        function SetRelatedCaseVisibility() {
            /// <summary>Manage the logic for the related case visibility</summary>
            //this validation is duplicated un code behind btnSave_ServerClick event
            if ($('#<%= txtRelatedCase.ClientID %>').val() != "0") {
                $('#<%= dtpIncidentDate.ClientID %>').attr('readonly', true);
                $('#<%= tpcIncidentDateTime.ClientID %>').attr('readonly', true);

                $('#<%= btnSearchRelatedCase.ClientID %>').removeAttr("disabled");
                $('#<%= txtDescription.ClientID %>').attr('readonly', true);
            }
            else {
                if (validator != null) {
                    validator.resetForm();
                }

                $('#<%= btnSearchRelatedCase.ClientID %>').attr("disabled", true);
                $('#<%= txtRelatedCase.ClientID %>').val("");

                $('#<%= txtDescription.ClientID %>').attr('readonly', false);

                $('#<%= dtpIncidentDate.ClientID %>').attr('readonly', false);
                $('#<%= tpcIncidentDateTime.ClientID %>').attr('readonly', false);
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
                '<%= GetLocalResourceObject("msjConfirmationSave") %>',
                '<%=GetLocalResourceObject("Yes")%>', function () {
                    ActivateModalProgress(true);
                    __doPostBack('<%= btnSave.UniqueID %>', '')
            },
                '<%=GetLocalResourceObject("No")%>', function () {
                    $("#" + resetId).button('reset');
                }
            );

            return false;
        }

        function ShowConfirmationMessageDelete(resetId) {
            /// <summary>Show confirmation message for Delete funtionality</summary>
            /// <param name="resetId" type="String">Id of the button to reset if cancel</param>
            /// <returns>False</returns>
            MostrarConfirmacion(
                '<%= GetLocalResourceObject("msjConfirmationDelete") %>', '<%=GetLocalResourceObject("Yes")%>', function () {
                    __doPostBack('<%= btnDelete.UniqueID %>', '')
                }, '<%=GetLocalResourceObject("No")%>', function () {
                    $("#" + resetId).button('reset');
                }
            );

            return false;
        }

        function ShowConfirmationMessageCloseDate(check) {
            /// <summary>Show confirmation message for Delete funtionality</summary>
            /// <param name="resetId" type="String">Id of the button to reset if cancel</param>
            /// <returns>False</returns>
            if ($('#' + check).prop('checked')) {
                MostrarConfirmacion(
                    '<%= GetLocalResourceObject("msjConfirmationCloseDate") %>', '<%=GetLocalResourceObject("Yes")%>', function () {
                        $('#' + check).prop('checked', true);
                },
                    '<%=GetLocalResourceObject("No")%>', function () {
                        $('#' + check).prop('checked', false);
                    }
                );
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
  
                ShowFooterAlert('<%=GetLocalResourceObject("msgWaitWhileProcessing")%>');

                AddTemporaryClass($("#" + args.get_postBackElement().id), "btn-warning", 1500);

                setTimeout(function () {
                    $("#" + args.get_postBackElement().id).button('error');
                    setTimeout(function () {
                        $("#" + args.get_postBackElement().id).button('reset');
                    }, 4500)
                }, 100);
            }
        }
    </script>
</asp:Content>
