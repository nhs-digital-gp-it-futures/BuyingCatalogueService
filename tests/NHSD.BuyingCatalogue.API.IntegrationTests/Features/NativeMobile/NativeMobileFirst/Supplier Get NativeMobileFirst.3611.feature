Feature:  Display Marketing Page Form Native Mobile First Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Native Mobile First 
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
        | Sup 2 | Supplier 2   |
    And Solutions exist
        | SolutionID | SolutionName   | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | 1                | Sup 2      |
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                                                                                                                                                                   |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": ["native-mobile"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins" : {"Required" : true, "AdditionalInformation": "orem ipsum"}, "HardwareRequirements": "Hardware Information",  "AdditionalInformation": "Some more info", "MobileFirstDesign": false, "NativeMobileFirstDesign": true} |
        | Sln2     | Testing System                 | Full System       | { "ClientApplicationTypes": ["native-mobile"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins" : {"Required" : true, "AdditionalInformation": "orem ipsum" } }                                                                                                                                                        |

@3611
Scenario: 1. Native Mobile First is retreived for the solution
    When a GET request is made for native-mobile-first section for solution Sln1
    Then a successful response is returned
    And the string value of element mobile-first-design is Yes

@3611
Scenario: 2. Native Mobile First is retrieved for the solution where no solutiondetail exists
    When a GET request is made for native-mobile-first section for solution Sln2
    Then a successful response is returned
    And the mobile-first-design string does not exist

@3611
Scenario: 3. Solution not found
    Given a Solution Sln3 does not exist
    When a GET request is made for native-mobile-first section for solution Sln3
    Then a response status of 404 is returned

@3611
Scenario: 4. Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for native-mobile-first section for solution Sln1
    Then a response status of 500 is returned

@3611
Scenario: 5. Solution id not present in request
    When a GET request is made for native-mobile-first section with no solution id
    Then a response status of 400 is returned

