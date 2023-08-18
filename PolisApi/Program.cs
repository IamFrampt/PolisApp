using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


    app.UseSwagger();
    app.UseSwaggerUI();

app.MapGet("/Stad", async (string searchWord) =>
{
    HttpClient client = new HttpClient();
    var res = await client.GetAsync($"http://polisen.se/api/events?locationName={searchWord}");
    var content = await res.Content.ReadAsStringAsync();

    var crimeList = JsonConvert.DeserializeObject<List<crimeDTO>>(content);

    return crimeList;
})
.WithName("GetCrimesFromCities")
.WithOpenApi();

app.MapGet("/Typ", async (string searchWord) =>
{
    HttpClient client = new HttpClient();
    var res = await client.GetAsync($"http://polisen.se/api/events?type={searchWord}");
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