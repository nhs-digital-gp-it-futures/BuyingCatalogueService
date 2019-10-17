Feature: Suppliers Edit Solution Description Section
    As a Supplier
    I want to Edit the About Solution Section
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
    And MarketingDetail exist
        | Solution | AboutUrl | Features                          |
        | Sln1     | UrlSln1  | [ "Appointments", "Prescribing" ] |
        | Sln2     | UrlSln2  | [ "Workflow", "Referrals" ]       |

@1843
Scenario: 1. Solution description section data is updated
    When a PUT request is made to update solution Sln1 description section
        | Summary                | Description            | Link       |
        | New type of medicine 4 | A new full description | UrlSln1New |
    Then a successful response is returned
    And Solutions exist
        | SolutionID | SolutionName   | SummaryDescription      | FullDescription        | SupplierStatusId |
        | Sln1       | MedicOnline    | New type of medicine 4  | A new full description | 1                |
        | Sln2       | TakeTheRedPill | Eye opening experience  | Eye opening6           | 1                |
        | Sln3       | PracticeMgr    | Fully fledged GP system | Fully fledged GP 12    | 1                |
    And MarketingDetail exist
        | Solution | AboutUrl   | Features                          |
        | Sln1     | UrlSln1New | [ "Appointments", "Prescribing" ] |
        | Sln2     | UrlSln2    | [ "Workflow", "Referrals" ]       |

Scenario: 2. Solution description section data is created on update
    Given a MarketingDetail Sln3 does not exist
    When a PUT request is made to update solution Sln3 description section
        | Summary                 | Description         | Link       |
        | Fully fledged GP system | Fully fledged GP 12 | UrlSln3New |
    Then a successful response is returned
    And Solutions exist
        | SolutionID | SolutionName   | SummaryDescription             | FullDescription     | SupplierStatusId |
        | Sln1       | MedicOnline    | An full online medicine system | Online medicine 1   | 1                |
        | Sln2       | TakeTheRedPill | Eye opening experience         | Eye opening6        | 1                |
        | Sln3       | PracticeMgr    | Fully fledged GP system        | Fully fledged GP 12 | 1                |
    And MarketingDetail exist
        | Solution | AboutUrl   | Features                          |
        | Sln1     | UrlSln1    | [ "Appointments", "Prescribing" ] |
        | Sln2     | UrlSln2    | [ "Workflow", "Referrals" ]       |
        | Sln3     | UrlSln3New |                                   |
