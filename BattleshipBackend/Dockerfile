# Use the official .NET SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set the working directory
WORKDIR /app

# Copy the project files
COPY *.csproj ./

# Restore dependencies
RUN dotnet restore

# Copy the rest of the application code
COPY . .

# Build the application
RUN dotnet publish -c Release -o out

# Use the official .NET runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:9.0

# Set the working directory
WORKDIR /app

# Copy the built application from the build stage
COPY --from=build /app/out .

# Expose port 80
EXPOSE 80

# Start the application
ENTRYPOINT ["dotnet", "BattleshipBackend.dll"]