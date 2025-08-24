FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

COPY ["EasyEnglish.UI/EasyEnglish.UI.csproj", "EasyEnglish.UI/"]
COPY ["EasyEnglish.ProxyApiMethods/EasyEnglish.ProxyApiMethods.csproj", "EasyEnglish.ProxyApiMethods/"]
COPY ["EasyEnglish.Persistence/EasyEnglish.Persistence.csproj", "EasyEnglish.Persistence/"]
COPY ["EasyEnglish.Core/EasyEnglish.Core.csproj", "EasyEnglish.Core/"]
COPY ["EasyEnglish.Domain/EasyEnglish.Domain.csproj", "EasyEnglish.Domain/"]
COPY ["EasyEnglish.DTO/EasyEnglish.DTO.csproj", "EasyEnglish.DTO/"]
COPY ["EasyEnglish.Infrastructure/EasyEnglish.Infrastructure.csproj", "EasyEnglish.Infrastructure/"]
COPY ["EasyEnglish.Application/EasyEnglish.Application.csproj", "EasyEnglish.Application/"]
COPY ["EasyEnglish.Generators/EasyEnglish.Generators.csproj", "EasyEnglish.Generators/"]
COPY ["EasyEnglish/EasyEnglish.csproj", "EasyEnglish/"]

RUN dotnet restore "EasyEnglish.UI/EasyEnglish.UI.csproj"

COPY ["EasyEnglish.UI/", "EasyEnglish.UI/"]
COPY ["EasyEnglish.ProxyApiMethods/", "EasyEnglish.ProxyApiMethods/"]
COPY ["EasyEnglish.Persistence/", "EasyEnglish.Persistence/"]
COPY ["EasyEnglish.Core/", "EasyEnglish.Core/"]
COPY ["EasyEnglish.Domain/", "EasyEnglish.Domain/"]
COPY ["EasyEnglish.DTO/", "EasyEnglish.DTO/"]
COPY ["EasyEnglish.Infrastructure/", "EasyEnglish.Infrastructure/"]
COPY ["EasyEnglish.Application/", "EasyEnglish.Application/"]
COPY ["EasyEnglish.Generators/", "EasyEnglish.Generators/"]
COPY ["EasyEnglish/", "EasyEnglish/"]

FROM build AS publish
WORKDIR "/src/EasyEnglish.UI"
RUN dotnet publish "./EasyEnglish.UI.csproj" --no-restore -c Release -o /app/publish /p:UseAppHost=false

FROM nginx:alpine AS final

RUN rm -rf /usr/share/nginx/html/*

COPY --from=publish /app/publish/wwwroot /usr/share/nginx/html

COPY nginx.conf /etc/nginx/conf.d/default.conf

ARG API_BASE_URL
RUN echo "{ \"ApiBaseUrl\": \"${API_BASE_URL}\" }" > /usr/share/nginx/html/appsettings.json

EXPOSE 80