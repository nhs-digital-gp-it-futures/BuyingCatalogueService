Feature: Display Marketing Page Preview Public Cloud Section
	As a Catalogue User
    I want to manage Marketing Page Information for the Public Cloud Section
    So that I can ensure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
    And Suppliers exist
        | Id    | SupplierName | OrganisationName |
        | Sup 1 | Supplier 1   | GPs-R-Us         |
    And Solutions exist
        | SolutionID | SolutionName | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline  | GPs-R-Us         | 1                | Sup 1      |
    And SolutionDetail exist
        | Solution | SummaryDescription            | FullDescription   | Hosting                                                                                                                                     |
        | Sln1     | A full online medicine system | Online medicine 1 | { "PublicCloud": { "Summary": "Some summary", "Link": "www.somelink.com", "RequiresHSCN": "This Solution requires a HSCN/N3 connection" } } |

@3639
Scenario:1. Get Solution Public contains Hosting for all data
    When a GET request is made for solution public Sln1
    Then a successful response is returned
    And the solutions hosting-type-public-cloud section is returned
    And the solutions hosting-type-public-cloud.answers section contains summary with value Some summary
    And the solutions hosting-type-public-cloud.answers section contains link with value www.somelink.com
    And the solutions hosting-type-public-cloud.answers section contains requires-hscn with value This Solution requires a HSCN/N3 connection
