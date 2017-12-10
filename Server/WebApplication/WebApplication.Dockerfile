FROM microsoft/aspnetcore-build:2.0 AS build-env
WORKDIR /app


COPY . ./
RUN dotnet clean
RUN dotnet restore WebApplication
RUN dotnet publish -c Release -o out WebApplication

# Build runtime image
FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=build-env /app/WebApplication/out .
ENTRYPOINT ["dotnet","WebApplication.dll"]