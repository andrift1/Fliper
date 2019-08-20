
class Global{



     constructor(){

         this.GetCountMsg();
         this.GetCountWow();

     }
 
     GetCountMsg(){
        var stan = false

        var f = document.getElementById('msgCount');
        var wartosc = f.getAttribute("val");  

        var wartoscint = +wartosc;
        if(wartoscint > 0){
        setInterval(function() {
          
            if(stan == false){
                f.innerText = "Nowa (" +wartosc +")";
              stan = true;
            } else{
                f.innerText = "Wiadomosc";
              stan = false;
            }
        }, 500); 
        }
        
    }

     
    GetCountWow(){
        var stan = false

        var f = document.getElementById('wowCount');
        var wartosc = f.getAttribute("val");  

        var wartoscint = +wartosc;
        if(wartoscint > 0){
        setInterval(function() {
    
            if(stan == false){
                f.innerText = "Nowe (" +wartosc +")";
              stan = true;
            } else{
                f.innerText = "W0W";
              stan = false;
            }
        }, 500); 
        }
        
    }
 }
 
 
 let global = new Global();