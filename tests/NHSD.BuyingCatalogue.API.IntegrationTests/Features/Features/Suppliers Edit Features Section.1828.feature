Feature: Suppliers Edit Features Section
    As a Supplier
    I want to Edit the Features Section
    So that I can make sure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
        | Drs. Inc |
    And Suppliers exist
        | Id    | SupplierName | OrganisationName |
        | Sup 1 | Supplier 1   | GPs-R-Us         |
        | Sup 2 | Supplier 2   | Drs. Inc         |
    And Solutions exist
        | SolutionID | SolutionName   | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | GPs-R-Us         | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | Drs. Inc         | 1                | Sup 2      |
        | Sln3       | PracticeMgr    | Drs. Inc         | 1                | Sup 2      |

@1828
Scenario: 1. Marketing Data is updated against the solution
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription     | AboutUrl | Features                          |
        | Sln1     | An full online medicine system | Online medicine 1   | UrlSln1  | [ "Appointments", "Prescribing" ] |
        | Sln2     | Eye opening experience         | Eye opening6        | UrlSln2  | [ "Workflow", "Referrals" ]       |
        | Sln3     | Fully fledged GP system        | Fully fledged GP 12 | UrlSln3  | [ "Dispensing" ]                  |
    When a PUT request is made to update solution Sln1 features section
        | Features                      |
        | Dispensing,Referrals,Workflow |
    Then a successful response is returned
    And Solutions exist
        | SolutionID | SolutionName   |
        | Sln1       | MedicOnline    |
        | Sln2       | TakeTheRedPill |
        | Sln3       | PracticeMgr    |
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription     | AboutUrl | Features                              |
        | Sln1     | An full online medicine system | Online medicine 1   | UrlSln1  | ["Dispensing","Referrals","Workflow"] |
        | Sln2     | Eye opening experience         | Eye opening6        | UrlSln2  | [ "Workflow", "Referrals" ]           |
        | Sln3     | Fully fledged GP system        | Fully fledged GP 12 | UrlSln3  | [ "Dispensing" ]                      |
    And Last Updated has updated on the SolutionDetail for solution Sln1

@1828
Scenario: 2. Marketing Data is added to the solution
	Given a SolutionDetail Sln1 does not exist
    When a PUT request is made to update solution Sln1 features section
        | Features                      |
        | Dispensing,Referrals,Workflow |
    Then a response status of 500 is returned

@1828
Scenario: 3. Solution not found
    Given a Solution Sln4 does not exist
    When a PUT request is made to update solution Sln4 features section
        | Features                      |
        | Dispensing,Referrals,Workflow |
    Then a response status of 404 is returned

@1828
Scenario: 4. Service failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update solution Sln1 features section
        | Features                      |
        | Dispensing,Referrals,Workflow |
    Then a response status of 500 is returned

@1828
Scenario: 4. Solution id not present in request
    When a PUT request is made to update solution features section with no solution id
        | Features                      |
        | Dispensing,Referrals,Workflow |
    Then a response status of 400 is returned
