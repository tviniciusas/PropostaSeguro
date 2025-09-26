FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/ContratacaoService/Presentation/ContratacaoService.Presentation.csproj", "src/ContratacaoService/Presentation/"]
COPY ["src/ContratacaoService/Infrastructure/ContratacaoService.Infrastructure.csproj", "src/ContratacaoService/Infrastructure/"]
COPY ["src/ContratacaoService/Application/ContratacaoService.Application.csproj", "src/ContratacaoService/Application/"]
COPY ["src/ContratacaoService/Domain/ContratacaoService.Domain.csproj", "src/ContratacaoService/Domain/"]
RUN dotnet restore "src/ContratacaoService/Presentation/ContratacaoService.Presentation.csproj"
COPY . .
WORKDIR "/src/src/ContratacaoService/Presentation"
RUN dotnet build "ContratacaoService.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ContratacaoService.Presentation.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ContratacaoService.Presentation.dll"]