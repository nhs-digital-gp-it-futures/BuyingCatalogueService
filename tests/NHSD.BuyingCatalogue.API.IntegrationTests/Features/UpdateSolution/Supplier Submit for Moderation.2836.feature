Feature: Supplier Submit for Moderation
	As a Supplier
    I want to submit my Marketing Page for Moderation
    So that my Marketing Page can be approved and published

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName     | SupplierId | SummaryDescription             | FullDescription | ClientApplication                                                                                                                             |
        | Sln1       | MedicOnline      | Sup 1      | An full online medicine system | Another Desc    | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : ["Firefox"], "MobileResponsive": true, "Plugins": { "Required": false } } |
        | Sln2       | TakeTheRedPill   | Sup 1      | SummaryDescription             | One more Desc   |                                                                                                                                               |
        | Sln3       | Another Solution | Sup 1      | SummaryDescription             | Desc            | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [], "MobileResponsive": true, "Plugins": { "Required": false } }          |
        | Sln4       | Medics           | Sup 1      | SummaryDescription             | One more Desc   | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : ["Firefox"], "Plugins": { "Required": false } }                           |
        | Sln5       | TakeTheBluePill  | Sup 1      | SummaryDescription             | A Desc          | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : ["Firefox"], "MobileResponsive": true }                                   |

@2836
Scenario: 1. Solution successfully submitted for review
    When a request is made to submit Solution Sln1 for review
    Then a response status of 204 is returned
    And Last Updated has updated on the SolutionEntity for solution Sln1

@2836
Scenario: 2. Solution not found
    Given a Solution Sln6 does not exist
    When a request is made to submit Solution Sln6 for review
    Then a response status of 404 is returned

@2836
Scenario: 3. Service failure
    And the call to the database to set the field will fail
    When a request is made to submit Solution Sln1 for review
    Then a response status of 500 is returned

@2836
Scenario: 4. Solution id not present in request
    When a request is made to submit Solution for review with no solution id
    Then a response status of 400 is returned

Scenario: 5. Solution failed on submit for review due to missing Solution summary
    When a request is made to submit Solution Sln1 for review
    Then a response status of 400 is returned
    And the response details of the submit Solution for review request are as follows
        | Property | InvalidSections      |
        | required | solution-description |

Scenario: 6. Solution failed on submit for review due to missing client application type
    When a request is made to submit Solution Sln2 for review
    Then a response status of 400 is returned
    And the response details of the submit Solution for review request are as follows
        | Property | InvalidSections          |
        | required | client-application-types |

Scenario: 7. Solution failed on submit for review due to missing browsers supported
    When a request is made to submit Solution Sln3 for review
    Then a response status of 400 is returned
    And the response details of the submit Solution for review request are as follows
        | Property | InvalidSections |
        | required | browser-based   |

Scenario: 8. Solution failed on submit for review due to missing mobile responsive
    When a request is made to submit Solution Sln4 for review
    Then a response status of 400 is returned
    And the response details of the submit Solution for review request are as follows
        | Property | InvalidSections |
        | required | browser-based   |

Scenario: 9. Solution failed on submit for review due to missing plugin requirement
    When a request is made to submit Solution Sln5 for review
    Then a response status of 400 is returned
    And the response details of the submit Solution for review request are as follows
        | Property | InvalidSections |
        | required | browser-based   |