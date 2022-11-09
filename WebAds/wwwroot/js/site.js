$(function () {
    GetCity();
    GetCategory();
});

var idCategory = -1;
var idCity = -1;

function GetAd() {

    $.ajax({
        url: "../api/ApiAds/" + (idCategory > 0 ? idCategory : -1) + "/" + (idCity > 0 ? idCity : -1),
        type: "GET",
        success: function (data) {
            $("#listAd").text("");

            for (var i = 0; i < data.length; i++) {
                $("#listAd").append('<form class="InputPageMovie" method="get" action="../Home/AdDetails">' +
                    '<input type="hidden" name="id" value="' + data[i].id + '"/>' +
                    '<div class="Ad" onclick="this.parentNode.submit();"> <img src="' + data[i].pathImg + '" style="width: 80px;">' +
                    '<div class="AddContent"> <h4 style="margin: auto;">' + data[i].name + '</h4>' +
                    '<p style="overflow: hidden;text-overflow: ellipsis;display: -webkit-box;-webkit-line-clamp: 3;-webkit-box-orient: vertical;font-size: 12px;">' + data[i].content + '</p>' +
                    '</div>' +
                    '<p style="color: forestgreen; font-size: 16px;">' + data[i].price + '$</p>' +
                    '</div>' +
                    '</form>');

            }
        },
        error: function (err) {
            console.log(err);
        }
    });

}

function AddAd(data)
{
    for (var i = 0; i < data.length; i++) {
        $("#listAd").append('<form class="InputPageMovie" method="get" action="../Home/AdDetails">' +
            '<input type="hidden" name="id" value="' + data[i].id + '"/>' +
            '<div class="Ad" onclick="this.parentNode.submit();"> <img src="' + data[i].pathImg + '" style="width: 80px;">' +
            '<div class="AddContent"> <h4 style="margin: auto;">' + data[i].name + '</h4>' +
            '<p style="overflow: hidden;text-overflow: ellipsis;display: -webkit-box;-webkit-line-clamp: 3;-webkit-box-orient: vertical;font-size: 12px;">' + data[i].content + '</p>' +
            '</div>' +
            '<p style="color: forestgreen; font-size: 16px;">' + data[i].price + '$</p>' +
            '</div>' +
            '</form>');

    }
}


//Get City
function GetCity() {
    $.ajax({
        url: "../../api/ApiCity",
        type: "GET",
        success: function (data) {
            console.log(data);
            $("#listAd").text("");//Clear ListAd
            for (var i = 0; i < data.length; i++) {
                

                if (data[i].ads.length > 0) {
                    $("#listCity").append('<li><a val="' + data[i].id + '">' + data[i].name + '</a></li>');
                    AddAd(data[i].ads);
                }

            }
            $("div[class='ListCity']").find("a").on("click", function () {
                idCity = $(this).attr("val");
                GetAd(idCategory, idCity);
                $("#cityName").text($(this).text());
            });
        },
        error: function (error) {
            console.log(error);
        }
    });
}
//Get Category
function GetCategory() {
    $.ajax({
        url: "../api/ApiCategory",
        type: "GET",
        success: function (data) {
            SetFilter(data, 0);
            $("div[class='ListCategory']").find("a").on("click", function () {
                idCategory = $(this).attr("val");
                GetAd(idCategory, idCity);
                $("#categoryName").text($(this).text());
            });
        },
        error: function (err) {
            console.log(err);
        }
    });

}

function SetFilter(arg, n) {
    for (var i = 0; i < arg.length; i++) {

        if (arg[i].categories != null) {

            var t = Math.floor(Math.random() * 9999999999) + 1;

            $("#n" + n).append("<li><a val='" + arg[i].id + "'>" + arg[i].name + "»</a><ul id='n" + t + "'>");

            SetFilter(arg[i].categories, t);

            $("#n" + n).append("</ul></li>");
        }
        else {
            $("#n" + n).append("<li><a val='" + arg[i].id + "'>" + arg[i].name + "</a></li>");
        }
    }
}