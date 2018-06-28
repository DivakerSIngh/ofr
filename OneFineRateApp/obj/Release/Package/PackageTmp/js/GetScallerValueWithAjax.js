var xmlhttp;
function getDetails(str, pagename, divname) {
    var url = pagename  + str;
    xmlhttp = null;
    if (window.XMLHttpRequest) {
        xmlhttp = new XMLHttpRequest();
    }
    else if (window.ActiveXObject) {
        new ActiveXObject("Microsoft.XMLHTTP");
    }
    if (xmlhttp != null) {
        xmlhttp.onreadystatechange = function() {
            if (xmlhttp.readyState == 4) {
                if (xmlhttp.status == 200) {
                    var msgdiv = document.getElementById(divname);
                    msgdiv.innerHTML = xmlhttp.responseText;
                }
            }
        };
        xmlhttp.open("GET", url, true);
        xmlhttp.send(null);
    }
    else {
        alert("Your browser does not support XMLHTTP.");
    }
}