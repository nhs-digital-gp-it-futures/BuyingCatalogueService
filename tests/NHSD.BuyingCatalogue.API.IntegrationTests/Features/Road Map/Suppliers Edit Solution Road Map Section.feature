Feature: Suppliers Edit Solution Roadmap Section
    As a Supplier
    I want to Edit the Solution Roadmap Section
    So that I can make sure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierId |
        | Sln1       | MedicOnline    | Sup 1      |
        | Sln2       | TakeTheRedPill | Sup 1      |
    And solutions have the following details
        | SolutionId | RoadMap                          |
        | Sln1       | An original roadmap summary      |
        | Sln2       | Another original roadmap summary |

@3664
Scenario: Solution roadmap section data is updated
    When a PUT request is made to update the roadmap section for solution Sln1
        | Summary            |
        | A new full summary |
    Then a successful response is returned
    And solutions have the following details
        | SolutionId | RoadMap                          |
        | Sln1       | A new full summary               |
        | Sln2       | Another original roadmap summary |
    And Last Updated has been updated for solution Sln1

@3664
Scenario: Solution roadmap section data is updated with trimmed whitespace
    When a PUT request is made to update the roadmap section for solution Sln1
        | Summary                                 |
        | "           A new full summary        " |
    Then a successful response is returned
    And solutions have the following details
        | SolutionId | RoadMap                          |
        | Sln1       | A new full summary               |
        | Sln2       | Another original roadmap summary |
    And Last Updated has been updated for solution Sln1

@3664
Scenario: Solution not found
    Given a Solution Sln3 does not exist
    When a PUT request is made to update the roadmap section for solution Sln3
        | Summary            |
        | A new full summary |
    Then a response status of 404 is returned

@3664
Scenario: Service failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the roadmap section for solution Sln1
        | Summary            |
        | A new full summary |
    Then a response status of 500 is returned

@3664
Scenario: Solution id not present in request
    When a PUT request is made to update the roadmap section with no solution id
        | Summary            |
        | A new full summary |
    Then a response status of 400 is returned
