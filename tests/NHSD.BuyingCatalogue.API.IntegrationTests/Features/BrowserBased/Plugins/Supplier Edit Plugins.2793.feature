Feature:  Supplier Edit Plugins
    As a Supplier
    I want to Edit the Plugins Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                             |
        | Sln1       | An full online medicine system | Online medicine 1 | {  "Plugins" : {"Required" : false, "AdditionalInformation": "orem ipsum" } } |

@2786
Scenario: 1. Plugins is updated
    When a PUT request is made to update the browser-plug-ins-or-extensions section for solution Sln1
        | PluginsRequired | PluginsDetail             |
        | yEs             | This is extra information |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                  |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [],"BrowsersSupported" : [], "Plugins" : { "Required" : true , "AdditionalInformation": "This is extra information"} } |
    And Last Updated has updated on the SolutionDetail for solution Sln1

@2786
Scenario: 2. Plugins is updated with trimmed whitespace
    When a PUT request is made to update the browser-plug-ins-or-extensions section for solution Sln1
        | PluginsRequired | PluginsDetail                        |
        | "     yEs     " | "     This is extra information    " |
    Then a successful response is returned
    And SolutionDetail exist
        | Solution | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                  |
        | Sln1     | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [],"BrowsersSupported" : [], "Plugins" : { "Required" : true , "AdditionalInformation": "This is extra information"} } |
    And Last Updated has updated on the SolutionDetail for solution Sln1

@2786
Scenario: 3. Solution is not found
    Given a Solution Sln2 does not exist
    When a PUT request is made to update the browser-plug-ins-or-extensions section for solution Sln2
        | PluginsRequired | PluginsDetail             |
        | nO              | This is extra information |
    Then a response status of 404 is returned

@2786
Scenario: 4. Service Failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the browser-plug-ins-or-extensions section for solution Sln1
        | PluginsRequired | PluginsDetail             |
        | No              | This is extra information |
    Then a response status of 500 is returned

@2786
Scenario: 5. Solution id is not present in the request
    When a PUT request is made to update the browser-plug-ins-or-extensions section with no solution id
        | PluginsRequired | PluginsDetail             |
        | no              | This is extra information |
    Then a response status of 400 is returned