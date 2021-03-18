Feature: Display Marketing Page Preview Solution Road Map Section
    As a Supplier
    I want to manage Marketing Page Information for the Road Map Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierId |
        | Sln1       | MedicOnline    | Sup 1      |
        | Sln2       | TakeTheRedPill | Sup 1      |
    And solutions have the following details
        | SolutionId | RoadMap          |
        | Sln1       | Some description |
    And Framework Solutions exist
        | SolutionId | IsFoundation | FrameworkId |
        | Sln1       | true         | NHSDGP001   |
        | Sln2       | true         | NHSDGP001   |

@3664
Scenario: Solution Road Map section presented where Solution RoadMap exists
    When a GET request is made for solution preview Sln1
    Then a successful response is returned
    And the response contains the following values
        | Section | Field   | Value            |
        | roadMap | summary | Some description |

@3664
Scenario: Solution Road Map section is not presented where no Solution RoadMap exists
    When a GET request is made for solution preview Sln2
    Then a successful response is returned
    And the solutions roadmap section is not returned

@3657
Scenario: Solution Road Map section presented where Document exists
    Given a document named roadmap exists with solutionId Sln1
    When a GET request is made for solution preview Sln1    
    Then a successful response is returned
    And the response contains the following values
        | Section | Field         | Value            |
        | roadMap | document-name | roadmap          |
        | roadMap | summary       | Some description |

@3657
Scenario: Solution Road Map section presented where Document API Fails
    Given the document api fails with solutionId Sln1
    When a GET request is made for solution preview Sln1    
    Then a successful response is returned
    And the response contains the following values
        | Section | Field        | Value            |		
        | roadMap | summary      | Some description |
    And the solutions roadmap section does not contain answer document-name
