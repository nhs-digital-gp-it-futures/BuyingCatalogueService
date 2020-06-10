Feature:  Get a single solution
    As a Buyer
    I want to view the information about a catalogue solution
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierId |
        | Sln1       | MedicOnline    | Sup 1      |
        | Sln2       | TakeTheRedPill | Sup 1      |
    And SolutionDetail exist
        | Solution | SummaryDescription             |
        | Sln1     | An full online medicine system |

@7261
Scenario: 1. Get a single solution
    When a GET request is made to retrieve a solution by ID 'Sln1'
    Then a successful response is returned
    And the string value of element name is MedicOnline
    And the string value of element summary is An full online medicine system

@7261
Scenario: 2. Get a single solution with only a name
    When a GET request is made to retrieve a solution by ID 'Sln2'
    Then a successful response is returned
    And the string value of element name is TakeTheRedPill
    And the solution does not contain summary

@7261
Scenario: 3. Solution not found
    Given a Solution Sln3 does not exist
    When a GET request is made to retrieve a solution by ID 'Sln3'
    Then a response status of 404 is returned

@7261
Scenario: 4. Service failure
    Given the call to the database to set the field will fail
    When a GET request is made to retrieve a solution by ID 'Sln1'
    Then a response status of 500 is returned
