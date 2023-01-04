var IsVerified = true;

function ControlAdVisibility() {
    var tInput = $(this);
    $.ajax({
        url: "https://localhost:44366/Manager/Home/ControlAdVisibility",
        type: "POST",
        data: {
            id: $(this).attr("val")
        },
        beforeSend: function () {
            $("#status_" + $(tInput).attr("val")).text("...");
            $(tInput).prop("disabled", true);
        },
        success: function (data) {
            if (data == 'Activated') {
                $(tInput).val("Deactivated");
                $("#status_" + $(tInput).attr("val")).text(data);
            }
            else {
                $(tInput).val("Activated");
                $("#status_" + $(tInput).attr("val")).text(data);
            }
            $(tInput).prop("disabled", false);
        },
        error: function (error) {
            $(tInput).prop("disabled", false);
            console.log(error);
        }
    });
}

function onClickSort() {
    $.ajax({
        url: "https://localhost:44366/Manager/Home/Sort?IsVerified=" + IsVerified,
        type: "GET",
        success: function (data) {
            $("#listAd").html("");
            for (var i = 0; i < data.length; i++) {
                var tempHtml = "<tr>" +
                    "<td>" + data[i].id + "</td>" +
                    "<td>" + data[i].dateCreate + "</td>" +
                    "<td>" + data[i].name + "</td>" +
                    "<td>" + data[i].userId + "</td>";

                if (data[i].isVerified) {
                    tempHtml += "<td id='status_" + data[i].id + "'>Activated</td>" +
                        "<td><input style='min-width: 100px;' class='btnVisibilityAd' type='button' value='Deactivated' val='" + data[i].id + "'/>";
                }
                else {
                    tempHtml += "<td id='status_" + data[i].id + "'>Deactivated</td>" +
                        "<td><input style='min-width: 100px;' class='btnVisibilityAd' type='button' value='Activated' val='" + data[i].id + "'/>";
                }

                tempHtml += "<a href='/Home/AdDetails/" + data[i].id + "'>Details</a></td>" +
                    "<td></td>" +
                    "</tr>";
                $("#listAd").append(tempHtml);
            }

            $("input[class=btnVisibilityAd]").click(ControlAdVisibility);
            IsVerified = !IsVerified;
        },
        error: function (error) {
            console.log(error);
            alert(error);
        }
    });
}