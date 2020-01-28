Feature: Display Marketing Page Form Supplier Validation
    As a Supplier
    I want to Edit the Solution Suppliers Section
    So that I can make sure the information is validated

Background:
	Given Suppliers exist
		| Id    | SupplierName | OrganisationName | Summary      | SupplierUrl |
		| Sup 1 | Supplier 1   | GPs-R-Us         | Some Summary | www.url.com |
	And Solutions exist
		| SolutionID | SolutionName | OrganisationName | SupplierStatusId | SupplierId |
		| Sln1       | MedicOnline  | GPs-R-Us         | 1                | Sup 1      |

@3653
Scenario: 1. Summary is greater than max length (1000 characters)
	When a PUT request is made to update the about-supplier section for solution Sln1
		| Summary                      | SupplierUrl     |
		| A string with length of 1001 | www.Someurl.com |
	Then a response status of 400 is returned
	And the description field value is the validation failure maxLength
	And Suppliers exist
		| Id    | Summary      | SupplierUrl |
		| Sup 1 | Some Summary | www.url.com |

@3653
Scenario: 2. Supplier Url is greater than max length (1000 characters)
	When a PUT request is made to update the about-supplier section for solution Sln1
		| Summary      | SupplierUrl                  |
		| More Summary | A string with length of 1001 |
	Then a response status of 400 is returned
	And the link field value is the validation failure maxLength
	And Suppliers exist
		| Id    | Summary      | SupplierUrl |
		| Sup 1 | Some Summary | www.url.com |

@3653
Scenario: 3. Summary & Supplier Url is greater than max length (1000 characters)
	When a PUT request is made to update the about-supplier section for solution Sln1
		| Summary                      | SupplierUrl                  |
		| A string with length of 1001 | A string with length of 1001 |
	Then a response status of 400 is returned
	And the description field value is the validation failure maxLength
	And the link field value is the validation failure maxLength
	And Suppliers exist
		| Id    | Summary      | SupplierUrl |
		| Sup 1 | Some Summary | www.url.com |
