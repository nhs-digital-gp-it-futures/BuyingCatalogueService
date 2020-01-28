Feature: Solution Foundations
    As a Public User
    I want to bring back the solutions where foundation is true
    So that I know what foundations are true

Background:
    Given Capabilities exist
        | CapabilityName          | IsFoundation |
        | Appointments Management | true         |
        | Prescribing             | true         |
        | Workflow                | true         |
        | Clinical Safety         | false        |
        | Resource Management     | false        |
    And Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
        | Sup 2 | Supplier 2   |
   And Solutions exist
        | SolutionID | SolutionName   | OrganisationName | SupplierStatusId | SupplierId | PublishedStatusId |
        | Sln1       | MedicOnline    | GPs-R-Us         | 1                | Sup 1      | 3                 |
        | Sln2       | TakeTheRedPill | Drs. Inc         | 1                | Sup 2      | 3                 |
        | Sln3       | PracticeMgr    | Drs. Inc         | 1                | Sup 2      | 3                 |
        | Sln4       | GPSurgery      | GPs-R-Us         | 1                | Sup 1      | 3                 |
        | Sln5       | Unpublished    | GPs-R-Us         | 1                | Sup 1      | 1                 |
   And Framework Solutions exist
        | SolutionId | IsFoundation |
        | Sln1       | true         |
        | Sln2       | false        |
        | Sln3       | true         |
        | Sln5       | true         |
   And Solutions are linked to Capabilities
        | Solution       | Capability              |
        | MedicOnline    | Appointments Management |
        | MedicOnline    | Clinical Safety         |
        | MedicOnline    | Workflow                |
        | TakeTheRedPill | Prescribing             |
        | TakeTheRedPill | Resource Management     |
        | PracticeMgr    | Clinical Safety         |
        | PracticeMgr    | Prescribing             |
        | GPSurgery      | Workflow                |
        | Unpublished    | Clinical Safety         |

@3505
Scenario: 1. Foundation solutions are retrieved 
    When a GET request is made for foundation solutions
    Then a successful response is returned
    And the solutions MedicOnline,PracticeMgr are found in the response
    And the solutions TakeTheRedPill,GPSurgery, Unpublished are not found in the response
    
@3505
Scenario: 2. Brings back all results
    When a GET request is made containing no selection criteria
    Then a successful response is returned
    And the solutions MedicOnline,TakeTheRedPill,PracticeMgr,GPSurgery are found in the response
    And the solutions Unpublished are not found in the response
