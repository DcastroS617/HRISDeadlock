<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="HouseholdContributionRanges.aspx.cs" Inherits="HRISWeb.SocialResponsability.Maintenances.HouseholdContributionRanges" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cntHeader" runat="server">
    <title><%=Convert.ToString(GetLocalResourceObject("lblScreenTitle")) %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cntBody" runat="server">
    <div class="main-content">
        <h1 class="text-left text-primary">
            <label class="PageTitle"><%= GetLocalResourceObject("lblScreenTitle") %></label>
        </h1><br />
        <asp:UpdatePanel runat="server" ID="main">
            <Triggers>                
            </Triggers>
            <ContentTemplate>
                <div class="container" style="width:100%">                    
                    <h4 class="text-left text-primary"><%= GetLocalResourceObject("lblFiltersSubtitle") %></h4>                
                    <br />
                    <div class="row">                        
                        <div class="col-sm-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-5 text-left">
                                        <label for="<%=cboUserDivisionsFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("cboUserDivisionsFilter")%></label>
                                    </div>
                                    <div class="col-sm-7">
                                        <asp:DropDownList ID="cboUserDivisionsFilter" CssClass="form-control cboAjaxAction control-validation" AutoPostBack="true" OnSelectedIndexChanged="cboUserDivisionsFilter_SelectedIndexChanged" runat="server"></asp:DropDownList>                                        
                                        <span id="cboUserDivisionsFilterWaiting" class='fa fa-spinner fa-spin waitingNotification'  style="display:none; float: right;margin-right: 25px;margin-top: -23px;position: relative;z-index: 2;"></span>
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
                                        <asp:Label ID="lblCurrency" AssociatedControlID="cboUserDivisionsFilter" runat="server" Text="" CssClass="control-label text-left text-danger"></asp:Label>                      
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>                   
                    <br />
                    <div class="row">                        
                        <div class="col-sm-12 text-center">
                            <asp:HiddenField runat="server" ID="hdfSelectedRowIndex" Value="-1"/>
                            <div>
                                <asp:GridView ID="grvList" Width="100%" runat="server" EmptyDataText='<%$ Code:GetLocalResourceObject("lblNoData") %>' 
                                    EmptyDataRowStyle-CssClass="emptyRow" AllowPaging="false" PagerSettings-Visible="false" AllowSorting="true"
                                    AutoGenerateColumns="false" ShowHeader="true" CssClass="table table-striped table-hover table-bordered"
                                    DataKeyNames="HouseholdContributionRangeCode" OnPreRender="grvList_PreRender" OnSorting="grvList_Sorting">                             
                                    <Columns>                                        
                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle hidden" HeaderStyle-CssClass="hidden" FooterStyle-CssClass="hidden" >
                                            <HeaderTemplate>
                                                <div style="width:100%; text-align:center;">
                                                    <asp:LinkButton ID="HouseholdContributionRangeCodeSort" runat="server" CommandName="Sort" CommandArgument="HouseholdContributionRangeCode" OnClientClick="SetWaitingGrvList(true);">          
                                                        <span><%= GetLocalResourceObject("HouseholdContributionRangeCode.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "HouseholdContributionRangeCode") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvHouseholdContributionRangeCode" data-id="householdcontributionrangecode" data-value="<%# Eval("HouseholdContributionRangeCode") %>"><%# Eval("HouseholdContributionRangeCode") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField> 
                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" >
                                            <HeaderTemplate>
                                                <div style="width:100%; text-align:center;">
                                                    <asp:LinkButton ID="RangeFromSort" runat="server" CommandName="Sort" CommandArgument="RangeFrom" OnClientClick="SetWaitingGrvList(true);">          
                                                        <span><%= GetLocalResourceObject("RangeFrom.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "RangeFrom") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvRangeFrom" data-id="rangefrom" data-value="<%# Convert.ToDecimal(Eval("RangeFrom").ToString()) %>"><%# String.Format("{0:N2}", Eval("RangeFrom")) %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>                                                                                 
                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" >
                                            <HeaderTemplate>
                                                <div style="width:100%; text-align:center;">
                                                    <asp:LinkButton ID="RangeToSort" runat="server" CommandName="Sort" CommandArgument="RangeTo" OnClientClick="SetWaitingGrvList(true);">          
                                                        <span><%= GetLocalResourceObject("RangeTo.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "RangeTo") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvRangeTo" data-id="rangeto" data-value="<%# Convert.ToDecimal(Eval("RangeTo").ToString()) %>"><%# String.Format("{0:N2}",Eval("RangeTo")) %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" >
                                            <HeaderTemplate>
                                                <div style="width:100%; text-align:center;">
                                                    <asp:LinkButton ID="RangeOrderSort" runat="server" CommandName="Sort" CommandArgument="RangeOrder" OnClientClick="SetWaitingGrvList(true);">      
                                                        <span><%= GetLocalResourceObject("RangeOrder.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "RangeOrder") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvRangeOrder" data-id="rangeorder" data-value="<%# Eval("RangeOrder") %>"><%# Eval("RangeOrder") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>                                        
                                    </Columns>
                                </asp:GridView>           
                                <div id="grvListWaiting" style="position: absolute; margin: auto; display: none; bottom: 0px; top: 0px; left: 50%;transform: translate(-50%, 35%);">
                                    <span class='fa fa-spinner fa-spin' style="font-size:50px;"></span>
                                </div>
                            </div>
                            <h4 class="text-left text-primary" id="htmlResultsSubtitle" runat="server"></h4>                
                            <br />
                            <nav>
                                <asp:BulletedList ID="blstPager" runat="server" CssClass="pagination" DisplayMode="LinkButton" OnClick="blstPager_Click" >
                                </asp:BulletedList>
                            </nav>
                        </div>
                    </div>
                </div>                                
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="ButtonsActions">
            <asp:UpdatePanel ID="uppActions" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                <Triggers>
                </Triggers>
                <ContentTemplate>
                    <div class="btn-group" role="group" aria-label="main-buttons">
                        <button id="btnAdd" type="button" runat="server" class="btn btn-default btnAjaxAction btns" onclick="return false;" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnAdd"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnAdd"))%>' >
                            <span class="glyphicon glyphicon-plus glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span><br /><%= GetLocalResourceObject("btnAdd") %>
                        </button>
                        <button id="btnEdit" type="button" runat="server" class="btn btn-default btnAjaxAction btns"  onserverclick="btnEdit_ServerClick" onclick="return ProcessEditRequest(this.id);" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnEdit"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnEdit"))%>' >
                            <span class="glyphicon glyphicon-edit glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span><br /><%= GetLocalResourceObject("btnEdit") %>
                        </button>
                        <button id="btnDelete" type="button" runat="server" class="btn btn-default btnAjaxAction btns" onserverclick="btnDelete_ServerClick"  onclick="return ProcessDeleteRequest(this.id);" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnDelete"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnDelete"))%>' >
                            <span class="glyphicon glyphicon-trash glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span><br /><%= GetLocalResourceObject("btnDelete") %>
                        </button>                        
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <%--  Modal for Add and Edit  --%>
    <div class="modal fade" id="MaintenanceDialog" tabindex="-1" role="dialog" aria-labelledby="MaintenanceDialogTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" id="btnClose" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title text-primary" id="dialogTitle"></h3>
                </div>
                <asp:UpdatePanel runat="server" ID="uppDialogControl">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="form-horizontal">
                                <asp:HiddenField ID="hdfHouseholdContributionRangeCodeEdit" runat="server" Value="" />  
                                <asp:HiddenField ID="hdfUiLanguage" runat="server" Value="" />
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%=cboDivisionCode.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblDivisionCode")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="cboDivisionCode" CssClass="form-control control-validation" Enabled="false" AutoPostBack="false" runat="server"></asp:DropDownList>
                                        <label id="cboDivisionCodeValidation" for="<%= cboDivisionCode.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgDivisionCodeValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtRangeFrom.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblRangeFrom")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtRangeFrom" CssClass="form-control cleanPasteDigits" runat="server" onkeypress="return isNumber(event) && checkMaxLength(this,event,9);" onkeyUp="return isNumber(event) && checkMaxLength(this,event,9);" MaxLength="9" TextMode="SingleLine" autocomplete="off"></asp:TextBox>
                                        <label id="txtRangeFromValidation" for="<%= txtRangeFrom.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgtxtRangeFromValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtRangeTo.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblRangeTo")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtRangeTo" CssClass="form-control cleanPasteDigits" runat="server" onkeypress="return isNumber(event) && checkMaxLength(this,event,9);" onkeyUp="return isNumber(event) && checkMaxLength(this,event,9);" MaxLength="9" TextMode="SingleLine" autocomplete="off"></asp:TextBox>
                                        <label id="txtRangeToValidation" for="<%= txtRangeTo.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgRangeToValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%=cboRangeOrder.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblRangeOrder")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="cboRangeOrder" CssClass="form-control control-validation" AutoPostBack="false" runat="server"></asp:DropDownList>
                                        <label id="cboRangeOrderValidation" for="<%= cboRangeOrder.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgRangeOrderValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                    </div>
                                </div>   
                                <input type="hidden" id="RangeOrderOld" name="RangeOrderOld" runat="server" value="" />
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= chkSearchEnabled.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblSearchEnabled")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:CheckBox ID="chkSearchEnabled" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                    </div>
                                </div>
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
                                <div id="divDuplicatedDialogText" runat="server">                                    
                                </div>                                                                
                                <asp:panel ID="pnlDuplicatedDialogDataDetail" runat="server">
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%=cboDuplicatedDivisionCode.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblDivisionCode")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="cboDuplicatedDivisionCode" CssClass="form-control control-validation" Enabled="false" AutoPostBack="false" runat="server"></asp:DropDownList>                                        
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtDuplicatedRangeFrom.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblRangeFrom")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtDuplicatedRangeFrom" CssClass="form-control cleanPasteText" runat="server" onkeypress="return isNumber(event) && checkMaxLength(this,event,12);" onkeyUp="return isNumberOrLetter(event) && checkMaxLength(this,event,12);" MaxLength="12" TextMode="SingleLine" ReadOnly="true"></asp:TextBox>                                        
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtDuplicatedRangeTo.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblRangeTo")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtDuplicatedRangeTo" CssClass="form-control cleanPasteText" runat="server" onkeypress="return isNumber(event) && checkMaxLength(this,event,12);" onkeyUp="return isNumberOrLetter(event) && checkMaxLength(this,event,12);" MaxLength="12" TextMode="SingleLine" ReadOnly="true"></asp:TextBox>                                        
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%=cboDuplicatedRangeOrder.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblRangeOrder")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="cboDuplicatedRangeOrder" CssClass="form-control control-validation" AutoPostBack="false" runat="server" Enabled="false"></asp:DropDownList>                                        
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
                                <%=Convert.ToString(GetLocalResourceObject("lblTextActivateDeletedDialog")) %>
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%=cboActivateDeletedDivisionCode.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblDivisionCode")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="cboActivateDeletedDivisionCode" CssClass="form-control control-validation" Enabled="false" AutoPostBack="false" runat="server"></asp:DropDownList>                                        
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtActivateDeletedRangeFrom.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblRangeFrom")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtActivateDeletedRangeFrom" CssClass="form-control cleanPasteText" runat="server" onkeypress="return isNumber(event) && checkMaxLength(this,event,12);" onkeyUp="return isNumberOrLetter(event) && checkMaxLength(this,event,12);" MaxLength="12" TextMode="SingleLine" ReadOnly="true"></asp:TextBox>                                        
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtActivateDeletedRangeTo.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblRangeTo")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtActivateDeletedRangeTo" CssClass="form-control cleanPasteText" runat="server" onkeypress="return isNumber(event) && checkMaxLength(this,event,12);" onkeyUp="return isNumberOrLetter(event) && checkMaxLength(this,event,12);" MaxLength="12" TextMode="SingleLine" ReadOnly="true"></asp:TextBox>                                        
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%=cboActivateDeletedRangeOrder.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblRangeOrder")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="cboActivateDeletedRangeOrder" CssClass="form-control control-validation" AutoPostBack="false" runat="server" Enabled="false"></asp:DropDownList>                                        
                                    </div>
                                </div>   
                            </div>                            
                        </div>
                        <div class="modal-footer">                            
                            <div class="form-horizontal">
                                <%=Convert.ToString(GetLocalResourceObject("lblTextActionActivateDeletedDialog")) %>
                                <div class="form-group">
                                    <div class="col-sm-2">                                        
                                        <asp:CheckBox ID="chkActivateDeleted" CssClass="groupedCheckbox" runat="server" Checked="true" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                    </div>
                                    <div class="col-sm-10 text-left">
                                        <label for="<%= chkActivateDeleted.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblActivateDeleted")%></label>
                                    </div>                                    
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-2">
                                        <asp:CheckBox ID="chkUpdateActivateDeleted" CssClass="groupedCheckbox" runat="server" Checked="false" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                    </div>
                                    <div class="col-sm-10 text-left">
                                        <label for="<%= chkUpdateActivateDeleted.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblUpdateActivateDeleted")%></label>
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
    <nav class="navbar-fixed-bottom">
        <div class="container center-block text-center">
            <b>
                <div class="alert alert-autocloseable-msg" style="display: none;"></div>
            </b>
        </div>
    </nav>
    <script type="text/javascript">
        //Variables for table ordening
        var dataSortAttribute, dataSortType, dataSortDirection;
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
            //In this section we set the dropdown state (element) for cboAjaxAction
            $('.cboAjaxAction').on('change', function () {
                var $this = $(this);
                $this.siblings(".waitingNotification").show();
                SetWaitingGrvList(true);
            });
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
            //others buttons 
            $('#<%= btnAdd.ClientID %>').on('click', function (ev) {
                /// <summary>Handles the click event for button add.</summary>
                ev.preventDefault();
                var selectedDivision = $("#<%=cboUserDivisionsFilter.ClientID%>").val();                
                if(selectedDivision == null || selectedDivision == '' || selectedDivision == '0' || selectedDivision == '-1'){
                    MostrarMensaje(TipoMensaje.VALIDACION, '<%= GetLocalResourceObject("msgDivisionCodeValidation") %>', function(){
                        $("#<%=btnAdd.ClientID%>").button('reset');
                    });
                    return;
                }
                var rangeCount = $('#<%= grvList.ClientID %> tbody tr').length;
                if(rangeCount >= 12){
                    MostrarMensaje(TipoMensaje.VALIDACION, '<%= GetLocalResourceObject("msgRangesNumberExceeded") %>', function(){
                        $("#<%=btnAdd.ClientID%>").button('reset');
                    });
                    return;
                }
                setTimeout(function () {
                    $("#<%=btnAdd.ClientID%>").button('reset');
                }, 500);                
                DisableToolBar();
                ClearModalForm();
                $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceAdd") %>');
                $('#dialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceAdd") %>');
                $("#<%=cboDivisionCode.ClientID%>").val($("#<%=cboUserDivisionsFilter.ClientID%>").val());
                $('#MaintenanceDialog').modal('show');
                EnableToolBar();
                return false;
            });
            $('#btnCancel, #btnClose').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                event.preventDefault();                                
                ClearModalForm();
                DisableButtonsDialog();
                $('#MaintenanceDialog').modal('hide');
                EnableButtonsDialog();
            });
            $('#btnActivateDeletedClose, #btnActivateDeletedCancel').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                event.preventDefault();                                
                $("#<%=hdfHouseholdContributionRangeCodeEdit.ClientID%>").val("-1");
            });
            $('#btnActivateDeletedCancel, #btnActivateDeletedClose').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                event.preventDefault();                    
                $('#MaintenanceDialog').modal('show');
                $('#ActivateDeletedDialog').modal('hide');                
            });
            $('#btnDuplicatedAccept, #btnDuplicatedClose').on('click', function (event) {
                /// <summary>Handles the click event for button accept in user dialog.</summary>            
                event.preventDefault();                
                $('#MaintenanceDialog').modal('show');
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

            $('#<%= chkActivateDeleted.ClientID %>').on('change', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>                            
                setTimeout(function (e) {
                    if ($('#<%= chkActivateDeleted.ClientID %>').is(":checked")) {
                        if ($('#<%= chkUpdateActivateDeleted.ClientID %>').is(":checked")) {
                            $('#<%= chkUpdateActivateDeleted.ClientID %>').prop('checked', false).change();
                        }
                    }
                    else {
                        if (!$('#<%= chkUpdateActivateDeleted.ClientID %>').is(":checked")) {
                            $('#<%= chkUpdateActivateDeleted.ClientID %>').prop('checked', true).change();
                        }
                    }
                }, 50);
            });

            $('#<%= chkUpdateActivateDeleted.ClientID %>').on('change', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>                            
                setTimeout(function (e) {
                    if ($('#<%= chkUpdateActivateDeleted.ClientID %>').is(":checked")) {
                        if ($('#<%= chkActivateDeleted.ClientID %>').is(":checked")) {
                            $('#<%= chkActivateDeleted.ClientID %>').prop('checked', false).change();
                        }
                    }
                    else {
                        if (!$('#<%= chkActivateDeleted.ClientID %>').is(":checked")) {
                            $('#<%= chkActivateDeleted.ClientID %>').prop('checked', true).change();
                        }
                    }
                }, 50);
            });
            //////////////////////////////////////////////////////////////////////////////////////////////////
            //In this section we initialize the checkbox toogles

            $('#<%= chkSearchEnabled.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });
            $('#<%= chkActivateDeleted.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });
            $('#<%= chkUpdateActivateDeleted.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });            

            //////////////////////////////////////////////////////////////////////////////////////////////////
            //In this section we initialize the popovers and tooltips for the elements.
            //We have both (popovers and tooltips) in order to accomplish information alert over desktop 
            //and mobiles devices
            InitializeTooltipsPopovers();            
            //////////////////////////////////////////////////////////////////////////////////////////////////
            //In this section we initialize the sorter elements for tables.

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
            });
            //////////////////////////////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////////////////////////////
            //In this section we set the components enabled/disabled according to hdfIsFormEnabled indicator
            //Special attention to client side components that can not be enabled/disabled from server side
            //////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////
            //Other executions
            //each time a ajax or page load execue we need to synch the selected row with its value
            SetRowSelected();

            //$('.numericInputMask').inputmask('decimal', {
            //    rightAlign: true
            //    , groupSeparator: ','
            //    , digits: 2
            //    , autoGroup: true
            //    , showMaskOnFocus: false
            //    , showMaskOnHover: true
            //    , removeMaskOnSubmit: true
            //    , autoUnmask: true
            //    , maxlength: 14
            //    , unmaskAsNumber: true
            //    , allowPlus: false
            //    , allowMinus: false
            //});
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
                    var valid = this.optional(element) || value !== "-1";
                    let rangeOrder = Number($('#<%=cboRangeOrder.ClientID%>').val());
                    let codeEdit = -1; 
                    codeEdit = Number($('#<%= hdfHouseholdContributionRangeCodeEdit.ClientID %>').val());
                    if(codeEdit == -1){
                        $('#ctl00_cntBody_grvList tbody tr').each(function(index, value){
                            let gridRangeOrder = parseInt($(value).find('span[data-id="rangeorder"]').data('value'));
                            //console.log("From:" + Number(gridRangeFrom) +"- To:"+ Number(gridRangeTo));
                            if(Number(rangeOrder) == Number(gridRangeOrder)){
                                valid = false;
                            }
                        });
                    }
                    return valid;
                }, "<%= GetLocalResourceObject("msgDivisionCodeValidation") %>");
                jQuery.validator.addMethod("validDecimal", function (value, element) {
                    let uiLanguage = $('#<%=hdfUiLanguage.ClientID%>').val();
                    let separator = uiLanguage == 'en-US' ? ',' : '.';
                    let separatorDecimal = uiLanguage == 'en-US' ? '.' : ',';

                    return Inputmask.isValid(value, { alias: "decimal", digits: 2, groupSeparator: separator, radixPoint: separatorDecimal });
                }, "<%= GetLocalResourceObject("msgDivisionCodeValidation") %>");
                jQuery.validator.addMethod("validGreaterThan", function (value, element) {
                    let rangeFrom = Number($('#<%=txtRangeFrom.ClientID%>').val());
                    let rangeTo = Number($('#<%=txtRangeTo.ClientID%>').val());

                    return rangeTo > rangeFrom;
                }, "<%= GetLocalResourceObject("msgtxtRangeFromValidation") %>");
                jQuery.validator.addMethod("validateDuplicateRange", function (value, element) {
                    var RangeOrderOld = parseInt($("#<%=RangeOrderOld.ClientID%>").val());
                    var rangeFrom = Number($('#<%=txtRangeFrom.ClientID%>').val());
                    var rangeTo = Number($('#<%=txtRangeTo.ClientID%>').val());
                    var rangeOrder = Number($('#<%=cboRangeOrder.ClientID%>').val());
                    var returnValue = true;
                    var i = 0;
                    $('#<%= grvList.ClientID %> tbody tr').each(function(index, value){
                        i += 1;
                        var gridRangeFrom = parseFloat($(value).find('span[data-id="rangefrom"]').data('value'));
                        var gridRangeTo = parseFloat($(value).find('span[data-id="rangeto"]').data('value'));
                        var gridRangeOrder = parseInt($(value).find('span[data-id="rangeorder"]').data('value'));
                        //console.log("From:" + Number(gridRangeFrom) +"- To:"+ Number(gridRangeTo));
                        var exists = false;
                        $('#ctl00_cntBody_grvList tbody tr').each(function(index, value){
                            var gridRangeOrderReview = parseInt($(value).find('span[data-id="rangeorder"]').data('value'));
                            if(Number(rangeOrder) == Number(gridRangeOrder)){
                                exists = true;
                            }
                        });
                        if(Number(rangeOrder) != Number(gridRangeOrder) && exists){
                            if(((rangeFrom == gridRangeFrom) && (gridRangeFrom > 0)) || ((rangeTo == gridRangeTo) && (gridRangeTo > 0))){
                                returnValue = false;
                                return;
                            }
                        } 
                        else if(Number(rangeOrder) == Number(gridRangeOrder) && exists) {
                            //console.log('Entra en el else');
                            var j = 0;
                            $('#<%= grvList.ClientID %> tbody tr').each(function(index, value){
                                j += 1;
                                let gridRangeFromAll = parseFloat($(value).find('span[data-id="rangefrom"]').data('value'));
                                let gridRangeToAll = parseFloat($(value).find('span[data-id="rangeto"]').data('value'));
                                if(j < i){
                                    if((rangeFrom < gridRangeToAll) && (gridRangeToAll > 0)){
                                        returnValue = false;
                                        return;
                                    }
                                } else if (j > i){
                                    if((rangeTo > gridRangeFromAll) && (gridRangeFromAll > 0)){
                                        returnValue = false;
                                        return;
                                    }
                                }
                            });
                        } else {
                            
                            $('#<%= grvList.ClientID %> tbody tr').each(function(index, value){
                                j += 1;
                                var gridRangeFromAll = parseFloat($(value).find('span[data-id="rangefrom"]').data('value'));
                                var gridRangeToAll = parseFloat($(value).find('span[data-id="rangeto"]').data('value'));
                                var gridRangeOrder = parseInt($(value).find('span[data-id="rangeorder"]').data('value'));
                               
                                if (RangeOrderOld == gridRangeOrder) {
                                     console.log("Entro aca");
                                }else if (gridRangeOrder < rangeOrder) {

                                    if (gridRangeToAll <= rangeFrom) {

                                    } else { returnValue = false; return;  }
                                } 
                                else if (gridRangeOrder > rangeOrder) {

                                if (rangeTo <= gridRangeFromAll) {

                                    } else { returnValue = false; return; }


                                }
                               else if (rangeOrder == gridRangeOrder) {
                                    
                                }
                               


                            });
                        }
                        
                        /*if((rangeOrder-1 == gridRangeOrder) &&  gridRangeTo >= rangeFrom){
                            returnValue = false;
                            return;
                        }
                        if((rangeOrder+1 == gridRangeOrder) &&  rangeTo >= gridRangeFrom){
                            returnValue = false;
                            return;
                        }*/
                    });

                    return returnValue;
                }, "<%= GetLocalResourceObject("msgtxtRangeFromValidation") %>");


                //declare the validator
                validator =
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
                            <%= cboDivisionCode.UniqueID %>: {
                                required: true,
                                validSelection: true
                            },
                            <%= txtRangeFrom.UniqueID %>: {
                                required: true,
                                normalizer: function(value) {                                 
                                    return $.trim(value);
                                },
                                minlength: 1,
                                maxlength: 9,
                                min:0,
                                max: 999999999,
                                number: true,
                                validGreaterThan : true,
                                validateDuplicateRange: true
                            },
                            <%= txtRangeTo.UniqueID %>: {
                                required: true,
                                normalizer: function(value) {
                                    return $.trim(value);
                                },
                                minlength: 1,
                                maxlength: 9,
                                min:0,
                                max: 999999999,
                                number: true,
                                validGreaterThan : true,
                                validateDuplicateRange: true
                            },
                            <%= cboRangeOrder.UniqueID %>: {
                                required: true,
                                validSelection: true
                            }
                        }
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
            $("#<%=hdfHouseholdContributionRangeCodeEdit.ClientID%>").val("-1");
            $("#<%=txtRangeFrom.ClientID%>").val("");
            $("#<%=txtRangeTo.ClientID%>").val("");
            $("#<%=cboRangeOrder.ClientID%>").val("-1");
            $("#<%=chkSearchEnabled.ClientID%>").bootstrapToggle('on');
            $("#RangeOrderOld").val("-1");
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
        }
        function ProcessEditRequest(resetId) {
            /// <summary>Process the edit request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>   
            $(".btns").prop("disabled",true);
            if (IsRowSelected()) {   
                $("#<%=cboDivisionCode.ClientID%>").val($("#<%=cboUserDivisionsFilter.ClientID%>").val());
                __doPostBack('<%= btnEdit.UniqueID %>', '');
                return true;
            }
            else {
                ShowFooterError('<%= GetLocalResourceObject("msgInvalidSelection") %>');
                ErrorButton(resetId);
                $(".btns").prop("disabled",false);
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
        function ReturnFromBtnEditClickPostBack(uiLanguage) {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $(".btns").prop("disabled",true);
            SetRowSelected();
            DisableToolBar();
            $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#dialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            
            $('.numericInputMask').inputmask('remove');
            var separator = uiLanguage == 'en-US' ? ',' : '.';
            var separatorDecimal = uiLanguage == 'en-US' ? '.' : ',';
            $('#MaintenanceDialog').modal('show');
            EnableToolBar();
            $(".btns").prop("disabled",false);
            //$('.numericInputMask').inputmask('decimal', {
            //    rightAlign: true
            //    , groupSeparator: separator
            //    , radixPoint: separatorDecimal
            //    , digits: 2
            //    , autoGroup: true
            //    , showMaskOnFocus: false
            //    , showMaskOnHover: true
            //    , removeMaskOnSubmit: true
            //    , autoUnmask: true
            //    , maxlength: 14
            //    , unmaskAsNumber: true
            //    , allowPlus: false
            //    , allowMinus: false
            //});            
        }
        function ReturnFromBtnAcceptClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            DisableButtonsDialog();
            SetRowSelected();
            
            $('#MaintenanceDialog').modal('hide');
            ShowFooterSuccess('<%= GetLocalResourceObject("msgOperationSuccesfull") %>');
            EnableButtonsDialog();
            
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
            $('#MaintenanceDialog').modal('hide');     
            $('#ActivateDeletedDialog').modal('show');
                   
        }
        function ReturnFromBtnAcceptClickPostBackDuplicated() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $('#MaintenanceDialog').modal('hide');    
            $('#DuplicatedDialog').modal('show');
                    
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
                prm.abortPostBack();
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