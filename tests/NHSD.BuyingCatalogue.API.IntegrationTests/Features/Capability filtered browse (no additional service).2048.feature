Feature: Capability filtered browse (no additional service)
    As a Public User
    I want to use Capabilities when browsing Solutions
    So that I know what Solutions include those Capabilities

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

@2048
Scenario: 1. No Capability selected, all solutions are returned
    Given a request containing no selection criteria
    When the request is made
    Then a successful response is returned
    And the solutions MedicOnline, TakeTheRedPill, PracticeMgr are found in the response

@2048
Scenario Outline: 2a. One Capability is selected, all solutions that deliver that capability are returned
    Given a request containing a single capability <Capability>
    When the request is made
    Then a successful response is returned
    And the solutions <Solutions> are found in the response
Examples: 
        | Capability              | Solutions                   |
        | Workflow                | MedicOnline, PracticeMgr    |
        | Appointments Management | MedicOnline                 |
        | Clinical Safety         | MedicOnline, TakeTheRedPill |
        | Prescribing             | TakeTheRedPill, PracticeMgr |

Scenario Outline: 2b. Multiple Capabilities are selected, all solutions that deliver ALL requested capabilities are returned
    Given a request containing the capabilities <Capabilities>
    When the request is made
    Then a successful response is returned
    And the solutions <Solutions> are found in the response
Examples: 
        | Capabilities                                 | Solutions                |
        | Workflow, Appointments Management            | MedicOnline              |
        | Appointments Management, Resource Management |                          |
        | Clinical Safety, Workflow                    | MedicOnline, PracticeMgr |
        | Clinical Safety, Prescribing                 | PracticeMgr              |
        | Prescribing, Workflow, Resource Management   |                          |
        | Prescribing, Resource Management             | TakeTheRedPill           |

@2048
Scenario Outline: 3. Multiple Capabilities are selected, solutions that do not deliver ALL requested capabilities are NOT returned
    Given a request containing the capabilities <Capabilities>
    When the request is made
    Then a successful response is returned
    And the solutions <Solutions> are not found in the response
Examples: 
        | Capabilities                                 | Solutions                                |
        | Workflow, Appointments Management            | PracticeMgr, TakeTheRedPill              |
        | Appointments Management, Resource Management | MedicOnline, PracticeMgr ,TakeTheRedPill |
        | Clinical Safety, Workflow                    | TakeTheRedPill                           |
        | Clinical Safety, Prescribing                 | MedicOnline, TakeTheRedPill              |
        | Prescribing, Workflow, Resource Management   | MedicOnline, PracticeMgr ,TakeTheRedPill |
        | Prescribing, Resource Management             | MedicOnline, PracticeMgr                 |
