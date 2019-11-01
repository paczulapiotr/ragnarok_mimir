@echo off
pushd %0\..\..
dotnet restore
dotnet ef database update %1 -s .\Mimir.API\Mimir.API.csproj -p .\Mimir.Database\Mimir.Database.csproj -c MimirDbContext
popd
set /p DUMMY=Hit ENTER to continue...