Feature: Display Marketing Page Public Solution Integrations Section
    As a Supplier
    I want to manage Marketing Page Information for the Integrations Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierId |
        | Sln1       | MedicOnline    | Sup 1      |
        | Sln2       | TakeTheRedPill | Sup 1      |
    And solutions have the following details
        | SolutionId | IntegrationsUrl       |
        | Sln1       | Some integrations url |
    And framework solutions exist
        | SolutionId | IsFoundation | FrameworkId |
        | Sln1       | true         | NHSDGP001   |
        | Sln2       | false        | DFOCVC001   |

@3667
Scenario: Solution integrations section presented where Solution Detail exists
    When a GET request is made for solution public Sln1
    Then a successful response is returned
    And the response contains the following values
        | Section      | Field | Value                 |
        | integrations | link  | Some integrations url |

@3667
Scenario: Solution integrations section presented where no Solution Detail exists
    When a GET request is made for solution public Sln2
    Then a successful response is returned
    And the solutions integrations section is not returned

@3697
Scenario: Solution Integration section presented where Document exists
    Given a document named integration exists with solutionId Sln1
    When a GET request is made for solution public Sln1
    Then a successful response is returned
    And the response contains the following values
        | Section      | Field         | Value                 |
        | integrations | document-name | integration           |
        | integrations | link          | Some integrations url |

@3697
Scenario: Solution Integration section presented where Document API Fails
    Given the document api fails with solutionId Sln1
    When a GET request is made for solution public Sln1
    Then a successful response is returned
    And the response contains the following values
        | Section      | Field | Value                 |
        | integrations | link  | Some integrations url |
    And the solutions integration section does not contain answer documentName
