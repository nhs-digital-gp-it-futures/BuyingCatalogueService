@ignore
Feature: Capability filtered browse (no additional service)
    As a Public User
    I want to use Capabilities when browsing Solutions
    So that I know what Solutions include those Capabilities

@2048
Scenario: 1. No Capability selected
    Given no Capability is selected
    When Solutions are presented
    Then no Solutions are excluded on the basis of the Capabilities they deliver

@2048
Scenario: 2. One or more Capability is selected
    Given one or more Capability is selected
    When Solutions are presented
    Then only Solutions that deliver all of the selected Capabilities are included
    And Additional Services are not included in the results

@2048
Scenario: 3. A Capability is de-selected
    Given a Capability is de-selected
    And there is one or more Capability selected
    When Solutions are presented
    Then only Solutions that deliver all of the selected Capabilities are included
    And Additional Services are not included in the results
