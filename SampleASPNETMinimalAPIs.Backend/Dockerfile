#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SampleASPNETMinimalAPIs.Web/SampleASPNETMinimalAPIs.Web.csproj", "SampleASPNETMinimalAPIs.Web/"]
RUN dotnet restore "SampleASPNETMinimalAPIs.Web/SampleASPNETMinimalAPIs.Web.csproj"
COPY . .
WORKDIR "/src/SampleASPNETMinimalAPIs.Web"
RUN dotnet build "SampleASPNETMinimalAPIs.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SampleASPNETMinimalAPIs.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SampleASPNETMinimalAPIs.Web.dll"]