<%@ Page Language="C#" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Dole Mensajes</title>

    <script language="javascript" type="text/javascript">
// <!CDATA[


function CargarVentana(){ 
    var objArgumentos = window.dialogArguments;
    var strArgumentos = objArgumentos.ErrorDole;
    var tipoExepcion = strArgumentos.substring(strArgumentos.length-1,strArgumentos.length)
    var tipoMensaje =  strArgumentos.substring(strArgumentos.length-2,strArgumentos.length-1);
    var textoExepcion = strArgumentos.substring(0,strArgumentos.length-2);
    
    //txtDescripcion.value = textoExepcion;
    document.getElementById('txtDescripcion').value = textoExepcion;
     
    switch(tipoMensaje)
    {
    case "1":
      this.form1.imgError.src="../Content/imagenes/ImgInformacion.jpg";
      document.getElementById('lblTituloMensaje').innerHTML = 'Information';
      break    
    case "2":
        this.form1.imgError.src = "../Content/imagenes/ImgAdvertencia.jpg";
      document.getElementById('lblTituloMensaje').innerHTML = 'Warning';
      break
    case "3":
        switch(tipoExepcion)
        {
        case "1":
            this.form1.imgError.src = "../Content/imagenes/ImgStop.jpg";
          document.getElementById('lblTituloMensaje').innerHTML = 'Error';
          
          break
          
        case "2":
            this.form1.imgError.src = "../Content/imagenes/ImgStop.jpg";
          document.getElementById('lblTituloMensaje').innerHTML = 'Error';
          
          break   
           
        case "3":
            this.form1.imgError.src = "../Content/imagenes/ImgValidacion.jpg";
          document.getElementById('lblTituloMensaje').innerHTML = 'Validation';

          break    
          
       case "4":
           this.form1.imgError.src = "../Content/imagenes/ImgValidacion.jpg";
          document.getElementById('lblTituloMensaje').innerHTML = 'Validation';

          break
  
        } 
        break      
    
    }
    
}
</script>

</head>
<body onload="CargarVentana()">

    <form id="form1" runat="server">
        <div>
            <table style="width: 250px; height: 140px">
                <tr>
                    <td align="center" valign="middle" style="width: 338px; height: 94px">
                        <img id="imgError" alt="" src="" height="72" width="72" />&nbsp;</td>
                </tr>
                <tr>
                    <td align="center" valign="middle" style="width: 338px; height: 27px">
                        <asp:Label ID="lblTituloMensaje" runat="server" Text="Label" Font-Bold="True" Font-Names="Arial"
                            ForeColor="Black" Font-Size="16pt">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="height: 141px; width: 338px;" align="center" valign="middle">
                        <asp:TextBox runat="server" id="txtDescripcion" style="font-size: 11pt; vertical-align: middle; text-align: center;" 
                        BorderStyle="None" Height="120px" ReadOnly="True" TextMode="MultiLine" Width="320px" Font-Names="Arial" Font-Size="11pt" ForeColor="DimGray"></asp:TextBox>
                        </td>
                        
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
