Feature:  Supplier Edit Native Mobile First
    As an Authority User
    I want to edit the Mobile First Sub-Section
    So that I can make sure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionID | SolutionName   | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | GPs-R-Us         | 1                | Sup 1      |
@3602
Scenario: 1. Native Mobile First is updated
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1     | An full online medicine system | Online medicine 1 | { }               |
    When a PUT request is made to update the native-mobile-first section for solution Sln1
        | MobileFirstDesign |
        | YEs               |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                           |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported" : [], "NativeMobileFirstDesign": true } |
        
@3602
Scenario: 2. Native Mobile First is updated with trimmed whitespace
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication |
        | Sln1     | An full online medicine system | Online medicine 1 | { }               |
    When a PUT request is made to update the native-mobile-first section for solution Sln1
        | MobileFirstDesign   |
        | "      YEs        " |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                           |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported" : [], "NativeMobileFirstDesign": true } |

@3602
Scenario: 3. Solution is not found
    Given a Solution Sln2 does not exist
    When a PUT request is made to update the native-mobile-first section for solution Sln2
       | MobileFirstDesign |
       | no                |
    Then a response status of 404 is returned 

@3602
Scenario: 4. Service Failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the native-mobile-first section for solution Sln1
        | MobileFirstDesign |
        | nO                |
    Then a response status of 500 is returned

@3602
Scenario: 5. Solution id is not present in the request
    When a PUT request is made to update the native-mobile-first section with no solution id
        | MobileFirstDesign |
        | YeS               |
    Then a response status of 400 is returned
