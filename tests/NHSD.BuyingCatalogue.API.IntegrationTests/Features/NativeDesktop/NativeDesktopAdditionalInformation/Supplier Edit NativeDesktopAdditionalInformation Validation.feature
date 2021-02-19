Feature:  Display Marketing Page Form Native Desktop AdditionalInformation Validation on Edit
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Native Desktop AdditionalInformation
    So that I can ensure the information is correct & valid

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | 1                | Sup 1      |
    And SolutionDetail exist
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                              |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": ["native-desktop"], "NativeDesktopAdditionalInformation": "Some additional info" } |

@3623
Scenario: AdditionalInformation exceeds the maxLength
    When a PUT request is made to update the native-desktop-additional-information section for solution Sln1
        | AdditionalInformation       |
        | A string with length of 501 |
    Then a response status of 400 is returned
    And the additional-information field value is the validation failure maxLength
