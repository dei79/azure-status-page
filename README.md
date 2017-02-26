# Azure Status Page Generator
[![Build status](https://ci.appveyor.com/api/projects/status/nd602nm4y669oijg?svg=true)](https://ci.appveyor.com/project/dei79/azure-status-page)

If you think about Status Page there are several requirements this kind of WebSites must meet, e.g.

* It must run on a separate infrastructure, e.g. a different geographical region or in a different cloud infrastructure
* It must support client side meters which will be pushed to the status page directly out of the application 
* It must support server side meters which will be executed from the status page on a recurrent schedule
* It must commmunicate service outages to the customers 
* It must support push communication to the operations staff with all the details to bring the service back on track

There are several SaaS services offering a solution, e.g. https://statuspage.io. This project is intended to offer a similar service hosted in Azure App Services which allows everybody to create status pages in Azure within minutes.

## Solutions Overview

TODO: Add an architecture picture here

## How to setup

1. Visit the Azure Portal (https://portal.azure.com)
2. Create a new Azure WebSite based on an existing Azure Service Plan or create a new Azure Service Plan for that. (Even the free pricing tier works well) 
3. Visit the "Extensions Menu" and install the "Status Page" Extension
4. Prepare a new Azure Storage Account which can be used from the Status Page service as backend. A Local Redundant account is totally enough. 
5. Visit the Status Page Extension and configure the different details, e.g. the credentials for the storage account or the image the status page should use.
TODO: Add Picture here
6. Ensure that you installed the WebJob in your Azure WebSite and voila the Status Page will be generated

## Meters
TODO: Add Picture here

### Client Side Meters
Client side Meters are information delivered from the services as self, e.g. a heartbeat or some complex technical checks from time to time. This meters will be shown automatically in the status page. To integrate client side meters check the Status Page SDKs here:
* NET SDK for Client Side Meters - https://www.nuget.org/packages/statuspageclient/
* nodejs SDK for Client Side Meters - https://www.npmjs.com/package/azure-status-page-client
As soon your client is posting data the first time client side meters will be visible at the Status Page!

### Server Side Meters
Service Side Meters are executed in the App Service the Status Page is hosted and can be used to check specific metrics from outside of the applicaton, e.g. the availability of a specific application.

## Contributing

1. Fork it!
2. Create your feature branch: `git checkout -b my-new-feature`
3. Commit your changes: `git commit -m 'Add some feature'`
4. Push to the branch: `git push origin my-new-feature`
5. Submit a pull request :)
