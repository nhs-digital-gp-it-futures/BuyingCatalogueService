Feature:  Supplier Edit Mobile Third Party
    As a Supplier
    I want to Edit the Mobile Third Party Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName | SupplierId |
        | Sln1       | MedicOnline  | Sup 1      |
    And framework solutions exist
        | SolutionId | IsFoundation | FrameworkId |
        | Sln1       | true         | NHSDGP001   |

@3608
Scenario: Mobile Third Party is updated
    Given solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                        |
        | Sln1       | An full online medicine system | Online medicine 1 | { "MobileThirdParty": { "ThirdPartyComponents": "Party", "DeviceCapabilities": "Device" } } |
    When a PUT request is made to update the native-mobile-third-party section for solution Sln1
        | ThirdPartyComponents | DeviceCapabilities |
        | Component            | Capabilities       |
    Then a successful response is returned
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                            |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported": [], "MobileThirdParty": { "ThirdPartyComponents": "Component", "DeviceCapabilities": "Capabilities" } } |
    And Last Updated has been updated for solution Sln1
                                
@3608
Scenario: Mobile Third Party is updated with trimmed whitespace
    Given solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                        |
        | Sln1       | An full online medicine system | Online medicine 1 | { "MobileThirdParty": { "ThirdPartyComponents": "Party", "DeviceCapabilities": "Device" } } |
    When a PUT request is made to update the native-mobile-third-party section for solution Sln1
        | ThirdPartyComponents  | DeviceCapabilities     |
        | "       Component   " | "    Capabilities    " |
    Then a successful response is returned
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                            |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported": [], "MobileThirdParty": { "ThirdPartyComponents": "Component", "DeviceCapabilities": "Capabilities" } } |
    And Last Updated has been updated for solution Sln1
                                                                                                                                                     
@3608
Scenario: Solution is not found
    Given a Solution Sln4 does not exist
    When a PUT request is made to update the native-mobile-third-party section for solution Sln4
        | ThirdPartyComponents | DeviceCapabilities |
        | Component            | Capabilities       |
    Then a response status of 404 is returned 

@3608
Scenario: Service Failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the native-mobile-third-party section for solution Sln1
        | ThirdPartyComponents | DeviceCapabilities |
        | Component            | Capabilities       |
    Then a response status of 500 is returned

@3608
Scenario: Solution id is not present in the request
    When a PUT request is made to update the native-mobile-third-party section with no solution id
        | ThirdPartyComponents | DeviceCapabilities |
        | Component            | Capabilities       |
    Then a response status of 400 is returned
