@ignore
Feature: Foundation Solution (Single) Capability filtered browse
    As a Public User
    I want to use Foundation Capabilities when browsing Solutions
    So that I know what  Solutions (Single) include those Capabilities

@2649
Scenario: 1. All the Foundation Capabilities and no other Capabilities are selected
    Given all the Foundation Capabilities are selected
    And no other Capabilities are selected
    When Solutions are presented
    Then only Solutions (Single) that deliver all the Foundation Capabilities are included

@2649
Scenario: 2. All the Foundation Capabilities and one or more other Capabilities are selected
    Given all the Foundation Capabilities are selected
    And one or more other Capabilities are selected
    When Solutions are presented
    Then Solutions that deliver all the Foundation Capabilities and the other selected Capabilities are included
    And Solutions that do not deliver all the Foundation Capabilities and the selected Capabilities are excluded 
