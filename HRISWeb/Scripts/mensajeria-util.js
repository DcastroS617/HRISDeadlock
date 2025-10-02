/*
define el nombre de la aplicación
Por ejemplo: si la url de la app es http://server:1526/HRIS/Default.aspx
el valor de la variable debe ser applicationName = "/HRIS".
Esto se hace porque la url del source de las imágenes se establecen por javascript
y con este cambio funcionaría para cualquier publicación y para el ambiente de desarrollo.
*/

var applicationName = "";

// define un conjunto de elementos seleccionables para manejar un teniscismo del index al mostrar mensajería
var SELECTOR_FOCUS = 'a, area, input, select, textarea, button, iframe, object, embed, *[tabindex], *[contenteditable]';

// La siguiente pseudo enumeración define los tipos de mensaje que se pueden usar.
var TipoMensaje = {
    INFORMACION: 1
    , ADVERTENCIA: 2
    , VALIDACION: 3
    , ERROR: 4
};

// Las etiquetas de los respectivos tipos de mensajes y elementos de UI (normalmente configurables desde un archivo de recursos).
var etiquetaMensajeriaInformacion
    , etiquetaMensajeriaAdvertencia
    , etiquetaMensajeriaValidacion
    , etiquetaMensajeriaError
    , etiquetaMensajeriaConfirmacion
    , etiquetaMensajeriaAceptar
    , mensajeCaracteresEspeciales;

function InicializarMensajeria(cadenaInformacion, cadenaAdvertencia, cadenaValidacion, cadenaError, cadenaConfirmacion, cadenaAceptar, cadenaCaracteresEspeciales) {
    /// <summary>Inicializa algunas configuraciones de la mensajería que son dependientes de recursos de idioma.</summary>

    etiquetaMensajeriaInformacion = cadenaInformacion;
    etiquetaMensajeriaAdvertencia = cadenaAdvertencia;
    etiquetaMensajeriaValidacion = cadenaValidacion;
    etiquetaMensajeriaError = cadenaError;
    etiquetaMensajeriaConfirmacion = cadenaConfirmacion
    etiquetaMensajeriaAceptar = cadenaAceptar;
    mensajeCaracteresEspeciales = cadenaCaracteresEspeciales;
}

function MostrarMensaje(tipo, mensaje, callback) {
    /// <summary>Muestra un mensaje emergente del tipo de la pseudo enumeración TipoMensaje, con la posibilidad de ejecutar un callback al cerrar el mensaje.</summary>
    if (typeOf(tipo) !== 'number'
        || typeOf(mensaje) !== 'string'
        || (callback != null && typeOf(callback) !== 'function')) {
        throw new Error(0, 'La función MostrarMensaje no se invocó correctamente de acuerdo a la firma MostrarMensaje(tipo, mensaje, callback|null);');
    }

    if (tipo !== TipoMensaje.INFORMACION
        && tipo !== TipoMensaje.ADVERTENCIA
        && tipo !== TipoMensaje.VALIDACION
        && tipo !== TipoMensaje.ERROR) {
        throw new Error(0, 'El parámetro tipo debe ser un tipo de mensaje válido. Utilice TipoMensaje.<INFORMACION|ADVERTENCIA|VALIDACION|ERROR> para una mejor invocación.');
    }

    var elementosConfigurados;
    var fuenteImagen = '';
    var titulo = '';

    switch (tipo) {
        case TipoMensaje.INFORMACION:
            fuenteImagen = applicationName + "/Content/images/ImgInformacion.jpg";
            titulo = etiquetaMensajeriaInformacion;
            break;
        case TipoMensaje.ADVERTENCIA:
            fuenteImagen = applicationName + "/Content/images/ImgAdvertencia.jpg";
            titulo = etiquetaMensajeriaAdvertencia;
            break;
        case TipoMensaje.VALIDACION:
            fuenteImagen = applicationName + "/Content/images/ImgValidacion.jpg";
            titulo = etiquetaMensajeriaValidacion;
            break;
        case TipoMensaje.ERROR:
            fuenteImagen = applicationName + "/Content/images/ImgStop.jpg";
            titulo = etiquetaMensajeriaError;
            break;
    }

    // establecer las partes de UI
    $('.contenedor-mensajeria .panel-heading h3').html(titulo);
    $('#imgMensajeria').attr('src', fuenteImagen);
    $('.contenedor-mensajeria .panel-body p').html(mensaje);
    $('#boton-mensajeria-uno').html(etiquetaMensajeriaAceptar);

    // enlazar los elementos de eventos
    $('#boton-mensajeria-uno').unbind('click');

    $('#boton-mensajeria-uno').on('click', function (event) {
        event.preventDefault();

        // ocultar los elementos
        $('#backdrop-mensajeria').hide();
        $('.contenedor-mensajeria').hide();

        // alterar los tabindex (re habilitar) de todos los elementos
        var elementos = $('#divPrincipal, #brHeader, #menuHeader').find(SELECTOR_FOCUS);
        if (elementosConfigurados.length > 0) {
            elementos = elementosConfigurados;
        }

        elementos.each(function (index, value) {
            value.removeAttribute('tabindex');
        });

        // verificar si se debe ejecutar el callback
        if (typeOf(callback) == 'function') {
            callback();
        }
    });

    // alterar los tabindex (deshabilitar) de todos los elementos (excepto de la pantalla de mensajería) para que no se pueda hacer
    // tab con el teclado hacia ellos
    var elementos = $('#divPrincipal, #brHeader, #menuHeader').find(SELECTOR_FOCUS).not('[tabindex=-1], :hidden');
    elementosConfigurados = elementos;
    elementos.each(function (index, value) {
        value.setAttribute('tabindex', '-1');
    });

    // y mostrar los elementos
    $('#boton-mensajeria-uno').show();
    $('#boton-mensajeria-dos').hide();

    $('#backdrop-mensajeria').show();
    $('.contenedor-mensajeria').show();

    $('#boton-mensajeria-uno').focus();
}

function MostrarConfirmacion(mensaje, etiquetaBoton1, callback1, etiquetaBoton2, callback2) {
    /// <summary>Muestra un mensaje emergente de confirmación, con la posibilidad de ejecutar callbacks al cerrar el mensaje.</summary>
    if (typeOf(mensaje) != 'string'
        || typeOf(etiquetaBoton1) != 'string'
        || typeOf(etiquetaBoton2) != 'string'
        || (callback1 != null && typeOf(callback1) != 'function')
        || (callback2 != null && typeOf(callback2) != 'function')) {

        throw new Error(0, 'La función MostrarConfirmacion no se invocó correctamente de acuerdo a la firma MostrarConfirmacion(mensaje, etiquetaBoton1, callback1|null, etiquetaBoton2, callback2|null);');
    }

    var elementosConfigurados;
    var fuenteImagen = applicationName + '/Content/images/ImgQuestion.jpg';
    var titulo = etiquetaMensajeriaConfirmacion;

    // establecer las partes de UI
    $('.contenedor-mensajeria .panel-heading h3').html(titulo);
    $('#imgMensajeria').attr('src', fuenteImagen);
    $('.contenedor-mensajeria .panel-body p').html(mensaje);
    $('#boton-mensajeria-uno').html(etiquetaBoton1);
    $('#boton-mensajeria-dos').html(etiquetaBoton2);

    // enlazar los elementos de eventos
    $('#boton-mensajeria-uno').unbind('click');
    $('#boton-mensajeria-dos').unbind('click');

    $('#boton-mensajeria-uno').on('click', function (event) {
        event.preventDefault();

        // ocultar los elementos
        $('#backdrop-mensajeria').hide();
        $('.contenedor-mensajeria').hide();

        // alterar los tabindex (re habilitar) de todos los elementos
        var elementos = $('#divPrincipal, #brHeader, #menuHeader').find(SELECTOR_FOCUS);
        if (elementosConfigurados.length > 0) {
            elementos = elementosConfigurados;
        }

        elementos.each(function (index, value) {
            value.removeAttribute('tabindex');
        });

        // verificar si se debe ejecutar el callback
        if (typeOf(callback1) == 'function') {
            callback1();
        }
    });

    $('#boton-mensajeria-dos').on('click', function (event) {
        event.preventDefault();

        // ocultar los elementos
        $('#backdrop-mensajeria').hide();
        $('.contenedor-mensajeria').hide();

        // alterar los tabindex (re habilitar) de todos los elementos
        var elementos = $('#divPrincipal, #brHeader, #menuHeader').find(SELECTOR_FOCUS);
        if (elementosConfigurados.length > 0) {
            elementos = elementosConfigurados;
        }

        elementos.each(function (index, value) {
            value.removeAttribute('tabindex');
        });

        // verificar si se debe ejecutar el callback
        if (typeOf(callback2) == 'function') {
            callback2();
        }
    });

    // alterar los tabindex (deshabilitar) de todos los elementos (excepto de la pantalla de mensajería) para que no se pueda hacer
    // tab con el teclado hacia ellos
    var elementos = $('#divPrincipal, #brHeader, #menuHeader').find(SELECTOR_FOCUS).not('[tabindex=-1], :hidden');
    elementosConfigurados = elementos;
    elementos.each(function (index, value) {
        value.setAttribute('tabindex', '-1');
    });

    // y mostrar los elementos
    $('#boton-mensajeria-uno').show();
    $('#boton-mensajeria-dos').show();

    $('#backdrop-mensajeria').show();

    $('.contenedor-mensajeria').show();
    $('.contenedor-mensajeria').focus();
}

function DesplegarBackdrop() {
    /// <summary>Despliega el backdrop (layer de bloqueo) de la mensajería. Normalmente no debe invocarse por aparte pues ya está integrado; se usa para un workaround al invocar desde el server side.</summary>
    $('#backdrop-mensajeria').show();
}

function typeOf(obj) {
    /// <summary>Determina el tipo de un objeto.</summary>
    /// <returns>El tipo del objeto.</returns>
    return ({}).toString.call(obj).match(/\s(\w+)/)[1].toLowerCase();
}
