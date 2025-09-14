FROM mcr.microsoft.com/azure-functions/dotnet:4
ENV AzureWebJobsScriptRoot=/home/site/wwwroot
ENV AzureWebJobsStorage: UseDevelopmentStorage=true,
ENV FUNCTIONS_WORKER_RUNTIME: dotnet
COPY ./Core.DotnetFunctions/bin/Release/net6.0 /home/site/wwwroot