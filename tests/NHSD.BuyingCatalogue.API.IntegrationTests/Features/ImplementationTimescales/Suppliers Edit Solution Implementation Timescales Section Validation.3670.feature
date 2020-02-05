Feature: Suppliers Edit Solution Implementation Timescales Section Validation
    As a Supplier
    I want to Edit the Solution Implementation Timescales Section
    So that I can make sure the information is validated

    Background:
        Given Suppliers exist
            | Id    | SupplierName |
            | Sup 1 | Supplier 1   |
        And Solutions exist
            | SolutionID | SolutionName   | SupplierStatusId | SupplierId |
            | Sln1       | MedicOnline    | 1                | Sup 1      |
        And SolutionDetail exist
            | Solution | ImplementationTimescales                          |
            | Sln1     | An original implementation timescales description |

    @3670
    Scenario: 1. Description is greater than max length (1000 characters)
        When a PUT request is made to update the implementation-timescales section for solution Sln1
            | ImplementationTimescales     |
            | A string with length of 1001 |
        Then a response status of 400 is returned
        And the description field value is the validation failure maxLength
        And SolutionDetail exist
            | Solution | ImplementationTimescales                          |
            | Sln1     | An original implementation timescales description |

