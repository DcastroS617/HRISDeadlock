<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ReportTraining.aspx.cs" Inherits="HRISWeb.Training.Report.ReportTraining" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntHeader" runat="server">
    <title id="TituloPantalla" runat="server"></title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cntBody" runat="server">
    <div id="content" class="main-content">
        <div class="row">
            <div class="col-12 col-md-10">
                <h1 class="text-left text-primary">
                    <label id="tituloReporte" class="PageTitle" runat="server"></label>
                </h1>
                <br />
            </div>

            <div class="col-6 col-md-2">
                <asp:LinkButton ID="lnkbtnAddInformation" runat="server" class="row justify-content-center" style="color: gray; cursor:default;" Enabled="false" Visible="false"><%= GetLocalResourceObject("lblAdditionalInformation") %> </asp:LinkButton>
            </div>
        </div>

        <div class="row">
            <div class="col-12">
                <div class="container">
                    <iframe id="Iframe1" class="iframeCollapsed" style="overflow: hidden; overflow-x: hidden; overflow-y: hidden; height: 100%; width: 100%; position: absolute; left: 0px; right: 0px;" target="_blank" frameborder="0" runat="server" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
