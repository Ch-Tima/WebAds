$(function () {

    GetCity();
    GetCategory();
    $("#btnAdd").on("click", AddAd);
});

function GetCity() {
    $.ajax({
        url: "../../api/City",
        type: "GET",
        success: function (data) {
            console.log(data);
            for (var i = 0; i < data.length; i++) {
                $("#listCity").append('<option value="' + data[i].id + '">' + data[i].name + '</option>')
            }
            console.log(data);
        },
        error: function (error) {
            console.log(error);
        }
    });
}

function GetCategory() {
    $.ajax({
        url: "../../api/Category",
        type: "GET",
        success: function (data) {
            console.log(data);
            for (var i = 0; i < data.length; i++) {
                $("#listCategory").append('<option value="' + data[i].id + '">' + data[i].name + '</option>')
            }
            console.log(data);
        },
        error: function (error) {
            console.log(error);
        }
    });
}
function AddAd() {

    var adForm = new FormData();

    adForm.append("ad.Name", $("#Name").val());
    adForm.append("ad.Content", $("#Content").val());
    adForm.append("ad.Price", $("#Price").val());

    adForm.append("ad.CityId", $("#listCity option:selected").val());
    adForm.append("ad.CategotyId", $("#listCategory option:selected").val());

    adForm.append("upload", $("#AdImg")[0].files[0]);

    $.ajax({
        url: "../../api/Ads",
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