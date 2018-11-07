Feature: Employee
	In order to set another employee status
	As a manager
	I want to be changed the employee status

@mytag
Scenario: Try change status to empty employee
	Given I have a empty employee
	When I press change status
	Then the result should be a message 'Id do funcionário deve ser maior que zero.'


@mytag
Scenario: Try change status to empty
	Given I have a employee ID
	And I try change status to number char
	When I press change status
	Then the result should be a message 'Status deve conter apenas letras.'

@mytag
Scenario: Try change status to 'S'
	Given I have a employee ID
	And I try change status to 'S'
	When I press change status
	Then the result should be a status changed