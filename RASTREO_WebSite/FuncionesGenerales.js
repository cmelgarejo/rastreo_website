//<!-- Funciones comunes para funcionamiento de las paginas
//var g_updatelimit = 30;
var g_seconds = 0;
var timer_CD;
function countdown_timer(timer) {
    if (g_seconds < timer) {
        clearTimeout(timer_CD);
        g_seconds = timer;
        timer_CD = setTimeout("countdown_timer(" + (g_seconds) + ")", 1000);
        return;
    }
    var countdowntimer = document.getElementById("cd_timer");
    if (countdowntimer != null) {
        if (g_seconds > 0) {
            g_seconds -= 1;
            countdowntimer.value = g_seconds;            
            timer_CD = setTimeout("countdown_timer(" + g_seconds + ")", 1000);
        }
        if (g_seconds == 0) {
            //document.getElementById("cd_timer") = document.getElementById("txtTimeout");
            g_seconds = document.getElementById("txtTimeout").value;
            countdowntimer.value = g_seconds;            
            timer_CD = setTimeout("countdown_timer(" + g_seconds + ")", 1000);
        }
    } 
}
//countdown_timer()

function cronometro(timer) {
    //    if (seconds <= timer) {
    //        seconds = timer;
    //        //return;
    //    }
    var countdowntimer = document.getElementById("cronometro");
    if (countdowntimer != null) {
        if (timer < 0) {
            timer = 1;
        } else {
            timer++;
            countdowntimer.value = "Elapsados: " + timer + " seg."
        }
        setTimeout("cronometro(" + timer + ")", 1000);
    }
}
//cronometro()

function GetValorFromObj(NameObj) {
    var retS = '';
    sourceObj = $get(NameObj);
    if (sourceObj != null)
        retS = sourceObj.value;
    //alert("GetValorObj: " + retS)
    return retS;
}

function seguro_que() {
    var seguroque = confirm("Seguro que va eliminar el registro?.");
    if (seguroque == true) {
        return true;
    }
    else {
        return false;
    }
}

function DisableSaveControl(controlId) {
    document.getElementById(controlId).disabled = true;
    document.getElementById(controlId).value = "Espera, Por favor...";
    document.getElementById(controlId).title = "Por favor aguarde...";
}

function DisableControl(controlId) {
    document.getElementById(controlId).disabled = true;
}

function DisableControl_SetTimeout(controlId, interval) {
    setTimeout("DisableSaveControl('" + controlId + "')", interval);
}

function DisableSave(control) {
    DisableControl_SetTimeout(control.id, 100);
}

function AbrirDialogo(myPage, w, h) {
    var WinSettings = "status=yes,scrollbars=yes,help=no,center=yes";
    if (w != null && h != null) {
        WinSettings += ",width=" + w + ",height=" + h;
    }
    else {
        WinSettings += ",height=600";
    }
    // LA LINEA DE ABAJO ES MAGICA - Abre ventana modal :D
    //var MyArgs = window.showModalDialog(myPage, new Array(0), WinSettings);
    var MyArgs = window.open(myPage, 'popup', WinSettings);

}

//Para hacer visible cualquier control
function setVisible(control) {
    if (document.getElementById(control) != null) {
        var ctrl = document.getElementById(control);
        if (ctrl != null) {
            ctrl.visible = true;
        }
    }
}
//-->