Feature: Display Marketing Page Preview Public Cloud Section
    As a Catalogue User
    I want to manage Marketing Page Information for the Public Cloud Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName | SupplierId |
        | Sln1       | MedicOnline  | Sup 1      |
    And solutions have the following details
        | SolutionId | Hosting                                                                                                                                     |
        | Sln1       | { "PublicCloud": { "Summary": "Some summary", "Link": "www.somelink.com", "RequiresHSCN": "This Solution requires a HSCN/N3 connection" } } |

@3639
Scenario: Get Solution Preview contains Hosting for all data
    When a GET request is made for solution preview Sln1
    Then a successful response is returned
    And the solutions hosting-type-public-cloud section is returned
    And the response contains the following values
        | Section                   | Field         | Value                                       |
        | hosting-type-public-cloud | summary       | Some summary                                |
        | hosting-type-public-cloud | link          | www.somelink.com                            |
        | hosting-type-public-cloud | requires-hscn | This Solution requires a HSCN/N3 connection |
