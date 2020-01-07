Feature: Mobile Connection Details
    As a Supplier
    I want to Edit the Mobile Connection Details section
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

@3606
Scenario: 1. Client Application is updated for the solution
    When a PUT request is made to update the native-mobile-connection-details section for solution Sln1
        | MinimumConnectionSpeed | ConnectionRequirementsDescription | ConnectionType        |
        | 1GBps                  | A description                     | Horse, Moose, Giraffe |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported": [], "MobileResponsive": null, "Plugins": null, "MinimumConnectionSpeed": null, "MinimumDesktopResolution": null, "HardwareRequirements": null, "NativeMobileHardwareRequirements": null, "NativeDesktopHardwareRequirements": null, "AdditionalInformation": null, "MobileFirstDesign": null, "NativeMobileFirstDesign": null, "MobileOperatingSystems": null, "MobileConnectionDetails": { "MinimumConnectionSpeed": "1GBps", "ConnectionType": [ "Horse", "Moose", "Giraffe" ], "Description": "A description" }, "MobileMemoryAndStorage": null, "MobileThirdParty": null, "NativeMobileAdditionalInformation": null, "NativeDesktopOperatingSystemsDescription": null } |
        
@3606
Scenario: 2. Client Application is updated with no Minimum Connection Speed
    When a PUT request is made to update the native-mobile-connection-details section for solution Sln1
        | MinimumConnectionSpeed | ConnectionRequirementsDescription | ConnectionType        |
        | NULL                   | A description                     | Horse, Moose, Giraffe |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported": [], "MobileResponsive": null, "Plugins": null, "MinimumConnectionSpeed": null, "MinimumDesktopResolution": null, "HardwareRequirements": null, "NativeMobileHardwareRequirements": null, "NativeDesktopHardwareRequirements": null, "AdditionalInformation": null, "MobileFirstDesign": null, "NativeMobileFirstDesign": null, "MobileOperatingSystems": null, "MobileConnectionDetails": { "MinimumConnectionSpeed": null, "ConnectionType": [ "Horse", "Moose", "Giraffe" ], "Description": "A description" }, "MobileMemoryAndStorage": null, "MobileThirdParty": null, "NativeMobileAdditionalInformation": null, "NativeDesktopOperatingSystemsDescription": null } |
        
@3606
Scenario: 3. Client Application is updated with no Connection Requirements Description
    When a PUT request is made to update the native-mobile-connection-details section for solution Sln1
        | MinimumConnectionSpeed | ConnectionRequirementsDescription | ConnectionType        |
        | 1GBps                  | NULL                              | Horse, Moose, Giraffe |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported": [], "MobileResponsive": null, "Plugins": null, "MinimumConnectionSpeed": null, "MinimumDesktopResolution": null, "HardwareRequirements": null, "NativeMobileHardwareRequirements": null, "NativeDesktopHardwareRequirements": null, "AdditionalInformation": null, "MobileFirstDesign": null, "NativeMobileFirstDesign": null, "MobileOperatingSystems": null, "MobileConnectionDetails": { "MinimumConnectionSpeed": "1GBps", "ConnectionType": [ "Horse", "Moose", "Giraffe" ], "Description": null }, "MobileMemoryAndStorage": null, "MobileThirdParty": null, "NativeMobileAdditionalInformation": null, "NativeDesktopOperatingSystemsDescription": null } |

@3606
Scenario: 4. Client Application is updated with no Connection Type
    When a PUT request is made to update the native-mobile-connection-details section for solution Sln1
        | MinimumConnectionSpeed | ConnectionRequirementsDescription | ConnectionType |
        | 1GBps                  | A description                     |                |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported": [], "MobileResponsive": null, "Plugins": null, "MinimumConnectionSpeed": null, "MinimumDesktopResolution": null, "HardwareRequirements": null, "NativeMobileHardwareRequirements": null, "NativeDesktopHardwareRequirements": null, "AdditionalInformation": null, "MobileFirstDesign": null, "NativeMobileFirstDesign": null, "MobileOperatingSystems": null, "MobileConnectionDetails": { "MinimumConnectionSpeed": "1GBps", "ConnectionType": [], "Description": "A description" }, "MobileMemoryAndStorage": null, "MobileThirdParty": null, "NativeMobileAdditionalInformation": null, "NativeDesktopOperatingSystemsDescription": null } |
@3606
Scenario: 5. Solution is not found
    Given a Solution Sln2 does not exist
    When a PUT request is made to update the native-mobile-connection-details section for solution Sln2
    | MinimumConnectionSpeed | ConnectionRequirementsDescription | ConnectionType        |
    | NULL                   | A description                     | Horse, Moose, Giraffe |
    Then a response status of 404 is returned 

@3606
Scenario: 6. Service Failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the native-mobile-connection-details section for solution Sln1
    | MinimumConnectionSpeed | ConnectionRequirementsDescription | ConnectionType        |
    | NULL                   | A description                     | Horse, Moose, Giraffe |
    Then a response status of 500 is returned

@3606
Scenario: 7. Solution id is not present in the request
    When a PUT request is made to update the native-mobile-connection-details section with no solution id
    | MinimumConnectionSpeed | ConnectionRequirementsDescription | ConnectionType        |
    | NULL                   | A description                     | Horse, Moose, Giraffe |
    Then a response status of 400 is returned
