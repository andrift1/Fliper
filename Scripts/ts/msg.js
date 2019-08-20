var Wiadomosci = (function () {

    //CONSTRUCTOR constructor !!!!!!!!!!!!!!!!!!!!!!!!!
    function Wiadomosci() {
        this.contosoChatHubProxy = $.connection.contosoChatHub;


       // this.idguid = document.getElementById("idchat").getAttribute("val");
        //this.idguidtosend = document.getElementById("idchattosend").getAttribute("val");
       // this.idchattosendyour = document.getElementById("idchattosendyour").getAttribute("val");

        this.YourID = document.getElementById("YourID").getAttribute("val");
        this.IdOtherUser = document.getElementById("IdOtherUser").getAttribute("val");


        this.textsend = document.getElementById("textclient");
        this.btnSend = document.getElementById("btnSend");
        this.chatmenu = document.getElementById("contants");
      
       this.chatinside = document.getElementById("msg");


        this.conn = "";
        this.Init();
        
       
        this.SetMsgMenu();
       
        //alert(":D   xsxs11");
    }










    Wiadomosci.prototype.Init = function () {
        let self = this;

        this.chatinside.scrollTo(0,document.body.scrollHeight);

        //alert("init");
        this.contosoChatHubProxy.client.addNewMessageToPage = function (YourID, IdOtherUser) {
            if(IdOtherUser == self.YourID){
                            if(self.IdOtherUser != YourID){
                                //alert(" do innej wiadomosci");
                                self.SetMsgMenu();
                                self.PlaySound();
                            } else {
                                //alert(YourID + "PRZYSZLA WIADOMOSC" + IdOtherUser);
                                self.GetMsg();
                                self.PlaySound();
                            }

            }
            //alert(IdOtherUser + " " + self.IdOtherUser)
           // self.contosoChatHubProxy.server.newContosoChatMessage(message, "XDD3 back");

        };

        $.connection.hub.start();

     /*    $.connection.hub.start().done(function () {
           // self.conn  = $.connection.hub.id;
       //     alert("111" +kogo+ "111");
           // self.SendGuid(self.conn);
            

            
            // var kogo = "bb97be41-ef34-4a8d-b75c-919f0af31d89";
            if(self.idguid != null){
                self.contosoChatHubProxy.server.newContosoChatMessage(self.idguid, self.idchattosendyour);
              //  alert("aaaaaaaa" + self.idchattosendyour + "aaaaaaaaaa");
            }
        }); */

        this.btnSend.addEventListener("click",function(){
            self.SendMsg();
        });

        self.textsend.addEventListener("keyup", function(event) {
            if (event.keyCode === 13) {
              btnSend.click();
            }
        });

        

    };

    Wiadomosci.prototype.SendMsg = function(){
       //var kogo = $.connection.hub.id;
       
     //  alert("111" +kogo+ "111");
        let self = this;
       var xhr = new XMLHttpRequest();
       var url = "../Api/SendMsg";
       xhr.open("POST", url, true);
       xhr.setRequestHeader("Content-Type", "application/json");
       xhr.onreadystatechange = function () {
           if (xhr.readyState === 4 && xhr.status === 200) {
             /////////////  alert("wiadmosc wyslana");
             self.contosoChatHubProxy.server.newContosoChatMessage(self.YourID, self.IdOtherUser);

           }
       };
       var data = JSON.stringify({"DoKogo": this.IdOtherUser, "Text":this.textsend.value });
       xhr.send(data);

       

        var para = document.createElement("P");                 
        var t = document.createTextNode(this.textsend.value)    
        para.appendChild(t);                                         
        this.chatinside.appendChild(para); 
        
        this.chatinside.scrollTo(0,document.body.scrollHeight);


       // let self = this;


        
            
       
        this.textsend.value = "";
    }


    Wiadomosci.prototype.GetMsg = function(){
        let self = this;
        
       var xhr = new XMLHttpRequest();
       var url = "../Api/GetMsg";
       xhr.open("POST", url, true);
       xhr.setRequestHeader("Content-Type", "application/json");
       xhr.onreadystatechange = function () {
           if (xhr.readyState === 4 && xhr.status === 200) {
              //alert(this.responseText);
                 var obj = JSON.parse(this.response);
                 var msgfinal = obj.tresc;


                var para = document.createElement("P");                 
                var t = document.createTextNode(msgfinal)    
                para.appendChild(t);                                         
                self.chatinside.appendChild(para); 
                
                self.chatinside.scrollTo(0,document.body.scrollHeight);
           }
       };
       var data = JSON.stringify({"DoKogo": this.IdOtherUser, "Text":"" });
       xhr.send(data);
    }

    Wiadomosci.prototype.PlaySound = function(){
        var audio = new Audio('../Content/sounds/parapa.mp3');
        audio.play();
    

    }
    

    Wiadomosci.prototype.SetMsgMenu = function(){


        var self = this;
        var xhr = new XMLHttpRequest();
        var url = "../Api/GetMsgMenu";
        xhr.open("POST", url, true);
        xhr.setRequestHeader("Content-Type", "application/json");
        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4 && xhr.status === 200) {

              //<div class="onecontact">         <p><span>Napisal do ciebie:  </span><h1>Hubert</h1> <span></span><span>How are ywhats is up...</span></p></div>
              self.chatmenu.innerHTML = "";
              var obj = JSON.parse(this.responseText);
              for (i = 0; i < obj.length; i++) {

                
                var ssss = obj[i].Nazwa;
                 var alltext = "<a href='Wiadomosci?userid="+ ssss +"'><div class='onecontact'><p><span>Napisal do ciebie:  </span><h1> "+ obj[i].Nazwa +" </h1> <span></span><span>Kliknij tu aby pisać...</span></p></div>"

                self.chatmenu.innerHTML += alltext;
              

              }
            }
        };
        var data = JSON.stringify({"DoKogo": this.IdOtherUser, "Text":"" });
        xhr.send(data);

    }
    

    return Wiadomosci;
}());


$(document).ready(function () {
    var wiadomosci = new Wiadomosci();
});