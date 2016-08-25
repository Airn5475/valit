# valitru
Valitru (Validation Unit Rule) is a basic framework created to do rule based validation.  
In a nutshell, you can create rules with custom logic and use them to validate an object.

[![Build status](https://ci.appveyor.com/api/projects/status/jyyrg2j7x02yqo8v?svg=true)](https://ci.appveyor.com/project/Airn5475/valitru)

## Example: A Basic Rule
```C#
public ValidationRule<Order> RuleOrderPlacedDateTimeMustBeInThePast()
    =>
    ValidationRule.NewRule<Order>()
        .ValidIf(order => DateTime.Now >= order.OrderDateTime)
        .SetErrorMessage(order => $"Order has an invalid Date/Time of {order.OrderDateTime }")
        .AddInvalidMember(order => order.OrderDateTime);
```

## Example: A Conditional Rule
```C#
public ValidationRule<Order> RuleOrderCannotHaveAShippedDateLaterThanDatePlaced()
    =>
    ConditionalValidationRule.NewRule<Order>()
        .OnlyCheckIf(order => order.ShipDateTime.HasValue)
        .ValidIf(order => order.ShipDateTime.Value >= order.OrderDateTime)
        .SetErrorMessage(order => $"Order Ship Date/Time '{order.ShipDateTime.Value}' is invalid")
        .AddInvalidMember(order => order.ShipDateTime);
```
