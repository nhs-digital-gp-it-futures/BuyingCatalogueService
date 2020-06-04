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
    Given the user has searched for suppliers matching ''
    When a GET request is made for suppliers
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id    | SupplierName |
        | Sup 3 | Superb       |
        | Sup 2 | Supplier A   |
        | Sup 1 | Supplier B   |

@4840
Scenario: 3. All suppliers are returned when no solution publication status is supplied
    Given the user has searched for suppliers with solutions matching the publication status ''
    When a GET request is made for suppliers
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id    | SupplierName |
        | Sup 3 | Superb       |
        | Sup 2 | Supplier A   |
        | Sup 1 | Supplier B   |

@4840
Scenario: 4. All suppliers are returned when no name and solution publication status are supplied
    Given the user has searched for suppliers matching ''
    And the user has searched for suppliers with solutions matching the publication status ''
    When a GET request is made for suppliers
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id    | SupplierName |
        | Sup 3 | Superb       |
        | Sup 2 | Supplier A   |
        | Sup 1 | Supplier B   |

@4840
Scenario: 5. No suppliers are returned when none match the name supplied
    Given the user has searched for suppliers matching 'InvisibleSupplier'
    When a GET request is made for suppliers
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id         | SupplierName |

@4840
Scenario: 6. Only suppliers matching the full name supplied are returned
    Given the user has searched for suppliers matching 'Supplier A'
    When a GET request is made for suppliers
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id    | SupplierName |
        | Sup 2 | Supplier A   |

@4840
Scenario: 7. Only suppliers matching the first part of the name supplied are returned
    Given the user has searched for suppliers matching 'Supplier'
    When a GET request is made for suppliers
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id    | SupplierName |
        | Sup 2 | Supplier A   |
        | Sup 1 | Supplier B   |

@4840
Scenario: 8. Only suppliers matching the partial name supplied are returned
    Given the user has searched for suppliers matching 'a'
    When a GET request is made for suppliers
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id    | SupplierName |
        | Sup 2 | Supplier A   |

@4840
Scenario: 9. Only suppliers matching solution publication status supplied are returned
    Given the user has searched for suppliers with solutions matching the publication status 'Published'
    When a GET request is made for suppliers
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id    | SupplierName |
        | Sup 2 | Supplier A   |
        | Sup 1 | Supplier B   |

@4840
Scenario: 10. Only suppliers matching the name and solution publication status supplied are returned
    Given the user has searched for suppliers matching 'Supplier B'
    And the user has searched for suppliers with solutions matching the publication status 'Published'
    When a GET request is made for suppliers
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id    | SupplierName |
        | Sup 1 | Supplier B   |

@4840
Scenario: 11. All suppliers with published solutions are returned when limited to published solutions
    Given the user has limited the search to suppliers with published solutions
    When a GET request is made for suppliers
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id    | SupplierName |
        | Sup 2 | Supplier A   |
        | Sup 1 | Supplier B   |

@4840
Scenario: 12. All suppliers are returned when not limited to published solutions and no name and solution publication status are supplied
    Given the user has searched for suppliers matching ''
    And the user has searched for suppliers with solutions matching the publication status ''
    And the user has not limited the search to suppliers with published solutions
    When a GET request is made for suppliers
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id    | SupplierName |
        | Sup 3 | Superb       |
        | Sup 2 | Supplier A   |
        | Sup 1 | Supplier B   |

@4840
Scenario: 13. Only matching suppliers with published solutions are returned when limited to published solutions and solution publication status is supplied
    Given the user has searched for suppliers matching 'Sup'
    And the user has searched for suppliers with solutions matching the publication status 'Withdrawn'
    And the user has limited the search to suppliers with published solutions
    When a GET request is made for suppliers
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id    | SupplierName |
        | Sup 2 | Supplier A   |
        | Sup 1 | Supplier B   |

@4840
Scenario: 14. Only matching suppliers with published solutions are returned when limited to published solutions
    Given the user has searched for suppliers matching 'Sup'
    And the user has limited the search to suppliers with published solutions
    When a GET request is made for suppliers
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id    | SupplierName |
        | Sup 2 | Supplier A   |
        | Sup 1 | Supplier B   |

@4840
Scenario: 15. A bad response is returned when an invalid publication status is supplied
    Given the user has searched for suppliers with solutions matching the publication status 'Incorrect'
    When a GET request is made for suppliers
    Then a response status of 400 is returned
