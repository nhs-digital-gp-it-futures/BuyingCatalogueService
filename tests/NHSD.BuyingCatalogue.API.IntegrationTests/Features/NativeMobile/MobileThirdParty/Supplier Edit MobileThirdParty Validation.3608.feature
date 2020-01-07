Feature:  Display Marketing Page Form Mobile Third Party Validation on Edit
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Mobile Third Party
    So that I can ensure the information is correct & valid

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
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                        |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": ["native-mobile"], "MobileThirdParty": { "ThirdPartyComponents": "Party", "DeviceCapabilities": "Device" } } |

@3608
Scenario: 1. Components length is greater than 500 chars, capabilities is valid
    When a PUT request is made to update the native-mobile-third-party section for solution Sln1
        | ThirdPartyComponents        | DeviceCapabilities |
        | A string with length of 501 | Capabilities       |
    Then a response status of 400 is returned
    And the third-party-components field value is the validation failure maxLength

@3608
Scenario: 2. Components is valid, Capabilities length is greater than 500 chars
    When a PUT request is made to update the native-mobile-third-party section for solution Sln1
        | ThirdPartyComponents | DeviceCapabilities          |
        | Yes                  | A string with length of 501 |
    Then a response status of 400 is returned
    And the device-capabilities field value is the validation failure maxLength
    
@3608
Scenario: 3. Components & Capabilities length is greater than 500 chars
    When a PUT request is made to update the native-mobile-third-party section for solution Sln1
       | ThirdPartyComponents        | DeviceCapabilities          |
       | A string with length of 501 | A string with length of 501 |
    Then a response status of 400 is returned
    And the third-party-components field value is the validation failure maxLength
    And the device-capabilities field value is the validation failure maxLength
