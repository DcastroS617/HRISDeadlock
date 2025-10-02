<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MainMenu.ascx.cs" Inherits="HRISWeb.UserControls.MainMenu" %>
<script runat="server">
    /// <summary>
    /// Variable de sustitución de ruta base para imágenes de la página.
    /// </summary>
    string rutaBase;

    /// <summary>
    /// Maneja el evento OnLoad. Resuelve la ruta base a utilizar.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnLoad(EventArgs e)
    {
        rutaBase = ResolveUrl("~");
    }
</script>

<asp:UpdatePanel runat="server" ID="udpMenu">
    <ContentTemplate>
        <div class="navbar navbar-security" role="navigation">
            <div class="container-fluid">
                <div class="navbar-header">
                    <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                        <span class="sr-only">Toggle navigation</span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </button>

                    <a class="navbar-brand" href="<%=rutaBase%>Default.aspx">
                        <span style="margin-right: 15px;" class="glyphicon glyphicon-home"></span>
                    </a>
                </div>

                <div class="collapse navbar-collapse">
                    <ul class="nav navbar-nav" runat="server" id="ulMenuOptions">
                        <!-- Inclusión dinámica de los menús -->
                    </ul>

                    <ul class="nav navbar-nav navbar-right">
                        <li>
                            <asp:LinkButton ID="lbtnChangeWorkingDivision" runat="server" meta:resourcekey="lbtnChangeWorkingDivision" ToolTip="" OnClick="LbtnChangeWorkingDivision_Click">
                                <span class="glyphicon glyphicon-globe"></span>
                            </asp:LinkButton>
                        </li>
                        <li>
                            <%-- Se deshabilita por el momento el botón de ayuda porque no cumple con lo programado --%>
                            <asp:LinkButton runat="server" Style="margin-right: 18px;" ID="lnkViewHelp" OnClick="LnkViewHelp_Click">
                                <span runat="server" id="spnHelp" class="glyphicon glyphicon-question-sign"></span>
                            </asp:LinkButton>
                        </li>
                    </ul>
                </div>
                <!--/.nav-collapse -->
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
