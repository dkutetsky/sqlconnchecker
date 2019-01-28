FROM microsoft/dotnet:2.2-sdk AS build-env
WORKDIR /app

# Copy everything and build
COPY ./src ./
RUN dotnet restore 
RUN dotnet publish -c Release -r linux-x64 -o out

# Build runtime image
FROM microsoft/dotnet:2.2-runtime
WORKDIR /app
COPY --from=build-env /app/SqlConnChecker/out/ .
ENTRYPOINT ["dotnet", "SqlConnChecker.dll"]