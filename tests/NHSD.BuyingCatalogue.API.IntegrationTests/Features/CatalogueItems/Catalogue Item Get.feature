Feature: Catalogue Items Get
    As a Public User
    I want to be able to retrieve the details of a catalogue item
    So that I know the correct information

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName  | SupplierId |
        | 100000-001 | Write on Time | Sup 1      |
    And AdditionalService exist
        | CatalogueItemId | CatalogueItemName                  | CatalogueSupplierId | Summary                   | SolutionId |
        | 100000-001A001  | Write on Time Additional Service 1 | Sup 1               | Addition to Write on Time | 100000-001 |

@7264
Scenario: Get a single catalogue item
    When a GET request is made to retrieve a catalogue item with ID '<CatalogueItemId>'
    Then a successful response is returned
    And the response contains the catalogue item details
        | CatalogueItemId   | Name   |
        | <CatalogueItemId> | <Name> |

    Examples:
        | CatalogueItemId | Name                               |
        | 100000-001      | Write on Time                      |
        | 100000-001A001  | Write on Time Additional Service 1 |

@7264
Scenario: Get a catalogue item that does not exist
    Given a catalogue item with ID '<CatalogueItemId>' does not exist
    When a GET request is made to retrieve a catalogue item with ID '<CatalogueItemId>'
    Then a response status of 404 is returned

    Examples:
        | CatalogueItemId |
        | 100000-002      |
        | 100000-001A002  |

@7264
Scenario: Service failure
    Given the call to the database to set the field will fail
    When a GET request is made to retrieve a catalogue item with ID '<CatalogueItemId>'
    Then a response status of 500 is returned

    Examples:
        | CatalogueItemId |
        | 100000-001      |
        | 100000-001A001  |
