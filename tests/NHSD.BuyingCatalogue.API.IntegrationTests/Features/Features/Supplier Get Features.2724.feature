Feature:  Display Marketing Page Form Features Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Features
    So that I can ensure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
        | Drs. Inc |
    And Suppliers exist
        | Id    | SupplierName | OrganisationName |
        | Sup 1 | Supplier 1   | GPs-R-Us         |
        | Sup 2 | Supplier 2   | Drs. Inc         |
    And Solutions exist
        | SolutionID | SolutionName   | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | GPs-R-Us         | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | Drs. Inc         | 1                | Sup 2      |
        | Sln3       | PracticeMgr    | Drs. Inc         | 1                | Sup 2      |
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription     | Features                          |
        | Sln1     | An full online medicine system | Online medicine 1   | [ "Appointments", "Prescribing" ] |
        | Sln3     | Eye opening experience         | Eye opening6        |                                   |
      

@2724
Scenario: 1. Features are retrieved for the solution
    When a GET request is made for features for solution Sln1
    Then a successful response is returned
    And the listing element contains
        | Elements                  |
        | Appointments, Prescribing |

@2724
Scenario: 2. Features are retrieved for the solution where no solution detail exists
    When a GET request is made for features for solution Sln2
    Then a successful response is returned
    And the listing element contains
    | Elements |
    |          |

@2724
Scenario: 3.Features are retrieved for the solution where no features exist
    When a GET request is made for features for solution Sln3
    Then a successful response is returned
    And the listing element contains
    | Elements |
    |          |

@2726
Scenario: 4. Solution not found
    Given a Solution Sln4 does not exist
    When a GET request is made for features for solution Sln4
    Then a response status of 404 is returned

@2726
Scenario: 5. Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for features for solution Sln1
    Then a response status of 500 is returned

@2726
Scenario: 6. Solution id not present in request
    When a GET request is made for features with no solution id
    Then a response status of 400 is returned
