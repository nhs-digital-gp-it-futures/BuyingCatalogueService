Feature: Display Marketing Page Dashboard Public Cloud Section
    As an Authority User
    I want to manage Marketing Page Information for the Public Cloud Hosting Type
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
	And Solutions exist
		| SolutionID | SolutionName   | OrganisationName | SupplierStatusId | SupplierId |
		| Sln1       | MedicOnline    | GPs-R-Us         | 1                | Sup 1      |
		| Sln2       | TakeTheRedPill | GPs-R-Us         | 1                | Sup 1      |

@3639
Scenario Outline: 1. Public cloud section is optional and is reported complete if there is text in the public Cloud
	Given SolutionDetail exist
		| Solution | AboutUrl | SummaryDescription | FullDescription   | Hosting   |
		| Sln1     | UrlSln1  |                    | Online medicine 1 | <Hosting> |
	When a GET request is made for solution dashboard <Solution>
	Then a successful response is returned
	And the solution hosting-type-public-cloud section status is <Status>
	And the solution hosting-type-public-cloud section requirement is Optional

	Examples:
		| Solution | Status     | Hosting                                                                                                                                     |
		| Sln1     | INCOMPLETE | { }                                                                                                                                         |
		| Sln1     | INCOMPLETE |                                                                                                                                             |
		| Sln1     | INCOMPLETE | { "PublicCloud": null }                                                                                                                     |
		| Sln1     | COMPLETE   | { "PublicCloud": {"Summary": "Some summary" } }                                                                                             |
		| Sln1     | COMPLETE   | { "PublicCloud": {"Link": "Some url" } }                                                                                                    |
		| Sln1     | COMPLETE   | { "PublicCloud": {"RequiresHSCN": "Some connectivity" } }                                                                                   |
		| Sln1     | COMPLETE   | { "PublicCloud": { "Summary": "Some summary", "Link": "www.somelink.com", "RequiresHSCN": "This Solution requires a HSCN/N3 connection" } } |
		| Sln2     | INCOMPLETE |                                                                                                                                             |
