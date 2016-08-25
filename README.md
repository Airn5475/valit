# valitru
Valitru (Validation Unit Rule) is a basic framework created to do rule based validation.  
In a nutshell, you can create rules with custom logic and use them to validate an object.

[![Build status](https://ci.appveyor.com/api/projects/status/jyyrg2j7x02yqo8v?svg=true)](https://ci.appveyor.com/project/Airn5475/valitru)

## Basic Rule
```C#
public ValidationRule<Order> RuleOrderPlacedDateTimeMustBeInThePast()
    =>
    ValidationRule.NewRule<Order>()
        .ValidIf(order => DateTime.Now >= order.OrderDateTime)
        .SetErrorMessage(order => $"Order has an invalid Date/Time of {order.OrderDateTime }")
        .AddInvalidMember(order => order.OrderDateTime);
```

## Conditional Rule
```C#
public ValidationRule<Order> RuleOrderCannotHaveAShippedDateLaterThanDatePlaced()
    =>
    ConditionalValidationRule.NewRule<Order>()
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
