﻿Feature: Search for supplier by name
    As a buyer
    I want to search for a supplier
    So that I can select the supplier to use on the order form

Background:
    Given Suppliers exist
        | Id    | SupplierName     |
        | Sup 1 | Supplier B       |
        | Sup 2 | Supplier A       |
        | Sup 3 | Superb           |
        | Sup 4 | % Su_p[p&l=ie+r- |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierStatusId | SupplierId | PublishedStatus |
        | Sln1       | MedicOnline    | 3                | Sup 1      | Published       |
        | Sln2       | TakeTheRedPill | 3                | Sup 2      | Published       |
        | Sln3       | PracticeMgr    | 3                | Sup 3      | Withdrawn       |
        | Sln4       | PracticeMg2    | 3                | Sup 4      | Withdrawn       |

@4840
Scenario: All suppliers are returned when there are no query parameters
    When a GET request is made for suppliers
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id    | SupplierName     |
        | Sup 3 | Superb           |
        | Sup 2 | Supplier A       |
        | Sup 1 | Supplier B       |
        | Sup 4 | % Su_p[p&l=ie+r- |

@4840
Scenario: All suppliers are returned when no name is supplied
    Given the user has searched for suppliers matching ''
    When a GET request is made for suppliers
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id    | SupplierName     |
        | Sup 3 | Superb           |
        | Sup 2 | Supplier A       |
        | Sup 1 | Supplier B       |
        | Sup 4 | % Su_p[p&l=ie+r- |

@4840
Scenario: All suppliers are returned when no solution publication status is supplied
    Given the user has searched for suppliers with solutions matching the publication status ''
    When a GET request is made for suppliers
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id    | SupplierName     |
        | Sup 3 | Superb           |
        | Sup 2 | Supplier A       |
        | Sup 1 | Supplier B       |
        | Sup 4 | % Su_p[p&l=ie+r- |

@4840
Scenario: All suppliers are returned when no name and solution publication status are supplied
    Given the user has searched for suppliers matching ''
    And the user has searched for suppliers with solutions matching the publication status ''
    When a GET request is made for suppliers
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id    | SupplierName     |
        | Sup 3 | Superb           |
        | Sup 2 | Supplier A       |
        | Sup 1 | Supplier B       |
        | Sup 4 | % Su_p[p&l=ie+r- |

@4840
Scenario: No suppliers are returned when none match the name supplied
    Given the user has searched for suppliers matching 'InvisibleSupplier'
    When a GET request is made for suppliers
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id         | SupplierName |

@4840
Scenario: Only suppliers matching the full name supplied are returned
    Given the user has searched for suppliers matching 'Supplier A'
    When a GET request is made for suppliers
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id    | SupplierName |
        | Sup 2 | Supplier A   |

@4840
Scenario: Only suppliers matching the first part of the name supplied are returned
    Given the user has searched for suppliers matching 'Supplier'
    When a GET request is made for suppliers
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id    | SupplierName |
        | Sup 2 | Supplier A   |
        | Sup 1 | Supplier B   |

@4840
Scenario: Only suppliers matching the partial name supplied are returned
    Given the user has searched for suppliers matching 'a'
    When a GET request is made for suppliers
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id    | SupplierName |
        | Sup 2 | Supplier A   |

@4840
Scenario: Only suppliers matching solution publication status supplied are returned
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
Scenario: 11. A bad response is returned when an invalid publication status is supplied
    Given the user has searched for suppliers with solutions matching the publication status 'Incorrect'
    When a GET request is made for suppliers
    Then a response status of 400 is returned

@4840
Scenario Outline: 12. Suppliers with a special character in the name are returned
    Given the user has searched for suppliers matching '<Search>'
    When a GET request is made for suppliers
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id           | SupplierName   |
        | <SupplierId> | <SupplierName> |
    Examples:
        | Search | SupplierId | SupplierName     |
        | %      | Sup 4      | % Su_p[p&l=ie+r- |
        | _      | Sup 4      | % Su_p[p&l=ie+r- |
        | [      | Sup 4      | % Su_p[p&l=ie+r- |
        | &      | Sup 4      | % Su_p[p&l=ie+r- |
        | =      | Sup 4      | % Su_p[p&l=ie+r- |
        | +      | Sup 4      | % Su_p[p&l=ie+r- |
        | -      | Sup 4      | % Su_p[p&l=ie+r- |

@4840
Scenario: 13. Only one result per supplier is returned when a supplier has multiple catalogue items
    Given AdditionalService exist
        | CatalogueItemId | CatalogueItemName                 | CatalogueSupplierId | Summary                    | SolutionId |
        | Sup1-Sln1A001   | MedicOnline Additional Service 1  | Sup 1               | Addition to MedicOnline    | Sln1       |
        | Sup2-Sln2A001   | TakeTheRedPill Additional Service | Sup 2               | Addition to TakeTheRedPill | Sln2       |
    And the user has searched for suppliers matching 'Supplier'
    When a GET request is made for suppliers
    Then a successful response is returned
    And a list of suppliers is returned with the following values
        | Id    | SupplierName |
        | Sup 2 | Supplier A   |
        | Sup 1 | Supplier B   |
