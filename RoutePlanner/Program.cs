using System;
using Microsoft.Office.Interop.Excel;

class RoutePlanner
{
    private static String googleAPIKey = "";
    private static HttpClient _httpClient;
    static void Main(string[] args)
    {

       
        


        _httpClient = new HttpClient();
        List<string> destinations = new List<string>();
        Console.WriteLine("Enter destinations (type 'done' to finish):");

        while (true)
        {
            Console.Write("Destination: ");
            String? input = Console.ReadLine();
            if (input == null)
            {
                Console.WriteLine("Null string entered. Quitting.");
                Console.ReadKey();
                return;
            }
            if (input.ToLower() == "done") break;

            List<GooglePlace> suggestions =  GetGoogleMapsSuggestions(input);
            if (suggestions.Count == 0)
            {
                Console.WriteLine("No matches found. Try again.");
                continue;
            }

            Console.WriteLine("Select a destination:");
            for (int i = 0; i < suggestions.Count; i++)
                Console.WriteLine($"{i + 1}. {suggestions[i]}");
            Console.WriteLine("4. Skip");

            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 4)
                Console.Write("Invalid choice. Select again: ");

            if (choice == 4) continue;
            // destinations.Add(suggestions[choice - 1]);
        }

        if (destinations.Count < 2)
        {
            Console.WriteLine("At least two destinations are required.");
            return;
        }

        List<Route> shortestRoute = CalculateShortestRoute(destinations);
        //GenerateExcelReport(shortestRoute);
    }

    static List<GooglePlace> GetGoogleMapsSuggestions(string query)
    {
        HttpClient client = new HttpClient();
        String url = "https://maps.googleapis.com/maps/api/place/textsearch/json?query=" + Uri.EscapeDataString(query) + "&key=" + googleAPIKey;
        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        HttpResponseMessage response = client.Send(requestMessage);
        response.EnsureSuccessStatusCode();
        String json = response.Content.ReadAsStringAsync().Result;
        return GooglePlace.FromJsonList(json);
    }

    static List<Route> CalculateShortestRoute(List<string> destinations)
    {
        // Placeholder: Implement shortest route calculation using Google Maps API
        return new List<Route>();
    }

    static void GenerateExcelReport(List<Route> route)
    {
        Application excelApp = new Application();
        Workbook workbook = excelApp.Workbooks.Add();
        Worksheet worksheet = (Worksheet)workbook.Sheets[1];

        worksheet.Cells[1, 1] = "Destination";
        worksheet.Cells[1, 2] = "Distance (km)";
        worksheet.Cells[1, 3] = "Fuel Cost";
        worksheet.Cells[1, 4] = "Driving Hours";

        // Placeholder: Fill in route details

        workbook.SaveAs("RoutePlan.xlsx");
        workbook.Close();
        excelApp.Quit();
    }
}

class Route
{
    public string From { get; set; }
    public string To { get; set; }
    public double Distance { get; set; }
}

