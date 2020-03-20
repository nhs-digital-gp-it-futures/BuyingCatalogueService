Feature: Display Marketing Page Preview Hybrid HostingType Section
	As a Catalogue User
    I want to manage Marketing Page Information for the Hybrid HostingType Section
    So that I can ensure the information is correct

Background:
	Given Suppliers exist
		| Id    | SupplierName |
		| Sup 1 | Supplier 1   |
	And Solutions exist
		| SolutionId | SolutionName | SupplierStatusId | SupplierId |
		| Sln1       | MedicOnline  | 1                | Sup 1      |
	And SolutionDetail exist
		| Solution | Hosting                                                                                                                                                                              |
		| Sln1     | { "HybridHostingType": { "Summary": "Some summary", "Link": "www.somelink.com", "HostingModel": "A hosting model", "RequiresHscn": "This Solution requires a HSCN/N3 connection" } } |

@3644
Scenario:1. Get Solution Preview contains Hosting for all data regarding Hybrid HostingType
	When a GET request is made for solution preview Sln1
	Then a successful response is returned
	And the solutions hosting-type-hybrid section is returned
	And the response contains the following values
		| Section             | Field         | Value                                       |
		| hosting-type-hybrid | summary       | Some summary                                |
		| hosting-type-hybrid | link          | www.somelink.com                            |
		| hosting-type-hybrid | hosting-model | A hosting model                             |
		| hosting-type-hybrid | requires-hscn | This Solution requires a HSCN/N3 connection |
