Feature: Display Marketing Page Preview Native Mobile Section
	As a Catalogue User
    I want to manage Marketing Page Information for the Client Application Types Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionID | SolutionName                   | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline                    | GPs-R-Us         | 1                | Sup 1      |
        | Sln2       | Mobile Connection Details Only | GPs-R-Us         | 1                | Sup 1      |
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription                  | ClientApplication                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    |
        | Sln1     | Online Description             | Online medicine 1                | { "ClientApplicationTypes" : [ "native-mobile"], "MobileOperatingSystems": { "OperatingSystems": ["Windows", "Linux"], "OperatingSystemsDescription": "For windows only version 10" }, "NativeMobileFirstDesign": true, "MobileConnectionDetails": { "ConnectionType": [ "3G", "4G" ], "MinimumConnectionSpeed": "1GBps", "Description": "A connecton detail description" }, "MobileMemoryAndStorage" : { "MinimumMemoryRequirement": "500MB", "Description": "Storage Description" }, "NativeMobileHardwareRequirements": "A native mobile hardware requirement", "MobileThirdParty": { "ThirdPartyComponents": "Components", "DeviceCapabilities": "Cap" }, "NativeMobileAdditionalInformation": "native mobile additional info" } |
        | Sln2     | Mobile Connection Details Only | Mobile Connection Details Only 1 | { "ClientApplicationTypes" : [ "native-mobile"], "MobileConnectionDetails": { "ConnectionType": [ "3G", "4G" ], "MinimumConnectionSpeed": "1GBps", "Description": "A connecton detail description" } }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               |

@3605
Scenario:1. Get Solution Preview contains client application types native-mobile answers for all data
    When a GET request is made for solution preview Sln1
    Then a successful response is returned
    And the solution client-application-types section is returned
    And the solution client-application-types section contains operating-systems
        | OperatingSystems |
        | Windows, Linux   |
   And the solution client-application-types section contains operating-systems-description with value For windows only version 10
   And the solution native-mobile native-mobile-connection-details section contains connection-types
        | ConnectionTypes |
        | 3G,4G           |
    And the solution native-mobile native-mobile-first section contains mobile-first-design with value Yes
    And the solution native-mobile native-mobile-connection-details section contains minimum-connection-speed with value 1GBps
    And the solution native-mobile native-mobile-connection-details section contains connection-requirements-description with value A connecton detail description
    And the solution client-application-types section contains minimum-memory-requirement with value 500MB
    And the solution client-application-types section contains storage-requirements-description with value Storage Description
    And the solution native-mobile native-mobile-hardware-requirements section contains hardware-requirements with value A native mobile hardware requirement
    And the solution native-mobile native-mobile-third-party section contains third-party-components with value Components
    And the solution native-mobile native-mobile-third-party section contains device-capabilities with value Cap
    And the solution client-application-types section contains native-mobile-additional-information with value native mobile additional info


Scenario:2. Get Solution Preview contains client application types native-mobile answers for mobile connection details
    When a GET request is made for solution preview Sln2
    Then a successful response is returned
    And the solution client-application-types section is returned
   And the solution native-mobile native-mobile-connection-details section contains connection-types
        | ConnectionTypes |
        | 3G,4G           |
    And the solution native-mobile native-mobile-connection-details section contains minimum-connection-speed with value 1GBps
    And the solution native-mobile native-mobile-connection-details section contains connection-requirements-description with value A connecton detail description
    And the solution native-mobile native-mobile-first section does not contain mobile-first-design
    And the solution native-mobile native-mobile-additional-information section does not contain additional-information

