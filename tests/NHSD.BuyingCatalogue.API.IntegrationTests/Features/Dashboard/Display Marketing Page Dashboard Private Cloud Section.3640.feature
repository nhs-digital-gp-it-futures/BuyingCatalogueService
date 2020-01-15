Feature: Display Marketing Page Dashboard Private Cloud Section
    As an Authority User
    I want to manage Marketing Page Information for the Private Cloud Hosting Type
    So that I can ensure the information is correct

    Background:
        Given Organisations exist
            | Name     |
            | GPs-R-Us |
            | Drs. Inc |
        And Suppliers exist
            | Id    | SupplierName | OrganisationName |
            | Sup 1 | Supplier 1   | GPs-R-Us         |
            | Sup 2 | Supplier 2   | Drs. Inc         |
        And Solutions exist
            | SolutionID | SolutionName   | OrganisationName | SupplierStatusId | SupplierId |
            | Sln1       | MedicOnline    | GPs-R-Us         | 1                | Sup 1      |
            | Sln2       | TakeTheRedPill | Drs. Inc         | 1                | Sup 2      |
            | Sln3       | PracticeMgr    | Drs. Inc         | 1                | Sup 2      |
        And SolutionDetail exist
            | Solution | AboutUrl| SummaryDescription      | FullDescription      | Features                          |
            | Sln1     | UrlSln1 |                         | Online medicine 1    | [ "Appointments", "Prescribing" ] |
            | Sln3     | UrlSln3 | Eye opening experience  | Eye opening6         | [ "Referrals", "Workflow" ]       |

    @3624
    Scenario: 1. Private cloud section is optional and is reported incomplete
        When a GET request is made for solution dashboard Sln1
        Then a successful response is returned
        And the solution private-cloud section status is INCOMPLETE
        And the solution private-cloud section requirement is Optional
