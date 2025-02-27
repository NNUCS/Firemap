using System.Diagnostics;
using BlazorBootstrap;
using System.Text.Json;
using System.Text;
using FiremapHelper;
using MudBlazor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Components;
using ChartOptions = BlazorBootstrap.ChartOptions;

namespace FuelModelComparer_BlazorFrontEnd.Components.Pages
{
    public partial class Home : Microsoft.AspNetCore.Components.ComponentBase
    {
        
        private string selectedComparer = "";
        private string selectedHumidity = "";
        private string selectedTreeCover = "";
        private string selectedFuelModels1 = "";
        private string selectedFuelModels2 = "";
        private string selectedFuelModels3 = "";
        private string selectedFuelModels4 = "";
        private string selectedFuelModels5 = "";
        private string selectedFuelModels6 = "";
        private string selectedFuelModels7 = "";
        private string selectedFuelModels8 = "";


        //private List<string> selectedFuelModels = new List<string>(new string[8]);
        public List<FiremapData>? DisplayData { get; set; } = new List<FiremapData>();


        private TaskCompletionSource _initializationTaskCompletionSource = new();
        private bool _isInitialized = false;
        public MudBlazor.ChartOptions ChtOptions { get; set; } = new MudBlazor.ChartOptions();

        
        public List<ChartSeries> Series { get; set; } = new List<ChartSeries>();
        // Set the X-axis labels
        public string[] XAxisLabels = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30" };
        private int chartIndex = -1; //default value cannot be 0 -> first selectedindex is 0.



        // drop down menu binding 
        private List<FiremapOption> options;
        private List<TreeCover> treeCovers;
        private List<Humidity> humidities; 
        private List<FuelModel> models;

        private bool ShouldRefresh { get; set; } = false;

        private async Task LoadingButton()
        {
            Debug.WriteLine("Hello, world!");

            // Load fresh data into DisplayData
            DisplayData = await GetFiremapDataFromWebService();

            // Toggle refresh flag
            ShouldRefresh = !ShouldRefresh;

            await InvokeAsync(StateHasChanged);

            // chech if can be null 
            if (DisplayData.Any())
            {
                InitializeChart();  // Initialize chart after data is loaded
            }


        }


        private void GenerateBoxes()
        {
            selectedComparer = options[1].Value.ToString();
            selectedHumidity = humidities[1].Value.ToString();
            selectedTreeCover = treeCovers[2].Value.ToString();
            selectedFuelModels1 = models[4].Value.ToString();
            selectedFuelModels2 = models[6].Value.ToString();
            selectedFuelModels3 = models[7].Value.ToString();
            selectedFuelModels4 = models[8].Value.ToString();
            selectedFuelModels5 = models[9].Value.ToString();
            selectedFuelModels6 = models[10].Value.ToString();
            selectedFuelModels7 = models[11].Value.ToString();
            selectedFuelModels8 = models[3].Value.ToString();
        }
        private void ModelCall(string CallSource)
        {
            Debug.WriteLine($"ModelCall: {CallSource}");

            Debug.WriteLine($"Selected comparer: {selectedComparer}");
            Debug.WriteLine($"Selected humdity: {selectedHumidity}");
            Debug.WriteLine($"Selected fuel models: {selectedFuelModels1}, " +
                $"{selectedFuelModels2}, " +
                $"{selectedFuelModels3}, " +
                $"{selectedFuelModels4}, " +
                $"{selectedFuelModels5}, " +
                $"{selectedFuelModels6}, " +
                $"{selectedFuelModels7}, " +
                $"{selectedFuelModels8}");
            //await GetFiremapDataFromWebService();

            //StateHasChanged();

        }

        protected override async Task OnInitializedAsync()
        {
            options = await GetOptions();
            treeCovers = await GetTreeCover();
            humidities = await GetHumidity();
            models = await GetModel();

            _isInitialized = true;
            _initializationTaskCompletionSource.SetResult();
            
            
        }

        private async Task LoadGraph()
        {
            Debug.WriteLine("Loading graph with updated dropdown values...");

            // Fetch data using current dropdown selections
            DisplayData = await GetFiremapDataFromWebService();

            // Ensure DisplayData is valid
            if (DisplayData == null || !DisplayData.Any())
            {
                Debug.WriteLine("No data available to display.");
                return;
            }

            // Update the chart
            InitializeChart();
        }


        private static async Task<string> PostContent(string url, StringContent content)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
        }

        private static async Task<List<FiremapOption>> GetOptions()
        {
            string url = "http://nnucsadmin-001-site2.atempurl.com/Services/FuelModelComparer/GetFiremapDataOptions";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string jsonStr = await response.Content.ReadAsStringAsync();
                var test = JsonSerializer.Deserialize<List<FiremapOption>>(jsonStr);
                return test;
            }
        }

       

        private static async Task<List<TreeCover>> GetTreeCover()
        {
            string url = "http://nnucsadmin-001-site2.atempurl.com/Services/FuelModelComparer/GetTreeCover";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string jsonStr = await response.Content.ReadAsStringAsync();
                var test = JsonSerializer.Deserialize<List<TreeCover>>(jsonStr);
                return test;
            }
        }

        private static async Task<List<Humidity>> GetHumidity()
        {
            string url = "http://nnucsadmin-001-site2.atempurl.com/Services/FuelModelComparer/GetHumidity";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string jsonStr = await response.Content.ReadAsStringAsync();
                var test = JsonSerializer.Deserialize<List<Humidity>>(jsonStr);
                return test;
            }
        } 
        
        private async Task<List<FuelModel>> GetModel()
        {
            if (models != null)
            {
                return models;
            }

            string url = "http://nnucsadmin-001-site2.atempurl.com/Services/FuelModelComparer/GetFuelModels";
            using (HttpClient client = new HttpClient())
            {

                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string jsonStr = await response.Content.ReadAsStringAsync();
                models = JsonSerializer.Deserialize<List<FuelModel>>(jsonStr);
                return models;

            }
        }

        private async Task<List<FiremapData>> GetFiremapDataFromWebService()
        {
            FiremapHelper.FiremapInputParameters fip = new()
            {
                Humidity = int.TryParse(selectedHumidity, out int humidityValue) ? humidityValue : 0,
                TreeCover = int.TryParse(selectedTreeCover, out int treeCoverValue) ? treeCoverValue : 0,
                //Model1 = string.IsNullOrEmpty(selectedFuelModels1) ? null : selectedFuelModels1,
                Model1 = selectedFuelModels1,
                Model2 = selectedFuelModels2,
                Model3 = selectedFuelModels3,
                Model4 = selectedFuelModels4,
                Model5 = selectedFuelModels5,
                Model6 = selectedFuelModels6,
                Model7 = selectedFuelModels7,
                Model8 = selectedFuelModels8
            };

            if (!fip.IsValid())
            {
                Debug.WriteLine("Invalid input parameters.");
                return new List<FiremapData>();
            }

            // Send data to the server
            string json = JsonSerializer.Serialize(fip);
            StringContent content = new(json, Encoding.UTF8, "application/json");

            using HttpClient client = new();
            HttpResponseMessage response = await client.PostAsync("http://nnucsadmin-001-site2.atempurl.com/Services/FuelModelComparer/GetFiremapDataParms", content);

            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine($"Error fetching data: {response.StatusCode}");
                return new List<FiremapData>();
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<FiremapData>>(jsonResponse) ?? new List<FiremapData>();
        }


        //private async Task<List<FiremapData>>? GetFiremapDataFromWebService()
        //{
        //    List<FiremapData> firemapData = null;

        //    if (!_isInitialized)
        //    {
        //        return null;
        //    }
        //    //this initialize the chart and grid and dropdowns if we have data 
        //    if (DisplayData == null || DisplayData.Count == 0)
        //    {

        //        // SmarterASP.NET
        //        //var jsonStr = await GetJsonFromWebServiceAsync("http://nnucsadmin-001-site2.atempurl.com/Services/FuelModelComparer/GetFiremapData");

        //        //create a FiremapData object/
        //        //FiremapHelper.FiremapInputParameters fip = new FiremapHelper.FiremapInputParameters();
        //        //fip.Humidity = 3;
        //        //fip.TreeCover = 4;
        //        //fip.Model1 = "12";
        //        //fip.Model2 = "4";
        //        //fip.Model3 = "5";
        //        //fip.Model4 = "6";
        //        //fip.Model5 = "7";
        //        //fip.Model6 = "8";
        //        //fip.Model7 = "9";
        //        //fip.Model8 = "10";

        //        FiremapHelper.FiremapInputParameters fip = new FiremapHelper.FiremapInputParameters();
        //        fip.Humidity = int.TryParse(selectedHumidity, out int humidityValue) ? humidityValue : 0; // Use selectedHumidity value
        //        fip.TreeCover = int.TryParse(selectedTreeCover, out int treeCoverValue) ? treeCoverValue : 0;
        //        fip.Model1 = selectedFuelModels1;
        //        fip.Model2 = selectedFuelModels2;
        //        fip.Model3 = selectedFuelModels3;
        //        fip.Model4 = selectedFuelModels4;
        //        fip.Model5 = selectedFuelModels5;
        //        fip.Model6 = selectedFuelModels6;
        //        fip.Model7 = selectedFuelModels7;
        //        fip.Model8 = selectedFuelModels8;

        //        //ModelCall("GetFiremapDataFromWebService");

        //        //fip.Model1 = selectedFuelModels[0];
        //        //fip.Model2 = selectedFuelModels[1];
        //        //fip.Model3 = selectedFuelModels[2];
        //        //fip.Model4 = selectedFuelModels[3];
        //        //fip.Model5 = selectedFuelModels[4];
        //        //fip.Model6 = selectedFuelModels[5];
        //        //fip.Model7 = selectedFuelModels[6];
        //        //fip.Model8 = selectedFuelModels[7];

        //        //need to validate the fip
        //        //Devin may need to validate on database side (throw error if data no good)

        //        if (!fip.IsValid())
        //        {
        //            Debug.WriteLine("Invalid input parameters.");
        //            return null;
        //        }

        //        // Serialize the object to JSON
        //        string json = JsonSerializer.Serialize(fip);
        //        Debug.WriteLine($"Request JSON: {json}");

        //        // Create the StringContent object for the HTTP request
        //        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

        //        string url = "http://nnucsadmin-001-site2.atempurl.com/Services/FuelModelComparer/GetFiremapDataParms";


        //        using (HttpClient client = new HttpClient())
        //        {
        //            HttpResponseMessage response = await client.PostAsync(url, content);
        //            Debug.WriteLine($"Response Status Code: {response.StatusCode}");

        //            if (!response.IsSuccessStatusCode)
        //            {
        //                string errorContent = await response.Content.ReadAsStringAsync();
        //                Debug.WriteLine($"Error Response: {errorContent}");
        //                return null;
        //            }

        //            string jsonResponse = await response.Content.ReadAsStringAsync();
        //            firemapData = JsonSerializer.Deserialize<List<FiremapData>>(jsonResponse) ?? new List<FiremapData>();
        //            //check for errors in DisplayData

        //            if (firemapData.Any(f => f.ErrorNumber.HasValue && f.ErrorNumber.Value == -1))
        //            {
        //                Debug.WriteLine("Display data error: " + firemapData[0].ErrorMessage);
        //                return firemapData;
        //            }



        //        }
        //    }

        //    return firemapData;
        //}

        //grid and graph 
        private async Task<GridDataProviderResult<FiremapData>> FiremapDataProvider(GridDataProviderRequest<FiremapData> request)
        {
            Debug.WriteLine("FiremapDataProvider called...");
            await WaitForInitializationAsync();
            await GetFiremapDataFromWebService();
            
            

            return await Task.FromResult(request.ApplyTo(DisplayData));
        }

        private Dictionary<string, string> GenerateModelColors()
        {
            var colors = new List<string>
            {
                //"red",
                //"green",
                //"blue",
                //"yellow",
                //"magenta",
                //"cyan",
                //"purple",
                //"orange"

                //hardcoded colors based on order that mudBlazor puts them?? There is a better way to do this but its not working
                "navy",
                "cyan", 
                "yellow",
                "orange",
                "red",
                "purple",
                "green",
                "blue"


            };

            var selectedModels = new List<string?>
            {
                selectedFuelModels1,
                selectedFuelModels2,
                selectedFuelModels3,
                selectedFuelModels4,
                selectedFuelModels5,
                selectedFuelModels6,
                selectedFuelModels7,
                selectedFuelModels8
            };

            var modelColors = new Dictionary<string, string>();

            for (int i = 0; i < selectedModels.Count; i++)
            {
                if (!string.IsNullOrEmpty(selectedModels[i]))
                {
                    modelColors[selectedModels[i]] = colors[i % colors.Count];
                }
            }

            return modelColors;
        }



        private void InitializeChart()
        {
            if (DisplayData == null || !DisplayData.Any())
            {
                Debug.WriteLine("No data available to initialize the chart.");
                return;
            }

            Series.Clear();

            // Generate model colors dynamically
            var modelColors = GenerateModelColors();
            
           
            // Selected fuel models
            var selectedModels = new List<string?>
            {
                selectedFuelModels1,
                selectedFuelModels2,
                selectedFuelModels3,
                selectedFuelModels4,
                selectedFuelModels5,
                selectedFuelModels6,
                selectedFuelModels7,
                selectedFuelModels8
            };

            foreach (var modelText in selectedModels)
            {
                if (!string.IsNullOrEmpty(modelText) && int.TryParse(modelText, out int modelId))
                {
                    // Retrieve the corresponding text value
                    var modelOption = models.FirstOrDefault(o => o.Value == modelId);
                    string modelName = modelOption != null ? modelOption.Text : $"Unknown Model ({modelText})";

                    // Filter data for this model based on selectedComparer
                    decimal[] filteredData = selectedComparer switch
                    {
                        var comparer when comparer == options[0].Value.ToString() => DisplayData
                            .Where(f => f.ROS.HasValue && f.ModelNumber == modelId)
                            .Select(f => f.ROS.Value)
                            .ToArray(),
                        var comparer when comparer == options[1].Value.ToString() => DisplayData
                            .Where(f => f.FL.HasValue && f.ModelNumber == modelId)
                            .Select(f => f.FL.Value)
                            .ToArray(),
                        var comparer when comparer == options[2].Value.ToString() => DisplayData
                            .Where(f => f.FLIN.HasValue && f.ModelNumber == modelId)
                            .Select(f => f.FLIN.Value)
                            .ToArray(),
                        var comparer when comparer == options[3].Value.ToString() => DisplayData
                            .Where(f => f.HPA.HasValue && f.ModelNumber == modelId)
                            .Select(f => f.HPA.Value)
                            .ToArray(),
                        var comparer when comparer == options[4].Value.ToString() => DisplayData
                            .Where(f => f.WAF.HasValue && f.ModelNumber == modelId)
                            .Select(f => f.WAF.Value)
                            .ToArray(),
                        _ => Array.Empty<decimal>() // Default case if no comparer matches
                    };

                    if (filteredData.Any())
                    {
                        // Get color for this model, default to black if not found
                        string modelColor = modelColors.TryGetValue(modelText, out var color) ? color : "#000000";

                        // Create a new chart series
                        ChartSeries chartSeries = new ChartSeries
                        {
                            Name = $"model: {modelName} (Color: {modelColor})", // Include color in the name
                            Data = filteredData.Select(f => (double)f).ToArray()


                        };

                        // Add the chart series
                        Series.Add(chartSeries);
                        Debug.WriteLine($"Added series for model: {modelName} with {filteredData.Length} data points.");
                    }
                    else
                    {
                        Debug.WriteLine($"No data found for model: {modelName}.");
                    }
                }
                else
                {
                    Debug.WriteLine($"Model is null or empty: {modelText}");
                }
            }


            // Refresh the UI
            StateHasChanged();
        }






        private IEnumerable<FiremapData> GetFiremapData()
        {
            return DisplayData;
        }

        private async Task WaitForInitializationAsync()
        {
            if (!_isInitialized)
            {
                // Await the TaskCompletionSource until initialization is complete
                await _initializationTaskCompletionSource.Task;
            }
            // Now you can access initialized data
        }
        private static async Task<string> GetJsonFromWebServiceAsync(string url, StringContent content)
        {
            using (HttpClient client = new HttpClient())
            {
                //HttpResponseMessage response = await client.GetAsync(url);
                // Send the POST request
                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
        }

    }
}
