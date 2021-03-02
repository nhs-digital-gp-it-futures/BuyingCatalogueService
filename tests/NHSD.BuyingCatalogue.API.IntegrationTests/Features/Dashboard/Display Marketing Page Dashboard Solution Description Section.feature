Feature: Display Marketing Page Dashboard Solution Description Section
    As a Supplier
    I want to manage Marketing Page Information for the About Solution + Summary Description Section
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
        | SolutionId | AboutUrl| SummaryDescription      | FullDescription      | Features                          |
        | Sln1       | UrlSln1 |                         | Online medicine 1    | [ "Appointments", "Prescribing" ] |
        | Sln3       | UrlSln3 | Eye opening experience  | Eye opening6         | [ "Referrals", "Workflow" ]       |

@1848
Scenario Outline: Solution description section is mandatory and is reported complete if there is text in the summary
    When a GET request is made for solution dashboard <SolutionId>
    Then a successful response is returned
    And the solution solution-description section status is <Status>
    And the solution solution-description section requirement is Mandatory
Examples:
    | SolutionId | Status     |
    | Sln1       | INCOMPLETE |
    | Sln2       | INCOMPLETE |
    | Sln3       | COMPLETE   |
