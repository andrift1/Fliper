



    var xhr = new XMLHttpRequest();
    var url = "../Api/IsGuidNull";
    xhr.open("GET", url, true);
    xhr.setRequestHeader("Content-Type", "application/json");
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200) {
            if(xhr.response === '"neeedtoaddnew"'){
                alert("trzeba dodac")
                $.connection.hub.start().done(function () {
                    var kogo  = $.connection.hub.id;
                
                
                    var xhr2 = new XMLHttpRequest();
                    var url = "../Api/AddYourGuid";
                    xhr2.open("POST", url, true);
                    xhr2.setRequestHeader("Content-Type", "application/json");
                    xhr2.onreadystatechange = function () {
                        if (xhr2.readyState === 4 && xhr2.status === 200) {
                            alert("poszloooo");
                        }
                    };
                    var data = JSON.stringify({"Guid":kogo });
                    xhr2.send(data); 
                });
                
                
                
            } else if (xhr.response === '"alreasyhas"'){
                alert("juz jest key")
            }
            
        }
    };
    //var data = JSON.stringify({"Guid":kogo });
    xhr.send();

