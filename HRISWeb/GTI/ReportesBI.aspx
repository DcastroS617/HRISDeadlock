<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ReportesBI.aspx.cs" Inherits="HRISWeb.GTI.ReportesBI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntHeader" runat="server">
    <title><%=Convert.ToString(GetLocalResourceObject("lblScreenTitle")) %></title>
    <link href="../../Content/css/custom-tabs.css" rel="stylesheet" />
    <link href="../../Content/css/dynamicTable.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cntBody" runat="server">
    <style>
    .responsive-iframe-container {
        position: relative;
        width: 100%;
        padding-bottom: 56.25%; /* Proporción 16:9 */
        height: 0;
        overflow: hidden;
    }

    .responsive-iframe-container iframe {
        position: absolute;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        border: 0;
    }
</style>
    <div class="main-content">
        <h1 class="text-left text-primary">
            <label class="PageTitle"><%= GetLocalResourceObject("lblScreenTitle") %></label>
        </h1>
        <br />
        
        
    </div>
    <div class=" responsive-iframe-container">
        <asp:UpdatePanel runat="server" ID="main">
        <Triggers>
        </Triggers>
        <ContentTemplate>
            <iframe id="Iframe1" src="https://apps.powerapps.com/play/e/dc2c8a8f-e9e1-ecd7-8a20-7aa005517d0c/a/bca94258-db42-4e03-b00e-6ef740414dd5?tenantId=35543620-96b1-488e-968a-f21222189a8a&hint=7908d623-a390-4ac8-9805-6bae1ea1b8fa&sourcetime=1747861949699" frameborder="0" runat="server"></iframe>
        </ContentTemplate>
    </asp:UpdatePanel>
    </div>

    <script src="../Scripts/custom-scripts.js"></script>

    <script type="text/javascript">

        $(document).ready(function () {

        });
       
    </script>
</asp:Content>
