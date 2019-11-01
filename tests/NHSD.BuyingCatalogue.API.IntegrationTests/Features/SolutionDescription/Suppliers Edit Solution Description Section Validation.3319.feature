Feature: Suppliers Edit Solution Description Section Validation
    As a Supplier
    I want to Edit the About Solution Section
    So that I can make sure the information is validated

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
        | Drs. Inc |
    And Solutions exist
        | SolutionID | SolutionName   | SummaryDescription             | OrganisationName | FullDescription     | SupplierStatusId |
        | Sln1       | MedicOnline    | An full online medicine system | GPs-R-Us         | Online medicine 1   | 1                |
    And MarketingDetail exist
        | Solution | AboutUrl | Features                          |
        | Sln1     | UrlSln1  | [ "Appointments", "Prescribing" ] |

@3319
Scenario: 1. Summary is not filled out
    Given a request where the summary is a string of 0 characters
    When the request is made for Sln1
    Then a response status of 400 is returned
    And the response required field contains summary

@3319
Scenario: 2. Summary exceeds the character limit
    Given a request where the summary is a string of 301 characters
    When the request is made for Sln1
    Then a response status of 400 is returned
    And the response maxlength field contains summary

@3319
Scenario: 3. Description exceeds the character limit
    Given a request where the description is a string of 1001 characters
    When the request is made for Sln1
    Then a response status of 400 is returned
    And the response maxlength field contains description

@3319
Scenario: 4. Link exceeds the character limit
    Given a request where the link is a string of 1001 characters
    When the request is made for Sln1
    Then a response status of 400 is returned
    And the response maxlength field contains link

@3319
Scenario: 5. Summary is not filled out & Description exceeds the character limit
    Given a request where the summary is a string of 0 characters
    And a request where the description is a string of 1001 characters
    When the request is made for Sln1
    Then a response status of 400 is returned
    And the response required field contains summary
    And the response maxlength field contains description

@3319
Scenario: 6. Summary is not filled out & Link exceeds the character limit
    Given a request where the summary is a string of 0 characters
    And a request where the link is a string of 1001 characters
    When the request is made for Sln1
    Then a response status of 400 is returned
    And the response required field contains summary
    And the response maxlength field contains link

@3319
Scenario: 7. Summary & Description exceeds the character limit
    Given a request where the summary is a string of 301 characters
    And a request where the description is a string of 1001 characters
    When the request is made for Sln1
    Then a response status of 400 is returned
    And the response maxlength field contains description
    And the response maxlength field contains summary

@3319
Scenario: 8. Summary & Link exceeds the character limit
    Given a request where the summary is a string of 301 characters
    And a request where the link is a string of 1001 characters
    When the request is made for Sln1
    Then a response status of 400 is returned
    And the response maxlength field contains summary
    And the response maxlength field contains link

@3319
Scenario: 9. Description & Link exceeds the character limit
    Given a request where the description is a string of 1001 characters
    And a request where the link is a string of 1001 characters
    When the request is made for Sln1
    Then a response status of 400 is returned
    And the response maxlength field contains link
    And the response maxlength field contains description

@3319
Scenario: 10. Summary is not filled out, Description & Link exceeds the character limit
    Given a request where the summary is a string of 0 characters
    And a request where the description is a string of 1001 characters
    And a request where the link is a string of 1001 characters
    When the request is made for Sln1
    Then a response status of 400 is returned
    And the response required field contains summary
    And the response maxlength field contains description
    And the response maxlength field contains link

@3319
Scenario: 11. Summary, Description & Link exceeds the character limit
    Given a request where the summary is a string of 301 characters
    And a request where the description is a string of 1001 characters
    And a request where the link is a string of 1001 characters    
    When the request is made for Sln1
    Then a response status of 400 is returned
    And the response maxlength field contains summary
    And the response maxlength field contains description
    And the response maxlength field contains link

