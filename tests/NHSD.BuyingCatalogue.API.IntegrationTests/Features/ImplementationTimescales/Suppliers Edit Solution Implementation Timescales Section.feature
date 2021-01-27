Feature: Suppliers Edit Solution Implementation Timescales Section
    As a Supplier
    I want to Edit the Solution Implementation Timescales Section
    So that I can make sure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | 1                | Sup 1      |
    And SolutionDetail exist
        | Solution | ImplementationDetail                                   |
        | Sln1     | An original implementation timescales description      |
        | Sln2     | Another original implementation timescales description |

@3670
Scenario: Solution implementation timescales section data is updated
    When a PUT request is made to update the implementation-timescales section for solution Sln1
        | ImplementationTimescales                    |
        | A new implementation timescales description |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | ImplementationDetail                                   |
        | Sln1     | A new implementation timescales description            |
        | Sln2     | Another original implementation timescales description |
    And Last Updated has updated on the SolutionDetail for solution Sln1

@3670
Scenario: Solution implementation timescale section data is updated with trimmed whitespace
    When a PUT request is made to update the implementation-timescales section for solution Sln1
        | ImplementationTimescales                                         |
        | "           A new implementation timescales description        " |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | ImplementationDetail                                   |
        | Sln1     | A new implementation timescales description            |
        | Sln2     | Another original implementation timescales description |
    And Last Updated has updated on the SolutionDetail for solution Sln1

@3670
Scenario: Solution not found
    Given a Solution Sln3 does not exist
    When a PUT request is made to update the implementation-timescales section for solution Sln3
        | ImplementationTimescales                    |
        | A new implementation timescales description |
    Then a response status of 404 is returned

@3670
Scenario: Service failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the implementation-timescales section for solution Sln1
        | ImplementationTimescales                    |
        | A new implementation timescales description |
    Then a response status of 500 is returned

@3670
Scenario: Solution id not present in request
    When a PUT request is made to update the implementation-timescales section with no solution id
        | ImplementationTimescales                    |
        | A new implementation timescales description |
    Then a response status of 400 is returned
