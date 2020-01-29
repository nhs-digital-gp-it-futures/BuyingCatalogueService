Feature: Display Marketing Page Dashboard On Premise Section
    As an Authority User
    I want to manage Marketing Page Information for the On Premise Hosting Type
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
	And Solutions exist
		| SolutionID | SolutionName   | SupplierStatusId | SupplierId |
		| Sln1       | MedicOnline    | 1                | Sup 1      |
		| Sln2       | TakeTheRedPill | 1                | Sup 1      |

@3651
Scenario Outline: 1. On premise section is optional and is reported complete if there is text in the On Premise
	Given SolutionDetail exist
		| Solution | AboutUrl | SummaryDescription | FullDescription   | Hosting   |
		| Sln1     | UrlSln1  |                    | Online medicine 1 | <Hosting> |
	When a GET request is made for solution dashboard <Solution>
	Then a successful response is returned
	And the solution hosting-type-on-premise section status is <Status>
	And the solution hosting-type-on-premise section requirement is Optional

	Examples:
		| Solution | Status     | Hosting                                                                                                                                                                         |
		| Sln1     | INCOMPLETE | { }                                                                                                                                                                             |
		| Sln1     | INCOMPLETE |                                                                                                                                                                                 |
		| Sln1     | INCOMPLETE | { "OnPremise": null }                                                                                                                                                           |
		| Sln1     | COMPLETE   | { "OnPremise": {"Summary": "Some summary" } }                                                                                                                                   |
		| Sln1     | COMPLETE   | { "OnPremise": {"Link": "Some url" } }                                                                                                                                          |
		| Sln1     | COMPLETE   | { "OnPremise": {"HostingModel": "Some hosting model" } }                                                                                                                        |
		| Sln1     | COMPLETE   | { "OnPremise": {"RequiresHSCN": "Some connectivity" } }                                                                                                                         |
		| Sln1     | COMPLETE   | { "OnPremise": { "Summary": "Some summary", "Link": "www.somelink.com", "HostingModel": "Some hosting model", "RequiresHSCN": "This Solution requires a HSCN/N3 connection" } } |
		| Sln2     | INCOMPLETE |                                                                                                                                                                                 |
