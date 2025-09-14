echo -e "\n\n"
echo -e "
\e[37m   #####     #####       #####   #######  ######   #######
 ##     ## ##     ##    #     #  #     #  #     #  #        2.0
#         #         #   #        #     #  ######   #####  
 ##     ## ##     ##    #     #  #     #  #    #   #      
   #####     #####       #####   #######  #     #  #######\e[0m

\e[34m===================================================================\e[0m
           \e[37m*** Welcome to Core 2.0 Local Development ***\e[0m           
\e[34m===================================================================\e[0m
"



if [[ $(uname ) == "MINGW32_NT" ]] || [[ $(uname ) == "MINGW64_NT" ]]
then
	echo "Windows dev environment"
	winget install -e --id Microsoft.AzureCLI
    npm i -g azure-functions-core-tools@4 --unsafe-perm true
else
	echo "Mac dev environment"
	brew update && brew install azure-cli
	brew tap azure/functions
	brew install azure-functions-core-tools@4
fi
az login
docker-compose -p "f92core" up