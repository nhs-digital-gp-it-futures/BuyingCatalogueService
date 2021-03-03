Feature: Display Marketing Page Public Native Mobile Section
    As a Catalogue User
    I want to manage Marketing Page Information for the Client Application Types Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName | SupplierId |
        | Sln1       | MedicOnline  | Sup 1      |
    And solutions have the following details
        | SolutionId | ClientApplication                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    |
        | Sln1       | { "ClientApplicationTypes" : [ "native-mobile"], "MobileOperatingSystems": { "OperatingSystems": ["Windows", "Linux"], "OperatingSystemsDescription": "For windows only version 10" }, "NativeMobileFirstDesign": true, "MobileConnectionDetails": { "ConnectionType": [ "3G", "4G" ], "MinimumConnectionSpeed": "1GBps", "Description": "A connecton detail description" }, "MobileMemoryAndStorage" : { "MinimumMemoryRequirement": "500MB", "Description": "Storage Description" }, "NativeMobileHardwareRequirements": "A native mobile hardware requirement", "MobileThirdParty": { "ThirdPartyComponents": "Components", "DeviceCapabilities": "Cap" }, "NativeMobileAdditionalInformation": "native mobile additional info" } |

@3605
Scenario: Get Solution Public contains client application types native-mobile answers for all data
    When a GET request is made for solution public Sln1
    Then a successful response is returned
    And the solution client-application-types section is returned
    And the response contains the following values
        | Section                              | Field                               | Value                                |
        | native-mobile-connection-details     | connection-types                    | 3G,4G                                |
        | native-mobile-connection-details     | minimum-connection-speed            | 1GBps                                |
        | native-mobile-connection-details     | connection-requirements-description | A connecton detail description       |
        | native-mobile-third-party            | third-party-components              | Components                           |
        | native-mobile-third-party            | device-capabilities                 | Cap                                  |
        | native-mobile-operating-systems      | operating-systems                   | Windows, Linux                       |
        | native-mobile-operating-systems      | operating-systems-description       | For windows only version 10          |
        | native-mobile-first                  | mobile-first-design                 | Yes                                  |
        | native-mobile-memory-and-storage     | minimum-memory-requirement          | 500MB                                |
        | native-mobile-memory-and-storage     | storage-requirements-description    | Storage Description                  |
        | native-mobile-hardware-requirements  | hardware-requirements               | A native mobile hardware requirement |
        | native-mobile-additional-information | additional-information              | native mobile additional info        |
