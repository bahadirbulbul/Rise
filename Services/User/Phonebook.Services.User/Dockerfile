#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/User/Phonebook.Services.User/Phonebook.Services.User.csproj", "Services/User/Phonebook.Services.User/"]
COPY ["Shared/Phonebook.Shared/Phonebook.Shared.csproj", "Shared/Phonebook.Shared/"]
RUN dotnet restore "Services/User/Phonebook.Services.User/Phonebook.Services.User.csproj"
COPY . .
WORKDIR "/src/Services/User/Phonebook.Services.User"
RUN dotnet build "Phonebook.Services.User.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Phonebook.Services.User.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Phonebook.Services.User.dll"]