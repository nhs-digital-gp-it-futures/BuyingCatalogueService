Feature: Suppliers Edit Features Section
    As a Supplier
    I want to Edit the Features Section
    So that I can make sure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
        | Sup 2 | Supplier 2   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierId |
        | Sln1       | MedicOnline    | Sup 1      |
        | Sln2       | TakeTheRedPill | Sup 2      |
        | Sln3       | PracticeMgr    | Sup 2      |
    And framework solutions exist
        | SolutionId | IsFoundation | FrameworkId |
        | Sln1       | true         | NHSDGP001   |
        | Sln2       | false        | DFOCVC001   |
        | Sln3       | false        | NHSDGP001   |

@1828
Scenario: Marketing Data is updated against the solution
    Given solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription     | AboutUrl | Features                          |
        | Sln1       | An full online medicine system | Online medicine 1   | UrlSln1  | [ "Appointments", "Prescribing" ] |
        | Sln2       | Eye opening experience         | Eye opening6        | UrlSln2  | [ "Workflow", "Referrals" ]       |
        | Sln3       | Fully fledged GP system        | Fully fledged GP 12 | UrlSln3  | [ "Dispensing" ]                  |
    When a PUT request is made to update solution Sln1 features section
        | Features                      |
        | Dispensing,Referrals,Workflow |
    Then a successful response is returned
    And Solutions exist
        | SolutionId | SolutionName   |
        | Sln1       | MedicOnline    |
        | Sln2       | TakeTheRedPill |
        | Sln3       | PracticeMgr    |
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription     | AboutUrl | Features                              |
        | Sln1       | An full online medicine system | Online medicine 1   | UrlSln1  | ["Dispensing","Referrals","Workflow"] |
        | Sln2       | Eye opening experience         | Eye opening6        | UrlSln2  | [ "Workflow", "Referrals" ]           |
        | Sln3       | Fully fledged GP system        | Fully fledged GP 12 | UrlSln3  | [ "Dispensing" ]                      |
    And Last Updated has been updated for solution Sln1

@1828
Scenario: Marketing Data is updated against the solution with trimmed whitespace
    Given solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription     | AboutUrl | Features                          |
        | Sln1       | An full online medicine system | Online medicine 1   | UrlSln1  | [ "Appointments", "Prescribing" ] |
        | Sln2       | Eye opening experience         | Eye opening6        | UrlSln2  | [ "Workflow", "Referrals" ]       |
        | Sln3       | Fully fledged GP system        | Fully fledged GP 12 | UrlSln3  | [ "Dispensing" ]                  |
    When a PUT request is made to update solution Sln1 features section
        | Features                                                     |
        | "      Dispensing     ","      Referrals","Workflow        " |
    Then a successful response is returned
    And Solutions exist
        | SolutionId | SolutionName   |
        | Sln1       | MedicOnline    |
        | Sln2       | TakeTheRedPill |
        | Sln3       | PracticeMgr    |
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription     | AboutUrl | Features                              |
        | Sln1       | An full online medicine system | Online medicine 1   | UrlSln1  | ["Dispensing","Referrals","Workflow"] |
        | Sln2       | Eye opening experience         | Eye opening6        | UrlSln2  | [ "Workflow", "Referrals" ]           |
        | Sln3       | Fully fledged GP system        | Fully fledged GP 12 | UrlSln3  | [ "Dispensing" ]                      |
    And Last Updated has been updated for solution Sln1

@1828
Scenario: Solution not found
    Given a Solution Sln4 does not exist
    When a PUT request is made to update solution Sln4 features section
        | Features                      |
        | Dispensing,Referrals,Workflow |
    Then a response status of 404 is returned

@1828
Scenario: Service failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update solution Sln1 features section
        | Features                      |
        | Dispensing,Referrals,Workflow |
    Then a response status of 500 is returned

@1828
Scenario: Solution id not present in request
    When a PUT request is made to update solution features section with no solution id
        | Features                      |
        | Dispensing,Referrals,Workflow |
    Then a response status of 400 is returned
