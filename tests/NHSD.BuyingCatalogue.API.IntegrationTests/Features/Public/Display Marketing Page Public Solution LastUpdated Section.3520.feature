Feature: Display Marketing Page Public Solution LastUpdated Section
	As a Catalogue User
    I want to view Marketing Page Information for the Solutions Last Updated Section
    So that I can ensure the lastUpdated is the latest within solution

#Background:
#    Given Organisations exist
#        | Name     |
#        | GPs-R-Us |
#    And Suppliers exist
#        | Id    | OrganisationName |
#        | Sup 1 | GPs-R-Us         |
#@3520
#Scenario Outline: 1. Last Updated is the latest of last updated in the solution tables
#    Given Solutions exist
#        | SolutionID | SolutionName | OrganisationName | SupplierStatusId | SupplierId | LastUpdated |
#        | Sln1       | MedicOnline  | GPs-R-Us         | 1                | Sup 1      | <Solution>  |
#    And SolutionDetail exist
#        | Solution | LastUpdated      |
#        | Sln1     | <SolutionDetail> |
#    And MarketingContacts exist
#        | SolutionId | LastUpdated |
#        | Sln1       | <MarketingContact1> |
#        | Sln1       | <MarketingContact2> |
#    When a GET request is made for solution public Sln1
#    Then a successful response is returned
#    And the last updated date in the solution is <LastUpdated>
#Examples:
#    | Solution            | SolutionDetail      | MarketingContact1   | MarketingContact2   | LastUpdated         |
#    | 01/01/0001 00:00:00 | 01/01/0001 00:00:00 | 01/01/0001 00:00:00 | 01/01/0001 00:00:00 | 01/01/0001 00:00:00 |
#    | 31/12/9999 23:59:59 | 31/12/9999 23:59:59 | 31/12/9999 23:59:59 | 31/12/9999 23:59:59 | 31/12/9999 23:59:59 |
#    | 01/01/0001 00:00:00 | 31/12/9999 23:59:59 | 21/12/2019 14:43:34 | 25/12/2025 25:11:34 | 31/12/9999 23:59:59 |
#    | 04/12/2019 09:00:00 | 04/12/2019 09:00:01 | 04/12/2019 08:59:59 | 04/12/2019 09:00:02 | 04/12/2019 09:00:02 |
#
#@3520
#Scenario Outline: 2. No marketing contact exist, Last Updated is the latest of last updated in the solution tables
#    Given Solutions exist
#        | SolutionID | SolutionName | OrganisationName | SupplierStatusId | SupplierId | LastUpdated |
#        | Sln1       | MedicOnline  | GPs-R-Us         | 1                | Sup 1      | <Solution>  |
#    And SolutionDetail exist
#        | Solution | LastUpdated      |
#        | Sln1     | <SolutionDetail> |
#    When a GET request is made for solution public Sln1
#    Then a successful response is returned
#    And the last updated date in the solution is <LastUpdated>
#Examples:
#    | Solution            | SolutionDetail      | LastUpdated         |
#    | 01/01/0001 00:00:00 | 01/01/0001 00:00:00 | 01/01/0001 00:00:00 |
#    | 31/12/9999 23:59:59 | 31/12/9999 23:59:59 | 31/12/9999 23:59:59 |
#    | 01/01/0001 00:00:00 | 31/12/9999 23:59:59 | 31/12/9999 23:59:59 |
#    | 04/12/2019 09:00:00 | 04/12/2019 09:00:01 | 04/12/2019 09:00:01 |
