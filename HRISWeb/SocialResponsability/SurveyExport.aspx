<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="SurveyExport.aspx.cs" Inherits="HRISWeb.SocialResponsability.SurveyExport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntHeader" runat="server">
    <title><%=Convert.ToString(GetLocalResourceObject("lblScreenTitle")) %></title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cntBody" runat="server">
    <urlreport data-url="<%=UrlReport %>" data-usercode="<%=UserCodeSession %>" />

    <div id="content" class="main-content">
        <h1 class="text-left text-primary">
            <label class="PageTitle"><%= GetLocalResourceObject("lblScreenTitle") %></label>
        </h1>
        <br />

        <asp:UpdatePanel ID="Maincbo" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="cboDivisionCode" EventName="SelectedIndexChanged" />
            </Triggers>

            <ContentTemplate>
                <div class="container" style="width: 100%">
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-5 text-left">
                                        <label for="<%=cboDivisionCode.ClientID%>" class="control-label"><%= GetLocalResourceObject("lblDivision") %></label>
                                    </div>

                                    <div class="col-sm-7">
                                        <asp:DropDownList AutoPostBack="true" ID="cboDivisionCode" OnSelectedIndexChanged="cboDivisionCode_SelectedIndexChanged" CssClass="form-control cboAjaxAction control-validation" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-sm-4">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-5 text-left">
                                        <label for="<%=cboCompany.ClientID%>" class="control-label"><%= GetLocalResourceObject("lblCompanies") %> </label>
                                    </div>

                                    <div class="col-sm-7">
                                        <asp:DropDownList ID="cboCompany" CssClass="form-control cboAjaxAction control-validation" AutoPostBack="false" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-sm-4">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-5 text-left">
                                        <label for="cboLenguaje" class="control-label"><%= GetLocalResourceObject("lblLanguage") %> </label>
                                    </div>

                                    <div class="col-sm-7">
                                        <select id="cboLenguaje" runat="server" class="form-control control-validation">
                                            <option value="EN">Ingles</option>
                                            <option value="ES">Español</option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-sm-5">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="dtpFechaDesde" class="control-label"><%= GetLocalResourceObject("lblFdesde") %></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <div class="input-group">
                                            <asp:TextBox runat="server" ID="dtpFechaDesde" class="dateinput form-control date control-validation cleanPasteDigits" type="text" autocomplete="off" />
                                            <label id="dtpFechaDesdelb" for="<%= dtpFechaDesde.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left"
                                                style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">
                                                !</label>
                                            <div class="input-group-addon">
                                                <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-sm-5">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-4 text-left">
                                        <label for="dtpFechaHasta" class="control-label"><%= GetLocalResourceObject("lblFHasta") %></label>
                                    </div>

                                    <div class="col-sm-8">
                                        <div class="input-group">
                                            <asp:TextBox runat="server" ID="dtpFechaHasta" class="dateinput form-control date control-validation cleanPasteDigits" type="text" autocomplete="off" />

                                            <label id="dtpFechaHastalbl" for="<%= dtpFechaHasta.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left"
                                                style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">
                                                !</label>
                                            <div class="input-group-addon">
                                                <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-sm-12 text-center">
                            <button id="btnExportar" type="button" class="btn btn-default btnAjaxAction" data-loading-text='<span class="fa fa-spinner fa-spin glyphicon-main-button"></span> &nbsp;<%= GetLocalResourceObject("btnExport") %>'>
                                <span class="glyphicon glyphicon-save-file  glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;
                                    <%= GetLocalResourceObject("btnExport") %>
                            </button>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript">
        function pageLoad(sender, args) {
            $(document).ready(function () {
                $(".btnAjaxAction").click(function () {
                    var $this = this;
                    $($this).button('loading');

                    setTimeout(function () {
                        $($this).button('reset');
                    }, 3000);
                });

                $('.dateinput').datetimepicker({
                    format: 'MM/DD/YYYY'
                });

                $('.dateinput').on("blur", function () {
                    var ValidDateVal = $(this).data("DateTimePicker").date().format('MM/DD/YYYY');
                    $(this).val(ValidDateVal);
                });

                $("#btnExportar").click(function () {
                    var FechaDesde = $("#<%=dtpFechaDesde.ClientID%>").val();
                    var FechaHasta = $("#<%=dtpFechaHasta.ClientID%>").val();
                    if (FechaDesde != '' && FechaHasta != '') {
                        var obj = {};
                        if ($("#<%=cboDivisionCode.ClientID%>").selectpicker("val") != "")
                            obj.DivisionCode = $("#<%=cboDivisionCode.ClientID%>").selectpicker("val");
                        if ($("#<%=cboCompany.ClientID%>").selectpicker("val") != "")
                            obj.CompanyCode = $("#<%=cboCompany.ClientID%>").selectpicker("val");

                        obj.FechaDesde = $("#<%=dtpFechaDesde.ClientID%>").data("DateTimePicker").date().format('YYYY-MM-DD');
                        obj.FechaHasta = $("#<%=dtpFechaHasta.ClientID%>").data("DateTimePicker").date().format('YYYY-MM-DD');

                        obj.Lang = $("#<%=cboLenguaje.ClientID%>").val();
                        obj.UserCode = $("urlreport").data("usercode");

                        var url = $("urlreport").data("url");

                        var rq = url + "&" + $.param(obj) + "&rs:Format=EXCELOPENXML";
                        console.log(rq);
                        window.location.href = rq;
                    } else {
                        MostrarMensaje(TipoMensaje.VALIDACION, '<%= GetLocalResourceObject("msgErrorParametros") %>', function () {
                            $('.btnAjaxAction').button('reset');

                            return false;
                        });

                    }
                });
            });
        }
    </script>
</asp:Content>
