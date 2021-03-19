Feature:  Display Marketing Page Form Features Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Features
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
        | Sup 2 | Supplier 2   |
    And Solutions exist
        | SolutionId | SolutionName   | SupplierId |
        | Sln1       | MedicOnline    | Sup 1      |
        | Sln2       | TakeTheRedPill | Sup 2      |
        | Sln3       | PracticeMgr    | Sup 2      |
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription     | Features                          |
        | Sln1       | An full online medicine system | Online medicine 1   | [ "Appointments", "Prescribing" ] |
        | Sln3       | Eye opening experience         | Eye opening6        |                                   |
    And framework solutions exist
        | SolutionId | IsFoundation | FrameworkId |
        | Sln1       | true         | NHSDGP001   |
        | Sln2       | false        | DFOCVC001   |
        | Sln3       | false        | NHSDGP001   |

@2724
Scenario: Features are retrieved for the solution
    When a GET request is made for features section for solution Sln1
    Then a successful response is returned
    And the listing element contains
        | Elements                  |
        | Appointments, Prescribing |

@2724
Scenario: Features are retrieved for the solution where no solution detail exists
    When a GET request is made for features section for solution Sln2
    Then a successful response is returned
    And the listing element contains
    | Elements |
    |          |

@2724
Scenario: Features are retrieved for the solution where no features exist
    When a GET request is made for features section for solution Sln3
    Then a successful response is returned
    And the listing element contains
    | Elements |
    |          |

@2726
Scenario: Solution not found
    Given a Solution Sln4 does not exist
    When a GET request is made for features section for solution Sln4
    Then a response status of 404 is returned

@2726
Scenario: Service failure
    Given the call to the database to set the field will fail
    When a GET request is made for features section for solution Sln1
    Then a response status of 500 is returned

@2726
Scenario: Solution id not present in request
    When a GET request is made for features section with no solution id
    Then a response status of 400 is returned
