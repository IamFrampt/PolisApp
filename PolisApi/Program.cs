using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/Stad", async (string cityNameOrNames) =>
{
    HttpClient client = new HttpClient();
    var res = await client.GetAsync($"http://polisen.se/api/events?locationName={cityNameOrNames}");
    var content = await res.Content.ReadAsStringAsync();

    var crimeList = JsonConvert.DeserializeObject<List<crimeDTO>>(content);

    return crimeList;
})
.WithName("GetCrimesFromCities")
.WithOpenApi();

app.MapGet("/Typ", async (string crimeTypeOrTypes) =>
{
    HttpClient client = new HttpClient();
    var res = await client.GetAsync($"http://polisen.se/api/events?type={crimeTypeOrTypes}");
    var content = await res.Content.ReadAsStringAsync();

    var crimeList = JsonConvert.DeserializeObject<List<crimeDTO>>(content);

    return crimeList;
})
.WithName("GetCrimesFromTypes")
.WithOpenApi();


app.Run();

public class crimeDTO
{
    public int Id { get; set; }
    public string Datetime { get; set; }
    public string Name { get; set; }
    public string Summary { get; set; }
    public string Url { get; set; }
    public string Type { get; set; }
    public Location Location { get; set; }
}

public class Location
{
    public string Name { get; set; }
    public string Gps { get; set; }
}