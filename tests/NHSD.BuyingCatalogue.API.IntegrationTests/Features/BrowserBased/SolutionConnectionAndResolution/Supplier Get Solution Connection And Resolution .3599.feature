Feature: Supplier Get Connection And Resolution
    As a Supplier
    I want to manage the Solution Connection And Resolution details
    So that I can make sure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
    And Suppliers exist
        | Id    | SupplierName | OrganisationName |
        | Sup 1 | Supplier 1   | GPs-R-Us         |
    And Solutions exist
        | SolutionID | SolutionName   | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | GPs-R-Us         | 1                | Sup 1      |

@3599
Scenario: 1. Connection and Resolution details are retreived for the solution
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                     |
        | Sln1     | An full online medicine system | Online medicine 1 | { "MinimumConnectionSpeed": "2GBps", "MinimumDesktopResolution": "1x1" } |
    When a GET request is made for browser-connectivity-and-resolution for solution Sln1
    Then a successful response is returned
    And the string value of element minimum-connection-speed is 2GBps
    And the string value of element minimum-desktop-resolution is 1x1

@3599
Scenario: 2. Details are retrieved for the solution where no solution detail exists
    When a GET request is made for browser-connectivity-and-resolution for solution Sln1
    Then a successful response is returned
    And the minimum-connection-speed string does not exist
    And the minimum-desktop-resolution string does not exist
    
@3599
Scenario: 3. Solution not found
    Given a Solution Sln4 does not exist
    When a GET request is made for browser-connectivity-and-resolution for solution Sln4
    Then a response status of 404 is returned
    
@3599
Scenario: 4. Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for browser-connectivity-and-resolution for solution Sln1
    Then a response status of 500 is returned
    
@3599
Scenario: 5. Solution id not present in request
    When a GET request is made for browser-connectivity-and-resolution with no solution id
    Then a response status of 400 is returned
