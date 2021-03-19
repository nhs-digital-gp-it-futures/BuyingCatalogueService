Feature:  Supplier Edit Browser Mobile First
    As a Supplier
    I want to Edit the Browser Mobile First Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName | SupplierId |
        | Sln1       | MedicOnline  | Sup 1      |
    And framework solutions exist
        | SolutionId | IsFoundation | FrameworkId |
        | Sln1       | true         | NHSDGP001   |

@3602
Scenario: Browser Mobile First is updated
    Given solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication              |
        | Sln1       | An full online medicine system | Online medicine 1 | { "MobileFirstDesign": false } |
    When a PUT request is made to update the browser-mobile-first section for solution Sln1
        | MobileFirstDesign |
        | YEs               |
    Then a successful response is returned
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                     |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported" : [], "MobileFirstDesign": true } |

@3602
Scenario: Browser Mobile First is updated with trimmed whitespace
    Given solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication              |
        | Sln1       | An full online medicine system | Online medicine 1 | { "MobileFirstDesign": false } |
    When a PUT request is made to update the browser-mobile-first section for solution Sln1
        | MobileFirstDesign  |
        | "    YEs         " |
    Then a successful response is returned
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                     |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported" : [], "MobileFirstDesign": true } |

@3602
Scenario: Solution is not found
    Given a Solution Sln2 does not exist
    When a PUT request is made to update the browser-mobile-first section for solution Sln2
       | MobileFirstDesign |
       | no                |
    Then a response status of 404 is returned

@3602
Scenario: Service Failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the browser-mobile-first section for solution Sln1
        | MobileFirstDesign |
        | nO                |
    Then a response status of 500 is returned

@3602
Scenario: Solution id is not present in the request
    When a PUT request is made to update the browser-mobile-first section with no solution id
        | MobileFirstDesign |
        | YeS               |
    Then a response status of 400 is returned
