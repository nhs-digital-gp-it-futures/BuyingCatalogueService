Feature:  Display Marketing Page Form Native Desktop Third Party Validation on Edit
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Native Desktop Third Party 
    So that I can ensure the information is correct & valid

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionID | SolutionName   | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | GPs-R-Us         | 1                | Sup 1      |

@3621
Scenario: 1. Native Desktop Third Party Component exceeds its maxLength
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                          |
        | Sln1     | An full online medicine system | Online medicine 1 | { "NativeDesktopThirdParty": { "ThirdPartyComponents": "Component", "DeviceCapabilities": "Capability" } } |
    When a PUT request is made to update the native-desktop-third-party section for solution Sln1
        | ThirdPartyComponents        | DeviceCapabilities |
        | A string with length of 501 | New Capability     |
    Then a response status of 400 is returned
    And the third-party-components field value is the validation failure maxLength
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                          |
        | Sln1     | An full online medicine system | Online medicine 1 | { "NativeDesktopThirdParty": { "ThirdPartyComponents": "Component", "DeviceCapabilities": "Capability" } } |

@3621
Scenario: 2.Native Desktop Third Party Capability exceeds its maxLength
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                          |
        | Sln1     | An full online medicine system | Online medicine 1 | { "NativeDesktopThirdParty": { "ThirdPartyComponents": "Component", "DeviceCapabilities": "Capability" } } |
 When a PUT request is made to update the native-desktop-third-party section for solution Sln1
        | ThirdPartyComponents | DeviceCapabilities          |
        | Updated Component    | A string with length of 501 |
    Then a response status of 400 is returned
    And the device-capabilities field value is the validation failure maxLength
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                          |
        | Sln1     | An full online medicine system | Online medicine 1 | { "NativeDesktopThirdParty": { "ThirdPartyComponents": "Component", "DeviceCapabilities": "Capability" } } |

@3621
Scenario: 3.Native Desktop Third Party Component & Capability exceeds their maxLength
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                          |
        | Sln1     | An full online medicine system | Online medicine 1 | { "NativeDesktopThirdParty": { "ThirdPartyComponents": "Component", "DeviceCapabilities": "Capability" } } |
 When a PUT request is made to update the native-desktop-third-party section for solution Sln1
        | ThirdPartyComponents        | DeviceCapabilities          |
        | A string with length of 501 | A string with length of 501 |
    Then a response status of 400 is returned
    And the third-party-components field value is the validation failure maxLength
    And the device-capabilities field value is the validation failure maxLength
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                          |
        | Sln1     | An full online medicine system | Online medicine 1 | { "NativeDesktopThirdParty": { "ThirdPartyComponents": "Component", "DeviceCapabilities": "Capability" } } |
