# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
USER 0

# Copy csproj first
COPY TTDesign.Api/TTDesign.API.csproj TTDesign.Api/
RUN dotnet restore TTDesign.Api/TTDesign.API.csproj

# Copy toàn bộ source đúng thư mục
COPY TTDesign.Api/ TTDesign.Api/

# Publish
RUN dotnet publish TTDesign.Api/TTDesign.API.csproj -c Release -o /app/out --no-restore


# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .

ARG ASPNETCORE_ENVIRONMENT
ENV ASPNETCORE_ENVIRONMENT=$ASPNETCORE_ENVIRONMENT

RUN echo $ASPNETCORE_ENVIRONMENT
RUN ls

RUN mkdir -p Upload/Images/
RUN mkdir -p Upload/Documents/
RUN chmod -R 755 /app/Upload

ENTRYPOINT ["dotnet", "TTDesign.API.dll"]
