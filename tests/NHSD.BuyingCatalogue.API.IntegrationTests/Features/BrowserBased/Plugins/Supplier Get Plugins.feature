@nullStringConversion
Feature:  Display Marketing Page Form Plugins Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Plugins
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
        | Sln5       | SolutionTest   | 1                | Sup 1      |
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription     | ClientApplication                                                                                                                                                                            |
        | Sln1       | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins" : {"Required" : true, "AdditionalInformation": "orem ipsum" } } |
        | Sln3       | Fully fledged GP system        | Fully fledged GP 12 |                                                                                                                                                                                              |
        | Sln5       | Testing System                 | Full System         | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Chrome" ], "MobileResponsive": false, "Plugins" : {"Required" : null, "AdditionalInformation": null } }        |

@2786
Scenario: Plugins are retrieved for the solution
    When a GET request is made for browser-plug-ins-or-extensions section for solution Sln1
    Then a successful response is returned
    And the string value of element plugins-required is Yes
    And the string value of element plugins-detail is orem ipsum

@2786
Scenario: Plugins are retrieved for the solution where no solution detail exists
    When a GET request is made for browser-plug-ins-or-extensions section for solution Sln2
    Then a successful response is returned
    And the plugins-required string does not exist
    And the plugins-detail string does not exist

@2786
Scenario: Solution not found
    Given a Solution Sln4 does not exist
    When a GET request is made for browser-plug-ins-or-extensions section for solution Sln4
    Then a response status of 404 is returned

@2786
Scenario: Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for browser-plug-ins-or-extensions section for solution Sln1
    Then a response status of 500 is returned

@2786
Scenario: Solution id not present in request
    When a GET request is made for browser-plug-ins-or-extensions section with no solution id
    Then a response status of 400 is returned

@2786
Scenario: Plugins are retrieved for the solution where no plugins-required
    When a GET request is made for browser-plug-ins-or-extensions section for solution Sln5
    Then a successful response is returned
    And the "plugins-required" string does not exist
    And the plugins-detail string does not exist
