# Use the official .NET 9.0 SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the .csproj file and restore dependencies
COPY ["OnlinePollingSystem/Ops.Api/Ops.Api.csproj", "."]
RUN dotnet restore "Ops.Api.csproj"

# Copy the rest of the application code
COPY OnlinePollingSystem/Ops.Api/ .

# Build the application
RUN dotnet build "Ops.Api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "Ops.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the official .NET 9.0 runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8090

# Copy the published output from the publish stage
COPY --from=publish /app/publish .

# Set environment variables (optional, adjust as needed)
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8090

# Run the application
ENTRYPOINT ["dotnet", "Ops.Api.dll"]