"use strict";
var Global = /** @class */ (function () {
    function Global() {
        this.GetCountMsg();
        this.GetCountWow();
    }
    Global.prototype.GetCountMsg = function () {
        var stan = false;
        var f = document.getElementById('msgCount');
        var wartosc = f.getAttribute("val");
        var wartoscint = +wartosc;
        if (wartoscint > 0) {
            setInterval(function () {
                if (stan == false) {
                    f.innerText = "Nowa (" + wartosc + ")";
                    stan = true;
                }
                else {
                    f.innerText = "Wiadomosc";
                    stan = false;
                }
            }, 500);
        }
    };
    Global.prototype.GetCountWow = function () {
        var stan = false;
        var f = document.getElementById('wowCount');
        var wartosc = f.getAttribute("val");
        var wartoscint = +wartosc;
        if (wartoscint > 0) {
            setInterval(function () {
                if (stan == false) {
                    f.innerText = "Nowe (" + wartosc + ")";
                    stan = true;
                }
                else {
                    f.innerText = "W0W";
                    stan = false;
                }
            }, 500);
        }
    };
    return Global;
}());
var global = new Global();
