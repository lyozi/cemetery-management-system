//using System.Net.Http;
//using System.Threading.Tasks;
//using Xunit;
//using Microsoft.AspNetCore.Mvc.Testing;
//using WebAPI;
//using System.Net;
//using System.Text.Json;
//using System.Collections.Generic;
//using Domain.Models;

//public class MessagesControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
//{
//  private readonly HttpClient _client;

//  public MessagesControllerTests(CustomWebApplicationFactory<Program> factory)
//  {
//    _client = factory.CreateClient();
//  }

//  [Fact]
//  public async Task GetMessageItems_ReturnsOkResponse()
//  {
//    var response = await _client.GetAsync("/api/Messages");

//    response.EnsureSuccessStatusCode();
//    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
//  }

//  [Fact]
//  public async Task GetMessageItems_ReturnsListOfMessages()
//  {
//    var response = await _client.GetAsync("/api/Messages");
//    response.EnsureSuccessStatusCode();
//    var responseString = await response.Content.ReadAsStringAsync();
//    var messages = JsonSerializer.Deserialize<List<Message>>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

//    Assert.NotNull(messages);
//    Assert.IsType<List<Message>>(messages);
//  }
//}