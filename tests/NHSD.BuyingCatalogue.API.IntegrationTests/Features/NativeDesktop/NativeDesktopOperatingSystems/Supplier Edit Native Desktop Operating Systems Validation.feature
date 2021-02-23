Feature:  Display Marketing Page Form Native Desktop Operating Systems Validation on Edit
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Native Desktop Operating Systems
    So that I can ensure the information is correct & valid

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | 1                | Sup 1      |

@3617
Scenario: Native Desktop Operating System Description is updated to be null
    Given SolutionDetail exist
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                      |
        | Sln1       | An full online medicine system | Online medicine 1 | { "NativeDesktopOperatingSystemsDescription": "Desc" } |
    When a PUT request is made to update the native-desktop-operating-systems section for solution Sln1
        | NativeDesktopOperatingSystemsDescription |
        | NULL                                     |
    Then a response status of 400 is returned
    And the operating-systems-description field value is the validation failure required
    And SolutionDetail exist
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                      |
        | Sln1       | An full online medicine system | Online medicine 1 | { "NativeDesktopOperatingSystemsDescription": "Desc" } |

@3617
Scenario: Native Desktop Operating System Description exceeds the maxlength 1000
    Given SolutionDetail exist
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                      |
        | Sln1       | An full online medicine system | Online medicine 1 | { "NativeDesktopOperatingSystemsDescription": "Desc" } |
    When a PUT request is made to update the native-desktop-operating-systems section for solution Sln1
        | NativeDesktopOperatingSystemsDescription |
        | A string with length of 1001             |
    Then a response status of 400 is returned
    And the operating-systems-description field value is the validation failure maxLength
    And SolutionDetail exist
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                      |
        | Sln1       | An full online medicine system | Online medicine 1 | { "NativeDesktopOperatingSystemsDescription": "Desc" } |
