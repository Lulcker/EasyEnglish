FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ["EasyEnglish/EasyEnglish.csproj", "EasyEnglish/"]
COPY ["EasyEnglish.Application/EasyEnglish.Application.csproj", "EasyEnglish.Application/"]
COPY ["EasyEnglish.DTO/EasyEnglish.DTO.csproj", "EasyEnglish.DTO/"]
COPY ["EasyEnglish.Persistence/EasyEnglish.Persistence.csproj", "EasyEnglish.Persistence/"]
COPY ["EasyEnglish.Core/EasyEnglish.Core.csproj", "EasyEnglish.Core/"]
COPY ["EasyEnglish.Domain/EasyEnglish.Domain.csproj", "EasyEnglish.Domain/"]

RUN dotnet restore "EasyEnglish/EasyEnglish.csproj"

COPY ["EasyEnglish/", "EasyEnglish/"]
COPY ["EasyEnglish.Application/", "EasyEnglish.Application/"]
COPY ["EasyEnglish.DTO/", "EasyEnglish.DTO/"]
COPY ["EasyEnglish.Persistence/", "EasyEnglish.Persistence/"]
COPY ["EasyEnglish.Core/", "EasyEnglish.Core/"]
COPY ["EasyEnglish.Domain/", "EasyEnglish.Domain/"]

WORKDIR "/src/EasyEnglish"
RUN dotnet build "./EasyEnglish.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EasyEnglish.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EasyEnglish.dll"]
