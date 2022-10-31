$(function () {

    GetAds();

});
function GetAds() {
    $.ajax({
        url: "../../api/Ads",
        type: "GET",
        data: {
            userId: $("#UserID").val()
        },
        success: function (data) {

            $("#userAds").html();
            for (var i = 0; i < data.length; i++) {
                $("#userAds").append('<div style="width: 25%; background-color:antiquewhite;">' +
                    '<p>Name:' + data[i].name + '"</p><p>Price:' + data[i].price + '</p>' +
                    '<img src="' + data[i].pathImg + '" style="width: 80px;"></div>');
            }
        },
        error: function (error) {
            console.log(error);
        }
    });
}