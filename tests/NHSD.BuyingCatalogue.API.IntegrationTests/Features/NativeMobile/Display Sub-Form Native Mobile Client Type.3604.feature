Feature: Supplier Display Sub-Form Native Mobile Client Type
    As a Supplier
    I want to Display Sub-Form Browser Based Client Type
    So that I can make sure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
        | Drs. Inc |
    And Suppliers exist
        | Id    | OrganisationName |
        | Sup 1 | GPs-R-Us         |
        | Sup 2 | Drs. Inc         |
    And Solutions exist
        | SolutionID | SolutionName   | SummaryDescription             | OrganisationName | FullDescription     | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | An full online medicine system | GPs-R-Us         | Online medicine 1   | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | Eye opening experience         | Drs. Inc         | Eye opening6        | 1                | Sup 2      |
        | Sln3       | PracticeMgr    | Fully fledged GP system        | Drs. Inc         | Fully fledged GP 12 | 1                | Sup 2      |

@3604
Scenario: 1. Sub-Form Native Mobile Client Type all sections are Displayed
    When a GET request is made for native-mobile for solution Sln1
    Then a successful response is returned
    And Solutions section contains all items  
        | Id                                        | Status     | Requirement |
        | mobile-operating-systems                  | INCOMPLETE | Mandatory   |
        | mobile-first                              | INCOMPLETE | Mandatory   |
        | mobile-memory-and-storage                 | INCOMPLETE | Mandatory   |
        | mobile-connection-details                 | INCOMPLETE | Optional    |
        | mobile-components-and-device-capabilities | INCOMPLETE | Optional    |
        | mobile-hardware-requirements              | INCOMPLETE | Optional    |
        | mobile-additional-information             | INCOMPLETE | Optional    |

@3604
Scenario: 2. Solution not found
    Given a Solution Sln4 does not exist
    When a GET request is made for native-mobile for solution Sln4
    Then a response status of 404 is returned

@3604
Scenario: 3. Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for native-mobile for solution Sln1
    Then a response status of 500 is returned

@3604
Scenario: 4. Solution id not present in request
    When a GET request is made for native-mobile with no solution id
    Then a response status of 400 is returned

@3605
Scenario Outline: 5. Native Mobile Operating Systems Based on data in Client Application
  Given SolutionDetail exist
        | Solution | ClientApplication   |
        | Sln1     | <ClientApplication> |
    When a GET request is made for native-mobile for solution Sln1
    Then a successful response is returned
    And the status of the mobile-operating-systems section is <Status>
Examples:
    | ClientApplication                                                                                             | Status     |
    |                                                                                                               | INCOMPLETE |
    | { "MobileOperatingSystems": { "OperatingSystems": [], "OperatingSystemsDescription": null } }                 | INCOMPLETE |
    | { "MobileOperatingSystems": { "OperatingSystems": [], "OperatingSystemsDescription": "Test" } }               | INCOMPLETE |
    | { "MobileOperatingSystems": { "OperatingSystems": ["IOS"], "OperatingSystemsDescription": null } }            | COMPLETE   |
    | { "MobileOperatingSystems": { "OperatingSystems": ["IOS", "Linux"], "OperatingSystemsDescription": null } }   | COMPLETE   |
    | { "MobileOperatingSystems": { "OperatingSystems": ["IOS", "Linux"], "OperatingSystemsDescription": "Test" } } | COMPLETE   |
    
@3606
Scenario Outline: 5. Native Mobile Connection Details Based on data in Client Application
  Given SolutionDetail exist
        | Solution | ClientApplication   |
        | Sln1     | <ClientApplication> |
    When a GET request is made for native-mobile for solution Sln1
    Then a successful response is returned
    And the status of the mobile-connection-details section is <Status>
Examples:
    | ClientApplication                                                                                                                | Status     |
    |                                                                                                                                  | INCOMPLETE |
    | { "MobileConnectionDetails": { "MinimumConnectionSpeed": "1GBps" } }                                                             | COMPLETE   |
    | { "MobileConnectionDetails": { "Description": "A description" } }                                                                | COMPLETE   |
    | { "MobileConnectionDetails": { "ConnectionType": [ "3G" ] } }                                                                    | COMPLETE   |
    | { "MobileConnectionDetails": { "ConnectionType": [ "3G" ], "Description": "A description" } }                                    | COMPLETE   |
    | { "MobileConnectionDetails": { "ConnectionType": [ "3G" ], "Description": "A description", "MinimumConnectionSpeed": "1GBps" } } | COMPLETE   |
        
@3607
Scenario Outline: 6. Native Mobile Memory And Storage Based on data in Client Application
  Given SolutionDetail exist
        | Solution | ClientApplication   |
        | Sln1     | <ClientApplication> |
    When a GET request is made for native-mobile for solution Sln1
    Then a successful response is returned
    And the status of the mobile-memory-and-storage section is <Status>
Examples:
    | ClientApplication                                                                                                                | Status     |
    |                                                                                                                                  | INCOMPLETE |
    | { "MobileMemoryAndStorage" : { "MinimumMemoryRequirement": "1GB", "Description": "A description" } }                             | COMPLETE   |
    | { "MobileMemoryAndStorage" : {  } }                                                                                              | INCOMPLETE |
    | { "MobileMemoryAndStorage" : { "Description": "A description" } }                                                                | INCOMPLETE |
    | { "MobileMemoryAndStorage" : { "MinimumMemoryRequirement": "1GB" } }                                                             | INCOMPLETE |
    | { "MobileConnectionDetails": { "ConnectionType": [ "3G" ], "Description": "A description", "MinimumConnectionSpeed": "1GBps" } } | INCOMPLETE |

@3611
Scenario Outline: 7. Native Mobile First Based on data in Client Application
  Given SolutionDetail exist
        | Solution | ClientApplication   |
        | Sln1     | <ClientApplication> |
    When a GET request is made for native-mobile for solution Sln1
    Then a successful response is returned
    And the status of the mobile-first section is <Status>
Examples:
    | ClientApplication                    | Status     |
    |                                      | INCOMPLETE |
    | { "NativeMobileFirstDesign": null }  | INCOMPLETE |
    | { "NativeMobileFirstDesign": false } | COMPLETE   |
    | { "NativeMobileFirstDesign": true }  | COMPLETE   |
