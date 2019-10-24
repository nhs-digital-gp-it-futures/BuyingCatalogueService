@ignore
Feature:  Display Marketing Page Form Browsers Supported Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Browsers Support
    So that I can ensure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
        | Drs. Inc |
    And Solutions exist
        | SolutionID | SolutionName   | SummaryDescription             | OrganisationName | FullDescription     | SupplierStatusId |
        | Sln1       | MedicOnline    | An full online medicine system | GPs-R-Us         | Online medicine 1   | 1                |
        | Sln2       | TakeTheRedPill | Eye opening experience         | Drs. Inc         | Eye opening6        | 1                |
        | Sln3       | PracticeMgr    | Fully fledged GP system        | Drs. Inc         | Fully fledged GP 12 | 1                |
    And MarketingDetail exist
        | Solution | SupportedBrowsers                               |
        | Sln1     | { "supported-browsers" : [ "Chrome", "Edge" ] } |
        | Sln3     |                                                 |
      

@2786
Scenario: 1. Supported Browsers are retrieved for the solution
    When a GET request is made for browsers-supported for solution Sln1
    Then a successful response is returned
    And the supported-browsers element contains
        | BrowsersSupported |
        | Chrome            |
        | Edge              |

@2786
Scenario: 2. Supported Browsers are retrieved for the solution where no marketing detail exists
    When a GET request is made for browsers-supported for solution Sln2
    Then a successful response is returned
    And the supported-browsers element contains
    | BrowsersSupported |

@2786
Scenario: 3.Supported Browsers are retrieved for the solution where no browsers-supported exist
    When a GET request is made for browsers-supported for solution Sln3
    Then a successful response is returned
    And the supported-browsers element contains
    | BrowsersSupported |

@2786
Scenario: 4. Solution not found
    Given a Solution Sln4 does not exist
    When a GET request is made for browsers-supported for solution Sln4
    Then a response status of 404 is returned

@2786
Scenario: 5. Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for browsers-supported for solution Sln1
    Then a response status of 500 is returned

@2786
Scenario: 6. Solution id not present in request
    When a GET request is made for browsers-supported with no solution id
    Then a response status of 400 is returned
