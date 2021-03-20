"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/TwitterHub").build();
document.getElementById("btnSend").disabled = true;

connection.on("ReceiveMessage", function (message) {
    try {
    document.getElementById("btnStop").disabled = false;
    var tweet = JSON.parse(message);
        const a = createElementWithClass('a', 'list-group-item list-group-item-action');
    const div = createElementWithClass('div', 'd-flex w-100 justify-content-between');
    div.setAttribute("data-toggle", "tooltip");
    div.setAttribute("title", "Click on the username to grab infos");
        a.appendChild(div);
        const h5 = createElementWithClass('h5', 'mb-1');
    h5.textContent = tweet.User.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;") + "said: ";
    h5.setAttribute("id", tweet.User.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;"));
        div.appendChild(h5);
        const small = createElementWithClass('small', '');
        small.textContent = tweet.DateOfTweet;
        div.appendChild(small);
        const p = createElementWithClass('p', 'mb-1');
    p.textContent = tweet.FullText.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    a.appendChild(p);
    const link = createElementWithClass('small', "");
        link.textContent=tweet.Link;
        a.appendChild(link);
    document.getElementById("tweetsGroup").appendChild(a);
    document.getElementById("tweetsGroup").scrollTop = document.getElementById("tweetsGroup").scrollHeight;
    $(function () {
        $('[data-toggle="tooltip"]').tooltip()
    })
    //searchuser infos data
    $('#tweetsGroup h5').on('click', function (elm) {
        $.ajax({
            type: "POST",
            url: "/Twitter?handler=userdata",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("TryHarder",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: { userid: elm.target.id },
            success: function (response) {
                var tuser = JSON.parse(response);
                $("#twitterUserName").text(tuser.Fullname);
                $("#twitterUserName").attr("href", tuser.Profile);
                $("#twitterDescription").text(tuser.Description);
                $("#twitterCreationDate").text(tuser.CreationDate);
                $("#twitterLocationtwitterLocation").text(tuser.GeoLoc);
                $("#avatar").attr("src", tuser.Avatar);
                $(".cardTwitterheader").attr("style", "background: url(" + tuser.PrifileBannerImg + ");");
            },
        });
    });
    } catch (err) {
        toastr.error(err); 
    }
});
function createElementWithClass(type, className) {
    const element = document.createElement(type);
    element.className = className
    return element;
}
connection.start().then(function () {
    document.getElementById("btnSend").disabled = false;
}).catch(function (err) {
        toastr.error(err); 
});
document.getElementById("btnSend").addEventListener("click", function (event) {
 
    var hashtags = document.getElementById("UserInput").value;
    connection.invoke("getTweetsAsync", hashtags).catch(function (err) {
        toastr.error(err + " You probably reached the rate limit. Try again in 15 minutes."); 
    });
    event.preventDefault();
});

document.getElementById("btnStop").addEventListener("click", function (event) {
    connection.stop().then(function () {
        document.getElementById("btnStop").disabled = true;
        toastr.success("Monitoring stopped");
    });
    event.preventDefault();
});


