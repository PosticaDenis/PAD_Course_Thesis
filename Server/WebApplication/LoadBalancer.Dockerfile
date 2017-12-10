FROM microsoft/aspnetcore-build:2.0 AS build-env
WORKDIR /app


COPY . ./
RUN dotnet clean
RUN dotnet restore LoadBalancer
RUN dotnet publish -c Release -o out LoadBalancer

# Build runtime image
FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=build-env /app/LoadBalancer/out .
ENTRYPOINT ["dotnet", "LoadBalancer.dll"]