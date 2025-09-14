FROM mcr.microsoft.com/dotnet/sdk:6.0.403 AS build-env

COPY ./Core.Backend ./Core.Backend
COPY ./Core.CoreLib ./Core.CoreLib

WORKDIR /Core.Backend

RUN dotnet restore

# Build and publish a release

RUN dotnet publish -c Release -o out

# Build runtime image

FROM mcr.microsoft.com/dotnet/aspnet:6.0
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
WORKDIR /App
COPY --from=build-env /Core.Backend/out .
ENTRYPOINT ["dotnet", "Core.Backend.dll"]
