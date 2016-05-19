var BinController = function ($http) {

    var vm = this;

    vm.visibility = {
        searchingForAddresses: false,
        searchingForBinInfo: false,
        showAddressList: false,
        showBinInfo: false,
        showError: {
            roadOrPostCode: false,
            addressList: false
        }
    };

    vm.errorMessage = {
        roadOrPostCode: "",
        addressList: ""
    };

    vm.addressList = [];
    vm.binInfo = [];

    vm.selectedAddress = null;
    vm.selectedBin = null;

    vm.findAddress = function (roadOrPostCode) {
        console.debug("Entering 'findAddress' function");

        reset();

        if (roadOrPostCode.length >= 3) {
            console.debug("Performing search...");

            vm.visibility.searchingForAddresses = true;
            
            var req = {
                method: 'GET',
                url: 'api/street/' + roadOrPostCode
            };

            $http(req).then(
                function (response) {
                    //Success callback
                    console.log("Street $http success");

                    console.debug("Search successful...");
                    console.debug("Status: " + response.status);
                    console.debug("Data: " + response.data);

                    if (response.data.length > 0) {
                        vm.addressList = response.data;
                        resetError("roadOrPostCode");
                        vm.visibility.showAddressList = true;
                    } else {
                        vm.visibility.showAddressList = false;
                        showError("roadOrPostCode", "No addresses matching '" + roadOrPostCode + "' could be found");
                        resetAddressList();
                    }

                    vm.visibility.searchingForAddresses = false;
                },
                function (response) {
                    //Error callback
                    console.log("Street $http error");

                    switch (response.status) {
                        case 403:
                        case 404:
                            showError("roadOrPostCode", response.data.Message);
                            resetAddressList();
                            break;
                        default:
                            showError("roadOrPostCode", "Unknown error occurred");
                            resetAddressList();
                    }
                    vm.visibility.searchingForAddresses = false;
                }
            );
        }

        console.debug("Exiting 'findAddress' function");
    };

    vm.findBinInfo = function (selectedAddress) {
        console.debug("Entering 'findBinInfo' function");

        if (selectedAddress != null && selectedAddress.Id.length > 0) {
            console.debug("Performing bin search...");

            vm.visibility.searchingForBinInfo = true;
            resetError("addressList"); //Need to clear any previous errors
            resetBinInfo();
            
            var req = {
                method: 'GET',
                url: 'https://waste-demo-api.herokuapp.com/services',
                params: { uprn: selectedAddress.Id }
            };

            $http(req).then(
                function (response) {
                    //Success callback
                    console.log("Bin $http success");

                    console.debug("Search successful...");
                    console.debug("Status: " + response.status);
                    console.debug("Data: " + response.data);

                    if (response.data.length > 0) {

                        var bins = response.data;
                        //Sort out any nulls in the returned data
                        angular.forEach(bins, function (bin) {
                            var status = "Collected";
                            var scheduledInfo = "Unknown";
                            
                            if (bin.description == null) {
                                bin.description = "No further information available";
                            }

                            if (bin.last_collections.length > 0) {
                                var lastcollection = bin.last_collections[bin.last_collections.length - 1];
                                if (lastcollection.status == null) { lastcollection.status = status; }
                                if (lastcollection.scheduled_start_date == null) { lastcollection.scheduled_start_date = scheduledInfo; }
                            }

                            if (bin.next_collections.length > 0) {
                                var nextcollection = bin.next_collections[0];
                                if (nextcollection.status == null) { nextcollection.status = status; }
                                if (nextcollection.scheduled_start_date == null) { nextcollection.scheduled_start_date = scheduledInfo; }
                            }

                        });

                        vm.binInfo = bins;
                        resetError("addressList");
                        vm.visibility.showBinInfo = true;
                    } else {
                        showError("addressList", "No bin information for that address could be found");
                    }

                    vm.visibility.searchingForBinInfo = false;
                },
                function (response) {
                    //Error callback
                    console.log("Bin $http error");

                    switch (response.status) {
                        case 403:
                        case 404:
                            showError("addressList", response.data.Message)
                            resetBinInfo();
                            break;
                        default:
                            showError("addressList", "Unknown error occurred");
                            resetBinInfo();
                    }

                    vm.visibility.searchingForBinInfo = false;
                }
            );

        }

        console.debug("Exiting 'findBinInfo' function");
    };

    vm.calcCollectionFrequency = function (lastCollectionDate, nextCollectionDate) {
        console.debug("Entering 'calcCollectionFrequency' function");

        if (lastCollectionDate != null && nextCollectionDate != null) {
            var lcTicks = new Date(lastCollectionDate).valueOf();
            var ncTicks = new Date(nextCollectionDate).valueOf();
        
            if (ncTicks > lcTicks) {
                var diffTicks = ncTicks - lcTicks;
                var dateDiff = new Date(diffTicks);
                var dayDiff = dateDiff.getDate();

                if (0 < dayDiff && dayDiff <= 8) { return "Weekly"; }
                if (8 < dayDiff && dayDiff <= 15) { return "Fortnightly"; }
                if (15 < dayDiff && dayDiff <= 29) { return "Monthly"; }
                if (29 < dayDiff) { return "Bi-monthly"; }
            }
        }

        console.debug("Exiting 'calcCollectionFrequency' function");

        return "Unknown";
    };

    var reset = function () {
        vm.visibility.searchingForAddresses = false;

        resetError("roadOrPostCode");
        resetError("addressList");
        resetAddressList();
        resetBinInfo();
    };

    var resetError = function (key) {
        vm.visibility.showError[key] = false;
        vm.errorMessage[key] = "";
    };

    var resetAddressList = function () {
        vm.visibility.searchingForBinInfo = false;
        vm.visibility.showAddressList = false;
        vm.selectedAddress = null;
    };

    var resetBinInfo = function () {
        vm.visibility.showBinInfo = false;
        vm.selectedBin = null;
    };

    var showError = function (key, message) {
        vm.errorMessage[key] = message;
        vm.visibility.showError[key] = true;
    };

    return vm;
};


