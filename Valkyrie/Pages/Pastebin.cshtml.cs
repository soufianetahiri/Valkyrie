using System;
using System.Collections.Generic;
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
                Fill = "false",
                LineTension = 0.1,
                BackgroundColor = ChartColor.FromRgba(75, 192, 192, 0.4),
                BorderColor = ChartColor.FromRgba(75, 192, 192, 1),
                BorderCapStyle = "butt",
                BorderDash = new List<int> { },
                BorderDashOffset = 0.0,
                BorderJoinStyle = "miter",
                PointBorderColor = new List<ChartColor>() { ChartColor.FromRgba(75, 192, 192, 1) },
                PointBackgroundColor = new List<ChartColor>() { ChartColor.FromHexString("#fff") },
                PointBorderWidth = new List<int> { 1 },
                PointHoverRadius = new List<int> { 5 },
                PointHoverBackgroundColor = new List<ChartColor>() { ChartColor.FromRgba(75, 192, 192, 1) },
                PointHoverBorderColor = new List<ChartColor>() { ChartColor.FromRgba(220, 220, 220, 1) },
                PointHoverBorderWidth = new List<int> { 2 },
                PointRadius = new List<int> { 1 },
                PointHitRadius = new List<int> { 10 },
                SpanGaps = false
            };
        }
        public async Task<JsonResult> OnPostAsync(string UserInput)
        {
            List<BinResult> binResults = new List<BinResult>();
            string[] keywords = UserInput.Split(" ").Take(5).ToArray();
            List<string> jReturns = new List<string>();
            Chart chart = new Chart();
            chart.Type = Enums.ChartType.Line;

            Data data = new Data();
            data.Labels = new List<string>();// { "January", "February", "March", "April", "May", "June", "July" };
            LineDataset lineDataset = new LineDataset();
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
                        catch (Exception ec)
                        {
                        }

                    }
                }
                catch (Exception ex)
                {

                }
            }
            //Create chart
            //var x = binResults.GroupBy(s => s.data.Select(f => DateTime.Parse(f.Date).ToUniversalTime()
            //.ToString("Y", System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))).ToList()).ToList();
            data.Datasets = new List<Dataset>();
            foreach (BinResult binResult in binResults)
            {
                LineDataset dataset = new LineDataset();
               
                List<double?> c = new List<double?>();
                var Ordered = binResult.data.GroupBy(f => DateTime.Parse(f.Time).ToUniversalTime()
           .ToString("Y", System.Globalization.CultureInfo.CreateSpecificCulture("en-US")));

                foreach (var grp in Ordered)
                {
                    data.Labels.Add(grp.Key);

                    c.Add(grp.Count());
                    Console.WriteLine("{0} {1}", grp.Key, grp.Count());
                }
                dataset = CreateChartDataSet(binResult.Search, c);
                data.Datasets.Add(dataset);
            }


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
            data.Labels = data.Labels.Distinct().ToList();
            chart.Data = data;












            return new JsonResult(new { table = jReturns, chart = chart.CreateChartCode("lineChart") });



            //List<string> colors = new List<string>() { "#FF6384", "#4BC0C0", "#FFCE56", "#E7E9ED", "#36A2EB" };

            //List<TweetsData> tweets = GetTweets(hashtags);

            //var dates = tweets.GroupBy(d => d.Date).Select(k => k.Key).ToList();
            //Chart chart = new Chart
            //{
            //    Type = Enums.ChartType.PolarArea
            //};

            //Data data = new Data();
            //data.Labels = tweets.GroupBy(k => k.Keyword).Select(k => k.Key).ToList();
            //PolarDataset dataset = new PolarDataset()
            //{
            //    Label = "Keywords dataset",
            //    BackgroundColor = new List<ChartColor>(),
            //    Data = new List<double?>()
            //};
            //Random rnd = new Random();
            //int i = 0;
            //foreach (string label in data.Labels)
            //{
            //    dataset.Data.Add(tweets.Where(l => l.Keyword == label).GroupBy(k => k.Count).Select(c => c.Key).Sum());
            //    //ChartColor.FromHexString(string.Format("#{0:X6}", rnd.Next(0x1000000))) < random colors
            //    dataset.BackgroundColor.Add(ChartColor.FromHexString(colors[i]));
            //    i++;
            //}
            //data.Datasets = new List<Dataset>
            //{
            //    dataset
            //};
            //chart.Data = data;
            //return Content(chart.CreateChartCode("lineChart"));

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
