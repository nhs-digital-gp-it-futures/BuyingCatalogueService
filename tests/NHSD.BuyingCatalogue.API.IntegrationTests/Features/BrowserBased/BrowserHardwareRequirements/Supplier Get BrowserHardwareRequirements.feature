Feature:  Display Marketing Page Form Browser Hardware Requirements
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Browsers Hardware Requirements
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
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                                                            |
        | Sln1       | An full online medicine system | Online medicine 1 | { "HardwareRequirements": "Hardware Information", "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins" : {"Required" : true, "AdditionalInformation": "orem ipsum" } } |
        | Sln3       | Testing System                 | Full System       | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins" : {"Required" : true, "AdditionalInformation": "orem ipsum" } }                                                 |

@3600
Scenario: Browser Hardware Requirements are retreived for the solution
    When a GET request is made for browser-hardware-requirements section for solution Sln1
    Then a successful response is returned
    And the string value of element hardware-requirements-description is Hardware Information

@3600
Scenario: Browser Hardware Requirements are retrieved for the solution where no solutiondetail exists
    When a GET request is made for browser-hardware-requirements section for solution Sln2
    Then a successful response is returned
    And there are no browser-hardware-requirements

@3600
Scenario: Browser Hardware Requirements are retrieved for the solution where there are no hardware requirements
    When a GET request is made for browser-hardware-requirements section for solution Sln3
    Then a successful response is returned
    And there are no browser-hardware-requirements

@3600
Scenario: Solution not found
    Given a Solution Sln4 does not exist
    When a GET request is made for browser-hardware-requirements section for solution Sln4
    Then a response status of 404 is returned

@2786
Scenario: Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for browser-hardware-requirements section for solution Sln1
    Then a response status of 500 is returned

@2786
Scenario: Solution id not present in request
    When a GET request is made for browser-hardware-requirements section with no solution id
    Then a response status of 400 is returned
