Feature: Suppliers Edit Features Section
    As a Supplier
    I want to Edit the Features Section
    So that I can make sure the information is correct

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

@1828
Scenario: 1. Marketing Data is updated against the solution
    Given MarketingDetail exist
        | Solution | SummaryDescription             | FullDescription     | AboutUrl | Features                          |
        | Sln1     | An full online medicine system | Online medicine 1   | UrlSln1  | [ "Appointments", "Prescribing" ] |
        | Sln2     | Eye opening experience         | Eye opening6        | UrlSln2  | [ "Workflow", "Referrals" ]       |
        | Sln3     | Fully fledged GP system        | Fully fledged GP 12 | UrlSln3  | [ "Dispensing" ]                  |
    When a PUT request is made to update solution Sln1 features section
        | Features                      |
        | Dispensing,Referrals,Workflow |
    Then a successful response is returned
    And Solutions exist
        | SolutionID | SolutionName   | SupplierStatusId |
        | Sln1       | MedicOnline    | 1                |
        | Sln2       | TakeTheRedPill | 1                |
        | Sln3       | PracticeMgr    | 1                |
    And MarketingDetail exist
        | Solution | SummaryDescription             | FullDescription     | AboutUrl | Features                              |
        | Sln1     | An full online medicine system | Online medicine 1   | UrlSln1  | ["Dispensing","Referrals","Workflow"] |
        | Sln2     | Eye opening experience         | Eye opening6        | UrlSln2  | [ "Workflow", "Referrals" ]           |
        | Sln3     | Fully fledged GP system        | Fully fledged GP 12 | UrlSln3  | [ "Dispensing" ]                      |

@1828
Scenario: 2. Marketing Data is added to the solution
    Given MarketingDetail exist
        | Solution | SummaryDescription             | FullDescription     | AboutUrl | Features                          |
        | Sln2     | An full online medicine system | Online medicine 1   | UrlSln2  | [ "Workflow", "Referrals" ]       |
        | Sln3     | Eye opening experience         | Eye opening6        | UrlSln3  | [ "Dispensing" ]                  |
    When a PUT request is made to update solution Sln1 features section
        | Features                      |
        | Dispensing,Referrals,Workflow |
    Then a successful response is returned
    And Solutions exist
        | SolutionID | SolutionName   | SupplierStatusId |
        | Sln1       | MedicOnline    | 1                |
        | Sln2       | TakeTheRedPill | 1                |
        | Sln3       | PracticeMgr    | 1                |
    And MarketingDetail exist
        | Solution | SummaryDescription             | FullDescription     | AboutUrl | Features                              |
        | Sln1     | An full online medicine system | Online medicine 1   |          | ["Dispensing","Referrals","Workflow"] |
        | Sln2     | Eye opening experience         | Eye opening6        | UrlSln2  | [ "Workflow", "Referrals" ]           |
        | Sln3     | Fully fledged GP system        | Fully fledged GP 12 | UrlSln3  | [ "Dispensing" ]                      |

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
