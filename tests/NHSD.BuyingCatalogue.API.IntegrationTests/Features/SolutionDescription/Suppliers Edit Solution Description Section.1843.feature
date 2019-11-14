Feature: Suppliers Edit Solution Description Section
    As a Supplier
    I want to Edit the About Solution Section
    So that I can make sure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
        | Drs. Inc |
    And Suppliers exist
        | Id    | OrganisationName |
        | Sup 1 | GPs-R-Us         |
        | Sup 2 | Drs. Inc         |
    And Solutions exist
        | SolutionID | SolutionName   | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | GPs-R-Us         | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | Drs. Inc         | 1                | Sup 2      |
        | Sln3       | PracticeMgr    | Drs. Inc         | 1                | Sup 2      |
    And SolutionDetail exist
        | Solution | AboutUrl | SummaryDescription             | FullDescription     | Features                          |
        | Sln1     | UrlSln1  | An full online medicine system | Online medicine 1   | [ "Appointments", "Prescribing" ] |
        | Sln2     | UrlSln2  | Eye opening experience         | Eye opening6        | [ "Workflow", "Referrals" ]       |

@1843
Scenario: 1. Solution description section data is updated
    When a PUT request is made to update solution Sln1 solution-description section
        | Summary                | Description            | Link       |
        | New type of medicine 4 | A new full description | UrlSln1New |
    Then a successful response is returned
    And Solutions exist
        | SolutionID | SolutionName   |
        | Sln1       | MedicOnline    |
        | Sln2       | TakeTheRedPill |
        | Sln3       | PracticeMgr    |
    And SolutionDetail exist
        | Solution | AboutUrl   | SummaryDescription      | FullDescription        | Features                          |
        | Sln1     | UrlSln1New | New type of medicine 4  | A new full description | [ "Appointments", "Prescribing" ] |
        | Sln2     | UrlSln2    | Eye opening experience  | Eye opening6           | [ "Workflow", "Referrals" ]       |

Scenario: 2. Solution description section data is not created on update, fail fast in this case
    Given a SolutionDetail Sln3 does not exist
    When a PUT request is made to update solution Sln3 solution-description section
        | Summary                 | Description         | Link       |
        | Fully fledged GP system | Fully fledged GP 12 | UrlSln3New |
    Then a response status of 500 is returned

@1828
Scenario: 3. Solution not found
    Given a Solution Sln4 does not exist
    When a PUT request is made to update solution Sln4 solution-description section
        | Summary                 | Description         | Link       |
        | Fully fledged GP system | Fully fledged GP 12 | UrlSln3New |
    Then a response status of 404 is returned

@1828
Scenario: 4. Service failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update solution Sln4 solution-description section
        | Summary                 | Description         | Link       |
        | Fully fledged GP system | Fully fledged GP 12 | UrlSln3New |
    Then a response status of 500 is returned

@1828
Scenario: 4. Solution id not present in request
    When a PUT request is made to update solution solution-description section with no solution id
        | Summary                 | Description         | Link       |
        | Fully fledged GP system | Fully fledged GP 12 | UrlSln3New |
    Then a response status of 400 is returned
