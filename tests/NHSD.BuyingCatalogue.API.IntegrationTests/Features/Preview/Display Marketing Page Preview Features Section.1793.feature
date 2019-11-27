Feature: Display Marketing Page Preview Features Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Features
    So that I can ensure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
        | Drs. Inc |
    And Suppliers exist
        | Id    | OrganisationName |
        | Sup 1 | GPs-R-Us         |
        | Sup 2 | Drs. Inc         |
    And Solutions exist
        | SolutionID | SolutionName   | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | GPs-R-Us         | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | Drs. Inc         | 1                | Sup 2      |
        | Sln3       | PracticeMgr    | Drs. Inc         | 1                | Sup 2      |
    And SolutionDetail exist
        | Solution | AboutUrl | SummaryDescription             | Features                          |
        | Sln1     | UrlSln1  | An full online medicine system | [ "Appointments", "Prescribing" ] |
        | Sln3     | UrlSln3  | Eye opening experience         | [ "Referrals", "Workflow" ]       |

@1793
Scenario: 1. Sections presented where SolutionDetail exists
    When a GET request is made for solution preview Sln3
    Then a successful response is returned
    And the solution solution-description section contains link of UrlSln3
    And the solution features section contains Features
        | Feature   |
        | Referrals |
        | Workflow  |
    
@1793
Scenario: 2. Sections not presented where no Solution Detail exists
    When a GET request is made for solution preview Sln2
    Then a successful response is returned
    And the solution solution-description section does not contain link
    And the solution features section contains no features
