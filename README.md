# Report a missed bin

This is a demo web application designed to work with the [Waste Service Standards](http://communitiesuk.github.io/waste-service-standards/) 
developed by the [Local Waste Service Standards Project](http://www.localdigitalcoalition.uk/product/local-waste-service-standards-project/). Developed by [Luton Borough Council}(http://www.luton.gov.uk) in partnership with our IM provider [Civica](https://www.civica.co.uk/).

## Pre-requisites

* Microsoft SQL server database to store LLPG data. Please see the INSTALL file for details on the database objects required and SQL to create them.
* Microsoft Windows server with IIS and .NET framework 4.5 for the web server

## How it works

1. User enters a post code/street name
2. Ajax call to Street controller which does lookup in LLPG database and returns a list of addresses matching post code/street name
3. User selects address from drop down list
4. Ajax call to Waste services API which returns bin information for the selected address
