using DandyMediator.Responses;
using DandyMediator.Validation.Tests.Fixtures;
using DandyMediator.Validation.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;

namespace DandyMediator.Validation.Tests;

public class RequestValidationTests(DefaultFixture fixture) : IClassFixture<DefaultFixture>
{
    [Fact]
    public async Task NormalRequest_PassesValidation()
    {
        var request = new ValidationRequest("1,2,3,4,5");
        var response = await fixture.GetMediator().SendAsync(request);

        Assert.True(response.WasValidRequest());
    }

    [Fact]
    public async Task NormalRequest_FailsValidation()
    {
        var request = new ValidationRequest("1,2,3,4,5,6,7,8,9,10");
        var response = await fixture.GetMediator().SendAsync(request);

        Assert.True(response.WasInvalidRequest());
    }

    [Fact]
    public async Task ComplexPropertyRequest_PassesValidation()
    {
        var complexProperty = new ComplexProperty("1,2,3,4,5");
        var request = new ComplexPropertyValidationRequest(complexProperty);
        var response = await fixture.GetMediator().SendAsync(request);

        Assert.True(response.WasValidRequest());
    }

    [Fact]
    public async Task ComplexPropertyRequest_FailsValidation()
    {
        var complexProperty = new ComplexProperty("1,2,3,4,5,6,7,8,9,10");
        var request = new ComplexPropertyValidationRequest(complexProperty);
        var response = await fixture.GetMediator().SendAsync(request);

        Assert.True(response.WasInvalidRequest());
    }

    [Fact]
    public async Task EnumerablePropertyRequest_PassesValidation()
    {
        var enumerableItem = new EnumerableItem("1,2,3,4,5");
        var request = new EnumerablePropertyValidationRequest([enumerableItem]);
        var response = await fixture.GetMediator().SendAsync(request);

        Assert.True(response.WasValidRequest());
    }

    [Fact]
    public async Task EnumerablePropertyRequest_FailsValidation()
    {
        var enumerableItem = new EnumerableItem("1,2,3,4,5,6,7,8,9,10");
        var request = new EnumerablePropertyValidationRequest([enumerableItem]);
        var response = await fixture.GetMediator().SendAsync(request);

        Assert.True(response.WasInvalidRequest());
    }

    [Fact]
    public async Task RequestWithoutValidation_PassesValidation()
    {
        var request = new RequestWithoutValidation("epstein-didnt-kill-himself");
        var response = await fixture.GetMediator().SendAsync(request);

        Assert.True(response.WasValidRequest());
    }
}