# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything to the container
COPY . .

# Restore dependencies
RUN dotnet restore

# Publish the app in Release mode
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published files from the build stage
COPY --from=build /app/publish .

# Set the URL and expose port (Render uses port 10000)
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

# Run the app
ENTRYPOINT ["dotnet", "coursesellingsite.dll"]
