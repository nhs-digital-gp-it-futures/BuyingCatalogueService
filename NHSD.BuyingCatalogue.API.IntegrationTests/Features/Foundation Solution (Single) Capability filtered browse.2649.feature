Feature: Foundation Solution (Single) Capability filtered browse
    As a Public User
    I want to use Foundation Capabilities when browsing Solutions
    So that I know what Solutions (Single) include those Capabilities

Background:
    Given Capabilities exist
        | CapabilityName          | IsFoundation |
        | Appointments Management | true         |
        | Prescribing             | true         |
        | Clinical Safety         | false        |
        | Workflow                | false        |
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
        | MedicOnline    | Prescribing             |
        | TakeTheRedPill | Prescribing             |
        | TakeTheRedPill | Appointments Management |
        | TakeTheRedPill | Workflow                |
        | PracticeMgr    | Clinical Safety         |

@2649
Scenario: 1. All the Foundation Capabilities and no other Capabilities are selected, only Solutions (Single) that deliver all the Foundation Capabilities are included
    Given a request containing the capabilities Prescribing, Appointments Management
    When the request is made
    Then a successful response is returned
    And the solutions MedicOnline, TakeTheRedPill are found in the response
    And the solutions PracticeMgr are not found in the response

@2649
Scenario: 2. All the Foundation Capabilities and one or more other Capabilities are selected, only Solutions that deliver all the Foundation Capabilities and the other selected Capabilities are included
    Given a request containing the capabilities Prescribing, Appointments Management, Clinical Safety
    When the request is made
    Then a successful response is returned
    And the solutions MedicOnline are found in the response
    And the solutions PracticeMgr, TakeTheRedPill are not found in the response
