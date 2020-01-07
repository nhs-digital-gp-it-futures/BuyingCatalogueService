Feature:  Supplier Edit Browser Mobile First
    As a Supplier
    I want to Edit the Browser Mobile First Section
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
@3602
Scenario: 1. Browser Mobile First is updated
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                                                                                                                                |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins": null, "MinimumConnectionSpeed": null, "MinimumDesktopResolution": null, "HardwareRequirements": "New Hardware", "AdditionalInformation": "Some Info", "MobileFirstDesign": false } |
    When a PUT request is made to update the browser-mobile-first section for solution Sln1
        | MobileFirstDesign |
        | YEs               |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins": null, "MinimumConnectionSpeed": null, "MinimumDesktopResolution": null, "HardwareRequirements": "New Hardware", "NativeMobileHardwareRequirements": null, "NativeDesktopHardwareRequirements": null, "AdditionalInformation": "Some Info", "MobileFirstDesign": true, "NativeMobileFirstDesign": null, "MobileOperatingSystems": null, "MobileConnectionDetails": null, "MobileMemoryAndStorage": null, "MobileThirdParty": null, "NativeMobileAdditionalInformation": null, "NativeDesktopOperatingSystemsDescription": null } |

@3602
Scenario: 2. Solution is not found
    Given a Solution Sln2 does not exist
    When a PUT request is made to update the browser-mobile-first section for solution Sln2
       | MobileFirstDesign |
       | no                |
    Then a response status of 404 is returned 

@3602
Scenario: 3. Service Failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the browser-mobile-first section for solution Sln1
        | MobileFirstDesign |
        | nO                |
    Then a response status of 500 is returned

@3602
Scenario: 4. Solution id is not present in the request
    When a PUT request is made to update the browser-mobile-first section with no solution id
        | MobileFirstDesign |
        | YeS               |
    Then a response status of 400 is returned
