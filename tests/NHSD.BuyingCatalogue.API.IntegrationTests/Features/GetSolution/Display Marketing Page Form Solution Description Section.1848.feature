Feature: Display Marketing Page Form Solution Description Section
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
    When a GET request is made for solution Sln3
    Then a successful response is returned
    And the solution solution-description section contains SummaryDescription of 'Fully fledged GP system'
    And the solution solution-description section contains FullDescription of 'Fully fledged GP 12'
    And the solution solution-description section contains Link of UrlSln3
    And the solution features section contains Features
        | Feature   |
        | Referrals |
        | Workflow  |
    And the solution features section status is COMPLETE

@1848
Scenario: 2. Solution description section presented where no Marketing Detail exists
    When a GET request is made for solution Sln2
    Then a successful response is returned
    And the solution solution-description section contains SummaryDescription of 'Eye opening experience'
    And the solution solution-description section contains FullDescription of 'Eye opening6'
    And the solution solution-description section does not contain Link
    And the solution features section contains no features
    And the solution features section status is INCOMPLETE

@1848
Scenario Outline: 3. Solution description section is mandatory and is reported complete if there is text in the summary
    When a GET request is made for solution <Solution>
    Then a successful response is returned
    And the solution solution-description section contains SummaryDescription of '<SummaryDescription>'
    And the solution solution-description section status is <Status>
    And the solution solution-description section requirement is Mandatory
    And the solution solution-description section Mandatory field list is
        | Mandatory |
        | summary   |
Examples:
    | Solution | Status     | SummaryDescription      |
    | Sln1     | INCOMPLETE |                         |
    | Sln2     | COMPLETE   | Eye opening experience  |
    | Sln3     | COMPLETE   | Fully fledged GP system | 
