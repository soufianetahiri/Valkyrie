
## What is it?
Valkirye is another OSINT tool, it allows you to perfom some basics recon/monitoring tasks.
This tool stills under developpement, many other features are yet to come. I barly handled errors, and user inputs so don't be surprised if nothing worked :) . As allways, pull requests are welcomed !
Valkirye is built using [Razor pages](https://docs.microsoft.com/en-us/aspnet/core/razor-pages/?view=aspnetcore-5.0&tabs=visual-studio) and depends on [jQuery](https://jquery.com/),  [Bootstrap](https://getbootstrap.com/), [Datatables](https://datatables.net/) and [Chart.js](https://www.chartjs.org/). Realtime monitoring is handled by [SignalR](https://dotnet.microsoft.com/apps/aspnet/signalr)

Happy recon falks.

## DNS Infos
Use this interface to get an idea on Current DNS records, Historical DNS records, Subdomains and Navigate through IP subnets and neighborhoods . 
All data is grabbed from https://securitytrailsr.com, using there API, so before using it, go and grab an API key and edit the appsettings.json accordinly.

**IP address research**
Returns the neighbors in any given IP level range and essentially allows you to explore closeby IP addresses. It will divide the range into 16 groups. Example: a /28 would be divided into 16 /32 blocks or a /24 would be divided into 16 /28 blocks.

![IP address research](https://raw.githubusercontent.com/soufianetahiri/Valkyrie/master/Valkyrie/Screenshots/dnsinfo_IPsearch.JPG)

**Historical DNS records**
Lists out specific historical information about the given hostname parameter. In addition of fetching the historical data for a particular type, the count statistic is returned as well, which represents the number of that particular resource against current data. (a records will have an ip_count field which will represent the number of records that has the same IP as that particular record) The results are sorted first_seen descending. The number of results is not limited.

![Historical DNS records](https://raw.githubusercontent.com/soufianetahiri/Valkyrie/master/Valkyrie/Screenshots/dnsinfo_historicaldns.JPG)

**Get Subdomains**
Returns child and sibling subdomains for a given hostname. Limited to 2000 results for the Free plan and to 10000 for all paid subscriptions. (for more subdomains you can use (in-depth) Subs section)
![Get Subdomains](https://raw.githubusercontent.com/soufianetahiri/Valkyrie/master/Valkyrie/Screenshots/dnsinfo_subdomains.JPG)

## Certificate
Discovers certificates by searching all of the publicly known Certificate Transparency (CT) logs.
![Certificate](https://raw.githubusercontent.com/soufianetahiri/Valkyrie/master/Valkyrie/Screenshots/cert_search.JPG)
## Twitter
Keep an eye on latest tweets that contain hashtags/keywords, space separated, (monitor up to 5 keywords). Chart might be incomplete due to Twitter ratelimit.

New tweets containing your keywords are added as they are tweeted, you can grab the user's info (including avatar, location, profile creation date/time...) by clicking on the user name from the list.

![Twitter](https://raw.githubusercontent.com/soufianetahiri/Valkyrie/master/Valkyrie/Screenshots/twitter_monitor.jpg)

A chart is drawn to give you an idea on how much "talk" is out there about topics you monitor.
![Twitter](https://raw.githubusercontent.com/soufianetahiri/Valkyrie/master/Valkyrie/Screenshots/twitter_monitor_2.jpg)

## (in-depth)Subdomains
It fully relises on Sublist3r which is tool designed to enumerate subdomains of websites using OSINT. It helps penetration testers and bug hunters collect and gather subdomains for the domain they are targeting. Sublist3r enumerates subdomains using many search engines such as Google, Yahoo, Bing, Baidu and Ask. Sublist3r also enumerates subdomains using Netcraft, Virustotal, ThreatCrowd, DNSdumpster and ReverseDNS.

You need to ***MANUALLY*** install it; and to set your Python path on the appsettings.json. Ex (yes I revoked the API key ;)):

      "AppSettings": {
        "PythonPath": "C:\\Users\\soufiane.tahiri\\AppData\\Local\\Programs\\Python\\Python39",
        "SecuritytrailsAPIKey": "blw0TiVLivBCXwHXZgy212suJ35uejEK"
      }
![Sublist3r ](https://raw.githubusercontent.com/soufianetahiri/Valkyrie/master/Valkyrie/Screenshots/subdomains.jpg)

## Pastbin
This section helps to keep an eye on bins that contain keywords (space separated)  publicly available on Pastbin.com. 
![Pastbin](https://raw.githubusercontent.com/soufianetahiri/Valkyrie/master/Valkyrie/Screenshots/pastbin.jpg)

You can click on "Fetch" to try to get bins content, the generated chart allows you to know "how much leak"/month is out there.

![Pastbin](https://raw.githubusercontent.com/soufianetahiri/Valkyrie/master/Valkyrie/Screenshots/pastbin_rawdata.jpg)
