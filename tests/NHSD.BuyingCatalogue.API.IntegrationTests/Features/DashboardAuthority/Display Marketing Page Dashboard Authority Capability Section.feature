Feature: Display Marketing Page Dashboard Authority Capability Section
    As an Authority User
    I want to manage Marketing Page Information for the Solution's Capability
    So that I can ensure the information is correct

Background:
    Given Capabilities exist
       | CapabilityName      | IsFoundation |
       | Resource Management | false        |
       | Prescribing         | true         |
       | Workflow            | true         |
     And Suppliers exist
       | Id    | SupplierName |
       | Sup 1 | Supplier 1   |
     And Solutions exist
       | SolutionId | SolutionName | SupplierId |
       | Sln1       | MedicOnline  | Sup 1      |
    And framework solutions exist
        | SolutionId | IsFoundation | FrameworkId |
        | Sln1       | true         | NHSDGP001   |

@3678
Scenario Outline: Capabilities section is mandatory and is reported complete if there are any capabilities for that solution
    Given Solutions are linked to Capabilities
        | Solution   | Capability   |
        | <Solution> | <Capability> |
    When a GET request is made for solution authority dashboard Sln1
    Then a successful response is returned
    And the solution capabilities section status is <Status>
    And the solution capabilities section requirement is Mandatory
    Examples:
        | Solution    | Capability           | Status     |
        | MedicOnline |                      | INCOMPLETE |
        | MedicOnline | Resource Management  | COMPLETE   |
        | MedicOnline | Workflow,Prescribing | COMPLETE   |
        | MedicOnline | Prescribing          | COMPLETE   |
