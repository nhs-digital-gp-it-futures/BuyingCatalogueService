Feature: Supplier Edit Connectivity And Resolution
    As a Supplier
    I want to Edit the Solution Connection And Resolution details
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

@3599
Scenario: 1. Connection and Resolution are updated for the solution
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                     |
        | Sln1     | An full online medicine system | Online medicine 1 | { "MinimumConnectionSpeed": "2GBps" } |
    When a PUT request is made to update solution Sln1 connectivity-and-resolution section
        | MinimumConnectionSpeed | MinimumDesktopResolution |
        | 1GBps                  | 800x600                  |
    Then a successful response is returned
    And Solutions exist
        | SolutionID | SolutionName   |
        | Sln1       | MedicOnline    |
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                                                                                                            |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [], "BrowsersSupported": [],  "MobileResponsive": null, "Plugins": null, "MinimumConnectionSpeed": "1GBps", "MinimumDesktopResolution": "800x600", "HardwareRequirements": null, "AdditionalInformation": null } |
    And Last Updated has updated on the SolutionDetail for solution Sln1
    
@3599
Scenario: 2. Connection and Resolution are updated for the solution with empty resolution
    Given SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                     |
        | Sln1     | An full online medicine system | Online medicine 1 | { "MinimumConnectionSpeed": "2GBps" } |
    When a PUT request is made to update solution Sln1 connectivity-and-resolution section
        | MinimumConnectionSpeed | MinimumDesktopResolution |
        | 1GBps                  | NULL                     |
    Then a successful response is returned
    And Solutions exist
        | SolutionID | SolutionName   |
        | Sln1       | MedicOnline    |
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription     | ClientApplication                                                                                                                                                      |
        | Sln1     | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes": [], "BrowsersSupported": [],  "MobileResponsive": null, "Plugins": null, "MinimumConnectionSpeed": "1GBps", "MinimumDesktopResolution": null, "HardwareRequirements": null } |
    And Last Updated has updated on the SolutionDetail for solution Sln1

@3599
Scenario: 3. If SolutionDetail is missing for the solution, thats an error case
	Given a SolutionDetail Sln1 does not exist
    When a PUT request is made to update solution Sln1 connectivity-and-resolution section
        | MinimumConnectionSpeed | MinimumDesktopResolution |
        | 1GBps                  | 800x600                  |
    Then a response status of 500 is returned

@3599
Scenario: 4. Solution not found
    Given a Solution Sln2 does not exist
    When a PUT request is made to update solution Sln2 connectivity-and-resolution section
        | MinimumConnectionSpeed | MinimumDesktopResolution |
        | 1GBps                  | 800x600                  |
    Then a response status of 404 is returned

@3599
Scenario: 5. Service failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update solution Sln1 connectivity-and-resolution section
        | MinimumConnectionSpeed | MinimumDesktopResolution |
        | 1GBps                  | 800x600                  |
    Then a response status of 500 is returned

@3599
Scenario: 6. Solution id not present in request
    When a PUT request is made to update solution connectivity-and-resolution section with no solution id
        | MinimumConnectionSpeed | MinimumDesktopResolution |
        | 1GBps                  | 800x600                  |
    Then a response status of 400 is returned
