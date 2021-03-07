using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Valkyrie.Helper
{
    public class SubdomainsHub : Hub
    {
        IHubContext<SubdomainsHub> _hubContext = null;
        public SubdomainsHub(IHubContext<SubdomainsHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task SendMessage(string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
        }
        public void getDomain(string domain)
        {
            string pythonPath = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["PythonPath"];
            if (!string.IsNullOrEmpty(domain)) // totdo add is valid domain
            {
                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = pythonPath + "\\python.exe",
                        Arguments = Directory.GetCurrentDirectory() + $"\\Scripts\\Sublist3r\\sublist3r.py -d {domain} -v -t 12 -p 80,443,21,22",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };

                proc.OutputDataReceived += async (object sender, DataReceivedEventArgs e) =>
                {
                    if (!string.IsNullOrEmpty(e?.Data))
                    {
                        await SendMessage(e.Data);
                    }
                };
                proc.Start();
                proc.BeginOutputReadLine();

            }
        }
    }
}
