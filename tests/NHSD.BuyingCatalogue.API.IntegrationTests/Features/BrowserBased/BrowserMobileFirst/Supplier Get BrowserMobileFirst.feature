Feature:  Display Marketing Page Form Browser Mobile First Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Browser Mobile First
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
        | Sup 2 | Supplier 2   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierId |
        | Sln1       | MedicOnline    | Sup 1      |
        | Sln2       | TakeTheRedPill | Sup 2      |
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                                                                                                                                  |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins" : {"Required" : true, "AdditionalInformation": "orem ipsum"}, "HardwareRequirements": "Hardware Information",  "AdditionalInformation": "Some more info", "MobileFirstDesign": true } |
        | Sln2       | Testing System                 | Full System       | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins" : {"Required" : true, "AdditionalInformation": "orem ipsum" } }                                                                                                                       |

@3602
Scenario: Browser Mobile First is retreived for the solution
    When a GET request is made for browser-mobile-first section for solution Sln1
    Then a successful response is returned
    And the string value of element mobile-first-design is Yes

@3602
Scenario: Browser Mobile First is retrieved for the solution where no solutiondetail exists
    When a GET request is made for browser-mobile-first section for solution Sln2
    Then a successful response is returned
    And the mobile-first-design string does not exist

@3602
Scenario: Solution not found
    Given a Solution Sln3 does not exist
    When a GET request is made for browser-mobile-first section for solution Sln3
    Then a response status of 404 is returned

@3602
Scenario: Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for browser-mobile-first section for solution Sln1
    Then a response status of 500 is returned

@3602
Scenario: Solution id not present in request
    When a GET request is made for browser-mobile-first section with no solution id
    Then a response status of 400 is returned
