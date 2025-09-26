FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/PropostaService/Presentation/PropostaService.Presentation.csproj", "src/PropostaService/Presentation/"]
COPY ["src/PropostaService/Infrastructure/PropostaService.Infrastructure.csproj", "src/PropostaService/Infrastructure/"]
COPY ["src/PropostaService/Application/PropostaService.Application.csproj", "src/PropostaService/Application/"]
COPY ["src/PropostaService/Domain/PropostaService.Domain.csproj", "src/PropostaService/Domain/"]
RUN dotnet restore "src/PropostaService/Presentation/PropostaService.Presentation.csproj"
COPY . .
WORKDIR "/src/src/PropostaService/Presentation"
RUN dotnet build "PropostaService.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PropostaService.Presentation.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PropostaService.Presentation.dll"]