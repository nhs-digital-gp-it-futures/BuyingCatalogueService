Feature: Suppliers Edit Solution Implementation Timescales Section Validation
    As a Supplier
    I want to Edit the Solution Implementation Timescales Section
    So that I can make sure the information is validated

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName | SupplierId |
        | Sln1       | MedicOnline  | Sup 1      |
    And solutions have the following details
        | SolutionId | ImplementationDetail                              |
        | Sln1       | An original implementation timescales description |

@3670
Scenario: Description is greater than max length (1100 characters)
    When a PUT request is made to update the implementation-timescales section for solution Sln1
        | ImplementationTimescales     |
        | A string with length of 1101 |
    Then a response status of 400 is returned
    And the description field value is the validation failure maxLength
    And solutions have the following details
        | SolutionId | ImplementationDetail                              |
        | Sln1       | An original implementation timescales description |

@3670
Scenario: Description is equal to max length (1100 characters)
    When a PUT request is made to update the implementation-timescales section for solution Sln1
        | ImplementationTimescales     |
        | A string with length of 1100 |
    Then a successful response is returned
