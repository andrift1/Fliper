class Profile{

public headPhoto: HTMLImageElement;
public allImages: NodeListOf<HTMLImageElement>
public nick:HTMLElement;

public prev: HTMLElement;
public next: HTMLElement;

public fajnyProfil:HTMLElement;

public startID:string | null;


    constructor(){
        
        this.headPhoto = <HTMLImageElement>document.getElementById("headerphoto");
        this.allImages = document.querySelectorAll("#smallphoto");
        this.nick = <HTMLElement>document.getElementById("nick");


        this.startID = "";
        if(this.headPhoto != null){
        this.startID = this.headPhoto.getAttribute("number");
        }   
        this.prev = <HTMLElement>document.getElementById("prev");
        this.next = <HTMLElement>document.getElementById("next");


        this.fajnyProfil = <HTMLElement>document.getElementById("fajnyProfil");


        this.Init();
        
        
    }

    Init(){  
        if(this.headPhoto != null){
        this.ChoosePhoto();
        this.NextImage();
        this.PrevImage();
        }

        this.polubProfil();

    }


    ChoosePhoto(){
        let self = this;

        for (let z = 0; z < this.allImages.length; z++) {

            this.allImages[z].addEventListener("click", function(){
                let imgurl = this.getAttribute("src");
                let thisid = this.getAttribute("number");

                if(imgurl != null){
                self.headPhoto.src = imgurl;
                self.startID = thisid;
                }

              });

              
        } 
    }


    NextImage(){
        let self = this;
        this.next.addEventListener("click", function(){
            var xhr = new XMLHttpRequest();
            var url = "Api/next";
            xhr.open("POST", url, true);
            xhr.setRequestHeader("Content-Type", "application/json");
            xhr.onreadystatechange = function () {
                if (xhr.readyState === 4 && xhr.status === 200) {
                    var json = JSON.parse(xhr.responseText);
                    if(json[0] != null){
                       // console.log(json[0].id + ", " + json[0].nazwazdjecia);
                        self.headPhoto.src = "Content/photos/" + json[0].nazwazdjecia;
                        self.startID = json[0].id;
                    }
                }
            };
            var data = JSON.stringify({"namewho":self.nick.innerText,"lastid":self.startID});
            xhr.send(data);
        });
    }

    PrevImage(){
        let self = this;
        this.prev.addEventListener("click", function(){
            var xhr = new XMLHttpRequest();
            var url = "Api/prev";
            xhr.open("POST", url, true);
            xhr.setRequestHeader("Content-Type", "application/json");
            xhr.onreadystatechange = function () {
                if (xhr.readyState === 4 && xhr.status === 200) {
                    var json = JSON.parse(xhr.responseText);
                    if(json[0] != null){
                      //  console.log(json[0].id + ", " + json[0].nazwazdjecia);
                        self.headPhoto.src = "Content/photos/" + json[0].nazwazdjecia;
                        self.startID = json[0].id;
                    }
                }
            };
            var data = JSON.stringify({"namewho":self.nick.innerText,"lastid":self.startID});
            xhr.send(data);
        });
    }

    public polubProfil(){
        if(this.fajnyProfil != null){
            let self = this;
            this.fajnyProfil.addEventListener('click',function(){
                self.fade(this);
                var xhr = new XMLHttpRequest();
                var url = "Api/fajnyprofil";
                xhr.open("POST", url, true);
                xhr.setRequestHeader("Content-Type", "application/json");
                xhr.onreadystatechange = function () {
                    if (xhr.readyState === 4 && xhr.status === 200) {
                  
                    }
                };
                var data = JSON.stringify({"DlaKogo":self.nick.innerText });
                xhr.send(data);











            });
        }
    }


    public fade(element:any) {
        var op = 1; 
        var timer = setInterval(function () {
            if (op <= 0.1){
                clearInterval(timer);
                element.style.display = 'none';
            }
            element.style.opacity = op;
            element.style.filter = 'alpha(opacity=' + op * 100 + ")";
            op -= op * 0.1;
        }, 50);
    }
}

window.onload = function () { 
let run = new Profile();
}