$(function () {

    alert("Hello");
    GetAd();

});


function GetAd() {

    $.ajax({
        url: "../api/Ad",
        type: "GET",
        success: function (data) {
            console.log(data);
        },
        error: function (err) {
            console.log(err);
        }
    });

}