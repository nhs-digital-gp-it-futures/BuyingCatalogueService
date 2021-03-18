Feature: View Solution Regardless Of Published Status
    As a Supplier
    I want to view Solutions with any published status
    So that I can preview my solutions before publishing

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName      | LastUpdated | SupplierId | PublishedStatusId |
        | Sln1       | MedicOnline       | 19/11/2019  | Sup 1      | 1                 |
        | Sln2       | TakeTheRedPill    | 15/11/2019  | Sup 1      | 2                 |
        | Sln3       | TakeTheBluePill   | 15/11/2019  | Sup 1      | 3                 |
        | Sln4       | TakeThePurplePill | 15/11/2019  | Sup 1      | 4                 |
    And Framework Solutions exist
        | SolutionId | IsFoundation | FrameworkId |
        | Sln1       | true         | NHSDGP001   |
        | Sln2       | false        | DFOCVC001   |
        | Sln3       | false        | NHSDGP001   |
        | Sln4       | false        | NHSDGP001   |

@3776
Scenario: Solution section is presented where the solution is Drafted
    When a GET request is made for solution preview Sln1
    Then a successful response is returned
    And the string value of element name is MedicOnline

@3776
Scenario: Solution section is presented where the solution is Unpublished
    When a GET request is made for solution preview Sln2
    Then a successful response is returned
    And the string value of element name is TakeTheRedPill
    
@3776
Scenario: Solution section is presented where the solution is Published
    When a GET request is made for solution preview Sln3
    Then a successful response is returned
    And the string value of element name is TakeTheBluePill

@3776
Scenario: Solution section is presented where the solution is Withdrawn
    When a GET request is made for solution preview Sln4
    Then a successful response is returned
    And the string value of element name is TakeThePurplePill
