$(function () {

    //alert("Hello");
    //GetAd();
    GetCategory();
});


function GetAd() {

    $.ajax({
        url: "../api/Ads",
        type: "GET",
        success: function (data) {
            console.log(data);
        },
        error: function (err) {
            console.log(err);
        }
    });

}

function GetCategory() {

    $.ajax({
        url: "../api/ApiCategory",
        type: "GET",
        success: function (data) {
            //console.log(data);

            $("#Filter").append("<ul id='n0'>");

            SetFilter(data, 0);

            $("#Filter").append("</ul>");


        },
        error: function (err) {
            console.log(err);
        }
    });

}

function SetFilter(arg, n) {

    console.log(arg);
    console.log(n);
    console.log("--------------------------------------------------------------");



    for (var i = 0; i < arg.length; i++) {
        if (arg[i].categories != null) {

            $("#n" + n).append("<li>" + arg[i].name + "</li>");

            var t = Math.floor(Math.random() * 9999999999) + 1;

            $("#n" + n).append("<ul id='n" + t + "'>");

            SetFilter(arg[i].categories, t);

            $("#n" + n).append("</ul>")
        }
        else {
            $("#n" + n).append("<li>" + arg[i].name + "</li>");
        }
    }
}