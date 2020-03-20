Feature: Display Native Desktop Operating Systems Section
    As a Supplier
    I want to Get the Native Desktop Operating Systems Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | 1                | Sup 1      |
        | Sln3       | PracticeMgr    | 1                | Sup 1      |
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                               |
        | Sln1     | An full online medicine system | Online medicine 1 | { "NativeDesktopOperatingSystemsDescription" : "Works fine on Windows, barely on Mac and blows up on *nix..." } |
        | Sln3     | Testing System                 | Full System       | {  }                                                                                                            |

@3617
Scenario: 1. Desktop Operating Systems Description is retrieved for the solution
    When a GET request is made for native-desktop-operating-systems section for solution Sln1
    Then a successful response is returned
    And the string value of element operating-systems-description is Works fine on Windows, barely on Mac and blows up on *nix...
  
@3617
Scenario: 2. Desktop Operating Systems Description is retrieved for the solution where no solution detail exists
    When a GET request is made for native-desktop-operating-systems section for solution Sln2
    Then a successful response is returned
    And the operating-systems-description string does not exist

@3617
Scenario: 3. Solution not found
    Given a Solution Sln4 does not exist
    When a GET request is made for native-desktop-operating-systems section for solution Sln4
    Then a response status of 404 is returned

@3617
Scenario: 4. Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for native-desktop-operating-systems section for solution Sln1
    Then a response status of 500 is returned

@3617
Scenario: 5. Solution id not present in request
    When a GET request is made for native-desktop-operating-systems section with no solution id
    Then a response status of 400 is returned
    
@3617
Scenario: 6. Desktop Operating Systems Description is retrieved as empty if it does not exist yet
    When a GET request is made for native-desktop-operating-systems section for solution Sln3
    Then a successful response is returned
    And the  operating-systems-description string does not exist
