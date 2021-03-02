Feature:  Supplier Edit Browser Additional Information
    As a Supplier
    I want to Edit the Browser Additional Information Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName | SupplierId |
        | Sln1       | MedicOnline  | Sup 1      |

@3601
Scenario: Browser Additional Information is updated
    Given solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                        |
        | Sln1       | An full online medicine system | Online medicine 1 | { "AdditionalInformation": "Some Info" } |
    When a PUT request is made to update the browser-additional-information section for solution Sln1
        | AdditionalInformation |
        | New Additional Info   |
    Then a successful response is returned
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                          |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported" : [], "AdditionalInformation": "New Additional Info" } |

@3601
Scenario: Browser Additional Information is updated with trimmed whitespace
    Given solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                        |
        | Sln1       | An full online medicine system | Online medicine 1 | { "AdditionalInformation": "Some Info" } |
    When a PUT request is made to update the browser-additional-information section for solution Sln1
        | AdditionalInformation         |
        | "      New Additional Info  " |
    Then a successful response is returned
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                          |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported" : [], "AdditionalInformation": "New Additional Info" } |

@3601
Scenario: Solution is not found
    Given a Solution Sln2 does not exist
    When a PUT request is made to update the browser-additional-information section for solution Sln2
        | AdditionalInformation |
        | New Additional Info   |
    Then a response status of 404 is returned

@3601
Scenario: Service Failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the browser-additional-information section for solution Sln1
        | AdditionalInformation |
        | New Additional Info   |
    Then a response status of 500 is returned

@3601
Scenario: Solution id is not present in the request
    When a PUT request is made to update the browser-additional-information section with no solution id
        | AdditionalInformation |
        | New Additional Info   |
    Then a response status of 400 is returned
