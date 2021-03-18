Feature: Display Marketing Page Dashboard Roadmap Section
    As an Authority User
    I want to manage Marketing Page Information for the Solution's Roadmap
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

@3664
Scenario Outline: Roadmap section is optional and is reported complete if there is text
    Given solutions have the following details
        | SolutionId | AboutUrl | SummaryDescription | FullDescription   | RoadMap   |
        | Sln1       | UrlSln1  |                    | Online medicine 1 | <RoadMap> |
    When a GET request is made for solution dashboard <SolutionId>
    Then a successful response is returned
    And the solution roadmap section status is <Status>
    And the solution roadmap section requirement is Optional

    Examples:
        | SolutionId | Status     | RoadMap           |
        | Sln1       | INCOMPLETE | ""                |
        | Sln1       | INCOMPLETE | "   "             |
        | Sln1       | INCOMPLETE |                   |
        | Sln1       | COMPLETE   | "Roadmap summary" |
        | Sln2       | INCOMPLETE |                   |
