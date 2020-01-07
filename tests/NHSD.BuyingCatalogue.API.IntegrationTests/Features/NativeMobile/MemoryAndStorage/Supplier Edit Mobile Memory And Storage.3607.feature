Feature: Mobile Memory And Storage
    As a Supplier
    I want to Edit the Mobile Memory And Storage section
    So that I can make sure the information is correct

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
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1     | An full online medicine system | Online medicine 1 | {}                |

@3607
Scenario: 1. Client Application is updated for the solution
    When a PUT request is made to update the native-mobile-memory-and-storage section for solution Sln1
        | MinimumMemoryRequirement | Description   |
        | 1GB                      | A description |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported": [], "MobileResponsive": null, "Plugins": null, "MinimumConnectionSpeed": null, "MinimumDesktopResolution": null, "HardwareRequirements": null, "NativeMobileHardwareRequirements": null, "NativeDesktopHardwareRequirements": null, "AdditionalInformation": null, "MobileFirstDesign": null, "NativeMobileFirstDesign": null, "MobileOperatingSystems": null, "MobileConnectionDetails": null, "MobileMemoryAndStorage" : { "MinimumMemoryRequirement": "1GB", "Description": "A description" }, "MobileThirdParty": null, "NativeMobileAdditionalInformation": null } |

@3607
Scenario: 2. Solution is not found
    Given a Solution Sln2 does not exist
    When a PUT request is made to update the native-mobile-memory-and-storage section for solution Sln2
    | MinimumMemoryRequirement | Description   |
    | 1GB                      | A description |
    Then a response status of 404 is returned 

@3607
Scenario: 3. Service Failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the native-mobile-memory-and-storage section for solution Sln1
    | MinimumMemoryRequirement | Description   |
    | 1GB                      | A description |
    Then a response status of 500 is returned

@3607
Scenario: 4. Solution id is not present in the request
    When a PUT request is made to update the native-mobile-memory-and-storage section with no solution id
    | MinimumMemoryRequirement | Description   |
    | 1GB                      | A description |
    Then a response status of 400 is returned
