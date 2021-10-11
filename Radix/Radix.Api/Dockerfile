#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
RUN apt-get update -yq \
    && apt-get install curl gnupg -yq \
    && curl -sL https://deb.nodesource.com/setup_10.x | bash \
    && apt-get install nodejs -yq
WORKDIR /src
COPY ["Radix.Api/Radix.Api.csproj", "Radix.Api/"]
RUN dotnet restore "Radix.Api/Radix.Api.csproj"
COPY . .
WORKDIR "/src/Radix.Api"
RUN dotnet build "Radix.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Radix.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Radix.Api.dll"]