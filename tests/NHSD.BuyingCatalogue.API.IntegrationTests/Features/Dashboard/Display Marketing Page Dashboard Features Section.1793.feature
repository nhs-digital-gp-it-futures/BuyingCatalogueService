Feature: Display Marketing Page Dashboard Features Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Features
    So that I can ensure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
        | Drs. Inc |
    And Solutions exist
        | SolutionID | SolutionName   | SummaryDescription             | OrganisationName | SupplierStatusId |
        | Sln1       | MedicOnline    | An full online medicine system | GPs-R-Us         | 1                |
        | Sln2       | TakeTheRedPill | Eye opening experience         | Drs. Inc         | 1                |
        | Sln3       | PracticeMgr    | Fully fledged GP system        | Drs. Inc         | 1                |
    And MarketingDetail exist
        | Solution | AboutUrl | Features                          |
        | Sln1     | UrlSln1  | [ "Appointments", "Prescribing" ] |
        | Sln3     | UrlSln3  | [ "Referrals", "Workflow" ]       |

@1793
Scenario: 1. Sections presented where MarketingDetail exists
    When a GET request is made for solution dashboard Sln3
    Then a successful response is returned
    And the solution features section status is COMPLETE
    And the solution features section requirement is Optional
    
@1793
Scenario: 2. Sections not presented where no Marketing Detail exists
    When a GET request is made for solution dashboard Sln2
    Then a successful response is returned
    And the solution features section status is INCOMPLETE
    And the solution features section requirement is Optional
