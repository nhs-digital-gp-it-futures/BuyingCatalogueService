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
        | SolutionID | SolutionName   | OrganisationName | SupplierStatusId |
        | Sln1       | MedicOnline    | GPs-R-Us         | 1                |
        | Sln2       | TakeTheRedPill | Drs. Inc         | 1                |
        | Sln3       | PracticeMgr    | Drs. Inc         | 1                |
    And MarketingDetail exist
        | Solution | SummaryDescription             | FullDescription     | AboutUrl | Features                          |
        | Sln1     | An full online medicine system | Online medicine 1   | UrlSln1  | [ "Appointments", "Prescribing" ] |
        | Sln3     | Fully fledged GP system        | Fully fledged GP 12 | UrlSln3  | [ "Referrals", "Workflow" ]       |
                                                       
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
