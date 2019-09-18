@ignore
Feature: No selection criteria browse
    As a Public User
    I want to browse the Solutions
    So that I know what those Solutions are

@2053
Scenario: 1. No selection criteria applied
    Given no selection criteria are applied
    When Solutions are presented
    Then no Solutions are excluded

@2053
Scenario: 2. Card Content
    Given that Solutions are presented
    When no selection criteria are applied 
    Then the Card will contain the correct contents
    | OrganisationName | SolutionID | SolutionName | SummaryDescription | Capabilities |
    | tbd              | tbd        | tbd          | tbd                | tbd          |
