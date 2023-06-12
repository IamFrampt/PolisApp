#base image
from mcr.microsoft.com/dotnet/aspnet:7.0 as base
workdir /app
expose 80
expose 443

#build image
from mcr.microsoft.com/dotnet/sdk:7.0 as build
workdir /src
copy ["PolisApi.csproj","."]
run dotnet restore "PolisApi.csproj"
copy . .

#publish image
from build as publish
run dotnet publish "PolisApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

#final image
from base as final
workdir /app
copy --from=publish /app/publish .
entrypoint ["dotnet","PolisApi.dll"]
