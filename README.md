# Core 2.0

## Brief Overview

Core 2.0 is a platform for agents and franchisees to author and deploy static sites that meet their umbrella organization's compliance and brand requirements. It allows a home office to supply franchises with approved content they can use to build marketing websites.

## Running Locally

You will need to install .NET 6: https://dotnet.microsoft.com/en-us/download/dotnet/6.0
You will need node version 16.18.1 https://nodejs.org/en/blog/release/v16.18.1/

Restart your terminal (or potentially computer) to get terminal commands.

- Copy Core.Builder/.env.example to Core.Builder/.env
- Get Core.Backend/appsettings.json file from a manager.

## Frontend Devs

If you are only doing frontend work and don't wish to set up the .NET api, you can set your API_BASE_URL to https://nyl-integration.fusion92core.com/api.

### Run all

Run `bash startmanual.sh` to startup the dotnet API and the Nuxt sitebuilder.

### Docker (Optional)

For a smooth developer experience, you will need [Docker Desktop](https://www.docker.com/products/docker-desktop/) and the [Azure CLI](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli).

Once you have these dependencies installed, run `bash startdocker.sh` from the root of this project. You'll be logged in to Azure and the Backend + Builder will spin up in a container group. Frontend is accessible via localhost:80, and is configured to respect local file changes and hot reload inside the container.

## Test Accounts
t.mueller@fusion92.com
88a34425-7289-4227-ad3f-e363f7248ddd

## Environments

### Development
https://nyl-development.fusion92core.com/

### NYL Integration (UAT)
https://nyl-integration.fusion92core.com/

## Deployments
/.github/test.yml contains all tests that run upon PR. These tests must pass before a PR is allowed to be merged into main.

Each piece of the project (Root level folders, except the libraries) should each have their own yaml file which describes how each piece is deployed and what triggers it. Currently, pushes into main will cause changes to autodeploy to the dev environment. Releasing to UAT / Production are manual steps.

## Components

Core 2.0 comprises four separate applications. Communication between the backend apps is coordinated by events sent to an [Azure Service Bus](https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-messaging-overview). A REST API is made available in Core.Backend for the Core.Builder frontend to consume.

### Core.Backend

Not to be confused with `Core.Api` but [Core.Backend](Core.Backend/README.md) is the dotnet api which consists of all kinds of various controllers and models that let you interact with the sql database that it wraps. Now the number one api used by Core.Builder and Core.Generator applications.

This application is responsible for:

- Saving and loading Brands, Sites, Sections, Themes, and Presets for each of these.
- Managing scoped access for all available functions. (Brand level, site level, global, etc.)
- Managing users in Azure Active Directory

To start the dotnet api run `cd ./Core.Backend && dotnet clean && dotnet restore && dotnet run` _(someone please feel free to correct this)_
Please note: you need to have dotnet sdk 6.0.403 installed for this to run correctly.

If you have problems w/ your https certificate, try running `dotnet dev-certs https --trust` to force your machine to trust the self signed certificate used to run the backend. If you have problems around connecting to the database, contact a team lead or devops to get your IP address added to the firewall rules.

### Core.CoreLib

Just a bunch of stuff used by `Core.Backend` which will probably need to be shared with other dotnet applications.

### Core.Builder

[Core.Builder](Core.Builder/README.md) is the user-facing application of the Core system, and allows clients to
author compliant sites, with automatic deployment.

### Core.DotnetFunctions

[Core.DotnetFunctions](Core.DotnetFunctions/README.md) contains a functions app that runs on dotnet. This is the foundation of our multi-tenant user ingestion application, and focal point of interactions with the systems belonging to external partners and clients.

This application is responsible for:

- Negotiating between external parties and our internal system.
- Ingesting user feeds, and providing instructions for Core to manifest these changes in our system, according to client-specific business rules.

### Core.E2E

Seems like [Core.E2E](Core.E2E/README.md) is where our End to End testing goes. Currently, this is being used as a smoke test after a push to staging and before promotion to prod. This should be fleshed out to provide E2E test coverage.

### Core.Generator

[Core.Generator](Core.Generator/README.md) is a factory for static sites that were authored in Core.Builder. It is a Nuxt application configured for SSG that, upon a site publish event from Core.Builder, parses its block structure and generates static files that can be hosted on any web server.

### Core.Library

[Core.Library](Core.Library/README.md) is a library for sharing components, assets, and utilities between
Core.Builder and Core.Generator.

## Environments and CI/CD

### Builder

Production: sitebuilder.fusion92.net
Staging: sitebuilder-staging.fusion92.net
Dev: sitebuilder-dev.fusion92.net

When builder changes are merged to the main branch, the app is autobuilt to https://sitebuilder-staging.fusion92.net. Changes to the development branch build on https://sitebuilder-dev.fusion92.net/. The production endpoint will be [swapped](https://learn.microsoft.com/en-us/azure/app-service/deploy-staging-slots#what-happens-during-a-swap) with staging when we want things to go live.

## Documentation

Documentation for this monorepo can be found in [SharePoint](https://fusion92.sharepoint.com/sites/newyorklife/shared%20documents/forms/allitems.aspx?skipSignal=true&id=%2Fsites%2FNewYorkLife%2FShared%20Documents%2FTechnology%20and%20Development%2FDevelopment%20and%20Code&viewid=ac358bca%2D4d34%2D4415%2D8994%2D66619cc899e1)

## Preferred VSCode Extensions

| Extension Name  | Extension ID                        |
| --------------- | ----------------------------------- |
| Azure Functions | ms-azuretools.vscode-azurefunctions |
| ESLint          | dbaeumer.vscode-eslint              |
| Prettier        | esbenp.prettier-vscode              |

## Retired Apps

### Core.Api

[Core.Api](Core.Api/README.md) consists of Azure functions that provide functionality for the Core.Builder and Core.Generator applications.
_This has mostly been replaced by `Core.Backend`, but there are still some functions within that we have yet to replicate._

To start api run `cd ./Core.Api && yarn && func host start`
