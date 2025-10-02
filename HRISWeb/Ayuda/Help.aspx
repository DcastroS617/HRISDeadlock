<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Help.aspx.cs" Inherits="FixedAssetsWeb.Ayuda.Help" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ayuda</title>
    <script type="text/javascript">

        function document(idParent, idChildren)
        {
            var enlace = document.getElementById(idParent);
            enlace.click();
            document.getElementById(idChildren).click();
        }

    </script>
</head>
<body>
    <form runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:updatepanel runat="server" ID="uppFrames" >
            <ContentTemplate>
                <FRAMESET border=4 cols="400, *">
                <FRAME name=left marginWidth=5 marginHeight=5 src="ayuda\___left.htm" >
                <FRAME name=body marginWidth=5 marginHeight=5 src="" >
                </FRAMESET>
            </ContentTemplate>
        </asp:updatepanel>
    </form>
</body>
</html>
