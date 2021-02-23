Feature: Display Marketing Page Preview Features Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Features
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierStatusId | SupplierId |
        | Sln1       | TakeTheRedPill | 1                | Sup 1      |
        | Sln2       | PracticeMgr    | 1                | Sup 1      |
    And Solution have following details
        | SolutionId | Features                    |
        | Sln1       | [ "Referrals", "Workflow" ] |

@1793
Scenario: Sections presented where SolutionDetail exists
    When a GET request is made for solution preview Sln1
    Then a successful response is returned
    And the response contains the following values
        | Section  | Field   | Value               |
        | features | listing | Referrals, Workflow |

@1793
Scenario: Sections not presented where no Solution Detail exists
    When a GET request is made for solution preview Sln2
    Then a successful response is returned
    And the solution features section contains no features
