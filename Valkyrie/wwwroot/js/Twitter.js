"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/TwitterHub").build();
document.getElementById("btnSend").disabled = true;

connection.on("ReceiveMessage", function (message) {
    document.getElementById("btnStop").disabled = false;
    var tweet = JSON.parse(message);
        const a = createElementWithClass('a', 'list-group-item list-group-item-action');
        const div = createElementWithClass('div', 'd-flex w-100 justify-content-between');
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
    //searchuser infos data
    $('#tweetsGroup h5').on('click', function (elm) {
        $("#loader").attr('style', 'display:flex');
        $.ajax({
            type: "POST",
            url: "/Twitter?handler=userdata",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("TryHarder",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: { userid: elm.target.id },
            success: function (response) {
                //$("#raw").text(response);
                alert(response);
            },
        });
    });
});
function createElementWithClass(type, className) {
    const element = document.createElement(type);
    element.className = className
    return element;
}
connection.start().then(function () {
    document.getElementById("btnSend").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});
document.getElementById("btnSend").addEventListener("click", function (event) {
 
    var hashtags = document.getElementById("UserInput").value;
    connection.invoke("getTweetsAsync", hashtags).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("btnStop").addEventListener("click", function (event) {
    connection.stop().then(function () {
        document.getElementById("btnStop").disabled = true;
        alert('monitoring stopped');
    });
    event.preventDefault();
});


