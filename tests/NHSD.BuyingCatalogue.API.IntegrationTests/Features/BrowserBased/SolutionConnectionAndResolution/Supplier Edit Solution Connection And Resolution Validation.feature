Feature: Supplier Edit Connection And Resolution Validation
    As a Supplier
    I want to Validate the Solution Connection And Resolution details
    So that I can make sure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline  | 1                | Sup 1      |
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                     |
        | Sln1       | An full online medicine system | Online medicine 1 | { "MinimumConnectionSpeed": "2GBps" } |

@3599
Scenario: Minimum connection speed is null and Minimum desktop resolution is empty
    When a PUT request is made to update the browser-connectivity-and-resolution section for solution Sln1
        | MinimumConnectionSpeed | MinimumDesktopResolution |
        | NULL                   |                          |
    Then a response status of 400 is returned
    And the minimum-connection-speed field value is the validation failure required
    And the minimum-desktop-resolution string does not exist

@3599
Scenario: Minimum connection speed is empty and Minimum desktop resolution is valid
    When a PUT request is made to update the browser-connectivity-and-resolution section for solution Sln1
        | MinimumConnectionSpeed | MinimumDesktopResolution |
        |                        | 1x1                      |
    Then a response status of 400 is returned
    And the minimum-connection-speed field value is the validation failure required
    And the minimum-desktop-resolution string does not exist
