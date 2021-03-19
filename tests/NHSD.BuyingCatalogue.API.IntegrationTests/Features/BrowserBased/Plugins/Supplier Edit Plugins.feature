Feature:  Supplier Edit Plugins
    As a Supplier
    I want to Edit the Plugins Section
    So that I can ensure the information is correct

Background:
    Given Suppliers exist
        | Id    | SupplierName |
        | Sup 1 | Supplier 1   |
    And Solutions exist
        | SolutionId | SolutionName | SupplierId |
        | Sln1       | MedicOnline  | Sup 1      |
    And framework solutions exist
        | SolutionId | IsFoundation | FrameworkId |
        | Sln1       | true         | NHSDGP001   |

@2786
Scenario: Plugins is updated
    Given solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                             |
        | Sln1       | An full online medicine system | Online medicine 1 | {  "Plugins" : {"Required" : false, "AdditionalInformation": "orem ipsum" } } |
    When a PUT request is made to update the browser-plug-ins-or-extensions section for solution Sln1
        | PluginsRequired | PluginsDetail             |
        | yEs             | This is extra information |
    Then a successful response is returned
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                  |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [],"BrowsersSupported" : [], "Plugins" : { "Required" : true , "AdditionalInformation": "This is extra information"} } |
    And Last Updated has been updated for solution Sln1

@2786
Scenario: Plugins is updated with trimmed whitespace
    Given solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                             |
        | Sln1      | An full online medicine system | Online medicine 1 | {  "Plugins" : {"Required" : false, "AdditionalInformation": "orem ipsum" } } |
    When a PUT request is made to update the browser-plug-ins-or-extensions section for solution Sln1
        | PluginsRequired | PluginsDetail                        |
        | "     yEs     " | "     This is extra information    " |
    Then a successful response is returned
    And solutions have the following details
        | SolutionId | SummaryDescription             | FullDescription   | ClientApplication                                                                                                                                  |
        | Sln1       | An full online medicine system | Online medicine 1 | { "ClientApplicationTypes": [],"BrowsersSupported" : [], "Plugins" : { "Required" : true , "AdditionalInformation": "This is extra information"} } |
    And Last Updated has been updated for solution Sln1

@2786
Scenario: Solution is not found
    Given a Solution Sln4 does not exist
    When a PUT request is made to update the browser-plug-ins-or-extensions section for solution Sln4
        | PluginsRequired | PluginsDetail             |
        | nO              | This is extra information |
    Then a response status of 404 is returned

@2786
Scenario: Service Failure
    Given the call to the database to set the field will fail
    When a PUT request is made to update the browser-plug-ins-or-extensions section for solution Sln1
        | PluginsRequired | PluginsDetail             |
        | No              | This is extra information |
    Then a response status of 500 is returned

@2786
Scenario: Solution id is not present in the request
    When a PUT request is made to update the browser-plug-ins-or-extensions section with no solution id
        | PluginsRequired | PluginsDetail             |
        | no              | This is extra information |
    Then a response status of 400 is returned
