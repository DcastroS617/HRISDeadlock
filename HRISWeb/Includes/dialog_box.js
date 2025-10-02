/* This script and many more are available free online at
The JavaScript Source!! http://javascript.internet.com
http://javascript.internet.com/page-details/custom-javascript-dialog-boxes.html
Created by: Michael Leigeber | http://www.leigeber.com/ */

// global variables //
var TIMER = 5;
var SPEED = 10;
var usuarioRespondioConfim = false;
var respuestaConfirm = false;

// calculate the current window width //
function pageWidth() {
  return window.innerWidth != null ? window.innerWidth : document.documentElement && document.documentElement.clientWidth ? document.documentElement.clientWidth : document.body != null ? document.body.clientWidth : null;
}

// calculate the current window height //
function pageHeight() {
  return window.innerHeight != null? window.innerHeight : document.documentElement && document.documentElement.clientHeight ? document.documentElement.clientHeight : document.body != null? document.body.clientHeight : null;
}

// calculate the current window vertical offset //
function topPosition() {
  return typeof window.pageYOffset != 'undefined' ? window.pageYOffset : document.documentElement && document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop ? document.body.scrollTop : 0;
}

// calculate the position starting at the left of the window //
function leftPosition() {
  return typeof window.pageXOffset != 'undefined' ? window.pageXOffset : document.documentElement && document.documentElement.scrollLeft ? document.documentElement.scrollLeft : document.body.scrollLeft ? document.body.scrollLeft : 0;
}

// build/show the dialog box, populate the data and call the fadeDialog function //
function showDialog(title,message,type,rutaBase,idWarp) {

try{
  
  if(!type) {
    type = 'error';
  }
  var dialog;
  var dialogheader;
  var dialogclose;
  var dialogtitle;
  var dialogcontent;
  var dialogmask;
  var cajaTextoMensaje;
  
  if(document.getElementById('dialog') != null)
  {    
    var dialog = document.getElementById('dialog');
    clearInterval(dialog.timer);
    
    document.getElementById('dialog-content').innerHTML = '';    
  }
  
  if(document.getElementById('dialog') == null) {
    dialog = document.createElement('div');
    dialog.id = 'dialog';
    dialogheader = document.createElement('div');
    dialogheader.id = 'dialog-header';
    dialogtitle = document.createElement('div');
    dialogtitle.id = 'dialog-title';
    dialogclose = document.createElement('div');
    dialogclose.id = 'dialog-close'
    dialogcontent = document.createElement('div');
    dialogcontent.id = 'dialog-content';
    //dialogmask = document.createElement('iframe');
    dialogmask = document.createElement('div');
    dialogmask.id = 'dialog-mask';
    document.getElementById(idWarp).appendChild(dialogmask);
    document.getElementById(idWarp).appendChild(dialog);    
    dialog.appendChild(dialogheader);
    dialogheader.appendChild(dialogtitle);
    dialogheader.appendChild(dialogclose);
    dialog.appendChild(dialogcontent);;
    dialogclose.setAttribute('onclick','hideDialog()');
    dialogclose.onclick = hideDialog;        
  } 
  else {
    dialog = document.getElementById('dialog');    
    dialogheader = document.getElementById('dialog-header');    
    dialogtitle = document.getElementById('dialog-title');    
    dialogclose = document.getElementById('dialog-close');    
    dialogcontent = document.getElementById('dialog-content');    
    dialogmask = document.getElementById('dialog-mask');    
    dialogmask.style.visibility = "visible";
    dialog.style.visibility = "visible";        
  }
  
  if(document.getElementById('txtMensaje') == null && type != 'confirmar')
  {
    //Se crea la caja de texto
    cajaTextoMensaje = document.createElement('textarea');
    cajaTextoMensaje.setAttribute("name","mytextarea");
    cajaTextoMensaje.setAttribute("cols","48");
    cajaTextoMensaje.setAttribute("rows","9");
    cajaTextoMensaje.id = 'txtMensaje';
  }
  else if(document.getElementById('txtMensaje') != null && type != 'confirmar')
  {
    cajaTextoMensaje = document.getElementById('txtMensaje');
    cajaTextoMensaje.innerHTML = '';
  }
  
  //Estilo de la ventana de mensaje
  dialog.style.opacity = .00;
  dialog.style.filter = 'alpha(opacity=50)';
  dialog.alpha = 0;  
  dialog.style.position='absolute';
  dialog.style.width='425px'; 
  dialog.style.padding='10px'; 
  dialog.style.zIndex='200'; 
  dialog.style.background='#fff';  
  //Estilo del contenedor del header de la ventana de mensaje(El título de la ventana)
  dialogheader.style.display = 'block';
  //dialogheader.style.position = 'relative';
  dialogheader.style.width = '411px'; 
  dialogheader.style.padding = '3px 6px 7px'; 
  dialogheader.style.height = '19px'; 
  dialogheader.style.fontSize = '12px'; 
  dialogheader.style.fontWeight = 'bold';
  dialogheader.style.color = '#000000';
  //Estilo del contenedor del mensaje de la ventana de mensaje(Background del titulo de la ventana)
  dialogtitle.style.styleFloat = 'left';
  //dialogtitle.style.height = '11px'; 
  //Estilo del contenedor para cerrar la ventana
  dialogclose.style.styleFloat = 'right';
  dialogclose.style.cursor = 'pointer'; 
  dialogclose.style.margin = '3px 3px 0 0'; 
  dialogclose.style.height = '19px'; 
  dialogclose.style.width = '11px';
  dialogclose.style.background = 'url('+rutaBase+'App_Themes/Imagenes/Dialogos/dialog_close.gif) no-repeat';
  //Estilo del contenedor que contiene el control del mensaje
  dialogcontent.style.display = 'block'; 
  dialogcontent.style.height = '160px'; 
  dialogcontent.style.padding = '6px'; 
  dialogcontent.style.color = '#666666'; 
  dialogcontent.style.fontSize = '12px';
  //Estilo del fondo de pantalla
  dialogmask.style.position = 'absolute'; 
  dialogmask.style.top = '0'; 
  dialogmask.style.left = '0'; 
  dialogmask.style.minHeight = '100%'; 
  dialogmask.style.width = '100%'; 
  dialogmask.style.background = '#D3D3D3';  
  //dialogmask.style.opacity = '0.75'; 
  dialogmask.style.filter = 'alpha(opacity=45)'; 
  //dialogmask.style.zIndex = '100';
  
  var width = pageWidth();
  var height = pageHeight();
  var left = leftPosition();
  var top = topPosition();
  var dialogwidth = dialog.offsetWidth;
  var dialogheight = dialog.offsetHeight;
  var topposition = top + (height / 3) - (dialogheight / 2);
  var leftposition = left + (width / 2) - (dialogwidth / 2);
  dialog.style.top = topposition + "px";
  dialog.style.left = leftposition + "px"; 
  dialogtitle.style.fontSize = '12px'; 
  //dialogtitle.innerHTML = '<H2>'+title+'</H2>';
  dialogtitle.style.fontFamily = 'arial';
  
  
  if(type == 'confirmar')
  {
	 usuarioRespondioConfim = false;
    dialogcontent.innerHTML = '<font face="Arial" color="#666666">'+ message + '</font>'+
    "<br/><br/><br/><br/><input id='btnAceptarConfirmacion' type='button' BackColor='#696969' style='font-family:Arial; font-size:12px' BorderColor='#dcdcdc' value='Aceptar' onclick='ventanaConfirmacion_OnClientClick(true);'/>"+
    "&nbsp&nbsp&nbsp&nbsp&nbsp<input id='btnCancelarConfirmacion' type='button' value='Cancelar' style='font-family:Arial; font-size:12px' onclick='ventanaConfirmacion_OnClientClick(false);' />";
  } 
  else
  {
      //Estilo de la Caja de texto que contiene el mensaje   
      cajaTextoMensaje.style.styleFloat = 'left';
      cajaTextoMensaje.style.opacity = '0.9';
      cajaTextoMensaje.style.background = '#ffffff';
      cajaTextoMensaje.readOnly = true;
      cajaTextoMensaje.style.borderColor = 'transparent';  
      cajaTextoMensaje.style.backgroundColor = 'transparent';
      cajaTextoMensaje.style.fontFamily = 'arial';
      cajaTextoMensaje.style.color = '#666666';
      cajaTextoMensaje.style.mozOpacity= 0.9;
      cajaTextoMensaje.style.overflow = 'auto';
      cajaTextoMensaje.innerHTML = message;  
      dialogcontent.appendChild(cajaTextoMensaje);      
  }
  
  switch(type)
  {
    case 'error':
        dialogcontent.style.background = '#fff';
        dialogcontent.style.background = 'url('+rutaBase+'App_Themes/Imagenes/Dialogos/error_bg.jpg) bottom right no-repeat';
        dialogcontent.style.border = '1px solid #924949';
        dialogheader.style.background = 'url('+rutaBase+'App_Themes/Imagenes/Dialogos/error_header.gif) repeat-x';
        //dialogheader.style.backgroundColor = '#6f2c2c';
        //dialogheader.style.filter = 'alpha(opacity=45)';
        //dialogheader.style.color = '#6f2c2c';
        dialogheader.style.border = '1px solid #924949'; 
        dialogheader.style.borderBottom = 'none';
        //dialogmask.style.background = '#6f2c2c';        
    break;    
    case 'advertencia':
        dialogcontent.style.background = '#fff';
        dialogcontent.style.background = 'url('+rutaBase+'App_Themes/Imagenes/Dialogos/warning_bg.jpg) bottom right no-repeat'; 
        dialogcontent.style.border = '1px solid #c5a524'; 
        //dialogcontent.style.borderTop = 'none';
        dialogheader.style.background = 'url('+rutaBase+'App_Themes/Imagenes/Dialogos/warning_header.gif) repeat-x';
        //dialogheader.style.color = '#957c17'; 
        dialogheader.style.border = '1px solid #c5a524'; 
        dialogheader.style.borderBottom = 'none';
        //dialogmask.style.background = '#957c17';
    break;    
    case 'informacion':
        dialogcontent.style.background = '#fff';
        dialogcontent.style.background = 'url('+rutaBase+'App_Themes/Imagenes/Dialogos/success_bg.jpg) bottom right no-repeat'; 
        dialogcontent.style.border = '1px solid #60a174'; 
        //dialogcontent.style.borderTop = 'none';
        dialogheader.style.background = 'url('+rutaBase+'App_Themes/Imagenes/Dialogos/success_header.gif) repeat-x'; 
        //dialogheader.style.color = '#3c7f51'; 
        dialogheader.style.border = '1px solid #60a174'; 
        dialogheader.style.borderBottom = 'none';
        //dialogmask.style.background = '#3c7f51';
    break;
    case 'Validacion':
     dialogcontent.style.background = '#fff';
     dialogcontent.style.background = 'url('+rutaBase+'App_Themes/Imagenes/Validacion_bg.jpg) bottom right no-repeat'; 
     dialogcontent.style.border = '1px solid #4f6d81'; 
     //dialogcontent.style.borderTop = 'none';
     dialogheader.style.background = 'url('+rutaBase+'App_Themes/Imagenes/validacion_header.gif) repeat-x'; 
     //dialogheader.style.color = '#355468';
     dialogtitle.innerHTML = '<H2>Validación de Datos</H2>';
     dialogheader.style.border = '1px solid #4f6d81'; 
     dialogheader.style.borderBottom = 'none';
     //dialogmask.style.background = '#355468';
    break;
    case 'Confirmacion':
     dialogcontent.style.background = '#fff';
     dialogcontent.style.background = 'url('+rutaBase+'App_Themes/Imagenes/prompt_bg.jpg) bottom right no-repeat'; 
     dialogcontent.style.border = '1px solid #4f6d81'; 
     //dialogcontent.style.borderTop = 'none';
     dialogheader.style.background = 'url('+rutaBase+'App_Themes/Imagenes/prompt_header.gif) repeat-x'; 
     //dialogheader.style.color = '#355468';
     dialogtitle.innerHTML = '<H2>Confirmación</H2>';
     dialogheader.style.border = '1px solid #4f6d81'; 
     dialogheader.style.borderBottom = 'none';
     //dialogmask.style.background = '#355468';
    break;

    default:
     dialogcontent.style.background = '#fff';
     dialogcontent.style.background = 'url('+rutaBase+'App_Themes/Imagenes/Dialogos/prompt_bg.jpg) bottom right no-repeat'; 
     dialogcontent.style.border = '1px solid #4f6d81'; 
     //dialogcontent.style.borderTop = 'none';
     dialogheader.style.background = 'url('+rutaBase+'App_Themes/Imagenes/Dialogos/prompt_header.gif) repeat-x'; 
     //dialogheader.style.color = '#355468'; 
     dialogheader.style.border = '1px solid #4f6d81'; 
     dialogheader.style.borderBottom = 'none';
     //dialogmask.style.background = '#355468';
    break;
  }
  
  dialog.timer = setInterval("fadeDialog(1)", TIMER);
  dialogclose.style.visibility = "visible";
  }
  catch(e)
  {
	document.getElementById(idWarp).innerHTML = '';
    alert(message);
  }
}

// build/show the dialog box, populate the data and call the fadeDialog function //
function MostrarMensajeCongelarPantalla(message, type, rutaBase, idWarp) {

    try {

        if (!type) {
            type = 'Exception';
        }
        var dialog;
        var dialogheader;
        var dialogclose;
        var dialogtitle;
        var dialogcontent;
        var dialogmask;
        var cajaTextoMensaje;

        if (document.getElementById('dialog') != null) {
            var dialog = document.getElementById('dialog');
            clearInterval(dialog.timer);

            document.getElementById('dialog-content').innerHTML = '';
        }

        if (document.getElementById('dialog') == null) {
            dialog = document.createElement('div');
            dialog.id = 'dialog';
            dialogheader = document.createElement('div');
            dialogheader.id = 'dialog-header';
            dialogtitle = document.createElement('div');
            dialogtitle.id = 'dialog-title';
            dialogclose = document.createElement('div');
            dialogclose.id = 'dialog-close'
            dialogcontent = document.createElement('div');
            dialogcontent.id = 'dialog-content';
            //dialogmask = document.createElement('iframe');
            dialogmask = document.createElement('div');
            dialogmask.id = 'dialog-mask';
            document.getElementById(idWarp).appendChild(dialogmask);
            document.getElementById(idWarp).appendChild(dialog);
            dialog.appendChild(dialogheader);
            dialogheader.appendChild(dialogtitle);
            dialogheader.appendChild(dialogclose);
            dialog.appendChild(dialogcontent); ;
            dialogclose.setAttribute('onclick', 'hideDialog()');
            dialogclose.onclick = hideDialog;
        }
        else {
            dialog = document.getElementById('dialog');
            dialogheader = document.getElementById('dialog-header');
            dialogtitle = document.getElementById('dialog-title');
            dialogclose = document.getElementById('dialog-close');
            dialogcontent = document.getElementById('dialog-content');
            dialogmask = document.getElementById('dialog-mask');
            dialogmask.style.visibility = "visible";
            dialog.style.visibility = "visible";
        }

        if (document.getElementById('txtMensaje') == null && type != 'Confirmacion') {
            //Se crea la caja de texto
            cajaTextoMensaje = document.createElement('textarea');
            cajaTextoMensaje.setAttribute("name", "mytextarea");
            cajaTextoMensaje.setAttribute("cols", "48");
            cajaTextoMensaje.setAttribute("rows", "9");
            cajaTextoMensaje.id = 'txtMensaje';
        }
        else if (document.getElementById('txtMensaje') != null && type != 'Confirmacion') {
            cajaTextoMensaje = document.getElementById('txtMensaje');
            cajaTextoMensaje.innerHTML = '';
        }

        //Estilo de la ventana de mensaje
        dialog.style.opacity = .00;
        dialog.style.filter = 'alpha(opacity=0)';
        dialog.alpha = 0;
        dialog.style.position = 'absolute';
        dialog.style.width = '425px';
        dialog.style.padding = '10px';
        dialog.style.zIndex = '200';
        dialog.style.background = '#fff';
        //Estilo del contenedor del header de la ventana de mensaje(El título de la ventana)
        dialogheader.style.display = 'block';
        //dialogheader.style.position = 'relative';
        dialogheader.style.width = '411px';
        dialogheader.style.padding = '3px 6px 7px';
        dialogheader.style.height = '19px';
        dialogheader.style.fontSize = '12px';
        dialogheader.style.fontWeight = 'bold';
        dialogheader.style.color = '#000000';
        //Estilo del contenedor del mensaje de la ventana de mensaje(Background del titulo de la ventana)
        dialogtitle.style.styleFloat = 'left';
        //dialogtitle.style.height = '11px'; 
        //Estilo del contenedor para cerrar la ventana
        dialogclose.style.styleFloat = 'right';
        dialogclose.style.cursor = 'pointer';
        dialogclose.style.margin = '3px 3px 0 0';
        dialogclose.style.height = '19px';
        dialogclose.style.width = '11px';
        dialogclose.style.background = 'url(' + rutaBase + 'App_Themes/Imagenes/dialog_close.gif) no-repeat';
        //Estilo del contenedor que contiene el control del mensaje
        dialogcontent.style.display = 'block';
        dialogcontent.style.height = '160px';
        dialogcontent.style.padding = '6px';
        dialogcontent.style.color = '#666666';
        dialogcontent.style.fontSize = '12px';
        //Estilo del fondo de pantalla
        dialogmask.style.position = 'absolute';
        dialogmask.style.top = '0';
        dialogmask.style.left = '0';
        dialogmask.style.minHeight = '100%';
        dialogmask.style.width = '100%';
        dialogmask.style.background = '#bebebe';
        //dialogmask.style.opacity = '0.75'; 
        dialogmask.style.filter = 'alpha(opacity=45)';
        //dialogmask.style.zIndex = '100';

        var width = pageWidth();
        var height = pageHeight();
        var left = leftPosition();
        var top = topPosition();
        var dialogwidth = dialog.offsetWidth;
        var dialogheight = dialog.offsetHeight;
        var topposition = top + (height / 3) - (dialogheight / 2);
        var leftposition = left + (width / 2) - (dialogwidth / 2);
        dialog.style.top = topposition + "px";
        dialog.style.left = leftposition + "px";
        dialogtitle.style.fontSize = '12px';
        //dialogtitle.innerHTML = '<H2>'+title+'</H2>';
        dialogtitle.style.fontFamily = 'arial';


        if (type == 'Confirmacion') {
            usuarioRespondioConfim = false;
            dialogcontent.innerHTML = '<font face="Arial" color="#666666">' + message + '</font>' +
    "<br/><br/><br/><br/><input id='btnAceptarConfirmacion' type='button' BackColor='#696969' style='font-family:Arial; font-size:12px' BorderColor='#dcdcdc' value='Aceptar' onclick='ventanaConfirmacion_OnClientClick(true);'/>" +
    "&nbsp&nbsp&nbsp&nbsp&nbsp<input id='btnCancelarConfirmacion' type='button' value='Cancelar' style='font-family:Arial; font-size:12px' onclick='ventanaConfirmacion_OnClientClick(false);' />";
        }
        else {
            //Estilo de la Caja de texto que contiene el mensaje   
            cajaTextoMensaje.style.styleFloat = 'left';
            cajaTextoMensaje.style.opacity = '0.75';
            cajaTextoMensaje.style.background = '#ffffff';
            cajaTextoMensaje.readOnly = true;
            cajaTextoMensaje.style.borderColor = 'transparent';
            cajaTextoMensaje.style.backgroundColor = 'transparent';
            cajaTextoMensaje.style.fontFamily = 'arial';
            cajaTextoMensaje.style.color = '#666666';
            cajaTextoMensaje.style.mozOpacity = 0.9;
            cajaTextoMensaje.style.overflow = 'auto';
            cajaTextoMensaje.innerHTML = message;
            dialogcontent.appendChild(cajaTextoMensaje);
        }

        switch (type) {
            case 'Exception':
                dialogcontent.style.background = '#fff';
                dialogcontent.style.background = 'url(' + rutaBase + 'App_Themes/Imagenes/error_bg.jpg) bottom right no-repeat';
                dialogcontent.style.border = '1px solid #924949';
                dialogheader.style.background = 'url(' + rutaBase + 'App_Themes/Imagenes/error_header.gif) repeat-x';
                //dialogheader.style.backgroundColor = '#6f2c2c';
                //dialogheader.style.filter = 'alpha(opacity=45)';
                //dialogheader.style.color = '#6f2c2c';
                dialogtitle.innerHTML = '<H2>Error</H2>';
                dialogheader.style.border = '1px solid #924949';
                dialogheader.style.borderBottom = 'none';
                //dialogmask.style.background = '#6f2c2c';        
                break;
            case 'Advertencia':
                dialogcontent.style.background = '#fff';
                dialogcontent.style.background = 'url(' + rutaBase + 'App_Themes/Imagenes/warning_bg.jpg) bottom right no-repeat';
                dialogcontent.style.border = '1px solid #c5a524';
                //dialogcontent.style.borderTop = 'none';
                dialogheader.style.background = 'url(' + rutaBase + 'App_Themes/Imagenes/warning_header.gif) repeat-x';
                //dialogheader.style.color = '#957c17'; 
                dialogtitle.innerHTML = '<H2>Advertencia</H2>';
                dialogheader.style.border = '1px solid #c5a524';
                dialogheader.style.borderBottom = 'none';
                //dialogmask.style.background = '#957c17';
                break;
            case 'Informacion':
                dialogcontent.style.background = '#fff';
                dialogcontent.style.background = 'url(' + rutaBase + 'App_Themes/Imagenes/Informacion_bg.jpg) bottom right no-repeat';
                dialogcontent.style.border = '1px solid #60a174';
                //dialogcontent.style.borderTop = 'none';
                dialogheader.style.background = 'url(' + rutaBase + 'App_Themes/Imagenes/informacion_header.gif) repeat-x';
                //dialogheader.style.color = '#3c7f51';
                dialogtitle.innerHTML = '<H2>Información</H2>';
                dialogheader.style.border = '1px solid #60a174';
                dialogheader.style.borderBottom = 'none';
                //dialogmask.style.background = '#3c7f51';
                break;
            case 'Validacion':
                dialogcontent.style.background = '#fff';
                dialogcontent.style.background = 'url(' + rutaBase + 'App_Themes/Imagenes/Validacion_bg.jpg) bottom right no-repeat';
                dialogcontent.style.border = '1px solid #4f6d81';
                //dialogcontent.style.borderTop = 'none';
                dialogheader.style.background = 'url(' + rutaBase + 'App_Themes/Imagenes/validacion_header.gif) repeat-x';
                //dialogheader.style.color = '#355468';
                dialogtitle.innerHTML = '<H2>Validación de Datos</H2>';
                dialogheader.style.border = '1px solid #4f6d81';
                dialogheader.style.borderBottom = 'none';
                //dialogmask.style.background = '#355468';
                break;
            case 'Confirmacion':
                dialogcontent.style.background = '#fff';
                dialogcontent.style.background = 'url(' + rutaBase + 'App_Themes/Imagenes/prompt_bg.jpg) bottom right no-repeat';
                dialogcontent.style.border = '1px solid #4f6d81';
                //dialogcontent.style.borderTop = 'none';
                dialogheader.style.background = 'url(' + rutaBase + 'App_Themes/Imagenes/prompt_header.gif) repeat-x';
                //dialogheader.style.color = '#355468';
                dialogtitle.innerHTML = '<H2>Confirmación</H2>';
                dialogheader.style.border = '1px solid #4f6d81';
                dialogheader.style.borderBottom = 'none';
                //dialogmask.style.background = '#355468';
                break;

            default:
                dialogcontent.style.background = '#fff';
                dialogcontent.style.background = 'url(' + rutaBase + 'App_Themes/Imagenes/Dialogos/prompt_bg.jpg) bottom right no-repeat';
                dialogcontent.style.border = '1px solid #4f6d81';
                //dialogcontent.style.borderTop = 'none';
                dialogheader.style.background = 'url(' + rutaBase + 'App_Themes/Imagenes/Dialogos/prompt_header.gif) repeat-x';
                //dialogheader.style.color = '#355468'; 
                dialogheader.style.border = '1px solid #4f6d81';
                dialogheader.style.borderBottom = 'none';
                //dialogmask.style.background = '#355468';
                break;
        }

        dialog.timer = setInterval("fadeDialog(1)", TIMER);
        dialogclose.style.visibility = "visible";

    }
    catch (e) {
        document.getElementById(idWarp).innerHTML = '';
        alert(message);
    }
}

// hide the dialog box //
function hideDialog() {
  var dialog = document.getElementById('dialog');
  //dialog.style.visibility = "hidden";
  //document.getElementById('dialog-mask').style.visibility = "hidden";
  clearInterval(dialog.timer);
  dialog.timer = setInterval("fadeDialog(0)", TIMER);
}

// fade-in the dialog box //
function fadeDialog(flag) {
  if(flag == null) {
    flag = 1;
  }
  var dialog = document.getElementById('dialog');
  var value = 0;
  if(flag == 1) {
    value = dialog.alpha + SPEED;
  } else {
    value = dialog.alpha - SPEED;
  }
  dialog.alpha = value;
  dialog.style.opacity = (value / 100);
  dialog.style.filter = 'alpha(opacity=' + value + ')';
  if(value >= 99) {
    clearInterval(dialog.timer);   
    dialog.timer = null; 
  } else if(value <= 1) {
    dialog.style.visibility = "hidden";
    document.getElementById('dialog-mask').style.visibility = "hidden";
    clearInterval(dialog.timer);
  }
}

//Eventos de los botones del contenedor de confirmación
function ventanaConfirmacion_OnClientClick(bEstado)
{
    hideDialog();
	usuarioRespondioConfim = true;
    return bEstado;
}
//Revisa que el usuario haya respondido el Confirm
function respondioConfirmUsuario()
{
    return usuarioRespondioConfim;
}
//Obtiene la respuesta de Usuario del Confirm 
function respuestaConfirmUsuario()
{
    return respuestaConfirm;
}