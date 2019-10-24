Feature:  Display Marketing Page Form SolutionDescription Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's SolutionDescription
    So that I can ensure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
        | Drs. Inc |
    And Solutions exist
        | SolutionID | SolutionName   | SummaryDescription             | OrganisationName | FullDescription     | SupplierStatusId |
        | Sln1       | MedicOnline    | An full online medicine system | GPs-R-Us         | Online medicine 1   | 1                |
        | Sln2       | TakeTheRedPill | Eye opening experience         | Drs. Inc         | Eye opening6        | 1                |
        | Sln3       | PracticeMgr    | Fully fledged GP system        | Drs. Inc         | Fully fledged GP 12 | 1                |
    And MarketingDetail exist
        | Solution | AboutUrl | Features                          |
        | Sln1     | UrlSln1  | [ "Appointments", "Prescribing" ] |
        | Sln3     | UrlSln3  | [ "Referrals", "Workflow" ]       |

@2724
Scenario: 1. SolutionDescription are retrieved for the solution
    When a GET request is made for solution-description for solution Sln1
    Then a successful response is returned
    And the solution link is UrlSln1
    And the solution summary is An full online medicine system
    And the solution description is Online medicine 1

@2724
Scenario: 2. SolutionDescription are retrieved for the solution where no marketing detail exists
    When a GET request is made for solution-description for solution Sln2
    Then a successful response is returned
    And the solution does not contain link
    And the solution summary is Eye opening experience
    And the solution description is Eye opening6 

@2726
Scenario: 4. Solution not found
    Given a Solution Sln4 does not exist
    When a GET request is made for solution-description for solution Sln4
    Then a response status of 404 is returned

@2726
Scenario: 5. Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for solution-description for solution Sln1
    Then a response status of 500 is returned

@2726
Scenario: 6. Solution id not present in request
    When a GET request is made for solution-description with no solution id
    Then a response status of 400 is returned
