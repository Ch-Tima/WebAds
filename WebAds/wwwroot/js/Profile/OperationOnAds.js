﻿//Get userAds
function GetUserAds(isPublic) {
    $.ajax({
        url: "../../api/ApiAds",
        type: "GET",
        data: {
            userId: $("#UserID").val()
        },
        success: function (data) {

            $("#userAds").html();
            for (var i = 0; i < data.length; i++) {

                var tempString = '<div id="Ad_' + data[i].id + '" class="Ad">' +
                    
                    '<div class="DataAd">' +
                    '<img src="' + data[i].pathImg + '" style="width: 80px;">' + 
                    '<div class="DataAdText">' +
                        '<p>Name:' + data[i].name + '"</p>' +
                        '<p>Price:' + data[i].price + '</p>' + 
                    '</div>' +
                    '</div>';


                if (isPublic) {
                    tempString += '<div class="btnControlAd">' +
                        '<button type="button" class="btnRemove" value="' + data[i].id + '">Remove</button>' +
                        '<form method="post" action="../../Profile/UpdateAd">' +
                        '<input type="hidden" value="' + data[i].id + '" name="idAd" />' +
                        '<button type="submit">Update</button></form></div>' +
                        '</div>';
                }
                else {
                    tempString += '</div>';
                }

                $("#userAds").append(tempString);
            }

            $("button[class='btnRemove']").on("click", function () {
                RemoveAd($(this).val());
            });
        },
        error: function (error) {
            console.log(error);
        }
    });
}

//Get City
function GetCity(selected = "") {
    $.ajax({
        url: "../../api/ApiCity",
        type: "GET",
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                if (selected == data[i].name) {
                    $("#listCity").append('<option value="' + data[i].name + '" selected>' + data[i].name + '</option>')
                }
                else {
                    $("#listCity").append('<option value="' + data[i].name + '">' + data[i].name + '</option>')
                }
            }
        },
        error: function (error) {
            console.log(error);
        }
    });
}
//Get Category
function GetCategory() {
    $.ajax({
        url: "../../api/ApiCategory",
        type: "GET",
        success: function (data) {
            SetFilter(data, 0);
            $("ul[class='Nav']").find("a").on("click", function () {
                $("#categotyId").val($(this).attr("val"));
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

//Add
function AddAd() {

    if ($("#Name").val().length == 0 || $("#Content").val() == 0 || $("#categotyId").val() == -1 || $("#AdImg")[0].files[0] == null) {
        alert("There are empty fields!");
    }
    else {
        var adForm = new FormData();

        adForm.append("ad.Name", $("#Name").val());
        adForm.append("ad.Content", $("#Content").val());
        adForm.append("ad.Price", $("#Price").val());

        adForm.append("ad.CityName", $("#listCity option:selected").val());
        adForm.append("ad.CategotyId", $("#categotyId").val());

        adForm.append("upload", $("#AdImg")[0].files[0]);

        $.ajax({
            url: "../../api/ApiAds",
            type: "PUT",
            contentType: false,
            processData: false,
            data: adForm,
            success: function (data) {
                alert(data);
                window.location.replace("../Profile")
            },
            error: function (error) {
                console.log(error);
            }
        });
    }
}

//Remove
function RemoveAd(idAd) {
    $.ajax({
        url: "../../api/ApiAds/" + idAd,
        type: "DELETE",
        success: function (data) {
            $("#Ad_" + idAd).remove();
            alert(data);
        },
        error: function (err) {
            alert(err);
        }
    });
}

function UpdateAd() {
    var newAd = new FormData();

    newAd.append("ad.Id", $("#idAd").val());
    newAd.append("ad.Name", $("#Name").val());
    newAd.append("ad.Content", $("#Content").val());
    newAd.append("ad.Price", $("#Price").val());

    newAd.append("ad.CityName", $("#listCity option:selected").val());
    newAd.append("ad.CategotyId", $("#categotyId").val());


    if ($("#AdImg")[0].files[0] != null) {
        newAd.append("upload", $("#AdImg")[0].files[0]);
    }
    newAd.append("ad.PathImg", $("#AdImgOld").val());

    $.ajax({
        url: "../../api/ApiAds",
        type: "POST",
        contentType: false,
        processData: false,
        data: newAd,
        success: function (data) {
            alert(data);
            window.location.replace("../Profile")
        },
        error: function (error) {
            console.log(error);
        }
    });
}

function MsgBox(title, msg) {
    $("body").append("<div id='msgBox' style='width: 150px; height: 125px; background-color: red; position: absolute;margin-left: auto; margin-right: auto;left: 0;right: 0;'>"
        + "<lable style='color: white;'>" + title + "</lable>"
        + "<p style='color: white;font-size: 17px; text-align:center;'>" + msg + "</p></div>");
}