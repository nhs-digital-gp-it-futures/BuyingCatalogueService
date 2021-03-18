Feature: Display Marketing Page Dashboard Implementation Timescales Section
    As an Authority User
    I want to manage Marketing Page Information for the Solution's Implementation Timescales
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierId |
        | Sln1       | MedicOnline    | Sup 1      |
        | Sln2       | TakeTheRedPill | Sup 1      |
    And Framework Solutions exist
        | SolutionId | IsFoundation | FrameworkId |
        | Sln1       | true         | NHSDGP001   |
        | Sln2       | false        | DFOCVC001   |

@3669
Scenario Outline: Implementation Timescales section is optional and is reported incomplete
    Given solutions have the following details
        | SolutionId | AboutUrl | SummaryDescription | FullDescription   | ImplementationDetail       |
        | Sln1       | UrlSln1  |                    | Online medicine 1 | <ImplementationTimescales> |
    When a GET request is made for solution dashboard <SolutionId>
    Then a successful response is returned
    And the solution implementation-timescales section status is <Status>
    And the solution implementation-timescales section requirement is Optional

    Examples:
        | SolutionId | Status     | ImplementationTimescales |
        | Sln1       | INCOMPLETE | ""                       |
        | Sln1       | INCOMPLETE | "   "                    |
        | Sln1       | INCOMPLETE |                          |
        | Sln1       | COMPLETE   | "integrations url"       |
        | Sln2       | INCOMPLETE |                          |
