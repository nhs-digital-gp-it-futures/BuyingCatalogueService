Feature: Display Marketing Page Preview Solution Description Section
	As a Supplier
    I want to manage Marketing Page Information for the About Solution + Summary Description Section
    So that I can ensure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
        | Drs. Inc |
    And Solutions exist
        | SolutionID | SolutionName   | SummaryDescription      | OrganisationName | FullDescription     | SupplierStatusId |
        | Sln1       | MedicOnline    |                         | GPs-R-Us         | Online medicine 1   | 1                |
        | Sln2       | TakeTheRedPill | Eye opening experience  | Drs. Inc         | Eye opening6        | 1                |
        | Sln3       | PracticeMgr    | Fully fledged GP system | Drs. Inc         | Fully fledged GP 12 | 1                |
    And MarketingDetail exist
        | Solution | AboutUrl | Features                          |
        | Sln1     | UrlSln1  | [ "Appointments", "Prescribing" ] |
        | Sln3     | UrlSln3  | [ "Referrals", "Workflow" ]       |

@1848
Scenario: 1. Solution description section presented where Marketing Detail exists
    When a GET request is made for solution preview Sln3
    Then a successful response is returned
    And the solution solution-description section contains SummaryDescription of 'Fully fledged GP system'
    And the solution solution-description section contains FullDescription of 'Fully fledged GP 12'
    And the solution solution-description section contains Link of UrlSln3
    And the solution features section contains Features
        | Feature   |
        | Referrals |
        | Workflow  |

@1848
Scenario: 2. Solution description section presented where no Marketing Detail exists
    When a GET request is made for solution preview Sln2
    Then a successful response is returned
    And the solution solution-description section contains SummaryDescription of 'Eye opening experience'
    And the solution solution-description section contains FullDescription of 'Eye opening6'
    And the solution solution-description section does not contain Link
    And the solution features section contains no features