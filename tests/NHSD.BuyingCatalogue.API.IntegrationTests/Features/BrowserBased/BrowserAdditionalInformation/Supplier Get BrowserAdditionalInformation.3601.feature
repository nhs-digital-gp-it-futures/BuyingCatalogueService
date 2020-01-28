Feature:  Display Marketing Page Form Browser Additional Information
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Browsers Additional Information
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
        | Sup 2 | Supplier 2   |
    And Solutions exist
        | SolutionID | SolutionName   | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | GPs-R-Us         | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | Drs. Inc         | 1                | Sup 2      |
        | Sln3       | PracticeMgr    | Drs. Inc         | 1                | Sup 2      |
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                                                                                                      |
        | Sln1     | An full online medicine system | Online medicine 1 | { "AdditionalInformation": "Some more info", "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins" : {"Required" : true, "AdditionalInformation": "orem ipsum"}, "HardwareRequirements": "Hardware Information" } |
        | Sln3     | Testing System                 | Full System       | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins" : {"Required" : true, "AdditionalInformation": "orem ipsum" } }                                                                                           |

@3601
Scenario: 1. Browser Additional Information are retreived for the solution
    When a GET request is made for browser-additional-information section for solution Sln1
    Then a successful response is returned
    And the string value of element additional-information is Some more info

@3601
Scenario: 2. Browser Additional Information are retrieved for the solution where no solutiondetail exists
    When a GET request is made for browser-additional-information section for solution Sln2
    Then a successful response is returned
    And the additional-information string does not exist

@3601
Scenario: 3. Browser Additional Information are retrieved for the solution where there is no additional information
    When a GET request is made for browser-additional-information section for solution Sln3
    Then a successful response is returned
    And the additional-information string does not exist

@3601
Scenario: 4. Solution not found
    Given a Solution Sln4 does not exist
    When a GET request is made for browser-additional-information section for solution Sln4
    Then a response status of 404 is returned

@3601
Scenario: 5. Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for browser-additional-information section for solution Sln1
    Then a response status of 500 is returned

@3601
Scenario: 6. Solution id not present in request
    When a GET request is made for browser-additional-information section with no solution id
    Then a response status of 400 is returned

