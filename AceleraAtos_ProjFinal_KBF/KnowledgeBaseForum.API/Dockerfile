#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["KnowledgeBaseForum.API/KnowledgeBaseForum.API.csproj", "KnowledgeBaseForum.API/"]
COPY ["KnowledgeBaseForum.Commons/KnowledgeBaseForum.Commons.csproj", "KnowledgeBaseForum.Commons/"]
COPY ["KnowledgeBaseForum.DataAccessLayer/KnowledgeBaseForum.DataAccessLayer.csproj", "KnowledgeBaseForum.DataAccessLayer/"]
RUN dotnet restore "KnowledgeBaseForum.API/KnowledgeBaseForum.API.csproj"
COPY . .
WORKDIR "/src/KnowledgeBaseForum.API"
RUN dotnet build "KnowledgeBaseForum.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KnowledgeBaseForum.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KnowledgeBaseForum.API.dll"]