Feature:  Native Desktop Third Party
    As a Supplier
    I want to Edit the Native Desktop Third Party Section
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

@3621
Scenario: Native Desktop Third Party is updated
    Given solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                          |
        | Sln1       | An full online medicine system | Online medicine 1 | { "NativeDesktopThirdParty": { "ThirdPartyComponents": "Component", "DeviceCapabilities": "Capability" } } |
    When a PUT request is made to update the native-desktop-third-party section for solution Sln1
        | ThirdPartyComponents | DeviceCapabilities |
        | New Component        | New Capability     |
    Then a successful response is returned
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                          |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported" : [], "NativeDesktopThirdParty": { "ThirdPartyComponents": "New Component", "DeviceCapabilities": "New Capability" } } |
        
@3621
Scenario: Native Desktop Third Party is updated with trimmed whitespace
    Given solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                          |
        | Sln1       | An full online medicine system | Online medicine 1 | { "NativeDesktopThirdParty": { "ThirdPartyComponents": "Component", "DeviceCapabilities": "Capability" } } |
    When a PUT request is made to update the native-desktop-third-party section for solution Sln1
        | ThirdPartyComponents     | DeviceCapabilities       |
        | "      New Component   " | "     New Capability   " |
    Then a successful response is returned
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                          |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported" : [], "NativeDesktopThirdParty": { "ThirdPartyComponents": "New Component", "DeviceCapabilities": "New Capability" } } |

@3621
Scenario: Solution is not found
    Given a Solution Sln2 does not exist
    When a PUT request is made to update the native-desktop-third-party section for solution Sln2
        | ThirdPartyComponents | DeviceCapabilities |
        | New Component        | New Capability     |
    Then a response status of 404 is returned 

@3621
Scenario: Service Failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the native-desktop-third-party section for solution Sln2
        | ThirdPartyComponents | DeviceCapabilities |
        | New Component        | New Capability     |
    Then a response status of 500 is returned

@3621
Scenario: Solution id is not present in the request
    When a PUT request is made to update the native-desktop-third-party section with no solution id
        | ThirdPartyComponents | DeviceCapabilities |
        | New Component        | New Capability     |
    Then a response status of 400 is returned
