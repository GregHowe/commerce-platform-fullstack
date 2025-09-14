# configure the app settings
az functionapp config appsettings set -n $functionAppName -g $resourceGroup `
    --settings "ServicebusSettings:ServicebusCoreEventsConnectionstring=servicebussconne" 
    "ServicebusSettings:ServicebusCoreEventsUserIngestionTopicname=useringest" 
    "ServicebusSettings:ServicebusCoreEventsUserIngestionSubscription=CoreUserMgmt" 
    "DbSettings:ConnectionString=sqlconn"