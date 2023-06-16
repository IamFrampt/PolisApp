using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/Stad", async (string name) =>
{
    HttpClient client = new HttpClient();
    var Staddusokt = await client.GetAsync($"http://polisen.se/api/events{name}");
    var res = await Staddusokt.Content.ReadAsStringAsync();

    var jabba = JsonConvert.DeserializeObject<List<SearchedStad>>(res);

    return jabba;
})
.WithName("GetStad")
.WithOpenApi();


app.Run();

public class SearchedStad
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