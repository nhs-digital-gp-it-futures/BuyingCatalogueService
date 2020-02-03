Feature: Display Marketing Page Preview Solution Road Map Section
	As a Supplier
    I want to manage Marketing Page Information for the Road Map Section
    So that I can ensure the information is correct

Background:
	Given Suppliers exist
		| Id    | SupplierName |
		| Sup 1 | Supplier 1   |
	And Solutions exist
		| SolutionID | SolutionName   | SupplierStatusId | SupplierId |
		| Sln1       | MedicOnline    | 1                | Sup 1      |
		| Sln2       | TakeTheRedPill | 1                | Sup 1      |
	And SolutionDetail exist
		| Solution | RoadMap          |
		| Sln1     | Some description |

@3664
Scenario: 1. Solution Road Map section presented where Solution Detail exists
	When a GET request is made for solution preview Sln1
	Then a successful response is returned
	And the response contains the following values
		| Section | Field   | Value            |
		| roadmap | summary | Some description |

@3664
Scenario: 2. Solution Road Map section presented where no Solution Detail exists
	When a GET request is made for solution preview Sln2
	Then a successful response is returned
	And the solutions roadmap section is not returned

@3657
Scenario: 3. Solution Road Map section presented where Document exists
    Given a document named roadmap exists with solutionId Sln1
    When a GET request is made for solution preview Sln1    
	Then a successful response is returned
	And the response contains the following values
		| Section | Field        | Value            |
		| roadmap | documentName | roadmap          |
		| roadmap | summary      | Some description |

@3657
Scenario: 4. Solution Road Map section presented where Document API Fails
    Given the document api fails with solutionId Sln1
    When a GET request is made for solution preview Sln1    
	Then a successful response is returned
	And the response contains the following values
		| Section | Field        | Value            |		
		| roadmap | summary      | Some description |
    And the solutions roadmap section does not contain answer documentName
