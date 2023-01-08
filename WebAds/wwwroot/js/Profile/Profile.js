function GetFavouriteAds() {
    $.ajax({
        url: "../../api/ApiFavouritesAd",
        type: "GET",
        success: function (data) {

            $("#listAds").empty();

            for (var i = 0; i < data.length; i++) {

                var tempString = '<form class="InputPageMovie" method="get" action="../../../Home/AdDetails">' +
                    '<div id="Ad_' + data[i].ad.id + '" class="Ad" onclick="this.parentNode.submit();">' +
                    '<input type="hidden" name="id" value="' + data[i].ad.id + '"/>' +
                    '<div class="DataAd">' +
                    '<img src="' + data[i].ad.pathImg + '" style="width: 80px;">' +
                    '<div class="DataAdText">' +
                    '<p>Name:' + data[i].ad.name + '"</p>' +
                    '<p>Price:' + data[i].ad.price + '</p>' +
                    '</div></div></div>' + 
                    '</form>';

                $("#listAds").append(tempString);
            }
        },
        error: function (error) {
            console.log(error);
        }
    });
}