#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WarrAttachmentManagementService.API/WarrAttachmentManagementService.API.csproj", "WarrAttachmentManagementService.API/"]
COPY ["WarrAttachmentManagementService.Domain/WarrAttachmentManagementService.Domain.csproj", "WarrAttachmentManagementService.Domain/"]
COPY ["WarrAttachmentManagementService.Application/WarrAttachmentManagementService.Application.csproj", "WarrAttachmentManagementService.Application/"]
COPY ["WarrAttachmentManagementService.Infrastructure/WarrAttachmentManagementService.Infrastructure.csproj", "WarrAttachmentManagementService.Infrastructure/"]
RUN dotnet restore "WarrAttachmentManagementService.API/WarrAttachmentManagementService.API.csproj"
COPY . .
WORKDIR "/src/WarrAttachmentManagementService.API"
RUN dotnet build "WarrAttachmentManagementService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WarrAttachmentManagementService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WarrAttachmentManagementService.API.dll"]