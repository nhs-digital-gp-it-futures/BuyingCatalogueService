Feature: Catalogue Item List
    As a Public User
    I want to be able to retrieve a list of catalogue items
    So that I can check the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
        | Sup 2 | Supplier 2   |
    And CatalogueItems exist
        | CatalogueItemId | CatalogueItemType | Name                     | SupplierId |
        | 100000-001A001  | AdditionalService | Additional Service 1     | Sup 1      |
        | 100000-001A002  | Solution          | Solution 1               | Sup 1      |
        | 100000-001A003  | AssociatedService | Associated Service 1     | Sup 1      |
        | 100000-001A004  | Solution          | Solution 2               | Sup 1      |
        | 100000-002A001  | Solution          | Solution Red 1           | Sup 2      |
        | 100000-002A002  | Solution          | Solution Red 2           | Sup 2      |
        | 100000-002A003  | AssociatedService | Associated Service Red 1 | Sup 2      |
        | 100000-002A004  | AssociatedService | Associated Service Red 2 | Sup 2      |

@7264
Scenario: 1. Get a list of catalogue items
    When a Get request is made to retrieve a list of catalogue items with supplierId <SupplierId> and catalogueItemType <CatalogueItemType>
    Then a successful response is returned
    And the response contains a list of catalogue item details filtered by <SupplierId> and <CatalogueItemType>

    Examples:
        | SupplierId | CatalogueItemType |
        | Sup 1      | Solution          |
        | Sup 1      | AdditionalService |
        | Sup 1      | AssociatedService |
        | Sup 2      | Solution          |
        | Sup 2      | AdditionalService |
        | Sup 2      | AssociatedService |
        | NULL       | Solution          |
        | NULL       | AdditionalService |
        | NULL       | AssociatedService |
        | Sup 1      | NULL              |
        | NULL       | NULL              |

@7264
Scenario: 2. Getting a list of catalogue items by filtering by an non-existant supplier ID, returns an empty list
    When a Get request is made to retrieve a list of catalogue items with supplierId INVALID and catalogueItemType NULL
    Then a successful response is returned
    And an empty catalogue items list is returned

@7264
Scenario: 3. Service failure
    Given the call to the database to set the field will fail
    When a Get request is made to retrieve a list of catalogue items with supplierId NULL and catalogueItemType Solution
    Then a response status of 500 is returned
