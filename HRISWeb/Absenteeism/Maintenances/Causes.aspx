<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Causes.aspx.cs" Inherits="HRISWeb.Training.Causes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntHeader" runat="server">
    <title><%=Convert.ToString(GetLocalResourceObject("lblScreenTitle")) %></title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cntBody" runat="server">

    <div class="main-content">

        <asp:Panel ID="pnlMainContent" runat="server" DefaultButton="btnSearchDefault" >

            <h1 class="text-left text-primary">
                <label class="PageTitle"><%= GetLocalResourceObject("lblScreenTitle") %></label>
            </h1><br />

            <asp:UpdatePanel runat="server" ID="main">
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnDownloadFile" />
                </Triggers>
                <ContentTemplate>

                    <div class="container" style="width:100%">
                    
                        <h4 class="text-left text-primary"><%= GetLocalResourceObject("lblFiltersSubtitle") %></h4>                
                        <br /> 
                                                    
                        <div class="row">
                            <div class="col-sm-4">
                                <div class="form-horizontal">                                
                                    <div class="form-group">                                
                                        <div class="col-sm-5 text-left">
                                            <label for="<%= txtCauseCodeFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCauseCode")%></label>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtCauseCodeFilter" CssClass="form-control control-validation cleanPasteText ignoreValidation" runat="server" onkeypress="return isNumberOrLetter(event)" MaxLength="50" autocomplete="off" type="text"></asp:TextBox>
                                            <asp:HiddenField ID="hdfCauseCodeFilter" runat="server" />
                                        </div>
                                    </div>                   
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="form-horizontal">
                                    <div class="form-group">                                
                                        <div class="col-sm-5 text-left">
                                            <label for="<%= txtCauseNameFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCauseName")%></label>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtCauseNameFilter" CssClass="form-control control-validation cleanPasteText ignoreValidation" runat="server" onkeypress="return isNumberOrLetter(event)" MaxLength="500" autocomplete="off" type="text"></asp:TextBox>
                                            <asp:HiddenField ID="hdfCauseNameFilter" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%=cboCauseCategoryFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCauseCategory")%></label>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:DropDownList ID="cboCauseCategoryFilter" CssClass="form-control cboAjaxAction control-validation ignoreValidation" AutoPostBack="false" runat="server" ></asp:DropDownList>                                        
                                            <asp:HiddenField ID="hdfCauseCategoryFilterValueFilter" runat="server" />
                                            <asp:HiddenField ID="hdfCauseCategoryFilterTextFilter" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        
                        </div>
                        <div class="row">
                            <div class="col-sm-12 text-center">
                                <button id="btnSearch" type="button" runat="server" class="btn btn-default btnAjaxAction" onserverclick="btnSearch_ServerClick"  data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>' >
                                    <span class="glyphicon glyphicon-search glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%= GetLocalResourceObject("btnSearch") %>
                                </button>
                                <asp:Button ID="btnSearchDefault" runat="server" OnClick="btnSearch_ServerClick" style="display:none;"/>
                            </div>
                        </div>
                                                
                        <br />

                        <div class="row">
                        
                            <div class="col-sm-12 text-center">

                                <asp:HiddenField runat="server" ID="hdfSelectedRowIndex" Value="-1"/>
                                <div>
                                    <asp:GridView ID="grvList" 
                                        Width="100%" 
                                        runat="server"                                 
                                        EmptyDataText='<%$ Code:GetLocalResourceObject("lblNoData") %>' EmptyDataRowStyle-CssClass="emptyRow"
                                        AllowPaging = "false" PagerSettings-Visible= "false" AllowSorting= "true"
                                        AutoGenerateColumns="false" ShowHeader="true"
                                        CssClass="table table-striped table-hover table-bordered"
                                        DataKeyNames="CauseCode,CauseName "
                                        OnPreRender="grvList_PreRender" OnSorting="grvList_Sorting">
                             
                                        <Columns>                                        
                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" >
                                                <HeaderTemplate>
                                                    <div style="width:100%; text-align:center;">
                                                        <asp:LinkButton ID="CauseCodeSort" runat="server" CommandName="Sort" CommandArgument="CauseCode" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("CauseCode.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "CauseCode") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <span id="dvCauseCode" data-id="CauseCode" data-value="<%# Eval("CauseCode") %>"><%# Eval("CauseCode") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField> 
                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" >
                                                <HeaderTemplate>
                                                    <div style="width:100%; text-align:center;">
                                                        <asp:LinkButton ID="CauseNameSort" runat="server" CommandName="Sort" CommandArgument="CauseName" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("CauseName.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "CauseName") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <span id="dvCauseName" data-id="CauseName" data-value="<%# Eval("CauseName") %>"><%# Eval("CauseName") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>                                                                                       
                                            <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" >
                                                <HeaderTemplate>
                                                    <div style="width:100%; text-align:center;">
                                                        <asp:LinkButton ID="CauseCategoryNameSort" runat="server" CommandName="Sort" CommandArgument="CauseCategoryName" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("CauseCategoryName.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "CauseCategoryName") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <span id="dvCauseCategoryName" data-id="CauseCategoryName" data-value="<%# DataBinder.Eval(Container.DataItem, "Category.CauseCategoryName") %>"><%# DataBinder.Eval(Container.DataItem, "Category.CauseCategoryName") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField> 
                                        </Columns>
                                    </asp:GridView>           
                                    <div id="grvListWaiting" style="position: absolute; margin: auto; display: none; bottom: 0px; top: 0px; left: 50%;transform: translate(-50%, 35%);">
                                        <span class='fa fa-spinner fa-spin' style="font-size:50px;"></span>
                                    </div>                                
                                </div>
                                <h4 class="text-left text-primary" id="htmlResultsSubtitle" runat="server"></h4>                                
                                <nav>
                                    <asp:BulletedList ID="blstPager" runat="server" CssClass="pagination" DisplayMode="LinkButton" OnClick="blstPager_Click" >
                                    </asp:BulletedList>
                                </nav>
                            

                            </div>
                        </div>
                    </div>                                
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>

        <div class="ButtonsActions">
            <asp:UpdatePanel ID="uppActions" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                <Triggers>
                    
                </Triggers>
                <ContentTemplate>
                    <div class="btn-group" role="group" aria-label="main-buttons">
                        <button id="btnAdd" type="button" runat="server" class="btn btn-default btnAjaxAction btndisabled" onclick="return false;" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnAdd"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnAdd"))%>' >
                            <span class="glyphicon glyphicon-plus glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span><br /><%= GetLocalResourceObject("btnAdd") %>
                        </button>

                        <button id="btnEdit" type="button" runat="server" class="btn btn-default btnAjaxAction btndisabled"  onserverclick="btnEdit_ServerClick" onclick="return ProcessEditRequest(this.id);" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnEdit"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnEdit"))%>' >
                            <span class="glyphicon glyphicon-edit glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span><br /><%= GetLocalResourceObject("btnEdit") %>
                        </button>

                        <button id="btnDelete" type="button" runat="server" class="btn btn-default btnAjaxAction btndisabled" onserverclick="btnDelete_ServerClick"  onclick="return ProcessDeleteRequest(this.id);" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnDelete"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnDelete"))%>' >
                            <span class="glyphicon glyphicon-trash glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span><br /><%= GetLocalResourceObject("btnDelete") %>
                        </button>
                        
                        <button id="btnExport" type="button" runat="server" class="btn btn-default btnAjaxAction" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnExportFile"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnExportFile"))%>' >
                            <span class="glyphicon glyphicon-export glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span><br /><%= GetLocalResourceObject("btnExportFile") %>
                        </button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
       
    </div>

    <%--  Modal for Add and Edit  --%>
    <div class="modal fade" id="MaintenanceDialog" tabindex="-1" role="dialog" aria-labelledby="MaintenanceDialogTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" id="btnClose" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title text-primary" id="dialogTitle"></h3>
                </div>
                <asp:UpdatePanel runat="server" ID="uppDialogControl">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="form-horizontal">

                                <asp:HiddenField ID="hdfCauseCodeEdit" runat="server" Value="" />                                
                                
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtCauseCode.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCauseCode")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtCauseCode" CssClass="form-control cleanPasteText" runat="server" onkeypress="return isNumberOrLetter(event)" MaxLength="50"></asp:TextBox>
                                        <label id="txtCauseCodeValidation" for="<%= txtCauseCode.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjCauseCodeValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtCauseName.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCauseName")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtCauseName" CssClass="form-control cleanPasteText" runat="server" onkeypress="return isNumberOrLetter(event) && checkMaxLength(this,event,500);" onkeyUp="return isNumberOrLetter(event) && checkMaxLength(this,event,500);" MaxLength="500" TextMode="MultiLine" Columns="3"></asp:TextBox>
                                        <label id="txtCauseNameValidation" for="<%= txtCauseName.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjCauseNameValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%=cboCauseCategory.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCauseCategory")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="cboCauseCategory" CssClass="form-control cboAjaxAction control-validation" AutoPostBack="false" runat="server" ></asp:DropDownList>
                                        <label id="cboCauseCategoryValidation" for="<%= cboCauseCategory.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjCauseCategoryValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtComments.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblComments")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtComments" CssClass="form-control cleanPasteText" runat="server" onkeypress="return isNumberOrLetter(event) && checkMaxLength(this,event,1000);" onkeyUp="return isNumberOrLetter(event) && checkMaxLength(this,event,1000);" MaxLength="1000" TextMode="MultiLine" Columns="3"></asp:TextBox>
                                        <label id="txtCommentsValidation" for="<%= txtComments.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjCommentsValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                    </div>
                                </div>                               
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= chbSearchEnabled.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblSearchEnabled")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:CheckBox ID="chbSearchEnabled" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                    </div>
                                </div>
                            </div>

                            <label class="control-label"><%=GetLocalResourceObject("lblCauseByDivision")%></label>

                            <div class="form-horizontal">

                                <asp:Repeater ID="rptCauseByDivision" runat="server" OnItemDataBound="rptCauseByDivision_ItemDataBound">
                                    <HeaderTemplate>
                                        <table id="tableCauseByDivision" class="table table-hover table-striped">
                                            <thead>
                                                <tr>                                            
                                                    <th>
                                                        <div>
                                                            <div class="col-xs-2 col-sm-2 text-primary sorter" style="cursor:pointer;white-space:nowrap;" data-sort-attr="data-sort-id" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblDivisionEnabledHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                            <div class="col-xs-2 col-sm-2 text-primary sorter" style="cursor:pointer;white-space:nowrap;" data-sort-attr="data-sort-name" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblDivisionNameHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                            <div class="col-xs-2 col-sm-2 text-primary sorter" style="cursor:pointer;white-space:nowrap;" data-sort-attr="data-sort-mapping" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblDivisionMappingHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                            <div class="col-xs-2 col-sm-2 text-primary sorter" style="cursor:pointer;white-space:nowrap;" data-sort-attr="data-sort-mapping" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblInterestGroupHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                            <div class="col-xs-1 col-sm-2 text-primary sorter" style="cursor:pointer;white-space:nowrap;" data-sort-attr="data-sort-additionalinforequired" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("lblDivisionAdditionalInfoRequiredHeader") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                            <div class="col-xs-1 col-sm-1 text-primary sorter" style="cursor:pointer;white-space:nowrap;" data-sort-attr="data-sort-Hours" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("LblHoursCauses") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                            <div class="col-xs-1 col-sm-1 text-primary sorter" style="cursor:pointer;white-space:nowrap;" data-sort-attr="data-sort-Days" data-sort-type="string" data-sort-direction=""><%= GetLocalResourceObject("LblDaysCauses") %> <i class="fa fa-sort sorterDirection" aria-hidden="true"></i></div>
                                                        </div>
                                                    </th>                                            
                                                </tr>
                                            </thead>
                                            <tbody >  
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>     
                                            <td class="row-fluid">
                                                 
                                                <div class="data-sort-src" data-sort-id='<%# Eval("Enabled") %>' data-sort-name='<%# Eval("Division") %>' data-sort-mapping='<%# Eval("Mapping") %>' data-sort-additionalinforequired='<%# Eval("AdditionalInfo") %>'>
                                                    <div class="col-xs-2 col-sm-2" style="text-align:center;">
                                                        <span> 
                                                            <asp:CheckBox ID="chbEnabled" runat="server" CssClass="<%# setCssCheckButton( Container.ItemIndex)  %>" Checked='<%# Eval("Enabled") %>' />  
                                                        </span> 
                                                    </div>
                                                    <div class="col-xs-2 col-sm-2">                                                        
                                                        <asp:HiddenField ID="hdfGeographicDivisionCode" runat="server" Value='<%# Eval("GeographicDivision") %>' />  
                                                        <asp:HiddenField ID="hdfDivisionCode" runat="server" Value='<%# Eval("Division") %>' />  
                                                        <asp:HiddenField ID="hdfDivisionName" runat="server" Value='<%# Eval("DivisionName") %>' />
                                                        <span> 
                                                            <%# Eval("DivisionName") %>                                                            
                                                        </span> 
                                                    </div> 
                                                    <div class="col-xs-2 col-sm-2">
                                                        <span>
                                                            <asp:HiddenField ID="hdfCodeMapSelected" runat="server" Value='<%# Eval("Mapping") %>' /> 
                                                            <asp:DropDownList ID="ddlCausesLocal" CssClass="form-control cboAjaxAction control-validation ddlCausesLocal" AutoPostBack="false" runat="server" ></asp:DropDownList>
                                                            <label id="txtMappingValidation" class="label label-danger label-validation labelMappingValidation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjCauseMappingValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                        </span> 
                                                    </div>
                                                    <div class="col-xs-2 col-sm-2">
                                                        <span>
                                                            <asp:DropDownList ID="ddlInterestGroupCode" CssClass="form-control cboAjaxAction control-validation ddlInterestGroupCode selectpicker" data-live-search="true" multiple="multiple" data-selected-text-format="count > 4" AutoPostBack="false" runat="server"></asp:DropDownList>
                                                            <label id="txtInterestGroupCodeValidation" class="label label-danger label-validation labelMappingValidation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjCauseMappingValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                                            <asp:HiddenField ID="hdfInterestGroupCodes" runat="server" Value='<%# Eval("InterestGroupCode") %>' />
                                                        </span> 
                                                    </div>
                                                    <div class="col-xs-2 col-sm-2" style="text-align:center;">
                                                        <span>
                                                            <asp:CheckBox  ID="chbAdditionalInfo" runat="server" CssClass="enableAdditionalInfoSpan" Checked='<%# Eval("AdditionalInfo") %>' />  
                                                        </span> 
                                                    </div>

                                                    <div class="col-xs-1 col-sm-1" style="text-align:center;">
                                                        <span>
                                                            <asp:CheckBox  ID="chbHours" runat="server" CssClass="enableAdditionalInfoSpan Hoursrules" Checked='<%# Eval("Hours") %>'  />  
                                                        </span> 
                                                    </div>

                                                      <div class="col-xs-1 col-sm-1" style="text-align:center;">
                                                        <span>
                                                            <asp:CheckBox  ID="chbDays" runat="server" CssClass="enableAdditionalInfoSpan Daysrules"  Checked='<%# Eval("Days") %>'  />  
                                                        </span> 
                                                    </div>
                                                </div>                                                        
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

                        <div class="modal-footer">                            
                            <button id="btnAccept" type="button" runat="server" class="btn btn-primary btnAjaxAction" onserverclick="btnAccept_ServerClick" onclick="return ProcessAcceptRequest(this.id);" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>'>
                                <span class="glyphicon glyphicon-ok glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnDialogSave")) %>
                            </button>
                            <button id="btnCancel" type="button" class="btn btn-default" >
                                <span class="glyphicon glyphicon-remove glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnDialogCancel")) %>
                            </button>                            
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <%--  Modal for Duplicated value  --%>
    <div class="modal fade" id="DuplicatedDialog" tabindex="-1" role="dialog" aria-labelledby="DuplicatedDialogTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button id="btnDuplicatedClose" type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title text-primary"><%=Convert.ToString(GetLocalResourceObject("TitleDuplicatedDialog")) %></h3>
                </div>
                <asp:UpdatePanel runat="server" ID="uppDuplicatedDialog">
                    <Triggers>
                
                    </Triggers>
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="form-horizontal">

                                
                                <asp:HiddenField ID="hdfDuplicatedCauseCode" runat="server" Value="" />   

                                <div id="divDuplicatedDialogText" runat="server">
                                    
                                </div>
                                                                
                                <asp:panel ID="pnlDuplicatedDialogDataDetail" runat="server">
                                
                                    <div class="form-group">
                                        <div class="col-sm-4 text-left">
                                            <label for="<%= txtDuplicatedCauseCode.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCauseCode")%></label>
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtDuplicatedCauseCode" CssClass="form-control cleanPasteText ignoreValidation" runat="server" onkeypress="return isNumberOrLetter(event)" MaxLength="50" ReadOnly="true"></asp:TextBox>                                        
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-sm-4 text-left">
                                            <label for="<%= txtDuplicatedCauseName.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCauseName")%></label>
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtDuplicatedCauseName" CssClass="form-control cleanPasteText ignoreValidation" runat="server" onkeypress="return isNumberOrLetter(event) && checkMaxLength(this,event,500);" onkeyUp="return isNumberOrLetter(event) && checkMaxLength(this,event,500);" MaxLength="500" TextMode="MultiLine" Columns="3" ReadOnly="true"></asp:TextBox>                                        
                                        </div>
                                    </div>                                
                                </asp:panel>
                            </div>
                        </div>

                        <div class="modal-footer">                                                       
                            <button id="btnDuplicatedAccept" type="button" class="btn btn-default" >
                                <span class="glyphicon glyphicon-remove glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnAccept")) %>
                            </button>                            
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <%--  Modal for ActivateDeleted value  --%>
    <div class="modal fade" id="ActivateDeletedDialog" tabindex="-1" role="dialog" aria-labelledby="ActivateDeletedDialogTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button id="btnActivateDeletedClose" type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title text-primary"><%=Convert.ToString(GetLocalResourceObject("TitleActivateDeletedDialog")) %></h3>
                </div>
                <asp:UpdatePanel runat="server" ID="uppActivateDeletedDialog">
                    <Triggers>
                
                    </Triggers>
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="form-horizontal">
                                
                                <asp:HiddenField ID="hdfActivateDeletedCauseCode" runat="server" Value="" /> 

                                <%=Convert.ToString(GetLocalResourceObject("lblTextActivateDeletedDialog")) %>

                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtActivateDeletedCauseCode.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCauseCode")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtActivateDeletedCauseCode" CssClass="form-control cleanPasteText ignoreValidation" runat="server" onkeypress="return isNumberOrLetter(event)" MaxLength="50" ReadOnly="true"></asp:TextBox>                                        
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtActivateDeletedCauseName.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCauseName")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtActivateDeletedCauseName" CssClass="form-control cleanPasteText ignoreValidation" runat="server" onkeypress="return isNumberOrLetter(event) && checkMaxLength(this,event,500);" onkeyUp="return isNumberOrLetter(event) && checkMaxLength(this,event,500);" MaxLength="500" TextMode="MultiLine" Columns="3" ReadOnly="true"></asp:TextBox>                                        
                                    </div>
                                </div>   
                            </div>
                            
                        </div>

                        <div class="modal-footer">                            
                            <div class="form-horizontal">

                                <%=Convert.ToString(GetLocalResourceObject("lblTextActionActivateDeletedDialog")) %>

                                <div class="form-group">
                                    <div class="col-sm-2">                                        
                                        <asp:CheckBox ID="chbActivateDeleted" CssClass="groupedCheckbox" runat="server" Checked="true" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                    </div>
                                    <div class="col-sm-10 text-left">
                                        <label for="<%= chbActivateDeleted.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblActivateDeleted")%></label>
                                    </div>                                    
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-2">
                                        <asp:CheckBox ID="chbUpdateActivateDeleted" CssClass="groupedCheckbox" runat="server" Checked="false" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                    </div>
                                    <div class="col-sm-10 text-left">
                                        <label for="<%= chbUpdateActivateDeleted.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblUpdateActivateDeleted")%></label>
                                    </div>                                    
                                </div>
                            </div>
                            <br /><br />
                            <button id="btnActivateDeletedAccept" type="button" runat="server" class="btn btn-primary btnAjaxAction" onserverclick="btnActivateDeletedAccept_ServerClick" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>'>
                                <span class="glyphicon glyphicon-ok glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnDialogSave")) %>
                            </button>
                            <button id="btnActivateDeletedCancel" type="button"  class="btn btn-default" >
                                <span class="glyphicon glyphicon-remove glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnDialogCancel")) %>
                            </button>                            
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>


    <!-- Modal for select the working division-->
    <div class="modal fade" id="workingdivisionmodal" tabindex="-1" role="dialog" aria-labelledby="workingdivisionmodalTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button id="btnExportClose" type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title text-primary" id="workingdivisionmodallabel"><%= Convert.ToString(GetLocalResourceObject("msjWorkinDivisionModalTitle")) %></h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="uppDivisions" runat="server">
                        <Triggers>
                            
                        </Triggers>
                        <ContentTemplate>
                            <div class="form-group row">
                                <label for="<%= cboUserDivisions.ClientID %>" class="col-sm-2 col-control-label"><%= Convert.ToString(GetLocalResourceObject("msjDivisionLabel")) %></label>
                                <div class="col-sm-10">
                                    <asp:DropDownList ID="cboUserDivisions" runat="server" CssClass="form-control selectpicker" data-live-search="true" AutoPostBack="false"></asp:DropDownList>                                    
                                </div>
                            </div>                            
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button id="btnDownloadFile" runat="server" type="button"  class="btn btn-primary btnAjaxAction" onclick="return ProcessDownloadRequest(this.id);" onserverclick="btnDownloadFile_ServerClick">
                        <span class="glyphicon glyphicon-download glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnDownloadFile")) %>
                    </button>
                    <button id="btnExportCancel" type="button"  class="btn btn-default" >
                        <span class="glyphicon glyphicon-remove glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnDialogCancel")) %>
                    </button>
                </div>
            </div>
        </div>
    </div>

    <nav class="navbar-fixed-bottom">
        <div class="container center-block text-center">  
            <b>
            <div class="alert alert-autocloseable-msg" style="display:none;">  	            
	        </div>            
            </b>
        </div>
    </nav>

    <%--  Modal  --%>

    <script type="text/javascript">
        
        //*******************************//
        //          VARIABLES            // 
        //*******************************//

        //Variables for table ordening
        var dataSortAttribute, dataSortType, dataSortDirection;

        //*******************************//
        //       EVENT BINDING           // 
        //*******************************//
        
        function OpenWorkingDivisionModal() {
            ///<summary>Show the working division modal.</summary>            
            $('#workingdivisionmodal').modal('show');
        }

        $(document).ready(function () {
            
        });

        function pageLoad(sender, args) {
            /// <summary>Execute at load even at partial and ajax requests</summary>
            /// We prefer to bind the events here over do it on document ready event in order to auto rebind the events in page and ajax request each time

            //////////////////////////////////////////////////////////////////////////////////////////////////
            //In this section we set the button state for btnAjaxAction
            $('.btnAjaxAction').on('click', function () {
                var $this = $(this);
                $this.button('loading');
                setTimeout(function () { $this.button('reset'); }, 30000);
            });


            
            $('.labelMappingValidation').each(function () {
                
                var $this = $(this);
                $this.prop("for", $this.parent().find('input:first').attr('id'));

            });

            $('.enableDivisionMappingSpan > input').addClass("enableDivisionMapping");

            $('.enableDivisionMapping').each(function() {
                
                var $this = $(this);
                if ($this.is(":checked")) {
                    $this.closest('tr').find(':input').removeAttr('disabled');
                }
                else {
                    $this.closest('tr').find(':input').attr('disabled', 'disabled');
                }

                $this.prop('disabled', false);

            });

            
            $('.enableDivisionMapping').on('click', function () {

                var $this = $(this);
                if ($this.is(":checked")) {
                    $this.closest('tr').find(':input').removeAttr('disabled');
                    $($this.closest('tr').find('.btn')[0]).removeClass('disabled');
                    $($this.closest('tr').find('div.ddlInterestGroupCode ')[0]).removeAttr('disabled');
                }
                else {
                    $this.closest('tr').find(':input').attr('disabled', 'disabled');
                    $this.closest('tr').find(':input').each(function () {

                        SetControlValid($(this).attr('id'));

                    });
                    
                }

                $this.prop('disabled', false);

            });
           
            $('.enableAdditionalInfoSpan > input').addClass("enableAdditionalInfo");

            //And the grvList selection row functionality
            $('#<%= grvList.ClientID %>').on('click', 'tbody tr', function (event) {

                if (! $(this).hasClass('emptyRow')) {
                    $(this).toggleClass('info').siblings().removeClass('info');
                    $('#<%=hdfSelectedRowIndex.ClientID%>').val($(this).hasClass("info") ? $(this).index() : -1);
                }
            });

            //And the grvList pager event for client side
            $('#<%= blstPager.ClientID %>').on('click', 'a', function (event) {
                SetWaitingGrvList(true);
            });

            $("#<%=txtComments.ClientID %>").change(function () {
                $(this).val($.trim( $(this).val() ) )
            });
            $('#<%= btnExport.ClientID %>').on('click', function (ev) {
                ev.preventDefault();

                setTimeout(function () {
                    $("#<%=btnExport.ClientID%>").button('reset');
                }, 500);
                OpenWorkingDivisionModal();
            });

            //others buttons 
            $('#<%= btnAdd.ClientID %>').on('click', function (ev) {
                /// <summary>Handles the click event for button add.</summary>

                ev.preventDefault();

                setTimeout(function () {
                    $("#<%=btnAdd.ClientID%>").button('reset');
                }, 500);

                DisableToolBar();

                ClearModalForm();

                $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceAdd") %>');
                $('#dialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceAdd") %>');

                //Clean data 
                $('.enableDivisionMapping').each(function() {
                    var $this = $(this);
                    $this.prop('checked', false);
                });
                $('input:checkbox').removeAttr('checked');
                $('.ddlCausesLocal').each(function() {
                    var $this = $(this);
                    $this.attr('disabled', 'disabled');
                });

                $('.ddlInterestGroupCode').each(function() {
                    var $this = $(this);
                    $this.attr('disabled', 'disabled');
                });

                $('.enableAdditionalInfo').each(function() {
                    var $this = $(this);
                    $this.prop('checked', false);
                    $this.attr('disabled', 'disabled');
                });

                $('#MaintenanceDialog').modal('show');
                EnableToolBar();
                return false;
            });

            $('#<%= btnSearchDefault.ClientID %>').on('click', function (event) {
                var $this = $('#<%= btnSearch.ClientID %>');
                $this.button('loading');
                setTimeout(function () { $this.button('reset'); }, 30000);
            });


            $('#btnCancel, #btnClose').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                event.preventDefault();
                                
                ClearModalForm();

                DisableButtonsDialog();
                $('#MaintenanceDialog').modal('hide');
                EnableButtonsDialog();
            });

            $('#btnActivateDeletedCancel, #btnActivateDeletedClose').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                event.preventDefault();                    
                $('#MaintenanceDialog').modal('show');
                $('#ActivateDeletedDialog').modal('hide');                
                
            });

            $('#btnExportCancel, #btnExportClose').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                event.preventDefault();                    
                $('#workingdivisionmodal').modal('hide');                 
                
            });
            $('#btnDuplicatedAccept, #btnDuplicatedClose').on('click', function (event) {
                /// <summary>Handles the click event for button accept in user dialog.</summary>            
                event.preventDefault();                
                $('#MaintenanceDialog').modal('show');
                $("#<%=txtCauseCode.ClientID%>").prop( "disabled", false );
                $('#DuplicatedDialog').modal('hide');  
                
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

            //And the clean paste manager
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

            $('#<%= chbActivateDeleted.ClientID %>').on('change', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>                            
                setTimeout(function (e) {
                    if ($('#<%= chbActivateDeleted.ClientID %>').is(":checked")) {
                        if ($('#<%= chbUpdateActivateDeleted.ClientID %>').is(":checked")) {
                            $('#<%= chbUpdateActivateDeleted.ClientID %>').prop('checked', false).change();
                        }
                    }
                    else {
                        if (!$('#<%= chbUpdateActivateDeleted.ClientID %>').is(":checked")) {
                            $('#<%= chbUpdateActivateDeleted.ClientID %>').prop('checked', true).change();
                        }
                    }
                }, 50);
            });

            $('#<%= chbUpdateActivateDeleted.ClientID %>').on('change', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>                            
                setTimeout(function (e) {
                    if ($('#<%= chbUpdateActivateDeleted.ClientID %>').is(":checked")) {
                        if ($('#<%= chbActivateDeleted.ClientID %>').is(":checked")) {
                            $('#<%= chbActivateDeleted.ClientID %>').prop('checked', false).change();
                        }
                    }
                    else {
                        if (!$('#<%= chbActivateDeleted.ClientID %>').is(":checked")) {
                            $('#<%= chbActivateDeleted.ClientID %>').prop('checked', true).change();
                        }
                    }
                }, 50);
            });
            
            //////////////////////////////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////////////////////////////
            //In this section we initialize the checkbox toogles

            $('#<%= chbSearchEnabled.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });
            $('#<%= chbActivateDeleted.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });
            $('#<%= chbUpdateActivateDeleted.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });
            

            //////////////////////////////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////////////////////////////
            //In this section we initialize the date time pickers
           

            //////////////////////////////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////////////////////////////
            //In this section we initialize the popovers and tooltips for the elements.
            //We have both (popovers and tooltips) in order to accomplish information alert over desktop 
            //and mobiles devices
            InitializeTooltipsPopovers();
            //////////////////////////////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////////////////////////////
            //In this section we set the components enabled/disabled according to hdfIsFormEnabled indicator
            //Special attention to client side components that can not be enabled/disabled from server side
            
            
            //////////////////////////////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////////////////////////////
            //Other executions

            //each time a ajax or page load execue we need to synch the selected row with its value
            SetRowSelected();


            $(".ddlInterestGroupCode").on('changed.bs.select', function (e) {
                /// <summary>Handles the select's value changed event for the cboLanguages control.</summary>
                
                var idHidden = $(this)[0].id.replace('ddlInterestGroupCode', 'hdfInterestGroupCodes');
            
                MultiSelectDropdownListSaveSelectedItemsCustom($("#"+ $(this)[0].id +""), $("#"+idHidden));

            });
            
        }

        function MultiSelectDropdownListRestoreSelectedItemsCustom(dropDownListControl, hiddenControl) {
            /// <summary>Recover the selected items that were stored before the postback and assign them to the control to be selected.</summary>
            /// <param name="dropDownListControl" type="The multiple selection dropdown list control.</param>
            /// <param name="hiddenControl" type="control">The hidden control where the selected items will be restored.</param>
            var selectedItems = hiddenControl.val();
            if (selectedItems !== '') {
                dropDownListControl.selectpicker('val', selectedItems.split(","));
            }
            else {
                dropDownListControl.selectpicker('val', '');
            }
        }

        function MultiSelectDropdownListSaveSelectedItemsCustom(dropDownListControl, hiddenControl) {
            /// <summary>Stores the selected items to be able to recover them in an eventual postback.</summary>
            /// <param name="dropDownListControl" type="control">The multiple selection dropdown list control</param>
            /// <param name="hiddenControl" type="control">The hidden control where the selected items will be saved</param>
            var selectedItems = dropDownListControl.selectpicker('val');
            if (selectedItems.length > 0) {
                hiddenControl.val(selectedItems.toString());
            }
            else {
                hiddenControl.val('');
            }
        }
        //*******************************//
        //          VALIDATION           // 
        //*******************************//

        var validator = null;
        function ValidateForm() {
            /// <summary>Validate the form with jquey validation plugin </summary>
            /// <returns> True if form is valid. False otherwise. </returns>
            
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
                            <%= txtCauseCode.UniqueID %>: {
                                required: true,
                                normalizer: function(value) {                                 
                                    return $.trim(value);
                                },
                                minlength: 1,
                                maxlength: 50
                            },
                            <%= txtCauseName.UniqueID %>: {
                                required: true,
                                normalizer: function(value) {
                                    return $.trim(value);
                                },
                                minlength: 1,
                                maxlength: 500
                            },
                            <%= cboCauseCategory.UniqueID %>: {
                                required: true,
                                validSelection: true
                            },                            
                            <%= txtComments.UniqueID %>: {
                                required: false,
                                normalizer: function(value) {
                                    return $.trim(value);
                                },
                                minlength: 1,
                                maxlength: 1000
                            }
                        }
                    });

                $('.txtMapping').each(function () {
                    $(this).rules("add", 
                        {                            
                            required: true,                            
                            normalizer: function(value) {                                 
                                return $.trim(value);
                            },
                            minlength: 1,
                            maxlength: 50
                
                        });
                });

               

            }
            
            //get the results
            
            var result = validator.form();
            return result;
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
                var selectedRow = $('#<%= grvList.ClientID %> tbody tr:eq(' + selectedRowIndex + ')');
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
            $('#<%= grvList.ClientID %> tbody tr').removeClass('info');            
        }
        

        //*******************************//
        //             LOGIC             // 
        //*******************************//

        function ClearModalForm() {
            /// <summary>Clear the modal form</summary>
                        
            $("#<%=hdfCauseCodeEdit.ClientID%>").val("");
            $("#<%=txtCauseCode.ClientID%>").val("");
            $("#<%=txtCauseCode.ClientID%>").removeAttr("disabled");
            $("#<%=txtCauseName.ClientID%>").val("");
            $("#<%=cboCauseCategory.ClientID%>").val("-1");            
            $("#<%=txtComments.ClientID%>").val("");
            $("#<%=chbSearchEnabled.ClientID%>").bootstrapToggle('on');

            if (validator != null) {
                validator.resetForm();
            }                
        }
        
        function SetWaitingGrvList(flag) {
            /// <summary>Process the request of set the grid as waiting style</summary>
            if (flag) {
                $('#<%= grvList.ClientID %>').find("input,button,textarea,select").attr("disabled", "disabled");
                $('#grvListWaiting').fadeIn('fast');
                $('#<%= grvList.ClientID %>').fadeTo('fast', 0.5);
            } else {
                $('#<%= grvList.ClientID %>').find("input,button,textarea,select").removeAttr("disabled");
                $('#grvListWaiting').fadeOut('fast');
                $('#<%= grvList.ClientID %>').fadeIn('fast', 0);
                $('#<%= grvList.ClientID %>').removeAttr("style opacity");
            }

            EnableToolBar();
        }


        function ProcessEditRequest(resetId) {
            /// <summary>Process the edit request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            $(".btndisabled").prop("disabled",true);
            if (IsRowSelected()) {
                
                __doPostBack('<%= btnEdit.UniqueID %>', '');
                return true;
            }

            else {
                $(".btndisabled").prop("disabled",false);
                ShowFooterError('<%= GetLocalResourceObject("msgInvalidSelection") %>');
                ErrorButton(resetId);
                return false;
            }

        }
        
        function ProcessAcceptRequest(resetId) {
            /// <summary>Process the accept request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            
            disableButton($('#btnCancel'));

            if (!ValidateForm()) {
                setTimeout(function () {                    
                    ResetButton(resetId);  
                    enableButton($('#btnCancel'));
                }, 150);

                MostrarMensaje(TipoMensaje.VALIDACION, '<%= GetLocalResourceObject("msgInvalidEntry") %>', null);
                
            }
            else {
                $("#modalProgress").show();
                __doPostBack('<%= btnAccept.UniqueID %>', '');
            }

            return false;
        }
        
        function ProcessDeleteRequest(resetId) {
            /// <summary>Process the delete request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            
            if (IsRowSelected()) {
                ShowConfirmationMessageDelete(resetId);
                return false;
            }

            else {
                ShowFooterError('<%= GetLocalResourceObject("msgInvalidSelection") %>');
                ErrorButton(resetId);
                return false;
            }

        }
        
        function ProcessDownloadRequest(resetId) {
            setTimeout(function () {                    
                ResetButton(resetId);  
                enableButton($('#btnDownloadFile'));
            }, 250);
            __doPostBack('<%= btnDownloadFile.UniqueID %>', '');
        }

        function CloseUserDialog() {
            /// <summary>Close a user dialog.</summary>
            $('#MaintenanceDialog').modal('hide');
        }

        function DisableToolBar() {
            ///<summary>Disables all buttons toolbar</summary>
            disableButton($('#<%= btnAdd.ClientID %>'));
            disableButton($('#<%= btnEdit.ClientID %>'));
            disableButton($('#<%= btnDelete.ClientID %>'));
        }

        function EnableToolBar() {
            ///<summary>Enabled all buttons toolbar</summary>
            enableButton($('#<%= btnAdd.ClientID %>'));
            enableButton($('#<%= btnEdit.ClientID %>'));
            enableButton($('#<%= btnDelete.ClientID %>'));
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

        //*******************************//
        //  RETURN FROM POSTBACKS LOGIC  // 
        //*******************************//

        function ReturnFromBtnEditClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            SetRowSelected();
            DisableToolBar();

            $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#dialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#MaintenanceDialog').modal('show');
            $("#<%=txtCauseCode.ClientID%>").attr("disabled", "disabled");

            EnableToolBar();
            $(".btndisabled").prop("disabled",false);
        }

        function ReturnFromBtnAcceptClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            SetRowSelected();
            DisableButtonsDialog();
            $('#MaintenanceDialog').modal('hide');
            EnableButtonsDialog();  
            $("#modalProgress").hide();
            ShowFooterSuccess('<%= GetLocalResourceObject("msgOperationSuccesfull") %>');
        }

        function ReturnFinally() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $("#modalProgress").hide();
        }

        function ReturnFromBtnAcceptActivateDeletedClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            SetRowSelected();
            DisableButtonsDialog();
            $('#ActivateDeletedDialog').modal('hide');
            $('#MaintenanceDialog').modal('hide');
            EnableButtonsDialog();

            ShowFooterSuccess('<%= GetLocalResourceObject("msgOperationSuccesfull") %>');
        }

       
        function ReturnFromBtnAcceptClickPostBackDeleted() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $('#ActivateDeletedDialog').modal('show');
            $('#MaintenanceDialog').modal('hide');
            
        }

        function ReturnFromBtnAcceptClickPostBackDuplicated() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $('#DuplicatedDialog').modal('show');
            $('#MaintenanceDialog').modal('hide');
            
        }



        function ReturnFromBtnDeleteClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            UnselectRow();
            ShowFooterSuccess('<%= GetLocalResourceObject("msgOperationSuccesfull") %>');

        }

        

        
        //*******************************//
        // MESSAGING AND CONFIRMATION    // 
        //*******************************//

        function ShowConfirmationMessageDelete(resetId) {
            /// <summary>Show confirmation message for Save and Close funtionality</summary>
            /// <param name="resetId" type="String">Id of the button to reset if cancel</param>
            /// <returns>False</returns>

            MostrarConfirmacion(
                '<%= GetLocalResourceObject("msjDelete") %>'
                , '<%=GetLocalResourceObject("Yes")%>'
                , function () {
                 
                    __doPostBack('<%= btnDelete.UniqueID %>', '');
                }
                , '<%=GetLocalResourceObject("No")%>'
                , function () {
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
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(initializeRequest);
        prm.add_endRequest(endingRequest);

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

        function endingRequest(sender, args) {
            if (prm.get_isInAsyncPostBack()) {
                SetWaitingGrvList(false)
            }
        }

    </script>
</asp:Content>
