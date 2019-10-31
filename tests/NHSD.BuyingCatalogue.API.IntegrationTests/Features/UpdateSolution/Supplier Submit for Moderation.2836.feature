Feature: Supplier Submit for Moderation
	As a Supplier
    I want to submit my Marketing Page for Moderation
    So that my Marketing Page can be approved and published

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |

@2836
Scenario: 1. Solution successfully submitted for review
    Given Solutions exist
        | SolutionID | SolutionName | SummaryDescription             | OrganisationName | SupplierStatusId |
        | Sln1       | MedicOnline  | An full online medicine system | GPs-R-Us         | 1                |
    And MarketingDetail exist
        | Solution | ClientApplication                                                                                          |
        | Sln1     | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : ["Firefox"], "MobileResponsive": true } |
    When a request is made to submit Solution Sln1 for review
    Then a response status of 204 is returned

@2836
Scenario: 2. Solution not found
    Given a Solution Sln2 does not exist
    When a request is made to submit Solution Sln2 for review
    Then a response status of 404 is returned

@2836
Scenario: 3. Service failure
    Given Solutions exist
        | SolutionID | SolutionName | SummaryDescription             | OrganisationName | SupplierStatusId |
        | Sln1       | MedicOnline  | An full online medicine system | GPs-R-Us         | 1                |
    And the call to the database to set the field will fail
    When a request is made to submit Solution Sln1 for review
    Then a response status of 500 is returned

@2836
Scenario: 4. Solution id not present in request
    When a request is made to submit Solution for review with no solution id
    Then a response status of 400 is returned

Scenario: 5. Solution failed on submit for review due to missing Solution summary
    Given Solutions exist
        | SolutionID | SolutionName | SummaryDescription | OrganisationName | SupplierStatusId |
        | Sln1       | MedicOnline  |                    | GPs-R-Us         | 1                |
    And MarketingDetail exist
        | Solution | ClientApplication                                                                                            |
        | Sln1     | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : ["Firefox"], "MobileResponsive": true } |
    When a request is made to submit Solution Sln1 for review
    Then a response status of 400 is returned
    And the response details of the submit Solution for review request are as follows
        | Property | InvalidSections      |
        | required | solution-description |

Scenario: 6. Solution failed on submit for review due to missing client application type
    Given Solutions exist
        | SolutionID | SolutionName | SummaryDescription             | OrganisationName | SupplierStatusId |
        | Sln1       | MedicOnline  | An full online medicine system | GPs-R-Us         | 1                |
    And MarketingDetail exist
        | Solution | ClientApplication |
        | Sln1     |                   |
    When a request is made to submit Solution Sln1 for review
    Then a response status of 400 is returned
    And the response details of the submit Solution for review request are as follows
        | Property | InvalidSections          |
        | required | client-application-types |

Scenario: 7. Solution failed on submit for review due to missing browsers supported
    Given Solutions exist
        | SolutionID | SolutionName | SummaryDescription             | OrganisationName | SupplierStatusId |
        | Sln1       | MedicOnline  | An full online medicine system | GPs-R-Us         | 1                |
    And MarketingDetail exist
        | Solution | ClientApplication                                                                                   |
        | Sln1     | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [], "MobileResponsive": true } |
    When a request is made to submit Solution Sln1 for review
    Then a response status of 400 is returned
    And the response details of the submit Solution for review request are as follows
        | Property | InvalidSections |
        | required | browser-based   |

Scenario: 8. Solution failed on submit for review due to missing mobile responsive
    Given Solutions exist
        | SolutionID | SolutionName | SummaryDescription             | OrganisationName | SupplierStatusId |
        | Sln1       | MedicOnline  | An full online medicine system | GPs-R-Us         | 1                |
    And MarketingDetail exist
         | Solution | ClientApplication                                                                 |
         | Sln1     | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : ["Firefox"] } |
    When a request is made to submit Solution Sln1 for review
    Then a response status of 400 is returned
    And the response details of the submit Solution for review request are as follows
        | Property | InvalidSections |
        | required | browser-based   |
