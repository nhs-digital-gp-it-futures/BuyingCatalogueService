Feature: Get List of Additional Services
    As a Public User
    I want to be able to retrieve additional services from a list of solutionIDs
    So that I know the correct information

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName      | SupplierId |
        | 100000-001 | Write on Time     | Sup 1      |
        | 100000-002 | Take the Red Pill | Sup 1      |
    And AdditionalService exist
        | CatalogueItemId | CatalogueItemName                  | CatalogueSupplierId | Summary                   | SolutionId |
        | 100000-001A001  | Write on Time Additional Service 1 | Sup 1               | Addition to Write on Time | 100000-001 |
        | 100000-002A001  | Red Pill Additional Service        | Sup 1               | Addition to Red Pill      | 100000-002 |

@5352
Scenario: 1. Get a single additional service from a solutionID
    When a Get request is made to retrieve the additional services with solutionIds
        | SolutionId |
        | 100000-001 |
    Then a successful response is returned
    And Additional Services are returned
        | AdditionalServiceId | Name                               | Summary                   | SolutionId | SolutionName  |
        | 100000-001A001      | Write on Time Additional Service 1 | Addition to Write on Time | 100000-001 | Write on Time |

@5352
Scenario: 2. Get multiple additional services from solutionIDs
    When a Get request is made to retrieve the additional services with solutionIds
        | SolutionId |
        | 100000-001 |
        | 100000-002 |
    Then a successful response is returned
    And Additional Services are returned
        | AdditionalServiceId | Name                               | Summary                   | SolutionId | SolutionName      |
        | 100000-001A001      | Write on Time Additional Service 1 | Addition to Write on Time | 100000-001 | Write on Time     |
        | 100000-002A001      | Red Pill Additional Service        | Addition to Red Pill      | 100000-002 | Take the Red Pill |

@5352
Scenario: 3. Providing no solution IDs returns not found
    When a Get request is made to retrieve the additional services with solutionIds
        | SolutionId |
    Then a response status of 404 is returned

@5352
Scenario: 4. Providing an invalid solution ID, returns an empty list
    When a Get request is made to retrieve the additional services with solutionIds
        | SolutionId |
        | INVALID    |
    Then a successful response is returned
    And an empty list is returned

@5352
Scenario: 5. Service failure
    Given the call to the database to set the field will fail
    When a Get request is made to retrieve the additional services with solutionIds
        | SolutionId |
        | 100000-001 |
    Then a response status of 500 is returned
