Feature: Mobile Connection Details
    As a Supplier
    I want to Edit the Mobile Connection Details section
    So that I can make sure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName | SupplierId |
        | Sln1       | MedicOnline  | Sup 1      |
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1       | An full online medicine system | Online medicine 1 | {}                |

@3606
Scenario: Client Application is updated for the solution
    When a PUT request is made to update the native-mobile-connection-details section for solution Sln1
        | MinimumConnectionSpeed | ConnectionRequirementsDescription | ConnectionType        |
        | 1GBps                  | A description                     | Horse, Moose, Giraffe |
    Then a successful response is returned
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                              |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported": [], "MobileConnectionDetails": { "MinimumConnectionSpeed": "1GBps", "ConnectionType": [ "Horse", "Moose", "Giraffe" ], "Description": "A description" } } |
        
@3606
Scenario: Client Application is updated for the solution with trimmed whitespace
    When a PUT request is made to update the native-mobile-connection-details section for solution Sln1
        | MinimumConnectionSpeed          | ConnectionRequirementsDescription | ConnectionType                               |
        | "          1GBps              " | "       A description        "    | "     Horse", "   Moose    ", "Giraffe     " |
    Then a successful response is returned
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                              |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported": [], "MobileConnectionDetails": { "MinimumConnectionSpeed": "1GBps", "ConnectionType": [ "Horse", "Moose", "Giraffe" ], "Description": "A description" } } |
        
@3606
Scenario: Client Application is updated with no Minimum Connection Speed
    When a PUT request is made to update the native-mobile-connection-details section for solution Sln1
        | MinimumConnectionSpeed | ConnectionRequirementsDescription | ConnectionType        |
        | NULL                   | A description                     | Horse, Moose, Giraffe |
    Then a successful response is returned
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                           |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported": [], "MobileConnectionDetails": { "ConnectionType": [ "Horse", "Moose", "Giraffe" ], "Description": "A description" } } |
        
@3606
Scenario: Client Application is updated with no Connection Requirements Description
    When a PUT request is made to update the native-mobile-connection-details section for solution Sln1
        | MinimumConnectionSpeed | ConnectionRequirementsDescription | ConnectionType        |
        | 1GBps                  | NULL                              | Horse, Moose, Giraffe |
    Then a successful response is returned
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                              |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported": [], "MobileConnectionDetails": { "MinimumConnectionSpeed": "1GBps", "ConnectionType": [ "Horse", "Moose", "Giraffe" ] } } |

@3606
Scenario: Client Application is updated with no Connection Type
    When a PUT request is made to update the native-mobile-connection-details section for solution Sln1
        | MinimumConnectionSpeed | ConnectionRequirementsDescription | ConnectionType |
        | 1GBps                  | A description                     |                |
    Then a successful response is returned
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                 |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported": [], "MobileConnectionDetails": { "MinimumConnectionSpeed": "1GBps", "ConnectionType": [], "Description": "A description" } } |
@3606
Scenario: Solution is not found
    Given a Solution Sln2 does not exist
    When a PUT request is made to update the native-mobile-connection-details section for solution Sln2
    | MinimumConnectionSpeed | ConnectionRequirementsDescription | ConnectionType        |
    | NULL                   | A description                     | Horse, Moose, Giraffe |
    Then a response status of 404 is returned 

@3606
Scenario: Service Failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the native-mobile-connection-details section for solution Sln1
    | MinimumConnectionSpeed | ConnectionRequirementsDescription | ConnectionType        |
    | NULL                   | A description                     | Horse, Moose, Giraffe |
    Then a response status of 500 is returned

@3606
Scenario: Solution id is not present in the request
    When a PUT request is made to update the native-mobile-connection-details section with no solution id
    | MinimumConnectionSpeed | ConnectionRequirementsDescription | ConnectionType        |
    | NULL                   | A description                     | Horse, Moose, Giraffe |
    Then a response status of 400 is returned
