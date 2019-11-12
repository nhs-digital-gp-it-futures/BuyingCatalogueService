Feature: Display Marketing Page Preview Features Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Features
    So that I can ensure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
        | Drs. Inc |
    And Solutions exist
        | SolutionID | SolutionName   | OrganisationName | SupplierStatusId |
        | Sln1       | MedicOnline    | GPs-R-Us         | 1                |
        | Sln2       | TakeTheRedPill | Drs. Inc         | 1                |
        | Sln3       | PracticeMgr    | Drs. Inc         | 1                |
    And MarketingDetail exist
        | Solution | AboutUrl | SummaryDescription             | Features                          |
        | Sln1     | UrlSln1  | An full online medicine system | [ "Appointments", "Prescribing" ] |
        | Sln3     | UrlSln3  | Eye opening experience         | [ "Referrals", "Workflow" ]       |

@1793
Scenario: 1. Sections presented where MarketingDetail exists
    When a GET request is made for solution preview Sln3
    Then a successful response is returned
    And the solution solution-description section contains Link of UrlSln3
    And the solution features section contains Features
        | Feature   |
        | Referrals |
        | Workflow  |
    
@1793
Scenario: 2. Sections not presented where no Marketing Detail exists
    When a GET request is made for solution preview Sln2
    Then a successful response is returned
    And the solution solution-description section does not contain Link
    And the solution features section contains no features
