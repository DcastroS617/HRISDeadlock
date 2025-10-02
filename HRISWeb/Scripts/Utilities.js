function checkMinValue(txtBox, evt, value) {
    /// <summary>Validate the min value of the textbox's number</summary>
    /// <param name="txtBox" type="objetc">Textbox.</param>
    /// <param name="evt" type="objetc">Object with information of the event.</param>
    /// <param name="value" type="objetc">Min value.</param>    
    var charCode = (evt.which) ? evt.which : event.keyCode;

    if (charCode >= 48 && charCode <= 57) {
        charCode = charCode - 48;

        if (txtBox) {
            var txtBoxValue = txtBox.value + charCode;
            return (parseInt(txtBoxValue) >= value);
        }
    }
}

function checkMaxValue(txtBox, evt, value) {
    /// <summary>Validate the max value of the textbox's number</summary>
    /// <param name="txtBox" type="objetc">Textbox.</param>
    /// <param name="evt" type="objetc">Object with information of the event.</param>
    /// <param name="value" type="objetc">Min value.</param>    
    var charCode = (evt.which) ? evt.which : event.keyCode;

    if (charCode >= 48 && charCode <= 57) {
        charCode = charCode - 48;

        if (txtBox) {
            var txtBoxValue = txtBox.value + charCode;
            return (parseInt(txtBoxValue) <= value);
        }
    }
}

function checkMaxLength(txtBox, e, maxLength) {
    /// <summary>Validate the max length of the textbox</summary>
    /// <param name="txtBox" type="objetc">Textbox.</param>
    /// <param name="e" type="objetc">Object with information of the event.</param>
    /// <param name="maxLength" type="objetc">Max length.</param>
    if (txtBox && txtBox.value) {
        if (txtBox.value.length > maxLength) {
            txtBox.value = txtBox.value.substring(0, maxLength);
        }

        return (txtBox.value.length < maxLength);
    }
}

function checkAlreadyDecimalIfSeparator(txtBox, evt) {
    /// <summary>Validate if the value in the textbox is already a decimal number</summary>
    /// <param name="txtBox" type="objetc">Textbox.</param>
    /// <param name="evt" type="objetc">Object with information of the event.</param>
    var charCode = (evt.which) ? evt.which : event.keyCode;

    if (charCode === 46 || charCode === 44) {
        if (txtBox) {
            var txtBoxValue = txtBox.value;
            return (txtBoxValue.indexOf(".") >= 0 || txtBoxValue.indexOf(",") >= 0);
        }
    }

    return false;
}

function replacePastedInvalidCharacters(obj) {
    /// <summary>Replace the pasted invalid characters.</summary>
    /// <param name="evt" type="objetc">Object with information of the event.</param>
    obj.val(obj.val().replace(/[^A-Za-z0-9\sáéíóúÁÉÍÓÚ@\-_:ñÑ]/g, '') // Quitar caracteres no válidos
        .replace(/[%_]/g, '')); // También quitar % y _
}

function replacePastedInvalidDigits(obj) {
    /// <summary>Replace the pasted invalid digits.</summary>
    /// <param name="evt" type="objetc">Object with information of the event.</param>
    obj.val(obj.val().replace(/[^0-9]/gi, ''));
}

function replacePastedInvalidDecimalDigits(obj) {
    /// <summary>Replace the pasted invalid digits.</summary>
    /// <param name="evt" type="objetc">Object with information of the event.</param>
    var value = obj.val();
    value = value.replace(/[^0-9\\.,]/gi, '');
    value = replaceFirstDecimalSeparatorForPlaceHolder(value, '*');
    value = value.replace(/[\\.,]/gi, '');
    value = value.replace('*', '.');

    obj.val(value);
}

function replaceFirstDecimalSeparatorForPlaceHolder(value, placeHolder) {
    var index1 = value.indexOf(",");
    var index2 = value.indexOf(".");
    var index = -1;

    if (index1 >= 0 && index2 >= 0) {
        index = Math.min(index1, index2);
    }
    else if (index1 >= 0 && index2 < 0) {
        index = index1;
    }
    else if (index1 < 0 && index2 >= 0) {
        index = index2;
    }

    if (index >= 0) {
        value = setCharAt(value, index, placeHolder);
    }

    return value;
}

function setCharAt(str, index, chr) {
    if (index > str.length - 1) return str;
    return str.substr(0, index) + chr + str.substr(index + 1);
}

/// <summary>Validates input characters in a text field.</summary>
/// <param name="evt" type="object">Object with information about the keyboard event.</param>
function isNumberOrLetter(evt) {

    /// <summary>Validate code char.</summary>
    /// <param name="evt" type="objetc">Object with information of the event.</param>
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if ((charCode >= 65 && charCode <= 90)  //A-Z
        || (charCode >= 97 && charCode <= 122) //a-z
        || (charCode >= 48 && charCode <= 57) //0-9                
        || (charCode === 186) //tílde
        //'|| (charCode >= 160 && charCode <= 163) //áíóú
        || (charCode === 130) //é
        // || (charCode >= 37 && charCode <= 40 && !evt.shiftKey) //arrows
        || (charCode === 64) //@
        || (charCode === 8) //backspace
        || (charCode === 9) //tab
        || (charCode === 13) //enter
        || (charCode === 46) //delete .    
        || (charCode === 110) //.        
        || (charCode === 45) //-
        || (charCode === 95) //_
        || (charCode === 32) //space
        || (charCode === 58) //:
        || (charCode === 241) //ñ
        || (charCode === 209) //Ñ
        || (charCode === 193) //Á
        || (charCode === 201) //É
        || (charCode === 205) //Í
        || (charCode === 211) //Ó
        || (charCode === 218) //Ú
        || (charCode === 225) //á
        || (charCode === 233) //é
        || (charCode === 237) //í
        || (charCode === 243) //ó
        || (charCode === 250) //ú
    ) {
        return true;
    }

    return false;
}

function isNumberOrLetterTFL(evt) {
    /// <summary>Validate code char.</summary>
    /// <param name="evt" type="objetc">Object with information of the event.</param>
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if ((charCode >= 65 && charCode <= 90)  //A-Z
        || (charCode >= 97 && charCode <= 122) //a-z
        || (charCode >= 48 && charCode <= 57) //0-9                
        || (charCode === 186) //tílde
        //'|| (charCode >= 160 && charCode <= 163) //áíóú
        || (charCode === 130) //é
        // || (charCode >= 37 && charCode <= 40 && !evt.shiftKey) //arrows
        || (charCode === 64) //@
        || (charCode === 8) //backspace
        || (charCode === 9) //tab
        || (charCode === 13) //enter
        || (charCode === 46) //delete .    
        || (charCode === 110) //.        
        || (charCode === 45) //-
        || (charCode === 95) //_
        || (charCode === 32) //space
        || (charCode === 58) //:
        || (charCode === 241) //ñ
        || (charCode === 209) //Ñ
        || (charCode === 193) //Á
        || (charCode === 201) //É
        || (charCode === 205) //Í
        || (charCode === 211) //Ó
        || (charCode === 218) //Ú
        || (charCode === 225) //á
        || (charCode === 233) //é
        || (charCode === 237) //í
        || (charCode === 243) //ó
        || (charCode === 250) //ú
        || (charCode === 37) //%
    ) {
        return true;
    }

    return false;
}

function isNumberOrLetterNoEnter(evt) {
    /// <summary>Validate code char.</summary>
    /// <param name="evt" type="objetc">Object with information of the event.</param>
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if ((charCode >= 65 && charCode <= 90)  //A-Z
        || (charCode >= 97 && charCode <= 122) //a-z
        || (charCode >= 48 && charCode <= 57) //0-9                
        || (charCode === 186) //tílde
        || (charCode >= 160 && charCode <= 163) //áíóú
        || (charCode === 130) //é
        || (charCode >= 37 && charCode <= 40 && !evt.shiftKey) //arrows
        || (charCode === 64) //@
        || (charCode === 8) //backspace
        || (charCode === 9) //tab        
        || (charCode === 46) //delete .    
        || (charCode === 110) //.        
        || (charCode === 45) //-
        || (charCode === 95) //_
        || (charCode === 32) //space
        || (charCode === 58) //:
        || (charCode === 241) //ñ
        || (charCode === 209) //Ñ
        || (charCode === 193) //Á
        || (charCode === 201) //É
        || (charCode === 205) //Í
        || (charCode === 211) //Ó
        || (charCode === 218) //Ú
        || (charCode === 225) //á
        || (charCode === 233) //é
        || (charCode === 237) //í
        || (charCode === 243) //ó
        || (charCode === 250) //ú
    ) {
        return true;
    }

    return false;
}

function isNumber(evt) {
    /// <summary>Validate if the char code is number</summary>
    /// <returns type="Boolean" />
    var charCode = (evt.which) ? evt.which : event.keyCode;

    if (charCode > 47 && charCode < 58) {
        return true;
    }

    return false;
}

function isDecimalNumber(evt) {
    /// <summary>Validate if the char code is number</summary>
    /// <returns type="Boolean" />
    var charCode = (evt.which) ? evt.which : event.keyCode;

    if ((charCode >= 48 && charCode <= 57) || (charCode === 46) || (charCode === 44)) ///0-9 , .
    {
        return true;
    }

    return false;
}

function IsValidEmail(emailAddress, titleMessage, textMessage) {
    /// <summary>Validate if a email address is valid or not. Displays the message according to the parameters</summary>
    /// <param name="emailAddress" type="string">Email address to validate.</param>
    /// <param name="titleMessage" type="string">Title of message.</param>
    /// <param name="textMessage" type="string">Descripton of message.</param>
    /// <returns type="Boolean" />
    var pattern = new RegExp(/^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/);
    var isValid = pattern.test(emailAddress);

    if (!isValid) {
        showClientMessage(MessageType.Validation, titleMessage, textMessage);
        return false;
    }

    return true;
}

// Sets a textbox to allow only numbers
function SetNumericTexbox(textBox) {
    $(textBox).keydown(function (e) {
        // Allow: backspace, delete, tab, escape, enter
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110]) !== -1

            // Allow: Ctrl+A, Command+A
            || (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true))

            // Allow: home, end, left, right, down, up
            || (e.keyCode >= 35 && e.keyCode <= 40)) {
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48  || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });
}

// Sets a textbox to allow only numbers and .
function SetDecimalNumericTexbox(textBox) {
    $(textBox).keydown(function (e) {
        // Allow: backspace, delete, tab, escape, enter and .
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1

            // Allow: Ctrl+A, Command+A
            || (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true))

            // Allow: home, end, left, right, down, up
            || (e.keyCode >= 35 && e.keyCode <= 40)) {
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57))  && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });
}

// Validate a file to upload(extension and size)
function ValidateFileToUpload(fileUploadControlId) {
    var file = document.getElementById(fileUploadControlId);

    // Validate select a file
    if (file.value === null || file.value === '') {
        showClientMessage(3, 'Validation', 'You must select a file to upload.');
        return false;
    }

    // Validate extension file
    var path = file.value;
    var extensionFile = path.substring(path.lastIndexOf(".") + 1, path.length).toLowerCase();
    if (extensionFile !== 'xlsx') {
        showClientMessage(3, 'Validation', 'Invalid File. Please upload a File with extension .xlsx');
        return false;
    }

    // Validate file size
    var fileSize = file.files[0].size;
    if (fileSize === 0 || fileSize > 26214400) {
        showClientMessage(3, 'Validation', 'Invalid file size. Size should be greater than zero(0) and less than 25 mb.');
        return false;
    }

    return true;
}

// If the control has a file validates, otherwise returns true
function NoFileOrIsValid(fileUploadControlId) {
    var file = document.getElementById(fileUploadControlId);

    // Validate select a file
    if (file.value === null
        || file.value === '') {
        return true;
    }

    else {
        if (ValidateFileToUpload(fileUploadControlId)) {
            return true;
        }
    }

    return false;
}

function OpenHelpDialog(urlPath) {
    /// <summary>Show help dialgo.</summary>
    /// <param name="urlPath" type="String">Url to open help dialog.</param>
    window.open(urlPath, "HelpDialog", "toolbar=no,scrollbars=yes,resizable=yes");
}

function GetValueInRow(row, id) {
    ///<summary>Get the value of the cell according to its id from the selected row</summary>
    ///<parameter type="table row" name="row">Container row of values</parameter>
    ///<parameter type="string" name="id">Identifier of the cell to search</parameter>
    ///<return type="string" />
    var cell = $(row).find('[data-id="' + id + '"]')[0];

    var value = $(cell).data("value");
    return value;
}

function isNumberKey(evt) {
    ///<summary>Validate if the text is number</summary>
    ///<return type="boolean"/>
    var charCode = (evt.which) ? evt.which : event.keyCode

    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }

    return true;
}

function ValidateSpecialCharacters(text, message, idControl) {
    /// <summary>Avoid special characters from text.</summary>
    /// <param type="text" name="text">Text to validate</param>
    /// <param type="text" name="title">Title to notification</param>
    /// <param type="text" name="message">Message to notification</param>
    /// <return type="boolean" />
    var str = new String(text);

    if (str.match(/([\<])([^\>]{1,})*([\>])*/gi) !== null) {
        MostrarMensaje(TipoMensaje.VALIDACION, message, null);

        return false;
    }

    return true;
}

function ValidateOnlyCharactersValid(text, idControl) {
    /// <summary>Avoid special characters from text.</summary>
    /// <param type="text" name="text">Text to validate</param>
    /// <return type="boolean" />
    return ValidateSpecialCharacters(text, mensajeCaracteresEspeciales, idControl);
}

$('body').on('hidden.bs.modal', '.modal', function (e) {

});

function IsNumeric(text) {
    /// <summary>validate that the text is a integer </summary>
    var pat = /^\d+$/;
    text = new String(text);

    return !text.search(pat);
}

function format(text, value1) {
    /// <summary>Format text</summary>
    return text.replace('{0}', value1);
}

function disableButton(buttonElement) {
    ///<summary>Disables the button element.</summary>
    ///<param type="control">Button to disable</param>
    buttonElement.disabled = true;
    buttonElement.prop('disabled', true);
    buttonElement.attr('disabled', 'disabled');

    buttonElement.css('pointer-events', 'none');
    buttonElement.attr('tabindex', '-1');
}

function enableButton(buttonElement) {
    ///<summary>Enables the button element.</summary>
    ///<param type="control">Button to disable</param>
    buttonElement.disabled = false;
    buttonElement.prop('disabled', false);

    buttonElement.removeAttr('disabled');

    buttonElement.css('pointer-events', '');
    buttonElement.removeAttr('tabindex');
}

$("input.groupedCheckbox:checkbox").click(function () {
    var group = "input:checkbox[data-group='" + $(this).attr("name") + "']";
    $(group).attr("checked", false);
    $(this).attr("checked", true);
});

//*******************************//
//   HRIS SPECIFIC UTILITIES     // 
//*******************************//
function AddTemporaryClass(element, className, miliseconds) {
    /// <summary>Adds the className to the element for the time specified and then removes it </summary>
    /// <param name="element" type="Object">Element to process</param>
    /// <param name="className" type="String">Class name to add</param>
    /// <param name="miliseconds" type="int">Time elapsed between add and remove</param>
    /// <returns> </returns>
    setTimeout(
        function () {
            element.addClass(className);

            setTimeout(function () {
                element.removeClass(className);
            }, miliseconds)
        }, 0);
}

function ErrorButton(elementId) {
    /// <summary>>Set the UI as error of the element</summary>
    /// <param name="elementId" type="String">Id of the element</param>
    setTimeout(function () {
        AddTemporaryClass($("#" + elementId), "btn-warning", 1500);

        $("#" + elementId).button('error');

        setTimeout(function () {
            $("#" + elementId).button('reset');
        }, 1500)
    }, 100);
}

function ResetButton(elementId) {
    /// <summary>>Reset the UI of the element</summary>
    /// <param name="elementId" type="String">Id of the element</param>
    setTimeout(function () { $('#' + elementId).button('reset'); }, 100);
}

function isEmptyOrSpaces(str) {
    /// <summary>Test if string is empty or whitespace</summary>
    /// <param name="str" type="String">String to test</param>
    /// <returns>True if empty or whitespace. False otherwise.</returns>
    return str === null || str.match(/^ *$/) !== null;
}

//////////////////////////////////////////////////////////////////////////////////////////////////
//In this section we initialize the popovers and tooltips for the elements.
//We have both (popovers and tooltips) in order to accomplish information alert over desktop 
//and mobiles devices
function InitializeTooltipsPopovers() {

    $('[data-toggle="tooltip"]').popover();
    $('body').on('click', function (e) {
        $('[data-toggle="tooltip"]').each(function () {
            //the 'is' for buttons that trigger popups
            //the 'has' for icons within a button that triggers a popup
            if (!$(this).is(e.target) && $(this).has(e.target).length === 0 && $('.popover').has(e.target).length === 0) {
                $(this).popover('hide');
            }
        });
    });

    $('[data-toggle="tooltip"]').tooltip({
        container: 'body',
        html: true,
        placement: 'bottom',
        title: function () {
            return $(this).attr("data-content");
        }
    });

    //Avoid overlap tooltip and popover
    $('.glyphicon.glyphicon-info-sign.text-center').on('click', function (e) { $('[data-toggle="tooltip"], .tooltip').tooltip("hide"); });

    //Avoid overlap tooltip and popover
    $('[data-toggle="tooltip"]').on('shown.bs.tooltip', function () {
        $('[data-toggle="tooltip"]').each(function () {
            $(this).popover('hide');
        });
    });
}

$(document).ready(function () {
    bindJqueryEventsInsidePagesUpdatePanels();

    // Get request manager instance
    var pageRqstMgr = Sys.WebForms.PageRequestManager.getInstance();
            
    pageRqstMgr.add_endRequest(function (sender, args) {
        /// <summary>re-binds ajax-related (via update panels) events before update panel begin request event and after update panel end request event.</summary>
        bindJqueryEventsInsidePagesUpdatePanels();
    });
});

function bindJqueryEventsInsidePagesUpdatePanels() {
    /// <summary>re-binds ajax-related (via update panels) events before update panel begin request event and after update panel end request event.</summary>
    // Bind and Configure the select picker controls    
    $(".selectpicker").selectpicker({
        size: 7
    });
    $(".selectpicker").selectpicker('refresh');

    // Bind the popover controls 
    $('[data-toggle="popover"]').popover();
    $('span[data-toggle="tooltip"]').tooltip();
}

//*******************************//
// MESSAGING AND CONFIRMATION    // 
//*******************************//
function ShowFooterMsg(msg, className) {
    /// <summary>Show a footer msg</summary>
    /// <param name="msg" type="String">Message to show</param>
    /// <param name="className" type="String">CSS className to use</param>
    $('.alert-autocloseable-msg').removeClass("alert-warning");
    $('.alert-autocloseable-msg').removeClass("alert-success");
    $('.alert-autocloseable-msg').removeClass("alert-danger");

    $('.alert-autocloseable-msg').addClass(className);
    $('.alert-autocloseable-msg').html(msg);
    $('.alert-autocloseable-msg').slideDown();
    $('.alert-autocloseable-msg').delay(2000).slideUp();
}

function ShowFooterAlert(msg) {
    /// <summary>Show a footer alert</summary>
    /// <param name="msg" type="String">Message to show</param>
    ShowFooterMsg(msg, "alert-warning");
}

function ShowFooterSuccess(msg) {
    /// <summary>Show a footer success</summary>
    /// <param name="msg" type="String">Message to show</param>
    ShowFooterMsg(msg, "alert-success");
}

function ShowFooterError(msg) {
    /// <summary>Show a footer error</summary>
    /// <param name="msg" type="String">Message to show</param>
    ShowFooterMsg(msg, "alert-danger");
}

function blockEnterKey() {
    /// <summary>Blocks or prevent the enter key</summary>
    if (event.keyCode === 10 || event.keyCode === 13) {
        event.preventDefault();
    }
}

function SetControlInvalid(controlId) {
    /// <summary>Set the appearance of the control as invalid </summary>
    /// <param name="controlId" type="String">Id of the control</param>            
    $('#' + controlId).addClass("Invalid");
    $('label[for=' + controlId + '].label-validation').show();
}

function SetControlValid(controlId) {
    /// <summary>Set the appearance of the control as valid </summary>
    /// <param name="controlId" type="String">Id of the control</param>                        
    $('#' + controlId).removeClass("Invalid");
    $('label[for=' + controlId + '].label-validation').hide();
}

var selectedLanguaje = 'es-CR'

function SetLanguaje(languaje) {
    /// <summary>Updates the selected languaje.</summary>
    selectedLanguaje = languaje;
    $(document).ready(function () {

        if (languaje === 'es-CR' || languaje === null || languaje === '') {
            $(".selectpicker").selectpicker({
                size: 7,
                noneSelectedText: 'No hay selección',
                noneResultsText: 'No hay resultados {0}',

                countSelectedText: function (numSelected, numTotal) {
                    return (numSelected == 1) ? "{0} ítem selecccionado" : "{0} ítems seleccionados";
                },

                maxOptionsText: function (numAll, numGroup) {
                    return [
                        (numAll == 1) ? 'Límite alcanzado ({n} ítem máximo)' : 'Límite alcanzado ({n} ítems máximo)',
                        (numGroup == 1) ? 'Límite del grupo alcanzado ({n} item max)' : 'Group limit reached ({n} items max)'
                    ];
                },

                selectAllText: 'Seleccionar Todos',
                deselectAllText: 'Desmarcar Todos',
                multipleSeparator: ', '
            });
        }

        else if (languaje === 'en-US') {
            $(".selectpicker").selectpicker({
                size: 7,
                noneSelectedText: 'Nothing selected',
                noneResultsText: 'No results match {0}',

                countSelectedText: function (numSelected, numTotal) {
                    return (numSelected == 1) ? "{0} item selected" : "{0} items selected";
                },

                maxOptionsText: function (numAll, numGroup) {
                    return [
                        (numAll == 1) ? 'Limit reached ({n} item max)' : 'Limit reached ({n} items max)',
                        (numGroup == 1) ? 'Group limit reached ({n} item max)' : 'Group limit reached ({n} items max)'
                    ];
                },

                selectAllText: 'Select All',
                deselectAllText: 'Deselect All',
                multipleSeparator: ', '
            });
        }

        $(".selectpicker").selectpicker('refresh');
    });
}

function MultiSelectDropdownListSaveSelectedItems(dropDownListControl, hiddenControl) {
    /// <summary>Stores the selected items to be able to recover them in an eventual postback.</summary>
    /// <param name="dropDownListControl" type="control">The multiple selection dropdown list control</param>
    /// <param name="hiddenControl" type="control">The hidden control where the selected items will be saved</param>
    var selectedItems = dropDownListControl.selectpicker('val');

    if (selectedItems.length > 0) {
        hiddenControl.val(selectedItems.toString());
    }
    else {
        hiddenControl.val('');
    }
}

function MultiSelectDropdownListRestoreSelectedItems(dropDownListControl, hiddenControl) {
    /// <summary>Recover the selected items that were stored before the postback and assign them to the control to be selected.</summary>
    /// <param name="dropDownListControl" type="The multiple selection dropdown list control.</param>
    /// <param name="hiddenControl" type="control">The hidden control where the selected items will be restored.</param>
    var selectedItems = hiddenControl.val();

    if (selectedItems !== '') {
        dropDownListControl.selectpicker('val', selectedItems.split(","));
    }
    else {
        dropDownListControl.selectpicker('val', '');
    }
}

//////////////////////////////////////////////////////////////////////////////////////////////////
//In this section we have the prototypes
"use strict"
    ; (function ($) {
        ///<summary>Prototype for sortChildren.</summary>
        $.fn.sortChildren = function (cmp) {
            return this.each(function () {
                var self = $(this),
                    children = $.makeArray(self.children());

                $.each(children.sort(cmp), function (i, child) {
                    self.append(child)
                })
            })
        }
    })(jQuery);