Feature: View Solution Regardless Of Published Status
    As a Supplier
    I want to view Solutions with any published status
    So that I can preview my solutions before publishing

Background:
    And Organisations exist
        | Name     |
        | GPs-R-Us |
    And Suppliers exist
        | Id    | OrganisationName |
        | Sup 1 | GPs-R-Us         |
    And Solutions exist
        | SolutionID | SolutionName      | OrganisationName | LastUpdated | SupplierStatusId | SupplierId | PublishedStatusId |
        | Sln1       | MedicOnline       | GPs-R-Us         | 19/11/2019  | 1                | Sup 1      | 1                 |
        | Sln2       | TakeTheRedPill    | GPs-R-Us         | 15/11/2019  | 1                | Sup 1      | 2                 |
        | Sln3       | TakeTheBluePill   | GPs-R-Us         | 15/11/2019  | 1                | Sup 1      | 3                 |
        | Sln4       | TakeThePurplePill | GPs-R-Us         | 15/11/2019  | 1                | Sup 1      | 4                 |

@3776
Scenario: 1. Solution section is presented where the solution is Drafted
    When a GET request is made for solution preview Sln1
    Then a successful response is returned
    And the string value of element name is MedicOnline

@3776
Scenario: 2. Solution section is presented where the solution is Unpublished
    When a GET request is made for solution preview Sln2
    Then a successful response is returned
    And the string value of element name is TakeTheRedPill
    
@3776
Scenario: 3. Solution section is presented where the solution is Published
    When a GET request is made for solution preview Sln3
    Then a successful response is returned
    And the string value of element name is TakeTheBluePill

@3776
Scenario: 4. Solution section is presented where the solution is Withdrawn
    When a GET request is made for solution preview Sln4
    Then a successful response is returned
    And the string value of element name is TakeThePurplePill
