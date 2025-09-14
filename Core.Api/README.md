# Core.Api


## Basic Overview
Core.Api contains a series of Azure Functions, a serverless backend for Core.Builder that is hosted on Microsoft Azure.


### Environment Variables
The CLOUDCMS_BRANCHID, CLOUDCMS_BRANDID, CLOUDCMS_PROJECT_ID, and CLOUDCMS_REPOSITORYID are all set directly in the Azure Function App's Configuration.ApplicationSettings for both production and staging deployment slots.

### CloudCMS JavaScript Library

We use this library extensively to integrate with CloudCMS, and there is a lot of documentation:

-   https://www.cloudcms.com/javascript.html
-   https://www.cloudcms.com/nodejs.html
-   https://www.cloudcms.com/documentation/cookbooks/nodejs.html

One of the best places I have found clues about things you can do which are not mentioned in their docs is by looking at their library's tests:

-   https://github.com/gitana/gitana-javascript-driver/tree/master/tests