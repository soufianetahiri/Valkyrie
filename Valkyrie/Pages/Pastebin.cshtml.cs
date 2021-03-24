using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ChartJSCore.Helpers;
using ChartJSCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Valkyrie.Helper;
using Chart = ChartJSCore.Models.Chart;

namespace Valkyrie.Pages
{
    public class PastbinModel : PageModel
    {
        private readonly string PastBinUrl = "https://pastebin.com/raw/#bin#";
        private readonly string PsDumpUrl = "https://psbdmp.ws/api/search/#keyword#";

        public void OnGet()
        {
        }
        private static Chart GenerateLineScatterChart()
        {
            Chart chart = new Chart();
            chart.Type = Enums.ChartType.Line;

            Data data = new Data();
            data.Labels = new List<string>() { "January", "February", "March", "April", "May", "June", "July" };



            data.Datasets = new List<Dataset>();


            Options options = new Options()
            {
                Scales = new Scales()
            };

            Scales scales = new Scales()
            {
                YAxes = new List<Scale>()
                {
                    new CartesianScale()
                }
            };

            CartesianScale yAxes = new CartesianScale()
            {
                Ticks = new Tick()
            };

            Tick tick = new Tick()
            {
                Callback = "function(value, index, values) {return '$' + value;}"
            };

            yAxes.Ticks = tick;
            scales.YAxes = new List<Scale>() { yAxes };
            options.Scales = scales;
            chart.Options = options;

            chart.Data = data;

            return chart;


        }

        static LineDataset CreateChartDataSet(string label, List<double?> data)
        {
            return new LineDataset()
            {
                Label = label,
                Data = data,
                BackgroundColor = ChartColor.CreateRandomChartColor(true),
                BorderColor = ChartColor.CreateRandomChartColor(true),
                PointBorderColor = new List<ChartColor>() { ChartColor.CreateRandomChartColor(true) },
                PointBackgroundColor = new List<ChartColor>() { ChartColor.CreateRandomChartColor(true) },
                PointHoverBackgroundColor = new List<ChartColor>() { ChartColor.CreateRandomChartColor(true) },
                PointHoverBorderColor = new List<ChartColor>() { ChartColor.CreateRandomChartColor(true) },
                Fill = "true"
            };
        }
        public async Task<ContentResult> OnPostRawDataAsync(string binid)
        {
            if (!string.IsNullOrEmpty(binid))
            {
                try
                {
                    HttpAsync httpAsync = new HttpAsync();
                    string response = await httpAsync.Get(PastBinUrl.Replace("#bin#", binid));
                    return Content(response);
                }
                catch (Exception ex)
                {

                    return Content(ex.Message);
                } 
            }
            else
            {
                return Content("No data not found");
            }
        }
        public async Task<JsonResult> OnPostAsync(string UserInput)
        {
            if (!string.IsNullOrEmpty(UserInput))
            {
                List<BinResult> binResults = new List<BinResult>();
                string[] keywords = UserInput.Split(" ").Take(5).ToArray();
                List<string> jReturns = new List<string>();
                foreach (string keyword in keywords)
                {
                    //    data.Labels.Add(keyword);
                    //todo add link to pastbin raw data based on binID
                    try
                    {
                        HttpAsync httpAsync = new HttpAsync();
                        string response = await httpAsync.Get(PsDumpUrl.Replace("#keyword#", keyword));
                        if (!string.IsNullOrEmpty(response))
                        {
                            try
                            {
                                binResults.Add(JsonConvert.DeserializeObject<BinResult>(response));
                                jReturns.Add(response);
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                //Create chart
                Chart chart = new Chart();
                chart.Type = Enums.ChartType.Line;
                LineDataset lineDataset = new LineDataset();
                Data data = new Data();
                data.Labels = new List<string>();
                data.Datasets = new List<Dataset>();
                foreach (BinResult binResult in binResults)
                {
                    LineDataset dataset = new LineDataset();
                    List<double?> c = new List<double?>();
                    foreach (var line in binResult.data.GroupBy(f => DateTime.Parse(f.Time).ToUniversalTime()
               .ToString("Y", System.Globalization.CultureInfo.CreateSpecificCulture("en-US")))
                               .Select(group => new
                               {
                                   xAxis = group.Key,
                                   yAxis = group.Count()
                               })
                               .OrderBy(x => x.xAxis))
                    {
                        Debug.WriteLine(line.xAxis);
                        if (!data.Labels.Contains(line.xAxis))
                        {
                            data.Labels.Add(line.xAxis);
                        }

                        if (c.Count() < data.Labels.IndexOf(line.xAxis))
                        {
                            for (int i = c.Count(); i < data.Labels.IndexOf(line.xAxis); i++)
                            {
                                c.Add(0);
                            }
                        }
                        c.Insert(data.Labels.IndexOf(line.xAxis), (line.yAxis));
                        Debug.WriteLine("{0} {1} {2}", binResult.Search, line.xAxis, line.yAxis);
                    }
                    lineDataset = CreateChartDataSet(binResult.Search, c);
                    data.Datasets.Add(lineDataset);
                }
                Options options = new Options()
                {
                    Scales = new Scales(),
                    Responsive = true,
                    MaintainAspectRatio = true
                };

                Scales scales = new Scales()
                {
                    YAxes = new List<Scale>()
                {
                    new CartesianScale()
                },
                    XAxes = new List<Scale>()
                {
                    new CartesianScale()
                    {
                        Type = "category",
                        Position = "bottom",
                        Ticks = new CartesianLinearTick()
                        {
                            BeginAtZero = false
                        }
                    }
                }
                };

                CartesianScale yAxes = new CartesianScale()
                {
                    Ticks = new Tick()
                };

                Tick tick = new Tick()
                {
                    Callback = "function(value, index, values) {return '#of bins' + value;}"
                };

                yAxes.Ticks = tick;
                scales.YAxes = new List<Scale>() { yAxes };
                options.Scales = scales;
                chart.Options = options;
                chart.Data = data;
                return new JsonResult(new { table = jReturns, chart = chart.CreateChartCode("lineChart") });
            }
            return null;

        }
    }


    public class BinResult
    {
        public string Search { get; set; }
        public int Count { get; set; }
        public List<BinData> data { get; set; }
    }
    public class BinData
    {

        public string Date { get; set; }
        public string Tags { get; set; }
        public string Id { get; set; }
        public string Time { get; set; }
        public string Text { get; set; }
        public string Search { get; set; }

    }
}
