Feature: Display Marketing Page Public Capabilities Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Capabilities
    So that I can ensure the information is correct

Background:
    Given Capabilities exist
        | CapabilityName          | IsFoundation | Version | Description | SourceUrl                          |
        | Appointments Management | true         | 2.0     | AM          | http://appointments.management.com |
        | Prescribing             | true         | 2.1     | P           | http://prescribing.com             |
        | Workflow                | true         | 2.2     | W           | http://workflow.com                |
        | Clinical Safety         | false        | 2.3     | CS          | http://clinical.safety.com         |
    And Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierId |
        | Sln1       | MedicOnline    | Sup 1      |
        | Sln2       | TakeTheRedPill | Sup 1      |
    And Solutions are linked to Capabilities
        | Solution    | Capability              | Pass  |
        | MedicOnline | Appointments Management | True  |
        | MedicOnline | Clinical Safety         | True  |
        | MedicOnline | Workflow                | True  |
        | MedicOnline | Prescribing             | False |
    And framework solutions exist
        | SolutionId | IsFoundation | FrameworkId |
        | Sln1       | true         | NHSDGP001   |
        | Sln2       | false        | DFOCVC001   |

@3507
Scenario: Sections presented where Capabilities exists
    When a GET request is made for solution public Sln1
    Then a successful response is returned
    And the response contains the following values
        | Section      | Field                           | Value                              |
        | capabilities | capabilities-met[0].name        | Appointments Management            |
        | capabilities | capabilities-met[0].version     | 2.0                                |
        | capabilities | capabilities-met[0].description | AM                                 |
        | capabilities | capabilities-met[0].link        | http://appointments.management.com |
        | capabilities | capabilities-met[1].name        | Clinical Safety                    |
        | capabilities | capabilities-met[1].version     | 2.3                                |
        | capabilities | capabilities-met[1].description | CS                                 |
        | capabilities | capabilities-met[1].link        | http://clinical.safety.com         |
        | capabilities | capabilities-met[2].name        | Workflow                           |
        | capabilities | capabilities-met[2].version     | 2.2                                |
        | capabilities | capabilities-met[2].description | W                                  |
        | capabilities | capabilities-met[2].link        | http://workflow.com                |

@3507
Scenario: Sections not presented where no Capabilities exists
    When a GET request is made for solution public Sln2
    Then a successful response is returned
    And the solution capabilities section contains no Capabilities
