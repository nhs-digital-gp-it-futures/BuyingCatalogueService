Feature:  Supplier Edit Browser Hardware Requirements
    As a Supplier
    I want to Edit the Browser Support Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName | SupplierId |
        | Sln1       | MedicOnline  | Sup 1      |

@3600
Scenario: Browser Hardware Requirements is updated
    Given solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                  |
        | Sln1       | An full online medicine system | Online medicine 1 | { "HardwareRequirements": "New Hardware", "ClientApplicationTypes": [], "BrowsersSupported" : [] } |
    When a PUT request is made to update the browser-hardware-requirements section for solution Sln1
        | HardwareRequirements |
        | New Hardware         |
    Then a successful response is returned
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                 |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [],"BrowsersSupported" : [], "HardwareRequirements": "New Hardware" } |

@3600
Scenario: Browser Hardware Requirements is updated with trimmed whitespace
    Given solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                  |
        | Sln1       | An full online medicine system | Online medicine 1 | { "HardwareRequirements": "New Hardware", "ClientApplicationTypes": [], "BrowsersSupported" : [] } |
    When a PUT request is made to update the browser-hardware-requirements section for solution Sln1
        | HardwareRequirements  |
        | "     New Hardware  " |
    Then a successful response is returned
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                 |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [],"BrowsersSupported" : [], "HardwareRequirements": "New Hardware" } |

@3600
Scenario: Solution is not found
    Given a Solution Sln2 does not exist
    When a PUT request is made to update the browser-hardware-requirements section for solution Sln2
       | HardwareRequirements      |
       | New Hardware Requirements |
    Then a response status of 404 is returned

@3600
Scenario: Service Failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the browser-hardware-requirements section for solution Sln1
        | HardwareRequirements      |
        | New Hardware Requirements |
    Then a response status of 500 is returned

@3600
Scenario: Solution id is not present in the request
    When a PUT request is made to update the browser-hardware-requirements section with no solution id
        | HardwareRequirements      |
        | New Hardware Requirements |
    Then a response status of 400 is returned
