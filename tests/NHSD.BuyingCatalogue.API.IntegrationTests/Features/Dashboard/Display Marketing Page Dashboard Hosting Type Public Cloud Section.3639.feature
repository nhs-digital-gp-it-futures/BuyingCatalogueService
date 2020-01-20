Feature: Display Marketing Page Dashboard Public Cloud Section
    As an Authority User
    I want to manage Marketing Page Information for the Public Cloud Hosting Type
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
        | Sln2       | TakeTheRedPill | GPs-R-Us         | 1                | Sup 1      |
    And SolutionDetail exist
        | Solution | AboutUrl | SummaryDescription | FullDescription   | Hosting                                                                                                                                            |
        | Sln1     | UrlSln1  |                    | Online medicine 1 | { "PublicCloud": { "Summary": "Some summary", "URL": "www.somelink.com", "ConnectivityRequired": "This Solution requires a HSCN/N3 connection" } } |
        | Sln2     | UrlSln2  |                    | Online medicine 1 | { }                                                                                                                                                |

@3639
Scenario: 1. Public cloud section is optional and is reported complete
    When a GET request is made for solution dashboard Sln1
    Then a successful response is returned
    And the solution hosting-type-public-cloud section status is COMPLETE
    And the solution hosting-type-public-cloud section requirement is Optional

@3639
Scenario: 2. Public cloud section is optional and is reported incomplete
    When a GET request is made for solution dashboard Sln2
    Then a successful response is returned
    And the solution hosting-type-public-cloud section status is INCOMPLETE
    And the solution hosting-type-public-cloud section requirement is Optional
