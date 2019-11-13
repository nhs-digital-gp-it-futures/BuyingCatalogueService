Feature: Display Marketing Page Dashboard Solution Description Section
	As a Supplier
    I want to manage Marketing Page Information for the About Solution + Summary Description Section
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
        | Solution | AboutUrl| SummaryDescription      | FullDescription      | Features                          |
        | Sln1     | UrlSln1 |                         | Online medicine 1    | [ "Appointments", "Prescribing" ] |
        | Sln3     | UrlSln3 | Eye opening experience  | Eye opening6         | [ "Referrals", "Workflow" ]       |

@1848
Scenario Outline: 3. Solution description section is mandatory and is reported complete if there is text in the summary
    When a GET request is made for solution dashboard <Solution>
    Then a successful response is returned
    And the solution solution-description section status is <Status>
    And the solution solution-description section requirement is Mandatory
Examples:
    | Solution | Status     | SummaryDescription      |
    | Sln1     | INCOMPLETE |                         |
    | Sln2     | INCOMPLETE |                         |
    | Sln3     | COMPLETE   | Fully fledged GP system |
