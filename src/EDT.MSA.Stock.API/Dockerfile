FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
RUN sed -i 's/TLSv1.2/TLSv1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["EDT.MSA.API.Shared/EDT.MSA.API.Shared.csproj", "EDT.MSA.API.Shared/"]
COPY ["EDT.MSA.Stock.API/EDT.MSA.Stocking.API.csproj", "EDT.MSA.Stock.API/"]
RUN dotnet restore "EDT.MSA.Stock.API/EDT.MSA.Stocking.API.csproj"
COPY . .
WORKDIR "/src/EDT.MSA.Stock.API"
RUN dotnet build "EDT.MSA.Stocking.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EDT.MSA.Stocking.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EDT.MSA.Stocking.API.dll"]