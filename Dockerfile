# 1. build phase
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy csproj and restore
COPY ["dotnet-webapi.csproj", "./"]
RUN dotnet restore "dotnet-webapi.csproj"

# copy the rest of the files and build
COPY . ./
RUN dotnet publish "dotnet-webapi.csproj" -c Release -o /app/publish

# 2. runtime phase
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# force the API to listen on port 8080
ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080
ENTRYPOINT ["dotnet", "dotnet-webapi.dll"]