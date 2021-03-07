"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/SubdomainsHub").build();
document.getElementById("btnSend").disabled = true;
connection.on("ReceiveMessage", function (message) {
    var samp = document.createElement("samp");
    samp.textContent = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    document.getElementById("sublisterOutput").appendChild(samp);
    document.getElementById("sublisterOutput").scrollTop = document.getElementById("sublisterOutput").scrollHeight;
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

    var domain = document.getElementById("UserInput").value;
    connection.invoke("getDomain", domain).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});