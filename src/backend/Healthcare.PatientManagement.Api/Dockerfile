FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "Healthcare.PatientManagement.Api.csproj"
RUN dotnet publish "Healthcare.PatientManagement.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:5062
EXPOSE 5062
ENTRYPOINT ["dotnet", "Healthcare.PatientManagement.Api.dll"]
