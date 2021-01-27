Feature:  Display Marketing Page Form Solution Supplier Implementation Timescales Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Solution Implementation Timescales
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | 1                | Sup 1      |
    And SolutionDetail exist
        | Solution | ImplementationDetail   |
        | Sln1     | some valid description |

@3670
Scenario: Solution Implementation Timescales is retrieved for the solution
    When a GET request is made for implementation-timescales section for solution Sln1
    Then a successful response is returned
    And the string value of element description is some valid description

@3670
Scenario: Solution Implementation Timescales is retrieved empty for the solution where no solution detail exists
    When a GET request is made for implementation-timescales section for solution Sln2
    Then a successful response is returned
    And the description string does not exist

@3670
Scenario: Solution not found
    Given a Solution Sln3 does not exist
    When a GET request is made for implementation-timescales section for solution Sln3
    Then a response status of 404 is returned

@3670
Scenario: Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for implementation-timescales section for solution Sln1
    Then a response status of 500 is returned

@3670
Scenario: Solution id not present in request
    When a GET request is made for implementation-timescales section with no solution id
    Then a response status of 400 is returned
