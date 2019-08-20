"use strict";
var Profile = /** @class */ (function () {
    function Profile() {
        this.headPhoto = document.getElementById("headerphoto");
        this.allImages = document.querySelectorAll("#smallphoto");
        this.nick = document.getElementById("nick");
        this.startID = "";
        if (this.headPhoto != null) {
            this.startID = this.headPhoto.getAttribute("number");
        }
        this.prev = document.getElementById("prev");
        this.next = document.getElementById("next");
        this.fajnyProfil = document.getElementById("fajnyProfil");
        this.Init();
    }
    Profile.prototype.Init = function () {
        if (this.headPhoto != null) {
            this.ChoosePhoto();
            this.NextImage();
            this.PrevImage();
        }
        this.polubProfil();
    };
    Profile.prototype.ChoosePhoto = function () {
        var self = this;
        for (var z = 0; z < this.allImages.length; z++) {
            this.allImages[z].addEventListener("click", function () {
                var imgurl = this.getAttribute("src");
                var thisid = this.getAttribute("number");
                if (imgurl != null) {
                    self.headPhoto.src = imgurl;
                    self.startID = thisid;
                }
            });
        }
    };
    Profile.prototype.NextImage = function () {
        var self = this;
        this.next.addEventListener("click", function () {
            var xhr = new XMLHttpRequest();
            var url = "Api/next";
            xhr.open("POST", url, true);
            xhr.setRequestHeader("Content-Type", "application/json");
            xhr.onreadystatechange = function () {
                if (xhr.readyState === 4 && xhr.status === 200) {
                    var json = JSON.parse(xhr.responseText);
                    if (json[0] != null) {
                        // console.log(json[0].id + ", " + json[0].nazwazdjecia);
                        self.headPhoto.src = "Content/photos/" + json[0].nazwazdjecia;
                        self.startID = json[0].id;
                    }
                }
            };
            var data = JSON.stringify({ "namewho": self.nick.innerText, "lastid": self.startID });
            xhr.send(data);
        });
    };
    Profile.prototype.PrevImage = function () {
        var self = this;
        this.prev.addEventListener("click", function () {
            var xhr = new XMLHttpRequest();
            var url = "Api/prev";
            xhr.open("POST", url, true);
            xhr.setRequestHeader("Content-Type", "application/json");
            xhr.onreadystatechange = function () {
                if (xhr.readyState === 4 && xhr.status === 200) {
                    var json = JSON.parse(xhr.responseText);
                    if (json[0] != null) {
                        //  console.log(json[0].id + ", " + json[0].nazwazdjecia);
                        self.headPhoto.src = "Content/photos/" + json[0].nazwazdjecia;
                        self.startID = json[0].id;
                    }
                }
            };
            var data = JSON.stringify({ "namewho": self.nick.innerText, "lastid": self.startID });
            xhr.send(data);
        });
    };
    Profile.prototype.polubProfil = function () {
        if (this.fajnyProfil != null) {
            var self_1 = this;
            this.fajnyProfil.addEventListener('click', function () {
                self_1.fade(this);
                var xhr = new XMLHttpRequest();
                var url = "Api/fajnyprofil";
                xhr.open("POST", url, true);
                xhr.setRequestHeader("Content-Type", "application/json");
                xhr.onreadystatechange = function () {
                    if (xhr.readyState === 4 && xhr.status === 200) {
                    }
                };
                var data = JSON.stringify({ "DlaKogo": self_1.nick.innerText });
                xhr.send(data);
            });
        }
    };
    Profile.prototype.fade = function (element) {
        var op = 1;
        var timer = setInterval(function () {
            if (op <= 0.1) {
                clearInterval(timer);
                element.style.display = 'none';
            }
            element.style.opacity = op;
            element.style.filter = 'alpha(opacity=' + op * 100 + ")";
            op -= op * 0.1;
        }, 50);
    };
    return Profile;
}());
window.onload = function () {
    var run = new Profile();
};
