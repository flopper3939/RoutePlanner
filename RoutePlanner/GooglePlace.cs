using Newtonsoft.Json.Linq;
using System;
using System.Text.Json;

public class GooglePlace
{
    private string _name;
    private string _address;
    private string _placeId;
    private (double Lat, double Lng)? _coordinates;
    private string _originalJson;

    // Constructor that initializes from raw JSON
    public GooglePlace(string rawJson)
    {
        _originalJson = rawJson;

        // Parse the JSON string using System.Text.Json
        using JsonDocument doc = JsonDocument.Parse(rawJson);
        JsonElement root = doc.RootElement;

        // Extract fields from JSON
        _name = root.GetProperty("name").GetString() ?? "";
        _address = root.GetProperty("formatted_address").GetString() ?? "";
        _placeId = root.GetProperty("place_id").GetString() ?? "";

        // Extract coordinates if available
        var location = root.GetProperty("geometry").GetProperty("location");
        _coordinates = (
            location.GetProperty("lat").GetDouble(),
            location.GetProperty("lng").GetDouble()
        );
    }
    public static List<GooglePlace> FromJsonList(string json)
    {

        List<GooglePlace> places = new List<GooglePlace>();
        JObject response = JObject.Parse(json);
        
        JArray results = (JArray)response["results"];

        foreach (var item in results)
        {
            string placeJson = item.ToString();
            GooglePlace place = new GooglePlace(placeJson);
            places.Add(place);
        }

        return places;
    }

    // Property for Name
    public string Name
    {
        get => _name;
        set => _name = value;
    }

    // Property for Address
    public string Address
    {
        get => _address;
        set => _address = value;
    }

    // Property for PlaceId
    public string PlaceId
    {
        get => _placeId;
        set => _placeId = value;
    }

    // Property for Coordinates (Lat, Lng)
    public (double Lat, double Lng)? Coordinates
    {
        get => _coordinates;
        set => _coordinates = value ;
    }

    // Property for Raw JSON
    public string OriginalJson
    {
        get => _originalJson;
        set => _originalJson = value;
    }

    // Override ToString to get a user-friendly string representation of the place
    public override string ToString()
    {
        return $"{Name} - {Address} (ID: {PlaceId})";
    }
}
