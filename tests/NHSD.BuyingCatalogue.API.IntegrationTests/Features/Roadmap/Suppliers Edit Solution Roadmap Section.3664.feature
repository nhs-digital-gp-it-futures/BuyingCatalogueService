Feature: Suppliers Edit Solution Roadmap Section
    As a Supplier
    I want to Edit the Solution Roadmap Section
    So that I can make sure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionID | SolutionName   | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | GPs-R-Us         | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | GPs-R-Us         | 1                | Sup 1      |
        | Sln3       | PracticeMgr    | GPs-R-Us         | 1                | Sup 1      |
    And SolutionDetail exist
        | Solution | RoadMap                          |
        | Sln1     | An original roadmap summary      |
        | Sln2     | Another original roadmap summary |

@3664
Scenario: 1. Solution roadmap section data is updated
    When a PUT request is made to update the roadmap section for solution Sln1
        | Summary            |
        | A new full summary |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | RoadMap                          |
        | Sln1     | A new full summary               |
        | Sln2     | Another original roadmap summary |
    And Last Updated has updated on the SolutionDetail for solution Sln1

@3664
Scenario: 2. Solution roadmap section data is updated with trimmed whitespace
    When a PUT request is made to update the roadmap section for solution Sln1
        | Summary                                 |
        | "           A new full summary        " |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | RoadMap                          |
        | Sln1     | A new full summary               |
        | Sln2     | Another original roadmap summary |
    And Last Updated has updated on the SolutionDetail for solution Sln1
    
@3664
Scenario: 3. Solution roadmap section data is not created on update, fail fast in this case
    Given a SolutionDetail Sln3 does not exist
    When a PUT request is made to update the roadmap section for solution Sln3
        | Summary            |
        | A new full summary |
    Then a response status of 500 is returned

@3664
Scenario: 4. Solution not found
    Given a Solution Sln4 does not exist
    When a PUT request is made to update the roadmap section for solution Sln4
        | Summary            |
        | A new full summary |
    Then a response status of 404 is returned
    
@3664
Scenario: 5. Service failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the roadmap section for solution Sln1
        | Summary            |
        | A new full summary |
    Then a response status of 500 is returned
    
@3664
Scenario: 6. Solution id not present in request
    When a PUT request is made to update the roadmap section with no solution id
        | Summary            |
        | A new full summary |
    Then a response status of 400 is returned
