# PolisApp

Steg-för-steg guide för Docker:

1. Bas Image:

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
- Här väljs vilken bas bild för min container och jag väljer aspnet:7.0, den väljs för att jag vill skapa en så liten image som möjligt och behöver bara runtime och dependencies för att köra applikationen.

WORKDIR /app
- Skapar/sätter katalogen vi arbetar i till /app

EXPOSE 80 and EXPOSE 443
- Exponerar portarna 80 samt 443 för HTTP trafik.

2. Skapa Image:

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
- Väljer min image som ska användas för att bygga min image. Använder sdk här istället för aspnet för att jag behöver fler verktyg den här gången för att kunna skapa, testa och debugga applikationen.

WORKDIR /src
- Skapar/sätter katalogen vi arbetar i till /src

COPY ["PolisApi.csproj", "PolisApi/"]
- Kopierar bara .csproj filen  till  PolisApi/ katalogen i containern för att jag vill se att den går att köra innan jag kopierar över allt innehåll.

RUN dotnet restore "PolisApi/PolisApi.csproj"
- Kör/Installerar projektets beroenden

WORKDIR "/src/PolisApi"
- Ändrar katalogen till /src/PolisApi

COPY . .
- Kopierar resterande innehåll till den nuvarande katalogen

RUN dotnet build "PolisApi.csproj" -c Release -o /app/build
- Bygger projektet med Release och lägger det i /app/build


3. Publicering:

FROM build AS publish
- Väljer bas imagen till den vi skapade från föregående steg.

RUN dotnet publish "PolisApi.csproj" -c Release -o /app/publish /p:UseAppHost=false
- Publicerar projektet och lägger det i /app/publish och vi väljer att inte ta med "app host" för att skapa en mindre image. Vi isolerar även vårt publish från build istället för att köra kommandot efter "RUN dotnet build "PolisApi.csproj" -c Release -o /app/build" för att då får vi en mindre image som bara innehåller det vi behöver.


4. Starta Container:

FROM base AS final
- Sätter bas imagen till den vi skapade i första steget för att nu behöver vi bara runtime och dependencies igen.

WORKDIR /app
- Sätter katalogen till /app

COPY --from=publish /app/publish .
- Kopierar allt from imagen vi skapade i publish steget och lägger det i nuvarande katalogen

ENTRYPOINT ["dotnet", "PolisApi.dll"]
- Specificerar entry point kommandot för att köra projektet när containern startar. Den startar min applikationen genom PolisApi.dll filen.

**PolisApp's Dockerfile ser likadan ut som denna så skriver ingen steg-för-steg om den.**


----------------------------------------------------------------------------

Steg-för-steg guide för Docker:

1. Väljer format:
version: '3.4'
- Väljer vilken version Docker Compose filens format ska vara i.

2. Skapar första servicen (frontend, app)

Service: polisapp
- Namnger servicen till polisapp

image: frampt/polisapp
- Väljer vilken image som ska användas för servicen, i detta fall är det frampt/polisapp

depends_on: - "polisapi": This indicates that the polisapp service depends on the polisapi service. It means that Docker will start the polisapi service before starting the polisapp service.
- Indikerar att denna service är beroende av en service vid namn polisapi som vi kommer skapa efter denna. Detta görs för att då startar polisapi servicen upp innan denna service. Om backend inte är igång före frontend så skulle vi få massor fel när vi försöker söka efter brott.

ports: - "8080:80"
- Väljer vilken/vilka portar som servicen ska mappas till för att kunna besöka den i webbläsaren.

build
- För att bygga en container till polisapp servicen.

context: .
- Sätter att vi kommer bygga containern i den nuvarande katalogen (där docker compose filen finns).

dockerfile: PolisApp/Dockerfile
- Specificerar vart Dockerfilen ligger för att kunna bygga servicen som ska vara i containern.
networks:  - apiconnection
- Sätter att servicen ska vara i nätverket apiconnection


2. Skapar andra servicen (backend, api)
Service: polisapi

image: frampt/polisapi
- Väljer vilken image som ska användas för servicen, i detta fall är det frampt/polisapp

ports: - "8081:80"
- Väljer vilken/vilka portar som servicen ska mappas till för att kunna besöka den i webbläsaren. 

build
- För att bygga en container till polisapp servicen.

context: .
- Sätter att vi kommer bygga containern i den nuvarande katalogen (där docker compose filen finns).

dockerfile: PolisApi/Dockerfile
- Specificerar vart Dockerfilen ligger för att kunna bygga servicen som ska vara i containern.

networks:  - apiconnection
- Sätter att servicen ska vara i nätverket apiconnection

networks:
    apiconnection:
       driver: bridge
- Skapar nätverket apiconnection
