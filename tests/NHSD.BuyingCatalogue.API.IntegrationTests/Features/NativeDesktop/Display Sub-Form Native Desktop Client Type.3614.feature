Feature: Supplier Display Sub-Form Native Desktop Client Type
    As a Supplier
    I want to Display Sub-Form Native Desktop Client Type
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
        | SolutionID | SolutionName   | SummaryDescription             | OrganisationName | FullDescription     | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | An full online medicine system | GPs-R-Us         | Online medicine 1   | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | Eye opening experience         | Drs. Inc         | Eye opening6        | 1                | Sup 2      |
        | Sln3       | PracticeMgr    | Fully fledged GP system        | Drs. Inc         | Fully fledged GP 12 | 1                | Sup 2      |

@3614
Scenario: 1. Sub-Form Native Mobile Client Type all sections are Displayed
    When a GET request is made for native-desktop dashboard for solution Sln1
    Then a successful response is returned
    And Solutions section contains all items  
        | Id                                    | Status     | Requirement |
        | native-desktop-operating-systems      | INCOMPLETE | Mandatory   |
        | native-desktop-connection-details     | INCOMPLETE | Mandatory   |
        | native-desktop-memory-and-storage     | INCOMPLETE | Mandatory   |
        | native-desktop-third-party            | INCOMPLETE | Optional    |
        | native-desktop-hardware-requirements  | INCOMPLETE | Optional    |
        | native-desktop-additional-information | INCOMPLETE | Optional    |

@ignore
@3614
Scenario: 2. Solution not found
    Given a Solution Sln4 does not exist
    When a GET request is made for native-desktop dashboard for solution Sln4
    Then a response status of 404 is returned

@ignore
@3614
Scenario: 3. Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for native-desktop dashboard for solution Sln1
    Then a response status of 500 is returned

@ignore
@3614
Scenario: 4. Solution id not present in request
    When a GET request is made for native-desktop dashboard with no solution id
    Then a response status of 400 is returned
