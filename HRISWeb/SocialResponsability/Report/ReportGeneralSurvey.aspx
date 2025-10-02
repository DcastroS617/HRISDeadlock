<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ReportGeneralSurvey.aspx.cs" Inherits="HRISWeb.SocialResponsability.Report.ReportGeneralSurvey" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cntHeader" runat="server">
    <title><%=Convert.ToString(GetLocalResourceObject("lblScreenTitle")) %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cntBody" runat="server">
       <div id="content" class="main-content">
             <h1 class="text-left text-primary">
            <label class="PageTitle"><%= GetLocalResourceObject("lblScreenTitle") %></label>
        </h1>
            <br />


            <div class="container">
                     <iframe id="Iframe1" class="iframeCollapsed" style="overflow:hidden;overflow-x:hidden;
        overflow-y:hidden;height:100%;width:100%;position:absolute;
        left:0px;right:0px;" height="100%" width="100%"
                         target="_blank"
                         frameborder="0" runat="server" />
            </div>
                  


            </div>

</asp:Content>
