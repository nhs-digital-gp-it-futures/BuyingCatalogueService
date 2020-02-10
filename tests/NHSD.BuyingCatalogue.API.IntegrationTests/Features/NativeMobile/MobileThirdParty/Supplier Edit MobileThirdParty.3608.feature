Feature:  Supplier Edit Mobile Third Party
    As a Supplier
    I want to Edit the Mobile Third Party Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | 1                | Sup 1      |

@3608
Scenario: 1. Mobile Third Party is updated
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                        |
        | Sln1     | An full online medicine system | Online medicine 1 | { "MobileThirdParty": { "ThirdPartyComponents": "Party", "DeviceCapabilities": "Device" } } |
    When a PUT request is made to update the native-mobile-third-party section for solution Sln1
        | ThirdPartyComponents | DeviceCapabilities |
        | Component            | Capabilities       |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                            |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported": [], "MobileThirdParty": { "ThirdPartyComponents": "Component", "DeviceCapabilities": "Capabilities" } } |
    And Last Updated has updated on the SolutionDetail for solution Sln1
                                
@3608
Scenario: 2. Mobile Third Party is updated with trimmed whitespace
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                        |
        | Sln1     | An full online medicine system | Online medicine 1 | { "MobileThirdParty": { "ThirdPartyComponents": "Party", "DeviceCapabilities": "Device" } } |
    When a PUT request is made to update the native-mobile-third-party section for solution Sln1
        | ThirdPartyComponents  | DeviceCapabilities     |
        | "       Component   " | "    Capabilities    " |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                            |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported": [], "MobileThirdParty": { "ThirdPartyComponents": "Component", "DeviceCapabilities": "Capabilities" } } |
    And Last Updated has updated on the SolutionDetail for solution Sln1
                                                                                                                                                     
@3608
Scenario: 3. Solution is not found
    Given a Solution Sln4 does not exist
    When a PUT request is made to update the native-mobile-third-party section for solution Sln4
        | ThirdPartyComponents | DeviceCapabilities |
        | Component            | Capabilities       |
    Then a response status of 404 is returned 

@3608
Scenario: 4. Service Failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the native-mobile-third-party section for solution Sln1
        | ThirdPartyComponents | DeviceCapabilities |
        | Component            | Capabilities       |
    Then a response status of 500 is returned

@3608
Scenario: 5. Solution id is not present in the request
    When a PUT request is made to update the native-mobile-third-party section with no solution id
        | ThirdPartyComponents | DeviceCapabilities |
        | Component            | Capabilities       |
    Then a response status of 400 is returned
