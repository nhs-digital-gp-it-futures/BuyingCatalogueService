Feature: Suppliers Edit Solution Roadmap Section Validation
    As a Supplier
    I want to Edit the Solution Roadmap Section
    So that I can make sure the information is validated

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | 1                | Sup 1      |
    And solutions have the following details
        | SolutionId | RoadMap                     |
        | Sln1       | An original roadmap summary |
        
@3664
Scenario: Summary is greater than max length (1000 characters)
    When a PUT request is made to update the roadmap section for solution Sln1
        | Summary                      |
        | A string with length of 1001 |
    Then a response status of 400 is returned
    And the summary field value is the validation failure maxLength
    And solutions have the following details
        | SolutionId | RoadMap                     |
        | Sln1       | An original roadmap summary |
