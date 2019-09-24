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
        | SolutionID                           | SolutionName   | SummaryDescription             | OrganisationName |
        | 73b06485-875c-40af-a82a-df52b2346d9a | MedicOnline    | An full online medicine system | GPs-R-Us         |
        | bc3cdf4d-46ff-4558-882a-6eaf1c6b750f | TakeTheRedPill | Eye opening experience         | Drs. Inc         |
        | 79189149-1e9a-45c3-bbb5-d46f18ca63d7 | PracticeMgr    | Fully fledged GP system        | Drs. Inc         |
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
    Given a request containing no selection criteria
    When the request is made
    Then a successful response is returned
    And the solutions MedicOnline, TakeTheRedPill, PracticeMgr are found in the response

@2053
Scenario: 2. Card Content
    Given a request containing no selection criteria
    When the request is made
    Then a successful response is returned
    Then the details of the solutions returned are as follows
        | SolutionID                           | SolutionName   | SummaryDescription             | OrganisationName | Capabilities                                       |
        | 73b06485-875c-40af-a82a-df52b2346d9a | MedicOnline    | An full online medicine system | GPs-R-Us         | Appointments Management, Clinical Safety, Workflow |
        | bc3cdf4d-46ff-4558-882a-6eaf1c6b750f | TakeTheRedPill | Eye opening experience         | Drs. Inc         | Prescribing, Resource Management                   |
        | 79189149-1e9a-45c3-bbb5-d46f18ca63d7 | PracticeMgr    | Fully fledged GP system        | Drs. Inc         | Clinical Safety, Prescribing, Workflow             |
