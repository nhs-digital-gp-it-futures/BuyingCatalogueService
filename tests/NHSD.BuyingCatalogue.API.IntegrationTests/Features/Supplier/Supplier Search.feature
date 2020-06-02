Feature: Search for supplier by name
    As a buyer
    I want to search for a supplier
    So that I can select the supplier to use on the order form

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier B   |
        | Sup 2 | Supplier A   |
        | Sup 3 | Superb       |

@4840
Scenario: 1. All suppliers are returned when there is no query
    When a GET request is made for suppliers
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id    | SupplierName |
        | Sup 3 | Superb       |
        | Sup 2 | Supplier A   |
        | Sup 1 | Supplier B   |

@4840
Scenario: 2. All suppliers are returned when no name is supplied
    When a GET request is made for suppliers with name
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id    | SupplierName |
        | Sup 3 | Superb       |
        | Sup 2 | Supplier A   |
        | Sup 1 | Supplier B   |

@4840
Scenario: 3. No suppliers are returned when none match the name supplied
    When a GET request is made for suppliers with name InvisibleSupplier
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id         | SupplierName |

@4840
Scenario: 4. Only suppliers matching the full name supplied are returned
    When a GET request is made for suppliers with name Supplier A
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id    | SupplierName |
        | Sup 2 | Supplier A   |

@4840
Scenario: 5. Only suppliers matching the first part of the name supplied are returned
    When a GET request is made for suppliers with name Supplier
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id    | SupplierName |
        | Sup 2 | Supplier A   |
        | Sup 1 | Supplier B   |

@4840
Scenario: 6. Only suppliers matching the partial name supplied are returned
    When a GET request is made for suppliers with name a
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id    | SupplierName |
        | Sup 2 | Supplier A   |
