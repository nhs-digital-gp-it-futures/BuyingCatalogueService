Feature: Display Marketing Page Preview Solution Description Section
    As a Supplier
    I want to manage Marketing Page Information for the About Solution + Summary Description Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierStatusId | SupplierId |
        | Sln1       | PracticeMgr    | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | 1                | Sup 1      |
    And SolutionDetail exist
        | Solution | AboutUrl | SummaryDescription      | FullDescription     |
        | Sln1     | UrlSln3  | Fully fledged GP system | Fully fledged GP 12 |

@1848
Scenario: Solution description section presented where Solution Detail exists
    When a GET request is made for solution preview Sln1
    Then a successful response is returned
    And the response contains the following values
        | Section              | Field       | Value                   |
        | solution-description | summary     | Fully fledged GP system |
        | solution-description | description | Fully fledged GP 12     |
        | solution-description | link        | UrlSln3                 |

@1848
Scenario: Solution description section presented where no Solution Detail exists
    When a GET request is made for solution preview Sln2
    Then a successful response is returned
    And the solution solution-description section does not contain summary
    And the solution solution-description section does not contain description
    And the solution solution-description section does not contain link
