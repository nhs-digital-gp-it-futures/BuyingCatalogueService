
Feature: Supplier Display Sub-Form Browser Based Client Type
    As a Supplier
    I want to Display Sub-Form Browser Based Client Type
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

@2782
Scenario: 1. Sub-Form Browser Based Client Type all sections are Displayed
    When a GET request is made to display solution Sln1 browser-based sections
    Then a successful response is returned
    And Solutions browser-based section contains all BrowserBased Sections   
    | Id                            | Status       | Requirement |
    | browsers-supported          | COMPLETE   | Mandatory |
    | plug-ins-or-extensions      | INCOMPLETE | Mandatory |
    | connectivity-and-resolution | INCOMPLETE | Mandatory |
    | hardware-requirements       | INCOMPLETE | Optional  |
    | additional-information      | INCOMPLETE | Optional  |
@2782
Scenario: 2. Solution not found
    Given a Solution Sln4 does not exist
    When a GET request is made to display solution Sln4 browser-based sections
    Then a response status of 404 is returned

@2782
Scenario: 3. Service failure
    Given the call to the database to set the field will fail
    When a GET request is made to display solution Sln1 browser-based sections
    Then a response status of 500 is returned

@2782
Scenario: 4. Solution id not present in request
    When a GET request is made to display solution browser-based sections with no solution id
    Then a response status of 400 is returned
