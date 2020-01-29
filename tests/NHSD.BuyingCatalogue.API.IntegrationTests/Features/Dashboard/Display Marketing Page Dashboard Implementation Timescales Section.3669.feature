Feature: Display Marketing Page Dashboard Implementation Timescales Section
    As an Authority User
    I want to manage Marketing Page Information for the Solution's Implementation Timescales
    So that I can ensure the information is correct
    Background:
        Given Suppliers exist
            | Id    | SupplierName |
            | Sup 1 | Supplier 1   |
        And Solutions exist
            | SolutionID | SolutionName   | SupplierStatusId | SupplierId |
            | Sln1       | MedicOnline    | 1                | Sup 1      |
            | Sln2       | TakeTheRedPill | 1                | Sup 1      |

    @3669
    Scenario Outline: 1. Implementation Timescales section is optional and is reported incomplete
        Given SolutionDetail exist
            | Solution | AboutUrl | SummaryDescription | FullDescription   | ImplementationTimescales   |
            | Sln1     | UrlSln1  |                    | Online medicine 1 | <ImplementationTimescales> |
        When a GET request is made for solution dashboard <Solution>
        Then a successful response is returned
        And the solution implementation-timescales section status is <Status>
        And the solution implementation-timescales section requirement is Optional

        Examples:
            | Solution | Status     | ImplementationTimescales |
            | Sln1     | INCOMPLETE | ""                       |
            | Sln1     | INCOMPLETE | "   "                    |
            | Sln1     | INCOMPLETE |                          |
