Feature: Display Marketing Page Form Supplier Validation
    As a Supplier
    I want to Edit the Solution Suppliers Section
    So that I can make sure the information is validated

Background:
    Given Suppliers exist
        | Id    | SupplierName | Summary      | SupplierUrl |
        | Sup 1 | Supplier 1   | Some Summary | www.url.com |
    And Solutions exist
        | SolutionId | SolutionName | SupplierId |
        | Sln1       | MedicOnline  | Sup 1      |

@3653
Scenario: Summary is greater than max length (1100 characters)
    When a PUT request is made to update the about-supplier section for solution Sln1
        | Summary                      | SupplierUrl     |
        | A string with length of 1101 | www.Someurl.com |
    Then a response status of 400 is returned
    And the description field value is the validation failure maxLength
    And Suppliers exist
        | Id    | Summary      | SupplierUrl |
        | Sup 1 | Some Summary | www.url.com |

@3653
Scenario: Summary is equal to max length (1100 characters)
    When a PUT request is made to update the about-supplier section for solution Sln1
        | Summary                      | SupplierUrl     |
        | A string with length of 1100 | www.Someurl.com |
    Then a successful response is returned

@3653
Scenario: Supplier Url is greater than max length (1000 characters)
    When a PUT request is made to update the about-supplier section for solution Sln1
        | Summary      | SupplierUrl                  |
        | More Summary | A string with length of 1001 |
    Then a response status of 400 is returned
    And the link field value is the validation failure maxLength
    And Suppliers exist
        | Id    | Summary      | SupplierUrl |
        | Sup 1 | Some Summary | www.url.com |

@3653
Scenario: Summary & Supplier Url is greater than max length (1100 characters)
    When a PUT request is made to update the about-supplier section for solution Sln1
        | Summary                      | SupplierUrl                  |
        | A string with length of 1101 | A string with length of 1001 |
    Then a response status of 400 is returned
    And the description field value is the validation failure maxLength
    And the link field value is the validation failure maxLength
    And Suppliers exist
        | Id    | Summary      | SupplierUrl |
        | Sup 1 | Some Summary | www.url.com |
