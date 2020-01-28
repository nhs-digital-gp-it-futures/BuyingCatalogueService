Feature:  Display Marketing Page Form Browsers Supported Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Browsers Support
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
        | Sln3       | PracticeMgr    | 1                | Sup 2      |
        | Sln5       | SolutionTest   | 1                | Sup 2      |
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription     | ClientApplication                                                        |
        | Sln1     | An full online medicine system | Online medicine 1   | { "BrowsersSupported" : [ "Chrome", "Edge" ], "MobileResponsive": true } |
        | Sln3     | Fully fledged GP system        | Fully fledged GP 12 |                                                                          |
        | Sln5     | Testing System                 | Full System         | {"MobileResponsive": false }                                             |

@2786
Scenario: 1. Supported Browsers are retrieved for the solution
    When a GET request is made for browser-browsers-supported section for solution Sln1
    Then a successful response is returned
    And the supported-browsers element contains
        | Elements     |
        | Chrome, Edge |
    And the mobile-responsive element is Yes

@2786
Scenario: 2. Supported Browsers are retrieved for the solution where no solution detail exists
    When a GET request is made for browser-browsers-supported section for solution Sln2
    Then a successful response is returned
    And the supported-browsers element contains
        | Elements |
        |          |
    And the mobile-responsive element is null

@2786
Scenario: 3.Supported Browsers are retrieved for the solution where no browser-browsers-supported exist
    When a GET request is made for browser-browsers-supported section for solution Sln3
    Then a successful response is returned
    And the supported-browsers element contains
        | Elements |
        |          |
    And the mobile-responsive element is null

@2786
Scenario: 4. Solution not found
    Given a Solution Sln4 does not exist
    When a GET request is made for browser-browsers-supported section for solution Sln4
    Then a response status of 404 is returned

@2786
Scenario: 5. Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for browser-browsers-supported section for solution Sln1
    Then a response status of 500 is returned

@2786
Scenario: 6. Solution id not present in request
    When a GET request is made for browser-browsers-supported section with no solution id
    Then a response status of 400 is returned


@2786
Scenario: 7.Supported Browsers are retrieved for the solution where no supported-browsers
    When a GET request is made for browser-browsers-supported section for solution Sln5
    Then a successful response is returned
    And the supported-browsers element contains
    | Elements |
    |          |
    And the mobile-responsive element is No
