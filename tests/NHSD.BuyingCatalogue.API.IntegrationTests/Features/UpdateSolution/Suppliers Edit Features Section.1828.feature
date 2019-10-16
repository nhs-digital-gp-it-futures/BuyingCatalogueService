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
        | SolutionID | SolutionName   | SummaryDescription             | OrganisationName | FullDescription     | SupplierStatusId |
        | Sln1       | MedicOnline    | An full online medicine system | GPs-R-Us         | Online medicine 1   | 1                |
        | Sln2       | TakeTheRedPill | Eye opening experience         | Drs. Inc         | Eye opening6        | 1                |
        | Sln3       | PracticeMgr    | Fully fledged GP system        | Drs. Inc         | Fully fledged GP 12 | 1                |

@1828
Scenario: 1. Marketing Data is updated against the solution
    Given MarketingDetail exist
        | Solution | AboutUrl | Features                          |
        | Sln1     | UrlSln1  | [ "Appointments", "Prescribing" ] |
        | Sln2     | UrlSln2  | [ "Workflow", "Referrals" ]       |
        | Sln3     | UrlSln3  | [ "Dispensing" ]                  |
    When a PUT request is made to update solution Sln1 features section
        | Features                      |
        | Dispensing,Referrals,Workflow |
    Then a successful response is returned
    And Solutions exist
        | SolutionID | SolutionName   | SummaryDescription             | FullDescription     | SupplierStatusId |
        | Sln1       | MedicOnline    | An full online medicine system | Online medicine 1   | 1                |
        | Sln2       | TakeTheRedPill | Eye opening experience         | Eye opening6        | 1                |
        | Sln3       | PracticeMgr    | Fully fledged GP system        | Fully fledged GP 12 | 1                |
    And MarketingDetail exist
        | Solution | AboutUrl | Features                              |
        | Sln1     | UrlSln1  | ["Dispensing","Referrals","Workflow"] |
        | Sln2     | UrlSln2  | [ "Workflow", "Referrals" ]           |
        | Sln3     | UrlSln3  | [ "Dispensing" ]                      |

@1828
Scenario: 2. Marketing Data is added to the solution
    Given MarketingDetail exist
        | Solution | AboutUrl | Features                          |
        | Sln2     | UrlSln2  | [ "Workflow", "Referrals" ]       |
        | Sln3     | UrlSln3  | [ "Dispensing" ]                  |
    When a PUT request is made to update solution Sln1 features section
        | Features                      |
        | Dispensing,Referrals,Workflow |
    Then a successful response is returned
    And Solutions exist
        | SolutionID | SolutionName   | SummaryDescription             | FullDescription     | SupplierStatusId |
        | Sln1       | MedicOnline    | An full online medicine system | Online medicine 1   | 1                |
        | Sln2       | TakeTheRedPill | Eye opening experience         | Eye opening6        | 1                |
        | Sln3       | PracticeMgr    | Fully fledged GP system        | Fully fledged GP 12 | 1                |
    And MarketingDetail exist
        | Solution | AboutUrl | Features                              |
        | Sln1     |          | ["Dispensing","Referrals","Workflow"] |
        | Sln2     | UrlSln2  | [ "Workflow", "Referrals" ]           |
        | Sln3     | UrlSln3  | [ "Dispensing" ]                      |

