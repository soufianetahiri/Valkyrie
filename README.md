
## What is it?
Valkiry is another OSINT tool, it allows you to perfom some basics recon/monitoring tasks.

## DNS Infos
Use this interface to get an idea on Current DNS records, Historical DNS records, Subdomains and Navigate through IP subnets and neighborhoods . 
All data is grabbed from https://securitytrailsr.com,

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
