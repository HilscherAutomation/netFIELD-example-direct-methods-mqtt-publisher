FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY NetfieldDeviceSample.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY *.json ./
COPY *.cs ./
COPY ./Models ./Models
COPY ./Classes ./Classes


RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/runtime:3.1
WORKDIR /app
COPY *.json ./
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "NetfieldDeviceSample.dll"]
