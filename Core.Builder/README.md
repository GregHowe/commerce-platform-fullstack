# Core.Builder

## Basic Overview

Core.Builder is the user-facing application of the Core system, and allows clients to
author compliant sites, with automatic deployment.

## Setup

If you like, set ENV variables DEV_USERNAME and DEV_PW to autofill your username/password at login when running locally.

### Prerequisites

-   [Core.Backend]() must be running at localhost:7283
-   You must be set up with an AzureAD account in order to login (if your work email does not work w/ SSO, please see a team lead to get you going).

```bash
# install dependencies
$ yarn install

# serve with hot reload at localhost:3000
$ yarn dev
```

Test user account:
user: j.mcjohnerson@fusion92.com
pass: Duwo7355

## Architectural Context

{{ WIP }}

## Notes

### Caveats ⚠️

-   **_Core_** Components represent the actual components that will be generated within Core.Generator from site schemas.
-   All import paths within components must be absolute paths, as when Core.Generator resolves components from Core.Builder, "~" will refer to NYL/Core.Generator rather than NYL/Core.Builder due to its different webpack configuration for module resolution.
-   External dependencies are **NOT ALLOWED** within **_Core_** components. They must consist of pure HTML & CSS/SCSS.
