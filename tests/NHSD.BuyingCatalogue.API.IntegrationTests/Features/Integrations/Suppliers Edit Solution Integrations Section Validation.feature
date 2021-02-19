Feature: Suppliers Edit Solution Integrations Section Validation
    As a Supplier
    I want to Edit the Solution Integrations Section
    So that I can make sure the information is validated

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | 1                | Sup 1      |
    And SolutionDetail exist
        | SolutionId | IntegrationsUrl              |
        | Sln1       | An original integrations url |

@3667
Scenario: Url is greater than max length (1000 characters)
    When a PUT request is made to update the integrations section for solution Sln1
        | IntegrationsUrl              |
        | A string with length of 1001 |
    Then a response status of 400 is returned
    And the link field value is the validation failure maxLength
    And SolutionDetail exist
        | SolutionId | IntegrationsUrl              |
        | Sln1       | An original integrations url |
