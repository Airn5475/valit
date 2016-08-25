# valitru
Valitru (**Val**idation+Un**it**+**Ru**le) is a basic framework created to do rule based validation.  
In a nutshell, you can create rules with custom logic and use them to validate an object.

This is not simply my version of a "rules engine".  Each rule in this framework allows custom logic to determine it's validity.

[![Build status](https://ci.appveyor.com/api/projects/status/jyyrg2j7x02yqo8v?svg=true)](https://ci.appveyor.com/project/Airn5475/valitru)

####Goal 1: Improve the testability of validation rules by allowing each rule to be tested separately.
Many times, object validation is done in a single method with many if statements.  This requires a developer to code a Unit Test that will pass *all* preceeding rules until it reachs the desired rule.

####Goal 2: Provide a simple, clean and consistent interface for validation.
All rules are registered and when validation is called for, *all* rules are checked\*.  
This single call for validation prevents rules from getting lost in the daily development shuffle of adding new methods where validation is needed.  
\* *The exception being that the object may not meet the conditional rule's criteria.*

## Basic Rule
Basic rules are run every time.
```C#
public ValidationRule<Order> RuleOrderPlacedDateTimeMustBeInThePast()
    =>
    ValidationRule.NewRule<Order>()
        .ValidIf(order => DateTime.Now >= order.OrderDateTime)
        .SetErrorMessage(order => $"Order has an invalid Date/Time of {order.OrderDateTime }")
        .AddInvalidMember(order => order.OrderDateTime);
```

## Conditional Rule
Conditional Rules differ in that they are only validated if the object meets the criteria provided in the `OnlyCheckIf` method.
```C#
public ValidationRule<Order> RuleOrderCannotHaveAShippedDateLaterThanDatePlaced()
    =>
    ValidationRule.NewRule<Order>()
        .OnlyCheckIf(order => order.ShipDateTime.HasValue)
        .ValidIf(order => order.ShipDateTime.Value >= order.OrderDateTime)
        .SetErrorMessage(order => $"Order Ship Date/Time '{order.ShipDateTime.Value}' is invalid")
        .AddInvalidMember(order => order.ShipDateTime);
```

## Validation Class
The pattern involves creating a class that inherits from `ValidationServiceBase<T>` and overrides the `AllRules()` method.
```C#
public class OrderValidation : ValidationServiceBase<Order>
{
    //rules...
    
    public override ValidationRules<Order> AllRules() => new ValidationRules<Order>
    {
        Rules =
        {
            RuleOrderPlacedDateTimeMustBeInThePast(),
            RuleOrderCannotHaveAShippedDateLaterThanDatePlaced()
        }
    };
}
```

## Call for Validation
Calling the `Validate` method will validate the instance against all basic rules and any conditional rules where applicable.
```C#
public class OrderService
{
    private readonly OrderValidation _orderValidator;
    private readonly IOrderRepository _orderRepository;

    //constructor...

    public bool PlaceOrder(Order orderToBePlaced)
    {
        var result = _orderValidator.Validate(orderToBePlaced);
        if (!result.IsValid) { return false; }

        //process the order...

        _orderRepository.Save(orderToBePlaced);

        return true;
    }

}
```

## Unit testing an individual rule
Rules declared separately within their validation class can easily be tested on an individual basis.
Rules can also be established outside of a validation class and easily shared among validation classes.
```C#
[TestMethod]
public void RuleOrderCannotHaveAShippedDateLaterThanDatePlaced_ShipDateAfterOrderDate_NotValid()
{
    //Arrange
    var order = new Order { OrderDateTime = DateTime.Today };
    order.ShipDateTime = order.OrderDateTime.AddDays(-7);
    //Act
    var res = _orderValidation.RuleOrderCannotHaveAShippedDateLaterThanDatePlaced().Validate(order);
    //Assert
    Assert.IsFalse(res.IsValid);
}
```
