Feature: Display Marketing Page Dashboard Public Cloud Section
    As an Authority User
    I want to manage Marketing Page Information for the Public Cloud Hosting Type
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierId |
        | Sln1       | MedicOnline    | Sup 1      |
        | Sln2       | TakeTheRedPill | Sup 1      |

@3639
Scenario Outline: Public cloud section is optional and is reported complete if there is text in the public Cloud
    Given solutions have the following details
        | SolutionId | AboutUrl | SummaryDescription | FullDescription   | Hosting   |
        | Sln1       | UrlSln1  |                    | Online medicine 1 | <Hosting> |
    When a GET request is made for solution dashboard <SolutionId>
    Then a successful response is returned
    And the solution hosting-type-public-cloud section status is <Status>
    And the solution hosting-type-public-cloud section requirement is Optional

    Examples:
        | SolutionId | Status     | Hosting                                                                                                                                     |
        | Sln1       | INCOMPLETE | { }                                                                                                                                         |
        | Sln1       | INCOMPLETE |                                                                                                                                             |
        | Sln1       | INCOMPLETE | { "PublicCloud": null }                                                                                                                     |
        | Sln1       | COMPLETE   | { "PublicCloud": {"Summary": "Some summary" } }                                                                                             |
        | Sln1       | COMPLETE   | { "PublicCloud": {"Link": "Some url" } }                                                                                                    |
        | Sln1       | COMPLETE   | { "PublicCloud": {"RequiresHSCN": "Some connectivity" } }                                                                                   |
        | Sln1       | COMPLETE   | { "PublicCloud": { "Summary": "Some summary", "Link": "www.somelink.com", "RequiresHSCN": "This Solution requires a HSCN/N3 connection" } } |
        | Sln2       | INCOMPLETE |                                                                                                                                             |
