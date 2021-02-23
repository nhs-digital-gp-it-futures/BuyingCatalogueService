Feature: Supplier Get Connection And Resolution
    As a Supplier
    I want to manage the Solution Connection And Resolution details
    So that I can make sure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | 1                | Sup 1      |

@3599
Scenario: Connection and Resolution details are retrieved for the solution
    Given Solution have following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                     |
        | Sln1       | An full online medicine system | Online medicine 1 | { "MinimumConnectionSpeed": "2GBps", "MinimumDesktopResolution": "1x1" } |
    When a GET request is made for browser-connectivity-and-resolution section for solution Sln1
    Then a successful response is returned
    And the string value of element minimum-connection-speed is 2GBps
    And the string value of element minimum-desktop-resolution is 1x1

@3599
Scenario: Details are retrieved for the solution where no solution detail exists
    When a GET request is made for browser-connectivity-and-resolution section for solution Sln1
    Then a successful response is returned
    And the minimum-connection-speed string does not exist
    And the minimum-desktop-resolution string does not exist

@3599
Scenario: Solution not found
    Given a Solution Sln4 does not exist
    When a GET request is made for browser-connectivity-and-resolution section for solution Sln4
    Then a response status of 404 is returned

@3599
Scenario: Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for browser-connectivity-and-resolution section for solution Sln1
    Then a response status of 500 is returned

@3599
Scenario: Solution id not present in request
    When a GET request is made for browser-connectivity-and-resolution section with no solution id
    Then a response status of 400 is returned
