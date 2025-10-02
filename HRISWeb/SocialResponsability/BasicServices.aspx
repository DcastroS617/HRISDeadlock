<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="BasicServices.aspx.cs" Inherits="HRISWeb.SocialResponsability.BasicServices" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntHeader" runat="server">
    <title><%=Convert.ToString(GetLocalResourceObject("lblScreenTitle")) %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cntBody" runat="server">

    <style>
        .bootstrap-select > .dropdown-toggle.bs-placeholder, .bootstrap-select > .dropdown-toggle.bs-placeholder:active,
        .bootstrap-select > .dropdown-toggle.bs-placeholder:focus, .bootstrap-select > .dropdown-toggle.bs-placeholder:hover {
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
                                        <a class="nav-link active" id="basicservices-tab" data-toggle="tab" href="#basicservices" role="tab" aria-controls="basicservices" aria-selected="true"><%= GetLocalResourceObject("basicservices-tab") %></a>
                                    </li>
                                </ul>
                                <div class="tab-content" id="survey">
                                    <div class="tab-pane fade in active" id="basicservices" role="tabpanel" aria-labelledby="basicservices-tab">
                                        <p>
                                            <br />
                                        </p>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-3">
                                                            <asp:Label ID="lblBasicServices" meta:resourcekey="lblBasicServices" AssociatedControlID="cboBasicServices" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <asp:DropDownList ID="cboBasicServices" runat="server" CssClass="form-control selectpicker" data-live-search="true" AutoPostBack="false" multiple="multiple" data-selected-text-format="count > 5"></asp:DropDownList>
                                                            <label id="cboBasicServicesValidation" for="<%= cboBasicServices.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgBasicServicesValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                            <asp:HiddenField ID="hdfSelectdBasicServices" runat="server" />
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <span class="glyphicon glyphicon-info-sign text-center" data-toggle="tooltip" title='<%=GetLocalResourceObject("MultiSelectionDropDownListTooltip")%>' style="cursor: pointer"></span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-11 col-sm-offset-1">
                                                            <asp:Label ID="lblAvailabilityOfServices" meta:resourcekey="lblAvailabilityOfServices" AssociatedControlID="hdfAvailabilityOfServices" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                            <asp:HiddenField ID="hdfAvailabilityOfServices" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-8 col-sm-offset-1">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-12">
                                                            <div class="table-responsive">
                                                                <asp:Repeater ID="rptServicesAvailability" runat="server" OnItemDataBound="rptServicesAvailability_ItemDataBound">
                                                                    <HeaderTemplate>
                                                                        <table class="table table-bordered table-striped table-hover" id="tbServicesAvailability">
                                                                            <thead>
                                                                                <tr>
                                                                                    <th class="hidden">Code</th>
                                                                                    <th style="width: 50%!important"><%= GetLocalResourceObject("rptServicesAvailability.Service.Header") %></th>
                                                                                    <th style="width: 50%!important"><%= GetLocalResourceObject("rptServicesAvailability.Availability.Header") %></th>
                                                                                </tr>
                                                                            </thead>
                                                                            <tbody>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <tr>
                                                                            <td class="hidden">
                                                                                <asp:Label ID="lblBasicServiceCode" runat="server" Text='<%#Eval("BasicServiceCode")%>'></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblServiceDescription" CssClass="control-label text-left" runat="server" Text='<%#Eval("BasicServiceDescriptionByCurrentCulture")%>'></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="cboServicesAvailability" runat="server" CssClass="form-control" AutoPostBack="false"></asp:DropDownList>
                                                                                <label id="cboServicesAvailabilityValidation" for="<%# Container.FindControl("cboServicesAvailability").ClientID %>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgServicesAvailabilityValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
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
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-3">
                                                            <asp:Label ID="lblOtherServices" meta:resourcekey="lblOtherServices" AssociatedControlID="cboOtherServices" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <asp:DropDownList ID="cboOtherServices" runat="server" CssClass="form-control selectpicker" data-live-search="true" AutoPostBack="false" multiple="multiple" data-selected-text-format="count > 5"></asp:DropDownList>
                                                            <label id="cboOtherServicesValidation" for="<%= cboOtherServices.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgOtherServicesValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                            <asp:HiddenField ID="hdfSelectedOtherServices" runat="server" />
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <span class="glyphicon glyphicon-info-sign text-center" data-toggle="tooltip" title='<%=GetLocalResourceObject("MultiSelectionDropDownListTooltip")%>' style="cursor: pointer"></span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-6 col-sm-offset-1">
                                                             <div class="col-xs-1 ml-auto mt-auto mb-auto">
                                                                <img  src="../Content/images/bulb_icon.png" />
                                                            </div>
                                                            <div class="col-sm-10 mr-auto mt-auto mb-auto">  
                                                                <asp:Label ID="lblUseInternet" meta:resourcekey="lblUseInternet" AssociatedControlID="chkUseInternet" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <asp:CheckBox ID="chkUseInternet" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
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
                                                            <img alt="" src="../Content/images/bulb_icon.png" /> 
                                                            <asp:Label ID="lblGarbageDisposal" meta:resourcekey="lblGarbageDisposal" AssociatedControlID="cboGarbageDisposal" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <asp:DropDownList ID="cboGarbageDisposal" runat="server" CssClass="form-control selectpicker" data-live-search="true" AutoPostBack="false" multiple="multiple" data-selected-text-format="count > 5"></asp:DropDownList>
                                                            <label id="cboGarbageDisposalValidation" for="<%= cboGarbageDisposal.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgGarbageDisposalValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                            <asp:HiddenField ID="hdfSelectedGarbageDisposal" runat="server" />
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <span class="glyphicon glyphicon-info-sign text-center" data-toggle="tooltip" title='<%=GetLocalResourceObject("MultiSelectionDropDownListTooltip")%>' style="cursor: pointer"></span>
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
                                                            <img alt="" src="../Content/images/bulb_icon.png" /> 
                                                            <asp:Label ID="lblWaterSupply" meta:resourcekey="lblWaterSupply" AssociatedControlID="cboWaterSupply" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <asp:DropDownList ID="cboWaterSupply" runat="server" CssClass="form-control selectpicker" data-live-search="true" AutoPostBack="false" multiple="multiple" data-selected-text-format="count > 5"></asp:DropDownList>
                                                            <label id="cboWaterSupplyValidation" for="<%= cboWaterSupply.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgWaterSupplyValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                            <asp:HiddenField ID="hdfSelectedWaterSuppliers" runat="server" />
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <span class="glyphicon glyphicon-info-sign text-center" data-toggle="tooltip" title='<%=GetLocalResourceObject("MultiSelectionDropDownListTooltip")%>' style="cursor: pointer"></span>
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
                                                            <img alt="" src="../Content/images/bulb_icon.png" /> 
                                                            <asp:Label ID="lblToilet" meta:resourcekey="lblToilet" AssociatedControlID="cboBasicServices" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <asp:DropDownList ID="cboToilet" runat="server" CssClass="form-control selectpicker" data-live-search="true" AutoPostBack="false"></asp:DropDownList>
                                                            <label id="cboToiletValidation" for="<%= cboToilet.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgToiletValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-sm-6 col-sm-offset-1">
                                                             <div class="col-xs-1 ml-auto mt-auto mb-auto">
                                                                <img  src="../Content/images/bulb_icon.png" />
                                                            </div>
                                                            <div class="col-sm-10 mr-auto mt-auto mb-auto"> 
                                                                <asp:Label ID="lblToiletShare" meta:resourcekey="lblToiletShare" AssociatedControlID="chkToiletShare" runat="server" Text="" CssClass="control-label text-left"> <%=GetLocalResourceObject("lblToiletShare.Text")%></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <asp:CheckBox ID="chkToiletShare" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
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
                                                            <img alt="" src="../Content/images/bulb_icon.png" /> 
                                                            <asp:Label ID="lblCookEnergyType" meta:resourcekey="lblCookEnergyType" AssociatedControlID="cboCookEnergyType" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <asp:DropDownList ID="cboCookEnergyType" runat="server" CssClass="form-control selectpicker" data-live-search="true" AutoPostBack="false" multiple="multiple" data-selected-text-format="count > 3"></asp:DropDownList>
                                                            <label id="cboCookEnergyTypeValidation" for="<%= cboCookEnergyType.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgCookEnergyTypeValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                            <asp:HiddenField ID="hdfSelectedCookEnergyTypes" runat="server" />
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <span class="glyphicon glyphicon-info-sign text-center" data-toggle="tooltip" title='<%=GetLocalResourceObject("MultiSelectionDropDownListTooltip")%>' style="cursor: pointer"></span>
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
                                                                <asp:Label ID="lblOtherCookEnergyType" meta:resourcekey="lblOtherCookEnergyType" AssociatedControlID="txtOtherCookEnergyType" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <asp:TextBox ID="txtOtherCookEnergyType" meta:resourcekey="txtOtherCookEnergyType" runat="server" CssClass="form-control control-validation cleanPasteText" onkeypress="blockEnterKey();return isNumberOrLetter(event);" MaxLength="40" placeholder="" disabled="disabled"></asp:TextBox>
                                                            <label id="txtOtherCookEnergyTypeValidation" for="<%= txtOtherCookEnergyType.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgOtherCookEnergyTypeValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
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
                                                                <asp:Label ID="lblCoilAir" meta:resourcekey="lblCoilAir" AssociatedControlID="chkCoilAir" runat="server" Text="" CssClass="control-label text-left"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <asp:CheckBox ID="chkCoilAir" runat="server" data-toggle="toggle" data-on='<%=GetLocalResourceObject("Yes")%>' data-off='<%=GetLocalResourceObject("No")%>'></asp:CheckBox>
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
                                <asp:LinkButton ID="lbtnFinish" runat="server" CssClass="btn btn-default btnAjaxAction" OnClick="lbtnFinish_Click" OnClientClick="return ProcessFinishRequest(this.id);"
                                    data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("lbtnFinish.Text"))%>'
                                    data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("lbtnFinish.Text"))%>'>
                                    <span class="glyphicon glyphicon-ok glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span><br />
                                    <%= GetLocalResourceObject("lbtnFinish.Text") %>
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
        "use strict";
        function pageLoad(sender, args) {
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

            $('#<%= chkUseInternet.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                 off: '<%= GetLocalResourceObject("No") %>'
             });


            $('#<%= chkToiletShare.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                 off: '<%= GetLocalResourceObject("No") %>'
             });


            $('#<%= chkCoilAir.ClientID %>').bootstrapToggle({
                on: '<%= GetLocalResourceObject("Yes") %>',
                 off: '<%= GetLocalResourceObject("No") %>'
             });


         

            $("#<%= cboBasicServices.ClientID %>").on('changed.bs.select', function (e) {
                /// <summary>Handles the select's value changed event for the cboBasicServices control.</summary>
                MultiSelectDropdownListSaveSelectedItems($("#<%= cboBasicServices.ClientID %>"), $("#<%= hdfSelectdBasicServices.ClientID %>"));
            });
            $("#<%= cboOtherServices.ClientID %>").on('changed.bs.select', function (e) {
                /// <summary>Handles the select's value changed event for the cboOtherServices control.</summary>
                let selectedItems = $("#<%= cboOtherServices.ClientID %>").selectpicker('val');
                for (let index = 0; index < selectedItems.length; index++) {
                    if (selectedItems[index] === '1') {
                        $("#<%= cboOtherServices.ClientID %>").selectpicker('val', '1');

                        break;
                    }
                }
                MultiSelectDropdownListSaveSelectedItems($("#<%= cboOtherServices.ClientID %>"), $("#<%= hdfSelectedOtherServices.ClientID %>"));
            });
            $("#<%= cboGarbageDisposal.ClientID %>").on('changed.bs.select', function (e) {
                /// <summary>Handles the select's value changed event for the cboGarbageDisposal control.</summary>
                MultiSelectDropdownListSaveSelectedItems($("#<%= cboGarbageDisposal.ClientID %>"), $("#<%= hdfSelectedGarbageDisposal.ClientID %>"));
            });
            $("#<%= cboWaterSupply.ClientID %>").on('changed.bs.select', function (e) {
                /// <summary>Handles the select's value changed event for the cboWaterSupply control.</summary>
                MultiSelectDropdownListSaveSelectedItems($("#<%= cboWaterSupply.ClientID %>"), $("#<%= hdfSelectedWaterSuppliers.ClientID %>"));
            });

            const otherCookEnergyTypeCode = "5";
            /// <summary>Execute at load even at partial and ajax requests</summary> 
            $("#<%= cboCookEnergyType.ClientID %>").on('changed.bs.select', function (e) {
                /// <summary>Handles the select's value changed event for the cboCookEnergyType control.</summary>                
                var selectedItems = $("#<%= cboCookEnergyType.ClientID %>").selectpicker('val');
                var hasSpecialItem = false;
                for (let i = 0; i < selectedItems.length; i++) {
                    if (selectedItems[i] === otherCookEnergyTypeCode) {
                        hasSpecialItem = true;
                        break;
                    }
                }
                $('#<%= txtOtherCookEnergyType.ClientID %>').prop('disabled', !hasSpecialItem);
                if (!hasSpecialItem) {
                    $('#<%= txtOtherCookEnergyType.ClientID %>').val('');
                    SetControlValid($('#<%= txtOtherCookEnergyType.ClientID %>').attr("id"));
                }
                MultiSelectDropdownListSaveSelectedItems($("#<%= cboCookEnergyType.ClientID %>"), $("#<%= hdfSelectedCookEnergyTypes.ClientID %>"));
            });
        }
        function RestoreMultiSelectedValues() {
            /// <summary>Recover the selected items that were stored and assign them to the control to be selected.</summary>
            MultiSelectDropdownListRestoreSelectedItems($('#<%= cboBasicServices.ClientID %>'), $('#<%= hdfSelectdBasicServices.ClientID %>'));
            MultiSelectDropdownListRestoreSelectedItems($('#<%= cboOtherServices.ClientID %>'), $('#<%= hdfSelectedOtherServices.ClientID %>'));
            let selectedItems = $("#<%= cboOtherServices.ClientID %>").selectpicker('val');
            if (selectedItems.length === 0) {
                $("#<%= cboOtherServices.ClientID %>").selectpicker('val', '1');

            }
            MultiSelectDropdownListRestoreSelectedItems($('#<%= cboGarbageDisposal.ClientID %>'), $('#<%= hdfSelectedGarbageDisposal.ClientID %>'));
            MultiSelectDropdownListRestoreSelectedItems($('#<%= cboWaterSupply.ClientID %>'), $('#<%= hdfSelectedWaterSuppliers.ClientID %>'));
            MultiSelectDropdownListRestoreSelectedItems($('#<%= cboCookEnergyType.ClientID %>'), $('#<%= hdfSelectedCookEnergyTypes.ClientID %>'));
        }
        function ProcessBackRequest(resetId) {
            /// <summary>Process the request for the back button</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            disableButton($('#<%= lbtnSaveAsDraft.ClientID %>'));
            disableButton($('#<%= lbtnFinish.ClientID %>'));
            setTimeout(function () {
                ResetButton(resetId);
                enableButton($('#<%= lbtnSaveAsDraft.ClientID %>'));
                enableButton($('#<%= lbtnFinish.ClientID %>'));
            }, 10000);
            return true;
        }
        function ProcessFinishRequest(resetId) {
            /// <summary>Process the Finish request according to the validation</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            disableButton($('#<%= lbtnSaveAsDraft.ClientID %>'));
            disableButton($('#<%= lbtnBack.ClientID %>'));

            if (!ValidateSurvey()) {
                MostrarMensaje(TipoMensaje.VALIDACION, '<%= GetLocalResourceObject("msgInvalidEntry") %>', function () {
                    $('#<%= lbtnFinish.ClientID %>').button('reset');
                    enableButton($('#<%= lbtnSaveAsDraft.ClientID %>'));
                    enableButton($('#<%= lbtnBack.ClientID %>'));
                    return false;
                });
                return false;
            }
            // Show confirmation message for finish the survey
            MostrarConfirmacion('<%= GetLocalResourceObject("msjFinishSocialResponsbailitySurvey") %>'
                , '<%=GetLocalResourceObject("Yes")%>', function () {
                    __doPostBack('<%= lbtnFinish.UniqueID %>', '');
                return true;
            }
                , '<%=GetLocalResourceObject("No")%>', function () {
                    setTimeout(function () {
                        ResetButton(resetId);
                        enableButton($('#<%= lbtnSaveAsDraft.ClientID %>'));
                        enableButton($('#<%= lbtnBack.ClientID %>'));
                    }, 200);
                return false;
            }
            );
            return false;
        }
        function ProcessSaveAsDraftRequest(resetId) {
            /// <summary>Process the request for the save as draft button</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            disableButton($('#<%= lbtnBack.ClientID %>'));
            disableButton($('#<%= lbtnFinish.ClientID %>'));
            return true;
        }
        function ProcessFinishResponse() {
            /// <summary>Process the response for the save as draft</summary>
            disableButton($('#<%= lbtnSaveAsDraft.ClientID %>'));
            disableButton($('#<%= lbtnBack.ClientID %>'));
            $('#<%= lbtnFinish.ClientID %>').button('loading');

            MostrarMensaje(TipoMensaje.INFORMACION, '<%= GetLocalResourceObject("msgSurveyFinished") %>', function () {
                //history.clear();
                window.location.href = "../Default.aspx";
                setTimeout(function () {
                    ResetButton($('#<%= lbtnFinish.ClientID %>').id);
                    $('#<%= lbtnFinish.ClientID %>').button('reset');
                    enableButton($('#<%= lbtnBack.ClientID %>'));
                    enableButton($('#<%= lbtnSaveAsDraft.ClientID %>'));
                }, 10000);
            });
        }
        function ProcessSaveAsDraftResponse() {
            /// <summary>Process the response for the save as draft</summary>
            disableButton($('#<%= lbtnFinish.ClientID %>'));
            disableButton($('#<%= lbtnBack.ClientID %>'));
            $('#<%= lbtnSaveAsDraft.ClientID %>').button('loading');

            MostrarMensaje(TipoMensaje.INFORMACION, '<%= GetLocalResourceObject("msjSurveySavedAsDraft") %>', function () {
                setTimeout(function () {
                    $('#<%= lbtnSaveAsDraft.ClientID %>').button('reset');
                    ResetButton($('#<%= lbtnSaveAsDraft.ClientID %>').id);
                    enableButton($('#<%= lbtnBack.ClientID %>'));
                    enableButton($('#<%= lbtnFinish.ClientID %>'));
                }, 200);
            });
        }
        function SetControlInvalid(controlId) {
            /// <summary>Set the appearance of the control as invalid </summary>
            /// <param name="controlId" type="String">Id of the control</param>            
            if ($("#" + controlId).is(".selectpicker")) {
                $('button[data-id=' + controlId + '].dropdown-toggle').addClass("Invalid");
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
            if ($("#" + controlId).is(".selectpicker")) {
                $('button[data-id=' + controlId + '].dropdown-toggle').removeClass("Invalid");
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
                            <%= cboBasicServices.UniqueID %>: {
                    required: true
                        , validSelection: true
                },
                            <%= cboOtherServices.UniqueID %>: {
                    required: true
                        , validSelection: true
                },
                            <%= cboGarbageDisposal.UniqueID %>: {
                    required: true
                        , validSelection: true
                },
                            <%= cboWaterSupply.UniqueID %>: {
                    required: true
                        , validSelection: true
                },
                            <%= cboToilet.UniqueID %>: {
                    required: true
                        , validSelection: true
                },
                            <%= cboCookEnergyType.UniqueID %>: {
                    required: true
                        , validSelection: true
                },
                            <%= txtOtherCookEnergyType.UniqueID %>: {
                    required: true
                        , normalizer: function(value) {
                            return $.trim(value);
                        }
                                , minlength: 0
                        , maxlength: 40
                }
            }
        });

        $('[name*="cboServicesAvailability"]').each(function () {
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
