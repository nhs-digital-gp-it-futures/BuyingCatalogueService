Feature: Display Marketing Page Dashboard Private Cloud Section
    As an Authority User
    I want to manage Marketing Page Information for the Private Cloud Hosting Type
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierId |
        | Sln1       | MedicOnline    | Sup 1      |
        | Sln2       | TakeTheRedPill | Sup 1      |
    And framework solutions exist
        | SolutionId | IsFoundation | FrameworkId |
        | Sln1       | true         | NHSDGP001   |
        | Sln2       | false        | DFOCVC001   |

@3641
Scenario Outline: Private cloud section is optional and is reported complete if there is text in the Private Cloud
    Given solutions have the following details
        | SolutionId | AboutUrl | SummaryDescription | FullDescription   | Hosting   |
        | Sln1       | UrlSln1  |                    | Online medicine 1 | <Hosting> |
    When a GET request is made for solution dashboard <SolutionId>
    Then a successful response is returned
    And the solution hosting-type-private-cloud section status is <Status>
    And the solution hosting-type-private-cloud section requirement is Optional

    Examples:
        | SolutionId | Status     | Hosting                                                                                                                                                                            |
        | Sln1       | INCOMPLETE | { }                                                                                                                                                                                |
        | Sln1       | INCOMPLETE |                                                                                                                                                                                    |
        | Sln1       | INCOMPLETE | { "PrivateCloud": null }                                                                                                                                                           |
        | Sln1       | COMPLETE   | { "PrivateCloud": {"Summary": "Some summary" } }                                                                                                                                   |
        | Sln1       | COMPLETE   | { "PrivateCloud": {"Link": "Some url" } }                                                                                                                                          |
        | Sln1       | COMPLETE   | { "PrivateCloud": {"HostingModel": "Some hosting model" } }                                                                                                                        |
        | Sln1       | COMPLETE   | { "PrivateCloud": {"RequiresHSCN": "Some connectivity" } }                                                                                                                         |
        | Sln1       | COMPLETE   | { "PrivateCloud": { "Summary": "Some summary", "Link": "www.somelink.com", "HostingModel": "Some hosting model", "RequiresHSCN": "This Solution requires a HSCN/N3 connection" } } |
        | Sln2       | INCOMPLETE |                                                                                                                                                                                    |
