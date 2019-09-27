Feature: Display Marketing Page Form Features Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Features
    So that I can ensure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
        | Drs. Inc |
    And Solutions exist
        | SolutionID | SolutionName   | SummaryDescription             | OrganisationName |
        | Sln1       | MedicOnline    | An full online medicine system | GPs-R-Us         |
        | Sln2       | TakeTheRedPill | Eye opening experience         | Drs. Inc         |
        | Sln3       | PracticeMgr    | Fully fledged GP system        | Drs. Inc         |
    And MarketingDetail exist
        | Solution | AboutUrl | Features  |
        | Sln1     | UrlSln1  | Features1 |
        | Sln2     | UrlSln2  | Features2 |
        | Sln3     | UrlSln3  | Features3 |

@1793
Scenario: 1. Sections presented
    When a GET request is made for solution Sln3
    Then a successful response is returned
    And the solution contains MarketingData of Features3 
    And the solution contains AboutUrl of UrlSln3 
