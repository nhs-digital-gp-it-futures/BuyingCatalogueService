Feature: Display Marketing Page Form Solution Description Section
	As a Supplier
    I want to manage Marketing Page Information for the About Solution + Summary Description Section
    So that I can ensure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
        | Drs. Inc |
    And Solutions exist
        | SolutionID | SolutionName   | SummaryDescription             | OrganisationName | FullDescription     | SupplierStatusId |
        | Sln1       | MedicOnline    | An full online medicine system | GPs-R-Us         | Online medicine 1   | 1                |
        | Sln2       | TakeTheRedPill | Eye opening experience         | Drs. Inc         | Eye opening6        | 1                |
        | Sln3       | PracticeMgr    | Fully fledged GP system        | Drs. Inc         | Fully fledged GP 12 | 1                |
    And MarketingDetail exist
        | Solution | AboutUrl | Features                                            |
        | Sln1     | UrlSln1  | { "customJson" : { "id" : 1, "name" : "feature1" }} |
        | Sln2     | UrlSln2  | { "customJson" : { "id" : 2, "name" : "feature2" }} |
        | Sln3     | UrlSln3  | { "customJson" : { "id" : 3, "name" : "feature3" }} |

@1793
Scenario: 1. Solution description section presented
    When a GET request is made for solution Sln3
    Then a successful response is returned
    And the solution contains SummaryDescription of 'Fully fledged GP system'
    And the solution contains FullDescription of 'Fully fledged GP 12'
    And the solution contains AboutUrl of UrlSln3 
