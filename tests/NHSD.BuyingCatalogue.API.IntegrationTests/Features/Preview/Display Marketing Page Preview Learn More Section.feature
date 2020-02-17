Feature: Display Marketing Page Preview Learn More Section
    As a supplier
    I want to manage marketing page information for the learn more section
    So I can ensure that the information is correct

Background:
	Given Suppliers exist
		| Id    | SupplierName |
		| Sup 1 | Supplier 1   |
	And Solutions exist
		| SolutionId | SolutionName   | SupplierStatusId | SupplierId |
		| Sln1       | MedicOnline    | 1                | Sup 1      |

Scenario: 1. Solution Learn More section is presented when the document exists
    Given a document named solution exists with solutionId Sln1
    When a GET request is made for solution preview Sln1
	Then a successful response is returned
	And the response contains the following values
		| Section    | Field         | Value            |
		| learn-more | document-name | solution         |

Scenario: 2. Solution Learn More section is not presented when no document exists
    When a GET request is made for solution preview Sln1
	Then a successful response is returned
	And the learn-more string does not exist

Scenario: 3. Solution Learn More section is not presented when the document API fails
    Given the document api fails with solutionId Sln1
    When a GET request is made for solution preview Sln1
	Then a successful response is returned
	And the learn-more string does not exist
