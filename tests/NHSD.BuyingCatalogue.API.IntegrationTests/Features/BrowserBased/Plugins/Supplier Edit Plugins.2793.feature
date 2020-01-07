Feature:  Supplier Edit Plugins
    As a Supplier
    I want to Edit the Plugins Section
    So that I can ensure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
    And Suppliers exist
        | Id    | OrganisationName |
        | Sup 1 | GPs-R-Us         |
    And Solutions exist
        | SolutionID | SolutionName   | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | GPs-R-Us         | 1                | Sup 1      |

@2786
Scenario: 1. Plugins is updated
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                                                                                     |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins" : {"Required" : false, "AdditionalInformation": "orem ipsum" }, "HardwareRequirements": "Hardware Info", "AdditionalInformation": null } |
    When a PUT request is made to update the browser-plug-ins-or-extensions section for solution Sln1
        | PluginsRequired | PluginsDetail             |
        | yEs             | This is extra information |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins" : { "Required" : true , "AdditionalInformation": "This is extra information"}, "MinimumConnectionSpeed": null, "MinimumDesktopResolution": null, "HardwareRequirements": "Hardware Info", "NativeMobileHardwareRequirements": null, "NativeDesktopHardwareRequirements": null, "AdditionalInformation": null, "MobileFirstDesign": null, "NativeMobileFirstDesign": null, "MobileOperatingSystems": null, "MobileConnectionDetails": null, "MobileMemoryAndStorage": null, "MobileThirdParty": null, "NativeMobileAdditionalInformation": null } |
    And Last Updated has updated on the SolutionDetail for solution Sln1
                                                                                                                                                                             
@2786
Scenario: 2. Solution is not found
    Given a Solution Sln4 does not exist
    When a PUT request is made to update the browser-plug-ins-or-extensions section for solution Sln4
        | PluginsRequired | PluginsDetail             |
        | nO              | This is extra information |
    Then a response status of 404 is returned 

@2786
Scenario: 3. Service Failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the browser-plug-ins-or-extensions section for solution Sln1
        | PluginsRequired | PluginsDetail             |
        | No              | This is extra information |
    Then a response status of 500 is returned

@2786
Scenario: 4. Solution id is not present in the request
    When a PUT request is made to update the browser-plug-ins-or-extensions section with no solution id
        | PluginsRequired | PluginsDetail             |
        | no              | This is extra information |
    Then a response status of 400 is returned
