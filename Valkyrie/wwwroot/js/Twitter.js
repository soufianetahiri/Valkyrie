"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/TwitterHub").build();

 

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

 
connection.start();
document.getElementById("btnSend").addEventListener("click", function (event) {
 
    var hashtags = document.getElementById("UserInput").value;
    connection.invoke("getTweetsAsync", hashtags).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});