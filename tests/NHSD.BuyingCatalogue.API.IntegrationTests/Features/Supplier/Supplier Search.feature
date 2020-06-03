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
    And Solutions exist
        | SolutionId | SolutionName   | SupplierStatusId | SupplierId | PublishedStatus |
        | Sln1       | MedicOnline    | 3                | Sup 1      | Published       |
        | Sln2       | TakeTheRedPill | 3                | Sup 2      | Published       |
        | Sln3       | PracticeMgr    | 3                | Sup 3      | Withdrawn       |

@4840
Scenario: 1. All suppliers are returned when there are no query parameters
    When a GET request is made for suppliers
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id    | SupplierName |
        | Sup 3 | Superb       |
        | Sup 2 | Supplier A   |
        | Sup 1 | Supplier B   |

@4840
Scenario: 2. All suppliers are returned when no name is supplied
    When a GET request is made for suppliers with name ''
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id    | SupplierName |
        | Sup 3 | Superb       |
        | Sup 2 | Supplier A   |
        | Sup 1 | Supplier B   |

@4840
Scenario: 3. All suppliers are returned when no solution publication status is supplied
    When a GET request is made for suppliers with solutionPublicationStatus ''
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id    | SupplierName |
        | Sup 3 | Superb       |
        | Sup 2 | Supplier A   |
        | Sup 1 | Supplier B   |

@4840
Scenario: 4. All suppliers are returned when no name and solution publication status are supplied
    When a GET request is made for suppliers with name '' and solution publication status ''
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id    | SupplierName |
        | Sup 3 | Superb       |
        | Sup 2 | Supplier A   |
        | Sup 1 | Supplier B   |

@4840
Scenario: 5. No suppliers are returned when none match the name supplied
    When a GET request is made for suppliers with name 'InvisibleSupplier'
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id         | SupplierName |

@4840
Scenario: 6. Only suppliers matching the full name supplied are returned
    When a GET request is made for suppliers with name 'Supplier A'
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id    | SupplierName |
        | Sup 2 | Supplier A   |

@4840
Scenario: 7. Only suppliers matching the first part of the name supplied are returned
    When a GET request is made for suppliers with name 'Supplier'
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id    | SupplierName |
        | Sup 2 | Supplier A   |
        | Sup 1 | Supplier B   |

@4840
Scenario: 8. Only suppliers matching the partial name supplied are returned
    When a GET request is made for suppliers with name 'a'
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id    | SupplierName |
        | Sup 2 | Supplier A   |

@4840
Scenario: 9. Only suppliers matching solution publication status supplied are returned
    When a GET request is made for suppliers with solutionPublicationStatus 'Published'
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id    | SupplierName |
        | Sup 2 | Supplier A   |
        | Sup 1 | Supplier B   |

@4840
Scenario: 10. Only suppliers matching the name and solution publication status supplied are returned
    When a GET request is made for suppliers with name 'Supplier B' and solution publication status 'Published'
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id    | SupplierName |
        | Sup 1 | Supplier B   |
