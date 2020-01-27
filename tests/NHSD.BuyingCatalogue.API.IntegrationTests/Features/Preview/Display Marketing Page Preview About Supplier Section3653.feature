Feature: Display Marketing Page Preview Solution About Supplier Section
    As a Supplier
    I want to manage Marketing Page Information for the About Supplier Section
    So that I can ensure the information is correct

Background:
	Given Organisations exist
		| Name     |
		| GPs-R-Us |
		| Drs. Inc |
	And Suppliers exist
		| Id    | SupplierName | OrganisationName | Summary            | SupplierUrl   |
		| Sup 1 | Supplier 1   | GPs-R-Us         |                    |               |
		| Sup 2 | Supplier 2   | Drs. Inc         | NULL               | supplier-url2 |
		| Sup 3 | Supplier 3   | Drs. Inc         | Supplier summary 3 | NULL          |
		| Sup 4 | Supplier 4   | Drs. Inc         | Supplier summary 4 | supplier-url4 |
	And Solutions exist
		| SolutionID | SolutionName    | OrganisationName | SupplierStatusId | SupplierId |
		| Sln1       | MedicOnline     | GPs-R-Us         | 1                | Sup 1      |
		| Sln2       | TakeTheRedPill  | Drs. Inc         | 1                | Sup 2      |
		| Sln3       | PracticeMgr     | Drs. Inc         | 1                | Sup 3      |
		| Sln4       | AnotherSolution | Drs. Inc         | 1                | Sup 4      |

@3653
Scenario: 1. About supplier section presented where description and link exists
	When a GET request is made for solution preview Sln4
	Then a successful response is returned
    And the solutions about-supplier section is returned
	And the solutions about-supplier.answers section contains description with value Supplier summary 4
	And the solutions about-supplier.answers section contains link with value supplier-url4

@3653
Scenario: 2. About supplier section presented where description exists
	When a GET request is made for solution preview Sln3
	Then a successful response is returned
    And the solutions about-supplier section is returned
	And the solutions about-supplier.answers section contains description with value Supplier summary 3
	And the solutions about-supplier section does not contain answer link

@3653
Scenario: 3. About supplier section presented where link exists
	When a GET request is made for solution preview Sln2
	Then a successful response is returned
    And the solutions about-supplier section is returned
	And the solutions about-supplier.answers section contains link with value supplier-url2
	And the solutions about-supplier section does not contain answer description

@3653
Scenario: 4. About supplier section not presented where no about supplier exists
	When a GET request is made for solution preview Sln1
	Then a successful response is returned
	And the solutions about-supplier section is not returned
