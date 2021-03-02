Feature: Update Epics Validation
    As a Public User
    I want to update the Claimed Epics for a Solution
    So that I can modify what claimed epics a solution has

Background:
    Given Capabilities exist
        | CapabilityName      | CapabilityRef | IsFoundation |
        | Resource Management | C1            | false        |
        | Prescribing         | C2            | true         |
        | Workflow            | C3            | true         |
    And Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierId |
        | Sln1       | MedicOnline    | Sup 1      |
        | Sln2       | TakeTheRedPill | Sup 1      |
    And Epics exist
        | Id    | CapabilityRef |
        | Epic1 | C1            |
        | Epic2 | C2            |
        | Epic3 | C3            |
    And Solutions are linked to Epics
        | SolutionId | EpicIds      |
        | Sln2       | Epic2, Epic3 |

@3681
Scenario: A Claimed Epic that does not exist is added
    When a PUT request is made to update a epics section for solution Sln1
        | EpicId | StatusName |
        | Test   | Passed     |
    Then a response status of 400 is returned
    And the epics field value is the validation failure epicsInvalid
    Then Solutions claim only these Epics
        | SolutionId | EpicIds      |
        | Sln1       |              |
        | Sln2       | Epic2, Epic3 |

@3681
Scenario: A Status that does not exist is added
    When a PUT request is made to update a epics section for solution Sln1
        | EpicId | StatusName |
        | Epic1  | Unknown    |
    Then a response status of 400 is returned
    And the epics field value is the validation failure epicsInvalid
    Then Solutions claim only these Epics
        | SolutionId | EpicIds      |
        | Sln1       |              |
        | Sln2       | Epic2, Epic3 |

@3681
Scenario: A Claimed Epic and Status that doesnt exist is added
    When a PUT request is made to update a epics section for solution Sln2
        | EpicId | StatusName |
        | Test   | Unknown    |
    Then a response status of 400 is returned
    And the epics field value is the validation failure epicsInvalid
    Then Solutions claim only these Epics
        | SolutionId | EpicIds      |
        | Sln1       |              |
        | Sln2       | Epic2, Epic3 |

@3681
Scenario: Duplicate EpicIds are added
    When a PUT request is made to update a epics section for solution Sln2
        | EpicId | StatusName    |
        | Epic1  | Passed        |
        | Epic2  | Passed        |
        | Epic1  | Not Evidenced |
    Then a response status of 400 is returned
    And the epics field value is the validation failure epicsInvalid
    Then Solutions claim only these Epics
        | SolutionId | EpicIds      |
        | Sln1       |              |
        | Sln2       | Epic2, Epic3 |

@3681
Scenario: Duplicate EpicIds are added & An EpicId & StatusName does not exist
    When a PUT request is made to update a epics section for solution Sln2
        | EpicId | StatusName    |
        | Epic1  | Passed        |
        | Epic2  | Passed        |
        | Epic3  | Unknown       |
        | Test   | Not Evidenced |
        | Epic1  | Not Evidenced |
    Then a response status of 400 is returned
    And the epics field value is the validation failure epicsInvalid
    Then Solutions claim only these Epics
        | SolutionId | EpicIds      |
        | Sln1       |              |
        | Sln2       | Epic2, Epic3 |
