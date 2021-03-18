Feature: Display Marketing Page Preview Learn More Section
    As a supplier
    I want to manage marketing page information for the learn more section
    So I can ensure that the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName | SupplierId |
        | Sln1       | MedicOnline  | Sup 1      |
    And Framework Solutions exist
        | SolutionId | IsFoundation | FrameworkId |
        | Sln1       | true         | NHSDGP001   |

Scenario: Solution Learn More section is presented when the document exists
    Given a document named solution exists with solutionId Sln1
    When a GET request is made for solution preview Sln1
    Then a successful response is returned
    And the response contains the following values
        | Section    | Field         | Value            |
        | learn-more | document-name | solution         |

Scenario: Solution Learn More section is not presented when no document exists
    When a GET request is made for solution preview Sln1
    Then a successful response is returned
    And the learn-more string does not exist

Scenario: Solution Learn More section is not presented when the document API fails
    Given the document api fails with solutionId Sln1
    When a GET request is made for solution preview Sln1
    Then a successful response is returned
    And the learn-more string does not exist
