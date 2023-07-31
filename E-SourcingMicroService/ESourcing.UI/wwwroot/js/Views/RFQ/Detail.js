////const { signalR } = require("../../../lib/microsoft-signalr/signalr");

var connection = new signalR.HubConnectionBuilder().withUrl("http://localhost:8001/AuctionHub").build();

var auctionId = document.getElementById("AuctionId").value;
var sellerUserName = document.getElementById("SellerUserName").value;
var groupName = `auction-${auctionId}`;
var sellerUserEmail = document.getElementById("Email").value;

//Disable send button until connection is established

document.getElementById("sendButton").disabled = true;


connection.start().then(() => {
    document.getElementById("sendButton").disabled = false;
    connection.invoke("AddToGroupAsync", sellerUserName,  groupName).catch((error) => {
        return console.error(error.toString());
    });
}).catch((error) => {
    return console.error(error.toString());
});


document.getElementById("sendButton").addEventListener("click", function () {

    var productId = document.getElementById("ProductId").value;
    var bid = document.getElementById("exampleInputPrice").value;

    var sendBidRequest = {
        AuctionId: auctionId,
        ProductId: productId,
        SellerUserName: sellerUserName,
        Email: sellerUserEmail,
        Price: parseFloat(bid).toString()
    };

    SendBid(sendBidRequest);
    event.preventDefault();
});




function SendBid(model) {
    $.ajax({
        url: "/Auction/SendBid",
        type: "POST",
        data: model,
        success: function (response) {
            if (response.isSuccess) {
                document.getElementById("exampleInputPrice").value = "";
                connection.invoke("SendBidAsync", groupName, auctionId).catch((error) => {
                    return console.error(error.toString());
                })
            }
        },
        error: function (sads, textStatus, errorThown) {
            console.log(textStatus + " / " + errorThown);
        }
    })
}

connection.on("Bids", function (bids) {
    addBidToTable(bids);
});

function addBidToTable(bids) {
    
    $(".bidLine").html("");

    bids.sort(compare);

    bids.forEach(value => {
        var str = "<tr>";
        str += `<td>${value.sellerUserName}</td>`;
        str += `<td>${value.price}</td>`;
        str += "</tr>"
        $(".bidLine").append(str);
    })

   
}

function compare(a, b) {
    if (a.price < b.price)
        return 1;
    if (a.price > b.price)
        return -1;

    return 0;
}

var finishButton = document.getElementById("finishButton");

finishButton.addEventListener("click", function () {
    var sendCompleteBidRequest = {
        AuctionId: auctionId
    };

    SendCompleteBid(sendCompleteBidRequest);
    event.preventDefault();

});

function SendCompleteBid(model) {
    $.ajax({
        url: "/Auction/CompleteBid",
        type: "POST",
        data: model,
        success: function (response) {
            if (response.isSuccess) {
                connection.invoke("SendCompleteBidAsync", groupName, auctionId).catch((error) => {
                    return console.error(error.toString());
                })
            }
        },
        error: function (sads, textStatus, errorThown) {
            console.log(textStatus + " / " + errorThown);
        }
    })
}

connection.on("CompleteBid", function (completeAuctionId) {
    if (completeAuctionId == auctionId) {
        document.getElementById("sendButton").disabled = true;
        alert("İhale Satışı Tamamlandı");

        setTimeout(function () {
            window.location.href = "/auction";
        }, 3000);

    }
});
