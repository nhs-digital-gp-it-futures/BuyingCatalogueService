Feature:  Supplier Edit Native Mobile Additional Information
    As a Supplier
    I want to Edit the Native Mobile Additional Information Section
    So that I can ensure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
    And Suppliers exist
        | Id    | SupplierName | OrganisationName |
        | Sup 1 | Supplier 1   | GPs-R-Us         |
    And Solutions exist
        | SolutionID | SolutionName   | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | GPs-R-Us         | 1                | Sup 1      |

@3610
Scenario: 1. Native Mobile Additional Information is updated
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1     | An full online medicine system | Online medicine 1 | { }               |
    When a PUT request is made to update the native-mobile-additional-information section for solution Sln1
        | AdditionalInformation |
        | New Additional Info   |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                     |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported": [], "NativeMobileAdditionalInformation": "New Additional Info" } |
        
@3610
Scenario: 2. Native Mobile Additional Information is updated with trimmed whitespace
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1     | An full online medicine system | Online medicine 1 | { }               |
    When a PUT request is made to update the native-mobile-additional-information section for solution Sln1
        | AdditionalInformation        |
        | "     New Additional Info  " |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                     |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported": [], "NativeMobileAdditionalInformation": "New Additional Info" } |

@3610
Scenario: 3. Solution is not found
    Given a Solution Sln2 does not exist
    When a PUT request is made to update the native-mobile-additional-information section for solution Sln2
       | AdditionalInformation |
       | New Additional Info   |
    Then a response status of 404 is returned 

@3610
Scenario: 4 Service Failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the native-mobile-additional-information section for solution Sln1
        | AdditionalInformation |
        | New Additional Info   |
    Then a response status of 500 is returned

@3610
Scenario: 5. Solution id is not present in the request
    When a PUT request is made to update the native-mobile-additional-information section with no solution id
        | AdditionalInformation |
        | New Additional Info   |
    Then a response status of 400 is returned

@3610
Scenario: 6. AdditionalInformation is set to null
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                               |
        | Sln1     | An full online medicine system | Online medicine 1 | { "NativeMobileAdditionalInformation": "Some additional info" } |
    When a PUT request is made to update the native-mobile-additional-information section for solution Sln1
        | AdditionalInformation |
        | NULL                  |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                         |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported": [] } |
