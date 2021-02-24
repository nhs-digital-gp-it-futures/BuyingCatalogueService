Feature: Supplier Display Sub-Form Native Mobile Client Type
    As a Supplier
    I want to Display Sub-Form Browser Based Client Type
    So that I can make sure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
        | Sup 2 | Supplier 2   |
    And Solutions exist
        | SolutionId | SolutionName   | SummaryDescription             | FullDescription     | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | An full online medicine system | Online medicine 1   | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | Eye opening experience         | Eye opening6        | 1                | Sup 2      |
        | Sln3       | PracticeMgr    | Fully fledged GP system        | Fully fledged GP 12 | 1                | Sup 2      |

@3604
Scenario: Sub-Form Native Mobile Client Type all sections are Displayed
    When a GET request is made for native-mobile section dashboard for solution Sln1
    Then a successful response is returned
    And Solutions section contains all items  
        | Id                                               | Status     | Requirement |
        | native-mobile-operating-systems                  | INCOMPLETE | Mandatory   |
        | native-mobile-first                              | INCOMPLETE | Mandatory   |
        | native-mobile-memory-and-storage                 | INCOMPLETE | Mandatory   |
        | native-mobile-connection-details                 | INCOMPLETE | Optional    |
        | native-mobile-components-and-device-capabilities | INCOMPLETE | Optional    |
        | native-mobile-hardware-requirements              | INCOMPLETE | Optional    |
        | native-mobile-third-party                        | INCOMPLETE | Optional    |
        | native-mobile-additional-information             | INCOMPLETE | Optional    |

@3604
Scenario: Solution not found
    Given a Solution Sln4 does not exist
    When a GET request is made for native-mobile section dashboard for solution Sln4
    Then a response status of 404 is returned

@3604
Scenario: Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for native-mobile section dashboard for solution Sln1
    Then a response status of 500 is returned

@3604
Scenario: Solution id not present in request
    When a GET request is made for native-mobile section dashboard with no solution id
    Then a response status of 400 is returned

@3605
Scenario Outline: Native Mobile Operating Systems Based on data in Client Application
  Given solutions have the following details
        | SolutionId | ClientApplication   |
        | Sln1       | <ClientApplication> |
    When a GET request is made for native-mobile section dashboard for solution Sln1
    Then a successful response is returned
    And the solution native-mobile-operating-systems section status is <Status>
Examples:
    | ClientApplication                                                                                             | Status     |
    |                                                                                                               | INCOMPLETE |
    | { "MobileOperatingSystems": { "OperatingSystems": [], "OperatingSystemsDescription": null } }                 | INCOMPLETE |
    | { "MobileOperatingSystems": { "OperatingSystems": [], "OperatingSystemsDescription": "Test" } }               | INCOMPLETE |
    | { "MobileOperatingSystems": { "OperatingSystems": ["IOS"], "OperatingSystemsDescription": null } }            | COMPLETE   |
    | { "MobileOperatingSystems": { "OperatingSystems": ["IOS", "Linux"], "OperatingSystemsDescription": null } }   | COMPLETE   |
    | { "MobileOperatingSystems": { "OperatingSystems": ["IOS", "Linux"], "OperatingSystemsDescription": "Test" } } | COMPLETE   |
    
@3606
Scenario Outline: Native Mobile Connection Details Based on data in Client Application
  Given solutions have the following details
        | SolutionId | ClientApplication   |
        | Sln1       | <ClientApplication> |
    When a GET request is made for native-mobile section dashboard for solution Sln1
    Then a successful response is returned
    And the solution native-mobile-connection-details section status is <Status>
Examples:
    | ClientApplication                                                                                                                | Status     |
    |                                                                                                                                  | INCOMPLETE |
    | { "MobileConnectionDetails": { "MinimumConnectionSpeed": "1GBps" } }                                                             | COMPLETE   |
    | { "MobileConnectionDetails": { "Description": "A description" } }                                                                | COMPLETE   |
    | { "MobileConnectionDetails": { "ConnectionType": [ "3G" ] } }                                                                    | COMPLETE   |
    | { "MobileConnectionDetails": { "ConnectionType": [ "3G" ], "Description": "A description" } }                                    | COMPLETE   |
    | { "MobileConnectionDetails": { "ConnectionType": [ "3G" ], "Description": "A description", "MinimumConnectionSpeed": "1GBps" } } | COMPLETE   |
        
@3607
Scenario Outline: Native Mobile Memory And Storage Based on data in Client Application
  Given solutions have the following details
        | SolutionId | ClientApplication   |
        | Sln1       | <ClientApplication> |
    When a GET request is made for native-mobile section dashboard for solution Sln1
    Then a successful response is returned
    And the solution native-mobile-memory-and-storage section status is <Status>
Examples:
    | ClientApplication                                                                                                                | Status     |
    |                                                                                                                                  | INCOMPLETE |
    | { "MobileMemoryAndStorage" : { "MinimumMemoryRequirement": "1GB", "Description": "A description" } }                             | COMPLETE   |
    | { "MobileMemoryAndStorage" : {  } }                                                                                              | INCOMPLETE |
    | { "MobileMemoryAndStorage" : { "Description": "A description" } }                                                                | INCOMPLETE |
    | { "MobileMemoryAndStorage" : { "MinimumMemoryRequirement": "1GB" } }                                                             | INCOMPLETE |
    | { "MobileConnectionDetails": { "ConnectionType": [ "3G" ], "Description": "A description", "MinimumConnectionSpeed": "1GBps" } } | INCOMPLETE |

@3611
Scenario Outline: Native Mobile First Based on data in Client Application
  Given solutions have the following details
        | SolutionId | ClientApplication   |
        | Sln1       | <ClientApplication> |
    When a GET request is made for native-mobile section dashboard for solution Sln1
    Then a successful response is returned
    And the solution native-mobile-first section status is <Status>
Examples:
    | ClientApplication                    | Status     |
    |                                      | INCOMPLETE |
    | { "NativeMobileFirstDesign": null }  | INCOMPLETE |
    | { "NativeMobileFirstDesign": false } | COMPLETE   |
    | { "NativeMobileFirstDesign": true }  | COMPLETE   |

@3609
Scenario Outline: Native Mobile Hardware Requirements based on data in Client Application
  Given solutions have the following details
        | SolutionId | ClientApplication   |
        | Sln1       | <ClientApplication> |
    When a GET request is made for native-mobile section dashboard for solution Sln1
    Then a successful response is returned
    And the solution native-mobile-hardware-requirements section status is <Status>
Examples:
    | ClientApplication                                          | Status     |
    |                                                            | INCOMPLETE |
    | { "NativeMobileHardwareRequirements": null }               | INCOMPLETE |
    | { "NativeMobileHardwareRequirements": "      " }           | INCOMPLETE |
    | { "NativeMobileHardwareRequirements": "Hardware Details" } | COMPLETE   |

@3608
Scenario Outline: Mobile Third Party based on data in Client Application
    Given solutions have the following details
        | SolutionId | ClientApplication   |
        | Sln1       | <ClientApplication> |
    When a GET request is made for native-mobile section dashboard for solution Sln1
    Then a successful response is returned
    And the solution native-mobile-third-party section status is <Status>
    Examples:
    | ClientApplication                                                                                   | Status     |
    |                                                                                                     | INCOMPLETE |
    | { "MobileThirdParty": null }                                                                        | INCOMPLETE |
    | { "MobileThirdParty": { "ThirdPartyComponents": "      ", "DeviceCapabilities": "Cap" } }           | COMPLETE   |
    | { "MobileThirdParty": { "ThirdPartyComponents": "Components", "DeviceCapabilities": "     " } }     | COMPLETE   |
    | { "MobileThirdParty": { "ThirdPartyComponents": "      ", "DeviceCapabilities": "      " } }        | INCOMPLETE |
    | { "MobileThirdParty": { "ThirdPartyComponents": null, "DeviceCapabilities": null } }                | INCOMPLETE |
    | { "MobileThirdParty": { "ThirdPartyComponents": "Component", "DeviceCapabilities": null } }         | COMPLETE   |
    | { "MobileThirdParty": { "ThirdPartyComponents": null, "DeviceCapabilities": "Capability" } }        | COMPLETE   |
    | { "MobileThirdParty": { "ThirdPartyComponents": "Component", "DeviceCapabilities": "Capability" } } | COMPLETE   |

@3609
Scenario Outline: Native Mobile Additional Information based on data in Client Application
    Given solutions have the following details
        | SolutionId | ClientApplication   |
        | Sln1       | <ClientApplication> |
    When a GET request is made for native-mobile section dashboard for solution Sln1
    Then a successful response is returned
    And the solution native-mobile-additional-information section status is <Status>
Examples:
    | ClientApplication                                                 | Status     |
    |                                                                   | INCOMPLETE |
    | { "NativeMobileAdditionalInformation": null }                     | INCOMPLETE |
    | { "NativeMobileAdditionalInformation": "      " }                 | INCOMPLETE |
    | { "NativeMobileAdditionalInformation": "Additional Information" } | COMPLETE   |
