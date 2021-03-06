Feature: Suppliers Edit Solution Description Section Validation
    As a Supplier
    I want to Edit the About Solution Section
    So that I can make sure the information is validated

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName | SupplierId |
        | Sln1       | MedicOnline  | Sup 1      |
    And solutions have the following details
        | SolutionId | AboutUrl | SummaryDescription             | FullDescription     | Features                          |
        | Sln1       | UrlSln1  | An full online medicine system | Online medicine 1   | [ "Appointments", "Prescribing" ] |
    And framework solutions exist
        | SolutionId | IsFoundation | FrameworkId |
        | Sln1       | true         | NHSDGP001   |

@3319
Scenario: No features are filled out
    Given a request with ten features
    When the update features request is made for Sln1
    Then a successful response is returned
    And Solutions exist
        | SolutionId | SolutionName   |
        | Sln1       | MedicOnline    |
    And solutions have the following details
        | SolutionId | AboutUrl | SummaryDescription             | FullDescription   | Features                                  |
        | Sln1       | UrlSln1  | An full online medicine system | Online medicine 1 | ["0","1","2","3","4","5","6","7","8","9"] |

@3319
Scenario: listing-1 exceeds the character limit
    Given a request with ten features
    And feature at position 1 is a string of 101 characters
    When the update features request is made for Sln1
    Then a response status of 400 is returned
    And the listing-1 field value is the validation failure maxLength
    And Solutions exist
        | SolutionId | SolutionName   |
        | Sln1       | MedicOnline    |
    And solutions have the following details
        | SolutionId | AboutUrl | SummaryDescription             | FullDescription     | Features                          |
        | Sln1       | UrlSln1  | An full online medicine system | Online medicine 1   | [ "Appointments", "Prescribing" ] |

@3319
Scenario: listing-1 & listing-3 are within the character limit. listing-5 & listing-8 exceeds the character limit
    Given a request with ten features
    And feature at position 1 is a string of 10 characters
    And feature at position 3 is a string of 70 characters
    And feature at position 5 is a string of 101 characters
    And feature at position 8 is a string of 101 characters
    When the update features request is made for Sln1
    Then a response status of 400 is returned
    And the listing-5 field value is the validation failure maxLength
    And the listing-8 field value is the validation failure maxLength
    And Solutions exist
        | SolutionId | SolutionName   |
        | Sln1       | MedicOnline    |
    And solutions have the following details
        | SolutionId | AboutUrl | SummaryDescription             | FullDescription     | Features                          |
        | Sln1       | UrlSln1  | An full online medicine system | Online medicine 1   | [ "Appointments", "Prescribing" ] |
