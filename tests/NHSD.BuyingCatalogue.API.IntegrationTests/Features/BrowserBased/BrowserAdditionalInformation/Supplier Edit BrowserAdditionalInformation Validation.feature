Feature:  Display Marketing Page Form BrowserAdditionalInformation Validation on Edit
    As a Supplier
    I want to manage Marketing Page Information for the Solution's BrowserAdditionalInformation
    So that I can ensure the information is correct & valid

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierId |
        | Sln1       | MedicOnline    | Sup 1      |
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                               |
        | Sln1       | An full online medicine system | Online medicine 1 | { "AdditionalInformation": "Some Info", "ClientApplicationTypes": [], "BrowsersSupported": [] } |        
    And framework solutions exist
        | SolutionId | IsFoundation | FrameworkId |
        | Sln1       | true         | NHSDGP001   |

@3601
Scenario: AdditionalInformation exceeds the maxLength
    When a PUT request is made to update the browser-additional-information section for solution Sln1
        | AdditionalInformation       |
        | A string with length of 501 |
    Then a response status of 400 is returned
    And the additional-information field value is the validation failure maxLength

@3601
Scenario: AdditionalInformation is set to null
    When a PUT request is made to update the browser-additional-information section for solution Sln1
        | AdditionalInformation |
        | NULL                  |
    Then a successful response is returned
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                         |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported": [] } |
