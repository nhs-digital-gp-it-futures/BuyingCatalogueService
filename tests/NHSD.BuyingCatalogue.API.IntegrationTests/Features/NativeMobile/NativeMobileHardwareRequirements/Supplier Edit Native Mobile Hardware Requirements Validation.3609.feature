Feature:  Display Marketing Page Form Native Mobile Hardware Requirements Validation on Edit
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Native Mobile Hardware Requirements
    So that I can ensure the information is correct & valid

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
    And Suppliers exist
        | Id    | OrganisationName |
        | Sup 1 | GPs-R-Us         |
    And Solutions exist
        | SolutionID | SolutionName   | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | GPs-R-Us         | 1                | Sup 1      |
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1     | An full online medicine system | Online medicine 1 |                   |

@3609
Scenario: 1. HardwareRequirements exceeds the maxLength
    When a PUT request is made to update the native-mobile-hardware-requirements section for solution Sln1
    | HardwareRequirements        |
    | A string with length of 501 |
    Then a response status of 400 is returned
    And the hardware-requirements field value is the validation failure maxLength

@3609
Scenario: 2. Hardware requirements is set to null
    When a PUT request is made to update the native-mobile-hardware-requirements section for solution Sln1
    | HardwareRequirements |
    | NULL                 |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                         |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported": [] } |
