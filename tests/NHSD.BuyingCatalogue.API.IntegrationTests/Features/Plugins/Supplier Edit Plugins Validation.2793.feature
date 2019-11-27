Feature:  Display Marketing Page Form Plugins Validation on Edit
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Plugins
    So that I can ensure the information is correct & valid

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
        | Solution | SummaryDescription             | FullDescription     | ClientApplication                                                                                                                                                                            |
        | Sln1     | An full online medicine system | Online medicine 1   | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins" : {"Required" : true, "AdditionalInformation": "orem ipsum" } } | 

@2793
Scenario: 1. Required is empty. AdditionalInformation is valid
    Given plug-ins is a string of null
    And additional-information is a string of 30 characters
    When a PUT request is made to update solution Sln1 plug-ins section
    Then a response status of 400 is returned
    And the plug-ins required field contains plugins-required

@2793
Scenario: 2. Required has a value. AdditionalInformation length is greater than 500 characters
    Given plug-ins is a string of Yes
    And additional-information is a string of 501 characters
    When a PUT request is made to update solution Sln1 plug-ins section
    Then a response status of 400 is returned
    And the plug-ins maxLength field contains plugins-detail
    
@2793
Scenario: 3. Required is empty and AdditionalInformations length is greater than 500 characters
    Given plug-ins is a string of null
    And additional-information is a string of 501 characters
    When a PUT request is made to update solution Sln1 plug-ins section
    Then a response status of 400 is returned
    And the plug-ins required field contains plugins-required
    And the plug-ins maxLength field contains plugins-detail
