Feature: Display Marketing Page Preview Private Cloud Section
    As a Catalogue User
    I want to manage Marketing Page Information for the Private Cloud Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName | SupplierId |
        | Sln1       | MedicOnline  | Sup 1      |
    And solutions have the following details
        | SolutionId | SummaryDescription            | FullDescription   | Hosting                                                                                                                                                                         |
        | Sln1       | A full online medicine system | Online medicine 1 | { "PrivateCloud": { "Summary": "Some summary", "Link": "www.somelink.com", "HostingModel": "A hosting model", "RequiresHscn": "This Solution requires a HSCN/N3 connection" } } |
    And framework solutions exist
        | SolutionId | IsFoundation | FrameworkId |
        | Sln1       | true         | NHSDGP001   |

@3641
Scenario: Get Solution Preview contains Hosting for all data
    When a GET request is made for solution preview Sln1
    Then a successful response is returned
    And the solutions hosting-type-private-cloud section is returned
    And the response contains the following values
        | Section                    | Field         | Value                                       |
        | hosting-type-private-cloud | summary       | Some summary                                |
        | hosting-type-private-cloud | link          | www.somelink.com                            |
        | hosting-type-private-cloud | hosting-model | A hosting model                             |
        | hosting-type-private-cloud | requires-hscn | This Solution requires a HSCN/N3 connection |
