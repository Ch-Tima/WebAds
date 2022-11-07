//Get userAds
function GetAds(arg) {
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
                    '<p>Name:' + data[i].name + '"</p><p>Price:' + data[i].price + '</p>' +
                    '<img src="' + data[i].pathImg + '" style="width: 80px;">';


                if (arg) {
                    tempString += '<button type="button" class="btnRemove" value="' + data[i].id + '">Remove</button>' +
                        '<form method="post" action="../../Profile/UpdateAd">' +
                        '<input type="hidden" value="' + data[i].id + '" name="idAd" />' +
                        '<button type="submit">Update</button></form></div>';
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
function GetCity() {
    $.ajax({
        url: "../../api/ApiCity",
        type: "GET",
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                $("#listCity").append('<option value="' + data[i].id + '">' + data[i].name + '</option>')
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

    var adForm = new FormData();

    adForm.append("ad.Name", $("#Name").val());
    adForm.append("ad.Content", $("#Content").val());
    adForm.append("ad.Price", $("#Price").val());

    adForm.append("ad.CityId", $("#listCity option:selected").val());
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
            console.log(err);
        }
    });
}

function UpdateAd() {
    var newAd = new FormData();

    newAd.append("ad.Id", $("#idAd").val());
    newAd.append("ad.Name", $("#Name").val());
    newAd.append("ad.Content", $("#Content").val());
    newAd.append("ad.Price", $("#Price").val());

    newAd.append("ad.CityId", $("#listCity option:selected").val());
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