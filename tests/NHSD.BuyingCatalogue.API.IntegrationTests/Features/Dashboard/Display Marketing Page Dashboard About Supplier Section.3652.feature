Feature: Display Marketing Page Dashboard About Supplier Section
    As an Authority User
    I want to manage Marketing Page Information for the About Supplier Section
    So that I can ensure the information is correct

    Background:
        Given Organisations exist
            | Name     |
            | GPs-R-Us |
        And Suppliers exist
            | Id    | SupplierName | OrganisationName |
            | Sup 1 | Supplier 1   | GPs-R-Us         |
        And Solutions exist
            | SolutionID | SolutionName   | OrganisationName | SupplierStatusId | SupplierId |
            | Sln1       | MedicOnline    | GPs-R-Us         | 1                | Sup 1      |
        And SolutionDetail exist
            | Solution | AboutUrl| SummaryDescription      | FullDescription      | Features                          |
            | Sln1     | UrlSln1 |                         | Online medicine 1    | [ "Appointments", "Prescribing" ] |

    @3652
    Scenario: 1. About Supplier section is optional and is reported incomplete
        When a GET request is made for solution dashboard Sln1
        Then a successful response is returned
        And the solution about-supplier section status is INCOMPLETE
        And the solution about-supplier section requirement is Optional
                                                                                                                                                                                |


