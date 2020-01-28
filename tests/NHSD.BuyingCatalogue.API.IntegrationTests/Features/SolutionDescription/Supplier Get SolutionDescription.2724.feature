Feature:  Display Marketing Page Form SolutionDescription Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's SolutionDescription
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
        | Sup 2 | Supplier 2   |
    And Solutions exist
        | SolutionID | SolutionName   | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | GPs-R-Us         | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | Drs. Inc         | 1                | Sup 2      |
        | Sln3       | PracticeMgr    | Drs. Inc         | 1                | Sup 2      |
    And SolutionDetail exist
        | Solution | AboutUrl | SummaryDescription             | FullDescription     | Features                          |
        | Sln1     | UrlSln1  | An full online medicine system | Online medicine 1   | [ "Appointments", "Prescribing" ] |
        | Sln3     | UrlSln3  | Eye opening experience         | Eye opening6        | [ "Referrals", "Workflow" ]       |

@2724
Scenario: 1. SolutionDescription are retrieved for the solution
    When a GET request is made for solution-description section for solution Sln1
    Then a successful response is returned
    And the solution link is UrlSln1
    And the solution summary is An full online medicine system
    And the solution description is Online medicine 1

@2724
Scenario: 2. SolutionDescription are retrieved empty for the solution where no solution detail exists
    When a GET request is made for solution-description section for solution Sln2
    Then a successful response is returned
    And the solution does not contain link
    And the solution does not contain summary
    And the solution does not contain description

@2726
Scenario: 4. Solution not found
    Given a Solution Sln4 does not exist
    When a GET request is made for solution-description section for solution Sln4
    Then a response status of 404 is returned

@2726
Scenario: 5. Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for solution-description section for solution Sln1
    Then a response status of 500 is returned

@2726
Scenario: 6. Solution id not present in request
    When a GET request is made for solution-description section with no solution id
    Then a response status of 400 is returned
