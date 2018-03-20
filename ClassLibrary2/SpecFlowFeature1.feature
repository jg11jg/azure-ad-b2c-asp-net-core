Feature: SpecFlowFeature1
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: Add two numbers
	Given an empty user repository
	When I add a user with email '<email>' and password '<password>'
	Then user repository contains 1 user
