git pull;
rm -rf .PublishFiles;
dotnet build;
dotnet publish -o /home/lgs.web.api/lgs.web.api.Api/bin/Debug/netcoreapp3.1;
cp -r /home/lgs.web.api/lgs.web.api.Api/bin/Debug/netcoreapp3.1 .PublishFiles;
echo "Successfully!!!! ^ please see the file .PublishFiles";