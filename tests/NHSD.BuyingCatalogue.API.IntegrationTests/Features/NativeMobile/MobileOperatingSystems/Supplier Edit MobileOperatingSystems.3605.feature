Feature:  Supplier Edit Mobile Operating Systems
    As a Supplier
    I want to Edit the Mobile Operating Systems Section
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
@3605
Scenario: 1. Mobile Operating Systems is updated
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                                                                                                                                                                                                                                                   |
        | Sln1     | An full online medicine system | Online medicine 1 | { "MobileOperatingSystems": { "OperatingSystems": ["Windows"], "OperatingSystemsDescription": "Windows 10 only" }, "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins" : {"Required" : true, "AdditionalInformation": "orem ipsum"}, "HardwareRequirements": "Hardware Information",  "AdditionalInformation": "Some more info", "MobileFirstDesign": true } |
    When a PUT request is made to update the native-mobile-operating-systems section for solution Sln1
        | OperatingSystems | OperatingSystemsDescription |
        | Linux, Windows   | Added Linux                 |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     |
        | Sln1     | An full online medicine system | Online medicine 1 | {"ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins" : {"Required" : true, "AdditionalInformation": "orem ipsum"}, "MinimumConnectionSpeed": null, "MinimumDesktopResolution": null, "HardwareRequirements": "Hardware Information", "NativeMobileHardwareRequirements": null, "NativeDesktopHardwareRequirements": null,  "AdditionalInformation": "Some more info", "MobileFirstDesign": true, "NativeMobileFirstDesign": null, "MobileOperatingSystems": { "OperatingSystems": ["Linux", "Windows"], "OperatingSystemsDescription": "Added Linux" }, "MobileConnectionDetails": null, "MobileMemoryAndStorage": null, "MobileThirdParty": null, "NativeMobileAdditionalInformation": null } |

@3605
Scenario: 2. Solution is not found
    Given a Solution Sln2 does not exist
    When a PUT request is made to update the native-mobile-operating-systems section for solution Sln2
      | OperatingSystems | OperatingSystemsDescription |
      | Linux            | Added Linux                 |
    Then a response status of 404 is returned 

@3605
Scenario: 3. Service Failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the native-mobile-operating-systems section for solution Sln1
       | OperatingSystems | OperatingSystemsDescription |
       | Linux, Windows   | Some more description       |
    Then a response status of 500 is returned

@3605
Scenario: 4. Solution id is not present in the request
    When a PUT request is made to update the native-mobile-operating-systems section with no solution id
       | OperatingSystems | OperatingSystemsDescription |
       | Linux, Windows   | Added Linux                 |
    Then a response status of 400 is returned
