Feature: Display Marketing Page Preview Solution Description Section
	As a Supplier
    I want to manage Marketing Page Information for the About Solution + Summary Description Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
        | Sup 2 | Supplier 2   |
    And Solutions exist
        | SolutionID | SolutionName   | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | GPs-R-Us         | 1                | Sup 1      |
        | Sln2       | TakeTheRedPill | Drs. Inc         | 1                | Sup 2      |
        | Sln3       | PracticeMgr    | Drs. Inc         | 1                | Sup 2      |
    And SolutionDetail exist
        | Solution | AboutUrl | SummaryDescription      | FullDescription     | Features                          |
        | Sln1     | UrlSln1  |                         | Online medicine 1   | [ "Appointments", "Prescribing" ] |
        | Sln3     | UrlSln3  | Fully fledged GP system | Fully fledged GP 12 | [ "Referrals", "Workflow" ]       |

@1848
Scenario: 1. Solution description section presented where Solution Detail exists
    When a GET request is made for solution preview Sln3
    Then a successful response is returned
    And the solution solution-description section contains summary of Fully fledged GP system
    And the solution solution-description section contains description of Fully fledged GP 12
    And the solution solution-description section contains link of UrlSln3
    And the solution features section contains Features
        | Feature   |
        | Referrals |
        | Workflow  |

@1848
Scenario: 2. Solution description section presented where no Solution Detail exists
    When a GET request is made for solution preview Sln2
    Then a successful response is returned
    And the solution solution-description section does not contain summary
    And the solution solution-description section does not contain description
    And the solution solution-description section does not contain link
    And the solution features section contains no features
