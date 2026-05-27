using Newtonsoft.Json;
using PolisApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();


app.MapGet("/Stad", async (string searchWord) =>
{
    HttpClient client = new HttpClient();
    var res = await client.GetAsync($"https://polisen.se/api/events?locationName={searchWord}");
    var content = await res.Content.ReadAsStringAsync();

    var crimeList = JsonConvert.DeserializeObject<List<crimeDTO>>(content);

    return crimeList;
})
.WithName("GetCrimesFromCities");

app.MapGet("/Typ", async (string searchWord) =>
{
    HttpClient client = new HttpClient();
    var res = await client.GetAsync($"https://polisen.se/api/events?type={searchWord}");
    var content = await res.Content.ReadAsStringAsync();

    var crimeList = JsonConvert.DeserializeObject<List<crimeDTO>>(content);

    return crimeList;
})
.WithName("GetCrimesFromTypes");


app.Run();