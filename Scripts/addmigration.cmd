@echo off
pushd %0\..\..
dotnet restore
dotnet ef migrations add %1 -s .\Mimir.API\Mimir.API.csproj -p .\Mimir.Database\Mimir.Database.csproj -c MimirDbContext
popd
set /p DUMMY=Hit ENTER to continue...