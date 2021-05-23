color B

del  .PublishFiles\*.*   /s /q

dotnet restore

dotnet build

cd lgs.web.api.Api

dotnet publish -o ..\lgs.web.api.Api\bin\Debug\netcoreapp3.1\

md ..\.PublishFiles

xcopy ..\lgs.web.api.Api\bin\Debug\netcoreapp3.1\*.* ..\.PublishFiles\ /s /e 

echo "Successfully!!!! ^ please see the file .PublishFiles"

cmd