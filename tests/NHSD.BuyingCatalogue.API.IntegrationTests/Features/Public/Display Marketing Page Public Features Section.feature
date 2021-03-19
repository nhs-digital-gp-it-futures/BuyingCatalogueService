Feature: Display Marketing Page Public Features Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Features
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierId |
        | Sln1       | TakeTheRedPill | Sup 1      |
        | Sln2       | PracticeMgr    | Sup 1      |
    And solutions have the following details
        | SolutionId | Features                    |
        | Sln1       | [ "Referrals", "Workflow" ] |
    And framework solutions exist
        | SolutionId | IsFoundation | FrameworkId |
        | Sln1       | true         | NHSDGP001   |
        | Sln2       | false        | DFOCVC001   |

@3576
Scenario: Sections presented where features exist
    When a GET request is made for solution public Sln1
    Then a successful response is returned
    And the response contains the following values
        | Section  | Field   | Value               |
        | features | listing | Referrals, Workflow |

@3576
Scenario: Sections not presented where no features exist
    When a GET request is made for solution public Sln2
    Then a successful response is returned
    And the solution features section contains no features
