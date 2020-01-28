Feature:  Display Marketing Page Form Native Desktop Memory, Storage, Processing and Resolution  Validation on Edit
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Native Desktop Connectivity Details
    So that I can ensure the information is correct & valid

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionID | SolutionName   | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | GPs-R-Us         | 1                | Sup 1      |

@3620
Scenario Outline: 1. Minimum Memory Requirement Field Fails Validation
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1     | An full online medicine system | Online medicine 1 | { }               |
    When a PUT request is made to update the native-desktop-memory-and-storage section for solution Sln1
        | MinimumMemoryRequirement | StorageRequirementsDescription | MinimumCpu | RecommendedResolution |
        | <FieldValue>             | Some description               | 1THz       | 800x600               |
    Then a response status of 400 is returned
    And the minimum-memory-requirement field value is the validation failure <ErrorType>
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1     | An full online medicine system | Online medicine 1 | { }               |
Examples:
        | ErrorType | FieldValue                  |
        | required  |                             |
        | required  | NULL                        |

@3620
Scenario Outline: 2. Storage Requirements Description Field Fails Validation
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1     | An full online medicine system | Online medicine 1 | {}                |
    When a PUT request is made to update the native-desktop-memory-and-storage section for solution Sln1
        | MinimumMemoryRequirement | StorageRequirementsDescription | MinimumCpu | RecommendedResolution |
        | 512TB                    | <FieldValue>                   | 1Hz        | 800x600               |
    Then a response status of 400 is returned
    And the storage-requirements-description field value is the validation failure <ErrorType>
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1     | An full online medicine system | Online medicine 1 | { }               |
Examples:
        | ErrorType | FieldValue                  |
        | maxLength | A string with length of 301 |
        | required  |                             |
        | required  | NULL                        |

@3620
Scenario Outline: 3. Minimum Cpu Field Fails Validation
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1     | An full online medicine system | Online medicine 1 | { }               |
    When a PUT request is made to update the native-desktop-memory-and-storage section for solution Sln1
        | MinimumMemoryRequirement | StorageRequirementsDescription | MinimumCpu   | RecommendedResolution |
        | 512TB                    | Some description               | <FieldValue> | 800x600               |
    Then a response status of 400 is returned
    And the minimum-cpu field value is the validation failure <ErrorType>
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1     | An full online medicine system | Online medicine 1 | { }               |
Examples:
        | ErrorType | FieldValue                  |
        | maxLength | A string with length of 301 |
        | required  |                             |
        | required  | NULL                        |

@3620
Scenario: 3. Multiple fields fail validation
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1     | An full online medicine system | Online medicine 1 | { }               |
    When a PUT request is made to update the native-desktop-memory-and-storage section for solution Sln1
        | MinimumMemoryRequirement | StorageRequirementsDescription | MinimumCpu                  | RecommendedResolution |
        | NULL                     | A string with length of 301    | A string with length of 301 | 800x600               |
    Then a response status of 400 is returned
    And the minimum-memory-requirement field value is the validation failure required
    And the storage-requirements-description field value is the validation failure maxLength
    And the minimum-cpu field value is the validation failure maxLength
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1     | An full online medicine system | Online medicine 1 | { }               |
