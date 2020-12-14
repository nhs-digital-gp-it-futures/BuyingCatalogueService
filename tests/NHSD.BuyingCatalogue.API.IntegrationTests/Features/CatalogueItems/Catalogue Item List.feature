Feature: Catalogue Item List
    As a public user
    I want to be able to retrieve a list of catalogue items
    So that I can check the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
        | Sup 2 | Supplier 2   |
    And CatalogueItems exist
        | CatalogueItemId | CatalogueItemType | Name                     | SupplierId | PublishedStatus |
        | 100000-001A001  | AdditionalService | Additional Service 1     | Sup 1      | Published       |
        | 100000-001A002  | Solution          | Solution 1               | Sup 1      | Published       |
        | 100000-002      | Solution          | Solution 2               | Sup 1      | Draft           |
        | 100000-002A001  | AdditionalService | Additional Service 3     | Sup 1      | Draft           |
        | 100000-001A003  | AssociatedService | Associated Service 1     | Sup 1      | Published       |
        | 100000-001A004  | Solution          | Solution 2               | Sup 1      | Published       |
        | 100001-002A001  | Solution          | Solution Red 1           | Sup 2      | Published       |
        | 100001-002A002  | Solution          | Solution Red 2           | Sup 2      | Published       |
        | 100001-003      | Solution          | Solution Red 3           | Sup 2      | Draft           |
        | 100001-002A003  | AssociatedService | Associated Service Red 1 | Sup 2      | Published       |
        | 100001-002A004  | AssociatedService | Associated Service Red 2 | Sup 2      | Published       |
        | 100001-003A001  | AssociatedService | Associated Service Red 3 | Sup 2      | Draft           |

@7264
Scenario: Get a list of catalogue items
    When a Get request is made to retrieve a list of catalogue items with supplierId <SupplierId> and catalogueItemType <CatalogueItemType> and publishedStatus <PublishedStatus>
    Then a successful response is returned
    And the response contains a list of catalogue item details filtered by <SupplierId> and <CatalogueItemType> and <PublishedStatus>

    Examples:
        | SupplierId | CatalogueItemType | PublishedStatus |
        | Sup 1      | Solution          | NULL            |
        | Sup 1      | AdditionalService | NULL            |
        | Sup 1      | AssociatedService | NULL            |
        | Sup 2      | Solution          | NULL            |
        | Sup 2      | AdditionalService | NULL            |
        | Sup 2      | AssociatedService | NULL            |
        | NULL       | Solution          | NULL            |
        | NULL       | AdditionalService | NULL            |
        | NULL       | AssociatedService | NULL            |
        | Sup 1      | NULL              | NULL            |
        | NULL       | NULL              | NULL            |
        | NULL       | NULL              | Published       |
        | NULL       | Solution          | Published       |
        | Sup 1      | Solution          | Published       |
        | Sup 1      | AdditionalService | Published       |
        | Sup 2      | Solution          | Published       |
        | Sup 2      | AssociatedService | Published       |

@7264
Scenario: Getting a list of catalogue items by filtering by an non-existant supplier ID, returns an empty list
    When a Get request is made to retrieve a list of catalogue items with supplierId INVALID and catalogueItemType NULL and publishedStatus NULL
    Then a successful response is returned
    And an empty catalogue items list is returned

@7264
Scenario: An invalid query parameter returns a validation error
    When a Get request is made to retrieve a list of catalogue items with supplierId <SupplierId> and catalogueItemType <CatalogueItemType> and publishedStatus <PublishedStatus>
    Then a response status of 400 is returned

    Examples:
        | SupplierId | CatalogueItemType | PublishedStatus |
        | NULL       | Invalid           | NULL            |
        | NULL       | NULL              | Invalid         |

@7264
Scenario: Service failure
    Given the call to the database to set the field will fail
    When a Get request is made to retrieve a list of catalogue items with supplierId NULL and catalogueItemType Solution and publishedStatus NULL
    Then a response status of 500 is returned
