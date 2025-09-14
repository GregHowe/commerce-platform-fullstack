# Core.Generator


## Basic Overview
Core.Generator is a pipeline tool for generating static sites that were authored in Core.Builder, and whose
structure is hosted in CloudCMS. It is a Nuxt application configured for SSG, that on build of a Core.Builder authored
site, parses its block structure and generates static files that can be hosted on any web server.

## Architectural Context
{{ WIP }}

## Usage
### Prerequisites
Before generating a site, you must configure the environment variables. This will require the ***NYL/Core.Generator/.env***
and ***NYL/Core.Generator/gitana.json*** files. To receive these files, please reach out to a member of the Core 2.0
team.

### Setup
```bash
# to generate a site
yarn dev # the "dev" command will clear node_modules and reinstall, to prevent caching issues with auto-imported components from external directories.
```
## Notes

### Imports
As a member of the Core monorepo, Core.Generator has access to all components declared within Core.Builder, which can be
imported using the following syntax:

```javascript
import x from "@builder/subdirectory";
```
