Feature: Display Marketing Page Dashboard Hybrid Hosting Type Section
    As an Authority User
    I want to manage Marketing Page Information for the Hybrid Hosting Type
    So that I can ensure the information is correct

Background:
	Given Suppliers exist
		| Id    | SupplierName |
		| Sup 1 | Supplier 1   |
	And Solutions exist
		| SolutionID | SolutionName   | OrganisationName | SupplierStatusId | SupplierId |
		| Sln1       | MedicOnline    | GPs-R-Us         | 1                | Sup 1      |
		| Sln2       | TakeTheRedPill | GPs-R-Us         | 1                | Sup 1      |

@3644
Scenario Outline: 1. Hybrid hosting type section is optional and is reported complete if there is text in the Hybrid Hosting Type
	Given SolutionDetail exist
		| Solution | AboutUrl | SummaryDescription | FullDescription   | Hosting   |
		| Sln1     | UrlSln1  |                    | Online medicine 1 | <Hosting> |
	When a GET request is made for solution dashboard <Solution>
	Then a successful response is returned
	And the solution hosting-type-hybrid section status is <Status>
	And the solution hosting-type-hybrid section requirement is Optional

	Examples:
		| Solution | Status     | Hosting                                                                                                                                                                                 |
		| Sln1     | INCOMPLETE | { }                                                                                                                                                                                     |
		| Sln1     | INCOMPLETE |                                                                                                                                                                                         |
		| Sln1     | INCOMPLETE | { "HybridHostingType": null }                                                                                                                                                           |
		| Sln1     | COMPLETE   | { "HybridHostingType": {"Summary": "Some summary" } }                                                                                                                                   |
		| Sln1     | COMPLETE   | { "HybridHostingType": {"Link": "Some url" } }                                                                                                                                          |
		| Sln1     | COMPLETE   | { "HybridHostingType": {"HostingModel": "Some hosting model" } }                                                                                                                        |
		| Sln1     | COMPLETE   | { "HybridHostingType": {"RequiresHSCN": "Some connectivity" } }                                                                                                                         |
		| Sln1     | COMPLETE   | { "HybridHostingType": { "Summary": "Some summary", "Link": "www.somelink.com", "HostingModel": "Some hosting model", "RequiresHSCN": "This Solution requires a HSCN/N3 connection" } } |
		| Sln2     | INCOMPLETE |                                                                                                                                                                                         |
