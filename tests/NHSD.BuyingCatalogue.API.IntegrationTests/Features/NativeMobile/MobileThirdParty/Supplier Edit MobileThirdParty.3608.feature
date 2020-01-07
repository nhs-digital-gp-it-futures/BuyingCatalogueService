Feature:  Supplier Edit Mobile Third Party
    As a Supplier
    I want to Edit the Mobile Third Party Section
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

@3608
Scenario: 1. Mobile Third Party is updated
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                        |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": ["native-mobile"], "MobileThirdParty": { "ThirdPartyComponents": "Party", "DeviceCapabilities": "Device" } } |
    When a PUT request is made to update the native-mobile-third-party section for solution Sln1
        | ThirdPartyComponents | DeviceCapabilities |
        | Component            | Capabilities       |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": ["native-mobile"],"BrowsersSupported" : [], "MobileResponsive": null, "Plugins" : null, "MinimumConnectionSpeed": null, "MinimumDesktopResolution": null, "HardwareRequirements": null, "NativeMobileHardwareRequirements": null, "NativeDesktopHardwareRequirements": null, "AdditionalInformation": null, "MobileFirstDesign": null, "NativeMobileFirstDesign": null, "MobileOperatingSystems": null, "MobileConnectionDetails": null, "MobileMemoryAndStorage": null, "MobileThirdParty": { "ThirdPartyComponents": "Component", "DeviceCapabilities": "Capabilities" }, "NativeMobileAdditionalInformation": null, "NativeDesktopOperatingSystemsDescription": null } |
    And Last Updated has updated on the SolutionDetail for solution Sln1
                                                                                                                                                                             
@3608
Scenario: 2. Solution is not found
    Given a Solution Sln4 does not exist
    When a PUT request is made to update the native-mobile-third-party section for solution Sln4
        | ThirdPartyComponents | DeviceCapabilities |
        | Component            | Capabilities       |
    Then a response status of 404 is returned 

@3608
Scenario: 3. Service Failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the native-mobile-third-party section for solution Sln1
        | ThirdPartyComponents | DeviceCapabilities |
        | Component            | Capabilities       |
    Then a response status of 500 is returned

@3608
Scenario: 4. Solution id is not present in the request
    When a PUT request is made to update the native-mobile-third-party section with no solution id
        | ThirdPartyComponents | DeviceCapabilities |
        | Component            | Capabilities       |
    Then a response status of 400 is returned
