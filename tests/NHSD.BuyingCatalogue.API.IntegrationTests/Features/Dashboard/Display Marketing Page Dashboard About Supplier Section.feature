Feature: Display Marketing Page Dashboard About Supplier Section
    As an Authority User
    I want to manage Marketing Page Information for the About Supplier
    So that I can ensure the information is correct

@3652
Scenario Outline: About supplier section is optional and is reported complete if there is text
    Given Suppliers exist
        | Id    | SupplierName | Summary   | SupplierUrl   |
        | Sup 1 | Supplier 1   | <Summary> | <SupplierUrl> |
    And Solutions exist
        | SolutionId | SolutionName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline  | 1                | Sup 1      |
    And SolutionDetail exist
        | SolutionId | SummaryDescription             | FullDescription   |
        | Sln1       | An full online medicine system | Online medicine 1 |
    When a GET request is made for solution dashboard <SolutionId>
    Then a successful response is returned
    And the solution about-supplier section status is <Status>
    And the solution about-supplier section requirement is Optional

    Examples:
        | SolutionId    | Status     | Summary     | SupplierUrl |
        | Sln1          | INCOMPLETE |             |             |
        | Sln1          | INCOMPLETE | NULL        |             |
        | Sln1          | INCOMPLETE | "   "       |             |
        | Sln1          | INCOMPLETE |             | "    "      |
        | Sln1          | COMPLETE   | "a summary" |             |
        | Sln1          | COMPLETE   |             | "url here"  |
        | Sln1          | COMPLETE   | "a summary" | "url here"  |
