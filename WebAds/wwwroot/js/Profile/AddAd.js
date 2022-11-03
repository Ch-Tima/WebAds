$(function () {

    GetCity();
    GetCategory();
    $("#btnAdd").on("click", AddAd);
});

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