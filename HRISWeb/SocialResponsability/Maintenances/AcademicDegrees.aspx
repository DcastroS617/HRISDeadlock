<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AcademicDegrees.aspx.cs" Inherits="HRISWeb.SocialResponsability.Maintenances.AcademicDegrees" %>
<asp:Content ID="Content" ContentPlaceHolderID="cntHeader" runat="server">
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
                        <div class="col-sm-12">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="form-group col-sm-12 col-md-8 ES">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%= txtPrincipalAcademicDegreeDescriptionSpanishFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblAcademicDegreeName")%></label>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtPrincipalAcademicDegreeDescriptionSpanishFilter" CssClass="form-control control-validation cleanPasteText ignoreValidation" runat="server" onkeypress="blockEnterKey(); return isNumberOrLetter(event)" MaxLength="150" autocomplete="off" type="text"></asp:TextBox>
                                        </div>
                                     </div>
                                    <div class="form-group col-sm-12 col-md-8 EN">
                                        <div class="col-sm-5 text-left">
                                            <label for="<%= txtPrincipalAcademicDegreeDescriptionEnglishFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblAcademicDegreeName")%></label>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtPrincipalAcademicDegreeDescriptionEnglishFilter" CssClass="form-control control-validation cleanPasteText ignoreValidation" runat="server" onkeypress="blockEnterKey(); return isNumberOrLetter(event)" MaxLength="150" autocomplete="off" type="text"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 text-center">
                            <button id="btnSearch" type="button" runat="server" class="btn btn-default btnAjaxAction" onserverclick="BtnSearch_ServerClick" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>'>
                                <span class="glyphicon glyphicon-search glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%= GetLocalResourceObject("btnSearch") %>
                            </button>                            
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
                                    DataKeyNames="AcademicDegreeCode" OnPreRender="GrvList_PreRender" OnSorting="GrvList_Sorting" OnRowDataBound="GrvList_RowDataBound">                             
                                    <Columns>                                        
                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle hidden" HeaderStyle-CssClass="hidden" FooterStyle-CssClass="hidden" >
                                            <HeaderTemplate>
                                                <div style="width:100%; text-align:center;">
                                                    <asp:LinkButton ID="AcademicDegreeCodeSort" runat="server" CommandName="Sort" CommandArgument="AcademicDegreeCode" OnClientClick="SetWaitingGrvList(true);">          
                                                        <span><%= GetLocalResourceObject("AcademicDegreeCode.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "AcademicDegreeCode") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvAcademicDegreeCode" data-id="PrincipalAcademicDegreecode" data-value="<%# Eval("AcademicDegreeCode") %>"><%# Eval("AcademicDegreeCode") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField> 
                                                                                                                      
                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" >
                                            <HeaderTemplate>
                                                <div style="width:100%; text-align:center;">
                                                    <asp:LinkButton ID="AcademicDegreeNameEnglishSort" runat="server" CommandName="Sort" CommandArgument="AcademicDegreeNameEnglish" OnClientClick="SetWaitingGrvList(true);">          
                                                        <span><%= GetLocalResourceObject("AcademicDegreeNameEN.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "AcademicDegreeNameEnglish") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvAcademicDegreeNameEnglish" data-id="AcademicDegreenameenglish" data-value="<%# Eval("AcademicDegreeDescriptionEnglish") %>"><%# Eval("AcademicDegreeDescriptionEnglish")%></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>  
                                        <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" >
                                            <HeaderTemplate>
                                                <div style="width:100%; text-align:center;">
                                                    <asp:LinkButton ID="AcademicDegreeNameSpanishSort" runat="server" CommandName="Sort" CommandArgument="AcademicDegreeNameSpanish" OnClientClick="SetWaitingGrvList(true);">          
                                                        <span><%= GetLocalResourceObject("AcademicDegreeNameES.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "AcademicDegreeNameSpanish") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span id="dvAcademicDegreeNameSpanish" data-id="AcademicDegreenamespanish" data-value="<%# Eval("AcademicDegreeDescriptionSpanish") %>"><%# Eval("AcademicDegreeDescriptionSpanish")%></span>
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
                                <asp:BulletedList ID="blstPager" runat="server" CssClass="pagination" DisplayMode="LinkButton" OnClick="BlstPager_Click" >
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
                        <button id="btnEdit" type="button" runat="server" class="btn btn-default btnAjaxAction btns"  onserverclick="BtnEdit_ServerClick" onclick="return ProcessEditRequest(this.id);" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnEdit"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnEdit"))%>' >
                            <span class="glyphicon glyphicon-edit glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span><br /><%= GetLocalResourceObject("btnEdit") %>
                        </button>
                        <button id="btnDelete" type="button" runat="server" class="btn btn-default btnAjaxAction btns" onserverclick="BtnDelete_ServerClick"  onclick="return ProcessDeleteRequest(this.id);" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnDelete"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnDelete"))%>' >
                            <span class="glyphicon glyphicon-trash glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span><br /><%= GetLocalResourceObject("btnDelete") %>
                        </button>  
                         <button id="btnCoursingYearEdit" type="button" runat="server" class="btn btn-default btnAjaxAction btns" onserverclick="BtnEditCoursingYear_ServerClick" onclick="return ProcessEditCoursingYearRequest(this.id);"  data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnAddEditCoursingYear"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnAddEditCoursingYear"))%>' >
                            <span class="glyphicon glyphicon-plus glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span><br /><%= GetLocalResourceObject("btnAddEditCoursingYear") %>
                        </button>
                         <button id="btnNotCoursingYearEdit" type="button" runat="server" class="btn btn-default btnAjaxAction btns" onserverclick="BtnEditNotCoursingYear_ServerClick" onclick="return ProcessEditNotCoursingYearRequest(this.id);"  data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnAddEditNotCoursingYear"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnAddEditNotCoursingYear"))%>' >
                            <span class="glyphicon glyphicon-plus glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span><br /><%= GetLocalResourceObject("btnAddEditNotCoursingYear") %>
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
                                <asp:HiddenField ID="hdfAcademicDegreeCodeEdit" runat="server" Value="" />
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtAcademicDegreeNameSpanish.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblAcademicDegreeNameSpanish")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtAcademicDegreeNameSpanish" CssClass="form-control cleanPasteText" runat="server" onkeypress="return isNumberOrLetter(event) && checkMaxLength(this,event,150);" onkeyUp="return isNumberOrLetter(event) && checkMaxLength(this,event,150);" MaxLength="150" TextMode="SingleLine" autocomplete="off"></asp:TextBox>
                                        <label id="txtAcademicDegreeNameSpanishValidation" for="<%= txtAcademicDegreeNameSpanish.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgAcademicDegreeNameSpanishValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtAcademicDegreeNameEnglish.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblAcademicDegreeNameEnglish")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtAcademicDegreeNameEnglish" CssClass="form-control cleanPasteText" runat="server" onkeypress="return isNumberOrLetter(event) && checkMaxLength(this,event,150);" onkeyUp="return isNumberOrLetter(event) && checkMaxLength(this,event,150);" MaxLength="150" TextMode="SingleLine" autocomplete="off"></asp:TextBox>
                                        <label id="txtAcademicDegreeNameEnglishValidation" for="<%= txtAcademicDegreeNameEnglish.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgAcademicDegreeNameEnglishValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtOrderList.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblAcademicDegreeOrderList")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtOrderList" CssClass="form-control cleanPasteText" runat="server" onkeypress="return isNumber(event);" onkeyUp="return isNumber(event)" MaxLength="3" TextMode="SingleLine" autocomplete="off"></asp:TextBox>
                                        <label id="txtOrderListValidation" for="<%= txtOrderList.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgAcademicDegreeOrderListValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= cboDegreeFormationType.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblAcademicDegreeTypeFormationSpanish")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="cboDegreeFormationType"  AutoPostBack="false" runat="server" CssClass="form-control selectpicker" data-live-search="true" ></asp:DropDownList>
                                        <label id="cboDegreeFormationTypeValidation" for="<%= cboDegreeFormationType.ClientID%>" class="label label-danger label-validation" disabled="disabled" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgDegreeFormationValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                    </div>
                                </div>
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
                            <button id="btnAccept" type="button" runat="server" class="btn btn-primary btnAjaxAction" onserverclick="BtnAccept_ServerClick" onclick="return ProcessAcceptRequest(this.id);" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>'>
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
                                        <label for="<%= txtDuplicatedAcademicDegreeNameSpanish.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblAcademicDegreeNameSpanish")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtDuplicatedAcademicDegreeNameSpanish" CssClass="form-control cleanPasteText" runat="server" onkeypress="return isNumber(event) && checkMaxLength(this,event,150);" onkeyUp="return isNumberOrLetter(event) && checkMaxLength(this,event,150);" MaxLength="150" TextMode="SingleLine" ReadOnly="true"></asp:TextBox>                                        
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtDuplicatedAcademicDegreeNameEnglish.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblAcademicDegreeNameEnglish")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtDuplicatedAcademicDegreeNameEnglish" CssClass="form-control cleanPasteText" runat="server" onkeypress="return isNumber(event) && checkMaxLength(this,event,150);" onkeyUp="return isNumberOrLetter(event) && checkMaxLength(this,event,150);" MaxLength="150" TextMode="SingleLine" ReadOnly="true"></asp:TextBox>                                        
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
                                        <label for="<%= txtActivateDeletedAcademicDegreeNameSpanish.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblAcademicDegreeNameSpanish")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtActivateDeletedAcademicDegreeNameSpanish" CssClass="form-control cleanPasteText" runat="server" onkeypress="return isNumber(event) && checkMaxLength(this,event,150);" onkeyUp="return isNumberOrLetter(event) && checkMaxLength(this,event,150);" MaxLength="150" TextMode="SingleLine" ReadOnly="true"></asp:TextBox>                                        
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= txtActivateDeletedAcademicDegreeNameEnglish.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblAcademicDegreeNameEnglish")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtActivateDeletedAcademicDegreeNameEnglish" CssClass="form-control cleanPasteText" runat="server" onkeypress="return isNumber(event) && checkMaxLength(this,event,150);" onkeyUp="return isNumberOrLetter(event) && checkMaxLength(this,event,150);" MaxLength="150" TextMode="SingleLine" ReadOnly="true"></asp:TextBox>                                        
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
                            <button id="btnActivateDeletedAccept" type="button" runat="server" class="btn btn-primary btnAjaxAction" onserverclick="BtnActivateDeletedAccept_ServerClick" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>'>
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

    <%--  Modal for Add and Edit Years off AcademicDegree Coursing Year --%>
    <div class="modal fade" id="CoursingYearMaintenanceDialog" tabindex="-1" role="dialog" aria-labelledby="CoursingYearMaintenanceDialogTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" id="btnCloseCoursingYear" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title text-primary" id="CoursingYearDialogTitle"></h3>
                </div>
                <asp:UpdatePanel runat="server" ID="uppAcademicCoursingYear">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="form-horizontal">
                                <asp:HiddenField ID="hdfAcademicDegreeCodeYearCoursing" runat="server" Value="" /> 
                                <div class="form-group ES">
                                    <div class="col-sm-4">                                        
                                       <label for="<%= txtAcademicDegreeNameEs.ClientID%>" class="control-label "><%=GetLocalResourceObject("lblAcademicDegreeName")%></label></div>
                                    <div class="col-sm-8 text-left">
                                         <asp:TextBox ID="txtAcademicDegreeNameEs" CssClass="form-control cleanPasteText" runat="server"  TextMode="SingleLine" ReadOnly="true"></asp:TextBox>                                      
                                    </div>                                    
                                </div>
                              <div class="form-group EN">
                                    <div class="col-sm-4">                                        
                                       <label for="<%= txtAcademicDegreeNameEn.ClientID%>" class="control-label "><%=GetLocalResourceObject("lblAcademicDegreeName")%></label></div>
                                    <div class="col-sm-8 text-left">
                                         <asp:TextBox ID="txtAcademicDegreeNameEn" CssClass="form-control cleanPasteText" runat="server"  TextMode="SingleLine" ReadOnly="true"></asp:TextBox>                                      
                                    </div>                                    
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-4">                                        
                                       <label for="<%= txtAcademicCoursingYearDialog.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblAcademicDegreeYear")%></label></div>
                                    <div class="col-sm-8 text-left">
                                         <asp:TextBox ID="txtAcademicCoursingYearDialog" CssClass="form-control cleanPasteDigits"  runat="server" onkeypress="return isNumber(event);" onkeyUp="return isNumber(event) && checkMaxLength(this,event,10);"  MaxLength="2" TextMode="SingleLine"></asp:TextBox>                                      
                                         <label id="txtAcademicCoursingYearDialogValidation" for="<%= txtAcademicCoursingYearDialog.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgtxtAcademicYearDialogValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                   </div>                                    
                                </div>
                                <div class="form-group" hidden="hidden">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= chkCoursing.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCoursingYear")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:CheckBox ID="chkCoursing" Checked="true" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= chkReadWrite.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblchkReadWrite")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:CheckBox ID="chkReadWrite" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-2"> 
                                        </div> 
                                    <div class="col-sm-4">                                        
                                       <button id="btnAddYear" type="button" runat="server" onserverclick="BtnAddCoursingYear_ServerClick" onclick="return ProcessAcceptRequestYear(this.id);" class="btn btn-primary btnAjaxAction" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogAddYear"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogAddYear"))%>'>
                                            <span class="glyphicon glyphicon-ok glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnDialogAddYear")) %>    
                                       </button>
                                        </div> 
                                    <div class="col-sm-4">
                                       <button id="btnDeleteYear" type="button" runat="server" onserverclick="BtnDeleteCoursingYear_ServerClick" class="btn btn-primary btnAjaxAction"  data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogDeleteYear"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogDeleteYear"))%>'>
                                            <span class="glyphicon glyphicon-remove glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnDialogDeleteYear")) %>    
                                       </button>
                                    </div>                                    
                                </div>
                                <div  class="form-group">
                                    <div id="CurseandoDiv" class="col-sm-12">
                                        <asp:GridView ID="grvYearCoursing" Width="100%" runat="server" EmptyDataText='<%$ Code:GetLocalResourceObject("lblNoData") %>' 
                                            EmptyDataRowStyle-CssClass="emptyRow" AllowPaging="false" PagerSettings-Visible="false" AllowSorting="false"
                                            AutoGenerateColumns="false" ShowHeader="true" CssClass="table table-striped table-hover table-bordered"
                                            DataKeyNames="AcademicYear" OnPreRender="GrvYearCoursing_PreRender" >                             
                                            <Columns>                                        
                                                <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" >
                                                    <HeaderTemplate>
                                                        <div style="width:100%; text-align:center;">
                                                            <asp:LinkButton ID="AcademicYearSort" runat="server" CommandName="Sort" CommandArgument="AcademicYear" OnClientClick="SetWaitingGrvList(true);">          
                                                                <span><%= GetLocalResourceObject("AcademicYearCoursing.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "AcademicYear") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                            </asp:LinkButton>
                                                        </div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div style="width:100%; text-align:center;">
                                                            <span id="dvAcademicYearCode" data-id="AcademicYear" data-value="<%# Eval("AcademicYear") %>"><%# Eval("AcademicYear") %></span>
                                                         </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField> 
                                                 <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle hidden" HeaderStyle-CssClass="hidden" FooterStyle-CssClass="hidden">
                                                    <HeaderTemplate>
                                                        <div style="width:100%; text-align:center;">
                                                            <asp:LinkButton ID="ChkRWSort" runat="server" CommandName="Sort" CommandArgument="ReadAndWrite" OnClientClick="SetWaitingGrvList(true);">          
                                                                <span><%= GetLocalResourceObject("AcademicYearNotCoursing.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "AcademicYear") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                            </asp:LinkButton>
                                                        </div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span id="dvReadAndWrite" data-id="AcademicYear" data-value="<%# Eval("ReadAndWrite") %>"><%# Eval("ReadAndWrite") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>           
                                        <div id="grvListCoursingWaiting" style="position: absolute; margin: auto; display: none; bottom: 0px; top: 0px; left: 50%;transform: translate(-50%, 35%);">
                                            <span class='fa fa-spinner fa-spin' style="font-size:50px;"></span>
                                        </div>                                      
                                     </div>                                 
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">                            
                            <button id="btnSaveCoursingYear" type="button" runat="server" class="btn btn-primary btnAjaxAction"  data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>'>
                                <span class="glyphicon glyphicon-ok glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnDialogSave")) %>
                            </button>
                            <button id="btnCancelCoursingYear" runat="server" type="button" class="btn btn-default" >
                                <span class="glyphicon glyphicon-remove glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnDialogCancel")) %>
                            </button>                            
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>  
    <%--  Modal for Add and Edit Years off AcademicDegree NOT Coursing Year --%>
    <div class="modal fade" id="NotCoursingYearMaintenanceDialog" tabindex="-1" role="dialog" aria-labelledby="NotCoursingYearMaintenanceDialogTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" id="btnCloseYear" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title text-primary" id="NotCoursingYearDialogTitle"></h3>
                </div>
                <asp:UpdatePanel runat="server" ID="uppNotCoursingYear">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="form-horizontal">
                                <asp:HiddenField ID="hdfAcademicDegreeCodeYearNotCoursing" runat="server" Value="" /> 
                                <div class="form-group ES">
                                    <div class="col-sm-4">                                        
                                       <label for="<%= txtAcademicDegreeNameEsNotCoursing.ClientID%>" class="control-label "><%=GetLocalResourceObject("lblAcademicDegreeName")%></label></div>
                                    <div class="col-sm-8 text-left">
                                         <asp:TextBox ID="txtAcademicDegreeNameEsNotCoursing" CssClass="form-control cleanPasteText" runat="server"  TextMode="SingleLine" ReadOnly="true"></asp:TextBox>                                      
                                    </div>                                    
                                </div>
                              <div class="form-group EN">
                                    <div class="col-sm-4">                                        
                                       <label for="<%= txtAcademicDegreeNameEnNotCoursing.ClientID%>" class="control-label "><%=GetLocalResourceObject("lblAcademicDegreeName")%></label></div>
                                    <div class="col-sm-8 text-left">
                                         <asp:TextBox ID="txtAcademicDegreeNameEnNotCoursing" CssClass="form-control cleanPasteText" runat="server"  TextMode="SingleLine" ReadOnly="true"></asp:TextBox>                                      
                                    </div>                                    
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-4">                                        
                                       <label for="<%= txtAcademicNotCoursingYearDialog.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblAcademicDegreeYear")%></label></div>
                                    <div class="col-sm-8 text-left">
                                         <asp:TextBox ID="txtAcademicNotCoursingYearDialog" CssClass="form-control cleanPasteDigits"  runat="server" onkeypress="return isNumber(event);" onkeyUp="return isNumber(event) && checkMaxLength(this,event,10);" MaxLength="2" TextMode="SingleLine"></asp:TextBox>                                      
                                         <label id="txtAcademicNotCoursingYearDialogValidation" for="<%= txtAcademicNotCoursingYearDialog.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgtxtAcademicYearDialogValidation") %>" style="display:none; float: right;margin-right: 6px;margin-top: -23px;position: relative;z-index: 2;">!</label>
                                   </div>                                    
                                </div>
                                <div class="form-group" hidden="hidden">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= chkNotCursing.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblCoursingYear")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:CheckBox ID="chkNotCursing" Checked="false" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="<%= chkReadWriteNotCoursing.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblchkReadWrite")%></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:CheckBox ID="chkReadWriteNotCoursing" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-2"> 
                                        </div> 
                                    <div class="col-sm-4">                                        
                                       <button id="btnAddYearNotCoursing" type="button" runat="server" onserverclick="BtnAddNotCoursingYear_ServerClick" onclick="return ProcessAcceptRequestNotYear(this.id);" class="btn btn-primary btnAjaxAction" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogAddYear"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogAddYear"))%>'>
                                            <span class="glyphicon glyphicon-ok glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnDialogAddYear")) %>    
                                       </button>
                                        </div> 
                                    <div class="col-sm-4">
                                       <button id="btnDeleteYearNotCoursing" type="button" runat="server" onserverclick="BtnDeleteNotCoursingYear_ServerClick" class="btn btn-primary btnAjaxAction"  data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogDeleteYear"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogDeleteYear"))%>'>
                                            <span class="glyphicon glyphicon-remove glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnDialogDeleteYear")) %>    
                                       </button>
                                    </div>                                    
                                </div>
                                <div  class="form-group">
                                    <div id="notCoursingDiv" class="col-sm-12">
                                        <asp:GridView ID="grvNotCoursingYear" Width="100%" runat="server" EmptyDataText='<%$ Code:GetLocalResourceObject("lblNoData") %>' 
                                            EmptyDataRowStyle-CssClass="emptyRow" AllowPaging="false" PagerSettings-Visible="false" AllowSorting="false"
                                            AutoGenerateColumns="false" ShowHeader="true" CssClass="table table-striped table-hover table-bordered"
                                            DataKeyNames="AcademicYear" OnPreRender="GrvNotCoursingYear_PreRender" >                             
                                            <Columns>                                        
                                                <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle" >
                                                    <HeaderTemplate>
                                                        <div style="width:100%; text-align:center;">
                                                            <asp:LinkButton ID="AcademicYearSort" runat="server" CommandName="Sort" CommandArgument="AcademicYear" OnClientClick="SetWaitingGrvList(true);">          
                                                                <span><%= GetLocalResourceObject("AcademicYearNotCoursing.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "AcademicYear") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                            </asp:LinkButton>
                                                        </div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div style="width:100%; text-align:center;">
                                                            <span id="dvAcademicYearCode" data-id="AcademicYear" data-value="<%# Eval("AcademicYear") %>"><%# Eval("AcademicYear") %></span>
                                                         </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>  
                                                 <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle hidden" HeaderStyle-CssClass="hidden" FooterStyle-CssClass="hidden" >
                                                    <HeaderTemplate>
                                                        <div style="width:100%; text-align:center;">
                                                            <asp:LinkButton ID="ChkRWSort" runat="server" CommandName="Sort" CommandArgument="ReadAndWrite" OnClientClick="SetWaitingGrvList(true);">          
                                                                <span><%= GetLocalResourceObject("AcademicYearNotCoursing.HeaderText") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "AcademicYear") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                            </asp:LinkButton>
                                                        </div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span id="dvReadAndWrite" data-id="AcademicYear" data-value="<%# Eval("ReadAndWrite") %>"><%# Eval("ReadAndWrite") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>  
                                            </Columns>
                                        </asp:GridView>           
                                        <div id="grvListNotCoursingWaiting" style="position: absolute; margin: auto; display: none; bottom: 0px; top: 0px; left: 50%;transform: translate(-50%, 35%);">
                                            <span class='fa fa-spinner fa-spin' style="font-size:50px;"></span>
                                        </div>                                          
                                    </div>                                    
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">                            
                            <button id="btnSaveNotCoursingYear" type="button" runat="server" class="btn btn-primary btnAjaxAction"  data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnDialogSave"))%>'>
                                <span class="glyphicon glyphicon-ok glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnDialogSave")) %>
                            </button>
                            <button id="btnCancelNotCoursingYear" runat="server" type="button" class="btn btn-default" >
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
                if (!$(this).hasClass('emptyRow')) {
                    $(this).toggleClass('info').siblings().removeClass('info');
                    $('#<%=hdfSelectedRowIndex.ClientID%>').val($(this).hasClass("info") ? $(this).index() : -1);
                }
            });

            //And the grvYearCoursing selection row functionality
            $('#<%= grvYearCoursing.ClientID %>').on('click', 'tbody tr', function (event) {
                if (!$(this).hasClass('emptyRow')) {
                    $(this).toggleClass('info').siblings().removeClass('info');
                    $('#<%=txtAcademicCoursingYearDialog.ClientID%>').val($(this).hasClass("info") ? this.cells[0].children[0].innerText : "");
                    if (this.cells[1].children[0].innerText === 'False') {
                        $('#<%=chkReadWrite.ClientID%>').bootstrapToggle('off');
                    } else {
                        $('#<%=chkReadWrite.ClientID%>').bootstrapToggle('on');
                    }   
                }
            });

            //And the grvYearNotCoursing selection row functionality
            $('#<%= grvNotCoursingYear.ClientID %>').on('click', 'tbody tr', function (event) {
                if (!$(this).hasClass('emptyRow')) {
                    $(this).toggleClass('info').siblings().removeClass('info');
                    $('#<%=txtAcademicNotCoursingYearDialog.ClientID%>').val($(this).hasClass("info") ? $(this)[0].innerText : "");
                    if (this.cells[1].children[0].innerText === 'False') {
                        $('#<%=chkReadWriteNotCoursing.ClientID%>').bootstrapToggle('off');
                    } else {
                        $('#<%=chkReadWriteNotCoursing.ClientID%>').bootstrapToggle('on');
                    }                   
                }
            });

            //And the grvList pager event for client side
            $('#<%= blstPager.ClientID %>').on('click', 'a', function (event) {
                SetWaitingGrvList(true);
            });
            $('#<%= btnSearch.ClientID %>').on('click', function (event) {
                var $this = $('#<%= btnSearch.ClientID %>');
                $this.button('loading');
                setTimeout(function () { $this.button('reset'); }, 30000);
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
            $('#btnActivateDeletedCancel, #btnActivateDeletedClose').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>
                $("#<%=hdfAcademicDegreeCodeEdit.ClientID%>").val("-1");
                event.preventDefault();                    
                $('#MaintenanceDialog').modal('show');
                $('#ActivateDeletedDialog').modal('hide');                
            });

            $('#<%= btnCancelCoursingYear.ClientID %>').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>
                $("#<%=hdfAcademicDegreeCodeEdit.ClientID%>").val("-1");
                event.preventDefault();
                ClearModalForm();
                DisableButtonsDialog();
                $('#CoursingYearMaintenanceDialog').modal('hide');
                EnableButtonsDialog();                
            });
            $('#<%= btnSaveCoursingYear.ClientID %>').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>
                 $("#<%=hdfAcademicDegreeCodeEdit.ClientID%>").val("-1");
                 event.preventDefault();
                 ClearModalForm();
                 DisableButtonsDialog();
                $('#CoursingYearMaintenanceDialog').modal('hide');
                 EnableButtonsDialog();
            });

            $('#<%= btnCancelNotCoursingYear.ClientID %>').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>
                $("#<%=hdfAcademicDegreeCodeEdit.ClientID%>").val("-1");
                event.preventDefault();
                ClearModalForm();
                DisableButtonsDialog();
                $('#NotCoursingYearMaintenanceDialog').modal('hide');
                EnableButtonsDialog();                
            });
            $('#<%= btnSaveNotCoursingYear.ClientID %>').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>
                $("#<%=hdfAcademicDegreeCodeEdit.ClientID%>").val("-1");
                event.preventDefault();
                ClearModalForm();
                DisableButtonsDialog();
                $('#NotCoursingYearMaintenanceDialog').modal('hide');
                EnableButtonsDialog();
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

            //And the clean paste cache manager
            $('.cleanPasteDigits').on('change', function (e) {
                debugger;
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
            var CultureGlobal = "<%= CultureGlobal %>";
            //In this section we initialize the textbox relations the culture
            if (CultureGlobal == "en-US") {
                $(".ES").remove();
            }
            else if (CultureGlobal == "es-CR") {
                $(".EN").remove();
            }
            //////////////////////////////////////////////////////////////////////////////////////////////////


            //////////////////////////////////////////////////////////////////////////////////////////////////
            //In this section we initialize the checkbox toogles

            $('#<%= chkSearchEnabled.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                off: '<%= GetLocalResourceObject("No") %>'
            });
            $('#<%= chkReadWrite.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                 off: '<%= GetLocalResourceObject("No") %>'
            });
            $('#<%= chkReadWriteNotCoursing.ClientID %>').bootstrapToggle({
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

            $('#<%= chkCoursing.ClientID %>').bootstrapToggle({
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
        }

        //*******************************//
        //          VALIDATION           // 
        //*******************************//

        var validator = null;
        function ValidateForm() {
            /// <summary>Validate the form with jquey validation plugin </summary>
            /// <returns> True if form is valid. False otherwise. </returns>     

            //add custom validation methods
            jQuery.validator.addMethod("validSelection", function (value, element) {
                return this.optional(element) || value != "-1";
            }, "Please select a valid option");

            jQuery.validator.addMethod("validNumber", function (value, element) {
                if (Number.isInteger(parseInt(value))) {
                    if (parseInt(value) >= 0) {
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    return false;
                }
            }, "Please select a valid option");

            if (validator == null) {
                //add custom validation methods
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
                            <%= txtAcademicDegreeNameSpanish.UniqueID %>: {
                                required: true,
                                normalizer: function(value) {                                 
                                    return $.trim(value);
                                },
                                minlength: 1,
                                maxlength: 150
                            },
                            <%= txtAcademicDegreeNameEnglish.UniqueID %>: {
                                required: true,
                                normalizer: function(value) {
                                    return $.trim(value);
                                },
                                minlength: 1,
                                maxlength: 150
                            },
                            <%= txtOrderList.UniqueID %>: {
                                required: true,
                                validNumber: true
                            },
                            <%= txtAcademicNotCoursingYearDialog.UniqueID %>: {
                            required: true,
                            validNumber: true
                            },
                            <%= txtAcademicCoursingYearDialog.UniqueID %>: {
                            required: true,
                            validNumber: true
                            },
                             "<%= cboDegreeFormationType.UniqueID %>": {
                            required: true
                            , validSelection: true
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
            $("#<%=hdfAcademicDegreeCodeEdit.ClientID%>").val("-1");
            $("#<%=txtAcademicDegreeNameSpanish.ClientID%>").val("");
            $("#<%=txtAcademicDegreeNameEnglish.ClientID%>").val("");
            $("#<%=txtOrderList.ClientID%>").val("");
            $("#<%=cboDegreeFormationType.ClientID%>").val("-1");
            $("#<%= cboDegreeFormationType.ClientID %>").selectpicker('refresh');
            $("#<%=chkSearchEnabled.ClientID%>").bootstrapToggle('on');            
            if (validator != null) {
                validator.resetForm();
            }                
        }
        function ClearModalCoursingYearForm() {
            /// <summary>Clear the modal form</summary>               
            $("#<%=txtAcademicCoursingYearDialog.ClientID%>").val("");
            $("#<%=chkReadWrite.ClientID%>").bootstrapToggle('on');
            if (validator != null) {
                validator.resetForm();
            }
        }
        function ClearModalNotCoursingYearForm() {
            /// <summary>Clear the modal form</summary>               
            $("#<%=txtAcademicNotCoursingYearDialog.ClientID%>").val("");
            $("#<%=chkReadWrite.ClientID%>").bootstrapToggle('on');
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
        function ProcessEditCoursingYearRequest(resetId) {
            /// <summary>Process the edit request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param> 
            $(".btns").prop("disabled", true);
            if (IsRowSelected()) {
                ClearModalCoursingYearForm();
                __doPostBack('<%= btnCoursingYearEdit.UniqueID %>', '');
                return true;
            }
            else {
                ShowFooterError('<%= GetLocalResourceObject("msgInvalidSelection") %>');
                ErrorButton(resetId);
                $(".btns").prop("disabled", false);
                return false;
            }
        }
        function ProcessEditNotCoursingYearRequest(resetId) {
            /// <summary>Process the edit request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param> 
            $(".btns").prop("disabled", true);
            if (IsRowSelected()) {
                ClearModalNotCoursingYearForm();
                __doPostBack('<%= btnNotCoursingYearEdit.UniqueID %>', '');
                return true;
            }
            else {
                ShowFooterError('<%= GetLocalResourceObject("msgInvalidSelection") %>');
                ErrorButton(resetId);
                $(".btns").prop("disabled", false);
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
        function ProcessAcceptRequestYear(resetId) {
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
                __doPostBack('<%= btnAddYear.UniqueID %>', '');
            }
            return false;
        }

        function clearTextField() {
            $('#<%=txtAcademicCoursingYearDialog.ClientID %>').val('');
            $('#<%=txtAcademicNotCoursingYearDialog.ClientID %>').val('');  
        }

        

        function ProcessAcceptRequestNotYear(resetId) {
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
                 __doPostBack('<%= btnAddYearNotCoursing.UniqueID %>', '');
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
        function ReturnFromBtnEditClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $(".btns").prop("disabled",true);
            SetRowSelected();
            DisableToolBar();
            $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#dialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#MaintenanceDialog').modal('show');
            EnableToolBar();
            $(".btns").prop("disabled",false);
        }
        function ReturnFromBtnEditCoursingYearClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $(".btns").prop("disabled", true);
            SetRowSelected();
            DisableToolBar();
            $('#CoursingYearMaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceCoursingYear") %>');
            $('#CoursingYearDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceCoursingYear") %>');
            $('#CoursingYearMaintenanceDialog').modal('show');
            EnableToolBar();
            $(".btns").prop("disabled", false);
        }
        function ReturnFromBtnEditNotCoursingYearClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $(".btns").prop("disabled", true);
            SetRowSelected();
            DisableToolBar();
            $('#NotCoursingYearMaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceNotCoursingYear") %>');
            $('#NotCoursingYearDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceNotCoursingYear") %>');
            $('#NotCoursingYearMaintenanceDialog').modal('show');
             EnableToolBar();
             $(".btns").prop("disabled", false);
         }
        function ReturnFromBtnAcceptClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            DisableButtonsDialog();
            SetRowSelected();
            
            $('#MaintenanceDialog').modal('hide');
            EnableButtonsDialog();
            ShowFooterSuccess('<%= GetLocalResourceObject("msgOperationSuccesfull") %>');
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
            EnableButtonsDialog();
            ShowFooterSuccess('<%= GetLocalResourceObject("msgOperationSuccesfull") %>');
            EnableToolBar();
        }
        function FocusYear() {
            ///<summary>Enabled all buttons toolbar</summary>
            $('#<%= txtAcademicCoursingYearDialog.ClientID %>').focus();
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
            clearTextField();
            if (prm.get_isInAsyncPostBack()) {                
                SetWaitingGrvList(false)
            }
        }
    </script>
</asp:Content>