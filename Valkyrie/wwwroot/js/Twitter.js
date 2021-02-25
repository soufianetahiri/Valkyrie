﻿"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/TwitterHub").build();
document.getElementById("btnSend").disabled = true;
connection.on("ReceiveMessage", function (message) {
    var tweet = JSON.parse(message);
        const a = createElementWithClass('a', 'list-group-item list-group-item-action');
        const div = createElementWithClass('div', 'd-flex w-100 justify-content-between');
        a.appendChild(div);
        const h5 = createElementWithClass('h5', 'mb-1');
        h5.textContent = tweet.User.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;") + "said: ";
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
    //google.charts.load('current', { 'packages': ['corechart'] });
    //google.charts.setOnLoadCallback(drawChart);
    //function drawChart() {
    //    var jsonData = $.ajax({
    //        url: "/Twitter?handler=ChartData",
    //        dataType: "json",
    //        async: false
    //    }).responseText;
    //    var data = new google.visualization.DataTable(jsonData);
    //    var options = {
    //        'title': '#Tweets / Dayt',
    //        'width': 600,
    //        'height': 500
    //    };

    //    var chart = new google.visualization.ColumnChart(document.getElementById('chart'));
    //    chart.draw(data, options);
    //    console.log(data);
    //}
    connection.invoke("getTweetsAsync", hashtags).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});