Feature:  Display Marketing Page Form Mobile Operating Systems Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Mobile Operating Systems
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
        | Sup 2 | Supplier 2   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | 1                | Sup 2      |
        | Sln3       | PracticeMgr    | 1                | Sup 2      |
    And Solution have following details
        | SolutionId | SummaryDescription             | FullDescription     | ClientApplication                                                                                                                                                                                                                                                                                                                  |
        | Sln1       | An full online medicine system | Online medicine 1   | { "MobileOperatingSystems": { "OperatingSystems": ["Windows", "Linux"], "OperatingSystemsDescription": "For windows only version 10" }, "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins" : {"Required" : true, "AdditionalInformation": "orem ipsum" } } |
        | Sln3       | Testing System                 | Full System         | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Chrome" ], "MobileResponsive": false, "Plugins" : {"Required" : null, "AdditionalInformation": null } }                                                                                                                                              |

@3605
Scenario: Mobile Operating Systems are retrieved for the solution
    When a GET request is made for native-mobile-operating-systems section for solution Sln1
    Then a successful response is returned
    And the operating-systems element contains
        | Elements       |
        | Windows, Linux |
    And the string value of element operating-systems-description is For windows only version 10

@3605
Scenario: Mobile Operating Systems are retrieved for the solution where no solution detail exists
    When a GET request is made for native-mobile-operating-systems section for solution Sln2
    Then a successful response is returned
    And the operating-systems element contains
        | Elements |
        |          |
    And the operating-systems-description string does not exist

@3605
Scenario: Solution not found
    Given a Solution Sln4 does not exist
    When a GET request is made for native-mobile-operating-systems section for solution Sln4
    Then a response status of 404 is returned

@3605
Scenario: Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for native-mobile-operating-systems section for solution Sln1
    Then a response status of 500 is returned

@3605
Scenario: Solution id not present in request
    When a GET request is made for native-mobile-operating-systems section with no solution id
    Then a response status of 400 is returned
    
@3605
Scenario: Mobile Operating Systems are retrieved for the solution where no mobile-operating-systems
    When a GET request is made for native-mobile-operating-systems section for solution Sln3
    Then a successful response is returned
    And the operating-systems element contains
        | Elements |
        |          |
    And the operating-systems-description string does not exist
