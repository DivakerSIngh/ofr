function SelectAllCheckboxesSpecific(spanChk, grd) {

     var IsChecked = spanChk.checked;
    var Chk = spanChk;
    Parent = document.getElementById(grd);
    var items = Parent.getElementsByTagName('input');
    for (i = 0; i < items.length; i++) {
        if (items[i].type == "checkbox") {
            if (items[i].checked != IsChecked) {
                items[i].click();
            }
        }
    }
}
function func_validate_chkbox(para) {
    var Parent = document.getElementById('<%=gv.ClientID %>');
    var items = Parent.getElementsByTagName('input');
    var ChildControl = "chkactive";
    for (i = 0; i < items.length; i++) {
        if (items[i].type == "checkbox" && items[i].checked == true) {
            return confirm("Are You Sure You Want " + para + ".");
        }
    }
    alert("Select Atleast One Checkbox");
    return false;
}
function valOnlyOneChkSelect(grd, id, redirectUrl) {

    var Parent = document.getElementById(grd);
    var items = Parent.getElementsByTagName('tr');
    counter = 0
    chkId = 0;
    for (i = 0; i < items.length; i++) {
        var Flag = false;
        cellVal = items[i].cells[0].getElementsByTagName('input');
        for (j = 0; j < cellVal.length; j++) {

            if (cellVal[j].type == "checkbox") {
                if (cellVal[j].checked == true) {
                    counter++;
                    Flag = true;
                }
            }
            if (Flag) {
                if (cellVal[j].type == "hidden") {
                    if (cellVal[j].id == id) {
                        chkId = cellVal[j].value;
                    }
                }
            }

        }
    }
    if (counter == 0) {
        alert("Select any one checkbox!!!");
        return false;
    }
    if (counter != 1 && counter > 0) {
        alert("Select only one row at a time!!!");
        return false;
    }
    window.location.href = redirectUrl + chkId.toString();
}

/* this function is for webgrid only, when the hidden field must be placed in 2nd column */
function valOnlyOneChkSelectForWebgrid(grd, id, redirectUrl) {

  var Parent = document.getElementById(grd);
  var items = Parent.getElementsByTagName('tr');
  counter = 0
  chkId = 0;
  for (i = 0; i < items.length; i++) {
    var Flag = false;
    cellVal = items[i].cells[0].getElementsByTagName('input');
    cellVal1 = items[i].cells[1].getElementsByTagName('input');
    for (j = 0; j < cellVal.length; j++) {

      if (cellVal[j].type == "checkbox") {
        if (cellVal[j].checked == true) {
          counter++;
          Flag = true;
        }
      }

      if (Flag) {
        if (cellVal1[j] != null) {
          if (cellVal1[j].type == "hidden") {

            //alert("cellVal " + cellVal1[j].id)
            //alert("id " + id)

            if (cellVal1[j].id == id) {
              chkId = cellVal1[j].value;
            }
          }
        }
      }

    }
  }
  if (counter == 0) {
    alert("Select any one checkbox");
    return false;
  }
  if (counter != 1 && counter > 0) {
    alert("select only one row at a time.");
    return false;
  }
  window.location.href = redirectUrl + chkId.toString();
  //return false;
}

function active_Deact(chkAct, act, dact) {

    if (chkAct.checked) {
        document.getElementById(act).style.display = "none";
        document.getElementById(dact).style.display = "block";
    }
    else {
        document.getElementById(act).style.display = "block";
        document.getElementById(dact).style.display = "none";
    }
}
function get_treeValue(containerId, hfID) {
    var Parent = document.getElementById(containerId);
    var items = Parent.getElementsByTagName('input');
    var menuID = '';
    for (i = 0; i < items.length; i++)
    {
        if (items[i].type == "checkbox" && items[i].checked == true)
        {
          menuID = menuID + ',' + items[i].getAttribute("datafield");
        }
    }
   
    document.getElementById(hfID).value = '';
    document.getElementById(hfID).value = menuID;
   
}
function enableDisableChield(chk) {
  var childeren = document.getElementsByName(chk.datafield);
    for (j = 0; j < childeren.length; j++) {
        if (childeren[j].type == "checkbox") {
            childeren[j].checked = chk.checked;
        }
    }
}
