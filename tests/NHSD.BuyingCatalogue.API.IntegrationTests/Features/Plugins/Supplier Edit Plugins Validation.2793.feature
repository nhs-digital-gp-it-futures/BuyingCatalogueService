@ignore

Feature:  Display Marketing Page Form Plugins Validation on Edit
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Plugins
    So that I can ensure the information is correct & valid

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
    And Solutions exist
        | SolutionID | SolutionName   | SummaryDescription             | OrganisationName | FullDescription     | SupplierStatusId |
        | Sln1       | MedicOnline    | An full online medicine system | GPs-R-Us         | Online medicine 1   | 1                |
    And MarketingDetail exist
        | Solution | ClientApplication                                                                                                                                                                                   |
        | Sln1     | { "PlugIns" : { "Required" : "yes", "AdditionalInformation": "orem ipsum…." }}, { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false } |  

@2793
Scenario: 1. Required is empty. AdditionalInformation is valid
    Given plug-ins is a string of null
    And additional-information is a string of 30 characters
    When a PUT request is made to update solution Sln1 plug-ins section
    Then a response status of 400 is returned
    And the plug-ins required field contains plugins-required

@2793
Scenario: 2. Required has a value. AdditionalInformation length is greater than 500 characters
    Given plug-ins is a string of yes
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
