#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ESourcing.UI/ESourcing.UI.csproj", "ESourcing.UI/"]
COPY ["ESourcing.Infrastructure/ESourcing.Infrastructure.csproj", "ESourcing.Infrastructure/"]
COPY ["ESourcing.Core/ESourcing.Core.csproj", "ESourcing.Core/"]
RUN dotnet restore "ESourcing.UI/ESourcing.UI.csproj"
COPY . .
WORKDIR "/src/ESourcing.UI"
RUN dotnet build "ESourcing.UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ESourcing.UI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ESourcing.UI.dll"]
