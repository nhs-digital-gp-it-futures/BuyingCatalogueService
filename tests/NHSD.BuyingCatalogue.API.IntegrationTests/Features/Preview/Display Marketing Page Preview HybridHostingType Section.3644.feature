Feature: Display Marketing Page Preview Hybrid HostingType Section
	As a Catalogue User
    I want to manage Marketing Page Information for the Hybrid HostingType Section
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
        | Solution | SummaryDescription            | FullDescription   | Hosting                                                                                                                                                                              |
        | Sln1     | A full online medicine system | Online medicine 1 | { "HybridHostingType": { "Summary": "Some summary", "Link": "www.somelink.com", "HostingModel": "A hosting model", "RequiresHscn": "This Solution requires a HSCN/N3 connection" } } |

@3644
Scenario:1. Get Solution Preview contains Hosting for all data regarding Hybrid HostingType
    When a GET request is made for solution preview Sln1
    Then a successful response is returned
    And the solutions hosting-type-hybrid section is returned
    And the solutions hosting-type-hybrid.answers section contains summary with value Some summary
    And the solutions hosting-type-hybrid.answers section contains link with value www.somelink.com
    And the solutions hosting-type-hybrid.answers section contains hosting-model with value A hosting model
    And the solutions hosting-type-hybrid.answers section contains requires-hscn with value This Solution requires a HSCN/N3 connection
