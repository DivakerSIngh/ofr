var countries = [];
var states = [];

function pairForCountries(geonameObj) {
    var result = Array();
    result[0] = geonameObj["name"];
    result[1] = geonameObj["geonameId"]
    return result;
}

function pairForCitiesOrStates(geonameObj) {
    var result = Array();
    result[0] = geonameObj["name"];
    result[1] = geonameObj["geonameId"]
    return result;
}

function listOfCountries(elemId) {
    countries = [];
    var countryGeonameIds = [6255146, 6255152, 6255147, 6255148, 6255149, 6255151, 6255150]
    var count = 0;
    for (x in countryGeonameIds) {
        count++;
        $.getJSON("http://www.geonames.org/childrenJSON?lang=en&geonameId=" + countryGeonameIds[x], function (data) {
            count--;
            $.merge(countries, data["geonames"]);
            if (count == 0)
                allCountries(elemId);
        });
    }
}

function listOfStatesOrCities(elem, elemId) {
    states = [];
    $.getJSON("http://www.geonames.org/childrenJSON?lang=en&geonameId=" + $("#" + elem.id + " option:selected").attr('data-role'), function (data) {
        states = data["geonames"];
        statesOrCities(elemId);
    });
}

function allCountries(elemId) {
    countries = (countries.map(pairForCountries)).sort();
    var src = document.getElementById(elemId);
    src.options.length = 0;
    var count = countries.length;
    if (count)
        src.options[src.options.length] = new Option('Select', '')
    else
        src.options[src.options.length] = new Option('No Data Available', '')
    for (var i = 0; i < count; i++) {
        var tmp = countries[i][0];
        var index = src.options.length;
        src.options[index] = new Option(tmp, tmp)
        $($("#" + elemId + " > option")[index]).attr('data-role', countries[i][1]);
    }

}

function statesOrCities(elemId) {
    states = (states.map(pairForCitiesOrStates)).sort();
    var src = document.getElementById(elemId);
    src.options.length = 0;
    var count = states.length;
    if (count) {
        src.options[src.options.length] = new Option('Select', '')
        src.disabled = false;
    }
    else {
        src.options[src.options.length] = new Option('No Data Available', '')
        src.disabled = true;
    }
    for (var i = 0; i < count; i++) {
        var tmp = states[i][0];
        var index = src.options.length;
        src.options[index] = new Option(tmp, tmp);
        $($("#" + elemId + " > option")[index]).attr('data-role', states[i][1]);
    }
}


if (true) {
    //<p>Country: 
    //    <select name="country" id="country" onchange="listOfStatesOrCities(this,'state');">
    //      <option value=""></option>            
    //    </select>
    //  </p>

    //  <p>State: 
    //    <select name="state" id="state" onchange="listOfStatesOrCities(this,'region');">
    //      <option value=""></option>
    //    </select>
    //  </p>

    //  <p>Region:
    //    <select name="region" id="region" onchange="listOfStatesOrCities(this,'city');">
    //      <option value=""></option>
    //    </select>
    //  </p>

    //  <p>City:
    //    <select name="city" id="city">
    //      <option value=""></option>
    //    </select>
    //  </p>
}