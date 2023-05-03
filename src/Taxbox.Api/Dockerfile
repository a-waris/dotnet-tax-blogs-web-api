##See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
#
FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Install cultures (same approach as Alpine SDK image)
RUN apk add --no-cache icu-libs

# Disable the invariant mode (set in base image)
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /src
COPY ["./Directory.Packages.props", "./"]
COPY ["src/Taxbox.Api/Taxbox.Api.csproj", "src/Taxbox.Api/"]
COPY ["src/Taxbox.Application/Taxbox.Application.csproj", "src/Taxbox.Application/"]
COPY ["src/Taxbox.Domain/Taxbox.Domain.csproj", "src/Taxbox.Domain/"]
COPY ["src/Taxbox.Infrastructure/Taxbox.Infrastructure.csproj", "src/Taxbox.Infrastructure/"]
COPY . .
WORKDIR "/src/src/Taxbox.Api"

FROM build AS publish
RUN dotnet publish "Taxbox.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Taxbox.Api.dll"]