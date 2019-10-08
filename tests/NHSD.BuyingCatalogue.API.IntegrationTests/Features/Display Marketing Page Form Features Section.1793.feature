Feature: Display Marketing Page Form Features Section
    As a Supplier
    I want to manage Marketing Page Information for the Solution's Features
    So that I can ensure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
        | Drs. Inc |
    And Solutions exist
        | SolutionID | SolutionName   | SummaryDescription             | OrganisationName | SupplierStatusId |
        | Sln1       | MedicOnline    | An full online medicine system | GPs-R-Us         | 1                |
        | Sln2       | TakeTheRedPill | Eye opening experience         | Drs. Inc         | 1                |
        | Sln3       | PracticeMgr    | Fully fledged GP system        | Drs. Inc         | 1                |
    And MarketingDetail exist
        | Solution | AboutUrl | Features                                            |
        | Sln1     | UrlSln1  | { "customJson" : { "id" : 1, "name" : "feature1" }} |
        | Sln2     | UrlSln2  | { "customJson" : { "id" : 2, "name" : "feature2" }} |
        | Sln3     | UrlSln3  | { "customJson" : { "id" : 3, "name" : "feature3" }} |

@1793
Scenario: 1. Sections presented
    When a GET request is made for solution Sln3
    Then a successful response is returned
    And the solution contains MarketingData
        | JsonPath                               | Value    |
        | solution.marketingData.customJson.id   | 3        |
        | solution.marketingData.customJson.name | feature3 |
    And the solution contains AboutUrl of UrlSln3 
