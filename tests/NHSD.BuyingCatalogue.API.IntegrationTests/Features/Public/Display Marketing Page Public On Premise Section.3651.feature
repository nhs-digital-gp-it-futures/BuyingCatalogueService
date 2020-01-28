Feature: Display Marketing Page Public On Premise Section
    As a Catalogue User
    I want to manage Marketing Page Information for the On Premise Section
    So that I can ensure the information is correct

Background:
	Given Suppliers exist
		| Id    | SupplierName |
		| Sup 1 | Supplier 1   |
	And Solutions exist
		| SolutionID | SolutionName | SupplierStatusId | SupplierId |
		| Sln1       | MedicOnline  | 1                | Sup 1      |
	And SolutionDetail exist
		| Solution | SummaryDescription            | FullDescription   | Hosting                                                                                                                                                                      |
		| Sln1     | A full online medicine system | Online medicine 1 | { "OnPremise": { "Summary": "Some summary", "Link": "www.somelink.com", "HostingModel": "A hosting model", "RequiresHscn": "This Solution requires a HSCN/N3 connection" } } |

@3651
Scenario:1. Get Solution Public for On Premise contains Hosting for all data
	When a GET request is made for solution public Sln1
	Then a successful response is returned
	And the solutions hosting-type-on-premise section is returned
	And the solutions hosting-type-on-premise.answers section contains summary with value Some summary
	And the solutions hosting-type-on-premise.answers section contains link with value www.somelink.com
	And the solutions hosting-type-on-premise.answers section contains hosting-model with value A hosting model
	And the solutions hosting-type-on-premise.answers section contains requires-hscn with value This Solution requires a HSCN/N3 connection
