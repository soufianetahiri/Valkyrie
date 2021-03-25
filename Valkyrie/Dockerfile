#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /Valkyrie

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /Valkyrie
COPY ["*Valkyrie.csproj", "Valkyrie/"]
RUN dotnet restore "Valkyrie/Valkyrie.csproj"
COPY . ./Valkyrie/
WORKDIR "/Valkyrie/Valkyrie"
RUN dotnet build "Valkyrie.csproj" -c Release -o /Valkyrie/build

FROM build AS publish
RUN dotnet publish "Valkyrie.csproj" -c Release -o /Valkyrie/publish

FROM base AS final
WORKDIR /Valkyrie
COPY --from=publish /Valkyrie/publish .
ENTRYPOINT ["dotnet", "Valkyrie.dll"]