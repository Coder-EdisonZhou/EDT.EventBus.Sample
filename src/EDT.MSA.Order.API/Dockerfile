FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
RUN sed -i 's/TLSv1.2/TLSv1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["EDT.MSA.API.Shared/EDT.MSA.API.Shared.csproj", "EDT.MSA.API.Shared/"]
COPY ["EDT.MSA.Order.API/EDT.MSA.Ordering.API.csproj", "EDT.MSA.Order.API/"]
RUN dotnet restore "EDT.MSA.Order.API/EDT.MSA.Ordering.API.csproj"
COPY . .
WORKDIR "/src/EDT.MSA.Order.API"
RUN dotnet build "EDT.MSA.Ordering.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EDT.MSA.Ordering.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EDT.MSA.Ordering.API.dll"]