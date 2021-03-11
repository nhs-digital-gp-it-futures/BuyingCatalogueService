Feature: Capability filtered browse (no additional service)
    As a Public User
    I want to use Capabilities when browsing Solutions
    So that I know what Solutions include those Capabilities

Background:
    Given Capabilities exist
        | CapabilityName          | IsFoundation | CapabilityRef |
        | Appointments Management | true         | C1            |
        | Prescribing             | true         | C7            |
        | Workflow                | true         | C10           |
        | Clinical Safety         | false        | C11           |
        | Resource Management     | false        | C27           |
    And Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
        | Sup 2 | Supplier 2   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierId |
        | Sln1       | MedicOnline    | Sup 1      |
        | Sln2       | TakeTheRedPill | Sup 2      |
        | Sln3       | PracticeMgr    | Sup 2      |
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
    And Framework Solutions exist
        | SolutionId | IsFoundation | FrameworkId |
        | Sln1       | true         | NHSDGP001   |
        | Sln2       | false        | DFOCVC001   |
        | Sln3       | false        | NHSDGP001   |

@2048
Scenario: No Capability selected, all solutions are returned
    When a POST request is made containing no selection criteria
    Then a successful response is returned
    And the solutions MedicOnline,TakeTheRedPill,PracticeMgr are found in the response

@2048
Scenario Outline: 2a. One Capability is selected, all solutions that deliver that capability are returned
    When a POST request is made containing a single capability '<Capability>'
    Then a successful response is returned
    And the solutions <Solutions> are found in the response

    Examples:
        | Capability              | Solutions                  |
        | Workflow                | MedicOnline,PracticeMgr    |
        | Appointments Management | MedicOnline                |
        | Clinical Safety         | MedicOnline,PracticeMgr    |
        | Prescribing             | TakeTheRedPill,PracticeMgr |

Scenario Outline: 2b. Multiple Capabilities are selected, all solutions that deliver ALL requested capabilities are returned
    When a POST request is made containing the capabilities <Capabilities>
    Then a successful response is returned
    And the solutions <Solutions> are found in the response

    Examples:
        | Capabilities                                | Solutions               |
        | Workflow,Appointments Management            | MedicOnline             |
        | Appointments Management,Resource Management |                         |
        | Clinical Safety,Workflow                    | MedicOnline,PracticeMgr |
        | Clinical Safety,Prescribing                 | PracticeMgr             |
        | Prescribing,Workflow,Resource Management    |                         |
        | Prescribing,Resource Management             | TakeTheRedPill          |

@2048
Scenario Outline: Multiple Capabilities are selected, solutions that do not deliver ALL requested capabilities are NOT returned
    When a POST request is made containing the capabilities <Capabilities>
    Then a successful response is returned
    And the solutions <Solutions> are not found in the response

    Examples:
        | Capabilities                                | Solutions                              |
        | Workflow,Appointments Management            | PracticeMgr,TakeTheRedPill             |
        | Appointments Management,Resource Management | MedicOnline,PracticeMgr,TakeTheRedPill |
        | Clinical Safety,Workflow                    | TakeTheRedPill                         |
        | Clinical Safety,Prescribing                 | MedicOnline,TakeTheRedPill             |
        | Prescribing,Workflow,Resource Management    | MedicOnline,PracticeMgr,TakeTheRedPill |
        | Prescribing,Resource Management             | MedicOnline,PracticeMgr                |

@2048
Scenario Outline: Duplicate Capabilities are selected, all solutions that deliver ALL unique requested capabilities are returned
    When a POST request is made containing the capabilities <Capabilities>
    Then a successful response is returned
    And the solutions <Solutions> are found in the response

    Examples:
        | Capabilities                                            | Solutions               |
        | Workflow,Workflow,Appointments Management               | MedicOnline             |
        | Appointments Management,Resource Management             |                         |
        | Clinical Safety,Workflow,Clinical Safety                | MedicOnline,PracticeMgr |
        | Clinical Safety,Prescribing                             | PracticeMgr             |
        | Prescribing,Workflow,Resource Management                |                         |
        | Prescribing,Resource Management,Prescribing,Prescribing | TakeTheRedPill          |
