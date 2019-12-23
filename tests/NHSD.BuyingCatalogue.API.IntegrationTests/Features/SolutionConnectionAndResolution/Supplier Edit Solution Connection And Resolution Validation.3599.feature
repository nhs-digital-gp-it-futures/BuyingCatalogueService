Feature: Supplier Edit Connection And Resolution Validation
    As a Supplier
    I want to Validate the Solution Connection And Resolution details
    So that I can make sure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
    And Suppliers exist
        | Id    | OrganisationName |
        | Sup 1 | GPs-R-Us         |
    And Solutions exist
        | SolutionID | SolutionName   | OrganisationName | SupplierStatusId | SupplierId |
        | Sln1       | MedicOnline    | GPs-R-Us         | 1                | Sup 1      |
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                     |
        | Sln1     | An full online medicine system | Online medicine 1 | { "MinimumConnectionSpeed": "2GBps" } |

@3599
Scenario: 1. Minimum connection speed is null and Minimum desktop resolution is empty
    When a PUT request is made to update the browser-connectivity-and-resolution section for solution Sln1
        | MinimumConnectionSpeed | MinimumDesktopResolution |
        | NULL                   |                          |
    Then a response status of 400 is returned
    And the required field contains minimum-connection-speed
    And the required field does not contain minimum-desktop-resolution

@3599
Scenario: 2. Minimum connection speed is empty and Minimum desktop resolution is valid
    When a PUT request is made to update the connectivity-and-resolution section for solution Sln1
        | MinimumConnectionSpeed | MinimumDesktopResolution |
        |                        | 1x1                      |
    Then a response status of 400 is returned
    And the required field contains minimum-connection-speed
    And the required field does not contain minimum-desktop-resolution
