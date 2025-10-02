
// Validate date in control, if empty is valid
function ValidateDateInControl(dateInControl)
{
    var dateNoFormat = replaceGuion(dateInControl);
    var dateNoFormat = replaceSeparator(dateNoFormat);
    //IF empty is valid
    if (dateNoFormat == '')
    {
        return true;
    }

    var blnErrorFormat = false;
    dateInControl = replaceSeparator(dateInControl);
    if (dateInControl.length != 8) {
        return false;
    }

    var month = dateInControl.substring(0, 2);
    var day = dateInControl.substring(2, 4);
    var year = dateInControl.substring(4, 8);
    
    if (!isNaN(month)) {
        var monthEnt = parseInt(month);
        if (monthEnt > 0 && monthEnt < 13) {
            if (!isNaN(day)) {
                var dayEnt = parseInt(day);
                if (dayEnt > 0 && dayEnt < 32) {
                    if (!isNaN(year)) {
                        var yearEnt = parseInt(year);
                        if (yearEnt > 1900) {
                            if (dayEnt > 28) {
                                if (dayEnt > 28 && monthEnt == 2)
                                {
                                    if (dayEnt == 29 && isLeapYear(yearEnt))
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        blnErrorFormat = true;
                                    }
                                }
                                else if (dayEnt == 31 && (monthEnt == 1
                                            || monthEnt == 3
                                            || monthEnt == 5
                                            || monthEnt == 7
                                            || monthEnt == 8
                                            || monthEnt == 10
                                            || monthEnt == 12))
                                {
                                    return true;
                                } else if (dayEnt < 31) {
                                    return true;
                                }
                                blnErrorFormat = true;
                            } else {
                                return true;
                            }

                        } else {
                            blnErrorFormat = true;
                        }
                    } else {
                        blnErrorFormat = true;
                    }
                } else {
                    blnErrorFormat = true;
                }
            } else {
                blnErrorFormat = true;
            }
        } else {
            blnErrorFormat = true;
        }
    } else {
        blnErrorFormat = true;
    }
    if (blnErrorFormat) {
        return false;
    }
}

// Validate id the date is empty
function DateIsEmpty(dateValue)
{
    var dateNoFormat = replaceGuion(dateValue);
    var dateNoFormat = replaceSeparator(dateNoFormat);
    //IF empty is valid
    if (dateNoFormat == '') {
        return true;
    }
    return false;
}

// Validate if the year is leap year
function isLeapYear(year) {
    if ((year % 4 == 0)
        && ((year % 100 != 0)
        || (year % 400 == 0)))
    {
        return true;
    }
    return false;
}

// Replace all character / -> empty 
function replaceSeparator(date) {
    while (date.indexOf("/") > -1) {
        date = date.replace("/", "");
    }
    return date;
}

// Replace all character _ -> empty
function replaceGuion(date) {
    while (date.indexOf("_") > -1) {
        date = date.replace("_", "");
    }
    return date;
}

// add separator character (/) to date
function addSeparators(date) {
    var dateSeparator;
    if (date.indexOf("/") == -1) {
        dateSeparator = date.substring(0, 2) + '/';
        dateSeparator = dateSeparator + date.substring(2, 4) + '/';
        dateSeparator = dateSeparator + date.substring(4, 8);
        return dateSeparator;
    }
    return date;
}

// Validate if the date is greater than the maximum allowed
function validDateMax(dateInControl, dateMax) {
    if (dateMax === "") {
        return true;
    }
    var dateInControlArreglo = dateInControl.split("/");
    var dateMaxArray = dateMax.split("/");

    if (parseInt(dateMaxArray[2]) < parseInt(dateInControlArreglo[2]))
    {
        return false;
    }
    if (parseInt(dateMaxArray[0]) < parseInt(dateInControlArreglo[0])
        && parseInt(dateMaxArray[2]) <= parseInt(dateInControlArreglo[2]))
    {
        return false;
    }
    if (parseInt(dateMaxArray[1]) < parseInt(dateInControlArreglo[1])
        && parseInt(dateMaxArray[0]) <= parseInt(dateInControlArreglo[0])
        && parseInt(dateMaxArray[2]) <= parseInt(dateInControlArreglo[2]))
    {
        return false;
    }
    return true;
}
