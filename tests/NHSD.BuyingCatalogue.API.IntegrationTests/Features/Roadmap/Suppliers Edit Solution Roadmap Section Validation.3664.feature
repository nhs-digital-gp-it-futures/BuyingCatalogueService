Feature: Suppliers Edit Solution Roadmap Section Validation
    As a Supplier
    I want to Edit the Solution Roadmap Section
    So that I can make sure the information is validated

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
    And SolutionDetail exist
        | Solution | RoadMap                         |
        | Sln1     | An original roadmap description |
        
@3664
Scenario: 1. Description is greater than max length (1000 characters)
    When a PUT request is made to update the roadmap section for solution Sln1
        | Description                  |
        | A string with length of 1001 |
    Then a response status of 400 is returned
    And the description field value is the validation failure maxLength
    And SolutionDetail exist
        | Solution | RoadMap                         |
        | Sln1     | An original roadmap description |
