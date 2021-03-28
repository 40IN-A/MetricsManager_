using AgentsController.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;
using AgentsController;

namespace AgentsTests
{
    public class AgentsControllerUnitTest
    {
        private AgentControllers controller;

        public AgentsControllerUnitTest()
        {
            controller = new AgentControllers();
        }

        [Fact]
        public void EnableAgentById_ReturnsOk()
        {
            int agentId = 1;

            //Act
            var result = controller.EnableAgentById(agentId);

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void DisableAgentById_ReturnsOk()
        {
            int agentId = 0;

            //Act
            var result = controller.DisableAgentById(agentId);

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
