Feature: Display Marketing Page Dashboard Integrations Section
    As an Authority User
    I want to manage Marketing Page Information for the Solution's Integrations
    So that I can ensure the information is correct
    Background:
        Given Suppliers exist
            | Id    | SupplierName |
            | Sup 1 | Supplier 1   |
        And Solutions exist
            | SolutionID | SolutionName   | SupplierStatusId | SupplierId |
            | Sln1       | MedicOnline    | 1                | Sup 1      |
            | Sln2       | TakeTheRedPill | 1                | Sup 1      |

    @3667
    Scenario Outline: 1. Integrations section is optional and is reported complete if there is text
        Given SolutionDetail exist
            | Solution | AboutUrl | SummaryDescription | FullDescription   | IntegrationsUrl   |
            | Sln1     | UrlSln1  |                    | Online medicine 1 | <IntegrationsUrl> |
        When a GET request is made for solution dashboard <Solution>
        Then a successful response is returned
        And the solution integrations section status is <Status>
        And the solution integrations section requirement is Optional

        Examples:
            | Solution | Status     | IntegrationsUrl    |
            | Sln1     | INCOMPLETE | ""                 |
            | Sln1     | INCOMPLETE | "   "              |
            | Sln1     | INCOMPLETE |                    |
            | Sln1     | COMPLETE   | "integrations url" |
            | Sln2     | INCOMPLETE |                    |
