Feature: View Solution Only When Published
    As a Public User
    I want to view Solutions which are set to Published
    So that I only view Solutions which are Published

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName      | LastUpdated | SupplierStatusId | SupplierId | PublishedStatusId |
        | Sln1       | MedicOnline       | 19/11/2019  | 1                | Sup 1      | 1                 |
        | Sln2       | TakeTheRedPill    | 15/11/2019  | 1                | Sup 1      | 2                 |
        | Sln3       | TakeTheBluePill   | 15/11/2019  | 1                | Sup 1      | 3                 |
        | Sln4       | TakeThePurplePill | 15/11/2019  | 1                | Sup 1      | 4                 |
        
@3776
Scenario: 1. Solution section is not presented where the solution is Drafted
When a GET request is made for solution public Sln1
Then a response status of 404 is returned

@3776
Scenario: 2. Solution section is not presented where the solution is Unpublished
When a GET request is made for solution public Sln2
Then a response status of 404 is returned
    
@3776
Scenario: 3. Solution section is presented where the solution is Published
When a GET request is made for solution public Sln3
Then a successful response is returned
And the string value of element name is TakeTheBluePill

@3776
Scenario: 4. Solution section is not presented where the solution is Withdrawn
When a GET request is made for solution public Sln4
Then a response status of 404 is returned
