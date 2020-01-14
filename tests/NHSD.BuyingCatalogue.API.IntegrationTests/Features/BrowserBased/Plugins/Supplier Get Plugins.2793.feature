@nullStringConversion
Feature:  Display Marketing Page Form Plugins Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Plugins
    So that I can ensure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
        | Drs. Inc |
    And Suppliers exist
        | Id    | SupplierName | OrganisationName |
        | Sup 1 | Supplier 1   | GPs-R-Us         |
        | Sup 2 | Supplier 2   | Drs. Inc         |
    And Solutions exist
        | SolutionID | SolutionName   | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | GPs-R-Us         | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | Drs. Inc         | 1                | Sup 2      |
        | Sln3       | PracticeMgr    | Drs. Inc         | 1                | Sup 2      |
        | Sln5       | SolutionTest   | GPs-R-Us         | 1                | Sup 1      |
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription     | ClientApplication                                                                                                                                                                            |
        | Sln1     | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins" : {"Required" : true, "AdditionalInformation": "orem ipsum" } } |
        | Sln3     | Fully fledged GP system        | Fully fledged GP 12 |                                                                                                                                                                                              |
        | Sln5     | Testing System                 | Full System         | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Chrome" ], "MobileResponsive": false, "Plugins" : {"Required" : null, "AdditionalInformation": null } }        |

@2786
Scenario: 1. Plugins are retrieved for the solution
    When a GET request is made for browser-plug-ins-or-extensions for solution Sln1
    Then a successful response is returned
    And the string value of element plugins-required is Yes
    And the string value of element plugins-detail is orem ipsum

@2786
Scenario: 2. Plugins are retrieved for the solution where no solution detail exists
    When a GET request is made for browser-plug-ins-or-extensions for solution Sln2
    Then a successful response is returned
    And the plugins-required string does not exist
    And the plugins-detail string does not exist

@2786
Scenario: 3. Solution not found
    Given a Solution Sln4 does not exist
    When a GET request is made for browser-plug-ins-or-extensions for solution Sln4
    Then a response status of 404 is returned

@2786
Scenario: 4. Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for browser-plug-ins-or-extensions for solution Sln1
    Then a response status of 500 is returned

@2786
Scenario: 5. Solution id not present in request
    When a GET request is made for browser-plug-ins-or-extensions with no solution id
    Then a response status of 400 is returned
    
@2786
Scenario: 6. Plugins are retrieved for the solution where no plugins-required
    When a GET request is made for browser-plug-ins-or-extensions for solution Sln5
    Then a successful response is returned
    And the "plugins-required" string does not exist
    And the plugins-detail string does not exist
