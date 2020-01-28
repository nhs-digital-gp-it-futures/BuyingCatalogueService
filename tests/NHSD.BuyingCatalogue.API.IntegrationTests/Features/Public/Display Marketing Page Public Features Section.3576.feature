Feature: Display Marketing Page Public Features Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Features
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
        | Sup 2 | Supplier 2   |
    And Solutions exist
        | SolutionID | SolutionName   | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | 1                | Sup 2      |
        | Sln3       | PracticeMgr    | 1                | Sup 2      |
    And SolutionDetail exist
        | Solution | AboutUrl | SummaryDescription             | Features                          |
        | Sln1     | UrlSln1  | An full online medicine system | [ "Appointments", "Prescribing" ] |
        | Sln3     | UrlSln3  | Eye opening experience         | [ "Referrals", "Workflow" ]       |

@1793
Scenario: 1. Sections presented where SolutionDetail exists
    When a GET request is made for solution public Sln3
    Then a successful response is returned
    And the solution solution-description section contains link of UrlSln3
    And the solution features section contains Features
        | Feature   |
        | Referrals |
        | Workflow  |
    
@1793
Scenario: 2. Sections not presented where no Solution Detail exists
    When a GET request is made for solution public Sln2
    Then a successful response is returned
    And the solution solution-description section does not contain link
    And the solution features section contains no features
