Feature:  Display Marketing Page Form Native Mobile Hardware Requirements Validation on Edit
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Native Mobile Hardware Requirements
    So that I can ensure the information is correct & valid

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | 1                | Sup 1      |
    And Solution have following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1       | An full online medicine system | Online medicine 1 |                   |

@3609
Scenario: HardwareRequirements exceeds the maxLength
    When a PUT request is made to update the native-mobile-hardware-requirements section for solution Sln1
    | HardwareRequirements        |
    | A string with length of 501 |
    Then a response status of 400 is returned
    And the hardware-requirements field value is the validation failure maxLength

@3609
Scenario: Hardware requirements is set to null
    When a PUT request is made to update the native-mobile-hardware-requirements section for solution Sln1
    | HardwareRequirements |
    | NULL                 |
    Then a successful response is returned
    And Solution have following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                         |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported": [] } |
