Feature: Display Marketing Page Preview On Premise Section
    As a Catalogue User
    I want to manage Marketing Page Information for the On Premise Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline  | 1                | Sup 1      |
    And Solution have following details
        | SolutionId | Hosting                                                                                                                                                                      |
        | Sln1       | { "OnPremise": { "Summary": "Some summary", "Link": "www.somelink.com", "HostingModel": "A hosting model", "RequiresHscn": "This Solution requires a HSCN/N3 connection" } } |

@3651
Scenario: Get Solution Preview for On Premise contains Hosting for all data
    When a GET request is made for solution preview Sln1
    Then a successful response is returned
    And the solutions hosting-type-on-premise section is returned
    And the response contains the following values
        | Section                 | Field         | Value                                       |
        | hosting-type-on-premise | summary       | Some summary                                |
        | hosting-type-on-premise | link          | www.somelink.com                            |
        | hosting-type-on-premise | hosting-model | A hosting model                             |
        | hosting-type-on-premise | requires-hscn | This Solution requires a HSCN/N3 connection |
