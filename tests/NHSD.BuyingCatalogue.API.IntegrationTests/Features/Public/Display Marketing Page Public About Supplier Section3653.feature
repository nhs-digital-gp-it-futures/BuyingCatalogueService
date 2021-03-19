Feature: Display Marketing Page Public Solution About Supplier Section
    As a Supplier
    I want to manage Marketing Page Information for the About Supplier Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName | Summary            | SupplierUrl   |
        | Sup 1 | Supplier 1   |                    |               |
        | Sup 2 | Supplier 2   | NULL               | supplier-url2 |
        | Sup 3 | Supplier 3   | Supplier summary 3 | NULL          |
        | Sup 4 | Supplier 4   | Supplier summary 4 | supplier-url4 |
    And Solutions exist
        | SolutionId | SolutionName    | SupplierId |
        | Sln1       | MedicOnline     | Sup 1      |
        | Sln2       | TakeTheRedPill  | Sup 2      |
        | Sln3       | PracticeMgr     | Sup 3      |
        | Sln4       | AnotherSolution | Sup 4      |
    And framework solutions exist
        | SolutionId | IsFoundation | FrameworkId |
        | Sln1       | true         | NHSDGP001   |
        | Sln2       | false        | DFOCVC001   |
        | Sln3       | false        | NHSDGP001   |
        | Sln4       | false        | NHSDGP001   |

@3653
Scenario: About supplier section presented where description and link exists
    When a GET request is made for solution public Sln4
    Then a successful response is returned
    And the solutions about-supplier section is returned
    And the response contains the following values
        | Section        | Field       | Value              |
        | about-supplier | description | Supplier summary 4 |
        | about-supplier | link        | supplier-url4      |

@3653
Scenario: About supplier section presented where description exists
    When a GET request is made for solution public Sln3
    Then a successful response is returned
    And the solutions about-supplier section is returned
    And the response contains the following values
        | Section        | Field       | Value              |
        | about-supplier | description | Supplier summary 3 |
    And the solutions about-supplier section does not contain answer link

@3653
Scenario: About supplier section presented where link exists
    When a GET request is made for solution public Sln2
    Then a successful response is returned
    And the solutions about-supplier section is returned
    And the response contains the following values
        | Section        | Field       | Value              |
        | about-supplier | link        | supplier-url2      |
    And the solutions about-supplier section does not contain answer description

@3653
Scenario: About supplier section not presented where no about supplier exists
    When a GET request is made for solution public Sln1
    Then a successful response is returned
    And the solutions about-supplier section is not returned
