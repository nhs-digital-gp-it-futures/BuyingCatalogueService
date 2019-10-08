Feature: Suppliers Edit Solution Description Section
    As a Supplier
    I want to Edit the About Solution Section
    So that I can make sure the information is correct

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

@1843
Scenario: 1. Solution description section data is updated
    When a POST request is made to update solution Sln1 features
        | Summary                | Description            | AboutUrl   | MarketingData                                       |
        | New type of medicine 4 | A new full description | UrlSln1New | { "customJson" : { "id" : 1, "name" : "feature1" }} |
    Then Solutions exist
        | SolutionID | SolutionName   | SummaryDescription      | FullDescription        | SupplierStatusId |
        | Sln1       | MedicOnline    | New type of medicine 4  | A new full description | 1                |
        | Sln2       | TakeTheRedPill | Eye opening experience  | Eye opening6           | 1                |
        | Sln3       | PracticeMgr    | Fully fledged GP system | Fully fledged GP 12    | 1                |
    And MarketingDetail exist
        | Solution | AboutUrl   | Features                                            |
        | Sln1     | UrlSln1New | { "customJson" : { "id" : 1, "name" : "feature1" }} |
        | Sln2     | UrlSln2    | { "customJson" : { "id" : 2, "name" : "feature2" }} |

Scenario: 2. Solution description section data is created on update
    Given a MarketingDetail Sln3 does not exist
    When a POST request is made to update solution Sln3 features
        | Summary                 | Description         | AboutUrl   | MarketingData                                       |
        | Fully fledged GP system | Fully fledged GP 12 | UrlSln3New | { "customJson" : { "id" : 3, "name" : "feature3" }} |
    Then Solutions exist
        | SolutionID | SolutionName   | SummaryDescription             | FullDescription     | SupplierStatusId |
        | Sln1       | MedicOnline    | An full online medicine system | Online medicine 1   | 1                |
        | Sln2       | TakeTheRedPill | Eye opening experience         | Eye opening6        | 1                |
        | Sln3       | PracticeMgr    | Fully fledged GP system        | Fully fledged GP 12 | 1                |
    And MarketingDetail exist
        | Solution | AboutUrl   | Features                                            |
        | Sln1     | UrlSln1    | { "customJson" : { "id" : 1, "name" : "feature1" }} |
        | Sln2     | UrlSln2    | { "customJson" : { "id" : 2, "name" : "feature2" }} |
        | Sln3     | UrlSln3New | { "customJson" : { "id" : 3, "name" : "feature3" }} |
