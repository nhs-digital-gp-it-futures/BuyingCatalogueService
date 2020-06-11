Feature: Display Marketing Page Dashboard Features Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Features
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
        | Sup 2 | Supplier 2   |
    And Solutions exist
        | SolutionId | SummaryDescription             | FullDescription     | AboutUrl | Features                          |
        | Sln1       | An full online medicine system | Online medicine 1   | UrlSln1  | [ "Appointments", "Prescribing" ] |
        | Sln2       | NULL                           | NULL                | NULL     | NULL                              |
        | Sln3       | Fully fledged GP system        | Fully fledged GP 12 | UrlSln3  | [ "Referrals", "Workflow" ]       |
                                                       
@1793
Scenario: 1. Sections presented where SolutionDetail exists
    When a GET request is made for solution dashboard Sln3
    Then a successful response is returned
    And the solution features section status is COMPLETE
    And the solution features section requirement is Optional
    
@1793
Scenario: 2. Sections not presented where no Solution Detail exists
    When a GET request is made for solution dashboard Sln2
    Then a successful response is returned
    And the solution features section status is INCOMPLETE
    And the solution features section requirement is Optional
