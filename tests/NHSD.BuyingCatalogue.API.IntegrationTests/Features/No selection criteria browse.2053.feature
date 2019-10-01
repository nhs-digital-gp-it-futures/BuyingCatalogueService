Feature: No selection criteria browse
    As a Public User
    I want to browse the Solutions
    So that I know what those Solutions are

Background:
    Given Capabilities exist
        | CapabilityName          | IsFoundation |
        | Appointments Management | true         |
        | Prescribing             | true         |
        | Workflow                | true         |
        | Clinical Safety         | false        |
        | Resource Management     | false        |
    And Organisations exist
        | Name     |
        | GPs-R-Us |
        | Drs. Inc |
    And Solutions exist
        | SolutionID | SolutionName   | SummaryDescription             | OrganisationName |
        | Sln1       | MedicOnline    | An full online medicine system | GPs-R-Us         |
        | Sln2       | TakeTheRedPill | Eye opening experience         | Drs. Inc         |
        | Sln3       | PracticeMgr    | Fully fledged GP system        | Drs. Inc         |
    And Solutions are linked to Capabilities
        | Solution       | Capability              |
        | MedicOnline    | Appointments Management |
        | MedicOnline    | Clinical Safety         |
        | MedicOnline    | Workflow                |
        | TakeTheRedPill | Prescribing             |
        | TakeTheRedPill | Resource Management     |
        | PracticeMgr    | Clinical Safety         |
        | PracticeMgr    | Prescribing             |
        | PracticeMgr    | Workflow                |

@2053
Scenario: 1. No selection criteria applied
    When a GET request is made containing no selection criteria
    Then a successful response is returned
    And the solutions MedicOnline,TakeTheRedPill,PracticeMgr are found in the response

@2053
Scenario: 2. Card Content
    When a GET request is made containing no selection criteria
    Then a successful response is returned
    And the details of the solutions returned are as follows
        | SolutionID | SolutionName   | SummaryDescription             | OrganisationName | Capabilities                                       |
        | Sln1       | MedicOnline    | An full online medicine system | GPs-R-Us         | Appointments Management, Clinical Safety, Workflow |
        | Sln2       | TakeTheRedPill | Eye opening experience         | Drs. Inc         | Prescribing, Resource Management                   |
        | Sln3       | PracticeMgr    | Fully fledged GP system        | Drs. Inc         | Clinical Safety, Prescribing, Workflow             |
